// ============================================================
// humanPoseControler.cs
// Version: v085_load_defaults_pink
// Date: 2026-07-06
// Base: humanControler_v036_turn_to_target_hdc_route.cs / HumanDrivenController_v103_external_enable_actions.cs
// Summary:
// - v085 makes Load User Defaults pink while keeping Cycle Pose / Cycle Back / Cycle Reset / numbered Cycle buttons standard color.
// - v084 moves Cycle Pose Slider to the very top of the right column and restores standard button color for Cycle Pose and later cycle buttons.
// - v083 moves Root X/Y/Z sliders to the right-top, places Load User Defaults directly above Cycle Pose, and keeps Cycle Slider Status above the left HDC/status text.
// - v082 hides Root XYZ status text and Root Origin/Home buttons, keeping only Root X/Y/Z sliders in the right Root Offset area.
// - v081 moves Cycle status under Target Person, moves Face/Butt Target to the right Root Offset area, keeps them pink, and places Cycle controls below Root Offset.
// - v080 reverts Face/Butt Target turn behavior to the pre-leg-IK-touch v077 path: hand/elbow IK OFF only, no foot/knee comply/restore, keeping stepped turn, root XYZ, Cycle Slider top, and Load User Defaults left button.
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
// - v039 makes HumanDrivenController optional: if HDC is missing, HDC-style phases run through a local direct-transform fallback and Load User Defaults uses PosePresets directly.
// - v040 fixes the missing TryExecutePosePresetAction helper used by the HDC-optional Load User Defaults fallback.
// - v041/v042 keeps Turn Front/Back root unchanged by rotating body IK controls around hipControl.
// - v042 changes Turn Front/Back no-root yaw basis so Front/Back are 180 degrees apart.
// - v042 adds Auto Pose test button/action that logs relative front/back relationship to the selected target.
// - v043 adds AI-readable POSE_TargetPosture / POSE_SpatialRelation detection, right-top info text, and 1-second polling.
// - v044 adds POSE_SelfPosture and POSE_DistanceRelation. Distance Near is calibrated per spatial relation when the relation changes, so Smart/Reverse Docking landing distance is treated as Near.
// - v045 adds POSE_*Posture_Dog detection: hipControl->chestControl line within +/-35 degrees of floor-parallel and raised enough above floor-like prone/supine height.
// - v046 adds POSE_TransitionMode: reads TargetLinePerson DOCKING pose-assist state, locks POSE detection during Smart/Reverse transition, and displays Push/Pull/Blend/Path without moving pose.
// - v049 adds Cycle Pose Slider with PoseNode blending and delayed HDC capture.
// - v050 adds Self Move sliders and hides visible Upper X buttons.
// - v051 fixes Self Move origin to use mainController/control so startup does not jump to atom transform zero, and restores Turn Front/Back to target-facing HDC control rotation.
// - v052 makes Self Move Height delta-based: height=0 is no-op, Move Origin/Home does not change Y unless the height slider is explicitly moved.
// - v053 adds Target Facing Yaw slider: 0 faces the selected target, +/-180 faces away, with delayed HDC capture.
// - v072 fixes double-press Prone routing and inserts 17_Prone_HandUp plus 18_Prone2 under 16_Prone.
// - v073 adds right-top Root X/Y/Z offset sliders with origin/home controls for direct mainController root position adjustment.
// - v074 carries body IK controls by the same world delta when Root XYZ moves so elbows/hands/knees/feet do not remain behind.
// - v071 updates 12_UpperLow_HandUp pose with symmetric hand/elbow/foot values.
// - v070 r4 updates 12_UpperLow_HandUp pose to reduce hand tangling.
// - v070 r3 inserts 12_UpperLow_HandUp under 11_Supine_HandUp and shifts later Cycle nodes.
// - v070 renumbers this slider/control branch to v070.
// - v070 replaces the Prone cycle pose with the provided POSE.
// - v070 inserts 11_Supine_HandUp immediately after 10_Supine and shifts later Cycle nodes.
// - v070 r2 updates 11_Supine_HandUp to the revised provided POSE.
// - v048 moves Cycle controls to the right-top, adds light-pink numbered Cycle step buttons, and keeps POSE monitor on the lower-left.
// - v049 adds a right-top Cycle Pose Slider using a CyclePoseNode list; slider blends between nodes and performs one HDC Capture after 0.2s idle.
// - v050 adds self Person move sliders (forward/back, left/right, height), hides visible Upper X buttons, and places move controls in that UI area.
// - v047 moves Auto Pose/POSE monitor and non-standalone utility controls to the left, leaving standalone pose buttons on the right.
// - v048 moves Cycle controls and direct numbered Cycle-step buttons to the right top, colors them light pink, and keeps POSE monitor on the lower-left.
// - v047 moves Auto Pose, POSE status displays, Transition Dead/Full sliders, Cycle/utility/non-single-pose controls to the left column; right column keeps standalone pose buttons only.
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
// - v075 changes visual target-facing controls and adds Face/Butt action aliases.
// - v076 corrects the actual tested mapping: Face Target uses yaw 0, Butt Target uses yaw 180.
// - v076 turns hand/elbow IK OFF before root target-facing rotation so arms do not remain in the old world pose.
// - v077 makes Face/Butt Target rotation stepped/smooth instead of a single rough root snap.
// ============================================================

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class humanPoseControler : MVRScript
{
    private const string MODE_PART = "Individual";
    private const string MODE_HIP_UPPER = "Hip-Upper";
    private const string MODE_HIP_LOWER = "Hip-Lower";

    private const string NONE_TARGET = "(none)";
    private const int CYCLE_POSE_COUNT = 28;

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
    private const string POSE_PRONE_HAND_UP = "ProneHandUp";
    private const string POSE_SUPINE = "Supine";
    private const string POSE_SUPINE_HAND_UP = "SupineHandUp";
    private const string POSE_UPPER_LOW_HAND_UP = "UpperLowHandUp";
    private const string POSE_DOG = "Dog";

    private const float PHASE_POS_EPS = 0.0005f;
    private const float PHASE_ROT_EPS = 0.25f;
    private const float POSE_POS_EPS = 0.0005f;
    private const float POSE_ROT_EPS = 0.25f;
    private const float DEFAULT_TRANSITION_TIME = 0.30f;
    private const float POSE_EASE_OUT_BLEND = 0.82f;
    private const float ROUTE_POSE_TIME_SCALE = 0.20f;
    private const float SIT_HIP_LOCAL_Y = 0.626f;

    private const float POSE_INFO_POLL_INTERVAL_SEC = 1.00f;
    private const float POSE_SPATIAL_RELATION_SIDE_THRESHOLD = 0.35f;
    private const float POSE_TARGET_POSTURE_STANDING_UP_DOT_MIN = 0.65f;
    private const float POSE_TARGET_POSTURE_SITTING_UP_DOT_MIN = 0.35f;
    private const float POSE_TARGET_POSTURE_LYING_ABS_UP_DOT_MAX = 0.35f;
    // Dog pose: hipControl -> chestControl line is almost floor-parallel.
    // +/-35 degrees from horizontal means abs(dot(bodyDir, worldUp)) <= sin(35deg) ~= 0.574.
    private const float POSE_TARGET_POSTURE_DOG_ABS_UP_DOT_MAX = 0.574f;
    // Dog should be horizontal-ish but raised. Prone/Supine are usually much lower.
    private const float POSE_TARGET_POSTURE_DOG_HIP_LOCAL_Y_MIN = 0.45f;
    private const float POSE_TARGET_POSTURE_DOG_CHEST_LOCAL_Y_MIN = 0.25f;
    private const float POSE_TARGET_POSTURE_STANDING_HIP_LOCAL_Y_MIN = 0.85f;
    private const float POSE_TARGET_POSTURE_SUPINE_PRONE_CHEST_UP_DOT = 0.20f;
    private const float POSE_DISTANCE_DEFAULT_NEAR_REFERENCE = 1.00f;
    private const float POSE_DISTANCE_NEAR_HALF_WIDTH = 0.25f;
    private const float POSE_DISTANCE_FAR_EXTRA = 0.90f;
    private const float POSE_DISTANCE_TOO_CLOSE_ABSOLUTE = 0.18f;
    private const float POSE_DISTANCE_CALIBRATE_MIN = 0.15f;
    private const float POSE_DISTANCE_CALIBRATE_MAX = 3.50f;
    private const float POSE_TRANSITION_INFO_POLL_INTERVAL_SEC = 0.10f;
    private const float POSE_TRANSITION_DEAD_ZONE_DEFAULT = 0.05f;
    private const float POSE_TRANSITION_FULL_DISTANCE_DEFAULT = 0.35f;
    private const float POSE_TRANSITION_DOCKING_EVENT_EPS = 0.001f;
    private const float CYCLE_SLIDER_CAPTURE_DELAY_SEC = 0.20f;
    private const float VISUAL_FACE_TARGET_YAW_OFFSET_DEG = 0.0f;
    private const float VISUAL_BUTT_TARGET_YAW_OFFSET_DEG = 180.0f;
    private const float TARGET_TURN_SECONDS_DEFAULT = 0.45f;
    private const float TARGET_TURN_STEPS_DEFAULT = 12.0f;

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
    private JSONStorableString poseSelfPostureText;
    private JSONStorableString poseTargetPostureText;
    private JSONStorableString poseSpatialRelationText;
    private JSONStorableString poseDistanceRelationText;
    private JSONStorableString poseTransitionModeText;
    private JSONStorableString poseTransitionIntentText;
    private JSONStorableString poseTransitionBlendText;
    private JSONStorableString poseTransitionPathText;
    private JSONStorableFloat poseTransitionDeadZone;
    private JSONStorableFloat poseTransitionFullDistance;
    private JSONStorableFloat cyclePoseSlider;
    private JSONStorableString cyclePoseSliderStatus;
    private JSONStorableFloat selfMoveForwardBack;
    private JSONStorableFloat selfMoveLeftRight;
    private JSONStorableFloat selfMoveHeight;
    private JSONStorableString selfMoveStatus;
    private JSONStorableFloat targetFacingYaw;
    private JSONStorableFloat targetTurnSeconds;
    private JSONStorableFloat targetTurnSteps;
    private JSONStorableString targetFacingStatus;
    private JSONStorableFloat rootMoveX;
    private JSONStorableFloat rootMoveY;
    private JSONStorableFloat rootMoveZ;
    private JSONStorableString rootMoveStatus;

    private JSONStorable hdc;
    private string hdcStorableId = "";
    private Coroutine activeRoutine;
    private Coroutine targetFacingTurnRoutine;
    private string currentPoseKey = POSE_NONE;
    private float nextPOSE_InfoPollTime = 0f;
    private string lastPOSE_SelfPostureDisplay = "";
    private string lastPOSE_TargetPostureDisplay = "";
    private string lastPOSE_SpatialRelationDisplay = "";
    private string lastPOSE_DistanceRelationDisplay = "";
    private string lastPOSE_SpatialRelationCodeForDistanceCalibration = "";
    private string lastPOSE_TransitionModeDisplay = "";
    private string lastPOSE_TransitionIntentDisplay = "";
    private string lastPOSE_TransitionBlendDisplay = "";
    private string lastPOSE_TransitionPathDisplay = "";
    private Dictionary<string, float> poseNearDistanceBySpatialRelation = new Dictionary<string, float>();

    private bool poseTransitionModeActive = false;
    private float poseTransitionStartTime = -1.0f;
    private float poseTransitionLastDockingEventTime = -1.0f;
    private float poseTransitionBaseDistance = 0.0f;
    private string poseTransitionDockingMode = "None";
    private string poseTransitionLockedSelfPostureCode = "POSE_SelfPosture_Unknown";
    private string poseTransitionLockedSelfPostureDisplay = "Unknown";
    private string poseTransitionLockedTargetPostureCode = "POSE_TargetPosture_Unknown";
    private string poseTransitionLockedTargetPostureDisplay = "Unknown";
    private string poseTransitionLockedSpatialRelationCode = "POSE_SpatialRelation_Unknown";
    private string poseTransitionLockedSpatialRelationDisplay = "Unknown";
    private string poseTransitionLockedDistanceRelationCode = "POSE_DistanceRelation_Unknown";
    private string poseTransitionLockedDistanceRelationDisplay = "Unknown";
    private bool poseTransitionIntentLocked = false;
    private string poseTransitionLockedIntent = "Neutral";
    private string poseTransitionLockedPath = "None";
    private float nextPOSE_TransitionInfoPollTime = 0f;

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
    private readonly Color cycleButtonColor = new Color(1.00f, 0.78f, 0.86f, 1f);
    private int cyclePoseIndex = -1;
    private readonly List<CyclePoseNode> cyclePoseNodes = new List<CyclePoseNode>();
    private bool cyclePoseSliderInitialized = false;
    private bool cyclePoseSliderDirty = false;
    private float cyclePoseSliderLastObservedValue = -999.0f;
    private float cyclePoseSliderLastChangeTime = -1.0f;
    private float cyclePoseSliderLastCapturedValue = -999.0f;
    private Vector3 selfMoveBaseWorldPos = Vector3.zero;
    private Quaternion selfMoveBaseWorldRot = Quaternion.identity;
    private bool selfMoveBaseCaptured = false;
    private bool selfMoveSuppressApply = false;
    private float selfMoveLastForwardBack = 999999.0f;
    private float selfMoveLastLeftRight = 999999.0f;
    private float selfMoveLastHeight = 999999.0f;
    private float selfMoveAppliedHeightOffset = 0.0f;
    private bool targetFacingSuppressApply = false;
    private float targetFacingLastYaw = 999999.0f;
    private bool targetFacingDirty = false;
    private float targetFacingLastChangeTime = -1.0f;
    private float targetFacingLastCapturedYaw = 999999.0f;
    private Vector3 rootMoveBaseWorldPos = Vector3.zero;
    private bool rootMoveBaseCaptured = false;
    private bool rootMoveSuppressApply = false;
    private float rootMoveLastX = 999999.0f;
    private float rootMoveLastY = 999999.0f;
    private float rootMoveLastZ = 999999.0f;

    private class CyclePoseNode
    {
        public int Step;
        public string Label;

        public CyclePoseNode(int step, string label)
        {
            Step = step;
            Label = label;
        }
    }

    private class SavedIKState
    {
        public bool Valid;
        public FreeControllerV3.PositionState PositionState;
        public FreeControllerV3.RotationState RotationState;
    }

    private class POSE_SpatialRelationResult
    {
        public string Code;
        public string Display;
        public bool MutualFront;
        public bool MutualBack;
        public bool SelfFrontToTarget;
        public bool SelfBackToTarget;
        public bool TargetFrontToSelf;
        public bool TargetBackToSelf;
        public float SelfDot;
        public float TargetDot;
        public float ForwardDot;
    }

    private class POSE_PostureResult
    {
        public string Code;
        public string Display;
        public Vector3 HipWorld;
        public Vector3 ChestWorld;
        public Vector3 BodyAxisWorld;
        public float BodyAxisLength;
        public float BodyUpDot;
        public float AbsBodyUpDot;
        public float BodyFloorAngleDeg;
        public bool DogParallelCandidate;
        public float HipLocalY;
        public float ChestLocalY;
        public float ChestUpDot;
        public float ChestForwardUpDot;
        public float TargetRootUpDot;
    }

    private class POSE_DistanceRelationResult
    {
        public string Code;
        public string Display;
        public float HipHorizontalDistance;
        public float HipVerticalDifference;
        public float NearReferenceDistance;
        public float NearHalfWidth;
        public bool CalibratedForSpatialRelation;
        public string SpatialRelationCode;
        public string NearReferenceSource;
    }

    private class DOCKING_PoseAssistState
    {
        public bool Valid;
        public bool Active;
        public string Mode;
        public string PushPullIntent;
        public float BaseDistance;
        public float CurrentDistance;
        public float DistanceDelta;
        public float EventTime;
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
            LocalRot = humanPoseControler.NormalizeQuaternionStatic(new Quaternion(qx, qy, qz, qw));
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

    private class NoRootTurnSnapshot
    {
        public string ControlName;
        public FreeControllerV3 Controller;
        public Transform ControlTransform;
        public Vector3 StartLocalPos;
        public Quaternion StartLocalRot;
        public Vector3 TargetLocalPos;
        public Quaternion TargetLocalRot;
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

            poseSelfPostureText = new JSONStorableString("POSE Self Posture", "POSE_SelfPosture: NoTarget");
            poseTargetPostureText = new JSONStorableString("POSE Target Posture", "POSE_TargetPosture: NoTarget");
            poseSpatialRelationText = new JSONStorableString("POSE Spatial Relation", "POSE_SpatialRelation: NoTarget");
            poseDistanceRelationText = new JSONStorableString("POSE Distance Relation", "POSE_DistanceRelation: NoTarget");
            poseTransitionModeText = new JSONStorableString("POSE Transition Mode", "POSE_TransitionMode: Inactive");
            poseTransitionIntentText = new JSONStorableString("POSE Transition Intent", "POSE_TransitionIntent: Neutral");
            poseTransitionBlendText = new JSONStorableString("POSE Transition Blend", "POSE_TransitionBlend: 0%");
            poseTransitionPathText = new JSONStorableString("POSE Transition Path", "POSE_TransitionPath: None");
            poseTransitionDeadZone = new JSONStorableFloat("POSE Transition Dead Zone", POSE_TRANSITION_DEAD_ZONE_DEFAULT, 0.00f, 0.15f, true, true);
            poseTransitionFullDistance = new JSONStorableFloat("POSE Transition Full Distance", POSE_TRANSITION_FULL_DISTANCE_DEFAULT, 0.15f, 0.80f, true, true);
            BuildCyclePoseNodes();
            cyclePoseSlider = new JSONStorableFloat("Cycle Pose Slider", 0.0f, 0.0f, Mathf.Max(0.0f, (float)(cyclePoseNodes.Count - 1)), true, true);
            cyclePoseSliderStatus = new JSONStorableString("Cycle Slider Status", "Cycle Slider: 01_Stand");
            selfMoveForwardBack = new JSONStorableFloat("Self Move Forward/Back", 0.0f, -2.0f, 2.0f, true, true);
            selfMoveLeftRight = new JSONStorableFloat("Self Move Left/Right", 0.0f, -2.0f, 2.0f, true, true);
            selfMoveHeight = new JSONStorableFloat("Self Move Height", 0.0f, -1.0f, 1.0f, true, true);
            selfMoveStatus = new JSONStorableString("Self Move Status", "Self Move: origin not captured");
            targetFacingYaw = new JSONStorableFloat("Target Facing Yaw", 0.0f, -180.0f, 180.0f, true, true);
            targetTurnSeconds = new JSONStorableFloat("Target Turn Seconds", TARGET_TURN_SECONDS_DEFAULT, 0.05f, 2.00f, true, true);
            targetTurnSteps = new JSONStorableFloat("Target Turn Steps", TARGET_TURN_STEPS_DEFAULT, 2.0f, 36.0f, true, true);
            targetFacingStatus = new JSONStorableString("Target Facing Status", "Target Facing: not applied");
            rootMoveX = new JSONStorableFloat("Root X Offset", 0.0f, -2.0f, 2.0f, true, true);
            rootMoveY = new JSONStorableFloat("Root Y Offset", 0.0f, -2.0f, 2.0f, true, true);
            rootMoveZ = new JSONStorableFloat("Root Z Offset", 0.0f, -2.0f, 2.0f, true, true);
            rootMoveStatus = new JSONStorableString("Root XYZ Status", "Root XYZ: origin not captured");
            RegisterString(poseSelfPostureText);
            RegisterString(poseTargetPostureText);
            RegisterString(poseSpatialRelationText);
            RegisterString(poseDistanceRelationText);
            RegisterString(poseTransitionModeText);
            RegisterString(poseTransitionIntentText);
            RegisterString(poseTransitionBlendText);
            RegisterString(poseTransitionPathText);
            RegisterFloat(poseTransitionDeadZone);
            RegisterFloat(poseTransitionFullDistance);
            RegisterFloat(cyclePoseSlider);
            RegisterString(cyclePoseSliderStatus);
            RegisterFloat(selfMoveForwardBack);
            RegisterFloat(selfMoveLeftRight);
            RegisterFloat(selfMoveHeight);
            RegisterString(selfMoveStatus);
            RegisterFloat(targetFacingYaw);
            RegisterFloat(targetTurnSeconds);
            RegisterFloat(targetTurnSteps);
            RegisterString(targetFacingStatus);
            RegisterFloat(rootMoveX);
            RegisterFloat(rootMoveY);
            RegisterFloat(rootMoveZ);
            RegisterString(rootMoveStatus);

            // v083: Cycle slider status is shown directly under Target Person,
            // above the left HDC/status text.
            if (cyclePoseSliderStatus != null)
            {
                UIDynamicTextField cycleStatusTf = CreateTextField(cyclePoseSliderStatus, false);
                if (cycleStatusTf != null) cycleStatusTf.height = 38f;
            }

            statusText = new JSONStorableString("humanControler Status", "ready");
            RegisterString(statusText);
            UIDynamicTextField tf = CreateTextField(statusText);
            if (tf != null) tf.height = 90f;

            CaptureSelfMoveOrigin(false);
            MarkSelfMoveSliderValuesObserved();
            MarkTargetFacingYawObserved();
            CaptureRootXYZOrigin(false);
            MarkRootXYZSliderValuesObserved();

            // v037: Debug checkbox is kept on the left column under Status.
            CreateToggle(debugLog, false);

            // ============================================================
            // v048 UI layout:
            // Left column  = manual utility controls + POSE monitor.
            // Right column = Cycle controls and numbered Cycle step buttons at the top.
            // The light-pink buttons are the Cycle workflow group.
            // ============================================================

            // v081: Face/Butt Target buttons moved to the right Root Offset area.
            // Keep the sliders/status here on the left for detailed tuning.
            CreateTargetFacingControlsLeft();

            // v050: Upper X visible buttons are hidden here.
            // Use this area for own-Person movement sliders instead.
            CreateSelfMoveControlsLeft();

            CreateLeftMacroButton("Refresh HDC/Target", delegate()
            {
                ResolveHDC(true);
                RefreshTargetChoices(true);
            });

            CreateLeftMacroButton("STOP / Cleanup", delegate()
            {
                StopMacroAndCleanup();
            });

            // Auto Pose and POSE monitor stay on the lower-left.
            CreateLeftMacroButton("Auto Pose", delegate()
            {
                StartMacro("Auto Pose", MacroAutoPoseRelationLog());
            });

            CreatePOSE_InfoTextFieldsLeft();

            // v084: Cycle Pose Slider is the very top of the right column.
            // Cycle Slider Status remains on the left, above the HDC/status text.
            CreateCycleSliderControlsRight();

            // v083/v084: Root X/Y/Z sliders sit directly under the Cycle Pose Slider.
            CreateRootXYZControlsRight();

            // v081/v084: visual target-facing buttons live near Root Offset and stay pink.
            CreatePinkRightMacroButton("Face Target", delegate()
            {
                SetTargetFacingYawAndApply(VISUAL_FACE_TARGET_YAW_OFFSET_DEG, true, "button face target");
            });

            CreatePinkRightMacroButton("Butt Target", delegate()
            {
                SetTargetFacingYawAndApply(VISUAL_BUTT_TARGET_YAW_OFFSET_DEG, true, "button butt target");
            });

            // v083: Load User Defaults is directly above Cycle Pose.
            // v085: Load User Defaults is pink; Cycle controls below it stay standard color.
            CreatePinkRightMacroButton("Load User Defaults", delegate()
            {
                StartMacro("Load User Defaults", MacroLoadUserDefaults());
            });

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

            // Direct Cycle step buttons. These call the same logic as Cycle Pose step-by-step,
            // but labels are now generated from the CyclePoseNode list for future insertion.
            CreateCycleNodeButtonsRight();

            RegisterExternalActions();

            ResolveHDC(true);
            RefreshPOSE_InfoText(false);
        }
        catch (Exception e)
        {
            if (statusText != null)
                statusText.val = "Init error: " + e.Message;

            if (debugLog != null && debugLog.val)
                SuperController.LogError("[humanControler] Init error: " + e);
        }
    }

    private UIDynamicButton CreateLeftMacroButton(string label, Action onClick)
    {
        return CreateMacroButton(label, onClick, false);
    }

    private UIDynamicButton CreateRightMacroButton(string label, Action onClick)
    {
        return CreateMacroButton(label, onClick, true);
    }

    private UIDynamicButton CreatePinkRightMacroButton(string label, Action onClick)
    {
        UIDynamicButton ui = CreateRightMacroButton(label, onClick);
        SetMacroButtonBaseColor(label, cycleButtonColor);
        return ui;
    }

    private UIDynamicButton CreateCycleStepRightButton(string label, int step)
    {
        return CreateRightMacroButton(label, delegate()
        {
            StartMacro(label, MacroCycleDirectStep(step, label));
        });
    }

    private void CreateRootXYZControlsRight()
    {
        // v082: keep this area compact. Only show the actual Root X/Y/Z sliders.
        // Root status text and Origin/Home buttons remain internal/external-only.
        if (rootMoveX != null)
            CreateSlider(rootMoveX, true);

        if (rootMoveY != null)
            CreateSlider(rootMoveY, true);

        if (rootMoveZ != null)
            CreateSlider(rootMoveZ, true);
    }

    private void CreateCycleSliderControlsRight()
    {
        if (cyclePoseSlider != null)
            CreateSlider(cyclePoseSlider, true);
    }

    private void CreateCycleNodeButtonsRight()
    {
        if (cyclePoseNodes == null || cyclePoseNodes.Count == 0)
            BuildCyclePoseNodes();

        for (int i = 0; i < cyclePoseNodes.Count; i++)
        {
            CyclePoseNode node = cyclePoseNodes[i];
            if (node == null)
                continue;
            CreateCycleStepRightButton(node.Label, node.Step);
        }
    }

    private UIDynamicButton CreateMacroButton(string label, Action onClick, bool rightSide)
    {
        UIDynamicButton ui = CreateButton(label, rightSide);
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

    private void CreatePOSE_InfoTextFieldsLeft()
    {
        UIDynamicTextField selfPostureTf = CreateTextField(poseSelfPostureText, false);
        if (selfPostureTf != null) selfPostureTf.height = 34f;

        UIDynamicTextField targetPostureTf = CreateTextField(poseTargetPostureText, false);
        if (targetPostureTf != null) targetPostureTf.height = 34f;

        UIDynamicTextField relationTf = CreateTextField(poseSpatialRelationText, false);
        if (relationTf != null) relationTf.height = 34f;

        UIDynamicTextField distanceTf = CreateTextField(poseDistanceRelationText, false);
        if (distanceTf != null) distanceTf.height = 34f;

        UIDynamicTextField transitionModeTf = CreateTextField(poseTransitionModeText, false);
        if (transitionModeTf != null) transitionModeTf.height = 34f;

        UIDynamicTextField transitionIntentTf = CreateTextField(poseTransitionIntentText, false);
        if (transitionIntentTf != null) transitionIntentTf.height = 34f;

        UIDynamicTextField transitionBlendTf = CreateTextField(poseTransitionBlendText, false);
        if (transitionBlendTf != null) transitionBlendTf.height = 34f;

        UIDynamicTextField transitionPathTf = CreateTextField(poseTransitionPathText, false);
        if (transitionPathTf != null) transitionPathTf.height = 34f;

        CreateSlider(poseTransitionDeadZone, false);
        CreateSlider(poseTransitionFullDistance, false);
    }



    private void CreateTargetFacingControlsLeft()
    {
        if (targetFacingYaw != null)
            CreateSlider(targetFacingYaw, false);

        if (targetTurnSeconds != null)
            CreateSlider(targetTurnSeconds, false);

        if (targetTurnSteps != null)
            CreateSlider(targetTurnSteps, false);

        if (targetFacingStatus != null)
        {
            UIDynamicTextField tf = CreateTextField(targetFacingStatus, false);
            if (tf != null) tf.height = 42f;
        }
    }

    private void MarkTargetFacingYawObserved()
    {
        targetFacingLastYaw = targetFacingYaw != null ? NormalizeSignedAngle(targetFacingYaw.val) : 0.0f;
        targetFacingLastCapturedYaw = 999999.0f;
        targetFacingDirty = false;
        targetFacingLastChangeTime = -1.0f;
    }

    private void SetTargetFacingYawAndApply(float yaw, bool force, string reason)
    {
        yaw = NormalizeSignedAngle(yaw);

        targetFacingSuppressApply = true;
        try
        {
            if (targetFacingYaw != null)
                targetFacingYaw.val = yaw;
        }
        catch { }
        targetFacingSuppressApply = false;

        targetFacingLastYaw = yaw;
        ApplyTargetFacingYaw(yaw, force, reason);
    }

    private void UpdateTargetFacingYawSliderAndCapture()
    {
        if (!targetFacingSuppressApply && targetFacingYaw != null)
        {
            float yaw = NormalizeSignedAngle(targetFacingYaw.val);
            if (Mathf.Abs(Mathf.DeltaAngle(targetFacingLastYaw, yaw)) > 0.0001f)
            {
                targetFacingLastYaw = yaw;
                ApplyTargetFacingYaw(yaw, false, "slider");
            }
        }

        if (targetFacingDirty && targetFacingLastChangeTime > 0.0f && Time.time - targetFacingLastChangeTime >= CYCLE_SLIDER_CAPTURE_DELAY_SEC)
        {
            targetFacingDirty = false;
            float yaw = targetFacingYaw != null ? NormalizeSignedAngle(targetFacingYaw.val) : 0.0f;
            targetFacingLastCapturedYaw = yaw;
            InvokeHdcAction("HDC Capture Current", false);
            UpdateTargetFacingStatus("captured", yaw);
        }
    }

    private void ApplyTargetFacingYaw(float yawOffsetDeg, bool force, string reason)
    {
        Transform root = GetSelfMoveRootTransform();
        Atom targetAtom = GetSelectedTargetAtom();
        Transform targetRoot = targetAtom != null ? GetAtomRootTransform(targetAtom) : null;

        if (root == null)
        {
            UpdateTargetFacingStatus("failed: self root missing", yawOffsetDeg);
            return;
        }

        if (targetRoot == null)
        {
            UpdateTargetFacingStatus("failed: target missing", yawOffsetDeg);
            return;
        }

        Vector3 dir = targetRoot.position - root.position;
        dir.y = 0.0f;
        if (dir.magnitude < 0.001f)
        {
            UpdateTargetFacingStatus("failed: too close", yawOffsetDeg);
            return;
        }

        Quaternion faceTarget = Quaternion.LookRotation(dir.normalized, Vector3.up);
        Vector3 currentEuler = NormalizeEuler(root.rotation.eulerAngles);
        float targetYaw = NormalizeSignedAngle(faceTarget.eulerAngles.y + yawOffsetDeg);
        Quaternion targetRot = Quaternion.Euler(currentEuler.x, targetYaw, currentEuler.z);

        // Target-facing root rotation can leave active hand/elbow IK controls behind in world space.
        // Turn them OFF first and keep them OFF; pose/cycle actions can turn them back ON when needed.
        SetHandArmIKOffForTargetFacing(reason);

        StartTargetFacingSteppedRotation(targetRot, yawOffsetDeg, reason);
    }

    private void StartTargetFacingSteppedRotation(Quaternion targetRot, float yawOffsetDeg, string reason)
    {
        if (targetFacingTurnRoutine != null)
        {
            try { StopCoroutine(targetFacingTurnRoutine); } catch { }
            targetFacingTurnRoutine = null;
        }

        targetFacingTurnRoutine = StartCoroutine(TargetFacingSteppedRotationRoutine(targetRot, yawOffsetDeg, reason));
    }

    private IEnumerator TargetFacingSteppedRotationRoutine(Quaternion targetRot, float yawOffsetDeg, string reason)
    {
        Transform root = GetSelfMoveRootTransform();
        if (root == null)
        {
            UpdateTargetFacingStatus("failed: self root missing", yawOffsetDeg);
            targetFacingTurnRoutine = null;
            yield break;
        }

        Quaternion startRot = root.rotation;
        Vector3 startEuler = NormalizeEuler(startRot.eulerAngles);
        Vector3 targetEuler = NormalizeEuler(targetRot.eulerAngles);

        float seconds = targetTurnSeconds != null ? Mathf.Max(0.01f, targetTurnSeconds.val) : TARGET_TURN_SECONDS_DEFAULT;
        int steps = targetTurnSteps != null ? Mathf.Clamp(Mathf.RoundToInt(targetTurnSteps.val), 2, 36) : Mathf.RoundToInt(TARGET_TURN_STEPS_DEFAULT);
        float wait = seconds / Mathf.Max(1, steps);

        for (int i = 1; i <= steps; i++)
        {
            float t = Mathf.Clamp01((float)i / (float)steps);
            float eased = t * t * (3.0f - 2.0f * t);
            float yaw = Mathf.LerpAngle(startEuler.y, targetEuler.y, eased);
            Quaternion stepRot = Quaternion.Euler(startEuler.x, yaw, startEuler.z);

            ApplySelfRootWorldRotation(stepRot);
            UpdateTargetFacingStatus("turning " + i + "/" + steps, yawOffsetDeg);

            if (i < steps)
                yield return new WaitForSeconds(wait);
        }

        ApplySelfRootWorldRotation(targetRot);

        targetFacingDirty = true;
        targetFacingLastChangeTime = Time.time;
        UpdateTargetFacingStatus(string.IsNullOrEmpty(reason) ? "applied" : reason, yawOffsetDeg);
        targetFacingTurnRoutine = null;
    }

    private void ApplySelfRootWorldRotation(Quaternion targetRot)
    {
        try
        {
            if (containingAtom != null && containingAtom.mainController != null && containingAtom.mainController.control != null)
            {
                containingAtom.mainController.control.rotation = targetRot;
                return;
            }

            if (containingAtom != null && containingAtom.mainController != null && containingAtom.mainController.transform != null)
            {
                containingAtom.mainController.transform.rotation = targetRot;
                return;
            }

            if (containingAtom != null && containingAtom.transform != null)
                containingAtom.transform.rotation = targetRot;
        }
        catch { }
    }
    private void SetHandArmIKOffForTargetFacing(string reason)
    {
        SetControllerIKOff("lHandControl");
        SetControllerIKOff("rHandControl");
        SetControllerIKOff("lElbowControl");
        SetControllerIKOff("rElbowControl");

        LogDebug("Target facing hand/elbow IK OFF / reason=" + (reason ?? ""));
    }

    private void SetControllerIKOff(string controllerName)
    {
        FreeControllerV3 fc = FindController(controllerName);
        if (fc == null)
            return;

        try
        {
            fc.currentPositionState = FreeControllerV3.PositionState.Off;
            fc.currentRotationState = FreeControllerV3.RotationState.Off;
        }
        catch { }
    }


    private void UpdateTargetFacingStatus(string state, float yaw)
    {
        if (targetFacingStatus == null)
            return;

        targetFacingStatus.val = "Target Facing: " + state + " / yaw=" + F(NormalizeSignedAngle(yaw)) + " / 0=face / 180=butt";
    }

    private float NormalizeSignedAngle(float angle)
    {
        while (angle > 180.0f) angle -= 360.0f;
        while (angle < -180.0f) angle += 360.0f;
        return angle;
    }

    private void MarkRootXYZSliderValuesObserved()
    {
        rootMoveLastX = rootMoveX != null ? rootMoveX.val : 0.0f;
        rootMoveLastY = rootMoveY != null ? rootMoveY.val : 0.0f;
        rootMoveLastZ = rootMoveZ != null ? rootMoveZ.val : 0.0f;
    }

    private void CaptureRootXYZOrigin(bool resetSliders)
    {
        Transform root = GetSelfMoveRootTransform();
        if (root == null)
        {
            rootMoveBaseCaptured = false;
            if (rootMoveStatus != null)
                rootMoveStatus.val = "Root XYZ: no self root";
            return;
        }

        rootMoveBaseWorldPos = root.position;
        rootMoveBaseCaptured = true;

        if (resetSliders)
        {
            ResetRootXYZSlidersNoCapture();
            UpdateRootXYZStatus("origin captured");
        }
        else
        {
            MarkRootXYZSliderValuesObserved();
            UpdateRootXYZStatus("origin captured / no move");
        }
    }

    private void ResetRootXYZSliders()
    {
        ResetRootXYZSlidersNoCapture();
        ApplyRootXYZSliders("home");
    }

    private void ResetRootXYZSlidersNoCapture()
    {
        rootMoveSuppressApply = true;
        try
        {
            if (rootMoveX != null) rootMoveX.val = 0.0f;
            if (rootMoveY != null) rootMoveY.val = 0.0f;
            if (rootMoveZ != null) rootMoveZ.val = 0.0f;
        }
        catch { }
        rootMoveSuppressApply = false;

        MarkRootXYZSliderValuesObserved();
    }

    private void UpdateRootXYZSliders()
    {
        if (rootMoveSuppressApply)
            return;

        if (rootMoveX == null || rootMoveY == null || rootMoveZ == null)
            return;

        float x = rootMoveX.val;
        float y = rootMoveY.val;
        float z = rootMoveZ.val;

        if (Mathf.Abs(x - rootMoveLastX) <= 0.0001f &&
            Mathf.Abs(y - rootMoveLastY) <= 0.0001f &&
            Mathf.Abs(z - rootMoveLastZ) <= 0.0001f)
            return;

        rootMoveLastX = x;
        rootMoveLastY = y;
        rootMoveLastZ = z;

        ApplyRootXYZSliders("moving");
    }

    private void ApplyRootXYZSliders(string reason)
    {
        Transform root = GetSelfMoveRootTransform();
        if (root == null)
            return;

        if (!rootMoveBaseCaptured)
            CaptureRootXYZOrigin(false);

        float x = rootMoveX != null ? rootMoveX.val : 0.0f;
        float y = rootMoveY != null ? rootMoveY.val : 0.0f;
        float z = rootMoveZ != null ? rootMoveZ.val : 0.0f;

        Vector3 beforeRootPos = root.position;
        Vector3 targetPos = rootMoveBaseWorldPos + new Vector3(x, y, z);
        ApplySelfRootWorldPositionCarryControls(targetPos, beforeRootPos);
        UpdateRootXYZStatus(reason);
    }

    private void ApplySelfRootWorldPositionCarryControls(Vector3 targetPos, Vector3 beforeRootPos)
    {
        Vector3 expectedDelta = targetPos - beforeRootPos;
        if (expectedDelta.sqrMagnitude <= 0.00000001f)
        {
            ApplySelfRootWorldPosition(targetPos);
            return;
        }

        Dictionary<FreeControllerV3, Vector3> beforeControlWorldPos = new Dictionary<FreeControllerV3, Vector3>();

        try
        {
            if (containingAtom != null && containingAtom.freeControllers != null)
            {
                for (int i = 0; i < containingAtom.freeControllers.Length; i++)
                {
                    FreeControllerV3 fc = containingAtom.freeControllers[i];
                    if (fc == null || fc.control == null)
                        continue;

                    if (containingAtom.mainController != null && fc == containingAtom.mainController)
                        continue;

                    beforeControlWorldPos[fc] = fc.control.position;
                }
            }
        }
        catch { }

        ApplySelfRootWorldPosition(targetPos);

        try
        {
            foreach (KeyValuePair<FreeControllerV3, Vector3> kv in beforeControlWorldPos)
            {
                FreeControllerV3 fc = kv.Key;
                if (fc == null || fc.control == null)
                    continue;

                Vector3 before = kv.Value;
                Vector3 after = fc.control.position;
                Vector3 actualDelta = after - before;
                Vector3 correction = expectedDelta - actualDelta;

                if (correction.sqrMagnitude > 0.00000001f)
                    fc.control.position = after + correction;
            }
        }
        catch { }
    }

    private void UpdateRootXYZStatus(string state)
    {
        if (rootMoveStatus == null)
            return;

        float x = rootMoveX != null ? rootMoveX.val : 0.0f;
        float y = rootMoveY != null ? rootMoveY.val : 0.0f;
        float z = rootMoveZ != null ? rootMoveZ.val : 0.0f;
        rootMoveStatus.val = "Root XYZ: " + state + " / X=" + F(x) + " / Y=" + F(y) + " / Z=" + F(z);
    }

    private void CreateSelfMoveControlsLeft()
    {
        if (selfMoveForwardBack != null)
            CreateSlider(selfMoveForwardBack, false);

        if (selfMoveLeftRight != null)
            CreateSlider(selfMoveLeftRight, false);

        if (selfMoveHeight != null)
            CreateSlider(selfMoveHeight, false);

        UIDynamicTextField tf = null;
        if (selfMoveStatus != null)
        {
            tf = CreateTextField(selfMoveStatus, false);
            if (tf != null) tf.height = 42f;
        }

        CreateLeftMacroButton("Move Origin Here", delegate()
        {
            CaptureSelfMoveOrigin(true);
        });

        CreateLeftMacroButton("Move Home XZ", delegate()
        {
            ResetSelfMoveSliders();
        });
    }

    private Transform GetSelfMoveRootTransform()
    {
        if (containingAtom == null)
            return null;

        if (containingAtom.mainController != null && containingAtom.mainController.control != null)
            return containingAtom.mainController.control;

        if (containingAtom.mainController != null)
            return containingAtom.mainController.transform;

        return containingAtom.transform;
    }

    private void MarkSelfMoveSliderValuesObserved()
    {
        selfMoveLastForwardBack = selfMoveForwardBack != null ? selfMoveForwardBack.val : 0.0f;
        selfMoveLastLeftRight = selfMoveLeftRight != null ? selfMoveLeftRight.val : 0.0f;
        selfMoveLastHeight = selfMoveHeight != null ? selfMoveHeight.val : 0.0f;
    }

    private void CaptureSelfMoveOrigin(bool resetSliders)
    {
        Transform root = GetSelfMoveRootTransform();
        if (root == null)
        {
            selfMoveBaseCaptured = false;
            if (selfMoveStatus != null)
                selfMoveStatus.val = "Self Move: no self root";
            return;
        }

        // v052: The move origin is a horizontal origin. Capturing/Home must not force height.
        // Keep the current Y untouched; Height is handled as an explicit slider delta only.
        Vector3 current = root.position;
        selfMoveBaseWorldPos = current;
        selfMoveBaseWorldRot = root.rotation;
        selfMoveBaseCaptured = true;

        if (resetSliders)
        {
            ResetSelfMoveHorizontalSlidersNoCapture();
            ApplySelfMoveSliders();
            UpdateSelfMoveStatus("origin captured / height unchanged");
        }
        else
        {
            MarkSelfMoveSliderValuesObserved();
            UpdateSelfMoveStatus("origin captured / no move");
        }
    }

    private void ResetSelfMoveSliders()
    {
        // v052: Home/Reset is horizontal only. Height stays exactly as it is.
        ResetSelfMoveHorizontalSlidersNoCapture();
        ApplySelfMoveSliders();
        UpdateSelfMoveStatus("home / height unchanged");
    }

    private void ResetSelfMoveSlidersNoCapture()
    {
        ResetSelfMoveHorizontalSlidersNoCapture();
        ResetSelfMoveHeightSliderNoCapture();
    }

    private void ResetSelfMoveHorizontalSlidersNoCapture()
    {
        selfMoveSuppressApply = true;
        try
        {
            if (selfMoveForwardBack != null) selfMoveForwardBack.val = 0.0f;
            if (selfMoveLeftRight != null) selfMoveLeftRight.val = 0.0f;
        }
        catch { }
        selfMoveSuppressApply = false;

        selfMoveLastForwardBack = selfMoveForwardBack != null ? selfMoveForwardBack.val : 0.0f;
        selfMoveLastLeftRight = selfMoveLeftRight != null ? selfMoveLeftRight.val : 0.0f;
    }

    private void ResetSelfMoveHeightSliderNoCapture()
    {
        selfMoveSuppressApply = true;
        try
        {
            if (selfMoveHeight != null) selfMoveHeight.val = 0.0f;
        }
        catch { }
        selfMoveSuppressApply = false;

        selfMoveLastHeight = selfMoveHeight != null ? selfMoveHeight.val : 0.0f;
        selfMoveAppliedHeightOffset = 0.0f;
    }

    private void UpdateSelfMoveSliders()
    {
        if (selfMoveSuppressApply)
            return;

        if (selfMoveForwardBack == null || selfMoveLeftRight == null || selfMoveHeight == null)
            return;

        float fb = selfMoveForwardBack.val;
        float lr = selfMoveLeftRight.val;
        float h = selfMoveHeight.val;

        if (Mathf.Abs(fb - selfMoveLastForwardBack) <= 0.0001f &&
            Mathf.Abs(lr - selfMoveLastLeftRight) <= 0.0001f &&
            Mathf.Abs(h - selfMoveLastHeight) <= 0.0001f)
            return;

        selfMoveLastForwardBack = fb;
        selfMoveLastLeftRight = lr;
        selfMoveLastHeight = h;

        ApplySelfMoveSliders();
    }

    private void ApplySelfMoveSliders()
    {
        Transform root = GetSelfMoveRootTransform();
        if (root == null)
            return;

        if (!selfMoveBaseCaptured)
            CaptureSelfMoveOrigin(false);

        float fb = selfMoveForwardBack != null ? selfMoveForwardBack.val : 0.0f;
        float lr = selfMoveLeftRight != null ? selfMoveLeftRight.val : 0.0f;
        float h = selfMoveHeight != null ? selfMoveHeight.val : 0.0f;

        Vector3 forward = selfMoveBaseWorldRot * Vector3.forward;
        forward.y = 0.0f;
        if (forward.sqrMagnitude < 0.000001f)
            forward = Vector3.forward;
        forward.Normalize();

        Vector3 right = selfMoveBaseWorldRot * Vector3.right;
        right.y = 0.0f;
        if (right.sqrMagnitude < 0.000001f)
            right = Vector3.right;
        right.Normalize();

        // v052: Forward/Left are absolute offsets from the captured horizontal origin.
        // Height is NOT restored to origin Y. It only applies the explicit delta since the last height value.
        Vector3 current = root.position;
        Vector3 targetPos = selfMoveBaseWorldPos + (forward * fb) + (right * lr);
        targetPos.y = current.y + (h - selfMoveAppliedHeightOffset);
        selfMoveAppliedHeightOffset = h;

        ApplySelfRootWorldPosition(targetPos);
        UpdateSelfMoveStatus("moving");
    }

    private void ApplySelfRootWorldPosition(Vector3 targetPos)
    {
        try
        {
            if (containingAtom != null && containingAtom.mainController != null && containingAtom.mainController.control != null)
            {
                containingAtom.mainController.control.position = targetPos;
                return;
            }

            if (containingAtom != null && containingAtom.mainController != null && containingAtom.mainController.transform != null)
            {
                containingAtom.mainController.transform.position = targetPos;
                return;
            }

            if (containingAtom != null && containingAtom.transform != null)
                containingAtom.transform.position = targetPos;
        }
        catch { }
    }

    private void UpdateSelfMoveStatus(string state)
    {
        if (selfMoveStatus == null)
            return;

        float fb = selfMoveForwardBack != null ? selfMoveForwardBack.val : 0.0f;
        float lr = selfMoveLeftRight != null ? selfMoveLeftRight.val : 0.0f;
        float h = selfMoveHeight != null ? selfMoveHeight.val : 0.0f;
        selfMoveStatus.val = "Self Move: " + state + " / FB=" + F(fb) + " / LR=" + F(lr) + " / H=" + F(h);
    }

    private void Update()
    {
        UpdatePOSE_TransitionModeFromDockingState();
        UpdateCyclePoseSliderInputAndCapture();
        UpdateTargetFacingYawSliderAndCapture();
        UpdateSelfMoveSliders();
        UpdateRootXYZSliders();

        if (poseTransitionModeActive)
        {
            if (Time.time >= nextPOSE_TransitionInfoPollTime)
            {
                nextPOSE_TransitionInfoPollTime = Time.time + POSE_TRANSITION_INFO_POLL_INTERVAL_SEC;
                RefreshPOSE_InfoText(false);
            }
            return;
        }

        if (Time.time < nextPOSE_InfoPollTime)
            return;

        nextPOSE_InfoPollTime = Time.time + POSE_INFO_POLL_INTERVAL_SEC;
        RefreshPOSE_InfoText(false);
    }

    private void RegisterExternalActions()
    {
        // External trigger names use an HC prefix so they are easy to find in VaM trigger lists.
        RegisterAction(new JSONStorableAction("HC Auto Pose", delegate()
        {
            StartMacro("Auto Pose", MacroAutoPoseRelationLog());
        }));

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

        RegisterAction(new JSONStorableAction("HC Upper Low Hand Up", delegate()
        {
            StartMacro("Upper Low Hand Up", MacroUpperLowHandUpPose());
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

        RegisterAction(new JSONStorableAction("HC Prone Hand Up", delegate()
        {
            StartMacro("Prone Hand Up", MacroProneHandUpPose());
        }));

        RegisterAction(new JSONStorableAction("HC Prone2", delegate()
        {
            StartMacro("Prone2", MacroProne2Pose());
        }));

        RegisterAction(new JSONStorableAction("HC Supine", delegate()
        {
            StartMacro("Supine", MacroSupinePose());
        }));

        RegisterAction(new JSONStorableAction("HC Supine Hand Up", delegate()
        {
            StartMacro("Supine Hand Up", MacroSupineHandUpPose());
        }));

        RegisterAction(new JSONStorableAction("HC Dog", delegate()
        {
            StartMacro("Dog", MacroDogPose());
        }));

        RegisterAction(new JSONStorableAction("HC Face Target", delegate()
        {
            SetTargetFacingYawAndApply(VISUAL_FACE_TARGET_YAW_OFFSET_DEG, true, "external face target");
        }));

        RegisterAction(new JSONStorableAction("HC Butt Target", delegate()
        {
            SetTargetFacingYawAndApply(VISUAL_BUTT_TARGET_YAW_OFFSET_DEG, true, "external butt target");
        }));

        // Compatibility aliases. These now use visual meaning, not root.forward meaning.
        RegisterAction(new JSONStorableAction("HC Turn Front To Target", delegate()
        {
            SetTargetFacingYawAndApply(VISUAL_FACE_TARGET_YAW_OFFSET_DEG, true, "external front alias");
        }));

        RegisterAction(new JSONStorableAction("HC Turn Back To Target", delegate()
        {
            SetTargetFacingYawAndApply(VISUAL_BUTT_TARGET_YAW_OFFSET_DEG, true, "external back alias");
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

        RegisterAction(new JSONStorableAction("HC Self Move Origin Here", delegate()
        {
            CaptureSelfMoveOrigin(true);
        }));

        RegisterAction(new JSONStorableAction("HC Self Move Reset Sliders", delegate()
        {
            ResetSelfMoveSliders();
        }));

        RegisterAction(new JSONStorableAction("HC Root XYZ Origin Here", delegate()
        {
            CaptureRootXYZOrigin(true);
        }));

        RegisterAction(new JSONStorableAction("HC Root XYZ Home", delegate()
        {
            ResetRootXYZSliders();
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

    private void SetMacroButtonBaseColor(string label, Color color)
    {
        if (string.IsNullOrEmpty(label))
            return;

        MacroButtonInfo info;
        if (!macroButtons.TryGetValue(label, out info) || info == null || info.Button == null || info.Button.button == null)
            return;

        ColorBlock cb = info.BaseColors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        info.BaseColors = cb;
        info.Button.button.colors = cb;

        if (info.Graphic != null)
        {
            info.BaseGraphicColor = color;
            info.Graphic.color = color;
        }
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
        if (targetFacingTurnRoutine != null)
        {
            try { StopCoroutine(targetFacingTurnRoutine); } catch { }
            targetFacingTurnRoutine = null;
        }

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

    private void BuildCyclePoseNodes()
    {
        cyclePoseNodes.Clear();
        cyclePoseNodes.Add(new CyclePoseNode(0, "01_Stand"));
        cyclePoseNodes.Add(new CyclePoseNode(1, "02_Stand_Fwd45"));
        cyclePoseNodes.Add(new CyclePoseNode(2, "03_UpperStandMidl"));
        cyclePoseNodes.Add(new CyclePoseNode(3, "04_Sit"));
        cyclePoseNodes.Add(new CyclePoseNode(4, "05_Sit_Fwd45"));
        cyclePoseNodes.Add(new CyclePoseNode(5, "06_UpperMid"));
        cyclePoseNodes.Add(new CyclePoseNode(6, "07_UpperLow"));
        cyclePoseNodes.Add(new CyclePoseNode(7, "08_LegFront"));
        cyclePoseNodes.Add(new CyclePoseNode(8, "09_LegFront_X-45"));
        cyclePoseNodes.Add(new CyclePoseNode(9, "10_Supine"));
        cyclePoseNodes.Add(new CyclePoseNode(10, "11_Supine_HandUp"));
        cyclePoseNodes.Add(new CyclePoseNode(11, "12_UpperLow_HandUp"));
        cyclePoseNodes.Add(new CyclePoseNode(12, "13_UpperLow"));
        cyclePoseNodes.Add(new CyclePoseNode(13, "14_UpperLow_Fwd45"));
        cyclePoseNodes.Add(new CyclePoseNode(14, "15_Dog"));
        cyclePoseNodes.Add(new CyclePoseNode(15, "16_Prone"));
        cyclePoseNodes.Add(new CyclePoseNode(16, "17_Prone_HandUp"));
        cyclePoseNodes.Add(new CyclePoseNode(17, "18_Prone2"));
        cyclePoseNodes.Add(new CyclePoseNode(18, "19_ProneToDog"));
        cyclePoseNodes.Add(new CyclePoseNode(19, "20_UpperLow"));
        cyclePoseNodes.Add(new CyclePoseNode(20, "21_LegMji"));
        cyclePoseNodes.Add(new CyclePoseNode(21, "22_UpperMid"));
        cyclePoseNodes.Add(new CyclePoseNode(22, "23_Stand"));
        cyclePoseNodes.Add(new CyclePoseNode(23, "24_ButtTarget"));
        cyclePoseNodes.Add(new CyclePoseNode(24, "25_ButtTarget_Fwd45"));
        cyclePoseNodes.Add(new CyclePoseNode(25, "26_ButtTarget_Fwd90"));
        cyclePoseNodes.Add(new CyclePoseNode(26, "27_FaceTargetStand"));
        cyclePoseNodes.Add(new CyclePoseNode(27, "28_StandHandUp"));
    }

    private void UpdateCyclePoseSliderInputAndCapture()
    {
        if (cyclePoseSlider == null)
            return;

        if (cyclePoseNodes == null || cyclePoseNodes.Count == 0)
            BuildCyclePoseNodes();

        float maxValue = Mathf.Max(0.0f, (float)(cyclePoseNodes.Count - 1));
        float value = Mathf.Clamp(cyclePoseSlider.val, 0.0f, maxValue);

        if (!cyclePoseSliderInitialized)
        {
            cyclePoseSliderInitialized = true;
            cyclePoseSliderLastObservedValue = value;
            UpdateCyclePoseSliderStatus(value, false, false);
            return;
        }

        if (Mathf.Abs(value - cyclePoseSliderLastObservedValue) > 0.0001f)
        {
            cyclePoseSliderLastObservedValue = value;
            cyclePoseSliderLastChangeTime = Time.time;
            cyclePoseSliderDirty = true;
            CancelActiveMacroForCycleSlider();
            ApplyCyclePoseSliderValue(value);
            UpdateCyclePoseSliderStatus(value, true, false);
        }

        if (cyclePoseSliderDirty && Time.time - cyclePoseSliderLastChangeTime >= CYCLE_SLIDER_CAPTURE_DELAY_SEC)
        {
            cyclePoseSliderDirty = false;
            cyclePoseSliderLastCapturedValue = value;
            InvokeHdcAction("HDC Capture Current", false);
            UpdateCyclePoseSliderStatus(value, false, true);
        }
    }

    private void CancelActiveMacroForCycleSlider()
    {
        if (targetFacingTurnRoutine != null)
        {
            try { StopCoroutine(targetFacingTurnRoutine); } catch { }
            targetFacingTurnRoutine = null;
        }

        if (activeRoutine != null)
        {
            try { StopCoroutine(activeRoutine); } catch { }
            activeRoutine = null;
        }

        RestoreFootIKIfSaved();
        SetHdcLiveApply(false);
        SetHdcEnabled(false);
        ClearActiveMacroButton();
    }

    private void UpdateCyclePoseSliderStatus(float value, bool moving, bool captured)
    {
        if (cyclePoseSliderStatus == null)
            return;

        string fromLabel;
        string toLabel;
        float blend;
        ResolveCyclePoseSliderLabels(value, out fromLabel, out toLabel, out blend);

        string state = captured ? "Captured" : (moving ? "Moving" : "Ready");
        cyclePoseSliderStatus.val = "Cycle Slider: " + state + " / " + F(value) + " / " + fromLabel + " -> " + toLabel + " / blend=" + F(blend * 100.0f) + "%";
    }

    private void SetCyclePoseSliderValueWithoutDirty(float value)
    {
        if (cyclePoseSlider == null)
            return;

        float maxValue = (cyclePoseNodes != null && cyclePoseNodes.Count > 0) ? Mathf.Max(0.0f, (float)(cyclePoseNodes.Count - 1)) : (float)(CYCLE_POSE_COUNT - 1);
        float v = Mathf.Clamp(value, 0.0f, maxValue);
        cyclePoseSlider.val = v;
        cyclePoseSliderLastObservedValue = v;
        cyclePoseSliderDirty = false;
        cyclePoseSliderLastCapturedValue = v;
        UpdateCyclePoseSliderStatus(v, false, true);
    }

    private void ResolveCyclePoseSliderLabels(float value, out string fromLabel, out string toLabel, out float blend)
    {
        fromLabel = "None";
        toLabel = "None";
        blend = 0.0f;

        if (cyclePoseNodes == null || cyclePoseNodes.Count == 0)
            return;

        float maxValue = Mathf.Max(0.0f, (float)(cyclePoseNodes.Count - 1));
        float v = Mathf.Clamp(value, 0.0f, maxValue);
        int fromIndex = Mathf.Clamp(Mathf.FloorToInt(v), 0, cyclePoseNodes.Count - 1);
        int toIndex = Mathf.Clamp(fromIndex + 1, 0, cyclePoseNodes.Count - 1);
        blend = Mathf.Clamp01(v - fromIndex);

        CyclePoseNode from = cyclePoseNodes[fromIndex];
        CyclePoseNode to = cyclePoseNodes[toIndex];
        fromLabel = from != null ? from.Label : "None";
        toLabel = to != null ? to.Label : fromLabel;
    }

    private void ApplyCyclePoseSliderValue(float value)
    {
        if (cyclePoseNodes == null || cyclePoseNodes.Count == 0)
            BuildCyclePoseNodes();

        if (cyclePoseNodes.Count == 0)
            return;

        float maxValue = Mathf.Max(0.0f, (float)(cyclePoseNodes.Count - 1));
        float v = Mathf.Clamp(value, 0.0f, maxValue);
        int fromIndex = Mathf.Clamp(Mathf.FloorToInt(v), 0, cyclePoseNodes.Count - 1);
        int toIndex = Mathf.Clamp(fromIndex + 1, 0, cyclePoseNodes.Count - 1);
        float blend = Mathf.Clamp01(v - fromIndex);

        StandPoseEntry[] fromEntries = BuildCyclePoseNodeEntries(fromIndex);
        StandPoseEntry[] toEntries = BuildCyclePoseNodeEntries(toIndex);
        if (fromEntries == null || fromEntries.Length == 0)
            return;
        if (toEntries == null || toEntries.Length == 0)
            toEntries = fromEntries;

        ApplyBlendedCyclePoseEntries(fromEntries, toEntries, blend);
    }

    private StandPoseEntry[] BuildCyclePoseNodeEntries(int nodeIndex)
    {
        if (cyclePoseNodes == null || nodeIndex < 0 || nodeIndex >= cyclePoseNodes.Count)
            return GetStandPoseEntries();

        CyclePoseNode node = cyclePoseNodes[nodeIndex];
        int step = node != null ? node.Step : nodeIndex;

        switch (step)
        {
            case 0:
                return GetStandPoseEntries();
            case 1:
                return ApplyHipUpperRotXToEntries(GetStandPoseEntries(), 45.0f);
            case 2:
                return GetUpperStandMidlPoseEntries();
            case 3:
                return GetSitPoseEntries();
            case 4:
                return ApplyHipUpperRotXToEntries(GetSitPoseEntries(), 45.0f);
            case 5:
                return GetUpperMidPoseEntries();
            case 6:
                return GetUpperLowPoseEntries();
            case 7:
                return GetUpperLowLegFrontPoseEntries();
            case 8:
                return ApplyHipUpperRotXToEntries(GetUpperLowLegFrontPoseEntries(), -45.0f);
            case 9:
                return GetSupinePoseEntries();
            case 10:
                return GetSupineHandUpPoseEntries();
            case 11:
                return GetUpperLowHandUpPoseEntries();
            case 12:
                return GetUpperLowPoseEntries();
            case 13:
                return ApplyHipUpperRotXToEntries(GetUpperLowPoseEntries(), 45.0f);
            case 14:
                return GetDogPoseEntries();
            case 15:
                return GetPronePoseEntries();
            case 16:
                return GetProneHandUpPoseEntries();
            case 17:
                return GetPronePoseEntries();
            case 18:
                return GetDogPoseEntries();
            case 19:
                return GetUpperLowPoseEntries();
            case 20:
                return GetUpperLowLegMjiPoseEntries();
            case 21:
                return GetUpperMidPoseEntries();
            case 22:
                return GetStandPoseEntries();
            case 23:
                return ApplyTargetNoRootYawToEntries(GetStandPoseEntries(), false);
            case 24:
                return ApplyHipUpperRotXToEntries(ApplyTargetNoRootYawToEntries(GetStandPoseEntries(), false), 45.0f);
            case 25:
                return ApplyHipUpperRotXToEntries(ApplyTargetNoRootYawToEntries(GetStandPoseEntries(), false), 90.0f);
            case 26:
                return ApplyTargetNoRootYawToEntries(GetStandPoseEntries(), true);
            case 27:
                return GetStandHandUpPoseEntries();
        }

        return GetStandPoseEntries();
    }

    private StandPoseEntry[] GetUpperMidPoseEntries()
    {
        return OverrideHipUpperYOnEntries(GetUpperLowPoseEntries(), upperMidY != null ? upperMidY.val : 0.40f);
    }

    private StandPoseEntry[] GetUpperStandMidlPoseEntries()
    {
        return OverrideHipUpperYOnEntries(GetUpperLowPoseEntries(), SIT_HIP_LOCAL_Y);
    }

    private StandPoseEntry[] OverrideHipUpperYOnEntries(StandPoseEntry[] source, float y)
    {
        StandPoseEntry[] copy = ClonePoseEntries(source);
        string[] names = GetLocalHdcTargetsForMode(MODE_HIP_UPPER);
        StandPoseEntry root = FindPoseEntry(copy, GetLocalHdcRootName(MODE_HIP_UPPER));
        if (root == null)
            return copy;

        float deltaY = y - root.LocalPos.y;
        for (int i = 0; i < copy.Length; i++)
        {
            StandPoseEntry e = copy[i];
            if (e == null || !NameInArray(names, e.ControlName))
                continue;
            e.LocalPos = new Vector3(e.LocalPos.x, e.LocalPos.y + deltaY, e.LocalPos.z);
        }
        return copy;
    }

    private StandPoseEntry[] ApplyHipUpperRotXToEntries(StandPoseEntry[] source, float xDeg)
    {
        return ApplyGroupRotXToEntries(source, MODE_HIP_UPPER, xDeg);
    }

    private StandPoseEntry[] ApplyGroupRotXToEntries(StandPoseEntry[] source, string mode, float xDeg)
    {
        StandPoseEntry[] copy = ClonePoseEntries(source);
        if (copy == null || copy.Length == 0)
            return copy;

        string rootName = GetLocalHdcRootName(mode);
        string[] targets = GetLocalHdcTargetsForMode(mode);
        StandPoseEntry root = FindPoseEntry(copy, rootName);
        if (root == null || targets == null || targets.Length == 0)
            return copy;

        Quaternion rootStartRot = NormalizeQuaternion(root.LocalRot);
        Vector3 rootEuler = NormalizeEuler(rootStartRot.eulerAngles);
        Quaternion rootTargetRot = ClosestQuaternionToStart(rootStartRot, Quaternion.Euler(xDeg, rootEuler.y, rootEuler.z));
        Quaternion deltaRot = rootTargetRot * Quaternion.Inverse(rootStartRot);
        Vector3 rootStartPos = root.LocalPos;
        Vector3 rootTargetPos = root.LocalPos;

        for (int i = 0; i < copy.Length; i++)
        {
            StandPoseEntry e = copy[i];
            if (e == null || !NameInArray(targets, e.ControlName))
                continue;

            Vector3 rel = e.LocalPos - rootStartPos;
            e.LocalPos = rootTargetPos + deltaRot * rel;
            e.LocalRot = NormalizeQuaternion(deltaRot * e.LocalRot);
        }

        return copy;
    }

    private StandPoseEntry[] ApplyTargetNoRootYawToEntries(StandPoseEntry[] source, bool faceTarget)
    {
        StandPoseEntry[] copy = ClonePoseEntries(source);
        if (copy == null || copy.Length == 0)
            return copy;

        Atom targetAtom = GetSelectedTargetAtom();
        if (targetAtom == null || containingAtom == null || containingAtom.transform == null)
            return copy;

        Transform targetRoot = GetAtomRootTransform(targetAtom);
        if (targetRoot == null)
            return copy;

        StandPoseEntry hip = FindPoseEntry(copy, "hipControl");
        if (hip == null)
            return copy;

        Transform root = containingAtom.transform;
        Vector3 originWorld = root.TransformPoint(hip.LocalPos);
        Vector3 dirWorld = targetRoot.position - originWorld;
        dirWorld.y = 0.0f;
        if (dirWorld.magnitude < 0.001f)
            return copy;

        // Actual tested mapping: local/root forward visually corresponds to the face side.
        // Butt Target therefore uses the opposite direction.
        if (!faceTarget)
            dirWorld = -dirWorld;

        Vector3 desiredLocalDir = root.InverseTransformDirection(dirWorld.normalized);
        desiredLocalDir.y = 0.0f;
        if (desiredLocalDir.magnitude < 0.001f)
            return copy;

        desiredLocalDir.Normalize();
        float deltaYaw = Vector3.SignedAngle(Vector3.forward, desiredLocalDir, Vector3.up);
        Quaternion deltaRot = Quaternion.Euler(0.0f, deltaYaw, 0.0f);
        Vector3 pivot = hip.LocalPos;
        string[] names = new string[]
        {
            "hipControl", "chestControl", "headControl",
            "lHandControl", "rHandControl", "lElbowControl", "rElbowControl",
            "lKneeControl", "rKneeControl", "lFootControl", "rFootControl"
        };

        for (int i = 0; i < copy.Length; i++)
        {
            StandPoseEntry e = copy[i];
            if (e == null || !NameInArray(names, e.ControlName))
                continue;

            e.LocalPos = pivot + deltaRot * (e.LocalPos - pivot);
            e.LocalRot = NormalizeQuaternion(deltaRot * e.LocalRot);
        }

        return copy;
    }

    private void ApplyBlendedCyclePoseEntries(StandPoseEntry[] fromEntries, StandPoseEntry[] toEntries, float blend)
    {
        blend = Mathf.Clamp01(blend);
        SetHdcLiveApply(false);
        SetHdcEnabled(false);

        for (int i = 0; i < fromEntries.Length; i++)
        {
            StandPoseEntry a = fromEntries[i];
            if (a == null || string.IsNullOrEmpty(a.ControlName))
                continue;

            StandPoseEntry b = FindPoseEntry(toEntries, a.ControlName);
            if (b == null)
                b = a;

            FreeControllerV3 fc = FindController(a.ControlName);
            if (fc == null || fc.control == null)
                continue;

            try
            {
                fc.currentPositionState = FreeControllerV3.PositionState.On;
                fc.currentRotationState = FreeControllerV3.RotationState.On;
            }
            catch { }

            Quaternion targetB = ClosestQuaternionToStart(a.LocalRot, b.LocalRot);
            fc.control.localPosition = Vector3.Lerp(a.LocalPos, b.LocalPos, blend);
            fc.control.localRotation = Quaternion.Slerp(a.LocalRot, targetB, blend);
        }
    }

    private StandPoseEntry[] ClonePoseEntries(StandPoseEntry[] source)
    {
        if (source == null)
            return new StandPoseEntry[0];

        StandPoseEntry[] copy = new StandPoseEntry[source.Length];
        for (int i = 0; i < source.Length; i++)
        {
            StandPoseEntry e = source[i];
            if (e == null)
                continue;
            copy[i] = new StandPoseEntry(e.ControlName, e.LocalPos.x, e.LocalPos.y, e.LocalPos.z, e.LocalRot.x, e.LocalRot.y, e.LocalRot.z, e.LocalRot.w);
        }
        return copy;
    }

    private StandPoseEntry FindPoseEntry(StandPoseEntry[] entries, string controlName)
    {
        if (entries == null || string.IsNullOrEmpty(controlName))
            return null;

        for (int i = 0; i < entries.Length; i++)
        {
            StandPoseEntry e = entries[i];
            if (e != null && e.ControlName == controlName)
                return e;
        }
        return null;
    }

    private bool NameInArray(string[] names, string name)
    {
        if (names == null || string.IsNullOrEmpty(name))
            return false;

        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] == name)
                return true;
        }
        return false;
    }

    private IEnumerator MacroCyclePose()
    {
        cyclePoseIndex = (cyclePoseIndex + 1) % CYCLE_POSE_COUNT;
        SetCyclePoseSliderValueWithoutDirty(cyclePoseIndex);
        yield return StartCoroutine(RunCyclePoseStep(cyclePoseIndex));
    }

    private IEnumerator MacroCycleBack()
    {
        if (cyclePoseIndex <= 0)
            cyclePoseIndex = CYCLE_POSE_COUNT - 1;
        else
            cyclePoseIndex = (cyclePoseIndex - 1) % CYCLE_POSE_COUNT;

        SetCyclePoseSliderValueWithoutDirty(cyclePoseIndex);
        yield return StartCoroutine(RunCyclePoseStep(cyclePoseIndex));
    }

    private IEnumerator MacroCycleReset()
    {
        cyclePoseIndex = -1;
        SetCyclePoseSliderValueWithoutDirty(0.0f);
        SetStatus("Cycle Pose index reset");
        yield return null;
    }

    private IEnumerator MacroCycleDirectStep(int step, string label)
    {
        int clampedStep = Mathf.Clamp(step, 0, CYCLE_POSE_COUNT - 1);
        cyclePoseIndex = clampedStep;
        SetCyclePoseSliderValueWithoutDirty(clampedStep);
        SetStatus("Cycle direct: " + label + " / step=" + (clampedStep + 1).ToString("00", CultureInfo.InvariantCulture));
        yield return StartCoroutine(RunCyclePoseStep(clampedStep));
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

            case 13:
                // Upper X +45 depends on Upper Low in the cycle route.
                yield return StartCoroutine(MacroUpperLowPose());
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                yield break;

            case 24:
                // Butt Target +45 depends on the previous Butt Target step.
                yield return StartCoroutine(MacroFaceTarget(false));
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                yield break;

            case 25:
                // Butt Target +90 depends on the previous Butt Target step.
                yield return StartCoroutine(MacroFaceTarget(false));
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
        if (poseKey == POSE_UPPER_LOW_HAND_UP)
        {
            yield return StartCoroutine(MacroUpperLowHandUpPose());
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
        if (poseKey == POSE_PRONE_HAND_UP)
        {
            yield return StartCoroutine(MacroProneHandUpPose());
            yield break;
        }
        if (poseKey == POSE_SUPINE)
        {
            yield return StartCoroutine(MacroSupinePose());
            yield break;
        }
        if (poseKey == POSE_SUPINE_HAND_UP)
        {
            yield return StartCoroutine(MacroSupineHandUpPose());
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
                SetStatus("Cycle Pose: Supine Hand Up");
                yield return StartCoroutine(MacroSupineHandUpPose());
                break;

            case 11:
                SetStatus("Cycle Pose: Upper Low Hand Up");
                yield return StartCoroutine(MacroUpperLowHandUpPose());
                break;

            case 12:
                SetStatus("Cycle Pose: Upper Low");
                yield return StartCoroutine(MacroUpperLowPose());
                break;

            case 13:
                SetStatus("Cycle Pose: Upper X +45");
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                break;

            case 14:
                SetStatus("Cycle Pose: Dog");
                yield return StartCoroutine(MacroDogPose());
                break;

            case 15:
                SetStatus("Cycle Pose: Prone");
                yield return StartCoroutine(MacroPronePose());
                break;

            case 16:
                SetStatus("Cycle Pose: Prone Hand Up");
                yield return StartCoroutine(MacroProneHandUpPose());
                break;

            case 17:
                SetStatus("Cycle Pose: Prone2");
                yield return StartCoroutine(MacroProne2Pose());
                break;

            case 18:
                SetStatus("Cycle Pose: Prone -> Dog direct");
                yield return StartCoroutine(MacroDogPose());
                break;

            case 19:
                SetStatus("Cycle Pose: Upper Low");
                yield return StartCoroutine(MacroUpperLowPose());
                break;

            case 20:
                SetStatus("Cycle Pose: Upper Low Leg Mji");
                yield return StartCoroutine(MacroUpperLowLegMjiPose());
                break;

            case 21:
                SetStatus("Cycle Pose: Upper Mid");
                yield return StartCoroutine(MacroUpperYWithUpperLowRoute("Upper Mid", upperMidY != null ? upperMidY.val : 0.40f, POSE_UPPER_MID));
                break;

            case 22:
                SetStatus("Cycle Pose: Stand");
                yield return StartCoroutine(MacroStandPose());
                break;

            case 23:
                SetStatus("Cycle Pose: Butt Target");
                yield return StartCoroutine(MacroFaceTarget(false));
                break;

            case 24:
                SetStatus("Cycle Pose: Butt Target +45");
                yield return StartCoroutine(MacroUpperX("Upper X 45", upperX45Deg != null ? upperX45Deg.val : 45f));
                break;

            case 25:
                SetStatus("Cycle Pose: Butt Target +90");
                yield return StartCoroutine(MacroUpperX("Upper X 90", upperX90Deg != null ? upperX90Deg.val : 90f));
                break;

            case 26:
                SetStatus("Cycle Pose: Face Target + Stand");
                yield return StartCoroutine(MacroUpperX("Upper X 0", 0f));
                yield return StartCoroutine(MacroFaceTarget(true));
                yield return StartCoroutine(MacroStandPose());
                break;

            case 27:
                SetStatus("Cycle Pose: Stand Hand Up");
                yield return StartCoroutine(MacroStandHandUpPose());
                break;
        }
    }


    private bool TryExecutePosePresetAction(string[] actionNames)
    {
        if (containingAtom == null || actionNames == null)
            return false;

        foreach (string storableId in containingAtom.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId))
                continue;

            if (storableId.IndexOf("PosePresets", StringComparison.OrdinalIgnoreCase) < 0)
                continue;

            JSONStorable storable = containingAtom.GetStorableByID(storableId);
            if (storable == null)
                continue;

            for (int i = 0; i < actionNames.Length; i++)
            {
                string actionName = actionNames[i];
                if (string.IsNullOrEmpty(actionName))
                    continue;

                JSONStorableAction action = storable.GetAction(actionName);
                if (action == null)
                    continue;

                action.actionCallback.Invoke();
                LogDebug("Load User Defaults fallback: pose action=" + storableId + " / " + actionName);
                return true;
            }
        }

        return false;
    }

    private IEnumerator MacroLoadUserDefaults()
    {
        SetStatus("Load User Defaults start");

        bool ok = false;
        if (hdc != null || ResolveHDC(false))
        {
            SetHdcEnabled(true);
            SetHdcLiveApply(false);
            ok = InvokeHdcAction("HDC Load VaM User Defaults", false);
        }

        if (!ok)
        {
            ok = TryExecutePosePresetAction(new string[]
            {
                "Load User Defaults",
                "LoadUserDefaults",
                "Load User Default",
                "LoadUserDefault",
                "Load Defaults",
                "LoadDefaults"
            });
        }

        if (!ok)
        {
            SetStatus("Load User Defaults failed: PosePresets action not found");
            yield break;
        }

        yield return null;
        yield return null;

        InvokeHdcAction("HDC Capture Current", false);
        currentPoseKey = POSE_NONE;
        SetStatus("Load User Defaults done" + (hdc == null ? " / HDC optional" : ""));
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
        // Prone full-body pose replaced in v070.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",  0.014f,  0.284f, -0.024f,  0.740f,  0.026f,  0.113f,  0.662f),
            new StandPoseEntry("chestControl",  0.008f,  0.359f,  0.184f,  0.511f,  0.123f, -0.057f,  0.849f),
            new StandPoseEntry("headControl", -0.089f,  0.284f,  0.561f,  0.411f,  0.513f,  0.436f,  0.615f),
            new StandPoseEntry("rHandControl",  0.156f,  0.161f,  0.534f, -0.107f, -0.909f, -0.079f,  0.395f),
            new StandPoseEntry("lHandControl",  0.000f,  0.196f,  0.623f, -0.014f, -0.999f, -0.045f, -0.020f),
            new StandPoseEntry("rFootControl",  0.144f,  0.217f, -0.890f,  0.982f, -0.181f, -0.032f,  0.034f),
            new StandPoseEntry("lFootControl",  0.008f,  0.222f, -0.903f, -0.995f, -0.027f, -0.088f,  0.049f),
            new StandPoseEntry("rKneeControl",  0.159f, -0.003f, -0.413f,  0.849f,  0.068f,  0.020f,  0.524f),
            new StandPoseEntry("lKneeControl", -0.005f, -0.001f, -0.425f,  0.844f,  0.057f,  0.132f,  0.517f),
            new StandPoseEntry("rElbowControl",  0.254f,  0.245f,  0.209f, -0.018f,  0.737f,  0.310f, -0.600f),
            new StandPoseEntry("lElbowControl", -0.163f,  0.156f,  0.629f,  0.252f, -0.955f, -0.111f, -0.106f)
        };
    }

    private StandPoseEntry[] GetProneHandUpPoseEntries()
    {
        // Cycle step 17 inserted in v072 under 16_Prone.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",    -0.002f,  0.312f,  0.040f, -0.603f, -0.305f, -0.248f, -0.695f),
            new StandPoseEntry("chestControl",  0.031f,  0.294f,  0.234f,  0.494f,  0.608f,  0.538f,  0.312f),
            new StandPoseEntry("headControl",   0.184f,  0.208f,  0.567f,  0.455f,  0.566f,  0.452f,  0.518f),
            new StandPoseEntry("rHandControl",  0.223f,  0.485f,  0.052f,  0.041f,  0.333f, -0.279f,  0.900f),
            new StandPoseEntry("lHandControl",  0.328f,  0.194f,  0.343f,  0.514f, -0.100f, -0.802f, -0.287f),
            new StandPoseEntry("rFootControl", -0.091f,  0.298f, -0.715f,  0.228f,  0.964f,  0.045f, -0.127f),
            new StandPoseEntry("lFootControl", -0.048f,  0.189f, -0.831f, -0.256f, -0.930f, -0.236f, -0.117f),
            new StandPoseEntry("rKneeControl",  0.256f,  0.340f, -0.457f, -0.453f, -0.850f, -0.265f, -0.045f),
            new StandPoseEntry("lKneeControl",  0.250f,  0.311f, -0.441f, -0.416f, -0.774f, -0.458f, -0.136f),
            new StandPoseEntry("rElbowControl",-0.003f,  0.532f,  0.192f,  0.077f,  0.289f, -0.091f,  0.950f),
            new StandPoseEntry("lElbowControl", 0.242f,  0.199f,  0.161f, -0.571f, -0.004f,  0.821f, -0.010f)
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

    private StandPoseEntry[] GetSupineHandUpPoseEntries()
    {
        // Supine Hand Up pose added in v070, updated in v070 r2.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",    -0.002f,  0.035f,  0.054f, -0.651f, -0.017f, -0.013f,  0.758f),
            new StandPoseEntry("chestControl",  0.006f, -0.017f, -0.147f, -0.756f, -0.012f, -0.012f,  0.654f),
            new StandPoseEntry("headControl",   0.020f, -0.065f, -0.462f, -0.609f, -0.018f, -0.012f,  0.793f),
            new StandPoseEntry("rHandControl",  0.124f,  0.117f, -0.286f,  0.220f,  0.870f, -0.235f,  0.374f),
            new StandPoseEntry("lHandControl", -0.117f,  0.179f, -0.267f, -0.212f,  0.884f, -0.245f, -0.336f),
            new StandPoseEntry("rFootControl",  0.170f, -0.051f,  0.990f, -0.165f,  0.046f,  0.166f,  0.971f),
            new StandPoseEntry("lFootControl", -0.254f, -0.051f,  0.972f,  0.157f,  0.087f,  0.172f, -0.968f),
            new StandPoseEntry("rKneeControl",  0.066f,  0.073f,  0.566f,  0.572f,  0.066f, -0.165f, -0.801f),
            new StandPoseEntry("lKneeControl", -0.115f,  0.074f,  0.558f, -0.562f,  0.034f, -0.192f,  0.804f),
            new StandPoseEntry("rElbowControl", 0.258f,  0.058f, -0.057f, -0.225f, -0.831f,  0.213f, -0.461f),
            new StandPoseEntry("lElbowControl",-0.244f,  0.054f, -0.068f,  0.318f, -0.852f,  0.174f,  0.377f)
        };
    }

    private StandPoseEntry[] GetUpperLowHandUpPoseEntries()
    {
        // Cycle step 12 inserted in v070 r3 under 11_Supine_HandUp.
        // v071: updated symmetric hand/elbow/foot pose provided by user.
        return new StandPoseEntry[]
        {
            new StandPoseEntry("hipControl",     0.000f,  0.204f, -0.003f,  0.000f,  0.000f,  0.000f,  1.000f),
            new StandPoseEntry("chestControl",  0.000f,  0.376f, -0.057f,  0.004f,  0.000f,  0.000f, -1.000f),
            new StandPoseEntry("headControl",   0.000f,  0.767f, -0.020f,  0.009f,  0.000f,  0.000f,  1.000f),
            new StandPoseEntry("rHandControl",  0.092f,  0.557f,  0.251f,  0.065f,  0.817f,  0.362f,  0.444f),
            new StandPoseEntry("lHandControl", -0.092f,  0.557f,  0.251f,  0.065f, -0.817f, -0.362f,  0.444f),
            new StandPoseEntry("rFootControl",  0.468f, -0.032f, -0.015f,  0.820f,  0.281f,  0.259f,  0.426f),
            new StandPoseEntry("lFootControl", -0.417f, -0.034f, -0.019f, -0.820f,  0.281f,  0.259f, -0.426f),
            new StandPoseEntry("rKneeControl",  0.253f, -0.037f,  0.376f,  0.696f,  0.230f,  0.178f,  0.657f),
            new StandPoseEntry("lKneeControl", -0.253f, -0.037f,  0.376f,  0.696f, -0.230f, -0.178f,  0.657f),
            new StandPoseEntry("rElbowControl", 0.292f,  0.469f,  0.183f,  0.267f,  0.839f,  0.475f,  0.001f),
            new StandPoseEntry("lElbowControl",-0.292f,  0.469f,  0.183f,  0.267f, -0.839f, -0.475f,  0.001f)
        };
    }

    private bool IsPronePoseKey(string poseKey)
    {
        return poseKey == POSE_PRONE || poseKey == POSE_PRONE_HAND_UP;
    }

    private bool IsSupinePoseKey(string poseKey)
    {
        return poseKey == POSE_SUPINE || poseKey == POSE_SUPINE_HAND_UP;
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

    private IEnumerator MacroUpperLowHandUpPose()
    {
        yield return StartCoroutine(MacroDirectPoseEntries("Upper Low Hand Up", GetUpperLowHandUpPoseEntries()));
        currentPoseKey = POSE_UPPER_LOW_HAND_UP;
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
        if (!IsPronePoseKey(currentPoseKey) && currentPoseKey != POSE_UPPER_LOW && currentPoseKey != POSE_DOG)
        {
            if (IsSupinePoseKey(currentPoseKey))
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

    private IEnumerator MacroProneHandUpPose()
    {
        if (!IsPronePoseKey(currentPoseKey))
        {
            yield return StartCoroutine(MacroPronePose());
        }

        yield return StartCoroutine(MacroDirectPoseEntries("Prone Hand Up", GetProneHandUpPoseEntries()));
        currentPoseKey = POSE_PRONE_HAND_UP;
    }

    private IEnumerator MacroProne2Pose()
    {
        yield return StartCoroutine(MacroDirectPoseEntries("Prone2", GetPronePoseEntries()));
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

    private IEnumerator MacroSupineHandUpPose()
    {
        if (!IsSupinePoseKey(currentPoseKey))
        {
            yield return StartCoroutine(MacroSupinePose());
        }

        yield return StartCoroutine(MacroDirectPoseEntries("Supine Hand Up", GetSupineHandUpPoseEntries()));
        currentPoseKey = POSE_SUPINE_HAND_UP;
    }

    private IEnumerator MacroDogPose()
    {
        if (currentPoseKey == POSE_PRONE)
        {
            yield return StartCoroutine(MacroDirectPoseEntries("Dog", GetDogPoseEntries()));
            currentPoseKey = POSE_DOG;
            yield break;
        }

        if (IsSupinePoseKey(currentPoseKey))
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
        if (IsSupinePoseKey(currentPoseKey))
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
        if (IsSupinePoseKey(currentPoseKey))
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

    private IEnumerator MacroAutoPoseRelationLog()
    {
        Atom targetAtom = GetSelectedTargetAtom();
        if (targetAtom == null)
        {
            SetStatus("Auto Pose failed: target not selected");
            RefreshPOSE_InfoText(true);
            yield break;
        }

        POSE_SpatialRelationResult relation;
        POSE_PostureResult selfPosture;
        POSE_PostureResult targetPosture;
        POSE_DistanceRelationResult distanceRelation;
        bool relationOk = DetectPOSE_SpatialRelation(targetAtom, out relation);
        bool selfPostureOk = DetectPOSE_SelfPosture(out selfPosture);
        bool targetPostureOk = DetectPOSE_TargetPosture(targetAtom, out targetPosture);
        UpdatePOSE_DistanceCalibrationIfNeeded(targetAtom, relation);
        bool distanceOk = DetectPOSE_DistanceRelation(targetAtom, relation, out distanceRelation);

        if (!relationOk && !selfPostureOk && !targetPostureOk && !distanceOk)
        {
            SetStatus("Auto Pose failed: POSE detection unavailable");
            RefreshPOSE_InfoText(true);
            yield break;
        }

        string msg = "[humanPoseControler] [POSE DETECT] " + BuildPOSE_DetectionLogSummary(relation, selfPosture, targetPosture, distanceRelation) + BuildPOSE_TransitionLogSuffix();
        SuperController.LogMessage(msg);
        RefreshPOSE_InfoText(true);
        SetStatus("Auto Pose: " + BuildPOSE_DetectionStatusSummary(relation, selfPosture, targetPosture, distanceRelation));
        yield return null;
    }

    private void RefreshPOSE_InfoText(bool force)
    {
        if (poseTransitionModeActive)
        {
            RefreshPOSE_TransitionInfoText(force);
            return;
        }

        string selfPostureDisplay = "POSE_SelfPosture: NoTarget";
        string targetPostureDisplay = "POSE_TargetPosture: NoTarget";
        string relationDisplay = "POSE_SpatialRelation: NoTarget";
        string distanceDisplay = "POSE_DistanceRelation: NoTarget";

        Atom targetAtom = GetSelectedTargetAtom();
        if (targetAtom != null)
        {
            POSE_PostureResult selfPosture;
            if (DetectPOSE_SelfPosture(out selfPosture) && selfPosture != null)
                selfPostureDisplay = "POSE_SelfPosture: " + selfPosture.Display;
            else
                selfPostureDisplay = "POSE_SelfPosture: Unknown";

            POSE_PostureResult targetPosture;
            if (DetectPOSE_TargetPosture(targetAtom, out targetPosture) && targetPosture != null)
                targetPostureDisplay = "POSE_TargetPosture: " + targetPosture.Display;
            else
                targetPostureDisplay = "POSE_TargetPosture: Unknown";

            POSE_SpatialRelationResult relation;
            if (DetectPOSE_SpatialRelation(targetAtom, out relation) && relation != null)
            {
                relationDisplay = "POSE_SpatialRelation: " + relation.Display;
                UpdatePOSE_DistanceCalibrationIfNeeded(targetAtom, relation);

                POSE_DistanceRelationResult distanceRelation;
                if (DetectPOSE_DistanceRelation(targetAtom, relation, out distanceRelation) && distanceRelation != null)
                    distanceDisplay = "POSE_DistanceRelation: " + distanceRelation.Display + " " + F(distanceRelation.HipHorizontalDistance) + "m";
                else
                    distanceDisplay = "POSE_DistanceRelation: Unknown";
            }
            else
            {
                relationDisplay = "POSE_SpatialRelation: Unknown";
                distanceDisplay = "POSE_DistanceRelation: Unknown";
            }
        }
        else
        {
            lastPOSE_SpatialRelationCodeForDistanceCalibration = "";
        }

        if (force || selfPostureDisplay != lastPOSE_SelfPostureDisplay)
        {
            lastPOSE_SelfPostureDisplay = selfPostureDisplay;
            if (poseSelfPostureText != null)
                poseSelfPostureText.val = selfPostureDisplay;
        }

        if (force || targetPostureDisplay != lastPOSE_TargetPostureDisplay)
        {
            lastPOSE_TargetPostureDisplay = targetPostureDisplay;
            if (poseTargetPostureText != null)
                poseTargetPostureText.val = targetPostureDisplay;
        }

        if (force || relationDisplay != lastPOSE_SpatialRelationDisplay)
        {
            lastPOSE_SpatialRelationDisplay = relationDisplay;
            if (poseSpatialRelationText != null)
                poseSpatialRelationText.val = relationDisplay;
        }

        if (force || distanceDisplay != lastPOSE_DistanceRelationDisplay)
        {
            lastPOSE_DistanceRelationDisplay = distanceDisplay;
            if (poseDistanceRelationText != null)
                poseDistanceRelationText.val = distanceDisplay;
        }

        RefreshPOSE_TransitionInactiveText(force);
    }

    private void UpdatePOSE_Text(JSONStorableString target, ref string lastDisplay, string nextDisplay, bool force)
    {
        if (target == null)
            return;

        if (force || nextDisplay != lastDisplay)
        {
            lastDisplay = nextDisplay;
            target.val = nextDisplay;
        }
    }

    private void RefreshPOSE_TransitionInactiveText(bool force)
    {
        UpdatePOSE_Text(poseTransitionModeText, ref lastPOSE_TransitionModeDisplay, "POSE_TransitionMode: Inactive", force);
        UpdatePOSE_Text(poseTransitionIntentText, ref lastPOSE_TransitionIntentDisplay, "POSE_TransitionIntent: Neutral", force);
        UpdatePOSE_Text(poseTransitionBlendText, ref lastPOSE_TransitionBlendDisplay, "POSE_TransitionBlend: 0%", force);
        UpdatePOSE_Text(poseTransitionPathText, ref lastPOSE_TransitionPathDisplay, "POSE_TransitionPath: None", force);
    }

    private void RefreshPOSE_TransitionInfoText(bool force)
    {
        DOCKING_PoseAssistState docking;
        bool hasDocking = TryReadPOSE_DOCKING_PoseAssistState(out docking);

        float currentDistance = hasDocking ? docking.CurrentDistance : poseTransitionBaseDistance;
        float delta = hasDocking ? docking.DistanceDelta : currentDistance - poseTransitionBaseDistance;

        float deadZone = poseTransitionDeadZone != null ? Mathf.Clamp(poseTransitionDeadZone.val, 0.0f, 0.15f) : POSE_TRANSITION_DEAD_ZONE_DEFAULT;
        float fullDistance = poseTransitionFullDistance != null ? Mathf.Clamp(poseTransitionFullDistance.val, 0.15f, 0.80f) : POSE_TRANSITION_FULL_DISTANCE_DEFAULT;
        if (fullDistance <= deadZone + 0.01f)
            fullDistance = deadZone + 0.01f;

        string currentIntent = "Neutral";
        if (delta < -deadZone) currentIntent = "PushNear";
        else if (delta > deadZone) currentIntent = "PullFar";

        if (!poseTransitionIntentLocked && currentIntent != "Neutral")
        {
            poseTransitionIntentLocked = true;
            poseTransitionLockedIntent = currentIntent;
            poseTransitionLockedPath = DecidePOSE_TransitionPath(poseTransitionLockedSelfPostureCode, poseTransitionLockedSpatialRelationCode, poseTransitionLockedIntent);
            SuperController.LogMessage("[humanPoseControler] [POSE TRANSITION] path locked / intent=" + poseTransitionLockedIntent + " / path=" + poseTransitionLockedPath + " / baseDistance=" + F(poseTransitionBaseDistance) + " / currentDistance=" + F(currentDistance));
        }

        float signedAmount = 0.0f;
        if (poseTransitionIntentLocked)
        {
            if (poseTransitionLockedIntent == "PushNear") signedAmount = -delta;
            else if (poseTransitionLockedIntent == "PullFar") signedAmount = delta;
        }

        float blend = 0.0f;
        if (poseTransitionIntentLocked && signedAmount > deadZone)
            blend = Mathf.Clamp01((signedAmount - deadZone) / (fullDistance - deadZone));

        string intentDisplay = poseTransitionIntentLocked ? poseTransitionLockedIntent : currentIntent;
        if (poseTransitionIntentLocked && currentIntent != "Neutral" && currentIntent != poseTransitionLockedIntent)
            intentDisplay = poseTransitionLockedIntent + " (opposite ignored)";

        string selfPostureDisplay = "POSE_SelfPosture: Locked " + poseTransitionLockedSelfPostureDisplay;
        string targetPostureDisplay = "POSE_TargetPosture: Locked " + poseTransitionLockedTargetPostureDisplay;
        string relationDisplay = "POSE_SpatialRelation: Locked " + poseTransitionLockedSpatialRelationDisplay;
        string distanceDisplay = "POSE_DistanceRelation: Locked " + poseTransitionLockedDistanceRelationDisplay;
        string modeDisplay = "POSE_TransitionMode: Active " + poseTransitionDockingMode;
        string intentText = "POSE_TransitionIntent: " + intentDisplay;
        string blendText = "POSE_TransitionBlend: " + Mathf.RoundToInt(blend * 100.0f).ToString(CultureInfo.InvariantCulture) + "% delta=" + F(delta) + "m";
        string pathText = "POSE_TransitionPath: " + (poseTransitionIntentLocked ? poseTransitionLockedPath : "WaitingDirection");

        UpdatePOSE_Text(poseSelfPostureText, ref lastPOSE_SelfPostureDisplay, selfPostureDisplay, force);
        UpdatePOSE_Text(poseTargetPostureText, ref lastPOSE_TargetPostureDisplay, targetPostureDisplay, force);
        UpdatePOSE_Text(poseSpatialRelationText, ref lastPOSE_SpatialRelationDisplay, relationDisplay, force);
        UpdatePOSE_Text(poseDistanceRelationText, ref lastPOSE_DistanceRelationDisplay, distanceDisplay, force);
        UpdatePOSE_Text(poseTransitionModeText, ref lastPOSE_TransitionModeDisplay, modeDisplay, force);
        UpdatePOSE_Text(poseTransitionIntentText, ref lastPOSE_TransitionIntentDisplay, intentText, force);
        UpdatePOSE_Text(poseTransitionBlendText, ref lastPOSE_TransitionBlendDisplay, blendText, force);
        UpdatePOSE_Text(poseTransitionPathText, ref lastPOSE_TransitionPathDisplay, pathText, force);
    }

    private void UpdatePOSE_TransitionModeFromDockingState()
    {
        DOCKING_PoseAssistState docking;
        bool hasDocking = TryReadPOSE_DOCKING_PoseAssistState(out docking);

        if (hasDocking && docking.Valid && docking.Active && docking.EventTime > poseTransitionLastDockingEventTime + POSE_TRANSITION_DOCKING_EVENT_EPS)
        {
            BeginPOSE_TransitionModeFromDocking(docking);
            return;
        }

        if (poseTransitionModeActive)
        {
            if (!hasDocking || !docking.Valid || !docking.Active)
            {
                EndPOSE_TransitionMode("docking-assist-ended");
            }
        }
    }

    private void BeginPOSE_TransitionModeFromDocking(DOCKING_PoseAssistState docking)
    {
        Atom targetAtom = GetSelectedTargetAtom();
        if (targetAtom == null || docking == null || !docking.Valid)
            return;

        POSE_SpatialRelationResult relation;
        POSE_PostureResult selfPosture;
        POSE_PostureResult targetPosture;
        POSE_DistanceRelationResult distanceRelation;
        bool relationOk = DetectPOSE_SpatialRelation(targetAtom, out relation);
        bool selfPostureOk = DetectPOSE_SelfPosture(out selfPosture);
        bool targetPostureOk = DetectPOSE_TargetPosture(targetAtom, out targetPosture);
        UpdatePOSE_DistanceCalibrationIfNeeded(targetAtom, relation);
        bool distanceOk = DetectPOSE_DistanceRelation(targetAtom, relation, out distanceRelation);

        poseTransitionModeActive = true;
        poseTransitionStartTime = Time.time;
        poseTransitionLastDockingEventTime = docking.EventTime;
        poseTransitionBaseDistance = docking.BaseDistance;
        poseTransitionDockingMode = string.IsNullOrEmpty(docking.Mode) ? "Unknown" : docking.Mode;
        poseTransitionIntentLocked = false;
        poseTransitionLockedIntent = "Neutral";
        poseTransitionLockedPath = "None";

        poseTransitionLockedSelfPostureCode = selfPostureOk && selfPosture != null ? selfPosture.Code : "POSE_SelfPosture_Unknown";
        poseTransitionLockedSelfPostureDisplay = selfPostureOk && selfPosture != null ? selfPosture.Display : "Unknown";
        poseTransitionLockedTargetPostureCode = targetPostureOk && targetPosture != null ? targetPosture.Code : "POSE_TargetPosture_Unknown";
        poseTransitionLockedTargetPostureDisplay = targetPostureOk && targetPosture != null ? targetPosture.Display : "Unknown";
        poseTransitionLockedSpatialRelationCode = relationOk && relation != null ? relation.Code : "POSE_SpatialRelation_Unknown";
        poseTransitionLockedSpatialRelationDisplay = relationOk && relation != null ? relation.Display : "Unknown";
        poseTransitionLockedDistanceRelationCode = distanceOk && distanceRelation != null ? distanceRelation.Code : "POSE_DistanceRelation_Unknown";
        poseTransitionLockedDistanceRelationDisplay = distanceOk && distanceRelation != null ? distanceRelation.Display : "Unknown";

        nextPOSE_TransitionInfoPollTime = 0f;

        SuperController.LogMessage("[humanPoseControler] [POSE TRANSITION] start / dockingMode=" + poseTransitionDockingMode + " / baseDistance=" + F(poseTransitionBaseDistance) + " / lockedSelfPosture=" + poseTransitionLockedSelfPostureCode + " / lockedTargetPosture=" + poseTransitionLockedTargetPostureCode + " / lockedSpatialRelation=" + poseTransitionLockedSpatialRelationCode + " / lockedDistanceRelation=" + poseTransitionLockedDistanceRelationCode);
        SetStatus("POSE Transition start: " + poseTransitionLockedSelfPostureDisplay + " / " + poseTransitionDockingMode);
        RefreshPOSE_InfoText(true);
    }

    private void EndPOSE_TransitionMode(string reason)
    {
        if (!poseTransitionModeActive)
            return;

        SuperController.LogMessage("[humanPoseControler] [POSE TRANSITION] end / reason=" + reason + " / intent=" + poseTransitionLockedIntent + " / path=" + poseTransitionLockedPath);
        poseTransitionModeActive = false;
        poseTransitionIntentLocked = false;
        poseTransitionLockedIntent = "Neutral";
        poseTransitionLockedPath = "None";
        nextPOSE_InfoPollTime = 0f;
        RefreshPOSE_InfoText(true);
    }

    private bool TryReadPOSE_DOCKING_PoseAssistState(out DOCKING_PoseAssistState state)
    {
        state = null;
        if (containingAtom == null)
            return false;

        try
        {
            foreach (string storableId in containingAtom.GetStorableIDs())
            {
                if (string.IsNullOrEmpty(storableId))
                    continue;

                JSONStorable storable = containingAtom.GetStorableByID(storableId);
                if (storable == null)
                    continue;

                if (storable.GetAction("Smart Docking") == null || storable.GetAction("Reverse Smart Docking") == null)
                    continue;

                JSONStorableString modeParam = storable.GetStringJSONParam("DOCKING Last Mode");
                if (modeParam == null)
                    continue;

                JSONStorableBool activeParam = storable.GetBoolJSONParam("DOCKING Pose Assist Active");
                JSONStorableString intentParam = storable.GetStringJSONParam("DOCKING Push Pull Intent");
                JSONStorableFloat baseParam = storable.GetFloatJSONParam("DOCKING Base Distance");
                JSONStorableFloat currentParam = storable.GetFloatJSONParam("DOCKING Current Distance");
                JSONStorableFloat deltaParam = storable.GetFloatJSONParam("DOCKING Distance Delta");
                JSONStorableFloat eventParam = storable.GetFloatJSONParam("DOCKING Pose Assist Last Event Time");

                float baseDistance = baseParam != null ? baseParam.val : 0.0f;
                float currentDistance = currentParam != null ? currentParam.val : baseDistance;
                float delta = deltaParam != null ? deltaParam.val : currentDistance - baseDistance;

                state = new DOCKING_PoseAssistState();
                state.Valid = true;
                state.Active = activeParam != null && activeParam.val;
                state.Mode = modeParam.val;
                state.PushPullIntent = intentParam != null ? intentParam.val : "Neutral";
                state.BaseDistance = baseDistance;
                state.CurrentDistance = currentDistance;
                state.DistanceDelta = delta;
                state.EventTime = eventParam != null ? eventParam.val : -1.0f;
                return true;
            }
        }
        catch { }

        return false;
    }

    private string DecidePOSE_TransitionPath(string selfPostureCode, string spatialRelationCode, string intent)
    {
        string posture = selfPostureCode ?? "";
        string relation = spatialRelationCode ?? "";

        bool selfBack = relation.IndexOf("SelfBackToTarget", StringComparison.OrdinalIgnoreCase) >= 0 || relation.IndexOf("MutualBack", StringComparison.OrdinalIgnoreCase) >= 0;
        bool selfFront = relation.IndexOf("SelfFrontToTarget", StringComparison.OrdinalIgnoreCase) >= 0 || relation.IndexOf("MutualFront", StringComparison.OrdinalIgnoreCase) >= 0;

        if (posture.IndexOf("Standing", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            if (intent == "PushNear") return selfBack ? "StandToSit" : "StandToSit";
            if (intent == "PullFar") return selfBack ? "StandTurnAround" : "StandTurnAround";
        }

        if (posture.IndexOf("SittingOrCrouching", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            if (intent == "PushNear") return selfBack ? "SitToSupine" : "SitToSupine";
            if (intent == "PullFar") return "SitToStand";
        }

        if (posture.IndexOf("LyingSupine", StringComparison.OrdinalIgnoreCase) >= 0)
            return "SupineToProne";

        if (posture.IndexOf("LyingProne", StringComparison.OrdinalIgnoreCase) >= 0)
            return "ProneToSupine";

        if (posture.IndexOf("Dog", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            if (intent == "PullFar") return "DogToStand";
            if (intent == "PushNear") return "DogHold";
        }

        return "NoRule" + (selfFront ? "SelfFront" : "") + (selfBack ? "SelfBack" : "");
    }

    private bool DetectPOSE_SpatialRelation(Atom targetAtom, out POSE_SpatialRelationResult result)
    {
        result = null;

        Transform selfRoot = GetAtomRootTransform(containingAtom);
        Transform targetRoot = GetAtomRootTransform(targetAtom);
        if (selfRoot == null || targetRoot == null)
            return false;

        Vector3 selfToTarget = targetRoot.position - selfRoot.position;
        selfToTarget.y = 0f;
        if (selfToTarget.magnitude < 0.001f)
            return false;
        selfToTarget.Normalize();

        Vector3 targetToSelf = -selfToTarget;
        Vector3 selfForward = FlattenForward(selfRoot.forward);
        Vector3 targetForward = FlattenForward(targetRoot.forward);
        if (selfForward.magnitude < 0.001f || targetForward.magnitude < 0.001f)
            return false;

        float selfDot = Vector3.Dot(selfForward, selfToTarget);
        float targetDot = Vector3.Dot(targetForward, targetToSelf);
        float forwardDot = Vector3.Dot(selfForward, targetForward);

        bool selfFrontToTarget = selfDot >= POSE_SPATIAL_RELATION_SIDE_THRESHOLD;
        bool selfBackToTarget = selfDot <= -POSE_SPATIAL_RELATION_SIDE_THRESHOLD;
        bool targetFrontToSelf = targetDot >= POSE_SPATIAL_RELATION_SIDE_THRESHOLD;
        bool targetBackToSelf = targetDot <= -POSE_SPATIAL_RELATION_SIDE_THRESHOLD;
        bool mutualFront = selfFrontToTarget && targetFrontToSelf;
        bool mutualBack = selfBackToTarget && targetBackToSelf;

        string code = "POSE_SpatialRelation_SideOrUnknown";
        string display = "SideOrUnknown";
        if (mutualFront)
        {
            code = "POSE_SpatialRelation_MutualFront";
            display = "MutualFront";
        }
        else if (mutualBack)
        {
            code = "POSE_SpatialRelation_MutualBack";
            display = "MutualBack";
        }
        else if (selfFrontToTarget)
        {
            code = "POSE_SpatialRelation_SelfFrontToTarget";
            display = "SelfFrontToTarget";
        }
        else if (selfBackToTarget)
        {
            code = "POSE_SpatialRelation_SelfBackToTarget";
            display = "SelfBackToTarget";
        }

        result = new POSE_SpatialRelationResult();
        result.Code = code;
        result.Display = display;
        result.MutualFront = mutualFront;
        result.MutualBack = mutualBack;
        result.SelfFrontToTarget = selfFrontToTarget;
        result.SelfBackToTarget = selfBackToTarget;
        result.TargetFrontToSelf = targetFrontToSelf;
        result.TargetBackToSelf = targetBackToSelf;
        result.SelfDot = selfDot;
        result.TargetDot = targetDot;
        result.ForwardDot = forwardDot;
        return true;
    }

    private bool DetectPOSE_SelfPosture(out POSE_PostureResult result)
    {
        return DetectPOSE_PostureForAtom(containingAtom, "POSE_SelfPosture", out result);
    }

    private bool DetectPOSE_TargetPosture(Atom targetAtom, out POSE_PostureResult result)
    {
        return DetectPOSE_PostureForAtom(targetAtom, "POSE_TargetPosture", out result);
    }

    private bool DetectPOSE_PostureForAtom(Atom atom, string codePrefix, out POSE_PostureResult result)
    {
        result = null;
        if (atom == null)
            return false;

        FreeControllerV3 hip = FindControllerOnAtom(atom, "hipControl");
        FreeControllerV3 chest = FindControllerOnAtom(atom, "chestControl");
        if (hip == null || hip.control == null || chest == null || chest.control == null)
            return false;

        Transform atomRoot = GetAtomRootTransform(atom);
        Vector3 hipPos = hip.control.position;
        Vector3 chestPos = chest.control.position;
        Vector3 bodyAxis = chestPos - hipPos;
        float bodyLen = bodyAxis.magnitude;
        if (bodyLen < 0.001f)
            return false;

        Vector3 bodyDir = bodyAxis / bodyLen;
        float upDot = Vector3.Dot(bodyDir, Vector3.up);
        float absUpDot = Mathf.Abs(upDot);
        float bodyFloorAngleDeg = Mathf.Asin(Mathf.Clamp(absUpDot, 0f, 1f)) * Mathf.Rad2Deg;
        float hipLocalY = hip.control.localPosition.y;
        float chestLocalY = chest.control.localPosition.y;
        if (atomRoot != null)
        {
            try
            {
                hipLocalY = atomRoot.InverseTransformPoint(hipPos).y;
                chestLocalY = atomRoot.InverseTransformPoint(chestPos).y;
            }
            catch { }
        }

        float chestUpDot = Vector3.Dot(chest.control.up, Vector3.up);
        float chestForwardUpDot = Vector3.Dot(chest.control.forward, Vector3.up);
        float atomRootUpDot = atomRoot != null ? Vector3.Dot(atomRoot.up, Vector3.up) : 0f;

        string code = codePrefix + "_Unknown";
        string display = "Unknown";
        bool dogParallelCandidate = absUpDot <= POSE_TARGET_POSTURE_DOG_ABS_UP_DOT_MAX;
        bool dogRaisedEnough = hipLocalY >= POSE_TARGET_POSTURE_DOG_HIP_LOCAL_Y_MIN && chestLocalY >= POSE_TARGET_POSTURE_DOG_CHEST_LOCAL_Y_MIN;

        // Dog is checked before Lying/Supine/Prone because the user-defined Dog pose has a hip->chest line
        // close to floor-parallel, but its hip/chest are raised compared with prone/supine.
        if (dogParallelCandidate && dogRaisedEnough)
        {
            code = codePrefix + "_Dog";
            display = "Dog";
        }
        else if (absUpDot <= POSE_TARGET_POSTURE_LYING_ABS_UP_DOT_MAX)
        {
            if (chestUpDot >= POSE_TARGET_POSTURE_SUPINE_PRONE_CHEST_UP_DOT)
            {
                code = codePrefix + "_LyingSupine";
                display = "LyingSupine";
            }
            else if (chestUpDot <= -POSE_TARGET_POSTURE_SUPINE_PRONE_CHEST_UP_DOT)
            {
                code = codePrefix + "_LyingProne";
                display = "LyingProne";
            }
            else
            {
                code = codePrefix + "_Lying";
                display = "Lying";
            }
        }
        else if (upDot >= POSE_TARGET_POSTURE_STANDING_UP_DOT_MIN && hipLocalY >= POSE_TARGET_POSTURE_STANDING_HIP_LOCAL_Y_MIN)
        {
            code = codePrefix + "_Standing";
            display = "Standing";
        }
        else if (upDot >= POSE_TARGET_POSTURE_SITTING_UP_DOT_MIN)
        {
            code = codePrefix + "_SittingOrCrouching";
            display = "SittingOrCrouching";
        }
        else
        {
            code = codePrefix + "_Unknown";
            display = "Unknown";
        }

        result = new POSE_PostureResult();
        result.Code = code;
        result.Display = display;
        result.HipWorld = hipPos;
        result.ChestWorld = chestPos;
        result.BodyAxisWorld = bodyAxis;
        result.BodyAxisLength = bodyLen;
        result.BodyUpDot = upDot;
        result.AbsBodyUpDot = absUpDot;
        result.BodyFloorAngleDeg = bodyFloorAngleDeg;
        result.DogParallelCandidate = dogParallelCandidate;
        result.HipLocalY = hipLocalY;
        result.ChestLocalY = chestLocalY;
        result.ChestUpDot = chestUpDot;
        result.ChestForwardUpDot = chestForwardUpDot;
        result.TargetRootUpDot = atomRootUpDot;
        return true;
    }

    private void UpdatePOSE_DistanceCalibrationIfNeeded(Atom targetAtom, POSE_SpatialRelationResult relation)
    {
        if (targetAtom == null || relation == null || string.IsNullOrEmpty(relation.Code))
            return;

        if (lastPOSE_SpatialRelationCodeForDistanceCalibration == relation.Code)
            return;

        float horizontalDistance;
        float verticalDifference;
        if (!TryGetPOSE_HipDistance(targetAtom, out horizontalDistance, out verticalDifference))
            return;

        lastPOSE_SpatialRelationCodeForDistanceCalibration = relation.Code;

        if (horizontalDistance < POSE_DISTANCE_CALIBRATE_MIN || horizontalDistance > POSE_DISTANCE_CALIBRATE_MAX)
            return;

        poseNearDistanceBySpatialRelation[relation.Code] = horizontalDistance;
    }

    private bool DetectPOSE_DistanceRelation(Atom targetAtom, POSE_SpatialRelationResult relation, out POSE_DistanceRelationResult result)
    {
        result = null;
        if (targetAtom == null)
            return false;

        float horizontalDistance;
        float verticalDifference;
        if (!TryGetPOSE_HipDistance(targetAtom, out horizontalDistance, out verticalDifference))
            return false;

        string relationCode = relation != null && !string.IsNullOrEmpty(relation.Code)
            ? relation.Code
            : "POSE_SpatialRelation_Unknown";

        float nearReference = POSE_DISTANCE_DEFAULT_NEAR_REFERENCE;
        bool calibrated = false;
        string nearReferenceSource = "default";

        float targetLinePersonDistance;
        if (TryGetPOSE_TargetLinePersonDistanceReference(out targetLinePersonDistance))
        {
            nearReference = targetLinePersonDistance;
            nearReferenceSource = "TargetLinePerson.Distance";
        }

        if (poseNearDistanceBySpatialRelation.ContainsKey(relationCode))
        {
            nearReference = poseNearDistanceBySpatialRelation[relationCode];
            calibrated = true;
            nearReferenceSource = "relation-change-capture";
        }

        string code = "POSE_DistanceRelation_Unknown";
        string display = "Unknown";

        float nearMin = Mathf.Max(POSE_DISTANCE_TOO_CLOSE_ABSOLUTE, nearReference - POSE_DISTANCE_NEAR_HALF_WIDTH);
        float nearMax = nearReference + POSE_DISTANCE_NEAR_HALF_WIDTH;
        float tooCloseLimit = Mathf.Min(POSE_DISTANCE_TOO_CLOSE_ABSOLUTE, nearMin * 0.60f);

        if (horizontalDistance <= tooCloseLimit)
        {
            code = "POSE_DistanceRelation_TooClose";
            display = "TooClose";
        }
        else if (horizontalDistance < nearMin)
        {
            code = "POSE_DistanceRelation_Close";
            display = "Close";
        }
        else if (horizontalDistance <= nearMax)
        {
            code = "POSE_DistanceRelation_Near";
            display = "Near";
        }
        else if (horizontalDistance <= nearReference + POSE_DISTANCE_FAR_EXTRA)
        {
            code = "POSE_DistanceRelation_Far";
            display = "Far";
        }
        else
        {
            code = "POSE_DistanceRelation_TooFar";
            display = "TooFar";
        }

        result = new POSE_DistanceRelationResult();
        result.Code = code;
        result.Display = display;
        result.HipHorizontalDistance = horizontalDistance;
        result.HipVerticalDifference = verticalDifference;
        result.NearReferenceDistance = nearReference;
        result.NearHalfWidth = POSE_DISTANCE_NEAR_HALF_WIDTH;
        result.CalibratedForSpatialRelation = calibrated;
        result.SpatialRelationCode = relationCode;
        result.NearReferenceSource = nearReferenceSource;
        return true;
    }

    private bool TryGetPOSE_TargetLinePersonDistanceReference(out float distanceReference)
    {
        distanceReference = 0f;
        if (containingAtom == null)
            return false;

        try
        {
            foreach (string storableId in containingAtom.GetStorableIDs())
            {
                if (string.IsNullOrEmpty(storableId))
                    continue;

                JSONStorable storable = containingAtom.GetStorableByID(storableId);
                if (storable == null)
                    continue;

                if (storable.GetAction("Smart Docking") == null || storable.GetAction("Reverse Smart Docking") == null)
                    continue;

                JSONStorableFloat distanceParam = storable.GetFloatJSONParam("Distance");
                if (distanceParam == null)
                    continue;

                float v = Mathf.Abs(distanceParam.val);
                if (v < POSE_DISTANCE_CALIBRATE_MIN || v > POSE_DISTANCE_CALIBRATE_MAX)
                    continue;

                distanceReference = v;
                return true;
            }
        }
        catch { }

        return false;
    }

    private bool TryGetPOSE_HipDistance(Atom targetAtom, out float horizontalDistance, out float verticalDifference)
    {
        horizontalDistance = 0f;
        verticalDifference = 0f;
        if (containingAtom == null || targetAtom == null)
            return false;

        FreeControllerV3 selfHip = FindControllerOnAtom(containingAtom, "hipControl");
        FreeControllerV3 targetHip = FindControllerOnAtom(targetAtom, "hipControl");
        if (selfHip == null || selfHip.control == null || targetHip == null || targetHip.control == null)
            return false;

        Vector3 selfPos = selfHip.control.position;
        Vector3 targetPos = targetHip.control.position;
        Vector3 d = targetPos - selfPos;
        verticalDifference = d.y;
        d.y = 0f;
        horizontalDistance = d.magnitude;
        return true;
    }

    private string BuildPOSE_DetectionStatusSummary(POSE_SpatialRelationResult relation, POSE_PostureResult selfPosture, POSE_PostureResult targetPosture, POSE_DistanceRelationResult distanceRelation)
    {
        string selfPostureDisplay = selfPosture != null ? selfPosture.Display : "Unknown";
        string targetPostureDisplay = targetPosture != null ? targetPosture.Display : "Unknown";
        string relationDisplay = relation != null ? relation.Display : "Unknown";
        string distanceDisplay = distanceRelation != null ? distanceRelation.Display + " " + F(distanceRelation.HipHorizontalDistance) + "m" : "Unknown";
        return "self=" + selfPostureDisplay + " / target=" + targetPostureDisplay + " / relation=" + relationDisplay + " / distance=" + distanceDisplay;
    }

    private string BuildPOSE_DetectionLogSummary(POSE_SpatialRelationResult relation, POSE_PostureResult selfPosture, POSE_PostureResult targetPosture, POSE_DistanceRelationResult distanceRelation)
    {
        string s = "";

        if (selfPosture != null)
        {
            s += "selfPosture=" + selfPosture.Code +
                " / selfPostureDisplay=" + selfPosture.Display +
                " / selfBodyUpDot=" + F(selfPosture.BodyUpDot) +
                " / selfAbsBodyUpDot=" + F(selfPosture.AbsBodyUpDot) +
                " / selfBodyFloorAngleDeg=" + F(selfPosture.BodyFloorAngleDeg) +
                " / selfDogParallelCandidate=" + (selfPosture.DogParallelCandidate ? "1" : "0") +
                " / selfHipLocalY=" + F(selfPosture.HipLocalY) +
                " / selfChestLocalY=" + F(selfPosture.ChestLocalY) +
                " / selfBodyLen=" + F(selfPosture.BodyAxisLength) +
                " / selfChestUpDot=" + F(selfPosture.ChestUpDot) +
                " / selfChestForwardUpDot=" + F(selfPosture.ChestForwardUpDot) +
                " / selfRootUpDot=" + F(selfPosture.TargetRootUpDot);
        }
        else
        {
            s += "selfPosture=POSE_SelfPosture_Unknown";
        }

        if (targetPosture != null)
        {
            s += " / targetPosture=" + targetPosture.Code +
                " / targetPostureDisplay=" + targetPosture.Display +
                " / targetBodyUpDot=" + F(targetPosture.BodyUpDot) +
                " / targetAbsBodyUpDot=" + F(targetPosture.AbsBodyUpDot) +
                " / targetBodyFloorAngleDeg=" + F(targetPosture.BodyFloorAngleDeg) +
                " / targetDogParallelCandidate=" + (targetPosture.DogParallelCandidate ? "1" : "0") +
                " / targetHipLocalY=" + F(targetPosture.HipLocalY) +
                " / targetChestLocalY=" + F(targetPosture.ChestLocalY) +
                " / targetBodyLen=" + F(targetPosture.BodyAxisLength) +
                " / targetChestUpDot=" + F(targetPosture.ChestUpDot) +
                " / targetChestForwardUpDot=" + F(targetPosture.ChestForwardUpDot) +
                " / targetRootUpDot=" + F(targetPosture.TargetRootUpDot);
        }
        else
        {
            s += " / targetPosture=POSE_TargetPosture_Unknown";
        }

        if (relation != null)
        {
            s += " / spatialRelation=" + relation.Code +
                " / spatialRelationDisplay=" + relation.Display +
                " / mutualFront=" + B(relation.MutualFront) +
                " / mutualBack=" + B(relation.MutualBack) +
                " / selfFrontToTarget=" + B(relation.SelfFrontToTarget) +
                " / selfBackToTarget=" + B(relation.SelfBackToTarget) +
                " / targetFrontToSelf=" + B(relation.TargetFrontToSelf) +
                " / targetBackToSelf=" + B(relation.TargetBackToSelf) +
                " / selfDot=" + F(relation.SelfDot) +
                " / targetDot=" + F(relation.TargetDot) +
                " / forwardDot=" + F(relation.ForwardDot);
        }
        else
        {
            s += " / spatialRelation=POSE_SpatialRelation_Unknown";
        }

        if (distanceRelation != null)
        {
            s += " / distanceRelation=" + distanceRelation.Code +
                " / distanceRelationDisplay=" + distanceRelation.Display +
                " / hipHorizontalDistance=" + F(distanceRelation.HipHorizontalDistance) +
                " / hipVerticalDifference=" + F(distanceRelation.HipVerticalDifference) +
                " / nearReference=" + F(distanceRelation.NearReferenceDistance) +
                " / nearHalfWidth=" + F(distanceRelation.NearHalfWidth) +
                " / nearCalibrated=" + B(distanceRelation.CalibratedForSpatialRelation) +
                " / nearReferenceSource=" + distanceRelation.NearReferenceSource +
                " / nearRelationCode=" + distanceRelation.SpatialRelationCode;
        }
        else
        {
            s += " / distanceRelation=POSE_DistanceRelation_Unknown";
        }

        return s;
    }

    private string BuildPOSE_TransitionLogSuffix()
    {
        DOCKING_PoseAssistState docking;
        if (!TryReadPOSE_DOCKING_PoseAssistState(out docking) || docking == null || !docking.Valid)
            return " / transitionMode=unavailable";

        return " / transitionMode=" + (poseTransitionModeActive ? "active" : "inactive") +
            " / dockingMode=" + docking.Mode +
            " / dockingActive=" + B(docking.Active) +
            " / dockingBaseDistance=" + F(docking.BaseDistance) +
            " / dockingCurrentDistance=" + F(docking.CurrentDistance) +
            " / dockingDistanceDelta=" + F(docking.DistanceDelta) +
            " / transitionIntentLocked=" + B(poseTransitionIntentLocked) +
            " / transitionIntent=" + poseTransitionLockedIntent +
            " / transitionPath=" + poseTransitionLockedPath;
    }

    private bool BuildAutoPoseRelationSummary(Atom targetAtom, out string summary)
    {
        summary = "";
        POSE_SpatialRelationResult relation;
        if (!DetectPOSE_SpatialRelation(targetAtom, out relation) || relation == null)
            return false;

        summary =
            "relation=" + relation.Display +
            " / mutualFront=" + B(relation.MutualFront) +
            " / mutualBack=" + B(relation.MutualBack) +
            " / selfFrontToTarget=" + B(relation.SelfFrontToTarget) +
            " / selfBackToTarget=" + B(relation.SelfBackToTarget) +
            " / targetFrontToSelf=" + B(relation.TargetFrontToSelf) +
            " / targetBackToSelf=" + B(relation.TargetBackToSelf) +
            " / selfDot=" + F(relation.SelfDot) +
            " / targetDot=" + F(relation.TargetDot) +
            " / forwardDot=" + F(relation.ForwardDot);

        return true;
    }

    private Vector3 FlattenForward(Vector3 v)
    {
        v.y = 0f;
        if (v.magnitude < 0.001f)
            return Vector3.zero;
        return v.normalized;
    }

    private string B(bool v)
    {
        return v ? "1" : "0";
    }

    private IEnumerator MacroFaceTarget(bool faceTarget)
    {
        SetTargetFacingYawAndApply(
            faceTarget ? VISUAL_FACE_TARGET_YAW_OFFSET_DEG : VISUAL_BUTT_TARGET_YAW_OFFSET_DEG,
            true,
            faceTarget ? "macro face target" : "macro butt target"
        );
        yield return null;
    }

    private List<NoRootTurnSnapshot> BuildNoRootTurnSnapshots(FreeControllerV3 hip, Quaternion deltaRot)
    {
        List<NoRootTurnSnapshot> snaps = new List<NoRootTurnSnapshot>();
        if (hip == null || hip.control == null)
            return snaps;

        Vector3 pivot = hip.control.localPosition;
        string[] names = new string[]
        {
            "hipControl", "chestControl", "headControl",
            "lHandControl", "rHandControl", "lElbowControl", "rElbowControl",
            "lKneeControl", "rKneeControl", "lFootControl", "rFootControl"
        };

        for (int i = 0; i < names.Length; i++)
        {
            string name = names[i];
            FreeControllerV3 fc = FindController(name);
            if (fc == null || fc.control == null)
            {
                LogDebug("NoRoot turn control missing: " + name);
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

            NoRootTurnSnapshot snap = new NoRootTurnSnapshot();
            snap.ControlName = name;
            snap.Controller = fc;
            snap.ControlTransform = fc.control;
            snap.StartLocalPos = startPos;
            snap.StartLocalRot = startRot;
            snap.TargetLocalPos = pivot + deltaRot * (startPos - pivot);
            snap.TargetLocalRot = ClosestQuaternionToStart(startRot, deltaRot * startRot);
            snaps.Add(snap);
        }

        return snaps;
    }

    private void ApplyNoRootTurnSnapshots(List<NoRootTurnSnapshot> snaps, float t)
    {
        if (snaps == null)
            return;

        float mt = PoseMotionT(t);
        for (int i = 0; i < snaps.Count; i++)
        {
            NoRootTurnSnapshot snap = snaps[i];
            if (snap == null || snap.ControlTransform == null)
                continue;

            snap.ControlTransform.localPosition = Vector3.Lerp(snap.StartLocalPos, snap.TargetLocalPos, mt);
            snap.ControlTransform.localRotation = Quaternion.Slerp(snap.StartLocalRot, snap.TargetLocalRot, mt);
        }
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
        bool hasHdc = hdc != null || ResolveHDC(false);

        if (!hasHdc)
        {
            yield return StartCoroutine(RunLocalHdcPhase(
                mode,
                targetBone,
                setPosX, targetPosX,
                setPosY, targetPosY,
                setPosZ, targetPosZ,
                setRotX, targetRotX,
                setRotY, targetRotY,
                setRotZ, targetRotZ,
                "HDC missing"
            ));
            yield break;
        }

        SetHdcEnabled(true);
        SetHdcLiveApply(true);
        SetHdcChooser("Mode", mode);
        if (!string.IsNullOrEmpty(targetBone))
            SetHdcChooser("TargetBone", targetBone);

        InvokeHdcAction("HDC Capture Current", false);
        yield return null;

        JSONStorableFloat hdcPosX = GetHdcFloat("Pos X");
        JSONStorableFloat hdcPosY = GetHdcFloat("Pos Y");
        JSONStorableFloat hdcPosZ = GetHdcFloat("Pos Z");
        JSONStorableFloat hdcRotX = GetHdcFloat("Rot X");
        JSONStorableFloat hdcRotY = GetHdcFloat("Rot Y");
        JSONStorableFloat hdcRotZ = GetHdcFloat("Rot Z");

        if (hdcPosX == null || hdcPosY == null || hdcPosZ == null || hdcRotX == null || hdcRotY == null || hdcRotZ == null)
        {
            yield return StartCoroutine(RunLocalHdcPhase(
                mode,
                targetBone,
                setPosX, targetPosX,
                setPosY, targetPosY,
                setPosZ, targetPosZ,
                setRotX, targetRotX,
                setRotY, targetRotY,
                setRotZ, targetRotZ,
                "HDC slider params missing"
            ));
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


    private class LocalHdcPhaseSnapshot
    {
        public string ControlName;
        public FreeControllerV3 Controller;
        public Transform ControlTransform;
        public Vector3 StartLocalPos;
        public Quaternion StartLocalRot;
        public Vector3 TargetLocalPos;
        public Quaternion TargetLocalRot;
        public bool IsRootControl;
        public Vector3 StartWorldPos;
        public Quaternion StartWorldRot;
        public Vector3 TargetWorldPos;
        public Quaternion TargetWorldRot;
    }

    private IEnumerator RunLocalHdcPhase(
        string mode,
        string targetBone,
        bool setPosX, float targetPosX,
        bool setPosY, float targetPosY,
        bool setPosZ, float targetPosZ,
        bool setRotX, float targetRotX,
        bool setRotY, float targetRotY,
        bool setRotZ, float targetRotZ,
        string reason
    )
    {
        List<LocalHdcPhaseSnapshot> snaps = BuildLocalHdcPhaseSnapshots(
            mode,
            targetBone,
            setPosX, targetPosX,
            setPosY, targetPosY,
            setPosZ, targetPosZ,
            setRotX, targetRotX,
            setRotY, targetRotY,
            setRotZ, targetRotZ
        );

        if (snaps == null || snaps.Count == 0)
        {
            SetStatus("Local HDC fallback failed: " + reason);
            yield break;
        }

        LogDebug("Local HDC fallback: " + reason + " / mode=" + mode + " / target=" + targetBone + " / controls=" + snaps.Count.ToString(CultureInfo.InvariantCulture));

        float dur = GetTransitionDuration();
        float elapsed = 0f;

        while (elapsed < dur)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dur);
            ApplyLocalHdcPhaseSnapshots(snaps, t);
            yield return null;
        }

        ApplyLocalHdcPhaseSnapshots(snaps, 1f);
        yield return null;
    }

    private List<LocalHdcPhaseSnapshot> BuildLocalHdcPhaseSnapshots(
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
        if (mode == MODE_PART)
            return BuildLocalHdcIndividualSnapshots(
                string.IsNullOrEmpty(targetBone) ? "control" : targetBone,
                setPosX, targetPosX,
                setPosY, targetPosY,
                setPosZ, targetPosZ,
                setRotX, targetRotX,
                setRotY, targetRotY,
                setRotZ, targetRotZ
            );

        return BuildLocalHdcGroupSnapshots(
            mode,
            setPosX, targetPosX,
            setPosY, targetPosY,
            setPosZ, targetPosZ,
            setRotX, targetRotX,
            setRotY, targetRotY,
            setRotZ, targetRotZ
        );
    }

    private List<LocalHdcPhaseSnapshot> BuildLocalHdcIndividualSnapshots(
        string targetBone,
        bool setPosX, float targetPosX,
        bool setPosY, float targetPosY,
        bool setPosZ, float targetPosZ,
        bool setRotX, float targetRotX,
        bool setRotY, float targetRotY,
        bool setRotZ, float targetRotZ
    )
    {
        List<LocalHdcPhaseSnapshot> snaps = new List<LocalHdcPhaseSnapshot>();

        if (targetBone == "control")
        {
            if (containingAtom == null || containingAtom.transform == null) return snaps;

            Transform root = containingAtom.transform;
            Vector3 startPos = root.position;
            Quaternion startRot = root.rotation;
            Vector3 startEuler = NormalizeEuler(startRot.eulerAngles);

            Vector3 targetPos = new Vector3(
                setPosX ? targetPosX : startPos.x,
                setPosY ? targetPosY : startPos.y,
                setPosZ ? targetPosZ : startPos.z
            );

            Quaternion targetRot = Quaternion.Euler(
                setRotX ? targetRotX : startEuler.x,
                setRotY ? targetRotY : startEuler.y,
                setRotZ ? targetRotZ : startEuler.z
            );

            LocalHdcPhaseSnapshot snap = new LocalHdcPhaseSnapshot();
            snap.ControlName = "control";
            snap.IsRootControl = true;
            snap.StartWorldPos = startPos;
            snap.StartWorldRot = startRot;
            snap.TargetWorldPos = targetPos;
            snap.TargetWorldRot = targetRot;
            snaps.Add(snap);
            return snaps;
        }

        FreeControllerV3 fc = FindController(targetBone);
        if (fc == null || fc.control == null)
            return snaps;

        try
        {
            fc.currentPositionState = FreeControllerV3.PositionState.On;
            fc.currentRotationState = FreeControllerV3.RotationState.On;
        }
        catch { }

        Vector3 startLocalPos = fc.control.localPosition;
        Quaternion startLocalRot = NormalizeQuaternion(fc.control.localRotation);
        Vector3 startLocalEuler = NormalizeEuler(startLocalRot.eulerAngles);

        LocalHdcPhaseSnapshot s = new LocalHdcPhaseSnapshot();
        s.ControlName = targetBone;
        s.Controller = fc;
        s.ControlTransform = fc.control;
        s.StartLocalPos = startLocalPos;
        s.StartLocalRot = startLocalRot;
        s.TargetLocalPos = new Vector3(
            setPosX ? targetPosX : startLocalPos.x,
            setPosY ? targetPosY : startLocalPos.y,
            setPosZ ? targetPosZ : startLocalPos.z
        );
        s.TargetLocalRot = ClosestQuaternionToStart(startLocalRot, Quaternion.Euler(
            setRotX ? targetRotX : startLocalEuler.x,
            setRotY ? targetRotY : startLocalEuler.y,
            setRotZ ? targetRotZ : startLocalEuler.z
        ));
        snaps.Add(s);

        return snaps;
    }

    private List<LocalHdcPhaseSnapshot> BuildLocalHdcGroupSnapshots(
        string mode,
        bool setPosX, float targetPosX,
        bool setPosY, float targetPosY,
        bool setPosZ, float targetPosZ,
        bool setRotX, float targetRotX,
        bool setRotY, float targetRotY,
        bool setRotZ, float targetRotZ
    )
    {
        List<LocalHdcPhaseSnapshot> snaps = new List<LocalHdcPhaseSnapshot>();
        string rootName = GetLocalHdcRootName(mode);
        string[] targets = GetLocalHdcTargetsForMode(mode);

        if (string.IsNullOrEmpty(rootName) || targets == null || targets.Length == 0)
            return snaps;

        FreeControllerV3 root = FindController(rootName);
        if (root == null || root.control == null)
            return snaps;

        Vector3 rootStartPos = root.control.localPosition;
        Quaternion rootStartRot = NormalizeQuaternion(root.control.localRotation);
        Vector3 rootStartEuler = NormalizeEuler(rootStartRot.eulerAngles);

        Vector3 rootTargetPos = new Vector3(
            setPosX ? targetPosX : rootStartPos.x,
            setPosY ? targetPosY : rootStartPos.y,
            setPosZ ? targetPosZ : rootStartPos.z
        );

        Quaternion rootTargetRot = ClosestQuaternionToStart(rootStartRot, Quaternion.Euler(
            setRotX ? targetRotX : rootStartEuler.x,
            setRotY ? targetRotY : rootStartEuler.y,
            setRotZ ? targetRotZ : rootStartEuler.z
        ));

        Quaternion deltaRot = rootTargetRot * Quaternion.Inverse(rootStartRot);

        for (int i = 0; i < targets.Length; i++)
        {
            string name = targets[i];
            FreeControllerV3 fc = FindController(name);
            if (fc == null || fc.control == null)
                continue;

            try
            {
                fc.currentPositionState = FreeControllerV3.PositionState.On;
                fc.currentRotationState = FreeControllerV3.RotationState.On;
            }
            catch { }

            Vector3 startPos = fc.control.localPosition;
            Quaternion startRot = NormalizeQuaternion(fc.control.localRotation);
            Vector3 rel = startPos - rootStartPos;

            LocalHdcPhaseSnapshot snap = new LocalHdcPhaseSnapshot();
            snap.ControlName = name;
            snap.Controller = fc;
            snap.ControlTransform = fc.control;
            snap.StartLocalPos = startPos;
            snap.StartLocalRot = startRot;
            snap.TargetLocalPos = rootTargetPos + deltaRot * rel;
            snap.TargetLocalRot = ClosestQuaternionToStart(startRot, deltaRot * startRot);
            snaps.Add(snap);
        }

        return snaps;
    }

    private string GetLocalHdcRootName(string mode)
    {
        if (mode == MODE_HIP_UPPER || mode == MODE_HIP_LOWER)
            return "hipControl";
        return "";
    }

    private string[] GetLocalHdcTargetsForMode(string mode)
    {
        if (mode == MODE_HIP_UPPER)
            return new string[] { "hipControl", "chestControl", "headControl", "lHandControl", "rHandControl", "lElbowControl", "rElbowControl" };

        if (mode == MODE_HIP_LOWER)
            return new string[] { "hipControl", "lKneeControl", "rKneeControl", "lFootControl", "rFootControl" };

        return new string[] { };
    }

    private void ApplyLocalHdcPhaseSnapshots(List<LocalHdcPhaseSnapshot> snaps, float t)
    {
        if (snaps == null)
            return;

        t = Mathf.Clamp01(t);

        for (int i = 0; i < snaps.Count; i++)
        {
            LocalHdcPhaseSnapshot s = snaps[i];
            if (s == null)
                continue;

            if (s.IsRootControl)
            {
                Vector3 p = Vector3.Lerp(s.StartWorldPos, s.TargetWorldPos, t);
                Quaternion r = Quaternion.Slerp(s.StartWorldRot, s.TargetWorldRot, t);

                if (containingAtom != null)
                {
                    containingAtom.transform.position = p;
                    containingAtom.transform.rotation = r;

                    if (containingAtom.mainController != null && containingAtom.mainController.control != null)
                    {
                        containingAtom.mainController.control.position = p;
                        containingAtom.mainController.control.rotation = r;
                    }
                }
                continue;
            }

            if (s.ControlTransform == null)
                continue;

            s.ControlTransform.localPosition = Vector3.Lerp(s.StartLocalPos, s.TargetLocalPos, t);
            s.ControlTransform.localRotation = Quaternion.Slerp(s.StartLocalRot, s.TargetLocalRot, t);
        }
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

    private FreeControllerV3 FindControllerOnAtom(Atom atom, string name)
    {
        if (atom == null || atom.freeControllers == null || string.IsNullOrEmpty(name))
            return null;

        for (int i = 0; i < atom.freeControllers.Length; i++)
        {
            FreeControllerV3 fc = atom.freeControllers[i];
            if (fc != null && fc.name == name)
                return fc;
        }
        return null;
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
