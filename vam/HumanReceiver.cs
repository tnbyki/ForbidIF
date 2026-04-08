// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// FILE:HumanReceiver.cs
// VAM BRIDGE (AI → TAM → PY → VAM)
// Real-time VOICE / POSE Receiver
// TRACK / TM Motion Executor
// Author: VAMT
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using SimpleJSON;

public class MyHumanReceiver : MVRScript
{
    // ============================================================
    // 内部データ型
    // ============================================================
    private class BoneMotionState
    {
        public Transform Bone;
        public Vector3 StartWorldPos;
        public Quaternion StartWorldRot;
        public Vector3 TargetLocalPos;
        public Quaternion TargetLocalRot;
        public float StartTime;
        public float Duration;
        public bool Active;
    }

    private class PoseEnvelope
    {
        public float Duration = 1.0f;
        public List<string> PrePoseTriggers = new List<string>();
        public List<string> BoneEntries = new List<string>();
    }

    // ============================================================
    // UI / VaM
    // ============================================================
    private JSONStorableFloat _portParam;
    private JSONStorableString _poseDataStorage;
    private JSONStorableString _bridgeVoiceCommand;

    // ============================================================
    // ネットワーク
    // ============================================================
    private UdpClient _receiverSocket;
    private Thread _receiverThread;
    private volatile bool _receiverRunning = false;

    // 受信スレッド → メインスレッド
    private readonly Queue<string> _incomingCommands = new Queue<string>();
    private readonly object _incomingLock = new object();

    // ============================================================
    // ボーン / モーション
    // ============================================================
    private readonly Dictionary<string, Transform> _boneMap = new Dictionary<string, Transform>();
    private readonly Dictionary<string, BoneMotionState> _motionMap = new Dictionary<string, BoneMotionState>();
    private readonly List<string> _completedBones = new List<string>();

    private Coroutine _currentUIButtonRoutine;

    private readonly string[] _targetIDs = {
        "hipControl", "chestControl", "headControl",
        "rHandControl", "lHandControl",
        "rFootControl", "lFootControl",
        "rKneeControl", "lKneeControl",
        "rElbowControl", "lElbowControl"
    };

    // ============================================================
    // Init
    // ============================================================
    public override void Init()
    {
        _portParam = new JSONStorableFloat("Port Number", 10001f, 1024f, 65535f, true, true);
        _portParam.setCallbackFunction = (f) => ReopenReceiver();
        CreateSlider(_portParam, true);

        _poseDataStorage = new JSONStorableString("poseData", "");
        CreateTextField(_poseDataStorage, true).height = 300f;

        CreateButton("Send Pose to Python").button.onClick.AddListener(OnSendPoseButton);
        CreateButton("Auto Detect Voice Bridge").button.onClick.AddListener(() => { DetectVoiceBridge(true); });
        CreateButton("Check All BT_ Atoms").button.onClick.AddListener(LogAllAtoms);

        BuildBoneMap();
        ReopenReceiver();
        DetectVoiceBridge(false);

        SuperController.LogMessage("MyHumanReceiver: 🟢 reworked receiver started");
        LogAllAtoms();
    }

    // ============================================================
    // Atom / BT utility
    // ============================================================
    private void LogAllAtoms()
    {
        SuperController.LogMessage("=== [BT_ SCAN START] ===");
        int count = 0;

        foreach (var atom in SuperController.singleton.GetAtoms())
        {
            if (!atom.uid.StartsWith("BT_", StringComparison.OrdinalIgnoreCase))
                continue;

            Button b = atom.GetComponentInChildren<Button>(true);
            string buttonStatus = (b != null) ? "✅[UI_BUTTON]" : "⚪[NO_UNITY_BUTTON]";
            string triggerStatus = (atom.GetStorableByID("Trigger") != null) ? "✅[TRIGGER]" : "❌[NO_TRIGGER]";

            SuperController.LogMessage(string.Format("{0} {1} UID: {2}", buttonStatus, triggerStatus, atom.uid));
            count++;
        }

        SuperController.LogMessage("=== [BT_ SCAN END] Total: " + count + " ===");
    }

    private void ExecuteBT(string atomUid)
    {
        JSONNode trigger = GetTriggerNode(atomUid);
        if (trigger == null) return;

        JSONNode arr = trigger["startActions"];
        if (arr == null || arr.Count == 0)
        {
            SuperController.LogMessage("!! [ExecuteBT] startActions none: " + atomUid);
            return;
        }

        if (_currentUIButtonRoutine != null)
        {
            StopCoroutine(_currentUIButtonRoutine);
            _currentUIButtonRoutine = null;
        }

        SuperController.LogMessage("[ExecuteBT] TriggerRunner -> " + atomUid);
        _currentUIButtonRoutine = StartCoroutine(RunActionArray(arr, atomUid));
    }

    private JSONNode GetTriggerNode(string atomUid)
    {
        Atom atom = SuperController.singleton.GetAtomByUid(atomUid);
        if (atom == null)
        {
            SuperController.LogMessage("!! [ExecuteBT] atom not found: " + atomUid);
            return null;
        }

        JSONStorable storable = atom.GetStorableByID("Trigger");
        if (storable == null)
        {
            SuperController.LogMessage("!! [ExecuteBT] Trigger storable not found on: " + atomUid);
            return null;
        }

        JSONClass json = storable.GetJSON();
        if (json == null)
        {
            SuperController.LogMessage("!! [ExecuteBT] Trigger JSON null: " + atomUid);
            return null;
        }

        JSONNode trigger = json["trigger"];
        if (trigger == null)
        {
            SuperController.LogMessage("!! [ExecuteBT] trigger node not found: " + atomUid);
            return null;
        }

        return trigger;
    }

    private IEnumerator RunActionArray(JSONNode arr, string atomUid)
    {
        SuperController.LogMessage("RUN UIButton START: " + atomUid + " / count=" + arr.Count);

        for (int i = 0; i < arr.Count; i++)
        {
            JSONNode a = arr[i];
            yield return StartCoroutine(RunActionNode(a, atomUid, i));
        }

        SuperController.LogMessage("RUN UIButton END: " + atomUid);
        _currentUIButtonRoutine = null;
    }

    private IEnumerator RunActionNode(JSONNode a, string atomUid, int index)
    {
        string name = a["name"];
        string receiverAtomUid = a["receiverAtom"];
        string receiver = a["receiver"];
        string target = a["receiverTargetName"];

        Atom receiverAtom = SuperController.singleton.GetAtomByUid(receiverAtomUid);
        if (receiverAtom == null)
        {
            SuperController.LogMessage(atomUid + "[" + index + "] receiverAtom not found: " + receiverAtomUid);
            yield break;
        }

        JSONStorable storable = receiverAtom.GetStorableByID(receiver);
        if (storable == null)
        {
            SuperController.LogMessage(atomUid + "[" + index + "] receiver storable not found: " + receiver);
            yield break;
        }

        if (!string.IsNullOrEmpty(target) && target.StartsWith("morph: "))
        {
            string morphName = target.Substring("morph: ".Length);
            JSONStorableFloat morph = storable.GetFloatJSONParam(morphName);
            if (morph == null)
            {
                SuperController.LogMessage(atomUid + "[" + index + "] morph not found: " + morphName);
                yield break;
            }

            float endValue = 0f;
            float.TryParse(a["floatValue"], NumberStyles.Float, CultureInfo.InvariantCulture, out endValue);

            bool useTimer = false;
            float timerLength = 0f;
            string timerType = a["timerType"];

            bool.TryParse(a["useTimer"], out useTimer);
            float.TryParse(a["timerLength"], NumberStyles.Float, CultureInfo.InvariantCulture, out timerLength);

            float startValue = morph.val;

            if (!useTimer || timerLength <= 0f)
            {
                morph.val = endValue;
                SuperController.LogMessage(
                    "RUN OK " + atomUid + "[" + index + "] " +
                    name + " | " + receiverAtomUid + " | " + receiver + " | " +
                    morphName + " = " + endValue.ToString(CultureInfo.InvariantCulture)
                );
                yield break;
            }

            float elapsed = 0f;
            while (elapsed < timerLength)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / timerLength);
                float eased = ApplyTimerType(t, timerType);
                morph.val = Mathf.Lerp(startValue, endValue, eased);
                yield return null;
            }

            morph.val = endValue;
            yield break;
        }

        if (a["boolValue"] != null && a["boolValue"].Value != "")
        {
            JSONStorableBool boolParam = storable.GetBoolJSONParam(target);
            if (boolParam != null)
            {
                bool v = false;
                bool.TryParse(a["boolValue"], out v);
                boolParam.val = v;
                yield break;
            }
        }

        if (a["floatValue"] != null && a["floatValue"].Value != "")
        {
            JSONStorableFloat floatParam = storable.GetFloatJSONParam(target);
            if (floatParam != null)
            {
                float endValue = 0f;
                float.TryParse(a["floatValue"], NumberStyles.Float, CultureInfo.InvariantCulture, out endValue);

                bool useTimer = false;
                float timerLength = 0f;
                string timerType = a["timerType"];

                bool.TryParse(a["useTimer"], out useTimer);
                float.TryParse(a["timerLength"], NumberStyles.Float, CultureInfo.InvariantCulture, out timerLength);

                float startValue = floatParam.val;

                if (!useTimer || timerLength <= 0f)
                {
                    floatParam.val = endValue;
                    yield break;
                }

                float elapsed = 0f;
                while (elapsed < timerLength)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / timerLength);
                    float eased = ApplyTimerType(t, timerType);
                    floatParam.val = Mathf.Lerp(startValue, endValue, eased);
                    yield return null;
                }

                floatParam.val = endValue;
                yield break;
            }
        }

        SuperController.LogMessage(
            "UNSUPPORTED " + atomUid + "[" + index + "] " +
            name + " | atom=" + receiverAtomUid +
            " | receiver=" + receiver +
            " | target=" + target
        );
    }

    private float ApplyTimerType(float t, string timerType)
    {
        if (string.IsNullOrEmpty(timerType)) return t;
        if (timerType == "EaseInOut") return t * t * (3f - 2f * t);
        if (timerType == "EaseIn") return t * t;
        if (timerType == "EaseOut") return 1f - (1f - t) * (1f - t);
        return t;
    }

    // ============================================================
    // Receiver lifecycle
    // ============================================================
    private void ReopenReceiver()
    {
        CloseReceiver();

        try
        {
            int port = Mathf.RoundToInt(_portParam.val);
            _receiverSocket = new UdpClient(port);
            _receiverSocket.Client.ReceiveTimeout = 250;

            _receiverRunning = true;
            _receiverThread = new Thread(ReceiverLoop);
            _receiverThread.IsBackground = true;
            _receiverThread.Start();

            SuperController.LogMessage("🌐 [UDP] Receiver opened on port " + port);
        }
        catch (Exception e)
        {
            SuperController.LogError("!! [UDP OPEN ERROR] " + e.Message);
        }
    }

    private void CloseReceiver()
    {
        _receiverRunning = false;

        try
        {
            if (_receiverSocket != null)
            {
                _receiverSocket.Close();
                _receiverSocket = null;
            }
        }
        catch { }

        try
        {
            if (_receiverThread != null && _receiverThread.IsAlive)
            {
                _receiverThread.Join(200);
            }
        }
        catch { }

        _receiverThread = null;
    }

    private void ReceiverLoop()
    {
        IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);

        while (_receiverRunning)
        {
            try
            {
                if (_receiverSocket == null) break;

                byte[] data = _receiverSocket.Receive(ref remote);
                if (data == null || data.Length == 0) continue;

                string text = Encoding.UTF8.GetString(data).Trim();
                if (string.IsNullOrEmpty(text)) continue;

                lock (_incomingLock)
                {
                    _incomingCommands.Enqueue(text);
                }
            }
            catch (SocketException se)
            {
                if (se.SocketErrorCode == SocketError.TimedOut)
                    continue;

                if (_receiverRunning)
                    SuperController.LogMessage("!! [UDP RECEIVE ERROR] " + se.Message);
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (Exception e)
            {
                if (_receiverRunning)
                    SuperController.LogMessage("!! [UDP RECEIVE ERROR] " + e.Message);
            }
        }
    }

    // ============================================================
    // Update / LateUpdate
    // ============================================================
    void Update()
    {
        DrainIncomingQueue();
    }

    void LateUpdate()
    {
        AdvanceBoneMotions();
    }

    private void DrainIncomingQueue()
    {
        while (true)
        {
            string raw = null;

            lock (_incomingLock)
            {
                if (_incomingCommands.Count > 0)
                    raw = _incomingCommands.Dequeue();
            }

            if (raw == null)
                break;

            HandleIncomingText(raw);
        }
    }

    private void AdvanceBoneMotions()
    {
        if (containingAtom == null || containingAtom.mainController == null) return;
        if (_motionMap.Count == 0) return;

        Transform rootT = containingAtom.mainController.transform;
        float now = Time.time;
        _completedBones.Clear();

        foreach (var kv in _motionMap)
        {
            BoneMotionState motion = kv.Value;
            if (motion == null || !motion.Active || motion.Bone == null) continue;

            float duration = Mathf.Max(0.01f, motion.Duration);
            float t = Mathf.Clamp01((now - motion.StartTime) / duration);
            float curve = t * t * (3f - 2f * t);

            Vector3 worldTargetPos = rootT.TransformPoint(motion.TargetLocalPos);
            Quaternion worldTargetRot = rootT.rotation * motion.TargetLocalRot;

            motion.Bone.position = Vector3.Lerp(motion.StartWorldPos, worldTargetPos, curve);
            motion.Bone.rotation = Quaternion.Slerp(motion.StartWorldRot, worldTargetRot, curve);

            if (t >= 1.0f)
                _completedBones.Add(kv.Key);
        }

        foreach (string boneId in _completedBones)
        {
            BoneMotionState motion = _motionMap[boneId];
            if (motion != null) motion.Active = false;
        }
    }

    // ============================================================
    // Incoming text handling
    // ============================================================
    private void HandleIncomingText(string rawText)
    {
        string[] lines = rawText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string cmd = line.Trim();
            if (string.IsNullOrEmpty(cmd)) continue;
            DispatchCommand(cmd);
        }
    }

    private void DispatchCommand(string cmd)
    {
        if (cmd.StartsWith("🔊VOICE|") || cmd.StartsWith("VOICE|"))
        {
            ForwardVoiceCommandToBridge(cmd);
            return;
        }

        if (cmd.StartsWith("BT_", StringComparison.OrdinalIgnoreCase))
        {
            ExecuteBT(cmd.Trim());
            return;
        }

        if (cmd.StartsWith("💽POSE|"))
        {
            PoseEnvelope env;
            if (TryParsePoseEnvelope(cmd, out env))
            {
                for (int i = 0; i < env.PrePoseTriggers.Count; i++)
                    ExecuteBT(env.PrePoseTriggers[i]);

                if (env.BoneEntries.Count > 0)
                    CommitPoseEnvelope(env);
            }
            return;
        }
    }

    private bool TryParsePoseEnvelope(string cmd, out PoseEnvelope env)
    {
        env = new PoseEnvelope();

        if (string.IsNullOrEmpty(cmd) || !cmd.StartsWith("💽POSE|"))
            return false;

        string content = cmd.Substring(7);
        string[] parts = content.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

        bool poseSectionStarted = false;

        foreach (string raw in parts)
        {
            string token = raw.Trim();
            if (string.IsNullOrEmpty(token))
                continue;

            if (!poseSectionStarted && token.StartsWith("TM,", StringComparison.OrdinalIgnoreCase))
            {
                float parsedDuration;
                if (TryParseDurationToken(token, out parsedDuration))
                    env.Duration = Mathf.Max(0.01f, parsedDuration);
                continue;
            }

            if (!poseSectionStarted && token.StartsWith("TRACK,", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!poseSectionStarted && token.StartsWith("BT_", StringComparison.OrdinalIgnoreCase))
            {
                env.PrePoseTriggers.Add(token);
                continue;
            }

            if (LooksLikeBoneEntry(token))
            {
                poseSectionStarted = true;
                env.BoneEntries.Add(token);
                continue;
            }

            if (poseSectionStarted)
            {
                env.BoneEntries.Add(token);
            }
        }

        return env.BoneEntries.Count > 0;
    }

    private bool TryParseDurationToken(string token, out float duration)
    {
        duration = 1.0f;
        if (string.IsNullOrEmpty(token)) return false;

        string[] tmParts = token.Split(',');
        if (tmParts.Length < 2) return false;

        float parsed;
        if (!float.TryParse(tmParts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out parsed))
            return false;

        duration = parsed;
        return true;
    }

    private bool LooksLikeBoneEntry(string token)
    {
        if (string.IsNullOrEmpty(token)) return false;

        string[] d = token.Split(',');
        if (d.Length != 8) return false;

        string boneId = d[0].Trim();
        return _boneMap.ContainsKey(boneId);
    }

    private bool TryParseBoneEntry(string token, out string boneId, out Vector3 localPos, out Quaternion localRot)
    {
        boneId = null;
        localPos = Vector3.zero;
        localRot = Quaternion.identity;

        if (string.IsNullOrEmpty(token)) return false;

        string[] d = token.Trim().Split(',');
        if (d.Length != 8) return false;

        boneId = d[0].Trim();
        if (!_boneMap.ContainsKey(boneId)) return false;

        try
        {
            localPos = new Vector3(
                float.Parse(d[1], CultureInfo.InvariantCulture),
                float.Parse(d[2], CultureInfo.InvariantCulture),
                float.Parse(d[3], CultureInfo.InvariantCulture)
            );

            localRot = new Quaternion(
                float.Parse(d[4], CultureInfo.InvariantCulture),
                float.Parse(d[5], CultureInfo.InvariantCulture),
                float.Parse(d[6], CultureInfo.InvariantCulture),
                float.Parse(d[7], CultureInfo.InvariantCulture)
            );

            return true;
        }
        catch (Exception e)
        {
            SuperController.LogMessage("!! [POSE PARSE ERROR] " + token + " / " + e.Message);
            return false;
        }
    }

    private void CommitPoseEnvelope(PoseEnvelope env)
    {
        float now = Time.time;

        for (int i = 0; i < env.BoneEntries.Count; i++)
        {
            string boneId;
            Vector3 localPos;
            Quaternion localRot;

            if (!TryParseBoneEntry(env.BoneEntries[i], out boneId, out localPos, out localRot))
                continue;

            Transform bone;
            if (!_boneMap.TryGetValue(boneId, out bone) || bone == null)
                continue;

            BoneMotionState state;
            if (!_motionMap.TryGetValue(boneId, out state) || state == null)
            {
                state = new BoneMotionState();
                _motionMap[boneId] = state;
            }

            state.Bone = bone;
            state.StartWorldPos = bone.position;
            state.StartWorldRot = bone.rotation;
            state.TargetLocalPos = localPos;
            state.TargetLocalRot = localRot;
            state.StartTime = now;
            state.Duration = Mathf.Max(0.01f, env.Duration);
            state.Active = true;
        }

        SuperController.LogMessage("[POSE APPLY] duration=" + env.Duration.ToString("F3", CultureInfo.InvariantCulture));
    }

    // ============================================================
    // Voice bridge
    // ============================================================
    private void ForwardVoiceCommandToBridge(string cmd)
    {
        if (_bridgeVoiceCommand == null)
            DetectVoiceBridge(false);

        if (_bridgeVoiceCommand != null)
            _bridgeVoiceCommand.val = cmd;
    }

    private void DetectVoiceBridge(bool log)
    {
        foreach (var atom in SuperController.singleton.GetAtoms())
        {
            foreach (string id in atom.GetStorableIDs())
            {
                if (!id.Contains("RM_AIChatCompanionBridgeMini"))
                    continue;

                JSONStorable st = atom.GetStorableByID(id);
                if (st == null) continue;

                JSONStorableString s = st.GetStringJSONParam("VoiceCommand");
                if (s != null)
                {
                    _bridgeVoiceCommand = s;
                    if (log) SuperController.LogMessage("✅ [BRIDGE] Connected to: " + atom.uid);
                    return;
                }
            }
        }
    }

    // ============================================================
    // Bone map / export
    // ============================================================
    private void BuildBoneMap()
    {
        _boneMap.Clear();

        if (containingAtom == null || containingAtom.freeControllers == null)
            return;

        foreach (var ctrl in containingAtom.freeControllers)
        {
            if (ctrl == null) continue;
            _boneMap[ctrl.name] = ctrl.transform;
        }
    }

    private void OnSendPoseButton()
    {
        string msg = "💽POSE|" + ExportPoseToText();
        _poseDataStorage.val = msg;
        SendUDP(msg);
        SuperController.LogMessage("📤 [SENT] Pose data to Python");
    }

    private string ExportPoseToText()
    {
        if (containingAtom == null || containingAtom.mainController == null)
            return "";

        StringBuilder sb = new StringBuilder();
        Transform rootT = containingAtom.mainController.transform;

        for (int i = 0; i < _targetIDs.Length; i++)
        {
            string id = _targetIDs[i];
            Transform t;
            if (!_boneMap.TryGetValue(id, out t) || t == null)
                continue;

            Vector3 lp = rootT.InverseTransformPoint(t.position);
            Quaternion lr = Quaternion.Inverse(rootT.rotation) * t.rotation;

            sb.AppendFormat(
                CultureInfo.InvariantCulture,
                "{0},{1:F3},{2:F3},{3:F3},{4:F3},{5:F3},{6:F3},{7:F3}|",
                id, lp.x, lp.y, lp.z, lr.x, lr.y, lr.z, lr.w
            );
        }

        return sb.ToString();
    }

    private void SendUDP(string message)
    {
        try
        {
            if (_receiverSocket != null)
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                _receiverSocket.Send(data, data.Length, "127.0.0.1", 9999);
            }
        }
        catch (Exception e)
        {
            SuperController.LogMessage("!! [UDP SEND ERROR] " + e.Message);
        }
    }

    // ============================================================
    // Cleanup
    // ============================================================
    void OnDestroy()
    {
        CloseReceiver();
    }
}