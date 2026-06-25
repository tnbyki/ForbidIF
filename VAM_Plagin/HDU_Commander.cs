// HDU_Commander_v024_hba_random_knee_force_button.cs
// v024_hba_random_knee_force_button 2026-06-25
// HDU-like command panel for VaM. It does not merge plugin logic; it only sets registered storables
// and triggers JSONStorableAction entries on existing plugins such as TargetGrabber / TargetLinePerson.
// v020: TargetGrabber=None no longer skips Grab Hand utility routes. It calls TargetGrabber's
//       HDU Grab Hand Pull/Push/Up/Down/Left/Right None actions so None Body Nudge works from HDU.
// v021: Moves Target Cycle Pose/Back/Reset buttons from the left column to the right column.
// v022: Moves Target Cycle Pose/Back/Reset buttons to the lower-right area after Target Load User Defaults.
// v023: Hides Grab Hand Left/Right UI buttons and places an HBA_Cover_RandomKneeToThigh button in that slot.
// v024: HDU HBA Random Knee button calls force action first so manual HDU presses bypass the 30% chance slider.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HDU_Commander_v024_hba_random_knee_force_button : MVRScript
{
    private const string VERSION = "v024_hba_random_knee_force_button";
    private const string ANY = "ANY";
    private const string NONE = "None";

    private JSONStorableBool debugJSON;
    private JSONStorableBool autoScanOnStartJSON;

    private JSONStorableStringChooser tgTargetChooser;
    private JSONStorableBool tgLeftHandJSON;
    private JSONStorableBool tgRightHandJSON;

    private JSONStorableStringChooser pushAutoModeChooser;
    private JSONStorableFloat distanceJSON;

    private JSONStorableString statusJSON;
    private JSONStorableString scanReportJSON;

    private UIDynamicButton clothSelfScanButton;
    private UIDynamicButton clothSelfNextButton;
    private UIDynamicButton clothSelfPrevButton;
    private UIDynamicButton clothTargetScanButton;
    private UIDynamicButton clothTargetNextButton;
    private UIDynamicButton clothTargetPrevButton;
    private float clothButtonRefreshTimer = 0.0f;

    private readonly List<JSONStorableString> labelStorables = new List<JSONStorableString>();

    private string lastTargetGrabber = "missing";
    private string lastTargetLinePerson = "missing";
    private string lastHumanBodyAction = "missing";
    private string lastPoseChanger = "missing";
    private string lastHumanControler = "missing";
    private string lastClothStateSwitcher = "missing";

    private class PluginHit
    {
        public Atom atom;
        public string storableId;
        public JSONStorable storable;
    }

    public override void Init()
    {
        try
        {
            BuildUi();
            RegisterExternalActions();

            if (autoScanOnStartJSON != null && autoScanOnStartJSON.val)
                ScanPlugins();

            SetStatus("Ready / " + VERSION);
        }
        catch (Exception e)
        {
            SuperController.LogError("[HDU_Commander] Init error: " + e);
        }
    }

    public void Update()
    {
        clothButtonRefreshTimer += Time.deltaTime;
        if (clothButtonRefreshTimer < 0.50f)
            return;

        clothButtonRefreshTimer = 0.0f;
        UpdateClothButtonStates();
    }

    private void BuildUi()
    {
        tgTargetChooser = new JSONStorableStringChooser(
            "TargetGrabber",
            new List<string>()
            {
                NONE,
                "Head",
                "Neck",
                "Chest Hold",
                "Hug Body",
                "Gen",
                "Hip Hold",
                "Hand Hold",
                "Foot Hold",
                "Knee Hold"
            },
            "Hug Body",
            "TargetGrabber",
            (JSONStorableStringChooser.SetStringCallback)OnTargetGrabberChoiceChanged
        );
        RegisterStringChooser(tgTargetChooser);
        CreateFilterablePopup(tgTargetChooser, false);

        tgLeftHandJSON = new JSONStorableBool("Left Hand", true);
        tgLeftHandJSON.setCallbackFunction = OnLeftHandChanged;
        RegisterBool(tgLeftHandJSON);
        CreateToggle(tgLeftHandJSON, false);

        tgRightHandJSON = new JSONStorableBool("Right Hand", true);
        tgRightHandJSON.setCallbackFunction = OnRightHandChanged;
        RegisterBool(tgRightHandJSON);
        CreateToggle(tgRightHandJSON, false);

        AddButton("Grab Hand", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand" }, "Grab Hand"); }, GrabButtonColor());
        AddButton("Grab Hand Pull", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Pull", "Grab Pull" }, "Grab Hand Pull"); }, GrabButtonColor());
        AddButton("Grab Hand Push", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Push", "Grab Push" }, "Grab Hand Push"); }, GrabButtonColor());
        AddButton("Grab Hand Up", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Up", "Grab Up" }, "Grab Hand Up"); }, GrabButtonColor());
        AddButton("Grab Hand Down", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Down", "Grab Down" }, "Grab Hand Down"); }, GrabButtonColor());
        AddButton("Grab Hand Open", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Open" }, "Grab Hand Open"); }, GrabButtonColor());
        AddButton("Grab Hand Close", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Close", "Grab Close" }, "Grab Hand Close"); }, GrabButtonColor());
        // v023/v024: Hide Grab Hand Left/Right UI buttons and use this slot for forced HBA random knee action.
        AddButton("HBA Random Knee", false, RunHbaRandomKneeToThigh, TargetButtonColor());

        clothSelfScanButton = AddButtonReturn("Self Cloth SCAN", false, RunClothSelfScan, SelfButtonColor());
        clothSelfNextButton = AddButtonReturn("Self Cloth NEXT", false, RunClothSelfNext, SelfButtonColor());
        clothSelfPrevButton = AddButtonReturn("Self Cloth PREV", false, RunClothSelfPrev, SelfButtonColor());
        clothTargetScanButton = AddButtonReturn("Target Cloth SCAN", false, RunClothTargetScan, TargetButtonColor());
        clothTargetNextButton = AddButtonReturn("Target Cloth NEXT", false, RunClothTargetNext, TargetButtonColor());
        clothTargetPrevButton = AddButtonReturn("Target Cloth PREV", false, RunClothTargetPrev, TargetButtonColor());

        UpdateClothButtonStates();

        debugJSON = new JSONStorableBool("Debug", false);
        RegisterBool(debugJSON);
        CreateToggle(debugJSON, false);

        autoScanOnStartJSON = new JSONStorableBool("Auto Scan On Start", true);
        RegisterBool(autoScanOnStartJSON);
        CreateToggle(autoScanOnStartJSON, false);

        AddButton("HDU Scan", false, ScanPlugins);

        statusJSON = new JSONStorableString("HDU Status", "Ready / " + VERSION);
        RegisterString(statusJSON);
        UIDynamicTextField statusField = CreateTextField(statusJSON, false);
        if (statusField != null)
            statusField.height = 80.0f;

        scanReportJSON = new JSONStorableString("HDU Scan Report", "Not scanned");
        RegisterString(scanReportJSON);
        UIDynamicTextField reportField = CreateTextField(scanReportJSON, false);
        if (reportField != null)
            reportField.height = 120.0f;


        AddButton("PUSH", true, delegate { RunQuick("TargetLinePerson", new string[] { "PUSH" }, "TLP PUSH"); }, DockingButtonColor());
        AddButton("P Midl Line", true, RunPMidlLine, DockingButtonColor());

        pushAutoModeChooser = new JSONStorableStringChooser(
            "PUSH auto Mode",
            new List<string>()
            {
                "None",
                "Auto Line",
                "Auto Line Slow",
                "Auto Line Fast",
                "Auto Spiral",
                "Auto Deep Stop",
                "Auto Random"
            },
            "None",
            "PUSH auto Mode",
            (JSONStorableStringChooser.SetStringCallback)OnPushAutoModeChanged
        );
        RegisterStringChooser(pushAutoModeChooser);
        CreateFilterablePopup(pushAutoModeChooser, true);

        distanceJSON = new JSONStorableFloat("Distance", 1.0f, -1.5f, 3.0f);
        distanceJSON.setCallbackFunction = OnDistanceChanged;
        RegisterFloat(distanceJSON);
        CreateSlider(distanceJSON, true);

        AddButton("Now Docking", true, delegate { RunQuick("TargetLinePerson", new string[] { "Now Docking" }, "Now Docking"); }, DockingButtonColor());
        AddButton("Smart Docking", true, delegate { RunQuick("TargetLinePerson", new string[] { "Smart Docking" }, "Smart Docking"); }, DockingButtonColor());
        AddButton("Reverse Smart Docking", true, delegate { RunQuick("TargetLinePerson", new string[] { "Reverse Smart Docking" }, "Reverse Smart Docking"); }, DockingButtonColor());
        AddButton("Target Swon Drop", true, delegate { RunQuick("TargetGrabber", new string[] { "Target Swoon Drop", "Swoon Drop", "Target Swon Drop" }, "Target Swon Drop"); }, TargetButtonColor());

        AddButton("Self Release", true, delegate { RunQuick("TargetGrabber", new string[] { "Self Release", "Release" }, "Self Release"); }, SelfButtonColor());
        AddButton("Self IK Defaults", true, delegate { RunQuick("TargetGrabber", new string[] { "Self IK Defaults", "Self IK Default" }, "Self IK Defaults"); }, SelfButtonColor());
        AddButton("Self Load User Defaults", true, delegate { RunQuick("TargetGrabber", new string[] { "Self Load User Defaults", "Load User Defaults", "LoadUserDefaults" }, "Self Load User Defaults"); }, SelfButtonColor());
        AddButton("Target Release", true, delegate { RunQuick("TargetGrabber", new string[] { "Target Release", "Release Target" }, "Target Release"); }, TargetButtonColor());
        AddButton("Target IK Default", true, delegate { RunQuick("TargetGrabber", new string[] { "Target IK Default" }, "Target IK Default"); }, TargetButtonColor());
        AddButton("Target Load User Defaults", true, delegate { RunQuick("TargetGrabber", new string[] { "Target Load User Defaults", "Target Load Defaults", "TargetLoadDefaults" }, "Target Load User Defaults"); }, TargetButtonColor());

        AddButton("Target Cycle Pose", true, RunTargetHcCyclePose, TargetButtonColor());
        AddButton("Target Cycle Back", true, RunTargetHcCycleBack, TargetButtonColor());
        AddButton("Target Cycle Reset", true, RunTargetHcCycleReset, TargetButtonColor());
    }

    private void RegisterExternalActions()
    {
        RegisterAction(new JSONStorableAction("HDU Scan", ScanPlugins));
        RegisterAction(new JSONStorableAction("TargetGrabber Apply Controller", ApplyTargetGrabberChoiceLegacy));
        RegisterAction(new JSONStorableAction("Grab Hand", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand" }, "Grab Hand"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Pull", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Pull", "Grab Pull" }, "Grab Hand Pull"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Push", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Push", "Grab Push" }, "Grab Hand Push"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Up", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Up", "Grab Up" }, "Grab Hand Up"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Down", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Down", "Grab Down" }, "Grab Hand Down"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Open", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Open" }, "Grab Hand Open"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Close", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Close", "Grab Close" }, "Grab Hand Close"); }));
        RegisterAction(new JSONStorableAction("HBA_Cover_RandomKneeToThigh", RunHbaRandomKneeToThigh));
        RegisterAction(new JSONStorableAction("HBA Random Knee", RunHbaRandomKneeToThigh));
        // Keep old external actions for compatibility even though the UI buttons are hidden in v023.
        RegisterAction(new JSONStorableAction("Grab Hand Left", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Left", "Grab Left" }, "Grab Hand Left"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Right", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Right", "Grab Right" }, "Grab Hand Right"); }));
        RegisterAction(new JSONStorableAction("Self Release", delegate { RunQuick("TargetGrabber", new string[] { "Self Release", "Release" }, "Self Release"); }));
        RegisterAction(new JSONStorableAction("Self IK Defaults", delegate { RunQuick("TargetGrabber", new string[] { "Self IK Defaults", "Self IK Default" }, "Self IK Defaults"); }));
        RegisterAction(new JSONStorableAction("Self Load User Defaults", delegate { RunQuick("TargetGrabber", new string[] { "Self Load User Defaults", "Load User Defaults", "LoadUserDefaults" }, "Self Load User Defaults"); }));
        RegisterAction(new JSONStorableAction("Target Release", delegate { RunQuick("TargetGrabber", new string[] { "Target Release", "Release Target" }, "Target Release"); }));
        RegisterAction(new JSONStorableAction("Target IK Default", delegate { RunQuick("TargetGrabber", new string[] { "Target IK Default" }, "Target IK Default"); }));
        RegisterAction(new JSONStorableAction("Target Load User Defaults", delegate { RunQuick("TargetGrabber", new string[] { "Target Load User Defaults", "Target Load Defaults", "TargetLoadDefaults" }, "Target Load User Defaults"); }));
        RegisterAction(new JSONStorableAction("TLP PUSH", delegate { RunQuick("TargetLinePerson", new string[] { "PUSH" }, "TLP PUSH"); }));
        RegisterAction(new JSONStorableAction("P Midl Line", RunPMidlLine));
        RegisterAction(new JSONStorableAction("P Midl G Aling", RunPMidlLine));
        RegisterAction(new JSONStorableAction("TLP Now Docking", delegate { RunQuick("TargetLinePerson", new string[] { "Now Docking" }, "Now Docking"); }));
        RegisterAction(new JSONStorableAction("TLP Smart Docking", delegate { RunQuick("TargetLinePerson", new string[] { "Smart Docking" }, "Smart Docking"); }));
        RegisterAction(new JSONStorableAction("TLP Reverse Smart Docking", delegate { RunQuick("TargetLinePerson", new string[] { "Reverse Smart Docking" }, "Reverse Smart Docking"); }));
        RegisterAction(new JSONStorableAction("Target Swon Drop", delegate { RunQuick("TargetGrabber", new string[] { "Target Swoon Drop", "Swoon Drop", "Target Swon Drop" }, "Target Swon Drop"); }));
        RegisterAction(new JSONStorableAction("TargetSwoon Drop", delegate { RunQuick("TargetGrabber", new string[] { "Target Swoon Drop", "Swoon Drop", "Target Swon Drop" }, "Target Swon Drop"); }));
        RegisterAction(new JSONStorableAction("Self Cloth SCAN", RunClothSelfScan));
        RegisterAction(new JSONStorableAction("Self Cloth NEXT", RunClothSelfNext));
        RegisterAction(new JSONStorableAction("Self Cloth PREV", RunClothSelfPrev));
        RegisterAction(new JSONStorableAction("Target Cloth SCAN", RunClothTargetScan));
        RegisterAction(new JSONStorableAction("Target Cloth NEXT", RunClothTargetNext));
        RegisterAction(new JSONStorableAction("Target Cloth PREV", RunClothTargetPrev));
        RegisterAction(new JSONStorableAction("Target Cycle Pose", RunTargetHcCyclePose));
        RegisterAction(new JSONStorableAction("Target Cycle Back", RunTargetHcCycleBack));
        RegisterAction(new JSONStorableAction("Target Cycle Reset", RunTargetHcCycleReset));
    }

    private void AddButton(string label, bool rightSide, UnityEngine.Events.UnityAction callback)
    {
        UIDynamicButton button = CreateButton(label, rightSide);
        if (button != null && button.button != null && callback != null)
            button.button.onClick.AddListener(callback);
    }

    private void AddButton(string label, bool rightSide, UnityEngine.Events.UnityAction callback, Color color)
    {
        AddButtonReturn(label, rightSide, callback, color);
    }

    private UIDynamicButton AddButtonReturn(string label, bool rightSide, UnityEngine.Events.UnityAction callback, Color color)
    {
        UIDynamicButton button = CreateButton(label, rightSide);
        if (button != null)
        {
            ApplyButtonColor(button, color);
            if (button.button != null && callback != null)
                button.button.onClick.AddListener(callback);
        }
        return button;
    }

    private Color GrabButtonColor() { return new Color(0.78f, 0.88f, 1.00f, 1.00f); }
    private Color DockingButtonColor() { return new Color(1.00f, 0.96f, 0.62f, 1.00f); }
    private Color SelfButtonColor() { return new Color(0.78f, 0.88f, 1.00f, 1.00f); }
    private Color TargetButtonColor() { return new Color(1.00f, 0.78f, 0.86f, 1.00f); }

    private Color MissingPluginButtonColor(Color activeColor)
    {
        return Color.Lerp(activeColor, new Color(0.36f, 0.36f, 0.36f, 1.00f), 0.55f);
    }

    private void ApplyButtonEnabled(UIDynamicButton dynamicButton, bool enabled, Color activeColor)
    {
        if (dynamicButton == null || dynamicButton.button == null)
            return;

        dynamicButton.button.interactable = true;
        ApplyButtonColor(dynamicButton, enabled ? activeColor : MissingPluginButtonColor(activeColor));
    }

    private void ApplyButtonColor(UIDynamicButton dynamicButton, Color normalColor)
    {
        if (dynamicButton == null || dynamicButton.button == null)
            return;

        ColorBlock colors = dynamicButton.button.colors;
        colors.normalColor = normalColor;
        colors.highlightedColor = Color.Lerp(normalColor, Color.white, 0.18f);
        colors.pressedColor = Color.Lerp(normalColor, Color.black, 0.16f);
        colors.disabledColor = new Color(normalColor.r, normalColor.g, normalColor.b, 0.45f);
        dynamicButton.button.colors = colors;
    }

    private void ApplyAndRunSelfHand(string[] actionNames, string label)
    {
        ApplySelfHandFlagsToTargetGrabber();

        if (TryRunHduTargetGrabRoute(label))
            return;

        ApplyTargetGrabberChoiceLegacy();
        RunQuick("TargetGrabber", actionNames, label);
    }

    private bool TryRunHduTargetGrabRoute(string label)
    {
        string display = tgTargetChooser != null ? tgTargetChooser.val : "Hug Body";
        string actual = MapTargetGrabberChoice(display);

        string actionName = BuildHduTargetGrabActionName(label, actual);
        if (string.IsNullOrEmpty(actionName))
            return false;

        return RunQuick("TargetGrabber", new string[] { actionName }, label + " / " + actual);
    }

    private string BuildHduTargetGrabActionName(string label, string actual)
    {
        if (string.IsNullOrEmpty(label) || string.IsNullOrEmpty(actual))
            return null;

        if (actual == NONE ||
            actual == "Head" ||
            actual == "Neck" ||
            actual == "Chest Hold" ||
            actual == "Hug Body" ||
            actual == "Hip Hold" ||
            actual == "Hand Hold" ||
            actual == "Foot Hold" ||
            actual == "Knee Hold")
        {
            return "HDU " + label + " " + actual;
        }

        return null;
    }

    private bool ApplySelfHandFlagsToTargetGrabber()
    {
        bool leftValue = tgLeftHandJSON == null || tgLeftHandJSON.val;
        bool rightValue = tgRightHandJSON == null || tgRightHandJSON.val;

        bool leftOk = TrySetBoolParam("TargetGrabber", new string[] { "Left Hand" }, leftValue, "Self Left Hand");
        bool rightOk = TrySetBoolParam("TargetGrabber", new string[] { "Right Hand" }, rightValue, "Self Right Hand");

        DebugLog("apply self hand flags to TargetGrabber / L=" + Bool01(leftValue) + " set=" + Bool01(leftOk) +
            " / R=" + Bool01(rightValue) + " set=" + Bool01(rightOk));
        return leftOk || rightOk;
    }

    private void OnTargetGrabberChoiceChanged(string value)
    {
        string display = tgTargetChooser != null ? tgTargetChooser.val : "Hug Body";
        SetStatus("TargetGrabber pending: " + display);
    }

    private void ApplyTargetGrabberChoiceLegacy()
    {
        string display = tgTargetChooser != null ? tgTargetChooser.val : "Hug Body";
        string actual = MapTargetGrabberChoice(display);
        if (actual == NONE)
        {
            bool noneOk = TrySetStringChooserParam("TargetGrabber", new string[] { "targetPersonController", "IK Select", "Target Controller" }, "<none>", "TargetGrabber controller");
            if (!noneOk)
                noneOk = TrySetStringChooserParam("TargetGrabber", new string[] { "targetPersonController", "IK Select", "Target Controller" }, NONE, "TargetGrabber controller");
            if (!noneOk)
                noneOk = RunQuick("TargetGrabber", new string[] { "Target Shortcut None" }, "TargetGrabber None");
            SetStatus((noneOk ? "TargetGrabber Controller: " : "TargetGrabber Controller failed: ") + display + " -> " + actual);
            return;
        }

        bool ok = TrySetStringChooserParam("TargetGrabber", new string[] { "targetPersonController", "IK Select", "Target Controller" }, actual, "TargetGrabber controller");
        if (!ok)
            ok = RunTargetGrabberShortcutFallback(actual);

        SetStatus((ok ? "TargetGrabber Controller: " : "TargetGrabber Controller failed: ") + display + " -> " + actual);
    }

    private string MapTargetGrabberChoice(string display)
    {
        if (display == NONE) return NONE;
        if (display == "Head") return "Head";
        if (display == "neck" || display == "NECK" || display == "Neck") return "Neck";
        if (display == "Chest Hold") return "Chest Hold";
        if (display == "Hug Body") return "Hug Body";
        if (display == "Gen") return "Gen";
        if (display == "Hip Hold") return "Hip Hold";
        if (display == "Hand Hold") return "Hand Hold";
        if (display == "Foot Hold") return "Foot Hold";
        if (display == "Knee Hold") return "Knee Hold";
        return display;
    }

    private bool RunTargetGrabberShortcutFallback(string actual)
    {
        if (actual == NONE)
            return RunQuick("TargetGrabber", new string[] { "Target Shortcut None" }, "TargetGrabber None");
        if (actual == "Head")
            return RunQuick("TargetGrabber", new string[] { "Target Shortcut Head" }, "TargetGrabber Head");
        if (actual == "Neck")
            return RunQuick("TargetGrabber", new string[] { "Target Shortcut Neck" }, "TargetGrabber Neck");
        if (actual == "Chest Hold")
            return RunQuick("TargetGrabber", new string[] { "Target Shortcut Chest Hold" }, "TargetGrabber Chest Hold");
        if (actual == "Hug Body")
            return RunQuick("TargetGrabber", new string[] { "Target Shortcut Hug Body" }, "TargetGrabber Hug Body");
        if (actual == "Hip Hold")
            return RunQuick("TargetGrabber", new string[] { "Target Shortcut Hip Hold" }, "TargetGrabber Hip Hold");
        if (actual == "Hand Hold")
            return RunQuick("TargetGrabber", new string[] { "Target Shortcut Hand Hold" }, "TargetGrabber Hand Hold");
        if (actual == "Foot Hold")
            return RunQuick("TargetGrabber", new string[] { "Target Shortcut Foot Hold" }, "TargetGrabber Foot Hold");
        if (actual == "Knee Hold")
            return RunQuick("TargetGrabber", new string[] { "Target Shortcut Knee Hold" }, "TargetGrabber Knee Hold");
        return false;
    }

    private void OnLeftHandChanged(bool value)
    {
        bool ok = TrySetBoolParam("TargetGrabber", new string[] { "Left Hand" }, value, "Self Left Hand");
        SetStatus("Self Left Hand: " + Bool01(value) + " / set=" + Bool01(ok));
    }

    private void OnRightHandChanged(bool value)
    {
        bool ok = TrySetBoolParam("TargetGrabber", new string[] { "Right Hand" }, value, "Self Right Hand");
        SetStatus("Self Right Hand: " + Bool01(value) + " / set=" + Bool01(ok));
    }

    private void RunHbaRandomKneeToThigh()
    {
        RunQuick("HumanBodyAction",
            new string[]
            {
                // v024: HDU button is explicit/manual, so prefer the force aliases added in HBA v073.
                "HBA_Cover_RandomKneeToThigh_Force",
                "HBR_Cover_RandomKneeToThigh_Force",
                // Fallback for older HBA builds. Older builds may still obey the chance slider.
                "HBA_Cover_RandomKneeToThigh",
                "HBR_Cover_RandomKneeToThigh"
            },
            "HBA Random Knee");
    }

    private void RunPMidlLine()
    {
        bool ok = RunQuick("TargetLinePerson",
            new string[]
            {
                "P Midl Line",
                "P Midl G Aling",
                "P Midl G Align",
                "P Mid G Align",
                "P Yellow Path Align"
            },
            "P Midl Line");

        if (!ok)
        {
            ok = TrySetBoolParam("TargetLinePerson",
                new string[] { "P Midl Line", "P Midl G Aling", "P Midl G Align", "P Mid G Align", "P Yellow Path Align" },
                true,
                "P Midl Line ON");

            SetStatus("P Midl Line ON fallback: set=" + Bool01(ok));
        }
    }

    private void OnPushAutoModeChanged(string value)
    {
        bool ok = TrySetStringChooserParam("TargetLinePerson", new string[] { "PUSH Auto Mode" }, value, "TLP PUSH Auto Mode");
        SetStatus("PUSH auto Mode: " + value + " / set=" + Bool01(ok));
    }

    private void OnDistanceChanged(float value)
    {
        bool ok = TrySetFloatParam("TargetLinePerson", new string[] { "Distance" }, value, "TLP Distance");
        SetStatus("Distance: " + value.ToString("F3") + " / set=" + Bool01(ok));
    }

    private void RunTargetHcCyclePose() { RunTargetHumanControlerQuick(new string[] { "HC Cycle Pose", "Cycle Pose" }, "Target Cycle Pose"); }
    private void RunTargetHcCycleBack() { RunTargetHumanControlerQuick(new string[] { "HC Cycle Back", "Cycle Back" }, "Target Cycle Back"); }
    private void RunTargetHcCycleReset() { RunTargetHumanControlerQuick(new string[] { "HC Cycle Reset", "Cycle Reset" }, "Target Cycle Reset"); }

    private bool RunTargetHumanControlerQuick(string[] actionNames, string label)
    {
        PluginHit hit;
        if (!TryFindTargetHumanControler(out hit) || hit == null || hit.storable == null)
        {
            SetStatus("Missing: " + label + " / target humanControler");
            return false;
        }

        EnsureHumanControlerTargetSelection(hit, label);
        return TriggerActionOnHit(hit, actionNames, label, "target HC");
    }

    private void RunClothSelfScan() { RunClothQuick(false, new string[] { "Scan Cloth" }, "Self Cloth SCAN"); }
    private void RunClothSelfNext() { RunClothQuick(false, new string[] { "Next Hide" }, "Self Cloth NEXT"); }
    private void RunClothSelfPrev() { RunClothQuick(false, new string[] { "Prev Wear" }, "Self Cloth PREV"); }
    private void RunClothTargetScan() { RunClothQuick(true, new string[] { "Scan Cloth" }, "Target Cloth SCAN"); }
    private void RunClothTargetNext() { RunClothQuick(true, new string[] { "Next Hide" }, "Target Cloth NEXT"); }
    private void RunClothTargetPrev() { RunClothQuick(true, new string[] { "Prev Wear" }, "Target Cloth PREV"); }

    private bool RunClothQuick(bool targetSide, string[] actionNames, string label)
    {
        PluginHit hit;
        bool found = targetSide ? TryFindTargetClothSwitcher(out hit) : TryFindSelfClothSwitcher(out hit);
        if (!found || hit == null || hit.storable == null)
        {
            SetStatus("Missing: " + label + " / ClothStateSwitcher " + (targetSide ? "target" : "self"));
            UpdateClothButtonStates();
            return false;
        }

        EnsureClothSwitcherSelfSelection(hit, targetSide ? "target" : "self");
        bool ok = TriggerActionOnHit(hit, actionNames, label, "cloth");
        UpdateClothButtonStates();
        return ok;
    }

    private void UpdateClothButtonStates()
    {
        PluginHit selfHit;
        PluginHit targetHit;
        bool selfOk = TryFindSelfClothSwitcher(out selfHit);
        bool targetOk = TryFindTargetClothSwitcher(out targetHit);

        SetButtonText(clothSelfScanButton, "Self Cloth SCAN " + BuildClothWornCountLabel(selfHit, selfOk));
        SetButtonText(clothTargetScanButton, "Target Cloth SCAN " + BuildClothWornCountLabel(targetHit, targetOk));

        ApplyButtonEnabled(clothSelfScanButton, selfOk, SelfButtonColor());
        ApplyButtonEnabled(clothSelfNextButton, selfOk, SelfButtonColor());
        ApplyButtonEnabled(clothSelfPrevButton, selfOk, SelfButtonColor());
        ApplyButtonEnabled(clothTargetScanButton, targetOk, TargetButtonColor());
        ApplyButtonEnabled(clothTargetNextButton, targetOk, TargetButtonColor());
        ApplyButtonEnabled(clothTargetPrevButton, targetOk, TargetButtonColor());
    }

    private void SetButtonText(UIDynamicButton dynamicButton, string text)
    {
        if (dynamicButton == null || dynamicButton.button == null || string.IsNullOrEmpty(text))
            return;

        try
        {
            Text label = dynamicButton.button.GetComponentInChildren<Text>();
            if (label != null && label.text != text)
                label.text = text;
        }
        catch
        {
        }
    }

    private string BuildClothWornCountLabel(PluginHit hit, bool found)
    {
        int hidden;
        int total;
        if (!found || !TryGetClothProgress(hit, out hidden, out total) || total <= 0)
            return "-/-";

        int worn = Mathf.Clamp(total - hidden, 0, total);
        return worn.ToString() + "/" + total.ToString();
    }

    private bool TryGetClothProgress(PluginHit hit, out int hidden, out int total)
    {
        hidden = 0;
        total = 0;

        if (hit == null || hit.storable == null)
            return false;

        JSONStorableString progress = hit.storable.GetStringJSONParam("State Progress");
        if (progress == null || string.IsNullOrEmpty(progress.val))
            return false;

        string[] parts = progress.val.Split('/');
        if (parts == null || parts.Length < 2)
            return false;

        if (!int.TryParse(parts[0], out hidden))
            return false;
        if (!int.TryParse(parts[1], out total))
            return false;

        hidden = Mathf.Max(0, hidden);
        total = Mathf.Max(0, total);
        return true;
    }

    private string BuildClothSwitcherReport()
    {
        PluginHit selfHit;
        PluginHit targetHit;

        bool selfOk = TryFindSelfClothSwitcher(out selfHit);
        bool targetOk = TryFindTargetClothSwitcher(out targetHit);

        if (selfOk)
            EnsureClothSwitcherSelfSelection(selfHit, "self");
        if (targetOk)
            EnsureClothSwitcherSelfSelection(targetHit, "target");

        string selfLabel = selfOk && selfHit != null ? selfHit.atom.uid + " / " + selfHit.storableId : "missing";
        string targetLabel = targetOk && targetHit != null ? targetHit.atom.uid + " / " + targetHit.storableId : "missing";

        return "Self: " + selfLabel + " / Target: " + targetLabel;
    }

    private bool TryFindTargetHumanControler(out PluginHit hit)
    {
        hit = null;
        List<Atom> atoms = BuildTargetAtomSearchList();
        string[] pluginNames = new string[] { "humanControler", "humanPoseControler" };

        for (int i = 0; i < atoms.Count; i++)
        {
            Atom atom = atoms[i];
            for (int p = 0; p < pluginNames.Length; p++)
            {
                if (TryFindPluginOnAtom(atom, pluginNames[p], out hit))
                    return true;
            }
        }
        return false;
    }

    private void EnsureHumanControlerTargetSelection(PluginHit hit, string label)
    {
        if (hit == null || hit.storable == null || containingAtom == null)
            return;

        string ownUid = containingAtom.uid;
        if (string.IsNullOrEmpty(ownUid))
            return;

        JSONStorableStringChooser chooser = hit.storable.GetStringChooserJSONParam("Target Person");
        if (chooser == null)
            chooser = hit.storable.GetStringChooserJSONParam("Target");
        if (chooser == null)
            return;

        if (chooser.choices != null && chooser.choices.Count > 0 && !chooser.choices.Contains(ownUid))
        {
            DebugLog("target HC chooser value missing / label=" + label + " / own=" + ownUid);
            return;
        }

        if (chooser.val != ownUid)
        {
            chooser.val = ownUid;
            DebugLog("target HC target auto-selected / label=" + label + " / own=" + ownUid);
        }
    }

    private void EnsureClothSwitcherSelfSelection(PluginHit hit, string sideLabel)
    {
        if (hit == null || hit.atom == null || hit.storable == null)
            return;

        string atomUid = hit.atom.uid;
        if (string.IsNullOrEmpty(atomUid))
            return;

        string[] paramNames = new string[] { "person", "Person" };
        for (int i = 0; i < paramNames.Length; i++)
        {
            JSONStorableStringChooser chooser = hit.storable.GetStringChooserJSONParam(paramNames[i]);
            if (chooser == null)
                continue;

            if (chooser.choices != null && chooser.choices.Count > 0 && !chooser.choices.Contains(atomUid))
            {
                DebugLog("cloth person chooser value missing / side=" + sideLabel + " / atom=" + atomUid + " / param=" + paramNames[i]);
                continue;
            }

            if (chooser.val != atomUid)
            {
                chooser.val = atomUid;
                DebugLog("cloth person auto-selected / side=" + sideLabel + " / atom=" + atomUid + " / param=" + paramNames[i]);
            }
            return;
        }
    }

    private void ScanPlugins()
    {
        lastTargetGrabber = FindPluginLabel("TargetGrabber");
        lastTargetLinePerson = FindPluginLabel("TargetLinePerson");
        lastHumanBodyAction = FindPluginLabel("HumanBodyAction");
        lastPoseChanger = FindPluginLabel("PoseChanger");
        lastHumanControler = FindTargetHumanControlerLabel();
        lastClothStateSwitcher = BuildClothSwitcherReport();

        UpdateClothButtonStates();

        string report =
            "TargetGrabber: " + lastTargetGrabber + "\n" +
            "TargetLinePerson: " + lastTargetLinePerson + "\n" +
            "HumanBodyAction: " + lastHumanBodyAction + "\n" +
            "PoseChanger: " + lastPoseChanger + "\n" +
            "Target humanControler: " + lastHumanControler + "\n" +
            "ClothStateSwitcher: " + lastClothStateSwitcher;

        if (scanReportJSON != null)
            scanReportJSON.val = report;

        SetStatus("Scan complete");
        DebugLog("scan complete / " + report.Replace("\n", " / "));
    }

    private string FindTargetHumanControlerLabel()
    {
        PluginHit hit;
        if (TryFindTargetHumanControler(out hit))
            return hit.atom.uid + " / " + hit.storableId;
        return "missing";
    }

    private string FindPluginLabel(string pluginContains)
    {
        PluginHit hit;
        if (TryFindPlugin(pluginContains, out hit))
            return hit.atom.uid + " / " + hit.storableId;
        return "missing";
    }

    private bool TryFindSelfClothSwitcher(out PluginHit hit)
    {
        return TryFindPluginOnAtom(containingAtom, "ClothStateSwitcher", out hit);
    }

    private bool TryFindTargetClothSwitcher(out PluginHit hit)
    {
        hit = null;
        List<Atom> atoms = BuildTargetAtomSearchList();
        for (int i = 0; i < atoms.Count; i++)
        {
            if (TryFindPluginOnAtom(atoms[i], "ClothStateSwitcher", out hit))
                return true;
        }
        return false;
    }

    private List<Atom> BuildTargetAtomSearchList()
    {
        List<Atom> result = new List<Atom>();

        try
        {
            List<Atom> atoms = SuperController.singleton.GetAtoms();
            if (atoms != null)
            {
                for (int i = 0; i < atoms.Count; i++)
                {
                    Atom atom = atoms[i];
                    if (atom == null || atom == containingAtom)
                        continue;
                    if (atom.type == "Person")
                        AddUniqueAtom(result, atom);
                }
            }
        }
        catch (Exception e)
        {
            DebugLog("BuildTargetAtomSearchList error: " + e.Message);
        }

        return result;
    }

    private List<Atom> BuildAtomSearchList()
    {
        List<Atom> result = new List<Atom>();
        AddUniqueAtom(result, containingAtom);

        try
        {
            List<Atom> atoms = SuperController.singleton.GetAtoms();
            if (atoms != null)
            {
                for (int i = 0; i < atoms.Count; i++)
                    AddUniqueAtom(result, atoms[i]);
            }
        }
        catch (Exception e)
        {
            DebugLog("BuildAtomSearchList error: " + e.Message);
        }

        return result;
    }

    private void AddUniqueAtom(List<Atom> list, Atom atom)
    {
        if (list == null || atom == null)
            return;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == atom)
                return;
        }
        list.Add(atom);
    }

    private bool RunQuick(string pluginContains, string[] actionNames, string label)
    {
        bool ok = TryTriggerAction(pluginContains, actionNames, label);
        if (!ok)
            SetStatus("Missing action: " + label + " / " + pluginContains + " / " + JoinActions(actionNames));
        return ok;
    }

    private bool TryTriggerAction(string pluginContains, string[] actionNames, string label)
    {
        if (string.IsNullOrEmpty(pluginContains) || actionNames == null || actionNames.Length == 0)
            return false;

        PluginHit hit;
        if (!TryFindPlugin(pluginContains, out hit))
        {
            DebugLog("plugin missing / " + pluginContains + " / label=" + label);
            return false;
        }

        return TriggerActionOnHit(hit, actionNames, label, "trigger");
    }

    private bool TriggerActionOnHit(PluginHit hit, string[] actionNames, string label, string kind)
    {
        if (hit == null || hit.storable == null || actionNames == null)
            return false;

        for (int i = 0; i < actionNames.Length; i++)
        {
            string actionName = actionNames[i];
            if (string.IsNullOrEmpty(actionName))
                continue;

            JSONStorableAction action = hit.storable.GetAction(actionName);
            if (action == null)
                continue;

            try
            {
                if (action.actionCallback != null)
                    action.actionCallback.Invoke();

                SetStatus("OK: " + label + " -> " + hit.atom.uid + " / " + hit.storableId + " / " + actionName);
                DebugLog(kind + " / label=" + label + " / atom=" + hit.atom.uid + " / storable=" + hit.storableId + " / action=" + actionName);
                return true;
            }
            catch (Exception e)
            {
                SuperController.LogError("[HDU_Commander] action error: " + label + " / " + actionName + " / " + e);
                SetStatus("Action error: " + label + " / " + actionName);
                return false;
            }
        }

        DebugLog("action missing / label=" + label + " / actions=" + JoinActions(actionNames) + " / storable=" + hit.storableId);
        return false;
    }

    private bool TrySetBoolParam(string pluginContains, string[] paramNames, bool value, string label)
    {
        PluginHit hit;
        if (!TryFindPlugin(pluginContains, out hit))
            return false;

        for (int i = 0; i < paramNames.Length; i++)
        {
            JSONStorableBool p = hit.storable.GetBoolJSONParam(paramNames[i]);
            if (p == null)
                continue;
            p.val = value;
            DebugLog("set bool / " + label + " / " + paramNames[i] + "=" + Bool01(value));
            return true;
        }
        return false;
    }

    private bool TrySetFloatParam(string pluginContains, string[] paramNames, float value, string label)
    {
        PluginHit hit;
        if (!TryFindPlugin(pluginContains, out hit))
            return false;

        for (int i = 0; i < paramNames.Length; i++)
        {
            JSONStorableFloat p = hit.storable.GetFloatJSONParam(paramNames[i]);
            if (p == null)
                continue;
            p.val = value;
            DebugLog("set float / " + label + " / " + paramNames[i] + "=" + value.ToString("F3"));
            return true;
        }
        return false;
    }

    private bool TrySetStringChooserParam(string pluginContains, string[] paramNames, string value, string label)
    {
        PluginHit hit;
        if (!TryFindPlugin(pluginContains, out hit))
            return false;

        for (int i = 0; i < paramNames.Length; i++)
        {
            JSONStorableStringChooser p = hit.storable.GetStringChooserJSONParam(paramNames[i]);
            if (p == null)
                continue;

            if (p.choices != null && p.choices.Count > 0 && !p.choices.Contains(value))
            {
                DebugLog("chooser value missing / " + label + " / param=" + paramNames[i] + " / value=" + value);
                continue;
            }

            p.val = value;
            DebugLog("set chooser / " + label + " / " + paramNames[i] + "=" + value);
            return true;
        }
        return false;
    }

    private bool TryFindPlugin(string pluginContains, out PluginHit hit)
    {
        hit = null;
        if (string.IsNullOrEmpty(pluginContains))
            return false;

        if (IndexOfIgnoreCase(pluginContains, "TargetGrabber") >= 0)
            return TryFindPluginOnAtom(containingAtom, pluginContains, out hit);

        List<Atom> atoms = BuildAtomSearchList();
        for (int i = 0; i < atoms.Count; i++)
        {
            Atom atom = atoms[i];
            if (TryFindPluginOnAtom(atom, pluginContains, out hit))
                return true;
        }

        return false;
    }

    private bool TryFindPluginOnAtom(Atom atom, string pluginContains, out PluginHit hit)
    {
        hit = null;
        if (atom == null || string.IsNullOrEmpty(pluginContains))
            return false;

        List<string> ids = atom.GetStorableIDs();
        if (ids == null)
            return false;

        for (int j = 0; j < ids.Count; j++)
        {
            string id = ids[j];
            if (string.IsNullOrEmpty(id))
                continue;
            if (IndexOfIgnoreCase(id, pluginContains) < 0)
                continue;

            JSONStorable storable = atom.GetStorableByID(id);
            if (storable == null)
                continue;

            hit = new PluginHit();
            hit.atom = atom;
            hit.storableId = id;
            hit.storable = storable;
            return true;
        }

        return false;
    }

    private int IndexOfIgnoreCase(string source, string value)
    {
        if (source == null || value == null)
            return -1;
        return source.IndexOf(value, StringComparison.OrdinalIgnoreCase);
    }

    private string JoinActions(string[] actionNames)
    {
        if (actionNames == null)
            return "";
        return string.Join(",", actionNames);
    }

    private string Bool01(bool value)
    {
        return value ? "1" : "0";
    }

    private void SetStatus(string text)
    {
        if (statusJSON != null)
            statusJSON.val = text;
        DebugLog(text);
    }

    private void DebugLog(string text)
    {
        if (debugJSON != null && debugJSON.val)
            SuperController.LogMessage("[HDU_Commander] " + text);
    }
}
