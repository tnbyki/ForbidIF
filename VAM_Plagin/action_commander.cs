// action_commander.cs
// v001_action_commander 2026-06-23
// HDU-like action commander for VaM: scans existing plugins and triggers their JSONStorableAction entries.
// Design: do not merge existing plugin logic; only call registered Actions from TargetGrabber / TargetLinePerson / HumanBodyAction etc.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class action_commander : MVRScript
{
    private const string VERSION = "v001_action_commander";
    private const string ANY = "ANY";

    private JSONStorableBool debugJSON;
    private JSONStorableBool autoScanOnStartJSON;
    private JSONStorableStringChooser atomScopeChooser;
    private JSONStorableString pluginContainsJSON;
    private JSONStorableString actionNameJSON;
    private JSONStorableFloat recipeDelayJSON;
    private JSONStorableString statusJSON;
    private JSONStorableString scanReportJSON;

    private Coroutine recipeRoutine;
    private bool recipeRunning = false;

    private string lastTargetGrabber = "missing";
    private string lastTargetLinePerson = "missing";
    private string lastHumanBodyAction = "missing";
    private string lastPoseChanger = "missing";
    private string lastClothStateSwitcher = "missing";

    private class CommandStep
    {
        public string label;
        public string pluginContains;
        public string[] actionNames;
        public float delayAfter;

        public CommandStep(string label, string pluginContains, string[] actionNames, float delayAfter)
        {
            this.label = label;
            this.pluginContains = pluginContains;
            this.actionNames = actionNames;
            this.delayAfter = delayAfter;
        }
    }

    public override void Init()
    {
        try
        {
            debugJSON = new JSONStorableBool("Debug", false);
            RegisterBool(debugJSON);
            CreateToggle(debugJSON, false);

            autoScanOnStartJSON = new JSONStorableBool("Auto Scan On Start", true);
            RegisterBool(autoScanOnStartJSON);
            CreateToggle(autoScanOnStartJSON, false);

            atomScopeChooser = new JSONStorableStringChooser(
                "Atom Scope",
                new List<string>(),
                ANY,
                "Atom Scope",
                (JSONStorableStringChooser.SetStringCallback)OnAtomScopeChanged
            );
            RegisterStringChooser(atomScopeChooser);
            UIDynamicPopup atomPopup = CreateFilterablePopup(atomScopeChooser, false);
            if (atomPopup != null)
                atomPopup.popup.onOpenPopupHandlers += UpdateAtomChoices;

            pluginContainsJSON = new JSONStorableString("Plugin Contains", "TargetGrabber");
            RegisterString(pluginContainsJSON);
            UIDynamicTextField pluginField = CreateTextField(pluginContainsJSON, false);
            if (pluginField != null)
                pluginField.height = 40.0f;

            actionNameJSON = new JSONStorableString("Action Name", "Grab Selected");
            RegisterString(actionNameJSON);
            UIDynamicTextField actionField = CreateTextField(actionNameJSON, false);
            if (actionField != null)
                actionField.height = 40.0f;

            UIDynamicButton scanButton = CreateButton("HDU Scan", false);
            if (scanButton != null)
                scanButton.button.onClick.AddListener(ScanPlugins);

            UIDynamicButton manualButton = CreateButton("Run Manual Action", false);
            if (manualButton != null)
                manualButton.button.onClick.AddListener(RunManualAction);

            recipeDelayJSON = new JSONStorableFloat("Recipe Step Delay", 0.15f, 0.00f, 2.00f, true, true);
            RegisterFloat(recipeDelayJSON);
            CreateSlider(recipeDelayJSON, false);

            statusJSON = new JSONStorableString("HDU Status", "Ready / " + VERSION);
            RegisterString(statusJSON);
            UIDynamicTextField statusField = CreateTextField(statusJSON, false);
            if (statusField != null)
                statusField.height = 80.0f;

            scanReportJSON = new JSONStorableString("HDU Scan Report", "Not scanned");
            RegisterString(scanReportJSON);
            UIDynamicTextField reportField = CreateTextField(scanReportJSON, false);
            if (reportField != null)
                reportField.height = 140.0f;

            CreateQuickButtons();
            RegisterExternalActions();

            UpdateAtomChoices();

            if (autoScanOnStartJSON != null && autoScanOnStartJSON.val)
                ScanPlugins();

            SetStatus("Ready / " + VERSION);
        }
        catch (Exception e)
        {
            SuperController.LogError("[action_commander] Init error: " + e);
        }
    }

    public override void OnDestroy()
    {
        StopRecipe();
    }

    private void CreateQuickButtons()
    {
        // Left: TargetGrabber quick operations.
        AddButton("TG Release", false, delegate { RunQuick("TargetGrabber", new string[] { "Release", "Self Release" }, "TG Release"); });
        AddButton("TG Target Defaults", false, delegate { RunQuick("TargetGrabber", new string[] { "Target Load User Defaults", "Target Load Defaults", "TargetLoadDefaults" }, "TG Target Defaults"); });
        AddButton("TG Self Defaults", false, delegate { RunQuick("TargetGrabber", new string[] { "Self Load User Defaults", "Load User Defaults", "LoadUserDefaults" }, "TG Self Defaults"); });
        AddButton("TG Hug Body", false, delegate { RunQuick("TargetGrabber", new string[] { "Target Shortcut Hug Body" }, "TG Hug Body"); });
        AddButton("TG Chest Hold", false, delegate { RunQuick("TargetGrabber", new string[] { "Target Shortcut Chest Hold" }, "TG Chest Hold"); });
        AddButton("TG Grab Selected", false, delegate { RunQuick("TargetGrabber", new string[] { "Grab Selected", "Grab Hand" }, "TG Grab Selected"); });
        AddButton("TG Pull", false, delegate { RunQuick("TargetGrabber", new string[] { "Grab Hand Pull", "Grab Pull" }, "TG Pull"); });
        AddButton("TG Push", false, delegate { RunQuick("TargetGrabber", new string[] { "Grab Hand Push", "Grab Push" }, "TG Push"); });
        AddButton("TG Open", false, delegate { RunQuick("TargetGrabber", new string[] { "Grab Hand Open" }, "TG Open"); });
        AddButton("TG Close", false, delegate { RunQuick("TargetGrabber", new string[] { "Grab Hand Close", "Grab Close" }, "TG Close"); });
        AddButton("TG Target Swoon", false, delegate { RunQuick("TargetGrabber", new string[] { "Target Swoon Drop", "Swoon Drop" }, "TG Target Swoon"); });

        // Right: TargetLinePerson / HumanBodyAction quick operations.
        AddButton("TLP Now Docking", true, delegate { RunQuick("TargetLinePerson", new string[] { "Now Docking", "Auto Docking" }, "TLP Now Docking"); });
        AddButton("TLP Smart Docking", true, delegate { RunQuick("TargetLinePerson", new string[] { "Smart Docking" }, "TLP Smart Docking"); });
        AddButton("TLP Reverse Smart", true, delegate { RunQuick("TargetLinePerson", new string[] { "Reverse Smart Docking" }, "TLP Reverse Smart"); });
        AddButton("TLP PUSH", true, delegate { RunQuick("TargetLinePerson", new string[] { "PUSH" }, "TLP PUSH"); });
        AddButton("TLP Pose Defaults", true, delegate { RunQuick("TargetLinePerson", new string[] { "Load Pose USER Defaults", "Load Pose User Defaults" }, "TLP Pose Defaults"); });

        AddButton("HBA Head Nod", true, delegate { RunQuick("HumanBodyAction", new string[] { "HBA_Head_Nod", "HBA_Head_QuickNod" }, "HBA Head Nod"); });
        AddButton("HBA Head Shake", true, delegate { RunQuick("HumanBodyAction", new string[] { "HBA_Head_Shake", "HBA_Head_IntenseShake" }, "HBA Head Shake"); });
        AddButton("HBA Twitch Normal", true, delegate { RunQuick("HumanBodyAction", new string[] { "HBA_Twitch_Normal", "HBA_Twitch_Weak" }, "HBA Twitch Normal"); });
        AddButton("HBA Reset", true, delegate { RunQuick("HumanBodyAction", new string[] { "HBA_Reset" }, "HBA Reset"); });

        AddButton("Recipe: Hug Body Grab", true, RecipeHugBodyGrab);
        AddButton("Recipe: Release Reset", true, RecipeReleaseReset);
        AddButton("Stop Recipe", true, StopRecipe);
    }

    private void RegisterExternalActions()
    {
        RegisterAction(new JSONStorableAction("HDU Scan", ScanPlugins));
        RegisterAction(new JSONStorableAction("HDU Run Manual Action", RunManualAction));
        RegisterAction(new JSONStorableAction("HDU Stop Recipe", StopRecipe));
        RegisterAction(new JSONStorableAction("HDU Recipe Hug Body Grab", RecipeHugBodyGrab));
        RegisterAction(new JSONStorableAction("HDU Recipe Release Reset", RecipeReleaseReset));

        RegisterAction(new JSONStorableAction("TG Release", delegate { RunQuick("TargetGrabber", new string[] { "Release", "Self Release" }, "TG Release"); }));
        RegisterAction(new JSONStorableAction("TG Hug Body", delegate { RunQuick("TargetGrabber", new string[] { "Target Shortcut Hug Body" }, "TG Hug Body"); }));
        RegisterAction(new JSONStorableAction("TG Chest Hold", delegate { RunQuick("TargetGrabber", new string[] { "Target Shortcut Chest Hold" }, "TG Chest Hold"); }));
        RegisterAction(new JSONStorableAction("TG Grab Selected", delegate { RunQuick("TargetGrabber", new string[] { "Grab Selected", "Grab Hand" }, "TG Grab Selected"); }));
        RegisterAction(new JSONStorableAction("TLP Now Docking", delegate { RunQuick("TargetLinePerson", new string[] { "Now Docking", "Auto Docking" }, "TLP Now Docking"); }));
        RegisterAction(new JSONStorableAction("TLP PUSH", delegate { RunQuick("TargetLinePerson", new string[] { "PUSH" }, "TLP PUSH"); }));
        RegisterAction(new JSONStorableAction("HBA Head Nod", delegate { RunQuick("HumanBodyAction", new string[] { "HBA_Head_Nod", "HBA_Head_QuickNod" }, "HBA Head Nod"); }));
    }

    private void AddButton(string label, bool rightSide, UnityEngine.Events.UnityAction callback)
    {
        UIDynamicButton button = CreateButton(label, rightSide);
        if (button != null)
            button.button.onClick.AddListener(callback);
    }

    private void OnAtomScopeChanged(string uid)
    {
        SetStatus("Atom Scope: " + uid);
    }

    private void UpdateAtomChoices()
    {
        if (atomScopeChooser == null)
            return;

        string current = atomScopeChooser.val;
        List<string> choices = new List<string>();
        choices.Add(ANY);

        try
        {
            List<Atom> atoms = SuperController.singleton.GetAtoms();
            if (atoms != null)
            {
                for (int i = 0; i < atoms.Count; i++)
                {
                    Atom atom = atoms[i];
                    if (atom == null || string.IsNullOrEmpty(atom.uid))
                        continue;
                    choices.Add(atom.uid);
                }
            }
        }
        catch (Exception e)
        {
            DebugLog("UpdateAtomChoices error: " + e.Message);
        }

        atomScopeChooser.choices = choices;
        if (!string.IsNullOrEmpty(current) && choices.Contains(current))
            atomScopeChooser.val = current;
        else
            atomScopeChooser.val = ANY;
    }

    private void ScanPlugins()
    {
        UpdateAtomChoices();

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

    private void RunManualAction()
    {
        string pluginContains = pluginContainsJSON != null ? pluginContainsJSON.val : "";
        string actionName = actionNameJSON != null ? actionNameJSON.val : "";

        if (string.IsNullOrEmpty(pluginContains) || string.IsNullOrEmpty(actionName))
        {
            SetStatus("Manual missing plugin/action text");
            return;
        }

        bool ok = TryTriggerAction(pluginContains, new string[] { actionName }, "Manual");
        if (!ok)
            SetStatus("Manual failed: " + pluginContains + " / " + actionName);
    }

    private void RunQuick(string pluginContains, string[] actionNames, string label)
    {
        bool ok = TryTriggerAction(pluginContains, actionNames, label);
        if (!ok)
            SetStatus("Missing: " + label + " / " + pluginContains + " / " + JoinActions(actionNames));
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
                SuperController.LogError("[action_commander] action error: " + label + " / " + actionName + " / " + e);
                SetStatus("Action error: " + label + " / " + actionName);
                return false;
            }
        }

        DebugLog("action missing / plugin=" + pluginContains + " / label=" + label + " / actions=" + JoinActions(actionNames) + " / storable=" + hit.storableId);
        return false;
    }

    private class PluginHit
    {
        public Atom atom;
        public string storableId;
        public JSONStorable storable;
    }

    private bool TryFindPlugin(string pluginContains, out PluginHit hit)
    {
        hit = null;
        if (string.IsNullOrEmpty(pluginContains))
            return false;

        List<Atom> atoms = BuildScopedAtomList();
        for (int i = 0; i < atoms.Count; i++)
        {
            Atom atom = atoms[i];
            if (atom == null)
                continue;

            List<string> ids = atom.GetStorableIDs();
            if (ids == null)
                continue;

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
        }

        return false;
    }

    private List<Atom> BuildScopedAtomList()
    {
        List<Atom> result = new List<Atom>();
        string scope = atomScopeChooser != null ? atomScopeChooser.val : ANY;

        if (!string.IsNullOrEmpty(scope) && scope != ANY)
        {
            Atom scoped = SuperController.singleton.GetAtomByUid(scope);
            if (scoped != null)
                result.Add(scoped);
            return result;
        }

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
            DebugLog("BuildScopedAtomList error: " + e.Message);
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

    private void RecipeHugBodyGrab()
    {
        CommandStep[] steps = new CommandStep[]
        {
            new CommandStep("TG Release", "TargetGrabber", new string[] { "Release", "Self Release" }, GetStepDelay()),
            new CommandStep("TG Target Defaults", "TargetGrabber", new string[] { "Target Load User Defaults", "Target Load Defaults", "TargetLoadDefaults" }, GetStepDelay()),
            new CommandStep("TG Hug Body", "TargetGrabber", new string[] { "Target Shortcut Hug Body" }, GetStepDelay()),
            new CommandStep("TG Grab Selected", "TargetGrabber", new string[] { "Grab Selected", "Grab Hand" }, 0.0f)
        };
        StartRecipe("Hug Body Grab", steps);
    }

    private void RecipeReleaseReset()
    {
        CommandStep[] steps = new CommandStep[]
        {
            new CommandStep("TG Release", "TargetGrabber", new string[] { "Release", "Self Release" }, GetStepDelay()),
            new CommandStep("TG Target Defaults", "TargetGrabber", new string[] { "Target Load User Defaults", "Target Load Defaults", "TargetLoadDefaults" }, GetStepDelay()),
            new CommandStep("TG Self Defaults", "TargetGrabber", new string[] { "Self Load User Defaults", "Load User Defaults", "LoadUserDefaults" }, GetStepDelay()),
            new CommandStep("HBA Reset", "HumanBodyAction", new string[] { "HBA_Reset" }, 0.0f)
        };
        StartRecipe("Release Reset", steps);
    }

    private float GetStepDelay()
    {
        if (recipeDelayJSON == null)
            return 0.15f;
        return Mathf.Clamp(recipeDelayJSON.val, 0.0f, 2.0f);
    }

    private void StartRecipe(string recipeName, CommandStep[] steps)
    {
        StopRecipe();
        if (steps == null || steps.Length == 0)
        {
            SetStatus("Recipe empty: " + recipeName);
            return;
        }

        recipeRoutine = StartCoroutine(RunRecipeRoutine(recipeName, steps));
    }

    private IEnumerator RunRecipeRoutine(string recipeName, CommandStep[] steps)
    {
        recipeRunning = true;
        SetStatus("Recipe start: " + recipeName);

        for (int i = 0; i < steps.Length; i++)
        {
            CommandStep step = steps[i];
            if (step == null)
                continue;

            bool ok = TryTriggerAction(step.pluginContains, step.actionNames, recipeName + " / " + step.label);
            if (!ok)
            {
                SetStatus("Recipe failed: " + recipeName + " / " + step.label);
                recipeRunning = false;
                recipeRoutine = null;
                yield break;
            }

            if (step.delayAfter > 0.0f)
                yield return new WaitForSeconds(step.delayAfter);
        }

        SetStatus("Recipe done: " + recipeName);
        recipeRunning = false;
        recipeRoutine = null;
    }

    private void StopRecipe()
    {
        if (recipeRoutine != null)
        {
            StopCoroutine(recipeRoutine);
            recipeRoutine = null;
        }

        if (recipeRunning)
            SetStatus("Recipe stopped");
        recipeRunning = false;
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
            SuperController.LogMessage("[action_commander] " + text);
    }
}
