using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using SimpleJSON;

public class BodyTouchTriggerProbe : MVRScript
{
    JSONStorableStringChooser targetAtomChooser;
    JSONStorableStringChooser triggerChooser;
    JSONStorableStringChooser[] ptActionChoosers;
    JSONStorableBool enabledToggle;
    JSONStorableBool autoPoll;
    JSONStorableBool logOnlyOnChange;
    JSONStorableBool showContactLog;
    JSONStorableBool showScreenLed;
    JSONStorableFloat staleSeconds;
    JSONStorableString statusText;

    JSONStorableFloat[] hitValues;
    JSONStorableBool[] hitBools;
    bool[] hitStates;
    bool[] lastHitStates;
    float[] lastContactTimes;
    int[] enterCounts;
    int[] stayCounts;
    int[] exitCounts;
    List<int>[] activeOtherIds;
    Coroutine[] ptActionRoutines;

    GameObject[] screenLedObjs;
    GameObject screenRightNippleLedObj;
    LineRenderer[] screenLedLines;
    LineRenderer screenRightNippleLine;
    Material ledOffMaterial;
    Material ledOnMaterial;
    Material ledMixedMaterial;

    string lastReport = "";
    float lastPollTime = -999f;
    float initTime = -999f;
    float nextCameraLookupTime = -999f;
    bool autoInstallDone;
    bool screenOverlayActive = true;
    Camera screenCamera;

    const float ScreenLedDistance = 1.25f;
    const float ScreenLedX = 0.855f;
    const float ScreenLedWidth = 0.022f;
    const float ScreenLedHeight = 0.0085f;
    const float NippleLedWidth = 0.010f;
    const float NippleLedOffsetX = 0.007f;
    const float AutoInstallDelay = 1.25f;

    readonly string[] builtinTriggerIds = new string[]
    {
        "LipTrigger",
        "MouthTrigger",
        "ThroatTrigger",
        "lNippleTrigger",
        "rNippleTrigger",
        "LabiaTrigger",
        "VaginaTrigger",
        "DeepVaginaTrigger",
        "DeeperVaginaTrigger"
    };

    readonly string[] screenRows = new string[]
    {
        "LIP",
        "MOUTH",
        "THROAT",
        "NIPPLE",
        "DEEPER VAGINA",
        "DEEP VAGINA",
        "VAGINA",
        "LABIA"
    };

    readonly float[] screenRowY = new float[]
    {
        0.720f,
        0.675f,
        0.630f,
        0.585f,
        0.505f,
        0.460f,
        0.415f,
        0.370f
    };

    public override void Init()
    {
        try
        {
            List<string> atomChoices = GetAtomChoices();
            string initialAtom = GetInitialAtomUid(atomChoices);

            targetAtomChooser = new JSONStorableStringChooser("Target Atom", atomChoices, initialAtom, "Target Atom");
            RegisterStringChooser(targetAtomChooser);
            CreatePopup(targetAtomChooser);

            triggerChooser = new JSONStorableStringChooser("Trigger", GetTriggerChoices(), "All Builtin", "Trigger");
            RegisterStringChooser(triggerChooser);
            CreatePopup(triggerChooser);

            enabledToggle = new JSONStorableBool("Enable", true);
            RegisterBool(enabledToggle);
            CreateToggle(enabledToggle);

            autoPoll = new JSONStorableBool("Auto Poll", false);
            RegisterBool(autoPoll);
            CreateToggle(autoPoll);

            logOnlyOnChange = new JSONStorableBool("Log Only On Change", true);
            RegisterBool(logOnlyOnChange);
            CreateToggle(logOnlyOnChange);

            showContactLog = new JSONStorableBool("Show Contact Log", false);
            RegisterBool(showContactLog);
            CreateToggle(showContactLog);

            showScreenLed = new JSONStorableBool("Show Screen LED", true);
            RegisterBool(showScreenLed);
            CreateToggle(showScreenLed);

            staleSeconds = new JSONStorableFloat("Stale Off Seconds", 0.35f, 0.05f, 2.0f, true);
            RegisterFloat(staleSeconds);
            CreateSlider(staleSeconds);

            statusText = new JSONStorableString("Status", "not checked");
            RegisterString(statusText);
            UIDynamicTextField status = CreateTextField(statusText);
            if (status != null)
            {
                status.height = 160f;
            }

            CreateReceivers();
            CreatePTActionSelectors();

            CreateButton("Refresh Atom List").button.onClick.AddListener(RefreshAtomList);
            CreateButton("Refresh PT List").button.onClick.AddListener(RefreshPTList);
            CreateButton("Install Collider Hooks").button.onClick.AddListener(InstallColliderHooksButton);
            CreateButton("Check Selected").button.onClick.AddListener(CheckSelectedNow);
            CreateButton("Check All Builtin").button.onClick.AddListener(CheckAllBuiltinNow);
            CreateButton("Reset Hit States").button.onClick.AddListener(ResetHitStates);
            CreateButton("Dump Trigger Colliders").button.onClick.AddListener(DumpTriggerColliders);

            CreateScreenOverlay();
            UpdateScreenOverlay(true);
            initTime = Time.time;
            autoInstallDone = false;

            SuperController.LogMessage("[BodyTouchTriggerProbe] Ready / v007 PT trigger actions / rect LED lateupdate small / collider hook / no reflection");
            SuperController.LogMessage("[BodyTouchTriggerProbe] Auto install is enabled by default. Select Target Atom and press Install Collider Hooks only if target changed.");
        }
        catch (Exception e)
        {
            SuperController.LogError("[BodyTouchTriggerProbe] Init error: " + e);
        }
    }

    void Update()
    {
        if (!autoInstallDone && Time.time - initTime >= AutoInstallDelay)
        {
            autoInstallDone = true;
            if (enabledToggle == null || enabledToggle.val)
            {
                InstallColliderHooks(false);
            }
        }

        ClearStaleStates();

        if (autoPoll == null || !autoPoll.val)
        {
            return;
        }

        if (Time.time - lastPollTime < 0.20f)
        {
            return;
        }

        lastPollTime = Time.time;
        CheckSelected(false);
    }

    void LateUpdate()
    {
        UpdateScreenOverlay(false);
    }

    void OnDestroy()
    {
        CleanupScreenOverlay();
    }

    void CreateReceivers()
    {
        int n = builtinTriggerIds.Length;
        hitValues = new JSONStorableFloat[n];
        hitBools = new JSONStorableBool[n];
        hitStates = new bool[n];
        lastHitStates = new bool[n];
        lastContactTimes = new float[n];
        enterCounts = new int[n];
        stayCounts = new int[n];
        exitCounts = new int[n];
        activeOtherIds = new List<int>[n];

        for (int i = 0; i < n; i++)
        {
            activeOtherIds[i] = new List<int>();
            lastContactTimes[i] = -999f;

            JSONStorableBool b = new JSONStorableBool("On " + builtinTriggerIds[i], false);
            b.storeType = JSONStorableParam.StoreType.Full;
            RegisterBool(b);
            hitBools[i] = b;

            JSONStorableFloat f = new JSONStorableFloat("Hit " + builtinTriggerIds[i], 0f, 0f, 1f, true);
            f.storeType = JSONStorableParam.StoreType.Full;
            RegisterFloat(f);
            hitValues[i] = f;
        }
    }

    void CreatePTActionSelectors()
    {
        int n = builtinTriggerIds.Length;
        ptActionChoosers = new JSONStorableStringChooser[n];
        ptActionRoutines = new Coroutine[n];
        List<string> choices = GetPTAtomChoices();

        for (int i = 0; i < n; i++)
        {
            JSONStorableStringChooser chooser = new JSONStorableStringChooser(
                "PT " + builtinTriggerIds[i],
                choices,
                "None",
                "PT " + builtinTriggerIds[i]
            );
            RegisterStringChooser(chooser);
            CreatePopup(chooser);
            ptActionChoosers[i] = chooser;
        }
    }

    List<string> GetPTAtomChoices()
    {
        List<string> choices = new List<string>();
        choices.Add("None");

        if (SuperController.singleton == null)
        {
            return choices;
        }

        foreach (Atom atom in SuperController.singleton.GetAtoms())
        {
            if (atom == null || string.IsNullOrEmpty(atom.uid))
            {
                continue;
            }

            if (atom.uid.StartsWith("PT_", StringComparison.OrdinalIgnoreCase))
            {
                choices.Add(atom.uid);
            }
        }

        choices.Sort();
        if (choices.Remove("None"))
        {
            choices.Insert(0, "None");
        }
        return choices;
    }

    void RefreshPTList()
    {
        if (ptActionChoosers == null)
        {
            return;
        }

        List<string> choices = GetPTAtomChoices();
        for (int i = 0; i < ptActionChoosers.Length; i++)
        {
            JSONStorableStringChooser chooser = ptActionChoosers[i];
            if (chooser == null)
            {
                continue;
            }

            string old = chooser.val;
            chooser.choices = choices;
            chooser.val = choices.Contains(old) ? old : "None";
        }

        SuperController.LogMessage("[BodyTouchTriggerProbe] PT list refreshed / count=" + (choices.Count - 1));
    }

    void CreateScreenOverlay()
    {
        CleanupScreenOverlay();

        ledOffMaterial = CreateLedMaterial(new Color(0.01f, 0.10f, 0.025f, 1.00f));
        ledOnMaterial = CreateLedMaterial(new Color(0.08f, 0.72f, 0.16f, 1.00f));
        ledMixedMaterial = CreateLedMaterial(new Color(0.05f, 0.42f, 0.10f, 1.00f));

        screenLedObjs = new GameObject[screenRows.Length];
        screenLedLines = new LineRenderer[screenRows.Length];

        for (int i = 0; i < screenRows.Length; i++)
        {
            GameObject led = new GameObject("BodyTouchTriggerProbe_ScreenLed_" + i);
            led.name = "BodyTouchTriggerProbe_ScreenLed_" + i;
            screenLedObjs[i] = led;
            screenLedLines[i] = CreateLedLine(led, ledOffMaterial);
        }

        screenRightNippleLedObj = new GameObject("BodyTouchTriggerProbe_ScreenLed_NippleR");
        screenRightNippleLedObj.name = "BodyTouchTriggerProbe_ScreenLed_NippleR";
        screenRightNippleLine = CreateLedLine(screenRightNippleLedObj, ledOffMaterial);
    }

    void CleanupScreenOverlay()
    {
        for (int i = 0; i < screenRows.Length; i++)
        {
            DestroyNamedObject("BodyTouchTriggerProbe_ScreenLed_" + i);
            DestroyNamedObject("BodyTouchTriggerProbe_ScreenLabel_" + i);
        }
        DestroyNamedObject("BodyTouchTriggerProbe_ScreenLed_NippleR");
    }

    void DestroyNamedObject(string objectName)
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject go = GameObject.Find(objectName);
            if (go == null)
            {
                break;
            }

            go.name = objectName + "_Destroyed_" + i;
            Destroy(go);
        }
    }

    LineRenderer CreateLedLine(GameObject obj, Material mat)
    {
        if (obj == null)
        {
            return null;
        }

        LineRenderer line = obj.AddComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.loop = false;
        line.positionCount = 2;
        line.numCapVertices = 0;
        line.numCornerVertices = 0;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        if (mat != null)
        {
            line.material = new Material(mat);
            line.startColor = mat.color;
            line.endColor = mat.color;
        }
        line.startWidth = ScreenLedHeight;
        line.endWidth = ScreenLedHeight;
        return line;
    }

    Material CreateLedMaterial(Color color)
    {
        Shader shader = Shader.Find("Unlit/Color");
        if (shader == null)
        {
            shader = Shader.Find("Sprites/Default");
        }
        if (shader == null)
        {
            shader = Shader.Find("Diffuse");
        }
        if (shader == null)
        {
            return null;
        }

        Material mat = new Material(shader);
        mat.color = color;
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.renderQueue = 3000;
        return mat;
    }

    void UpdateScreenOverlay(bool force)
    {
        bool visible = showScreenLed == null || showScreenLed.val;
        visible = visible && (enabledToggle == null || enabledToggle.val);
        if (screenLedLines == null)
        {
            return;
        }

        Camera cam = GetScreenCamera();
        if (cam == null || !visible)
        {
            SetScreenOverlayActive(false);
            return;
        }

        SetScreenOverlayActive(true);

        SetScreenRow(0, "LIP", IsOn("LipTrigger"), false);
        SetScreenRow(1, "MOUTH", IsOn("MouthTrigger"), false);
        SetScreenRow(2, "THROAT", IsOn("ThroatTrigger"), false);
        SetNippleScreenRow(3);
        SetScreenRow(4, "DEEPER VAGINA", IsOn("DeeperVaginaTrigger"), false);
        SetScreenRow(5, "DEEP VAGINA", IsOn("DeepVaginaTrigger"), false);
        SetScreenRow(6, "VAGINA", IsOn("VaginaTrigger"), false);
        SetScreenRow(7, "LABIA", IsOn("LabiaTrigger"), false);

        for (int i = 0; i < screenRows.Length; i++)
        {
            float x = i == 3 ? ScreenLedX - NippleLedOffsetX : ScreenLedX;
            Vector3 ledPos = cam.ViewportToWorldPoint(new Vector3(x, screenRowY[i], ScreenLedDistance));

            if (screenLedLines[i] != null)
            {
                float width = i == 3 ? NippleLedWidth : ScreenLedWidth;
                SetLedLinePosition(screenLedLines[i], ledPos, cam.transform.right, width, ScreenLedHeight);
            }
        }

        if (screenRightNippleLine != null)
        {
            Vector3 rightPos = cam.ViewportToWorldPoint(new Vector3(ScreenLedX + NippleLedOffsetX, screenRowY[3], ScreenLedDistance));
            SetLedLinePosition(screenRightNippleLine, rightPos, cam.transform.right, NippleLedWidth, ScreenLedHeight);
        }
    }

    void SetLedLinePosition(LineRenderer line, Vector3 center, Vector3 right, float length, float width)
    {
        if (line == null)
        {
            return;
        }

        Vector3 half = right.normalized * (length * 0.5f);
        line.startWidth = width;
        line.endWidth = width;
        line.SetPosition(0, center - half);
        line.SetPosition(1, center + half);
    }

    void SetScreenOverlayActive(bool active)
    {
        if (screenOverlayActive == active)
        {
            return;
        }

        screenOverlayActive = active;

        if (screenLedObjs != null)
        {
            for (int i = 0; i < screenLedObjs.Length; i++)
            {
                if (screenLedObjs[i] != null) screenLedObjs[i].SetActive(active);
            }
        }
        if (screenRightNippleLedObj != null)
        {
            screenRightNippleLedObj.SetActive(active);
        }
    }

    Camera GetScreenCamera()
    {
        if (screenCamera != null && Time.time < nextCameraLookupTime)
        {
            return screenCamera;
        }

        nextCameraLookupTime = Time.time + 0.50f;
        screenCamera = Camera.main;
        return screenCamera;
    }

    void SetScreenRow(int row, string label, bool on, bool mixed)
    {
        if (row < 0 || row >= screenRows.Length)
        {
            return;
        }

        if (screenLedLines != null && screenLedLines[row] != null)
        {
            SetLedLineColor(screenLedLines[row], mixed ? LedColor(ledMixedMaterial, new Color(0.05f, 0.42f, 0.10f, 1.00f)) : (on ? LedColor(ledOnMaterial, new Color(0.08f, 0.72f, 0.16f, 1.00f)) : LedColor(ledOffMaterial, new Color(0.01f, 0.10f, 0.025f, 1.00f))));
        }
    }

    void SetNippleScreenRow(int row)
    {
        bool left = IsOn("lNippleTrigger");
        bool right = IsOn("rNippleTrigger");
        if (screenLedLines != null && screenLedLines[row] != null)
        {
            SetLedLineColor(screenLedLines[row], left ? LedColor(ledOnMaterial, new Color(0.08f, 0.72f, 0.16f, 1.00f)) : LedColor(ledOffMaterial, new Color(0.01f, 0.10f, 0.025f, 1.00f)));
        }

        if (screenRightNippleLine != null)
        {
            SetLedLineColor(screenRightNippleLine, right ? LedColor(ledOnMaterial, new Color(0.08f, 0.72f, 0.16f, 1.00f)) : LedColor(ledOffMaterial, new Color(0.01f, 0.10f, 0.025f, 1.00f)));
        }
    }

    Color LedColor(Material mat, Color fallback)
    {
        return mat != null ? mat.color : fallback;
    }

    void SetLedLineColor(LineRenderer line, Color color)
    {
        if (line == null)
        {
            return;
        }

        if (line.startColor == color && line.endColor == color)
        {
            return;
        }

        line.startColor = color;
        line.endColor = color;
        if (line.material != null)
        {
            line.material.color = color;
        }
    }

    bool IsOn(string triggerId)
    {
        int index = FindTriggerIndex(triggerId);
        return IsValidIndex(index) && hitStates[index];
    }

    public void HookContact(int index, Collider other, string phase)
    {
        if (enabledToggle != null && !enabledToggle.val)
        {
            return;
        }

        if (!IsValidIndex(index))
        {
            return;
        }

        int otherId = other != null ? other.GetInstanceID() : 0;
        if (otherId != 0 && !activeOtherIds[index].Contains(otherId))
        {
            activeOtherIds[index].Add(otherId);
        }

        SetHitOn(index);
        lastContactTimes[index] = Time.time;

        if (phase == "enter")
        {
            enterCounts[index]++;
            if (showContactLog == null || showContactLog.val)
            {
                SuperController.LogMessage("[BodyTouchTriggerProbe] CONTACT ENTER / " + builtinTriggerIds[index] + OtherText(other));
            }
        }
        else if (phase == "stay")
        {
            stayCounts[index]++;
        }
    }

    public void HookExit(int index, Collider other)
    {
        if (enabledToggle != null && !enabledToggle.val)
        {
            return;
        }

        if (!IsValidIndex(index))
        {
            return;
        }

        int otherId = other != null ? other.GetInstanceID() : 0;
        if (otherId != 0)
        {
            activeOtherIds[index].Remove(otherId);
        }

        exitCounts[index]++;
        if (showContactLog == null || showContactLog.val)
        {
            SuperController.LogMessage("[BodyTouchTriggerProbe] CONTACT EXIT / " + builtinTriggerIds[index] + OtherText(other) + " / remain=" + activeOtherIds[index].Count);
        }

        if (activeOtherIds[index].Count == 0)
        {
            SetHitOff(index);
        }
    }

    void SetHitOn(int index)
    {
        bool rising = !hitStates[index];
        hitStates[index] = true;
        if (hitBools[index] != null) hitBools[index].val = true;
        if (hitValues[index] != null) hitValues[index].val = 1f;
        if (rising)
        {
            ExecutePTAction(index);
        }
    }

    void SetHitOff(int index)
    {
        hitStates[index] = false;
        if (hitBools[index] != null) hitBools[index].val = false;
        if (hitValues[index] != null) hitValues[index].val = 0f;
    }

    void ExecutePTAction(int index)
    {
        if (!IsValidIndex(index) || ptActionChoosers == null || index >= ptActionChoosers.Length)
        {
            return;
        }

        JSONStorableStringChooser chooser = ptActionChoosers[index];
        if (chooser == null)
        {
            return;
        }

        string atomUid = chooser.val;
        if (string.IsNullOrEmpty(atomUid) || atomUid == "None")
        {
            return;
        }

        JSONNode arr = GetPTStartActions(atomUid);
        if (arr == null || arr.Count == 0)
        {
            SuperController.LogMessage("[BodyTouchTriggerProbe] PT skipped / trigger=" + builtinTriggerIds[index] + " / atom=" + atomUid + " / no startActions");
            return;
        }

        if (ptActionRoutines != null && ptActionRoutines[index] != null)
        {
            StopCoroutine(ptActionRoutines[index]);
            ptActionRoutines[index] = null;
        }

        ptActionRoutines[index] = StartCoroutine(RunPTActionArray(index, atomUid, arr));
    }

    JSONNode GetPTStartActions(string atomUid)
    {
        if (SuperController.singleton == null || string.IsNullOrEmpty(atomUid))
        {
            return null;
        }

        Atom atom = SuperController.singleton.GetAtomByUid(atomUid);
        if (atom == null)
        {
            SuperController.LogMessage("[BodyTouchTriggerProbe] PT atom missing / " + atomUid);
            return null;
        }

        JSONStorable storable = atom.GetStorableByID("Trigger");
        if (storable == null)
        {
            SuperController.LogMessage("[BodyTouchTriggerProbe] PT storable missing / atom=" + atomUid + " / storable=Trigger");
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

        return trigger["startActions"];
    }

    IEnumerator RunPTActionArray(int triggerIndex, string atomUid, JSONNode arr)
    {
        for (int i = 0; i < arr.Count; i++)
        {
            yield return StartCoroutine(RunPTActionNode(atomUid, i, arr[i]));
        }

        if (ptActionRoutines != null && IsValidIndex(triggerIndex))
        {
            ptActionRoutines[triggerIndex] = null;
        }
    }

    IEnumerator RunPTActionNode(string atomUid, int actionIndex, JSONNode a)
    {
        if (a == null || SuperController.singleton == null)
        {
            yield break;
        }

        string name = a["name"];
        string receiverAtomUid = a["receiverAtom"];
        string receiver = a["receiver"];
        string target = a["receiverTargetName"];

        Atom receiverAtom = SuperController.singleton.GetAtomByUid(receiverAtomUid);
        if (receiverAtom == null)
        {
            SuperController.LogMessage("[BodyTouchTriggerProbe] PT receiver atom missing / " + atomUid + "[" + actionIndex + "] / " + receiverAtomUid);
            yield break;
        }

        JSONStorable storable = receiverAtom.GetStorableByID(receiver);
        if (storable == null)
        {
            SuperController.LogMessage("[BodyTouchTriggerProbe] PT receiver storable missing / " + atomUid + "[" + actionIndex + "] / " + receiver);
            yield break;
        }

        if (!string.IsNullOrEmpty(target))
        {
            JSONStorableAction action = storable.GetAction(target);
            if (action != null && action.actionCallback != null)
            {
                action.actionCallback.Invoke();
                yield break;
            }
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
            string floatTarget = target;
            if (!string.IsNullOrEmpty(floatTarget) && floatTarget.StartsWith("morph: "))
            {
                floatTarget = floatTarget.Substring("morph: ".Length);
            }

            JSONStorableFloat floatParam = storable.GetFloatJSONParam(floatTarget);
            if (floatParam != null)
            {
                float v = 0f;
                float.TryParse(a["floatValue"], NumberStyles.Float, CultureInfo.InvariantCulture, out v);

                bool useTimer = false;
                float timerLength = 0f;
                string timerType = a["timerType"];
                bool.TryParse(a["useTimer"], out useTimer);
                float.TryParse(a["timerLength"], NumberStyles.Float, CultureInfo.InvariantCulture, out timerLength);

                if (useTimer && timerLength > 0f)
                {
                    yield return StartCoroutine(RunFloatTimer(floatParam, floatParam.val, v, timerLength, timerType));
                    yield break;
                }

                floatParam.val = v;
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
            {
                actionValue = name.Substring(colon + 1).Trim();
            }
        }

        if (!string.IsNullOrEmpty(actionValue))
        {
            JSONStorableStringChooser chooserParam = storable.GetStringChooserJSONParam(target);
            if (chooserParam != null)
            {
                chooserParam.val = actionValue;
                yield break;
            }

            JSONStorableString strParam = storable.GetStringJSONParam(target);
            if (strParam != null)
            {
                strParam.val = actionValue;
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
                yield break;
            }

            JSONStorableFloat floatParamFromString = storable.GetFloatJSONParam(target);
            if (floatParamFromString != null)
            {
                float v;
                if (float.TryParse(actionValue, NumberStyles.Float, CultureInfo.InvariantCulture, out v))
                {
                    floatParamFromString.val = v;
                    yield break;
                }
            }
        }

        SuperController.LogMessage(
            "[BodyTouchTriggerProbe] PT unsupported / " + atomUid + "[" + actionIndex + "] " +
            name + " / atom=" + receiverAtomUid + " / receiver=" + receiver + " / target=" + target
        );
    }

    IEnumerator RunFloatTimer(JSONStorableFloat param, float startValue, float endValue, float timerLength, string timerType)
    {
        if (param == null)
        {
            yield break;
        }

        timerLength = Mathf.Max(0.01f, timerLength);
        float elapsed = 0f;

        while (elapsed < timerLength)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / timerLength);
            float eased = ApplyTimerType(t, timerType);
            param.val = Mathf.Lerp(startValue, endValue, eased);
            yield return null;
        }

        param.val = endValue;
    }

    float ApplyTimerType(float t, string timerType)
    {
        if (string.IsNullOrEmpty(timerType)) return t;
        if (timerType == "EaseInOut") return t * t * (3f - 2f * t);
        if (timerType == "EaseIn") return t * t;
        if (timerType == "EaseOut") return 1f - (1f - t) * (1f - t);
        return t;
    }

    string OtherText(Collider other)
    {
        if (other == null)
        {
            return " / other=null";
        }

        Rigidbody rb = other.attachedRigidbody;
        string rbName = rb != null ? rb.name : "none";
        return " / other=" + other.name + " / rb=" + rbName;
    }

    void ClearStaleStates()
    {
        float limit = staleSeconds != null ? staleSeconds.val : 0.35f;
        for (int i = 0; i < builtinTriggerIds.Length; i++)
        {
            if (!hitStates[i])
            {
                continue;
            }

            if (Time.time - lastContactTimes[i] > limit)
            {
                activeOtherIds[i].Clear();
                SetHitOff(i);
            }
        }
    }

    List<string> GetTriggerChoices()
    {
        List<string> choices = new List<string>();
        choices.Add("All Builtin");
        for (int i = 0; i < builtinTriggerIds.Length; i++)
        {
            choices.Add(builtinTriggerIds[i]);
        }
        return choices;
    }

    string GetInitialAtomUid(List<string> choices)
    {
        if (choices == null || choices.Count == 0)
        {
            return "";
        }

        if (containingAtom != null && !string.IsNullOrEmpty(containingAtom.uid) && choices.Contains(containingAtom.uid))
        {
            return containingAtom.uid;
        }

        for (int i = 0; i < choices.Count; i++)
        {
            if (!string.IsNullOrEmpty(choices[i]) && choices[i].IndexOf("person", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return choices[i];
            }
        }

        return choices[0];
    }

    void RefreshAtomList()
    {
        if (targetAtomChooser == null)
        {
            return;
        }

        string old = targetAtomChooser.val;
        List<string> choices = GetAtomChoices();
        targetAtomChooser.choices = choices;

        if (!string.IsNullOrEmpty(old) && choices.Contains(old))
        {
            targetAtomChooser.val = old;
        }
        else
        {
            targetAtomChooser.val = GetInitialAtomUid(choices);
        }

        SuperController.LogMessage("[BodyTouchTriggerProbe] Atom list refreshed / count=" + choices.Count + " / selected=" + targetAtomChooser.val);
    }

    List<string> GetAtomChoices()
    {
        List<string> choices = new List<string>();
        if (SuperController.singleton == null)
        {
            return choices;
        }

        foreach (Atom atom in SuperController.singleton.GetAtoms())
        {
            if (atom == null || string.IsNullOrEmpty(atom.uid))
            {
                continue;
            }
            choices.Add(atom.uid);
        }

        choices.Sort();
        return choices;
    }

    Atom GetTargetAtom()
    {
        if (SuperController.singleton == null || targetAtomChooser == null)
        {
            return null;
        }

        string uid = targetAtomChooser.val;
        if (string.IsNullOrEmpty(uid))
        {
            return null;
        }

        return SuperController.singleton.GetAtomByUid(uid);
    }

    void InstallColliderHooksButton()
    {
        InstallColliderHooks(true);
    }

    void InstallColliderHooks(bool verbose)
    {
        Atom atom = GetTargetAtom();
        if (atom == null)
        {
            SuperController.LogMessage("[BodyTouchTriggerProbe] Install skipped / target atom missing");
            return;
        }

        int triggerCount = 0;
        int colliderCount = 0;

        for (int i = 0; i < builtinTriggerIds.Length; i++)
        {
            CollisionTrigger trig = atom.GetStorableByID(builtinTriggerIds[i]) as CollisionTrigger;
            if (trig == null)
            {
                if (verbose)
                {
                    SuperController.LogMessage("[BodyTouchTriggerProbe] hook skipped / trigger missing / " + builtinTriggerIds[i]);
                }
                continue;
            }

            triggerCount++;
            Collider[] colliders = trig.GetComponentsInChildren<Collider>(true);
            if (colliders == null || colliders.Length == 0)
            {
                if (verbose)
                {
                    SuperController.LogMessage("[BodyTouchTriggerProbe] hook warning / no colliders / " + builtinTriggerIds[i]);
                }
                continue;
            }

            for (int c = 0; c < colliders.Length; c++)
            {
                Collider col = colliders[c];
                if (col == null || col.gameObject == null)
                {
                    continue;
                }

                BodyTouchTriggerProbeColliderHook hook = col.gameObject.GetComponent<BodyTouchTriggerProbeColliderHook>();
                if (hook == null)
                {
                    hook = col.gameObject.AddComponent<BodyTouchTriggerProbeColliderHook>();
                }

                hook.owner = this;
                hook.triggerIndex = i;
                hook.triggerId = builtinTriggerIds[i];
                colliderCount++;

                if (verbose)
                {
                    SuperController.LogMessage(
                        "[BodyTouchTriggerProbe] hook installed / trigger=" + builtinTriggerIds[i] +
                        " / collider=" + col.name +
                        " / isTrigger=" + col.isTrigger +
                        " / enabled=" + col.enabled
                    );
                }
            }
        }

        SuperController.LogMessage("[BodyTouchTriggerProbe] Install Collider Hooks done / triggers=" + triggerCount + " / colliders=" + colliderCount);
        UpdateScreenOverlay(true);
    }

    void CheckSelectedNow()
    {
        CheckSelected(true);
    }

    void CheckSelected(bool forceLog)
    {
        Atom atom = GetTargetAtom();
        string atomUid = atom != null ? atom.uid : "MISSING";
        string selected = triggerChooser != null ? triggerChooser.val : "All Builtin";

        string report;
        if (selected == "All Builtin")
        {
            report = BuildAllReport(atom, atomUid);
        }
        else
        {
            int index = FindTriggerIndex(selected);
            report = "selected / atom=" + atomUid + " / " + BuildOneState(atom, selected, index);
        }

        SetStatus(report);

        bool changed = HasHitStateChanged();
        if (logOnlyOnChange == null || !logOnlyOnChange.val)
        {
            LogReport(report, forceLog);
        }
        else
        {
            LogReport(report, forceLog || changed);
        }

        SaveLastHitStates();
    }

    void CheckAllBuiltinNow()
    {
        Atom atom = GetTargetAtom();
        string atomUid = atom != null ? atom.uid : "MISSING";
        string report = BuildAllReport(atom, atomUid);
        SetStatus(report);
        LogReport(report, true);
        SaveLastHitStates();
        UpdateScreenOverlay(true);
    }

    string BuildAllReport(Atom atom, string atomUid)
    {
        string report = "AllBuiltin / atom=" + atomUid;

        for (int i = 0; i < builtinTriggerIds.Length; i++)
        {
            report += " / " + BuildOneState(atom, builtinTriggerIds[i], i);
        }

        return report;
    }

    string BuildOneState(Atom atom, string triggerId, int index)
    {
        string existsText = GetTriggerExistsText(atom, triggerId);
        bool on = IsValidIndex(index) && hitStates[index];
        float hit = GetHitValue(index);
        int active = IsValidIndex(index) ? activeOtherIds[index].Count : 0;
        int ec = IsValidIndex(index) ? enterCounts[index] : 0;
        int sc = IsValidIndex(index) ? stayCounts[index] : 0;
        int xc = IsValidIndex(index) ? exitCounts[index] : 0;
        return triggerId + "(" + existsText + ",state=" + (on ? "ON" : "OFF") + ",hit=" + hit.ToString("0.0") + ",active=" + active + ",in=" + ec + ",stay=" + sc + ",out=" + xc + ")";
    }

    string GetTriggerExistsText(Atom atom, string triggerId)
    {
        if (atom == null)
        {
            return "atom-missing";
        }

        JSONStorable storable = atom.GetStorableByID(triggerId);
        if (storable == null)
        {
            return "missing";
        }

        CollisionTrigger trig = storable as CollisionTrigger;
        if (trig == null)
        {
            return "not-collision";
        }

        return trig.enabled ? "enabled" : "disabled";
    }

    float GetHitValue(int index)
    {
        if (hitValues == null || !IsValidIndex(index) || hitValues[index] == null)
        {
            return 0f;
        }
        return hitValues[index].val;
    }

    int FindTriggerIndex(string triggerId)
    {
        for (int i = 0; i < builtinTriggerIds.Length; i++)
        {
            if (builtinTriggerIds[i] == triggerId)
            {
                return i;
            }
        }
        return -1;
    }

    bool IsValidIndex(int index)
    {
        return index >= 0 && index < builtinTriggerIds.Length;
    }

    bool HasHitStateChanged()
    {
        if (hitStates == null || lastHitStates == null)
        {
            return false;
        }

        for (int i = 0; i < hitStates.Length; i++)
        {
            if (hitStates[i] != lastHitStates[i])
            {
                return true;
            }
        }

        return false;
    }

    void SaveLastHitStates()
    {
        if (hitStates == null || lastHitStates == null)
        {
            return;
        }

        for (int i = 0; i < hitStates.Length; i++)
        {
            lastHitStates[i] = hitStates[i];
        }
    }

    void ResetHitStates()
    {
        for (int i = 0; i < builtinTriggerIds.Length; i++)
        {
            SetHitOff(i);
            activeOtherIds[i].Clear();
            enterCounts[i] = 0;
            stayCounts[i] = 0;
            exitCounts[i] = 0;
            lastContactTimes[i] = -999f;
        }

        SaveLastHitStates();
        SuperController.LogMessage("[BodyTouchTriggerProbe] Reset Hit States");
        CheckAllBuiltinNow();
        UpdateScreenOverlay(true);
    }

    void DumpTriggerColliders()
    {
        Atom atom = GetTargetAtom();
        string atomUid = atom != null ? atom.uid : "MISSING";

        SuperController.LogMessage("[BodyTouchTriggerProbe] Dump Trigger Colliders / atom=" + atomUid);

        for (int i = 0; i < builtinTriggerIds.Length; i++)
        {
            string id = builtinTriggerIds[i];
            CollisionTrigger trig = atom != null ? atom.GetStorableByID(id) as CollisionTrigger : null;
            if (trig == null)
            {
                SuperController.LogMessage("[BodyTouchTriggerProbe] trigger=" + id + " / missing");
                continue;
            }

            Collider[] colliders = trig.GetComponentsInChildren<Collider>(true);
            SuperController.LogMessage("[BodyTouchTriggerProbe] trigger=" + id + " / colliders=" + (colliders != null ? colliders.Length : 0));

            if (colliders == null)
            {
                continue;
            }

            for (int c = 0; c < colliders.Length; c++)
            {
                Collider col = colliders[c];
                if (col == null)
                {
                    continue;
                }

                Rigidbody rb = col.attachedRigidbody;
                SuperController.LogMessage(
                    "[BodyTouchTriggerProbe] collider / trigger=" + id +
                    " / name=" + col.name +
                    " / isTrigger=" + col.isTrigger +
                    " / enabled=" + col.enabled +
                    " / rb=" + (rb != null ? rb.name : "none")
                );
            }
        }
    }

    void SetStatus(string text)
    {
        if (statusText != null)
        {
            statusText.val = text;
        }
    }

    void LogReport(string report, bool forceLog)
    {
        if (forceLog || report != lastReport)
        {
            SuperController.LogMessage("[BodyTouchTriggerProbe] " + report);
            lastReport = report;
        }
    }
}

class BodyTouchTriggerProbeColliderHook : MonoBehaviour
{
    public BodyTouchTriggerProbe owner;
    public int triggerIndex = -1;
    public string triggerId = "";

    void OnTriggerEnter(Collider other)
    {
        if (owner != null)
        {
            owner.HookContact(triggerIndex, other, "enter");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (owner != null)
        {
            owner.HookContact(triggerIndex, other, "stay");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (owner != null)
        {
            owner.HookExit(triggerIndex, other);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (owner != null)
        {
            owner.HookContact(triggerIndex, collision != null ? collision.collider : null, "enter");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (owner != null)
        {
            owner.HookContact(triggerIndex, collision != null ? collision.collider : null, "stay");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (owner != null)
        {
            owner.HookExit(triggerIndex, collision != null ? collision.collider : null);
        }
    }
}
