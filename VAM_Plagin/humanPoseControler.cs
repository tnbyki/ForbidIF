// ============================================================
// humanControler.cs
// Version: v038_reapply_current_pose_action
// Date: 2026-06-20
// Base: humanControler_v036_turn_to_target_hdc_route.cs / HumanDrivenController_v103_external_enable_actions.cs
// Summary:
// - v018 makes Upper Low Leg <-> Upper Low Leg transitions direct.
// - v018 makes Upper Low Leg Front a full target pose, not a two-step Upper Low + leg-only route.
// - v018 skips no-op pose controls and fixes quaternion hemisphere before Slerp.
// - v018 uses a mild responsive pose curve to reduce the heavy feel while keeping duration.
// - v019 adds Sit full POSE button directly under Stand.
// - v020 hides left-side tuning sliders/toggle and keeps only Target Person + Status on the left.
// - v020 keeps all macro buttons on the right in the finalized order.
// - v021 adds Prone and Supine full POSE buttons.
// - v021 routes Prone through Upper Low and Supine through Upper Low Leg Front.
// - v022 replaces Supine with the corrected provided POSE.
// - v022 changes Prone/Supine UI labels and status text to English.
// - v023 adds Dog full POSE button and route support.
// - v024 adds a top Cycle Pose button and cycle route state.
// - v025 changes Cycle Pose order: Prone -> Dog direct, adds Back +45 step.
// - v026 changes Cycle Pose order: adds Sit/+45, -45 before Supine, +45 before Dog.
// - v027 changes Cycle Pose: Stand +45, adds Upper Low Leg Mji after Dog direct.
// - v028 adds Stand Hand Up full POSE and appends it to the Cycle Pose end.
// - v029 exposes buttons as external JSONStorableAction triggers with HC prefix.
// - v030 makes direct POSE transitions more responsive and shortens hidden route poses.
// - v031 makes pose response more aggressive and gates VaM log output behind Debug ON.
// - v032 moves Debug ON to the right column and avoids redundant HDC enable/capture work.
// - v033 makes hidden route poses leaner: quiet status, shorter duration, fewer redundant HDC writes.
// - v034 adds Upper StandMidl / HC Upper StandMidl: Upper Mid with Sit hip height.
// - v035 inserts Upper StandMidl into Cycle Pose immediately before Sit.
// - v036 renames Face/Back Target to Turn Front/Back To Target.
// - v036 reverts target turning to pre-v010 HDC Individual/control rotation.
// - v037 moves Debug ON to the left column under Status.
// - v037 adds Cycle Back and Cycle Reset buttons under Cycle Pose.
// - v037 keeps normal VaM LogMessage output gated behind Debug ON.
// - v038 adds HC Reapply Current Pose / HC Reapply Current Cycle Pose actions for HDU Target Release restoration.
// - Calls HumanDrivenController from a separate macro/control plugin.
// - Smoothly changes HDC Mode / TargetBone / Pos / Rot values.
// - Adds first macro buttons for basic pose transitions:
//   Upper Stand/Mid/Low, Upper X 0/90/-90, Turn Front/Back To Target,
// - Does not store pose files. It drives current state smoothly through HDC.
// - v002 default Upper Y values: Stand=1.0 / Mid=0.4 / Low=0.2.
// - v002 can automatically turn HDC Enable OFF after macro/cleanup to reduce LateUpdate load.
// - v004 keeps Upper Stand/Mid/Low as Hip-Upper PosY only.
// - v004 moves Upper Mid button to the right column.
// - v004 does NOT add foot lock and does NOT touch foot controls for simple Upper Y buttons.
// - v005 adds top Stand reset button.
// - v005 Upper Stand/Mid/Low first return Hip-Upper RotX to 0, then apply PosY.
// - v005 Stand reset returns Hip-Upper RotX=0, Hip-Lower RotX=0, then Hip-Upper PosY=StandY.
// - v005 places Upper X 0 below Upper X 90.
// - v006 moves the Stand button to the right column.
// - v006 reduces default Transition Time to 0.55.
// - v006 skips no-op phases so Upper Y does not wait through an already-zero Upper X phase.
// - v006 avoids repeated HDC Enable ON action / unchanged chooser callbacks during phases.
// - v007 moves every button to the right column.
// - v007 reorders buttons: Stand, Upper height, Upper X, compound, facing, utility.
// - v008 highlights the running macro button.
// - v008 adds Upper X 45 / Upper X -45 buttons.
// - v008 adds HipLower -90 button.
// - v009 adds top Load User Defaults button.
// - v009 changes Stand to apply the provided full POSE data smoothly.
// - v010 changes Face/Back Target to direct root yaw using mainController/control roots.
// - v010 adds Legs Forward / Legs Back direct knee+foot local position macros.
// - v011 changes Upper Low to the provided direct full POSE.
// - v011 changes Leg Front to the provided lower-body POSE for knee+foot controls.
// - v012 adds Low Mji full POSE.
// - v012 removes HipLower -90 and Low + compound macro buttons.
// - v013 updates Upper Low to the new provided full POSE.
// - v013 renames Leg Front to Upper Low Leg Front and routes Stand/Mid via Upper Low.
// - v013 renames Low Mji to Upper Low Leg Mji.
// - v013 removes the visible Legs Back button and leg calculation sliders.
// - v015 updates Upper Low POSE with corrected axis.
// - v016 updates Upper Low POSE to the flat/non-twist test pose.
// ============================================================

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class humanControler : MVRScript
{
    private const string MODE_PART = "Individual";
    private const string MODE_HIP_UPPER = "Hip-Upper";
    private const string MODE_HIP_LOWER = "Hip-Lower";

    private const string NONE_TARGET = "(none)";
    private const int CYCLE_POSE_COUNT = 24;

    private const string POSE_NONE = "";
    private const string POSE_STAND = "Stand";
    private const string POSE_STAND_HAND_UP = "StandHandUp";
    private const string POSE_SIT = "Sit";
    private const string POSE_UPPER_STAND = "UpperStand";
    private const string POSE_UPPER_MID = "UpperMid";
    private const string POSE_UPPER_STAND_MIDL = "UpperStandMidl";
    private const string POSE_UPPER_LOW = "UpperLow";
    private const string POSE_UPPER_LOW_LEG_FRONT = "UpperLowLegFront";
    private const string POSE_UPPER_LOW_LEG_MJI = "UpperLowLegMji";
    private const string POSE_PRONE = "Prone";
    private const string POSE_SUPINE = "Supine";
    private const string POSE_DOG = "Dog";

    private const float PHASE_POS_EPS = 0.0005f;
    private const float PHASE_ROT_EPS = 0.25f;
    private const float POSE_POS_EPS = 0.0005f;
    private const float POSE_ROT_EPS = 0.25f;
    private const float DEFAULT_TRANSITION_TIME = 0.30f;
    private const float POSE_EASE_OUT_BLEND = 0.82f;
    private const float ROUTE_POSE_TIME_SCALE = 0.20f;
    private const float SIT_HIP_LOCAL_Y = 0.626f;

    private JSONStorableFloat transitionTime;
    private JSONStorableFloat upperStandY;
    private JSONStorableFloat upperMidY;
    private JSONStorableFloat upperLowY;
    private JSONStorableFloat upperX45Deg;
    private JSONStorableFloat upperX90Deg;
    private JSONStorableFloat upperXMinus45Deg;
    private JSONStorableFloat upperXMinus90Deg;
    private JSONStorableFloat faceYawOffsetDeg;
    private JSONStorableFloat legForwardZ;
    private JSONStorableFloat legBackZ;
    private JSONStorableFloat legKneeRatio;
    private JSONStorableBool autoDisableHdcAfterMacro;
    private JSONStorableBool debugLog;
    private JSONStorableStringChooser targetPersonChooser;
    private JSONStorableString statusText;

    private JSONStorable hdc;
    private string hdcStorableId = "";
    private Coroutine activeRoutine;
    private string currentPoseKey = POSE_NONE;

    private List<string> targetChoices = new List<string>();
    private Dictionary<string, FreeControllerV3> controllerCache = new Dictionary<string, FreeControllerV3>();

    private class MacroButtonInfo
    {
        public string Label;
        public UIDynamicButton Button;
        public Graphic Graphic;
        public Color BaseGraphicColor;
        public ColorBlock BaseColors;
    }

    private Dictionary<string, MacroButtonInfo> macroButtons = new Dictionary<string, MacroButtonInfo>();
    private string activeMacroButtonLabel = "";
    private readonly Color runningButtonColor = new Color(0.45f, 0.95f, 0.45f, 1f);
    private int cyclePoseIndex = -1;

    private class SavedIKState
    {
        public bool Valid;
        public FreeControllerV3.PositionState PositionState;
        public FreeControllerV3.RotationState RotationState;
    }

    private class StandPoseEntry
    {
        public string ControlName;
        public Vector3 LocalPos;
        public Quaternion LocalRot;

        public StandPoseEntry(string controlName, float px, float py, float pz, float qx, float qy, float qz, float qw)
        {
            ControlName = controlName;
            LocalPos = new Vector3(px, py, pz);
            LocalRot = humanControler.NormalizeQuaternionStatic(new Quaternion(qx, qy, qz, qw));
        }
    }

    private class StandPoseSnapshot
    {
        public string ControlName;
        public FreeControllerV3 Controller;
        public Transform ControlTransform;
        public Vector3 StartLocalPos;
        public Quaternion StartLocalRot;
        public Vector3 TargetLocalPos;
        public Quaternion TargetLocalRot;
    }

    private class LegMoveSnapshot
    {
        public string ControlName;
        public FreeControllerV3 Controller;
        public Transform ControlTransform;
        public Vector3 StartLocalPos;
        public Vector3 TargetLocalPos;
    }

    private Dictionary<string, SavedIKState> savedFootIK = new Dictionary<string, SavedIKState>();
    private bool footIKSaved = false;

    public override void Init()
    {
        try
        {
            transitionTime = new JSONStorableFloat("Transition Time", DEFAULT_TRANSITION_TIME, 0.05f, 5.00f, true, true);
            upperStandY = new JSONStorableFloat("Upper Stand Y", 1.00f, -1.00f, 3.00f, true, true);
            upperMidY = new JSONStorableFloat("Upper Mid Y", 0.40f, -1.00f, 3.00f, true, true);
            upperLowY = new JSONStorableFloat("Upper Low Y", 0.20f, -1.00f, 3.00f, true, true);
            upperX45Deg = new JSONStorableFloat("Upper X 45 Deg", 45.0f, -180.0f, 180.0f, true, true);
            upperX90Deg = new JSONStorableFloat("Upper X 90 Deg", 90.0f, -180.0f, 180.0f, true, true);
            upperXMinus45Deg = new JSONStorableFloat("Upper X -45 Deg", -45.0f, -180.0f, 180.0f, true, true);
            upperXMinus90Deg = new JSONStorableFloat("Upper X -90 Deg", -90.0f, -180.0f, 180.0f, true, true);
            faceYawOffsetDeg = new JSONStorableFloat("Face Yaw Offset Deg", 0.0f, -180.0f, 180.0f, true, true);
            legForwardZ = new JSONStorableFloat("Leg Forward Z", 0.45f, 0.00f, 1.50f, true, true);
            legBackZ = new JSONStorableFloat("Leg Back Z", 0.35f, 0.00f, 1.50f, true, true);
            legKneeRatio = new JSONStorableFloat("Leg Knee Ratio", 0.50f, 0.00f, 1.00f, true, true);
            autoDisableHdcAfterMacro = new JSONStorableBool("Auto HDC OFF After Macro", true);
            debugLog = new JSONStorableBool("Debug ON", false);

            RegisterFloat(transitionTime);
            RegisterFloat(upperStandY);
            RegisterFloat(upperMidY);
            RegisterFloat(upperLowY);
            RegisterFloat(upperX45Deg);
            RegisterFloat(upperX90Deg);
            RegisterFloat(upperXMinus45Deg);
            RegisterFloat(upperXMinus90Deg);
            RegisterFloat(faceYawOffsetDeg);
            RegisterFloat(legForwardZ);
            RegisterFloat(legBackZ);
            RegisterFloat(legKneeRatio);
            RegisterBool(autoDisableHdcAfterMacro);
            RegisterBool(debugLog);

            RefreshTargetChoices(false);
            string defaultTarget = targetChoices.Count > 1 ? targetChoices[1] : NONE_TARGET;
            targetPersonChooser = new JSONStorableStringChooser("Target Person", targetChoices, defaultTarget, "Target Person", delegate(string value) { });
            RegisterStringChooser(targetPersonChooser);
            CreatePopup(targetPersonChooser);

            statusText = new JSONStorableString("humanControler Status", "ready");
            RegisterString(statusText);
            UIDynamicTextField tf = CreateTextField(statusText);
            if (tf != null) tf.height = 90f;

            // v037: Debug checkbox is kept on the left column under Status.
            CreateToggle(debugLog, false);

            // ============================================================
            // Buttons: all right column, ordered by practical workflow.
            // 1) Reset
            // 2) Stand/Sit bases
            // 3) Upper height / leg pose
            // 4) Facing
            // 5) Upper bend
            // 6) Utility
            // ============================================================

            CreateRightMacroButton("Cycle Pose", delegate()
            {
                StartMacro("Cycle Pose", MacroCyclePose());
            });

            CreateRightMacroButton("Cycle Back", delegate()
            {
                StartMacro("Cycle Back", MacroCycleBack());
            });

            CreateRightMacroButton("Cycle Reset", delegate()
            {
                StartMacro("Cycle Reset", MacroCycleReset());
            });

            CreateRightMacroButton("Load User Defaults", delegate()
            {
                StartMacro("Load User Defaults", MacroLoadUserDefaults());
            });

            CreateRightMacroButton("Stand", delegate()
            {
                StartMacro("Stand", MacroStandPose());
            });

            CreateRightMacroButton("Stand Hand Up", delegate()
            {
                StartMacro("Stand Hand Up", MacroStandHandUpPose());
            });

            CreateRightMacroButton("Sit", delegate()
            {
                StartMacro("Sit", MacroSitPose());
            });

            CreateRightMacroButton("Upper Stand", delegate()
            {
                StartMacro("Upper Stand", MacroUpperYWithUpperLowRoute("Upper Stand", upperStandY.val, POSE_UPPER_STAND));
            });

            CreateRightMacroButton("Upper Mid", delegate()
            {
                StartMacro("Upper Mid", MacroUpperYWithUpperLowRoute("Upper Mid", upperMidY.val, POSE_UPPER_MID));
            });

            CreateRightMacroButton("Upper StandMidl", delegate()
            {
                StartMacro("Upper StandMidl", MacroUpperStandMidl());
            });

            CreateRightMacroButton("Upper Low", delegate()
            {
                StartMacro("Upper Low", MacroUpperLowPose());
            });

            CreateRightMacroButton("Upper Low Leg Front", delegate()
            {
                StartMacro("Upper Low Leg Front", MacroUpperLowLegFrontPose());
            });

            CreateRightMacroButton("Upper Low Leg Mji", delegate()
            {
                StartMacro("Upper Low Leg Mji", MacroUpperLowLegMjiPose());
            });

            CreateRightMacroButton("Prone", delegate()
            {
                StartMacro("Prone", MacroPronePose());
            });

            CreateRightMacroButton("Supine", delegate()
            {
                StartMacro("Supine", MacroSupinePose());
            });

            CreateRightMacroButton("Dog", delegate()
            {
                StartMacro("Dog", MacroDogPose());
            });

            CreateRightMacroButton("Turn Front To Target", delegate()
            {
                StartMacro("Turn Front To Target", MacroFaceTarget(false));
            });

            CreateRightMacroButton("Turn Back To Target", delegate()
            {
                StartMacro("Turn Back To Target", MacroFaceTarget(true));
            });

            CreateRightMacroButton("Upper X 45", delegate()
            {
                StartMacro("Upper X 45", MacroUpperX("Upper X 45", upperX45Deg.val));
            });

            CreateRightMacroButton("Upper X 90", delegate()
            {
                StartMacro("Upper X 90", MacroUpperX("Upper X 90", upperX90Deg.val));
            });

            CreateRightMacroButton("Upper X 0", delegate()
            {
                StartMacro("Upper X 0", MacroUpperX("Upper X 0", 0.0f));
            });

            CreateRightMacroButton("Upper X -45", delegate()
            {
                StartMacro("Upper X -45", MacroUpperX("Upper X -45", upperXMinus45Deg.val));
            });

            CreateRightMacroButton("Upper X -90", delegate()
            {
                StartMacro("Upper X -90", MacroUpperX("Upper X -90", upperXMinus90Deg.val));
            });

            CreateRightMacroButton("Refresh HDC/Target", delegate()
            {
                ResolveHDC(true);
                RefreshTargetChoices(true);
            });

            CreateRightMacroButton("STOP / Cleanup", delegate()
            {
                StopMacroAndCleanup();
            });

            RegisterExternalActions();

            ResolveHDC(true);
        }
        catch (Exception e)
        {
            if (statusText != null)
                statusText.val = "Init error: " + e.Message;

            if (debugLog != null && debugLog.val)
                SuperController.LogError("[humanControler] Init error: " + e);
        }
    }

    private UIDynamicButton CreateRightMacroButton(string label, Action onClick)
    {
        UIDynamicButton ui = CreateButton(label, true);
        if (ui != null && ui.button != null)
        {
            ui.button.onClick.AddListener(delegate()
            {
                if (onClick != null)
                    onClick();
            });
        }

        RegisterMacroButton(label, ui);
        return ui;
    }

    private void RegisterExternalActions()
    {
        // External trigger names use an HC prefix so they are easy to find in VaM trigger lists.
        RegisterAction(new JSONStorableAction("HC Cycle Pose", delegate()
        {
            StartMacro("Cycle Pose", MacroCyclePose());
        }));

        RegisterAction(new JSONStorableAction("HC Cycle Back", delegate()
        {
            StartMacro("Cycle Back", MacroCycleBack());
        }));

        RegisterAction(new JSONStorableAction("HC Cycle Reset", delegate()
        {
            StartMacro("Cycle Reset", MacroCycleReset());
        }));

        RegisterAction(new JSONStorableAction("HC Reapply Current Pose", delegate()
        {
            StartMacro("Reapply Current Pose", MacroReapplyCurrentPose());
        }));

        RegisterAction(new JSONStorableAction("HC Reapply Current Cycle Pose", delegate()
        {
            StartMacro("Reapply Current Pose", MacroReapplyCurrentPose());
        }));

        RegisterAction(new JSONStorableAction("HC Load User Defaults", delegate()
        {
            StartMacro("Load User Defaults", MacroLoadUserDefaults());
        }));

        RegisterAction(new JSONStorableAction("HC Stand", delegate()
        {
            StartMacro("Stand", MacroStandPose());
        }));

        RegisterAction(new JSONStorableAction("HC Stand Hand Up", delegate()
        {
            StartMacro("Stand Hand Up", MacroStandHandUpPose());
        }));

        RegisterAction(new JSONStorableAction("HC Sit", delegate()
        {
            StartMacro("Sit", MacroSitPose());
        }));

        RegisterAction(new JSONStorableAction("HC Upper Stand", delegate()
        {
            StartMacro("Upper Stand", MacroUpperYWithUpperLowRoute("Upper Stand", upperStandY.val, POSE_UPPER_STAND));
        }));

        RegisterAction(new JSONStorableAction("HC Upper Mid", delegate()
        {
            StartMacro("Upper Mid", MacroUpperYWithUpperLowRoute("Upper Mid", upperMidY.val, POSE_UPPER_MID));
        }));

        RegisterAction(new JSONStorableAction("HC Upper StandMidl", delegate()
        {
            StartMacro("Upper StandMidl", MacroUpperStandMidl());
        }));

        RegisterAction(new JSONStorableAction("HC Upper Low", delegate()
        {
            StartMacro("Upper Low", MacroUpperLowPose());
        }));

        RegisterAction(new JSONStorableAction("HC Upper Low Leg Front", delegate()
        {
            StartMacro("Upper Low Leg Front", MacroUpperLowLegFrontPose());
        }));

        RegisterAction(new JSONStorableAction("HC Upper Low Leg Mji", delegate()
        {
            StartMacro("Upper Low Leg Mji", MacroUpperLowLegMjiPose());
        }));

        RegisterAction(new JSONStorableAction("HC Prone", delegate()
        {
            StartMacro("Prone", MacroPronePose());
        }));

        RegisterAction(new JSONStorableAction("HC Supine", delegate()
        {
            StartMacro("Supine", MacroSupinePose());
        }));

        RegisterAction(new JSONStorableAction("HC Dog", delegate()
        {
            StartMacro("Dog", MacroDogPose());
        }));

        RegisterAction(new JSONStorableAction("HC Turn Front To Target", delegate()
        {
            StartMacro("Turn Front To Target", MacroFaceTarget(false));
        }));

        RegisterAction(new JSONStorableAction("HC Turn Back To Target", delegate()
        {
            StartMacro("Turn Back To Target", MacroFaceTarget(true));
        }));

        RegisterAction(new JSONStorableAction("HC Upper X 45", delegate()
        {
            StartMacro("Upper X 45", MacroUpperX("Upper X 45", upperX45Deg.val));
        }));

        RegisterAction(new JSONStorableAction("HC Upper X 90", delegate()
        {
            StartMacro("Upper X 90", MacroUpperX("Upper X 90", upperX90Deg.val));
        }));

        RegisterAction(new JSONStorableAction("HC Upper X 0", delegate()
        {
            StartMacro("Upper X 0", MacroUpperX("Upper X 0", 0.0f));
        }));

        RegisterAction(new JSONStorableAction("HC Upper X -45", delegate()
        {
            StartMacro("Upper X -45", MacroUpperX("Upper X -45", upperXMinus45Deg.val));
        }));

        RegisterAction(new JSONStorableAction("HC Upper X -90", delegate()
        {
            StartMacro("Upper X -90", MacroUpperX("Upper X -90", upperXMinus90Deg.val));
        }));

        RegisterAction(new JSONStorableAction("HC Refresh HDC/Target", delegate()
        {
            ResolveHDC(true);
            RefreshTargetChoices(true);
        }));

        RegisterAction(new JSONStorableAction("HC STOP / Cleanup", delegate()
        {
            StopMacroAndCleanup();
        }));
    }

    private void RegisterMacroButton(string label, UIDynamicButton ui)
    {
        if (string.IsNullOrEmpty(label) || ui == null || ui.button == null)
            return;

        MacroButtonInfo info = new MacroButtonInfo();
        info.Label = label;
        info.Button = ui;
        info.BaseColors = ui.button.colors;
        info.Graphic = ui.button.targetGraphic;
        if (info.Graphic == null)
            info.Graphic = ui.button.GetComponent<Graphic>();
        if (info.Graphic != null)
            info.BaseGraphicColor = info.Graphic.color;

        macroButtons[label] = info;
    }

    private void SetActiveMacroButton(string label)
    {
        activeMacroButtonLabel = label ?? "";

        foreach (KeyValuePair<string, MacroButtonInfo> kv in macroButtons)
        {
            MacroButtonInfo info = kv.Value;
            if (info == null || info.Button == null || info.Button.button == null)
                continue;

            bool active = kv.Key == activeMacroButtonLabel;

            ColorBlock cb = info.BaseColors;
            if (active)
            {
                cb.normalColor = runningButtonColor;
                cb.highlightedColor = runningButtonColor;
                cb.pressedColor = runningButtonColor;
            }
            info.Button.button.colors = cb;

            if (info.Graphic != null)
                info.Graphic.color = active ? runningButtonColor : info.BaseGraphicColor;
        }
    }

    private void ClearActiveMacroButton()
    {
        activeMacroButtonLabel = "";

        foreach (KeyValuePair<string, MacroButtonInfo> kv in macroButtons)
        {
            MacroButtonInfo info = kv.Value;
            if (info == null || info.Button == null || info.Button.button == null)
                continue;

            info.Button.button.colors = info.BaseColors;
            if (info.Graphic != null)
                info.Graphic.color = info.BaseGraphicColor;
        }
    }

    private float GuessControllerLocalY(string controlName, float fallback)
    {
        FreeControllerV3 fc = FindController(controlName);
        if (fc != null && fc.control != null)
            return fc.control.localPosition.y;
        return fallback;
    }

    private void StartMacro(string buttonLabel, IEnumerator routine)
    {
        StopMacroInternal(false);
        SetActiveMacroButton(buttonLabel);
        ResolveHDC(false);
        activeRoutine = StartCoroutine(MacroWrapper(routine));
    }

    private IEnumerator MacroWrapper(IEnumerator routine)
    {
        yield return StartCoroutine(routine);
        activeRoutine = null;
        RestoreFootIKIfSaved();
        DisableHdcAfterMacroIfNeeded();
        ClearActiveMacroButton();
        SetStatus("macro done" + (ShouldAutoDisableHdc() ? " / HDC OFF" : ""));
    }

    private void StopMacroAndCleanup()
    {
        StopMacroInternal(true);
    }

    private void StopMacroInternal(bool log)
    {
        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
            activeRoutine = null;
        }

        RestoreFootIKIfSaved();
        InvokeHdcAction("HDC Capture Current", false);
        DisableHdcAfterMacroIfNeeded();
        ClearActiveMacroButton();

        if (log)
            SetStatus("stopped / cleanup" + (ShouldAutoDisableHdc() ? " / HDC OFF" : ""));
    }

    private IEnumerator MacroCyclePose()
    {
        cyclePoseIndex = (cyclePoseIndex + 1) % CYCLE_POSE_COUNT;
        yield return StartCoroutine(RunCyclePoseStep(cyclePoseIndex));
    }

    private IEnumerator MacroCycleBack()
    {
        if (cyclePoseIndex <= 0)
            cyclePoseIndex = CYCLE_POSE_COUNT - 1;
        else
            cyclePoseIndex = (cyclePoseIndex - 1) % CYCLE_POSE_COUNT;

        yield return StartCoroutine(RunCyclePoseStep(cyclePoseIndex));
    }

    private IEnumerator MacroCycleReset()
    {
        cyclePoseIndex = -1;
        SetStatus("Cycle Pose index reset");
        yield return null;
    }

    private IEnumerator MacroReapplyCurrentPose()
    {
        if (cyclePoseIndex >= 0 && cyclePoseIndex < CYCLE_POSE_COUNT)
        {
            yield return StartCoroutine(RunCyclePoseStepForReapply(cyclePoseIndex));
            yield break;
        }

        if (string.IsNullOrEmpty(currentPoseKey))
        {
            SetStatus("Reapply skipped: no current pose");
            yield return null;
            yield break;
        }

        SetStatus("Reapply Current Pose: " + currentPoseKey);
        yield return StartCoroutine(RunCurrentPoseKey(currentPoseKey));
    }

    private IEnumerator RunCyclePoseStepForReapply(int step)
    {
        SetStatus("Reapply Current Cycle Pose step=" + step.ToString(CultureInfo.InvariantCulture));

        switch (step)
        {
            case 1:
                // Stand +45 depends on the previous Stand step during normal cycle. Reapply must rebuild it.
                yield return StartCoroutine(MacroStandPose());
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                yield break;

            case 4:
                // Sit +45 depends on the previous Sit step during normal cycle. Reapply must rebuild it.
                yield return StartCoroutine(MacroSitPose());
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                yield break;

            case 8:
                // Upper X -45 depends on Upper Low Leg Front in the cycle route.
                yield return StartCoroutine(MacroUpperLowLegFrontPose());
                yield return StartCoroutine(MacroUpperX("Upper X -45", upperXMinus45Deg != null ? upperXMinus45Deg.val : -45f));
                yield break;

            case 11:
                // Upper X +45 depends on Upper Low in the cycle route.
                yield return StartCoroutine(MacroUpperLowPose());
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                yield break;

            case 20:
                // Turn Back +45 depends on the previous Turn Back step.
                yield return StartCoroutine(MacroFaceTarget(true));
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                yield break;

            case 21:
                // Turn Back +90 depends on the previous Turn Back step.
                yield return StartCoroutine(MacroFaceTarget(true));
                yield return StartCoroutine(MacroUpperX("Upper X 90", upperX90Deg != null ? upperX90Deg.val : 90f));
                yield break;
        }

        yield return StartCoroutine(RunCyclePoseStep(step));
    }

    private IEnumerator RunCurrentPoseKey(string poseKey)
    {
        if (poseKey == POSE_STAND)
        {
            yield return StartCoroutine(MacroStandPose());
            yield break;
        }
        if (poseKey == POSE_STAND_HAND_UP)
        {
            yield return StartCoroutine(MacroStandHandUpPose());
            yield break;
        }
        if (poseKey == POSE_SIT)
        {
            yield return StartCoroutine(MacroSitPose());
            yield break;
        }
        if (poseKey == POSE_UPPER_STAND)
        {
            yield return StartCoroutine(MacroUpperYWithUpperLowRoute("Upper Stand", upperStandY != null ? upperStandY.val : 1.00f, POSE_UPPER_STAND));
            yield break;
        }
        if (poseKey == POSE_UPPER_MID)
        {
            yield return StartCoroutine(MacroUpperYWithUpperLowRoute("Upper Mid", upperMidY != null ? upperMidY.val : 0.40f, POSE_UPPER_MID));
            yield break;
        }
        if (poseKey == POSE_UPPER_STAND_MIDL)
        {
            yield return StartCoroutine(MacroUpperStandMidl());
            yield break;
        }
        if (poseKey == POSE_UPPER_LOW)
        {
            yield return StartCoroutine(MacroUpperLowPose());
            yield break;
        }
        if (poseKey == POSE_UPPER_LOW_LEG_FRONT)
        {
            yield return StartCoroutine(MacroUpperLowLegFrontPose());
            yield break;
        }
        if (poseKey == POSE_UPPER_LOW_LEG_MJI)
        {
            yield return StartCoroutine(MacroUpperLowLegMjiPose());
            yield break;
        }
        if (poseKey == POSE_PRONE)
        {
            yield return StartCoroutine(MacroPronePose());
            yield break;
        }
        if (poseKey == POSE_SUPINE)
        {
            yield return StartCoroutine(MacroSupinePose());
            yield break;
        }
        if (poseKey == POSE_DOG)
        {
            yield return StartCoroutine(MacroDogPose());
            yield break;
        }

        SetStatus("Reapply skipped: unsupported pose=" + poseKey);
        yield return null;
    }

    private IEnumerator RunCyclePoseStep(int step)
    {
        SetStatus("Cycle Pose step=" + step);

        switch (step)
        {
            case 0:
                SetStatus("Cycle Pose: Stand");
                yield return StartCoroutine(MacroStandPose());
                break;

            case 1:
                SetStatus("Cycle Pose: Stand +45");
                if (currentPoseKey != POSE_STAND)
                    yield return StartCoroutine(MacroStandPose());
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                break;

            case 2:
                SetStatus("Cycle Pose: Upper StandMidl");
                yield return StartCoroutine(MacroUpperStandMidl());
                break;

            case 3:
                SetStatus("Cycle Pose: Upper X 0 + Sit");
                yield return StartCoroutine(MacroUpperX("Upper X 0", 0f));
                yield return StartCoroutine(MacroSitPose());
                break;

            case 4:
                SetStatus("Cycle Pose: Sit +45");
                if (currentPoseKey != POSE_SIT)
                    yield return StartCoroutine(MacroSitPose());
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                break;

            case 5:
                SetStatus("Cycle Pose: Upper X 0 + Upper Mid");
                yield return StartCoroutine(MacroUpperX("Upper X 0", 0f));
                yield return StartCoroutine(MacroUpperYWithUpperLowRoute("Upper Mid", upperMidY != null ? upperMidY.val : 0.40f, POSE_UPPER_MID));
                break;

            case 6:
                SetStatus("Cycle Pose: Upper Low");
                yield return StartCoroutine(MacroUpperLowPose());
                break;

            case 7:
                SetStatus("Cycle Pose: Upper Low Leg Front");
                yield return StartCoroutine(MacroUpperLowLegFrontPose());
                break;

            case 8:
                SetStatus("Cycle Pose: Upper X -45");
                yield return StartCoroutine(MacroUpperX("Upper X -45", upperXMinus45Deg != null ? upperXMinus45Deg.val : -45f));
                break;

            case 9:
                SetStatus("Cycle Pose: Supine direct");
                yield return StartCoroutine(MacroSupinePose());
                break;

            case 10:
                SetStatus("Cycle Pose: Upper Low");
                yield return StartCoroutine(MacroUpperLowPose());
                break;

            case 11:
                SetStatus("Cycle Pose: Upper X +45");
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                break;

            case 12:
                SetStatus("Cycle Pose: Dog");
                yield return StartCoroutine(MacroDogPose());
                break;

            case 13:
                SetStatus("Cycle Pose: Prone");
                yield return StartCoroutine(MacroPronePose());
                break;

            case 14:
                SetStatus("Cycle Pose: Prone -> Dog direct");
                yield return StartCoroutine(MacroDogPose());
                break;

            case 15:
                SetStatus("Cycle Pose: Upper Low");
                yield return StartCoroutine(MacroUpperLowPose());
                break;

            case 16:
                SetStatus("Cycle Pose: Upper Low Leg Mji");
                yield return StartCoroutine(MacroUpperLowLegMjiPose());
                break;

            case 17:
                SetStatus("Cycle Pose: Upper Mid");
                yield return StartCoroutine(MacroUpperYWithUpperLowRoute("Upper Mid", upperMidY != null ? upperMidY.val : 0.40f, POSE_UPPER_MID));
                break;

            case 18:
                SetStatus("Cycle Pose: Stand");
                yield return StartCoroutine(MacroStandPose());
                break;

            case 19:
                SetStatus("Cycle Pose: Turn Back To Target");
                yield return StartCoroutine(MacroFaceTarget(true));
                break;

            case 20:
                SetStatus("Cycle Pose: Turn Back +45");
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                break;

            case 21:
                SetStatus("Cycle Pose: Turn Back +90");
                yield return StartCoroutine(MacroUpperX("Upper X 90", upperX90Deg != null ? upperX90Deg.val : 90f));
                break;

            case 22:
                SetStatus("Cycle Pose: Turn Front + Stand");
                yield return StartCoroutine(MacroUpperX("Upper X 0", 0f));
                yield return StartCoroutine(MacroFaceTarget(false));
                yield return StartCoroutine(MacroStandPose());
                break;

            case 23:
                SetStatus("Cycle Pose: Stand Hand Up");
                yield return StartCoroutine(MacroStandHandUpPose());
                break;
        }
    }

    private IEnumerator MacroLoadUserDefaults()
    {
        SetStatus("Load User Defaults start");

        if (hdc == null && !ResolveHDC(false))
        {
            SetStatus("Load User Defaults failed: HDC not found");
            yield break;
        }

        SetHdcEnabled(true);
        SetHdcLiveApply(false);

        bool ok = InvokeHdcAction("HDC Load VaM User Defaults", true);
        if (!ok)
        {
            SetStatus("Load User Defaults failed: HDC action missing");
            yield break;
        }

        yield return null;
        yield return null;

        InvokeHdcAction("HDC Capture Current", false);
        currentPoseKey = POSE_NONE;
        SetStatus("Load User Defaults done");
    }

    private IEnumerator MacroStandPose()
    {
        SetStatus("Stand pose start");

        // This macro writes the POSE directly to VaM controls.
        // HDC Live Apply is disabled during the direct pose tween so it does not fight the pose.
        SetHdcLiveApply(false);
        SetHdcEnabled(false);

        List<StandPoseSnapshot> snapshots = BuildStandPoseSnapshots();
        if (snapshots.Count == 0)
        {
            SetStatus("Stand pose failed: no controls");
            yield break;
        }

        float dur = GetPoseTransitionDuration(true);
        float elapsed = 0f;

        while (elapsed < dur)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dur);
            ApplyStandPoseSnapshots(snapshots, t);
            yield return null;
        }

        ApplyStandPoseSnapshots(snapshots, 1f);

        yield return null;
        InvokeHdcAction("HDC Capture Current", false);
        currentPoseKey = POSE_STAND;
        SetStatus("Stand pose done");
    }

    private List<StandPoseSnapshot> BuildStandPoseSnapshots()
    {
        List<StandPoseSnapshot> list = new List<StandPoseSnapshot>();
        StandPoseEntry[] entries = GetStandPoseEntries();

        for (int i = 0; i < entries.Length; i++)
        {
            StandPoseEntry entry = entries[i];
            if (entry == null || string.IsNullOrEmpty(entry.ControlName))
                continue;

            FreeControllerV3 fc = FindController(entry.ControlName);
            if (fc == null || fc.control == null)
            {
                LogDebug("Stand pose control missing: " + entry.ControlName);
                continue;
            }

            try
            {
                fc.currentPositionState = FreeControllerV3.PositionState.On;
                fc.currentRotationState = FreeControllerV3.RotationState.On;
            }
            catch { }

            Vector3 startPos = fc.control.localPosition;
            Quaternion startRot = NormalizeQuaternion(fc.control.localRotation);
            Vector3 targetPos = entry.LocalPos;
            Quaternion targetRot = ClosestQuaternionToStart(startRot, entry.LocalRot);

            if (IsPoseAlreadyAtTarget(startPos, startRot, targetPos, targetRot))
                continue;

            StandPoseSnapshot snap = new StandPoseSnapshot();
            snap.ControlName = entry.ControlName;
            snap.Controller = fc;
            snap.ControlTransform = fc.control;
            snap.StartLocalPos = startPos;
            snap.StartLocalRot = startRot;
            snap.TargetLocalPos = targetPos;
            snap.TargetLocalRot = targetRot;

            list.Add(snap);
        }

        return list;
    }

    private void ApplyStandPoseSnapshots(List<StandPoseSnapshot> snapshots, float t)
    {
        if (snapshots == null) return;

        for (int i = 0; i < snapshots.Count; i++)
        {
            StandPoseSnapshot snap = snapshots[i];
            if (snap == null || snap.ControlTransform == null)
                continue;

            float mt = PoseMotionT(t);
            snap.ControlTransform.localPosition = Vector3.Lerp(snap.StartLocalPos, snap.TargetLocalPos, mt);
            snap.ControlTransform.localRotation = Quaternion.Slerp(snap.StartLocalRot, snap.TargetLocalRot, mt);
        }
    }

    private StandPoseEntry[] GetStandPoseEntries()
    {
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",    0.000f, 1.059f, -0.003f,  0.000f,  0.000f,  0.000f,  1.000f),
            new StandPoseEntry("chestControl", 0.000f, 1.273f, -0.034f,  0.001f,  0.000f,  0.000f, -1.000f),
            new StandPoseEntry("headControl",  0.000f, 1.622f, -0.020f,  0.009f,  0.000f,  0.000f,  1.000f),

            new StandPoseEntry("rHandControl",  0.293f, 0.932f, 0.045f,  0.207f,  0.213f,  0.640f, -0.709f),
            new StandPoseEntry("lHandControl", -0.277f, 0.927f, 0.050f,  0.197f, -0.215f, -0.635f, -0.716f),

            new StandPoseEntry("rFootControl",  0.109f, 0.064f, -0.027f,  0.161f,  0.124f,  0.000f,  0.979f),
            new StandPoseEntry("lFootControl", -0.109f, 0.064f, -0.027f,  0.161f, -0.124f,  0.000f,  0.979f),

            new StandPoseEntry("rKneeControl",  0.109f, 0.507f, 0.076f,  0.108f,  0.051f,  0.001f,  0.993f),
            new StandPoseEntry("lKneeControl", -0.109f, 0.507f, 0.076f,  0.107f, -0.051f, -0.001f,  0.993f),

            new StandPoseEntry("rElbowControl",  0.241f, 1.149f, -0.048f, -0.075f, -0.198f, -0.599f,  0.772f),
            new StandPoseEntry("lElbowControl", -0.233f, 1.145f, -0.043f, -0.070f,  0.205f,  0.608f,  0.764f)
        };
    }

    private StandPoseEntry[] GetStandHandUpPoseEntries()
    {
        // Full-body Stand Hand Up pose provided by user. Added in v028.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",     0.000f, 1.059f, -0.003f,  0.000f,  0.040f,  0.000f,  0.999f),
            new StandPoseEntry("chestControl", -0.003f, 1.273f, -0.034f, -0.001f,  0.040f,  0.000f,  0.999f),
            new StandPoseEntry("headControl",  -0.002f, 1.622f, -0.020f, -0.009f, -0.040f,  0.000f, -0.999f),

            new StandPoseEntry("rHandControl",  0.019f, 1.857f,  0.040f, -0.863f, -0.309f, -0.002f,  0.399f),
            new StandPoseEntry("lHandControl", -0.055f, 1.872f,  0.054f, -0.860f,  0.340f,  0.070f,  0.374f),

            new StandPoseEntry("rFootControl",  0.110f, 0.046f, -0.009f,  0.000f,  0.000f,  0.000f,  1.000f),
            new StandPoseEntry("lFootControl", -0.110f, 0.045f, -0.009f,  0.000f,  0.000f,  0.000f, -1.000f),

            new StandPoseEntry("rKneeControl",  0.116f, 0.506f,  0.043f,  0.031f,  0.129f,  0.004f,  0.991f),
            new StandPoseEntry("lKneeControl", -0.108f, 0.506f,  0.061f,  0.031f, -0.050f, -0.007f,  0.998f),

            new StandPoseEntry("rElbowControl",  0.167f, 1.694f,  0.140f,  0.427f,  0.797f, -0.280f, -0.322f),
            new StandPoseEntry("lElbowControl", -0.143f, 1.694f,  0.165f,  0.448f, -0.820f,  0.245f, -0.257f)
        };
    }

    private StandPoseEntry[] GetSitPoseEntries()
    {
        // Full-body Sit pose provided by user. Added in v019.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",     0.000f, 0.626f, -0.003f,  0.000f,  0.000f,  0.000f,  1.000f),
            new StandPoseEntry("chestControl",  0.000f, 0.840f, -0.034f,  0.001f,  0.000f,  0.000f, -1.000f),
            new StandPoseEntry("headControl",   0.000f, 1.189f, -0.020f,  0.009f,  0.000f,  0.000f,  1.000f),

            new StandPoseEntry("rHandControl",  0.293f, 0.499f,  0.045f,  0.207f,  0.213f,  0.640f, -0.709f),
            new StandPoseEntry("lHandControl", -0.277f, 0.494f,  0.050f,  0.197f, -0.215f, -0.635f, -0.716f),

            new StandPoseEntry("rFootControl",  0.101f, 0.032f, 0.303f, -0.069f,  0.101f,  0.004f, -0.992f),
            new StandPoseEntry("lFootControl", -0.101f, 0.032f, 0.303f, -0.069f, -0.101f, -0.004f, -0.992f),

            new StandPoseEntry("rKneeControl",  0.084f, 0.447f, 0.463f,  0.165f, -0.094f, -0.004f,  0.982f),
            new StandPoseEntry("lKneeControl", -0.084f, 0.447f, 0.463f,  0.165f,  0.094f,  0.004f,  0.982f),

            new StandPoseEntry("rElbowControl",  0.241f, 0.716f, -0.048f, -0.075f, -0.198f, -0.599f,  0.772f),
            new StandPoseEntry("lElbowControl", -0.233f, 0.712f, -0.043f, -0.070f,  0.205f,  0.608f,  0.764f)
        };
    }

    private StandPoseEntry[] GetUpperLowPoseEntries()
    {
        // Full-body Upper Low pose provided by user. Updated in v017.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",     0.000f, 0.204f, -0.003f,  0.000f, -0.044f,  0.000f,  0.999f),
            new StandPoseEntry("chestControl",  0.003f, 0.418f, -0.034f,  0.001f,  0.044f,  0.000f, -0.999f),
            new StandPoseEntry("headControl",   0.002f, 0.767f, -0.020f,  0.009f, -0.044f,  0.000f,  0.999f),

            new StandPoseEntry("rHandControl",  0.283f, 0.101f,  0.041f, -0.181f, -0.195f, -0.631f,  0.729f),
            new StandPoseEntry("lHandControl", -0.286f, 0.101f, -0.009f, -0.235f,  0.131f,  0.613f,  0.743f),

            new StandPoseEntry("rFootControl",  0.210f, 0.006f, -0.028f,  0.812f,  0.232f,  0.354f,  0.403f),
            new StandPoseEntry("lFootControl", -0.201f, 0.006f, -0.064f,  0.839f, -0.266f, -0.282f,  0.381f),

            new StandPoseEntry("rKneeControl",  0.208f, 0.036f,  0.437f, -0.031f, -0.050f, -0.007f, -0.998f),
            new StandPoseEntry("lKneeControl", -0.281f, 0.036f,  0.394f, -0.031f,  0.137f,  0.004f, -0.990f),

            new StandPoseEntry("rElbowControl",  0.237f, 0.310f, -0.044f, -0.001f,  0.229f,  0.001f, -0.973f),
            new StandPoseEntry("lElbowControl", -0.226f, 0.310f, -0.085f,  0.000f, -0.143f, -0.001f, -0.990f)
        };
    }

    private StandPoseEntry[] GetUpperLowLegMjiPoseEntries()
    {
        // Full-body Upper Low Leg Mji pose provided by user.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",    0.000f, 0.200f, -0.003f, -0.643f,  0.000f,  0.000f,  0.766f),
            new StandPoseEntry("chestControl", 0.000f, 0.410f, -0.128f,  0.001f,  0.000f,  0.000f, -1.000f),
            new StandPoseEntry("headControl",  0.000f, 0.729f, -0.154f,  0.009f,  0.000f,  0.000f,  1.000f),

            new StandPoseEntry("rHandControl",  0.304f, 0.073f, 0.049f, -0.189f, -0.216f, -0.624f,  0.727f),
            new StandPoseEntry("lHandControl", -0.304f, 0.073f, 0.048f, -0.189f,  0.216f,  0.624f,  0.727f),

            new StandPoseEntry("rFootControl",  0.181f, 0.062f, 0.186f,  0.026f,  0.102f, -0.128f,  0.986f),
            new StandPoseEntry("lFootControl", -0.181f, 0.062f, 0.186f,  0.000f,  0.000f,  0.000f, -1.000f),

            new StandPoseEntry("rKneeControl",  0.262f, 0.499f, 0.047f, -0.031f, -0.050f, -0.007f, -0.998f),
            new StandPoseEntry("lKneeControl", -0.262f, 0.499f, 0.048f, -0.031f,  0.050f,  0.007f, -0.998f),

            new StandPoseEntry("rElbowControl",  0.257f, 0.264f, -0.069f, -0.001f,  0.229f,  0.001f, -0.973f),
            new StandPoseEntry("lElbowControl", -0.257f, 0.264f, -0.070f, -0.001f, -0.229f, -0.001f, -0.973f)
        };
    }

    private StandPoseEntry[] GetUpperLowLegFrontPoseEntries()
    {
        // Full-body Upper Low Leg Front target.
        // Upper body uses the current Upper Low base; knee+foot uses the provided Leg Front pose.
        // This allows Upper Low Leg Front <-> Upper Low Leg Mji to move directly without an Upper Low detour.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",     0.000f, 0.204f, -0.003f,  0.000f, -0.044f,  0.000f,  0.999f),
            new StandPoseEntry("chestControl",  0.003f, 0.418f, -0.034f,  0.001f,  0.044f,  0.000f, -0.999f),
            new StandPoseEntry("headControl",   0.002f, 0.767f, -0.020f,  0.009f, -0.044f,  0.000f,  0.999f),

            new StandPoseEntry("rHandControl",  0.283f, 0.101f,  0.041f, -0.181f, -0.195f, -0.631f,  0.729f),
            new StandPoseEntry("lHandControl", -0.286f, 0.101f, -0.009f, -0.235f,  0.131f,  0.613f,  0.743f),

            new StandPoseEntry("rFootControl",  0.438f, 0.048f, 0.848f,  0.138f,  0.124f,  0.003f, 0.983f),
            new StandPoseEntry("lFootControl", -0.438f, 0.048f, 0.848f,  0.138f, -0.124f, -0.003f, 0.983f),
            new StandPoseEntry("rKneeControl", -0.027f, 0.327f, 0.258f,  0.108f,  0.051f,  0.001f, 0.993f),
            new StandPoseEntry("lKneeControl",  0.027f, 0.327f, 0.258f,  0.108f, -0.051f, -0.001f, 0.993f),

            new StandPoseEntry("rElbowControl",  0.237f, 0.310f, -0.044f, -0.001f,  0.229f,  0.001f, -0.973f),
            new StandPoseEntry("lElbowControl", -0.226f, 0.310f, -0.085f,  0.000f, -0.143f, -0.001f, -0.990f)
        };
    }

    private StandPoseEntry[] GetDogPoseEntries()
    {
        // Dog full-body pose provided by user. Intended route: Upper Low -> Dog.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",    -0.001f, 0.600f, -0.006f, -0.704f, -0.062f,  0.062f, -0.704f),
            new StandPoseEntry("chestControl",  0.058f, 0.386f,  0.324f, -0.284f, -0.084f,  0.025f, -0.955f),
            new StandPoseEntry("headControl",   0.111f, 0.096f,  0.625f, -0.147f,  0.087f,  0.013f,  0.985f),

            new StandPoseEntry("rHandControl",  0.153f, 0.066f,  0.801f,  0.000f, -0.441f,  0.000f,  0.897f),
            new StandPoseEntry("lHandControl", -0.131f, 0.050f,  0.761f,  0.000f,  0.353f,  0.000f,  0.936f),

            new StandPoseEntry("rFootControl",  0.274f, 0.066f, -0.369f, -0.940f, -0.337f, -0.009f, -0.059f),
            new StandPoseEntry("lFootControl", -0.280f,-0.015f, -0.245f, -0.857f,  0.213f,  0.325f, -0.340f),

            new StandPoseEntry("rKneeControl",  0.296f,-0.047f,  0.071f, -0.783f, -0.167f, -0.149f, -0.581f),
            new StandPoseEntry("lKneeControl", -0.304f, 0.111f,  0.197f, -0.559f,  0.207f,  0.101f, -0.796f),

            new StandPoseEntry("rElbowControl",  0.188f,-0.073f,  0.198f, -0.165f,  0.417f,  0.662f, -0.601f),
            new StandPoseEntry("lElbowControl", -0.164f,-0.057f,  0.305f, -0.206f, -0.430f, -0.601f, -0.641f)
        };
    }

    private StandPoseEntry[] GetPronePoseEntries()
    {
        // Prone full-body pose provided by user. Intended route: Upper Low -> Prone.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",    -0.011f, 0.226f, -0.059f,  0.704f,  0.062f, -0.062f,  0.704f),
            new StandPoseEntry("chestControl",  0.022f, 0.259f,  0.125f,  0.748f,  0.058f, -0.066f,  0.658f),
            new StandPoseEntry("headControl",   0.087f, 0.207f,  0.488f, -0.711f, -0.062f,  0.063f, -0.698f),

            new StandPoseEntry("rHandControl",  0.163f, 0.265f, -0.290f, -0.425f, -0.334f,  0.523f, -0.659f),
            new StandPoseEntry("lHandControl", -0.253f, 0.265f, -0.216f, -0.510f,  0.213f, -0.441f, -0.707f),

            new StandPoseEntry("rFootControl", -0.176f, 0.203f, -1.155f, -0.927f, -0.358f, -0.112f,  0.025f),
            new StandPoseEntry("lFootControl", -0.234f, 0.203f, -1.145f, -0.893f,  0.357f,  0.273f, -0.038f),

            new StandPoseEntry("rKneeControl",  0.004f, 0.154f, -0.602f, -0.795f, -0.219f, -0.063f, -0.562f),
            new StandPoseEntry("lKneeControl", -0.211f, 0.154f, -0.563f, -0.772f,  0.117f,  0.201f, -0.592f),

            new StandPoseEntry("rElbowControl",  0.180f, 0.226f, -0.031f,  0.570f,  0.446f, -0.534f,  0.437f),
            new StandPoseEntry("lElbowControl", -0.180f, 0.226f,  0.033f,  0.655f, -0.362f,  0.426f,  0.509f)
        };
    }

    private StandPoseEntry[] GetSupinePoseEntries()
    {
        // Supine full-body pose provided by user. Intended route: Upper Low Leg Front -> Supine.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",     0.010f, 0.220f,  0.054f,  0.704f, -0.062f, -0.062f, -0.704f),
            new StandPoseEntry("chestControl", -0.050f, 0.135f, -0.279f,  0.704f, -0.062f, -0.062f, -0.705f),
            new StandPoseEntry("headControl",  -0.108f, 0.149f, -0.609f, -0.698f,  0.063f,  0.062f,  0.711f),

            new StandPoseEntry("rHandControl",  0.269f, 0.121f,  0.141f, -0.652f, -0.567f, -0.347f,  0.365f),
            new StandPoseEntry("lHandControl", -0.204f, 0.121f,  0.225f, -0.580f,  0.622f,  0.456f,  0.260f),

            new StandPoseEntry("rFootControl",  0.237f, 0.171f,  0.907f, -0.049f, -0.123f, -0.032f, -0.991f),
            new StandPoseEntry("lFootControl",  0.091f, 0.171f,  0.933f, -0.042f, -0.053f,  0.040f, -0.997f),

            new StandPoseEntry("rKneeControl",  0.106f, 0.189f,  0.445f,  0.466f, -0.020f, -0.182f, -0.866f),
            new StandPoseEntry("lKneeControl",  0.054f, 0.189f,  0.455f,  0.490f, -0.132f,  0.098f, -0.856f),

            new StandPoseEntry("rElbowControl",  0.228f, 0.141f, -0.103f,  0.579f,  0.529f,  0.264f, -0.562f),
            new StandPoseEntry("lElbowControl", -0.249f, 0.141f, -0.019f,  0.524f, -0.619f, -0.361f, -0.460f)
        };
    }

    private static Quaternion NormalizeQuaternionStatic(Quaternion q)
    {
        float m = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
        if (m <= 0.000001f)
            return Quaternion.identity;

        return new Quaternion(q.x / m, q.y / m, q.z / m, q.w / m);
    }

    private Quaternion NormalizeQuaternion(Quaternion q)
    {
        return NormalizeQuaternionStatic(q);
    }

    private Quaternion ClosestQuaternionToStart(Quaternion startRot, Quaternion targetRot)
    {
        startRot = NormalizeQuaternion(startRot);
        targetRot = NormalizeQuaternion(targetRot);

        if (Quaternion.Dot(startRot, targetRot) < 0f)
            return new Quaternion(-targetRot.x, -targetRot.y, -targetRot.z, -targetRot.w);

        return targetRot;
    }

    private bool IsPoseAlreadyAtTarget(Vector3 startPos, Quaternion startRot, Vector3 targetPos, Quaternion targetRot)
    {
        if (Vector3.Distance(startPos, targetPos) > POSE_POS_EPS)
            return false;

        if (Quaternion.Angle(startRot, targetRot) > POSE_ROT_EPS)
            return false;

        return true;
    }

    private float PoseMotionT(float t)
    {
        t = Mathf.Clamp01(t);
        float easeOut = 1f - ((1f - t) * (1f - t));
        return Mathf.Lerp(t, easeOut, POSE_EASE_OUT_BLEND);
    }

    private float GetTransitionDuration()
    {
        return Mathf.Max(0.01f, transitionTime != null ? transitionTime.val : DEFAULT_TRANSITION_TIME);
    }

    private float GetPoseTransitionDuration(bool captureAtEnd)
    {
        float dur = GetTransitionDuration();
        if (!captureAtEnd)
            dur *= ROUTE_POSE_TIME_SCALE;
        return Mathf.Max(0.01f, dur);
    }

    private IEnumerator MacroStandHandUpPose()
    {
        yield return StartCoroutine(MacroDirectPoseEntries("Stand Hand Up", GetStandHandUpPoseEntries()));
        currentPoseKey = POSE_STAND_HAND_UP;
    }

    private IEnumerator MacroSitPose()
    {
        yield return StartCoroutine(MacroDirectPoseEntries("Sit", GetSitPoseEntries()));
        currentPoseKey = POSE_SIT;
    }

    private IEnumerator MacroUpperLowPose()
    {
        yield return StartCoroutine(MacroDirectPoseEntries("Upper Low", GetUpperLowPoseEntries()));
        currentPoseKey = POSE_UPPER_LOW;
    }

    private IEnumerator MacroUpperLowLegMjiPose()
    {
        yield return StartCoroutine(MacroDirectPoseEntries("Upper Low Leg Mji", GetUpperLowLegMjiPoseEntries()));
        currentPoseKey = POSE_UPPER_LOW_LEG_MJI;
    }

    private IEnumerator MacroUpperLowLegFrontPose()
    {
        yield return StartCoroutine(MacroDirectPoseEntries("Upper Low Leg Front", GetUpperLowLegFrontPoseEntries()));
        currentPoseKey = POSE_UPPER_LOW_LEG_FRONT;
    }

    private IEnumerator MacroPronePose()
    {
        if (currentPoseKey != POSE_UPPER_LOW && currentPoseKey != POSE_DOG)
        {
            if (currentPoseKey == POSE_SUPINE)
            {
                yield return StartCoroutine(MacroRoutePoseEntries("Upper Low Leg Front", GetUpperLowLegFrontPoseEntries()));
                currentPoseKey = POSE_UPPER_LOW_LEG_FRONT;
            }

            yield return StartCoroutine(MacroRoutePoseEntries("Upper Low", GetUpperLowPoseEntries()));
            currentPoseKey = POSE_UPPER_LOW;
        }

        yield return StartCoroutine(MacroDirectPoseEntries("Prone", GetPronePoseEntries()));
        currentPoseKey = POSE_PRONE;
    }

    private IEnumerator MacroSupinePose()
    {
        if (currentPoseKey != POSE_UPPER_LOW_LEG_FRONT)
        {
            yield return StartCoroutine(MacroRoutePoseEntries("Upper Low Leg Front", GetUpperLowLegFrontPoseEntries()));
            currentPoseKey = POSE_UPPER_LOW_LEG_FRONT;
        }

        yield return StartCoroutine(MacroDirectPoseEntries("Supine", GetSupinePoseEntries()));
        currentPoseKey = POSE_SUPINE;
    }

    private IEnumerator MacroDogPose()
    {
        if (currentPoseKey == POSE_PRONE)
        {
            yield return StartCoroutine(MacroDirectPoseEntries("Dog", GetDogPoseEntries()));
            currentPoseKey = POSE_DOG;
            yield break;
        }

        if (currentPoseKey == POSE_SUPINE)
        {
            yield return StartCoroutine(MacroRoutePoseEntries("Upper Low Leg Front", GetUpperLowLegFrontPoseEntries()));
            currentPoseKey = POSE_UPPER_LOW_LEG_FRONT;
        }

        if (currentPoseKey != POSE_UPPER_LOW)
        {
            yield return StartCoroutine(MacroRoutePoseEntries("Upper Low", GetUpperLowPoseEntries()));
            currentPoseKey = POSE_UPPER_LOW;
        }

        yield return StartCoroutine(MacroDirectPoseEntries("Dog", GetDogPoseEntries()));
        currentPoseKey = POSE_DOG;
    }

    private IEnumerator MacroDirectPoseEntries(string label, StandPoseEntry[] entries)
    {
        yield return StartCoroutine(MacroDirectPoseEntries(label, entries, true));
    }

    private IEnumerator MacroRoutePoseEntries(string label, StandPoseEntry[] entries)
    {
        yield return StartCoroutine(MacroDirectPoseEntries(label, entries, false, true));
    }

    private IEnumerator MacroDirectPoseEntries(string label, StandPoseEntry[] entries, bool captureAtEnd)
    {
        yield return StartCoroutine(MacroDirectPoseEntries(label, entries, captureAtEnd, false));
    }

    private IEnumerator MacroDirectPoseEntries(string label, StandPoseEntry[] entries, bool captureAtEnd, bool quiet)
    {
        if (!quiet)
            SetStatus(label + " pose start");

        // Direct pose writes should not fight HDC's LateUpdate apply loop.
        SetHdcLiveApply(false);
        SetHdcEnabled(false);

        List<StandPoseSnapshot> snapshots = BuildPoseSnapshots(entries, label);
        if (snapshots.Count == 0)
        {
            if (!quiet)
                SetStatus(label + " pose already at target");
            yield break;
        }

        float dur = GetPoseTransitionDuration(captureAtEnd);
        float elapsed = 0f;

        while (elapsed < dur)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dur);
            ApplyPoseSnapshots(snapshots, PoseMotionT(t));
            yield return null;
        }

        ApplyPoseSnapshots(snapshots, 1f);

        if (captureAtEnd)
        {
            yield return null;
            InvokeHdcAction("HDC Capture Current", false);
        }
        if (!quiet)
            SetStatus(label + " pose done");
    }

    private List<StandPoseSnapshot> BuildPoseSnapshots(StandPoseEntry[] entries, string label)
    {
        List<StandPoseSnapshot> list = new List<StandPoseSnapshot>();
        if (entries == null)
            return list;

        for (int i = 0; i < entries.Length; i++)
        {
            StandPoseEntry entry = entries[i];
            if (entry == null || string.IsNullOrEmpty(entry.ControlName))
                continue;

            FreeControllerV3 fc = FindController(entry.ControlName);
            if (fc == null || fc.control == null)
            {
                LogDebug(label + " pose control missing: " + entry.ControlName);
                continue;
            }

            try
            {
                fc.currentPositionState = FreeControllerV3.PositionState.On;
                fc.currentRotationState = FreeControllerV3.RotationState.On;
            }
            catch { }

            Vector3 startPos = fc.control.localPosition;
            Quaternion startRot = NormalizeQuaternion(fc.control.localRotation);
            Vector3 targetPos = entry.LocalPos;
            Quaternion targetRot = ClosestQuaternionToStart(startRot, entry.LocalRot);

            if (IsPoseAlreadyAtTarget(startPos, startRot, targetPos, targetRot))
                continue;

            StandPoseSnapshot snap = new StandPoseSnapshot();
            snap.ControlName = entry.ControlName;
            snap.Controller = fc;
            snap.ControlTransform = fc.control;
            snap.StartLocalPos = startPos;
            snap.StartLocalRot = startRot;
            snap.TargetLocalPos = targetPos;
            snap.TargetLocalRot = targetRot;
            list.Add(snap);
        }

        return list;
    }

    private void ApplyPoseSnapshots(List<StandPoseSnapshot> snapshots, float t)
    {
        if (snapshots == null) return;

        for (int i = 0; i < snapshots.Count; i++)
        {
            StandPoseSnapshot snap = snapshots[i];
            if (snap == null || snap.ControlTransform == null)
                continue;

            snap.ControlTransform.localPosition = Vector3.Lerp(snap.StartLocalPos, snap.TargetLocalPos, t);
            snap.ControlTransform.localRotation = Quaternion.Slerp(snap.StartLocalRot, snap.TargetLocalRot, t);
        }
    }

    private IEnumerator MacroUpperStandMidl()
    {
        // Upper Mid behavior with the hipControl Y height taken from the Sit pose.
        // Route out of low/lying poses first, then set only the hip height, then run the Upper Mid HDC phase.
        if (currentPoseKey == POSE_SUPINE)
        {
            yield return StartCoroutine(MacroRoutePoseEntries("Upper Low Leg Front", GetUpperLowLegFrontPoseEntries()));
            currentPoseKey = POSE_UPPER_LOW_LEG_FRONT;
        }

        if (currentPoseKey == POSE_PRONE || currentPoseKey == POSE_DOG || currentPoseKey == POSE_UPPER_LOW_LEG_FRONT || currentPoseKey == POSE_UPPER_LOW_LEG_MJI)
        {
            yield return StartCoroutine(MacroRoutePoseEntries("Upper Low", GetUpperLowPoseEntries()));
            currentPoseKey = POSE_UPPER_LOW;
        }

        yield return StartCoroutine(MacroHipLocalYOnly("Sit Hip Height", SIT_HIP_LOCAL_Y, false));
        yield return StartCoroutine(MacroUpperY("Upper StandMidl", upperMidY != null ? upperMidY.val : 0.40f));
        currentPoseKey = POSE_UPPER_STAND_MIDL;
    }

    private IEnumerator MacroHipLocalYOnly(string label, float targetY, bool captureAtEnd)
    {
        if (!captureAtEnd)
            LogDebug(label + " route start Y=" + F(targetY));
        else
            SetStatus(label + " start Y=" + F(targetY));

        SetHdcLiveApply(false);
        SetHdcEnabled(false);

        FreeControllerV3 hip = FindController("hipControl");
        if (hip == null || hip.control == null)
        {
            SetStatus(label + " failed: hipControl missing");
            yield break;
        }

        try
        {
            hip.currentPositionState = FreeControllerV3.PositionState.On;
        }
        catch { }

        Vector3 startPos = hip.control.localPosition;
        Vector3 targetPos = new Vector3(startPos.x, targetY, startPos.z);

        if (Vector3.Distance(startPos, targetPos) <= POSE_POS_EPS)
        {
            if (captureAtEnd)
                SetStatus(label + " already at target");
            yield break;
        }

        float dur = GetPoseTransitionDuration(captureAtEnd);
        float elapsed = 0f;

        while (elapsed < dur)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dur);
            hip.control.localPosition = Vector3.Lerp(startPos, targetPos, PoseMotionT(t));
            yield return null;
        }

        hip.control.localPosition = targetPos;

        if (captureAtEnd)
        {
            yield return null;
            InvokeHdcAction("HDC Capture Current", false);
            SetStatus(label + " done");
        }
    }

    private IEnumerator MacroUpperYWithUpperLowRoute(string label, float y, string resultPoseKey)
    {
        if (currentPoseKey == POSE_SUPINE)
        {
            yield return StartCoroutine(MacroRoutePoseEntries("Upper Low Leg Front", GetUpperLowLegFrontPoseEntries()));
            currentPoseKey = POSE_UPPER_LOW_LEG_FRONT;
        }

        if (currentPoseKey == POSE_PRONE || currentPoseKey == POSE_DOG || currentPoseKey == POSE_UPPER_LOW_LEG_FRONT || currentPoseKey == POSE_UPPER_LOW_LEG_MJI)
        {
            yield return StartCoroutine(MacroRoutePoseEntries("Upper Low", GetUpperLowPoseEntries()));
            currentPoseKey = POSE_UPPER_LOW;
        }

        yield return StartCoroutine(MacroUpperY(label, y));
        currentPoseKey = resultPoseKey;
    }

    private IEnumerator MacroUpperY(string label, float y)
    {
        SetStatus(label + " -> Upper X 0 then Hip-Upper PosY=" + F(y));

        yield return StartCoroutine(RunHdcPhase(
            MODE_HIP_UPPER,
            "",
            false, 0f,
            false, 0f,
            false, 0f,
            true, 0.0f,
            false, 0f,
            false, 0f
        ));

        yield return StartCoroutine(RunHdcPhase(
            MODE_HIP_UPPER,
            "",
            false, 0f,
            true, y,
            false, 0f,
            false, 0f,
            false, 0f,
            false, 0f
        ));
    }

    private IEnumerator MacroUpperX(string label, float xDeg)
    {
        SetStatus(label + " -> Hip-Upper RotX=" + F(xDeg));
        yield return StartCoroutine(RunHdcPhase(
            MODE_HIP_UPPER,
            "",
            false, 0f,
            false, 0f,
            false, 0f,
            true, xDeg,
            false, 0f,
            false, 0f
        ));
    }

    private IEnumerator MacroLowerX(string label, float xDeg)
    {
        SetStatus(label + " -> Hip-Lower RotX=" + F(xDeg));
        yield return StartCoroutine(RunHdcPhase(
            MODE_HIP_LOWER,
            "",
            false, 0f,
            false, 0f,
            false, 0f,
            true, xDeg,
            false, 0f,
            false, 0f
        ));
    }

    private IEnumerator MacroFaceTarget(bool back)
    {
        string label = back ? "Turn Back To Target" : "Turn Front To Target";

        Atom targetAtom = GetSelectedTargetAtom();
        if (targetAtom == null)
        {
            SetStatus(label + " failed: target not selected");
            yield break;
        }

        Transform targetRoot = GetAtomRootTransform(targetAtom);
        if (targetRoot == null || containingAtom == null || containingAtom.transform == null)
        {
            SetStatus(label + " failed: target root missing");
            yield break;
        }

        // v036: pre-v010 style.
        // Do not rotate containingAtom.transform/mainController.control directly.
        // Only drive HDC Individual/control rotation so this button means "turn direction only".
        Vector3 ownPos = containingAtom.transform.position;
        Vector3 dir = targetRoot.position - ownPos;
        dir.y = 0f;
        if (dir.magnitude < 0.001f)
        {
            SetStatus(label + " failed: too close");
            yield break;
        }

        if (back)
            dir = -dir;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
        Vector3 e = NormalizeEuler(targetRot.eulerAngles);

        SetStatus(label + " -> HDC control rot=" + FormatVec(e));

        yield return StartCoroutine(RunHdcPhase(
            MODE_PART,
            "control",
            false, 0f,
            false, 0f,
            false, 0f,
            true, e.x,
            true, e.y,
            true, e.z
        ));
    }

    private IEnumerator MacroLegsStretch(string label, bool forward)
    {
        float footAmount = forward
            ? (legForwardZ != null ? legForwardZ.val : 0.45f)
            : -(legBackZ != null ? legBackZ.val : 0.35f);

        float ratio = legKneeRatio != null ? Mathf.Clamp01(legKneeRatio.val) : 0.50f;
        float kneeAmount = footAmount * ratio;

        SetStatus(label + " start footZ=" + F(footAmount) + " kneeZ=" + F(kneeAmount));

        SetHdcLiveApply(false);
        SetHdcEnabled(false);

        List<LegMoveSnapshot> snaps = new List<LegMoveSnapshot>();
        AddLegMoveSnapshot(snaps, "rFootControl", footAmount);
        AddLegMoveSnapshot(snaps, "lFootControl", footAmount);
        AddLegMoveSnapshot(snaps, "rKneeControl", kneeAmount);
        AddLegMoveSnapshot(snaps, "lKneeControl", kneeAmount);

        if (snaps.Count == 0)
        {
            SetStatus(label + " failed: no leg controls");
            yield break;
        }

        float dur = GetTransitionDuration();
        float elapsed = 0f;

        while (elapsed < dur)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dur);
            ApplyLegMoveSnapshots(snaps, t);
            yield return null;
        }

        ApplyLegMoveSnapshots(snaps, 1f);

        yield return null;
        InvokeHdcAction("HDC Capture Current", false);
        SetStatus(label + " done");
    }

    private void AddLegMoveSnapshot(List<LegMoveSnapshot> snaps, string controlName, float deltaZ)
    {
        if (snaps == null || string.IsNullOrEmpty(controlName))
            return;

        FreeControllerV3 fc = FindController(controlName);
        if (fc == null || fc.control == null)
        {
            LogDebug("Leg control missing: " + controlName);
            return;
        }

        try
        {
            fc.currentPositionState = FreeControllerV3.PositionState.On;
        }
        catch { }

        LegMoveSnapshot s = new LegMoveSnapshot();
        s.ControlName = controlName;
        s.Controller = fc;
        s.ControlTransform = fc.control;
        s.StartLocalPos = fc.control.localPosition;
        s.TargetLocalPos = s.StartLocalPos + new Vector3(0f, 0f, deltaZ);
        snaps.Add(s);
    }

    private void ApplyLegMoveSnapshots(List<LegMoveSnapshot> snaps, float t)
    {
        if (snaps == null)
            return;

        for (int i = 0; i < snaps.Count; i++)
        {
            LegMoveSnapshot s = snaps[i];
            if (s == null || s.ControlTransform == null)
                continue;

            s.ControlTransform.localPosition = Vector3.Lerp(s.StartLocalPos, s.TargetLocalPos, t);
        }
    }

    private IEnumerator RunHdcPhase(
        string mode,
        string targetBone,
        bool setPosX, float targetPosX,
        bool setPosY, float targetPosY,
        bool setPosZ, float targetPosZ,
        bool setRotX, float targetRotX,
        bool setRotY, float targetRotY,
        bool setRotZ, float targetRotZ
    )
    {
        if (hdc == null && !ResolveHDC(false))
        {
            SetStatus("HDC not found");
            yield break;
        }

        SetHdcEnabled(true);
        SetHdcLiveApply(true);
        SetHdcChooser("Mode", mode);
        if (!string.IsNullOrEmpty(targetBone))
            SetHdcChooser("TargetBone", targetBone);

        InvokeHdcAction("HDC Capture Current", true);
        yield return null;

        JSONStorableFloat hdcPosX = GetHdcFloat("Pos X");
        JSONStorableFloat hdcPosY = GetHdcFloat("Pos Y");
        JSONStorableFloat hdcPosZ = GetHdcFloat("Pos Z");
        JSONStorableFloat hdcRotX = GetHdcFloat("Rot X");
        JSONStorableFloat hdcRotY = GetHdcFloat("Rot Y");
        JSONStorableFloat hdcRotZ = GetHdcFloat("Rot Z");

        if (hdcPosX == null || hdcPosY == null || hdcPosZ == null || hdcRotX == null || hdcRotY == null || hdcRotZ == null)
        {
            SetStatus("HDC slider params missing");
            yield break;
        }

        float startPosX = hdcPosX.val;
        float startPosY = hdcPosY.val;
        float startPosZ = hdcPosZ.val;
        float startRotX = hdcRotX.val;
        float startRotY = hdcRotY.val;
        float startRotZ = hdcRotZ.val;

        if (IsPhaseAlreadyAtTarget(
            setPosX, startPosX, targetPosX,
            setPosY, startPosY, targetPosY,
            setPosZ, startPosZ, targetPosZ,
            setRotX, startRotX, targetRotX,
            setRotY, startRotY, targetRotY,
            setRotZ, startRotZ, targetRotZ
        ))
        {
            ApplyHdcTweenValues(
                hdcPosX, hdcPosY, hdcPosZ,
                hdcRotX, hdcRotY, hdcRotZ,
                setPosX, startPosX, targetPosX,
                setPosY, startPosY, targetPosY,
                setPosZ, startPosZ, targetPosZ,
                setRotX, startRotX, targetRotX,
                setRotY, startRotY, targetRotY,
                setRotZ, startRotZ, targetRotZ,
                1f
            );

            InvokeHdcAction("HDC Apply Now", false);
            InvokeHdcAction("HDC Capture Current", false);
            yield break;
        }

        float dur = GetTransitionDuration();
        float elapsed = 0f;

        while (elapsed < dur)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dur);
            ApplyHdcTweenValues(
                hdcPosX, hdcPosY, hdcPosZ,
                hdcRotX, hdcRotY, hdcRotZ,
                setPosX, startPosX, targetPosX,
                setPosY, startPosY, targetPosY,
                setPosZ, startPosZ, targetPosZ,
                setRotX, startRotX, targetRotX,
                setRotY, startRotY, targetRotY,
                setRotZ, startRotZ, targetRotZ,
                t
            );
            yield return null;
        }

        ApplyHdcTweenValues(
            hdcPosX, hdcPosY, hdcPosZ,
            hdcRotX, hdcRotY, hdcRotZ,
            setPosX, startPosX, targetPosX,
            setPosY, startPosY, targetPosY,
            setPosZ, startPosZ, targetPosZ,
            setRotX, startRotX, targetRotX,
            setRotY, startRotY, targetRotY,
            setRotZ, startRotZ, targetRotZ,
            1f
        );

        InvokeHdcAction("HDC Apply Now", false);
        yield return null;
        InvokeHdcAction("HDC Capture Current", true);
        yield return null;
    }

    private void ApplyHdcTweenValues(
        JSONStorableFloat hdcPosX,
        JSONStorableFloat hdcPosY,
        JSONStorableFloat hdcPosZ,
        JSONStorableFloat hdcRotX,
        JSONStorableFloat hdcRotY,
        JSONStorableFloat hdcRotZ,
        bool setPosX, float startPosX, float targetPosX,
        bool setPosY, float startPosY, float targetPosY,
        bool setPosZ, float startPosZ, float targetPosZ,
        bool setRotX, float startRotX, float targetRotX,
        bool setRotY, float startRotY, float targetRotY,
        bool setRotZ, float startRotZ, float targetRotZ,
        float t
    )
    {
        if (setPosX) hdcPosX.val = Mathf.Lerp(startPosX, targetPosX, t);
        if (setPosY) hdcPosY.val = Mathf.Lerp(startPosY, targetPosY, t);
        if (setPosZ) hdcPosZ.val = Mathf.Lerp(startPosZ, targetPosZ, t);

        if (setRotX) hdcRotX.val = Mathf.LerpAngle(startRotX, targetRotX, t);
        if (setRotY) hdcRotY.val = Mathf.LerpAngle(startRotY, targetRotY, t);
        if (setRotZ) hdcRotZ.val = Mathf.LerpAngle(startRotZ, targetRotZ, t);
    }

    private bool IsPhaseAlreadyAtTarget(
        bool setPosX, float startPosX, float targetPosX,
        bool setPosY, float startPosY, float targetPosY,
        bool setPosZ, float startPosZ, float targetPosZ,
        bool setRotX, float startRotX, float targetRotX,
        bool setRotY, float startRotY, float targetRotY,
        bool setRotZ, float startRotZ, float targetRotZ
    )
    {
        if (setPosX && Mathf.Abs(startPosX - targetPosX) > PHASE_POS_EPS) return false;
        if (setPosY && Mathf.Abs(startPosY - targetPosY) > PHASE_POS_EPS) return false;
        if (setPosZ && Mathf.Abs(startPosZ - targetPosZ) > PHASE_POS_EPS) return false;

        if (setRotX && AngleDistance(startRotX, targetRotX) > PHASE_ROT_EPS) return false;
        if (setRotY && AngleDistance(startRotY, targetRotY) > PHASE_ROT_EPS) return false;
        if (setRotZ && AngleDistance(startRotZ, targetRotZ) > PHASE_ROT_EPS) return false;

        return true;
    }

    private float AngleDistance(float a, float b)
    {
        return Mathf.Abs(Mathf.DeltaAngle(a, b));
    }

    private bool ResolveHDC(bool log)
    {
        if (containingAtom == null)
            return false;

        hdc = null;
        hdcStorableId = "";

        JSONStorable fallback = null;
        string fallbackId = "";

        foreach (string storableId in containingAtom.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId)) continue;

            JSONStorable st = containingAtom.GetStorableByID(storableId);
            if (st == null) continue;

            JSONStorableFloat py = st.GetFloatJSONParam("Pos Y");
            JSONStorableFloat rx = st.GetFloatJSONParam("Rot X");
            JSONStorableStringChooser mode = st.GetStringChooserJSONParam("Mode");
            JSONStorableStringChooser bone = st.GetStringChooserJSONParam("TargetBone");

            if (py == null || rx == null || mode == null || bone == null)
                continue;

            // Prefer the modified HDC that exposes external actions.
            if (st.GetAction("HDC Capture Current") != null)
            {
                hdc = st;
                hdcStorableId = storableId;
                break;
            }

            if (fallback == null)
            {
                fallback = st;
                fallbackId = storableId;
            }
        }

        if (hdc == null && fallback != null)
        {
            hdc = fallback;
            hdcStorableId = fallbackId;
        }

        if (log)
        {
            if (hdc != null)
            {
                string actionStatus = hdc.GetAction("HDC Capture Current") != null ? "external-actions=1" : "external-actions=0";
                SetStatus("HDC found: " + hdcStorableId + " / " + actionStatus);
            }
            else
            {
                SetStatus("HDC not found on this atom");
            }
        }

        return hdc != null;
    }

    private JSONStorableFloat GetHdcFloat(string name)
    {
        if (hdc == null) return null;
        return hdc.GetFloatJSONParam(name);
    }

    private bool SetHdcChooser(string name, string value)
    {
        if (hdc == null) return false;

        JSONStorableStringChooser chooser = hdc.GetStringChooserJSONParam(name);
        if (chooser == null) return false;

        if (chooser.val != value)
            chooser.val = value;

        return true;
    }

    private void SetHdcLiveApply(bool enabled)
    {
        if (hdc == null) return;

        JSONStorableBool b = hdc.GetBoolJSONParam("Live Apply");
        if (b != null)
        {
            if (b.val != enabled)
                b.val = enabled;
        }
        else
            InvokeHdcAction(enabled ? "HDC Live Apply ON" : "HDC Live Apply OFF", false);
    }

    private bool ShouldAutoDisableHdc()
    {
        return autoDisableHdcAfterMacro != null && autoDisableHdcAfterMacro.val;
    }

    private void DisableHdcAfterMacroIfNeeded()
    {
        if (!ShouldAutoDisableHdc()) return;
        SetHdcLiveApply(false);
        SetHdcEnabled(false);
    }

    private void SetHdcEnabled(bool enabled)
    {
        if (hdc == null) return;

        JSONStorableBool b = hdc.GetBoolJSONParam("Enable");
        if (b != null)
        {
            if (b.val != enabled)
                b.val = enabled;
            return;
        }

        InvokeHdcAction(enabled ? "HDC Enable ON" : "HDC Enable OFF", false);
    }

    private bool InvokeHdcAction(string actionName, bool important)
    {
        if (hdc == null) return false;

        JSONStorableAction action = hdc.GetAction(actionName);
        if (action == null)
        {
            if (important)
                LogDebug("HDC action missing: " + actionName);
            return false;
        }

        try
        {
            action.actionCallback.Invoke();
            return true;
        }
        catch (Exception e)
        {
            LogDebug("HDC action failed: " + actionName + " / " + e.Message);
            return false;
        }
    }

    private void RefreshTargetChoices(bool log)
    {
        targetChoices.Clear();
        targetChoices.Add(NONE_TARGET);

        try
        {
            foreach (Atom atom in SuperController.singleton.GetAtoms())
            {
                if (LooksLikePersonAtom(atom))
                    targetChoices.Add(atom.uid);
            }
        }
        catch { }

        if (targetPersonChooser != null)
        {
            string old = targetPersonChooser.val;
            targetPersonChooser.choices = targetChoices;

            if (!targetChoices.Contains(old))
                targetPersonChooser.val = targetChoices.Count > 1 ? targetChoices[1] : NONE_TARGET;
        }

        if (log)
            SetStatus("targets refreshed: " + targetChoices.Count.ToString(CultureInfo.InvariantCulture));
    }

    private bool LooksLikePersonAtom(Atom atom)
    {
        if (atom == null || atom == containingAtom) return false;
        if (atom.freeControllers == null) return false;

        bool hasHip = false;
        bool hasChest = false;
        bool hasHead = false;

        for (int i = 0; i < atom.freeControllers.Length; i++)
        {
            FreeControllerV3 fc = atom.freeControllers[i];
            if (fc == null) continue;
            if (fc.name == "hipControl") hasHip = true;
            else if (fc.name == "chestControl") hasChest = true;
            else if (fc.name == "headControl") hasHead = true;
        }

        return hasHip && hasChest && hasHead;
    }

    private Atom GetSelectedTargetAtom()
    {
        if (targetPersonChooser == null) return null;
        string uid = targetPersonChooser.val;
        if (string.IsNullOrEmpty(uid) || uid == NONE_TARGET) return null;
        return SuperController.singleton.GetAtomByUid(uid);
    }

    private Transform GetAtomRootTransform(Atom atom)
    {
        if (atom == null) return null;
        if (atom.mainController != null && atom.mainController.control != null)
            return atom.mainController.control;
        if (atom.mainController != null)
            return atom.mainController.transform;
        return atom.transform;
    }

    private FreeControllerV3 FindController(string name)
    {
        if (containingAtom == null || containingAtom.freeControllers == null || string.IsNullOrEmpty(name))
            return null;

        FreeControllerV3 cached;
        if (controllerCache.TryGetValue(name, out cached))
        {
            if (cached != null && cached.name == name)
                return cached;
            controllerCache.Remove(name);
        }

        for (int i = 0; i < containingAtom.freeControllers.Length; i++)
        {
            FreeControllerV3 fc = containingAtom.freeControllers[i];
            if (fc != null && fc.name == name)
            {
                controllerCache[name] = fc;
                return fc;
            }
        }
        return null;
    }

    private void SaveAndSetFootIK(bool enabled)
    {
        if (!footIKSaved)
        {
            savedFootIK.Clear();
            SaveFootState("lFootControl");
            SaveFootState("rFootControl");
            footIKSaved = true;
        }

        SetFootState("lFootControl", enabled);
        SetFootState("rFootControl", enabled);
        SetStatus("foot IK " + (enabled ? "ON" : "OFF") + " temporary");
    }

    private void SaveFootState(string name)
    {
        FreeControllerV3 fc = FindController(name);
        SavedIKState s = new SavedIKState();
        if (fc != null)
        {
            s.Valid = true;
            s.PositionState = fc.currentPositionState;
            s.RotationState = fc.currentRotationState;
        }
        savedFootIK[name] = s;
    }

    private void SetFootState(string name, bool enabled)
    {
        FreeControllerV3 fc = FindController(name);
        if (fc == null) return;

        try
        {
            fc.currentPositionState = enabled
                ? FreeControllerV3.PositionState.On
                : FreeControllerV3.PositionState.Off;
            fc.currentRotationState = enabled
                ? FreeControllerV3.RotationState.On
                : FreeControllerV3.RotationState.Off;
        }
        catch { }
    }

    private void RestoreFootIKIfSaved()
    {
        if (!footIKSaved) return;

        RestoreFootState("lFootControl");
        RestoreFootState("rFootControl");
        footIKSaved = false;
        savedFootIK.Clear();
        SetStatus("foot IK restored");
    }

    private void RestoreFootState(string name)
    {
        FreeControllerV3 fc = FindController(name);
        if (fc == null) return;
        if (!savedFootIK.ContainsKey(name)) return;

        SavedIKState s = savedFootIK[name];
        if (s == null || !s.Valid) return;

        try
        {
            fc.currentPositionState = s.PositionState;
            fc.currentRotationState = s.RotationState;
        }
        catch { }
    }

    private Vector3 NormalizeEuler(Vector3 e)
    {
        return new Vector3(NormalizeAngle(e.x), NormalizeAngle(e.y), NormalizeAngle(e.z));
    }

    private float NormalizeAngle(float a)
    {
        while (a > 180f) a -= 360f;
        while (a < -180f) a += 360f;
        return a;
    }

    private string FormatVec(Vector3 v)
    {
        return "(" + F(v.x) + "," + F(v.y) + "," + F(v.z) + ")";
    }

    private string F(float v)
    {
        return v.ToString("F3", CultureInfo.InvariantCulture);
    }

    private void SetStatus(string msg)
    {
        if (statusText != null)
            statusText.val = msg;

        // v037: status text is always updated, but VaM log output is Debug ON only.
        LogDebug(msg);
    }

    private void LogDebug(string msg)
    {
        if (debugLog == null || !debugLog.val)
            return;
        SuperController.LogMessage("[humanControler] " + msg);
    }
}
