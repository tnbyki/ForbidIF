// HDU_Commander_v009_grab_blue_knee_docking_yellow.cs
// v009_grab_blue_knee_docking_yellow 2026-06-23
// HDU-like command panel for VaM. It does not merge plugin logic; it only sets registered storables
// and triggers JSONStorableAction entries on existing plugins such as TargetGrabber / TargetLinePerson.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HDU_Commander_v009_grab_blue_knee_docking_yellow : MVRScript
{
    private const string VERSION = "v009_grab_blue_knee_docking_yellow";
    private const string ANY = "ANY";
    private const string NONE = "None";

    private JSONStorableBool debugJSON;
    private JSONStorableBool autoScanOnStartJSON;

    private JSONStorableStringChooser tgTargetChooser;
    private JSONStorableBool tgLeftHandJSON;
    private JSONStorableBool tgRightHandJSON;

    private JSONStorableBool pMidGAlignJSON;
    private JSONStorableStringChooser pushAutoModeChooser;
    private JSONStorableFloat distanceJSON;

    private JSONStorableString statusJSON;
    private JSONStorableString scanReportJSON;

    private readonly List<JSONStorableString> labelStorables = new List<JSONStorableString>();

    private string lastTargetGrabber = "missing";
    private string lastTargetLinePerson = "missing";
    private string lastHumanBodyAction = "missing";
    private string lastPoseChanger = "missing";
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

    private void BuildUi()
    {
        // Left column: TargetGrabber controls. Target person is managed by each source plugin.
        tgTargetChooser = new JSONStorableStringChooser(
            "TargetGrabber",
            new List<string>()
            {
                "NECK",
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

        // Grab Hand buttons are directly under Right Hand.
        // The HDU Left/Right Hand toggles are self-side hand flags on the TargetGrabber found on this Person first.
        AddButton("Grab Hand", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand" }, "Grab Hand"); }, GrabButtonColor());
        AddButton("Grab Hand Pull", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Pull", "Grab Pull" }, "Grab Hand Pull"); }, GrabButtonColor());
        AddButton("Grab Hand Push", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Push", "Grab Push" }, "Grab Hand Push"); }, GrabButtonColor());
        AddButton("Grab Hand Up", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Up", "Grab Up" }, "Grab Hand Up"); }, GrabButtonColor());
        AddButton("Grab Hand Down", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Down", "Grab Down" }, "Grab Hand Down"); }, GrabButtonColor());
        AddButton("Grab Hand Open", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Open" }, "Grab Hand Open"); }, GrabButtonColor());
        AddButton("Grab Hand Close", false, delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Close", "Grab Close" }, "Grab Hand Close"); }, GrabButtonColor());

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

        // Right column: TargetLinePerson style controls.
        AddButton("PUSH", true, delegate { RunQuick("TargetLinePerson", new string[] { "PUSH" }, "TLP PUSH"); }, DockingButtonColor());

        pMidGAlignJSON = new JSONStorableBool("P Midl G Aling", false);
        pMidGAlignJSON.setCallbackFunction = OnPMidGAlignChanged;
        RegisterBool(pMidGAlignJSON);
        CreateToggle(pMidGAlignJSON, true);

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

        // Restore / reset block moved to lower-right, same order as TargetGrabber.
        // Self buttons are light blue, Target buttons are light pink.
        AddButton("Self Release", true, delegate { RunQuick("TargetGrabber", new string[] { "Self Release", "Release" }, "Self Release"); }, SelfButtonColor());
        AddButton("Self IK Defaults", true, delegate { RunQuick("TargetGrabber", new string[] { "Self IK Defaults", "Self IK Default" }, "Self IK Defaults"); }, SelfButtonColor());
        AddButton("Self Load User Defaults", true, delegate { RunQuick("TargetGrabber", new string[] { "Self Load User Defaults", "Load User Defaults", "LoadUserDefaults" }, "Self Load User Defaults"); }, SelfButtonColor());
        AddButton("Target Release", true, delegate { RunQuick("TargetGrabber", new string[] { "Target Release", "Release Target" }, "Target Release"); }, TargetButtonColor());
        AddButton("Target IK Default", true, delegate { RunQuick("TargetGrabber", new string[] { "Target IK Default" }, "Target IK Default"); }, TargetButtonColor());
        AddButton("Target Load User Defaults", true, delegate { RunQuick("TargetGrabber", new string[] { "Target Load User Defaults", "Target Load Defaults", "TargetLoadDefaults" }, "Target Load User Defaults"); }, TargetButtonColor());
    }

    private void RegisterExternalActions()
    {
        RegisterAction(new JSONStorableAction("HDU Scan", ScanPlugins));
        RegisterAction(new JSONStorableAction("TargetGrabber Apply Controller", ApplyTargetGrabberChoice));
        RegisterAction(new JSONStorableAction("Grab Hand", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand" }, "Grab Hand"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Pull", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Pull", "Grab Pull" }, "Grab Hand Pull"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Push", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Push", "Grab Push" }, "Grab Hand Push"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Up", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Up", "Grab Up" }, "Grab Hand Up"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Down", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Down", "Grab Down" }, "Grab Hand Down"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Open", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Open" }, "Grab Hand Open"); }));
        RegisterAction(new JSONStorableAction("Grab Hand Close", delegate { ApplyAndRunSelfHand(new string[] { "Grab Hand Close", "Grab Close" }, "Grab Hand Close"); }));
        RegisterAction(new JSONStorableAction("Self Release", delegate { RunQuick("TargetGrabber", new string[] { "Self Release", "Release" }, "Self Release"); }));
        RegisterAction(new JSONStorableAction("Self IK Defaults", delegate { RunQuick("TargetGrabber", new string[] { "Self IK Defaults", "Self IK Default" }, "Self IK Defaults"); }));
        RegisterAction(new JSONStorableAction("Self Load User Defaults", delegate { RunQuick("TargetGrabber", new string[] { "Self Load User Defaults", "Load User Defaults", "LoadUserDefaults" }, "Self Load User Defaults"); }));
        RegisterAction(new JSONStorableAction("Target Release", delegate { RunQuick("TargetGrabber", new string[] { "Target Release", "Release Target" }, "Target Release"); }));
        RegisterAction(new JSONStorableAction("Target IK Default", delegate { RunQuick("TargetGrabber", new string[] { "Target IK Default" }, "Target IK Default"); }));
        RegisterAction(new JSONStorableAction("Target Load User Defaults", delegate { RunQuick("TargetGrabber", new string[] { "Target Load User Defaults", "Target Load Defaults", "TargetLoadDefaults" }, "Target Load User Defaults"); }));
        RegisterAction(new JSONStorableAction("TLP PUSH", delegate { RunQuick("TargetLinePerson", new string[] { "PUSH" }, "TLP PUSH"); }));
        RegisterAction(new JSONStorableAction("TLP Now Docking", delegate { RunQuick("TargetLinePerson", new string[] { "Now Docking" }, "Now Docking"); }));
        RegisterAction(new JSONStorableAction("TLP Smart Docking", delegate { RunQuick("TargetLinePerson", new string[] { "Smart Docking" }, "Smart Docking"); }));
        RegisterAction(new JSONStorableAction("TLP Reverse Smart Docking", delegate { RunQuick("TargetLinePerson", new string[] { "Reverse Smart Docking" }, "Reverse Smart Docking"); }));
        RegisterAction(new JSONStorableAction("Target Swon Drop", delegate { RunQuick("TargetGrabber", new string[] { "Target Swoon Drop", "Swoon Drop", "Target Swon Drop" }, "Target Swon Drop"); }));
        RegisterAction(new JSONStorableAction("TargetSwoon Drop", delegate { RunQuick("TargetGrabber", new string[] { "Target Swoon Drop", "Swoon Drop", "Target Swon Drop" }, "Target Swon Drop"); }));
    }

    private void AddLabel(string text, bool rightSide, float height)
    {
        string key = "label_" + labelStorables.Count.ToString("000") + "_" + text;
        JSONStorableString label = new JSONStorableString(key, text);
        labelStorables.Add(label);
        RegisterString(label);
        UIDynamicTextField field = CreateTextField(label, rightSide);
        if (field != null)
            field.height = height;
    }

    private void AddButton(string label, bool rightSide, UnityEngine.Events.UnityAction callback)
    {
        UIDynamicButton button = CreateButton(label, rightSide);
        if (button != null)
            button.button.onClick.AddListener(callback);
    }

    private void AddButton(string label, bool rightSide, UnityEngine.Events.UnityAction callback, Color color)
    {
        UIDynamicButton button = CreateButton(label, rightSide);
        if (button != null)
        {
            ApplyButtonColor(button, color);
            button.button.onClick.AddListener(callback);
        }
    }

    private Color GrabButtonColor()
    {
        return new Color(0.78f, 0.88f, 1.00f, 1.00f);
    }

    private Color DockingButtonColor()
    {
        return new Color(1.00f, 0.96f, 0.62f, 1.00f);
    }

    private Color SelfButtonColor()
    {
        return new Color(0.78f, 0.88f, 1.00f, 1.00f);
    }

    private Color TargetButtonColor()
    {
        return new Color(1.00f, 0.78f, 0.86f, 1.00f);
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
        RunQuick("TargetGrabber", actionNames, label);
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
        ApplyTargetGrabberChoice();
    }

    private void ApplyTargetGrabberChoice()
    {
        string display = tgTargetChooser != null ? tgTargetChooser.val : "Hug Body";
        string actual = MapTargetGrabberChoice(display);

        bool ok = TrySetStringChooserParam("TargetGrabber", new string[] { "targetPersonController", "IK Select", "Target Controller" }, actual, "TargetGrabber controller");

        if (!ok)
        {
            // Fallback for the shortcut buttons that exist on TargetGrabber.
            ok = RunTargetGrabberShortcutFallback(actual);
        }

        SetStatus((ok ? "TargetGrabber Controller: " : "TargetGrabber Controller failed: ") + display + " -> " + actual);
    }

    private string MapTargetGrabberChoice(string display)
    {
        if (display == "NECK") return "Neck";
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

    private void OnPMidGAlignChanged(bool value)
    {
        bool ok = TrySetBoolParam("TargetLinePerson", new string[] { "P Yellow Path Align", "P Midl G Aling", "P Mid G Align" }, value, "TLP P Midl G Aling");
        SetStatus("P Midl G Aling: " + Bool01(value) + " / set=" + Bool01(ok));
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

    private void ScanPlugins()
    {
        lastTargetGrabber = FindPluginLabel("TargetGrabber");
        lastTargetLinePerson = FindPluginLabel("TargetLinePerson");
        lastHumanBodyAction = FindPluginLabel("HumanBodyAction");
        lastPoseChanger = FindPluginLabel("PoseChanger");
        lastClothStateSwitcher = FindPluginLabel("ClothStateSwitcher");

        string report =
            "TargetGrabber: " + lastTargetGrabber + "\n" +
            "TargetLinePerson: " + lastTargetLinePerson + "\n" +
            "HumanBodyAction: " + lastHumanBodyAction + "\n" +
            "PoseChanger: " + lastPoseChanger + "\n" +
            "ClothStateSwitcher: " + lastClothStateSwitcher;

        if (scanReportJSON != null)
            scanReportJSON.val = report;

        SetStatus("Scan complete");
        DebugLog("scan complete / " + report.Replace("\n", " / "));
    }

    private string FindPluginLabel(string pluginContains)
    {
        PluginHit hit;
        if (TryFindPlugin(pluginContains, out hit))
            return hit.atom.uid + " / " + hit.storableId;
        return "missing";
    }

    private bool RunQuick(string pluginContains, string[] actionNames, string label)
    {
        bool ok = TryTriggerAction(pluginContains, actionNames, label);
        if (!ok)
            SetStatus("Missing: " + label + " / " + pluginContains + " / " + JoinActions(actionNames));
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
                DebugLog("trigger / label=" + label + " / atom=" + hit.atom.uid + " / storable=" + hit.storableId + " / action=" + actionName);
                return true;
            }
            catch (Exception e)
            {
                SuperController.LogError("[HDU_Commander] action error: " + label + " / " + actionName + " / " + e);
                SetStatus("Action error: " + label + " / " + actionName);
                return false;
            }
        }

        DebugLog("action missing / plugin=" + pluginContains + " / label=" + label + " / actions=" + JoinActions(actionNames) + " / storable=" + hit.storableId);
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

        // TargetGrabber commands should operate the self-side plugin.
        // In normal use HDU is placed on Person#2 together with TargetGrabber, so do not accidentally
        // grab a TargetGrabber from the other Person just because it appears later in the scene.
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
