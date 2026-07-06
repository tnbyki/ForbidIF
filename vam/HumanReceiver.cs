// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// FILE:HumanReceiver_v026_hum_life_external_if.cs
// V026_HUM_LIFE_EXTERNAL_IF: Adds HUM_LIFE("state:sleep" / "expression:like" / "personality:bold") external interface that routes to HumanLifeAction actions.
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
        public bool ApplyRotation = true;
    }

private class PoseEnvelope
{
    public float Duration = 1.0f;
    public bool IsLoop = false; // ★追加
    public bool HasHeadTrigger = false;
    public List<string> PrePoseTriggers = new List<string>();
    public List<HumAction> HumTriggers = new List<HumAction>();
    public List<string> BoneEntries = new List<string>();
}

private class HumAction
{
    public string Kind;
    public string ActionName;
}

    // ============================================================
    // UI / VaM
    // ============================================================
    private JSONStorableFloat _portParam;
    private JSONStorableString _poseDataStorage;
    private InputField _poseDataInputField;
    private JSONStorableString _bridgeVoiceCommand;

    // ★ SEND command
    private JSONStorableString _pySendInput;
    private JSONStorableString _lastPySend;
    
    // ★ 追加：回転反映ON/OFF
    private JSONStorableBool _applyHeadRotation;
    private JSONStorableBool _applyHandRotation;
    private JSONStorableBool _stateReportEnabled;
    private JSONStorableBool _handMotionReportEnabled;
    private JSONStorableBool _characterStateEnabled;
    private JSONStorableBool _debugLogEnabled;
    private JSONStorableFloat _handMotionThreshold;
    private JSONStorableFloat _stateReportCooldown;

    // ============================================================
    // ネットワーク
    // ============================================================
    private UdpClient _receiverSocket;
    private Thread _receiverThread;
    private volatile bool _receiverRunning = false;

    // 受信スレッド → メインスレッド
    private readonly Queue<string> _incomingCommands = new Queue<string>();
    private readonly object _incomingLock = new object();
    private const int MaxIncomingQueue = 96;
    private const int MaxCommandsPerFrame = 8;

    // Repeated lookups in VaM are fairly expensive, so cache stable references.
    private readonly Dictionary<string, JSONNode> _triggerCache = new Dictionary<string, JSONNode>();
    private JSONStorableString _bubbleTextTarget;
    private string _lastBubbleText = null;

    // ============================================================
    // ボーン / モーション
    // ============================================================
    private readonly Dictionary<string, Transform> _boneMap = new Dictionary<string, Transform>();
    private readonly Dictionary<string, BoneMotionState> _motionMap = new Dictionary<string, BoneMotionState>();
    private readonly List<string> _completedBones = new List<string>();

private Coroutine _currentUIButtonRoutine;
private Coroutine _localPoseApplyRoutine;
    private Vector3 _lastRHandWorldPos;
    private Vector3 _lastLHandWorldPos;
    private bool _handMotionInitialized = false;
    private float _lastHandMotionReportTime = -9999f;
    private float _lastCharacterStateSendTime = -9999f;

    // ★ SEND POSE 録画設定
    private JSONStorableFloat _recordDuration;
    private JSONStorableFloat _recordInterval;
    private Coroutine _recordPoseRoutine;

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

        // ★ 追加：回転反映チェック
        _applyHeadRotation = new JSONStorableBool("Apply Head Loop", true);
        RegisterBool(_applyHeadRotation);
        CreateToggle(_applyHeadRotation, true);

        _applyHandRotation = new JSONStorableBool("Apply Hand Rotation", true);
        RegisterBool(_applyHandRotation);
        CreateToggle(_applyHandRotation, true);

        _stateReportEnabled = new JSONStorableBool("State Report", true);
        RegisterBool(_stateReportEnabled);
        CreateToggle(_stateReportEnabled, true);

        _handMotionReportEnabled = new JSONStorableBool("Hand Motion Report", false);
        RegisterBool(_handMotionReportEnabled);
        CreateToggle(_handMotionReportEnabled, true);

        _characterStateEnabled = new JSONStorableBool("Character State", true);
        RegisterBool(_characterStateEnabled);
        CreateToggle(_characterStateEnabled, true);

        _debugLogEnabled = new JSONStorableBool("Debug Log", true);
        RegisterBool(_debugLogEnabled);
        CreateToggle(_debugLogEnabled, true);

        _handMotionThreshold = new JSONStorableFloat("Hand Motion Threshold", 1.2f, 0.2f, 5.0f, true, true);
        RegisterFloat(_handMotionThreshold);
        CreateSlider(_handMotionThreshold, true);

        _stateReportCooldown = new JSONStorableFloat("State Report Cooldown", 20.0f, 3.0f, 120.0f, true, true);
        RegisterFloat(_stateReportCooldown);
        CreateSlider(_stateReportCooldown, true);

        // ============================================================
        // SEND / RECORD POSE UI  ※右側にまとめる
        // ============================================================
        JSONStorableAction sendCurrentPoseAction = new JSONStorableAction(
            "Send Current Pose",
            OnSendCurrentPoseButton
        );
        RegisterAction(sendCurrentPoseAction);
        CreateButton("Send Current Pose", false).button.onClick.AddListener(OnSendCurrentPoseButton);


        JSONStorableAction recordPoseAction = new JSONStorableAction(
            "Record Pose And Send",
            OnSendPoseButton
        );
        RegisterAction(recordPoseAction);

        CreateButton("Record Pose And Send", false).button.onClick.AddListener(OnSendPoseButton);

        JSONStorableAction legacySendPoseAction = new JSONStorableAction(
            "Send Pose to Python",
            OnSendPoseButton
        );
        RegisterAction(legacySendPoseAction);

        _recordDuration = new JSONStorableFloat("Record Seconds", 3.0f, 0.1f, 30.0f, true, true);
        RegisterFloat(_recordDuration);
        CreateSlider(_recordDuration, false);

        _recordInterval = new JSONStorableFloat("Capture Interval", 0.5f, 0.03f, 2.0f, true, true);
        RegisterFloat(_recordInterval);
        CreateSlider(_recordInterval, false);


        _poseDataStorage = new JSONStorableString("Last Pose Send Data", "");
        RegisterString(_poseDataStorage);
        UIDynamicTextField poseTextHost = CreateTextField(new JSONStorableString(" ", ""), false);
        poseTextHost.height = 300f;
        BuildEditableMultilinePoseInput(poseTextHost.gameObject, _poseDataStorage);

        JSONStorableAction capturePoseTextAction = new JSONStorableAction(
            "Capture Pose Text",
            OnCapturePoseTextButton
        );
        RegisterAction(capturePoseTextAction);
        CreateButton("Capture Pose Text", false).button.onClick.AddListener(OnCapturePoseTextButton);

        JSONStorableAction applyPoseTextAction = new JSONStorableAction(
            "Apply Pose Text",
            OnApplyPoseTextButton
        );
        RegisterAction(applyPoseTextAction);
        CreateButton("Apply Pose Text", false).button.onClick.AddListener(OnApplyPoseTextButton);

        // ★ SEND command input / last sent
        _pySendInput = new JSONStorableString("Command To Python", "");
        _pySendInput.setCallbackFunction = (s) =>
        {
            if (string.IsNullOrEmpty(s)) return;

            SendUDP(s);

            if (_lastPySend != null)
                _lastPySend.val = s;

            SuperController.LogMessage("📤 [AUTO SEND] " + s);

            if (_pySendInput != null && _pySendInput.val == s)
                _pySendInput.valNoCallback = "";
        };
        RegisterString(_pySendInput);
        CreateTextField(_pySendInput, false).height = 80f;

        _lastPySend = new JSONStorableString("Last Command Sent", "");
        RegisterString(_lastPySend);
        CreateTextField(_lastPySend, false).height = 80f;

        JSONStorableAction sendCommandAction = new JSONStorableAction(
            "Send Command",
            OnSendCommandToPython
        );
        RegisterAction(sendCommandAction);

        CreateButton("Send Command", false).button.onClick.AddListener(OnSendCommandToPython);

        CreateButton("Auto Detect Voice Bridge", true).button.onClick.AddListener(() => { DetectVoiceBridge(true); });
        CreateButton("Check All BT_ Atoms", true).button.onClick.AddListener(LogAllAtoms);

        JSONStorableAction handPoseAction = new JSONStorableAction(
            "BT_HAND_pose",
            ApplyHandPoseIK
        );
        RegisterAction(handPoseAction);
        CreateButton("BT_HAND_pose", false).button.onClick.AddListener(ApplyHandPoseIK);

        JSONStorableAction handFreeAction = new JSONStorableAction(
            "BT_HAND_free",
            ApplyHandFreeIK
        );
        RegisterAction(handFreeAction);
        CreateButton("BT_HAND_free", false).button.onClick.AddListener(ApplyHandFreeIK);

        BuildBoneMap();
        ReopenReceiver();
        DetectVoiceBridge(false);

        SuperController.LogMessage("MyHumanReceiver: 🟢 reworked receiver started");
        // Full BT scanning is useful for debugging, but noisy and expensive at startup.
        // Use the "Check All BT_ Atoms" button when you need it.
    }

    // ============================================================
    // Atom / BT utility
    // ============================================================
    private void LogAllAtoms()
    {
        _triggerCache.Clear();
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



private FreeControllerV3 FindController(string controllerName)
{
    if (containingAtom == null || containingAtom.freeControllers == null || string.IsNullOrEmpty(controllerName))
        return null;

    for (int i = 0; i < containingAtom.freeControllers.Length; i++)
    {
        FreeControllerV3 c = containingAtom.freeControllers[i];
        if (c != null && c.name == controllerName)
            return c;
    }

    return null;
}

private void SetControllerIK(string controllerName, bool enabled)
{
    FreeControllerV3 c = FindController(controllerName);
    if (c == null)
    {
        if (_debugLogEnabled == null || _debugLogEnabled.val)
            SuperController.LogMessage("[HAND IK] controller not found: " + controllerName);
        return;
    }

    try
    {
        c.currentPositionState = enabled
            ? FreeControllerV3.PositionState.On
            : FreeControllerV3.PositionState.Off;

        c.currentRotationState = enabled
            ? FreeControllerV3.RotationState.On
            : FreeControllerV3.RotationState.Off;
    }
    catch (Exception e)
    {
        SuperController.LogMessage("!! [HAND IK] failed: " + controllerName + " / " + e.Message);
    }
}

private void SetHandArmIK(bool enabled)
{
    SetControllerIK("rHandControl", enabled);
    SetControllerIK("lHandControl", enabled);
    SetControllerIK("rElbowControl", enabled);
    SetControllerIK("lElbowControl", enabled);

//    SuperController.LogMessage("[HAND IK] " + (enabled ? "BT_HAND_pose / hands+elbows ON" : "BT_HAND_free / hands+elbows OFF"));
}

private void ApplyHandPoseIK()
{
    SetHandArmIK(true);
}

private void ApplyHandFreeIK()
{
    SetHandArmIK(false);
}

private void ExecuteBT(string atomUid)
{
    string simpleCmd = NormalizeSimpleCommand(atomUid);

    if (simpleCmd.Equals("BT_HAND_pose", StringComparison.OrdinalIgnoreCase))
    {
        ApplyHandPoseIK();
        return;
    }

    if (simpleCmd.Equals("BT_HAND_free", StringComparison.OrdinalIgnoreCase))
    {
        ApplyHandFreeIK();
        return;
    }

    if (simpleCmd.Equals("POSE_SEND", StringComparison.OrdinalIgnoreCase))
    {
        OnSendPoseButton();
        return;
    }

    JSONNode trigger = GetTriggerNode(atomUid);
    if (trigger == null)
        return;

    JSONNode arr = trigger["startActions"];
    if (arr == null || arr.Count == 0)
    {
        return;
    }

    if (_currentUIButtonRoutine != null)
    {
        StopCoroutine(_currentUIButtonRoutine);
        _currentUIButtonRoutine = null;
    }

//    SuperController.LogMessage("[ExecuteBT] TriggerRunner -> " + atomUid);
    _currentUIButtonRoutine = StartCoroutine(RunActionArray(arr, atomUid));
}
    private JSONNode GetTriggerNode(string atomUid)
    {
        JSONNode cached;
        if (_triggerCache.TryGetValue(atomUid, out cached) && cached != null)
            return cached;

        Atom atom = SuperController.singleton.GetAtomByUid(atomUid);
        if (atom == null)
        {
            if (_debugLogEnabled == null || _debugLogEnabled.val)
                SuperController.LogMessage("[BT MISS] " + atomUid + " not found");
            return null;
        }

        JSONStorable storable = atom.GetStorableByID("Trigger");
        if (storable == null)
        {
            return null;
        }

        JSONClass json = storable.GetJSON();
        if (json == null)
        {
            return null;
        }

        JSONNode trigger = json["trigger"];
        if (trigger == null)
        {
            return null;
        }

        _triggerCache[atomUid] = trigger;
        return trigger;
    }

    private IEnumerator RunActionArray(JSONNode arr, string atomUid)
    {
//        SuperController.LogMessage("RUN UIButton START: " + atomUid + " / count=" + arr.Count);

        for (int i = 0; i < arr.Count; i++)
        {
            JSONNode a = arr[i];
            yield return StartCoroutine(RunActionNode(a, atomUid, i));
        }

//        SuperController.LogMessage("RUN UIButton END: " + atomUid);
        _currentUIButtonRoutine = null;
    }

private IEnumerator RunActionNode(JSONNode a, string atomUid, int index)
{
//    SuperController.LogMessage(
//        "[BT ACTION JSON] " + atomUid + "[" + index + "] " + a.ToString()
//    );

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

        bool useSecondPoint = false;
        float secondPointValue = 0f;
        float secondPointLocation = 0.5f;

        bool.TryParse(a["useTimer"], out useTimer);
        float.TryParse(a["timerLength"], NumberStyles.Float, CultureInfo.InvariantCulture, out timerLength);

        bool.TryParse(a["useSecondTimerPoint"], out useSecondPoint);
        float.TryParse(a["secondTimerPointValue"], NumberStyles.Float, CultureInfo.InvariantCulture, out secondPointValue);
        float.TryParse(a["secondTimerPointCurveLocation"], NumberStyles.Float, CultureInfo.InvariantCulture, out secondPointLocation);

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

        yield return StartCoroutine(RunMorphTimerWithSecondPoint(
            morph,
            startValue,
            endValue,
            timerLength,
            timerType,
            useSecondPoint,
            secondPointValue,
            secondPointLocation
        ));

        SuperController.LogMessage(
            "RUN OK " + atomUid + "[" + index + "] " +
            name + " | " + receiverAtomUid + " | " + receiver + " | " +
            morphName + " = " + endValue.ToString(CultureInfo.InvariantCulture) +
            " / timer=" + timerLength.ToString("F3", CultureInfo.InvariantCulture) +
            " / second=" + useSecondPoint
        );

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

            SuperController.LogMessage(
                "RUN OK " + atomUid + "[" + index + "] " +
                name + " | atom=" + receiverAtomUid +
                " | receiver=" + receiver +
                " | target=" + target +
                " | bool=" + v
            );
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

                SuperController.LogMessage(
                    "RUN OK " + atomUid + "[" + index + "] " +
                    name + " | atom=" + receiverAtomUid +
                    " | receiver=" + receiver +
                    " | target=" + target +
                    " | float=" + endValue.ToString(CultureInfo.InvariantCulture)
                );
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

    string actionValue = "";
    if (a["stringValue"] != null && a["stringValue"].Value != "")
    {
        actionValue = a["stringValue"].Value.Trim();
    }

    if (string.IsNullOrEmpty(actionValue) && !string.IsNullOrEmpty(name))
    {
        int colon = name.LastIndexOf(':');
        if (colon >= 0 && colon + 1 < name.Length)
            actionValue = name.Substring(colon + 1).Trim();
    }

    if (!string.IsNullOrEmpty(actionValue))
    {
        JSONStorableStringChooser chooserParam = storable.GetStringChooserJSONParam(target);
        if (chooserParam != null)
        {
            chooserParam.val = actionValue;

            SuperController.LogMessage(
                "RUN OK " + atomUid + "[" + index + "] " +
                name + " | atom=" + receiverAtomUid +
                " | receiver=" + receiver +
                " | target=" + target +
                " | chooser=" + actionValue
            );
            yield break;
        }

        JSONStorableString strParam = storable.GetStringJSONParam(target);
        if (strParam != null)
        {
            strParam.val = actionValue;

            SuperController.LogMessage(
                "RUN OK " + atomUid + "[" + index + "] " +
                name + " | atom=" + receiverAtomUid +
                " | receiver=" + receiver +
                " | target=" + target +
                " | string=" + actionValue
            );
            yield break;
        }

        JSONStorableBool boolParamFromString = storable.GetBoolJSONParam(target);
        if (boolParamFromString != null)
        {
            bool v =
                actionValue.Equals("on", StringComparison.OrdinalIgnoreCase) ||
                actionValue.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                actionValue.Equals("1", StringComparison.OrdinalIgnoreCase);

            if (
                actionValue.Equals("off", StringComparison.OrdinalIgnoreCase) ||
                actionValue.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                actionValue.Equals("0", StringComparison.OrdinalIgnoreCase)
            )
            {
                v = false;
            }

            boolParamFromString.val = v;

            SuperController.LogMessage(
                "RUN OK " + atomUid + "[" + index + "] " +
                name + " | atom=" + receiverAtomUid +
                " | receiver=" + receiver +
                " | target=" + target +
                " | bool(from string)=" + v
            );
            yield break;
        }

        JSONStorableFloat floatParamFromString = storable.GetFloatJSONParam(target);
        if (floatParamFromString != null)
        {
            float fv;
            if (float.TryParse(actionValue, NumberStyles.Float, CultureInfo.InvariantCulture, out fv))
            {
                floatParamFromString.val = fv;

                SuperController.LogMessage(
                    "RUN OK " + atomUid + "[" + index + "] " +
                    name + " | atom=" + receiverAtomUid +
                    " | receiver=" + receiver +
                    " | target=" + target +
                    " | float(from string)=" + fv.ToString(CultureInfo.InvariantCulture)
                );
                yield break;
            }
        }
    }

    // ============================================================
    // Plugin Action fallback
    // 例:
    // receiver = plugin#10_Foost.SexyFluids
    // target   = squirt:start / lnipple:start / rnipple:start
    // ============================================================
    if (!string.IsNullOrEmpty(target))
    {
        JSONStorableAction action = storable.GetAction(target);
        if (action != null)
        {
            action.actionCallback.Invoke();

            SuperController.LogMessage(
                "RUN ACTION OK " + atomUid + "[" + index + "] " +
                name + " | atom=" + receiverAtomUid +
                " | receiver=" + receiver +
                " | target=" + target
            );
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

    private IEnumerator RunMorphTimerWithSecondPoint(
        JSONStorableFloat morph,
        float startValue,
        float endValue,
        float timerLength,
        string timerType,
        bool useSecondPoint,
        float secondPointValue,
        float secondPointLocation
    )
    {
        if (morph == null)
            yield break;

        timerLength = Mathf.Max(0.01f, timerLength);
        secondPointLocation = Mathf.Clamp(secondPointLocation, 0.01f, 0.99f);

        float elapsed = 0f;

        while (elapsed < timerLength)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / timerLength);

            if (useSecondPoint)
            {
                if (t <= secondPointLocation)
                {
                    float localT = Mathf.Clamp01(t / secondPointLocation);
                    float eased = ApplyTimerType(localT, timerType);
                    morph.val = Mathf.Lerp(startValue, secondPointValue, eased);
                }
                else
                {
                    float localT = Mathf.Clamp01((t - secondPointLocation) / (1f - secondPointLocation));
                    float eased = ApplyTimerType(localT, timerType);
                    morph.val = Mathf.Lerp(secondPointValue, endValue, eased);
                }
            }
            else
            {
                float eased = ApplyTimerType(t, timerType);
                morph.val = Mathf.Lerp(startValue, endValue, eased);
            }

            yield return null;
        }

        morph.val = endValue;
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
            _receiverSocket.Client.ReceiveBufferSize = 65536;

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
                    while (_incomingCommands.Count >= MaxIncomingQueue)
                        _incomingCommands.Dequeue();
                    _incomingCommands.Enqueue(text);
                }
            }
            catch (SocketException se)
            {
                if (se.SocketErrorCode == SocketError.TimedOut)
                    continue;

                // Windows UDP can report ICMP "port unreachable" as ConnectionReset.
                // It is noisy and does not mean the VaM receiver itself must stop.
                if (se.SocketErrorCode == SocketError.ConnectionReset)
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
        DetectHandMotionStateReport();
        SendCharacterStateIfNeeded();
    }

    void LateUpdate()
    {
        AdvanceBoneMotions();
    }

    private void DrainIncomingQueue()
    {
        int processed = 0;
        while (processed < MaxCommandsPerFrame)
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
            processed++;
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
            //float curve = t * t * (3f - 2f * t);
			float curve = t;
			//float curve = Mathf.Lerp( t,t * t * (3f - 2f * t), 0.15f);
			//float curve = Mathf.Lerp( t,t * t * (3f - 2f * t), 0.25f);
			//float curve = 1f - (1f - t) * (1f - t);
			
            Vector3 worldTargetPos = rootT.TransformPoint(motion.TargetLocalPos);
            Quaternion worldTargetRot = rootT.rotation * motion.TargetLocalRot;

            motion.Bone.position = Vector3.Lerp(motion.StartWorldPos, worldTargetPos, curve);
            if (motion.ApplyRotation)
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

    private void DetectHandMotionStateReport()
    {
        if (_stateReportEnabled == null || !_stateReportEnabled.val) return;
        if (_handMotionReportEnabled == null || !_handMotionReportEnabled.val) return;
        if (_boneMap == null || _boneMap.Count == 0) return;
        if (Time.deltaTime <= 0.0001f) return;
        if (IsAnyBoneMotionActive()) return;

        Transform rHand;
        Transform lHand;
        bool hasR = _boneMap.TryGetValue("rHandControl", out rHand) && rHand != null;
        bool hasL = _boneMap.TryGetValue("lHandControl", out lHand) && lHand != null;
        if (!hasR && !hasL) return;

        Vector3 currentR = hasR ? rHand.position : Vector3.zero;
        Vector3 currentL = hasL ? lHand.position : Vector3.zero;

        if (!_handMotionInitialized)
        {
            _lastRHandWorldPos = currentR;
            _lastLHandWorldPos = currentL;
            _handMotionInitialized = true;
            return;
        }

        float maxSpeed = 0f;
        if (hasR)
            maxSpeed = Mathf.Max(maxSpeed, Vector3.Distance(currentR, _lastRHandWorldPos) / Time.deltaTime);
        if (hasL)
            maxSpeed = Mathf.Max(maxSpeed, Vector3.Distance(currentL, _lastLHandWorldPos) / Time.deltaTime);

        _lastRHandWorldPos = currentR;
        _lastLHandWorldPos = currentL;

        float threshold = _handMotionThreshold != null ? _handMotionThreshold.val : 1.2f;
        float strongThreshold = threshold * 2.4f;
        if (maxSpeed < strongThreshold) return;

        float cooldown = _stateReportCooldown != null ? _stateReportCooldown.val : 20.0f;
        if (Time.time - _lastHandMotionReportTime < cooldown) return;

        _lastHandMotionReportTime = Time.time;

        string msg = string.Format(
            CultureInfo.InvariantCulture,
            "STATE_REPORT|source=vam|kind=interaction|level=notice|target=hand|action=active_motion|strength=strong|speed={0:F2}|summary=ユーザーの手の動きが大きく見えた。接触や意図はまだ判定していない",
            maxSpeed
        );
        SendUDP(msg);
        SuperController.LogMessage("[STATE_REPORT] hand active_motion / strong / speed=" + maxSpeed.ToString("F2", CultureInfo.InvariantCulture));
    }

    private bool IsAnyBoneMotionActive()
    {
        foreach (var kv in _motionMap)
        {
            BoneMotionState motion = kv.Value;
            if (motion != null && motion.Active)
                return true;
        }
        return false;
    }

    private void SendCharacterStateIfNeeded()
    {
        if (_characterStateEnabled == null || !_characterStateEnabled.val) return;
        if (Time.time - _lastCharacterStateSendTime < 1.0f) return;
        if (_boneMap == null || _boneMap.Count == 0) return;

        Transform head;
        if (!_boneMap.TryGetValue("headControl", out head) || head == null) return;

        Transform gazeTarget = FindMainCameraTarget();
        Transform distanceTarget = FindCamCubeTarget();
        if (gazeTarget == null && distanceTarget == null) return;
        if (gazeTarget == null) gazeTarget = distanceTarget;
        if (distanceTarget == null) distanceTarget = gazeTarget;

        _lastCharacterStateSendTime = Time.time;

        Vector3 toGazeTarget = gazeTarget.position - head.position;
        if (toGazeTarget.magnitude <= 0.0001f) return;

        Vector3 dir = toGazeTarget.normalized;
        float angleForward = Vector3.Angle(head.forward, dir);
        float angleUp = Vector3.Angle(head.up, dir);
        float angleRight = Vector3.Angle(head.right, dir);
        float angleMinusRight = Vector3.Angle(-head.right, dir);

        float angle = angleForward;
        string gaze = "measure";

        float distance = Vector3.Distance(gazeTarget.position, distanceTarget.position);
        string distanceBand = "far";
        if (distance <= 1.20f)
            distanceBand = "touch";
        else if (distance <= 1.50f)
            distanceBand = "talk";

        string lookTarget = "none";
        string lookTargetAtom = "";
        float lookTargetDistance = -1.0f;
        DetectUserLookTargetByCamCube(distanceTarget, gazeTarget, distanceBand, out lookTarget, out lookTargetAtom, out lookTargetDistance);

        string gazeTargetName = EscapeJson(gazeTarget.name);
        string distanceTargetName = EscapeJson(distanceTarget.name);
        string msg = string.Format(
            CultureInfo.InvariantCulture,
            "CHARACTER_STATE|{{\"character_gaze\":\"{0}\",\"character_gaze_angle\":{1:F1},\"angle_forward\":{2:F1},\"angle_up\":{3:F1},\"angle_right\":{4:F1},\"angle_minus_right\":{5:F1},\"character_distance\":\"{6}\",\"distance\":{7:F3},\"gaze_from\":\"headControl\",\"gaze_target\":\"{8}\",\"distance_from\":\"{9}\",\"distance_to\":\"{10}\",\"target\":\"{10}\",\"look_target\":\"{11}\",\"look_target_atom\":\"{12}\",\"look_target_distance\":{13:F3}}}",
            gaze,
            angle,
            angleForward,
            angleUp,
            angleRight,
            angleMinusRight,
            distanceBand,
            distance,
            gazeTargetName,
            EscapeJson(gazeTarget.name),
            distanceTargetName,
            EscapeJson(lookTarget),
            EscapeJson(lookTargetAtom),
            lookTargetDistance
        );
        SendUDP(msg);
    }

    private void DetectUserLookTargetByCamCube(
        Transform camCubeT,
        Transform cameraT,
        string distanceBand,
        out string lookTarget,
        out string lookTargetAtom,
        out float lookTargetDistance
    )
    {
        lookTarget = "none";
        lookTargetAtom = "";
        lookTargetDistance = -1.0f;

        if (camCubeT == null) return;

        if (distanceBand == "far")
        {
            lookTarget = IsCameraInFront(cameraT) ? "front_body" : "back_body";
            lookTargetAtom = "body";
            lookTargetDistance = 0.0f;
            return;
        }

        string[] controls = new string[] { "headControl", "chestControl", "hipControl" };
        string[] labels = new string[] { "face", "chest", "hip" };

        for (int i = 0; i < controls.Length; i++)
        {
            Transform t;
            if (!_boneMap.TryGetValue(controls[i], out t) || t == null) continue;

            float d = Vector3.Distance(camCubeT.position, t.position);
            if (lookTargetDistance < 0.0f || d < lookTargetDistance)
            {
                lookTargetDistance = d;
                lookTargetAtom = controls[i];
                lookTarget = labels[i];
            }
        }

        if (lookTargetAtom == "hipControl")
            lookTarget = IsCameraInFront(cameraT) ? "belly" : "butt";
    }

    private bool IsCameraInFront(Transform cameraT)
    {
        if (cameraT == null || containingAtom == null || containingAtom.mainController == null)
            return true;

        Vector3 toCamera = cameraT.position - containingAtom.mainController.transform.position;
        if (toCamera.magnitude <= 0.0001f) return true;
        float dot = Vector3.Dot(containingAtom.mainController.transform.forward, toCamera.normalized);
        return dot >= 0.0f;
    }

    private Transform FindCamCubeTarget()
    {
        try
        {
            Atom camCube = SuperController.singleton.GetAtomByUid("CAM_cube");
            if (camCube != null && camCube.mainController != null)
                return camCube.mainController.transform;
        }
        catch { }

        return null;
    }

    private Transform FindMainCameraTarget()
    {
        try
        {
            if (Camera.main != null)
                return Camera.main.transform;
        }
        catch { }

        return null;
    }

    private string EscapeJson(string value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
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
    private string NormalizeSimpleCommand(string cmd)
    {
        if (string.IsNullOrEmpty(cmd))
            return "";

        string s = cmd.Trim();

        if (s.EndsWith("|#", StringComparison.Ordinal))
            s = s.Substring(0, s.Length - 2).Trim();

        if (s.EndsWith("#", StringComparison.Ordinal))
            s = s.Substring(0, s.Length - 1).Trim();

        s = s.Trim('|').Trim();
        return s;
    }

    private void DispatchCommand(string cmd)
    {
        string simpleCmd = NormalizeSimpleCommand(cmd);

        if (simpleCmd.Equals("BT_HAND_pose", StringComparison.OrdinalIgnoreCase))
        {
            ApplyHandPoseIK();
            return;
        }

        if (simpleCmd.Equals("BT_HAND_free", StringComparison.OrdinalIgnoreCase))
        {
            ApplyHandFreeIK();
            return;
        }

        if (simpleCmd.Equals("POSE_SEND", StringComparison.OrdinalIgnoreCase))
        {
            OnSendPoseButton();
            return;
        }

        if (cmd.StartsWith("🔊VOICE|") || cmd.StartsWith("VOICE|"))
        {
            SuperController.LogMessage("[VOICE] incoming");
            ForwardVoiceCommandToBridge(cmd);

            string voiceText = ExtractVoiceText(cmd);
            if (!string.IsNullOrEmpty(voiceText))
            {
//                SetBubbleText("Person", "SpeechBubble", "bubbleText", voiceText);
string bubble = "・・・・・";

if (voiceText == "?")
{
    bubble = "Thinking.......";
}

SetBubbleText("Person", "SpeechBubble", "bubbleText", bubble);
            }

            return;
        }

        if (cmd.Equals("VAM_SEND|MOTION_STOP", StringComparison.OrdinalIgnoreCase))
        {
            StopAllBoneMotions();
            return;
        }

        if (cmd.StartsWith("HUM_MOTION_DICT|", StringComparison.OrdinalIgnoreCase))
        {
            string dictText = UnescapeInlineText(cmd.Substring("HUM_MOTION_DICT|".Length));
            if (TrySetPluginString("Motion Dictionary", dictText, "HumanMotionControl", true))
            {
                TryExecutePluginAction("Reload Motions", "HumanMotionControl", true);
                SuperController.LogMessage("[HUM] Motion Dictionary -> HumanMotionControl / chars=" + dictText.Length);
            }
            else
            {
                SuperController.LogMessage("[HUM] Motion Dictionary receiver not found");
            }
            return;
        }

        HumAction humAction;
        if (TryParseHumCommand(cmd, out humAction))
        {
            StartCoroutine(RunHumActionAfterCurrentPose(humAction));
            return;
        }

        if (cmd.StartsWith("BT_", StringComparison.OrdinalIgnoreCase))
        {
            ExecuteBT(cmd.Trim());
            return;
        }

        if (IsPoseCommand(cmd))
        {
            PoseEnvelope env;
            if (TryParsePoseEnvelope(cmd, out env))
            {
                for (int i = 0; i < env.PrePoseTriggers.Count; i++)
                    ExecuteBT(env.PrePoseTriggers[i]);

                if (env.BoneEntries.Count > 0)
                    CommitPoseEnvelope(env);

                if (env.HumTriggers.Count > 0)
                {
                    if (env.BoneEntries.Count > 0)
                    {
                        for (int i = 0; i < env.HumTriggers.Count; i++)
                            ExecuteHumAction(env.HumTriggers[i]);
                    }
                    else
                    {
                        StartCoroutine(RunHumActionsAfterCurrentPose(env.HumTriggers));
                    }
                }

            }
            return;
        }
    }
    private void StopAllBoneMotions()
    {
        foreach (var kv in _motionMap)
        {
            BoneMotionState motion = kv.Value;
            if (motion != null)
                motion.Active = false;
        }

        SuperController.LogMessage("[POSE] motion stop requested");
    }
    private string ExtractVoiceText(string cmd)
    {
        if (string.IsNullOrEmpty(cmd)) return "";

        string[] parts = cmd.Split('|');
        foreach (string raw in parts)
        {
            string p = raw.Trim();
            if (p.StartsWith("text=", StringComparison.OrdinalIgnoreCase))
            {
                return p.Substring(5);
            }
        }

        return "";
    }

    private string UnescapeInlineText(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";

        return text
            .Replace("\\r", "\r")
            .Replace("\\n", "\n")
            .Replace("\\\\", "\\");
    }

    private bool TryParseHumCommand(string cmd, out HumAction humAction)
    {
        humAction = null;
        if (string.IsNullOrEmpty(cmd)) return false;

        string s = cmd.Trim().Trim('|').Trim();
        if (s.EndsWith("#", StringComparison.Ordinal))
            s = s.Substring(0, s.Length - 1).Trim();

        int open = s.IndexOf('(');
        int close = s.LastIndexOf(')');
        if (open <= 0 || close <= open)
            return false;

        string prefix = s.Substring(0, open).Trim().ToUpperInvariant();
        if (
            prefix != "HUM_HEAD" &&
            prefix != "HUM_HAND" &&
            prefix != "HUM_LEG" &&
            prefix != "HUM_MOTION" &&
            prefix != "HUM_LIFE"
        )
        {
            return false;
        }

        string actionName = s.Substring(open + 1, close - open - 1).Trim();
        if (
            (actionName.StartsWith("\"", StringComparison.Ordinal) && actionName.EndsWith("\"", StringComparison.Ordinal)) ||
            (actionName.StartsWith("'", StringComparison.Ordinal) && actionName.EndsWith("'", StringComparison.Ordinal))
        )
        {
            actionName = actionName.Substring(1, actionName.Length - 2).Trim();
        }

        if (string.IsNullOrEmpty(actionName))
            return false;

        humAction = new HumAction
        {
            Kind = prefix.Substring("HUM_".Length),
            ActionName = actionName
        };
        return true;
    }

    private IEnumerator RunHumActionAfterCurrentPose(HumAction humAction)
    {
        float start = Time.time;
        while (HasActiveBoneMotions() && (Time.time - start) < 10.0f)
            yield return null;

        ExecuteHumAction(humAction);
    }

    private IEnumerator RunHumActionsAfterCurrentPose(List<HumAction> humActions)
    {
        float start = Time.time;
        while (HasActiveBoneMotions() && (Time.time - start) < 10.0f)
            yield return null;

        if (humActions == null)
            yield break;

        for (int i = 0; i < humActions.Count; i++)
        {
            ExecuteHumAction(humActions[i]);
            yield return null;
        }
    }

    private bool HasActiveBoneMotions()
    {
        foreach (var kv in _motionMap)
        {
            BoneMotionState motion = kv.Value;
            if (motion != null && motion.Active)
                return true;
        }

        return false;
    }

    private bool ExecuteHumAction(HumAction humAction)
    {
        if (humAction == null || string.IsNullOrEmpty(humAction.ActionName))
            return false;

        string preferredPlugin = "";
        if (humAction.Kind == "LIFE")
        {
            return ExecuteHumLifeAction(humAction.ActionName);
        }
        if (humAction.Kind == "HEAD") preferredPlugin = "HumanHeadOpenControl";
        else if (humAction.Kind == "HAND") preferredPlugin = "HumanHandOpenControl";
        else if (humAction.Kind == "LEG") preferredPlugin = "HumanLegOpenControl";
        else if (humAction.Kind == "MOTION")
        {
            if (TrySetPluginString("Play Motion", humAction.ActionName, "HumanMotionControl", true))
            {
                SuperController.LogMessage("[HUM] MOTION -> " + humAction.ActionName);
                return true;
            }

            if (TrySetPluginString("Play Motion", humAction.ActionName, "", false))
            {
                SuperController.LogMessage("[HUM] MOTION -> " + humAction.ActionName);
                return true;
            }

            return false;
        }

        if (TryExecutePluginAction(humAction.ActionName, preferredPlugin, true))
        {
            SuperController.LogMessage("[HUM] " + humAction.Kind + " -> " + humAction.ActionName);
            return true;
        }

        if (TryExecutePluginAction(humAction.ActionName, "", false))
        {
            SuperController.LogMessage("[HUM] " + humAction.Kind + " -> " + humAction.ActionName);
            return true;
        }

        // Unknown HUM actions are intentionally ignored.
        return false;
    }


    private bool ExecuteHumLifeAction(string command)
    {
        if (string.IsNullOrEmpty(command))
            return false;

        string raw = command.Trim();
        string directAction = raw;

        // Direct plugin Action path, e.g. HUM_LIFE("HLA_Expression_Like").
        if (directAction.StartsWith("HLA_", StringComparison.OrdinalIgnoreCase))
        {
            if (TryExecutePluginAction(directAction, "HumanLifeAction", true))
            {
                SuperController.LogMessage("[HUM] LIFE action -> " + directAction);
                return true;
            }
        }

        string actionName = ResolveHumLifeActionName(raw);
        if (string.IsNullOrEmpty(actionName))
        {
            SuperController.LogMessage("[HUM] LIFE unsupported: " + raw);
            return false;
        }

        if (TryExecutePluginAction(actionName, "HumanLifeAction", true))
        {
            SuperController.LogMessage("[HUM] LIFE -> " + raw + " / action=" + actionName);
            return true;
        }

        SuperController.LogMessage("[HUM] LIFE action not found: " + actionName + " / raw=" + raw);
        return false;
    }

    private string ResolveHumLifeActionName(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return "";

        string s = raw.Trim();
        if ((s.StartsWith("\"", StringComparison.Ordinal) && s.EndsWith("\"", StringComparison.Ordinal)) ||
            (s.StartsWith("'", StringComparison.Ordinal) && s.EndsWith("'", StringComparison.Ordinal)))
        {
            s = s.Substring(1, s.Length - 2).Trim();
        }

        string lower = s.ToLowerInvariant().Trim();
        lower = lower.Replace("life_", "life ");
        lower = lower.Replace("life-", "life ");
        lower = lower.Replace("set_", "set ");
        lower = lower.Replace("set-", "set ");
        lower = lower.Replace("=", ":");
        lower = lower.Replace("：", ":");

        string category = "";
        string value = "";

        int colon = lower.IndexOf(':');
        if (colon >= 0)
        {
            category = lower.Substring(0, colon).Trim();
            value = lower.Substring(colon + 1).Trim();
        }
        else
        {
            string compact = lower;
            while (compact.StartsWith("set ", StringComparison.Ordinal)) compact = compact.Substring(4).Trim();
            while (compact.StartsWith("life ", StringComparison.Ordinal)) compact = compact.Substring(5).Trim();

            string[] parts = compact.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                category = parts[0];
                value = parts[1];
            }
            else if (parts.Length == 1)
            {
                value = parts[0];
            }
        }

        category = NormalizeHumLifeWord(category);
        value = NormalizeHumLifeWord(value);

        if (category == "state")
            return ResolveHumLifeStateAction(value);

        if (category == "expression" || category == "affection" || category == "mood" || category == "face")
            return ResolveHumLifeExpressionAction(value);

        if (category == "personality" || category == "character")
            return ResolveHumLifePersonalityAction(value);

        // Short form fallback. This is intentionally conservative:
        // obvious movement states go to Life State, expression words go to Life Expression.
        string stateAction = ResolveHumLifeStateAction(value);
        if (!string.IsNullOrEmpty(stateAction)) return stateAction;

        string expressionAction = ResolveHumLifeExpressionAction(value);
        if (!string.IsNullOrEmpty(expressionAction)) return expressionAction;

        string personalityAction = ResolveHumLifePersonalityAction(value);
        if (!string.IsNullOrEmpty(personalityAction)) return personalityAction;

        return "";
    }

    private string NormalizeHumLifeWord(string word)
    {
        if (string.IsNullOrEmpty(word)) return "";
        string w = word.Trim().ToLowerInvariant();
        w = w.Replace("\"", "").Replace("'", "");
        w = w.Replace("_", " ").Replace("-", " ");
        w = w.Replace("　", " ");
        while (w.IndexOf("  ", StringComparison.Ordinal) >= 0)
            w = w.Replace("  ", " ");
        return w.Trim();
    }

    private string ResolveHumLifeStateAction(string value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        if (value == "sleep" || value == "sleeping" || value == "寝ている" || value == "寝る") return "HLA_State_Sleeping";
        if (value == "quiet" || value == "calm" || value == "おとなしい" || value == "大人しい") return "HLA_State_Quiet";
        if (value == "normal" || value == "default" || value == "普通" || value == "ふつう") return "HLA_State_Normal";
        if (value == "active" || value == "lively" || value == "energetic" || value == "活発") return "HLA_State_Active";
        return "";
    }

    private string ResolveHumLifeExpressionAction(string value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        if (value == "neutral" || value == "normal" || value == "default" || value == "ふつう" || value == "普通") return "HLA_Expression_Neutral";
        if (value == "like" || value == "love" || value == "happy" || value == "smile" || value == "好き") return "HLA_Expression_Like";
        if (value == "dislike" || value == "hate" || value == "angry" || value == "frown" || value == "嫌い") return "HLA_Expression_Dislike";
        if (value == "sad" || value == "shy" || value == "embarrassed" || value == "embarrass" || value == "しょんぼり" || value == "悲しい" || value == "恥ずかしい") return "HLA_Expression_Sad";
        return "";
    }

    private string ResolveHumLifePersonalityAction(string value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        if (value == "normal" || value == "standard" || value == "default" || value == "標準") return "HLA_Personality_Normal";
        if (value == "bold" || value == "positive" || value == "aggressive" || value == "積極的") return "HLA_Personality_Bold";
        return "";
    }

    private bool TryExecutePluginAction(string actionName, string preferredPlugin, bool preferredOnly)
    {
        if (containingAtom == null || string.IsNullOrEmpty(actionName))
            return false;

        foreach (string storableId in containingAtom.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId))
                continue;

            if (!string.IsNullOrEmpty(preferredPlugin) &&
                storableId.IndexOf(preferredPlugin, StringComparison.OrdinalIgnoreCase) < 0)
            {
                continue;
            }

            JSONStorable storable = containingAtom.GetStorableByID(storableId);
            if (storable == null)
                continue;

            JSONStorableAction action = storable.GetAction(actionName);
            if (action == null)
                continue;

            action.actionCallback.Invoke();
            return true;
        }

        if (preferredOnly)
            return false;

        return false;
    }

    private bool TrySetPluginString(string paramName, string value, string preferredPlugin, bool preferredOnly)
    {
        if (containingAtom == null || string.IsNullOrEmpty(paramName))
            return false;

        foreach (string storableId in containingAtom.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId))
                continue;

            if (!string.IsNullOrEmpty(preferredPlugin) &&
                storableId.IndexOf(preferredPlugin, StringComparison.OrdinalIgnoreCase) < 0)
            {
                continue;
            }

            JSONStorable storable = containingAtom.GetStorableByID(storableId);
            if (storable == null)
                continue;

            JSONStorableString stringParam = storable.GetStringJSONParam(paramName);
            if (stringParam == null)
                continue;

            stringParam.val = value;
            return true;
        }

        if (preferredOnly)
            return false;

        return false;
    }

    private void SetBubbleText(string atomUid, string storableId, string targetName, string text)
    {
        if (_bubbleTextTarget != null)
        {
            if (_lastBubbleText == text)
                return;

            _bubbleTextTarget.val = text;
            _lastBubbleText = text;
            return;
        }

        Atom atom = SuperController.singleton.GetAtomByUid(atomUid);
        if (atom == null)
        {
            SuperController.LogMessage("!! [BUBBLE] atom not found: " + atomUid);
            return;
        }

        JSONStorable storable = atom.GetStorableByID(storableId);
        if (storable == null)
        {
            SuperController.LogMessage("!! [BUBBLE] storable not found: " + storableId);
            return;
        }

        JSONStorableString str = storable.GetStringJSONParam(targetName);
        if (str == null)
        {
            SuperController.LogMessage("!! [BUBBLE] string target not found: " + targetName);
            return;
        }

        _bubbleTextTarget = str;
        if (_lastBubbleText == text)
            return;

        str.val = text;
        _lastBubbleText = text;
//        SuperController.LogMessage("[BUBBLE] text set: " + text);
//        SuperController.LogMessage("[BUBBLE] text set: ");
    }

    private bool IsPoseCommand(string cmd)
    {
        if (string.IsNullOrEmpty(cmd)) return false;

        return
            cmd.StartsWith("💽POSE|", StringComparison.OrdinalIgnoreCase) ||
            cmd.StartsWith("POSE|", StringComparison.OrdinalIgnoreCase);
    }

    private string StripPoseCommandPrefix(string cmd)
    {
        if (cmd.StartsWith("💽POSE|", StringComparison.OrdinalIgnoreCase))
            return cmd.Substring("💽POSE|".Length);

        if (cmd.StartsWith("POSE|", StringComparison.OrdinalIgnoreCase))
            return cmd.Substring("POSE|".Length);

        return cmd;
    }

    private bool TryParsePoseEnvelope(string cmd, out PoseEnvelope env)
    {
        env = new PoseEnvelope();

        if (string.IsNullOrEmpty(cmd) || !IsPoseCommand(cmd))
            return false;

        string content = StripPoseCommandPrefix(cmd);
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
    bool isLoop;

    if (TryParseDurationToken(token, out parsedDuration, out isLoop))
    {
        env.Duration = Mathf.Max(0.01f, parsedDuration);
        if (isLoop) env.IsLoop = true;
    }
    continue;
}

            if (!poseSectionStarted && token.StartsWith("TRACK,", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            HumAction humAction;
            if (TryParseHumCommand(token, out humAction))
            {
                env.HumTriggers.Add(humAction);
                continue;
            }

            if (!poseSectionStarted && token.StartsWith("BT_", StringComparison.OrdinalIgnoreCase))
            {
                env.PrePoseTriggers.Add(token);
                if (token.StartsWith("BT_HEAD_", StringComparison.OrdinalIgnoreCase))
                    env.HasHeadTrigger = true;
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

        return env.BoneEntries.Count > 0 || env.PrePoseTriggers.Count > 0 || env.HumTriggers.Count > 0;
    }

private bool TryParseDurationToken(string token, out float duration, out bool isLoop)
{
    duration = 1.0f;
    isLoop = false;

    if (string.IsNullOrEmpty(token)) return false;

    string[] tmParts = token.Split(',');
    if (tmParts.Length < 2) return false;

    float parsed;
    if (!float.TryParse(tmParts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out parsed))
        return false;

    duration = parsed;

    if (tmParts.Length >= 3)
    {
        string flag = tmParts[2].Trim().ToUpper();
        if (flag == "L")
            isLoop = true;
    }

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

        float px, py, pz, rx, ry, rz, rw;
        if (
            !float.TryParse(d[1], NumberStyles.Float, CultureInfo.InvariantCulture, out px) ||
            !float.TryParse(d[2], NumberStyles.Float, CultureInfo.InvariantCulture, out py) ||
            !float.TryParse(d[3], NumberStyles.Float, CultureInfo.InvariantCulture, out pz) ||
            !float.TryParse(d[4], NumberStyles.Float, CultureInfo.InvariantCulture, out rx) ||
            !float.TryParse(d[5], NumberStyles.Float, CultureInfo.InvariantCulture, out ry) ||
            !float.TryParse(d[6], NumberStyles.Float, CultureInfo.InvariantCulture, out rz) ||
            !float.TryParse(d[7], NumberStyles.Float, CultureInfo.InvariantCulture, out rw)
        )
        {
            return false;
        }

        localPos = new Vector3(px, py, pz);
        localRot = new Quaternion(rx, ry, rz, rw);
        return true;
    }

    private bool ShouldApplyRotation(string boneId)
    {
        if (boneId == "headControl")
        {
            return _applyHeadRotation == null || _applyHeadRotation.val;
        }

        if (boneId == "rHandControl" || boneId == "lHandControl")
        {
            return _applyHandRotation == null || _applyHandRotation.val;
        }

        return true;
    }

    private void CommitPoseEnvelope(PoseEnvelope env)
    {
        if (containingAtom == null || containingAtom.mainController == null) return;

        Transform rootT = containingAtom.mainController.transform;
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

            bool applyRotation = ShouldApplyRotation(boneId);
            if (boneId == "headControl" && env.HasHeadTrigger)
                applyRotation = false;

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
            state.ApplyRotation = applyRotation;
        }

//        SuperController.LogMessage(
//            "[POSE APPLY] duration=" + env.Duration.ToString("F3", CultureInfo.InvariantCulture) +
//            " / HeadRot=" + ((_applyHeadRotation != null && _applyHeadRotation.val) ? "ON" : "OFF") +
//            " / HandRot=" + ((_applyHandRotation != null && _applyHandRotation.val) ? "ON" : "OFF")
//        );
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

    private GameObject CreateUIElement(string name, Transform parent)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localPosition = Vector3.zero;
        go.AddComponent<RectTransform>();
        return go;
    }

    private void BuildEditableMultilinePoseInput(GameObject host, JSONStorableString st)
    {
        if (host == null || st == null) return;

        try
        {
            for (int i = 0; i < host.transform.childCount; i++)
                host.transform.GetChild(i).gameObject.SetActive(false);
        }
        catch { }

        try
        {
            HorizontalLayoutGroup h = host.GetComponent<HorizontalLayoutGroup>();
            if (h != null) h.enabled = false;

            VerticalLayoutGroup v = host.GetComponent<VerticalLayoutGroup>();
            if (v != null) v.enabled = false;

            ContentSizeFitter f = host.GetComponent<ContentSizeFitter>();
            if (f != null) f.enabled = false;

            InputField oldInput = host.GetComponent<InputField>();
            if (oldInput != null)
            {
                oldInput.interactable = false;
                oldInput.enabled = false;
            }

            Selectable oldSelectable = host.GetComponent<Selectable>();
            if (oldSelectable != null)
                oldSelectable.interactable = false;
        }
        catch { }

        GameObject root = CreateUIElement("PoseMultilineInput", host.transform);
        RectTransform rt = root.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.offsetMin = new Vector2(8f, 6f);
        rt.offsetMax = new Vector2(-8f, -6f);

        Image img = root.AddComponent<Image>();
        img.color = new Color(0.92f, 0.92f, 0.92f, 1f);

        _poseDataInputField = root.AddComponent<InputField>();
        _poseDataInputField.lineType = InputField.LineType.MultiLineNewline;
        _poseDataInputField.characterLimit = 0;
        _poseDataInputField.targetGraphic = img;
        _poseDataInputField.customCaretColor = true;
        _poseDataInputField.caretColor = Color.black;
        _poseDataInputField.selectionColor = new Color(0.2f, 0.5f, 1f, 0.35f);
        _poseDataInputField.caretBlinkRate = 0.85f;

        Font font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        GameObject textGo = CreateUIElement("Text", root.transform);
        RectTransform textRt = textGo.GetComponent<RectTransform>();
        textRt.anchorMin = new Vector2(0f, 0f);
        textRt.anchorMax = new Vector2(1f, 1f);
        textRt.offsetMin = new Vector2(10f, 8f);
        textRt.offsetMax = new Vector2(-10f, -8f);

        Text txt = textGo.AddComponent<Text>();
        txt.font = font;
        txt.fontSize = 18;
        txt.color = Color.black;
        txt.alignment = TextAnchor.UpperLeft;
        txt.horizontalOverflow = HorizontalWrapMode.Wrap;
        txt.verticalOverflow = VerticalWrapMode.Overflow;
        txt.supportRichText = false;
        _poseDataInputField.textComponent = txt;

        GameObject phGo = CreateUIElement("Placeholder", root.transform);
        RectTransform phRt = phGo.GetComponent<RectTransform>();
        phRt.anchorMin = new Vector2(0f, 0f);
        phRt.anchorMax = new Vector2(1f, 1f);
        phRt.offsetMin = new Vector2(10f, 8f);
        phRt.offsetMax = new Vector2(-10f, -8f);

        Text ph = phGo.AddComponent<Text>();
        ph.font = font;
        ph.fontSize = 18;
        ph.color = new Color(0f, 0f, 0f, 0.35f);
        ph.alignment = TextAnchor.UpperLeft;
        ph.horizontalOverflow = HorizontalWrapMode.Wrap;
        ph.verticalOverflow = VerticalWrapMode.Overflow;
        ph.supportRichText = false;
        ph.text = "Paste POSE text here, then Apply Pose Text";
        _poseDataInputField.placeholder = ph;

        bool syncing = false;
        _poseDataInputField.text = st.val ?? "";
        _poseDataInputField.onValueChanged.AddListener(value =>
        {
            if (syncing) return;
            syncing = true;
            st.val = value ?? "";
            syncing = false;
            try { _poseDataInputField.ForceLabelUpdate(); } catch { }
        });

        var oldCb = st.setCallbackFunction;
        st.setCallbackFunction = value =>
        {
            if (oldCb != null) oldCb(value);
            if (_poseDataInputField == null) return;
            if (syncing) return;

            syncing = true;
            string next = value ?? "";
            if (_poseDataInputField.text != next)
                _poseDataInputField.text = next;
            syncing = false;
            try { _poseDataInputField.ForceLabelUpdate(); } catch { }
        };
    }

    private void OnSendCommandToPython()
    {
        string msg = _pySendInput != null ? _pySendInput.val : "";
        if (string.IsNullOrEmpty(msg))
        {
            SuperController.LogMessage("!! [SEND COMMAND] empty");
            return;
        }

        SendUDP(msg);

        if (_lastPySend != null)
            _lastPySend.val = msg;

        SuperController.LogMessage("📤 [SENT COMMAND] " + msg);
    }

    private void OnSendCurrentPoseButton()
    {
        string msg = "💽POSE|TM,1.00,0|" + ExportPoseToText() + "|#";

        if (_poseDataStorage != null)
            _poseDataStorage.val = msg;

        SendUDP(msg);
        SuperController.LogMessage("📤 [SENT CURRENT POSE]");
    }

    private void OnCapturePoseTextButton()
    {
        string msg = BuildCurrentPoseCommand(1.0f);

        if (_poseDataStorage != null)
            _poseDataStorage.val = msg;

        SuperController.LogMessage("[LOCAL POSE] captured current pose text");
    }

    private void OnApplyPoseTextButton()
    {
        string msg = _poseDataStorage != null ? _poseDataStorage.val : "";
        msg = NormalizePoseTextForLocalApply(msg);

        if (string.IsNullOrEmpty(msg))
        {
            SuperController.LogMessage("!! [LOCAL POSE] pose text is empty");
            return;
        }

        if (_poseDataStorage != null)
            _poseDataStorage.val = msg;

        if (_localPoseApplyRoutine != null)
        {
            StopCoroutine(_localPoseApplyRoutine);
            _localPoseApplyRoutine = null;
        }

        _localPoseApplyRoutine = StartCoroutine(ApplyPoseTextLines(msg));
        SuperController.LogMessage("[LOCAL POSE] applying pose text");
    }

    private IEnumerator ApplyPoseTextLines(string msg)
    {
        string[] lines = msg.Split(
            new char[] { '\r', '\n' },
            StringSplitOptions.RemoveEmptyEntries
        );

        int count = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = NormalizePoseTextForLocalApply(lines[i]);
            if (string.IsNullOrEmpty(line))
                continue;

            HandleIncomingText(line);
            count++;

            float wait = GetPoseCommandDuration(line);
            yield return new WaitForSeconds(Mathf.Max(0.03f, wait));
        }

        SuperController.LogMessage("[LOCAL POSE] applied lines=" + count);
        _localPoseApplyRoutine = null;
    }

    private float GetPoseCommandDuration(string line)
    {
        if (string.IsNullOrEmpty(line) || !IsPoseCommand(line))
            return 0.05f;

        string content = StripPoseCommandPrefix(line);
        string[] parts = content.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < parts.Length; i++)
        {
            string token = parts[i].Trim();
            if (!token.StartsWith("TM,", StringComparison.OrdinalIgnoreCase))
                continue;

            float duration;
            bool isLoop;
            if (TryParseDurationToken(token, out duration, out isLoop))
                return Mathf.Max(0.03f, duration);
        }

        return 0.05f;
    }

    private string BuildCurrentPoseCommand(float duration)
    {
        return "💽POSE|TM," + duration.ToString("F2", CultureInfo.InvariantCulture) + ",0|" + ExportPoseToText() + "|#";
    }

    private string NormalizePoseTextForLocalApply(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        string s = text.Trim();
        if (string.IsNullOrEmpty(s))
            return "";

        if (s.StartsWith("💽POSE|", StringComparison.OrdinalIgnoreCase) ||
            s.StartsWith("POSE|", StringComparison.OrdinalIgnoreCase) ||
            s.StartsWith("VOICE|", StringComparison.OrdinalIgnoreCase) ||
            s.StartsWith("🔊VOICE|", StringComparison.OrdinalIgnoreCase) ||
            s.StartsWith("BT_", StringComparison.OrdinalIgnoreCase) ||
            s.StartsWith("VAM_SEND|", StringComparison.OrdinalIgnoreCase))
        {
            return s;
        }

        if (s.Contains("Control,"))
            return "💽POSE|TM,1.00,0|" + s.Trim('|') + "|#";

        return s;
    }

    private void OnSendPoseButton()
    {
        if (_recordPoseRoutine != null)
        {
            StopCoroutine(_recordPoseRoutine);
            _recordPoseRoutine = null;
        }

        _recordPoseRoutine = StartCoroutine(RecordPoseAndSend());
    }

    private IEnumerator RecordPoseAndSend()
    {
        float duration = _recordDuration != null
            ? Mathf.Max(0.1f, _recordDuration.val)
            : 3.0f;

        float interval = _recordInterval != null
            ? Mathf.Max(0.03f, _recordInterval.val)
            : 0.2f;

        StringBuilder sb = new StringBuilder();

        float start = Time.time;
        int frameCount = 0;

        while (Time.time - start <= duration)
        {
            string pose = ExportPoseToText();

            sb.AppendFormat(
                CultureInfo.InvariantCulture,
                "💽POSE|TM,{0:F2},0|{1}|#\n",
                interval,
                pose
            );

            frameCount++;

            yield return new WaitForSeconds(interval);
        }

string msg = sb.ToString();

string[] lines = msg.Split(
    new char[] { '\r', '\n' },
    StringSplitOptions.RemoveEmptyEntries
);

// Send first so large text-field updates do not delay Python.
for (int i = 0; i < lines.Length; i++)
{
    SendUDP(lines[i]);
    yield return new WaitForSeconds(0.05f);
}

if (_poseDataStorage != null)
    _poseDataStorage.val = msg;

SuperController.LogMessage(
    "📤 [REC POSE SEND] frames=" + frameCount +
    " dur=" + duration.ToString("F2", CultureInfo.InvariantCulture) +
    " int=" + interval.ToString("F2", CultureInfo.InvariantCulture)
);

_recordPoseRoutine = null;
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
