// HBA_LINE_CLASSIFIER_STABLE_BUILD 2026-06-24: Raises Fast classification to Auto Line Fast-level peaks and adds a slow-entry grace so normal Auto Line stays Active instead of Fast->Slow.
// HBA_TW_TUNED_DEFAULTS_BUILD 2026-06-24: Sets TW slider defaults to the user-tuned high-FPS values: Motion 1.25, Up 1.07, Side 1.52, Forward 1.42, Chest 1.69, Hip 1.81, Limb 1.43.
// HBA_TW_SLIDERS_TUNABLE_BUILD 2026-06-24: Adds visible TW axis/part strength sliders and expands TW Motion Scale range so high-FPS twitch can be tuned in-scene.
// HBA_HEAD_SLOW_STABLE_TIMING_BUILD 2026-06-24: Slows Head actions for high-FPS scenes, removes centerRot rebase snap, adds Head Time Scale, and syncs headControl transform/control rotation.
// HBA_TW_SOFT_TUNED_BUILD 2026-06-24: Softens Twitch presets for high-FPS playback, lowers vertical/body multipliers, and adds TW Motion Scale.
// HBA_HEAD_SLOW_STABLE_TIMING_BUILD 2026-06-24: Restores near-original head pose timing, adds Head Time Scale, uses startRot as head baseline, and syncs transform/control rotations.
// HBA_FAST_PEAK_LATCH_BUILD 2026-06-24: Latches short Fast inward progress peaks so Auto Line Fast cannot finish before Start/Inside classification sees it.
// HBA_LINE_FAST_LESS_STICKY_BUILD 2026-06-24: Raises Fast thresholds, shortens the Fast peak latch, and stops previous Fast actions from keeping Inside classification sticky so Auto Line stays Active more often.
// HBA_HEAD_ROTATION_STATE_ONLY_BUILD 2026-06-23: Head actions are rotation-only; do not force or restore headControl position/positionState, only rotation/rotationState.
// HBA_MANUAL_BUTTONS_LEFT_BUILD 2026-06-22: Moves all manual trial buttons (Twitch and Head) to the left column; Event/TG settings remain on the right.
// HBA_DEFAULT_ACTIONS_TG_ATOM_BUILD 2026-06-22: Sets practical default Action/TG routing: Slow uses HBA_Twitch_Slow, Fast uses Strong, and TG Atom defaults to HBA Head button-pulse actions.
// HBA_TG_ATOM_ONLY_SLOW_TWITCH_BUILD 2026-06-22: Highlights only TG Atom labels and adds HBA_Twitch_Slow, a slower graceful body/eyes/mouth reaction.
// HBA_TG_LABEL_DIRECT_FIX_BUILD 2026-06-22: Searches popup parents and normalizes UI text so Start/Inside/Deep/End TG Atom/Mode labels highlight reliably.
// HBA_TG_LABEL_HIGHLIGHT_BUILD 2026-06-22: Applies yellow target/red fire label highlighting to TG Atom/Mode chooser labels as well.
// HBA_ACTION_LABEL_YELLOW_FIRE_RED_BUILD 2026-06-22: Keeps the current target chooser label yellow, flashes it red only at action fire, then returns to yellow.
// HBA_INSIDE_REACTION_ENERGY_BUILD 2026-06-22: Adds non-timer Inside reaction gating using motion changes, inbound turn candidates, and Hold Energy pulses without TargetLinePerson changes.
// HBA_ONLY_INSIDE_SPEED_CALIBRATED_BUILD 2026-06-22: HumanBodyAction-only Inside classification; uses smoothed inward Progress speed calibrated to Auto Line Slow/Line/Fast bands, without TargetLinePerson line-speed changes.
// HBA_INSIDE_FAST_HYSTERESIS_BUILD 2026-06-22: Renames Inside Fast to Inside Fast and adds hysteresis/stability timing so Active/Fast does not flap near the threshold.
// HBA_POPUP_LABEL_HIGHLIGHT_BUILD 2026-06-22: Highlights the executed Start/Inside/Deep/End chooser label itself instead of using a separate action highlight panel.
// HBA_ACTION_LABEL_HIGHLIGHT_BUILD 2026-06-22: Adds a right-side action label panel and highlights the last executed Start/Inside/Deep/End action row.
// HBA_START_INSIDE_MOTION_ACTIONS_BUILD 2026-06-22: Splits Start into Slow/Normal/Fast and Inside into Hold/Slow/Active/Fast actions; Start waits briefly to classify insertion speed, Inside fires on motion-state changes with cooldown.
// HBA_STATUS_LAYOUT_ONLY_BUILD 2026-06-22: Reorganizes UI into left status/twitch/runtime controls and right event/TG settings/head manual buttons, adds HBA data age display, and keeps current event linkage behavior unchanged.
// HBA_ENABLE_LEFT_TWITCH_BUILD 2026-06-22: Adds top-left HBA Enable toggle, moves Twitch buttons to left above Body/Eyes/Mouth toggles, and improves action-receive status diagnostics.
// HBA_TG_UI_LAYOUT_BUILD 2026-06-22: Moves HBA/TG settings above buttons, hides TG Prefix text UI, keeps TW/log controls on left, and allows TG slots to call safe HBA actions.
// HBA_EVENT_TG_SETTINGS_BUILD 2026-06-22: Moves event reaction/TG routing settings into HumanBodyAction; TargetLinePerson only sends HBA status/events.
// HBA_STATUS_TG_EVENTS_BUILD 2026-06-22: Adds HBA status input/display, event actions, and moves simple TG_ trigger firing into HumanBodyAction.
// HBA_HEAD_FAST_CAP_BUILD 2026-06-22: Makes Head actions much lighter by scaling/capping long head keyframe durations, shortening enter/return blends further, skipping tiny rotations, and using a less mushy ease curve.
// HBA_HEAD_LIGHT_BUILD 2026-06-22: Makes Head actions lighter/snappier by caching parsed keyframes, skipping the first reference keyframe during playback, shortening enter/return blend, and avoiding redundant head position writes.
// HBA_SIMPLE_BUTTONS_TOGGLES_BUILD 2026-06-22: Simplifies UI to left Head buttons and right Twitch Weak/Normal/Strong, adds Body/Eyes/Mouth/Queue/Debug toggles, and removes extra Twitch action buttons from UI/actions.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class HumanBodyAction : MVRScript
{
    JSONStorableString helpText;
    JSONStorableString hbaStatusText;
    JSONStorableString hbaActionLabelText;
    JSONStorableFloat hbaTargetId;
    JSONStorableFloat hbaProgress;
    JSONStorableBool hbaActive;
    JSONStorableFloat hbaBridgeVersion;
    JSONStorableBool hbaEnabled;
    JSONStorableBool hbaTgTriggers;
    JSONStorableString hbaTgPrefix;
    JSONStorableBool hbaMotionBoost;
    JSONStorableStringChooser hbaStartNormalAction;
    JSONStorableStringChooser hbaStartSlowAction;
    JSONStorableStringChooser hbaStartFastAction;
    JSONStorableStringChooser hbaInsideActiveAction;
    JSONStorableStringChooser hbaInsideHoldAction;
    JSONStorableStringChooser hbaInsideSlowAction;
    JSONStorableStringChooser hbaInsideIntenseAction;
    JSONStorableStringChooser hbaEventDeepAction;
    JSONStorableStringChooser hbaEventEndAction;
    JSONStorableStringChooser hbaTgStartAtom;
    JSONStorableStringChooser hbaTgInsideAtom;
    JSONStorableStringChooser hbaTgDeepAtom;
    JSONStorableStringChooser hbaTgEndAtom;
    JSONStorableStringChooser hbaTgStartMode;
    JSONStorableStringChooser hbaTgInsideMode;
    JSONStorableStringChooser hbaTgDeepMode;
    JSONStorableStringChooser hbaTgEndMode;
    JSONStorableBool debugLog;
    JSONStorableBool twitchBody;
    JSONStorableBool queueLastAction;

    JSONStorableBool headActionEnabled;
    JSONStorableStringChooser headWeakPreset;
    JSONStorableStringChooser headNormalPreset;
    JSONStorableStringChooser headStrongPreset;
    JSONStorableStringChooser headOnlyPreset;
    JSONStorableFloat headTimeScale;
    JSONStorableFloat twitchMotionScale;
    JSONStorableFloat twitchSideScale;
    JSONStorableFloat twitchUpScale;
    JSONStorableFloat twitchForwardScale;
    JSONStorableFloat twitchChestScale;
    JSONStorableFloat twitchHipScale;
    JSONStorableFloat twitchLimbScale;

    JSONStorableBool useBodyAxes;
    JSONStorableBool twitchChest;
    JSONStorableBool twitchHip;
    JSONStorableBool twitchHeadMicro;
    JSONStorableBool twitchHands;
    JSONStorableBool twitchFeet;
    JSONStorableBool twitchEyes;
    JSONStorableBool twitchMouth;

    JSONStorableFloat duration;
    JSONStorableFloat strength;
    JSONStorableFloat hitCount;
    JSONStorableFloat sharpness;
    JSONStorableFloat randomness;

    Coroutine actionRoutine;
    ActionRequest pendingRequest;
    Coroutine twitchRoutine;
    Coroutine eyesRoutine;
    Coroutine mouthRoutine;
    Coroutine tgStartOffRoutine;
    Coroutine tgDeepOffRoutine;
    Coroutine tgEndOffRoutine;

    FreeControllerV3 headControl;
    HeadControlSnapshot activeHeadSnapshot;

    DAZMorph eyesClosedLeftMorph;
    DAZMorph eyesClosedRightMorph;
    DAZMorph eyesClosedMorph;
    DAZMorph mouthOpenMorph;

    bool eyesSaved;
    float savedEyesClosedLeft;
    float savedEyesClosedRight;
    float savedEyesClosedSingle;

    bool mouthSaved;
    float savedMouthOpen;

    const float DefaultDuration = 0.56f;
    const float DefaultStrength = 0.150f;
    const float DefaultHitCount = 2.40f;
    const float DefaultSharpness = 0.90f;
    const float DefaultRandomness = 0.45f;

    const float EyesClosedThreshold = 0.50f;
    const float MouthOpenThreshold = 0.25f;
    const float MouthOpenClampMin = -0.50f;
    const float MouthOpenClampMax = 1.00f;

    const float SlowEyesDuration = 0.78f;
    const float SlowEyesTarget = 0.55f;
    const float SlowMouthDuration = 0.78f;
    const float SlowMouthOpenMax = 0.25f;
    const float SlowMouthOpenMin = -0.02f;

    const float WeakEyesDuration = 0.42f;
    const float WeakEyesTarget = 0.75f;
    const float WeakMouthDuration = 0.22f;
    const float WeakMouthOpenMax = 0.35f;
    const float WeakMouthOpenMin = -0.10f;

    const float NormalEyesDuration = 0.30f;
    const float NormalEyesTarget = 0.75f;
    const float NormalMouthDuration = 0.30f;
    const float NormalMouthOpenMax = 0.65f;
    const float NormalMouthOpenMin = -0.25f;

    const float StrongEyesDuration = 0.36f;
    const float StrongEyesTarget = 1.00f;
    const float StrongMouthDuration = 0.36f;
    const float StrongMouthOpenMax = 1.00f;
    const float StrongMouthOpenMin = -0.50f;

    float currentEyesDuration = NormalEyesDuration;
    float currentEyesTarget = NormalEyesTarget;
    float currentMouthDuration = NormalMouthDuration;
    float currentMouthOpenMax = NormalMouthOpenMax;
    float currentMouthOpenMin = NormalMouthOpenMin;

    const float BodySideScale = 0.45f; // v027: less side snap at high FPS
    const float BodyUpScale = 0.90f; // v027: vertical twitch was the most visible/harsh part
    const float BodyForwardScale = 0.80f; // v027: soften forward/back kick
    const float ChestScale = 1.05f; // v027: high-FPS peak no longer hidden by 10fps sampling
    const float HipScale = 0.85f; // v027: reduce hip impulse
    const float HeadMicroScale = 0.55f;
    const float LimbScale = 0.22f; // v027: keep hands/feet from popping

    const float ReturnToStartDuration = 0.30f; // v026: slower return; high-FPS scenes made the old 0.16 snap too visible
    const float HeadEntryDuration = 0.12f; // v026: avoid immediate entry snap
    const int HeadPlaybackStartIndex = 1; // first keyframe is reference pose, not a visible wait
    const float HeadDurationScale = 1.00f; // v026: stop compressing imported HumanHeadOpenControl timings by default
    const float HeadSegmentMinDuration = 0.12f;
    const float HeadSegmentMaxDuration = 2.20f; // v026: keep long head poses from being crushed into a 0.55s burst
    const float HeadSkipAngleDegrees = 0.25f;
    const float HeadSmoothEaseMix = 0.55f; // v026: smoother high-FPS interpolation
    const float HeadTimeScaleDefault = 1.50f;
    const float HeadTimeScaleMin = 0.50f;
    const float HeadTimeScaleMax = 3.00f;
    const float TwitchMotionScaleDefault = 1.25f; // v029: user-tuned high-FPS TW default
    const float TwitchMotionScaleMin = 0.00f;
    const float TwitchMotionScaleMax = 3.00f;
    const float TwitchSideScaleDefault = 1.52f; // v029: user-tuned high-FPS TW default
    const float TwitchUpScaleDefault = 1.07f; // v029: user-tuned high-FPS TW default
    const float TwitchForwardScaleDefault = 1.42f; // v029: user-tuned high-FPS TW default
    const float TwitchChestScaleDefault = 1.69f; // v029: user-tuned high-FPS TW default
    const float TwitchHipScaleDefault = 1.81f; // v029: user-tuned high-FPS TW default
    const float TwitchLimbScaleDefault = 1.43f; // v029: user-tuned high-FPS TW default
    const float TwitchAxisScaleMin = 0.00f;
    const float TwitchAxisScaleMax = 2.50f;
    const float TwitchPartScaleMin = 0.00f;
    const float TwitchPartScaleMax = 3.00f;
    const float DebugPoseDuration = 0.20f;
    const float HbaStatusUpdateInterval = 0.10f;
    const float HbaStatusSpeedIdle = 0.030f;
    // v017: Progress-speed classifier is calibrated from Auto Line speeds 6.5 / 10.5 / 15.0.
    // Boundaries are the midpoints 8.5 and 12.75, mapped to HBA Progress-speed bands.
    const float HbaStatusSpeedSlow = 0.300f;      // Slow / Active boundary
    const float HbaStatusSpeedFastEnter = 1.050f; // v032: stronger Fast gate; normal Auto Line should stay Active, Fast is for true fast peaks
    const float HbaStatusSpeedFastExit = 0.620f;  // v032: exit threshold below enter to avoid Active/Fast flapping after a real Fast
    const float HbaInSpeedSmoothing = 0.35f;
    const float HbaStatusHoldSeconds = 0.60f;
    const float HbaStartDecisionDelay = 0.25f;
    const float HbaStartSlowSpeed = 0.20f;
    const float HbaStartFastSpeed = 1.20f; // v032: avoid classifying normal Auto Line start as Fast
    const float HbaInsideFirstDelay = 0.25f;
    const float HbaInsideMotionCooldown = 1.00f;
    const float HbaInsideHoldDelay = 0.60f;
    const float HbaInsideFastEnterSeconds = 0.35f; // v032: require Fast to persist a little longer before switching from Active
    const float HbaInsideFastExitSeconds = 0.35f;
    const float HbaInsideSlowEnterSeconds = 0.35f; // v031: do not let normal Auto Line tail immediately become Slow
    const float HbaFastPeakLatchSeconds = 0.25f; // v032: shorter latch so normal Line initial peaks do not keep forcing Fast
    const float HbaFastPeakMinProgress = 0.18f; // v032: avoid latching shallow/synthetic Start/Inside progress floors
    const float HbaInsideTurnCooldown = 0.75f;
    const float HbaInsideHoldPulseCooldown = 1.20f;
    const float HbaInsideReactionEnergyThreshold = 0.34f;
    const float HbaInsideHoldEnergyThreshold = 1.00f;
    const float HbaInsideReactionEnergyMax = 1.25f;
    const float HbaInsideHoldEnergyMax = 1.50f;
    const float HbaInsideEnergyAfterFire = 0.08f;
    const float HbaActionLabelFireFlashSeconds = 0.35f;
    const float TgButtonPulseSeconds = 0.10f;
    const float TgTimer1Seconds = 1.00f;
    const float TgTimer5Seconds = 5.00f;
    const string TgDefaultPrefix = "TG_";
    const string HbaActionOff = "Off";
    const string HbaActionSameAsNormal = "Same as Normal";
    const string HbaActionSameAsActive = "Same as Active";
    const string HbaActionTwitchSlow = "HBA_Twitch_Slow";
    const string HbaActionTwitchWeak = "HBA_Twitch_Weak";
    const string HbaActionTwitchNormal = "HBA_Twitch_Normal";
    const string HbaActionTwitchStrong = "HBA_Twitch_Strong";
    const string TgModeOff = "Off";
    const string TgModeState = "State";
    const string TgModeButtonPulse = "Button Pulse";
    const string TgModeTimer1 = "Timer 1s";
    const string TgModeTimer5 = "Timer 5s";

    float hbaLastStatusUpdateTime = -999.0f;
    float hbaLastDataReceiveTime = -999.0f;
    float hbaLastProgressSampleTime = -999.0f;
    float hbaLastProgress = 0.0f;
    float hbaDepthSpeed = 0.0f;
    float hbaDepthInSpeedRaw = 0.0f;
    float hbaDepthInSpeedAvg = 0.0f;
    float hbaFastPeakTime = -999.0f;
    float hbaFastPeakSpeed = 0.0f;
    float hbaFastPeakProgress = 0.0f;
    int hbaDepthDirection = 0;
    float hbaHoldSince = -1.0f;
    string hbaMotionState = "Idle";
    string hbaLastEvent = "None";
    string hbaLastAction = "None";
    string hbaHighlightedActionLabel = "";
    string hbaActionLabelFire = "";
    float hbaActionLabelFireUntil = -1.0f;
    string hbaLastBlock = "";

    UIDynamicPopup uiStartNormalAction;
    UIDynamicPopup uiStartSlowAction;
    UIDynamicPopup uiStartFastAction;
    UIDynamicPopup uiInsideActiveAction;
    UIDynamicPopup uiInsideHoldAction;
    UIDynamicPopup uiInsideSlowAction;
    UIDynamicPopup uiInsideIntenseAction;
    UIDynamicPopup uiDeepAction;
    UIDynamicPopup uiEndAction;
    UIDynamicPopup uiTgStartAtom;
    UIDynamicPopup uiTgStartMode;
    UIDynamicPopup uiTgInsideAtom;
    UIDynamicPopup uiTgInsideMode;
    UIDynamicPopup uiTgDeepAtom;
    UIDynamicPopup uiTgDeepMode;
    UIDynamicPopup uiTgEndAtom;
    UIDynamicPopup uiTgEndMode;
    readonly Dictionary<UIDynamicPopup, Color> actionPopupDefaultLabelColors = new Dictionary<UIDynamicPopup, Color>();
    string hbaHighlightedTgLabel = "";
    string hbaTgLabelFire = "";
    float hbaTgLabelFireUntil = -1.0f;
    Coroutine hbaStartDecisionRoutine;
    Coroutine hbaInsideMonitorRoutine;
    string hbaInsideMotionState = "None";
    string hbaLastInsideActionMotion = "None";
    float hbaLastInsideActionTime = -999.0f;
    float hbaInsideFastEnterSince = -1.0f;
    float hbaInsideFastExitSince = -1.0f;
    float hbaInsideSlowEnterSince = -1.0f;
    float hbaInsideReactionEnergy = 0.0f;
    float hbaInsideHoldEnergy = 0.0f;
    float hbaLastInsideEnergyUpdateTime = -1.0f;
    float hbaLastInsideTurnActionTime = -999.0f;
    float hbaLastInsideHoldPulseTime = -999.0f;
    int hbaLastInsideDirection = 0;

    readonly string[] tgFallbackStorableIds =
    {
        "Trigger",
        "UIToggle",
        "Toggle",
        "Control",
        "plugin#0",
        "plugin#1",
        "plugin#2"
    };
    readonly string[] tgFallbackBoolNames =
    {
        "value",
        "on",
        "On",
        "toggle",
        "Toggle",
        "isOn",
        "enabled",
        "Enabled"
    };

    readonly List<TwitchPart> parts = new List<TwitchPart>();
    readonly List<string> headPresetChoices = new List<string>()
    {
        "Off",
        "Head Shake",
        "Head Tilt",
        "Head Big Nod",
        "Head Nod",
        "Head Look Up",
        "Head Intense Shake",
        "Head Ecstasy Arch",
        "Head Rapid Orgasm",
        "Head Shy",
        "Head Look Around",
        "Head Neck Roll",
        "Head Quick Nod",
        "Head Up Eyes"
    };

    readonly List<string> hbaEventActionChoices = new List<string>()
    {
        HbaActionOff,
        HbaActionTwitchSlow,
        HbaActionTwitchWeak,
        HbaActionTwitchNormal,
        HbaActionTwitchStrong,
        "HBA_Head_Shake",
        "HBA_Head_Tilt",
        "HBA_Head_BigNod",
        "HBA_Head_Nod",
        "HBA_Head_LookUp",
        "HBA_Head_IntenseShake",
        "HBA_Head_EcstasyArch",
        "HBA_Head_RapidOrgasm",
        "HBA_Head_Shy",
        "HBA_Head_LookAround",
        "HBA_Head_NeckRoll",
        "HBA_Head_QuickNod",
        "HBA_Head_UpEyes"
    };

    readonly List<string> hbaStartVariantActionChoices = new List<string>()
    {
        HbaActionSameAsNormal,
        HbaActionOff,
        HbaActionTwitchSlow,
        HbaActionTwitchWeak,
        HbaActionTwitchNormal,
        HbaActionTwitchStrong,
        "HBA_Head_Shake",
        "HBA_Head_Tilt",
        "HBA_Head_BigNod",
        "HBA_Head_Nod",
        "HBA_Head_LookUp",
        "HBA_Head_IntenseShake",
        "HBA_Head_EcstasyArch",
        "HBA_Head_RapidOrgasm",
        "HBA_Head_Shy",
        "HBA_Head_LookAround",
        "HBA_Head_NeckRoll",
        "HBA_Head_QuickNod",
        "HBA_Head_UpEyes"
    };

    readonly List<string> hbaInsideVariantActionChoices = new List<string>()
    {
        HbaActionSameAsActive,
        HbaActionOff,
        HbaActionTwitchSlow,
        HbaActionTwitchWeak,
        HbaActionTwitchNormal,
        HbaActionTwitchStrong,
        "HBA_Head_Shake",
        "HBA_Head_Tilt",
        "HBA_Head_BigNod",
        "HBA_Head_Nod",
        "HBA_Head_LookUp",
        "HBA_Head_IntenseShake",
        "HBA_Head_EcstasyArch",
        "HBA_Head_RapidOrgasm",
        "HBA_Head_Shy",
        "HBA_Head_LookAround",
        "HBA_Head_NeckRoll",
        "HBA_Head_QuickNod",
        "HBA_Head_UpEyes"
    };

    readonly List<string> hbaTgAtomChoices = new List<string>() { "" };
    readonly List<string> hbaTgModeChoices = new List<string>()
    {
        TgModeOff,
        TgModeState,
        TgModeButtonPulse,
        TgModeTimer1,
        TgModeTimer5
    };

    class TwitchPart
    {
        public string label;
        public string controllerName;
        public FreeControllerV3 controller;
        public Vector3 lastOffset;
        public Vector3 direction;
        public float phase;
        public float weight;
    }

    class ActionRequest
    {
        public string source;
        public string preset;
        public bool applyPreset;
        public bool runBody;
        public bool runEyes;
        public bool runMouth;
        public bool runHead;
        public string headPreset;
    }

    class HeadControlSnapshot
    {
        public Vector3 position;
        public Quaternion rotation;
        public FreeControllerV3.PositionState positionState;
        public FreeControllerV3.RotationState rotationState;
    }

    class Keyframe
    {
        public Quaternion rotation;
        public float duration;
    }

    readonly Dictionary<string, Keyframe[]> headPoseCache = new Dictionary<string, Keyframe[]>();

    // ==================== Head pose data from HumanHeadOpenControl ====================
    private readonly string[] ShakeData = {
        "💽POSE|TM,0.20,0|headControl,0.008,1.184,-0.482,-0.023,0.057,-0.000,-0.998|#",
        "💽POSE|TM,0.35,0|headControl,0.008,1.184,-0.482,-0.023,-0.152,0.004,-0.988|#",
        "💽POSE|TM,0.35,0|headControl,0.008,1.184,-0.482,-0.023,0.013,0.001,-1.000|#",
        "💽POSE|TM,0.35,0|headControl,0.008,1.184,-0.482,-0.023,-0.109,0.003,-0.994|#",
        "💽POSE|TM,0.45,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#"
    };

    private readonly string[] TiltData = {
        "💽POSE|TM,0.20,0|headControl,0.008,1.184,-0.482,-0.023,0.057,-0.000,-0.998|#",
        "💽POSE|TM,1.45,0|headControl,0.008,1.184,-0.482,-0.022,0.055,0.170,-0.984|#",
        "💽POSE|TM,2.10,0|headControl,0.008,1.184,-0.482,-0.022,0.055,-0.170,-0.984|#",
        "💽POSE|TM,1.55,0|headControl,0.008,1.184,-0.482,-0.023,0.057,-0.030,-0.998|#"
    };

    private readonly string[] BigNodData = {
        "💽POSE|TM,0.80,0|headControl,0.008,1.184,-0.482,-0.023,0.057,-0.000,-0.998|#",
        "💽POSE|TM,0.35,0|headControl,0.008,1.184,-0.482,-0.095,0.090,-0.040,-0.991|#",
        "💽POSE|TM,0.48,0|headControl,0.008,1.184,-0.482,0.018,-0.100,0.175,-0.979|#",
        "💽POSE|TM,0.40,0|headControl,0.008,1.184,-0.482,-0.010,0.000,-0.070,-0.997|#",
        "💽POSE|TM,0.36,0|headControl,0.008,1.184,-0.482,0.008,-0.050,0.095,-0.994|#",
        "💽POSE|TM,0.75,0|headControl,0.008,1.184,-0.482,-0.012,0.020,0.040,-0.999|#"
    };

    private readonly string[] NodData = {
        "💽POSE|TM,0.20,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#",
        "💽POSE|TM,0.30,0|headControl,0.008,1.184,-0.482,-0.125,-0.046,0.002,-0.991|#",
        "💽POSE|TM,0.22,0|headControl,0.008,1.184,-0.482,-0.015,-0.048,0.002,-0.999|#",
        "💽POSE|TM,0.28,0|headControl,0.008,1.184,-0.482,-0.070,-0.047,0.002,-0.997|#",
        "💽POSE|TM,0.45,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#"
    };

    private readonly string[] LookUpData = {
        "💽POSE|TM,0.20,0|headControl,-0.035,1.755,0.153,-0.014,-0.112,-0.001,0.994|#",
        "💽POSE|TM,0.35,0|headControl,-0.035,1.755,0.153,-0.091,-0.111,-0.010,0.990|#",
        "💽POSE|TM,0.60,0|headControl,-0.035,1.755,0.153,-0.214,-0.109,-0.024,0.971|#",
        "💽POSE|TM,0.45,0|headControl,-0.035,1.755,0.153,-0.091,-0.111,-0.010,0.990|#"
    };

    private readonly string[] IntenseShakeData = {
        "💽POSE|TM,0.60,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#",
        "💽POSE|TM,0.22,0|headControl,0.008,1.184,-0.482,-0.185,-0.120,0.045,-0.975|#",
        "💽POSE|TM,0.18,0|headControl,0.008,1.184,-0.482,0.095,0.180,-0.085,-0.978|#",
        "💽POSE|TM,0.20,0|headControl,0.008,1.184,-0.482,-0.210,-0.080,0.120,-0.965|#",
        "💽POSE|TM,0.19,0|headControl,0.008,1.184,-0.482,0.125,0.165,-0.095,-0.972|#",
        "💽POSE|TM,0.25,0|headControl,0.008,1.184,-0.482,-0.165,-0.045,0.035,-0.985|#",
        "💽POSE|TM,0.45,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#"
    };

    private readonly string[] EcstasyArchData = {
        "💽POSE|TM,0.70,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#",
        "💽POSE|TM,0.35,0|headControl,0.008,1.184,-0.482,-0.165,0.080,-0.095,-0.980|#",
        "💽POSE|TM,0.28,0|headControl,0.008,1.184,-0.482,0.095,-0.045,0.135,-0.985|#",
        "💽POSE|TM,0.32,0|headControl,0.008,1.184,-0.482,-0.180,0.065,-0.110,-0.975|#",
        "💽POSE|TM,0.40,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#"
    };

    private readonly string[] RapidOrgasmData = {
        "💽POSE|TM,0.85,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#",
        "💽POSE|TM,0.38,0|headControl,0.008,1.184,-0.482,-0.08,0.26,0.03,-0.960|#",
        "💽POSE|TM,0.36,0|headControl,0.008,1.184,-0.482,-0.07,-0.29,-0.02,-0.955|#",
        "💽POSE|TM,0.39,0|headControl,0.008,1.184,-0.482,0.10,0.24,0.04,-0.962|#",
        "💽POSE|TM,0.35,0|headControl,0.008,1.184,-0.482,-0.11,-0.27,-0.03,-0.958|#",
        "💽POSE|TM,0.37,0|headControl,0.008,1.184,-0.482,0.06,0.28,0.02,-0.957|#",
        "💽POSE|TM,0.34,0|headControl,0.008,1.184,-0.482,-0.09,-0.25,-0.01,-0.960|#",
        "💽POSE|TM,0.52,0|headControl,0.008,1.184,-0.482,0.25,0.02,0.01,-0.968|#",
        "💽POSE|TM,0.65,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#"
    };

    private readonly string[] ShyLookData = {
        "💽POSE|TM,0.80,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#",
        "💽POSE|TM,0.45,0|headControl,0.008,1.184,-0.482,-0.120,-0.030,0.010,-0.992|#",
        "💽POSE|TM,0.55,0|headControl,0.008,1.184,-0.482,-0.085,0.080,0.045,-0.994|#",
        "💽POSE|TM,0.40,0|headControl,0.008,1.184,-0.482,-0.135,0.025,0.015,-0.991|#",
        "💽POSE|TM,0.60,0|headControl,0.008,1.184,-0.482,-0.095,-0.015,0.008,-0.995|#",
        "💽POSE|TM,0.70,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#"
    };

    private readonly string[] LookAroundData = {
        "💽POSE|TM,0.90,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#",
        "💽POSE|TM,0.65,0|headControl,0.008,1.184,-0.482,-0.015,0.220,0.008,-0.975|#",
        "💽POSE|TM,0.70,0|headControl,0.008,1.184,-0.482,-0.018,-0.195,-0.005,-0.981|#",
        "💽POSE|TM,0.60,0|headControl,0.008,1.184,-0.482,-0.020,0.145,0.012,-0.989|#",
        "💽POSE|TM,0.75,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#"
    };

    private readonly string[] NeckRollData = {
        "💽POSE|TM,0.20,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#",
        "💽POSE|TM,0.50,0|headControl,0.008,1.184,-0.482,0.045,0.080,0.120,-0.989|#",
        "💽POSE|TM,0.55,0|headControl,0.008,1.184,-0.482,-0.110,0.015,-0.085,-0.990|#",
        "💽POSE|TM,0.50,0|headControl,0.008,1.184,-0.482,0.035,-0.095,0.130,-0.987|#",
        "💽POSE|TM,0.80,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#"
    };

    private readonly string[] QuickNodData = {
        "💽POSE|TM,0.60,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#",
        "💽POSE|TM,0.18,0|headControl,0.008,1.184,-0.482,-0.155,-0.040,0.005,-0.988|#",
        "💽POSE|TM,0.15,0|headControl,0.008,1.184,-0.482,-0.025,-0.045,0.003,-0.999|#",
        "💽POSE|TM,0.18,0|headControl,0.008,1.184,-0.482,-0.140,-0.038,0.004,-0.990|#",
        "💽POSE|TM,0.35,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#"
    };

    private readonly string[] UpEyesData = {
        "💽POSE|TM,0.50,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#",
        "💽POSE|TM,0.45,0|headControl,0.008,1.184,-0.482,-0.18,0.02,0.01,-0.985|#",
        "💽POSE|TM,1.95,0|headControl,0.008,1.184,-0.482,-0.20,0.01,0.00,-0.980|#",
        "💽POSE|TM,1.70,0|headControl,0.008,1.184,-0.482,-0.15,0.03,0.02,-0.988|#",
        "💽POSE|TM,0.55,0|headControl,0.008,1.184,-0.482,-0.023,-0.048,0.002,-0.999|#"
    };

    public override void Init()
    {
        try
        {
            InitHiddenSettings();
            BuildHeadPoseCache();
            // Left top: keep Enable visible first, then the live status block.
            CreateEnableUi();
            CreateStatusUi();
            CreateEventSettingsUi();
            CreateTgSettingsUi();
            CreateTwitchButtons();
            CreateHeadButtons();
            CreateLeftRuntimeSettingsUi();
            RegisterHbaActions();

            RefreshControllers();
            RefreshFaceMorphs();
            RefreshHbaTgAtomList();
            DebugMessage("[HumanBodyAction] Ready / v029 tw tuned defaults / head slow stable / HBA_BridgeVersion");
        }
        catch (Exception e)
        {
            SuperController.LogError("[HumanBodyAction] Init Error: " + e);
        }
    }

    void InitHiddenSettings()
    {
        // Hidden settings: this plugin is operated by buttons/actions and HBA status values.
        // Keep these JSONStorables registered so TargetLinePerson can update them directly,
        // but do not create UI sliders for them.
        hbaEnabled = new JSONStorableBool("HBA Enable", true);
        RegisterBool(hbaEnabled);

        hbaTargetId = new JSONStorableFloat("HBA_TargetId", 0.0f, 0.0f, 3.0f, true);
        hbaTargetId.setCallbackFunction = delegate(float value) { MarkHbaDataReceived(); };
        RegisterFloat(hbaTargetId);

        hbaProgress = new JSONStorableFloat("HBA_Progress", 0.0f, 0.0f, 1.2f);
        hbaProgress.setCallbackFunction = delegate(float value) { MarkHbaDataReceived(); };
        RegisterFloat(hbaProgress);

        hbaActive = new JSONStorableBool("HBA_Active", false);
        hbaActive.setCallbackFunction = delegate(bool value) { MarkHbaDataReceived(); };
        RegisterBool(hbaActive);

        // Bridge marker used by TargetLinePerson to prefer the currently loaded HBA build
        // instead of a stale/old HBA instance when scripts are swapped during testing.
        hbaBridgeVersion = new JSONStorableFloat("HBA_BridgeVersion", 26.0f, 0.0f, 999.0f, true);
        RegisterFloat(hbaBridgeVersion);

        hbaTgTriggers = new JSONStorableBool("TG Triggers", true);
        RegisterBool(hbaTgTriggers);

        hbaTgPrefix = new JSONStorableString("TG Prefix", TgDefaultPrefix);
        hbaTgPrefix.setCallbackFunction = delegate(string value)
        {
            RefreshHbaTgAtomList();
        };
        RegisterString(hbaTgPrefix);

        hbaMotionBoost = new JSONStorableBool("Motion Boost", true);
        RegisterBool(hbaMotionBoost);

        hbaStartNormalAction = CreateHiddenActionChooser("Start Normal Action", hbaEventActionChoices, HbaActionTwitchWeak);
        hbaStartSlowAction = CreateHiddenActionChooser("Start Slow Action", hbaStartVariantActionChoices, HbaActionTwitchSlow);
        hbaStartFastAction = CreateHiddenActionChooser("Start Fast Action", hbaStartVariantActionChoices, HbaActionSameAsNormal);
        hbaInsideActiveAction = CreateHiddenActionChooser("Inside Active Action", hbaEventActionChoices, HbaActionTwitchNormal);
        hbaInsideHoldAction = CreateHiddenActionChooser("Inside Hold Action", hbaInsideVariantActionChoices, HbaActionTwitchSlow);
        hbaInsideSlowAction = CreateHiddenActionChooser("Inside Slow Action", hbaInsideVariantActionChoices, HbaActionTwitchSlow);
        hbaInsideIntenseAction = CreateHiddenActionChooser("Inside Fast Action", hbaInsideVariantActionChoices, HbaActionTwitchStrong);
        hbaEventDeepAction = CreateHiddenActionChooser("Deep Action", hbaEventActionChoices, HbaActionTwitchStrong);
        hbaEventEndAction = CreateHiddenActionChooser("End Action", hbaEventActionChoices, HbaActionTwitchWeak);

        hbaTgStartAtom = CreateHiddenTgAtomChooser("Start TG Atom");
        hbaTgInsideAtom = CreateHiddenTgAtomChooser("Inside TG Atom");
        hbaTgDeepAtom = CreateHiddenTgAtomChooser("Deep TG Atom");
        hbaTgEndAtom = CreateHiddenTgAtomChooser("End TG Atom");
        hbaTgStartMode = CreateHiddenTgModeChooser("Start TG Mode", TgModeButtonPulse);
        hbaTgInsideMode = CreateHiddenTgModeChooser("Inside TG Mode", TgModeButtonPulse);
        hbaTgDeepMode = CreateHiddenTgModeChooser("Deep TG Mode", TgModeButtonPulse);
        hbaTgEndMode = CreateHiddenTgModeChooser("End TG Mode", TgModeButtonPulse);

        hbaStatusText = new JSONStorableString("HBA Status", "HBA: Idle");
        RegisterString(hbaStatusText);

        hbaActionLabelText = new JSONStorableString("HBA Action Labels", "");
        RegisterString(hbaActionLabelText);

        duration = new JSONStorableFloat("Duration Sec", DefaultDuration, 0.10f, 1.50f);
        RegisterFloat(duration);

        strength = new JSONStorableFloat("Strength", DefaultStrength, 0.000f, 0.250f);
        RegisterFloat(strength);

        hitCount = new JSONStorableFloat("Hit Count", DefaultHitCount, 1.00f, 7.00f);
        RegisterFloat(hitCount);

        sharpness = new JSONStorableFloat("Sharpness", DefaultSharpness, 0.000f, 1.000f);
        RegisterFloat(sharpness);

        randomness = new JSONStorableFloat("Randomness", DefaultRandomness, 0.000f, 1.000f);
        RegisterFloat(randomness);

        useBodyAxes = new JSONStorableBool("Use Body Axes", true);
        RegisterBool(useBodyAxes);

        twitchBody = new JSONStorableBool("Body", true);
        RegisterBool(twitchBody);

        twitchChest = new JSONStorableBool("Chest", true);
        RegisterBool(twitchChest);

        twitchHip = new JSONStorableBool("Hip", true);
        RegisterBool(twitchHip);

        // TW and Head are separated in this build. TW does not micro-move headControl.
        twitchHeadMicro = new JSONStorableBool("Head Micro", false);
        RegisterBool(twitchHeadMicro);

        twitchHands = new JSONStorableBool("Hands", true);
        RegisterBool(twitchHands);

        twitchFeet = new JSONStorableBool("Feet", true);
        RegisterBool(twitchFeet);

        twitchEyes = new JSONStorableBool("Eyes", true);
        RegisterBool(twitchEyes);

        twitchMouth = new JSONStorableBool("Mouth", true);
        RegisterBool(twitchMouth);

        headTimeScale = new JSONStorableFloat("Head Time Scale", HeadTimeScaleDefault, HeadTimeScaleMin, HeadTimeScaleMax);
        RegisterFloat(headTimeScale);

        twitchMotionScale = new JSONStorableFloat("TW Motion Scale", TwitchMotionScaleDefault, TwitchMotionScaleMin, TwitchMotionScaleMax);
        RegisterFloat(twitchMotionScale);

        twitchSideScale = new JSONStorableFloat("TW Side Scale", TwitchSideScaleDefault, TwitchAxisScaleMin, TwitchAxisScaleMax);
        RegisterFloat(twitchSideScale);

        twitchUpScale = new JSONStorableFloat("TW Up Scale", TwitchUpScaleDefault, TwitchAxisScaleMin, TwitchAxisScaleMax);
        RegisterFloat(twitchUpScale);

        twitchForwardScale = new JSONStorableFloat("TW Forward Scale", TwitchForwardScaleDefault, TwitchAxisScaleMin, TwitchAxisScaleMax);
        RegisterFloat(twitchForwardScale);

        twitchChestScale = new JSONStorableFloat("TW Chest Scale", TwitchChestScaleDefault, TwitchPartScaleMin, TwitchPartScaleMax);
        RegisterFloat(twitchChestScale);

        twitchHipScale = new JSONStorableFloat("TW Hip Scale", TwitchHipScaleDefault, TwitchPartScaleMin, TwitchPartScaleMax);
        RegisterFloat(twitchHipScale);

        twitchLimbScale = new JSONStorableFloat("TW Limb Scale", TwitchLimbScaleDefault, TwitchPartScaleMin, TwitchPartScaleMax);
        RegisterFloat(twitchLimbScale);

        queueLastAction = new JSONStorableBool("Queue Last Action", true);
        RegisterBool(queueLastAction);

        debugLog = new JSONStorableBool("Debug Log", false);
        RegisterBool(debugLog);
    }

    JSONStorableStringChooser CreateHiddenActionChooser(string name, List<string> choices, string defaultAction)
    {
        JSONStorableStringChooser chooser = new JSONStorableStringChooser(
            name,
            choices,
            defaultAction,
            name
        );
        RegisterStringChooser(chooser);
        return chooser;
    }

    JSONStorableStringChooser CreateHiddenEventActionChooser(string name, string defaultAction)
    {
        return CreateHiddenActionChooser(name, hbaEventActionChoices, defaultAction);
    }

    JSONStorableStringChooser CreateHiddenTgAtomChooser(string name)
    {
        JSONStorableStringChooser chooser = new JSONStorableStringChooser(
            name,
            new List<string>(hbaTgAtomChoices),
            "",
            name
        );
        RegisterStringChooser(chooser);
        return chooser;
    }

    JSONStorableStringChooser CreateHiddenTgModeChooser(string name, string defaultMode)
    {
        JSONStorableStringChooser chooser = new JSONStorableStringChooser(
            name,
            hbaTgModeChoices,
            defaultMode,
            name
        );
        RegisterStringChooser(chooser);
        return chooser;
    }


    void CreateStatusUi()
    {
        UIDynamicTextField statusField = CreateTextField(hbaStatusText, false);
        if (statusField != null)
        {
            statusField.height = 160.0f;
        }
        UpdateHbaStatus(true);
    }

    void CreateEnableUi()
    {
        CreateToggle(hbaEnabled, false);
    }

    void CreateHeadButtons()
    {
        // Left column: manual Head trial buttons. Event/TG routing settings stay on the right.
        CreateButton("HBA_Head_Shake", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_Shake", "Head Shake"); });
        CreateButton("HBA_Head_Tilt", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_Tilt", "Head Tilt"); });
        CreateButton("HBA_Head_BigNod", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_BigNod", "Head Big Nod"); });
        CreateButton("HBA_Head_Nod", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_Nod", "Head Nod"); });
        CreateButton("HBA_Head_LookUp", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_LookUp", "Head Look Up"); });

        CreateButton("HBA_Head_IntenseShake", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_IntenseShake", "Head Intense Shake"); });
        CreateButton("HBA_Head_EcstasyArch", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_EcstasyArch", "Head Ecstasy Arch"); });
        CreateButton("HBA_Head_RapidOrgasm", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_RapidOrgasm", "Head Rapid Orgasm"); });

        CreateButton("HBA_Head_Shy", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_Shy", "Head Shy"); });
        CreateButton("HBA_Head_LookAround", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_LookAround", "Head Look Around"); });
        CreateButton("HBA_Head_NeckRoll", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_NeckRoll", "Head Neck Roll"); });
        CreateButton("HBA_Head_QuickNod", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_QuickNod", "Head Quick Nod"); });
        CreateButton("HBA_Head_UpEyes", false).button.onClick.AddListener(delegate { RequestHeadOnly("button:HBA_Head_UpEyes", "Head Up Eyes"); });
    }

    void CreateLeftRuntimeSettingsUi()
    {
        // Left column: runtime TW/log controls, directly below Twitch buttons.
        CreateToggle(twitchBody, false);
        CreateSlider(twitchMotionScale, false);
        CreateSlider(twitchUpScale, false);
        CreateSlider(twitchSideScale, false);
        CreateSlider(twitchForwardScale, false);
        CreateSlider(twitchChestScale, false);
        CreateSlider(twitchHipScale, false);
        CreateSlider(twitchLimbScale, false);
        CreateToggle(twitchEyes, false);
        CreateToggle(twitchMouth, false);
        CreateSlider(headTimeScale, false);
        CreateToggle(queueLastAction, false);
        CreateToggle(debugLog, false);

        CreateButton("HBA_Reset", false).button.onClick.AddListener(delegate { StopAllAndReset("button"); });
        CreateButton("HBA_LogStatus", false).button.onClick.AddListener(delegate { LogStatus("button"); });
    }

    void CreateTwitchButtons()
    {
        // Left column: Twitch actions. Keep them directly above Body/Eyes/Mouth toggles.
        CreateButton("HBA_Twitch_Slow", false).button.onClick.AddListener(delegate { RequestPresetAction("button:HBA_Twitch_Slow", "Slow"); });
        CreateButton("HBA_Twitch_Weak", false).button.onClick.AddListener(delegate { RequestPresetAction("button:HBA_Twitch_Weak", "Weak"); });
        CreateButton("HBA_Twitch_Normal", false).button.onClick.AddListener(delegate { RequestPresetAction("button:HBA_Twitch_Normal", "Normal"); });
        CreateButton("HBA_Twitch_Strong", false).button.onClick.AddListener(delegate { RequestPresetAction("button:HBA_Twitch_Strong", "Strong"); });
    }


    void CreateActionLabelUi()
    {
        UIDynamicTextField actionLabelField = CreateTextField(hbaActionLabelText, true);
        if (actionLabelField != null)
        {
            actionLabelField.height = 150.0f;
        }
        UpdateActionLabelText();
    }

    bool IsActionLabelFireActive(string label)
    {
        return !string.IsNullOrEmpty(label) &&
            label == hbaActionLabelFire &&
            Time.time < hbaActionLabelFireUntil;
    }

    string RichLabel(string label, string defaultColor)
    {
        string safe = string.IsNullOrEmpty(label) ? "" : label;
        if (safe == hbaHighlightedActionLabel)
        {
            if (IsActionLabelFireActive(safe))
            {
                return "<color=#ff4040>▶ " + safe + "</color>";
            }
            return "<color=#ffd92e>▶ " + safe + "</color>";
        }
        return "<color=" + defaultColor + ">  " + safe + "</color>";
    }

    void HighlightActionLabel(string label)
    {
        hbaHighlightedActionLabel = string.IsNullOrEmpty(label) ? "" : label;
        if (string.IsNullOrEmpty(hbaHighlightedActionLabel))
        {
            hbaActionLabelFire = "";
            hbaActionLabelFireUntil = -1.0f;
        }
        else
        {
            hbaActionLabelFire = hbaHighlightedActionLabel;
            hbaActionLabelFireUntil = Time.time + HbaActionLabelFireFlashSeconds;
        }
        UpdateActionLabelText();
        UpdateActionChooserHighlights();
    }

    void UpdateActionLabelFireFlash()
    {
        if (!string.IsNullOrEmpty(hbaActionLabelFire) && Time.time >= hbaActionLabelFireUntil)
        {
            hbaActionLabelFire = "";
            hbaActionLabelFireUntil = -1.0f;
            UpdateActionLabelText();
            UpdateActionChooserHighlights();
        }
    }

    bool IsTgLabelFireActive(string label)
    {
        return !string.IsNullOrEmpty(label) &&
            label == hbaTgLabelFire &&
            Time.time < hbaTgLabelFireUntil;
    }

    void HighlightTgLabel(string eventName, bool firing)
    {
        hbaHighlightedTgLabel = string.IsNullOrEmpty(eventName) ? "" : eventName;
        if (firing && !string.IsNullOrEmpty(hbaHighlightedTgLabel))
        {
            hbaTgLabelFire = hbaHighlightedTgLabel;
            hbaTgLabelFireUntil = Time.time + HbaActionLabelFireFlashSeconds;
        }
        UpdateTgChooserHighlights();
    }

    void UpdateTgLabelFireFlash()
    {
        if (!string.IsNullOrEmpty(hbaTgLabelFire) && Time.time >= hbaTgLabelFireUntil)
        {
            hbaTgLabelFire = "";
            hbaTgLabelFireUntil = -1.0f;
            UpdateTgChooserHighlights();
        }
    }

    void UpdateActionLabelText()
    {
        if (hbaActionLabelText == null) return;

        string grey = "#c8c8c8";
        string text =
            "<b>Action Highlight</b>\n" +
            RichLabel("Start Slow", grey) + " / " +
            RichLabel("Start Normal", grey) + " / " +
            RichLabel("Start Fast", grey) + "\n" +
            RichLabel("Inside Hold", grey) + " / " +
            RichLabel("Inside Slow", grey) + "\n" +
            RichLabel("Inside Active", grey) + " / " +
            RichLabel("Inside Fast", grey) + "\n" +
            RichLabel("Deep", grey) + " / " +
            RichLabel("End", grey);

        if (hbaActionLabelText.val != text)
        {
            hbaActionLabelText.val = text;
        }
    }

    UIDynamicPopup CreateActionChooserPopup(JSONStorableStringChooser chooser, string label, bool rightSide)
    {
        UIDynamicPopup popup = CreateScrollablePopup(chooser, rightSide);
        SetActionChooserLabelVisual(popup, label, false, false);
        return popup;
    }

    string NormalizeChooserLabelText(string value)
    {
        if (string.IsNullOrEmpty(value)) return "";

        string s = value;
        s = s.Replace("▶", "");
        s = s.Replace("<b>", "").Replace("</b>", "");
        s = s.Replace("<B>", "").Replace("</B>", "");
        s = s.Replace("<color=yellow>", "").Replace("</color>", "");
        s = s.Replace("<color=red>", "").Replace("</color>", "");
        s = s.Trim();

        while (s.EndsWith(":") || s.EndsWith("："))
        {
            s = s.Substring(0, s.Length - 1).Trim();
        }

        return s;
    }

    Text FindLabelTextUnder(Transform root, string label, bool allowContains)
    {
        if (root == null || string.IsNullOrEmpty(label)) return null;

        Text[] texts = root.GetComponentsInChildren<Text>(true);
        if (texts == null) return null;

        string normalizedLabel = NormalizeChooserLabelText(label);

        // First pass: exact label match. This is safest and avoids selected value text.
        for (int i = 0; i < texts.Length; i++)
        {
            Text text = texts[i];
            if (text == null) continue;
            string current = NormalizeChooserLabelText(text.text);
            if (current == normalizedLabel) return text;
        }

        if (!allowContains) return null;

        // Second pass: VaM sometimes decorates labels or keeps punctuation/spacing around them.
        // Use a conservative contains match only for longer labels such as "Start TG Atom".
        for (int i = 0; i < texts.Length; i++)
        {
            Text text = texts[i];
            if (text == null) continue;
            string current = NormalizeChooserLabelText(text.text);
            if (string.IsNullOrEmpty(current)) continue;
            if (current.Contains(normalizedLabel)) return text;
        }

        return null;
    }

    Text FindActionChooserLabelText(UIDynamicPopup popup, string label)
    {
        if (popup == null || string.IsNullOrEmpty(label)) return null;

        // Usually the label is under the UIDynamicPopup itself.
        Text found = FindLabelTextUnder(popup.transform, label, true);
        if (found != null) return found;

        // Some VaM popup layouts place the label as a sibling/parent-side text instead of a child.
        // Search nearby parents so TG Atom/Mode labels are found just like Action labels.
        Transform parent = popup.transform != null ? popup.transform.parent : null;
        int depth = 0;
        while (parent != null && depth < 3)
        {
            found = FindLabelTextUnder(parent, label, true);
            if (found != null) return found;
            parent = parent.parent;
            depth++;
        }

        return null;
    }

    void SetActionChooserLabelVisual(UIDynamicPopup popup, string label, bool target, bool firing)
    {
        if (popup == null || string.IsNullOrEmpty(label)) return;

        Text text = FindActionChooserLabelText(popup, label);
        if (text == null) return;

        if (!actionPopupDefaultLabelColors.ContainsKey(popup))
        {
            actionPopupDefaultLabelColors[popup] = text.color;
        }

        if (target)
        {
            text.text = "▶ " + label;
            if (firing)
            {
                text.color = new Color(1.0f, 0.18f, 0.18f, 1.0f);
            }
            else
            {
                text.color = new Color(1.0f, 0.86f, 0.20f, 1.0f);
            }
            text.fontStyle = FontStyle.Bold;
        }
        else
        {
            text.text = label;
            Color color;
            if (actionPopupDefaultLabelColors.TryGetValue(popup, out color))
            {
                text.color = color;
            }
            text.fontStyle = FontStyle.Normal;
        }
    }

    string ChooserLabelForActionHighlight(string label)
    {
        if (label == "Start Slow") return "Start Slow Action";
        if (label == "Start Normal") return "Start Normal Action";
        if (label == "Start Fast") return "Start Fast Action";
        if (label == "Inside Hold") return "Inside Hold Action";
        if (label == "Inside Slow") return "Inside Slow Action";
        if (label == "Inside Active") return "Inside Active Action";
        if (label == "Inside Fast") return "Inside Fast Action";
        if (label == "Deep") return "Deep Action";
        if (label == "End") return "End Action";
        return "";
    }

    void UpdateActionChooserHighlights()
    {
        string activeChooserLabel = ChooserLabelForActionHighlight(hbaHighlightedActionLabel);
        string fireChooserLabel = IsActionLabelFireActive(hbaActionLabelFire) ? ChooserLabelForActionHighlight(hbaActionLabelFire) : "";

        SetActionChooserLabelVisual(uiStartNormalAction, "Start Normal Action", activeChooserLabel == "Start Normal Action", fireChooserLabel == "Start Normal Action");
        SetActionChooserLabelVisual(uiStartSlowAction, "Start Slow Action", activeChooserLabel == "Start Slow Action", fireChooserLabel == "Start Slow Action");
        SetActionChooserLabelVisual(uiStartFastAction, "Start Fast Action", activeChooserLabel == "Start Fast Action", fireChooserLabel == "Start Fast Action");
        SetActionChooserLabelVisual(uiInsideActiveAction, "Inside Active Action", activeChooserLabel == "Inside Active Action", fireChooserLabel == "Inside Active Action");
        SetActionChooserLabelVisual(uiInsideHoldAction, "Inside Hold Action", activeChooserLabel == "Inside Hold Action", fireChooserLabel == "Inside Hold Action");
        SetActionChooserLabelVisual(uiInsideSlowAction, "Inside Slow Action", activeChooserLabel == "Inside Slow Action", fireChooserLabel == "Inside Slow Action");
        SetActionChooserLabelVisual(uiInsideIntenseAction, "Inside Fast Action", activeChooserLabel == "Inside Fast Action", fireChooserLabel == "Inside Fast Action");
        SetActionChooserLabelVisual(uiDeepAction, "Deep Action", activeChooserLabel == "Deep Action", fireChooserLabel == "Deep Action");
        SetActionChooserLabelVisual(uiEndAction, "End Action", activeChooserLabel == "End Action", fireChooserLabel == "End Action");
    }

    void CreateEventSettingsUi()
    {
        // v020: no separate Action Highlight text panel.
        // The current target branch stays yellow. At the exact fire moment it flashes red, then returns to yellow.
        uiStartNormalAction = CreateActionChooserPopup(hbaStartNormalAction, "Start Normal Action", true);
        uiStartSlowAction = CreateActionChooserPopup(hbaStartSlowAction, "Start Slow Action", true);
        uiStartFastAction = CreateActionChooserPopup(hbaStartFastAction, "Start Fast Action", true);
        uiInsideActiveAction = CreateActionChooserPopup(hbaInsideActiveAction, "Inside Active Action", true);
        uiInsideHoldAction = CreateActionChooserPopup(hbaInsideHoldAction, "Inside Hold Action", true);
        uiInsideSlowAction = CreateActionChooserPopup(hbaInsideSlowAction, "Inside Slow Action", true);
        uiInsideIntenseAction = CreateActionChooserPopup(hbaInsideIntenseAction, "Inside Fast Action", true);
        uiDeepAction = CreateActionChooserPopup(hbaEventDeepAction, "Deep Action", true);
        uiEndAction = CreateActionChooserPopup(hbaEventEndAction, "End Action", true);
        UpdateActionChooserHighlights();
        CreateToggle(hbaMotionBoost, true);
    }

    UIDynamicPopup CreateTgChooserPopup(JSONStorableStringChooser chooser, string label, bool rightSide)
    {
        UIDynamicPopup popup = CreateScrollablePopup(chooser, rightSide);
        SetActionChooserLabelVisual(popup, label, false, false);
        return popup;
    }

    void SetTgEventLabelVisual(string eventName, UIDynamicPopup atomPopup, string atomLabel, UIDynamicPopup modePopup, string modeLabel)
    {
        bool target = hbaHighlightedTgLabel == eventName;
        bool firing = IsTgLabelFireActive(eventName);
        // TG highlight is shown only on the TG Atom row. Mode rows stay normal.
        SetActionChooserLabelVisual(atomPopup, atomLabel, target, firing);
        SetActionChooserLabelVisual(modePopup, modeLabel, false, false);
    }

    void UpdateTgChooserHighlights()
    {
        SetTgEventLabelVisual("Start", uiTgStartAtom, "Start TG Atom", uiTgStartMode, "Start TG Mode");
        SetTgEventLabelVisual("Inside", uiTgInsideAtom, "Inside TG Atom", uiTgInsideMode, "Inside TG Mode");
        SetTgEventLabelVisual("Deep", uiTgDeepAtom, "Deep TG Atom", uiTgDeepMode, "Deep TG Mode");
        SetTgEventLabelVisual("End", uiTgEndAtom, "End TG Atom", uiTgEndMode, "End TG Mode");
    }

    void CreateTgSettingsUi()
    {
        CreateToggle(hbaTgTriggers, true);
        // TG Prefix is registered but no longer shown as a text field. The text UI is hard to edit in VaM,
        // and the fixed TG_ convention is safer for this plugin.
        CreateButton("Refresh TG/HBA List", true).button.onClick.AddListener(delegate { RefreshHbaTgAtomList(); });

        uiTgStartAtom = CreateTgChooserPopup(hbaTgStartAtom, "Start TG Atom", true);
        uiTgStartMode = CreateTgChooserPopup(hbaTgStartMode, "Start TG Mode", true);
        uiTgInsideAtom = CreateTgChooserPopup(hbaTgInsideAtom, "Inside TG Atom", true);
        uiTgInsideMode = CreateTgChooserPopup(hbaTgInsideMode, "Inside TG Mode", true);
        uiTgDeepAtom = CreateTgChooserPopup(hbaTgDeepAtom, "Deep TG Atom", true);
        uiTgDeepMode = CreateTgChooserPopup(hbaTgDeepMode, "Deep TG Mode", true);
        uiTgEndAtom = CreateTgChooserPopup(hbaTgEndAtom, "End TG Atom", true);
        uiTgEndMode = CreateTgChooserPopup(hbaTgEndMode, "End TG Mode", true);
        UpdateTgChooserHighlights();
    }

    void RegisterHbaActions()
    {
        RegisterAction(new JSONStorableAction("HBA_Twitch_Slow", delegate { RequestPresetAction("action:HBA_Twitch_Slow", "Slow"); }));
        RegisterAction(new JSONStorableAction("HBA_Twitch_Weak", delegate { RequestPresetAction("action:HBA_Twitch_Weak", "Weak"); }));
        RegisterAction(new JSONStorableAction("HBA_Twitch_Normal", delegate { RequestPresetAction("action:HBA_Twitch_Normal", "Normal"); }));
        RegisterAction(new JSONStorableAction("HBA_Twitch_Strong", delegate { RequestPresetAction("action:HBA_Twitch_Strong", "Strong"); }));

        RegisterAction(new JSONStorableAction("HBA_Event_Start", delegate { HandleHbaEvent("Start", 0); }));
        RegisterAction(new JSONStorableAction("HBA_Event_Inside", delegate { HandleHbaEvent("Inside", 0); }));
        RegisterAction(new JSONStorableAction("HBA_Event_Deep", delegate { HandleHbaEvent("Deep", 0); }));
        RegisterAction(new JSONStorableAction("HBA_Event_End", delegate { HandleHbaEvent("End", 0); }));
        RegisterAction(new JSONStorableAction("HBA_Gen_Start", delegate { HandleHbaEvent("Start", 1); }));
        RegisterAction(new JSONStorableAction("HBA_Gen_Inside", delegate { HandleHbaEvent("Inside", 1); }));
        RegisterAction(new JSONStorableAction("HBA_Gen_Deep", delegate { HandleHbaEvent("Deep", 1); }));
        RegisterAction(new JSONStorableAction("HBA_Gen_End", delegate { HandleHbaEvent("End", 1); }));
        RegisterAction(new JSONStorableAction("HBA_Anus_Start", delegate { HandleHbaEvent("Start", 2); }));
        RegisterAction(new JSONStorableAction("HBA_Anus_Inside", delegate { HandleHbaEvent("Inside", 2); }));
        RegisterAction(new JSONStorableAction("HBA_Anus_Deep", delegate { HandleHbaEvent("Deep", 2); }));
        RegisterAction(new JSONStorableAction("HBA_Anus_End", delegate { HandleHbaEvent("End", 2); }));
        RegisterAction(new JSONStorableAction("HBA_Mouth_Start", delegate { HandleHbaEvent("Start", 3); }));
        RegisterAction(new JSONStorableAction("HBA_Mouth_Inside", delegate { HandleHbaEvent("Inside", 3); }));
        RegisterAction(new JSONStorableAction("HBA_Mouth_Deep", delegate { HandleHbaEvent("Deep", 3); }));
        RegisterAction(new JSONStorableAction("HBA_Mouth_End", delegate { HandleHbaEvent("End", 3); }));

        RegisterAction(new JSONStorableAction("HBA_Head_Shake", delegate { RequestHeadOnly("action:HBA_Head_Shake", "Head Shake"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_Tilt", delegate { RequestHeadOnly("action:HBA_Head_Tilt", "Head Tilt"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_BigNod", delegate { RequestHeadOnly("action:HBA_Head_BigNod", "Head Big Nod"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_Nod", delegate { RequestHeadOnly("action:HBA_Head_Nod", "Head Nod"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_LookUp", delegate { RequestHeadOnly("action:HBA_Head_LookUp", "Head Look Up"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_IntenseShake", delegate { RequestHeadOnly("action:HBA_Head_IntenseShake", "Head Intense Shake"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_EcstasyArch", delegate { RequestHeadOnly("action:HBA_Head_EcstasyArch", "Head Ecstasy Arch"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_RapidOrgasm", delegate { RequestHeadOnly("action:HBA_Head_RapidOrgasm", "Head Rapid Orgasm"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_Shy", delegate { RequestHeadOnly("action:HBA_Head_Shy", "Head Shy"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_LookAround", delegate { RequestHeadOnly("action:HBA_Head_LookAround", "Head Look Around"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_NeckRoll", delegate { RequestHeadOnly("action:HBA_Head_NeckRoll", "Head Neck Roll"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_QuickNod", delegate { RequestHeadOnly("action:HBA_Head_QuickNod", "Head Quick Nod"); }));
        RegisterAction(new JSONStorableAction("HBA_Head_UpEyes", delegate { RequestHeadOnly("action:HBA_Head_UpEyes", "Head Up Eyes"); }));

        RegisterAction(new JSONStorableAction("HBA_LogStatus", delegate { LogStatus("action"); }));
        RegisterAction(new JSONStorableAction("HBA_Reset", delegate { StopAllAndReset("action"); }));
    }


    void Update()
    {
        UpdateActionLabelFireFlash();
        UpdateTgLabelFireFlash();
        UpdateHbaStatus(false);
    }

    bool IsHbaEnabled()
    {
        return hbaEnabled == null || hbaEnabled.val;
    }

    void HandleHbaEvent(string eventName, int targetOverride)
    {
        if (targetOverride > 0)
        {
            SetFloat(hbaTargetId, (float)targetOverride);
        }

        hbaLastEvent = eventName;

        if (!IsHbaEnabled())
        {
            hbaLastBlock = "Disabled: event received, action skipped";
            if (eventName == "End")
            {
                SetBool(hbaActive, false);
                SetFloat(hbaProgress, 0.0f);
                StopStartDecisionRoutine();
                StopInsideMonitorRoutine("disabled-end");
            }
            else
            {
                SetBool(hbaActive, true);
                EnsureEventProgressFloor(eventName);
            }
            UpdateHbaStatus(true);
            return;
        }

        hbaLastBlock = "";

        if (eventName == "End")
        {
            SetBool(hbaActive, false);
            SetFloat(hbaProgress, 0.0f);
            StopStartDecisionRoutine();
            StopInsideMonitorRoutine("end");
            FireTgEvent("End");
            RunConfiguredEventAction("End");
            UpdateHbaStatus(true);
            return;
        }

        SetBool(hbaActive, true);
        EnsureEventProgressFloor(eventName);

        if (eventName == "Start")
        {
            FireTgEvent("Start");
            StartStartDecisionRoutine();
        }
        else if (eventName == "Inside")
        {
            FireTgEvent("Inside");
            StartInsideMonitorRoutine();
        }
        else if (eventName == "Deep")
        {
            FireTgEvent("Deep");
            RunConfiguredEventAction("Deep");
        }
        UpdateHbaStatus(true);
    }

    void EnsureEventProgressFloor(string eventName)
    {
        if (eventName == "Start")
        {
            if (hbaProgress != null && hbaProgress.val < 0.001f) SetFloat(hbaProgress, 0.01f);
        }
        else if (eventName == "Inside")
        {
            if (hbaProgress != null && hbaProgress.val < 0.050f) SetFloat(hbaProgress, 0.10f);
        }
        else if (eventName == "Deep")
        {
            if (hbaProgress != null && hbaProgress.val < 1.000f) SetFloat(hbaProgress, 1.00f);
        }
    }

    void StartStartDecisionRoutine()
    {
        StopStartDecisionRoutine();
        float startProgress = hbaProgress != null ? hbaProgress.val : 0.0f;
        float startTime = Time.time;
        hbaStartDecisionRoutine = StartCoroutine(StartDecisionRoutine(startProgress, startTime));
    }

    void StopStartDecisionRoutine()
    {
        if (hbaStartDecisionRoutine != null)
        {
            StopCoroutine(hbaStartDecisionRoutine);
            hbaStartDecisionRoutine = null;
        }
    }

    IEnumerator StartDecisionRoutine(float startProgress, float startTime)
    {
        yield return new WaitForSeconds(HbaStartDecisionDelay);

        hbaStartDecisionRoutine = null;

        if (!IsHbaEnabled() || hbaActive == null || !hbaActive.val)
        {
            yield break;
        }

        float nowProgress = hbaProgress != null ? hbaProgress.val : 0.0f;
        float dt = Mathf.Max(0.001f, Time.time - startTime);
        float speed = Mathf.Max(0.0f, nowProgress - startProgress) / dt;
        float peakSpeed = GetFreshFastPeakSpeed();
        if (peakSpeed > speed) speed = peakSpeed;
        string startType = "Normal";
        if (speed < HbaStartSlowSpeed) startType = "Slow";
        else if (speed >= HbaStartFastSpeed) startType = "Fast";

        string actionName = GetConfiguredStartAction(startType);
        hbaLastEvent = "Start " + startType;
        HighlightActionLabel("Start " + startType);
        hbaLastBlock = "StartSpeed=" + speed.ToString("F2") + " Peak=" + peakSpeed.ToString("F2");
        TriggerConfiguredAction("event:HBA_Event_Start:" + startType, actionName);
        UpdateHbaStatus(true);
    }

    void StartInsideMonitorRoutine()
    {
        if (hbaInsideMonitorRoutine != null)
        {
            hbaLastBlock = "Inside monitor already active";
            return;
        }

        hbaInsideMotionState = "Pending";
        hbaLastInsideActionMotion = "None";
        hbaLastInsideActionTime = -999.0f;
        hbaInsideFastEnterSince = -1.0f;
        hbaInsideFastExitSince = -1.0f;
        hbaInsideSlowEnterSince = -1.0f;
        // v030: keep any Fast peak that happened just before Inside event/monitor startup.
        hbaInsideReactionEnergy = 0.0f;
        hbaInsideHoldEnergy = 0.0f;
        hbaLastInsideEnergyUpdateTime = -1.0f;
        hbaLastInsideTurnActionTime = -999.0f;
        hbaLastInsideHoldPulseTime = -999.0f;
        hbaLastInsideDirection = 0;
        hbaInsideMonitorRoutine = StartCoroutine(InsideMonitorRoutine());
    }

    void StopInsideMonitorRoutine(string reason)
    {
        if (hbaInsideMonitorRoutine != null)
        {
            StopCoroutine(hbaInsideMonitorRoutine);
            hbaInsideMonitorRoutine = null;
        }
        hbaInsideMotionState = "None";
        hbaLastInsideActionMotion = "None";
        hbaInsideFastEnterSince = -1.0f;
        hbaInsideFastExitSince = -1.0f;
        hbaInsideSlowEnterSince = -1.0f;
        ClearFastPeakLatch();
        hbaInsideReactionEnergy = 0.0f;
        hbaInsideHoldEnergy = 0.0f;
        hbaLastInsideEnergyUpdateTime = -1.0f;
        hbaLastInsideDirection = 0;
        DebugMessage("[HumanBodyAction] Inside monitor stopped / reason=" + reason);
    }

    IEnumerator InsideMonitorRoutine()
    {
        yield return new WaitForSeconds(HbaInsideFirstDelay);

        while (hbaActive != null && hbaActive.val)
        {
            UpdateInsideReactionEnergy();

            string motion = ResolveInsideActionMotion();
            if (!string.IsNullOrEmpty(motion))
            {
                hbaInsideMotionState = motion;
                bool changed = motion != hbaLastInsideActionMotion;
                bool cooled = Time.time - hbaLastInsideActionTime >= HbaInsideMotionCooldown;
                if (changed && cooled)
                {
                    FireInsideMotionAction(motion, "motion-change");
                }
                else if (motion == "Hold")
                {
                    TryFireInsideHoldEnergyAction();
                }
                else
                {
                    TryFireInsideTurnEnergyAction(motion);
                }
            }
            else
            {
                TrackInsideDirectionOnly();
            }
            yield return new WaitForSeconds(HbaStatusUpdateInterval);
        }

        hbaInsideMonitorRoutine = null;
        hbaInsideMotionState = "None";
    }

    void UpdateInsideReactionEnergy()
    {
        float now = Time.time;
        float dt = hbaLastInsideEnergyUpdateTime < 0.0f ? HbaStatusUpdateInterval : Mathf.Clamp(now - hbaLastInsideEnergyUpdateTime, 0.001f, 0.25f);
        hbaLastInsideEnergyUpdateTime = now;

        float progress = hbaProgress != null ? Mathf.Clamp(hbaProgress.val, 0.0f, 1.2f) : 0.0f;
        float speed = Mathf.Max(0.0f, hbaDepthSpeed);
        float depthBoost = 0.75f + Mathf.Clamp01(progress) * 0.65f;
        float motionBoost = 1.0f;
        if (hbaInsideMotionState == "Fast") motionBoost = 1.35f;
        else if (hbaInsideMotionState == "Active") motionBoost = 1.15f;
        else if (hbaInsideMotionState == "Slow") motionBoost = 0.90f;

        if (speed > HbaStatusSpeedIdle * 0.5f)
        {
            hbaInsideReactionEnergy += speed * dt * depthBoost * motionBoost;
        }
        else
        {
            hbaInsideReactionEnergy -= 0.08f * dt;
        }
        hbaInsideReactionEnergy = Mathf.Clamp(hbaInsideReactionEnergy, 0.0f, HbaInsideReactionEnergyMax);

        bool holding = hbaMotionState == "Hold" || hbaMotionState == "DeepHold" || hbaInsideMotionState == "Hold";
        if (holding)
        {
            float previousMotionBoost = 0.25f + Mathf.Clamp01(hbaInsideReactionEnergy);
            float deepBoost = progress >= 0.70f ? 0.45f : 0.0f;
            hbaInsideHoldEnergy += dt * (0.18f + Mathf.Clamp01(progress) * 0.34f + previousMotionBoost * 0.22f + deepBoost);
        }
        else
        {
            hbaInsideHoldEnergy -= 0.30f * dt;
        }
        hbaInsideHoldEnergy = Mathf.Clamp(hbaInsideHoldEnergy, 0.0f, HbaInsideHoldEnergyMax);
    }

    void TrackInsideDirectionOnly()
    {
        if (hbaDepthDirection != 0)
        {
            hbaLastInsideDirection = hbaDepthDirection;
        }
    }

    bool IsInboundTurn()
    {
        return hbaLastInsideDirection < 0 && hbaDepthDirection > 0;
    }

    void TryFireInsideTurnEnergyAction(string motion)
    {
        if (motion == "Hold" || string.IsNullOrEmpty(motion)) return;

        bool inboundTurn = IsInboundTurn();
        if (hbaDepthDirection != 0)
        {
            hbaLastInsideDirection = hbaDepthDirection;
        }

        if (!inboundTurn) return;
        if (Time.time - hbaLastInsideTurnActionTime < HbaInsideTurnCooldown) return;
        if (Time.time - hbaLastInsideActionTime < HbaInsideMotionCooldown) return;
        if (hbaInsideReactionEnergy < HbaInsideReactionEnergyThreshold)
        {
            hbaLastBlock = "TurnGate E=" + hbaInsideReactionEnergy.ToString("F2");
            return;
        }

        hbaLastInsideTurnActionTime = Time.time;
        hbaInsideReactionEnergy = Mathf.Min(hbaInsideReactionEnergy, HbaInsideEnergyAfterFire);
        FireInsideMotionAction(motion, "inbound-turn");
    }

    void TryFireInsideHoldEnergyAction()
    {
        if (hbaDepthDirection != 0)
        {
            hbaLastInsideDirection = hbaDepthDirection;
        }

        if (Time.time - hbaLastInsideHoldPulseTime < HbaInsideHoldPulseCooldown) return;
        if (Time.time - hbaLastInsideActionTime < HbaInsideMotionCooldown) return;
        if (hbaInsideHoldEnergy < HbaInsideHoldEnergyThreshold)
        {
            hbaLastBlock = "HoldGate H=" + hbaInsideHoldEnergy.ToString("F2");
            return;
        }

        hbaLastInsideHoldPulseTime = Time.time;
        hbaInsideHoldEnergy = 0.0f;
        hbaInsideReactionEnergy = Mathf.Min(hbaInsideReactionEnergy, HbaInsideEnergyAfterFire);
        FireInsideMotionAction("Hold", "hold-energy");
    }

    void FireInsideMotionAction(string motion, string reason)
    {
        hbaLastInsideActionMotion = motion;
        hbaLastInsideActionTime = Time.time;
        string actionName = GetConfiguredInsideAction(motion);
        hbaLastEvent = "Inside " + motion;
        HighlightActionLabel("Inside " + motion);
        hbaLastBlock = "Inside " + reason;
        TriggerConfiguredAction("event:HBA_Event_Inside:" + motion + ":" + reason, actionName);
        UpdateHbaStatus(true);
    }

    string ResolveInsideActionMotion()
    {
        if (hbaActive == null || !hbaActive.val) return "";

        // v030: Auto Line Fast can complete and begin returning before the old 0.25s Inside classifier wakes up.
        // Keep a short inward-speed peak latch so Fast is not missed just because the live direction is already outward.
        float peakSpeed = GetFreshFastPeakSpeed();
        if (hbaDepthDirection < 0)
        {
            hbaInsideFastEnterSince = -1.0f;
            hbaInsideFastExitSince = -1.0f;
            hbaInsideSlowEnterSince = -1.0f;
            if (peakSpeed >= HbaStatusSpeedFastEnter)
            {
                hbaLastBlock = "FastPeakLatch return-dir speed=" + peakSpeed.ToString("F2");
                return "Fast";
            }
            return "";
        }

        float classifySpeed = Mathf.Max(hbaDepthInSpeedAvg, peakSpeed);

        if (classifySpeed < HbaStatusSpeedIdle)
        {
            hbaInsideFastEnterSince = -1.0f;
            hbaInsideSlowEnterSince = -1.0f;
            if (hbaHoldSince >= 0.0f && Time.time - hbaHoldSince >= HbaInsideHoldDelay)
            {
                return "Hold";
            }
            return "";
        }

        if (classifySpeed < HbaStatusSpeedSlow)
        {
            hbaInsideFastEnterSince = -1.0f;
            hbaInsideFastExitSince = -1.0f;

            bool comingFromMovingLine = hbaInsideMotionState == "Active" ||
                hbaInsideMotionState == "Fast" ||
                hbaLastInsideActionMotion == "Active" ||
                hbaLastInsideActionMotion == "Fast";
            if (comingFromMovingLine)
            {
                if (hbaInsideSlowEnterSince < 0.0f) hbaInsideSlowEnterSince = Time.time;
                if (Time.time - hbaInsideSlowEnterSince < HbaInsideSlowEnterSeconds)
                {
                    hbaLastBlock = "SlowGrace speed=" + classifySpeed.ToString("F2");
                    return "Active";
                }
            }
            else
            {
                hbaInsideSlowEnterSince = -1.0f;
            }
            return "Slow";
        }

        hbaInsideSlowEnterSince = -1.0f;
        bool currentlyFast = hbaInsideMotionState == "Fast"; // v032: previous Fast action should not keep the classifier sticky-Fast
        if (currentlyFast)
        {
            hbaInsideFastEnterSince = -1.0f;
            if (classifySpeed <= HbaStatusSpeedFastExit)
            {
                if (hbaInsideFastExitSince < 0.0f) hbaInsideFastExitSince = Time.time;
                if (Time.time - hbaInsideFastExitSince >= HbaInsideFastExitSeconds)
                {
                    hbaInsideFastExitSince = -1.0f;
                    return "Active";
                }
            }
            else
            {
                hbaInsideFastExitSince = -1.0f;
            }
            return "Fast";
        }

        hbaInsideFastExitSince = -1.0f;
        if (classifySpeed >= HbaStatusSpeedFastEnter)
        {
            if (hbaInsideFastEnterSince < 0.0f) hbaInsideFastEnterSince = Time.time;
            if (Time.time - hbaInsideFastEnterSince >= HbaInsideFastEnterSeconds)
            {
                hbaInsideFastEnterSince = -1.0f;
                return "Fast";
            }
        }
        else
        {
            hbaInsideFastEnterSince = -1.0f;
        }

        return "Active";
    }

    void ClearFastPeakLatch()
    {
        hbaFastPeakTime = -999.0f;
        hbaFastPeakSpeed = 0.0f;
        hbaFastPeakProgress = 0.0f;
    }

    void UpdateFastPeakLatch(float progress, float rawSpeed, float avgSpeed)
    {
        if (hbaActive == null || !hbaActive.val) return;
        if (hbaDepthDirection <= 0) return;
        if (progress < HbaFastPeakMinProgress) return;

        float speed = Mathf.Max(rawSpeed, avgSpeed);
        if (speed < HbaStatusSpeedFastEnter) return;

        hbaFastPeakTime = Time.time;
        hbaFastPeakSpeed = speed;
        hbaFastPeakProgress = progress;
    }

    float GetFreshFastPeakSpeed()
    {
        if (hbaFastPeakTime < 0.0f) return 0.0f;
        if (Time.time - hbaFastPeakTime > HbaFastPeakLatchSeconds) return 0.0f;
        return hbaFastPeakSpeed;
    }

    void RunConfiguredEventAction(string eventName)
    {
        string actionName = GetConfiguredEventAction(eventName);
        actionName = ApplyMotionBoostToAction(actionName);
        HighlightActionLabel(eventName);
        TriggerConfiguredAction("event:HBA_Event_" + eventName, actionName);
    }

    string GetConfiguredEventAction(string eventName)
    {
        if (eventName == "Start") return GetConfiguredStartAction("Normal");
        if (eventName == "Inside") return GetConfiguredInsideAction("Active");
        if (eventName == "Deep") return GetChooserValue(hbaEventDeepAction, HbaActionTwitchStrong);
        if (eventName == "End") return GetChooserValue(hbaEventEndAction, HbaActionTwitchWeak);
        return HbaActionOff;
    }

    string GetConfiguredStartAction(string startType)
    {
        string normalAction = GetChooserValue(hbaStartNormalAction, HbaActionTwitchWeak);
        if (startType == "Slow")
        {
            string slowAction = GetChooserValue(hbaStartSlowAction, HbaActionSameAsNormal);
            return slowAction == HbaActionSameAsNormal ? normalAction : slowAction;
        }
        if (startType == "Fast")
        {
            string fastAction = GetChooserValue(hbaStartFastAction, HbaActionSameAsNormal);
            return fastAction == HbaActionSameAsNormal ? normalAction : fastAction;
        }
        return normalAction;
    }

    string GetConfiguredInsideAction(string motion)
    {
        string activeAction = GetChooserValue(hbaInsideActiveAction, HbaActionTwitchNormal);
        if (motion == "Hold")
        {
            string holdAction = GetChooserValue(hbaInsideHoldAction, HbaActionSameAsActive);
            return holdAction == HbaActionSameAsActive ? activeAction : holdAction;
        }
        if (motion == "Slow")
        {
            string slowAction = GetChooserValue(hbaInsideSlowAction, HbaActionSameAsActive);
            return slowAction == HbaActionSameAsActive ? activeAction : slowAction;
        }
        if (motion == "Fast")
        {
            string fastAction = GetChooserValue(hbaInsideIntenseAction, HbaActionSameAsActive);
            return fastAction == HbaActionSameAsActive ? activeAction : fastAction;
        }
        return activeAction;
    }

    string ApplyMotionBoostToAction(string actionName)
    {
        if (hbaMotionBoost == null || !hbaMotionBoost.val || IsOff(actionName))
        {
            return actionName;
        }

        if (actionName == HbaActionTwitchNormal)
        {
            if (hbaMotionState == "Fast" || hbaMotionState == "DeepHold") return HbaActionTwitchStrong;
            if (hbaMotionState == "Hold" || hbaMotionState == "Slow") return HbaActionTwitchWeak;
        }
        else if (actionName == HbaActionTwitchWeak)
        {
            if (hbaMotionState == "Fast") return HbaActionTwitchNormal;
        }

        return actionName;
    }

    bool TriggerConfiguredAction(string source, string actionName)
    {
        if (IsOff(actionName))
        {
            DebugMessage("[HumanBodyAction] Event action off / source=" + source);
            return false;
        }

        if (actionName == HbaActionTwitchSlow)
        {
            RequestPresetAction(source + ":" + actionName, "Slow");
            return true;
        }
        if (actionName == HbaActionTwitchWeak)
        {
            RequestPresetAction(source + ":" + actionName, "Weak");
            return true;
        }
        if (actionName == HbaActionTwitchNormal)
        {
            RequestPresetAction(source + ":" + actionName, "Normal");
            return true;
        }
        if (actionName == HbaActionTwitchStrong)
        {
            RequestPresetAction(source + ":" + actionName, "Strong");
            return true;
        }

        string headPreset = GetHeadPresetFromHbaAction(actionName);
        if (!IsOff(headPreset))
        {
            RequestHeadOnly(source + ":" + actionName, headPreset);
            return true;
        }

        hbaLastBlock = "Unknown action: " + actionName;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Unknown event action / source=" + source + " / action=" + actionName);
        return false;
    }

    string GetHeadPresetFromHbaAction(string actionName)
    {
        if (actionName == "HBA_Head_Shake") return "Head Shake";
        if (actionName == "HBA_Head_Tilt") return "Head Tilt";
        if (actionName == "HBA_Head_BigNod") return "Head Big Nod";
        if (actionName == "HBA_Head_Nod") return "Head Nod";
        if (actionName == "HBA_Head_LookUp") return "Head Look Up";
        if (actionName == "HBA_Head_IntenseShake") return "Head Intense Shake";
        if (actionName == "HBA_Head_EcstasyArch") return "Head Ecstasy Arch";
        if (actionName == "HBA_Head_RapidOrgasm") return "Head Rapid Orgasm";
        if (actionName == "HBA_Head_Shy") return "Head Shy";
        if (actionName == "HBA_Head_LookAround") return "Head Look Around";
        if (actionName == "HBA_Head_NeckRoll") return "Head Neck Roll";
        if (actionName == "HBA_Head_QuickNod") return "Head Quick Nod";
        if (actionName == "HBA_Head_UpEyes") return "Head Up Eyes";
        return HbaActionOff;
    }

    void MarkHbaDataReceived()
    {
        hbaLastDataReceiveTime = Time.time;
    }

    void UpdateHbaStatus(bool force)
    {
        if (!force && Time.time - hbaLastStatusUpdateTime < HbaStatusUpdateInterval)
        {
            return;
        }

        float now = Time.time;
        float progress = hbaProgress != null ? Mathf.Clamp(hbaProgress.val, 0.0f, 1.2f) : 0.0f;
        bool active = hbaActive != null && hbaActive.val;

        if (hbaLastProgressSampleTime >= 0.0f)
        {
            float dt = Mathf.Max(0.001f, now - hbaLastProgressSampleTime);
            float delta = progress - hbaLastProgress;
            hbaDepthSpeed = Mathf.Abs(delta) / dt;
            hbaDepthDirection = delta > 0.002f ? 1 : (delta < -0.002f ? -1 : 0);
            hbaDepthInSpeedRaw = hbaDepthDirection > 0 ? Mathf.Max(0.0f, delta) / dt : 0.0f;
            hbaDepthInSpeedAvg = Mathf.Lerp(hbaDepthInSpeedAvg, hbaDepthInSpeedRaw, HbaInSpeedSmoothing);
            UpdateFastPeakLatch(progress, hbaDepthInSpeedRaw, hbaDepthInSpeedAvg);
        }
        else
        {
            hbaDepthInSpeedRaw = 0.0f;
            hbaDepthInSpeedAvg = 0.0f;
        }

        if (!active)
        {
            hbaDepthInSpeedRaw = 0.0f;
            hbaDepthInSpeedAvg = 0.0f;
            ClearFastPeakLatch();
        }

        hbaLastProgressSampleTime = now;
        hbaLastProgress = progress;
        hbaMotionState = ResolveMotionState(active, progress, hbaDepthSpeed);

        if (hbaStatusText != null)
        {
            string dataAge = hbaLastDataReceiveTime < 0.0f ? "n/a" : Mathf.Max(0.0f, now - hbaLastDataReceiveTime).ToString("F2") + "s";
            string text =
                "HBA " + hbaMotionState +
                "  Enable: " + IsHbaEnabled() +
                "\nTarget: " + GetTargetNameFromId(GetTargetId()) +
                "  Active: " + active +
                "  Data Age: " + dataAge +
                "\nProgress: " + Mathf.RoundToInt(progress * 100.0f) + "%" +
                "  Speed: " + hbaDepthSpeed.ToString("F2") +
                "  InAvg: " + hbaDepthInSpeedAvg.ToString("F2") +
                "  Peak: " + GetFreshFastPeakSpeed().ToString("F2") +
                "  Dir: " + GetDirectionLabel(hbaDepthDirection) +
                "  E/H: " + hbaInsideReactionEnergy.ToString("F2") + "/" + hbaInsideHoldEnergy.ToString("F2") +
                "\nEvent: " + hbaLastEvent +
                "  Action: " + hbaLastAction +
                "\nTG: " + (hbaTgTriggers != null && hbaTgTriggers.val) +
                "  Block: " + (string.IsNullOrEmpty(hbaLastBlock) ? "None" : hbaLastBlock);
            if (hbaStatusText.val != text)
            {
                hbaStatusText.val = text;
            }
        }

        hbaLastStatusUpdateTime = now;
    }

    string ResolveMotionState(bool active, float progress, float speed)
    {
        if (!active)
        {
            hbaHoldSince = -1.0f;
            return "Idle";
        }

        float classifySpeed = hbaDepthInSpeedAvg;

        if (classifySpeed < HbaStatusSpeedIdle)
        {
            if (hbaHoldSince < 0.0f) hbaHoldSince = Time.time;
            if (Time.time - hbaHoldSince >= HbaStatusHoldSeconds)
            {
                return progress >= 0.95f ? "DeepHold" : "Hold";
            }
        }
        else
        {
            hbaHoldSince = -1.0f;
        }

        if (hbaDepthDirection < 0)
        {
            return string.IsNullOrEmpty(hbaInsideMotionState) || hbaInsideMotionState == "None" ? "Active" : hbaInsideMotionState;
        }

        if (classifySpeed < HbaStatusSpeedSlow) return "Slow";
        if (classifySpeed < HbaStatusSpeedFastEnter) return "Active";
        return "Fast";
    }

    int GetTargetId()
    {
        return hbaTargetId != null ? Mathf.RoundToInt(hbaTargetId.val) : 0;
    }

    string GetTargetNameFromId(int id)
    {
        if (id == 1) return "Gen";
        if (id == 2) return "Anus";
        if (id == 3) return "Mouth";
        return "None";
    }

    string GetDirectionLabel(int dir)
    {
        if (dir > 0) return "In";
        if (dir < 0) return "Out";
        return "Hold";
    }

    void FireTgEvent(string eventName)
    {
        if (hbaTgTriggers == null || !hbaTgTriggers.val)
        {
            return;
        }

        // End always clears state-like channels first, even when the End slot itself is off.
        if (eventName == "End")
        {
            SetTgSlot("Inside", false, "end-inside-off");
            SetTgSlot("Deep", false, "end-deep-off");
        }

        string mode = GetTgModeForEvent(eventName);
        if (mode == TgModeOff)
        {
            HighlightTgLabel(eventName, false);
            DebugMessage("[HumanBodyAction] TG event off / event=" + eventName);
            return;
        }

        HighlightTgLabel(eventName, true);

        if (mode == TgModeState)
        {
            SetTgSlot(eventName, true, "state-on");
            return;
        }

        float seconds = GetTgTimedDuration(mode);
        if (seconds > 0.0f)
        {
            PulseTg(eventName, seconds);
        }
    }

    string GetTgModeForEvent(string eventName)
    {
        JSONStorableStringChooser chooser = GetTgModeChooser(eventName);
        if (chooser == null || string.IsNullOrEmpty(chooser.val))
        {
            return TgModeOff;
        }
        return chooser.val;
    }

    JSONStorableStringChooser GetTgModeChooser(string eventName)
    {
        if (eventName == "Start") return hbaTgStartMode;
        if (eventName == "Inside") return hbaTgInsideMode;
        if (eventName == "Deep") return hbaTgDeepMode;
        if (eventName == "End") return hbaTgEndMode;
        return null;
    }

    JSONStorableStringChooser GetTgAtomChooser(string eventName)
    {
        if (eventName == "Start") return hbaTgStartAtom;
        if (eventName == "Inside") return hbaTgInsideAtom;
        if (eventName == "Deep") return hbaTgDeepAtom;
        if (eventName == "End") return hbaTgEndAtom;
        return null;
    }

    float GetTgTimedDuration(string mode)
    {
        if (mode == TgModeButtonPulse) return TgButtonPulseSeconds;
        if (mode == TgModeTimer1) return TgTimer1Seconds;
        if (mode == TgModeTimer5) return TgTimer5Seconds;
        return 0.0f;
    }

    void PulseTg(string suffix, float seconds)
    {
        SetTgSlot(suffix, true, "pulse-on");

        if (suffix == "Start")
        {
            if (tgStartOffRoutine != null) StopCoroutine(tgStartOffRoutine);
            tgStartOffRoutine = StartCoroutine(TgOffAfter(suffix, seconds));
        }
        else if (suffix == "Deep")
        {
            if (tgDeepOffRoutine != null) StopCoroutine(tgDeepOffRoutine);
            tgDeepOffRoutine = StartCoroutine(TgOffAfter(suffix, seconds));
        }
        else if (suffix == "End")
        {
            if (tgEndOffRoutine != null) StopCoroutine(tgEndOffRoutine);
            tgEndOffRoutine = StartCoroutine(TgOffAfter(suffix, seconds));
        }
        else
        {
            StartCoroutine(TgOffAfter(suffix, seconds));
        }
    }

    IEnumerator TgOffAfter(string suffix, float seconds)
    {
        yield return new WaitForSeconds(Mathf.Max(0.01f, seconds));
        SetTgSlot(suffix, false, "pulse-off");
    }

    void SetTgSlot(string suffix, bool value, string reason)
    {
        string atomUid = GetTgAtomUidForEvent(suffix);
        if (string.IsNullOrEmpty(atomUid))
        {
            DebugMessage("[HumanBodyAction] TG/HBA target empty / slot=" + suffix + " / value=" + value + " / reason=" + reason);
            return;
        }

        if (IsSafeExternalHbaAction(atomUid))
        {
            if (value)
            {
                TriggerConfiguredAction("tg-slot:" + suffix, atomUid);
                DebugMessage("[HumanBodyAction] HBA external fired / slot=" + suffix + " / action=" + atomUid + " / reason=" + reason);
            }
            else
            {
                DebugMessage("[HumanBodyAction] HBA external off ignored / slot=" + suffix + " / action=" + atomUid + " / reason=" + reason);
            }
            return;
        }

        Atom atom = SuperController.singleton.GetAtomByUid(atomUid);
        if (atom == null)
        {
            DebugMessage("[HumanBodyAction] TG atom missing / atom=" + atomUid + " / value=" + value + " / reason=" + reason);
            return;
        }

        bool unityOk = SetTgUnityToggle(atom, value);
        string boolInfo;
        bool boolOk = SetTgBoolParam(atom, value, out boolInfo);
        DebugMessage(
            "[HumanBodyAction] TG set" +
            " / slot=" + suffix +
            " / atom=" + atomUid +
            " / value=" + value +
            " / unityToggle=" + unityOk +
            " / boolParam=" + boolOk +
            boolInfo +
            " / reason=" + reason
        );
    }

    bool IsSafeExternalHbaAction(string actionName)
    {
        if (string.IsNullOrEmpty(actionName)) return false;
        if (actionName == HbaActionTwitchSlow || actionName == HbaActionTwitchWeak || actionName == HbaActionTwitchNormal || actionName == HbaActionTwitchStrong) return true;
        return !IsOff(GetHeadPresetFromHbaAction(actionName));
    }

    string GetTgAtomUidForEvent(string eventName)
    {
        JSONStorableStringChooser chooser = GetTgAtomChooser(eventName);
        if (chooser != null && !string.IsNullOrEmpty(chooser.val))
        {
            return chooser.val;
        }
        return GetTgPrefix() + eventName;
    }

    string GetTgPrefix()
    {
        if (hbaTgPrefix == null || string.IsNullOrEmpty(hbaTgPrefix.val))
        {
            return TgDefaultPrefix;
        }
        return hbaTgPrefix.val;
    }

    void RefreshHbaTgAtomList()
    {
        string startCurrent = hbaTgStartAtom != null ? hbaTgStartAtom.val : "";
        string insideCurrent = hbaTgInsideAtom != null ? hbaTgInsideAtom.val : "";
        string deepCurrent = hbaTgDeepAtom != null ? hbaTgDeepAtom.val : "";
        string endCurrent = hbaTgEndAtom != null ? hbaTgEndAtom.val : "";

        hbaTgAtomChoices.Clear();
        hbaTgAtomChoices.Add("");

        string prefix = GetTgPrefix();
        int atomCount = 0;
        int matchCount = 0;
        int hbaCount = 0;
        foreach (Atom atom in SuperController.singleton.GetAtoms())
        {
            if (atom == null || string.IsNullOrEmpty(atom.uid))
            {
                continue;
            }

            atomCount++;
            if (atom.uid.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) && !hbaTgAtomChoices.Contains(atom.uid))
            {
                hbaTgAtomChoices.Add(atom.uid);
                matchCount++;
            }
        }

        for (int i = 0; i < hbaEventActionChoices.Count; i++)
        {
            string actionName = hbaEventActionChoices[i];
            if (IsSafeExternalHbaAction(actionName) && !hbaTgAtomChoices.Contains(actionName))
            {
                hbaTgAtomChoices.Add(actionName);
                hbaCount++;
            }
        }

        hbaTgAtomChoices.Sort(StringComparer.OrdinalIgnoreCase);
        ApplyTgAtomChoices(hbaTgStartAtom, startCurrent, "Start");
        ApplyTgAtomChoices(hbaTgInsideAtom, insideCurrent, "Inside");
        ApplyTgAtomChoices(hbaTgDeepAtom, deepCurrent, "Deep");
        ApplyTgAtomChoices(hbaTgEndAtom, endCurrent, "End");

        DebugMessage("[HumanBodyAction] TG/HBA list refreshed / prefix=" + prefix + " / tgFound=" + matchCount + " / hbaFound=" + hbaCount + " / atoms=" + atomCount);
    }

    void ApplyTgAtomChoices(JSONStorableStringChooser chooser, string previous, string suffix)
    {
        if (chooser == null) return;
        chooser.choices = new List<string>(hbaTgAtomChoices);

        if (!string.IsNullOrEmpty(previous) && hbaTgAtomChoices.Contains(previous))
        {
            chooser.val = previous;
            return;
        }

        // Default routing is intentionally HBA Head actions. Real TG_ atoms can still be
        // selected manually after Refresh TG/HBA List. This keeps the first-run setup useful
        // even when no external TG_ atoms exist in the scene.
        string defaultAction = GetDefaultTgAtomForEvent(suffix);
        if (!string.IsNullOrEmpty(defaultAction) && hbaTgAtomChoices.Contains(defaultAction))
        {
            chooser.val = defaultAction;
            return;
        }

        string defaultUid = GetTgPrefix() + suffix;
        if (hbaTgAtomChoices.Contains(defaultUid))
        {
            chooser.val = defaultUid;
        }
        else
        {
            chooser.val = "";
        }
    }

    string GetDefaultTgAtomForEvent(string suffix)
    {
        if (suffix == "Start") return "HBA_Head_LookUp";
        if (suffix == "Inside") return "HBA_Head_LookAround";
        if (suffix == "Deep") return "HBA_Head_RapidOrgasm";
        if (suffix == "End") return "HBA_Head_LookUp";
        return "";
    }

    bool SetTgUnityToggle(Atom atom, bool value)
    {
        if (atom == null) return false;
        UnityEngine.UI.Toggle toggle = atom.GetComponentInChildren<UnityEngine.UI.Toggle>(true);
        if (toggle == null) return false;
        toggle.isOn = value;
        return true;
    }

    bool SetTgBoolParam(Atom atom, bool value, out string info)
    {
        info = "";
        if (atom == null) return false;

        if (TrySetTgBoolParam(atom, "Trigger", "value", value, out info)) return true;

        for (int i = 0; i < tgFallbackStorableIds.Length; i++)
        {
            for (int j = 0; j < tgFallbackBoolNames.Length; j++)
            {
                if (TrySetTgBoolParam(atom, tgFallbackStorableIds[i], tgFallbackBoolNames[j], value, out info)) return true;
            }
        }

        foreach (string sid in atom.GetStorableIDs())
        {
            for (int j = 0; j < tgFallbackBoolNames.Length; j++)
            {
                if (TrySetTgBoolParam(atom, sid, tgFallbackBoolNames[j], value, out info)) return true;
            }
        }

        info = " / bool not found";
        return false;
    }

    bool TrySetTgBoolParam(Atom atom, string sid, string param, bool value, out string info)
    {
        info = "";
        if (atom == null || string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(param)) return false;

        JSONStorable storable = atom.GetStorableByID(sid);
        if (storable == null) return false;

        JSONStorableBool boolParam = storable.GetBoolJSONParam(param);
        if (boolParam == null) return false;

        boolParam.val = value;
        info = " / storable=" + sid + " / param=" + param;
        return true;
    }

    void RequestPresetAction(string source, string preset)
    {
        // TW buttons are body/eyes/mouth only. Head is a separate HBA_Head_* button/action.
        QueueAction(new ActionRequest
        {
            source = source,
            preset = preset,
            applyPreset = true,
            runBody = true,
            runEyes = true,
            runMouth = true,
            runHead = false,
            headPreset = "Off"
        });
    }

    void RequestBodyOnly(string source)
    {
        QueueAction(new ActionRequest
        {
            source = source,
            preset = "CurrentBody",
            applyPreset = false,
            runBody = true,
            runEyes = false,
            runMouth = false,
            runHead = false,
            headPreset = "Off"
        });
    }

    void RequestEyesOnly(string source)
    {
        ApplyNormalFacePreset();
        QueueAction(new ActionRequest
        {
            source = source,
            preset = "EyesOnly",
            applyPreset = false,
            runBody = false,
            runEyes = true,
            runMouth = false,
            runHead = false,
            headPreset = "Off"
        });
    }

    void RequestMouthOnly(string source)
    {
        ApplyNormalFacePreset();
        QueueAction(new ActionRequest
        {
            source = source,
            preset = "MouthOnly",
            applyPreset = false,
            runBody = false,
            runEyes = false,
            runMouth = true,
            runHead = false,
            headPreset = "Off"
        });
    }

    void RequestFaceOnly(string source)
    {
        ApplyNormalFacePreset();
        QueueAction(new ActionRequest
        {
            source = source,
            preset = "FaceOnly",
            applyPreset = false,
            runBody = false,
            runEyes = true,
            runMouth = true,
            runHead = false,
            headPreset = "Off"
        });
    }

    void RequestHeadOnly(string source, string headPreset)
    {
        QueueAction(new ActionRequest
        {
            source = source,
            preset = "HeadOnly",
            applyPreset = false,
            runBody = false,
            runEyes = false,
            runMouth = false,
            runHead = true,
            headPreset = headPreset
        });
    }

    void QueueAction(ActionRequest request)
    {
        if (request == null) return;

        if (!IsHbaEnabled())
        {
            hbaLastBlock = "Disabled: action skipped";
            DebugMessage("[HumanBodyAction] Action skipped because HBA Enable is OFF / source=" + request.source + " / preset=" + request.preset + " / head=" + request.headPreset);
            UpdateHbaStatus(true);
            return;
        }

        if (actionRoutine != null)
        {
            if (queueLastAction != null && queueLastAction.val)
            {
                pendingRequest = request;
                hbaLastBlock = "Busy: queued latest";
                DebugMessage("[HumanBodyAction] Action queued latest-only / source=" + request.source + " / preset=" + request.preset + " / head=" + request.headPreset);
            }
            else
            {
                hbaLastBlock = "Busy: ignored";
                DebugMessage("[HumanBodyAction] Action ignored while busy / source=" + request.source + " / preset=" + request.preset + " / head=" + request.headPreset);
            }
            UpdateHbaStatus(true);
            return;
        }

        hbaLastBlock = "";
        actionRoutine = StartCoroutine(ActionQueueRoutine(request));
    }

    IEnumerator ActionQueueRoutine(ActionRequest firstRequest)
    {
        ActionRequest current = firstRequest;
        while (current != null)
        {
            pendingRequest = null;
            yield return StartCoroutine(ExecuteAction(current));
            current = pendingRequest;
        }

        actionRoutine = null;
    }

    IEnumerator ExecuteAction(ActionRequest request)
    {
        if (request == null) yield break;

        hbaLastAction = request.source;
        hbaLastBlock = "";
        UpdateHbaStatus(true);

        DebugMessage("[HumanBodyAction] ACTION START / source=" + request.source + " / preset=" + request.preset + " / head=" + request.headPreset);

        if (request.applyPreset)
        {
            ApplyPresetByName(request.preset);
        }

        if (request.runBody && IsTwitchBodyEnabled())
        {
            StartBodyTwitchOnce();
        }

        if (request.runEyes)
        {
            ActionEyesOnce();
        }

        if (request.runMouth)
        {
            ActionMouthOnce();
        }

        while (twitchRoutine != null || eyesRoutine != null || mouthRoutine != null)
        {
            yield return null;
        }

        if (request.runHead && IsHeadActionEnabled() && !IsOff(request.headPreset))
        {
            yield return StartCoroutine(PlayHeadPoseByName(request.headPreset));
        }

        DebugMessage("[HumanBodyAction] ACTION DONE / source=" + request.source + " / preset=" + request.preset);
    }

    bool IsTwitchBodyEnabled()
    {
        return twitchBody == null || twitchBody.val;
    }

    bool IsHeadActionEnabled()
    {
        // Head buttons/actions are explicit in this build, so Head playback is always allowed.
        return true;
    }

    bool IsOff(string value)
    {
        return string.IsNullOrEmpty(value) || value == "Off";
    }

    string GetChooserValue(JSONStorableStringChooser chooser, string fallback)
    {
        if (chooser == null || string.IsNullOrEmpty(chooser.val)) return fallback;
        return chooser.val;
    }

    void ApplyPresetByName(string preset)
    {
        if (preset == "Slow") ApplySlowPreset();
        else if (preset == "Weak") ApplyWeakPreset();
        else if (preset == "Strong") ApplyStrongPreset();
        else ApplyNormalPreset();
    }

    void ApplySlowPreset()
    {
        ApplySlowFacePreset();
        ApplyPresetCore(1.15f, 0.030f, 1.05f, 0.10f, 0.08f);
    }

    void ApplyWeakPreset()
    {
        ApplyWeakFacePreset();
        ApplyPresetCore(0.55f, 0.045f, 1.70f, 0.55f, 0.20f);
    }

    void ApplyNormalPreset()
    {
        ApplyNormalFacePreset();
        ApplyPresetCore(0.68f, 0.070f, 1.85f, 0.65f, 0.28f);
    }

    void ApplyStrongPreset()
    {
        ApplyStrongFacePreset();
        ApplyPresetCore(0.70f, 0.110f, 2.00f, 0.72f, 0.32f);
    }

    void ApplySlowFacePreset()
    {
        SetFacePreset(SlowEyesDuration, SlowEyesTarget, SlowMouthDuration, SlowMouthOpenMin, SlowMouthOpenMax);
    }

    void ApplyWeakFacePreset()
    {
        SetFacePreset(WeakEyesDuration, WeakEyesTarget, WeakMouthDuration, WeakMouthOpenMin, WeakMouthOpenMax);
    }

    void ApplyNormalFacePreset()
    {
        SetFacePreset(NormalEyesDuration, NormalEyesTarget, NormalMouthDuration, NormalMouthOpenMin, NormalMouthOpenMax);
    }

    void ApplyStrongFacePreset()
    {
        SetFacePreset(StrongEyesDuration, StrongEyesTarget, StrongMouthDuration, StrongMouthOpenMin, StrongMouthOpenMax);
    }

    void SetFacePreset(float eyesDuration, float eyesTarget, float mouthDuration, float mouthOpenMin, float mouthOpenMax)
    {
        currentEyesDuration = Mathf.Max(0.05f, eyesDuration);
        currentEyesTarget = Mathf.Clamp(eyesTarget, 0.0f, 1.0f);
        currentMouthDuration = Mathf.Max(0.05f, mouthDuration);
        currentMouthOpenMin = Mathf.Clamp(mouthOpenMin, MouthOpenClampMin, MouthOpenClampMax);
        currentMouthOpenMax = Mathf.Clamp(mouthOpenMax, MouthOpenClampMin, MouthOpenClampMax);
    }

    void ApplyPresetCore(float durationValue, float strengthValue, float hitCountValue, float sharpnessValue, float randomnessValue)
    {
        SetBool(twitchChest, true);
        SetBool(twitchHip, true);
        SetBool(twitchHeadMicro, false);
        SetBool(twitchHands, true);
        SetBool(twitchFeet, true);
        SetBool(useBodyAxes, true);

        SetFloat(duration, durationValue);
        SetFloat(strength, strengthValue);
        SetFloat(hitCount, hitCountValue);
        SetFloat(sharpness, sharpnessValue);
        SetFloat(randomness, randomnessValue);
    }

    void StartBodyTwitchOnce()
    {
        if (twitchRoutine != null)
        {
            StopCoroutine(twitchRoutine);
            twitchRoutine = null;
        }

        ResetOffsets("restart body");
        RefreshControllersNoReset();
        RandomizeDirections();
        twitchRoutine = StartCoroutine(TwitchOnceRoutine());
    }

    IEnumerator TwitchOnceRoutine()
    {
        float start = Time.time;
        float dur = Mathf.Max(0.05f, GetFloat(duration, DefaultDuration));

        DebugMessage("[HumanBodyAction] Body twitch start / duration=" + dur.ToString("0.00") +
            " / strength=" + GetFloat(strength, DefaultStrength).ToString("0.000") +
            " / motionScale=" + GetFloat(twitchMotionScale, TwitchMotionScaleDefault).ToString("0.00") +
            " / up=" + GetFloat(twitchUpScale, TwitchUpScaleDefault).ToString("0.00") +
            " / side=" + GetFloat(twitchSideScale, TwitchSideScaleDefault).ToString("0.00") +
            " / forward=" + GetFloat(twitchForwardScale, TwitchForwardScaleDefault).ToString("0.00"));

        while (Time.time - start < dur)
        {
            float t = Mathf.Clamp01((Time.time - start) / dur);
            ApplySingleTwitchFrame(t);
            yield return null;
        }

        ResetOffsets("body done");
        twitchRoutine = null;
        DebugMessage("[HumanBodyAction] Body twitch done");
    }

    void ApplySingleTwitchFrame(float t)
    {
        Vector3 sideAxis;
        Vector3 upAxis;
        Vector3 forwardAxis;
        GetAxes(out sideAxis, out upAxis, out forwardAxis);

        float amp = GetFloat(strength, DefaultStrength) * Mathf.Clamp(GetFloat(twitchMotionScale, TwitchMotionScaleDefault), TwitchMotionScaleMin, TwitchMotionScaleMax);
        float sideScale = Mathf.Clamp(GetFloat(twitchSideScale, TwitchSideScaleDefault), TwitchAxisScaleMin, TwitchAxisScaleMax);
        float upScale = Mathf.Clamp(GetFloat(twitchUpScale, TwitchUpScaleDefault), TwitchAxisScaleMin, TwitchAxisScaleMax);
        float forwardScale = Mathf.Clamp(GetFloat(twitchForwardScale, TwitchForwardScaleDefault), TwitchAxisScaleMin, TwitchAxisScaleMax);
        float hits = Mathf.Max(1.0f, GetFloat(hitCount, DefaultHitCount));
        float sharp = Mathf.Clamp01(GetFloat(sharpness, DefaultSharpness));
        float rnd = Mathf.Clamp01(GetFloat(randomness, DefaultRandomness));

        float basePulse = Mathf.Sin(t * Mathf.PI * hits);
        float signPulse = Mathf.Sign(basePulse == 0.0f ? 1.0f : basePulse);
        float absPulse = Mathf.Abs(basePulse);
        float sharpenPower = Mathf.Lerp(1.60f, 0.34f, sharp);
        float pulse = Mathf.Pow(absPulse, sharpenPower);
        float decay = Mathf.Pow(1.0f - t, Mathf.Lerp(1.20f, 0.55f, sharp));
        float envelope = pulse * decay;

        if (t > 0.18f)
        {
            signPulse *= -1.0f;
        }

        for (int i = 0; i < parts.Count; i++)
        {
            TwitchPart p = parts[i];
            if (p == null || p.controller == null) continue;

            Vector3 basePosition = p.controller.transform.position - p.lastOffset;
            Vector3 targetOffset = Vector3.zero;

            if (IsPartEnabled(p))
            {
                float partScale = GetPartScale(p);
                float partPhase = Mathf.Sin((t * Mathf.PI * hits) + p.phase);
                float mixed = Mathf.Lerp(signPulse, partPhase, rnd * 0.55f);
                float localPulse = envelope * Mathf.Clamp(mixed, -1.0f, 1.0f);

                Vector3 dir =
                    sideAxis * (p.direction.x * sideScale) +
                    upAxis * (p.direction.y * upScale) +
                    forwardAxis * (p.direction.z * forwardScale);

                if (dir.sqrMagnitude > 0.000001f) dir.Normalize();
                else dir = upAxis;

                targetOffset = dir * amp * partScale * localPulse;
            }

            p.controller.transform.position = basePosition + targetOffset;
            p.lastOffset = targetOffset;
        }
    }

    void RandomizeDirections()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            TwitchPart p = parts[i];
            if (p == null) continue;

            float side = UnityEngine.Random.Range(-0.55f, 0.55f);
            float up = UnityEngine.Random.Range(0.75f, 1.15f);
            float forward = UnityEngine.Random.Range(-0.60f, 0.60f);

            if (p.label == "Hip")
            {
                up *= -0.45f;
                forward *= 0.60f;
            }
            else if (p.label == "Head")
            {
                up *= 0.35f;
                side *= 0.50f;
            }
            else if (p.label == "L Hand" || p.label == "R Hand" || p.label == "L Foot" || p.label == "R Foot")
            {
                up *= 0.35f;
                forward *= 0.50f;
            }

            p.direction = new Vector3(side, up, forward);
            p.phase = UnityEngine.Random.Range(-0.85f, 0.85f);
        }
    }

    void RefreshControllers()
    {
        ResetOffsets("refresh");
        RefreshControllersNoReset();
    }

    void RefreshControllersNoReset()
    {
        parts.Clear();
        AddPart("Chest", "chestControl", 1.00f);
        AddPart("Hip", "hipControl", 0.70f);
        AddPart("Head", "headControl", 0.40f);
        AddPart("L Hand", "lHandControl", 0.25f);
        AddPart("R Hand", "rHandControl", 0.25f);
        AddPart("L Foot", "lFootControl", 0.20f);
        AddPart("R Foot", "rFootControl", 0.20f);
        headControl = FindControllerExact(containingAtom, "headControl");
    }

    void AddPart(string label, string controllerName, float weight)
    {
        TwitchPart part = new TwitchPart();
        part.label = label;
        part.controllerName = controllerName;
        part.controller = FindControllerExact(containingAtom, controllerName);
        part.lastOffset = Vector3.zero;
        part.direction = Vector3.up;
        part.phase = 0.0f;
        part.weight = weight;
        parts.Add(part);
    }

    FreeControllerV3 FindControllerExact(Atom atom, string controllerName)
    {
        if (atom == null || atom.freeControllers == null || string.IsNullOrEmpty(controllerName)) return null;

        for (int i = 0; i < atom.freeControllers.Length; i++)
        {
            FreeControllerV3 fc = atom.freeControllers[i];
            if (fc != null && fc.name == controllerName) return fc;
        }

        string lowered = controllerName.ToLower();
        for (int i = 0; i < atom.freeControllers.Length; i++)
        {
            FreeControllerV3 fc = atom.freeControllers[i];
            if (fc != null && fc.name != null && fc.name.ToLower().Contains(lowered)) return fc;
        }

        return null;
    }

    bool IsPartEnabled(TwitchPart p)
    {
        if (p.label == "Chest") return twitchChest == null || twitchChest.val;
        if (p.label == "Hip") return twitchHip == null || twitchHip.val;
        if (p.label == "Head")
        {
            if (IsHeadActionEnabled()) return false;
            return twitchHeadMicro == null || twitchHeadMicro.val;
        }
        if (p.label == "L Hand" || p.label == "R Hand") return twitchHands != null && twitchHands.val;
        if (p.label == "L Foot" || p.label == "R Foot") return twitchFeet != null && twitchFeet.val;
        return true;
    }

    float GetPartScale(TwitchPart p)
    {
        if (p.label == "Chest") return p.weight * Mathf.Clamp(GetFloat(twitchChestScale, TwitchChestScaleDefault), TwitchPartScaleMin, TwitchPartScaleMax);
        if (p.label == "Hip") return p.weight * Mathf.Clamp(GetFloat(twitchHipScale, TwitchHipScaleDefault), TwitchPartScaleMin, TwitchPartScaleMax);
        if (p.label == "Head") return p.weight * HeadMicroScale;
        return p.weight * Mathf.Clamp(GetFloat(twitchLimbScale, TwitchLimbScaleDefault), TwitchPartScaleMin, TwitchPartScaleMax);
    }

    void GetAxes(out Vector3 sideAxis, out Vector3 upAxis, out Vector3 forwardAxis)
    {
        Transform basis = containingAtom != null ? containingAtom.transform : null;
        if (useBodyAxes != null && useBodyAxes.val && basis != null)
        {
            sideAxis = basis.right;
            upAxis = Vector3.up;
            forwardAxis = basis.forward;
            forwardAxis.y = 0.0f;
            if (forwardAxis.sqrMagnitude < 0.0001f) forwardAxis = basis.forward;
            forwardAxis.Normalize();
            return;
        }

        sideAxis = Vector3.right;
        upAxis = Vector3.up;
        forwardAxis = Vector3.forward;
    }

    void ActionEyesOnce()
    {
        if (twitchEyes != null && !twitchEyes.val) return;

        if (eyesRoutine != null)
        {
            StopCoroutine(eyesRoutine);
            eyesRoutine = null;
            RestoreEyesMorphs("restart");
        }

        if (!EnsureEyeMorphs())
        {
            DebugMessage("[HumanBodyAction] Eyes morph not found. Tried Eyes Closed Left / Eyes Closed Right / Eyes Closed.");
            return;
        }

        eyesRoutine = StartCoroutine(EyesOnceRoutine());
    }

    IEnumerator EyesOnceRoutine()
    {
        SaveEyesMorphs();

        float baseL = savedEyesClosedLeft;
        float baseR = savedEyesClosedRight;
        float baseSingle = savedEyesClosedSingle;
        float currentAvg = GetEyesAverageValue();
        bool currentlyClosed = currentAvg >= EyesClosedThreshold;
        float target = currentlyClosed ? 0.0f : currentEyesTarget;

        float start = Time.time;
        float dur = Mathf.Max(0.05f, currentEyesDuration);
        float half = Mathf.Max(0.01f, dur * 0.5f);

        DebugMessage("[HumanBodyAction] Eyes start / current=" + currentAvg.ToString("0.000") + " / target=" + target.ToString("0.000"));

        while (Time.time - start < dur)
        {
            float elapsed = Time.time - start;
            float v;
            if (elapsed <= half)
            {
                float t = Smooth01(Mathf.Clamp01(elapsed / half));
                v = Mathf.Lerp(currentAvg, target, t);
            }
            else
            {
                float t = Smooth01(Mathf.Clamp01((elapsed - half) / half));
                v = Mathf.Lerp(target, currentAvg, t);
            }

            ApplyEyesValue(v, baseL, baseR, baseSingle, currentAvg);
            yield return null;
        }

        RestoreEyesMorphs("eyes done");
        eyesRoutine = null;
        DebugMessage("[HumanBodyAction] Eyes done");
    }

    float Smooth01(float t)
    {
        return t * t * (3.0f - 2.0f * t);
    }

    void ApplyEyesValue(float sharedValue, float baseL, float baseR, float baseSingle, float baseAvg)
    {
        float delta = sharedValue - baseAvg;

        if (eyesClosedLeftMorph != null) eyesClosedLeftMorph.morphValue = Mathf.Clamp(baseL + delta, -1.0f, 1.0f);
        if (eyesClosedRightMorph != null) eyesClosedRightMorph.morphValue = Mathf.Clamp(baseR + delta, -1.0f, 1.0f);
        if (eyesClosedMorph != null) eyesClosedMorph.morphValue = Mathf.Clamp(baseSingle + delta, -1.0f, 1.0f);
    }

    void SaveEyesMorphs()
    {
        savedEyesClosedLeft = eyesClosedLeftMorph != null ? eyesClosedLeftMorph.morphValue : 0.0f;
        savedEyesClosedRight = eyesClosedRightMorph != null ? eyesClosedRightMorph.morphValue : 0.0f;
        savedEyesClosedSingle = eyesClosedMorph != null ? eyesClosedMorph.morphValue : 0.0f;
        eyesSaved = true;
    }

    void RestoreEyesMorphs(string reason)
    {
        if (!eyesSaved) return;

        if (eyesClosedLeftMorph != null) eyesClosedLeftMorph.morphValue = savedEyesClosedLeft;
        if (eyesClosedRightMorph != null) eyesClosedRightMorph.morphValue = savedEyesClosedRight;
        if (eyesClosedMorph != null) eyesClosedMorph.morphValue = savedEyesClosedSingle;
        eyesSaved = false;
        DebugMessage("[HumanBodyAction] Restore eyes / reason=" + reason);
    }

    float GetEyesAverageValue()
    {
        float sum = 0.0f;
        int count = 0;
        if (eyesClosedLeftMorph != null) { sum += eyesClosedLeftMorph.morphValue; count++; }
        if (eyesClosedRightMorph != null) { sum += eyesClosedRightMorph.morphValue; count++; }
        if (eyesClosedMorph != null) { sum += eyesClosedMorph.morphValue; count++; }
        return count > 0 ? sum / (float)count : 0.0f;
    }

    bool EnsureEyeMorphs()
    {
        if (eyesClosedLeftMorph != null || eyesClosedRightMorph != null || eyesClosedMorph != null) return true;
        RefreshFaceMorphs();
        return eyesClosedLeftMorph != null || eyesClosedRightMorph != null || eyesClosedMorph != null;
    }

    void ActionMouthOnce()
    {
        if (twitchMouth != null && !twitchMouth.val) return;

        if (mouthRoutine != null)
        {
            StopCoroutine(mouthRoutine);
            mouthRoutine = null;
            RestoreMouthMorphs("restart");
        }

        if (!EnsureMouthMorph())
        {
            DebugMessage("[HumanBodyAction] Mouth morph not found. Tried Mouth Open.");
            return;
        }

        mouthRoutine = StartCoroutine(MouthOnceRoutine());
    }

    IEnumerator MouthOnceRoutine()
    {
        SaveMouthMorphs();

        float baseValue = savedMouthOpen;
        bool currentlyOpen = baseValue >= MouthOpenThreshold;
        float target = currentlyOpen ? currentMouthOpenMin : currentMouthOpenMax;

        float start = Time.time;
        float dur = Mathf.Max(0.05f, currentMouthDuration);
        float half = Mathf.Max(0.01f, dur * 0.5f);

        DebugMessage("[HumanBodyAction] Mouth start / current=" + baseValue.ToString("0.000") + " / target=" + target.ToString("0.000") + " / duration=" + dur.ToString("0.00"));

        while (Time.time - start < dur)
        {
            float elapsed = Time.time - start;
            float v;
            if (elapsed <= half)
            {
                float t = Smooth01(Mathf.Clamp01(elapsed / half));
                v = Mathf.Lerp(baseValue, target, t);
            }
            else
            {
                float t = Smooth01(Mathf.Clamp01((elapsed - half) / half));
                v = Mathf.Lerp(target, baseValue, t);
            }

            ApplyMouthValue(v);
            yield return null;
        }

        RestoreMouthMorphs("mouth done");
        mouthRoutine = null;
        DebugMessage("[HumanBodyAction] Mouth done");
    }

    void ApplyMouthValue(float value)
    {
        if (mouthOpenMorph != null) mouthOpenMorph.morphValue = Mathf.Clamp(value, MouthOpenClampMin, MouthOpenClampMax);
    }

    void SaveMouthMorphs()
    {
        savedMouthOpen = mouthOpenMorph != null ? mouthOpenMorph.morphValue : 0.0f;
        mouthSaved = true;
    }

    void RestoreMouthMorphs(string reason)
    {
        if (!mouthSaved) return;
        if (mouthOpenMorph != null) mouthOpenMorph.morphValue = savedMouthOpen;
        mouthSaved = false;
        DebugMessage("[HumanBodyAction] Restore mouth / reason=" + reason);
    }

    bool EnsureMouthMorph()
    {
        if (mouthOpenMorph != null) return true;
        RefreshFaceMorphs();
        return mouthOpenMorph != null;
    }

    void RefreshFaceMorphs()
    {
        eyesClosedLeftMorph = null;
        eyesClosedRightMorph = null;
        eyesClosedMorph = null;
        mouthOpenMorph = null;

        if (containingAtom == null) return;

        JSONStorable geometry = containingAtom.GetStorableByID("geometry");
        DAZCharacterSelector dcs = geometry as DAZCharacterSelector;
        if (dcs == null || dcs.morphsControlUI == null)
        {
            DebugMessage("[HumanBodyAction] geometry/morph UI not found");
            return;
        }

        GenerateDAZMorphsControlUI morphUI = dcs.morphsControlUI;
        eyesClosedLeftMorph = morphUI.GetMorphByDisplayName("Eyes Closed Left");
        eyesClosedRightMorph = morphUI.GetMorphByDisplayName("Eyes Closed Right");
        eyesClosedMorph = morphUI.GetMorphByDisplayName("Eyes Closed");
        mouthOpenMorph = morphUI.GetMorphByDisplayName("Mouth Open");

        DebugMessage(
            "[HumanBodyAction] Face morph refresh / eyesL=" + (eyesClosedLeftMorph != null) +
            " / eyesR=" + (eyesClosedRightMorph != null) +
            " / eyesSingle=" + (eyesClosedMorph != null) +
            " / mouthOpen=" + (mouthOpenMorph != null)
        );
    }

    void BuildHeadPoseCache()
    {
        headPoseCache.Clear();
        CacheHeadPose("Head Shake", ShakeData);
        CacheHeadPose("Head Tilt", TiltData);
        CacheHeadPose("Head Big Nod", BigNodData);
        CacheHeadPose("Head Nod", NodData);
        CacheHeadPose("Head Look Up", LookUpData);
        CacheHeadPose("Head Intense Shake", IntenseShakeData);
        CacheHeadPose("Head Ecstasy Arch", EcstasyArchData);
        CacheHeadPose("Head Rapid Orgasm", RapidOrgasmData);
        CacheHeadPose("Head Shy", ShyLookData);
        CacheHeadPose("Head Look Around", LookAroundData);
        CacheHeadPose("Head Neck Roll", NeckRollData);
        CacheHeadPose("Head Quick Nod", QuickNodData);
        CacheHeadPose("Head Up Eyes", UpEyesData);
    }

    void CacheHeadPose(string preset, string[] lines)
    {
        if (string.IsNullOrEmpty(preset) || lines == null || lines.Length == 0) return;

        List<Keyframe> parsed = new List<Keyframe>();
        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;
            Keyframe kf = ParsePoseLine(lines[i]);
            if (kf != null) parsed.Add(kf);
        }

        if (parsed.Count > 0)
        {
            headPoseCache[preset] = parsed.ToArray();
        }
    }

    Keyframe[] GetHeadPoseFrames(string preset)
    {
        if (string.IsNullOrEmpty(preset)) return null;

        Keyframe[] frames;
        if (headPoseCache.TryGetValue(preset, out frames))
        {
            return frames;
        }

        string[] lines = GetHeadPoseData(preset);
        if (lines == null || lines.Length == 0) return null;
        CacheHeadPose(preset, lines);
        if (headPoseCache.TryGetValue(preset, out frames)) return frames;
        return null;
    }

    IEnumerator PlayHeadPoseByName(string preset)
    {
        Keyframe[] frames = GetHeadPoseFrames(preset);
        if (frames == null || frames.Length == 0)
        {
            DebugMessage("[HumanBodyAction] Head skipped / unknown=" + preset);
            yield break;
        }

        if (headControl == null)
        {
            headControl = FindControllerExact(containingAtom, "headControl");
        }

        if (headControl == null)
        {
            DebugMessage("[HumanBodyAction] Head skipped / headControl not found");
            yield break;
        }

        DebugMessage("[HumanBodyAction] Head start / preset=" + preset);
        yield return StartCoroutine(PlayHeadPoseSequence(frames));
        DebugMessage("[HumanBodyAction] Head done / preset=" + preset);
    }

    string[] GetHeadPoseData(string preset)
    {
        if (preset == "Head Shake") return ShakeData;
        if (preset == "Head Tilt") return TiltData;
        if (preset == "Head Big Nod") return BigNodData;
        if (preset == "Head Nod") return NodData;
        if (preset == "Head Look Up") return LookUpData;
        if (preset == "Head Intense Shake") return IntenseShakeData;
        if (preset == "Head Ecstasy Arch") return EcstasyArchData;
        if (preset == "Head Rapid Orgasm") return RapidOrgasmData;
        if (preset == "Head Shy") return ShyLookData;
        if (preset == "Head Look Around") return LookAroundData;
        if (preset == "Head Neck Roll") return NeckRollData;
        if (preset == "Head Quick Nod") return QuickNodData;
        if (preset == "Head Up Eyes") return UpEyesData;
        return null;
    }

    IEnumerator PlayHeadPoseSequence(Keyframe[] frames)
    {
        if (headControl == null || frames == null || frames.Length == 0) yield break;

        activeHeadSnapshot = CaptureHeadControlSnapshot();
        HeadControlSnapshot restoreSnapshot = activeHeadSnapshot;
        Vector3 currentPos = GetHeadControlPosition();
        Quaternion currentRot = GetHeadControlRotation();

        ApplyHeadControlOn();

        Vector3 startPos = restoreSnapshot.position;
        Quaternion startRot = restoreSnapshot.rotation;
        // v026: use the captured start rotation as the neutral base.
        // The old LookRotation(..., Vector3.up) rebuilt the base rotation and could create a snap.
        Quaternion centerRot = startRot;

        yield return StartCoroutine(MoveHeadSmooth(currentPos, currentRot, startPos, centerRot, ScaleHeadDuration(HeadEntryDuration)));

        Quaternion firstRot = frames[0].rotation;
        Quaternion prevRot = centerRot;
        int startIndex = frames.Length > 1 ? HeadPlaybackStartIndex : 0;

        for (int i = startIndex; i < frames.Length; i++)
        {
            Keyframe kf = frames[i];
            if (kf == null) continue;

            Quaternion relativeRot = Quaternion.Inverse(firstRot) * kf.rotation;
            Quaternion targetRot = centerRot * relativeRot;
            float angle = Quaternion.Angle(prevRot, targetRot);
            if (angle < HeadSkipAngleDegrees)
            {
                prevRot = targetRot;
                continue;
            }

            float dur = ScaleHeadDuration(Mathf.Clamp(kf.duration * HeadDurationScale, HeadSegmentMinDuration, HeadSegmentMaxDuration));
            LogHeadPoseIfDebug("PLAY", dur, startPos, targetRot);
            yield return StartCoroutine(MoveHeadSmooth(startPos, prevRot, startPos, targetRot, dur));
            prevRot = targetRot;
        }

        yield return StartCoroutine(MoveHeadSmooth(startPos, prevRot, startPos, startRot, ScaleHeadDuration(ReturnToStartDuration)));
        RestoreHeadControlState(restoreSnapshot);
        activeHeadSnapshot = null;
    }

    HeadControlSnapshot CaptureHeadControlSnapshot()
    {
        HeadControlSnapshot snapshot = new HeadControlSnapshot();
        snapshot.position = GetHeadControlPosition();
        snapshot.rotation = GetHeadControlRotation();
        snapshot.positionState = headControl.currentPositionState;
        snapshot.rotationState = headControl.currentRotationState;
        return snapshot;
    }

    void ApplyHeadControlOn()
    {
        if (headControl == null) return;

        // v026: HeadAction is rotation-only.
        // Do not force PositionState ON here; that can pin headControl position while
        // TargetGrabber/Target Swoon Drop is trying to release body IK.
        headControl.currentRotationState = FreeControllerV3.RotationState.On;
    }

    void RestoreHeadControlState(HeadControlSnapshot snapshot)
    {
        if (headControl == null || snapshot == null) return;

        // v026: HeadAction only owns rotation.
        // Leave position and PositionState exactly as other plugins/physics currently have them.
        // This prevents a completed HeadAction from re-pinning the head position after Swoon Drop.
        SetHeadControlRotation(snapshot.rotation);
        headControl.currentRotationState = snapshot.rotationState;
    }

    Keyframe ParsePoseLine(string line)
    {
        try
        {
            string content = line.Replace("💽POSE|TM,", "").Replace("|#", "").Trim();
            string[] partsLine = content.Split('|');
            if (partsLine.Length < 2) return null;

            string[] timeParts = partsLine[0].Split(',');
            float dur = float.Parse(timeParts[0], CultureInfo.InvariantCulture);

            string[] ctrlParts = partsLine[1].Split(',');
            if (ctrlParts.Length < 8) return null;

            Keyframe keyframe = new Keyframe();
            keyframe.rotation = new Quaternion(
                float.Parse(ctrlParts[4], CultureInfo.InvariantCulture),
                float.Parse(ctrlParts[5], CultureInfo.InvariantCulture),
                float.Parse(ctrlParts[6], CultureInfo.InvariantCulture),
                float.Parse(ctrlParts[7], CultureInfo.InvariantCulture)
            );
            keyframe.duration = dur;
            return keyframe;
        }
        catch
        {
            return null;
        }
    }

    IEnumerator MoveHeadSmooth(Vector3 startPos, Quaternion startRot, Vector3 targetPos, Quaternion targetRot, float dur)
    {
        float t = 0.0f;
        float invDur = 1.0f / Mathf.Max(0.01f, dur);
        bool movePosition = (targetPos - startPos).sqrMagnitude > 0.0000001f;
        Transform tr = headControl != null ? headControl.transform : null;
        if (tr == null) yield break;

        while (t < 1.0f)
        {
            t += Time.deltaTime * invDur;
            float linear = Mathf.Clamp01(t);
            float smooth = Mathf.SmoothStep(0.0f, 1.0f, linear);
            float ease = Mathf.Lerp(linear, smooth, HeadSmoothEaseMix);
            if (movePosition)
            {
                SetHeadControlPosition(Vector3.Lerp(startPos, targetPos, ease));
            }
            SetHeadControlRotation(Quaternion.Slerp(startRot, targetRot, ease));
            yield return null;
        }

        if (movePosition)
        {
            SetHeadControlPosition(targetPos);
        }
        SetHeadControlRotation(targetRot);
    }

    float GetHeadTimeScale()
    {
        return Mathf.Clamp(headTimeScale != null ? headTimeScale.val : HeadTimeScaleDefault, HeadTimeScaleMin, HeadTimeScaleMax);
    }

    float ScaleHeadDuration(float seconds)
    {
        return Mathf.Max(0.01f, seconds * GetHeadTimeScale());
    }

    Vector3 GetHeadControlPosition()
    {
        if (headControl == null) return Vector3.zero;
        if (headControl.control != null) return headControl.control.position;
        return headControl.transform.position;
    }

    Quaternion GetHeadControlRotation()
    {
        if (headControl == null) return Quaternion.identity;
        if (headControl.control != null) return headControl.control.rotation;
        return headControl.transform.rotation;
    }

    void SetHeadControlPosition(Vector3 position)
    {
        if (headControl == null) return;
        if (headControl.control != null) headControl.control.position = position;
        if (headControl.transform != null) headControl.transform.position = position;
    }

    void SetHeadControlRotation(Quaternion rotation)
    {
        if (headControl == null) return;
        if (headControl.control != null) headControl.control.rotation = rotation;
        if (headControl.transform != null) headControl.transform.rotation = rotation;
    }

    void LogHeadPoseIfDebug(string label, float dur, Vector3 position, Quaternion rotation)
    {
        if (!IsDebug()) return;
        SuperController.LogMessage("[HumanBodyAction] HEAD " + label + " " + BuildPoseLine(dur, position, rotation));
    }

    string BuildPoseLine(float dur, Vector3 position, Quaternion rotation)
    {
        return "💽POSE|TM," + F(dur) + ",0|headControl,"
            + F(position.x) + "," + F(position.y) + "," + F(position.z) + ","
            + F(rotation.x) + "," + F(rotation.y) + "," + F(rotation.z) + "," + F(rotation.w) + "|#";
    }

    string F(float value)
    {
        return value.ToString("0.000", CultureInfo.InvariantCulture);
    }

    void StopAllAndReset(string reason)
    {
        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
            actionRoutine = null;
        }
        pendingRequest = null;
        StopStartDecisionRoutine();
        StopInsideMonitorRoutine("reset");

        SetTgSlot("Start", false, "reset");
        SetTgSlot("Inside", false, "reset");
        SetTgSlot("Deep", false, "reset");
        SetTgSlot("End", false, "reset");

        if (twitchRoutine != null)
        {
            StopCoroutine(twitchRoutine);
            twitchRoutine = null;
        }

        if (eyesRoutine != null)
        {
            StopCoroutine(eyesRoutine);
            eyesRoutine = null;
        }

        if (mouthRoutine != null)
        {
            StopCoroutine(mouthRoutine);
            mouthRoutine = null;
        }

        RestoreEyesMorphs(reason);
        RestoreMouthMorphs(reason);
        ResetOffsets(reason);

        if (activeHeadSnapshot != null)
        {
            RestoreHeadControlState(activeHeadSnapshot);
            activeHeadSnapshot = null;
        }

        hbaLastEvent = "Reset";
        hbaLastAction = "None";
        hbaLastBlock = "";
        HighlightActionLabel("");
        HighlightTgLabel("", false);
        UpdateHbaStatus(true);

        DebugMessage("[HumanBodyAction] Reset / reason=" + reason);
    }

    void ResetOffsets(string reason)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            TwitchPart p = parts[i];
            if (p == null || p.controller == null) continue;

            if (p.lastOffset != Vector3.zero)
            {
                p.controller.transform.position = p.controller.transform.position - p.lastOffset;
            }

            p.lastOffset = Vector3.zero;
        }

        DebugMessage("[HumanBodyAction] Reset offsets / reason=" + reason);
    }

    void LogStatus(string source)
    {
        RefreshControllersNoReset();
        RefreshFaceMorphs();

        int activeParts = 0;
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i] != null && parts[i].controller != null) activeParts++;
        }

        SuperController.LogMessage(
            "[HumanBodyAction] STATUS" +
            " / source=" + source +
            " / atom=" + (containingAtom != null ? containingAtom.uid : "") +
            " / parts=" + activeParts + "/" + parts.Count +
            " / headControl=" + (headControl != null) +
            " / headCache=" + headPoseCache.Count +
            " / headAction=" + IsHeadActionEnabled() +
            " / body=" + IsTwitchBodyEnabled() +
            " / eyes=" + (twitchEyes != null && twitchEyes.val) +
            " / mouth=" + (twitchMouth != null && twitchMouth.val) +
            " / eyesMorphL=" + (eyesClosedLeftMorph != null) +
            " / eyesMorphR=" + (eyesClosedRightMorph != null) +
            " / eyesMorphSingle=" + (eyesClosedMorph != null) +
            " / mouthOpenMorph=" + (mouthOpenMorph != null) +
            " / queueLast=" + (queueLastAction != null && queueLastAction.val) +
            " / hbaTarget=" + GetTargetNameFromId(GetTargetId()) +
            " / hbaProgress=" + (hbaProgress != null ? hbaProgress.val.ToString("F3") : "n/a") +
            " / hbaActive=" + (hbaActive != null && hbaActive.val) +
            " / motion=" + hbaMotionState +
            " / tgTriggers=" + (hbaTgTriggers != null && hbaTgTriggers.val) +
            " / tgPrefix=" + GetTgPrefix() +
            " / startActions[slow=" + GetConfiguredStartAction("Slow") +
            ",normal=" + GetConfiguredStartAction("Normal") +
            ",fast=" + GetConfiguredStartAction("Fast") + "]" +
            " / insideActions[hold=" + GetConfiguredInsideAction("Hold") +
            ",slow=" + GetConfiguredInsideAction("Slow") +
            ",active=" + GetConfiguredInsideAction("Active") +
            ",fast=" + GetConfiguredInsideAction("Fast") + "]" +
            " / eventActions[deep=" + GetConfiguredEventAction("Deep") +
            ",end=" + GetConfiguredEventAction("End") + "]" +
            " / actions=HBA_Event_Start,HBA_Event_Inside,HBA_Event_Deep,HBA_Event_End,HBA_Twitch_Slow,HBA_Twitch_Weak,HBA_Twitch_Normal,HBA_Twitch_Strong,HBA_Head_Nod,HBA_Head_QuickNod,HBA_Head_IntenseShake"
        );
    }

    void SetFloat(JSONStorableFloat storable, float value)
    {
        if (storable != null)
        {
            storable.val = value;
            if (storable == hbaTargetId || storable == hbaProgress) MarkHbaDataReceived();
        }
    }

    void SetBool(JSONStorableBool storable, bool value)
    {
        if (storable != null)
        {
            storable.val = value;
            if (storable == hbaActive) MarkHbaDataReceived();
        }
    }

    float GetFloat(JSONStorableFloat storable, float fallback)
    {
        return storable != null ? storable.val : fallback;
    }

    void DebugMessage(string message)
    {
        if (IsDebug()) SuperController.LogMessage(message);
    }

    bool IsDebug()
    {
        return debugLog != null && debugLog.val;
    }

    void OnDisable()
    {
        StopAllAndReset("disable");
    }

    void OnDestroy()
    {
        StopAllAndReset("destroy");
    }
}
