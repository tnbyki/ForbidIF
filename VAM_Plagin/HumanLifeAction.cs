// HLA_HAND_COVER_IGNORE_HAND_IK_ON_FIX_BUILD 2026-06-28: Life RandomCover no longer treats Hand IK PositionState.On as unavailable; Respect Existing Hand IK is kept for save compatibility but no longer blocks hand selection.
// HLA_TG_HELD_HAND_TARGET_UID_STRICT_BUILD 2026-06-28: Uses TargetGrabber exported TG Held Target Person UID to block only the matching target Person's held L/R hand; legacy same-atom only, no cross-person fallback.
// HLA_TG_HELD_HAND_GLOBAL_TARGET_UID_SCAN_BUILD 2026-06-28: Scans all scene TargetGrabber instances and matches TG Held Target Person UID, so held target-hand flags are read even when TargetGrabber is installed on the grabbing/self Person, not on the target Person.
// HLA_TG_HELD_HAND_LINK_COMPILE_FIX_BUILD 2026-06-28: Removes stray HBA TwitchPart p.label block from TargetGrabber held-hand resolver and restores local JSONStorable declaration.
// HLA_TG_HELD_HAND_SIDE_ONLY_BLOCK_BUILD 2026-06-28: TargetGrabber L/R held-target-hand flags now block only the matching Life hand; the other hand can still run RandomCover.
// HLA_TG_HELD_HAND_PAUSE_COVER_BUILD 2026-06-27: Reads TargetGrabber hidden held-target-hand flags and skips/stops Life RandomCover while TG is holding target L/R Hand, preventing HLA self-hand motion from dragging the TG-held target hand.
// HLA_REMOVE_HAND_LOCK_UI_BUILD 2026-06-27: Removes the simple L/R Hand Locked checkbox and target text UI; keeps internal Life-owned hand/elbow ownership and HBA handoff release behavior.
// HLA_BREATH_AUTO_PAUSE_HBA_BUILD 2026-06-27: Adds Auto Pause Breath On HBA Active so chest breathing yields during HBA/progress/docking, stops without restoring old chest pose, and resumes from the current chest pose after idle.
// HLA_LIFE_HAND_LOCK_OWNERSHIP_BUILD 2026-06-27: Adds explicit HumanLifeAction-owned hand/elbow lock state and releases only Life-owned hand/elbow locks during HBA handoff.
// HLA_HBA_HANDOFF_RELEASE_BUILD 2026-06-27: When HBA becomes active/progressing during a Life RandomCover, HumanLifeAction now releases its hand/elbow with Comply/Off state instead of restoring PositionState.On at the mid-gesture point, preventing hand lock during docking handoff.
// HLA_HBA_RESUME_COVER_GESTURE_BUILD 2026-06-27: Pauses intermittent Life gestures while same-Person HBA is active/progressing, then schedules a quick resume gesture and forces RandomCover once when Cover frequency is enabled so Cover appears after docking/HBA returns idle.
// HLA_HBA_LEG_AUTO_PAUSE_BUILD 2026-06-27: Adds Auto Pause Leg On HBA Active; continuous Life Leg Motion pauses while same-Person HumanBodyAction reports HBA_Progress>0.005 or HBA_Active, then resumes from the current pose after a short idle delay without restoring old thigh pose.
// HLA_VISIBLE_BASE_LEG_SWAY_BUILD 2026-06-27: Makes continuous Life Leg Motion visibly stronger; raises base thigh rotation, adds tiny thigh position sway assist, keeps normal logs off and restores states on stop.
// HLA_BREATH5_LEG_SCALE_BUILD 2026-06-27: Defaults Life Breath Scale to 5.0 and adds visible Life Leg Scale slider for continuous thigh base-motion strength.
// HLA_BASE_LEG_MOTION_LOG_OFF_BUILD 2026-06-27: Changes Life Leg Motion from random gesture into a breath-like continuous subtle thigh-rotation base layer, and defaults cover detail logging OFF so normal logs stay quiet.
// HLA_HEADLOOK_TOGGLE_LEG_THIGH_MOTION_BUILD 2026-06-27: Adds Life Head Look toggle to let external gaze plugins own head/eyes, and adds lightweight Life Leg Motion using only L/R thigh rotation with pose-change-safe restore.
 // HLA_LONG_REACH_SOFTER_TILT_BUILD 2026-06-27: Extends Life RandomCover reach for all targets so far Head/Chest/Belly/Hip still pull the hand visibly toward the target, raises loose reach ratio, and softens LookTarget tilt.
// HLA_COVER_DETAIL_LOG_REACH_EFFORT_BUILD 2026-06-27: Adds default-ON cover detail logs showing which hand targets which point, raw distance, reach clamp/effort, and raises Head cover weight to make Head easier to appear.
// HLA_DEFAULTS_HEAD_FREE_TILT8_BUILD 2026-06-27: Sets Life Look Frequency default to 50 and Cover Frequency default to 90, weights RandomCover Head higher while keeping Free Hand at about 10% within Cover, and caps LookTarget slow tilt at 8 degrees.
// HLA_HAND_LINGER_LESS_ROBOTIC_BUILD 2026-06-27: RandomCover no longer snaps to an exact hand destination and immediately returns; it stops near the target, lingers with small drift, and returns more slowly to reduce robot-like hand motion.
// HLA_LOOSE_HAND_ROT_OFF_BUILD 2026-06-27: Life RandomCover now treats hand rotation as loose/off, avoids exact 100% target locking, returns near the original pose instead of snapping rotation back, and uses position-only pose-change checks for hand gestures.
// HLA_SOFT_LOOK_EYE_TARGET_BUILD 2026-06-27: Softens LookTarget head/neck motion, lowers slow gaze tilt from 15% to 13%, and optionally moves eyeTargetControl toward the selected Person during target gaze.
// HLA_TARGET_GAZE_SLOW_TILT_BUILD 2026-06-27: LookTarget hold slowly adds a subtle neck/head roll tilt; v019 lowers the ratio to 13% and smooths the motion.
// HLA_RANDOM_COVER_EXTRA_POINTS_FREE_BUILD 2026-06-27: Adds Life RandomCover targets Self/Target Belly and Hip, and adds Free Hand as a cover choice that briefly relaxes the selected hand/elbow before restoring.
// HLA_SAFE_ATOM_NAME_COMPILE_FIX_BUILD 2026-06-27: Fixes v013/v014 compile error by adding missing SafeAtomName helper; keeps LookTarget hold and LookAway vertical random gaze.
// HLA_LOOKAWAY_VERTICAL_BUILD 2026-06-27: LookAway random gaze now includes a stronger vertical component so glances can look up/down as well as left/right, scaled by Life Motion.
// HLA_TARGET_GAZE_HOLD_BUILD 2026-06-27: LookTarget now holds the gaze longer and softly tracks the selected target during the hold instead of immediately returning like camera/away glances.
// HLA_SOFT_RANDOM_COVER_BUILD 2026-06-27: Softens Life RandomCover by using slower eased Bezier hand paths, subtle organic arcs, softer Cover100 timing, and keeping elbow relaxed during the whole gesture.
// HLA_POSE_CHANGE_SAFE_BUILD 2026-06-27: Adds Pose Change Safe. If controllers are externally moved during Life breath/look/cover, old restore targets are abandoned, states are restored safely, breath is rebased to the current pose, and Life resumes after a short cooldown.
// HLA_LOOK_AWAY_COVER100_MORE_BUILD 2026-06-27: Adds LookAway (random empty/off-direction look) to Life look gestures, with force action/button and Look Away % split; Cover Frequency 100 is made more obvious/frequent while same-side elbow stays relaxed during hand cover.
// HLA_COVER100_ELBOW_RELAX_BUILD 2026-06-27: Random Cover frequency 100 now forces near-continuous RandomCover gestures with shorter intervals/stronger reach, and same-side elbow is temporarily relaxed/restored during hand cover.
// HLA_CONSTANT_BREATH_GESTURE_SPLIT_BUILD 2026-06-27: Breath is now a continuous base Life layer while Life is enabled; Look/Cover remain intermittent random gestures. HLA_Stop_Restore restores both gesture and breath.
// HLA_FREQ_SLIDERS_BREATH_FIX_BUILD 2026-06-27: Adds visible Cover/Look frequency sliders, Cover Self/Target and Look Target/Camera split sliders, and makes Breath visible by driving chest position+rotation with stronger scale and fallback aliases.
// HLA_BREATH_SCALE_HAND_GESTURE_BUILD 2026-06-27: Adds visible Life Breath Scale slider, makes breath/chest sway stronger and tunable, and makes Life RandomCover hand gestures actually run by default even when hand IK is On.
// HLA_WIDE_LOOK_BREATH_SWAY_BUILD 2026-06-27: Expands Life Look head rotation presets up to 180 degrees on Large, and adds light chest-based body sway during Life Breath.
// HLA_MOTION_PRESET_UI_BUILD 2026-06-27: Simplifies HumanLifeAction tuning to one Life Motion preset: Small / Normal / Large. Hidden detailed sliders remain registered for compatibility but are not used by the motion preset path.
// HLA_SIMPLE_UI_BUILD 2026-06-27: Same standalone Life gesture prototype as v002, but hides tuning-heavy toggles/sliders by default. Only Enable, Debug, Target, force buttons, and Stop are shown.
// HLA_STANDALONE_LIFE_GESTURE_BUILD 2026-06-27
// Standalone Life gesture prototype. It does not read HumanBodyAction yet.
// Purpose: low-frequency idle-like gestures: light chest breathing, brief target/camera look, and light random hand cover.
// Later build can read HBA_Progress/HBA_Active and allow Life only when HBA progress is zero.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class HumanLifeAction : MVRScript
{
    JSONStorableString statusText;
    JSONStorableBool lifeEnabled;
    JSONStorableBool debugLog;
    JSONStorableBool logCoverDetail;
    JSONStorableStringChooser lifeMotionMode;
    JSONStorableBool breathEnabled;
    JSONStorableBool autoPauseBreathOnHbaActive;
    JSONStorableBool lifeHeadLookEnabled;
    JSONStorableBool lookTargetEnabled;
    JSONStorableBool lookCameraEnabled;
    JSONStorableBool randomCoverEnabled;
    JSONStorableBool lifeLegMotionEnabled;
    JSONStorableBool autoPauseLegOnHbaActive;
    JSONStorableBool autoPauseGesturesOnHbaActive;
    JSONStorableBool respectExistingHandIk;
    JSONStorableBool poseChangeSafe;
    JSONStorableStringChooser targetPersonChooser;
    JSONStorableFloat intervalMin;
    JSONStorableFloat intervalMax;
    JSONStorableFloat lifeStrength;
    JSONStorableFloat breathAmount;
    JSONStorableFloat breathScale;
    JSONStorableFloat legScale;
    JSONStorableFloat coverFrequency;
    JSONStorableFloat lookFrequency;
    JSONStorableFloat coverSelfPercent;
    JSONStorableFloat lookTargetPercent;
    JSONStorableFloat lookAwayPercent;
    JSONStorableFloat lookMaxAngle;
    JSONStorableFloat coverMaxDistance;

    readonly List<string> targetPersonChoices = new List<string>() { TargetAutoOtherPerson };
    readonly List<string> lifeMotionChoices = new List<string>() { LifeMotionSmall, LifeMotionNormal, LifeMotionLarge };

    const string TargetAutoOtherPerson = "Auto Other Person";
    const string LifeMotionSmall = "Small";
    const string LifeMotionNormal = "Normal";
    const string LifeMotionLarge = "Large";
    const float DefaultIntervalMin = 4.0f;
    const float DefaultIntervalMax = 10.0f;
    const float DefaultLifeStrength = 1.0f;
    const float DefaultBreathAmount = 0.007f;
    const float DefaultBreathScale = 5.00f;
    const float DefaultLegScale = 1.00f;
    const float DefaultCoverFrequency = 90.0f;
    const float DefaultLookFrequency = 50.0f;
    const float DefaultCoverSelfPercent = 50.0f;
    const float DefaultLookTargetPercent = 50.0f;
    const float DefaultLookAwayPercent = 20.0f;
    const float DefaultLookMaxAngle = 90.0f;
    const float DefaultCoverMaxDistance = 0.58f;
    const float GestureLegMotionWeight = 14.0f;
    const float HbaProgressPauseThreshold = 0.005f;
    const float HbaLegResumeDelaySeconds = 0.75f;
    const float HbaBreathResumeDelaySeconds = 0.75f;
    const float HbaGestureResumeDelaySeconds = 0.45f;

    const float GestureNoneWeight = 40.0f;
    const float GestureBreathWeight = 25.0f;

    const float BreathSecondsPerCycle = 1.75f;
    const int BreathCycles = 2;
    const float BreathSwaySideRatio = 0.25f;
    const float BreathSwayForwardRatio = 0.75f;
    const float LookEnterSeconds = 0.72f;
    const float LookHoldSecondsMin = 0.35f;
    const float LookHoldSecondsMax = 0.90f;
    const float LookTargetHoldSmallMin = 1.20f;
    const float LookTargetHoldSmallMax = 2.20f;
    const float LookTargetHoldNormalMin = 2.20f;
    const float LookTargetHoldNormalMax = 4.00f;
    const float LookTargetHoldLargeMin = 3.50f;
    const float LookTargetHoldLargeMax = 6.00f;
    const float LookTargetTrackLerp = 0.055f;
    const float LookTargetTiltRatio = 0.13f;
    const float LookTargetTiltMinDegrees = 0.0f;
    const float LookTargetTiltMaxDegrees = 4.0f;
    const float LookTargetTiltInSeconds = 1.80f;
    const float LookReturnSeconds = 0.92f;
    const float EyeTargetEnterLerp = 0.18f;
    const float EyeTargetHoldLerp = 0.12f;
    const float EyeTargetReturnLerp = 0.20f;
    const float CoverPrepareSeconds = 0.16f;
    const float CoverMoveSeconds = 0.92f;
    const float CoverHoldSecondsMin = 0.85f;
    const float CoverHoldSecondsMax = 2.10f;
    const float CoverReturnSeconds = 1.45f;
    const float CoverSurfaceOffset = 0.055f;
    const float CoverSoftArcUp = 0.045f;
    const float CoverSoftArcSide = 0.030f;
    const float CoverSoftReturnArcScale = 0.55f;
    const float CoverPrepareBackScale = 0.070f;
    const float CoverPrepareArcScale = 0.28f;
    const float CoverHoldSwayAmount = 0.014f;
    const float CoverElbowNudgeScale = 0.135f;
    const float CoverElbowArcScale = 0.55f;
    const float CoverLooseReachMin = 0.88f;
    const float CoverLooseReachMax = 1.00f;
    const float Cover100LooseReachMin = 0.92f;
    const float Cover100LooseReachMax = 1.00f;
    const float CoverLooseReturnNearAmount = 0.018f;
    const float PoseChangePositionThreshold = 0.080f;
    const float PoseChangeRotationThreshold = 28.0f;
    const float PoseChangeCooldownMin = 0.50f;
    const float PoseChangeCooldownMax = 1.10f;
    const float LegMoveSecondsMin = 0.85f;
    const float LegMoveSecondsMax = 1.45f;
    const float LegHoldSecondsMin = 0.12f;
    const float LegHoldSecondsMax = 0.45f;
    const float LegReturnSecondsMin = 1.05f;
    const float LegReturnSecondsMax = 1.85f;
    const float LegPairChance = 30.0f;
    const float LegBaseSwaySideRatio = 0.80f;
    const float LegBaseSingleSideBias = 0.72f;

    FreeControllerV3 chestControl;
    FreeControllerV3 headControl;
    FreeControllerV3 eyeTargetControl;
    FreeControllerV3 lHandControl;
    FreeControllerV3 rHandControl;
    FreeControllerV3 hipControl;
    FreeControllerV3 lElbowControl;
    FreeControllerV3 rElbowControl;
    FreeControllerV3 lThighControl;
    FreeControllerV3 rThighControl;

    Coroutine lifeGestureRoutine;
    Coroutine breathLoopRoutine;
    Coroutine legBaseLoopRoutine;
    JSONStorable hbaStorable;
    JSONStorableFloat hbaProgressParam;
    JSONStorableBool hbaActiveParam;
    JSONStorable targetGrabberStorable;
    JSONStorableBool tgHeldTargetLHandParam;
    JSONStorableBool tgHeldTargetRHandParam;
    bool tgHeldTargetLHandCached = false;
    bool tgHeldTargetRHandCached = false;
    string tgHeldTargetSourceCached = "";
    float nextHbaResolveTime = -999.0f;
    float nextTargetGrabberResolveTime = -999.0f;
    float hbaLegResumeAllowedTime = -999.0f;
    bool legPausedByHba = false;
    float hbaBreathResumeAllowedTime = -999.0f;
    bool breathPausedByHba = false;
    float hbaGestureResumeAllowedTime = -999.0f;
    bool lifeGesturePausedByHba = false;
    bool forceCoverOnNextGesture = false;
    ControllerSnapshot activeBreathSnapshot;
    ControllerSnapshot activeLookSnapshot;
    ControllerSnapshot activeEyeSnapshot;
    ControllerSnapshot activeCoverHandSnapshot;
    ControllerSnapshot activeCoverElbowSnapshot;
    ControllerSnapshot activeLegLeftSnapshot;
    ControllerSnapshot activeLegRightSnapshot;
    ControllerSnapshot activeLegBaseLeftSnapshot;
    ControllerSnapshot activeLegBaseRightSnapshot;
    float nextGestureTime = -1.0f;
    string lastGesture = "None";
    bool initialized;

    class ControllerSnapshot
    {
        public FreeControllerV3 controller;
        public Vector3 position;
        public Quaternion rotation;
        public FreeControllerV3.PositionState positionState;
        public FreeControllerV3.RotationState rotationState;
    }

    class LifeLockState
    {
        public string name;
        public bool ownedByLife;
        public bool isHand;
        public string label;
        public FreeControllerV3 controller;
        public ControllerSnapshot snapshot;

        public LifeLockState(string name, bool isHand)
        {
            this.name = name;
            this.isHand = isHand;
            Clear();
        }

        public void Clear()
        {
            ownedByLife = false;
            label = "";
            controller = null;
            snapshot = null;
        }
    }

    LifeLockState lifeLHandLock = new LifeLockState("L Hand", true);
    LifeLockState lifeRHandLock = new LifeLockState("R Hand", true);
    LifeLockState lifeLElbowLock = new LifeLockState("L Elbow", false);
    LifeLockState lifeRElbowLock = new LifeLockState("R Elbow", false);

    class GestureChoice
    {
        public string name;
        public float weight;
        public Action action;

        public GestureChoice(string name, float weight, Action action)
        {
            this.name = name;
            this.weight = weight;
            this.action = action;
        }
    }

    public override void Init()
    {
        try
        {
            statusText = new JSONStorableString("HLA Status", "HumanLifeAction ready");
            RegisterString(statusText);
            CreateTextField(statusText, false);


            lifeEnabled = new JSONStorableBool("HLA Life Enable", true);
            RegisterBool(lifeEnabled);
            CreateToggle(lifeEnabled, false);

            debugLog = new JSONStorableBool("HLA Debug Log", false);
            RegisterBool(debugLog);
            CreateToggle(debugLog, false);

            logCoverDetail = new JSONStorableBool("HLA Log Cover Detail", false);
            RegisterBool(logCoverDetail);
            CreateToggle(logCoverDetail, false);

            lifeMotionMode = new JSONStorableStringChooser(
                "Life Motion",
                new List<string>(lifeMotionChoices),
                LifeMotionNormal,
                "Life Motion"
            );
            RegisterStringChooser(lifeMotionMode);
            CreatePopup(lifeMotionMode, false);

            breathEnabled = new JSONStorableBool("Life Breath", true);
            RegisterBool(breathEnabled);
            // v003 simple UI: keep registered/default ON but hide tuning toggle.

            autoPauseBreathOnHbaActive = new JSONStorableBool("Auto Pause Breath On HBA Active", true);
            RegisterBool(autoPauseBreathOnHbaActive);
            CreateToggle(autoPauseBreathOnHbaActive, false);

            lifeHeadLookEnabled = new JSONStorableBool("Life Head Look", true);
            RegisterBool(lifeHeadLookEnabled);
            CreateToggle(lifeHeadLookEnabled, false);

            lookTargetEnabled = new JSONStorableBool("Life Look Target", true);
            RegisterBool(lookTargetEnabled);
            // v003 simple UI: keep registered/default ON but hide tuning toggle.

            lookCameraEnabled = new JSONStorableBool("Life Look Camera", true);
            RegisterBool(lookCameraEnabled);
            // v003 simple UI: keep registered/default ON but hide tuning toggle.

            randomCoverEnabled = new JSONStorableBool("Life Random Cover", true);
            RegisterBool(randomCoverEnabled);
            // v003 simple UI: keep registered/default ON but hide tuning toggle.

            lifeLegMotionEnabled = new JSONStorableBool("Life Leg Motion", true);
            RegisterBool(lifeLegMotionEnabled);
            CreateToggle(lifeLegMotionEnabled, false);

            autoPauseLegOnHbaActive = new JSONStorableBool("Auto Pause Leg On HBA Active", true);
            RegisterBool(autoPauseLegOnHbaActive);
            CreateToggle(autoPauseLegOnHbaActive, false);

            autoPauseGesturesOnHbaActive = new JSONStorableBool("Auto Pause Gestures On HBA Active", true);
            RegisterBool(autoPauseGesturesOnHbaActive);
            CreateToggle(autoPauseGesturesOnHbaActive, false);

            respectExistingHandIk = new JSONStorableBool("Respect Existing Hand IK", false);
            RegisterBool(respectExistingHandIk);
            // v003 simple UI: keep registered/default ON but hide tuning toggle.

            poseChangeSafe = new JSONStorableBool("Pose Change Safe", true);
            RegisterBool(poseChangeSafe);
            CreateToggle(poseChangeSafe, false);

            RefreshTargetPersonChoices(false);
            targetPersonChooser = new JSONStorableStringChooser(
                "Life Target Person",
                new List<string>(targetPersonChoices),
                GetDefaultTargetPersonChoice(),
                "Life Target Person"
            );
            RegisterStringChooser(targetPersonChooser);
            CreatePopup(targetPersonChooser, false);

            intervalMin = new JSONStorableFloat("Life Interval Min", DefaultIntervalMin, 1.0f, 30.0f, true);
            RegisterFloat(intervalMin);
            // v003 simple UI: keep registered/default but hide tuning slider.

            intervalMax = new JSONStorableFloat("Life Interval Max", DefaultIntervalMax, 1.0f, 45.0f, true);
            RegisterFloat(intervalMax);
            // v003 simple UI: keep registered/default but hide tuning slider.

            lifeStrength = new JSONStorableFloat("Life Strength", DefaultLifeStrength, 0.0f, 2.0f, true);
            RegisterFloat(lifeStrength);
            // v003 simple UI: keep registered/default but hide tuning slider.

            breathAmount = new JSONStorableFloat("Life Breath Amount", DefaultBreathAmount, 0.0f, 0.030f, true);
            RegisterFloat(breathAmount);
            // v003 simple UI: keep registered/default but hide tuning slider.

            breathScale = new JSONStorableFloat("Life Breath Scale", DefaultBreathScale, 0.0f, 10.0f, true);
            RegisterFloat(breathScale);
            CreateSlider(breathScale, false);

            legScale = new JSONStorableFloat("Life Leg Scale", DefaultLegScale, 0.0f, 5.0f, true);
            RegisterFloat(legScale);
            CreateSlider(legScale, false);

            coverFrequency = new JSONStorableFloat("Life Cover Frequency", DefaultCoverFrequency, 0.0f, 100.0f, true);
            RegisterFloat(coverFrequency);
            CreateSlider(coverFrequency, false);

            lookFrequency = new JSONStorableFloat("Life Look Frequency", DefaultLookFrequency, 0.0f, 100.0f, true);
            RegisterFloat(lookFrequency);
            CreateSlider(lookFrequency, false);

            coverSelfPercent = new JSONStorableFloat("Life Cover Self %", DefaultCoverSelfPercent, 0.0f, 100.0f, true);
            RegisterFloat(coverSelfPercent);
            CreateSlider(coverSelfPercent, false);

            lookTargetPercent = new JSONStorableFloat("Life Look Target %", DefaultLookTargetPercent, 0.0f, 100.0f, true);
            RegisterFloat(lookTargetPercent);
            CreateSlider(lookTargetPercent, false);

            lookAwayPercent = new JSONStorableFloat("Life Look Away %", DefaultLookAwayPercent, 0.0f, 100.0f, true);
            RegisterFloat(lookAwayPercent);
            CreateSlider(lookAwayPercent, false);

            lookMaxAngle = new JSONStorableFloat("Life Look Max Angle", DefaultLookMaxAngle, 0.0f, 180.0f, true);
            RegisterFloat(lookMaxAngle);
            // v003 simple UI: keep registered/default but hide tuning slider.

            coverMaxDistance = new JSONStorableFloat("Life Cover Max Distance", DefaultCoverMaxDistance, 0.05f, 0.80f, true);
            RegisterFloat(coverMaxDistance);
            // v003 simple UI: keep registered/default but hide tuning slider.

            CreateButton("Refresh Life Targets", false).button.onClick.AddListener(delegate { RefreshTargetPersonChoices(true); });
            CreateButton("HLA_Force_Breath", false).button.onClick.AddListener(delegate { RequestBreath("button"); });
            CreateButton("HLA_Force_LookTarget", false).button.onClick.AddListener(delegate { RequestLookTarget("button"); });
            CreateButton("HLA_Force_LookCamera", false).button.onClick.AddListener(delegate { RequestLookCamera("button"); });
            CreateButton("HLA_Force_LookAway", false).button.onClick.AddListener(delegate { RequestLookAway("button"); });
            CreateButton("HLA_Force_RandomCover", false).button.onClick.AddListener(delegate { RequestRandomCover("button"); });
            CreateButton("HLA_Force_LegMotion", false).button.onClick.AddListener(delegate { RequestLegMotion("button"); });
            CreateButton("HLA_Stop_Restore", false).button.onClick.AddListener(delegate { StopAllLife("button"); });

            RegisterAction(new JSONStorableAction("HLA_Force_Breath", delegate { RequestBreath("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Force_LookTarget", delegate { RequestLookTarget("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Force_LookCamera", delegate { RequestLookCamera("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Force_LookAway", delegate { RequestLookAway("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Force_RandomCover", delegate { RequestRandomCover("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Force_LegMotion", delegate { RequestLegMotion("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Stop_Restore", delegate { StopAllLife("action"); }));

            ResolveControllers();
            ScheduleNextGesture("init");
            initialized = true;
            UpdateStatus("Ready");
        }
        catch (Exception e)
        {
            SuperController.LogError("[HumanLifeAction] Init exception: " + e);
        }
    }

    void Update()
    {
        if (!initialized) return;

        if (lifeEnabled == null || !lifeEnabled.val)
        {
            StopBreathLoop("life-off");
            StopLegBaseLoop("life-off");
            if (statusText != null) statusText.val = "HumanLifeAction: OFF / last=" + lastGesture;
            return;
        }

        if (!IsHeadLookEnabled() && activeLookSnapshot != null)
        {
            StopLifeGesture("head-look-off");
        }

        UpdateBreathLoopState();
        UpdateLegBaseLoopState();

        if (activeCoverHandSnapshot != null && IsHandBlockedByTargetGrabberHeldTargetHand(activeCoverHandSnapshot.controller))
            StopLifeGestureForTargetGrabberHeldHand();

        if (IsLifeGesturePausedByHba()) return;

        if (lifeGestureRoutine != null) return;

        if (Time.time >= nextGestureTime)
        {
            RunRandomLifeGesture();
        }
    }

    void ResolveControllers()
    {
        chestControl = FindControllerByAliases("chestControl", "chest", "abdomenControl", "abdomen");
        headControl = FindControllerByAliases("headControl", "head");
        eyeTargetControl = FindControllerByAliases("eyeTargetControl", "eyeTarget", "eyesTarget", "eyesTargetControl", "lookAtControl", "lookTargetControl", "eyeControl", "eyesControl");
        lHandControl = FindControllerByAliases("lHandControl", "lHand");
        rHandControl = FindControllerByAliases("rHandControl", "rHand");
        hipControl = FindControllerByAliases("hipControl", "hip", "pelvisControl", "pelvis");
        lElbowControl = FindControllerByAliases("lElbowControl", "lElbow", "leftElbowControl", "leftElbow");
        rElbowControl = FindControllerByAliases("rElbowControl", "rElbow", "rightElbowControl", "rightElbow");
        lThighControl = FindControllerByAliases("lThighControl", "lThigh", "leftThighControl", "leftThigh", "lUpperLeg", "leftUpperLeg");
        rThighControl = FindControllerByAliases("rThighControl", "rThigh", "rightThighControl", "rightThigh", "rUpperLeg", "rightUpperLeg");
    }

    void RunRandomLifeGesture()
    {
        ResolveControllers();

        if (forceCoverOnNextGesture)
        {
            forceCoverOnNextGesture = false;
            if (randomCoverEnabled != null && randomCoverEnabled.val && SafeFloat(coverFrequency, DefaultCoverFrequency) > 0.001f)
            {
                Log("Life roll / selected=RandomCover / mode=hba-resume");
                RequestRandomCover("hba-resume");
                return;
            }
        }

        // v009: when Cover Frequency is maxed, make the test intentionally obvious.
        // This bypasses None/Look rolls so 100 means RandomCover every scheduled Life tick.
        if (IsCover100Mode())
        {
            // v040: Do not suppress RandomCover globally just because TargetGrabber holds one target hand.
            // PickHandForCover() will exclude only the held L/R side; the opposite hand can still move.
            Log("Life roll / selected=RandomCover / mode=cover100");
            RequestRandomCover("life-cover100");
            return;
        }

        List<GestureChoice> choices = new List<GestureChoice>();
        choices.Add(new GestureChoice("None", GestureNoneWeight, delegate { GestureNone(); }));
        // v008: Breath is no longer a random gesture. It runs as the base Life layer while HLA Life Enable and Life Breath are ON.
        if (IsHeadLookEnabled())
        {
            float lookTotalWeight = Mathf.Max(0.0f, SafeFloat(lookFrequency, DefaultLookFrequency));
            float lookAwayWeight = lookTotalWeight * Mathf.Clamp01(SafeFloat(lookAwayPercent, DefaultLookAwayPercent) / 100.0f);
            float lookRemainWeight = Mathf.Max(0.0f, lookTotalWeight - lookAwayWeight);
            float lookTargetWeight = lookRemainWeight * Mathf.Clamp01(SafeFloat(lookTargetPercent, DefaultLookTargetPercent) / 100.0f);
            float lookCameraWeight = Mathf.Max(0.0f, lookRemainWeight - lookTargetWeight);
            if (lookTargetEnabled != null && lookTargetEnabled.val && lookTargetWeight > 0.001f)
                choices.Add(new GestureChoice("LookTarget", lookTargetWeight, delegate { RequestLookTarget("life"); }));
            if (lookCameraEnabled != null && lookCameraEnabled.val && lookCameraWeight > 0.001f)
                choices.Add(new GestureChoice("LookCamera", lookCameraWeight, delegate { RequestLookCamera("life"); }));
            if (lookTargetEnabled != null && lookTargetEnabled.val && lookAwayWeight > 0.001f)
                choices.Add(new GestureChoice("LookAway", lookAwayWeight, delegate { RequestLookAway("life"); }));
        }
        if (randomCoverEnabled != null && randomCoverEnabled.val)
        {
            // v040: RandomCover remains eligible; held TargetGrabber hand sides are filtered at hand selection.
            float coverWeight = Mathf.Max(0.0f, SafeFloat(coverFrequency, DefaultCoverFrequency));
            if (coverWeight > 0.001f) choices.Add(new GestureChoice("RandomCover", coverWeight, delegate { RequestRandomCover("life"); }));
        }

        // v026: Leg motion is now a continuous base layer like Breath, not a random gesture.

        float total = 0.0f;
        for (int i = 0; i < choices.Count; i++)
        {
            if (choices[i] != null && choices[i].weight > 0.0f) total += choices[i].weight;
        }

        if (total <= 0.001f)
        {
            ScheduleNextGesture("no-choices");
            return;
        }

        float roll = UnityEngine.Random.Range(0.0f, total);
        float acc = 0.0f;
        for (int i = 0; i < choices.Count; i++)
        {
            GestureChoice c = choices[i];
            if (c == null || c.weight <= 0.0f) continue;
            acc += c.weight;
            if (roll <= acc)
            {
                Log("Life roll / selected=" + c.name + " / roll=" + roll.ToString("F1", CultureInfo.InvariantCulture) + " / total=" + total.ToString("F1", CultureInfo.InvariantCulture));
                c.action();
                return;
            }
        }

        GestureNone();
    }

    void GestureNone()
    {
        lastGesture = "None";
        UpdateStatus("None");
        ScheduleNextGesture("none");
    }

    void RequestBreath(string source)
    {
        ResolveControllers();
        if (chestControl == null)
        {
            UpdateStatus("Breath skipped: no chestControl");
            return;
        }

        StopBreathLoop(source + ":rebase-breath");
        StartBreathLoop(source);
    }

    void UpdateBreathLoopState()
    {
        if (breathEnabled == null || !breathEnabled.val || SafeFloat(breathScale, DefaultBreathScale) <= 0.0001f)
        {
            StopBreathLoop("breath-disabled");
            return;
        }

        if (IsBreathPausedByHba())
        {
            // HBA/TargetLine owns chest during active/progress/docking. Do not restore the old Life breath pose;
            // stop breathing and let the current HBA/docking chest pose become the next baseline.
            StopBreathLoop("hba-active", false);
            return;
        }

        if (breathLoopRoutine == null)
        {
            ResolveControllers();
            if (chestControl != null)
            {
                StartBreathLoop("auto");
            }
        }
    }

    void StartBreathLoop(string source)
    {
        if (chestControl == null) return;
        activeBreathSnapshot = CaptureController(chestControl);
        breathLoopRoutine = StartCoroutine(BreathLoopRoutine(source));
        Log("Breath loop start / source=" + source + " / controller=" + (chestControl != null ? chestControl.name : "<none>"));
    }

    IEnumerator BreathLoopRoutine(string source)
    {
        lastGesture = "BreathLoop";

        ControllerSnapshot snap = activeBreathSnapshot;
        if (snap == null || snap.controller == null)
        {
            breathLoopRoutine = null;
            yield break;
        }

        FreeControllerV3 ctrl = snap.controller;
        Vector3 basePos = snap.position;
        Quaternion baseRot = snap.rotation;
        Vector3 lastAppliedPos = basePos;
        Quaternion lastAppliedRot = baseRot;
        float phase = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);
        float slowDriftPhase = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);
        float lastLog = -999.0f;

        try { ctrl.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { ctrl.currentRotationState = FreeControllerV3.RotationState.On; } catch { }

        while (lifeEnabled != null && lifeEnabled.val && breathEnabled != null && breathEnabled.val && ctrl != null)
        {
            if (IsPoseChangeSafeOn() && ControllerExternallyMoved(ctrl, lastAppliedPos, lastAppliedRot))
            {
                basePos = GetControllerPosition(ctrl);
                baseRot = GetControllerRotation(ctrl);
                lastAppliedPos = basePos;
                lastAppliedRot = baseRot;
                UpdateControllerSnapshotPose(snap, basePos, baseRot);
                activeBreathSnapshot = snap;
                phase = 0.0f;
                slowDriftPhase = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);
                Log("Pose change safe / breath rebase / controller=" + (ctrl != null ? ctrl.name : "<none>"));
                yield return null;
                continue;
            }

            float dt = Mathf.Max(0.0001f, Time.deltaTime);
            float cycleSeconds = EffectiveBreathCycleSeconds();
            phase += (Mathf.PI * 2.0f) * dt / cycleSeconds;
            slowDriftPhase += (Mathf.PI * 2.0f) * dt / Mathf.Max(4.0f, cycleSeconds * 3.0f);

            Vector3 rootForward = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.forward : Vector3.forward;
            Vector3 rootRight = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.right : Vector3.right;
            rootForward.y = 0.0f;
            rootRight.y = 0.0f;
            if (rootForward.sqrMagnitude < 0.0001f) rootForward = Vector3.forward;
            if (rootRight.sqrMagnitude < 0.0001f) rootRight = Vector3.right;
            rootForward.Normalize();
            rootRight.Normalize();
            Vector3 up = Vector3.up;

            float inhale = 0.5f + 0.5f * Mathf.Sin(phase);
            float breathWave = Smooth01(inhale);
            float exhaleWave = Mathf.Sin(phase);
            float sideWave = Mathf.Sin(slowDriftPhase);

            float amount = EffectiveBreathAmount();
            float swayAmount = EffectiveBreathBodySwayAmount();
            float rotAmount = EffectiveBreathRotationDegrees();

            Vector3 breathOffset = ((up * amount * 0.62f) + (rootForward * amount * 0.38f)) * breathWave;
            Vector3 bodySwayOffset = (rootForward * (swayAmount * 0.45f * exhaleWave)) + (rootRight * (swayAmount * 0.22f * sideWave));
            float pitch = rotAmount * 0.65f * breathWave;
            float lean = rotAmount * 0.20f * sideWave;
            Quaternion breathRot = Quaternion.AngleAxis(pitch, rootRight) * Quaternion.AngleAxis(lean, rootForward) * baseRot;

            Vector3 applyPos = basePos + breathOffset + bodySwayOffset;
            SetControllerPosition(ctrl, applyPos);
            SetControllerRotation(ctrl, breathRot);
            lastAppliedPos = applyPos;
            lastAppliedRot = breathRot;

            if (debugLog != null && debugLog.val && Time.time - lastLog > 3.0f)
            {
                lastLog = Time.time;
                Log("Breath loop / amount=" + amount.ToString("F3", CultureInfo.InvariantCulture) + " / sway=" + swayAmount.ToString("F3", CultureInfo.InvariantCulture) + " / rot=" + rotAmount.ToString("F2", CultureInfo.InvariantCulture));
            }

            yield return null;
        }

        RestoreController(snap);
        breathLoopRoutine = null;
        activeBreathSnapshot = null;
        Log("Breath loop stop / source=" + source);
    }

    void StopBreathLoop(string reason)
    {
        StopBreathLoop(reason, true);
    }

    void StopBreathLoop(string reason, bool restorePose)
    {
        if (breathLoopRoutine != null)
        {
            try { StopCoroutine(breathLoopRoutine); } catch { }
            breathLoopRoutine = null;
            Log("Stop breath loop / reason=" + reason + " / restorePose=" + (restorePose ? "1" : "0"));
        }

        if (activeBreathSnapshot != null)
        {
            if (restorePose) RestoreController(activeBreathSnapshot);
            else RestoreControllerStateOnly(activeBreathSnapshot);
            activeBreathSnapshot = null;
        }
    }

    void UpdateLegBaseLoopState()
    {
        if (!IsLegMotionEnabled())
        {
            StopLegBaseLoop("leg-disabled");
            return;
        }

        if (IsLegPausedByHba())
        {
            // v029: Docking/HBA-driven motion owns the lower body. Do not restore old Life thigh pose here;
            // just stop the base leg loop and let the current/docking pose become the next baseline.
            StopLegBaseLoop("hba-active", false);
            return;
        }

        if (legBaseLoopRoutine == null)
        {
            ResolveControllers();
            if (lThighControl != null || rThighControl != null)
            {
                StartLegBaseLoop("auto");
            }
        }
    }

    void StartLegBaseLoop(string source)
    {
        if (lThighControl == null && rThighControl == null) return;

        activeLegBaseLeftSnapshot = lThighControl != null ? CaptureController(lThighControl) : null;
        activeLegBaseRightSnapshot = rThighControl != null ? CaptureController(rThighControl) : null;
        legBaseLoopRoutine = StartCoroutine(LegBaseLoopRoutine(source));
        Log("Leg base loop start / source=" + source);
    }

    IEnumerator LegBaseLoopRoutine(string source)
    {
        ControllerSnapshot lSnap = activeLegBaseLeftSnapshot;
        ControllerSnapshot rSnap = activeLegBaseRightSnapshot;
        FreeControllerV3 lCtrl = lSnap != null ? lSnap.controller : null;
        FreeControllerV3 rCtrl = rSnap != null ? rSnap.controller : null;
        if (lCtrl == null && rCtrl == null)
        {
            legBaseLoopRoutine = null;
            yield break;
        }

        Vector3 lBasePos = lSnap != null ? lSnap.position : Vector3.zero;
        Vector3 rBasePos = rSnap != null ? rSnap.position : Vector3.zero;
        Quaternion lBaseRot = lSnap != null ? lSnap.rotation : Quaternion.identity;
        Quaternion rBaseRot = rSnap != null ? rSnap.rotation : Quaternion.identity;
        Quaternion lLastRot = lBaseRot;
        Quaternion rLastRot = rBaseRot;
        float phase = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);
        float slowPhase = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);
        float sideBias = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
        float lastLog = -999.0f;

        if (lCtrl != null)
        {
            try { lCtrl.currentRotationState = FreeControllerV3.RotationState.On; } catch { }
            try { lCtrl.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        }
        if (rCtrl != null)
        {
            try { rCtrl.currentRotationState = FreeControllerV3.RotationState.On; } catch { }
            try { rCtrl.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        }

        while (lifeEnabled != null && lifeEnabled.val && IsLegMotionEnabled())
        {
            if (IsPoseChangeSafeOn())
            {
                bool rebase = false;
                if (lCtrl != null && ControllerExternallyMoved(lCtrl, lBasePos, lLastRot)) rebase = true;
                if (rCtrl != null && ControllerExternallyMoved(rCtrl, rBasePos, rLastRot)) rebase = true;
                if (rebase)
                {
                    if (lCtrl != null && lSnap != null)
                    {
                        lBasePos = GetControllerPosition(lCtrl);
                        lBaseRot = GetControllerRotation(lCtrl);
                        lLastRot = lBaseRot;
                        UpdateControllerSnapshotPose(lSnap, lBasePos, lBaseRot);
                    }
                    if (rCtrl != null && rSnap != null)
                    {
                        rBasePos = GetControllerPosition(rCtrl);
                        rBaseRot = GetControllerRotation(rCtrl);
                        rLastRot = rBaseRot;
                        UpdateControllerSnapshotPose(rSnap, rBasePos, rBaseRot);
                    }
                    phase = 0.0f;
                    slowPhase = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);
                    sideBias = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
                    Log("Pose change safe / leg base rebase");
                    yield return null;
                    continue;
                }
            }

            float dt = Mathf.Max(0.0001f, Time.deltaTime);
            float cycleSeconds = EffectiveLegBaseCycleSeconds();
            phase += (Mathf.PI * 2.0f) * dt / cycleSeconds;
            slowPhase += (Mathf.PI * 2.0f) * dt / Mathf.Max(5.0f, cycleSeconds * 3.5f);

            Vector3 rootForward = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.forward : Vector3.forward;
            Vector3 rootRight = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.right : Vector3.right;
            rootForward.y = 0.0f;
            rootRight.y = 0.0f;
            if (rootForward.sqrMagnitude < 0.0001f) rootForward = Vector3.forward;
            if (rootRight.sqrMagnitude < 0.0001f) rootRight = Vector3.right;
            rootForward.Normalize();
            rootRight.Normalize();

            float amount = EffectiveLegBaseRotationDegrees();
            float posAmount = EffectiveLegBasePositionAmount();
            float openNorm = Mathf.Sin(phase);
            float relaxNorm = Mathf.Sin(slowPhase);
            float singleNorm = Mathf.Sin(slowPhase * 0.73f + 1.7f) * sideBias;
            float openWave = openNorm * amount;
            float relaxWave = relaxNorm * amount * LegBaseSwaySideRatio;
            float singleBias = singleNorm * amount * LegBaseSingleSideBias;

            if (lCtrl != null)
            {
                Quaternion lApply = Quaternion.AngleAxis((-openWave + singleBias), rootForward) * Quaternion.AngleAxis(relaxWave, rootRight) * lBaseRot;
                Vector3 lPos = lBasePos
                    + rootRight * ((-openNorm + singleNorm * 0.45f) * posAmount)
                    + rootForward * (relaxNorm * posAmount * 0.35f);
                SetControllerRotation(lCtrl, lApply);
                SetControllerPosition(lCtrl, lPos);
                lLastRot = lApply;
            }
            if (rCtrl != null)
            {
                Quaternion rApply = Quaternion.AngleAxis((openWave + singleBias), rootForward) * Quaternion.AngleAxis(-relaxWave, rootRight) * rBaseRot;
                Vector3 rPos = rBasePos
                    + rootRight * ((openNorm + singleNorm * 0.45f) * posAmount)
                    + rootForward * (-relaxNorm * posAmount * 0.35f);
                SetControllerRotation(rCtrl, rApply);
                SetControllerPosition(rCtrl, rPos);
                rLastRot = rApply;
            }

            if (debugLog != null && debugLog.val && Time.time - lastLog > 4.0f)
            {
                lastLog = Time.time;
                Log("Leg base loop / amount=" + amount.ToString("F2", CultureInfo.InvariantCulture)
                    + " / pos=" + posAmount.ToString("F3", CultureInfo.InvariantCulture)
                    + " / cycle=" + cycleSeconds.ToString("F1", CultureInfo.InvariantCulture));
            }

            yield return null;
        }

        RestoreController(activeLegBaseLeftSnapshot);
        RestoreController(activeLegBaseRightSnapshot);
        activeLegBaseLeftSnapshot = null;
        activeLegBaseRightSnapshot = null;
        legBaseLoopRoutine = null;
        Log("Leg base loop stop / source=" + source);
    }

    void StopLegBaseLoop(string reason)
    {
        StopLegBaseLoop(reason, true);
    }

    void StopLegBaseLoop(string reason, bool restorePose)
    {
        if (legBaseLoopRoutine != null)
        {
            try { StopCoroutine(legBaseLoopRoutine); } catch { }
            legBaseLoopRoutine = null;
            Log("Stop leg base loop / reason=" + reason + " / restorePose=" + (restorePose ? "1" : "0"));
        }

        if (activeLegBaseLeftSnapshot != null)
        {
            if (restorePose) RestoreController(activeLegBaseLeftSnapshot);
            else RestoreControllerStateOnly(activeLegBaseLeftSnapshot);
            activeLegBaseLeftSnapshot = null;
        }
        if (activeLegBaseRightSnapshot != null)
        {
            if (restorePose) RestoreController(activeLegBaseRightSnapshot);
            else RestoreControllerStateOnly(activeLegBaseRightSnapshot);
            activeLegBaseRightSnapshot = null;
        }
    }


    bool IsAutoPauseGesturesOnHbaActive()
    {
        return autoPauseGesturesOnHbaActive != null && autoPauseGesturesOnHbaActive.val;
    }

    bool IsLifeGesturePausedByHba()
    {
        if (!IsAutoPauseGesturesOnHbaActive())
        {
            lifeGesturePausedByHba = false;
            return false;
        }

        bool hasHba = false;
        float progress = 0.0f;
        bool active = false;
        if (TryReadHbaProgress(out progress)) hasHba = true;
        if (TryReadHbaActive(out active)) hasHba = true;

        if (!hasHba)
        {
            lifeGesturePausedByHba = false;
            return false;
        }

        bool nowActive = active || progress > HbaProgressPauseThreshold;
        if (nowActive)
        {
            hbaGestureResumeAllowedTime = Time.time + HbaGestureResumeDelaySeconds;
            forceCoverOnNextGesture = false;
            if (!lifeGesturePausedByHba)
            {
                lifeGesturePausedByHba = true;
                StopLifeGestureForHbaActive(progress, active);
                Log("Life gestures auto pause by HBA / progress=" + progress.ToString("F3", CultureInfo.InvariantCulture) + " / active=" + (active ? "1" : "0"));
            }
            return true;
        }

        if (Time.time < hbaGestureResumeAllowedTime)
        {
            return true;
        }

        if (lifeGesturePausedByHba)
        {
            lifeGesturePausedByHba = false;
            if (randomCoverEnabled != null && randomCoverEnabled.val && SafeFloat(coverFrequency, DefaultCoverFrequency) > 0.001f)
            {
                forceCoverOnNextGesture = true;
                ScheduleNextGestureSoon("hba-idle-resume-cover", 0.20f, 0.55f);
            }
            else
            {
                ScheduleNextGestureSoon("hba-idle-resume", 0.30f, 0.80f);
            }
            Log("Life gestures auto resume after HBA idle / progress=" + progress.ToString("F3", CultureInfo.InvariantCulture));
        }
        return false;
    }

    void StopLifeGestureForHbaActive(float progress, bool active)
    {
        if (lifeGestureRoutine != null)
        {
            try { StopCoroutine(lifeGestureRoutine); } catch { }
            lifeGestureRoutine = null;
        }

        // HBA/TargetLine owns the body now. This is not a pose-change abort.
        // Life must release its temporary cover hand/elbow before handing control over;
        // otherwise restoring a previous PositionState.On can lock the hand at the mid-gesture point.
        RestoreActiveGestureForHbaHandoff();
        UpdateStatus("Paused by HBA / released handoff / progress=" + progress.ToString("F3", CultureInfo.InvariantCulture) + " / active=" + (active ? "1" : "0"));
    }

    bool IsAutoPauseBreathOnHbaActive()
    {
        return autoPauseBreathOnHbaActive != null && autoPauseBreathOnHbaActive.val;
    }

    bool IsBreathPausedByHba()
    {
        if (!IsAutoPauseBreathOnHbaActive())
        {
            breathPausedByHba = false;
            return false;
        }

        bool hasHba = false;
        float progress = 0.0f;
        bool active = false;
        if (TryReadHbaProgress(out progress)) hasHba = true;
        if (TryReadHbaActive(out active)) hasHba = true;

        if (!hasHba)
        {
            breathPausedByHba = false;
            return false;
        }

        bool nowActive = active || progress > HbaProgressPauseThreshold;
        if (nowActive)
        {
            hbaBreathResumeAllowedTime = Time.time + HbaBreathResumeDelaySeconds;
            if (!breathPausedByHba)
            {
                breathPausedByHba = true;
                Log("Breath auto pause by HBA / progress=" + progress.ToString("F3", CultureInfo.InvariantCulture) + " / active=" + (active ? "1" : "0"));
            }
            return true;
        }

        if (Time.time < hbaBreathResumeAllowedTime)
        {
            return true;
        }

        if (breathPausedByHba)
        {
            breathPausedByHba = false;
            Log("Breath auto resume after HBA idle / progress=" + progress.ToString("F3", CultureInfo.InvariantCulture));
        }
        return false;
    }

    bool IsAutoPauseLegOnHbaActive()
    {
        return autoPauseLegOnHbaActive != null && autoPauseLegOnHbaActive.val;
    }

    bool IsLegPausedByHba()
    {
        if (!IsAutoPauseLegOnHbaActive())
        {
            legPausedByHba = false;
            return false;
        }

        bool hasHba = false;
        float progress = 0.0f;
        bool active = false;
        if (TryReadHbaProgress(out progress)) hasHba = true;
        if (TryReadHbaActive(out active)) hasHba = true;

        if (!hasHba)
        {
            legPausedByHba = false;
            return false;
        }

        bool nowActive = active || progress > HbaProgressPauseThreshold;
        if (nowActive)
        {
            hbaLegResumeAllowedTime = Time.time + HbaLegResumeDelaySeconds;
            if (!legPausedByHba)
            {
                legPausedByHba = true;
                Log("Leg auto pause by HBA / progress=" + progress.ToString("F3", CultureInfo.InvariantCulture) + " / active=" + (active ? "1" : "0"));
            }
            return true;
        }

        if (Time.time < hbaLegResumeAllowedTime)
        {
            return true;
        }

        if (legPausedByHba)
        {
            legPausedByHba = false;
            Log("Leg auto resume after HBA idle / progress=" + progress.ToString("F3", CultureInfo.InvariantCulture));
        }
        return false;
    }

    bool TryReadHbaProgress(out float progress)
    {
        progress = 0.0f;
        ResolveHbaParams(false);
        if (hbaProgressParam == null) return false;
        try
        {
            progress = hbaProgressParam.val;
            return true;
        }
        catch
        {
            return false;
        }
    }

    bool TryReadHbaActive(out bool active)
    {
        active = false;
        ResolveHbaParams(false);
        if (hbaActiveParam == null) return false;
        try
        {
            active = hbaActiveParam.val;
            return true;
        }
        catch
        {
            return false;
        }
    }

    void ResolveHbaParams(bool force)
    {
        if (containingAtom == null) return;
        if (!force && hbaStorable != null && (hbaProgressParam != null || hbaActiveParam != null)) return;
        if (!force && Time.time < nextHbaResolveTime) return;
        nextHbaResolveTime = Time.time + 2.0f;

        hbaStorable = null;
        hbaProgressParam = null;
        hbaActiveParam = null;

        List<string> ids = null;
        try { ids = containingAtom.GetStorableIDs(); } catch { ids = null; }
        if (ids == null) return;

        for (int i = 0; i < ids.Count; i++)
        {
            string sid = ids[i];
            if (string.IsNullOrEmpty(sid)) continue;
            if (sid.IndexOf("HumanBodyAction", StringComparison.OrdinalIgnoreCase) < 0) continue;

            JSONStorable st = null;
            try { st = containingAtom.GetStorableByID(sid); } catch { st = null; }
            if (st == null) continue;

            JSONStorableFloat p = null;
            JSONStorableBool a = null;
            try { p = st.GetFloatJSONParam("HBA_Progress"); } catch { p = null; }
            try { a = st.GetBoolJSONParam("HBA_Active"); } catch { a = null; }

            if (p != null || a != null)
            {
                hbaStorable = st;
                hbaProgressParam = p;
                hbaActiveParam = a;
                Log("HBA params linked / storable=" + sid + " / progress=" + (p != null ? "1" : "0") + " / active=" + (a != null ? "1" : "0"));
                return;
            }
        }
    }

    bool IsTargetGrabberHoldingTargetHand()
    {
        ResolveTargetGrabberParamsIfNeeded();
        return tgHeldTargetLHandCached || tgHeldTargetRHandCached;
    }

    bool IsTargetGrabberHoldingTargetHandSide(int side)
    {
        ResolveTargetGrabberParamsIfNeeded();
        if (side < 0) return tgHeldTargetLHandCached;
        if (side > 0) return tgHeldTargetRHandCached;
        return tgHeldTargetLHandCached || tgHeldTargetRHandCached;
    }

    bool IsHandBlockedByTargetGrabberHeldTargetHand(FreeControllerV3 hand)
    {
        int side = GetHandSide(hand);
        return side != 0 && IsTargetGrabberHoldingTargetHandSide(side);
    }

    int GetHandSide(FreeControllerV3 hand)
    {
        if (hand == null || hand.name == null) return 0;
        string n = hand.name.ToLowerInvariant();
        if (n.Contains("lhand")) return -1;
        if (n.Contains("rhand")) return 1;
        return 0;
    }

    string GetStringJsonParamValue(JSONStorable st, string paramName)
    {
        if (st == null || string.IsNullOrEmpty(paramName)) return "";
        try
        {
            JSONStorableString p = st.GetStringJSONParam(paramName);
            return p != null ? p.val : "";
        }
        catch
        {
            return "";
        }
    }

    void ResolveTargetGrabberParamsIfNeeded()
    {
        if (Time.time < nextTargetGrabberResolveTime) return;
        nextTargetGrabberResolveTime = Time.time + 0.10f;

        targetGrabberStorable = null;
        tgHeldTargetLHandParam = null;
        tgHeldTargetRHandParam = null;
        tgHeldTargetLHandCached = false;
        tgHeldTargetRHandCached = false;
        tgHeldTargetSourceCached = "";

        string selfUid = containingAtom != null ? containingAtom.uid : "";
        List<Atom> atoms = null;
        try { atoms = SuperController.singleton != null ? SuperController.singleton.GetAtoms() : null; } catch { atoms = null; }
        if (atoms == null) return;

        bool linked = false;
        for (int ai = 0; ai < atoms.Count; ai++)
        {
            Atom atom = atoms[ai];
            if (atom == null) continue;

            List<string> ids = null;
            try { ids = atom.GetStorableIDs(); } catch { ids = null; }
            if (ids == null) continue;

            for (int i = 0; i < ids.Count; i++)
            {
                string sid = ids[i];
                if (string.IsNullOrEmpty(sid)) continue;

                JSONStorable st = null;
                try { st = atom.GetStorableByID(sid); } catch { st = null; }
                if (st == null) continue;

                JSONStorableBool left = null;
                JSONStorableBool right = null;
                try { left = st.GetBoolJSONParam("TG Held Target L Hand"); } catch { left = null; }
                try { right = st.GetBoolJSONParam("TG Held Target R Hand"); } catch { right = null; }
                if (left == null && right == null) continue;

                bool leftHeld = false;
                bool rightHeld = false;
                try { if (left != null) leftHeld = left.val; } catch { leftHeld = false; }
                try { if (right != null) rightHeld = right.val; } catch { rightHeld = false; }

                string targetUid = GetStringJsonParamValue(st, "TG Held Target Person UID");
                bool matchesThisPerson = false;
                if (!string.IsNullOrEmpty(targetUid))
                {
                    matchesThisPerson = !string.IsNullOrEmpty(selfUid) && targetUid == selfUid;
                }
                else
                {
                    // Legacy TargetGrabber builds had no target UID export.
                    // Only same-atom params are safe; do not globally apply an unidentified held hand to every Person.
                    matchesThisPerson = atom == containingAtom;
                }
                if (!matchesThisPerson) continue;

                if (!linked)
                {
                    targetGrabberStorable = st;
                    tgHeldTargetLHandParam = left;
                    tgHeldTargetRHandParam = right;
                    tgHeldTargetSourceCached = (atom != null ? atom.uid : "<atom>") + "/" + sid;
                    linked = true;
                }

                if (leftHeld) tgHeldTargetLHandCached = true;
                if (rightHeld) tgHeldTargetRHandCached = true;
            }
        }

        if (linked && (tgHeldTargetLHandCached || tgHeldTargetRHandCached))
        {
            Log("TargetGrabber held hand aggregate / L=" + (tgHeldTargetLHandCached ? "1" : "0") +
                " / R=" + (tgHeldTargetRHandCached ? "1" : "0") +
                " / source=" + tgHeldTargetSourceCached);
        }
    }

    void StopLifeGestureForTargetGrabberHeldHand()
    {
        if (lifeGestureRoutine != null)
        {
            try { StopCoroutine(lifeGestureRoutine); } catch { }
            lifeGestureRoutine = null;
        }

        RestoreActiveGestureForHbaHandoff();
        UpdateStatus("RandomCover paused: TG held target hand");
        Log("Life cover auto pause by TargetGrabber held target hand");
        ScheduleNextGestureSoon("tg-held-target-hand", 0.45f, 0.90f);
    }

    float EffectiveBreathCycleSeconds()
    {
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 2.65f;
        if (mode == LifeMotionLarge) return 1.85f;
        return 2.25f;
    }

    void RequestLookTarget(string source)
    {
        ResolveControllers();
        Atom target = GetSelectedTargetPerson();
        Vector3 targetPos;
        if (target == null || !TryGetPersonLookPoint(target, out targetPos))
        {
            UpdateStatus("LookTarget skipped: no target");
            ScheduleNextGesture("look-target-no-target");
            return;
        }
        RequestLookAtPosition("LookTarget", targetPos, source, target);
    }

    void RequestLookCamera(string source)
    {
        ResolveControllers();
        Camera cam = Camera.main;
        if (cam == null || cam.transform == null)
        {
            UpdateStatus("LookCamera skipped: no Camera.main");
            ScheduleNextGesture("look-camera-no-camera");
            return;
        }
        RequestLookAtPosition("LookCamera", cam.transform.position, source);
    }

    void RequestLookAway(string source)
    {
        ResolveControllers();
        if (headControl == null)
        {
            UpdateStatus("LookAway skipped: no headControl");
            ScheduleNextGesture("look-away-no-head");
            return;
        }

        Vector3 headPos = GetControllerPosition(headControl);
        Vector3 rootForward = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.forward : Vector3.forward;
        rootForward.y = 0.0f;
        if (rootForward.sqrMagnitude < 0.0001f) rootForward = Vector3.forward;
        rootForward.Normalize();

        // "LookAway" deliberately avoids target/camera and points to a random empty side/front-back direction.
        // Avoid a tiny yaw so it does not look like a normal target/camera glance.
        float sign = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
        float yaw = sign * UnityEngine.Random.Range(65.0f, 165.0f);
        Vector3 dir = Quaternion.AngleAxis(yaw, Vector3.up) * rootForward;

        // v014: add clear up/down variation to the random away gaze.
        // This is still a direction vector, not a body pose change; LookAtRoutine clamps it by Life Motion angle.
        float vertical = UnityEngine.Random.Range(EffectiveLookAwayVerticalMin(), EffectiveLookAwayVerticalMax());
        // Avoid too many nearly-horizontal glances; if it picked a tiny vertical value, push it a bit up or down.
        if (Mathf.Abs(vertical) < 0.12f) vertical = (UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f) * UnityEngine.Random.Range(0.12f, 0.24f);
        dir.y = vertical;

        if (dir.sqrMagnitude < 0.0001f) dir = rootForward;
        dir.Normalize();

        if (debugLog != null && debugLog.val)
        {
            Log("LookAway random dir / yaw=" + yaw.ToString("F1", CultureInfo.InvariantCulture)
                + " / vertical=" + vertical.ToString("F2", CultureInfo.InvariantCulture)
                + " / mode=" + CurrentMotionMode());
        }

        Vector3 lookPoint = headPos + dir * 3.0f;
        RequestLookAtPosition("LookAway", lookPoint, source);
    }

    void RequestLookAtPosition(string label, Vector3 targetPos, string source)
    {
        RequestLookAtPosition(label, targetPos, source, null);
    }

    void RequestLookAtPosition(string label, Vector3 targetPos, string source, Atom trackingTarget)
    {
        ResolveControllers();
        if (!IsHeadLookEnabled())
        {
            UpdateStatus(label + " skipped: Life Head Look OFF");
            ScheduleNextGesture(label + "-headlook-off");
            return;
        }
        if (headControl == null)
        {
            UpdateStatus(label + " skipped: no headControl");
            ScheduleNextGesture(label + "-no-head");
            return;
        }

        StopLifeGesture(source + ":before-look");
        lifeGestureRoutine = StartCoroutine(LookAtRoutine(label, targetPos, source, trackingTarget));
    }

    IEnumerator LookAtRoutine(string label, Vector3 targetPos, string source, Atom trackingTarget)
    {
        lastGesture = label;
        UpdateStatus(label + " running / source=" + source);

        ControllerSnapshot snap = CaptureController(headControl);
        activeLookSnapshot = snap;
        ControllerSnapshot eyeSnap = null;
        if (label == "LookTarget" && trackingTarget != null && eyeTargetControl != null)
        {
            eyeSnap = CaptureController(eyeTargetControl);
            activeEyeSnapshot = eyeSnap;
            try { eyeTargetControl.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        }
        Quaternion startRot = GetControllerRotation(headControl);
        Vector3 headPos = GetControllerPosition(headControl);
        Vector3 dir = targetPos - headPos;
        if (dir.sqrMagnitude < 0.0001f)
        {
            RestoreController(snap);
            RestoreController(eyeSnap);
            activeLookSnapshot = null;
            activeEyeSnapshot = null;
            lifeGestureRoutine = null;
            ScheduleNextGesture("look-zero-dir");
            yield break;
        }
        dir.Normalize();

        Quaternion desired = Quaternion.LookRotation(dir, Vector3.up);
        float maxAngle = EffectiveLookMaxAngle();
        Quaternion targetRot = Quaternion.RotateTowards(startRot, desired, Mathf.Max(0.0f, maxAngle));
        Vector3 lastAppliedPos = headPos;
        Quaternion lastAppliedRot = startRot;

        try { headControl.currentRotationState = FreeControllerV3.RotationState.On; } catch { }

        float t = 0.0f;
        while (t < LookEnterSeconds)
        {
            if (IsPoseChangeSafeOn() && ControllerExternallyMoved(headControl, lastAppliedPos, lastAppliedRot))
            {
                AbortGestureForPoseChange(snap, null, label + ":enter");
                yield break;
            }
            t += Time.deltaTime;
            float e = Smoother01(t / LookEnterSeconds);
            Quaternion applyRot = Quaternion.Slerp(startRot, targetRot, e);
            SetControllerRotation(headControl, applyRot);
            if (eyeSnap != null) ApplyEyeTargetToward(targetPos, EyeTargetEnterLerp * e);
            lastAppliedPos = GetControllerPosition(headControl);
            lastAppliedRot = applyRot;
            yield return null;
        }
        SetControllerRotation(headControl, targetRot);
        if (eyeSnap != null) ApplyEyeTargetToward(targetPos, EyeTargetEnterLerp);
        lastAppliedPos = GetControllerPosition(headControl);
        lastAppliedRot = targetRot;

        bool trackTargetHold = label == "LookTarget" && trackingTarget != null;
        float hold = trackTargetHold ? UnityEngine.Random.Range(EffectiveLookTargetHoldMin(), EffectiveLookTargetHoldMax()) : UnityEngine.Random.Range(LookHoldSecondsMin, LookHoldSecondsMax);
        float targetGazeAngle = Quaternion.Angle(startRot, targetRot);
        float targetTiltAbs = Mathf.Clamp(targetGazeAngle * LookTargetTiltRatio, LookTargetTiltMinDegrees, LookTargetTiltMaxDegrees);
        float targetTiltSign = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
        float targetTiltDegrees = trackTargetHold ? targetTiltAbs * targetTiltSign : 0.0f;
        if (trackTargetHold) Log("LookTarget hold / seconds=" + hold.ToString("F2", CultureInfo.InvariantCulture)
            + " / tilt=" + targetTiltDegrees.ToString("F1", CultureInfo.InvariantCulture)
            + " / target=" + SafeAtomName(trackingTarget));
        float holdT = 0.0f;
        while (holdT < hold)
        {
            if (IsPoseChangeSafeOn() && ControllerExternallyMoved(headControl, lastAppliedPos, lastAppliedRot))
            {
                AbortGestureForPoseChange(snap, null, label + ":hold");
                yield break;
            }

            if (trackTargetHold)
            {
                Quaternion holdBaseRot = targetRot;
                Vector3 liveTargetPos;
                if (TryGetPersonLookPoint(trackingTarget, out liveTargetPos))
                {
                    Vector3 liveDir = liveTargetPos - GetControllerPosition(headControl);
                    if (liveDir.sqrMagnitude > 0.0001f)
                    {
                        liveDir.Normalize();
                        Quaternion liveDesired = Quaternion.LookRotation(liveDir, Vector3.up);
                        Quaternion liveTargetRot = Quaternion.RotateTowards(startRot, liveDesired, Mathf.Max(0.0f, maxAngle));
                        holdBaseRot = Quaternion.Slerp(GetControllerRotation(headControl), liveTargetRot, LookTargetTrackLerp);
                        if (eyeSnap != null) ApplyEyeTargetToward(liveTargetPos, EyeTargetHoldLerp);
                    }
                }

                if (eyeSnap != null) ApplyEyeTargetToward(targetPos, EyeTargetHoldLerp * 0.55f);
                float tiltIn = Mathf.Max(0.25f, Mathf.Min(LookTargetTiltInSeconds, hold * 0.55f));
                float tiltAlpha = Smoother01(holdT / tiltIn);
                Quaternion tiltedRot = ApplyLocalRoll(holdBaseRot, targetTiltDegrees * tiltAlpha);
                SetControllerRotation(headControl, tiltedRot);
                lastAppliedPos = GetControllerPosition(headControl);
                lastAppliedRot = tiltedRot;
            }

            holdT += Time.deltaTime;
            yield return null;
        }

        Quaternion from = GetControllerRotation(headControl);
        t = 0.0f;
        while (t < LookReturnSeconds)
        {
            if (IsPoseChangeSafeOn() && ControllerExternallyMoved(headControl, lastAppliedPos, lastAppliedRot))
            {
                AbortGestureForPoseChange(snap, null, label + ":return");
                yield break;
            }
            t += Time.deltaTime;
            float e = Smoother01(t / LookReturnSeconds);
            Quaternion applyRot = Quaternion.Slerp(from, startRot, e);
            SetControllerRotation(headControl, applyRot);
            if (eyeSnap != null && eyeSnap.controller != null)
            {
                Vector3 eyeReturn = Vector3.Lerp(GetControllerPosition(eyeSnap.controller), eyeSnap.position, Mathf.Clamp01(EyeTargetReturnLerp * e));
                SetControllerPosition(eyeSnap.controller, eyeReturn);
            }
            lastAppliedPos = GetControllerPosition(headControl);
            lastAppliedRot = applyRot;
            yield return null;
        }

        RestoreController(snap);
        RestoreController(eyeSnap);
        activeLookSnapshot = null;
        activeEyeSnapshot = null;
        lifeGestureRoutine = null;
        UpdateStatus(label + " done");
        ScheduleNextGesture(label + "-done");
    }

    void RequestRandomCover(string source)
    {
        // v040: TargetGrabber held hand is not a global cover stop.
        // Only the matching L/R hand is excluded in PickHandForCover().
        ResolveControllers();
        FreeControllerV3 hand = PickHandForCover();
        if (hand == null)
        {
            UpdateStatus("RandomCover skipped: no available hand");
            ScheduleNextGesture("cover-no-hand");
            return;
        }

        Vector3 targetPos;
        string targetLabel;
        if (!TryPickCoverTarget(out targetPos, out targetLabel))
        {
            UpdateStatus("RandomCover skipped: no cover target");
            LogCover("Cover selected / skipped=no-target / hand=" + GetHandLabel(hand) + " / source=" + source);
            ScheduleNextGesture("cover-no-target");
            return;
        }

        if (IsFreeCoverLabel(targetLabel))
        {
            LogCover("Cover selected / hand=" + GetHandLabel(hand) + " / target=Free Hand / action=free / source=" + source);
        }
        else
        {
            float selectedDist = Vector3.Distance(GetControllerPosition(hand), targetPos);
            float selectedReach = EffectiveCoverMaxDistance();
            string selectedPlan = selectedDist > selectedReach ? "stretch-to-reach" : "direct";
            LogCover("Cover selected / hand=" + GetHandLabel(hand)
                + " / target=" + targetLabel
                + " / dist=" + selectedDist.ToString("F3", CultureInfo.InvariantCulture)
                + " / reach=" + selectedReach.ToString("F3", CultureInfo.InvariantCulture)
                + " / plan=" + selectedPlan
                + " / source=" + source);
        }

        StopLifeGesture(source + ":before-cover");
        if (IsFreeCoverLabel(targetLabel))
            lifeGestureRoutine = StartCoroutine(RandomFreeHandRoutine(hand, targetLabel, source));
        else
            lifeGestureRoutine = StartCoroutine(RandomCoverRoutine(hand, targetPos, targetLabel, source));
    }

    bool IsFreeCoverLabel(string label)
    {
        return !string.IsNullOrEmpty(label) && label.IndexOf("Free", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    IEnumerator RandomFreeHandRoutine(FreeControllerV3 hand, string targetLabel, string source)
    {
        lastGesture = "RandomCover Free";
        UpdateStatus("RandomCover Free running / " + GetHandLabel(hand) + " / source=" + source);

        ControllerSnapshot snap = CaptureController(hand);
        activeCoverHandSnapshot = snap;
        FreeControllerV3 elbow = GetSameSideElbow(hand);
        ControllerSnapshot elbowSnap = CaptureController(elbow);
        activeCoverElbowSnapshot = elbowSnap;
        AcquireLifeLock(hand, true, targetLabel, snap);
        AcquireLifeLock(elbow, false, targetLabel, elbowSnap);

        Vector3 start = GetControllerPosition(hand);
        Quaternion startRot = GetControllerRotation(hand);
        Vector3 right = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.right : Vector3.right;
        Vector3 up = Vector3.up;
        Vector3 forward = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.forward : Vector3.forward;
        float side = (lHandControl != null && hand == lHandControl) ? -1.0f : 1.0f;
        if (rHandControl != null && hand == rHandControl) side = 1.0f;

        float dist = Mathf.Min(EffectiveCoverMaxDistance() * 0.40f, IsCover100Mode() ? 0.18f : 0.13f);
        Vector3 freeGoal = start + (right * side * dist * UnityEngine.Random.Range(0.32f, 0.70f))
                               + (forward * dist * UnityEngine.Random.Range(-0.24f, 0.16f))
                               + (up * dist * UnityEngine.Random.Range(-0.28f, 0.16f));
        Vector3 toGoal = freeGoal - start;
        Vector3 dir = toGoal.sqrMagnitude > 0.0001f ? toGoal.normalized : right * side;
        Vector3 arc = BuildSoftCoverArc(start, freeGoal, hand) * 0.85f;
        Vector3 prepare = start - dir * Mathf.Min(0.025f, toGoal.magnitude * CoverPrepareBackScale) + arc * CoverPrepareArcScale;
        Vector3 c1 = start + arc * 0.90f - dir * 0.018f;
        Vector3 c2 = freeGoal + arc * 0.35f - dir * 0.035f;

        Vector3 elbowStart = elbowSnap != null ? elbowSnap.position : Vector3.zero;
        Quaternion elbowStartRot = elbowSnap != null ? elbowSnap.rotation : Quaternion.identity;
        Vector3 elbowGoal = elbowStart;
        if (elbow != null)
        {
            elbowGoal = elbowStart + (toGoal * CoverElbowNudgeScale) + (arc * CoverElbowArcScale);
            try { elbow.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
            try { elbow.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }
        try { hand.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { hand.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        float prepareSeconds = EffectiveCoverPrepareSeconds();
        float moveSeconds = EffectiveCoverMoveSeconds() * 0.95f;
        float holdSeconds = UnityEngine.Random.Range(EffectiveCoverHoldMinSeconds(), EffectiveCoverHoldMaxSeconds()) + 0.12f;
        float returnSeconds = EffectiveCoverReturnSeconds() * 0.88f;

        float t = 0.0f;
        while (t < prepareSeconds)
        {
            t += Time.deltaTime;
            float e = Smoother01(t / prepareSeconds);
            Vector3 applyPos = Vector3.Lerp(start, prepare, e);
            SetControllerPosition(hand, applyPos);
            if (elbow != null) SetControllerPosition(elbow, Vector3.Lerp(elbowStart, elbowGoal, e * 0.35f));
            yield return null;
        }

        t = 0.0f;
        while (t < moveSeconds)
        {
            t += Time.deltaTime;
            float e = Smoother01(t / moveSeconds);
            Vector3 applyPos = CubicBezier(prepare, c1, c2, freeGoal, e);
            SetControllerPosition(hand, applyPos);
            if (elbow != null) SetControllerPosition(elbow, Vector3.Lerp(elbowStart, elbowGoal, Smoother01(e) * 0.82f));
            yield return null;
        }

        float holdT = 0.0f;
        Vector3 swayA = arc.sqrMagnitude > 0.0001f ? arc.normalized : up;
        Vector3 swayB = right * side;
        while (holdT < holdSeconds)
        {
            holdT += Time.deltaTime;
            float wave = Mathf.Sin(holdT * 5.2f) * CoverHoldSwayAmount * MotionScale();
            float wave2 = Mathf.Sin(holdT * 3.1f + 1.7f) * CoverHoldSwayAmount * 0.45f * MotionScale();
            SetControllerPosition(hand, freeGoal + swayA * wave + swayB * wave2);
            if (elbow != null) SetControllerPosition(elbow, elbowGoal + swayA * wave * 0.35f);
            yield return null;
        }

        Vector3 from = GetControllerPosition(hand);
        Vector3 elbowFrom = elbow != null ? GetControllerPosition(elbow) : Vector3.zero;
        Vector3 looseReturn = BuildLooseReturnPosition(snap.position, start, hand);
        Vector3 returnMid = ((from + looseReturn) * 0.5f) + (arc * CoverSoftReturnArcScale) - dir * 0.018f;
        Vector3 returnC1 = from + arc * 0.25f;
        Vector3 returnC2 = returnMid;
        t = 0.0f;
        while (t < returnSeconds)
        {
            t += Time.deltaTime;
            float e = Smoother01(t / returnSeconds);
            Vector3 applyPos = CubicBezier(from, returnC1, returnC2, looseReturn, e);
            SetControllerPosition(hand, applyPos);
            if (elbow != null) SetControllerPosition(elbow, Vector3.Lerp(elbowFrom, elbowSnap.position, Smoother01(e)));
            yield return null;
        }

        RestoreControllerStateOnly(snap);
        RestoreController(elbowSnap);
        ClearLifeLockForController(hand);
        ClearLifeLockForController(elbow);
        activeCoverHandSnapshot = null;
        activeCoverElbowSnapshot = null;
        activeLegLeftSnapshot = null;
        activeLegRightSnapshot = null;
        lifeGestureRoutine = null;
        UpdateStatus("RandomCover Free done");
        ScheduleNextGesture("cover-free-done");
    }

    IEnumerator RandomCoverRoutine(FreeControllerV3 hand, Vector3 targetPos, string targetLabel, string source)
    {
        lastGesture = "RandomCover";
        UpdateStatus("RandomCover running / " + GetHandLabel(hand) + " -> " + targetLabel + " / source=" + source);

        ControllerSnapshot snap = CaptureController(hand);
        activeCoverHandSnapshot = snap;
        FreeControllerV3 elbow = GetSameSideElbow(hand);
        ControllerSnapshot elbowSnap = CaptureController(elbow);
        activeCoverElbowSnapshot = elbowSnap;
        AcquireLifeLock(hand, true, targetLabel, snap);
        AcquireLifeLock(elbow, false, targetLabel, elbowSnap);
        Vector3 start = GetControllerPosition(hand);
        Quaternion startRot = GetControllerRotation(hand);
        Vector3 outward = start - targetPos;
        if (outward.sqrMagnitude < 0.0001f)
        {
            outward = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.forward : Vector3.forward;
        }
        outward.Normalize();

        Vector3 rawGoal = targetPos + outward * CoverSurfaceOffset;
        Vector3 goal = rawGoal;
        Vector3 delta = goal - start;
        float rawDist = delta.magnitude;
        float maxDist = EffectiveCoverMaxDistance();
        bool stretchToReach = false;
        if (delta.magnitude > maxDist)
        {
            // HBA-style behavior: never drop a far target. Move as far as this Life gesture may reach toward it.
            stretchToReach = true;
            goal = start + delta.normalized * maxDist;
            delta = goal - start;
        }
        Vector3 dir = delta.sqrMagnitude > 0.0001f ? delta.normalized : outward;
        float reachRatio = 1.0f;
        if (delta.sqrMagnitude > 0.0001f)
        {
            float reachMin = IsCover100Mode() ? Cover100LooseReachMin : CoverLooseReachMin;
            float reachMax = IsCover100Mode() ? Cover100LooseReachMax : CoverLooseReachMax;
            reachRatio = UnityEngine.Random.Range(reachMin, reachMax);
            goal = start + delta * Mathf.Clamp01(reachRatio);
            delta = goal - start;
            dir = delta.sqrMagnitude > 0.0001f ? delta.normalized : outward;
        }
        LogCover("Cover move plan / hand=" + GetHandLabel(hand)
            + " / target=" + targetLabel
            + " / rawDist=" + rawDist.ToString("F3", CultureInfo.InvariantCulture)
            + " / reach=" + maxDist.ToString("F3", CultureInfo.InvariantCulture)
            + " / far=" + (stretchToReach ? "1" : "0")
            + " / reachRatio=" + reachRatio.ToString("F2", CultureInfo.InvariantCulture)
            + " / finalDist=" + delta.magnitude.ToString("F3", CultureInfo.InvariantCulture));

        try { hand.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { hand.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        Vector3 elbowStart = elbowSnap != null ? elbowSnap.position : Vector3.zero;
        Quaternion elbowStartRot = elbowSnap != null ? elbowSnap.rotation : Quaternion.identity;
        Vector3 coverArc = BuildSoftCoverArc(start, goal, hand);
        Vector3 elbowGoal = elbowStart;
        if (elbow != null)
        {
            elbowGoal = elbowStart + (delta * CoverElbowNudgeScale) + (coverArc * CoverElbowArcScale);
            try { elbow.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
            try { elbow.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }

        float prepareSeconds = EffectiveCoverPrepareSeconds();
        float moveSeconds = EffectiveCoverMoveSeconds();
        float holdMin = EffectiveCoverHoldMinSeconds();
        float holdMax = EffectiveCoverHoldMaxSeconds();
        float returnSeconds = EffectiveCoverReturnSeconds();

        Vector3 lastAppliedPos = start;
        Quaternion lastAppliedRot = startRot;
        Vector3 sideAxisForHold = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.right : Vector3.right;
        float handSideForHold = (lHandControl != null && hand == lHandControl) ? -1.0f : 1.0f;
        if (rHandControl != null && hand == rHandControl) handSideForHold = 1.0f;
        Vector3 arcAxisForHold = coverArc.sqrMagnitude > 0.0001f ? coverArc.normalized : Vector3.up;
        float looseDrift = UnityEngine.Random.Range(0.006f, 0.020f) * MotionScale();
        Vector3 holdAnchor = goal + arcAxisForHold * looseDrift + sideAxisForHold * handSideForHold * UnityEngine.Random.Range(-0.010f, 0.014f) * MotionScale();
        Vector3 prepare = start - dir * Mathf.Min(0.030f, delta.magnitude * CoverPrepareBackScale) + coverArc * CoverPrepareArcScale;
        Vector3 c1 = start + coverArc * 1.05f - dir * 0.020f;
        Vector3 c2 = holdAnchor + coverArc * 0.38f - dir * Mathf.Min(0.055f, delta.magnitude * 0.16f);

        float t = 0.0f;
        while (t < prepareSeconds)
        {
            if (IsPoseChangeSafeOn() && ControllerExternallyMovedPositionOnly(hand, lastAppliedPos))
            {
                AbortGestureForPoseChange(snap, elbowSnap, "cover:prepare");
                yield break;
            }
            t += Time.deltaTime;
            float e = Smoother01(t / prepareSeconds);
            Vector3 applyPos = Vector3.Lerp(start, prepare, e);
            SetControllerPosition(hand, applyPos);
            if (elbow != null) SetControllerPosition(elbow, Vector3.Lerp(elbowStart, elbowGoal, e * 0.28f));
            lastAppliedPos = applyPos;
            lastAppliedRot = GetControllerRotation(hand);
            yield return null;
        }

        t = 0.0f;
        while (t < moveSeconds)
        {
            if (IsPoseChangeSafeOn() && ControllerExternallyMovedPositionOnly(hand, lastAppliedPos))
            {
                AbortGestureForPoseChange(snap, elbowSnap, "cover:move");
                yield break;
            }
            t += Time.deltaTime;
            float e = Smoother01(t / moveSeconds);
            Vector3 applyPos = CubicBezier(prepare, c1, c2, holdAnchor, e);
            SetControllerPosition(hand, applyPos);
            if (elbow != null) SetControllerPosition(elbow, Vector3.Lerp(elbowStart, elbowGoal, Smoother01(e) * 0.88f));
            lastAppliedPos = applyPos;
            lastAppliedRot = GetControllerRotation(hand);
            yield return null;
        }
        Vector3 currentHoldAnchor = GetControllerPosition(hand);
        if (elbow != null) SetControllerPosition(elbow, elbowGoal);
        lastAppliedPos = currentHoldAnchor;
        lastAppliedRot = GetControllerRotation(hand);

        float holdSeconds = UnityEngine.Random.Range(holdMin, holdMax);
        Vector3 swayA = coverArc.sqrMagnitude > 0.0001f ? coverArc.normalized : Vector3.up;
        Vector3 swayB = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.right : Vector3.right;
        float handSide = (lHandControl != null && hand == lHandControl) ? -1.0f : 1.0f;
        if (rHandControl != null && hand == rHandControl) handSide = 1.0f;
        float holdT = 0.0f;
        while (holdT < holdSeconds)
        {
            if (IsPoseChangeSafeOn() && ControllerExternallyMovedPositionOnly(hand, lastAppliedPos))
            {
                AbortGestureForPoseChange(snap, elbowSnap, "cover:hold");
                yield break;
            }
            holdT += Time.deltaTime;
            float fade = Mathf.Sin(Mathf.Clamp01(holdT / Mathf.Max(0.001f, holdSeconds)) * Mathf.PI);
            float wave = Mathf.Sin(holdT * 5.1f) * CoverHoldSwayAmount * MotionScale() * fade;
            float wave2 = Mathf.Sin(holdT * 3.3f + 1.9f) * CoverHoldSwayAmount * 0.50f * MotionScale() * fade;
            Vector3 applyPos = currentHoldAnchor + swayA * wave + swayB * handSide * wave2;
            SetControllerPosition(hand, applyPos);
            if (elbow != null) SetControllerPosition(elbow, elbowGoal + swayA * wave * 0.35f);
            lastAppliedPos = applyPos;
            lastAppliedRot = GetControllerRotation(hand);
            yield return null;
        }

        Vector3 from = GetControllerPosition(hand);
        Vector3 elbowFrom = elbow != null ? GetControllerPosition(elbow) : Vector3.zero;
        Vector3 looseReturn = BuildLooseReturnPosition(snap.position, start, hand);
        Vector3 returnC1 = from + coverArc * 0.25f - dir * 0.010f;
        Vector3 returnC2 = ((from + looseReturn) * 0.5f) + (coverArc * CoverSoftReturnArcScale) - dir * Mathf.Min(0.035f, delta.magnitude * 0.10f);
        t = 0.0f;
        while (t < returnSeconds)
        {
            if (IsPoseChangeSafeOn() && ControllerExternallyMovedPositionOnly(hand, lastAppliedPos))
            {
                AbortGestureForPoseChange(snap, elbowSnap, "cover:return");
                yield break;
            }
            t += Time.deltaTime;
            float e = Smoother01(t / returnSeconds);
            Vector3 applyPos = CubicBezier(from, returnC1, returnC2, looseReturn, e);
            SetControllerPosition(hand, applyPos);
            if (elbow != null) SetControllerPosition(elbow, Vector3.Lerp(elbowFrom, elbowSnap.position, Smoother01(e)));
            lastAppliedPos = applyPos;
            lastAppliedRot = GetControllerRotation(hand);
            yield return null;
        }

        RestoreControllerStateOnly(snap);
        RestoreController(elbowSnap);
        ClearLifeLockForController(hand);
        ClearLifeLockForController(elbow);
        activeCoverHandSnapshot = null;
        activeCoverElbowSnapshot = null;
        lifeGestureRoutine = null;
        UpdateStatus("RandomCover done");
        ScheduleNextGesture("cover-done");
    }


    void RequestLegMotion(string source)
    {
        ResolveControllers();
        if (!IsLegMotionEnabled())
        {
            UpdateStatus("LegMotion skipped: disabled");
            ScheduleNextGesture("leg-disabled");
            return;
        }

        if (lThighControl == null && rThighControl == null)
        {
            UpdateStatus("LegMotion skipped: no thigh controls");
            ScheduleNextGesture("leg-no-thigh");
            return;
        }

        StopLifeGesture(source + ":before-leg");
        StopLegBaseLoop(source + ":before-force-leg");
        lifeGestureRoutine = StartCoroutine(LegMotionRoutine(source));
    }

    IEnumerator LegMotionRoutine(string source)
    {
        lastGesture = "LegMotion";
        UpdateStatus("LegMotion running / source=" + source);

        bool usePair = lThighControl != null && rThighControl != null && UnityEngine.Random.Range(0.0f, 100.0f) < LegPairChance;
        FreeControllerV3 primary = null;
        FreeControllerV3 secondary = null;

        if (usePair)
        {
            primary = lThighControl;
            secondary = rThighControl;
        }
        else
        {
            if (lThighControl != null && rThighControl != null)
                primary = UnityEngine.Random.value < 0.5f ? lThighControl : rThighControl;
            else
                primary = lThighControl != null ? lThighControl : rThighControl;
        }

        if (primary == null)
        {
            lifeGestureRoutine = null;
            ScheduleNextGesture("leg-no-primary");
            yield break;
        }

        ControllerSnapshot snapA = CaptureController(primary);
        ControllerSnapshot snapB = usePair ? CaptureController(secondary) : null;
        if (primary == lThighControl) activeLegLeftSnapshot = snapA;
        else if (primary == rThighControl) activeLegRightSnapshot = snapA;
        if (secondary == lThighControl) activeLegLeftSnapshot = snapB;
        else if (secondary == rThighControl) activeLegRightSnapshot = snapB;

        Vector3 rootForward = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.forward : Vector3.forward;
        Vector3 rootRight = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.right : Vector3.right;
        rootForward.y = 0.0f;
        rootRight.y = 0.0f;
        if (rootForward.sqrMagnitude < 0.0001f) rootForward = Vector3.forward;
        if (rootRight.sqrMagnitude < 0.0001f) rootRight = Vector3.right;
        rootForward.Normalize();
        rootRight.Normalize();

        float amount = EffectiveLegRotationDegrees();
        float openSign = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
        float relaxSign = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;

        Quaternion startA = snapA != null ? snapA.rotation : GetControllerRotation(primary);
        Quaternion startB = snapB != null ? snapB.rotation : Quaternion.identity;

        float sideA = primary == lThighControl ? -1.0f : 1.0f;
        Quaternion goalA = BuildLegGoalRotation(startA, rootForward, rootRight, amount, sideA, openSign, relaxSign);
        Quaternion goalB = startB;
        if (usePair && secondary != null && snapB != null)
        {
            float sideB = secondary == lThighControl ? -1.0f : 1.0f;
            goalB = BuildLegGoalRotation(startB, rootForward, rootRight, amount, sideB, openSign, -relaxSign);
        }

        try { primary.currentRotationState = FreeControllerV3.RotationState.On; } catch { }
        if (secondary != null) { try { secondary.currentRotationState = FreeControllerV3.RotationState.On; } catch { } }

        float moveSeconds = UnityEngine.Random.Range(LegMoveSecondsMin, LegMoveSecondsMax);
        float holdSeconds = UnityEngine.Random.Range(LegHoldSecondsMin, LegHoldSecondsMax);
        float returnSeconds = UnityEngine.Random.Range(LegReturnSecondsMin, LegReturnSecondsMax);
        Quaternion lastA = startA;
        Quaternion lastB = startB;

        Log("LegMotion selected / mode=" + (usePair ? "Pair Thigh" : (primary == lThighControl ? "L Thigh" : "R Thigh"))
            + " / degrees=" + amount.ToString("F1", CultureInfo.InvariantCulture));

        float t = 0.0f;
        while (t < moveSeconds)
        {
            if (IsPoseChangeSafeOn())
            {
                if (ControllerExternallyMoved(primary, snapA.position, lastA) || (secondary != null && ControllerExternallyMoved(secondary, snapB.position, lastB)))
                {
                    AbortGestureForPoseChange(snapA, snapB, "leg:move");
                    yield break;
                }
            }
            t += Time.deltaTime;
            float e = Smoother01(t / moveSeconds);
            Quaternion applyA = Quaternion.Slerp(startA, goalA, e);
            SetControllerRotation(primary, applyA);
            lastA = applyA;
            if (secondary != null && snapB != null)
            {
                Quaternion applyB = Quaternion.Slerp(startB, goalB, e);
                SetControllerRotation(secondary, applyB);
                lastB = applyB;
            }
            yield return null;
        }

        float holdT = 0.0f;
        while (holdT < holdSeconds)
        {
            holdT += Time.deltaTime;
            float wobble = Mathf.Sin(holdT * 4.7f) * Mathf.Min(0.45f, amount * 0.10f);
            Quaternion applyA = Quaternion.AngleAxis(wobble, rootForward) * goalA;
            SetControllerRotation(primary, applyA);
            lastA = applyA;
            if (secondary != null && snapB != null)
            {
                Quaternion applyB = Quaternion.AngleAxis(-wobble, rootForward) * goalB;
                SetControllerRotation(secondary, applyB);
                lastB = applyB;
            }
            yield return null;
        }

        Quaternion fromA = GetControllerRotation(primary);
        Quaternion fromB = secondary != null ? GetControllerRotation(secondary) : Quaternion.identity;
        t = 0.0f;
        while (t < returnSeconds)
        {
            if (IsPoseChangeSafeOn())
            {
                if (ControllerExternallyMoved(primary, snapA.position, lastA) || (secondary != null && ControllerExternallyMoved(secondary, snapB.position, lastB)))
                {
                    AbortGestureForPoseChange(snapA, snapB, "leg:return");
                    yield break;
                }
            }
            t += Time.deltaTime;
            float e = Smoother01(t / returnSeconds);
            Quaternion applyA = Quaternion.Slerp(fromA, startA, e);
            SetControllerRotation(primary, applyA);
            lastA = applyA;
            if (secondary != null && snapB != null)
            {
                Quaternion applyB = Quaternion.Slerp(fromB, startB, e);
                SetControllerRotation(secondary, applyB);
                lastB = applyB;
            }
            yield return null;
        }

        RestoreController(snapA);
        RestoreController(snapB);
        activeLegLeftSnapshot = null;
        activeLegRightSnapshot = null;
        lifeGestureRoutine = null;
        UpdateStatus("LegMotion done");
        ScheduleNextGesture("leg-done");
        UpdateLegBaseLoopState();
    }

    Quaternion BuildLegGoalRotation(Quaternion start, Vector3 rootForward, Vector3 rootRight, float amount, float side, float openSign, float relaxSign)
    {
        float openAngle = amount * side * openSign;
        float relaxAngle = amount * 0.45f * relaxSign;
        return Quaternion.AngleAxis(openAngle, rootForward) * Quaternion.AngleAxis(relaxAngle, rootRight) * start;
    }

    FreeControllerV3 GetSameSideElbow(FreeControllerV3 hand)
    {
        if (hand == null) return null;
        if (lHandControl != null && hand == lHandControl) return lElbowControl;
        if (rHandControl != null && hand == rHandControl) return rElbowControl;
        string label = GetHandLabel(hand);
        if (label.IndexOf("L Hand", StringComparison.OrdinalIgnoreCase) >= 0) return lElbowControl;
        if (label.IndexOf("R Hand", StringComparison.OrdinalIgnoreCase) >= 0) return rElbowControl;
        return null;
    }

    FreeControllerV3 PickHandForCover()
    {
        List<FreeControllerV3> candidates = new List<FreeControllerV3>();
        if (IsHandUsableForLife(lHandControl)) candidates.Add(lHandControl);
        if (IsHandUsableForLife(rHandControl)) candidates.Add(rHandControl);
        if (candidates.Count == 0 && respectExistingHandIk != null && !respectExistingHandIk.val)
        {
            // v040: legacy fallback may ignore IK state, but must not ignore TargetGrabber held L/R side.
            if (lHandControl != null && !IsHandBlockedByTargetGrabberHeldTargetHand(lHandControl)) candidates.Add(lHandControl);
            if (rHandControl != null && !IsHandBlockedByTargetGrabberHeldTargetHand(rHandControl)) candidates.Add(rHandControl);
        }
        if (candidates.Count == 0) return null;
        return candidates[UnityEngine.Random.Range(0, candidates.Count)];
    }

    bool IsHandUsableForLife(FreeControllerV3 hand)
    {
        // v040: Hand IK PositionState.On is allowed, but a TargetGrabber-held target hand side is not.
        // If TG holds only L Hand, R Hand remains selectable; if TG holds only R Hand, L Hand remains selectable.
        return hand != null && !IsHandBlockedByTargetGrabberHeldTargetHand(hand);
    }

    bool TryPickCoverTarget(out Vector3 pos, out string label)
    {
        List<string> selfLabels = new List<string>();
        List<Vector3> selfPositions = new List<Vector3>();
        List<string> targetLabels = new List<string>();
        List<Vector3> targetPositions = new List<Vector3>();

        List<float> selfWeights = new List<float>();
        List<float> targetWeights = new List<float>();

        Vector3 p;
        // v023: weighted Life cover targets. Head is intentionally much easier to appear,
        // while Free Hand remains about 10% inside the Cover branch.
        if (TryGetSelfHeadPoint(out p)) { selfLabels.Add("Self Head"); selfPositions.Add(p); selfWeights.Add(5.0f); }
        if (TryGetSelfChestPoint(out p)) { selfLabels.Add("Self Chest"); selfPositions.Add(p); selfWeights.Add(2.0f); }
        if (TryGetSelfBellyPoint(out p)) { selfLabels.Add("Self Belly"); selfPositions.Add(p); selfWeights.Add(2.0f); }
        if (TryGetSelfHipPoint(out p)) { selfLabels.Add("Self Hip"); selfPositions.Add(p); selfWeights.Add(2.0f); }
        selfLabels.Add("Free Hand"); selfPositions.Add(Vector3.zero); selfWeights.Add(1.0f);

        Atom target = GetSelectedTargetPerson();
        if (target != null)
        {
            if (TryGetControllerPoint(target, out p, "headControl", "head")) { targetLabels.Add("Target Head"); targetPositions.Add(p); targetWeights.Add(5.0f); }
            if (TryGetControllerPoint(target, out p, "chestControl", "chest")) { targetLabels.Add("Target Chest"); targetPositions.Add(p); targetWeights.Add(2.0f); }
            if (TryGetTargetBellyPoint(target, out p)) { targetLabels.Add("Target Belly"); targetPositions.Add(p); targetWeights.Add(2.0f); }
            if (TryGetTargetHipPoint(target, out p)) { targetLabels.Add("Target Hip"); targetPositions.Add(p); targetWeights.Add(2.0f); }
            targetLabels.Add("Free Hand"); targetPositions.Add(Vector3.zero); targetWeights.Add(1.0f);
        }

        bool preferSelf = UnityEngine.Random.Range(0.0f, 100.0f) < Mathf.Clamp(SafeFloat(coverSelfPercent, DefaultCoverSelfPercent), 0.0f, 100.0f);

        if (preferSelf && TryPickFromWeightedList(selfLabels, selfPositions, selfWeights, out pos, out label))
        {
            Log("Cover target group / Self / self%=" + SafeFloat(coverSelfPercent, DefaultCoverSelfPercent).ToString("F0", CultureInfo.InvariantCulture)
                + " / weighted=head5 free1");
            return true;
        }
        if (!preferSelf && TryPickFromWeightedList(targetLabels, targetPositions, targetWeights, out pos, out label))
        {
            Log("Cover target group / Target / self%=" + SafeFloat(coverSelfPercent, DefaultCoverSelfPercent).ToString("F0", CultureInfo.InvariantCulture)
                + " / weighted=head5 free1");
            return true;
        }

        // Fallback to the other group when the preferred side has no point.
        if (TryPickFromWeightedList(selfLabels, selfPositions, selfWeights, out pos, out label))
        {
            Log("Cover target group fallback / Self / weighted=head5 free1");
            return true;
        }
        if (TryPickFromWeightedList(targetLabels, targetPositions, targetWeights, out pos, out label))
        {
            Log("Cover target group fallback / Target / weighted=head5 free1");
            return true;
        }

        pos = Vector3.zero;
        label = "<none>";
        return false;
    }

    bool TryPickFromWeightedList(List<string> labels, List<Vector3> positions, List<float> weights, out Vector3 pos, out string label)
    {
        if (labels == null || positions == null || weights == null || positions.Count == 0 || labels.Count != positions.Count || weights.Count != positions.Count)
        {
            pos = Vector3.zero;
            label = "<none>";
            return false;
        }

        float total = 0.0f;
        for (int i = 0; i < weights.Count; i++) total += Mathf.Max(0.0f, weights[i]);
        if (total <= 0.001f)
        {
            pos = Vector3.zero;
            label = "<none>";
            return false;
        }

        float roll = UnityEngine.Random.Range(0.0f, total);
        float acc = 0.0f;
        for (int i = 0; i < weights.Count; i++)
        {
            acc += Mathf.Max(0.0f, weights[i]);
            if (roll <= acc)
            {
                pos = positions[i];
                label = labels[i];
                return true;
            }
        }

        pos = positions[positions.Count - 1];
        label = labels[labels.Count - 1];
        return true;
    }

    bool TryPickFromList(List<string> labels, List<Vector3> positions, out Vector3 pos, out string label)
    {
        if (labels == null || positions == null || positions.Count == 0 || labels.Count != positions.Count)
        {
            pos = Vector3.zero;
            label = "<none>";
            return false;
        }
        int idx = UnityEngine.Random.Range(0, positions.Count);
        pos = positions[idx];
        label = labels[idx];
        return true;
    }

    bool TryGetSelfChestPoint(out Vector3 pos)
    {
        if (chestControl != null)
        {
            pos = GetControllerPosition(chestControl);
            return true;
        }
        pos = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.position + Vector3.up * 1.15f : Vector3.zero;
        return containingAtom != null;
    }

    bool TryGetSelfHeadPoint(out Vector3 pos)
    {
        if (headControl != null)
        {
            pos = GetControllerPosition(headControl);
            return true;
        }
        pos = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.position + Vector3.up * 1.55f : Vector3.zero;
        return containingAtom != null;
    }

    bool TryGetSelfBellyPoint(out Vector3 pos)
    {
        if (TryGetControllerPoint(containingAtom, out pos, "abdomenControl", "abdomen", "stomachControl", "stomach")) return true;
        Vector3 chest;
        Vector3 hip;
        if (TryGetSelfChestPoint(out chest) && TryGetSelfHipPoint(out hip))
        {
            pos = Vector3.Lerp(hip, chest, 0.45f);
            return true;
        }
        if (containingAtom != null && containingAtom.transform != null)
        {
            pos = containingAtom.transform.position + Vector3.up * 1.05f;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }

    bool TryGetSelfHipPoint(out Vector3 pos)
    {
        if (hipControl != null)
        {
            pos = GetControllerPosition(hipControl);
            return true;
        }
        if (TryGetControllerPoint(containingAtom, out pos, "hipControl", "hip", "pelvisControl", "pelvis")) return true;
        if (containingAtom != null && containingAtom.transform != null)
        {
            pos = containingAtom.transform.position + Vector3.up * 0.90f;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }

    bool TryGetTargetBellyPoint(Atom atom, out Vector3 pos)
    {
        if (TryGetControllerPoint(atom, out pos, "abdomenControl", "abdomen", "stomachControl", "stomach")) return true;
        Vector3 chest;
        Vector3 hip;
        if (TryGetControllerPoint(atom, out chest, "chestControl", "chest") && TryGetTargetHipPoint(atom, out hip))
        {
            pos = Vector3.Lerp(hip, chest, 0.45f);
            return true;
        }
        if (atom != null && atom.transform != null)
        {
            pos = atom.transform.position + Vector3.up * 1.05f;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }

    bool TryGetTargetHipPoint(Atom atom, out Vector3 pos)
    {
        if (TryGetControllerPoint(atom, out pos, "hipControl", "hip", "pelvisControl", "pelvis")) return true;
        if (atom != null && atom.transform != null)
        {
            pos = atom.transform.position + Vector3.up * 0.90f;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }

    bool TryGetPersonLookPoint(Atom atom, out Vector3 pos)
    {
        if (TryGetControllerPoint(atom, out pos, "headControl", "head")) return true;
        if (TryGetControllerPoint(atom, out pos, "chestControl", "chest")) return true;
        if (atom != null && atom.transform != null)
        {
            pos = atom.transform.position + Vector3.up * 1.45f;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }

    bool TryGetControllerPoint(Atom atom, out Vector3 pos, params string[] aliases)
    {
        FreeControllerV3 fc = FindControllerByAliasesOnAtom(atom, aliases);
        if (fc != null)
        {
            pos = GetControllerPosition(fc);
            return true;
        }
        pos = Vector3.zero;
        return false;
    }

    void StopLifeGesture(string reason)
    {
        if (lifeGestureRoutine != null)
        {
            try { StopCoroutine(lifeGestureRoutine); } catch { }
            lifeGestureRoutine = null;
            Log("Stop life gesture / reason=" + reason);
        }

        if (IsPoseChangeSafeOn() && reason != null && reason.IndexOf("pose-change", StringComparison.OrdinalIgnoreCase) >= 0)
            RestoreActiveGestureStateOnly();
        else
            RestoreActiveGestureSnapshots();

        ScheduleNextGesture("stop:" + reason);
    }

    void StopAllLife(string reason)
    {
        StopLifeGesture(reason);
        StopBreathLoop(reason);
        StopLegBaseLoop(reason);
        UpdateStatus("Stopped / restored");
    }

    public void OnDestroy()
    {
        try { StopAllLife("destroy"); } catch { }
    }

    bool IsPoseChangeSafeOn()
    {
        return poseChangeSafe == null || poseChangeSafe.val;
    }

    bool ControllerExternallyMoved(FreeControllerV3 fc, Vector3 expectedPos, Quaternion expectedRot)
    {
        if (fc == null) return false;
        float posDelta = Vector3.Distance(GetControllerPosition(fc), expectedPos);
        float rotDelta = Quaternion.Angle(GetControllerRotation(fc), expectedRot);
        return posDelta > PoseChangePositionThreshold || rotDelta > PoseChangeRotationThreshold;
    }

    bool ControllerExternallyMovedPositionOnly(FreeControllerV3 fc, Vector3 expectedPos)
    {
        if (fc == null) return false;
        float posDelta = Vector3.Distance(GetControllerPosition(fc), expectedPos);
        return posDelta > PoseChangePositionThreshold;
    }

    Vector3 BuildLooseReturnPosition(Vector3 original, Vector3 gestureStart, FreeControllerV3 hand)
    {
        Vector3 right = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.right : Vector3.right;
        Vector3 up = Vector3.up;
        float side = (lHandControl != null && hand == lHandControl) ? -1.0f : 1.0f;
        if (rHandControl != null && hand == rHandControl) side = 1.0f;
        float amount = CoverLooseReturnNearAmount * Mathf.Clamp(MotionScale(), 0.6f, 1.8f);
        Vector3 drift = right * side * UnityEngine.Random.Range(-amount, amount) + up * UnityEngine.Random.Range(-amount * 0.35f, amount * 0.55f);
        return original + drift;
    }

    void UpdateControllerSnapshotPose(ControllerSnapshot snap, Vector3 pos, Quaternion rot)
    {
        if (snap == null) return;
        snap.position = pos;
        snap.rotation = rot;
        if (snap.controller != null)
        {
            try { snap.positionState = snap.controller.currentPositionState; } catch { }
            try { snap.rotationState = snap.controller.currentRotationState; } catch { }
        }
    }

    void AbortGestureForPoseChange(ControllerSnapshot primary, ControllerSnapshot secondary, string reason)
    {
        RestoreControllerStateOnly(primary);
        RestoreControllerStateOnly(secondary);
        RestoreControllerStateOnly(activeEyeSnapshot);
        activeLookSnapshot = null;
        activeEyeSnapshot = null;
        ClearLifeLockForSnapshot(activeCoverHandSnapshot);
        ClearLifeLockForSnapshot(activeCoverElbowSnapshot);
        activeCoverHandSnapshot = null;
        activeCoverElbowSnapshot = null;
        activeLegLeftSnapshot = null;
        activeLegRightSnapshot = null;
        lifeGestureRoutine = null;
        UpdateStatus("Pose changed: Life gesture aborted");
        ScheduleNextGesture("pose-change:" + reason);
        Log("Pose change safe / abort gesture / reason=" + reason);
    }

    void RestoreActiveGestureSnapshots()
    {
        RestoreController(activeLookSnapshot);
        RestoreController(activeEyeSnapshot);
        RestoreController(activeCoverHandSnapshot);
        RestoreController(activeCoverElbowSnapshot);
        ClearLifeLockForSnapshot(activeCoverHandSnapshot);
        ClearLifeLockForSnapshot(activeCoverElbowSnapshot);
        RestoreController(activeLegLeftSnapshot);
        RestoreController(activeLegRightSnapshot);
        activeLookSnapshot = null;
        activeEyeSnapshot = null;
        activeCoverHandSnapshot = null;
        activeCoverElbowSnapshot = null;
        activeLegLeftSnapshot = null;
        activeLegRightSnapshot = null;
    }

    void RestoreActiveGestureStateOnly()
    {
        RestoreControllerStateOnly(activeLookSnapshot);
        RestoreControllerStateOnly(activeEyeSnapshot);
        RestoreControllerStateOnly(activeCoverHandSnapshot);
        RestoreControllerStateOnly(activeCoverElbowSnapshot);
        ClearLifeLockForSnapshot(activeCoverHandSnapshot);
        ClearLifeLockForSnapshot(activeCoverElbowSnapshot);
        RestoreControllerStateOnly(activeLegLeftSnapshot);
        RestoreControllerStateOnly(activeLegRightSnapshot);
        activeLookSnapshot = null;
        activeEyeSnapshot = null;
        activeCoverHandSnapshot = null;
        activeCoverElbowSnapshot = null;
        activeLegLeftSnapshot = null;
        activeLegRightSnapshot = null;
    }

    void RestoreActiveGestureForHbaHandoff()
    {
        // Look/eye and leg one-shot gestures are not the lock source, so restore state flags only.
        RestoreControllerStateOnly(activeLookSnapshot);
        RestoreControllerStateOnly(activeEyeSnapshot);
        RestoreControllerStateOnly(activeLegLeftSnapshot);
        RestoreControllerStateOnly(activeLegRightSnapshot);

        // RandomCover handoff is different: do not restore the previous hand PositionState.On,
        // because that can keep the Life-midpoint as the new IK target. Release softly instead.
        ReleaseLifeLockForHbaHandoff(activeCoverHandSnapshot, true);
        ReleaseLifeLockForHbaHandoff(activeCoverElbowSnapshot, false);

        activeLookSnapshot = null;
        activeEyeSnapshot = null;
        activeCoverHandSnapshot = null;
        activeCoverElbowSnapshot = null;
        activeLegLeftSnapshot = null;
        activeLegRightSnapshot = null;
    }

    void AcquireLifeLock(FreeControllerV3 ctrl, bool isHand, string label, ControllerSnapshot snap)
    {
        if (ctrl == null || snap == null) return;
        LifeLockState st = GetLifeLockStateForController(ctrl, isHand);
        if (st == null) return;
        st.ownedByLife = true;
        st.isHand = isHand;
        st.label = string.IsNullOrEmpty(label) ? "Life" : label;
        st.controller = ctrl;
        st.snapshot = snap;
        UpdateLifeLockStatus();
        Log("Life lock acquire / " + st.name + " [" + st.label + "] / posState=" + snap.positionState + " / rotState=" + snap.rotationState);
    }

    LifeLockState GetLifeLockStateForController(FreeControllerV3 ctrl, bool isHand)
    {
        if (ctrl == null) return null;
        if (isHand)
        {
            if (lHandControl != null && ctrl == lHandControl) return lifeLHandLock;
            if (rHandControl != null && ctrl == rHandControl) return lifeRHandLock;
        }
        else
        {
            if (lElbowControl != null && ctrl == lElbowControl) return lifeLElbowLock;
            if (rElbowControl != null && ctrl == rElbowControl) return lifeRElbowLock;
        }
        return null;
    }

    void ClearLifeLockForController(FreeControllerV3 ctrl)
    {
        if (ctrl == null) return;
        bool changed = false;
        LifeLockState[] locks = new LifeLockState[] { lifeLHandLock, lifeRHandLock, lifeLElbowLock, lifeRElbowLock };
        for (int i = 0; i < locks.Length; i++)
        {
            LifeLockState st = locks[i];
            if (st != null && st.ownedByLife && st.controller == ctrl)
            {
                Log("Life lock clear / " + st.name + " [" + st.label + "]");
                st.Clear();
                changed = true;
            }
        }
        if (changed) UpdateLifeLockStatus();
    }

    void ClearLifeLockForSnapshot(ControllerSnapshot snap)
    {
        if (snap == null) return;
        ClearLifeLockForController(snap.controller);
    }

    void ReleaseLifeLockForHbaHandoff(ControllerSnapshot snap, bool isHand)
    {
        if (snap == null || snap.controller == null) return;
        LifeLockState st = GetLifeLockStateForController(snap.controller, isHand);
        if (st == null || !st.ownedByLife)
        {
            Log("HBA handoff release skipped / not-owned-by-life / controller=" + (snap.controller != null ? snap.controller.name : "<none>") + " / role=" + (isHand ? "hand" : "elbow"));
            return;
        }

        try { snap.controller.currentPositionState = FreeControllerV3.PositionState.Comply; } catch { }
        try { snap.controller.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        Log("HBA handoff release / " + st.name + " [" + st.label + "] / posState=Comply / rotState=Off");
        st.Clear();
        UpdateLifeLockStatus();
    }

    string FormatLifeLock(LifeLockState st)
    {
        if (st == null || !st.ownedByLife) return "-";
        return "Locked [" + (string.IsNullOrEmpty(st.label) ? "Life" : st.label) + "]";
    }

    string LockTargetLabel(LifeLockState st)
    {
        if (st == null || !st.ownedByLife) return "-";
        return string.IsNullOrEmpty(st.label) ? "Life" : st.label;
    }

    void UpdateLifeLockStatus()
    {
        // UI display was removed in v035.
        // Keep this method as a no-op because ownership changes still call it.
    }

    void ScheduleNextGesture(string reason)
    {
        float min;
        float max;
        if (reason != null && reason.IndexOf("pose-change", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            min = PoseChangeCooldownMin;
            max = PoseChangeCooldownMax;
        }
        else
        {
            GetEffectiveInterval(out min, out max);
        }
        nextGestureTime = Time.time + UnityEngine.Random.Range(min, max);
        Log("Next life gesture / in=" + (nextGestureTime - Time.time).ToString("F1", CultureInfo.InvariantCulture) + " / reason=" + reason);
    }

    void ScheduleNextGestureSoon(string reason, float min, float max)
    {
        min = Mathf.Max(0.0f, min);
        max = Mathf.Max(min, max);
        nextGestureTime = Time.time + UnityEngine.Random.Range(min, max);
        Log("Next life gesture soon / in=" + (nextGestureTime - Time.time).ToString("F2", CultureInfo.InvariantCulture) + " / reason=" + reason);
    }

    ControllerSnapshot CaptureController(FreeControllerV3 fc)
    {
        if (fc == null) return null;
        ControllerSnapshot snap = new ControllerSnapshot();
        snap.controller = fc;
        snap.position = GetControllerPosition(fc);
        snap.rotation = GetControllerRotation(fc);
        try { snap.positionState = fc.currentPositionState; } catch { snap.positionState = FreeControllerV3.PositionState.Off; }
        try { snap.rotationState = fc.currentRotationState; } catch { snap.rotationState = FreeControllerV3.RotationState.Off; }
        return snap;
    }

    void RestoreController(ControllerSnapshot snap)
    {
        if (snap == null || snap.controller == null) return;
        SetControllerPosition(snap.controller, snap.position);
        SetControllerRotation(snap.controller, snap.rotation);
        try { snap.controller.currentPositionState = snap.positionState; } catch { }
        try { snap.controller.currentRotationState = snap.rotationState; } catch { }
    }

    void RestoreControllerStateOnly(ControllerSnapshot snap)
    {
        if (snap == null || snap.controller == null) return;
        try { snap.controller.currentPositionState = snap.positionState; } catch { }
        try { snap.controller.currentRotationState = snap.rotationState; } catch { }
    }

    Atom GetSelectedTargetPerson()
    {
        if (targetPersonChooser == null) return FindNearestOtherPersonAtom();
        string selected = targetPersonChooser.val;
        if (string.IsNullOrEmpty(selected) || selected == TargetAutoOtherPerson)
            return FindNearestOtherPersonAtom();

        List<Atom> atoms = SuperController.singleton != null ? SuperController.singleton.GetAtoms() : null;
        if (atoms == null) return null;
        for (int i = 0; i < atoms.Count; i++)
        {
            Atom atom = atoms[i];
            if (atom != null && atom.type == "Person" && atom.name == selected) return atom;
        }
        return FindNearestOtherPersonAtom();
    }

    void RefreshTargetPersonChoices(bool updateChooser)
    {
        targetPersonChoices.Clear();
        targetPersonChoices.Add(TargetAutoOtherPerson);

        if (SuperController.singleton != null)
        {
            List<Atom> atoms = SuperController.singleton.GetAtoms();
            if (atoms != null)
            {
                for (int i = 0; i < atoms.Count; i++)
                {
                    Atom atom = atoms[i];
                    if (atom == null || atom == containingAtom || atom.type != "Person") continue;
                    if (!targetPersonChoices.Contains(atom.name)) targetPersonChoices.Add(atom.name);
                }
            }
        }

        if (updateChooser && targetPersonChooser != null)
        {
            string old = targetPersonChooser.val;
            targetPersonChooser.choices = new List<string>(targetPersonChoices);
            if (!targetPersonChoices.Contains(old)) old = GetDefaultTargetPersonChoice();
            targetPersonChooser.val = old;
        }
    }

    string GetDefaultTargetPersonChoice()
    {
        Atom atom = FindNearestOtherPersonAtom();
        if (atom != null && !string.IsNullOrEmpty(atom.name)) return atom.name;
        return TargetAutoOtherPerson;
    }

    Atom FindNearestOtherPersonAtom()
    {
        if (SuperController.singleton == null || containingAtom == null || containingAtom.transform == null)
            return null;

        Atom best = null;
        float bestDistanceSqr = float.MaxValue;
        Vector3 selfPos = containingAtom.transform.position;
        List<Atom> atoms = SuperController.singleton.GetAtoms();
        if (atoms == null) return null;

        for (int i = 0; i < atoms.Count; i++)
        {
            Atom atom = atoms[i];
            if (atom == null || atom == containingAtom || atom.type != "Person" || atom.transform == null)
                continue;

            float d = (atom.transform.position - selfPos).sqrMagnitude;
            if (d < bestDistanceSqr)
            {
                bestDistanceSqr = d;
                best = atom;
            }
        }

        return best;
    }

    FreeControllerV3 FindControllerByAliases(params string[] aliases)
    {
        return FindControllerByAliasesOnAtom(containingAtom, aliases);
    }

    FreeControllerV3 FindControllerByAliasesOnAtom(Atom atom, params string[] aliases)
    {
        if (atom == null || aliases == null) return null;

        for (int i = 0; i < aliases.Length; i++)
        {
            FreeControllerV3 found = FindControllerExactOnly(atom, aliases[i]);
            if (found != null) return found;
        }

        for (int i = 0; i < aliases.Length; i++)
        {
            FreeControllerV3 found = FindControllerContains(atom, aliases[i]);
            if (found != null) return found;
        }

        return null;
    }

    FreeControllerV3 FindControllerExactOnly(Atom atom, string controllerName)
    {
        if (atom == null || atom.freeControllers == null || string.IsNullOrEmpty(controllerName)) return null;
        for (int i = 0; i < atom.freeControllers.Length; i++)
        {
            FreeControllerV3 fc = atom.freeControllers[i];
            if (fc != null && string.Equals(fc.name, controllerName, StringComparison.OrdinalIgnoreCase)) return fc;
        }
        return null;
    }

    FreeControllerV3 FindControllerContains(Atom atom, string controllerName)
    {
        if (atom == null || atom.freeControllers == null || string.IsNullOrEmpty(controllerName)) return null;
        string lowered = controllerName.ToLowerInvariant();
        for (int i = 0; i < atom.freeControllers.Length; i++)
        {
            FreeControllerV3 fc = atom.freeControllers[i];
            if (fc != null && fc.name != null && fc.name.ToLowerInvariant().Contains(lowered)) return fc;
        }
        return null;
    }

    Vector3 GetControllerPosition(FreeControllerV3 fc)
    {
        if (fc == null) return Vector3.zero;
        return fc.control != null ? fc.control.position : fc.transform.position;
    }

    Quaternion GetControllerRotation(FreeControllerV3 fc)
    {
        if (fc == null) return Quaternion.identity;
        return fc.control != null ? fc.control.rotation : fc.transform.rotation;
    }

    void SetControllerPosition(FreeControllerV3 fc, Vector3 pos)
    {
        if (fc == null) return;
        fc.transform.position = pos;
        if (fc.control != null) fc.control.position = pos;
    }

    void SetControllerRotation(FreeControllerV3 fc, Quaternion rot)
    {
        if (fc == null) return;
        fc.transform.rotation = rot;
        if (fc.control != null) fc.control.rotation = rot;
    }


    void ApplyEyeTargetToward(Vector3 targetPos, float lerp)
    {
        if (eyeTargetControl == null) return;
        lerp = Mathf.Clamp01(lerp);
        Vector3 current = GetControllerPosition(eyeTargetControl);
        Vector3 desired = targetPos;
        SetControllerPosition(eyeTargetControl, Vector3.Lerp(current, desired, lerp));
    }

    Quaternion ApplyLocalRoll(Quaternion baseRot, float degrees)
    {
        if (Mathf.Abs(degrees) < 0.001f) return baseRot;
        return baseRot * Quaternion.AngleAxis(degrees, Vector3.forward);
    }

    float Smooth01(float t)
    {
        t = Mathf.Clamp01(t);
        return t * t * (3.0f - 2.0f * t);
    }

    float Smoother01(float t)
    {
        t = Mathf.Clamp01(t);
        return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
    }

    Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        t = Mathf.Clamp01(t);
        float u = 1.0f - t;
        return (u * u * a) + (2.0f * u * t * b) + (t * t * c);
    }

    Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        t = Mathf.Clamp01(t);
        float u = 1.0f - t;
        return (u * u * u * a) + (3.0f * u * u * t * b) + (3.0f * u * t * t * c) + (t * t * t * d);
    }

    Vector3 BuildSoftCoverArc(Vector3 start, Vector3 goal, FreeControllerV3 hand)
    {
        Vector3 up = Vector3.up;
        Vector3 right = Vector3.right;
        if (containingAtom != null && containingAtom.transform != null)
        {
            up = containingAtom.transform.up;
            right = containingAtom.transform.right;
        }
        float handSide = 0.0f;
        if (lHandControl != null && hand == lHandControl) handSide = -1.0f;
        else if (rHandControl != null && hand == rHandControl) handSide = 1.0f;
        else handSide = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;

        float dist = Vector3.Distance(start, goal);
        float scale = Mathf.Clamp01(dist / 0.45f) * MotionScale();
        if (IsCover100Mode()) scale *= 0.75f; // keep cover100 frequent, but do not make each path too snappy.
        return (up * CoverSoftArcUp + right * CoverSoftArcSide * handSide) * scale;
    }



    bool IsHeadLookEnabled()
    {
        return lifeHeadLookEnabled == null || lifeHeadLookEnabled.val;
    }

    bool IsLegMotionEnabled()
    {
        return lifeLegMotionEnabled != null && lifeLegMotionEnabled.val;
    }

    float EffectiveLegMotionWeight()
    {
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return GestureLegMotionWeight * 0.55f;
        if (mode == LifeMotionLarge) return GestureLegMotionWeight * 1.35f;
        return GestureLegMotionWeight;
    }

    float EffectiveLegRotationDegrees()
    {
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return UnityEngine.Random.Range(0.8f, 1.8f);
        if (mode == LifeMotionLarge) return UnityEngine.Random.Range(2.8f, 5.0f);
        return UnityEngine.Random.Range(1.6f, 3.2f);
    }

    string CurrentMotionMode()
    {
        if (lifeMotionMode == null || string.IsNullOrEmpty(lifeMotionMode.val)) return LifeMotionNormal;
        string v = lifeMotionMode.val;
        if (string.Equals(v, LifeMotionSmall, StringComparison.OrdinalIgnoreCase)) return LifeMotionSmall;
        if (string.Equals(v, LifeMotionLarge, StringComparison.OrdinalIgnoreCase)) return LifeMotionLarge;
        return LifeMotionNormal;
    }

    float MotionScale()
    {
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 0.65f;
        if (mode == LifeMotionLarge) return 1.35f;
        return 1.00f;
    }

    float EffectiveBreathAmount()
    {
        string mode = CurrentMotionMode();
        float baseAmount = 0.0100f;
        if (mode == LifeMotionSmall) baseAmount = 0.0060f;
        else if (mode == LifeMotionLarge) baseAmount = 0.0150f;
        return baseAmount * Mathf.Max(0.0f, SafeFloat(breathScale, DefaultBreathScale));
    }

    float EffectiveLookTargetHoldMin()
    {
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return LookTargetHoldSmallMin;
        if (mode == LifeMotionLarge) return LookTargetHoldLargeMin;
        return LookTargetHoldNormalMin;
    }

    float EffectiveLookTargetHoldMax()
    {
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return LookTargetHoldSmallMax;
        if (mode == LifeMotionLarge) return LookTargetHoldLargeMax;
        return LookTargetHoldNormalMax;
    }

    float EffectiveLookMaxAngle()
    {
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 35.0f;
        if (mode == LifeMotionLarge) return 180.0f;
        return 90.0f;
    }

    float EffectiveLookAwayVerticalMin()
    {
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return -0.22f;
        if (mode == LifeMotionLarge) return -0.65f;
        return -0.42f;
    }

    float EffectiveLookAwayVerticalMax()
    {
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 0.28f;
        if (mode == LifeMotionLarge) return 0.75f;
        return 0.50f;
    }

    float EffectiveBreathBodySwayAmount()
    {
        string mode = CurrentMotionMode();
        float baseAmount = 0.0080f;
        if (mode == LifeMotionSmall) baseAmount = 0.0040f;
        else if (mode == LifeMotionLarge) baseAmount = 0.0120f;
        return baseAmount * Mathf.Max(0.0f, SafeFloat(breathScale, DefaultBreathScale));
    }

    float EffectiveBreathRotationDegrees()
    {
        string mode = CurrentMotionMode();
        float baseDegrees = 1.00f;
        if (mode == LifeMotionSmall) baseDegrees = 0.55f;
        else if (mode == LifeMotionLarge) baseDegrees = 1.45f;
        return baseDegrees * Mathf.Max(0.0f, SafeFloat(breathScale, DefaultBreathScale));
    }

    bool IsCover100Mode()
    {
        return randomCoverEnabled != null && randomCoverEnabled.val && SafeFloat(coverFrequency, DefaultCoverFrequency) >= 99.5f;
    }

    float EffectiveCoverPrepareSeconds()
    {
        if (!IsCover100Mode()) return CoverPrepareSeconds;
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 0.14f;
        if (mode == LifeMotionLarge) return 0.10f;
        return 0.12f;
    }

    float EffectiveCoverMoveSeconds()
    {
        if (!IsCover100Mode()) return CoverMoveSeconds;
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 0.70f;
        if (mode == LifeMotionLarge) return 0.58f;
        return 0.64f;
    }

    float EffectiveCoverHoldMinSeconds()
    {
        if (!IsCover100Mode()) return CoverHoldSecondsMin;
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 0.55f;
        if (mode == LifeMotionLarge) return 0.34f;
        return 0.44f;
    }

    float EffectiveCoverHoldMaxSeconds()
    {
        if (!IsCover100Mode()) return CoverHoldSecondsMax;
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 1.10f;
        if (mode == LifeMotionLarge) return 0.72f;
        return 0.90f;
    }

    float EffectiveCoverReturnSeconds()
    {
        if (!IsCover100Mode()) return CoverReturnSeconds;
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 1.05f;
        if (mode == LifeMotionLarge) return 0.82f;
        return 0.94f;
    }

    float EffectiveCoverMaxDistance()
    {
        string mode = CurrentMotionMode();
        // v024: Life cover is allowed to make a visible effort toward far targets.
        // It still clamps when the target is unreachable, but the clamp is long enough
        // for Head/Chest/Belly/Hip reactions to read clearly.
        if (IsCover100Mode())
        {
            if (mode == LifeMotionSmall) return 0.62f;
            if (mode == LifeMotionLarge) return 1.00f;
            return 0.82f;
        }
        if (mode == LifeMotionSmall) return 0.42f;
        if (mode == LifeMotionLarge) return 0.78f;
        return DefaultCoverMaxDistance;
    }

    void GetEffectiveInterval(out float min, out float max)
    {
        string mode = CurrentMotionMode();
        if (IsCover100Mode())
        {
            // v010: Cover 100 is a deliberate stress/visibility mode.
            if (mode == LifeMotionSmall) { min = 1.0f; max = 1.8f; return; }
            if (mode == LifeMotionLarge) { min = 0.25f; max = 0.65f; return; }
            min = 0.45f; max = 1.00f; return;
        }
        if (mode == LifeMotionSmall)
        {
            min = 6.0f;
            max = 14.0f;
            return;
        }
        if (mode == LifeMotionLarge)
        {
            min = 3.0f;
            max = 7.0f;
            return;
        }
        min = DefaultIntervalMin;
        max = DefaultIntervalMax;
    }

    float EffectiveLegBaseRotationDegrees()
    {
        string mode = CurrentMotionMode();
        // v028: v027 was too subtle in many poses because thigh rotation alone was masked by knee/foot IK.
        // Make scale=5 clearly visible while keeping scale=1 usable as a small base motion.
        float baseAmount = 2.40f;
        if (mode == LifeMotionSmall) baseAmount = 1.35f;
        else if (mode == LifeMotionLarge) baseAmount = 4.20f;
        float scale = Mathf.Clamp(SafeFloat(legScale, DefaultLegScale), 0.0f, 5.0f);
        return baseAmount * scale;
    }

    float EffectiveLegBasePositionAmount()
    {
        string mode = CurrentMotionMode();
        // Tiny thigh-control position assist; this is why the leg base motion remains visible even when rotation IK is visually damped.
        float baseAmount = 0.010f;
        if (mode == LifeMotionSmall) baseAmount = 0.005f;
        else if (mode == LifeMotionLarge) baseAmount = 0.018f;
        float scale = Mathf.Clamp(SafeFloat(legScale, DefaultLegScale), 0.0f, 5.0f);
        return baseAmount * scale;
    }

    float EffectiveLegBaseCycleSeconds()
    {
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 4.20f;
        if (mode == LifeMotionLarge) return 2.85f;
        return 3.45f;
    }

    float SafeFloat(JSONStorableFloat f, float fallback)
    {
        if (f == null) return fallback;
        return f.val;
    }

    string GetHandLabel(FreeControllerV3 hand)
    {
        if (hand == null || hand.name == null) return "<none>";
        string n = hand.name.ToLowerInvariant();
        if (n.Contains("lhand")) return "L Hand";
        if (n.Contains("rhand")) return "R Hand";
        return hand.name;
    }

    string SafeAtomName(Atom atom)
    {
        if (atom == null) return "<none>";
        if (!string.IsNullOrEmpty(atom.uid)) return atom.uid;
        if (!string.IsNullOrEmpty(atom.name)) return atom.name;
        return "<atom>";
    }

    void UpdateStatus(string message)
    {
        UpdateLifeLockStatus();
        if (statusText == null) return;
        float nextIn = nextGestureTime > 0.0f ? Mathf.Max(0.0f, nextGestureTime - Time.time) : 0.0f;
        statusText.val = "HumanLifeAction / " + message
            + " / motion=" + CurrentMotionMode()
            + " / breath=" + (breathLoopRoutine != null ? "ON" : "OFF")
            + " / headLook=" + (IsHeadLookEnabled() ? "ON" : "OFF")
            + " / leg=" + (IsLegMotionEnabled() ? "ON" : "OFF")
            + " / legScale=" + SafeFloat(legScale, DefaultLegScale).ToString("F1", CultureInfo.InvariantCulture)
            + " / legBase=" + (legBaseLoopRoutine != null ? "ON" : "OFF")
            + " / hbaPause=" + (legPausedByHba ? "ON" : "OFF")
            + " / last=" + lastGesture
            + " / next=" + nextIn.ToString("F1", CultureInfo.InvariantCulture) + "s";
    }

    void LogCover(string message)
    {
        if ((logCoverDetail != null && logCoverDetail.val) || (debugLog != null && debugLog.val))
            SuperController.LogMessage("[HumanLifeAction] " + message);
    }

    void Log(string message)
    {
        if (debugLog != null && debugLog.val)
            SuperController.LogMessage("[HumanLifeAction] " + message);
    }
}
