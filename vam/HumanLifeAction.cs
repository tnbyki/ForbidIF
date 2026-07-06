// HLA_V117_EXTERNAL_LIFE_IF_ALIASES: Adds AI-facing alias actions for HumanReceiver HUM_LIFE(state/expression/personality) routing while keeping v116 expression Sad default 0.60.
// HLA_V116_EXPRESSION_SAD_DEFAULT_060: Renames the expression-side Shy slot to Sad : しょんぼり, defaults Sad Morph to Sad with max 0.60, and keeps old Shy actions as compatibility aliases.
// HLA_V115_EXPRESSION_SHY_SAD_DEFAULT: Changes Shy expression built-in default morph to Sad with max 0.20 while keeping Like/Dislike/Shy under Life Expression.
// HLA_V114_EXPRESSION_LIKE_DISLIKE_SHY: Groups Like / Dislike / Shy under the expression combo (UI label Life Expression) instead of Life State/Personality, keeps old Shy actions as compatibility aliases to expression Shy.
// HLA_V113_SHY_LIFE_ACTION: Moves Shy from Life Personality to the Life State/Action combo as "Shy : 恥ずかしい" while keeping Shy Morph and compatibility actions.
// HLA_V111_AFFECTION_MORPH_FILTERABLE_POPUP: Removes unusable text search fields and uses VaM's CreateFilterablePopup for Like/Dislike morph choosers, so the popup itself can be typed/searched. Keeps defaults Like=Smile Full Face 0.90 / Dislike=Frown 0.90.
// HLA_V110_AFFECTION_MORPH_SEARCH_DEFAULTS: Adds Like/Dislike morph search fields, narrows each chooser independently, and sets built-in defaults Like=Smile Full Face 0.90 / Dislike=Frown 0.90.
// HLA_V109_AFFECTION_MORPH_CHOOSER: Replaces manual Like/Dislike Morph text input with popups populated from geometry float/morph parameters, plus a Refresh Affection Morphs button. Keeps max sliders and fade behavior from v108.
// HLA_V108_AFFECTION_MORPH_USER_SELECT: Adds user-entered Like/Dislike Morph names with max-value sliders and a fade slider. Life Affection drives the selected morph toward its max and restores HLA-touched morphs when returning to Neutral or the opposite affection.
// HLA_V107_LIFE_AFFECTION_LIKE_DISLIKE: Adds Life Affection combo (Neutral/Like/Dislike with Japanese labels) that biases target gaze, target cover, look-away, interval, and self-fidget behavior while keeping Sleeping/Quiet state rules dominant.
// HLA_V106_PERSONALITY_COMPILE_FIX: Initializes targetSuppressReason before mutual-back/self-only target-cover suppression so VaM/Mono compiles the Life Personality build.
// HLA_V105_LIFE_PERSONALITY_SHY_BOLD: Adds Life Personality combo (Normal/Shy/Bold with Japanese labels) that biases gaze, self/target cover, fidget weights, intervals, and body micro-motion while keeping Sleeping state dominant.
// HLA_V104_LIFE_STATE_JAPANESE_LABELS: Life State combo entries now include Japanese labels such as Sleeping : 寝ている while keeping legacy English values/actions compatible.
// HLA_V103_SLEEP_EYE_TRANSITION_SETTLE: Sleeping transition now slowly closes eyes and opens them on wake, keeps auto eye systems off during the transition, and briefly releases hands/elbows to Comply/Off for a natural sleep settle without restoring old IK targets.
// HLA_V100_SLEEPING_EYELIDCONTROL_COMPILE_FIX: Fixes VaM/Mono Type name collision in EyelidControl reflection scan by explicitly using System.Type; keeps v099 direct EyelidControl sleeping eye behavior.
// HLA_V102_SLEEP_QUIET_FIDGET: Makes Sleeping and Quiet more alive with self-only fidget/mogimogi behavior, stronger quiet breathing/shoulder/leg micro-motion, and higher Free/Self Hip cover weights while keeping no target gaze/cover for those states.
// HLA_V101_SLEEPING_NO_REFLECTION_MORE_MOTION: Removes prohibited System.Reflection access, keeps direct EyelidControl blinkEnabled/eyelidMorphsEnabled lookup, and makes Sleeping slightly more alive while remaining self-only/no target gaze.
// HLA_V099_SLEEPING_EYELIDCONTROL_DIRECT: Sleeping directly targets EyelidControl blinkEnabled / eyelidMorphsEnabled before fallback scanning, so Auto Systems > Eye Control checkboxes are actually disabled and restored.
// HLA_V098_SLEEPING_AUTO_EYE_SYSTEMS_OFF: Sleeping disables Auto Systems / Auto Blink plus Auto Eyelid Morphs when accessible, retries with reflection, and holds Eyes Closed every frame.
// HLA_V096_LIFE_STATE_SLEEP_EYES_SELF_ONLY: Sleeping closes eyes, blocks target/camera gaze, keeps only low random away/self motion, and Quiet/Sleeping cover only self body; other states open eyes.
// HLA_V095_LIFE_STATE_SIMPLE_UI: Adds Life State (Quiet/Normal/Active/Sleeping) to control motion level, hides confusing internal checkboxes/sliders from the visible UI while keeping them registered for compatibility.
// HLA_V094_SURFACE_BASE_MAINCONTROLLER: Body-surface RandomCover points now use the Person mainController as the base position, falling back to hipControl with hip-relative height before atom.transform, preventing Self/Target shoulder points from staying near the Atom origin after root/pose movement.
// HLA_V093_LEG_MOTION_COOPERATIVE: Life Leg Motion now defaults to rotation-only cooperative thigh sway, leaves thigh PositionState untouched unless optional Position Assist is enabled, and yields briefly when external pose/root motion moves the thighs.
// HLA_V091_SHOULDER_SWAY_BREATH: Adds optional Life Shoulder Sway driven by the Breath loop using small l/r elbow offsets to make breathing visible without touching hand IK or chest position; keeps v090 Breath Scale max 50 and v089 surface cover/return snap behavior.
// HLA_V090_BREATH_SCALE_MAX50_VISIBLE: Raises Life Breath Scale slider max from 10.0 to 50.0 so rotation-only breath can be visibly verified; keeps v089 surface cover and rotation-only breath behavior.
// HLA_V089_SURFACE_COVER_ROTATION_BREATH: RandomCover uses shoulder/upper-chest/belly/thigh body-surface points instead of Head/Chest/Hip IK-center targets; Life Breath is chest rotation-only and never restores chest position.
// HLA_V088_TARGET_COVER_RETURN_SNAP_BACK_GUARD: Target Cover still stretches toward the selected target point, but target-side final snap is skipped for Target* labels and the hand IK controller snaps back to the captured self hand position before restoring states; random Target Cover is suppressed for mutual-back facing.
// HLA_V087_LEG_BASE_DOCKING_HANDOFF_RELEASE: Life Leg Motion now also pauses on external TargetLinePerson Docking Pose Assist and releases Life-owned thigh base motion to Position=Comply / Rotation=Off during handoff, preventing thigh On residue after docking/pose interruption.
// HLA_V086_EXTERNAL_COVER_ACTIONS: Adds stable external Action aliases for HBA/HDU delegation: HLA_Cover_SelfHead and HLA_Cover_SelfHip, while keeping existing test/manual buttons.
// HLA_V085_SELF_HEAD_ELBOW_LOOSE_FINAL: Self Head/Face/Mouth keeps larger chest avoidance, but after the outward bypass the same-side elbow becomes loose/Comply at the final head touch so large chest poses do not pull the elbow inward.
// HLA_V084_HEAD_CHEST_AVOID_BIGGER_HEAD_FORWARD: Self Head/Face/Mouth chest avoidance is larger, tilted body poses get extra avoid boost, and Self Head cover point moves 0.040m forward.
// HLA_THIGH_SIDE_HAND_TEST_BUILD 2026-06-29: Adds isolated test buttons to move L/R hands to the outside of their matching self L/R thigh using the current thigh pair positions; does not touch Hip/Butt cover routing or chest-avoid cover logic.
// HLA_COVER_TEST_HEAD_HIP_BUTTONS_BUILD 2026-06-28: Adds direct test buttons/actions for Self Head, Self Hip, Target Head, and Target Hip cover routes so target point, snap, and visual reach can be verified without random selection.
// HLA_V083_SELF_HIP_STEP7_BACK009_ELBOW_FREE: Step7 thigh-mid-back offset is 0.090m and Hip path temporarily releases same-side elbow IK during the hand route.
// HLA_COVER_EXACT_TOUCH_SNAP_VERIFY_BUILD 2026-06-28: Non-free Cover targets now move and snap to the exact selected cover point instead of a loose/surface/near anchor, hold sway is reduced after snap, and verify logs show actual controller distance to the target point.
// HLA_COVER_NO_GIVEUP_STRETCH_SNAP_BUILD 2026-06-28: Cover targets no longer clamp/stop short by reach; all non-free cover targets command the IK to the final surface goal, then do a short final snap before hold. Keeps head weight 8, independent cover roll, self75/target25, head touch, and breast-size chest avoid.
// HLA_COVER_HEAD_WEIGHT8_BUILD 2026-06-28: Raises Life RandomCover Head target weight from 5 to 8 while keeping Self/Target split and independent Cover roll unchanged.
// HLA_COVER_SELF75_TARGET25_BUILD 2026-06-28: Defaults Life Cover Self % to 75 so RandomCover selects Self targets about 75% and Target targets about 25%.
// HLA_INDEPENDENT_COVER_ROLL_BUILD 2026-06-28: Makes Life Cover Frequency an independent percentage roll; Cover no longer shares one weighted pool with Look/None, and Look rolls only after Cover misses.
// HLA_HEAD_COVER_STRETCH_THEN_SNAP_BUILD 2026-06-28: Head cover targets now stretch farther and then snap the hand IK target to the exact head touch goal after the move phase, so Head does not stop short.
// HLA_HEAD_COVER_TOUCH_GOAL_BUILD 2026-06-28: Head cover targets use a much smaller head surface offset, exact head hold anchor, and reduced head hold sway so the selected hand actually reaches the head instead of hovering short.
// HLA_SELF_FACE_BREAST_SIZE_AVOID_BUILD 2026-06-28: Self Head RandomCover now measures self L/R nipple protrusion from chestControl; small/flat chest keeps the normal path, while large breast + chest-crossing hand-to-face line uses the outward/up avoid path.
// HLA_HEAD_COVER_REACH_FIX_BUILD 2026-06-28: Head cover targets now use a larger head-specific reach and exact head reach ratio so selected Self/Target Head actually reaches instead of stopping at the generic 0.58m clamp.
// HLA_SELF_FACE_CHEST_AVOID_PATH_BUILD 2026-06-28: Self Head RandomCover uses a chest-proximity line test and only routes hand outward/up when the hand-to-face path would cross the chest area.
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
    JSONStorableStringChooser lifeStateMode;
    JSONStorableStringChooser lifePersonalityMode;
    JSONStorableStringChooser lifeAffectionMode;
    JSONStorableStringChooser shyPersonalityMorphName;
    JSONStorableFloat shyPersonalityMorphMax;
    JSONStorableString likeAffectionMorphSearch;
    JSONStorableStringChooser likeAffectionMorphName;
    JSONStorableFloat likeAffectionMorphMax;
    JSONStorableString dislikeAffectionMorphSearch;
    JSONStorableStringChooser dislikeAffectionMorphName;
    JSONStorableFloat dislikeAffectionMorphMax;
    JSONStorableFloat affectionMorphFadeSeconds;
    JSONStorableBool breathEnabled;
    JSONStorableBool autoPauseBreathOnHbaActive;
    JSONStorableBool lifeHeadLookEnabled;
    JSONStorableBool lookTargetEnabled;
    JSONStorableBool lookCameraEnabled;
    JSONStorableBool randomCoverEnabled;
    JSONStorableBool lifeLegMotionEnabled;
    JSONStorableBool lifeLegPositionAssistEnabled;
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
    JSONStorableBool shoulderSwayEnabled;
    JSONStorableFloat shoulderSwayScale;
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
    readonly List<string> lifeStateChoices = new List<string>() { LifeStateQuiet, LifeStateNormal, LifeStateActive, LifeStateSleeping };
    readonly List<string> lifePersonalityChoices = new List<string>() { LifePersonalityNormal, LifePersonalityBold };
    readonly List<string> lifeAffectionChoices = new List<string>() { LifeAffectionNeutral, LifeAffectionLike, LifeAffectionDislike, LifeAffectionShy };
    readonly List<string> affectionMorphChoices = new List<string>() { AffectionMorphNone };
    readonly List<string> dislikeAffectionMorphChoices = new List<string>() { AffectionMorphNone };
    readonly List<string> shyPersonalityMorphChoices = new List<string>() { AffectionMorphNone };

    const string TargetAutoOtherPerson = "Auto Other Person";
    const string LifeStateQuiet = "Quiet : おとなしい";
    const string LifeStateShy = "Shy : 恥ずかしい";
    const string LifeStateNormal = "Normal : ふつう";
    const string LifeStateActive = "Active : 活発";
    const string LifeStateSleeping = "Sleeping : 寝ている";
    const string LifeStateQuietLegacy = "Quiet";
    const string LifeStateShyLegacy = "Shy";
    const string LifeStateNormalLegacy = "Normal";
    const string LifeStateActiveLegacy = "Active";
    const string LifeStateSleepingLegacy = "Sleeping";
    const string LifePersonalityNormal = "Normal : 標準";
    const string LifePersonalityShy = "Shy : 恥ずかしい";
    const string LifePersonalityBold = "Bold : 積極的";
    const string LifePersonalityNormalLegacy = "Normal";
    const string LifePersonalityShyLegacy = "Shy";
    const string LifePersonalityBoldLegacy = "Bold";
    const string LifeAffectionNeutral = "Neutral : ふつう";
    const string LifeAffectionLike = "Like : 好き";
    const string LifeAffectionDislike = "Dislike : 嫌い";
    const string LifeAffectionShy = "Sad : しょんぼり";
    const string LifeAffectionNeutralLegacy = "Neutral";
    const string LifeAffectionLikeLegacy = "Like";
    const string LifeAffectionDislikeLegacy = "Dislike";
    const string LifeAffectionShyLegacy = "Shy";
    const string LifeAffectionSadLegacy = "Sad";
    const string DefaultLikeAffectionMorphName = "Smile Full Face";
    const string DefaultDislikeAffectionMorphName = "Frown";
    const string DefaultShyPersonalityMorphName = "Sad";
    const string DefaultLikeAffectionMorphSearch = "smile";
    const string DefaultDislikeAffectionMorphSearch = "frown";
    const float DefaultLikeAffectionMorphMax = 0.90f;
    const float DefaultDislikeAffectionMorphMax = 0.90f;
    const float DefaultShyPersonalityMorphMax = 0.60f;
    const float DefaultAffectionMorphFadeSeconds = 1.00f;
    const string AffectionMorphNone = "None : なし";
    const string LifeMotionSmall = "Small";
    const string LifeMotionNormal = "Normal";
    const string LifeMotionLarge = "Large";
    const float DefaultIntervalMin = 4.0f;
    const float DefaultIntervalMax = 10.0f;
    const float DefaultLifeStrength = 1.0f;
    const float DefaultBreathAmount = 0.007f;
    const float DefaultBreathScale = 20.00f;
    const float DefaultShoulderSwayScale = 5.00f;
    const float DefaultLegScale = 1.00f;
    const float DefaultCoverFrequency = 90.0f;
    const float DefaultLookFrequency = 50.0f;
    const float DefaultCoverSelfPercent = 75.0f;
    const float DefaultLookTargetPercent = 50.0f;
    const float DefaultLookAwayPercent = 20.0f;
    const float DefaultLookMaxAngle = 90.0f;
    const float DefaultCoverMaxDistance = 0.58f;
    const float SelfHeadCoverMaxDistance = 1.25f;
    const float TargetHeadCoverMaxDistance = 1.75f;
    const float HeadCoverSnapSeconds = 0.12f;
    const float CoverFinalSnapSeconds = 0.16f;
    const float CoverReturnSnapSeconds = 0.12f;
    const float TargetCoverMutualBackSuppressDot = -0.35f;
    const float CoverTouchHoldSwayScale = 0.20f;
    const float HeadCoverSurfaceOffset = 0.012f;
    const float SelfHeadCoverPointForwardOffset = 0.040f;
    const float HeadCoverHoldDrift = 0.002f;
    const float HeadCoverSideDrift = 0.003f;
    const float HeadCoverHoldSwayScale = 0.25f;
    const float SelfHeadElbowFinalLooseBlend = 0.62f;
    const float GestureLegMotionWeight = 14.0f;
    const float HbaProgressPauseThreshold = 0.005f;
    const float HbaLegResumeDelaySeconds = 0.75f;
    const float LegExternalChangeResumeDelaySeconds = 0.75f;
    const float HbaBreathResumeDelaySeconds = 0.75f;
    const float HbaGestureResumeDelaySeconds = 0.45f;
    const float ExternalDockingPoseAssistPauseWindowSeconds = 7.00f; // TargetLinePerson 5s assist + 2s settle suppress
    const float ExternalDockingPoseAssistResolveInterval = 0.15f;

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
    const float ThighSideHandOffset = 0.100f;
    const float ThighSideUpOffset = 0.100f;
    const float ThighSideBackOffset = 0.100f;
    const float ThighSidePreHandOffset = 0.105f;   // step1
    const float ThighSidePreUpOffset = 0.300f;     // step1 / v078+
    const float ThighSidePreBackOffset = -0.100f;  // step1
    const float ThighSideStep2HandOffset = 0.100f; // step2
    const float ThighSideStep2UpOffset = 0.100f;
    const float ThighSideStep2BackOffset = 0.200f;
    const float ThighSideStep3HandOffset = 0.000f; // step3
    const float ThighSideStep3UpOffset = 0.100f;
    const float ThighSideStep3BackOffset = 0.200f;
    const float ThighSideStep4HandOffset = -0.100f; // step4
    const float ThighSideStep4UpOffset = 0.000f;
    const float ThighSideStep4BackOffset = 0.200f;
    const float ThighSideStep5HandOffset = -0.100f; // step5
    const float ThighSideStep5UpOffset = 0.100f;
    const float ThighSideStep5BackOffset = 0.200f;
    const float ThighSideStep6HandOffset = -0.200f; // step6
    const float ThighSideStep6UpOffset = 0.100f;
    const float ThighSideStep6BackOffset = 0.200f;
    const float ThighSideStep7MidBackOffset = 0.090f; // step7: L/R thigh 中央後ろ / v083
    const float ThighSideStep7MidUpOffset = 0.000f;
    const float ThighSideMoveSeconds = 0.35f;
    const float ThighSidePreMoveSeconds = 0.20f;
    const float ThighSideFinalMoveSeconds = 0.25f;
    const float ThighSideHoldSeconds = 0.80f;
    const float ThighSideReturnSeconds = 0.55f;
    const float CoverHoldSecondsMin = 0.85f;
    const float CoverHoldSecondsMax = 2.10f;
    const float CoverReturnSeconds = 1.45f;
    const float CoverSurfaceOffset = 0.055f;
    const float CoverSoftArcUp = 0.045f;
    const float CoverSoftArcSide = 0.030f;
    const float CoverSoftReturnArcScale = 0.55f;
    const float SelfFaceChestAvoidRadius = 0.24f;
    const float SelfFaceChestAvoidChestForwardOffset = 0.08f;
    const float SelfFaceChestAvoidSideOffset = 0.24f;
    const float SelfFaceChestAvoidUpOffset = 0.22f;
    const float SelfFaceChestAvoidForwardOffset = 0.075f;
    const float SelfFaceBreastAvoidProtrusionStart = 0.095f;
    const float SelfFaceBreastAvoidProtrusionFull = 0.180f;
    const float SelfFaceBreastAvoidFallbackRadius = 0.205f;
    const float SelfFaceBreastAvoidMaxRadius = 0.420f;
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

    private bool hipSmoothPathAborted = false;
    Coroutine breathLoopRoutine;
    Coroutine legBaseLoopRoutine;
    JSONStorable hbaStorable;
    JSONStorableFloat hbaProgressParam;
    JSONStorableBool hbaActiveParam;
    JSONStorable externalDockingPoseAssistStorable;
    JSONStorableBool externalDockingPoseAssistActiveParam;
    JSONStorableFloat externalDockingPoseAssistLastEventTimeParam;
    JSONStorable targetGrabberStorable;
    JSONStorableBool tgHeldTargetLHandParam;
    JSONStorableBool tgHeldTargetRHandParam;
    bool tgHeldTargetLHandCached = false;
    bool tgHeldTargetRHandCached = false;
    string tgHeldTargetSourceCached = "";
    bool externalDockingPoseAssistCached = false;
    string externalDockingPoseAssistSourceCached = "";
    float externalDockingPoseAssistElapsedCached = -1.0f;
    float nextHbaResolveTime = -999.0f;
    float nextExternalDockingPoseAssistResolveTime = -999.0f;
    float nextTargetGrabberResolveTime = -999.0f;
    float hbaLegResumeAllowedTime = -999.0f;
    float legExternalResumeAllowedTime = -999.0f;
    bool legPausedByHba = false;
    bool legPausedByExternalDockingPoseAssist = false;
    float hbaBreathResumeAllowedTime = -999.0f;
    bool breathPausedByHba = false;
    float hbaGestureResumeAllowedTime = -999.0f;
    bool lifeGesturePausedByHba = false;
    bool forceCoverOnNextGesture = false;
    ControllerSnapshot activeBreathSnapshot;
    ControllerSnapshot activeBreathLeftElbowSnapshot;
    ControllerSnapshot activeBreathRightElbowSnapshot;
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
    Coroutine affectionMorphRoutine;
    Dictionary<string, float> affectionMorphOriginalValues = new Dictionary<string, float>();
    string lastAffectionMorphAffection = "";
    string lastAffectionMorphPersonality = "";
    string lastAffectionMorphLikeName = "";
    string lastAffectionMorphDislikeName = "";
    string lastAffectionMorphShyName = "";
    float lastAffectionMorphLikeMax = -999.0f;
    float lastAffectionMorphDislikeMax = -999.0f;
    float lastAffectionMorphShyMax = -999.0f;
    float lastAffectionMorphFadeSeconds = -999.0f;
    bool initialized;
    bool suppressLifeStateCallback = false;
    string lastAppliedEyeState = "";
    JSONStorableBool sleepingAutoBlinkParam;
    bool sleepingAutoBlinkResolved = false;
    bool sleepingAutoBlinkOriginalSaved = false;
    bool sleepingAutoBlinkOriginalValue = false;
    string sleepingAutoBlinkSource = "";
    float nextSleepingEyeHoldTime = -999.0f;
    const float SleepingEyeHoldInterval = 0.35f;
    const float SleepingEyeCloseSeconds = 1.65f;
    const float SleepingEyeOpenSeconds = 1.10f;
    const float SleepSettleComplySeconds = 2.50f;
    Coroutine sleepingEyeTransitionRoutine;
    Coroutine sleepSettleComplyRoutine;

    class SleepingEyeAutoBoolState
    {
        public JSONStorableBool param;
        public bool originalValue;
        public bool saved;
        public string source;
    }

    readonly List<SleepingEyeAutoBoolState> sleepingEyeAutoBoolStates = new List<SleepingEyeAutoBoolState>();
    bool sleepingEyeAutoSystemsResolved = false;

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
            // v112: keep the main state controls on the left and move debug/morph tools to the right column.
            CreateToggle(debugLog, true);

            logCoverDetail = new JSONStorableBool("HLA Log Cover Detail", false);
            RegisterBool(logCoverDetail);
            // v095: internal/debug detail toggle is registered for compatibility but hidden from the simple UI.

            lifeStateMode = new JSONStorableStringChooser(
                "Life State",
                new List<string>(lifeStateChoices),
                LifeStateNormal,
                "Life State",
                delegate(string value) { OnLifeStateChanged(value); }
            );
            RegisterStringChooser(lifeStateMode);
            CreatePopup(lifeStateMode, false);

            lifePersonalityMode = new JSONStorableStringChooser(
                "Life Personality",
                new List<string>(lifePersonalityChoices),
                LifePersonalityNormal,
                "Life Personality",
                delegate(string value) { OnLifePersonalityChanged(value); }
            );
            RegisterStringChooser(lifePersonalityMode);
            CreatePopup(lifePersonalityMode, false);

            RefreshAffectionMorphChoices(false);

            lifeAffectionMode = new JSONStorableStringChooser(
                "Life Affection",
                new List<string>(lifeAffectionChoices),
                LifeAffectionNeutral,
                "Life Expression",
                delegate(string value) { OnLifeAffectionChanged(value); }
            );
            RegisterStringChooser(lifeAffectionMode);
            CreatePopup(lifeAffectionMode, true);


            // v116: Sad is part of the expression group together with Like/Dislike. The popup is filterable. Old Shy actions remain aliases.
            shyPersonalityMorphName = new JSONStorableStringChooser(
                "Sad Morph",
                new List<string>(shyPersonalityMorphChoices),
                DefaultShyPersonalityMorphName,
                "Sad Morph",
                delegate(string value) { ApplyAffectionMorphTarget("shy-morph-choice"); }
            );
            RegisterStringChooser(shyPersonalityMorphName);
            CreateFilterablePopup(shyPersonalityMorphName, true);

            shyPersonalityMorphMax = new JSONStorableFloat("Sad Morph Max", DefaultShyPersonalityMorphMax, 0.0f, 1.0f, true);
            RegisterFloat(shyPersonalityMorphMax);
            CreateSlider(shyPersonalityMorphMax, true);

            // v111: VaM text fields are not usable enough for morph search here.
            // Keep these registered for save compatibility, but hide them from the UI.
            // The actual user-facing search is the filter box inside CreateFilterablePopup.
            likeAffectionMorphSearch = new JSONStorableString("Like Morph Search", DefaultLikeAffectionMorphSearch);
            RegisterString(likeAffectionMorphSearch);

            dislikeAffectionMorphSearch = new JSONStorableString("Dislike Morph Search", DefaultDislikeAffectionMorphSearch);
            RegisterString(dislikeAffectionMorphSearch);

            RefreshAffectionMorphChoices(false);
            likeAffectionMorphName = new JSONStorableStringChooser(
                "Like Morph",
                new List<string>(affectionMorphChoices),
                DefaultLikeAffectionMorphName,
                "Like Morph",
                delegate(string value) { ApplyAffectionMorphTarget("like-morph-choice"); }
            );
            RegisterStringChooser(likeAffectionMorphName);
            // v111: filterable popup lets the user type inside the combo popup.
            CreateFilterablePopup(likeAffectionMorphName, true);

            likeAffectionMorphMax = new JSONStorableFloat("Like Morph Max", DefaultLikeAffectionMorphMax, 0.0f, 1.0f, true);
            RegisterFloat(likeAffectionMorphMax);
            CreateSlider(likeAffectionMorphMax, true);

            dislikeAffectionMorphName = new JSONStorableStringChooser(
                "Dislike Morph",
                new List<string>(dislikeAffectionMorphChoices),
                DefaultDislikeAffectionMorphName,
                "Dislike Morph",
                delegate(string value) { ApplyAffectionMorphTarget("dislike-morph-choice"); }
            );
            RegisterStringChooser(dislikeAffectionMorphName);
            // v111: filterable popup lets the user type inside the combo popup.
            CreateFilterablePopup(dislikeAffectionMorphName, true);

            dislikeAffectionMorphMax = new JSONStorableFloat("Dislike Morph Max", DefaultDislikeAffectionMorphMax, 0.0f, 1.0f, true);
            RegisterFloat(dislikeAffectionMorphMax);
            CreateSlider(dislikeAffectionMorphMax, true);

            affectionMorphFadeSeconds = new JSONStorableFloat("Affection Morph Fade Seconds", DefaultAffectionMorphFadeSeconds, 0.05f, 5.0f, true);
            RegisterFloat(affectionMorphFadeSeconds);
            CreateSlider(affectionMorphFadeSeconds, true);

            CreateButton("Refresh Affection Morphs", true).button.onClick.AddListener(delegate { RefreshAffectionMorphChoices(true); });

            lifeMotionMode = new JSONStorableStringChooser(
                "Life Motion",
                new List<string>(lifeMotionChoices),
                LifeMotionNormal,
                "Life Motion"
            );
            RegisterStringChooser(lifeMotionMode);
            // v095: hidden legacy motion chooser. Life State drives the effective motion level.

            breathEnabled = new JSONStorableBool("Life Breath", true);
            RegisterBool(breathEnabled);
            // v003 simple UI: keep registered/default ON but hide tuning toggle.

            autoPauseBreathOnHbaActive = new JSONStorableBool("Auto Pause Breath On HBA Active", true);
            RegisterBool(autoPauseBreathOnHbaActive);
            // v095: hidden safety toggle. Keep registered/default ON.

            lifeHeadLookEnabled = new JSONStorableBool("Life Head Look", true);
            RegisterBool(lifeHeadLookEnabled);
            // v095: hidden. Life State controls whether head look is effectively used.

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
            // v095: hidden. Life State controls effective leg motion.

            lifeLegPositionAssistEnabled = new JSONStorableBool("Life Leg Position Assist", false);
            RegisterBool(lifeLegPositionAssistEnabled);
            // v095: hidden advanced option. Default OFF for coexistence.

            autoPauseLegOnHbaActive = new JSONStorableBool("Auto Pause Leg On HBA Active", true);
            RegisterBool(autoPauseLegOnHbaActive);
            // v095: hidden safety toggle. Keep registered/default ON.

            autoPauseGesturesOnHbaActive = new JSONStorableBool("Auto Pause Gestures On HBA Active", true);
            RegisterBool(autoPauseGesturesOnHbaActive);
            // v095: hidden safety toggle. Keep registered/default ON.

            respectExistingHandIk = new JSONStorableBool("Respect Existing Hand IK", false);
            RegisterBool(respectExistingHandIk);
            // v003 simple UI: keep registered/default ON but hide tuning toggle.

            poseChangeSafe = new JSONStorableBool("Pose Change Safe", true);
            RegisterBool(poseChangeSafe);
            // v095: hidden safety toggle. Keep registered/default ON.

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

            breathScale = new JSONStorableFloat("Life Breath Scale", DefaultBreathScale, 0.0f, 50.0f, true);
            RegisterFloat(breathScale);
            // v095: hidden tuning slider. Life State applies the effective scale.

            shoulderSwayEnabled = new JSONStorableBool("Life Shoulder Sway", true);
            RegisterBool(shoulderSwayEnabled);
            // v095: hidden tuning toggle. Life State applies the effective behavior.

            shoulderSwayScale = new JSONStorableFloat("Life Shoulder Sway Scale", DefaultShoulderSwayScale, 0.0f, 50.0f, true);
            RegisterFloat(shoulderSwayScale);
            // v095: hidden tuning slider. Life State applies the effective scale.

            legScale = new JSONStorableFloat("Life Leg Scale", DefaultLegScale, 0.0f, 5.0f, true);
            RegisterFloat(legScale);
            // v095: hidden tuning slider. Life State applies the effective scale.

            coverFrequency = new JSONStorableFloat("Life Cover Frequency", DefaultCoverFrequency, 0.0f, 100.0f, true);
            RegisterFloat(coverFrequency);
            // v095: hidden tuning slider. Life State applies the effective frequency.

            lookFrequency = new JSONStorableFloat("Life Look Frequency", DefaultLookFrequency, 0.0f, 100.0f, true);
            RegisterFloat(lookFrequency);
            // v095: hidden tuning slider. Life State applies the effective frequency.

            coverSelfPercent = new JSONStorableFloat("Life Cover Self %", DefaultCoverSelfPercent, 0.0f, 100.0f, true);
            RegisterFloat(coverSelfPercent);
            // v095: hidden tuning slider. Life State applies the effective split.

            lookTargetPercent = new JSONStorableFloat("Life Look Target %", DefaultLookTargetPercent, 0.0f, 100.0f, true);
            RegisterFloat(lookTargetPercent);
            // v095: hidden tuning slider. Life State applies the effective split.

            lookAwayPercent = new JSONStorableFloat("Life Look Away %", DefaultLookAwayPercent, 0.0f, 100.0f, true);
            RegisterFloat(lookAwayPercent);
            // v095: hidden tuning slider. Life State applies the effective split.

            lookMaxAngle = new JSONStorableFloat("Life Look Max Angle", DefaultLookMaxAngle, 0.0f, 180.0f, true);
            RegisterFloat(lookMaxAngle);
            // v003 simple UI: keep registered/default but hide tuning slider.

            coverMaxDistance = new JSONStorableFloat("Life Cover Max Distance", DefaultCoverMaxDistance, 0.05f, 1.50f, true);
            RegisterFloat(coverMaxDistance);
            // v003 simple UI: keep registered/default but hide tuning slider.

            CreateButton("Refresh Life Targets", false).button.onClick.AddListener(delegate { RefreshTargetPersonChoices(true); });
            CreateButton("HLA_Force_Breath", false).button.onClick.AddListener(delegate { RequestBreath("button"); });
            CreateButton("HLA_Force_LookTarget", false).button.onClick.AddListener(delegate { RequestLookTarget("button"); });
            CreateButton("HLA_Force_LookCamera", false).button.onClick.AddListener(delegate { RequestLookCamera("button"); });
            CreateButton("HLA_Force_LookAway", false).button.onClick.AddListener(delegate { RequestLookAway("button"); });
            CreateButton("HLA_Force_RandomCover", false).button.onClick.AddListener(delegate { RequestRandomCover("button"); });
            CreateButton("HLA Cover Self Head", false).button.onClick.AddListener(delegate { RequestExternalSelfHeadCover("button"); });
            CreateButton("HLA Cover Self Hip", false).button.onClick.AddListener(delegate { RequestExternalSelfHipCover("button"); });
            CreateButton("HLA_Test_SelfHead", false).button.onClick.AddListener(delegate { RequestTestSelfHeadCover("button"); });
            CreateButton("HLA Hip Self", false).button.onClick.AddListener(delegate { RequestTestSelfHipCover("button"); });
            CreateButton("HLA_Test_TargetHead", false).button.onClick.AddListener(delegate { RequestTestTargetHeadCover("button"); });
            CreateButton("HLA_Test_TargetHip", false).button.onClick.AddListener(delegate { RequestTestTargetHipCover("button"); });
            CreateButton("HLA Hip Left", false).button.onClick.AddListener(delegate { RequestTestLHandToLThigh("button"); });
            CreateButton("HLA Hip Right", false).button.onClick.AddListener(delegate { RequestTestRHandToRThigh("button"); });
            CreateButton("HLA Hip Both", false).button.onClick.AddListener(delegate { RequestTestBothHandsToThighs("button"); });
            CreateButton("HLA_Force_LegMotion", false).button.onClick.AddListener(delegate { RequestLegMotion("button"); });
            CreateButton("HLA_Stop_Restore", false).button.onClick.AddListener(delegate { StopAllLife("button"); });

            RegisterAction(new JSONStorableAction("HLA_Force_Breath", delegate { RequestBreath("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Force_LookTarget", delegate { RequestLookTarget("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Force_LookCamera", delegate { RequestLookCamera("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Force_LookAway", delegate { RequestLookAway("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Force_RandomCover", delegate { RequestRandomCover("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Cover_SelfHead", delegate { RequestExternalSelfHeadCover("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Cover_SelfHip", delegate { RequestExternalSelfHipCover("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Test_SelfHead", delegate { RequestTestSelfHeadCover("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Hip_SelfCover", delegate { RequestExternalSelfHipCover("action-legacy"); }));
            RegisterAction(new JSONStorableAction("HLA_Test_TargetHead", delegate { RequestTestTargetHeadCover("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Test_TargetHip", delegate { RequestTestTargetHipCover("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Hip_LeftPath", delegate { RequestTestLHandToLThigh("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Hip_RightPath", delegate { RequestTestRHandToRThigh("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Hip_BothPath", delegate { RequestTestBothHandsToThighs("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Force_LegMotion", delegate { RequestLegMotion("action"); }));
            RegisterAction(new JSONStorableAction("HLA_Stop_Restore", delegate { StopAllLife("action"); }));
            RegisterAction(new JSONStorableAction("HLA_State_Quiet", delegate { SetLifeState(LifeStateQuiet, "action"); }));
            RegisterAction(new JSONStorableAction("HLA_State_Shy", delegate { SetLifeAffection(LifeAffectionShy, "action-legacy-state-shy"); }));
            RegisterAction(new JSONStorableAction("HLA_State_Normal", delegate { SetLifeState(LifeStateNormal, "action"); }));
            RegisterAction(new JSONStorableAction("HLA_State_Active", delegate { SetLifeState(LifeStateActive, "action"); }));
            RegisterAction(new JSONStorableAction("HLA_State_Sleeping", delegate { SetLifeState(LifeStateSleeping, "action"); }));
            RegisterAction(new JSONStorableAction("HLA_Personality_Normal", delegate { SetLifePersonality(LifePersonalityNormal, "action"); }));
            RegisterAction(new JSONStorableAction("HLA_Personality_Shy", delegate { SetLifeAffection(LifeAffectionShy, "action-legacy-personality-shy"); }));
            RegisterAction(new JSONStorableAction("HLA_Personality_Bold", delegate { SetLifePersonality(LifePersonalityBold, "action"); }));
            RegisterAction(new JSONStorableAction("HLA_Affection_Neutral", delegate { SetLifeAffection(LifeAffectionNeutral, "action"); }));
            RegisterAction(new JSONStorableAction("HLA_Affection_Like", delegate { SetLifeAffection(LifeAffectionLike, "action"); }));
            RegisterAction(new JSONStorableAction("HLA_Affection_Dislike", delegate { SetLifeAffection(LifeAffectionDislike, "action"); }));
            RegisterAction(new JSONStorableAction("HLA_Affection_Shy", delegate { SetLifeAffection(LifeAffectionShy, "action-legacy-shy"); }));
            RegisterAction(new JSONStorableAction("HLA_Affection_Sad", delegate { SetLifeAffection(LifeAffectionShy, "action"); }));
            RegisterAction(new JSONStorableAction("HLA_Expression_Neutral", delegate { SetLifeAffection(LifeAffectionNeutral, "action-expression"); }));
            RegisterAction(new JSONStorableAction("HLA_Expression_Like", delegate { SetLifeAffection(LifeAffectionLike, "action-expression"); }));
            RegisterAction(new JSONStorableAction("HLA_Expression_Dislike", delegate { SetLifeAffection(LifeAffectionDislike, "action-expression"); }));
            RegisterAction(new JSONStorableAction("HLA_Expression_Shy", delegate { SetLifeAffection(LifeAffectionShy, "action-expression-legacy-shy"); }));
            RegisterAction(new JSONStorableAction("HLA_Expression_Sad", delegate { SetLifeAffection(LifeAffectionShy, "action-expression"); }));

            // v117: AI-facing external IF aliases. HumanReceiver HUM_LIFE(...) maps to these names,
            // but they are also usable directly from VaM triggers.
            RegisterAction(new JSONStorableAction("HLA_Life_State_Sleep", delegate { SetLifeState(LifeStateSleeping, "action-life-if"); }));
            RegisterAction(new JSONStorableAction("HLA_Life_State_Sleeping", delegate { SetLifeState(LifeStateSleeping, "action-life-if"); }));
            RegisterAction(new JSONStorableAction("HLA_Life_State_Quiet", delegate { SetLifeState(LifeStateQuiet, "action-life-if"); }));
            RegisterAction(new JSONStorableAction("HLA_Life_State_Normal", delegate { SetLifeState(LifeStateNormal, "action-life-if"); }));
            RegisterAction(new JSONStorableAction("HLA_Life_State_Active", delegate { SetLifeState(LifeStateActive, "action-life-if"); }));
            RegisterAction(new JSONStorableAction("HLA_Life_Expression_Neutral", delegate { SetLifeAffection(LifeAffectionNeutral, "action-life-if"); }));
            RegisterAction(new JSONStorableAction("HLA_Life_Expression_Like", delegate { SetLifeAffection(LifeAffectionLike, "action-life-if"); }));
            RegisterAction(new JSONStorableAction("HLA_Life_Expression_Dislike", delegate { SetLifeAffection(LifeAffectionDislike, "action-life-if"); }));
            RegisterAction(new JSONStorableAction("HLA_Life_Expression_Sad", delegate { SetLifeAffection(LifeAffectionShy, "action-life-if"); }));
            RegisterAction(new JSONStorableAction("HLA_Life_Personality_Normal", delegate { SetLifePersonality(LifePersonalityNormal, "action-life-if"); }));
            RegisterAction(new JSONStorableAction("HLA_Life_Personality_Bold", delegate { SetLifePersonality(LifePersonalityBold, "action-life-if"); }));

            ResolveControllers();
            ScheduleNextGesture("init");
            initialized = true;
            ApplyLifeStateEyeControl("init");
            ApplyAffectionMorphTarget("init");
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

        MaintainLifeStateEyeControl();
        MaintainAffectionMorphControl();

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
            if (randomCoverEnabled != null && randomCoverEnabled.val && EffectiveCoverFrequency() > 0.001f)
            {
                Log("Life roll / selected=RandomCover / mode=hba-resume");
                RequestRandomCover("hba-resume");
                return;
            }
        }

        // v046: Cover Frequency is now an independent percent roll.
        // It is no longer mixed into the same weighted bucket as Look/None.
        // Example default: Cover=90 means about 90% cover, then Look rolls only in the remaining 10%.
        if (randomCoverEnabled != null && randomCoverEnabled.val)
        {
            float coverPercent = EffectiveCoverFrequency();
            if (coverPercent > 0.001f)
            {
                float coverRoll = UnityEngine.Random.Range(0.0f, 100.0f);
                if (coverRoll <= coverPercent)
                {
                    Log("Life roll / selected=RandomCover / coverRoll=" + coverRoll.ToString("F1", CultureInfo.InvariantCulture)
                        + " / cover%=" + coverPercent.ToString("F1", CultureInfo.InvariantCulture)
                        + " / mode=independent");
                    RequestRandomCover(coverPercent >= 99.999f ? "life-cover100" : "life");
                    return;
                }

                Log("Life roll / cover miss / coverRoll=" + coverRoll.ToString("F1", CultureInfo.InvariantCulture)
                    + " / cover%=" + coverPercent.ToString("F1", CultureInfo.InvariantCulture));
            }
        }

        // v046: Look Frequency is also interpreted as a percent roll, but only after Cover misses.
        if (IsHeadLookEnabled())
        {
            float lookPercent = EffectiveLookFrequency();
            if (lookPercent > 0.001f)
            {
                float lookRoll = UnityEngine.Random.Range(0.0f, 100.0f);
                if (lookRoll <= lookPercent)
                {
                    List<GestureChoice> lookChoices = new List<GestureChoice>();
                    float lookAwayWeight = EffectiveLookAwayPercent();
                    float lookRemainWeight = Mathf.Max(0.0f, 100.0f - lookAwayWeight);
                    float lookTargetWeight = lookRemainWeight * Mathf.Clamp01(EffectiveLookTargetPercent() / 100.0f);
                    float lookCameraWeight = Mathf.Max(0.0f, lookRemainWeight - lookTargetWeight);

                    if (lookTargetEnabled != null && lookTargetEnabled.val && lookTargetWeight > 0.001f)
                        lookChoices.Add(new GestureChoice("LookTarget", lookTargetWeight, delegate { RequestLookTarget("life"); }));
                    if (lookCameraEnabled != null && lookCameraEnabled.val && lookCameraWeight > 0.001f)
                        lookChoices.Add(new GestureChoice("LookCamera", lookCameraWeight, delegate { RequestLookCamera("life"); }));
                    if (lookAwayWeight > 0.001f)
                        lookChoices.Add(new GestureChoice("LookAway", lookAwayWeight, delegate { RequestLookAway("life"); }));

                    float lookTotal = 0.0f;
                    for (int i = 0; i < lookChoices.Count; i++)
                    {
                        if (lookChoices[i] != null && lookChoices[i].weight > 0.0f) lookTotal += lookChoices[i].weight;
                    }

                    if (lookTotal > 0.001f)
                    {
                        float pickRoll = UnityEngine.Random.Range(0.0f, lookTotal);
                        float acc = 0.0f;
                        for (int i = 0; i < lookChoices.Count; i++)
                        {
                            GestureChoice c = lookChoices[i];
                            if (c == null || c.weight <= 0.0f) continue;
                            acc += c.weight;
                            if (pickRoll <= acc)
                            {
                                Log("Life roll / selected=" + c.name
                                    + " / lookRoll=" + lookRoll.ToString("F1", CultureInfo.InvariantCulture)
                                    + " / look%=" + lookPercent.ToString("F1", CultureInfo.InvariantCulture)
                                    + " / pick=" + pickRoll.ToString("F1", CultureInfo.InvariantCulture)
                                    + " / lookTotal=" + lookTotal.ToString("F1", CultureInfo.InvariantCulture)
                                    + " / mode=independent");
                                c.action();
                                return;
                            }
                        }
                    }
                }

                Log("Life roll / look miss / lookRoll=" + lookRoll.ToString("F1", CultureInfo.InvariantCulture)
                    + " / look%=" + lookPercent.ToString("F1", CultureInfo.InvariantCulture));
            }
        }

        Log("Life roll / selected=None / mode=independent");
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
        if (breathEnabled == null || !breathEnabled.val || EffectiveBreathScale() <= 0.0001f)
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

        // v089+: Breath must not own chest position. Holding chest PositionState.On can make
        // chest stay behind when root/control is moved. Breath is now rotation-only for chest.
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
                RebaseBreathShoulderSwaySnapshots();
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

            // v089: rotation-only breath. Do not set chest position and do not keep
            // PositionState.On just for breathing. This avoids chestControl being left behind
            // when the Person root/control is moved while Life is active.
            float amount = 0.0f;
            float swayAmount = 0.0f;
            float rotAmount = EffectiveBreathRotationDegrees();

            float pitch = rotAmount * 0.65f * breathWave;
            float lean = rotAmount * 0.20f * sideWave;
            Quaternion breathRot = Quaternion.AngleAxis(pitch, rootRight) * Quaternion.AngleAxis(lean, rootForward) * baseRot;

            SetControllerRotation(ctrl, breathRot);
            lastAppliedPos = GetControllerPosition(ctrl);
            lastAppliedRot = breathRot;

            float shoulderAmount = 0.0f;
            bool shoulderSwayOn = IsShoulderSwayEnabled();
            bool shoulderSwaySuppressed = IsBreathShoulderSwaySuppressed();
            if (shoulderSwayOn && !shoulderSwaySuppressed)
            {
                EnsureBreathShoulderSwaySnapshots();
                shoulderAmount = EffectiveShoulderSwayAmount();
                ApplyBreathShoulderSway(rootForward, rootRight, up, breathWave, exhaleWave, sideWave, shoulderAmount);
            }
            else if (!shoulderSwayOn && (activeBreathLeftElbowSnapshot != null || activeBreathRightElbowSnapshot != null))
            {
                RestoreBreathShoulderSway(true);
            }

            if (debugLog != null && debugLog.val && Time.time - lastLog > 3.0f)
            {
                lastLog = Time.time;
                Log("Breath loop / mode=rotation-only+shoulder-sway / amount=" + amount.ToString("F3", CultureInfo.InvariantCulture)
                    + " / sway=" + swayAmount.ToString("F3", CultureInfo.InvariantCulture)
                    + " / rot=" + rotAmount.ToString("F2", CultureInfo.InvariantCulture)
                    + " / shoulder=" + shoulderAmount.ToString("F3", CultureInfo.InvariantCulture)
                    + " / shoulderOn=" + (shoulderSwayOn ? "1" : "0")
                    + " / shoulderSuppressed=" + (shoulderSwaySuppressed ? "1" : "0"));
            }

            yield return null;
        }

        RestoreBreathController(snap);
        RestoreBreathShoulderSway(true);
        breathLoopRoutine = null;
        activeBreathSnapshot = null;
        Log("Breath loop stop / source=" + source + " / mode=rotation-only+shoulder-sway");
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
            if (restorePose) RestoreBreathController(activeBreathSnapshot);
            else RestoreControllerStateOnly(activeBreathSnapshot);
            activeBreathSnapshot = null;
        }

        RestoreBreathShoulderSway(restorePose);
    }

    bool IsShoulderSwayEnabled()
    {
        // v102: Sleeping still gets a very small elbow/shoulder micro-sway so it does not look frozen.
        return shoulderSwayEnabled != null && shoulderSwayEnabled.val && EffectiveShoulderSwayScale() > 0.0001f;
    }

    bool IsBreathShoulderSwaySuppressed()
    {
        // Do not fight RandomCover or other Life hand gestures that may temporarily own elbows.
        return activeCoverHandSnapshot != null || activeCoverElbowSnapshot != null;
    }

    void EnsureBreathShoulderSwaySnapshots()
    {
        if (!IsShoulderSwayEnabled()) return;
        ResolveControllers();

        if (activeBreathLeftElbowSnapshot == null && lElbowControl != null)
        {
            activeBreathLeftElbowSnapshot = CaptureController(lElbowControl);
            try { lElbowControl.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        }
        if (activeBreathRightElbowSnapshot == null && rElbowControl != null)
        {
            activeBreathRightElbowSnapshot = CaptureController(rElbowControl);
            try { rElbowControl.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        }
    }

    void RebaseBreathShoulderSwaySnapshots()
    {
        if (activeBreathLeftElbowSnapshot != null && activeBreathLeftElbowSnapshot.controller != null)
        {
            activeBreathLeftElbowSnapshot.position = GetControllerPosition(activeBreathLeftElbowSnapshot.controller);
            activeBreathLeftElbowSnapshot.rotation = GetControllerRotation(activeBreathLeftElbowSnapshot.controller);
        }
        if (activeBreathRightElbowSnapshot != null && activeBreathRightElbowSnapshot.controller != null)
        {
            activeBreathRightElbowSnapshot.position = GetControllerPosition(activeBreathRightElbowSnapshot.controller);
            activeBreathRightElbowSnapshot.rotation = GetControllerRotation(activeBreathRightElbowSnapshot.controller);
        }
    }

    void ApplyBreathShoulderSway(Vector3 rootForward, Vector3 rootRight, Vector3 up, float breathWave, float exhaleWave, float sideWave, float shoulderAmount)
    {
        if (shoulderAmount <= 0.0001f) return;

        // v091: Shoulder Sway intentionally does not touch hand IK. It only nudges elbows
        // slightly so the shoulder/upper-arm area reads as breathing while chest position stays free.
        Vector3 common = up * (shoulderAmount * 0.28f * breathWave)
            + rootForward * (shoulderAmount * 0.10f * exhaleWave);
        Vector3 microSide = rootRight * (shoulderAmount * 0.08f * sideWave);

        if (activeBreathLeftElbowSnapshot != null && activeBreathLeftElbowSnapshot.controller != null)
        {
            Vector3 leftOffset = (-rootRight * (shoulderAmount * 0.62f * breathWave)) + common - microSide;
            SetControllerPosition(activeBreathLeftElbowSnapshot.controller, activeBreathLeftElbowSnapshot.position + leftOffset);
        }
        if (activeBreathRightElbowSnapshot != null && activeBreathRightElbowSnapshot.controller != null)
        {
            Vector3 rightOffset = (rootRight * (shoulderAmount * 0.62f * breathWave)) + common + microSide;
            SetControllerPosition(activeBreathRightElbowSnapshot.controller, activeBreathRightElbowSnapshot.position + rightOffset);
        }
    }

    void RestoreBreathShoulderSway(bool restorePose)
    {
        if (activeBreathLeftElbowSnapshot != null)
        {
            if (restorePose) RestoreController(activeBreathLeftElbowSnapshot);
            else RestoreControllerStateOnly(activeBreathLeftElbowSnapshot);
            activeBreathLeftElbowSnapshot = null;
        }
        if (activeBreathRightElbowSnapshot != null)
        {
            if (restorePose) RestoreController(activeBreathRightElbowSnapshot);
            else RestoreControllerStateOnly(activeBreathRightElbowSnapshot);
            activeBreathRightElbowSnapshot = null;
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

        if (Time.time < legExternalResumeAllowedTime)
            return;

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
        bool positionAssist = IsLegPositionAssistEnabled();

        if (lCtrl != null)
        {
            try { lCtrl.currentRotationState = FreeControllerV3.RotationState.On; } catch { }
            if (positionAssist)
            {
                try { lCtrl.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
            }
        }
        if (rCtrl != null)
        {
            try { rCtrl.currentRotationState = FreeControllerV3.RotationState.On; } catch { }
            if (positionAssist)
            {
                try { rCtrl.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
            }
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
                    // v093: Do not keep owning the thighs while another pose/root move is active.
                    // Release only the Life-owned states, wait briefly, then resume from the new pose.
                    legExternalResumeAllowedTime = Time.time + LegExternalChangeResumeDelaySeconds;
                    RestoreControllerStateOnly(activeLegBaseLeftSnapshot);
                    RestoreControllerStateOnly(activeLegBaseRightSnapshot);
                    activeLegBaseLeftSnapshot = null;
                    activeLegBaseRightSnapshot = null;
                    legBaseLoopRoutine = null;
                    Log("Pose change safe / leg base yield / resumeAfter=" + LegExternalChangeResumeDelaySeconds.ToString("F2", CultureInfo.InvariantCulture));
                    yield break;
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
            float posAmount = positionAssist ? EffectiveLegBasePositionAmount() : 0.0f;
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
                if (positionAssist) SetControllerPosition(lCtrl, lPos);
                lLastRot = lApply;
            }
            if (rCtrl != null)
            {
                Quaternion rApply = Quaternion.AngleAxis((openWave + singleBias), rootForward) * Quaternion.AngleAxis(-relaxWave, rootRight) * rBaseRot;
                Vector3 rPos = rBasePos
                    + rootRight * ((openNorm + singleNorm * 0.45f) * posAmount)
                    + rootForward * (-relaxNorm * posAmount * 0.35f);
                SetControllerRotation(rCtrl, rApply);
                if (positionAssist) SetControllerPosition(rCtrl, rPos);
                rLastRot = rApply;
            }

            if (debugLog != null && debugLog.val && Time.time - lastLog > 4.0f)
            {
                lastLog = Time.time;
                Log("Leg base loop / amount=" + amount.ToString("F2", CultureInfo.InvariantCulture)
                    + " / pos=" + posAmount.ToString("F3", CultureInfo.InvariantCulture)
                    + " / posAssist=" + (positionAssist ? "1" : "0")
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
            else ReleaseLegBaseThighForHandoff(activeLegBaseLeftSnapshot, reason);
            activeLegBaseLeftSnapshot = null;
        }
        if (activeLegBaseRightSnapshot != null)
        {
            if (restorePose) RestoreController(activeLegBaseRightSnapshot);
            else ReleaseLegBaseThighForHandoff(activeLegBaseRightSnapshot, reason);
            activeLegBaseRightSnapshot = null;
        }
    }


    void ReleaseLegBaseThighForHandoff(ControllerSnapshot snap, string reason)
    {
        if (snap == null || snap.controller == null) return;
        FreeControllerV3 ctrl = snap.controller;
        string beforePos = "?";
        string beforeRot = "?";
        try { beforePos = ctrl.currentPositionState.ToString(); } catch { }
        try { beforeRot = ctrl.currentRotationState.ToString(); } catch { }

        try { ctrl.currentPositionState = FreeControllerV3.PositionState.Comply; } catch { }
        try { ctrl.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        string afterPos = "?";
        string afterRot = "?";
        try { afterPos = ctrl.currentPositionState.ToString(); } catch { }
        try { afterRot = ctrl.currentRotationState.ToString(); } catch { }

        NormalLog("Leg base thigh release for handoff / reason=" + reason
            + " / controller=" + (ctrl != null ? ctrl.name : "<none>")
            + " / pos=" + beforePos + "->" + afterPos
            + " / rot=" + beforeRot + "->" + afterRot);
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
            if (randomCoverEnabled != null && randomCoverEnabled.val && EffectiveCoverFrequency() > 0.001f)
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
            legPausedByExternalDockingPoseAssist = false;
            return false;
        }

        bool hasHba = false;
        float progress = 0.0f;
        bool active = false;
        if (TryReadHbaProgress(out progress)) hasHba = true;
        if (TryReadHbaActive(out active)) hasHba = true;

        bool dockingPoseAssistActive = IsExternalDockingPoseAssistActive();
        bool hbaNowActive = hasHba && (active || progress > HbaProgressPauseThreshold);
        bool nowActive = hbaNowActive || dockingPoseAssistActive;

        if (nowActive)
        {
            hbaLegResumeAllowedTime = Time.time + HbaLegResumeDelaySeconds;
            if (!legPausedByHba || legPausedByExternalDockingPoseAssist != dockingPoseAssistActive)
            {
                legPausedByHba = true;
                legPausedByExternalDockingPoseAssist = dockingPoseAssistActive;
                NormalLog("Leg auto pause / hba=" + (hbaNowActive ? "1" : "0")
                    + " / dockingPoseAssist=" + (dockingPoseAssistActive ? "1" : "0")
                    + " / progress=" + progress.ToString("F3", CultureInfo.InvariantCulture)
                    + " / active=" + (active ? "1" : "0")
                    + " / dockingSource=" + (string.IsNullOrEmpty(externalDockingPoseAssistSourceCached) ? "-" : externalDockingPoseAssistSourceCached)
                    + " / dockingElapsed=" + externalDockingPoseAssistElapsedCached.ToString("F2", CultureInfo.InvariantCulture));
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
            legPausedByExternalDockingPoseAssist = false;
            NormalLog("Leg auto resume after external idle / progress=" + progress.ToString("F3", CultureInfo.InvariantCulture));
        }
        return false;
    }

    bool IsExternalDockingPoseAssistActive()
    {
        ResolveExternalDockingPoseAssistParams(false);
        return externalDockingPoseAssistCached;
    }

    void ResolveExternalDockingPoseAssistParams(bool force)
    {
        if (!force && Time.time < nextExternalDockingPoseAssistResolveTime) return;
        nextExternalDockingPoseAssistResolveTime = Time.time + ExternalDockingPoseAssistResolveInterval;

        externalDockingPoseAssistStorable = null;
        externalDockingPoseAssistActiveParam = null;
        externalDockingPoseAssistLastEventTimeParam = null;
        externalDockingPoseAssistCached = false;
        externalDockingPoseAssistSourceCached = "";
        externalDockingPoseAssistElapsedCached = -1.0f;

        List<Atom> atoms = null;
        try { atoms = SuperController.singleton != null ? SuperController.singleton.GetAtoms() : null; } catch { atoms = null; }
        if (atoms == null) return;

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

                JSONStorableBool activeParam = null;
                JSONStorableFloat lastEventParam = null;
                try { activeParam = st.GetBoolJSONParam("DOCKING Pose Assist Active"); } catch { activeParam = null; }
                if (activeParam == null) continue;
                try { lastEventParam = st.GetFloatJSONParam("DOCKING Pose Assist Last Event Time"); } catch { lastEventParam = null; }

                bool activeVal = false;
                try { activeVal = activeParam.val; } catch { activeVal = false; }
                if (!activeVal) continue;

                float elapsed = -1.0f;
                bool withinWindow = true;
                if (lastEventParam != null)
                {
                    float lastEvent = -1.0f;
                    try { lastEvent = lastEventParam.val; } catch { lastEvent = -1.0f; }
                    if (lastEvent >= 0.0f)
                    {
                        elapsed = Time.time - lastEvent;
                        withinWindow = elapsed >= 0.0f && elapsed <= ExternalDockingPoseAssistPauseWindowSeconds;
                    }
                }

                if (!withinWindow) continue;

                externalDockingPoseAssistStorable = st;
                externalDockingPoseAssistActiveParam = activeParam;
                externalDockingPoseAssistLastEventTimeParam = lastEventParam;
                externalDockingPoseAssistCached = true;
                externalDockingPoseAssistElapsedCached = elapsed;
                externalDockingPoseAssistSourceCached = (atom != null ? atom.uid : "<atom>") + "/" + sid;
                return;
            }
        }
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
        string state = CurrentLifeState();
        if (state == LifeStateSleeping) return 3.85f;
        if (state == LifeStateQuiet) return 3.10f;
        if (state == LifeStateActive) return 1.85f;
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 2.65f;
        if (mode == LifeMotionLarge) return 1.85f;
        return 2.25f;
    }

    void RequestLookTarget(string source)
    {
        if (CurrentLifeState() == LifeStateSleeping)
        {
            UpdateStatus("LookTarget skipped: Sleeping");
            ScheduleNextGesture("look-target-sleeping");
            return;
        }

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
        if (CurrentLifeState() == LifeStateSleeping)
        {
            UpdateStatus("LookCamera skipped: Sleeping");
            ScheduleNextGesture("look-camera-sleeping");
            return;
        }

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
            float selectedReach = EffectiveCoverMaxDistanceForTarget(targetLabel);
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
        {
            lifeGestureRoutine = StartCoroutine(RandomFreeHandRoutine(hand, targetLabel, source));
        }
        else if (IsSelfHipCoverLabel(targetLabel))
        {
            // v081: Self Hip fully replaces the old generic targetPos cover. It uses the completed
            // step1->step7 thigh-mid path, so pose changes follow l/r thigh midpoint.
            lifeGestureRoutine = StartCoroutine(MoveHandToSelfThighSideRoutine(hand == lHandControl, "self-hip:" + source));
        }
        else
        {
            lifeGestureRoutine = StartCoroutine(RandomCoverRoutine(hand, targetPos, targetLabel, source));
        }
    }


    void RequestExternalSelfHeadCover(string source)
    {
        RequestTestSelfHeadCover("external-self-head:" + source);
    }

    void RequestExternalSelfHipCover(string source)
    {
        RequestTestSelfHipCover("external-self-hip:" + source);
    }

    void RequestTestSelfHeadCover(string source)
    {
        ResolveControllers();
        Vector3 targetPos;
        if (!TryGetSelfShoulderPoint(0, out targetPos))
        {
            UpdateStatus("Test Self Head skipped: no self shoulder surface point");
            LogCover("Cover test skipped / target=Self Shoulder / reason=no-self-shoulder / source=" + source);
            return;
        }
        RequestFixedCoverTarget("Self Shoulder", targetPos, "test-self-head-compat:" + source);
    }

    void RequestTestSelfHipCover(string source)
    {
        ResolveControllers();
        Vector3 targetPos;
        if (!TryGetSelfHipPoint(out targetPos))
        {
            UpdateStatus("Test Self Hip skipped: no self hip point");
            LogCover("Cover test skipped / target=Self Hip / reason=no-self-hip / source=" + source);
            return;
        }
        RequestFixedCoverTarget("Self Hip", targetPos, "test-self-hip:" + source);
    }

    void RequestTestTargetHeadCover(string source)
    {
        ResolveControllers();
        Atom target = GetSelectedTargetPerson();
        Vector3 targetPos;
        if (target == null || !TryGetTargetShoulderPoint(target, 0, out targetPos))
        {
            UpdateStatus("Test Target Head skipped: no target shoulder surface point");
            LogCover("Cover test skipped / target=Target Shoulder / reason=no-target-shoulder / source=" + source);
            return;
        }
        RequestFixedCoverTarget("Target Shoulder", targetPos, "test-target-head-compat:" + source);
    }

    void RequestTestTargetHipCover(string source)
    {
        ResolveControllers();
        Atom target = GetSelectedTargetPerson();
        Vector3 targetPos;
        if (target == null || !TryGetTargetThighSurfacePoint(target, out targetPos))
        {
            UpdateStatus("Test Target Hip skipped: no target thigh surface point");
            LogCover("Cover test skipped / target=Target Thigh Surface / reason=no-target-thigh-surface / source=" + source);
            return;
        }
        RequestFixedCoverTarget("Target Thigh Surface", targetPos, "test-target-hip-compat:" + source);
    }

    void RequestTestLHandToLThigh(string source)
    {
        RequestTestHandToSelfThighSide(true, source);
    }

    void RequestTestRHandToRThigh(string source)
    {
        RequestTestHandToSelfThighSide(false, source);
    }

    void RequestTestBothHandsToThighs(string source)
    {
        ResolveControllers();
        Vector3 lThighPos;
        Vector3 lTarget;
        Vector3 lSide;
        string lReason;
        Vector3 rThighPos;
        Vector3 rTarget;
        Vector3 rSide;
        string rReason;
        bool hasL = TryGetSelfThighSideHandTarget(true, out lThighPos, out lTarget, out lSide, out lReason);
        bool hasR = TryGetSelfThighSideHandTarget(false, out rThighPos, out rTarget, out rSide, out rReason);
        if (lHandControl == null || rHandControl == null || !hasL || !hasR)
        {
            UpdateStatus("Thigh side test skipped: missing hand/thigh");
            LogCover("Thigh side test skipped / mode=both / lHand=" + (lHandControl != null ? "1" : "0")
                + " / rHand=" + (rHandControl != null ? "1" : "0")
                + " / hasL=" + (hasL ? "1" : "0")
                + " / hasR=" + (hasR ? "1" : "0")
                + " / lReason=" + lReason
                + " / rReason=" + rReason
                + " / source=" + source);
            return;
        }

        StopLifeGesture(source + ":before-both-thigh-side-test");
        lifeGestureRoutine = StartCoroutine(MoveBothHandsToSelfThighSideRoutine(source));
    }

    void RequestTestHandToSelfThighSide(bool left, string source)
    {
        ResolveControllers();
        FreeControllerV3 hand = left ? lHandControl : rHandControl;
        Vector3 thighPos;
        Vector3 targetPos;
        Vector3 sideDir;
        string reason = "hand-null";
        if (hand == null || !TryGetSelfThighSideHandTarget(left, out thighPos, out targetPos, out sideDir, out reason))
        {
            string label = left ? "LHandToLThigh" : "RHandToRThigh";
            UpdateStatus("Thigh side test skipped: " + label);
            LogCover("Thigh side test skipped / mode=" + label
                + " / hand=" + (hand != null ? "1" : "0")
                + " / reason=" + reason
                + " / source=" + source);
            return;
        }

        StopLifeGesture(source + ":before-thigh-side-test");
        lifeGestureRoutine = StartCoroutine(MoveHandToSelfThighSideRoutine(left, source));
    }

    Vector3 GetSelfThighBackDirection()
    {
        Vector3 backDir = Vector3.zero;

        if (containingAtom != null && containingAtom.mainController != null)
        {
            backDir = -containingAtom.mainController.transform.forward;
            backDir.y = 0.0f;
            if (backDir.sqrMagnitude > 0.0001f)
                return backDir.normalized;
        }

        if (hipControl != null && chestControl != null)
        {
            Vector3 hipPos = GetControllerPosition(hipControl);
            Vector3 chestPos = GetControllerPosition(chestControl);
            Vector3 upAxis = chestPos - hipPos;
            upAxis.y = 0.0f;
            if (upAxis.sqrMagnitude > 0.0001f)
                return (-upAxis).normalized;
        }

        return Vector3.zero;
    }

    bool TryGetSelfThighSideHandTarget(bool left, out Vector3 thighPos, out Vector3 targetPos, out Vector3 sideDir, out string reason)
    {
        return TryGetSelfThighSideHandTargetWithOffsets(
            left,
            ThighSideHandOffset,
            ThighSideUpOffset,
            ThighSideBackOffset,
            out thighPos,
            out targetPos,
            out sideDir,
            out reason
        );
    }

    bool TryGetSelfThighSideHandTargetV071(bool left, out Vector3 thighPos, out Vector3 targetPos, out Vector3 sideDir, out string reason)
    {
        return TryGetSelfThighSideHandTargetWithOffsets(
            left,
            ThighSidePreHandOffset,
            ThighSidePreUpOffset,
            ThighSidePreBackOffset,
            out thighPos,
            out targetPos,
            out sideDir,
            out reason
        );
    }

    bool TryGetSelfThighSideHandTargetStep2(bool left, out Vector3 thighPos, out Vector3 targetPos, out Vector3 sideDir, out string reason)
    {
        return TryGetSelfThighSideHandTargetWithOffsets(
            left,
            ThighSideStep2HandOffset,
            ThighSideStep2UpOffset,
            ThighSideStep2BackOffset,
            out thighPos,
            out targetPos,
            out sideDir,
            out reason
        );
    }

    bool TryGetSelfThighSideHandTargetStep3(bool left, out Vector3 thighPos, out Vector3 targetPos, out Vector3 sideDir, out string reason)
    {
        return TryGetSelfThighSideHandTargetWithOffsets(
            left,
            ThighSideStep3HandOffset,
            ThighSideStep3UpOffset,
            ThighSideStep3BackOffset,
            out thighPos,
            out targetPos,
            out sideDir,
            out reason
        );
    }

    bool TryGetSelfThighSideHandTargetStep4(bool left, out Vector3 thighPos, out Vector3 targetPos, out Vector3 sideDir, out string reason)
    {
        return TryGetSelfThighSideHandTargetWithOffsets(
            left,
            ThighSideStep4HandOffset,
            ThighSideStep4UpOffset,
            ThighSideStep4BackOffset,
            out thighPos,
            out targetPos,
            out sideDir,
            out reason
        );
    }

    bool TryGetSelfThighSideHandTargetStep5(bool left, out Vector3 thighPos, out Vector3 targetPos, out Vector3 sideDir, out string reason)
    {
        return TryGetSelfThighSideHandTargetWithOffsets(
            left,
            ThighSideStep5HandOffset,
            ThighSideStep5UpOffset,
            ThighSideStep5BackOffset,
            out thighPos,
            out targetPos,
            out sideDir,
            out reason
        );
    }

    bool TryGetSelfThighSideHandTargetStep6(bool left, out Vector3 thighPos, out Vector3 targetPos, out Vector3 sideDir, out string reason)
    {
        return TryGetSelfThighSideHandTargetWithOffsets(
            left,
            ThighSideStep6HandOffset,
            ThighSideStep6UpOffset,
            ThighSideStep6BackOffset,
            out thighPos,
            out targetPos,
            out sideDir,
            out reason
        );
    }

    bool TryGetSelfThighSideHandTargetStep7ThighMidBack(bool left, out Vector3 thighPos, out Vector3 targetPos, out Vector3 sideDir, out string reason)
    {
        thighPos = Vector3.zero;
        targetPos = Vector3.zero;
        sideDir = Vector3.zero;
        reason = "";

        FreeControllerV3 thigh = left ? lThighControl : rThighControl;
        if (thigh != null)
            thighPos = GetControllerPosition(thigh);

        Vector3 midPos;
        string midReason;
        if (!TryGetSelfThighMidBackPoint(out midPos, out targetPos, out midReason))
        {
            reason = midReason;
            return false;
        }

        if (lThighControl != null && rThighControl != null)
        {
            Vector3 lPos = GetControllerPosition(lThighControl);
            Vector3 rPos = GetControllerPosition(rThighControl);
            Vector3 pairAxis = rPos - lPos;
            pairAxis.y = 0.0f;
            if (pairAxis.sqrMagnitude > 0.0001f)
                sideDir = pairAxis.normalized * (left ? -1.0f : 1.0f);
        }

        reason = "thigh-mid-back/" + midReason;
        return true;
    }

    bool TryGetSelfThighMidBackPoint(out Vector3 midPos, out Vector3 targetPos, out string reason)
    {
        midPos = Vector3.zero;
        targetPos = Vector3.zero;
        reason = "";

        if (lThighControl == null || rThighControl == null)
        {
            reason = "missing-lr-thigh";
            return false;
        }

        Vector3 lPos = GetControllerPosition(lThighControl);
        Vector3 rPos = GetControllerPosition(rThighControl);
        midPos = (lPos + rPos) * 0.5f;
        reason = "thigh-pair-mid";

        Vector3 backDir = GetSelfThighBackDirection();
        if (backDir.sqrMagnitude <= 0.0001f)
        {
            reason += ":no-back-axis";
            return false;
        }

        targetPos = midPos + backDir.normalized * ThighSideStep7MidBackOffset + Vector3.up * ThighSideStep7MidUpOffset;
        return true;
    }

    bool TryGetSelfThighSideHandTargetWithOffsets(bool left, float sideOffset, float upOffset, float backOffset, out Vector3 thighPos, out Vector3 targetPos, out Vector3 sideDir, out string reason)
    {
        thighPos = Vector3.zero;
        targetPos = Vector3.zero;
        sideDir = Vector3.zero;
        reason = "";

        FreeControllerV3 thigh = left ? lThighControl : rThighControl;
        if (thigh == null)
        {
            reason = left ? "missing-lThigh" : "missing-rThigh";
            return false;
        }

        thighPos = GetControllerPosition(thigh);

        if (lThighControl != null && rThighControl != null)
        {
            Vector3 lPos = GetControllerPosition(lThighControl);
            Vector3 rPos = GetControllerPosition(rThighControl);
            Vector3 pairAxis = rPos - lPos;
            pairAxis.y = 0.0f;
            if (pairAxis.sqrMagnitude > 0.0001f)
            {
                sideDir = pairAxis.normalized * (left ? -1.0f : 1.0f);
                reason = "thigh-pair-axis";
            }
        }

        if (sideDir.sqrMagnitude <= 0.0001f)
        {
            Vector3 origin = Vector3.zero;
            bool hasOrigin = false;
            if (hipControl != null)
            {
                origin = GetControllerPosition(hipControl);
                hasOrigin = true;
            }
            else if (containingAtom != null && containingAtom.mainController != null)
            {
                origin = containingAtom.mainController.transform.position;
                hasOrigin = true;
            }
            else if (containingAtom != null && containingAtom.transform != null)
            {
                origin = containingAtom.transform.position;
                hasOrigin = true;
            }

            if (hasOrigin)
            {
                Vector3 fromCenter = thighPos - origin;
                fromCenter.y = 0.0f;
                if (fromCenter.sqrMagnitude > 0.0001f)
                {
                    sideDir = fromCenter.normalized;
                    reason = "hip-to-thigh-axis";
                }
            }
        }

        if (sideDir.sqrMagnitude <= 0.0001f && containingAtom != null && containingAtom.mainController != null)
        {
            sideDir = containingAtom.mainController.transform.right * (left ? -1.0f : 1.0f);
            reason = "root-right-fallback";
        }

        if (sideDir.sqrMagnitude <= 0.0001f)
        {
            reason = "no-side-axis";
            return false;
        }

        sideDir.Normalize();
        Vector3 backDir = GetSelfThighBackDirection();
        targetPos = thighPos + sideDir * sideOffset + backDir * backOffset + Vector3.up * upOffset;
        return true;
    }


    bool TryBuildSelfHipSmoothPath(bool left, Vector3 startPos, Vector3 originalPos, bool reverse, out Vector3[] path, out string reason)
    {
        path = null;
        reason = "";

        Vector3 thighPos;
        Vector3 sideDir;
        Vector3 step1;
        Vector3 step2;
        Vector3 step3;
        Vector3 step4;
        Vector3 step5;
        Vector3 step6;
        Vector3 step7;

        if (!TryGetSelfThighSideHandTargetV071(left, out thighPos, out step1, out sideDir, out reason))
            return false;

        TryGetSelfThighSideHandTargetStep2(left, out thighPos, out step2, out sideDir, out reason);
        TryGetSelfThighSideHandTargetStep3(left, out thighPos, out step3, out sideDir, out reason);
        TryGetSelfThighSideHandTargetStep4(left, out thighPos, out step4, out sideDir, out reason);
        TryGetSelfThighSideHandTargetStep5(left, out thighPos, out step5, out sideDir, out reason);
        TryGetSelfThighSideHandTargetStep6(left, out thighPos, out step6, out sideDir, out reason);
        TryGetSelfThighSideHandTargetStep7ThighMidBack(left, out thighPos, out step7, out sideDir, out reason);

        if (!reverse)
        {
            path = new Vector3[] { startPos, step1, step2, step3, step4, step5, step6, step7 };
        }
        else
        {
            path = new Vector3[] { startPos, step6, step5, step4, step3, step2, step1, originalPos };
        }
        return true;
    }

    Vector3 EvaluateSmoothPath(Vector3[] path, float normalized)
    {
        if (path == null || path.Length == 0)
            return Vector3.zero;

        if (path.Length == 1)
            return path[0];

        float u = Mathf.Clamp01(normalized);
        int last = path.Length - 1;
        if (u >= 1.0f)
            return path[last];

        float scaled = u * last;
        int i = Mathf.Clamp(Mathf.FloorToInt(scaled), 0, last - 1);
        float t = scaled - i;
        float tt = t * t;
        float ttt = tt * t;

        Vector3 p0 = path[Mathf.Max(i - 1, 0)];
        Vector3 p1 = path[i];
        Vector3 p2 = path[i + 1];
        Vector3 p3 = path[Mathf.Min(i + 2, last)];

        return 0.5f * (
            (2.0f * p1) +
            (-p0 + p2) * t +
            (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * tt +
            (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * ttt
        );
    }

    IEnumerator MoveHandAlongSelfHipSmoothPath(FreeControllerV3 hand, bool left, ControllerSnapshot snap, Vector3 startPos, bool reverse, string label, string phase)
    {
        float duration = reverse
            ? (ThighSideFinalMoveSeconds * 6.0f + ThighSideReturnSeconds)
            : (ThighSidePreMoveSeconds + ThighSideFinalMoveSeconds * 6.0f);
        duration = Mathf.Max(0.03f, duration);

        float t = 0.0f;
        Vector3 lastApplied = startPos;
        string reason = "";
        while (t < duration)
        {
            if (IsPoseChangeSafeOn() && ControllerExternallyMovedPositionOnly(hand, lastApplied))
            {
                hipSmoothPathAborted = true;
                AbortGestureForPoseChange(snap, activeCoverElbowSnapshot, "thigh-side:smooth-" + phase);
                yield break;
            }

            t += Time.deltaTime;
            Vector3[] path;
            if (!TryBuildSelfHipSmoothPath(left, startPos, snap.position, reverse, out path, out reason))
            {
                hipSmoothPathAborted = true;
                AbortGestureForPoseChange(snap, activeCoverElbowSnapshot, "thigh-side:smooth-no-path:" + reason);
                yield break;
            }

            float e = Smoother01(t / duration);
            Vector3 applyPos = EvaluateSmoothPath(path, e);
            SetControllerPosition(hand, applyPos);
            lastApplied = applyPos;
            yield return null;
        }

        Vector3[] finalPath;
        if (TryBuildSelfHipSmoothPath(left, startPos, snap.position, reverse, out finalPath, out reason))
        {
            Vector3 finalPos = EvaluateSmoothPath(finalPath, 1.0f);
            SetControllerPosition(hand, finalPos);
        }
    }

    IEnumerator MoveBothHandsAlongSelfHipSmoothPath(ControllerSnapshot lSnap, ControllerSnapshot rSnap, Vector3 lStartPos, Vector3 rStartPos, bool reverse, string phase)
    {
        float duration = reverse
            ? (ThighSideFinalMoveSeconds * 6.0f + ThighSideReturnSeconds)
            : (ThighSidePreMoveSeconds + ThighSideFinalMoveSeconds * 6.0f);
        duration = Mathf.Max(0.03f, duration);

        float t = 0.0f;
        string lReason = "";
        string rReason = "";
        while (t < duration)
        {
            t += Time.deltaTime;
            Vector3[] lPath;
            Vector3[] rPath;
            if (!TryBuildSelfHipSmoothPath(true, lStartPos, lSnap.position, reverse, out lPath, out lReason) ||
                !TryBuildSelfHipSmoothPath(false, rStartPos, rSnap.position, reverse, out rPath, out rReason))
            {
                LogCover("Thigh side smooth path skipped / build=v083 / mode=BothHandsToThighs / phase=" + phase + " / lReason=" + lReason + " / rReason=" + rReason);
                yield break;
            }

            float e = Smoother01(t / duration);
            SetControllerPosition(lHandControl, EvaluateSmoothPath(lPath, e));
            SetControllerPosition(rHandControl, EvaluateSmoothPath(rPath, e));
            yield return null;
        }

        Vector3[] lf;
        Vector3[] rf;
        if (TryBuildSelfHipSmoothPath(true, lStartPos, lSnap.position, reverse, out lf, out lReason))
            SetControllerPosition(lHandControl, EvaluateSmoothPath(lf, 1.0f));
        if (TryBuildSelfHipSmoothPath(false, rStartPos, rSnap.position, reverse, out rf, out rReason))
            SetControllerPosition(rHandControl, EvaluateSmoothPath(rf, 1.0f));
    }
    IEnumerator MoveHandToSelfThighSideRoutine(bool left, string source)
    {
        FreeControllerV3 hand = left ? lHandControl : rHandControl;
        string label = left ? "LHandToLThigh" : "RHandToRThigh";
        if (hand == null)
        {
            UpdateStatus("Thigh side test skipped: no hand / " + label);
            lifeGestureRoutine = null;
            ScheduleNextGesture("thigh-side-no-hand");
            yield break;
        }

        ControllerSnapshot snap = CaptureController(hand);
        activeCoverHandSnapshot = snap;
        AcquireLifeLock(hand, true, label, snap);
        try { hand.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { hand.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        FreeControllerV3 elbow = left ? lElbowControl : rElbowControl;
        ControllerSnapshot elbowSnap = CaptureController(elbow);
        activeCoverElbowSnapshot = elbowSnap;
        if (elbow != null && elbowSnap != null)
        {
            AcquireLifeLock(elbow, false, label + ":ElbowFree", elbowSnap);
            try { elbow.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
            try { elbow.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
            LogCover("Thigh side elbow release / build=v083 / mode=" + label + " / elbow=" + elbow.name + " / posState=Off / rotState=Off");
        }

        Vector3 start = GetControllerPosition(hand);
        Vector3 thighPos;
        Vector3 step1;
        Vector3 step2;
        Vector3 step3;
        Vector3 step4;
        Vector3 step5;
        Vector3 step6;
        Vector3 step7;
        Vector3 sideDir;
        string reason;
        if (!TryGetSelfThighSideHandTargetV071(left, out thighPos, out step1, out sideDir, out reason))
        {
            RestoreController(snap);
            RestoreController(elbowSnap);
            ClearLifeLockForController(hand);
            ClearLifeLockForController(elbow);
            activeCoverHandSnapshot = null;
            activeCoverElbowSnapshot = null;
            lifeGestureRoutine = null;
            UpdateStatus("Thigh side test skipped: " + label + " / " + reason);
            ScheduleNextGesture("thigh-side-no-target");
            yield break;
        }
        TryGetSelfThighSideHandTargetStep2(left, out thighPos, out step2, out sideDir, out reason);
        TryGetSelfThighSideHandTargetStep3(left, out thighPos, out step3, out sideDir, out reason);
        TryGetSelfThighSideHandTargetStep4(left, out thighPos, out step4, out sideDir, out reason);
        TryGetSelfThighSideHandTargetStep5(left, out thighPos, out step5, out sideDir, out reason);
        TryGetSelfThighSideHandTargetStep6(left, out thighPos, out step6, out sideDir, out reason);
        TryGetSelfThighSideHandTargetStep7ThighMidBack(left, out thighPos, out step7, out sideDir, out reason);

        LogCover("Thigh side test selected / build=v083 / mode=" + label
            + " / hand=" + GetHandLabel(hand)
            + " / thigh=" + thighPos.ToString("F3")
            + " / step1=" + step1.ToString("F3")
            + " / step2=" + step2.ToString("F3")
            + " / step3=" + step3.ToString("F3")
            + " / step4=" + step4.ToString("F3")
            + " / step5=" + step5.ToString("F3")
            + " / step6=" + step6.ToString("F3")
            + " / step7=" + step7.ToString("F3")
            + " / sideDir=" + sideDir.ToString("F3")
            + " / axis=" + reason
            + " / source=" + source);
        UpdateStatus("Thigh side test running / " + label);

        LogCover("Thigh side test route / build=v083 / mode=" + label
            + " / order=step1-to-step7-hold-reverse-return"
            + " / step1Offsets=" + ThighSidePreHandOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSidePreUpOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSidePreBackOffset.ToString("F3", CultureInfo.InvariantCulture)
            + " / step2Offsets=" + ThighSideStep2HandOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSideStep2UpOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSideStep2BackOffset.ToString("F3", CultureInfo.InvariantCulture)
            + " / step3Offsets=" + ThighSideStep3HandOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSideStep3UpOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSideStep3BackOffset.ToString("F3", CultureInfo.InvariantCulture)
            + " / step4Offsets=" + ThighSideStep4HandOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSideStep4UpOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSideStep4BackOffset.ToString("F3", CultureInfo.InvariantCulture)
            + " / step5Offsets=" + ThighSideStep5HandOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSideStep5UpOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSideStep5BackOffset.ToString("F3", CultureInfo.InvariantCulture)
            + " / step6Offsets=" + ThighSideStep6HandOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSideStep6UpOffset.ToString("F3", CultureInfo.InvariantCulture) + "," + ThighSideStep6BackOffset.ToString("F3", CultureInfo.InvariantCulture)
            + " / step7=thighMidBack"
            + " / step7Offsets=back:" + ThighSideStep7MidBackOffset.ToString("F3", CultureInfo.InvariantCulture) + ",up:" + ThighSideStep7MidUpOffset.ToString("F3", CultureInfo.InvariantCulture));

        hipSmoothPathAborted = false;
        LogCover("Thigh side smooth path / build=v083 / mode=" + label + " / order=forward-step1-to-step7 / interpolation=catmull-rom");
        yield return StartCoroutine(MoveHandAlongSelfHipSmoothPath(hand, left, snap, start, false, label, "forward"));
        if (hipSmoothPathAborted) yield break;

        float holdT = 0.0f;
        Vector3 holdStep7 = step7;
        Vector3 holdThigh;
        Vector3 holdSide;
        string holdReason;
        while (holdT < ThighSideHoldSeconds)
        {
            if (IsPoseChangeSafeOn() && ControllerExternallyMovedPositionOnly(hand, holdStep7))
            {
                AbortGestureForPoseChange(snap, elbowSnap, "thigh-side:hold");
                yield break;
            }
            holdT += Time.deltaTime;
            TryGetSelfThighSideHandTargetStep7ThighMidBack(left, out holdThigh, out holdStep7, out holdSide, out holdReason);
            SetControllerPosition(hand, holdStep7);
            yield return null;
        }

        LogCover("Thigh side smooth return / build=v083 / mode=" + label + " / order=step7-to-step1-to-original / interpolation=catmull-rom");
        hipSmoothPathAborted = false;
        yield return StartCoroutine(MoveHandAlongSelfHipSmoothPath(hand, left, snap, GetControllerPosition(hand), true, label, "return"));
        if (hipSmoothPathAborted) yield break;

        RestoreController(snap);
        RestoreController(elbowSnap);
        ClearLifeLockForController(hand);
        ClearLifeLockForController(elbow);
        activeCoverHandSnapshot = null;
        activeCoverElbowSnapshot = null;
        lifeGestureRoutine = null;
        UpdateStatus("Hip/Thigh side step route done / " + label);
        ScheduleNextGesture("thigh-side-done");
    }

    IEnumerator MoveBothHandsToSelfThighSideRoutine(string source)
    {
        ControllerSnapshot lSnap = CaptureController(lHandControl);
        ControllerSnapshot rSnap = CaptureController(rHandControl);
        ControllerSnapshot lElbowSnap = CaptureController(lElbowControl);
        ControllerSnapshot rElbowSnap = CaptureController(rElbowControl);
        AcquireLifeLock(lHandControl, true, "BothHandsToThighs", lSnap);
        AcquireLifeLock(rHandControl, true, "BothHandsToThighs", rSnap);
        if (lElbowControl != null && lElbowSnap != null) AcquireLifeLock(lElbowControl, false, "BothHandsToThighs:ElbowFree", lElbowSnap);
        if (rElbowControl != null && rElbowSnap != null) AcquireLifeLock(rElbowControl, false, "BothHandsToThighs:ElbowFree", rElbowSnap);
        try { lHandControl.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { lHandControl.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        try { rHandControl.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { rHandControl.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        try { if (lElbowControl != null) lElbowControl.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
        try { if (lElbowControl != null) lElbowControl.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        try { if (rElbowControl != null) rElbowControl.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
        try { if (rElbowControl != null) rElbowControl.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        LogCover("Thigh side elbow release / build=v083 / mode=BothHandsToThighs / elbows=both / posState=Off / rotState=Off");

        Vector3 lStart = GetControllerPosition(lHandControl);
        Vector3 rStart = GetControllerPosition(rHandControl);
        Vector3 lThigh;
        Vector3 lStep1;
        Vector3 lStep2;
        Vector3 lStep3;
        Vector3 lStep4;
        Vector3 lStep5;
        Vector3 lStep6;
        Vector3 lStep7;
        Vector3 lSide;
        string lAxis;
        Vector3 rThigh;
        Vector3 rStep1;
        Vector3 rStep2;
        Vector3 rStep3;
        Vector3 rStep4;
        Vector3 rStep5;
        Vector3 rStep6;
        Vector3 rStep7;
        Vector3 rSide;
        string rAxis;
        TryGetSelfThighSideHandTargetV071(true, out lThigh, out lStep1, out lSide, out lAxis);
        TryGetSelfThighSideHandTargetStep2(true, out lThigh, out lStep2, out lSide, out lAxis);
        TryGetSelfThighSideHandTargetStep3(true, out lThigh, out lStep3, out lSide, out lAxis);
        TryGetSelfThighSideHandTargetStep4(true, out lThigh, out lStep4, out lSide, out lAxis);
        TryGetSelfThighSideHandTargetStep5(true, out lThigh, out lStep5, out lSide, out lAxis);
        TryGetSelfThighSideHandTargetStep6(true, out lThigh, out lStep6, out lSide, out lAxis);
        TryGetSelfThighSideHandTargetStep7ThighMidBack(true, out lThigh, out lStep7, out lSide, out lAxis);
        TryGetSelfThighSideHandTargetV071(false, out rThigh, out rStep1, out rSide, out rAxis);
        TryGetSelfThighSideHandTargetStep2(false, out rThigh, out rStep2, out rSide, out rAxis);
        TryGetSelfThighSideHandTargetStep3(false, out rThigh, out rStep3, out rSide, out rAxis);
        TryGetSelfThighSideHandTargetStep4(false, out rThigh, out rStep4, out rSide, out rAxis);
        TryGetSelfThighSideHandTargetStep5(false, out rThigh, out rStep5, out rSide, out rAxis);
        TryGetSelfThighSideHandTargetStep6(false, out rThigh, out rStep6, out rSide, out rAxis);
        TryGetSelfThighSideHandTargetStep7ThighMidBack(false, out rThigh, out rStep7, out rSide, out rAxis);

        LogCover("Thigh side test selected / build=v083 / mode=BothHandsToThighs"
            + " / order=step1-to-step7-hold-reverse-return"
            + " / lStep1=" + lStep1.ToString("F3")
            + " / lStep2=" + lStep2.ToString("F3")
            + " / lStep3=" + lStep3.ToString("F3")
            + " / lStep4=" + lStep4.ToString("F3")
            + " / lStep5=" + lStep5.ToString("F3")
            + " / lStep6=" + lStep6.ToString("F3")
            + " / lStep7=" + lStep7.ToString("F3")
            + " / rStep1=" + rStep1.ToString("F3")
            + " / rStep2=" + rStep2.ToString("F3")
            + " / rStep3=" + rStep3.ToString("F3")
            + " / rStep4=" + rStep4.ToString("F3")
            + " / rStep5=" + rStep5.ToString("F3")
            + " / rStep6=" + rStep6.ToString("F3")
            + " / rStep7=" + rStep7.ToString("F3")
            + " / lAxis=" + lAxis
            + " / rAxis=" + rAxis
            + " / source=" + source);
        UpdateStatus("Thigh side test running / both");

        LogCover("Thigh side smooth path / build=v083 / mode=BothHandsToThighs / order=forward-step1-to-step7 / interpolation=catmull-rom");
        yield return StartCoroutine(MoveBothHandsAlongSelfHipSmoothPath(lSnap, rSnap, lStart, rStart, false, "forward"));

        float holdT = 0.0f;
        while (holdT < ThighSideHoldSeconds)
        {
            holdT += Time.deltaTime;
            TryGetSelfThighSideHandTargetStep7ThighMidBack(true, out lThigh, out lStep7, out lSide, out lAxis);
            TryGetSelfThighSideHandTargetStep7ThighMidBack(false, out rThigh, out rStep7, out rSide, out rAxis);
            SetControllerPosition(lHandControl, lStep7);
            SetControllerPosition(rHandControl, rStep7);
            yield return null;
        }

        LogCover("Thigh side smooth return / build=v083 / mode=BothHandsToThighs / order=step7-to-step1-to-original / interpolation=catmull-rom");
        yield return StartCoroutine(MoveBothHandsAlongSelfHipSmoothPath(lSnap, rSnap, GetControllerPosition(lHandControl), GetControllerPosition(rHandControl), true, "return"));

        RestoreController(lSnap);
        RestoreController(rSnap);
        RestoreController(lElbowSnap);
        RestoreController(rElbowSnap);
        ClearLifeLockForController(lHandControl);
        ClearLifeLockForController(rHandControl);
        ClearLifeLockForController(lElbowControl);
        ClearLifeLockForController(rElbowControl);
        activeCoverHandSnapshot = null;
        activeCoverElbowSnapshot = null;
        lifeGestureRoutine = null;
        UpdateStatus("Hip/Thigh side step route done / both");
        ScheduleNextGesture("thigh-side-done");
    }

    void RequestFixedCoverTarget(string targetLabel, Vector3 targetPos, string source)
    {
        ResolveControllers();
        FreeControllerV3 hand = PickHandForCover();
        if (hand == null)
        {
            UpdateStatus("Cover test skipped: no available hand / " + targetLabel);
            LogCover("Cover test skipped / target=" + targetLabel + " / reason=no-hand / source=" + source);
            return;
        }

        float selectedDist = Vector3.Distance(GetControllerPosition(hand), targetPos);
        float selectedReach = EffectiveCoverMaxDistanceForTarget(targetLabel);
        string selectedPlan = selectedDist > selectedReach ? "stretch-to-reach" : "direct";
        LogCover("Cover test selected / hand=" + GetHandLabel(hand)
            + " / target=" + targetLabel
            + " / dist=" + selectedDist.ToString("F3", CultureInfo.InvariantCulture)
            + " / reach=" + selectedReach.ToString("F3", CultureInfo.InvariantCulture)
            + " / plan=" + selectedPlan
            + " / targetPos=" + targetPos.ToString("F3")
            + " / source=" + source);

        StopLifeGesture(source + ":before-cover-test");
        if (IsSelfHipCoverLabel(targetLabel))
        {
            // v081: test Self Hip uses the same completed hip step path as the live Self Hip cover.
            lifeGestureRoutine = StartCoroutine(MoveHandToSelfThighSideRoutine(hand == lHandControl, "self-hip-test:" + source));
        }
        else
        {
            lifeGestureRoutine = StartCoroutine(RandomCoverRoutine(hand, targetPos, targetLabel, source));
        }
    }

    bool IsSelfHipCoverLabel(string label)
    {
        return !string.IsNullOrEmpty(label) && label.Equals("Self Hip", StringComparison.OrdinalIgnoreCase);
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
            if (elbow != null && elbowSnap != null) SetControllerPosition(elbow, Vector3.Lerp(elbowFrom, elbowSnap.position, Smoother01(e)));
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

        bool headCoverTarget = IsHeadCoverTargetLabel(targetLabel);
        float surfaceOffset = EffectiveCoverSurfaceOffsetForTarget(targetLabel);
        Vector3 rawGoal = targetPos + outward * surfaceOffset;
        Vector3 goal = rawGoal;
        Vector3 delta = goal - start;
        float rawDist = delta.magnitude;
        float maxDist = EffectiveCoverMaxDistanceForTarget(targetLabel);
        bool stretchToReach = delta.magnitude > maxDist;
        Vector3 dir = delta.sqrMagnitude > 0.0001f ? delta.normalized : outward;
        float reachRatio = 1.0f;
        if (delta.sqrMagnitude > 0.0001f)
        {
            // v051: do not give up or stop short. Even when the target is beyond the old reach
            // budget, command the IK all the way to the requested surface goal and let VaM solve
            // the actual limb extension. A short final snap after the soft path makes the selected
            // cover point visibly reached instead of hovering at maxDist or looseReach.
            goal = rawGoal;
            delta = goal - start;
            dir = delta.sqrMagnitude > 0.0001f ? delta.normalized : outward;
        }
        LogCover("Cover move plan / hand=" + GetHandLabel(hand)
            + " / target=" + targetLabel
            + " / rawDist=" + rawDist.ToString("F3", CultureInfo.InvariantCulture)
            + " / reach=" + maxDist.ToString("F3", CultureInfo.InvariantCulture)
            + " / surface=" + surfaceOffset.ToString("F3", CultureInfo.InvariantCulture)
            + " / far=" + (stretchToReach ? "1" : "0")
            + " / reachRatio=" + reachRatio.ToString("F2", CultureInfo.InvariantCulture)
            + " / headReach=" + (headCoverTarget ? "1" : "0")
            + " / headTouch=" + (headCoverTarget ? "1" : "0")
            + " / noGiveUp=1"
            + " / finalDist=" + delta.magnitude.ToString("F3", CultureInfo.InvariantCulture));

        try { hand.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { hand.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        Vector3 elbowStart = elbowSnap != null ? elbowSnap.position : Vector3.zero;
        Quaternion elbowStartRot = elbowSnap != null ? elbowSnap.rotation : Quaternion.identity;
        Vector3 coverArc = BuildSoftCoverArc(start, goal, hand);
        bool selfFaceChestAvoidApplied = false;
        Vector3 chestAvoidArc;
        if (TryBuildSelfFaceChestAvoidArc(hand, start, goal, targetLabel, out chestAvoidArc))
        {
            selfFaceChestAvoidApplied = true;
            coverArc += chestAvoidArc;
            LogCover("Cover chest avoid / hand=" + GetHandLabel(hand)
                + " / target=" + targetLabel
                + " / side=" + GetHandSideForCover(hand).ToString(CultureInfo.InvariantCulture)
                + " / arc=" + chestAvoidArc.ToString("F3"));
        }
        Vector3 pathArc = coverArc;
        Vector3 elbowArc = coverArc;
        Vector3 leftHeadFrontLane;
        bool leftHeadFrontLaneApplied = TryBuildSelfHeadLeftFrontLane(hand, targetLabel, out leftHeadFrontLane);
        if (leftHeadFrontLaneApplied)
        {
            // v096: L Hand -> Self Head/Face/Mouth only.
            // Do not move the final head goal. Put only the outbound hand path and lElbow lane
            // in front of the breast so the route avoids the chest the same way the R side already does.
            pathArc += leftHeadFrontLane;
            elbowArc += leftHeadFrontLane * 1.35f;
            LogCover("Cover head left front lane / build=v096"
                + " / hand=" + GetHandLabel(hand)
                + " / target=" + targetLabel
                + " / lane=" + leftHeadFrontLane.ToString("F3")
                + " / pathArc=" + pathArc.ToString("F3")
                + " / elbowArc=" + elbowArc.ToString("F3"));
        }

        Vector3 elbowGoal = elbowStart;
        bool looseFinalElbow = false;
        Vector3 elbowFinalLooseGoal = elbowStart;
        if (elbow != null)
        {
            elbowGoal = elbowStart + (delta * CoverElbowNudgeScale) + (elbowArc * CoverElbowArcScale);
            looseFinalElbow = selfFaceChestAvoidApplied && IsSelfFaceChestAvoidTargetLabel(targetLabel);
            elbowFinalLooseGoal = looseFinalElbow
                ? Vector3.Lerp(elbowStart, elbowGoal, SelfHeadElbowFinalLooseBlend)
                : elbowGoal;
            try { elbow.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
            try { elbow.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

            if (looseFinalElbow)
            {
                LogCover("Cover head elbow loose final / build=v085"
                    + " / hand=" + GetHandLabel(hand)
                    + " / target=" + targetLabel
                    + " / blend=" + SelfHeadElbowFinalLooseBlend.ToString("F2", CultureInfo.InvariantCulture)
                    + " / elbowGoal=" + elbowGoal.ToString("F3")
                    + " / looseGoal=" + elbowFinalLooseGoal.ToString("F3"));
            }
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
        float looseDrift = headCoverTarget
            ? HeadCoverHoldDrift * MotionScale()
            : UnityEngine.Random.Range(0.006f, 0.020f) * MotionScale();
        float sideDrift = headCoverTarget
            ? UnityEngine.Random.Range(-HeadCoverSideDrift, HeadCoverSideDrift) * MotionScale()
            : UnityEngine.Random.Range(-0.010f, 0.014f) * MotionScale();
        // v052: do not intentionally hover off non-free targets.
        // The selected cover point itself is the move/snap/hold anchor.
        // Organic sway is applied only as a small post-snap hold vibration below.
        Vector3 headExactGoal = rawGoal;
        Vector3 holdAnchor = goal;
        Vector3 prepare = start - dir * Mathf.Min(0.030f, delta.magnitude * CoverPrepareBackScale) + pathArc * CoverPrepareArcScale;
        Vector3 c1 = start + pathArc * 1.05f - dir * 0.020f;
        Vector3 c2 = holdAnchor + pathArc * 0.38f - dir * Mathf.Min(0.055f, delta.magnitude * 0.16f);

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
        if (elbow != null)
        {
            if (looseFinalElbow)
            {
                SetControllerPosition(elbow, elbowFinalLooseGoal);
                try { elbow.currentPositionState = FreeControllerV3.PositionState.Comply; } catch { }
            }
            else
            {
                SetControllerPosition(elbow, elbowGoal);
            }
        }
        lastAppliedPos = currentHoldAnchor;
        lastAppliedRot = GetControllerRotation(hand);

        // v088: Target* cover is a reach gesture, not a target-side snap/attach.
        // It still stretches toward the selected target point, but the final snap is reserved
        // for returning the hand IK controller back to the captured self hand position.
        bool targetCoverLabel = IsTargetCoverLabel(targetLabel);
        if (!targetCoverLabel)
        {
            Vector3 snapStart = currentHoldAnchor;
            Vector3 snapGoal = goal;
            float snapSeconds = headCoverTarget ? HeadCoverSnapSeconds : CoverFinalSnapSeconds;
            float snapDist = Vector3.Distance(snapStart, snapGoal);
            if (snapDist > 0.0025f)
            {
                LogCover("Cover final snap / hand=" + GetHandLabel(hand)
                    + " / target=" + targetLabel
                    + " / head=" + (headCoverTarget ? "1" : "0")
                    + " / dist=" + snapDist.ToString("F3", CultureInfo.InvariantCulture)
                    + " / seconds=" + snapSeconds.ToString("F2", CultureInfo.InvariantCulture));

                float snapT = 0.0f;
                while (snapT < snapSeconds)
                {
                    if (IsPoseChangeSafeOn() && ControllerExternallyMovedPositionOnly(hand, lastAppliedPos))
                    {
                        AbortGestureForPoseChange(snap, elbowSnap, "cover:final-snap");
                        yield break;
                    }
                    snapT += Time.deltaTime;
                    float e = Smoother01(snapT / Mathf.Max(0.001f, snapSeconds));
                    Vector3 applyPos = Vector3.Lerp(snapStart, snapGoal, e);
                    SetControllerPosition(hand, applyPos);
                    if (elbow != null && !looseFinalElbow) SetControllerPosition(elbow, elbowGoal);
                    lastAppliedPos = applyPos;
                    lastAppliedRot = GetControllerRotation(hand);
                    yield return null;
                }
            }

            SetControllerPosition(hand, snapGoal);
            if (elbow != null && !looseFinalElbow) SetControllerPosition(elbow, elbowGoal);
            currentHoldAnchor = snapGoal;
            lastAppliedPos = currentHoldAnchor;
            lastAppliedRot = GetControllerRotation(hand);
            LogCover("Cover snap verify / hand=" + GetHandLabel(hand)
                + " / target=" + targetLabel
                + " / toTarget=" + Vector3.Distance(GetControllerPosition(hand), targetPos).ToString("F3", CultureInfo.InvariantCulture)
                + " / toGoal=" + Vector3.Distance(GetControllerPosition(hand), snapGoal).ToString("F3", CultureInfo.InvariantCulture)
                + " / goal=" + snapGoal.ToString("F3")
                + " / targetPos=" + targetPos.ToString("F3"));
        }
        else
        {
            // The move phase already ended at holdAnchor/goal. Keep the reach gesture, but do not
            // add an extra target-side snap. The hand IK will be return-snapped after hold.
            currentHoldAnchor = GetControllerPosition(hand);
            lastAppliedPos = currentHoldAnchor;
            lastAppliedRot = GetControllerRotation(hand);
            LogCover("Cover target reach / hand=" + GetHandLabel(hand)
                + " / target=" + targetLabel
                + " / targetFinalSnap=0"
                + " / returnSnap=1"
                + " / toTarget=" + Vector3.Distance(GetControllerPosition(hand), targetPos).ToString("F3", CultureInfo.InvariantCulture)
                + " / toGoal=" + Vector3.Distance(GetControllerPosition(hand), goal).ToString("F3", CultureInfo.InvariantCulture)
                + " / goal=" + goal.ToString("F3")
                + " / targetPos=" + targetPos.ToString("F3"));
        }

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
            float headSwayScale = headCoverTarget ? HeadCoverHoldSwayScale : CoverTouchHoldSwayScale;
            float wave = Mathf.Sin(holdT * 5.1f) * CoverHoldSwayAmount * headSwayScale * MotionScale() * fade;
            float wave2 = Mathf.Sin(holdT * 3.3f + 1.9f) * CoverHoldSwayAmount * 0.50f * headSwayScale * MotionScale() * fade;
            Vector3 applyPos = currentHoldAnchor + swayA * wave + swayB * handSide * wave2;
            SetControllerPosition(hand, applyPos);
            if (elbow != null && !looseFinalElbow) SetControllerPosition(elbow, elbowGoal + swayA * wave * 0.35f);
            lastAppliedPos = applyPos;
            lastAppliedRot = GetControllerRotation(hand);
            yield return null;
        }

        Vector3 from = GetControllerPosition(hand);
        Vector3 elbowFrom = elbow != null ? GetControllerPosition(elbow) : Vector3.zero;
        if (elbow != null && looseFinalElbow)
        {
            try { elbow.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
            try { elbow.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
            elbowFrom = GetControllerPosition(elbow);
        }
        Vector3 looseReturn = targetCoverLabel ? snap.position : BuildLooseReturnPosition(snap.position, start, hand);
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
            if (elbow != null && elbowSnap != null) SetControllerPosition(elbow, Vector3.Lerp(elbowFrom, elbowSnap.position, Smoother01(e)));
            lastAppliedPos = applyPos;
            lastAppliedRot = GetControllerRotation(hand);
            yield return null;
        }

        Vector3 returnSnapStart = GetControllerPosition(hand);
        Vector3 returnSnapGoal = targetCoverLabel ? snap.position : looseReturn;
        Vector3 elbowReturnSnapStart = (elbow != null && elbowSnap != null) ? GetControllerPosition(elbow) : Vector3.zero;
        float returnSnapDist = Vector3.Distance(returnSnapStart, returnSnapGoal);
        if (returnSnapDist > 0.0015f)
        {
            LogCover("Cover return snap / hand=" + GetHandLabel(hand)
                + " / target=" + targetLabel
                + " / targetCover=" + (targetCoverLabel ? "1" : "0")
                + " / dist=" + returnSnapDist.ToString("F3", CultureInfo.InvariantCulture)
                + " / seconds=" + CoverReturnSnapSeconds.ToString("F2", CultureInfo.InvariantCulture)
                + " / goal=" + returnSnapGoal.ToString("F3"));

            float returnSnapT = 0.0f;
            while (returnSnapT < CoverReturnSnapSeconds)
            {
                if (IsPoseChangeSafeOn() && ControllerExternallyMovedPositionOnly(hand, lastAppliedPos))
                {
                    AbortGestureForPoseChange(snap, elbowSnap, "cover:return-snap");
                    yield break;
                }
                returnSnapT += Time.deltaTime;
                float e = Smoother01(returnSnapT / Mathf.Max(0.001f, CoverReturnSnapSeconds));
                Vector3 applyPos = Vector3.Lerp(returnSnapStart, returnSnapGoal, e);
                SetControllerPosition(hand, applyPos);
                if (elbow != null && elbowSnap != null) SetControllerPosition(elbow, Vector3.Lerp(elbowReturnSnapStart, elbowSnap.position, Smoother01(e)));
                lastAppliedPos = applyPos;
                lastAppliedRot = GetControllerRotation(hand);
                yield return null;
            }
        }

        SetControllerPosition(hand, returnSnapGoal);
        lastAppliedPos = returnSnapGoal;
        lastAppliedRot = GetControllerRotation(hand);

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


    bool IsSelfFaceChestAvoidTargetLabel(string label)
    {
        if (string.IsNullOrEmpty(label)) return false;
        return label.IndexOf("Self Head", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Self Face", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Self Mouth", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    bool IsHeadCoverTargetLabel(string label)
    {
        if (string.IsNullOrEmpty(label)) return false;
        return label.IndexOf("Head", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Face", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Mouth", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Neck", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    float EffectiveCoverSurfaceOffsetForTarget(string label)
    {
        if (IsHeadCoverTargetLabel(label))
            return HeadCoverSurfaceOffset;
        // v052: for Chest/Belly/Hip, do not stop at a generic near-surface offset.
        // Move the IK to the actual selected cover point, then snap/hold there.
        return 0.0f;
    }

    float EffectiveCoverMaxDistanceForTarget(string label)
    {
        float baseReach = EffectiveCoverMaxDistance();
        if (string.IsNullOrEmpty(label)) return baseReach;

        if (label.IndexOf("Self Head", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Self Face", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Self Mouth", StringComparison.OrdinalIgnoreCase) >= 0)
            return Mathf.Max(baseReach, SelfHeadCoverMaxDistance);

        if (label.IndexOf("Target Head", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Target Face", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Target Mouth", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Target Neck", StringComparison.OrdinalIgnoreCase) >= 0)
            return Mathf.Max(baseReach, TargetHeadCoverMaxDistance);

        return baseReach;
    }

    int GetHandSideForCover(FreeControllerV3 hand)
    {
        if (hand == null) return 1;
        if (lHandControl != null && hand == lHandControl) return -1;
        if (rHandControl != null && hand == rHandControl) return 1;
        string name = hand.name != null ? hand.name.ToLowerInvariant() : "";
        if (name.IndexOf("lhand") >= 0 || name.IndexOf("left") >= 0) return -1;
        return 1;
    }

    bool IsLeftHandController(FreeControllerV3 hand)
    {
        if (hand == null) return false;
        if (lHandControl != null && hand == lHandControl) return true;
        string n = hand.name != null ? hand.name.ToLowerInvariant() : "";
        return n.IndexOf("lhand") >= 0 || n.IndexOf("left") >= 0;
    }

    bool TryBuildSelfHeadLeftFrontLane(FreeControllerV3 hand, string targetLabel, out Vector3 frontLane)
    {
        frontLane = Vector3.zero;
        if (!IsLeftHandController(hand)) return false;
        if (!IsSelfFaceChestAvoidTargetLabel(targetLabel)) return false;
        if (chestControl == null) return false;

        Vector3 chestPos = GetControllerPosition(chestControl);
        Vector3 bodyForward = containingAtom != null && containingAtom.transform != null
            ? containingAtom.transform.forward
            : Vector3.forward;
        bodyForward.y = 0.0f;
        if (bodyForward.sqrMagnitude < 0.0001f) bodyForward = Vector3.forward;
        bodyForward.Normalize();

        float protrusion;
        float halfWidth;
        float sign;
        Vector3 breastCenter;
        bool measured = TryMeasureSelfBreastForCover(bodyForward, chestPos, out protrusion, out halfWidth, out sign, out breastCenter);

        Vector3 breastForward = Vector3.zero;
        if (measured)
        {
            // Prefer the actual chest-to-breast direction for the L side front lane.
            // This is only an outbound path/elbow lane offset; final head goal remains unchanged.
            breastForward = breastCenter - chestPos;
            if (breastForward.sqrMagnitude > 0.0001f)
            {
                // Remove most of the vertical component so this stays a "front lane" and does not lift/drop the elbow too much.
                breastForward.y *= 0.25f;
            }
        }

        if (breastForward.sqrMagnitude < 0.0001f)
        {
            breastForward = bodyForward * (measured && sign < 0.0f ? -1.0f : 1.0f);
        }

        if (breastForward.sqrMagnitude < 0.0001f) return false;
        breastForward.Normalize();

        float amount = measured
            ? Mathf.Clamp(0.060f + protrusion * 0.70f, 0.080f, 0.180f)
            : 0.095f;

        frontLane = breastForward * amount;
        return frontLane.sqrMagnitude > 0.0001f;
    }

    bool TryBuildSelfFaceChestAvoidArc(FreeControllerV3 hand, Vector3 from, Vector3 to, string targetLabel, out Vector3 avoidArc)
    {
        avoidArc = Vector3.zero;
        if (!IsSelfFaceChestAvoidTargetLabel(targetLabel)) return false;
        if (hand == null || chestControl == null) return false;

        Vector3 delta = to - from;
        if (delta.sqrMagnitude < 0.0025f) return false;

        Vector3 forward = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.forward : Vector3.forward;
        Vector3 right = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.right : Vector3.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
        if (right.sqrMagnitude < 0.0001f) right = Vector3.right;
        forward.Normalize();
        right.Normalize();

        Vector3 chestPos = GetControllerPosition(chestControl);
        float breastProtrusion;
        float breastHalfWidth;
        float breastSign;
        Vector3 breastCenter;
        bool breastMeasured = TryMeasureSelfBreastForCover(forward, chestPos, out breastProtrusion, out breastHalfWidth, out breastSign, out breastCenter);

        Vector3 breastForward = forward;
        float avoidRadius;
        Vector3 chestProbe;
        float breastStrength = 0.0f;

        if (breastMeasured)
        {
            if (breastProtrusion < SelfFaceBreastAvoidProtrusionStart)
            {
                // Flat/small chest: keep the old direct face-cover path even if the geometric line passes near chestControl.
                return false;
            }

            breastStrength = Mathf.Clamp01((breastProtrusion - SelfFaceBreastAvoidProtrusionStart) / Mathf.Max(0.001f, SelfFaceBreastAvoidProtrusionFull - SelfFaceBreastAvoidProtrusionStart));
            breastForward = forward * (breastSign >= 0.0f ? 1.0f : -1.0f);
            float probeForward = Mathf.Clamp(SelfFaceChestAvoidChestForwardOffset + breastProtrusion * 0.72f, 0.070f, 0.230f);
            avoidRadius = Mathf.Clamp(SelfFaceChestAvoidRadius + (breastProtrusion - SelfFaceBreastAvoidProtrusionStart) * 0.95f + breastHalfWidth * 0.05f, 0.150f, SelfFaceBreastAvoidMaxRadius);
            chestProbe = chestPos + breastForward * probeForward + Vector3.up * 0.025f;
        }
        else
        {
            // No nipple controls found. Keep a conservative path-crossing fallback so old scenes do not lose avoidance completely.
            breastForward = forward;
            avoidRadius = SelfFaceBreastAvoidFallbackRadius;
            chestProbe = chestPos + breastForward * SelfFaceChestAvoidChestForwardOffset + Vector3.up * 0.025f;
        }

        float bodyTiltBoost = SelfFaceChestAvoidTiltBoost();
        if (bodyTiltBoost > 0.001f)
        {
            // Tilted upper-body poses make the simple hand-to-head segment more likely to graze the chest,
            // so broaden the detection radius before the segment test and later add strength to the avoid arc.
            avoidRadius = Mathf.Min(SelfFaceBreastAvoidMaxRadius, avoidRadius + Mathf.Lerp(0.000f, 0.085f, bodyTiltBoost));
        }

        float along;
        float dist = DistancePointToSegment(chestProbe, from, to, out along);
        if (along < 0.08f || along > 0.92f) return false;
        if (dist > avoidRadius) return false;

        float closeness = Mathf.Clamp01((avoidRadius - dist) / Mathf.Max(0.001f, avoidRadius));
        float strength = breastMeasured
            ? Mathf.Lerp(0.70f, 1.45f, Mathf.Max(closeness, breastStrength))
            : Mathf.Lerp(0.55f, 1.05f, closeness);
        if (bodyTiltBoost > 0.001f)
            strength += 0.30f * bodyTiltBoost;
        strength = Mathf.Clamp(strength, 0.0f, 1.75f);
        int side = GetHandSideForCover(hand);
        avoidArc = right * side * SelfFaceChestAvoidSideOffset * strength
            + Vector3.up * SelfFaceChestAvoidUpOffset * strength
            + breastForward * SelfFaceChestAvoidForwardOffset * strength;

        LogCover("Cover chest avoid / hand=" + GetHandLabel(hand)
            + " / target=" + targetLabel
            + " / measured=" + (breastMeasured ? "1" : "0")
            + " / protrusion=" + breastProtrusion.ToString("F3", CultureInfo.InvariantCulture)
            + " / halfWidth=" + breastHalfWidth.ToString("F3", CultureInfo.InvariantCulture)
            + " / radius=" + avoidRadius.ToString("F3", CultureInfo.InvariantCulture)
            + " / tiltBoost=" + bodyTiltBoost.ToString("F2", CultureInfo.InvariantCulture)
            + " / dist=" + dist.ToString("F3", CultureInfo.InvariantCulture)
            + " / arc=" + avoidArc.ToString("F3"));
        return avoidArc.sqrMagnitude > 0.0001f;
    }

    float SelfFaceChestAvoidTiltBoost()
    {
        Transform t = chestControl != null ? chestControl.transform : null;
        if (t == null && containingAtom != null) t = containingAtom.transform;
        if (t == null) return 0.0f;

        float angle = Vector3.Angle(t.up, Vector3.up);
        // Start boosting after a mild lean. Full boost around a strong lean.
        return Mathf.Clamp01((angle - 8.0f) / 32.0f);
    }

    Vector3 SelfHeadCoverForwardDir()
    {
        Vector3 f = Vector3.forward;
        if (headControl != null && headControl.transform != null) f = headControl.transform.forward;
        else if (containingAtom != null && containingAtom.transform != null) f = containingAtom.transform.forward;
        if (f.sqrMagnitude < 0.0001f) f = Vector3.forward;
        return f.normalized;
    }

    bool TryMeasureSelfBreastForCover(Vector3 bodyForward, Vector3 chestPos, out float protrusion, out float halfWidth, out float sign, out Vector3 breastCenter)
    {
        protrusion = 0.0f;
        halfWidth = 0.0f;
        sign = 1.0f;
        breastCenter = Vector3.zero;

        Vector3 left;
        Vector3 right;
        if (!TryGetSelfNipplePositions(out left, out right)) return false;

        Vector3 f = bodyForward;
        f.y = 0.0f;
        if (f.sqrMagnitude < 0.0001f) f = Vector3.forward;
        f.Normalize();

        breastCenter = (left + right) * 0.5f;
        halfWidth = Vector3.Distance(left, right) * 0.5f;

        float signedCenter = Vector3.Dot(breastCenter - chestPos, f);
        float signedLeft = Vector3.Dot(left - chestPos, f);
        float signedRight = Vector3.Dot(right - chestPos, f);
        protrusion = Mathf.Max(Mathf.Abs(signedCenter), Mathf.Max(Mathf.Abs(signedLeft), Mathf.Abs(signedRight)));

        if (Mathf.Abs(signedCenter) > 0.001f) sign = signedCenter >= 0.0f ? 1.0f : -1.0f;
        else
        {
            float signedMax = Mathf.Abs(signedLeft) >= Mathf.Abs(signedRight) ? signedLeft : signedRight;
            sign = signedMax >= 0.0f ? 1.0f : -1.0f;
        }

        return protrusion > 0.001f || halfWidth > 0.001f;
    }

    bool TryGetSelfNipplePositions(out Vector3 left, out Vector3 right)
    {
        left = Vector3.zero;
        right = Vector3.zero;
        if (containingAtom == null) return false;

        bool hasLeft = TryGetControllerPoint(containingAtom, out left,
            "lNippleControl", "leftNippleControl", "lNipple", "lnipple", "leftNipple", "LeftNipple", "nipple_l", "nippleL");
        bool hasRight = TryGetControllerPoint(containingAtom, out right,
            "rNippleControl", "rightNippleControl", "rNipple", "rnipple", "rightNipple", "RightNipple", "nipple_r", "nippleR");

        return hasLeft && hasRight;
    }

    float DistancePointToSegment(Vector3 point, Vector3 a, Vector3 b, out float along01)
    {
        Vector3 ab = b - a;
        float lenSq = ab.sqrMagnitude;
        if (lenSq < 0.000001f)
        {
            along01 = 0.0f;
            return Vector3.Distance(point, a);
        }

        along01 = Mathf.Clamp01(Vector3.Dot(point - a, ab) / lenSq);
        Vector3 closest = a + ab * along01;
        return Vector3.Distance(point, closest);
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

    bool IsTargetCoverLabel(string label)
    {
        return !string.IsNullOrEmpty(label) && label.IndexOf("Target", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    bool IsTargetCoverSuppressedForMutualBack(Atom target, out string reason)
    {
        reason = "";
        if (target == null || containingAtom == null || containingAtom.transform == null || target.transform == null)
            return false;

        Vector3 selfPos = containingAtom.transform.position;
        Vector3 targetPos = target.transform.position;
        Vector3 selfToTarget = targetPos - selfPos;
        selfToTarget.y = 0.0f;
        if (selfToTarget.sqrMagnitude < 0.0001f)
            return false;
        selfToTarget.Normalize();
        Vector3 targetToSelf = -selfToTarget;

        Vector3 selfForward = containingAtom.transform.forward;
        selfForward.y = 0.0f;
        if (selfForward.sqrMagnitude < 0.0001f)
            return false;
        selfForward.Normalize();

        Vector3 targetForward = target.transform.forward;
        targetForward.y = 0.0f;
        if (targetForward.sqrMagnitude < 0.0001f)
            return false;
        targetForward.Normalize();

        float selfDot = Vector3.Dot(selfForward, selfToTarget);
        float targetDot = Vector3.Dot(targetForward, targetToSelf);
        bool mutualBack = selfDot <= TargetCoverMutualBackSuppressDot && targetDot <= TargetCoverMutualBackSuppressDot;
        reason = "selfDot=" + selfDot.ToString("F2", CultureInfo.InvariantCulture)
            + "/targetDot=" + targetDot.ToString("F2", CultureInfo.InvariantCulture)
            + "/mutualBack=" + (mutualBack ? "1" : "0");
        return mutualBack;
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
        // v089: Life cover should not target Head/Chest/Hip IK centers.
        // Use shoulder / upper chest / belly / thigh body-surface estimates instead.
        string coverState = CurrentLifeState();
        string coverPersonality = CurrentLifePersonality();
        string coverAffection = CurrentLifeAffection();
        float selfShoulderWeight = 4.0f;
        float selfUpperChestWeight = 2.0f;
        float selfBellyWeight = 2.0f;
        float selfThighWeight = 2.0f;
        float selfFreeWeight = 1.0f;
        float selfHipFidgetWeight = 0.0f;

        // v102: Sleeping/Quiet should feel alive, but must remain self-only.
        // Favor Free Hand and Self Hip fidget paths over target/upper-body reaching.
        if (coverState == LifeStateSleeping)
        {
            selfShoulderWeight = 1.0f;
            selfUpperChestWeight = 0.8f;
            selfBellyWeight = 2.2f;
            selfThighWeight = 4.2f;
            selfFreeWeight = 6.0f;
            selfHipFidgetWeight = 3.2f;
        }
        else if (coverState == LifeStateQuiet)
        {
            selfShoulderWeight = 2.0f;
            selfUpperChestWeight = 1.4f;
            selfBellyWeight = 2.8f;
            selfThighWeight = 4.0f;
            selfFreeWeight = 4.0f;
            selfHipFidgetWeight = 4.0f;
        }
        else if (coverAffection == LifeAffectionShy || coverState == LifeStateShy || coverPersonality == LifePersonalityShy)
        {
            selfShoulderWeight = 2.2f;
            selfUpperChestWeight = 1.5f;
            selfBellyWeight = 3.0f;
            selfThighWeight = 4.2f;
            selfFreeWeight = 4.5f;
            selfHipFidgetWeight = 4.5f;
        }
        else if (coverPersonality == LifePersonalityBold)
        {
            selfShoulderWeight = 4.5f;
            selfUpperChestWeight = 2.4f;
            selfBellyWeight = 2.2f;
            selfThighWeight = 2.0f;
            selfFreeWeight = 0.8f;
            selfHipFidgetWeight = 1.2f;
        }

        if (coverState != LifeStateSleeping && coverState != LifeStateQuiet)
        {
            if (coverAffection == LifeAffectionDislike)
            {
                selfThighWeight *= 1.15f;
                selfFreeWeight *= 1.35f;
                selfHipFidgetWeight = Mathf.Max(selfHipFidgetWeight, 2.8f);
            }
            else if (coverAffection == LifeAffectionShy)
            {
                selfThighWeight *= 1.18f;
                selfFreeWeight *= 1.45f;
                selfHipFidgetWeight = Mathf.Max(selfHipFidgetWeight, 4.0f);
            }
            else if (coverAffection == LifeAffectionLike)
            {
                selfFreeWeight *= 0.72f;
                selfHipFidgetWeight *= 0.78f;
            }
        }

        if (TryGetSelfShoulderPoint(-1, out p)) { selfLabels.Add("Self L Shoulder"); selfPositions.Add(p); selfWeights.Add(selfShoulderWeight); }
        if (TryGetSelfShoulderPoint(1, out p)) { selfLabels.Add("Self R Shoulder"); selfPositions.Add(p); selfWeights.Add(selfShoulderWeight); }
        if (TryGetSelfUpperChestSurfacePoint(out p)) { selfLabels.Add("Self UpperChest Surface"); selfPositions.Add(p); selfWeights.Add(selfUpperChestWeight); }
        if (TryGetSelfBellySurfacePoint(out p)) { selfLabels.Add("Self Belly Surface"); selfPositions.Add(p); selfWeights.Add(selfBellyWeight); }
        if (TryGetSelfThighSurfacePoint(out p)) { selfLabels.Add("Self Thigh Surface"); selfPositions.Add(p); selfWeights.Add(selfThighWeight); }
        if (selfHipFidgetWeight > 0.001f) { selfLabels.Add("Self Hip"); selfPositions.Add(Vector3.zero); selfWeights.Add(selfHipFidgetWeight); }
        selfLabels.Add("Free Hand"); selfPositions.Add(Vector3.zero); selfWeights.Add(selfFreeWeight);

        Atom target = GetSelectedTargetPerson();
        string targetSuppressReason = "";
        bool stateSelfOnlyCover = coverState == LifeStateSleeping || coverState == LifeStateQuiet;
        bool suppressTargetCover = stateSelfOnlyCover || IsTargetCoverSuppressedForMutualBack(target, out targetSuppressReason);
        if (stateSelfOnlyCover) targetSuppressReason = "state-self-only";
        if (target != null && !suppressTargetCover)
        {
            float targetWeightScale = 1.0f;
            if (coverAffection == LifeAffectionDislike) targetWeightScale = 0.55f;
            else if (coverAffection == LifeAffectionShy) targetWeightScale = 0.35f;
            else if (coverAffection == LifeAffectionLike) targetWeightScale = 1.25f;

            if (TryGetTargetShoulderPoint(target, -1, out p)) { targetLabels.Add("Target L Shoulder"); targetPositions.Add(p); targetWeights.Add(4.0f * targetWeightScale); }
            if (TryGetTargetShoulderPoint(target, 1, out p)) { targetLabels.Add("Target R Shoulder"); targetPositions.Add(p); targetWeights.Add(4.0f * targetWeightScale); }
            if (TryGetTargetUpperChestSurfacePoint(target, out p)) { targetLabels.Add("Target UpperChest Surface"); targetPositions.Add(p); targetWeights.Add(2.0f * targetWeightScale); }
            if (TryGetTargetBellySurfacePoint(target, out p)) { targetLabels.Add("Target Belly Surface"); targetPositions.Add(p); targetWeights.Add(2.0f * targetWeightScale); }
            if (TryGetTargetThighSurfacePoint(target, out p)) { targetLabels.Add("Target Thigh Surface"); targetPositions.Add(p); targetWeights.Add(2.0f * targetWeightScale); }
            targetLabels.Add("Free Hand"); targetPositions.Add(Vector3.zero); targetWeights.Add(Mathf.Max(0.25f, 1.0f * targetWeightScale));
        }
        else if (target != null && suppressTargetCover)
        {
            LogCover("Cover target group suppressed / reason=" + targetSuppressReason + " / fallback=SelfOrFree");
        }

        bool preferSelf = UnityEngine.Random.Range(0.0f, 100.0f) < EffectiveCoverSelfPercent();

        if (preferSelf && TryPickFromWeightedList(selfLabels, selfPositions, selfWeights, out pos, out label))
        {
            Log("Cover target group / Self / self%=" + EffectiveCoverSelfPercent().ToString("F0", CultureInfo.InvariantCulture)
                + " / weighted=surface-shoulder4x2 free1");
            return true;
        }
        if (!preferSelf && TryPickFromWeightedList(targetLabels, targetPositions, targetWeights, out pos, out label))
        {
            Log("Cover target group / Target / self%=" + EffectiveCoverSelfPercent().ToString("F0", CultureInfo.InvariantCulture)
                + " / weighted=surface-shoulder4x2 free1");
            return true;
        }

        // Fallback to the other group when the preferred side has no point.
        if (TryPickFromWeightedList(selfLabels, selfPositions, selfWeights, out pos, out label))
        {
            Log("Cover target group fallback / Self / weighted=surface-shoulder4x2 free1");
            return true;
        }
        if (TryPickFromWeightedList(targetLabels, targetPositions, targetWeights, out pos, out label))
        {
            Log("Cover target group fallback / Target / weighted=surface-shoulder4x2 free1");
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

    bool TryGetBodySurfacePoint(Atom atom, float height, float sideOffset, float forwardOffset, out Vector3 pos)
    {
        pos = Vector3.zero;
        if (atom == null) return false;

        Vector3 basePos;
        float heightFromBase;
        Vector3 forward;
        Vector3 right;
        string baseSource;
        if (!TryGetBodySurfaceBase(atom, height, out basePos, out heightFromBase, out forward, out right, out baseSource))
            return false;

        pos = basePos + Vector3.up * heightFromBase + right * sideOffset + forward * forwardOffset;

        if (debugLog != null && debugLog.val)
        {
            Log("Body surface point / atom=" + SafeAtomName(atom)
                + " / base=" + baseSource
                + " / height=" + height.ToString("F3", CultureInfo.InvariantCulture)
                + " / heightFromBase=" + heightFromBase.ToString("F3", CultureInfo.InvariantCulture)
                + " / side=" + sideOffset.ToString("F3", CultureInfo.InvariantCulture)
                + " / forward=" + forwardOffset.ToString("F3", CultureInfo.InvariantCulture)
                + " / pos=" + pos.ToString("F3"));
        }

        return true;
    }

    bool TryGetBodySurfaceBase(Atom atom, float nominalHeight, out Vector3 basePos, out float heightFromBase, out Vector3 forward, out Vector3 right, out string baseSource)
    {
        basePos = Vector3.zero;
        heightFromBase = nominalHeight;
        forward = Vector3.forward;
        right = Vector3.right;
        baseSource = "<none>";

        if (atom == null) return false;

        Transform basisTransform = null;

        // Prefer the Person's live root/main controller. atom.transform can remain near the
        // original Atom origin after pose/root handoff, which makes surface cover points appear
        // meters away from the visible body.
        try
        {
            if (atom.mainController != null && atom.mainController.transform != null)
            {
                basePos = atom.mainController.transform.position;
                basisTransform = atom.mainController.transform;
                heightFromBase = nominalHeight;
                baseSource = "mainController";
            }
        }
        catch { }

        if (baseSource == "<none>")
        {
            FreeControllerV3 hip = FindControllerByAliasesOnAtom(atom, "hipControl", "hip", "pelvisControl", "pelvis");
            if (hip != null)
            {
                basePos = GetControllerPosition(hip);
                basisTransform = atom.transform != null ? atom.transform : hip.transform;
                // Existing body-surface heights are roughly floor/root-relative. If we must use
                // hipControl as the base, convert them to hip-relative offsets instead of adding
                // the full shoulder/chest height above the hip.
                heightFromBase = nominalHeight - 0.90f;
                baseSource = "hipControl";
            }
        }

        if (baseSource == "<none>")
        {
            if (atom.transform == null) return false;
            basePos = atom.transform.position;
            basisTransform = atom.transform;
            heightFromBase = nominalHeight;
            baseSource = "atomTransform";
        }

        if (basisTransform != null)
        {
            forward = basisTransform.forward;
            right = basisTransform.right;
        }
        else if (atom.transform != null)
        {
            forward = atom.transform.forward;
            right = atom.transform.right;
        }

        forward.y = 0.0f;
        right.y = 0.0f;
        if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
        if (right.sqrMagnitude < 0.0001f) right = Vector3.right;
        forward.Normalize();
        right.Normalize();
        return true;
    }

    bool TryGetSelfShoulderPoint(int side, out Vector3 pos)
    {
        float s = side < 0 ? -0.22f : (side > 0 ? 0.22f : 0.0f);
        return TryGetBodySurfacePoint(containingAtom, 1.38f, s, 0.055f, out pos);
    }

    bool TryGetTargetShoulderPoint(Atom atom, int side, out Vector3 pos)
    {
        float s = side < 0 ? -0.22f : (side > 0 ? 0.22f : 0.0f);
        return TryGetBodySurfacePoint(atom, 1.38f, s, 0.055f, out pos);
    }

    bool TryGetSelfUpperChestSurfacePoint(out Vector3 pos)
    {
        return TryGetBodySurfacePoint(containingAtom, 1.24f, 0.0f, 0.065f, out pos);
    }

    bool TryGetTargetUpperChestSurfacePoint(Atom atom, out Vector3 pos)
    {
        return TryGetBodySurfacePoint(atom, 1.24f, 0.0f, 0.065f, out pos);
    }

    bool TryGetSelfBellySurfacePoint(out Vector3 pos)
    {
        return TryGetBodySurfacePoint(containingAtom, 1.02f, 0.0f, 0.060f, out pos);
    }

    bool TryGetTargetBellySurfacePoint(Atom atom, out Vector3 pos)
    {
        return TryGetBodySurfacePoint(atom, 1.02f, 0.0f, 0.060f, out pos);
    }

    bool TryGetSelfThighSurfacePoint(out Vector3 pos)
    {
        int side = UnityEngine.Random.value < 0.5f ? -1 : 1;
        return TryGetBodySurfacePoint(containingAtom, 0.72f, 0.13f * side, 0.030f, out pos);
    }

    bool TryGetTargetThighSurfacePoint(Atom atom, out Vector3 pos)
    {
        int side = UnityEngine.Random.value < 0.5f ? -1 : 1;
        return TryGetBodySurfacePoint(atom, 0.72f, 0.13f * side, 0.030f, out pos);
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
            pos = GetControllerPosition(headControl) + SelfHeadCoverForwardDir() * SelfHeadCoverPointForwardOffset;
            return true;
        }
        pos = containingAtom != null && containingAtom.transform != null
            ? containingAtom.transform.position + Vector3.up * 1.55f + containingAtom.transform.forward.normalized * SelfHeadCoverPointForwardOffset
            : Vector3.zero;
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
        StopSleepSettleComply(reason);
        StopSleepingEyeTransition(reason);
        RestoreAffectionMorphs(reason);
        UpdateStatus("Stopped / restored");
    }

    public void OnDestroy()
    {
        try { StopAllLife("destroy"); } catch { }
        try { SetBlinkSuppressMorphs(0.0f); } catch { }
        try { SetEyesClosedMorphs(0.0f); } catch { }
        try { RestoreAutoBlinkAfterSleeping("destroy"); } catch { }
        try { RestoreAffectionMorphs("destroy"); } catch { }
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

    void RestoreBreathController(ControllerSnapshot snap)
    {
        if (snap == null || snap.controller == null) return;
        // v089: Breath is rotation-only. Restore rotation/state, but never push chest
        // position back to the old snapshot position.
        SetControllerRotation(snap.controller, snap.rotation);
        try { snap.controller.currentPositionState = snap.positionState; } catch { }
        try { snap.controller.currentRotationState = snap.rotationState; } catch { }
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
        // v096: Sleeping still allows very low random LookAway/head drift,
        // but RequestLookTarget/RequestLookCamera are blocked separately.
        return lifeHeadLookEnabled == null || lifeHeadLookEnabled.val;
    }

    bool IsLegMotionEnabled()
    {
        // v102: Sleeping/Quiet use rotation-only thigh micro-motion for subtle fidgeting.
        return lifeLegMotionEnabled != null && lifeLegMotionEnabled.val && EffectiveLegScale() > 0.0001f;
    }

    bool IsLegPositionAssistEnabled()
    {
        if (CurrentLifeState() == LifeStateSleeping) return false;
        return lifeLegPositionAssistEnabled != null && lifeLegPositionAssistEnabled.val;
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
        string state = CurrentLifeState();
        if (state == LifeStateQuiet || state == LifeStateSleeping) return LifeMotionSmall;
        if (state == LifeStateActive) return LifeMotionLarge;

        if (lifeMotionMode == null || string.IsNullOrEmpty(lifeMotionMode.val)) return LifeMotionNormal;
        string v = lifeMotionMode.val;
        if (string.Equals(v, LifeMotionSmall, StringComparison.OrdinalIgnoreCase)) return LifeMotionSmall;
        if (string.Equals(v, LifeMotionLarge, StringComparison.OrdinalIgnoreCase)) return LifeMotionLarge;
        return LifeMotionNormal;
    }

    string CurrentLifeState()
    {
        if (lifeStateMode == null || string.IsNullOrEmpty(lifeStateMode.val)) return LifeStateNormal;
        return NormalizeLifeState(lifeStateMode.val);
    }

    void SetLifeState(string state, string source)
    {
        string next = NormalizeLifeState(state);
        suppressLifeStateCallback = true;
        try
        {
            if (lifeStateMode != null)
                lifeStateMode.val = next;
        }
        catch { }
        suppressLifeStateCallback = false;
        ApplyLifeStateChange(source);
    }

    bool IsLifeStateAlias(string value, string legacy, string display)
    {
        if (string.IsNullOrEmpty(value)) return false;
        if (string.Equals(value, display, StringComparison.OrdinalIgnoreCase)) return true;
        if (string.Equals(value, legacy, StringComparison.OrdinalIgnoreCase)) return true;
        return value.StartsWith(legacy + " ", StringComparison.OrdinalIgnoreCase)
            || value.StartsWith(legacy + ":", StringComparison.OrdinalIgnoreCase);
    }

    string NormalizeLifeState(string state)
    {
        if (IsLifeStateAlias(state, LifeStateQuietLegacy, LifeStateQuiet)) return LifeStateQuiet;
        if (IsLifeStateAlias(state, LifeStateShyLegacy, LifeStateShy)) return LifeStateNormal;
        if (IsLifeStateAlias(state, LifeStateActiveLegacy, LifeStateActive)) return LifeStateActive;
        if (IsLifeStateAlias(state, LifeStateSleepingLegacy, LifeStateSleeping)) return LifeStateSleeping;
        if (IsLifeStateAlias(state, LifeStateNormalLegacy, LifeStateNormal)) return LifeStateNormal;
        return LifeStateNormal;
    }

    string CurrentLifePersonality()
    {
        if (lifePersonalityMode == null || string.IsNullOrEmpty(lifePersonalityMode.val)) return LifePersonalityNormal;
        return NormalizeLifePersonality(lifePersonalityMode.val);
    }

    void SetLifePersonality(string personality, string source)
    {
        string next = NormalizeLifePersonality(personality);
        try
        {
            if (lifePersonalityMode != null)
                lifePersonalityMode.val = next;
        }
        catch { }
        ApplyLifePersonalityChange(source);
    }

    bool IsLifePersonalityAlias(string value, string legacy, string display)
    {
        if (string.IsNullOrEmpty(value)) return false;
        if (string.Equals(value, display, StringComparison.OrdinalIgnoreCase)) return true;
        if (string.Equals(value, legacy, StringComparison.OrdinalIgnoreCase)) return true;
        return value.StartsWith(legacy + " ", StringComparison.OrdinalIgnoreCase)
            || value.StartsWith(legacy + ":", StringComparison.OrdinalIgnoreCase);
    }

    string NormalizeLifePersonality(string personality)
    {
        if (IsLifePersonalityAlias(personality, LifePersonalityShyLegacy, LifePersonalityShy)) return LifePersonalityNormal;
        if (IsLifePersonalityAlias(personality, LifePersonalityBoldLegacy, LifePersonalityBold)) return LifePersonalityBold;
        if (IsLifePersonalityAlias(personality, LifePersonalityNormalLegacy, LifePersonalityNormal)) return LifePersonalityNormal;
        return LifePersonalityNormal;
    }

    void OnLifePersonalityChanged(string value)
    {
        ApplyLifePersonalityChange("ui");
    }

    void ApplyLifePersonalityChange(string source)
    {
        if (!initialized) return;
        StopLifeGesture("life-personality-" + CurrentLifePersonality());
        ScheduleNextGestureSoon("life-personality-" + CurrentLifePersonality(), 0.20f, 0.75f);
        ApplyAffectionMorphTarget("life-personality-" + source);
        UpdateStatus("Life Personality: " + CurrentLifePersonality() + " / state=" + CurrentLifeState() + " / expression=" + CurrentLifeAffection() + " / source=" + source);
    }


    string CurrentLifeAffection()
    {
        if (lifeAffectionMode == null || string.IsNullOrEmpty(lifeAffectionMode.val)) return LifeAffectionNeutral;
        return NormalizeLifeAffection(lifeAffectionMode.val);
    }

    void SetLifeAffection(string affection, string source)
    {
        string next = NormalizeLifeAffection(affection);
        try
        {
            if (lifeAffectionMode != null)
                lifeAffectionMode.val = next;
        }
        catch { }
        ApplyLifeAffectionChange(source);
    }

    bool IsLifeAffectionAlias(string value, string legacy, string display)
    {
        if (string.IsNullOrEmpty(value)) return false;
        if (string.Equals(value, display, StringComparison.OrdinalIgnoreCase)) return true;
        if (string.Equals(value, legacy, StringComparison.OrdinalIgnoreCase)) return true;
        return value.StartsWith(legacy + " ", StringComparison.OrdinalIgnoreCase)
            || value.StartsWith(legacy + ":", StringComparison.OrdinalIgnoreCase);
    }

    string NormalizeLifeAffection(string affection)
    {
        if (IsLifeAffectionAlias(affection, LifeAffectionLikeLegacy, LifeAffectionLike)) return LifeAffectionLike;
        if (IsLifeAffectionAlias(affection, LifeAffectionDislikeLegacy, LifeAffectionDislike)) return LifeAffectionDislike;
        if (IsLifeAffectionAlias(affection, LifeAffectionSadLegacy, LifeAffectionShy)) return LifeAffectionShy;
        if (IsLifeAffectionAlias(affection, LifeAffectionShyLegacy, LifeAffectionShy)) return LifeAffectionShy;
        if (IsLifeAffectionAlias(affection, LifeAffectionNeutralLegacy, LifeAffectionNeutral)) return LifeAffectionNeutral;
        return LifeAffectionNeutral;
    }

    void OnLifeAffectionChanged(string value)
    {
        ApplyLifeAffectionChange("ui");
    }

    void ApplyLifeAffectionChange(string source)
    {
        if (!initialized) return;
        StopLifeGesture("life-affection-" + CurrentLifeAffection());
        ScheduleNextGestureSoon("life-affection-" + CurrentLifeAffection(), 0.20f, 0.75f);
        ApplyAffectionMorphTarget("life-affection-" + source);
        UpdateStatus("Life Expression: " + CurrentLifeAffection() + " / state=" + CurrentLifeState() + " / personality=" + CurrentLifePersonality() + " / source=" + source);
    }


    void RefreshAffectionMorphChoices(bool updateUi)
    {
        string keepLike = likeAffectionMorphName != null ? likeAffectionMorphName.val : DefaultLikeAffectionMorphName;
        string keepDislike = dislikeAffectionMorphName != null ? dislikeAffectionMorphName.val : DefaultDislikeAffectionMorphName;
        string keepShy = shyPersonalityMorphName != null ? shyPersonalityMorphName.val : DefaultShyPersonalityMorphName;
        // v111: do not pre-filter the choices with a separate text field.
        // CreateFilterablePopup performs the search inside the combo itself.
        List<string> likeChoices = BuildAffectionMorphChoices(keepLike, DefaultLikeAffectionMorphName, "");
        List<string> dislikeChoices = BuildAffectionMorphChoices(keepDislike, DefaultDislikeAffectionMorphName, "");
        List<string> shyChoices = BuildAffectionMorphChoices(keepShy, DefaultShyPersonalityMorphName, "");

        affectionMorphChoices.Clear();
        for (int i = 0; i < likeChoices.Count; i++) affectionMorphChoices.Add(likeChoices[i]);

        dislikeAffectionMorphChoices.Clear();
        for (int i = 0; i < dislikeChoices.Count; i++) dislikeAffectionMorphChoices.Add(dislikeChoices[i]);

        shyPersonalityMorphChoices.Clear();
        for (int i = 0; i < shyChoices.Count; i++) shyPersonalityMorphChoices.Add(shyChoices[i]);

        if (updateUi)
        {
            UpdateAffectionMorphChooserChoices(likeAffectionMorphName, affectionMorphChoices, keepLike);
            UpdateAffectionMorphChooserChoices(dislikeAffectionMorphName, dislikeAffectionMorphChoices, keepDislike);
            UpdateAffectionMorphChooserChoices(shyPersonalityMorphName, shyPersonalityMorphChoices, keepShy);
            ApplyAffectionMorphTarget("refresh-affection-morphs");
            if (debugLog != null && debugLog.val)
            {
                Log("Affection morph choices refreshed"
                    + " / mode=filterable-popup"
                    + " / likeCount=" + affectionMorphChoices.Count.ToString(CultureInfo.InvariantCulture)
                    + " / dislikeCount=" + dislikeAffectionMorphChoices.Count.ToString(CultureInfo.InvariantCulture)
                    + " / shyCount=" + shyPersonalityMorphChoices.Count.ToString(CultureInfo.InvariantCulture));
            }
        }
    }

    List<string> BuildAffectionMorphChoices(string keep, string defaultMorph, string searchText)
    {
        List<string> choices = new List<string>();
        HashSet<string> seen = new HashSet<string>();
        AddAffectionMorphChoice(choices, seen, AffectionMorphNone);
        AddAffectionMorphChoice(choices, seen, defaultMorph);
        AddAffectionMorphChoice(choices, seen, keep);

        JSONStorable geometry = null;
        try { if (containingAtom != null) geometry = containingAtom.GetStorableByID("geometry"); } catch { geometry = null; }
        if (geometry != null)
        {
            List<string> names = null;
            try { names = geometry.GetFloatParamNames(); } catch { names = null; }
            if (names != null)
            {
                names.Sort(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < names.Count; i++)
                {
                    string n = names[i];
                    if (string.IsNullOrEmpty(n)) continue;
                    string trimmed = n.Trim();
                    if (!AffectionMorphMatchesSearch(trimmed, searchText)) continue;
                    AddAffectionMorphChoice(choices, seen, trimmed);
                }
            }
        }

        return choices;
    }

    bool AffectionMorphMatchesSearch(string morphName, string searchText)
    {
        if (string.IsNullOrEmpty(morphName)) return false;
        if (string.IsNullOrEmpty(searchText)) return true;

        string hay = morphName.ToLowerInvariant();
        string normalized = searchText.ToLowerInvariant()
            .Replace('　', ' ')
            .Replace(',', ' ')
            .Replace(';', ' ')
            .Replace('|', ' ');
        string[] terms = normalized.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (terms == null || terms.Length <= 0) return true;

        for (int i = 0; i < terms.Length; i++)
        {
            string term = terms[i];
            if (string.IsNullOrEmpty(term)) continue;
            if (hay.IndexOf(term, StringComparison.OrdinalIgnoreCase) < 0) return false;
        }
        return true;
    }

    string SafeSearchLabel(string s)
    {
        if (string.IsNullOrEmpty(s)) return "<all>";
        return s.Trim();
    }

    void AddAffectionMorphChoice(List<string> choices, HashSet<string> seen, string raw)
    {
        if (choices == null || seen == null) return;
        string value = string.IsNullOrEmpty(raw) ? AffectionMorphNone : raw.Trim();
        if (string.IsNullOrEmpty(value)) value = AffectionMorphNone;
        string key = value.ToLowerInvariant();
        if (seen.Contains(key)) return;
        seen.Add(key);
        choices.Add(value);
    }

    void UpdateAffectionMorphChooserChoices(JSONStorableStringChooser chooser, List<string> choices, string keep)
    {
        if (chooser == null) return;
        if (choices == null) choices = new List<string>() { AffectionMorphNone };
        string desired = string.IsNullOrEmpty(keep) ? AffectionMorphNone : keep;
        if (!choices.Contains(desired))
            AddAffectionMorphChoice(choices, new HashSet<string>(choices, StringComparer.OrdinalIgnoreCase), desired);
        try { chooser.choices = new List<string>(choices); } catch { }
        try
        {
            if (choices.Contains(desired)) chooser.val = desired;
            else chooser.val = AffectionMorphNone;
        }
        catch { }
    }

    void MaintainAffectionMorphControl()
    {
        if (!initialized) return;

        string affection = lifeEnabled != null && !lifeEnabled.val ? LifeAffectionNeutral : CurrentLifeAffection();
        string personality = lifeEnabled != null && !lifeEnabled.val ? LifePersonalityNormal : CurrentLifePersonality();
        string state = CurrentLifeState();
        string likeName = CleanAffectionMorphName(likeAffectionMorphName != null ? likeAffectionMorphName.val : "");
        string dislikeName = CleanAffectionMorphName(dislikeAffectionMorphName != null ? dislikeAffectionMorphName.val : "");
        string shyName = CleanAffectionMorphName(shyPersonalityMorphName != null ? shyPersonalityMorphName.val : "");
        float likeMax = Mathf.Clamp01(SafeFloat(likeAffectionMorphMax, DefaultLikeAffectionMorphMax));
        float dislikeMax = Mathf.Clamp01(SafeFloat(dislikeAffectionMorphMax, DefaultDislikeAffectionMorphMax));
        float shyMax = Mathf.Clamp01(SafeFloat(shyPersonalityMorphMax, DefaultShyPersonalityMorphMax));
        float fade = Mathf.Max(0.05f, SafeFloat(affectionMorphFadeSeconds, DefaultAffectionMorphFadeSeconds));

        if (string.Equals(affection, lastAffectionMorphAffection, StringComparison.Ordinal)
            && string.Equals(personality, lastAffectionMorphPersonality, StringComparison.Ordinal)
            && string.Equals(likeName, lastAffectionMorphLikeName, StringComparison.Ordinal)
            && string.Equals(dislikeName, lastAffectionMorphDislikeName, StringComparison.Ordinal)
            && string.Equals(shyName, lastAffectionMorphShyName, StringComparison.Ordinal)
            && Mathf.Abs(likeMax - lastAffectionMorphLikeMax) < 0.0005f
            && Mathf.Abs(dislikeMax - lastAffectionMorphDislikeMax) < 0.0005f
            && Mathf.Abs(shyMax - lastAffectionMorphShyMax) < 0.0005f
            && Mathf.Abs(fade - lastAffectionMorphFadeSeconds) < 0.0005f)
            return;

        ApplyAffectionMorphTarget("maintain");
    }

    void ApplyAffectionMorphTarget(string source)
    {
        if (!initialized) return;

        string affection = lifeEnabled != null && !lifeEnabled.val ? LifeAffectionNeutral : CurrentLifeAffection();
        string personality = lifeEnabled != null && !lifeEnabled.val ? LifePersonalityNormal : CurrentLifePersonality();
        string state = CurrentLifeState();
        string likeName = CleanAffectionMorphName(likeAffectionMorphName != null ? likeAffectionMorphName.val : "");
        string dislikeName = CleanAffectionMorphName(dislikeAffectionMorphName != null ? dislikeAffectionMorphName.val : "");
        string shyName = CleanAffectionMorphName(shyPersonalityMorphName != null ? shyPersonalityMorphName.val : "");
        float likeMax = Mathf.Clamp01(SafeFloat(likeAffectionMorphMax, DefaultLikeAffectionMorphMax));
        float dislikeMax = Mathf.Clamp01(SafeFloat(dislikeAffectionMorphMax, DefaultDislikeAffectionMorphMax));
        float shyMax = Mathf.Clamp01(SafeFloat(shyPersonalityMorphMax, DefaultShyPersonalityMorphMax));
        float fade = Mathf.Max(0.05f, SafeFloat(affectionMorphFadeSeconds, DefaultAffectionMorphFadeSeconds));

        lastAffectionMorphAffection = affection;
        lastAffectionMorphPersonality = personality;
        lastAffectionMorphLikeName = likeName;
        lastAffectionMorphDislikeName = dislikeName;
        lastAffectionMorphShyName = shyName;
        lastAffectionMorphLikeMax = likeMax;
        lastAffectionMorphDislikeMax = dislikeMax;
        lastAffectionMorphShyMax = shyMax;
        lastAffectionMorphFadeSeconds = fade;

        Dictionary<string, float> targets = new Dictionary<string, float>();

        AddAffectionMorphRestoreTargets(targets);

        if (!string.IsNullOrEmpty(likeName))
        {
            float likeTarget = GetAffectionMorphOriginalOrCurrent(likeName);
            if (affection == LifeAffectionLike && likeMax > 0.0001f)
                likeTarget = likeMax;
            targets[likeName] = Mathf.Clamp01(likeTarget);
        }

        if (!string.IsNullOrEmpty(dislikeName))
        {
            float dislikeTarget = GetAffectionMorphOriginalOrCurrent(dislikeName);
            if (affection == LifeAffectionDislike && dislikeMax > 0.0001f)
                dislikeTarget = dislikeMax;
            targets[dislikeName] = Mathf.Clamp01(dislikeTarget);
        }

        if (!string.IsNullOrEmpty(shyName))
        {
            float shyTarget = GetAffectionMorphOriginalOrCurrent(shyName);
            // Sad is part of the Life Expression group; Sleeping stays state-dominant and suppresses it.
            if (affection == LifeAffectionShy && state != LifeStateSleeping && shyMax > 0.0001f)
                shyTarget = shyMax;
            targets[shyName] = Mathf.Clamp01(shyTarget);
        }

        if (targets.Count <= 0) return;

        if (affectionMorphRoutine != null)
        {
            try { StopCoroutine(affectionMorphRoutine); } catch { }
            affectionMorphRoutine = null;
        }

        affectionMorphRoutine = StartCoroutine(AffectionMorphFadeRoutine(targets, fade, source));
    }

    void AddAffectionMorphRestoreTargets(Dictionary<string, float> targets)
    {
        if (targets == null || affectionMorphOriginalValues == null) return;
        foreach (KeyValuePair<string, float> kv in affectionMorphOriginalValues)
        {
            if (string.IsNullOrEmpty(kv.Key)) continue;
            targets[kv.Key] = Mathf.Clamp01(kv.Value);
        }
    }

    IEnumerator AffectionMorphFadeRoutine(Dictionary<string, float> targets, float fadeSeconds, string source)
    {
        Dictionary<string, float> starts = new Dictionary<string, float>();
        List<string> names = new List<string>();

        foreach (KeyValuePair<string, float> kv in targets)
        {
            string name = CleanAffectionMorphName(kv.Key);
            if (string.IsNullOrEmpty(name)) continue;
            JSONStorableFloat f = FindAffectionMorphParam(name);
            if (f == null)
            {
                if (debugLog != null && debugLog.val) Log("Affection morph not found / name=" + name + " / source=" + source);
                continue;
            }

            if (!affectionMorphOriginalValues.ContainsKey(name))
            {
                try { affectionMorphOriginalValues[name] = Mathf.Clamp01(f.val); } catch { affectionMorphOriginalValues[name] = 0.0f; }
            }

            try { starts[name] = Mathf.Clamp01(f.val); } catch { starts[name] = GetAffectionMorphOriginalOrCurrent(name); }
            names.Add(name);
        }

        if (names.Count <= 0)
        {
            affectionMorphRoutine = null;
            yield break;
        }

        float seconds = Mathf.Max(0.01f, fadeSeconds);
        float elapsed = 0.0f;
        while (elapsed < seconds)
        {
            float e = Smoother01(elapsed / seconds);
            for (int i = 0; i < names.Count; i++)
            {
                string name = names[i];
                JSONStorableFloat f = FindAffectionMorphParam(name);
                if (f == null) continue;
                float start = starts.ContainsKey(name) ? starts[name] : 0.0f;
                float target = targets.ContainsKey(name) ? targets[name] : start;
                try { f.val = Mathf.Clamp01(Mathf.Lerp(start, target, e)); } catch { }
            }
            elapsed += Mathf.Max(0.0001f, Time.deltaTime);
            yield return null;
        }

        for (int i = 0; i < names.Count; i++)
        {
            string name = names[i];
            JSONStorableFloat f = FindAffectionMorphParam(name);
            if (f == null) continue;
            float target = targets.ContainsKey(name) ? targets[name] : GetAffectionMorphOriginalOrCurrent(name);
            try { f.val = Mathf.Clamp01(target); } catch { }
        }

        affectionMorphRoutine = null;
        if (debugLog != null && debugLog.val)
        {
            Log("Life morph applied / affection=" + lastAffectionMorphAffection
                + " / personality=" + lastAffectionMorphPersonality
                + " / like=" + lastAffectionMorphLikeName
                + " / dislike=" + lastAffectionMorphDislikeName
                + " / sad=" + lastAffectionMorphShyName
                + " / count=" + names.Count.ToString(CultureInfo.InvariantCulture)
                + " / source=" + source);
        }
    }

    void RestoreAffectionMorphs(string source)
    {
        if (affectionMorphRoutine != null)
        {
            try { StopCoroutine(affectionMorphRoutine); } catch { }
            affectionMorphRoutine = null;
        }

        if (affectionMorphOriginalValues == null || affectionMorphOriginalValues.Count <= 0) return;

        foreach (KeyValuePair<string, float> kv in affectionMorphOriginalValues)
        {
            JSONStorableFloat f = FindAffectionMorphParam(kv.Key);
            if (f == null) continue;
            try { f.val = Mathf.Clamp01(kv.Value); } catch { }
        }

        if (debugLog != null && debugLog.val)
            Log("Affection morph restored / count=" + affectionMorphOriginalValues.Count.ToString(CultureInfo.InvariantCulture) + " / source=" + source);
    }

    float GetAffectionMorphOriginalOrCurrent(string morphName)
    {
        string name = CleanAffectionMorphName(morphName);
        if (string.IsNullOrEmpty(name)) return 0.0f;
        if (affectionMorphOriginalValues != null && affectionMorphOriginalValues.ContainsKey(name))
            return Mathf.Clamp01(affectionMorphOriginalValues[name]);

        JSONStorableFloat f = FindAffectionMorphParam(name);
        if (f == null) return 0.0f;
        try { return Mathf.Clamp01(f.val); } catch { }
        return 0.0f;
    }

    JSONStorableFloat FindAffectionMorphParam(string morphName)
    {
        string name = CleanAffectionMorphName(morphName);
        if (string.IsNullOrEmpty(name) || containingAtom == null) return null;
        JSONStorable geometry = null;
        try { geometry = containingAtom.GetStorableByID("geometry"); } catch { geometry = null; }
        if (geometry == null) return null;

        JSONStorableFloat f = null;
        try { f = geometry.GetFloatJSONParam(name); } catch { f = null; }
        if (f != null) return f;

        // Small convenience fallback for copied labels such as "DAZMorph:Smile" or "geometry:Smile".
        string stripped = StripAffectionMorphPrefix(name);
        if (!string.Equals(stripped, name, StringComparison.Ordinal))
        {
            try { f = geometry.GetFloatJSONParam(stripped); } catch { f = null; }
            if (f != null) return f;
        }

        return null;
    }

    string CleanAffectionMorphName(string name)
    {
        if (string.IsNullOrEmpty(name)) return "";
        string n = name.Trim();
        if (string.IsNullOrEmpty(n)) return "";
        if (string.Equals(n, AffectionMorphNone, StringComparison.OrdinalIgnoreCase)) return "";
        if (string.Equals(n, "None", StringComparison.OrdinalIgnoreCase)) return "";
        if (string.Equals(n, "<None>", StringComparison.OrdinalIgnoreCase)) return "";
        if (string.Equals(n, "なし", StringComparison.OrdinalIgnoreCase)) return "";
        return StripAffectionMorphPrefix(n).Trim();
    }

    string StripAffectionMorphPrefix(string name)
    {
        if (string.IsNullOrEmpty(name)) return "";
        string n = name.Trim();
        string[] prefixes = new string[] { "DAZMorph:", "Morph:", "geometry:", "Geometry:" };
        for (int i = 0; i < prefixes.Length; i++)
        {
            string p = prefixes[i];
            if (n.StartsWith(p, StringComparison.OrdinalIgnoreCase))
                return n.Substring(p.Length).Trim();
        }
        return n;
    }

    void OnLifeStateChanged(string value)
    {
        if (suppressLifeStateCallback) return;
        ApplyLifeStateChange("ui");
    }

    void ApplyLifeStateChange(string source)
    {
        string state = CurrentLifeState();
        try
        {
            if (lifeMotionMode != null)
                lifeMotionMode.val = CurrentMotionMode();
        }
        catch { }

        ApplyLifeStateEyeControl(source);

        if (!initialized)
            return;

        StopLifeGesture("life-state-" + state);
        StopBreathLoop("life-state-" + state, false);
        StopLegBaseLoop("life-state-" + state, false);

        if (state == LifeStateSleeping)
        {
            StartSleepSettleComply(source);
            ScheduleNextGestureSoon("life-state-" + state, SleepSettleComplySeconds + 0.20f, SleepSettleComplySeconds + 1.20f);
        }
        else
        {
            StopSleepSettleComply("life-state-" + state);
            ScheduleNextGestureSoon("life-state-" + state, 0.20f, 0.60f);
        }

        UpdateStatus("Life State: " + state + " / personality=" + CurrentLifePersonality() + " / expression=" + CurrentLifeAffection() + " / source=" + source);
    }

    void StartSleepSettleComply(string source)
    {
        StopSleepSettleComply("restart");
        ResolveControllers();
        sleepSettleComplyRoutine = StartCoroutine(SleepSettleComplyRoutine(source));
    }

    IEnumerator SleepSettleComplyRoutine(string source)
    {
        float endTime = Time.time + SleepSettleComplySeconds;
        float nextReapply = -999.0f;

        while (Time.time < endTime)
        {
            if (CurrentLifeState() != LifeStateSleeping)
                break;

            if (Time.time >= nextReapply)
            {
                ResolveControllers();
                SetControllerComplyOff(lHandControl);
                SetControllerComplyOff(rHandControl);
                SetControllerComplyOff(lElbowControl);
                SetControllerComplyOff(rElbowControl);
                nextReapply = Time.time + 0.25f;
            }
            yield return null;
        }

        sleepSettleComplyRoutine = null;
        if (debugLog != null && debugLog.val)
        {
            Log("Sleep settle comply complete / seconds=" + SleepSettleComplySeconds.ToString("F2", CultureInfo.InvariantCulture)
                + " / state=" + CurrentLifeState()
                + " / source=" + source);
        }
    }

    void StopSleepSettleComply(string reason)
    {
        if (sleepSettleComplyRoutine != null)
        {
            try { StopCoroutine(sleepSettleComplyRoutine); } catch { }
            sleepSettleComplyRoutine = null;
            if (debugLog != null && debugLog.val) Log("Stop sleep settle comply / reason=" + reason);
        }
    }

    void SetControllerComplyOff(FreeControllerV3 ctrl)
    {
        if (ctrl == null) return;
        try { ctrl.currentPositionState = FreeControllerV3.PositionState.Comply; } catch { }
        try { ctrl.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
    }

    void ApplyLifeStateEyeControl(string source)
    {
        string state = CurrentLifeState();
        bool closeEyes = state == LifeStateSleeping;
        string desiredState = closeEyes ? "closed" : "open";
        if (lastAppliedEyeState == desiredState && source != "init")
        {
            if (closeEyes && sleepingEyeTransitionRoutine == null) ApplySleepingEyeClosedHold(source + ":same-state");
            return;
        }

        int touchedMorphs = 0;

        // Init/load should not animate. Runtime state changes do animate.
        if (!initialized || string.Equals(source, "init", StringComparison.OrdinalIgnoreCase))
        {
            StopSleepingEyeTransition("eye-init");
            if (closeEyes)
            {
                DisableAutoBlinkForSleeping(source);
                touchedMorphs = SetEyesClosedMorphs(1.0f);
                nextSleepingEyeHoldTime = Time.time + SleepingEyeHoldInterval;
            }
            else
            {
                RestoreAutoBlinkAfterSleeping(source);
                SetBlinkSuppressMorphs(0.0f);
                touchedMorphs = SetEyesClosedMorphs(0.0f);
            }
            lastAppliedEyeState = desiredState;
        }
        else
        {
            touchedMorphs = StartSleepingEyeTransition(closeEyes, source);
        }

        if (debugLog != null && debugLog.val)
        {
            Log("Life State eye control / state=" + state
                + " / eyes=" + desiredState
                + " / morphs=" + touchedMorphs.ToString(CultureInfo.InvariantCulture)
                + " / transition=" + (sleepingEyeTransitionRoutine != null ? "1" : "0")
                + " / autoBlink=" + AutoBlinkDebugLabel()
                + " / source=" + source);
        }
    }

    void MaintainLifeStateEyeControl()
    {
        string state = CurrentLifeState();
        if (state == LifeStateSleeping)
        {
            if (sleepingEyeTransitionRoutine != null) return;

            // Auto Blink / Auto Eyelid Morphs can run every frame.
            // Re-assert the final sleeping eye state after the close transition finishes.
            ApplySleepingEyeClosedHold("maintain");
            nextSleepingEyeHoldTime = Time.time + SleepingEyeHoldInterval;
            return;
        }

        if (sleepingEyeTransitionRoutine != null) return;

        if (sleepingAutoBlinkOriginalSaved || HasSavedEyeAutoSystemState())
        {
            RestoreAutoBlinkAfterSleeping("maintain-open");
        }
    }

    int StartSleepingEyeTransition(bool closeEyes, string source)
    {
        StopSleepingEyeTransition("new-eye-transition");

        if (closeEyes)
        {
            DisableAutoBlinkForSleeping(source + ":close-start");
            sleepingEyeTransitionRoutine = StartCoroutine(SleepingEyeTransitionRoutine(true, source));
            return SetEyesClosedMorphs(GetEyesClosedMorphValueOrDefault(0.0f));
        }

        // Keep Auto Blink / Auto Eyelid Morphs disabled until the eyes are fully open.
        DisableAutoBlinkForSleeping(source + ":open-start");
        SetBlinkSuppressMorphs(0.0f);
        sleepingEyeTransitionRoutine = StartCoroutine(SleepingEyeTransitionRoutine(false, source));
        return SetEyesClosedMorphs(GetEyesClosedMorphValueOrDefault(1.0f));
    }

    IEnumerator SleepingEyeTransitionRoutine(bool closeEyes, string source)
    {
        float target = closeEyes ? 1.0f : 0.0f;
        float fallbackStart = closeEyes ? 0.0f : 1.0f;
        float start = GetEyesClosedMorphValueOrDefault(fallbackStart);
        float seconds = Mathf.Max(0.05f, closeEyes ? SleepingEyeCloseSeconds : SleepingEyeOpenSeconds);
        float elapsed = 0.0f;

        while (elapsed < seconds)
        {
            if (closeEyes && CurrentLifeState() != LifeStateSleeping)
            {
                sleepingEyeTransitionRoutine = null;
                yield break;
            }
            if (!closeEyes && CurrentLifeState() == LifeStateSleeping)
            {
                sleepingEyeTransitionRoutine = null;
                yield break;
            }

            DisableAutoBlinkForSleeping(source + (closeEyes ? ":closing" : ":opening"));
            SetBlinkSuppressMorphs(0.0f);
            float e = Smoother01(elapsed / seconds);
            float value = Mathf.Lerp(start, target, e);
            SetEyesClosedMorphs(value);
            elapsed += Mathf.Max(0.0001f, Time.deltaTime);
            yield return null;
        }

        SetBlinkSuppressMorphs(0.0f);
        SetEyesClosedMorphs(target);
        lastAppliedEyeState = closeEyes ? "closed" : "open";
        sleepingEyeTransitionRoutine = null;

        if (closeEyes)
        {
            DisableAutoBlinkForSleeping(source + ":close-complete");
            nextSleepingEyeHoldTime = Time.time + SleepingEyeHoldInterval;
        }
        else
        {
            RestoreAutoBlinkAfterSleeping(source + ":open-complete");
        }

        if (debugLog != null && debugLog.val)
        {
            Log("Life State eye transition complete / eyes=" + (closeEyes ? "closed" : "open")
                + " / seconds=" + seconds.ToString("F2", CultureInfo.InvariantCulture)
                + " / source=" + source);
        }
    }

    void StopSleepingEyeTransition(string reason)
    {
        if (sleepingEyeTransitionRoutine != null)
        {
            try { StopCoroutine(sleepingEyeTransitionRoutine); } catch { }
            sleepingEyeTransitionRoutine = null;
            if (debugLog != null && debugLog.val) Log("Stop sleeping eye transition / reason=" + reason);
        }
    }

    float GetEyesClosedMorphValueOrDefault(float fallback)
    {
        if (containingAtom == null) return fallback;
        JSONStorable geometry = null;
        try { geometry = containingAtom.GetStorableByID("geometry"); } catch { geometry = null; }
        if (geometry == null) return fallback;

        string[] aliases = new string[]
        {
            "Eyes Closed",
            "EyesClosed",
            "Eyes Closed Left",
            "Eyes Closed Right",
            "Eyes Closed L",
            "Eyes Closed R",
            "Eyelids Closed",
            "Eyelids Closed Left",
            "Eyelids Closed Right",
            "Eyelid Closed",
            "Eyelid Closed Left",
            "Eyelid Closed Right"
        };

        float maxValue = -1.0f;
        for (int i = 0; i < aliases.Length; i++)
        {
            JSONStorableFloat f = null;
            try { f = geometry.GetFloatJSONParam(aliases[i]); } catch { f = null; }
            if (f == null) continue;
            try { maxValue = Mathf.Max(maxValue, f.val); } catch { }
        }

        if (maxValue < -0.5f) return fallback;
        return Mathf.Clamp01(maxValue);
    }

    void ApplySleepingEyeClosedHold(string source)
    {
        DisableAutoBlinkForSleeping(source);
        SetBlinkSuppressMorphs(0.0f);
        SetEyesClosedMorphs(1.0f);
    }

    int SetEyesClosedMorphs(float value)
    {
        if (containingAtom == null) return 0;
        JSONStorable geometry = null;
        try { geometry = containingAtom.GetStorableByID("geometry"); } catch { geometry = null; }
        if (geometry == null) return 0;

        string[] aliases = new string[]
        {
            "Eyes Closed",
            "EyesClosed",
            "Eyes Closed Left",
            "Eyes Closed Right",
            "Eyes Closed L",
            "Eyes Closed R",
            "Eyelids Closed",
            "Eyelids Closed Left",
            "Eyelids Closed Right",
            "Eyelid Closed",
            "Eyelid Closed Left",
            "Eyelid Closed Right"
        };

        HashSet<string> touched = new HashSet<string>();
        int count = 0;
        for (int i = 0; i < aliases.Length; i++)
        {
            string alias = aliases[i];
            if (string.IsNullOrEmpty(alias)) continue;
            JSONStorableFloat f = null;
            try { f = geometry.GetFloatJSONParam(alias); } catch { f = null; }
            if (f == null) continue;
            string key = f.name;
            if (string.IsNullOrEmpty(key)) key = alias;
            if (touched.Contains(key)) continue;
            touched.Add(key);
            try
            {
                f.val = Mathf.Clamp01(value);
                count++;
            }
            catch { }
        }
        return count;
    }

    int SetBlinkSuppressMorphs(float value)
    {
        if (containingAtom == null) return 0;
        JSONStorable geometry = null;
        try { geometry = containingAtom.GetStorableByID("geometry"); } catch { geometry = null; }
        if (geometry == null) return 0;

        string[] aliases = new string[]
        {
            "Blink",
            "blink",
            "Eye Blink",
            "EyeBlink",
            "Eyes Blink",
            "EyesBlink",
            "Blink Left",
            "Blink Right",
            "Blink L",
            "Blink R",
            "Eye Blink Left",
            "Eye Blink Right",
            "Eyes Blink Left",
            "Eyes Blink Right",
            "Eyelid Blink",
            "Eyelid Blink Left",
            "Eyelid Blink Right",
            "Left Eye Blink",
            "Right Eye Blink"
        };

        HashSet<string> touched = new HashSet<string>();
        int count = 0;
        for (int i = 0; i < aliases.Length; i++)
        {
            string alias = aliases[i];
            if (string.IsNullOrEmpty(alias)) continue;
            JSONStorableFloat f = null;
            try { f = geometry.GetFloatJSONParam(alias); } catch { f = null; }
            if (f == null) continue;
            string key = f.name;
            if (string.IsNullOrEmpty(key)) key = alias;
            if (touched.Contains(key)) continue;
            touched.Add(key);
            try
            {
                f.val = Mathf.Clamp01(value);
                count++;
            }
            catch { }
        }
        return count;
    }

    bool HasSavedEyeAutoSystemState()
    {
        if (sleepingEyeAutoBoolStates == null) return false;
        for (int i = 0; i < sleepingEyeAutoBoolStates.Count; i++)
        {
            SleepingEyeAutoBoolState st = sleepingEyeAutoBoolStates[i];
            if (st != null && st.saved) return true;
        }
        return false;
    }

    void DisableAutoBlinkForSleeping(string source)
    {
        DisableEyeAutoSystemsForSleeping(source);
    }

    void RestoreAutoBlinkAfterSleeping(string source)
    {
        RestoreEyeAutoSystemsAfterSleeping(source);
    }

    void DisableEyeAutoSystemsForSleeping(string source)
    {
        if (!sleepingEyeAutoSystemsResolved || sleepingEyeAutoBoolStates.Count == 0)
        {
            ResolveEyeAutoSystemBoolParams(source);
        }

        if (sleepingEyeAutoBoolStates.Count == 0)
        {
            // Keep the old single-param fallback alive for older builds / unexpected storables.
            if (sleepingAutoBlinkParam == null && !sleepingAutoBlinkResolved)
            {
                ResolveAutoBlinkParam(source);
            }
            if (sleepingAutoBlinkParam == null) return;

            if (!sleepingAutoBlinkOriginalSaved)
            {
                try { sleepingAutoBlinkOriginalValue = sleepingAutoBlinkParam.val; } catch { sleepingAutoBlinkOriginalValue = false; }
                sleepingAutoBlinkOriginalSaved = true;
            }
            try { if (sleepingAutoBlinkParam.val) sleepingAutoBlinkParam.val = false; } catch { }
            return;
        }

        for (int i = 0; i < sleepingEyeAutoBoolStates.Count; i++)
        {
            SleepingEyeAutoBoolState st = sleepingEyeAutoBoolStates[i];
            if (st == null || st.param == null) continue;
            if (!st.saved)
            {
                try { st.originalValue = st.param.val; } catch { st.originalValue = false; }
                st.saved = true;
            }
            try { if (st.param.val) st.param.val = false; } catch { }
        }
    }

    void RestoreEyeAutoSystemsAfterSleeping(string source)
    {
        bool restoredAny = false;
        for (int i = 0; i < sleepingEyeAutoBoolStates.Count; i++)
        {
            SleepingEyeAutoBoolState st = sleepingEyeAutoBoolStates[i];
            if (st == null || st.param == null || !st.saved) continue;
            try { st.param.val = st.originalValue; restoredAny = true; } catch { }
            st.saved = false;
        }

        if (sleepingAutoBlinkOriginalSaved)
        {
            if (sleepingAutoBlinkParam != null)
            {
                try { sleepingAutoBlinkParam.val = sleepingAutoBlinkOriginalValue; restoredAny = true; } catch { }
            }
            sleepingAutoBlinkOriginalSaved = false;
        }

        if (restoredAny && debugLog != null && debugLog.val)
        {
            Log("Auto eye systems restore / params=" + AutoBlinkDebugLabel() + " / source=" + source);
        }
    }

    void ResolveEyeAutoSystemBoolParams(string source)
    {
        sleepingEyeAutoSystemsResolved = true;
        sleepingEyeAutoBoolStates.Clear();

        if (containingAtom == null)
        {
            if (debugLog != null && debugLog.val) Log("Auto eye systems resolve / found=0 / source=" + source + " / reason=no-atom");
            return;
        }

        HashSet<string> seen = new HashSet<string>();
        string[] paramNames = EyeAutoSystemBoolParamNames();
        string[] directStorableIds = EyeAutoSystemCandidateStorableIds();

        for (int i = 0; i < directStorableIds.Length; i++)
        {
            string sid = directStorableIds[i];
            JSONStorable storable = GetStorableSafe(sid);
            if (storable == null) continue;
            AddEyeAutoSystemParamsFromStorable(storable, sid, paramNames, seen);
        }

        List<string> ids = null;
        try { ids = containingAtom.GetStorableIDs(); } catch { ids = null; }
        if (ids != null)
        {
            for (int idIndex = 0; idIndex < ids.Count; idIndex++)
            {
                string sid = ids[idIndex];
                if (string.IsNullOrEmpty(sid)) continue;
                JSONStorable storable = GetStorableSafe(sid);
                if (storable == null) continue;

                // VaM blocks System.Reflection in plugin security, so use only public JSON params.
                AddEyeAutoSystemParamsFromStorable(storable, sid, paramNames, seen);
            }
        }

        if (debugLog != null && debugLog.val)
        {
            Log("Auto eye systems resolve / found=" + sleepingEyeAutoBoolStates.Count.ToString(CultureInfo.InvariantCulture)
                + " / params=" + AutoBlinkDebugLabel()
                + " / source=" + source);
        }
    }

    string[] EyeAutoSystemBoolParamNames()
    {
        return new string[]
        {
            "Auto Blink",
            "AutoBlink",
            "autoBlink",
            "Auto Blinking",
            "AutoBlinking",
            "autoBlinking",
            "Blink",
            "blink",
            "Blinking",
            "blinking",
            "Auto Blink Enabled",
            "AutoBlinkEnabled",
            "autoBlinkEnabled",
            "blinkEnabled",
            "Blink Enabled",
            "BlinkEnabled",
            "Auto Eyelid Morphs",
            "AutoEyelidMorphs",
            "autoEyelidMorphs",
            "Auto Eyelids",
            "AutoEyelids",
            "autoEyelids",
            "Auto Eyelid",
            "AutoEyelid",
            "autoEyelid",
            "Auto Eye Lid Morphs",
            "AutoEyeLidMorphs",
            "autoEyeLidMorphs",
            "Auto Eyelid Morphs Enabled",
            "AutoEyelidMorphsEnabled",
            "autoEyelidMorphsEnabled",
            "Auto Eye Lid Morphs Enabled",
            "AutoEyeLidMorphsEnabled",
            "autoEyeLidMorphsEnabled",
            "Eyelid Morphs Enabled",
            "EyelidMorphsEnabled",
            "eyelidMorphsEnabled",
            "Eyelid Morph Enabled",
            "EyelidMorphEnabled",
            "eyelidMorphEnabled"
        };
    }

    string[] EyeAutoSystemCandidateStorableIds()
    {
        return new string[]
        {
            "EyelidControl",
            "Eye Control",
            "EyeControl",
            "Auto Systems",
            "Auto System",
            "AutoSystems",
            "AutoSystem",
            "Auto Behaviors",
            "AutoBehaviors",
            "Auto Behavior",
            "AutoBehavior",
            "Auto Behaviours",
            "AutoBehaviours",
            "Auto Behaviour",
            "AutoBehaviour",
            "Auto Morphs",
            "AutoMorphs",
            "geometry",
            "Geometry"
        };
    }

    void AddEyeAutoSystemParamsFromStorable(JSONStorable storable, string storableId, string[] paramNames, HashSet<string> seen)
    {
        if (storable == null || paramNames == null) return;
        for (int i = 0; i < paramNames.Length; i++)
        {
            string paramName = paramNames[i];
            if (string.IsNullOrEmpty(paramName)) continue;
            JSONStorableBool b = null;
            try { b = storable.GetBoolJSONParam(paramName); } catch { b = null; }
            if (b == null) continue;
            AddEyeAutoSystemBoolState(b, storableId + "/" + paramName, seen);
        }
    }

    void AddEyeAutoSystemBoolState(JSONStorableBool param, string source, HashSet<string> seen)
    {
        if (param == null) return;
        string key = source;
        try
        {
            if (!string.IsNullOrEmpty(param.name)) key = source + "#" + param.name;
        }
        catch { }
        if (seen != null && seen.Contains(key)) return;
        if (seen != null) seen.Add(key);

        SleepingEyeAutoBoolState st = new SleepingEyeAutoBoolState();
        st.param = param;
        st.originalValue = false;
        st.saved = false;
        st.source = source;
        sleepingEyeAutoBoolStates.Add(st);
    }

    bool LooksLikeEyeAutoSystemStorable(string storableId)
    {
        if (string.IsNullOrEmpty(storableId)) return false;
        string s = storableId.ToLowerInvariant();
        return s.Contains("auto") || s.Contains("blink") || s.Contains("eyelid") || s.Contains("eyecontrol") || s.Contains("eye control") || s.Contains("geometry");
    }

    void ResolveAutoBlinkParam(string source)
    {
        sleepingAutoBlinkResolved = true;
        sleepingAutoBlinkParam = FindAutoBlinkBoolParam(out sleepingAutoBlinkSource);
        if (debugLog != null && debugLog.val)
        {
            Log("Auto Blink resolve / found=" + (sleepingAutoBlinkParam != null ? "1" : "0")
                + " / source=" + source
                + " / param=" + AutoBlinkDebugLabel());
        }
    }

    JSONStorableBool FindAutoBlinkBoolParam(out string foundSource)
    {
        foundSource = "";
        if (containingAtom == null) return null;

        List<string> ids = null;
        try { ids = containingAtom.GetStorableIDs(); } catch { ids = null; }
        if (ids == null || ids.Count == 0) return null;

        string[] explicitBlinkParamNames = new string[]
        {
            "Auto Blink",
            "AutoBlink",
            "autoBlink",
            "Auto Blinking",
            "AutoBlinking",
            "autoBlinking",
            "Blink",
            "blink",
            "Blinking",
            "blinking",
            "Auto Blink Enabled",
            "AutoBlinkEnabled",
            "autoBlinkEnabled",
            "blinkEnabled",
            "Blink Enabled",
            "BlinkEnabled"
        };

        string[] genericEnableParamNames = new string[]
        {
            "enabled",
            "Enabled",
            "enable",
            "Enable",
            "on",
            "On",
            "active",
            "Active"
        };

        // First pass: explicit Blink parameter names on any storable.
        for (int idIndexA = 0; idIndexA < ids.Count; idIndexA++)
        {
            string storableIdA = ids[idIndexA];
            JSONStorable storableA = GetStorableSafe(storableIdA);
            JSONStorableBool explicitParam = FindFirstBoolParam(storableA, explicitBlinkParamNames, out foundSource, storableIdA);
            if (explicitParam != null) return explicitParam;
        }

        // Second pass: generic enable/on only when the storable itself is clearly an Auto Blink storable.
        for (int idIndexB = 0; idIndexB < ids.Count; idIndexB++)
        {
            string storableIdB = ids[idIndexB];
            if (!LooksLikeAutoBlinkStorable(storableIdB)) continue;
            JSONStorable storableB = GetStorableSafe(storableIdB);
            JSONStorableBool genericParam = FindFirstBoolParam(storableB, genericEnableParamNames, out foundSource, storableIdB);
            if (genericParam != null) return genericParam;
        }

        return null;
    }

    JSONStorable GetStorableSafe(string storableId)
    {
        if (containingAtom == null || string.IsNullOrEmpty(storableId)) return null;
        try { return containingAtom.GetStorableByID(storableId); } catch { return null; }
    }

    JSONStorableBool FindFirstBoolParam(JSONStorable storable, string[] paramNames, out string foundSource, string storableId)
    {
        foundSource = "";
        if (storable == null || paramNames == null) return null;

        for (int paramIndex = 0; paramIndex < paramNames.Length; paramIndex++)
        {
            string paramName = paramNames[paramIndex];
            if (string.IsNullOrEmpty(paramName)) continue;
            JSONStorableBool boolParam = null;
            try { boolParam = storable.GetBoolJSONParam(paramName); } catch { boolParam = null; }
            if (boolParam == null) continue;
            foundSource = storableId + "/" + paramName;
            return boolParam;
        }

        return null;
    }

    bool LooksLikeAutoBlinkStorable(string storableId)
    {
        if (string.IsNullOrEmpty(storableId)) return false;
        string lowerId = storableId.ToLowerInvariant();
        bool hasBlink = lowerId.Contains("blink");
        bool hasAuto = lowerId.Contains("auto");
        bool hasSystem = lowerId.Contains("system");
        bool hasEyelidControl = lowerId.Contains("eyelidcontrol") || lowerId.Contains("eyecontrol") || lowerId.Contains("eye control");
        return hasBlink || hasEyelidControl || (hasAuto && hasSystem);
    }

    string AutoBlinkDebugLabel()
    {
        if (sleepingEyeAutoBoolStates != null && sleepingEyeAutoBoolStates.Count > 0)
        {
            int max = Mathf.Min(4, sleepingEyeAutoBoolStates.Count);
            string label = "";
            for (int i = 0; i < max; i++)
            {
                SleepingEyeAutoBoolState st = sleepingEyeAutoBoolStates[i];
                if (st == null) continue;
                if (!string.IsNullOrEmpty(label)) label += ",";
                label += !string.IsNullOrEmpty(st.source) ? st.source : "<param>";
            }
            if (sleepingEyeAutoBoolStates.Count > max) label += ",+" + (sleepingEyeAutoBoolStates.Count - max).ToString(CultureInfo.InvariantCulture);
            return label;
        }

        if (sleepingAutoBlinkParam == null) return "<not-found>";
        if (!string.IsNullOrEmpty(sleepingAutoBlinkSource)) return sleepingAutoBlinkSource;
        try
        {
            if (!string.IsNullOrEmpty(sleepingAutoBlinkParam.name)) return sleepingAutoBlinkParam.name;
        }
        catch { }
        return "<resolved>";
    }

    float MotionScale()
    {
        string mode = CurrentMotionMode();
        if (mode == LifeMotionSmall) return 0.65f;
        if (mode == LifeMotionLarge) return 1.35f;
        return 1.00f;
    }

    float PersonalityCoverFrequencyMultiplier()
    {
        string personality = CurrentLifePersonality();
        if (personality == LifePersonalityBold) return 1.18f;
        return 1.0f;
    }

    float PersonalityLookFrequencyMultiplier()
    {
        string personality = CurrentLifePersonality();
        if (personality == LifePersonalityBold) return 1.18f;
        return 1.0f;
    }

    float AffectionCoverFrequencyMultiplier()
    {
        string affection = CurrentLifeAffection();
        if (affection == LifeAffectionDislike) return 0.95f;
        if (affection == LifeAffectionShy) return 1.06f;
        if (affection == LifeAffectionLike) return 1.12f;
        return 1.0f;
    }

    float AffectionLookFrequencyMultiplier()
    {
        string affection = CurrentLifeAffection();
        if (affection == LifeAffectionDislike) return 0.88f;
        if (affection == LifeAffectionShy) return 0.94f;
        if (affection == LifeAffectionLike) return 1.18f;
        return 1.0f;
    }

    float EffectiveCoverFrequency()
    {
        if (randomCoverEnabled == null || !randomCoverEnabled.val) return 0.0f;
        string state = CurrentLifeState();
        float result;
        if (state == LifeStateSleeping) result = 32.0f;
        else if (state == LifeStateQuiet) result = 48.0f;
        else if (state == LifeStateShy) result = 68.0f;
        else if (state == LifeStateActive) result = 95.0f;
        else result = Mathf.Clamp(SafeFloat(coverFrequency, DefaultCoverFrequency), 0.0f, 100.0f);

        if (state != LifeStateSleeping)
        {
            result *= PersonalityCoverFrequencyMultiplier();
            result *= AffectionCoverFrequencyMultiplier();
        }
        return Mathf.Clamp(result, 0.0f, 100.0f);
    }

    float EffectiveLookFrequency()
    {
        if (!IsHeadLookEnabled()) return 0.0f;
        string state = CurrentLifeState();
        float result;
        if (state == LifeStateSleeping) result = 16.0f;
        else if (state == LifeStateQuiet) result = 28.0f;
        else if (state == LifeStateShy) result = 46.0f;
        else if (state == LifeStateActive) result = 75.0f;
        else result = Mathf.Clamp(SafeFloat(lookFrequency, DefaultLookFrequency), 0.0f, 100.0f);

        if (state != LifeStateSleeping)
        {
            result *= PersonalityLookFrequencyMultiplier();
            result *= AffectionLookFrequencyMultiplier();
        }
        return Mathf.Clamp(result, 0.0f, 100.0f);
    }

    float EffectiveCoverSelfPercent()
    {
        string state = CurrentLifeState();
        string personality = CurrentLifePersonality();
        string affection = CurrentLifeAffection();
        if (state == LifeStateQuiet) return 100.0f;
        if (state == LifeStateSleeping) return 100.0f;

        float result;
        if (state == LifeStateShy) result = 92.0f;
        else if (personality == LifePersonalityShy) result = state == LifeStateActive ? 85.0f : 92.0f;
        else if (personality == LifePersonalityBold) result = state == LifeStateActive ? 45.0f : 55.0f;
        else if (state == LifeStateActive) result = 65.0f;
        else result = Mathf.Clamp(SafeFloat(coverSelfPercent, DefaultCoverSelfPercent), 0.0f, 100.0f);

        if (affection == LifeAffectionDislike) result += 24.0f;
        else if (affection == LifeAffectionShy) result += 30.0f;
        else if (affection == LifeAffectionLike) result -= 22.0f;
        return Mathf.Clamp(result, 0.0f, 100.0f);
    }

    float EffectiveLookTargetPercent()
    {
        string state = CurrentLifeState();
        string personality = CurrentLifePersonality();
        string affection = CurrentLifeAffection();
        if (state == LifeStateSleeping) return 0.0f;

        float result;
        if (state == LifeStateShy) result = 22.0f;
        else if (personality == LifePersonalityShy)
        {
            if (state == LifeStateQuiet) result = 12.0f;
            else if (state == LifeStateActive) result = 30.0f;
            else result = 22.0f;
        }
        else if (personality == LifePersonalityBold)
        {
            if (state == LifeStateQuiet) result = 45.0f;
            else if (state == LifeStateActive) result = 82.0f;
            else result = 72.0f;
        }
        else if (state == LifeStateQuiet) result = 35.0f;
        else if (state == LifeStateActive) result = 70.0f;
        else result = Mathf.Clamp(SafeFloat(lookTargetPercent, DefaultLookTargetPercent), 0.0f, 100.0f);

        if (affection == LifeAffectionDislike) result = result * 0.35f;
        else if (affection == LifeAffectionShy) result = result * 0.42f;
        else if (affection == LifeAffectionLike) result = result * 1.25f + 8.0f;
        return Mathf.Clamp(result, 0.0f, 100.0f);
    }

    float EffectiveLookAwayPercent()
    {
        string state = CurrentLifeState();
        string personality = CurrentLifePersonality();
        string affection = CurrentLifeAffection();
        if (state == LifeStateSleeping) return 100.0f;

        float result;
        if (state == LifeStateShy) result = 72.0f;
        else if (personality == LifePersonalityShy)
        {
            if (state == LifeStateQuiet) result = 78.0f;
            else if (state == LifeStateActive) result = 62.0f;
            else result = 68.0f;
        }
        else if (personality == LifePersonalityBold)
        {
            if (state == LifeStateQuiet) result = 18.0f;
            else if (state == LifeStateActive) result = 10.0f;
            else result = 12.0f;
        }
        else if (state == LifeStateQuiet) result = 10.0f;
        else if (state == LifeStateActive) result = 25.0f;
        else result = Mathf.Clamp(SafeFloat(lookAwayPercent, DefaultLookAwayPercent), 0.0f, 100.0f);

        if (affection == LifeAffectionDislike) result += 28.0f;
        else if (affection == LifeAffectionShy) result += 34.0f;
        else if (affection == LifeAffectionLike) result -= 14.0f;
        return Mathf.Clamp(result, 0.0f, 100.0f);
    }

    float EffectiveBreathScale()
    {
        float scale = Mathf.Max(0.0f, SafeFloat(breathScale, DefaultBreathScale));
        string state = CurrentLifeState();
        string personality = CurrentLifePersonality();
        if (state == LifeStateSleeping) return scale * 0.55f;
        if (state == LifeStateQuiet) scale *= 0.78f;
        else if (state == LifeStateShy) scale *= 0.88f;
        else if (state == LifeStateActive) scale *= 1.30f;
        if (personality == LifePersonalityShy) scale *= 0.95f;
        else if (personality == LifePersonalityBold) scale *= 1.08f;
        return scale;
    }

    float EffectiveShoulderSwayScale()
    {
        float scale = Mathf.Max(0.0f, SafeFloat(shoulderSwayScale, DefaultShoulderSwayScale));
        string state = CurrentLifeState();
        string personality = CurrentLifePersonality();
        if (state == LifeStateSleeping) return scale * 0.35f;
        if (state == LifeStateQuiet) scale *= 0.70f;
        else if (state == LifeStateShy) scale *= 1.10f;
        else if (state == LifeStateActive) scale *= 1.25f;
        if (personality == LifePersonalityShy) scale *= 1.18f;
        else if (personality == LifePersonalityBold) scale *= 1.08f;
        if (CurrentLifeAffection() == LifeAffectionDislike) scale *= 1.10f;
        else if (CurrentLifeAffection() == LifeAffectionShy) scale *= 1.16f;
        else if (CurrentLifeAffection() == LifeAffectionLike) scale *= 1.04f;
        return scale;
    }

    float EffectiveLegScale()
    {
        float scale = Mathf.Clamp(SafeFloat(legScale, DefaultLegScale), 0.0f, 5.0f);
        string state = CurrentLifeState();
        string personality = CurrentLifePersonality();
        if (state == LifeStateSleeping) return Mathf.Clamp(scale * 0.32f, 0.0f, 5.0f);
        if (state == LifeStateQuiet) scale *= 0.70f;
        else if (state == LifeStateShy) scale *= 1.12f;
        else if (state == LifeStateActive) scale *= 1.30f;
        if (personality == LifePersonalityShy) scale *= 1.20f;
        else if (personality == LifePersonalityBold) scale *= 1.10f;
        if (CurrentLifeAffection() == LifeAffectionDislike) scale *= 1.12f;
        else if (CurrentLifeAffection() == LifeAffectionShy) scale *= 1.18f;
        else if (CurrentLifeAffection() == LifeAffectionLike) scale *= 1.04f;
        return Mathf.Clamp(scale, 0.0f, 5.0f);
    }

    float EffectiveBreathAmount()
    {
        string mode = CurrentMotionMode();
        float baseAmount = 0.0100f;
        if (mode == LifeMotionSmall) baseAmount = 0.0060f;
        else if (mode == LifeMotionLarge) baseAmount = 0.0150f;
        return baseAmount * Mathf.Max(0.0f, EffectiveBreathScale());
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
        return baseAmount * Mathf.Max(0.0f, EffectiveBreathScale());
    }

    float EffectiveBreathRotationDegrees()
    {
        string mode = CurrentMotionMode();
        float baseDegrees = 1.00f;
        if (mode == LifeMotionSmall) baseDegrees = 0.55f;
        else if (mode == LifeMotionLarge) baseDegrees = 1.45f;
        return baseDegrees * Mathf.Max(0.0f, EffectiveBreathScale());
    }

    float EffectiveShoulderSwayAmount()
    {
        string mode = CurrentMotionMode();
        float baseAmount = 0.0040f;
        if (mode == LifeMotionSmall) baseAmount = 0.0025f;
        else if (mode == LifeMotionLarge) baseAmount = 0.0060f;
        return baseAmount * Mathf.Max(0.0f, EffectiveShoulderSwayScale());
    }

    bool IsCover100Mode()
    {
        return randomCoverEnabled != null && randomCoverEnabled.val && EffectiveCoverFrequency() >= 99.5f;
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
        float configured = Mathf.Clamp(SafeFloat(coverMaxDistance, DefaultCoverMaxDistance), 0.05f, 1.50f);
        // v044: honor the visible Life Cover Max Distance slider. Older builds always returned
        // the hard-coded 0.58 in Normal mode, which made selected head targets stop short.
        // Head labels still get their own larger minimum in EffectiveCoverMaxDistanceForTarget().
        if (IsCover100Mode())
        {
            if (mode == LifeMotionSmall) return Mathf.Max(configured, 0.62f);
            if (mode == LifeMotionLarge) return Mathf.Max(configured, 1.00f);
            return Mathf.Max(configured, 0.82f);
        }
        if (mode == LifeMotionSmall) return Mathf.Min(configured, 0.42f);
        if (mode == LifeMotionLarge) return Mathf.Max(configured, 0.78f);
        return configured;
    }

    void GetEffectiveInterval(out float min, out float max)
    {
        string state = CurrentLifeState();
        string personality = CurrentLifePersonality();
        string affection = CurrentLifeAffection();
        if (state == LifeStateSleeping)
        {
            min = 6.0f;
            max = 14.0f;
            return;
        }
        if (state == LifeStateQuiet)
        {
            min = 4.0f;
            max = 10.0f;
            if (personality == LifePersonalityBold) { min = 3.0f; max = 7.0f; }
            ApplyAffectionIntervalBias(ref min, ref max, affection);
            return;
        }
        if (state == LifeStateShy)
        {
            min = 3.2f;
            max = 8.5f;
            if (personality == LifePersonalityBold) { min = 2.6f; max = 6.8f; }
            ApplyAffectionIntervalBias(ref min, ref max, affection);
            return;
        }
        if (state == LifeStateActive && !IsCover100Mode())
        {
            min = 2.0f;
            max = 5.0f;
            if (personality == LifePersonalityShy) { min = 1.8f; max = 4.5f; }
            else if (personality == LifePersonalityBold) { min = 1.4f; max = 3.5f; }
            ApplyAffectionIntervalBias(ref min, ref max, affection);
            return;
        }

        string mode = CurrentMotionMode();
        if (IsCover100Mode())
        {
            // v010: Cover 100 is a deliberate stress/visibility mode.
            if (mode == LifeMotionSmall) { min = 1.0f; max = 1.8f; ApplyAffectionIntervalBias(ref min, ref max, affection); return; }
            if (mode == LifeMotionLarge) { min = 0.25f; max = 0.65f; ApplyAffectionIntervalBias(ref min, ref max, affection); return; }
            min = 0.45f;
            max = 1.00f;
            ApplyAffectionIntervalBias(ref min, ref max, affection);
            return;
        }
        if (mode == LifeMotionSmall)
        {
            min = 6.0f;
            max = 14.0f;
            ApplyAffectionIntervalBias(ref min, ref max, affection);
            return;
        }
        if (mode == LifeMotionLarge)
        {
            min = 3.0f;
            max = 7.0f;
            ApplyAffectionIntervalBias(ref min, ref max, affection);
            return;
        }
        min = DefaultIntervalMin;
        max = DefaultIntervalMax;
        ApplyAffectionIntervalBias(ref min, ref max, affection);
    }

    void ApplyAffectionIntervalBias(ref float min, ref float max, string affection)
    {
        if (affection == LifeAffectionLike)
        {
            min *= 0.90f;
            max *= 0.90f;
        }
        else if (affection == LifeAffectionDislike)
        {
            min *= 1.08f;
            max *= 1.12f;
        }
        else if (affection == LifeAffectionShy)
        {
            min *= 0.92f;
            max *= 1.02f;
        }
    }

    float EffectiveLegBaseRotationDegrees()
    {
        string mode = CurrentMotionMode();
        // v028: v027 was too subtle in many poses because thigh rotation alone was masked by knee/foot IK.
        // Make scale=5 clearly visible while keeping scale=1 usable as a small base motion.
        float baseAmount = 2.40f;
        if (mode == LifeMotionSmall) baseAmount = 1.35f;
        else if (mode == LifeMotionLarge) baseAmount = 4.20f;
        float scale = EffectiveLegScale();
        return baseAmount * scale;
    }

    float EffectiveLegBasePositionAmount()
    {
        string mode = CurrentMotionMode();
        // Optional thigh-control position assist. v093 default keeps this OFF for better coexistence; enable only when you need clearly visible leg base sway.
        float baseAmount = 0.010f;
        if (mode == LifeMotionSmall) baseAmount = 0.005f;
        else if (mode == LifeMotionLarge) baseAmount = 0.018f;
        float scale = EffectiveLegScale();
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
            + " / state=" + CurrentLifeState()
            + " / motion=" + CurrentMotionMode()
            + " / breath=" + (breathLoopRoutine != null ? "ON" : "OFF")
            + " / headLook=" + (IsHeadLookEnabled() ? "ON" : "OFF")
            + " / leg=" + (IsLegMotionEnabled() ? "ON" : "OFF")
            + " / legPos=" + (IsLegPositionAssistEnabled() ? "ON" : "OFF")
            + " / legScale=" + EffectiveLegScale().ToString("F1", CultureInfo.InvariantCulture)
            + " / legBase=" + (legBaseLoopRoutine != null ? "ON" : "OFF")
            + " / hbaPause=" + (legPausedByHba ? "ON" : "OFF")
            + " / dockingPause=" + (legPausedByExternalDockingPoseAssist ? "ON" : "OFF")
            + " / last=" + lastGesture
            + " / next=" + nextIn.ToString("F1", CultureInfo.InvariantCulture) + "s";
    }


    void NormalLog(string message)
    {
        SuperController.LogMessage("[HumanLifeAction] " + message);
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
