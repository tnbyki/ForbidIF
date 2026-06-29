// HBA_V089_TARGET_UPPER_BEHIND_GUARD_BUILD 2026-06-29: When the selected/nearest other Person is behind this Person, normal Cover upper-body targets (Head/Head Side/Neck/Chest/Nipple) are filtered out before selection so hands do not try to reach target-side upper body behind the back. Keeps v088 HBA->HLA delegation diagnostics and PushAway back-stretch limit.
// HBA_V087_HLA_DELEGATE_DIAGNOSTICS_READY_LOG_BUILD 2026-06-29: Adds always-visible READY and HBA->HLA CHECK diagnostics so it is obvious whether this HBA build is loaded and whether Head/Hip Back selections match delegation; keeps v086 matching and v085 PushAway back stretch limit.
// HBA_V083_DELEGATE_SELF_HEAD_HIP_TO_HLA_BUILD 2026-06-29: RandomHandCover delegates completed self Head/Head Side and Hip Back cover targets to same-Person HumanLifeAction external actions when present; adds direct HBA buttons/actions for HLA Self Head/Hip delegation and falls back to old HBA cover when HLA is absent.
// HBA_SELF_FACE_BREAST_SIZE_AVOID_BUILD 2026-06-28: Self Head/Head Side/Neck hand cover now measures self L/R nipple protrusion from chestControl; flat/small chest keeps the normal path, while large breast + chest-crossing line uses outward/up avoid path.
// HBA_HEAD_COVER_REACH_FIX_BUILD 2026-06-28: Raises upper/head hand-cover command and too-far limits so selected Self Head/Head Side/Neck cover can actually reach instead of stopping around shoulder distance. Keeps TG held-target-hand UID strict guard and self-face chest avoid path.
// HBA_SELF_FACE_CHEST_AVOID_PATH_BUILD 2026-06-28: Self Head/Head Side/Neck hand cover uses a chest-proximity line test and only routes outward/up when the hand-to-face path would cross the chest area.
// HBA_HAND_COVER_IGNORE_HAND_IK_ON_FIX_BUILD 2026-06-28: RandomHandCover no longer treats Hand IK PositionState.On as unavailable; hand cover can run from normal IK-on poses while TG-held target hand guard remains active.
// HBA_TG_HELD_HAND_TARGET_UID_STRICT_BUILD 2026-06-28: Uses TargetGrabber exported TG Held Target Person UID to block only the matching target Person's held L/R hand; legacy same-atom only, no cross-person fallback.
// HBA_TG_HELD_HAND_GLOBAL_TARGET_UID_SCAN_BUILD 2026-06-28: Scans all scene TargetGrabber instances and matches TG Held Target Person UID, so held target-hand flags are read even when TargetGrabber is installed on the grabbing/self Person, not on the target Person.
// HBA_TG_HELD_HAND_SIDE_ONLY_BLOCK_BUILD 2026-06-28: TargetGrabber L/R held-target-hand flags now block only the matching HBA hand/twitch part instead of disabling all Hand Cover.
// HBA_TG_HELD_HAND_METHOD_ALIAS_FIX_BUILD 2026-06-28: Fixes v074 compile error by adding the missing IsTargetGrabberHeldTargetHandActive wrapper used by hand twitch gating.
// HBA_TG_HELD_HAND_SKIP_HAND_MOTION_BUILD 2026-06-27: Extends TG held-target-hand guard to HBA body twitch hand parts, and links TargetGrabber held flags by bool-param scan instead of storable-id name only.
// HBA_PUSHAWAY_BACK_STRETCH_LIMIT_BUILD 2026-06-28: Limits only backward PushAway stretch; when a far PushAway target pulls the hand too far behind the body, it stops at a capped rear position and releases IK instead of over-reaching.
// HBA_TG_HELD_HAND_SKIP_COVER_BUILD 2026-06-27: Reads TargetGrabber hidden held-target-hand flags and skips/restores HBA Hand Cover/PushAway while TG is holding target L/R Hand, preventing HBA self-hand motion from dragging the TG-held target hand.
// HBA_COVER_AUTO_RESTORE_PROGRESS_ZERO_BUILD 2026-06-27: While a Hand Cover is active, automatically restores the covered hand when HBA_Progress returns to zero or HBA_Active becomes false after seeing an active HBA signal; defaults ON.
// HBA_PUSHAWAY_STRETCH_LOG_OFF_FIX_BUILD 2026-06-27: Fix build; keeps PushAway stretch-to-reach behavior but defaults Log Cover Hand OFF so compact Cover/PushAway logs appear only when explicitly enabled.
// HBA_PUSHAWAY_STRETCH_TO_REACH_BUILD 2026-06-27: PushAway Person targets no longer disappear when beyond reach; far Head/Chest/Hip/Thigh are converted into a reachable stretch point in the same direction, with Log Cover Hand diagnostics.
// HBA_PUSHAWAY_HEAD_WEIGHT_EFFECTIVE_REACH_BUILD 2026-06-27: Forces effective PushAway reach to at least the 1.20 default even if an old scene value remains, and weights PushAway Person Head as 3 slots so Person#2 Head appears more often.
// HBA_PUSHAWAY_PERSON_REACH_TUNE_BUILD 2026-06-27: Raises PushAway Person reach default/range for other-Person targets, logs per-target reach skips with Log Cover Hand, and logs fallback when PushAway branch has no reachable target.
// HBA_PUSHAWAY_FORCE_BUTTON_LOG_PROBE_BUILD 2026-06-27: Adds HBA_Cover_RandomHand_PushAway / HBR_Cover_RandomHand_PushAway to force selected PushAway Person targets, and logs mix roll / empty PushAway candidates when Log Cover Hand is ON.
// HBA_LOG_COVER_HAND_TOGGLE_BUILD 2026-06-27: Adds Log Cover Hand toggle; current fix defaults it OFF to control compact Cover selected / PushAway selected logs independently from Debug Log.
// HBA_PUSHAWAY_PERSON_COMBO_BUILD 2026-06-27: Adds PushAway Person chooser; PushAway targets now use the selected non-self Person Head/Chest/Hip/nearest Thigh with reach filter ON. Defaults to the nearest other Person.
// HBA_PUSHAWAY_SELF_REACH_NEAREST_THIGH_BUILD 2026-06-27: Changes Self PushAway targets to Self Head/Chest/Hip plus nearest reachable Self Thigh, with reach filter ON for all self PushAway targets. Keeps v063 Cover same-side filters and Debug-only Cover logs.
// HBA_COVER_SAME_SIDE_LOG_DEBUG_ONLY_FIX_BUILD 2026-06-27: Fix build; keeps v062 Head3/Nipple and same-side Head Side/Thigh routing, but suppresses compact Cover selected logs unless Debug is ON.
// HBA_COVER_NIPPLE_HEAD3_SAME_SIDE_BUILD 2026-06-27: Replaces normal Chest cover weight with Target L/R Nipple, raises Head cover weight to 3, and restricts Head Side/L/R Thigh cover targets to the same-side hand with (L)/(R) labels.
// HBA_G_PROGRESS_CHEST_HUMAN_S_CURVE_BUILD 2026-06-27: Adds soft top wobble, S-curve side sway, and short random follow-miss windows so G Progress Chest Follow feels less linear/mechanical.
// HBA_G_PROGRESS_CHEST_SPRING_FOLLOW_BUILD 2026-06-27: Replaces the G Progress Chest Follow lerp target with spring/inertia physics, adds subtle Up/Side mix, and keeps progress<=0.005 idle release.
// HBA_G_PROGRESS_IDLE_RELEASE_BUILD 2026-06-27: When G Progress Chest Follow is ON, progress<=0.005 or HBA inactive releases/restores chest IK and waits; chest control starts only when progress becomes active again.
// HBA_G_PROGRESS_DEFAULT_ON_REVERSE_DELAY_BUILD 2026-06-27: Defaults G Progress Chest Follow ON, reverses progress-driven chest offset direction, and adds a small delayed-progress response slider.
// HBA_G_PROGRESS_CHEST_ONLY_FOLLOW_BUILD 2026-06-27: G Progress Follow and manual G Parallel shake now move only chestControl; headControl is not captured/moved/restored to avoid conflicts with head actions.
// HBA_G_PROGRESS_CHEST_HEAD_FOLLOW_BUILD 2026-06-27: Adds toggle-driven G Progress Chest/Head Follow; while ON, fine HBA_Progress changes drive large chest/head parallel offsets along the detected G axis, with clamp/decay/restore.
// HBA_G_PARALLEL_SHAKE_RETURN_BUILD 2026-06-27: Changes HBA_G_Parallel_ChestHead from one push-and-return into a short G-axis parallel shake: forward, small reverse, small forward, then restore. Keeps compact Cover selected logs outside Debug.
// HBA_G_PARALLEL_COMPILE_FIX_COVER_LOG_BUILD 2026-06-27: Fixes v053 compile error by removing stray reason reference from RandomKnee route and restores compact Cover selected logs outside Debug for tuning.
// HBA_COVER_COMPLY_DEFAULT_ON_LOG_DEBUG_ONLY_FIX_BUILD 2026-06-27: Fix build; keeps Hand Cover Comply Hold ON by default but returns Cover selected / PushAway selected logs to Debug-only.
// HBA_COVER_COMPLY_HOLD_TOGGLE_BUILD 2026-06-27: Adds Hand Cover Comply Hold toggle; when ON, RandomHandCover moves with PositionState.On then switches the held hand PositionState to Comply after reaching the cover point.
// HBA_COVER_CHANCE50_LOG_DEBUG_ONLY_BUILD 2026-06-27: Sets Cover Random Chance default to 50% and makes the short Hand Cover selection log obey Debug ON; status text still shows the selected target.
// HBA_HAND_COVER_SELECTION_LOG_SHORT_BUILD 2026-06-27: Shortens the always-visible Hand Cover selection log to only "Cover selected: L Hand -> Target" / "PushAway selected: R Hand -> Target" while keeping verbose details behind Debug.
// HBA_HAND_COVER_SELECTION_LOG_ALWAYS_BUILD 2026-06-27: Shows the selected Hand Cover target even when Debug is OFF; updates status immediately and logs a compact Cover selected line.
// HBA_HAND_COVER_HEAD_SIDE_DOWN_SELF_NIPPLE_BUILD 2026-06-27: Sets Head Side L/R down offset to 0.10 and replaces Self Chest PushAway with Self L/R Nipple point targets while keeping Hand Cover behavior unchanged.
// HBA_HAND_COVER_EXTRA_TARGETS_BUILD 2026-06-27: Adds Belly, G, and L/R head-side offset targets to RandomHandCover while keeping behavior/chance/restore unchanged.
// HBA_G_PARALLEL_CHEST_HEAD_BUILD 2026-06-27: Adds manual/action HBA_G_Parallel_ChestHead; moves chestControl/headControl together along the detected G/LabiaTrigger axis, then returns and restores controller states.
// HBA_KNEE_PAIR_ONLY_DISABLE_BONUS_LOCAL_BUILD 2026-06-26: Disables automatic RandomHand bonus knee-local/single knee nudges and keeps RandomKnee reactions to Pair Open/Close only; pair fallback now skips instead of running a single nudge.
// HBA_RANDOM_HAND_BONUS_KNEE_MICRO_BUILD 2026-06-26: Makes RandomHand bonus knee local a tiny micro reaction only: about one-third of v042 again, no overshoot, much smaller arc, shorter hold, and snapshots/restores only the selected knee.
// HBA_RANDOM_HAND_BONUS_KNEE_1THIRD_BUILD 2026-06-26: Reduces RandomHand bonus knee local nudge travel to roughly one-third while keeping chance/timing unchanged.
// HBA_RANDOM_KNEE_PAIR_Y_GUARD_BUILD 2026-06-26: Pair Open/Close now skips when L/R knee heights are far apart and falls back to a single safe knee nudge; single knee selection prefers the higher/stable knee when vertical gap is large to avoid whole-body drop.
// HBA_RANDOM_KNEE_NO_DROP_GUARD_BUILD 2026-06-26: RandomKnee single Knee->Thigh no longer lowers knee target Y or temporarily turns Foot IK off; Pair/Free still leave feet untouched to prevent hip/root drop.
// HBA_RANDOM_KNEE_BRANCH_SNAPSHOT_BUILD 2026-06-26: RandomKnee now captures/restores only the actually affected knee/foot branch; single Knee->Thigh never snapshots/restores the opposite knee or opposite foot, Pair snapshots knees only and no feet.
// HBA_RANDOM_KNEE_FOOT_OFF_SINGLE_THIGH_ONLY_BUILD 2026-06-26: Limits temporary Foot IK Off to the single Knee->Thigh branch only; Pair Open/Close and Knee Free leave Foot IK untouched to prevent whole-body drop.
// HBA_RANDOM_KNEE_RESTORE_THIGH_ROUTE_BUILD 2026-06-26: Restores the single-knee branch to nearest safe Thigh anchor and makes Cover Restore return knees smoothly instead of snapping.
// HBA_RANDOM_KNEE_MOVING_FOOT_ONLY_TEMP_OFF_BUILD 2026-06-26: RandomKneeReaction temporarily turns Off only the Foot IK on the same side as the Knee being actively moved; Knee Free remains knee-only and foot transforms are never repositioned.
// HBA_RANDOM_KNEE_REACTION_SOFTER_BUILD 2026-06-26: Softens RandomKneeReaction by reducing travel, using peak->settle->start two-stage return, and delaying state restore by a short stabilization frame.
// HBA_RANDOM_KNEE_MOVE_OWNER_FIXED_BUILD 2026-06-26: RandomKneeToThigh now resolves moving knee/foot controllers explicitly on containingAtom so the non-plugin Person cannot be moved by ambiguous controller lookup.
// HBA_RANDOM_KNEE_REACTION_BUILD 2026-06-26: Reworks HBA_Cover_RandomKneeToThigh into random knee reaction: 80% overall, 20 small nudge / 30 pair open / 30 pair close / 20 free.
// HBA_BONUS_CHANCE50_KNEE_ACTION_BUILD 2026-06-26: Sets RandomHand bonus Knee/Elbow chance to 50% and adds standalone HBA/HBR Bonus Knee Nudge actions.
// HBA_HAND_BONUS_ELBOW_NUDGE_BUILD 2026-06-26: Adds a soft local elbow nudge bonus/test using the same restore-original approach as v083 knee nudge.
// HBA_HAND_BONUS_KNEE_RESTORE_ORIGINAL_BUILD 2026-06-26: Bonus KneeNudge avoids the final Comply/Off snap; it eases back to the start and restores the original knee IK state.
// HBA_HAND_BONUS_KNEE_SOFT_RELEASE_BUILD 2026-06-26: Softens RandomHand bonus Knee Nudge release by easing back before a short Comply phase, then IK Off.
// HBA_KNEE_NUDGE_TEST_BUTTON_FORCE_BONUS_BUILD 2026-06-26: RandomHand manual button always fires the small knee nudge bonus; adds a legs-only knee nudge test button/action.
// HBA_RANDOM_HAND_KNEE_NUDGE_NATURAL_ALTERNATE_BUILD 2026-06-25: RandomHand small knee nudge alternates L/R when possible and uses a slower arced motion.
// HBA_FAR_REACH_IK_OFF_BUILD
// HBA_RANDOM_HAND_FAIL_KNEE_NUDGE_BUILD 2026-06-25: If RandomHand/RandomKnee target is far beyond reach, stretch toward it briefly then turn that IK Off instead of holding an impossible command.
// HBA_RANDOM_HAND_SLOW_RESTORE_BUILD 2026-06-25: RandomHand restore now adds a short linger and smooth eased return instead of instant snapping back.
// HBA_RANDOM_HAND_UPPER_REACH_SNAP_FIX_BUILD 2026-06-25: Lets RandomHand upper targets actually reach Head/Neck by using a larger command clamp and skipping soft-snap for upper labels.
// HBA_RANDOM_HAND_UPPER_TARGETS_RESTORE_BUILD 2026-06-25: Keeps RandomHand upper targets reachable/weighted again; Self Head/Chest bypass PushAway reach filter and Head/Neck/Chest get cover weight.
// HBA_RANDOM_KNEE_NEAREST_THIGH_BUILD 2026-06-25: RandomKnee uses thigh-side safe anchor, low cross chance, foot pre-free, and knee soft snap.
// HBA_RANDOM_KNEE_FORCE_ACTION_BUILD 2026-06-25: Adds force RandomKnee actions for HDU/manual external buttons; chance slider still applies to normal Event/TG/HBR routes.
// HBA_RANDOM_KNEE_DEEP_DEFAULT_BUILD 2026-06-25: Defaults Deep Action to HBA_Cover_RandomKneeToThigh while keeping v066 RandomHand fallback and RandomKnee behavior.
// HBA_RANDOM_HAND_FALLBACK_BOTH_ON_BUILD 2026-06-25: If both hand IK position states are On, RandomHandCover now falls back to either hand instead of skipping no-free-hand.
// HBA_RANDOM_KNEE_LEGACY_TG_ALIAS_BUILD 2026-06-25: Keeps random knee chance30 minimal build, but accepts old saved TG/Test action names as hidden aliases so Start/Inside TG Atom routes do not silently die.
// HBA_RANDOM_KNEE_MINIMAL_CHANCE30_BUILD 2026-06-25: Adds random one-knee action: moveAtom L/R Knee -> goalAtom L/R Thigh with same-side weighting, or free one knee; keeps v055 direct test.
// HBA_RANDOM_HAND_COVER_HIP2_THIGH_BUILD 2026-06-25: Changes Hip Back weight from 3 to 2 and adds L/R Thigh cover targets. Hold/rot-fixed behavior unchanged.
// HBA_RANDOM_HAND_COVER_BUILD 2026-06-25: Adds random free-hand cover action/button. Moves one non-held hand to non-explicit body cover targets, then restores. Adds HBA_ and HBR_ actions.
// HBA_RANDOM_HAND_COVER_SOFT_SNAP_BUILD 2026-06-25: Adds hidden fixed soft-snap after RandomHandCover move so IK holds at the reachable body-hand position instead of pushing through.
// HBA_RANDOM_HAND_COVER_SCOPE_DEFAULT_BUILD 2026-06-25: Adds Cover Scope All/Self/Target, defaults chance to 80%, and defaults Inside Active Action to HBA_Cover_RandomHand.
// HBA_RANDOM_HAND_COVER_PUSHAWAY_MIX_BUILD 2026-06-25: Keeps HBA_Cover_RandomHand button but mixes normal Cover with reachable PushAway hand placement targets, adding Mix/Reach/Offset sliders.
// HBA_RANDOM_HAND_COVER_CHANCE_BUILD 2026-06-25: Adds Cover Random Chance % slider under HBA_Cover_RandomHand; event/action/TG calls obey probability while manual button forces execution.
// HBA_RANDOM_HAND_COVER_HIP_BACK_WEIGHT_BUILD 2026-06-25: Makes Hip Back easier to appear in RandomHandCover by weighting it higher; hold/rot-fixed behavior unchanged.
// HBA_RANDOM_HAND_COVER_HOLD_BUILD 2026-06-25: RandomHandCover moves hand position only, turns hand rotation IK OFF/fixed, and keeps the hand at the cover point until explicit restore/reset or the next cover action.
// HBA_COVER_KNEE_THIGH_DIRECT_BUILD 2026-06-25: Test Knee/Thigh cover runs immediately outside the latest-only queue and temporarily suppresses event Cover overwrite.
// HBA_COVER_REACH_ELBOW_ASSIST_BUILD 2026-06-25: Clamps hand-cover command distance and temporarily relaxes the same-side elbow so Knee/Thigh test can visibly move without stale coroutine overwrite.
// HBA_TARGET_KNEE_TO_SELF_THIGH_TEST_BUILD 2026-06-25: Replaces the manual cover test with Target L/R Knee -> Self L/R Thigh IK move; keeps random cover behavior unchanged.
// HBA_COVER_SELF_THIGH_TEST_BUILD 2026-06-25: Changes the manual cover test button to Self L/R Thigh only; removes Target L/R Knee from the test route.
// HBA_TARGET_KNEE_OPPOSITE_PERSON_BUILD 2026-06-25: Swaps the knee test route so the Person that did NOT move in v054 is moved: containingAtom knees -> nearest other Person thighs.
// HBA_TARGET_KNEE_TO_SELF_THIGH_FREE_MIX_BUILD 2026-06-25: Adds direct test action that moves target L/R knee IK to self L/R thigh or randomly frees target knees; restore via HBA_Cover_Restore.
// HBA_TARGET_KNEE_TO_SELF_THIGH_RUNSERIAL_FIX_BUILD 2026-06-25: Fixes test route stopping after pre-free by assigning run serial after restart restore.
// HBA_TARGET_KNEE_TO_SELF_THIGH_PREFREE_BUILD 2026-06-25: Test action always moves target L/R knee IK to self L/R thigh, with a brief pre-free relax before the move.
// HBA_SLOW_MORE_SENSITIVE_BUILD 2026-06-24: Widens the Slow band so Auto Line Slow is less likely to be classified as Active, while keeping v032 Fast compromise and v035 Face Time Scale.
// HBA_FACE_MORPH_TIME_SCALE_BUILD 2026-06-24: Adds Face Time Scale to slow Eyes/Mouth morph playback without changing HBA classifier v032.
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
    JSONStorableBool logCoverHand;
    JSONStorableBool twitchBody;
    JSONStorableBool queueLastAction;

    JSONStorableBool headActionEnabled;
    JSONStorableStringChooser headWeakPreset;
    JSONStorableStringChooser headNormalPreset;
    JSONStorableStringChooser headStrongPreset;
    JSONStorableStringChooser headOnlyPreset;
    JSONStorableFloat headTimeScale;
    JSONStorableFloat faceTimeScale;
    JSONStorableFloat twitchMotionScale;
    JSONStorableFloat twitchSideScale;
    JSONStorableFloat twitchUpScale;
    JSONStorableFloat twitchForwardScale;
    JSONStorableFloat twitchChestScale;
    JSONStorableFloat twitchHipScale;
    JSONStorableFloat twitchLimbScale;
    JSONStorableStringChooser handCoverScope;
    JSONStorableFloat handCoverChance;
    JSONStorableFloat randomKneeToThighChance;
    JSONStorableFloat handCoverPushAwayMix;
    JSONStorableStringChooser handCoverPushAwayPerson;
    JSONStorableFloat handCoverPushAwayReach;
    JSONStorableFloat handCoverPushAwayOffset;
    JSONStorableBool handCoverComplyHold;
    JSONStorableBool handCoverAutoRestoreOnProgressZero;
    JSONStorableFloat gParallelChestHeadAmount;
    JSONStorableBool gProgressChestHeadFollow;
    JSONStorableFloat gProgressChestHeadAmount;
    JSONStorableFloat gProgressChestHeadMaxOffset;
    JSONStorableFloat gProgressChestHeadReturnSpeed;
    JSONStorableFloat gProgressChestHeadSmooth;
    JSONStorableFloat gProgressChestHeadDelay;
    JSONStorableFloat gProgressChestHeadUpMix;
    JSONStorableFloat gProgressChestHeadSideMix;
    JSONStorableFloat gProgressChestHeadSCurve;
    JSONStorableFloat gProgressChestHeadTopWobble;
    JSONStorableFloat gProgressChestHeadMissChance;

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
    HandCoverSnapshot activeHandCoverSnapshot;
    TargetKneeToSelfThighSnapshot activeTargetKneeToSelfThighSnapshot;
    HandBonusElbowSnapshot activeHandBonusElbowSnapshot;
    Coroutine directHandCoverRoutine;
    Coroutine directRandomKneeRoutine;
    Coroutine targetKneeRestoreRoutine;
    Coroutine handCoverRestoreRoutine;
    Coroutine gParallelChestHeadRoutine;
    int handCoverRunSerial = 0;
    int targetKneeToSelfThighRunSerial = 0;
    int handFallbackKneeLastSide = 0; // -1=L, +1=R, 0=not chosen yet. Used only for the small RandomHand bonus nudge.
    int handBonusElbowRunSerial = 0;
    int handBonusElbowLastSide = 0; // -1=L, +1=R, 0=not chosen yet. Used only for the small RandomHand bonus elbow nudge.
    float suppressEventHandCoverUntil = -999.0f;
    bool handCoverAutoRestoreArmed = false;
    JSONStorable targetGrabberStorable;
    JSONStorableBool tgHeldTargetLHandParam;
    JSONStorableBool tgHeldTargetRHandParam;
    bool tgHeldTargetLHandCached = false;
    bool tgHeldTargetRHandCached = false;
    string tgHeldTargetSourceCached = "";
    float nextTargetGrabberResolveTime = -999.0f;

    FreeControllerV3 headControl;
    HeadControlSnapshot activeHeadSnapshot;
    ControllerPoseSnapshot activeGParallelChestSnapshot;
    ControllerPoseSnapshot activeGParallelHeadSnapshot;
    ControllerPoseSnapshot activeGProgressChestSnapshot;
    ControllerPoseSnapshot activeGProgressHeadSnapshot;
    bool gProgressChestHeadActive = false;
    Vector3 gProgressChestStart = Vector3.zero;
    Vector3 gProgressHeadStart = Vector3.zero;
    Vector3 gProgressChestHeadDir = Vector3.zero;
    Vector3 gProgressChestHeadTargetOffset = Vector3.zero;
    Vector3 gProgressChestHeadCurrentOffset = Vector3.zero;
    Vector3 gProgressChestHeadVelocity = Vector3.zero;
    float gProgressChestHeadLastProgress = 0.0f;
    float gProgressChestHeadDelayedProgress = 0.0f;
    float gProgressChestHeadSideSign = 1.0f;
    float gProgressChestHeadShapePhase = 0.0f;
    float gProgressChestHeadMissUntil = -999.0f;
    float gProgressChestHeadLastMissLogTime = -999.0f;
    string gProgressChestHeadAtom = "";
    string gProgressChestHeadTransform = "";

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
    const float FaceTimeScaleDefault = 1.35f; // v035: slow Eyes/Mouth morph playback a little for high-FPS scenes
    const float FaceTimeScaleMin = 0.50f;
    const float FaceTimeScaleMax = 2.50f;
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
    const float HbaStatusSpeedSlow = 0.420f;      // v036: wider Slow band; Auto Line Slow was drifting into Active
    const float HbaStatusSpeedFastEnter = 1.050f; // v032: stronger Fast gate; normal Auto Line should stay Active, Fast is for true fast peaks
    const float HbaStatusSpeedFastExit = 0.620f;  // v032: exit threshold below enter to avoid Active/Fast flapping after a real Fast
    const float HbaInSpeedSmoothing = 0.35f;
    const float HbaStatusHoldSeconds = 0.60f;
    const float HbaStartDecisionDelay = 0.25f;
    const float HbaStartSlowSpeed = 0.28f; // v036: Start Slow should catch slower line movement before it becomes Normal/Active
    const float HbaStartFastSpeed = 1.20f; // v032: avoid classifying normal Auto Line start as Fast
    const float HbaInsideFirstDelay = 0.25f;
    const float HbaInsideMotionCooldown = 1.00f;
    const float HbaInsideHoldDelay = 0.60f;
    const float HbaInsideFastEnterSeconds = 0.35f; // v032: require Fast to persist a little longer before switching from Active
    const float HbaInsideFastExitSeconds = 0.35f;
    const float HbaInsideSlowEnterSeconds = 0.18f; // v036: shorter Active grace so real Slow does not stay Active too long
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
    const string HbaActionCoverRandomHand = "HBA_Cover_RandomHand";
    const string HbrActionCoverRandomHand = "HBR_Cover_RandomHand";
    const string HbaActionHlaSelfHead = "HBA_HLA_SelfHead";
    const string HbaActionHlaSelfHip = "HBA_HLA_SelfHip";
    const string HbaActionCoverRandomHandPushAway = "HBA_Cover_RandomHand_PushAway";
    const string HbrActionCoverRandomHandPushAway = "HBR_Cover_RandomHand_PushAway";
    const string HbaActionCoverRestore = "HBA_Cover_Restore";
    const string HbrActionCoverRestore = "HBR_Cover_Restore";
    // Hidden legacy aliases: older saved scenes may still store these in Start/Inside/Deep/End TG Atom.
    // Do not show them in the UI choices, but treat them as safe HBA actions and map them to the new random knee route.
    const string HbaActionCoverTestKneeThighLegacy = "HBA_Cover_Test_TargetKneeToSelfThigh";
    const string HbrActionCoverTestKneeThighLegacy = "HBR_Cover_Test_TargetKneeToSelfThigh";
    const string HbaActionCoverRandomKneeToThigh = "HBA_Cover_RandomKneeToThigh";
    const string HbrActionCoverRandomKneeToThigh = "HBR_Cover_RandomKneeToThigh";
    const string HbaActionCoverRandomKneeToThighForce = "HBA_Cover_RandomKneeToThigh_Force";
    const string HbrActionCoverRandomKneeToThighForce = "HBR_Cover_RandomKneeToThigh_Force";
    const string HbaActionBonusKneeNudge = "HBA_Bonus_KneeNudge";
    const string HbrActionBonusKneeNudge = "HBR_Bonus_KneeNudge";
    const string HbaActionTestKneeNudge = "HBA_Test_KneeNudge";
    const string HbrActionTestKneeNudge = "HBR_Test_KneeNudge";
    const string HbaActionTestElbowNudge = "HBA_Test_ElbowNudge";
    const string HbrActionTestElbowNudge = "HBR_Test_ElbowNudge";
    const string HbaActionGParallelChestHead = "HBA_G_Parallel_ChestHead";
    const string HbrActionGParallelChestHead = "HBR_G_Parallel_ChestHead";
    const float HandCoverMoveSeconds = 0.32f;
    const float HandCoverHoldSeconds = 0.42f;
    const float HandCoverReturnSeconds = 0.68f;
    const float HandCoverReturnLingerSeconds = 0.10f;
    const float HandCoverSoftSnapDelay = 0.12f;
    const float HandCoverSoftSnapMaxDistance = 0.12f;
    const float HandCoverSoftSnapMinDistance = 0.004f;
    const float HandCoverManualSuppressEventSeconds = 3.00f;
    const float HandCoverCommandMaxDistance = 0.46f;
    const float HandCoverUpperCommandMaxDistance = 1.05f;
    const float HandCoverKneeThighCommandMaxDistance = 0.42f;
    const float HandCoverTooFarDistance = 0.85f;
    const float HandCoverUpperTooFarDistance = 1.35f;
    const float HandCoverKneeThighTooFarDistance = 0.80f;
    const float HandCoverTooFarIkOffDelay = 0.12f;
    const float TargetKneeToSelfThighMoveSeconds = 0.32f;
    const float TargetKneeToSelfThighPreFreeSeconds = 0.10f;
    const float TargetKneeToThighSoftSnapDelay = 0.12f;
    const float TargetKneeToThighSoftSnapMinDistance = 0.004f;
    const float TargetKneeToThighSoftSnapMaxDistance = 0.140f;
    const float TargetKneeToThighSafeOutwardOffset = 0.160f;
    const float TargetKneeToThighSafeDownOffset = 0.100f;
    const float TargetKneeToThighSafeForwardOffset = 0.040f;
    const float RandomKneeToThighTooFarDistance = 0.65f;
    const float RandomKneeToThighTooFarStretchDistance = 0.50f;
    const float RandomKneeToThighTooFarIkOffDelay = 0.12f;
    const float RandomKneeToThighMoveChance = 80.0f;
    const float RandomKneeToThighSameSideChance = 90.0f;
    const float RandomKneeToThighChanceDefault = 80.0f;
    const float RandomKneeToThighChanceMin = 0.0f;
    const float RandomKneeToThighChanceMax = 100.0f;
    const float RandomKneeReactionSmallNudgeChance = 20.0f;
    const float RandomKneeReactionPairOpenChance = 30.0f;
    const float RandomKneeReactionPairCloseChance = 30.0f;
    const float RandomKneeReactionFreeChance = 20.0f;
    const float RandomKneeReactionSingleAmountMin = 0.045f;
    const float RandomKneeReactionSingleAmountMax = 0.090f;
    const float RandomKneeReactionPairAmountMin = 0.060f;
    const float RandomKneeReactionPairAmountMax = 0.100f;
    const float RandomKneeReactionPairMinDistance = 0.160f;
    const float RandomKneeReactionPairMaxVerticalGap = 0.180f;
    const float RandomKneeReactionMoveSecondsMin = 0.75f;
    const float RandomKneeReactionMoveSecondsMax = 1.25f;
    const float RandomKneeReactionHoldSecondsMin = 0.02f;
    const float RandomKneeReactionHoldSecondsMax = 0.10f;
    const float RandomKneeReactionSettleSecondsMin = 0.32f;
    const float RandomKneeReactionSettleSecondsMax = 0.56f;
    const float RandomKneeReactionReturnSecondsMin = 0.60f;
    const float RandomKneeReactionReturnSecondsMax = 1.05f;
    const float RandomKneeReactionRestoreStabilizeSeconds = 0.06f;
    const float RandomKneeReactionReturnRatioMin = 0.36f;
    const float RandomKneeReactionReturnRatioMax = 0.62f;
    const float RandomKneeReactionArcSideMax = 0.012f;
    const float RandomKneeReactionArcUpMin = 0.006f;
    const float RandomKneeReactionArcUpMax = 0.020f;
    const float HandFailKneeNudgeAmount = 0.010f;
    const float HandFailKneeNudgeAmountMin = 0.004f;
    const float HandFailKneeNudgeAmountMax = 0.012f;
    const float HandFailKneeNudgeChance = 50.0f;
    const float HandFailKneeNudgeMoveSeconds = 0.42f;
    const float HandFailKneeNudgeMoveSecondsMin = 0.55f;
    const float HandFailKneeNudgeMoveSecondsMax = 0.95f;
    const float HandFailKneeNudgeHoldSeconds = 0.20f;
    const float HandFailKneeNudgeHoldSecondsMin = 0.02f;
    const float HandFailKneeNudgeHoldSecondsMax = 0.07f;
    const float HandFailKneeNudgeArcSideMax = 0.004f;
    const float HandFailKneeNudgeArcUpMin = 0.001f;
    const float HandFailKneeNudgeArcUpMax = 0.006f;
    const float HandFailKneeNudgeOvershoot = 0.000f;
    const float HandFailKneeNudgeThighGuideMaxDistance = 0.650f;
    const float HandFailKneeNudgeSettleBackMin = 0.650f;
    const float HandFailKneeNudgeSettleBackMax = 0.850f;
    const float HandFailKneeNudgeSettleSecondsMin = 0.120f;
    const float HandFailKneeNudgeSettleSecondsMax = 0.280f;
    const float HandFailKneeNudgeReleaseBackMin = 0.620f;
    const float HandFailKneeNudgeReleaseBackMax = 0.880f;
    const float HandFailKneeNudgeReleaseSecondsMin = 0.300f;
    const float HandFailKneeNudgeReleaseSecondsMax = 0.560f;
    const float HandFailKneeNudgeComplySecondsMin = 0.120f;
    const float HandFailKneeNudgeComplySecondsMax = 0.260f;
    const float HandBonusElbowNudgeChance = 50.0f;
    const float HandBonusElbowNudgeAmountMin = 0.025f;
    const float HandBonusElbowNudgeAmountMax = 0.060f;
    const float HandBonusElbowNudgeMoveSecondsMin = 0.48f;
    const float HandBonusElbowNudgeMoveSecondsMax = 0.86f;
    const float HandBonusElbowNudgeHoldSecondsMin = 0.04f;
    const float HandBonusElbowNudgeHoldSecondsMax = 0.14f;
    const float HandBonusElbowNudgeSettleSecondsMin = 0.10f;
    const float HandBonusElbowNudgeSettleSecondsMax = 0.24f;
    const float HandBonusElbowNudgeReleaseSecondsMin = 0.44f;
    const float HandBonusElbowNudgeReleaseSecondsMax = 0.78f;
    const float HandBonusElbowNudgeArcSideMax = 0.012f;
    const float HandBonusElbowNudgeArcUpMin = 0.006f;
    const float HandBonusElbowNudgeArcUpMax = 0.020f;
    const float HandBonusElbowNudgeOvershoot = 0.055f;
    const float HandCoverSurfaceOffset = 0.055f;
    const float SelfFaceChestAvoidRadius = 0.20f;
    const float SelfFaceChestAvoidChestForwardOffset = 0.08f;
    const float SelfFaceChestAvoidSideOffset = 0.18f;
    const float SelfFaceChestAvoidUpOffset = 0.16f;
    const float SelfFaceChestAvoidForwardOffset = 0.05f;
    const float SelfFaceBreastAvoidProtrusionStart = 0.095f;
    const float SelfFaceBreastAvoidProtrusionFull = 0.180f;
    const float SelfFaceBreastAvoidFallbackRadius = 0.155f;
    const float SelfFaceBreastAvoidMaxRadius = 0.320f;
    const float HandCoverHipBackOffset = 0.18f;
    const int HandCoverHipBackWeight = 2;
    const string HandCoverScopeAll = "All";
    const string HandCoverScopeSelf = "Self";
    const string HandCoverScopeTarget = "Target";
    const string HandCoverScopeDefault = HandCoverScopeAll;
    const float HandCoverChanceDefault = 50.0f;
    const float HandCoverChanceMin = 0.0f;
    const float HandCoverChanceMax = 100.0f;
    const float HandCoverPushAwayMixDefault = 30.0f;
    const float HandCoverPushAwayMixMin = 0.0f;
    const float HandCoverPushAwayMixMax = 100.0f;
    const float HandCoverPushAwayReachDefault = 1.20f;
    const float HandCoverPushAwayReachMin = 0.05f;
    const float HandCoverPushAwayReachMax = 2.50f;
    const float HandCoverPushAwayOffsetDefault = 0.075f;
    const float HandCoverPushAwayOffsetMin = 0.00f;
    const float HandCoverPushAwayOffsetMax = 0.25f;
    const float HandCoverPushAwayBackStretchLimit = 0.32f;
    const float HandCoverAutoRestoreProgressMin = 0.005f;
    const string HandCoverPushAwayPersonAuto = "Auto Other Person";
    const float GParallelChestHeadAmountDefault = 0.080f;
    const float GParallelChestHeadAmountMin = 0.000f;
    const float GParallelChestHeadAmountMax = 0.250f;
    const float GParallelChestHeadMoveSeconds = 0.18f;
    const float GParallelChestHeadReverseSeconds = 0.16f;
    const float GParallelChestHeadSmallForwardSeconds = 0.12f;
    const float GParallelChestHeadReturnSeconds = 0.34f;
    const float GParallelChestHeadReverseScale = 0.45f;
    const float GParallelChestHeadSmallForwardScale = 0.25f;
    const float GProgressChestHeadAmountDefault = 2.00f;
    const float GProgressChestHeadAmountMin = 0.00f;
    const float GProgressChestHeadAmountMax = 5.00f;
    const float GProgressChestHeadMaxOffsetDefault = 0.120f;
    const float GProgressChestHeadMaxOffsetMin = 0.000f;
    const float GProgressChestHeadMaxOffsetMax = 0.300f;
    const float GProgressChestHeadReturnDefault = 18.00f; // v060: used as spring strength. Higher returns faster/stronger.
    const float GProgressChestHeadReturnMin = 0.00f;
    const float GProgressChestHeadReturnMax = 40.00f;
    const float GProgressChestHeadSmoothDefault = 7.00f; // v060: used as velocity damping. Higher reduces bounce.
    const float GProgressChestHeadSmoothMin = 0.00f;
    const float GProgressChestHeadSmoothMax = 25.00f;
    const float GProgressChestHeadDelayDefault = 0.10f;
    const float GProgressChestHeadDelayMin = 0.00f;
    const float GProgressChestHeadDelayMax = 0.50f;
    const float GProgressChestHeadUpMixDefault = 0.15f;
    const float GProgressChestHeadUpMixMin = -0.50f;
    const float GProgressChestHeadUpMixMax = 0.50f;
    const float GProgressChestHeadSideMixDefault = 0.08f;
    const float GProgressChestHeadSideMixMin = -0.50f;
    const float GProgressChestHeadSideMixMax = 0.50f;
    const float GProgressChestHeadSCurveDefault = 0.020f;
    const float GProgressChestHeadSCurveMin = 0.000f;
    const float GProgressChestHeadSCurveMax = 0.080f;
    const float GProgressChestHeadTopWobbleDefault = 0.010f;
    const float GProgressChestHeadTopWobbleMin = 0.000f;
    const float GProgressChestHeadTopWobbleMax = 0.050f;
    const float GProgressChestHeadMissChanceDefault = 20.0f;
    const float GProgressChestHeadMissChanceMin = 0.0f;
    const float GProgressChestHeadMissChanceMax = 80.0f;
    const float GProgressChestHeadMissSecondsMin = 0.10f;
    const float GProgressChestHeadMissSecondsMax = 0.26f;
    const float GProgressChestHeadMissDeltaMin = 0.003f;
    const float GProgressChestHeadTopWobbleStartRatio = 0.72f;
    const float GProgressChestHeadSoftLimitExtraRatio = 1.18f;
    const float GProgressChestHeadDeltaClamp = 0.060f;
    const float GProgressChestHeadActiveProgressMin = 0.005f;
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
        HbaActionCoverRandomHand,
        HbaActionCoverRandomHandPushAway,
        HbaActionBonusKneeNudge,
        HbaActionGParallelChestHead,
        HbaActionCoverRestore,
        HbaActionCoverRandomKneeToThigh,
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
        HbaActionCoverRandomHand,
        HbaActionCoverRandomHandPushAway,
        HbaActionBonusKneeNudge,
        HbaActionGParallelChestHead,
        HbaActionCoverRestore,
        HbaActionCoverRandomKneeToThigh,
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
        HbaActionCoverRandomHand,
        HbaActionCoverRandomHandPushAway,
        HbaActionBonusKneeNudge,
        HbaActionGParallelChestHead,
        HbaActionCoverRestore,
        HbaActionCoverRandomKneeToThigh,
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
    readonly List<string> handCoverPushAwayPersonChoices = new List<string>() { HandCoverPushAwayPersonAuto };
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
        public bool runHandCover;
        public bool runTargetKneeToSelfThighTest;
        public string headPreset;
    }

    class HeadControlSnapshot
    {
        public Vector3 position;
        public Quaternion rotation;
        public FreeControllerV3.PositionState positionState;
        public FreeControllerV3.RotationState rotationState;
    }

    class HandCoverSnapshot
    {
        public FreeControllerV3 hand;
        public Vector3 position;
        public Quaternion rotation;
        public FreeControllerV3.PositionState positionState;
        public FreeControllerV3.RotationState rotationState;
        public FreeControllerV3 elbow;
        public Vector3 elbowPosition;
        public Quaternion elbowRotation;
        public FreeControllerV3.PositionState elbowPositionState;
        public FreeControllerV3.RotationState elbowRotationState;
    }

    class HandBonusElbowSnapshot
    {
        public FreeControllerV3 elbow;
        public Vector3 position;
        public Quaternion rotation;
        public FreeControllerV3.PositionState positionState;
        public FreeControllerV3.RotationState rotationState;
    }

    class ControllerPoseSnapshot
    {
        public FreeControllerV3 controller;
        public Vector3 position;
        public Quaternion rotation;
        public FreeControllerV3.PositionState positionState;
        public FreeControllerV3.RotationState rotationState;
    }



    class TargetKneeToSelfThighSnapshot
    {
        public Atom targetAtom;
        public FreeControllerV3 lKnee;
        public FreeControllerV3 rKnee;
        public FreeControllerV3 lFoot;
        public FreeControllerV3 rFoot;
        public Vector3 lKneePosition;
        public Vector3 rKneePosition;
        public Quaternion lKneeRotation;
        public Quaternion rKneeRotation;
        public FreeControllerV3.PositionState lKneePositionState;
        public FreeControllerV3.PositionState rKneePositionState;
        public FreeControllerV3.RotationState lKneeRotationState;
        public FreeControllerV3.RotationState rKneeRotationState;
        public Vector3 lFootPosition;
        public Vector3 rFootPosition;
        public Quaternion lFootRotation;
        public Quaternion rFootRotation;
        public FreeControllerV3.PositionState lFootPositionState;
        public FreeControllerV3.PositionState rFootPositionState;
        public FreeControllerV3.RotationState lFootRotationState;
        public FreeControllerV3.RotationState rFootRotationState;
    }

    class HandCoverTarget
    {
        public string label;
        public Vector3 position;
        public Vector3 outward;
        public bool pushAway;
        // 0 = either hand, -1 = L Hand only, +1 = R Hand only.
        public int requiredHandSide;

        public HandCoverTarget(string label, Vector3 position, Vector3 outward)
            : this(label, position, outward, false, 0)
        {
        }

        public HandCoverTarget(string label, Vector3 position, Vector3 outward, bool pushAway)
            : this(label, position, outward, pushAway, 0)
        {
        }

        public HandCoverTarget(string label, Vector3 position, Vector3 outward, bool pushAway, int requiredHandSide)
        {
            this.label = label;
            this.position = position;
            this.outward = outward;
            this.pushAway = pushAway;
            this.requiredHandSide = requiredHandSide < 0 ? -1 : requiredHandSide > 0 ? 1 : 0;
        }
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
            SuperController.LogMessage("[HumanBodyAction] READY / build=v089 / HBA->HLA delegate diagnostics ON / loadedFile=HumanBodyAction_v089_target_upper_behind_guard.cs");
            DebugMessage("[HumanBodyAction] Ready / build=v089 hla delegate diagnostics / v086 force HLA delegate logs / v085 pushaway back stretch limit / HBA_BridgeVersion");
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
        hbaBridgeVersion = new JSONStorableFloat("HBA_BridgeVersion", 38.0f, 0.0f, 999.0f, true);
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
        hbaInsideActiveAction = CreateHiddenActionChooser("Inside Active Action", hbaEventActionChoices, HbaActionCoverRandomHand);
        hbaInsideHoldAction = CreateHiddenActionChooser("Inside Hold Action", hbaInsideVariantActionChoices, HbaActionTwitchSlow);
        hbaInsideSlowAction = CreateHiddenActionChooser("Inside Slow Action", hbaInsideVariantActionChoices, HbaActionTwitchSlow);
        hbaInsideIntenseAction = CreateHiddenActionChooser("Inside Fast Action", hbaInsideVariantActionChoices, HbaActionTwitchStrong);
        hbaEventDeepAction = CreateHiddenActionChooser("Deep Action", hbaEventActionChoices, HbaActionCoverRandomKneeToThigh);
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

        faceTimeScale = new JSONStorableFloat("Face Time Scale", FaceTimeScaleDefault, FaceTimeScaleMin, FaceTimeScaleMax);
        RegisterFloat(faceTimeScale);

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

        handCoverScope = new JSONStorableStringChooser(
            "Cover Scope",
            new List<string>() { HandCoverScopeAll, HandCoverScopeSelf, HandCoverScopeTarget },
            HandCoverScopeDefault,
            "Cover Scope"
        );
        RegisterStringChooser(handCoverScope);

        handCoverChance = new JSONStorableFloat("Cover Random Chance %", HandCoverChanceDefault, HandCoverChanceMin, HandCoverChanceMax);
        RegisterFloat(handCoverChance);

        randomKneeToThighChance = new JSONStorableFloat("Random Knee To Thigh Chance %", RandomKneeToThighChanceDefault, RandomKneeToThighChanceMin, RandomKneeToThighChanceMax);
        RegisterFloat(randomKneeToThighChance);

        handCoverPushAwayMix = new JSONStorableFloat("Cover PushAway Mix %", HandCoverPushAwayMixDefault, HandCoverPushAwayMixMin, HandCoverPushAwayMixMax);
        RegisterFloat(handCoverPushAwayMix);

        RefreshHandCoverPushAwayPersonChoices(false);
        handCoverPushAwayPerson = new JSONStorableStringChooser(
            "PushAway Person",
            new List<string>(handCoverPushAwayPersonChoices),
            GetDefaultHandCoverPushAwayPersonChoice(),
            "PushAway Person"
        );
        RegisterStringChooser(handCoverPushAwayPerson);

        handCoverPushAwayReach = new JSONStorableFloat("Cover PushAway Reach", HandCoverPushAwayReachDefault, HandCoverPushAwayReachMin, HandCoverPushAwayReachMax);
        RegisterFloat(handCoverPushAwayReach);

        handCoverPushAwayOffset = new JSONStorableFloat("Cover PushAway Offset", HandCoverPushAwayOffsetDefault, HandCoverPushAwayOffsetMin, HandCoverPushAwayOffsetMax);
        RegisterFloat(handCoverPushAwayOffset);

        handCoverComplyHold = new JSONStorableBool("Hand Cover Comply Hold", true);
        RegisterBool(handCoverComplyHold);

        handCoverAutoRestoreOnProgressZero = new JSONStorableBool("Cover Auto Restore On Progress Zero", true);
        RegisterBool(handCoverAutoRestoreOnProgressZero);

        logCoverHand = new JSONStorableBool("Log Cover Hand", false);
        RegisterBool(logCoverHand);

        gParallelChestHeadAmount = new JSONStorableFloat("G Parallel Chest/Head Amount", GParallelChestHeadAmountDefault, GParallelChestHeadAmountMin, GParallelChestHeadAmountMax);
        RegisterFloat(gParallelChestHeadAmount);

        gProgressChestHeadFollow = new JSONStorableBool("G Progress Chest Follow", true);
        RegisterBool(gProgressChestHeadFollow);

        gProgressChestHeadAmount = new JSONStorableFloat("G Progress Follow Amount", GProgressChestHeadAmountDefault, GProgressChestHeadAmountMin, GProgressChestHeadAmountMax);
        RegisterFloat(gProgressChestHeadAmount);

        gProgressChestHeadMaxOffset = new JSONStorableFloat("G Progress Follow Max", GProgressChestHeadMaxOffsetDefault, GProgressChestHeadMaxOffsetMin, GProgressChestHeadMaxOffsetMax);
        RegisterFloat(gProgressChestHeadMaxOffset);

        gProgressChestHeadReturnSpeed = new JSONStorableFloat("G Progress Spring", GProgressChestHeadReturnDefault, GProgressChestHeadReturnMin, GProgressChestHeadReturnMax);
        RegisterFloat(gProgressChestHeadReturnSpeed);

        gProgressChestHeadSmooth = new JSONStorableFloat("G Progress Damping", GProgressChestHeadSmoothDefault, GProgressChestHeadSmoothMin, GProgressChestHeadSmoothMax);
        RegisterFloat(gProgressChestHeadSmooth);

        gProgressChestHeadDelay = new JSONStorableFloat("G Progress Follow Delay", GProgressChestHeadDelayDefault, GProgressChestHeadDelayMin, GProgressChestHeadDelayMax);
        RegisterFloat(gProgressChestHeadDelay);

        gProgressChestHeadUpMix = new JSONStorableFloat("G Progress Up Mix", GProgressChestHeadUpMixDefault, GProgressChestHeadUpMixMin, GProgressChestHeadUpMixMax);
        RegisterFloat(gProgressChestHeadUpMix);

        gProgressChestHeadSideMix = new JSONStorableFloat("G Progress Side Mix", GProgressChestHeadSideMixDefault, GProgressChestHeadSideMixMin, GProgressChestHeadSideMixMax);
        RegisterFloat(gProgressChestHeadSideMix);

        gProgressChestHeadSCurve = new JSONStorableFloat("G Progress S Curve", GProgressChestHeadSCurveDefault, GProgressChestHeadSCurveMin, GProgressChestHeadSCurveMax);
        RegisterFloat(gProgressChestHeadSCurve);

        gProgressChestHeadTopWobble = new JSONStorableFloat("G Progress Top Wobble", GProgressChestHeadTopWobbleDefault, GProgressChestHeadTopWobbleMin, GProgressChestHeadTopWobbleMax);
        RegisterFloat(gProgressChestHeadTopWobble);

        gProgressChestHeadMissChance = new JSONStorableFloat("G Progress Miss Chance %", GProgressChestHeadMissChanceDefault, GProgressChestHeadMissChanceMin, GProgressChestHeadMissChanceMax);
        RegisterFloat(gProgressChestHeadMissChance);

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
        CreateSlider(faceTimeScale, false);
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
        CreateButton("HBA_Cover_RandomHand", false).button.onClick.AddListener(delegate { RequestRandomHandCover("button:HBA_Cover_RandomHand"); });
        CreateButton("HBA HLA Self Head", false).button.onClick.AddListener(delegate { RequestHumanLifeActionCover("HLA_Cover_SelfHead", "button:" + HbaActionHlaSelfHead); });
        CreateButton("HBA HLA Self Hip", false).button.onClick.AddListener(delegate { RequestHumanLifeActionCover("HLA_Cover_SelfHip", "button:" + HbaActionHlaSelfHip); });
        CreateButton("HBA_Cover_RandomHand_PushAway", false).button.onClick.AddListener(delegate { RequestRandomHandCoverPushAway("button:HBA_Cover_RandomHand_PushAway"); });
        CreateButton("HBA_G_Parallel_ChestHead", false).button.onClick.AddListener(delegate { RequestGParallelChestHead("button:HBA_G_Parallel_ChestHead"); });
        CreateButton("HBA_Bonus_KneeNudge", false).button.onClick.AddListener(delegate { RequestHandBonusKneeNudge("button:HBA_Bonus_KneeNudge"); });
        CreateButton("HBA_Test_KneeNudge", false).button.onClick.AddListener(delegate { RequestHandFallbackKneeNudgeTest("button:HBA_Test_KneeNudge"); });
        CreateButton("HBA_Test_ElbowNudge", false).button.onClick.AddListener(delegate { RequestHandBonusElbowNudgeTest("button:HBA_Test_ElbowNudge"); });
        CreateScrollablePopup(handCoverScope, false);
        CreateSlider(handCoverChance, false);
        CreateToggle(handCoverComplyHold, false);
        CreateToggle(handCoverAutoRestoreOnProgressZero, false);
        CreateToggle(logCoverHand, false);
        CreateSlider(gParallelChestHeadAmount, false);
        CreateToggle(gProgressChestHeadFollow, false);
        CreateSlider(gProgressChestHeadAmount, false);
        CreateSlider(gProgressChestHeadMaxOffset, false);
        CreateSlider(gProgressChestHeadReturnSpeed, false);
        CreateSlider(gProgressChestHeadSmooth, false);
        CreateSlider(gProgressChestHeadDelay, false);
        CreateSlider(gProgressChestHeadUpMix, false);
        CreateSlider(gProgressChestHeadSideMix, false);
        CreateSlider(gProgressChestHeadSCurve, false);
        CreateSlider(gProgressChestHeadTopWobble, false);
        CreateSlider(gProgressChestHeadMissChance, false);
        CreateSlider(randomKneeToThighChance, false);
        CreateSlider(handCoverPushAwayMix, false);
        CreateScrollablePopup(handCoverPushAwayPerson, false);
        CreateButton("Refresh PushAway Persons", false).button.onClick.AddListener(delegate { RefreshHandCoverPushAwayPersonChoices(true); });
        CreateSlider(handCoverPushAwayReach, false);
        CreateSlider(handCoverPushAwayOffset, false);
        CreateButton("HBA_Cover_Restore", false).button.onClick.AddListener(delegate { RequestHandCoverRestore("button:HBA_Cover_Restore", true); RestoreTargetKneeToSelfThighSnapshot("button:HBA_Cover_Restore"); UpdateHbaStatus(true); });
        CreateButton("HBA_Cover_RandomKneeToThigh", false).button.onClick.AddListener(delegate { RequestRandomKneeToThigh("button:HBA_Cover_RandomKneeToThigh"); });
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
        RegisterAction(new JSONStorableAction(HbaActionCoverRandomHand, delegate { RequestRandomHandCover("action:" + HbaActionCoverRandomHand); }));
        RegisterAction(new JSONStorableAction(HbrActionCoverRandomHand, delegate { RequestRandomHandCover("action:" + HbrActionCoverRandomHand); }));
        RegisterAction(new JSONStorableAction(HbaActionHlaSelfHead, delegate { RequestHumanLifeActionCover("HLA_Cover_SelfHead", "action:" + HbaActionHlaSelfHead); }));
        RegisterAction(new JSONStorableAction(HbaActionHlaSelfHip, delegate { RequestHumanLifeActionCover("HLA_Cover_SelfHip", "action:" + HbaActionHlaSelfHip); }));
        RegisterAction(new JSONStorableAction(HbaActionCoverRandomHandPushAway, delegate { RequestRandomHandCoverPushAway("action:" + HbaActionCoverRandomHandPushAway); }));
        RegisterAction(new JSONStorableAction(HbrActionCoverRandomHandPushAway, delegate { RequestRandomHandCoverPushAway("action:" + HbrActionCoverRandomHandPushAway); }));
        RegisterAction(new JSONStorableAction(HbaActionGParallelChestHead, delegate { RequestGParallelChestHead("action:" + HbaActionGParallelChestHead); }));
        RegisterAction(new JSONStorableAction(HbrActionGParallelChestHead, delegate { RequestGParallelChestHead("action:" + HbrActionGParallelChestHead); }));
        RegisterAction(new JSONStorableAction(HbaActionBonusKneeNudge, delegate { RequestHandBonusKneeNudge("action:" + HbaActionBonusKneeNudge); }));
        RegisterAction(new JSONStorableAction(HbrActionBonusKneeNudge, delegate { RequestHandBonusKneeNudge("action:" + HbrActionBonusKneeNudge); }));
        RegisterAction(new JSONStorableAction(HbaActionTestKneeNudge, delegate { RequestHandFallbackKneeNudgeTest("action:" + HbaActionTestKneeNudge); }));
        RegisterAction(new JSONStorableAction(HbrActionTestKneeNudge, delegate { RequestHandFallbackKneeNudgeTest("action:" + HbrActionTestKneeNudge); }));
        RegisterAction(new JSONStorableAction(HbaActionTestElbowNudge, delegate { RequestHandBonusElbowNudgeTest("action:" + HbaActionTestElbowNudge); }));
        RegisterAction(new JSONStorableAction(HbrActionTestElbowNudge, delegate { RequestHandBonusElbowNudgeTest("action:" + HbrActionTestElbowNudge); }));
        RegisterAction(new JSONStorableAction(HbaActionCoverRestore, delegate { RequestHandCoverRestore("action:" + HbaActionCoverRestore, true); RestoreTargetKneeToSelfThighSnapshot("action:" + HbaActionCoverRestore); UpdateHbaStatus(true); }));
        RegisterAction(new JSONStorableAction(HbrActionCoverRestore, delegate { RequestHandCoverRestore("action:" + HbrActionCoverRestore, true); RestoreTargetKneeToSelfThighSnapshot("action:" + HbrActionCoverRestore); UpdateHbaStatus(true); }));
        RegisterAction(new JSONStorableAction(HbaActionCoverRandomKneeToThigh, delegate { RequestRandomKneeToThigh("action:" + HbaActionCoverRandomKneeToThigh); }));
        RegisterAction(new JSONStorableAction(HbrActionCoverRandomKneeToThigh, delegate { RequestRandomKneeToThigh("action:" + HbrActionCoverRandomKneeToThigh); }));
        // Force variants are intended for explicit manual buttons such as HDU.
        // They use a button: source so ShouldRunRandomKneeToThigh bypasses the chance slider.
        RegisterAction(new JSONStorableAction(HbaActionCoverRandomKneeToThighForce, delegate { RequestRandomKneeToThigh("button:" + HbaActionCoverRandomKneeToThighForce); }));
        RegisterAction(new JSONStorableAction(HbrActionCoverRandomKneeToThighForce, delegate { RequestRandomKneeToThigh("button:" + HbrActionCoverRandomKneeToThighForce); }));
        // Hidden legacy action aliases for old saved routes.
        RegisterAction(new JSONStorableAction(HbaActionCoverTestKneeThighLegacy, delegate { RequestRandomKneeToThigh("legacy-action:" + HbaActionCoverTestKneeThighLegacy); }));
        RegisterAction(new JSONStorableAction(HbrActionCoverTestKneeThighLegacy, delegate { RequestRandomKneeToThigh("legacy-action:" + HbrActionCoverTestKneeThighLegacy); }));

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
        UpdateGProgressChestHeadFollow();
        UpdateHandCoverPauseForTargetGrabberHeldHand();
        UpdateHandCoverAutoRestoreOnProgressZero();
        UpdateHbaStatus(false);
    }

    bool IsTargetGrabberHoldingTargetHand()
    {
        ResolveTargetGrabberHeldHandParamsIfNeeded();
        return tgHeldTargetLHandCached || tgHeldTargetRHandCached;
    }

    bool IsTargetGrabberHoldingTargetHandSide(int side)
    {
        ResolveTargetGrabberHeldHandParamsIfNeeded();
        if (side < 0) return tgHeldTargetLHandCached;
        if (side > 0) return tgHeldTargetRHandCached;
        return tgHeldTargetLHandCached || tgHeldTargetRHandCached;
    }

    bool IsHandBlockedByTargetGrabberHeldTargetHand(FreeControllerV3 hand)
    {
        int side = GetHandSide(hand);
        return side != 0 && IsTargetGrabberHoldingTargetHandSide(side);
    }

    bool IsTwitchHandPartBlockedByTargetGrabber(TwitchPart p)
    {
        if (p == null || string.IsNullOrEmpty(p.label)) return false;
        if (p.label == "L Hand") return IsTargetGrabberHoldingTargetHandSide(-1);
        if (p.label == "R Hand") return IsTargetGrabberHoldingTargetHandSide(1);
        return false;
    }

    bool IsTargetGrabberHeldTargetHandActive()
    {
        return IsTargetGrabberHoldingTargetHand();
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

    void ResolveTargetGrabberHeldHandParamsIfNeeded()
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
            DebugMessage("[HumanBodyAction] TargetGrabber held hand aggregate / L=" + (tgHeldTargetLHandCached ? "1" : "0") +
                " / R=" + (tgHeldTargetRHandCached ? "1" : "0") +
                " / source=" + tgHeldTargetSourceCached);
        }
    }

    void UpdateHandCoverPauseForTargetGrabberHeldHand()
    {
        if (activeHandCoverSnapshot == null || activeHandCoverSnapshot.hand == null) return;
        if (!IsHandBlockedByTargetGrabberHeldTargetHand(activeHandCoverSnapshot.hand)) return;

        hbaLastBlock = "Cover paused: TG held " + GetHandLabel(activeHandCoverSnapshot.hand);
        DebugMessage("[HumanBodyAction] Cover auto restore / reason=tg-held-target-hand-side / hand=" + GetHandLabel(activeHandCoverSnapshot.hand));
        RequestHandCoverRestore("tg-held-target-hand-side", true);
        UpdateHbaStatus(true);
    }

    bool TryBlockHandCoverForTargetGrabberHeldHand(string source)
    {
        // v077: Do not block the entire Hand Cover route when only one target hand is held by TargetGrabber.
        // Selection filters the held L/R side via IsHandFreeForCover(). Only when both sides are held is there
        // no safe hand left to move.
        bool leftHeld = IsTargetGrabberHoldingTargetHandSide(-1);
        bool rightHeld = IsTargetGrabberHoldingTargetHandSide(1);
        if (!(leftHeld && rightHeld)) return false;

        hbaLastBlock = "Cover skipped: TG held both target hands";
        DebugMessage("[HumanBodyAction] Cover skipped / source=" + source + " / reason=tg-held-both-target-hands");

        if (activeHandCoverSnapshot != null)
            RequestHandCoverRestore("tg-held-both-target-hands", true);

        UpdateHbaStatus(true);
        return true;
    }

    bool IsHandCoverAutoRestoreOnProgressZeroEnabled()
    {
        return handCoverAutoRestoreOnProgressZero == null || handCoverAutoRestoreOnProgressZero.val;
    }

    bool HasHandCoverAutoRestoreSignal()
    {
        if (!IsHbaEnabled()) return false;
        if (hbaActive == null || !hbaActive.val) return false;
        float progress = hbaProgress != null ? hbaProgress.val : 0.0f;
        return progress > HandCoverAutoRestoreProgressMin;
    }

    void UpdateHandCoverAutoRestoreOnProgressZero()
    {
        if (activeHandCoverSnapshot == null)
        {
            handCoverAutoRestoreArmed = false;
            return;
        }

        if (!IsHandCoverAutoRestoreOnProgressZeroEnabled())
        {
            return;
        }

        if (HasHandCoverAutoRestoreSignal())
        {
            if (!handCoverAutoRestoreArmed)
            {
                handCoverAutoRestoreArmed = true;
                DebugMessage("[HumanBodyAction] Cover auto restore armed / progress=" + F3(hbaProgress != null ? hbaProgress.val : 0.0f));
            }
            return;
        }

        if (!handCoverAutoRestoreArmed)
        {
            return;
        }

        string reason = (hbaActive != null && !hbaActive.val) ? "progress-zero:hba-inactive" : "progress-zero";
        hbaLastBlock = "Cover auto restore: " + reason;
        DebugMessage("[HumanBodyAction] Cover auto restore fire / reason=" + reason +
            " / progress=" + F3(hbaProgress != null ? hbaProgress.val : 0.0f) +
            " / active=" + ((hbaActive != null && hbaActive.val) ? "1" : "0"));
        RequestHandCoverRestore(reason, true);
        UpdateHbaStatus(true);
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
        if (eventName == "Deep") return GetChooserValue(hbaEventDeepAction, HbaActionCoverRandomKneeToThigh);
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
        if (actionName == HbaActionCoverRandomHand || actionName == HbrActionCoverRandomHand)
        {
            RequestRandomHandCover(source + ":" + actionName);
            return true;
        }
        if (actionName == HbaActionCoverRandomHandPushAway || actionName == HbrActionCoverRandomHandPushAway)
        {
            RequestRandomHandCoverPushAway(source + ":" + actionName);
            return true;
        }
        if (actionName == HbaActionGParallelChestHead || actionName == HbrActionGParallelChestHead)
        {
            RequestGParallelChestHead(source + ":" + actionName);
            return true;
        }
        if (actionName == HbaActionBonusKneeNudge || actionName == HbrActionBonusKneeNudge)
        {
            RequestHandBonusKneeNudge(source + ":" + actionName);
            return true;
        }
        if (actionName == HbaActionCoverRestore || actionName == HbrActionCoverRestore)
        {
            RequestHandCoverRestore(source + ":" + actionName, true);
            RestoreTargetKneeToSelfThighSnapshot(source + ":" + actionName);
            UpdateHbaStatus(true);
            return true;
        }
        if (actionName == HbaActionCoverTestKneeThighLegacy || actionName == HbrActionCoverTestKneeThighLegacy)
        {
            DebugMessage("[HumanBodyAction] Legacy knee action alias / source=" + source + " / oldAction=" + actionName + " / mapped=" + HbaActionCoverRandomKneeToThigh);
            RequestRandomKneeToThigh(source + ":legacy:" + actionName);
            return true;
        }
        if (actionName == HbaActionCoverRandomKneeToThigh || actionName == HbrActionCoverRandomKneeToThigh)
        {
            RequestRandomKneeToThigh(source + ":" + actionName);
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
        if (actionName == HbaActionCoverRandomHand || actionName == HbrActionCoverRandomHand) return true;
        if (actionName == HbaActionCoverRandomHandPushAway || actionName == HbrActionCoverRandomHandPushAway) return true;
        if (actionName == HbaActionGParallelChestHead || actionName == HbrActionGParallelChestHead) return true;
        if (actionName == HbaActionBonusKneeNudge || actionName == HbrActionBonusKneeNudge) return true;
        if (actionName == HbaActionCoverRestore || actionName == HbrActionCoverRestore) return true;
        if (actionName == HbaActionCoverTestKneeThighLegacy || actionName == HbrActionCoverTestKneeThighLegacy) return true;
        if (actionName == HbaActionCoverRandomKneeToThigh || actionName == HbrActionCoverRandomKneeToThigh) return true;
        return !IsOff(GetHeadPresetFromHbaAction(actionName));
    }

    string GetTgAtomUidForEvent(string eventName)
    {
        JSONStorableStringChooser chooser = GetTgAtomChooser(eventName);
        if (chooser != null && !string.IsNullOrEmpty(chooser.val))
        {
            string v = chooser.val;
            // If a saved scene keeps an old HBA/HBR action string, route it only when it is still known-safe.
            // Unknown HBA/HBR strings should not be treated as Atom UIDs because that silently kills TG output.
            if ((v.StartsWith("HBA_", StringComparison.OrdinalIgnoreCase) || v.StartsWith("HBR_", StringComparison.OrdinalIgnoreCase)) && !IsSafeExternalHbaAction(v))
            {
                DebugMessage("[HumanBodyAction] TG/HBA stale action fallback / event=" + eventName + " / value=" + v + " / fallback=" + GetDefaultTgAtomForEvent(eventName));
                string fallback = GetDefaultTgAtomForEvent(eventName);
                if (!string.IsNullOrEmpty(fallback)) return fallback;
            }
            return v;
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
        if (!hbaTgAtomChoices.Contains(HbrActionCoverRandomHand))
        {
            hbaTgAtomChoices.Add(HbrActionCoverRandomHand);
            hbaCount++;
        }
        if (!hbaTgAtomChoices.Contains(HbrActionCoverRestore))
        {
            hbaTgAtomChoices.Add(HbrActionCoverRestore);
            hbaCount++;
        }
        if (!hbaTgAtomChoices.Contains(HbrActionCoverRandomKneeToThigh))
        {
            hbaTgAtomChoices.Add(HbrActionCoverRandomKneeToThigh);
            hbaCount++;
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
            runHandCover = false,
            headPreset = headPreset
        });
    }



    bool HasGProgressChestFollowSignal()
    {
        if (hbaActive == null || !hbaActive.val) return false;
        float progress = hbaProgress != null ? hbaProgress.val : 0.0f;
        return progress > GProgressChestHeadActiveProgressMin;
    }

    void UpdateGProgressChestHeadFollow()
    {
        if (gProgressChestHeadFollow == null) return;

        bool shouldRun = gProgressChestHeadFollow.val && IsHbaEnabled() && gParallelChestHeadRoutine == null;
        if (!shouldRun)
        {
            if (gProgressChestHeadActive)
            {
                StopGProgressChestHeadFollow(gProgressChestHeadFollow.val ? "suspend" : "toggle-off");
            }
            return;
        }

        bool signalActive = HasGProgressChestFollowSignal();
        if (!signalActive)
        {
            if (gProgressChestHeadActive || activeGProgressChestSnapshot != null || activeGProgressHeadSnapshot != null)
            {
                StopGProgressChestHeadFollow("progress-zero");
            }
            return;
        }

        if (!gProgressChestHeadActive)
        {
            StartGProgressChestHeadFollow("progress-active");
            return;
        }

        FreeControllerV3 chest = activeGProgressChestSnapshot != null ? activeGProgressChestSnapshot.controller : null;
        if (chest == null)
        {
            StopGProgressChestHeadFollow("lost-chest-controller");
            return;
        }

        float dt = Mathf.Clamp(Time.deltaTime, 0.001f, 0.050f);
        float progress = hbaProgress != null ? hbaProgress.val : 0.0f;
        float delay = gProgressChestHeadDelay != null ? gProgressChestHeadDelay.val : GProgressChestHeadDelayDefault;
        float delayedProgress = progress;
        if (delay > 0.0001f)
        {
            // Low-pass the incoming progress so the chest follows slightly behind the P motion.
            float delayT = Mathf.Clamp01(dt / Mathf.Max(0.0001f, delay));
            gProgressChestHeadDelayedProgress = Mathf.Lerp(gProgressChestHeadDelayedProgress, progress, delayT);
            delayedProgress = gProgressChestHeadDelayedProgress;
        }
        else
        {
            gProgressChestHeadDelayedProgress = progress;
        }

        float delta = delayedProgress - gProgressChestHeadLastProgress;
        gProgressChestHeadLastProgress = delayedProgress;

        bool active = hbaActive != null && hbaActive.val;
        if (!active) delta = 0.0f;
        delta = Mathf.Clamp(delta, -GProgressChestHeadDeltaClamp, GProgressChestHeadDeltaClamp);

        float amount = gProgressChestHeadAmount != null ? gProgressChestHeadAmount.val : GProgressChestHeadAmountDefault;
        float maxOffset = gProgressChestHeadMaxOffset != null ? gProgressChestHeadMaxOffset.val : GProgressChestHeadMaxOffsetDefault;
        float spring = gProgressChestHeadReturnSpeed != null ? gProgressChestHeadReturnSpeed.val : GProgressChestHeadReturnDefault;
        float damping = gProgressChestHeadSmooth != null ? gProgressChestHeadSmooth.val : GProgressChestHeadSmoothDefault;
        float upMix = gProgressChestHeadUpMix != null ? gProgressChestHeadUpMix.val : GProgressChestHeadUpMixDefault;
        float sideMix = gProgressChestHeadSideMix != null ? gProgressChestHeadSideMix.val : GProgressChestHeadSideMixDefault;
        float sCurve = gProgressChestHeadSCurve != null ? gProgressChestHeadSCurve.val : GProgressChestHeadSCurveDefault;
        float topWobble = gProgressChestHeadTopWobble != null ? gProgressChestHeadTopWobble.val : GProgressChestHeadTopWobbleDefault;
        float missChance = gProgressChestHeadMissChance != null ? gProgressChestHeadMissChance.val : GProgressChestHeadMissChanceDefault;

        Vector3 baseDir = gProgressChestHeadDir.sqrMagnitude > 0.0001f ? -gProgressChestHeadDir.normalized : Vector3.zero; // v058 reversed direction kept.
        Vector3 upDir = Vector3.up;
        Vector3 sideDir = Vector3.zero;
        if (baseDir.sqrMagnitude > 0.0001f)
        {
            sideDir = Vector3.Cross(upDir, baseDir);
            if (sideDir.sqrMagnitude < 0.0001f && containingAtom != null) sideDir = containingAtom.transform.right;
            if (sideDir.sqrMagnitude > 0.0001f) sideDir.Normalize();
        }

        Vector3 impulseDir = baseDir;
        if (impulseDir.sqrMagnitude > 0.0001f)
        {
            impulseDir = impulseDir + upDir * upMix + sideDir * (sideMix * gProgressChestHeadSideSign);
            if (impulseDir.sqrMagnitude > 0.0001f) impulseDir.Normalize();
        }

        bool missActive = Time.time < gProgressChestHeadMissUntil;
        if (!missActive && Mathf.Abs(delta) > GProgressChestHeadMissDeltaMin && missChance > 0.001f)
        {
            if (UnityEngine.Random.value < Mathf.Clamp01(missChance * 0.01f))
            {
                gProgressChestHeadMissUntil = Time.time + UnityEngine.Random.Range(GProgressChestHeadMissSecondsMin, GProgressChestHeadMissSecondsMax);
                missActive = true;
                if (debugLog != null && debugLog.val && Time.time - gProgressChestHeadLastMissLogTime > 0.50f)
                {
                    gProgressChestHeadLastMissLogTime = Time.time;
                    DebugMessage("[HumanBodyAction] G Progress Follow miss / chance=" + F1(missChance) + " / delta=" + F3(delta));
                }
            }
        }

        if (!missActive && Mathf.Abs(delta) > 0.00001f && impulseDir.sqrMagnitude > 0.0001f)
        {
            // v060+: progress changes add velocity instead of directly teleporting a target offset.
            // v061: sometimes skip a short impulse window, so it does not follow every beat too perfectly.
            gProgressChestHeadVelocity += impulseDir * (delta * amount * Mathf.Max(0.0f, spring));
        }

        if (spring > 0.0001f)
        {
            gProgressChestHeadVelocity += (-gProgressChestHeadCurrentOffset * spring) * dt;
        }

        if (damping > 0.0001f)
        {
            float dampingT = Mathf.Exp(-damping * dt);
            gProgressChestHeadVelocity *= dampingT;
        }

        gProgressChestHeadCurrentOffset += gProgressChestHeadVelocity * dt;
        gProgressChestHeadTargetOffset = gProgressChestHeadCurrentOffset;

        // v061: soft limit instead of a hard stop. It can hover near the top and wobble slightly,
        // but still prevents runaway offsets.
        if (maxOffset > 0.0f)
        {
            float mag = gProgressChestHeadCurrentOffset.magnitude;
            float softMax = maxOffset;
            float hardMax = maxOffset * GProgressChestHeadSoftLimitExtraRatio;
            if (mag > softMax && mag > 0.0001f)
            {
                Vector3 outward = gProgressChestHeadCurrentOffset / mag;
                float over = mag - softMax;
                gProgressChestHeadVelocity += (-outward * over * Mathf.Max(0.0f, spring) * 1.35f) * dt;
                float outwardVelocity = Vector3.Dot(gProgressChestHeadVelocity, outward);
                if (outwardVelocity > 0.0f)
                {
                    gProgressChestHeadVelocity -= outward * (outwardVelocity * 0.55f);
                }
            }
            if (hardMax > 0.0f && mag > hardMax)
            {
                Vector3 outward = gProgressChestHeadCurrentOffset / mag;
                gProgressChestHeadCurrentOffset = outward * hardMax;
                gProgressChestHeadTargetOffset = gProgressChestHeadCurrentOffset;
            }
        }

        float currentMag = gProgressChestHeadCurrentOffset.magnitude;
        float ratio = maxOffset > 0.0001f ? Mathf.Clamp01(currentMag / maxOffset) : 0.0f;
        gProgressChestHeadShapePhase += dt * (4.2f + ratio * 3.6f + Mathf.Abs(delta) * 18.0f);

        Vector3 shapedOffset = gProgressChestHeadCurrentOffset;
        if (sideDir.sqrMagnitude > 0.0001f && sCurve > 0.0001f)
        {
            // S-shaped path: side sway changes phase as the chest moves along the main offset.
            float sT = Mathf.Clamp01(ratio);
            float sWave = Mathf.Sin((sT * Mathf.PI * 2.0f) + gProgressChestHeadShapePhase) * sCurve * sT;
            shapedOffset += sideDir * (sWave * gProgressChestHeadSideSign);
        }
        if ((sideDir.sqrMagnitude > 0.0001f || upDir.sqrMagnitude > 0.0001f) && topWobble > 0.0001f)
        {
            float topT = Mathf.InverseLerp(GProgressChestHeadTopWobbleStartRatio, 1.0f, ratio);
            if (topT > 0.0001f)
            {
                float wobbleA = Mathf.Sin(gProgressChestHeadShapePhase * 2.1f) * topWobble * topT;
                float wobbleB = Mathf.Sin(gProgressChestHeadShapePhase * 1.37f + 1.2f) * topWobble * 0.55f * topT;
                shapedOffset += sideDir * (wobbleA * gProgressChestHeadSideSign) + upDir * wobbleB;
            }
        }

        try { chest.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        SetControllerPosition(chest, gProgressChestStart + shapedOffset);
    }

    void StartGProgressChestHeadFollow(string reason)
    {
        FreeControllerV3 chest = FindControllerByAliases("chestControl", "chest");
        if (chest == null)
        {
            hbaLastBlock = "G Progress Follow skipped: no chest";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] G Progress Follow skip / reason=no-chest / source=" + reason);
            if (gProgressChestHeadFollow != null) gProgressChestHeadFollow.val = false;
            return;
        }

        Vector3 gDir;
        Vector3 gPos;
        string gAtom;
        string gTransform;
        if (!TryFindGParallelDirection(out gDir, out gPos, out gAtom, out gTransform))
        {
            hbaLastBlock = "G Progress Follow skipped: no G direction";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] G Progress Follow skip / reason=no-g-direction / source=" + reason);
            if (gProgressChestHeadFollow != null) gProgressChestHeadFollow.val = false;
            return;
        }

        activeGProgressChestSnapshot = CaptureControllerPoseSnapshot(chest);
        activeGProgressHeadSnapshot = null; // v057: Progress follow does not touch headControl.
        gProgressChestStart = GetControllerPosition(chest);
        gProgressHeadStart = Vector3.zero;
        gProgressChestHeadDir = gDir.normalized;
        gProgressChestHeadTargetOffset = Vector3.zero;
        gProgressChestHeadCurrentOffset = Vector3.zero;
        gProgressChestHeadVelocity = Vector3.zero;
        gProgressChestHeadLastProgress = hbaProgress != null ? hbaProgress.val : 0.0f;
        gProgressChestHeadDelayedProgress = gProgressChestHeadLastProgress;
        gProgressChestHeadSideSign = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
        gProgressChestHeadShapePhase = UnityEngine.Random.Range(0.0f, 6.28318f);
        gProgressChestHeadMissUntil = -999.0f;
        gProgressChestHeadLastMissLogTime = -999.0f;
        gProgressChestHeadAtom = gAtom;
        gProgressChestHeadTransform = gTransform;
        gProgressChestHeadActive = true;

        try { chest.currentPositionState = FreeControllerV3.PositionState.On; } catch { }

        hbaLastAction = "G Progress Chest Follow";
        hbaLastBlock = "G Progress Chest Follow ON";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] G Progress Follow start / reason=" + reason +
            " / atom=" + gAtom +
            " / transform=" + gTransform +
            " / gPos=" + V3(gPos) +
            " / dir=" + V3(gDir) +
            " / progress=" + F3(gProgressChestHeadLastProgress) +
            " / amount=" + F3(gProgressChestHeadAmount != null ? gProgressChestHeadAmount.val : GProgressChestHeadAmountDefault) +
            " / max=" + F3(gProgressChestHeadMaxOffset != null ? gProgressChestHeadMaxOffset.val : GProgressChestHeadMaxOffsetDefault) +
            " / delay=" + F3(gProgressChestHeadDelay != null ? gProgressChestHeadDelay.val : GProgressChestHeadDelayDefault) +
            " / spring=" + F3(gProgressChestHeadReturnSpeed != null ? gProgressChestHeadReturnSpeed.val : GProgressChestHeadReturnDefault) +
            " / damping=" + F3(gProgressChestHeadSmooth != null ? gProgressChestHeadSmooth.val : GProgressChestHeadSmoothDefault) +
            " / upMix=" + F3(gProgressChestHeadUpMix != null ? gProgressChestHeadUpMix.val : GProgressChestHeadUpMixDefault) +
            " / sideMix=" + F3(gProgressChestHeadSideMix != null ? gProgressChestHeadSideMix.val : GProgressChestHeadSideMixDefault) +
            " / sCurve=" + F3(gProgressChestHeadSCurve != null ? gProgressChestHeadSCurve.val : GProgressChestHeadSCurveDefault) +
            " / topWobble=" + F3(gProgressChestHeadTopWobble != null ? gProgressChestHeadTopWobble.val : GProgressChestHeadTopWobbleDefault) +
            " / missChance=" + F1(gProgressChestHeadMissChance != null ? gProgressChestHeadMissChance.val : GProgressChestHeadMissChanceDefault) +
            " / reversed=1" +
            " / activeProgressMin=" + F3(GProgressChestHeadActiveProgressMin));
    }

    void StopGProgressChestHeadFollow(string reason)
    {
        RestoreControllerPoseSnapshot(activeGProgressChestSnapshot);
        // v057: Progress follow no longer captures/moves headControl.
        if (gProgressChestHeadActive || activeGProgressChestSnapshot != null || activeGProgressHeadSnapshot != null)
        {
            DebugMessage("[HumanBodyAction] G Progress Follow stop / reason=" + reason +
                " / atom=" + gProgressChestHeadAtom +
                " / transform=" + gProgressChestHeadTransform +
                " / offset=" + V3(gProgressChestHeadCurrentOffset));
        }
        activeGProgressChestSnapshot = null;
        activeGProgressHeadSnapshot = null;
        gProgressChestHeadActive = false;
        gProgressChestHeadDir = Vector3.zero;
        gProgressChestHeadTargetOffset = Vector3.zero;
        gProgressChestHeadCurrentOffset = Vector3.zero;
        gProgressChestHeadVelocity = Vector3.zero;
        gProgressChestHeadDelayedProgress = 0.0f;
        gProgressChestHeadSideSign = 1.0f;
        gProgressChestHeadShapePhase = 0.0f;
        gProgressChestHeadMissUntil = -999.0f;
        gProgressChestHeadLastMissLogTime = -999.0f;
        gProgressChestHeadAtom = "";
        gProgressChestHeadTransform = "";
        hbaLastBlock = "";
        UpdateHbaStatus(true);
    }

    void RequestGParallelChestHead(string source)
    {
        if (!IsHbaEnabled())
        {
            hbaLastBlock = "Disabled: G Parallel skipped";
            DebugMessage("[HumanBodyAction] G Parallel skipped because HBA Enable is OFF / source=" + source);
            UpdateHbaStatus(true);
            return;
        }

        FreeControllerV3 chest = FindControllerByAliases("chestControl", "chest");
        FreeControllerV3 head = null; // v057: G Parallel manual shake also does not touch headControl.
        if (chest == null)
        {
            hbaLastAction = "G Parallel Chest";
            hbaLastBlock = "No chest controller";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] G Parallel skip / reason=no-chest / source=" + source);
            return;
        }

        Vector3 gDir;
        Vector3 gPos;
        string gAtom;
        string gTransform;
        if (!TryFindGParallelDirection(out gDir, out gPos, out gAtom, out gTransform))
        {
            hbaLastAction = "G Parallel Chest";
            hbaLastBlock = "G direction not found";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] G Parallel skip / reason=no-g-direction / source=" + source);
            return;
        }

        if (gParallelChestHeadRoutine != null)
        {
            StopCoroutine(gParallelChestHeadRoutine);
            gParallelChestHeadRoutine = null;
            RestoreGParallelChestHeadSnapshots("restart");
        }
        if (gProgressChestHeadActive || activeGProgressChestSnapshot != null || activeGProgressHeadSnapshot != null)
        {
            StopGProgressChestHeadFollow("manual-g-parallel");
        }

        float amount = gParallelChestHeadAmount != null ? gParallelChestHeadAmount.val : GParallelChestHeadAmountDefault;
        amount = Mathf.Clamp(amount, GParallelChestHeadAmountMin, GParallelChestHeadAmountMax);
        gParallelChestHeadRoutine = StartCoroutine(GParallelChestHeadRoutine(source, chest, head, gDir, amount, gAtom, gTransform, gPos));
    }

    IEnumerator GParallelChestHeadRoutine(string source, FreeControllerV3 chest, FreeControllerV3 head, Vector3 gDir, float amount, string gAtom, string gTransform, Vector3 gPos)
    {
        activeGParallelChestSnapshot = CaptureControllerPoseSnapshot(chest);
        activeGParallelHeadSnapshot = null; // v057: G Parallel manual shake does not touch headControl.

        Vector3 chestStart = chest != null ? GetControllerPosition(chest) : Vector3.zero;
        Vector3 headStart = Vector3.zero;
        Vector3 offset = gDir.normalized * amount;
        Vector3 reverseOffset = -offset * GParallelChestHeadReverseScale;
        Vector3 smallForwardOffset = offset * GParallelChestHeadSmallForwardScale;

        Vector3 chestForward = chest != null ? chestStart + offset : Vector3.zero;
        Vector3 headForward = head != null ? headStart + offset : Vector3.zero;
        Vector3 chestReverse = chest != null ? chestStart + reverseOffset : Vector3.zero;
        Vector3 headReverse = head != null ? headStart + reverseOffset : Vector3.zero;
        Vector3 chestSmallForward = chest != null ? chestStart + smallForwardOffset : Vector3.zero;
        Vector3 headSmallForward = head != null ? headStart + smallForwardOffset : Vector3.zero;

        if (chest != null) { try { chest.currentPositionState = FreeControllerV3.PositionState.On; } catch { } }
        hbaLastAction = "G Parallel Chest";
        hbaLastBlock = "G Parallel shake";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] G Parallel shake start / source=" + source +
            " / atom=" + gAtom +
            " / transform=" + gTransform +
            " / gPos=" + V3(gPos) +
            " / dir=" + V3(gDir) +
            " / amount=" + F3(amount) +
            " / reverse=" + F3(amount * GParallelChestHeadReverseScale) +
            " / smallForward=" + F3(amount * GParallelChestHeadSmallForwardScale) +
            " / chest=" + (chest != null ? "1" : "0") +
            " / head=0");

        yield return StartCoroutine(MoveChestHeadPair(chest, chestStart, chestForward, head, headStart, headForward, GParallelChestHeadMoveSeconds));
        yield return StartCoroutine(MoveChestHeadPair(chest, chestForward, chestReverse, head, headForward, headReverse, GParallelChestHeadReverseSeconds));
        yield return StartCoroutine(MoveChestHeadPair(chest, chestReverse, chestSmallForward, head, headReverse, headSmallForward, GParallelChestHeadSmallForwardSeconds));
        yield return StartCoroutine(MoveChestHeadPair(chest, chestSmallForward, chestStart, head, headSmallForward, headStart, GParallelChestHeadReturnSeconds));

        RestoreGParallelChestHeadSnapshots("complete");
        activeGParallelChestSnapshot = null;
        activeGParallelHeadSnapshot = null;
        hbaLastBlock = "";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] G Parallel shake complete / source=" + source);
        gParallelChestHeadRoutine = null;
    }

    IEnumerator MoveChestHeadPair(FreeControllerV3 chest, Vector3 chestFrom, Vector3 chestTo, FreeControllerV3 head, Vector3 headFrom, Vector3 headTo, float seconds)
    {
        float dur = Mathf.Max(0.01f, seconds);
        float start = Time.time;
        while (Time.time - start < dur)
        {
            float t = Mathf.Clamp01((Time.time - start) / dur);
            float e = Smooth01(t);
            if (chest != null) SetControllerPosition(chest, Vector3.Lerp(chestFrom, chestTo, e));
            if (head != null) SetControllerPosition(head, Vector3.Lerp(headFrom, headTo, e));
            yield return null;
        }
        if (chest != null) SetControllerPosition(chest, chestTo);
        if (head != null) SetControllerPosition(head, headTo);
    }

    IEnumerator HoldChestHeadPair(FreeControllerV3 chest, Vector3 chestPosition, FreeControllerV3 head, Vector3 headPosition, float seconds)
    {
        float dur = Mathf.Max(0.0f, seconds);
        float start = Time.time;
        while (Time.time - start < dur)
        {
            if (chest != null) SetControllerPosition(chest, chestPosition);
            if (head != null) SetControllerPosition(head, headPosition);
            yield return null;
        }
    }

    ControllerPoseSnapshot CaptureControllerPoseSnapshot(FreeControllerV3 controller)
    {
        if (controller == null) return null;
        ControllerPoseSnapshot snap = new ControllerPoseSnapshot();
        snap.controller = controller;
        snap.position = GetControllerPosition(controller);
        snap.rotation = GetControllerRotation(controller);
        try { snap.positionState = controller.currentPositionState; } catch { snap.positionState = FreeControllerV3.PositionState.Off; }
        try { snap.rotationState = controller.currentRotationState; } catch { snap.rotationState = FreeControllerV3.RotationState.Off; }
        return snap;
    }

    void RestoreControllerPoseSnapshot(ControllerPoseSnapshot snap)
    {
        if (snap == null || snap.controller == null) return;
        SetControllerPosition(snap.controller, snap.position);
        SetControllerRotation(snap.controller, snap.rotation);
        try { snap.controller.currentPositionState = snap.positionState; } catch { }
        try { snap.controller.currentRotationState = snap.rotationState; } catch { }
    }

    void RestoreGParallelChestHeadSnapshots(string reason)
    {
        RestoreControllerPoseSnapshot(activeGParallelChestSnapshot);
        RestoreControllerPoseSnapshot(activeGParallelHeadSnapshot);
        if (activeGParallelChestSnapshot != null || activeGParallelHeadSnapshot != null)
        {
            DebugMessage("[HumanBodyAction] G Parallel restore / reason=" + reason);
        }
        activeGParallelChestSnapshot = null;
        activeGParallelHeadSnapshot = null;
    }

    bool TryFindGParallelDirection(out Vector3 dir, out Vector3 position, out string atomUid, out string transformName)
    {
        dir = Vector3.zero;
        position = Vector3.zero;
        atomUid = "";
        transformName = "";

        Transform t;
        Atom atom;
        if (TryFindGTransformOnAtom(containingAtom, out t))
        {
            atom = containingAtom;
        }
        else
        {
            atom = FindNearestOtherPersonAtomWithGTransform(out t);
        }

        if (atom == null || t == null) return false;

        // Keep this aligned with TargetLinePerson's practical G line convention: the usable line
        // is normally the opposite of LabiaTrigger.up. This is only used as an axis for a small
        // chest/head parallel nudge, so no TargetLinePerson dependency is required.
        Vector3 raw = -t.up;
        if (raw.sqrMagnitude < 0.0001f) raw = t.forward;
        if (raw.sqrMagnitude < 0.0001f) return false;
        raw.Normalize();

        dir = raw;
        position = t.position;
        atomUid = atom.uid;
        transformName = t.name;
        return true;
    }

    Atom FindNearestOtherPersonAtomWithGTransform(out Transform transform)
    {
        transform = null;
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

            Transform candidate;
            if (!TryFindGTransformOnAtom(atom, out candidate))
                continue;

            float d = (atom.transform.position - selfPos).sqrMagnitude;
            if (d < bestDistanceSqr)
            {
                bestDistanceSqr = d;
                best = atom;
                transform = candidate;
            }
        }

        return best;
    }

    bool TryFindGTransformOnAtom(Atom atom, out Transform found)
    {
        found = null;
        if (atom == null) return false;

        Transform[] transforms = atom.GetComponentsInChildren<Transform>(true);
        if (transforms == null) return false;

        Transform best = null;
        int bestScore = -1;
        for (int i = 0; i < transforms.Length; i++)
        {
            Transform t = transforms[i];
            if (t == null || string.IsNullOrEmpty(t.name)) continue;
            string n = t.name.ToLowerInvariant();

            int score = -1;
            if (n == "labiatrigger") score = 100;
            else if (n.Contains("labiatrigger")) score = 95;
            else if (n.Contains("labia") && !n.Contains("control")) score = 80;
            else if (n.Contains("vagina") && !n.Contains("control")) score = 65;
            else if (n.Contains("genital") && !n.Contains("control")) score = 50;

            if (score > bestScore)
            {
                bestScore = score;
                best = t;
            }
        }

        if (best == null) return false;
        found = best;
        return true;
    }

    void HbaHlaLog(string message)
    {
        SuperController.LogMessage(message);
    }

    bool RequestHumanLifeActionCover(string hlaActionName, string source)
    {
        HbaHlaLog("[HumanBodyAction] HBA->HLA REQUEST / build=v089 / caller=HumanBodyAction / route=button-or-action / hlaAction=" + hlaActionName + " / source=" + source);

        if (TryInvokeHumanLifeAction(hlaActionName, source))
        {
            hbaLastBlock = "HBA->HLA OK: " + hlaActionName;
            UpdateHbaStatus(true);
            CoverSelectionMessage("HBA->HLA OK: " + hlaActionName);
            return true;
        }

        hbaLastBlock = "HBA->HLA missing: " + hlaActionName;
        UpdateHbaStatus(true);
        HbaHlaLog("[HumanBodyAction] HBA->HLA FALLBACK / build=v089 / caller=HumanBodyAction / hlaAction=" + hlaActionName + " / reason=no-HumanLifeAction-action / source=" + source);
        return false;
    }

    bool TryInvokeHumanLifeAction(string actionName, string source)
    {
        if (containingAtom == null || string.IsNullOrEmpty(actionName))
        {
            HbaHlaLog("[HumanBodyAction] HBA->HLA SKIP / build=v089 / caller=HumanBodyAction / hlaAction=" + actionName + " / reason=no-containingAtom-or-empty-action / source=" + source);
            return false;
        }

        List<string> ids = null;
        try { ids = containingAtom.GetStorableIDs(); } catch { ids = null; }
        if (ids == null)
        {
            HbaHlaLog("[HumanBodyAction] HBA->HLA SKIP / build=v089 / caller=HumanBodyAction / hlaAction=" + actionName + " / reason=no-storable-ids / source=" + source);
            return false;
        }

        bool foundHlaStorable = false;

        for (int i = 0; i < ids.Count; i++)
        {
            string sid = ids[i];
            if (string.IsNullOrEmpty(sid)) continue;
            if (sid.IndexOf("HumanLifeAction", StringComparison.OrdinalIgnoreCase) < 0) continue;

            foundHlaStorable = true;

            JSONStorable st = null;
            try { st = containingAtom.GetStorableByID(sid); } catch { st = null; }
            if (st == null) continue;

            JSONStorableAction action = null;
            try { action = st.GetAction(actionName); } catch { action = null; }
            if (action == null)
            {
                HbaHlaLog("[HumanBodyAction] HBA->HLA MISS / build=v089 / caller=HumanBodyAction / hlaStorable=" + sid + " / hlaAction=" + actionName + " / reason=action-not-found / source=" + source);
                continue;
            }

            try
            {
                HbaHlaLog("[HumanBodyAction] HBA->HLA CALL / build=v089 / caller=HumanBodyAction / hlaStorable=" + sid + " / hlaAction=" + actionName + " / source=" + source);
                action.actionCallback.Invoke();
                HbaHlaLog("[HumanBodyAction] HBA->HLA OK / build=v089 / caller=HumanBodyAction / hlaStorable=" + sid + " / hlaAction=" + actionName + " / source=" + source);
                return true;
            }
            catch (Exception e)
            {
                SuperController.LogMessage("[HumanBodyAction] HBA->HLA ERROR / build=v089 / caller=HumanBodyAction / hlaStorable=" + sid + " / hlaAction=" + actionName + " / error=" + e.Message + " / source=" + source);
                return false;
            }
        }

        HbaHlaLog("[HumanBodyAction] HBA->HLA MISS / build=v089 / caller=HumanBodyAction / hlaAction=" + actionName + " / reason=" + (foundHlaStorable ? "action-not-found-on-HLA" : "no-HumanLifeAction-storable") + " / source=" + source);
        return false;
    }

    bool IsHlaDelegateLabelMatch(string label, string displayLabel, string match)
    {
        if (!string.IsNullOrEmpty(label) && label.IndexOf(match, StringComparison.OrdinalIgnoreCase) >= 0) return true;
        if (!string.IsNullOrEmpty(displayLabel) && displayLabel.IndexOf(match, StringComparison.OrdinalIgnoreCase) >= 0) return true;
        return false;
    }

    bool TryGetHumanLifeActionCoverDelegate(HandCoverTarget target, out string hlaActionName, out string reason)
    {
        hlaActionName = "";
        reason = "";
        if (target == null) return false;

        string label = target.label ?? "";
        string displayLabel = "";
        try { displayLabel = GetHandCoverTargetDisplayLabel(target); } catch { displayLabel = ""; }

        // HBA logs show the display label, while the internal raw label can be different.
        // Match both so visible selections such as "Head" and "Hip Back" actually delegate.
        if (IsHlaDelegateLabelMatch(label, displayLabel, "Hip Back") ||
            label.Equals("Self Hip", StringComparison.OrdinalIgnoreCase) ||
            displayLabel.Equals("Self Hip", StringComparison.OrdinalIgnoreCase))
        {
            hlaActionName = "HLA_Cover_SelfHip";
            reason = "self-hip/raw=" + label + "/display=" + displayLabel;
            return true;
        }

        if (label.Equals("Head", StringComparison.OrdinalIgnoreCase) ||
            displayLabel.Equals("Head", StringComparison.OrdinalIgnoreCase) ||
            IsHlaDelegateLabelMatch(label, displayLabel, "Head Side") ||
            label.Equals("Self Head", StringComparison.OrdinalIgnoreCase) ||
            displayLabel.Equals("Self Head", StringComparison.OrdinalIgnoreCase))
        {
            hlaActionName = "HLA_Cover_SelfHead";
            reason = "self-head/raw=" + label + "/display=" + displayLabel;
            return true;
        }

        return false;
    }

    void RequestRandomHandCover(string source)
    {
        if (!ShouldRunRandomHandCover(source))
            return;

        if (TryBlockHandCoverForTargetGrabberHeldHand(source))
            return;

        // RandomHandCover must not use the shared latest-only action queue.
        // Head/Twitch actions can be queued repeatedly while Inside/Deep is active, and they can overwrite
        // a queued CoverRandomHand request before ExecuteAction reaches runHandCover.
        // Run hand cover directly, like the knee random route, so it actually starts when the chance passes.
        if (!IsHbaEnabled())
        {
            hbaLastBlock = "Disabled: cover skipped";
            DebugMessage("[HumanBodyAction] Cover skipped because HBA Enable is OFF / source=" + source);
            UpdateHbaStatus(true);
            return;
        }

        // Cancel any stale RandomHand cover routine, including old queued CoverRandomHand runs.
        // The previous build could show logs like "selected Self Chest" followed by an older "Hip Back"
        // soft-snap/hold because an old cover coroutine was still alive. Bump the serial before starting
        // the new direct routine so old routines exit at their next guard.
        handCoverRunSerial++;

        if (directHandCoverRoutine != null)
        {
            StopCoroutine(directHandCoverRoutine);
            directHandCoverRoutine = null;
        }
        if (handCoverRestoreRoutine != null)
        {
            StopCoroutine(handCoverRestoreRoutine);
            handCoverRestoreRoutine = null;
        }

        directHandCoverRoutine = StartCoroutine(DirectRandomHandCoverRoutine(source));
    }

    void RequestRandomHandCoverPushAway(string source)
    {
        if (!ShouldRunRandomHandCover(source))
            return;

        if (TryBlockHandCoverForTargetGrabberHeldHand(source))
            return;

        if (!IsHbaEnabled())
        {
            hbaLastBlock = "Disabled: push-away cover skipped";
            DebugMessage("[HumanBodyAction] PushAway cover skipped because HBA Enable is OFF / source=" + source);
            UpdateHbaStatus(true);
            return;
        }

        handCoverRunSerial++;

        if (directHandCoverRoutine != null)
        {
            StopCoroutine(directHandCoverRoutine);
            directHandCoverRoutine = null;
        }
        if (handCoverRestoreRoutine != null)
        {
            StopCoroutine(handCoverRestoreRoutine);
            handCoverRestoreRoutine = null;
        }

        directHandCoverRoutine = StartCoroutine(DirectRandomHandCoverPushAwayRoutine(source));
    }

    IEnumerator DirectRandomHandCoverRoutine(string source)
    {
        hbaLastAction = source;
        hbaLastBlock = "CoverRandomHand direct";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] ACTION START DIRECT / source=" + source + " / preset=CoverRandomHand / head=Off");

        yield return StartCoroutine(RandomHandCoverRoutine(source));

        DebugMessage("[HumanBodyAction] ACTION DONE DIRECT / source=" + source + " / preset=CoverRandomHand");
        directHandCoverRoutine = null;
    }

    IEnumerator DirectRandomHandCoverPushAwayRoutine(string source)
    {
        hbaLastAction = source;
        hbaLastBlock = "PushAwayRandomHand direct";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] ACTION START DIRECT / source=" + source + " / preset=CoverRandomHand_PushAway / head=Off");

        yield return StartCoroutine(RandomHandCoverRoutine(source, false, true));

        DebugMessage("[HumanBodyAction] ACTION DONE DIRECT / source=" + source + " / preset=CoverRandomHand_PushAway");
        directHandCoverRoutine = null;
    }

    bool ShouldRunRandomKneeToThigh(string source)
    {
        // Manual button is deterministic for verification. Event/Action/TG/HBR routes obey this probability slider.
        if (!string.IsNullOrEmpty(source) && source.StartsWith("button:"))
            return true;

        float chance = randomKneeToThighChance != null ? randomKneeToThighChance.val : RandomKneeToThighChanceDefault;
        chance = Mathf.Clamp(chance, RandomKneeToThighChanceMin, RandomKneeToThighChanceMax);

        if (chance >= 99.999f)
            return true;

        if (chance <= 0.001f)
        {
            hbaLastBlock = "Random knee chance skipped: 0%";
            DebugMessage("[HumanBodyAction] Random knee chance skipped / source=" + source + " / chance=" + F1(chance) + "%");
            UpdateHbaStatus(true);
            return false;
        }

        float roll = UnityEngine.Random.Range(0.0f, 100.0f);
        if (roll > chance)
        {
            hbaLastBlock = "Random knee chance skipped: " + F1(roll) + " > " + F1(chance) + "%";
            DebugMessage("[HumanBodyAction] Random knee chance skipped / source=" + source + " / roll=" + F1(roll) + " / chance=" + F1(chance) + "%");
            UpdateHbaStatus(true);
            return false;
        }

        DebugMessage("[HumanBodyAction] Random knee chance pass / source=" + source + " / roll=" + F1(roll) + " / chance=" + F1(chance) + "%");
        return true;
    }

    void RequestRandomKneeToThigh(string source)
    {
        // Random knee action is direct like the manual knee test: it must run immediately and not be swallowed by the latest-only body/head queue.
        if (!IsHbaEnabled())
        {
            hbaLastBlock = "Disabled: random knee skipped";
            DebugMessage("[HumanBodyAction] Random knee->thigh skipped because HBA Enable is OFF / source=" + source);
            UpdateHbaStatus(true);
            return;
        }

        if (!ShouldRunRandomKneeToThigh(source))
        {
            return;
        }

        // Keep RandomKnee independent from RandomHandCover.
        // Do not clear pendingRequest, do not increment handCoverRunSerial,
        // and do not stop directHandCoverRoutine here; otherwise a knee action
        // can silently cancel the existing RandomHandCover route/hold.
        targetKneeToSelfThighRunSerial++;

        if (directRandomKneeRoutine != null)
        {
            StopCoroutine(directRandomKneeRoutine);
            directRandomKneeRoutine = null;
        }
        if (gParallelChestHeadRoutine != null)
        {
            StopCoroutine(gParallelChestHeadRoutine);
            gParallelChestHeadRoutine = null;
            RestoreGParallelChestHeadSnapshots("reset:" + source);
        }

        directRandomKneeRoutine = StartCoroutine(DirectRandomKneeToThighRoutine(source));
    }

    IEnumerator DirectRandomKneeToThighRoutine(string source)
    {
        hbaLastAction = source;
        hbaLastBlock = "Random knee reaction direct";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] ACTION START DIRECT / source=" + source + " / preset=RandomKneeReaction / head=Off");

        yield return StartCoroutine(RandomKneeToThighRoutine(source));

        DebugMessage("[HumanBodyAction] ACTION DONE DIRECT / source=" + source + " / preset=RandomKneeReaction");
        directRandomKneeRoutine = null;
    }

    IEnumerator RandomKneeToThighRoutine(string source)
    {
        RefreshControllersNoReset();

        // v091: Keep the public action name for compatibility.
        // Internal distribution after the outer chance gate:
        // 20% single-knee nearest safe Thigh move / 30% pair open / 30% pair close / 20% single-knee free.
        // Foot IK is temporarily Off only for the single Knee->Thigh travel branch.
        // Pair Open/Close and Knee Free intentionally leave Foot IK untouched to avoid whole-body drop.
        RestoreTargetKneeToSelfThighSnapshot("restart-random-knee-reaction");
        int runSerial = ++targetKneeToSelfThighRunSerial;

        Atom moveAtom = containingAtom;
        if (moveAtom == null)
        {
            hbaLastBlock = "Random knee reaction: missing Person";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] Random knee reaction skipped / reason=missing-move-person" +
                " / source=" + source);
            yield break;
        }

        // v094: moving side must be the Person that owns this HumanBodyAction plugin.
        // Resolve these controls on moveAtom explicitly; do not use any target/nearest Person lookup here.
        FreeControllerV3 moveLKnee = FindControllerByAliasesOnAtom(moveAtom, "lKneeControl", "leftKneeControl", "lKnee", "leftKnee");
        FreeControllerV3 moveRKnee = FindControllerByAliasesOnAtom(moveAtom, "rKneeControl", "rightKneeControl", "rKnee", "rightKnee");
        FreeControllerV3 moveLFoot = FindControllerByAliasesOnAtom(moveAtom, "lFootControl", "leftFootControl", "lFoot", "leftFoot");
        FreeControllerV3 moveRFoot = FindControllerByAliasesOnAtom(moveAtom, "rFootControl", "rightFootControl", "rFoot", "rightFoot");

        if (moveLKnee == null && moveRKnee == null)
        {
            hbaLastBlock = "Random knee reaction: missing knees";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] Random knee reaction skipped / reason=missing-knee-ik" +
                " / moveAtom=" + moveAtom.uid +
                " / moveLKnee=" + (moveLKnee != null ? "1" : "0") +
                " / moveRKnee=" + (moveRKnee != null ? "1" : "0") +
                " / source=" + source);
            yield break;
        }

        // v092: Do not capture both knees/feet up front.
        // Each branch captures only the controls it will actually touch.
        // This avoids single-knee actions restoring/forcing the opposite knee or both feet.

        float roll = UnityEngine.Random.Range(0.0f, 100.0f);
        // v044: keep knee reactions to Pair Open / Pair Close only.
        // Single thigh/free/small local knee routes are disabled because they looked too jumpy in motion.
        string mode = roll < 50.0f ? "pair-open" : "pair-close";

        DebugMessage("[HumanBodyAction] Random knee reaction roll" +
            " / source=" + source +
            " / moveAtom=" + moveAtom.uid +
            " / moveOwner=containingAtom" +
            " / moveLKneeOwnerGuard=" + (moveLKnee != null ? "1" : "0") +
            " / moveRKneeOwnerGuard=" + (moveRKnee != null ? "1" : "0") +
            " / roll=" + F2(roll) +
            " / mode=" + mode +
            " / distribution=pair-open50/pair-close50/single-disabled" +
            " / outerChance=" + F1(randomKneeToThighChance != null ? randomKneeToThighChance.val : RandomKneeToThighChanceDefault) + "%");

        if (mode == "single-free")
        {
            hbaLastBlock = "Random knee reaction single-free disabled";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] Random knee reaction single-free disabled" +
                " / source=" + source +
                " / moveAtom=" + moveAtom.uid);
            yield break;
        }

        if (mode == "pair-open" || mode == "pair-close")
        {
            if (moveLKnee == null || moveRKnee == null)
            {
                // Pair reaction needs both knees. Fall back to a single smooth nudge.
                DebugMessage("[HumanBodyAction] Random knee pair fallback" +
                    " / source=" + source +
                    " / reason=missing-one-knee" +
                    " / requested=" + mode +
                    " / lKnee=" + (moveLKnee != null ? "1" : "0") +
                    " / rKnee=" + (moveRKnee != null ? "1" : "0"));
                hbaLastBlock = "Random knee pair skipped: missing one knee";
                UpdateHbaStatus(true);
                DebugMessage("[HumanBodyAction] Random knee pair skipped / single fallback disabled" +
                    " / source=" + source +
                    " / requested=" + mode +
                    " / moveAtom=" + moveAtom.uid);
                yield break;
            }

            yield return StartCoroutine(RandomKneePairOpenCloseRoutine(source, runSerial, moveAtom, moveLKnee, moveRKnee, moveLFoot, moveRFoot, mode == "pair-open"));
            yield break;
        }

        hbaLastBlock = "Random knee single-thigh disabled";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Random knee single-thigh disabled" +
            " / source=" + source +
            " / moveAtom=" + moveAtom.uid +
            " / mode=" + mode);
        yield break;
    }

    bool PickAvailableKneeSide(FreeControllerV3 lKnee, FreeControllerV3 rKnee)
    {
        if (lKnee != null && rKnee != null)
            return UnityEngine.Random.Range(0, 2) == 0;
        if (lKnee != null)
            return true;
        return false;
    }

    bool PickStableKneeSideForReaction(FreeControllerV3 lKnee, FreeControllerV3 rKnee, string reason)
    {
        if (lKnee != null && rKnee != null)
        {
            float lY = GetControllerPosition(lKnee).y;
            float rY = GetControllerPosition(rKnee).y;
            float gap = Mathf.Abs(lY - rY);
            if (gap > RandomKneeReactionPairMaxVerticalGap)
            {
                bool useLeft = lY >= rY;
                DebugMessage("[HumanBodyAction] Random knee stable side selected" +
                    " / reason=" + reason +
                    " / select=" + (useLeft ? "L Knee" : "R Knee") +
                    " / lY=" + F3(lY) +
                    " / rY=" + F3(rY) +
                    " / verticalGap=" + F3(gap) +
                    " / threshold=" + F3(RandomKneeReactionPairMaxVerticalGap) +
                    " / avoidLowerKnee=1");
                return useLeft;
            }
            return UnityEngine.Random.Range(0, 2) == 0;
        }
        if (lKnee != null)
            return true;
        return false;
    }

    IEnumerator RandomKneeSingleThighSmoothRoutine(string source, int runSerial, Atom moveAtom, bool useLeftKnee, FreeControllerV3 lKnee, FreeControllerV3 rKnee, FreeControllerV3 lFoot, FreeControllerV3 rFoot)
    {
        FreeControllerV3 knee = useLeftKnee ? lKnee : rKnee;
        FreeControllerV3 movingFoot = useLeftKnee ? lFoot : rFoot;
        string kneeLabel = useLeftKnee ? "L Knee" : "R Knee";
        string footLabel = useLeftKnee ? "L Foot" : "R Foot";
        if (knee == null)
            yield break;

        // v092: single branch snapshot = moving knee + same-side foot only.
        CaptureTargetKneeToSelfThighSnapshot(moveAtom, useLeftKnee ? lKnee : null, useLeftKnee ? null : rKnee, useLeftKnee ? lFoot : null, useLeftKnee ? null : rFoot);

        Atom goalAtom = FindNearestOtherPersonAtom();
        FreeControllerV3 goalLThigh = goalAtom != null ? FindControllerByAliasesOnAtom(goalAtom, "lThighControl", "leftThighControl", "lThigh", "leftThigh") : null;
        FreeControllerV3 goalRThigh = goalAtom != null ? FindControllerByAliasesOnAtom(goalAtom, "rThighControl", "rightThighControl", "rThigh", "rightThigh") : null;
        if (goalAtom == null || (goalLThigh == null && goalRThigh == null))
        {
            DebugMessage("[HumanBodyAction] Random knee thigh branch fallback" +
                " / source=" + source +
                " / reason=missing-goal-thigh" +
                " / goalAtom=" + (goalAtom != null ? goalAtom.uid : "<none>") +
                " / goalLThigh=" + (goalLThigh != null ? "1" : "0") +
                " / goalRThigh=" + (goalRThigh != null ? "1" : "0") +
                " / fallback=local-small-nudge");
            yield return StartCoroutine(RandomKneeSingleSmallNudgeRoutine(source, runSerial, moveAtom, useLeftKnee, lKnee, rKnee, lFoot, rFoot));
            yield break;
        }

        Vector3 preFreeStartPos = GetControllerPosition(knee);
        Vector3 goalLRaw = goalLThigh != null ? GetControllerPosition(goalLThigh) : Vector3.zero;
        Vector3 goalRRaw = goalRThigh != null ? GetControllerPosition(goalRThigh) : Vector3.zero;
        float distToLThigh = goalLThigh != null ? Vector3.Distance(preFreeStartPos, goalLRaw) : float.MaxValue;
        float distToRThigh = goalRThigh != null ? Vector3.Distance(preFreeStartPos, goalRRaw) : float.MaxValue;
        bool goalLeftThigh = distToLThigh <= distToRThigh;
        FreeControllerV3 goalThigh = goalLeftThigh ? goalLThigh : goalRThigh;
        string thighLabel = goalLeftThigh ? "L Thigh" : "R Thigh";
        bool sameSide = (useLeftKnee && goalLeftThigh) || (!useLeftKnee && !goalLeftThigh);

        // v093: Do not turn Foot IK Off here.
        // Even a same-side temporary Foot Off can let VaM solve the lower body by dropping hip/root.
        // Keep the foot untouched and move the knee target on a no-down guarded path.
        FreeControllerV3.PositionState movingFootPosState = movingFoot != null ? movingFoot.currentPositionState : FreeControllerV3.PositionState.Off;
        FreeControllerV3.RotationState movingFootRotState = movingFoot != null ? movingFoot.currentRotationState : FreeControllerV3.RotationState.Off;

        if (runSerial != targetKneeToSelfThighRunSerial) yield break;

        Vector3 startPos = GetControllerPosition(knee);
        string safeGoalInfo;
        Vector3 rawGoalPos = GetControllerPosition(goalThigh);
        Vector3 safeGoal = BuildRandomKneeSafeThighAnchor(goalAtom, goalThigh, goalLeftThigh, sameSide, startPos, out safeGoalInfo);
        float requestedDistance = Vector3.Distance(startPos, safeGoal);
        Vector3 goalPos = requestedDistance > RandomKneeToThighTooFarDistance
            ? ClampVectorFromStart(startPos, safeGoal, RandomKneeToThighTooFarStretchDistance)
            : safeGoal;
        bool clampedFar = requestedDistance > RandomKneeToThighTooFarDistance;

        Quaternion startRot = GetControllerRotation(knee);
        FreeControllerV3.PositionState startPosState = knee.currentPositionState;
        FreeControllerV3.RotationState startRotState = knee.currentRotationState;

        Vector3 right = GetAtomHorizontalRight(moveAtom);
        Vector3 arc = right * UnityEngine.Random.Range(-RandomKneeReactionArcSideMax, RandomKneeReactionArcSideMax) +
            Vector3.up * UnityEngine.Random.Range(RandomKneeReactionArcUpMin, RandomKneeReactionArcUpMax);
        float moveDur = UnityEngine.Random.Range(RandomKneeReactionMoveSecondsMin, RandomKneeReactionMoveSecondsMax);
        float holdDur = UnityEngine.Random.Range(0.10f, 0.24f);

        try { knee.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { knee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        hbaLastBlock = "Random knee -> thigh smooth: " + kneeLabel + " -> " + thighLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Random knee->thigh smooth start" +
            " / source=" + source +
            " / moveAtom=" + (moveAtom != null ? moveAtom.uid : "<none>") +
            " / goalAtom=" + (goalAtom != null ? goalAtom.uid : "<none>") +
            " / moveOwner=containingAtom" +
            " / knee=" + kneeLabel +
            " / foot=" + footLabel +
            " / footTempOff=0" +
            " / footUntouched=1" +
            " / noDropYGuard=1" +
            " / thigh=" + thighLabel +
            " / select=nearest-thigh" +
            " / distL=" + F3(distToLThigh) +
            " / distR=" + F3(distToRThigh) +
            " / start=" + V3(startPos) +
            " / rawGoal=" + V3(rawGoalPos) +
            " / safeGoal=" + V3(safeGoal) +
            " / goal=" + V3(goalPos) +
            " / requestedDistance=" + F3(requestedDistance) +
            " / clampedFar=" + (clampedFar ? "1" : "0") +
            " / moveSeconds=" + F3(moveDur) +
            " / holdSeconds=" + F3(holdDur) +
            " / safe=" + safeGoalInfo +
            " / return=HBA_Cover_Restore_smooth");

        float startTime = Time.time;
        while (Time.time - startTime < moveDur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - startTime) / moveDur);
            float e = Smooth01(Smooth01(t));
            SetControllerPosition(knee, Vector3.Lerp(startPos, goalPos, e) + arc * Mathf.Sin(Mathf.PI * e));
            yield return null;
        }

        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        SetControllerPosition(knee, goalPos);
        if (holdDur > 0.0f)
            yield return new WaitForSeconds(holdDur);
        if (runSerial != targetKneeToSelfThighRunSerial) yield break;

        // v093: Foot was never touched in this route. Keep it untouched.
        // Keep the knee at the safe thigh anchor until Cover Restore/Reset/restart.
        // The snapshot restore now animates explicit Cover Restore so the return does not snap mechanically.
        SetControllerPosition(knee, goalPos);
        try { knee.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { knee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        hbaLastBlock = "Random knee holding thigh: " + kneeLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Random knee->thigh smooth hold" +
            " / source=" + source +
            " / knee=" + kneeLabel +
            " / thigh=" + thighLabel +
            " / hold=" + V3(goalPos) +
            " / footUntouched=1" +
            " / footState=untouched:" + (movingFoot != null ? movingFootPosState.ToString() : "none") +
            " / kneeStartState=" + startPosState.ToString() +
            " / kneeStartRotState=" + startRotState.ToString() +
            " / startRot=" + startRot.ToString() +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");
    }

    IEnumerator RandomKneeSingleSmallNudgeRoutine(string source, int runSerial, Atom moveAtom, bool useLeftKnee, FreeControllerV3 lKnee, FreeControllerV3 rKnee, FreeControllerV3 lFoot, FreeControllerV3 rFoot)
    {
        FreeControllerV3 knee = useLeftKnee ? lKnee : rKnee;
        string kneeLabel = useLeftKnee ? "L Knee" : "R Knee";
        if (knee == null)
            yield break;

        // v092: local fallback small nudge only touches the selected knee; no foot snapshot/off.
        CaptureTargetKneeToSelfThighSnapshot(moveAtom, useLeftKnee ? lKnee : null, useLeftKnee ? null : rKnee, null, null);

        // v091: Local small nudge is not the thigh travel branch, so do not relax Foot IK here.

        Vector3 startPos = GetControllerPosition(knee);
        Quaternion startRot = GetControllerRotation(knee);
        FreeControllerV3.PositionState startPosState = knee.currentPositionState;
        FreeControllerV3.RotationState startRotState = knee.currentRotationState;

        Vector3 right = GetAtomHorizontalRight(moveAtom);
        Vector3 forward = GetAtomHorizontalForward(moveAtom);
        Vector3 outward = useLeftKnee ? -right : right;
        if (outward.sqrMagnitude < 0.0001f) outward = useLeftKnee ? Vector3.left : Vector3.right;
        outward.Normalize();

        float variantRoll = UnityEngine.Random.Range(0.0f, 100.0f);
        string guide;
        Vector3 dir;
        if (variantRoll < 45.0f)
        {
            guide = "local-out-up";
            dir = outward * 0.85f + Vector3.up * 0.45f + forward * UnityEngine.Random.Range(-0.10f, 0.12f);
        }
        else if (variantRoll < 75.0f)
        {
            guide = "local-out-forward";
            dir = outward * 0.80f + forward * 0.38f + Vector3.up * 0.22f;
        }
        else if (variantRoll < 90.0f)
        {
            guide = "local-up";
            dir = Vector3.up * 0.90f + outward * 0.22f + forward * UnityEngine.Random.Range(-0.10f, 0.10f);
        }
        else
        {
            guide = "local-relax-back";
            dir = -outward * 0.25f - forward * 0.25f + Vector3.up * 0.18f;
        }

        if (dir.sqrMagnitude < 0.0001f) dir = outward;
        dir.Normalize();

        float amount = UnityEngine.Random.Range(RandomKneeReactionSingleAmountMin, RandomKneeReactionSingleAmountMax);
        Vector3 peak = startPos + dir * amount;
        Vector3 settle = Vector3.Lerp(peak, startPos, UnityEngine.Random.Range(RandomKneeReactionReturnRatioMin, RandomKneeReactionReturnRatioMax));
        Vector3 arc = right * UnityEngine.Random.Range(-RandomKneeReactionArcSideMax, RandomKneeReactionArcSideMax) +
            Vector3.up * UnityEngine.Random.Range(RandomKneeReactionArcUpMin, RandomKneeReactionArcUpMax);

        float moveDur = UnityEngine.Random.Range(RandomKneeReactionMoveSecondsMin, RandomKneeReactionMoveSecondsMax);
        float holdDur = UnityEngine.Random.Range(RandomKneeReactionHoldSecondsMin, RandomKneeReactionHoldSecondsMax);
        float settleDur = UnityEngine.Random.Range(RandomKneeReactionSettleSecondsMin, RandomKneeReactionSettleSecondsMax);
        float returnDur = UnityEngine.Random.Range(RandomKneeReactionReturnSecondsMin, RandomKneeReactionReturnSecondsMax);

        try { knee.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { knee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        hbaLastBlock = "Random knee small nudge: " + kneeLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Random knee small nudge start" +
            " / source=" + source +
            " / moveAtom=" + (moveAtom != null ? moveAtom.uid : "<none>") +
            " / knee=" + kneeLabel +
            " / footTempOff=0" +
            " / footUntouched=1" +
            " / guide=" + guide +
            " / start=" + V3(startPos) +
            " / peak=" + V3(peak) +
            " / settle=" + V3(settle) +
            " / amount=" + F3(amount) +
            " / moveSeconds=" + F3(moveDur) +
            " / holdSeconds=" + F3(holdDur) +
            " / settleSeconds=" + F3(settleDur) +
            " / returnSeconds=" + F3(returnDur) +
            " / restoreOriginal=1");

        float startTime = Time.time;
        while (Time.time - startTime < moveDur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - startTime) / moveDur);
            float e = Smooth01(t);
            Vector3 pos = Vector3.Lerp(startPos, peak, e) + arc * Mathf.Sin(Mathf.PI * e);
            SetControllerPosition(knee, pos);
            yield return null;
        }

        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        SetControllerPosition(knee, peak);
        if (holdDur > 0.0f)
            yield return new WaitForSeconds(holdDur);
        if (runSerial != targetKneeToSelfThighRunSerial) yield break;

        startTime = Time.time;
        while (Time.time - startTime < settleDur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - startTime) / settleDur);
            float e = Smooth01(Smooth01(t));
            SetControllerPosition(knee, Vector3.Lerp(peak, settle, e));
            yield return null;
        }

        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        SetControllerPosition(knee, settle);

        startTime = Time.time;
        while (Time.time - startTime < returnDur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - startTime) / returnDur);
            float e = Smooth01(Smooth01(t));
            SetControllerPosition(knee, Vector3.Lerp(settle, startPos, e));
            yield return null;
        }

        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        SetControllerPosition(knee, startPos);
        SetControllerRotation(knee, startRot);
        yield return new WaitForSeconds(RandomKneeReactionRestoreStabilizeSeconds);
        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        try { knee.currentPositionState = startPosState; } catch { }
        try { knee.currentRotationState = startRotState; } catch { }

        DebugMessage("[HumanBodyAction] Random knee small nudge restored" +
            " / source=" + source +
            " / knee=" + kneeLabel +
            " / footUntouched=1" +
            " / positionState=restore-original:" + startPosState.ToString() +
            " / rotationState=restore-original:" + startRotState.ToString());
    }

    IEnumerator RandomKneePairOpenCloseRoutine(string source, int runSerial, Atom moveAtom, FreeControllerV3 lKnee, FreeControllerV3 rKnee, FreeControllerV3 lFoot, FreeControllerV3 rFoot, bool open)
    {
        Vector3 lStart = GetControllerPosition(lKnee);
        Vector3 rStart = GetControllerPosition(rKnee);
        Quaternion lRot = GetControllerRotation(lKnee);
        Quaternion rRot = GetControllerRotation(rKnee);
        FreeControllerV3.PositionState lPosState = lKnee.currentPositionState;
        FreeControllerV3.RotationState lRotState = lKnee.currentRotationState;
        FreeControllerV3.PositionState rPosState = rKnee.currentPositionState;
        FreeControllerV3.RotationState rRotState = rKnee.currentRotationState;

        // v092: pair branch snapshots knees only; both feet are excluded completely.
        CaptureTargetKneeToSelfThighSnapshot(moveAtom, lKnee, rKnee, null, null);

        // v091: Pair Open/Close moves both knees, so both Foot IK controls must stay untouched.
        // Turning both feet Off can make VaM drop the whole lower body/root.

        Vector3 axis = rStart - lStart;
        axis.y = 0.0f;
        if (axis.sqrMagnitude < 0.0001f)
            axis = GetAtomHorizontalRight(moveAtom);
        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.right;
        axis.Normalize();

        float totalAmount = UnityEngine.Random.Range(RandomKneeReactionPairAmountMin, RandomKneeReactionPairAmountMax);
        float eachAmount = totalAmount * 0.5f;
        float currentDistance = Vector3.Distance(lStart, rStart);
        float verticalGap = Mathf.Abs(lStart.y - rStart.y);
        string mode = open ? "pair-open" : "pair-close";

        // v044: Pair Open/Close is allowed, but non-pair single knee fallback is disabled.
        // If the knees are too far apart vertically, skip instead of falling back to a single nudge.
        if (verticalGap > RandomKneeReactionPairMaxVerticalGap)
        {
            hbaLastBlock = "Random knee pair " + mode + " skipped: vertical gap";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] Random knee pair " + (open ? "open" : "close") + " skipped" +
                " / source=" + source +
                " / moveAtom=" + (moveAtom != null ? moveAtom.uid : "<none>") +
                " / reason=vertical-gap" +
                " / lStart=" + V3(lStart) +
                " / rStart=" + V3(rStart) +
                " / verticalGap=" + F3(verticalGap) +
                " / threshold=" + F3(RandomKneeReactionPairMaxVerticalGap) +
                " / fallback=disabled" +
                " / footUntouched=1" +
                " / avoidWholeBodyDrop=1");
            yield break;
        }

        if (!open)
        {
            float allowedEach = Mathf.Max(0.0f, (currentDistance - RandomKneeReactionPairMinDistance) * 0.5f);
            eachAmount = Mathf.Min(eachAmount, allowedEach);
            if (eachAmount < 0.004f)
            {
                hbaLastBlock = "Random knee pair close skipped: already close";
                UpdateHbaStatus(true);
                DebugMessage("[HumanBodyAction] Random knee pair close skipped" +
                    " / source=" + source +
                    " / moveAtom=" + (moveAtom != null ? moveAtom.uid : "<none>") +
                    " / currentDistance=" + F3(currentDistance) +
                    " / minDistance=" + F3(RandomKneeReactionPairMinDistance));
                yield break;
            }
        }

        Vector3 lPeak = open ? lStart - axis * eachAmount : lStart + axis * eachAmount;
        Vector3 rPeak = open ? rStart + axis * eachAmount : rStart - axis * eachAmount;
        Vector3 lSettle = Vector3.Lerp(lPeak, lStart, UnityEngine.Random.Range(RandomKneeReactionReturnRatioMin, RandomKneeReactionReturnRatioMax));
        Vector3 rSettle = Vector3.Lerp(rPeak, rStart, UnityEngine.Random.Range(RandomKneeReactionReturnRatioMin, RandomKneeReactionReturnRatioMax));
        Vector3 arc = Vector3.up * UnityEngine.Random.Range(RandomKneeReactionArcUpMin, RandomKneeReactionArcUpMax);

        float moveDur = UnityEngine.Random.Range(RandomKneeReactionMoveSecondsMin, RandomKneeReactionMoveSecondsMax);
        float holdDur = UnityEngine.Random.Range(RandomKneeReactionHoldSecondsMin, RandomKneeReactionHoldSecondsMax);
        float settleDur = UnityEngine.Random.Range(RandomKneeReactionSettleSecondsMin, RandomKneeReactionSettleSecondsMax);
        float returnDur = UnityEngine.Random.Range(RandomKneeReactionReturnSecondsMin, RandomKneeReactionReturnSecondsMax);

        try { lKnee.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { rKnee.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { lKnee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        try { rKnee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        hbaLastBlock = "Random knee " + mode;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Random knee pair " + (open ? "open" : "close") + " start" +
            " / source=" + source +
            " / moveAtom=" + (moveAtom != null ? moveAtom.uid : "<none>") +
            " / lFootTempOff=0" +
            " / rFootTempOff=0" +
            " / footUntouched=1" +
            " / lStart=" + V3(lStart) +
            " / rStart=" + V3(rStart) +
            " / lPeak=" + V3(lPeak) +
            " / rPeak=" + V3(rPeak) +
            " / lSettle=" + V3(lSettle) +
            " / rSettle=" + V3(rSettle) +
            " / amountTotal=" + F3(eachAmount * 2.0f) +
            " / currentDistance=" + F3(currentDistance) +
            " / verticalGap=" + F3(verticalGap) +
            " / yGuardThreshold=" + F3(RandomKneeReactionPairMaxVerticalGap) +
            " / moveSeconds=" + F3(moveDur) +
            " / holdSeconds=" + F3(holdDur) +
            " / settleSeconds=" + F3(settleDur) +
            " / returnSeconds=" + F3(returnDur) +
            " / restoreOriginal=1");

        float startTime = Time.time;
        while (Time.time - startTime < moveDur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - startTime) / moveDur);
            float e = Smooth01(t);
            float wave = Mathf.Sin(Mathf.PI * e);
            SetControllerPosition(lKnee, Vector3.Lerp(lStart, lPeak, e) + arc * wave);
            SetControllerPosition(rKnee, Vector3.Lerp(rStart, rPeak, e) + arc * wave);
            yield return null;
        }

        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        SetControllerPosition(lKnee, lPeak);
        SetControllerPosition(rKnee, rPeak);
        if (holdDur > 0.0f)
            yield return new WaitForSeconds(holdDur);
        if (runSerial != targetKneeToSelfThighRunSerial) yield break;

        startTime = Time.time;
        while (Time.time - startTime < settleDur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - startTime) / settleDur);
            float e = Smooth01(Smooth01(t));
            SetControllerPosition(lKnee, Vector3.Lerp(lPeak, lSettle, e));
            SetControllerPosition(rKnee, Vector3.Lerp(rPeak, rSettle, e));
            yield return null;
        }

        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        SetControllerPosition(lKnee, lSettle);
        SetControllerPosition(rKnee, rSettle);

        startTime = Time.time;
        while (Time.time - startTime < returnDur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - startTime) / returnDur);
            float e = Smooth01(Smooth01(t));
            SetControllerPosition(lKnee, Vector3.Lerp(lSettle, lStart, e));
            SetControllerPosition(rKnee, Vector3.Lerp(rSettle, rStart, e));
            yield return null;
        }

        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        SetControllerPosition(lKnee, lStart);
        SetControllerPosition(rKnee, rStart);
        SetControllerRotation(lKnee, lRot);
        SetControllerRotation(rKnee, rRot);
        yield return new WaitForSeconds(RandomKneeReactionRestoreStabilizeSeconds);
        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        try { lKnee.currentPositionState = lPosState; } catch { }
        try { lKnee.currentRotationState = lRotState; } catch { }
        try { rKnee.currentPositionState = rPosState; } catch { }
        try { rKnee.currentRotationState = rRotState; } catch { }

        DebugMessage("[HumanBodyAction] Random knee pair " + (open ? "open" : "close") + " restored" +
            " / source=" + source +
            " / footUntouched=1" +
            " / positionState=restore-original:L=" + lPosState.ToString() + ",R=" + rPosState.ToString() +
            " / rotationState=restore-original:L=" + lRotState.ToString() + ",R=" + rRotState.ToString());
    }

    void SetMovingKneeFootIkTemporaryOff(FreeControllerV3 foot, string source, string footLabel, string kneeLabel, string reason)
    {
        if (foot == null)
            return;

        try { foot.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
        try { foot.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        DebugMessage("[HumanBodyAction] Random knee single-thigh moving-foot IK temp off" +
            " / source=" + source +
            " / knee=" + kneeLabel +
            " / foot=" + footLabel +
            " / reason=" + reason +
            " / restore=state-only-after-knee-return");
    }

    void RestoreMovingKneeFootIkStateOnly(FreeControllerV3 foot, FreeControllerV3.PositionState positionState, FreeControllerV3.RotationState rotationState)
    {
        if (foot == null)
            return;

        try { foot.currentPositionState = positionState; } catch { }
        try { foot.currentRotationState = rotationState; } catch { }
    }

    Vector3 BuildRandomKneeSafeThighAnchor(Atom goalAtom, FreeControllerV3 goalThigh, bool goalLeftThigh, bool sameSide, Vector3 startPos, out string info)
    {
        Vector3 thighPos = GetControllerPosition(goalThigh);
        Vector3 right = GetAtomHorizontalRight(goalAtom);
        Vector3 forward = GetAtomHorizontalForward(goalAtom);

        Vector3 outward = goalLeftThigh ? -right : right;
        if (outward.sqrMagnitude < 0.0001f) outward = Vector3.right;
        outward.Normalize();

        Vector3 goal = thighPos
            + outward * TargetKneeToThighSafeOutwardOffset
            + forward * TargetKneeToThighSafeForwardOffset;

        // v093 no-drop guard:
        // The previous safe anchor used Vector3.down * 0.100 and could pull the knee target downward.
        // With leg IK, a downward knee target can make VaM solve by lowering hip/root.
        // Keep the thigh-directed XZ anchor, but never lower the knee target below its current Y.
        float guardedY = startPos.y + 0.015f;
        if (thighPos.y > startPos.y + 0.045f)
            guardedY = Mathf.Min(thighPos.y - 0.035f, startPos.y + 0.045f);
        goal.y = Mathf.Max(startPos.y, guardedY);

        info = "raw=" + V3(thighPos) +
            ",out=" + V3(outward) +
            ",forward=" + V3(forward) +
            ",outOff=" + F3(TargetKneeToThighSafeOutwardOffset) +
            ",downOff=0.000" +
            ",fwdOff=" + F3(TargetKneeToThighSafeForwardOffset) +
            ",noDropY=1" +
            ",sameSide=" + (sameSide ? "1" : "0");
        return goal;
    }

    Vector3 GetAtomHorizontalRight(Atom atom)
    {
        Transform t = null;
        try
        {
            if (atom != null && atom.mainController != null) t = atom.mainController.transform;
        }
        catch { }
        if (t == null && atom != null)
        {
            try { t = atom.transform; } catch { }
        }

        Vector3 v = t != null ? t.right : Vector3.right;
        v.y = 0.0f;
        if (v.sqrMagnitude < 0.0001f) v = Vector3.right;
        v.Normalize();
        return v;
    }

    Vector3 GetAtomHorizontalForward(Atom atom)
    {
        Transform t = null;
        try
        {
            if (atom != null && atom.mainController != null) t = atom.mainController.transform;
        }
        catch { }
        if (t == null && atom != null)
        {
            try { t = atom.transform; } catch { }
        }

        Vector3 v = t != null ? t.forward : Vector3.forward;
        v.y = 0.0f;
        if (v.sqrMagnitude < 0.0001f) v = Vector3.forward;
        v.Normalize();
        return v;
    }

    IEnumerator HoldRandomKneePosition(FreeControllerV3 knee, Vector3 position, float seconds)
    {
        if (knee == null) yield break;
        float dur = Mathf.Max(0.01f, seconds);
        float start = Time.time;
        while (Time.time - start < dur)
        {
            SetControllerPosition(knee, position);
            yield return null;
        }
        SetControllerPosition(knee, position);
    }

    Vector3 SoftSnapRandomKneePosition(FreeControllerV3 knee, Vector3 commandedPosition, string source, string kneeLabel, string thighLabel)
    {
        if (knee == null) return commandedPosition;

        Vector3 bodyKneePosition;
        string bodyName;
        if (!TryGetBodyTransformPositionForController(knee, out bodyKneePosition, out bodyName))
        {
            DebugMessage("[HumanBodyAction] Random knee soft snap skip / reason=no-body-transform" +
                " / source=" + source +
                " / knee=" + kneeLabel +
                " / thigh=" + thighLabel +
                " / commanded=" + V3(commandedPosition));
            return commandedPosition;
        }

        float distance = Vector3.Distance(commandedPosition, bodyKneePosition);
        if (distance < TargetKneeToThighSoftSnapMinDistance)
        {
            DebugMessage("[HumanBodyAction] Random knee soft snap skip / reason=near" +
                " / source=" + source +
                " / knee=" + kneeLabel +
                " / thigh=" + thighLabel +
                " / dist=" + F3(distance));
            return commandedPosition;
        }

        Vector3 snapped = bodyKneePosition;
        bool clamped = distance > TargetKneeToThighSoftSnapMaxDistance;
        SetControllerPosition(knee, snapped);

        DebugMessage("[HumanBodyAction] Random knee soft snap" +
            " / source=" + source +
            " / knee=" + kneeLabel +
            " / thigh=" + thighLabel +
            " / body=" + bodyName +
            " / commanded=" + V3(commandedPosition) +
            " / bodyPos=" + V3(bodyKneePosition) +
            " / hold=" + V3(snapped) +
            " / dist=" + F3(distance) +
            " / max=" + F3(TargetKneeToThighSoftSnapMaxDistance) +
            " / clamped=" + (clamped ? "1" : "0"));
        return snapped;
    }

    void SetSingleMoveKneeFree(FreeControllerV3 knee, string source, string moveUid, string goalUid, string kneeLabel, string reason)
    {
        if (knee != null)
        {
            try { knee.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
            try { knee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }

        hbaLastBlock = "Random knee free: " + kneeLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Random knee free" +
            " / source=" + source +
            " / moveAtom=" + moveUid +
            " / goalAtom=" + goalUid +
            " / knee=" + kneeLabel +
            " / reason=" + reason +
            " / seconds=" + F2(TargetKneeToSelfThighPreFreeSeconds) +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");
    }

    void SetSingleMoveKneeAndFootFree(FreeControllerV3 knee, FreeControllerV3 foot, string source, string moveUid, string goalUid, string kneeLabel, string footLabel, string reason)
    {
        if (knee != null)
        {
            try { knee.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
            try { knee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }
        if (foot != null)
        {
            try { foot.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
            try { foot.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }

        hbaLastBlock = "Random knee+foot pre-free: " + kneeLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Random knee foot pre-free" +
            " / source=" + source +
            " / moveAtom=" + moveUid +
            " / goalAtom=" + goalUid +
            " / knee=" + kneeLabel +
            " / foot=" + footLabel +
            " / kneeFree=" + (knee != null ? "1" : "0") +
            " / footFree=" + (foot != null ? "1" : "0") +
            " / reason=" + reason +
            " / seconds=" + F2(TargetKneeToSelfThighPreFreeSeconds) +
            " / next=move-knee-to-thigh" +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");
    }

    Vector3 ClampVectorFromStart(Vector3 startPosition, Vector3 requestedPosition, float maxDistance)
    {
        Vector3 delta = requestedPosition - startPosition;
        float distance = delta.magnitude;
        if (distance <= maxDistance || distance < 0.0001f)
            return requestedPosition;
        return startPosition + delta.normalized * Mathf.Max(0.0f, maxDistance);
    }

    void SetRandomKneeAndFootIkOffAfterFarReach(FreeControllerV3 knee, FreeControllerV3 foot, string source, string moveUid, string goalUid, string kneeLabel, string footLabel, string thighLabel, Vector3 requestedGoal, Vector3 reachedPosition, float requestedDistance)
    {
        if (knee != null)
        {
            SetControllerPosition(knee, reachedPosition);
            try { knee.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
            try { knee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }
        if (foot != null)
        {
            try { foot.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
            try { foot.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }

        hbaLastBlock = "Random knee far reach-off: " + kneeLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Random knee far reach IK off" +
            " / source=" + source +
            " / moveAtom=" + moveUid +
            " / goalAtom=" + goalUid +
            " / knee=" + kneeLabel +
            " / foot=" + footLabel +
            " / thigh=" + thighLabel +
            " / requestedGoal=" + V3(requestedGoal) +
            " / reached=" + V3(reachedPosition) +
            " / requestDist=" + F3(requestedDistance) +
            " / tooFarLimit=" + F3(RandomKneeToThighTooFarDistance) +
            " / kneeState=Off / footState=Off" +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");
    }

    void RequestTargetKneeToSelfThighTest(string source)
    {
        // Test button/action is direct because it is intended to verify Target knee IK routing immediately.
        // It does not use the shared latest-only action queue or the random hand cover path.
        suppressEventHandCoverUntil = Time.time + HandCoverManualSuppressEventSeconds;
        pendingRequest = null;
        handCoverRunSerial++;
        targetKneeToSelfThighRunSerial++;

        if (!IsHbaEnabled())
        {
            hbaLastBlock = "Disabled: target knee test skipped";
            DebugMessage("[HumanBodyAction] Target knee test skipped because HBA Enable is OFF / source=" + source);
            UpdateHbaStatus(true);
            return;
        }

        if (directHandCoverRoutine != null)
        {
            StopCoroutine(directHandCoverRoutine);
            directHandCoverRoutine = null;
        }

        directHandCoverRoutine = StartCoroutine(DirectTargetKneeToSelfThighTestRoutine(source));
    }

    IEnumerator DirectTargetKneeToSelfThighTestRoutine(string source)
    {
        hbaLastAction = source;
        hbaLastBlock = "Move knee -> other thigh direct prefree";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] ACTION START DIRECT / source=" + source + " / preset=TargetKneeToSelfThighTest / head=Off");

        yield return StartCoroutine(TargetKneeToSelfThighTestRoutine(source));

        suppressEventHandCoverUntil = Mathf.Max(suppressEventHandCoverUntil, Time.time + HandCoverManualSuppressEventSeconds);
        DebugMessage("[HumanBodyAction] ACTION DONE DIRECT / source=" + source + " / preset=TargetKneeToSelfThighTest");
        directHandCoverRoutine = null;
    }

    IEnumerator TargetKneeToSelfThighTestRoutine(string source)
    {
        RefreshControllersNoReset();

        // This test is not a hand-cover action. Restore any held cover first so the old hand route does not confuse verification.
        // Important: RestoreTargetKneeToSelfThighSnapshot increments targetKneeToSelfThighRunSerial.
        // Assign this run serial AFTER restore, otherwise the route exits after pre-free before moving.
        RestoreHandCoverSnapshot("target-knee-test-start");
        RestoreTargetKneeToSelfThighSnapshot("restart-target-knee-test");
        int runSerial = ++targetKneeToSelfThighRunSerial;

        Atom moveAtom = containingAtom;
        Atom goalAtom = FindNearestOtherPersonAtom();
        if (moveAtom == null || goalAtom == null)
        {
            hbaLastBlock = "Knee test: missing move/goal Person";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] Knee->thigh test skipped / reason=missing-person" +
                " / moveAtom=" + (moveAtom != null ? moveAtom.uid : "<none>") +
                " / goalAtom=" + (goalAtom != null ? goalAtom.uid : "<none>") +
                " / source=" + source);
            yield break;
        }

        // v055: opposite route from v054.
        // Move the knees on the Person that owns this HumanBodyAction, and use the nearest other Person thighs as the goals.
        FreeControllerV3 moveLKnee = FindControllerByAliases("lKneeControl", "leftKneeControl", "lKnee", "leftKnee");
        FreeControllerV3 moveRKnee = FindControllerByAliases("rKneeControl", "rightKneeControl", "rKnee", "rightKnee");
        FreeControllerV3 moveLFoot = FindControllerByAliases("lFootControl", "leftFootControl", "lFoot", "leftFoot");
        FreeControllerV3 moveRFoot = FindControllerByAliases("rFootControl", "rightFootControl", "rFoot", "rightFoot");
        FreeControllerV3 goalLThigh = FindControllerByAliasesOnAtom(goalAtom, "lThighControl", "leftThighControl", "lThigh", "leftThigh");
        FreeControllerV3 goalRThigh = FindControllerByAliasesOnAtom(goalAtom, "rThighControl", "rightThighControl", "rThigh", "rightThigh");

        if (moveLKnee == null || moveRKnee == null || goalLThigh == null || goalRThigh == null)
        {
            hbaLastBlock = "Knee test: missing IK";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] Knee->thigh test skipped / reason=missing-ik" +
                " / moveAtom=" + moveAtom.uid +
                " / goalAtom=" + goalAtom.uid +
                " / moveLKnee=" + (moveLKnee != null ? "1" : "0") +
                " / moveRKnee=" + (moveRKnee != null ? "1" : "0") +
                " / moveLFoot=" + (moveLFoot != null ? "1" : "0") +
                " / moveRFoot=" + (moveRFoot != null ? "1" : "0") +
                " / goalLThigh=" + (goalLThigh != null ? "1" : "0") +
                " / goalRThigh=" + (goalRThigh != null ? "1" : "0") +
                " / source=" + source);
            yield break;
        }

        CaptureTargetKneeToSelfThighSnapshot(moveAtom, moveLKnee, moveRKnee, moveLFoot, moveRFoot);

        // Briefly free the moving knees and feet first so the current IK chain does not fight the move.
        // Then positionState is turned back On and the knees are moved to the other Person's thighs.
        SetTargetKneeFootPreFreeForTest(moveLKnee, moveRKnee, moveLFoot, moveRFoot, source, moveAtom.uid, goalAtom.uid);
        if (TargetKneeToSelfThighPreFreeSeconds > 0.0f)
            yield return new WaitForSeconds(TargetKneeToSelfThighPreFreeSeconds);
        if (runSerial != targetKneeToSelfThighRunSerial) yield break;

        Vector3 lStart = GetControllerPosition(moveLKnee);
        Vector3 rStart = GetControllerPosition(moveRKnee);
        Vector3 lGoal = GetControllerPosition(goalLThigh);
        Vector3 rGoal = GetControllerPosition(goalRThigh);

        try { moveLKnee.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { moveRKnee.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        // Keep moving knee rotation free while the position IK is moved.
        try { moveLKnee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        try { moveRKnee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        hbaLastBlock = "Move knee -> other thigh: " + moveAtom.uid + " -> " + goalAtom.uid;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Move knee->other thigh start" +
            " / source=" + source +
            " / moveAtom=" + moveAtom.uid +
            " / goalAtom=" + goalAtom.uid +
            " / lStart=" + V3(lStart) +
            " / lGoal=" + V3(lGoal) +
            " / lFootFree=" + (moveLFoot != null ? "1" : "0") +
            " / rStart=" + V3(rStart) +
            " / rGoal=" + V3(rGoal) +
            " / rFootFree=" + (moveRFoot != null ? "1" : "0") +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");

        float dur = Mathf.Max(0.01f, TargetKneeToSelfThighMoveSeconds);
        float start = Time.time;
        while (Time.time - start < dur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - start) / dur);
            float e = Smooth01(t);
            SetControllerPosition(moveLKnee, Vector3.Lerp(lStart, lGoal, e));
            SetControllerPosition(moveRKnee, Vector3.Lerp(rStart, rGoal, e));
            yield return null;
        }

        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        SetControllerPosition(moveLKnee, lGoal);
        SetControllerPosition(moveRKnee, rGoal);

        hbaLastBlock = "Move knee holding other thigh";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Move knee->other thigh hold" +
            " / source=" + source +
            " / moveAtom=" + moveAtom.uid +
            " / goalAtom=" + goalAtom.uid +
            " / lKnee=" + V3(GetControllerPosition(moveLKnee)) +
            " / rKnee=" + V3(GetControllerPosition(moveRKnee)) +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");
    }


    void SetTargetKneeFootPreFreeForTest(FreeControllerV3 lKnee, FreeControllerV3 rKnee, FreeControllerV3 lFoot, FreeControllerV3 rFoot, string source, string moveUid, string goalUid)
    {
        if (lKnee != null)
        {
            try { lKnee.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
            try { lKnee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }
        if (rKnee != null)
        {
            try { rKnee.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
            try { rKnee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }
        if (lFoot != null)
        {
            try { lFoot.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
            try { lFoot.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }
        if (rFoot != null)
        {
            try { rFoot.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
            try { rFoot.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }

        hbaLastBlock = "Move knee+foot pre-free: " + moveUid;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Move knee foot pre-free" +
            " / source=" + source +
            " / moveAtom=" + moveUid +
            " / goalAtom=" + goalUid +
            " / seconds=" + F2(TargetKneeToSelfThighPreFreeSeconds) +
            " / lKnee=" + (lKnee != null ? "free" : "none") +
            " / rKnee=" + (rKnee != null ? "free" : "none") +
            " / lFoot=" + (lFoot != null ? "free" : "none") +
            " / rFoot=" + (rFoot != null ? "free" : "none") +
            " / next=move-knee-to-other-thigh" +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");
    }

    bool ShouldRunRandomHandCover(string source)
    {
        // Manual button is a test button, so it always runs.
        // Event/Action/TG/HBR routes obey the probability slider to avoid firing on every Inside Active tick.
        if (!string.IsNullOrEmpty(source) && source.StartsWith("button:"))
            return true;

        if (Time.time < suppressEventHandCoverUntil)
        {
            hbaLastBlock = "Cover skipped: manual test hold";
            DebugMessage("[HumanBodyAction] Cover skipped / source=" + source + " / reason=manual-test-hold / remain=" + F1(suppressEventHandCoverUntil - Time.time));
            UpdateHbaStatus(true);
            return false;
        }

        float chance = handCoverChance != null ? handCoverChance.val : HandCoverChanceDefault;
        chance = Mathf.Clamp(chance, HandCoverChanceMin, HandCoverChanceMax);

        if (chance >= 99.999f)
            return true;

        if (chance <= 0.001f)
        {
            hbaLastBlock = "Cover chance skipped: 0%";
            DebugMessage("[HumanBodyAction] Cover chance skipped / source=" + source + " / chance=" + F1(chance) + "%");
            UpdateHbaStatus(true);
            return false;
        }

        float roll = UnityEngine.Random.Range(0.0f, 100.0f);
        if (roll > chance)
        {
            hbaLastBlock = "Cover chance skipped: " + F1(roll) + " > " + F1(chance) + "%";
            DebugMessage("[HumanBodyAction] Cover chance skipped / source=" + source + " / roll=" + F1(roll) + " / chance=" + F1(chance) + "%");
            UpdateHbaStatus(true);
            return false;
        }

        DebugMessage("[HumanBodyAction] Cover chance hit / source=" + source + " / roll=" + F1(roll) + " / chance=" + F1(chance) + "%");
        return true;
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

        if (request.runHandCover)
        {
            yield return StartCoroutine(RandomHandCoverRoutine(request.source));
        }

        if (request.runTargetKneeToSelfThighTest)
        {
            yield return StartCoroutine(TargetKneeToSelfThighTestRoutine(request.source));
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
        float faceScale = faceTimeScale != null ? Mathf.Clamp(faceTimeScale.val, FaceTimeScaleMin, FaceTimeScaleMax) : FaceTimeScaleDefault;
        currentEyesDuration = Mathf.Max(0.05f, eyesDuration * faceScale);
        currentEyesTarget = Mathf.Clamp(eyesTarget, 0.0f, 1.0f);
        currentMouthDuration = Mathf.Max(0.05f, mouthDuration * faceScale);
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


    IEnumerator RandomHandCoverRoutine(string source)
    {
        return RandomHandCoverRoutine(source, false, false);
    }

    IEnumerator RandomHandCoverRoutine(string source, bool kneeThighTestOnly)
    {
        return RandomHandCoverRoutine(source, kneeThighTestOnly, false);
    }

    IEnumerator RandomHandCoverRoutine(string source, bool kneeThighTestOnly, bool forcePushAwayOnly)
    {
        int runSerial = ++handCoverRunSerial;
        RefreshControllersNoReset();

        // v039: a previous cover hand stays at the cover point until explicit restore/reset.
        // Before starting a new random cover, restore the previous hand first so hand-free detection is correct.
        yield return StartCoroutine(RestoreHandCoverSnapshotRoutine("restart-cover", true));
        if (!IsHandCoverRunStillCurrent(runSerial)) yield break;

        FreeControllerV3 leftHand = FindControllerByAliases("lHandControl", "leftHandControl", "lHand");
        FreeControllerV3 rightHand = FindControllerByAliases("rHandControl", "rightHandControl", "rHand");

        List<FreeControllerV3> freeHands = new List<FreeControllerV3>();
        if (IsHandFreeForCover(leftHand)) freeHands.Add(leftHand);
        if (IsHandFreeForCover(rightHand)) freeHands.Add(rightHand);

        // In many VaM poses both hand IK controllers are already PositionState.On even though
        // they are not being held by this HBA cover routine. Older builds treated that as
        // "no free hand" and skipped forever. Prefer Off/Comply hands, but if both are On,
        // fall back to either available hand so RandomHandCover can still run.
        if (freeHands.Count == 0)
        {
            if (leftHand != null && !IsHandBlockedByTargetGrabberHeldTargetHand(leftHand)) freeHands.Add(leftHand);
            if (rightHand != null && !IsHandBlockedByTargetGrabberHeldTargetHand(rightHand)) freeHands.Add(rightHand);

            if (freeHands.Count > 0)
            {
                DebugMessage("[HumanBodyAction] Cover hand fallback / reason=no-position-off-hand" +
                    " / source=" + source +
                    " / lState=" + GetPositionStateLabel(leftHand) +
                    " / rState=" + GetPositionStateLabel(rightHand) +
                    " / lTgHeld=" + (IsTargetGrabberHoldingTargetHandSide(-1) ? "1" : "0") +
                    " / rTgHeld=" + (IsTargetGrabberHoldingTargetHandSide(1) ? "1" : "0") +
                    " / candidates=" + freeHands.Count);
            }
        }

        if (freeHands.Count == 0)
        {
            hbaLastBlock = "Cover: no hand controller";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] Cover skipped / no hand controller / source=" + source);
            DebugMessage("[HumanBodyAction] Hand bonus knee local disabled / source=" + source + " / reason=no-hand-controller");
            yield return StartCoroutine(MaybeRandomHandElbowNudgeRoutine(source, "no-hand-controller"));
            yield break;
        }

        FreeControllerV3 hand;
        HandCoverTarget target;
        string selectedCoverMode;
        int targetCandidateCount;
        if (!TrySelectRandomHandCoverTarget(source, kneeThighTestOnly, forcePushAwayOnly, freeHands, out hand, out target, out selectedCoverMode, out targetCandidateCount))
        {
            hbaLastBlock = kneeThighTestOnly ? "Cover test: no Self Thigh target" : "Cover: no compatible target";
            UpdateHbaStatus(true);
            DebugMessage("[HumanBodyAction] Cover skipped / no compatible target / source=" + source +
                " / kneeThighTest=" + (kneeThighTestOnly ? "1" : "0") +
                " / freeHands=" + freeHands.Count);
            DebugMessage("[HumanBodyAction] Hand bonus knee local disabled / source=" + source + " / reason=" + (kneeThighTestOnly ? "no-self-thigh-target" : "no-compatible-cover-target"));
            yield return StartCoroutine(MaybeRandomHandElbowNudgeRoutine(source, kneeThighTestOnly ? "no-self-thigh-target" : "no-compatible-cover-target"));
            yield break;
        }

        Vector3 startPosition = GetControllerPosition(hand);
        string targetDisplayLabel = GetHandCoverTargetDisplayLabel(target);
        bool upperTarget = IsUpperHandCoverTargetLabel(target.label);

        hbaLastBlock = selectedCoverMode + " selected: " + GetHandLabel(hand) + " -> " + targetDisplayLabel;
        UpdateHbaStatus(true);
        CoverSelectionMessage(hbaLastBlock);
        DebugMessage("[HumanBodyAction] Cover selected detail" +
            " / mode=" + selectedCoverMode +
            " / source=" + source +
            " / hand=" + GetHandLabel(hand) +
            " / target=" + targetDisplayLabel +
            " / rawTarget=" + target.label +
            " / requiredHand=" + GetRequiredHandSideLabel(target.requiredHandSide) +
            " / upper=" + (upperTarget ? "1" : "0") +
            " / pushAway=" + (target.pushAway ? "1" : "0") +
            " / candidates=" + targetCandidateCount +
            " / targetPos=" + V3(target.position));

        string hlaActionName = "";
        string hlaDelegateReason = "";
        bool hlaDelegateCandidate = !target.pushAway && TryGetHumanLifeActionCoverDelegate(target, out hlaActionName, out hlaDelegateReason);
        if (!target.pushAway && (hlaDelegateCandidate ||
            targetDisplayLabel.IndexOf("Head", StringComparison.OrdinalIgnoreCase) >= 0 ||
            targetDisplayLabel.IndexOf("Hip", StringComparison.OrdinalIgnoreCase) >= 0 ||
            (!string.IsNullOrEmpty(target.label) && target.label.IndexOf("Head", StringComparison.OrdinalIgnoreCase) >= 0) ||
            (!string.IsNullOrEmpty(target.label) && target.label.IndexOf("Hip", StringComparison.OrdinalIgnoreCase) >= 0)))
        {
            HbaHlaLog("[HumanBodyAction] HBA->HLA CHECK / build=v089 / caller=HumanBodyAction / route=RandomHandCover / match=" + (hlaDelegateCandidate ? "1" : "0") +
                " / target=" + targetDisplayLabel +
                " / rawTarget=" + target.label +
                " / pushAway=" + (target.pushAway ? "1" : "0") +
                " / hlaAction=" + hlaActionName +
                " / reason=" + hlaDelegateReason +
                " / source=" + source);
        }


        if (hlaDelegateCandidate)
        {
            string delegateSource = source + ":random-hand:" + hlaDelegateReason;
            HbaHlaLog("[HumanBodyAction] HBA->HLA SELECTED / build=v089 / caller=HumanBodyAction / route=RandomHandCover / target=" + targetDisplayLabel + " / rawTarget=" + target.label + " / reason=" + hlaDelegateReason + " / hlaAction=" + hlaActionName + " / source=" + source);

            if (TryInvokeHumanLifeAction(hlaActionName, delegateSource))
            {
                hbaLastBlock = "HBA->HLA delegated: " + targetDisplayLabel + " -> " + hlaActionName;
                UpdateHbaStatus(true);
                CoverSelectionMessage("HBA->HLA delegated: " + targetDisplayLabel + " -> " + hlaActionName);
                yield break;
            }

            HbaHlaLog("[HumanBodyAction] HBA->HLA FALLBACK -> HBA cover / build=v089 / caller=HumanBodyAction / target=" + target.label + " / hlaAction=" + hlaActionName + " / reason=delegate-not-available / source=" + source);
        }

        CaptureHandCoverSnapshot(hand);
        RelaxHandCoverElbow(hand);

        Quaternion lockedRotation = GetControllerRotation(hand);
        Vector3 coverOut = target.outward;
        if (coverOut.sqrMagnitude < 0.0001f) coverOut = Vector3.forward;
        coverOut.Normalize();
        float targetOffset = target.pushAway
            ? Mathf.Clamp(handCoverPushAwayOffset != null ? handCoverPushAwayOffset.val : HandCoverPushAwayOffsetDefault, HandCoverPushAwayOffsetMin, HandCoverPushAwayOffsetMax)
            : HandCoverSurfaceOffset;
        Vector3 coverPosition = target.position + coverOut * targetOffset;
        Vector3 requestedCoverPosition = coverPosition;
        float requestedCoverDistance;
        float handTooFarThreshold;
        bool handFarTooFar = IsHandCoverFarTooFar(startPosition, requestedCoverPosition, target.label, kneeThighTestOnly, out requestedCoverDistance, out handTooFarThreshold);

        try
        {
            hand.currentPositionState = FreeControllerV3.PositionState.On;
        }
        catch { }

        // v039: position-only cover hold. Rotation IK is OFF and the current wrist rotation is kept.
        // The hand does not auto-return; HBA_Cover_Restore/HBR_Cover_Restore or HBA_Reset restores it.
        try
        {
            hand.currentRotationState = FreeControllerV3.RotationState.Off;
        }
        catch { }
        SetControllerRotation(hand, lockedRotation);

        string coverMode = selectedCoverMode;
        coverPosition = ClampHandCoverCommandPosition(startPosition, coverPosition, coverMode, target.label, kneeThighTestOnly);
        hbaLastBlock = coverMode + " hold: " + GetHandLabel(hand) + " -> " + targetDisplayLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Cover start / mode=" + coverMode + " / source=" + source + " / hand=" + GetHandLabel(hand) + " / target=" + targetDisplayLabel + " / start=" + V3(startPosition) + " / requested=" + V3(requestedCoverPosition) + " / cover=" + V3(coverPosition) + " / requestDist=" + F3(requestedCoverDistance) + " / tooFar=" + (handFarTooFar ? "1" : "0") + " / tooFarLimit=" + F3(handTooFarThreshold) + " / offset=" + F3(targetOffset) + " / rot=off-fixed / return=manual");

        if (ShouldUseSelfFaceChestAvoidPath(hand, startPosition, coverPosition, target.label))
        {
            yield return StartCoroutine(MoveHandCoverPositionChestAvoid(hand, startPosition, coverPosition, HandCoverMoveSeconds, lockedRotation, true, target.label));
        }
        else
        {
            yield return StartCoroutine(MoveHandCoverPosition(hand, startPosition, coverPosition, HandCoverMoveSeconds, lockedRotation, true));
        }
        if (!IsHandCoverRunStillCurrent(runSerial)) yield break;

        // v045: Hidden fixed soft snap. Give VaM/body collision a short moment to settle while the hand is
        // still commanded toward the cover point, then snap the IK control back toward the actual body hand.
        // This reduces the strong "push through the body" feeling without exposing more tuning UI.
        yield return StartCoroutine(HoldHandCoverPosition(hand, coverPosition, Mathf.Max(0.01f, handFarTooFar ? HandCoverTooFarIkOffDelay : HandCoverSoftSnapDelay), lockedRotation, true));
        if (!IsHandCoverRunStillCurrent(runSerial)) yield break;

        if (target.pushAway && IsPushAwayBackLimitedTargetLabel(target.label))
        {
            SetHandCoverIkOffAfterFarReach(hand, lockedRotation, source, coverMode, target.label, requestedCoverPosition, coverPosition, requestedCoverDistance, HandCoverPushAwayBackStretchLimit);
            DebugMessage("[HumanBodyAction] PushAway back stretch give up / source=" + source +
                " / hand=" + GetHandLabel(hand) +
                " / target=" + target.label +
                " / reached=" + V3(coverPosition) +
                " / backLimit=" + F3(HandCoverPushAwayBackStretchLimit) +
                " / positionState=Off / rotationState=Off");
            DebugMessage("[HumanBodyAction] Hand bonus knee local disabled / source=" + source + " / reason=pushaway-back-limit:" + target.label);
            yield return StartCoroutine(MaybeRandomHandElbowNudgeRoutine(source, "pushaway-back-limit:" + target.label));
            yield break;
        }

        if (handFarTooFar)
        {
            SetHandCoverIkOffAfterFarReach(hand, lockedRotation, source, coverMode, target.label, requestedCoverPosition, coverPosition, requestedCoverDistance, handTooFarThreshold);
            DebugMessage("[HumanBodyAction] Hand bonus knee local disabled / source=" + source + " / reason=hand-far-too-far:" + target.label);
            yield return StartCoroutine(MaybeRandomHandElbowNudgeRoutine(source, "hand-far-too-far:" + target.label));
            yield break;
        }

        Vector3 holdPosition = coverPosition;
        if (IsUpperHandCoverTargetLabel(target.label))
        {
            // Head/Neck/upper-body cover was often selected but visually stopped around the shoulder
            // because soft-snap immediately replaced the command with the current body-hand position.
            // For upper targets, keep the commanded IK point so the hand actually travels upward.
            DebugMessage("[HumanBodyAction] Cover soft snap skip / reason=upper-target / hand=" + GetHandLabel(hand) + " / mode=" + coverMode + " / target=" + target.label + " / hold=" + V3(holdPosition));
        }
        else
        {
            holdPosition = SoftSnapHandCoverPosition(hand, coverPosition, lockedRotation, coverMode, target.label);
        }
        yield return StartCoroutine(HoldHandCoverPosition(hand, holdPosition, Mathf.Max(0.01f, HandCoverHoldSeconds), lockedRotation, true));
        if (!IsHandCoverRunStillCurrent(runSerial)) yield break;

        SetControllerPosition(hand, holdPosition);
        SetControllerRotation(hand, lockedRotation);
        ApplyHandCoverComplyHoldIfNeeded(hand, holdPosition, lockedRotation, coverMode, target.label);
        hbaLastBlock = coverMode + " holding: " + target.label;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Cover hold / mode=" + coverMode + " / source=" + source + " / hand=" + GetHandLabel(hand) + " / target=" + target.label + " / hold=" + V3(holdPosition) + " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");

        // v077: RandomHand's small knee nudge was too rare when it only ran on hard failures.
        // Also let a successful hand cover add a subtle knee reaction at a fixed 30% chance.
        DebugMessage("[HumanBodyAction] Hand bonus knee local disabled / source=" + source + " / reason=random-hand-bonus");
            yield return StartCoroutine(MaybeRandomHandElbowNudgeRoutine(source, "random-hand-bonus"));
    }

    IEnumerator MaybeRandomHandKneeNudgeRoutine(string source, string reason)
    {
        bool forceManualRandomHandBonus = IsManualRandomHandKneeNudgeBonus(source, reason);
        float roll = forceManualRandomHandBonus ? 0.0f : UnityEngine.Random.Range(0.0f, 100.0f);
        if (!forceManualRandomHandBonus && roll > HandFailKneeNudgeChance)
        {
            DebugMessage("[HumanBodyAction] Hand fallback knee nudge chance skipped" +
                " / source=" + source +
                " / reason=" + reason +
                " / roll=" + F1(roll) +
                " / chance=" + F1(HandFailKneeNudgeChance) + "%");
            yield break;
        }

        DebugMessage("[HumanBodyAction] Hand fallback knee nudge chance hit" +
            " / source=" + source +
            " / reason=" + reason +
            " / roll=" + (forceManualRandomHandBonus ? "force" : F1(roll)) +
            " / chance=" + F1(HandFailKneeNudgeChance) + "%" +
            " / force=" + (forceManualRandomHandBonus ? "1" : "0"));
        yield return StartCoroutine(RandomHandFailKneeNudgeRoutine(source, reason));
    }

    bool IsManualRandomHandKneeNudgeBonus(string source, string reason)
    {
        if (reason != "random-hand-bonus")
            return false;
        if (string.IsNullOrEmpty(source))
            return false;
        return source.IndexOf("button:HBA_Cover_RandomHand", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    void RequestHandBonusKneeNudge(string source)
    {
        if (!IsHbaEnabled())
        {
            hbaLastBlock = "Disabled: bonus knee nudge skipped";
            DebugMessage("[HumanBodyAction] Bonus knee nudge skipped because HBA Enable is OFF / source=" + source);
            UpdateHbaStatus(true);
            return;
        }

        StartCoroutine(DirectHandBonusKneeNudgeRoutine(source));
    }

    IEnumerator DirectHandBonusKneeNudgeRoutine(string source)
    {
        hbaLastBlock = "Bonus knee nudge direct";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] ACTION START DIRECT / source=" + source + " / preset=BonusKneeNudge / head=Off");
        yield return StartCoroutine(RandomHandFailKneeNudgeRoutine(source, "bonus-action"));
        DebugMessage("[HumanBodyAction] ACTION DONE DIRECT / source=" + source + " / preset=BonusKneeNudge");
    }

    void RequestHandFallbackKneeNudgeTest(string source)
    {
        if (!IsHbaEnabled())
        {
            hbaLastBlock = "Disabled: knee nudge test skipped";
            DebugMessage("[HumanBodyAction] Knee nudge test skipped because HBA Enable is OFF / source=" + source);
            UpdateHbaStatus(true);
            return;
        }

        StartCoroutine(DirectHandFallbackKneeNudgeTestRoutine(source));
    }

    IEnumerator DirectHandFallbackKneeNudgeTestRoutine(string source)
    {
        hbaLastBlock = "Knee nudge test direct";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] ACTION START DIRECT / source=" + source + " / preset=KneeNudgeTest / head=Off");
        yield return StartCoroutine(RandomHandFailKneeNudgeRoutine(source, "test-button"));
        DebugMessage("[HumanBodyAction] ACTION DONE DIRECT / source=" + source + " / preset=KneeNudgeTest");
    }

    IEnumerator RandomHandFailKneeNudgeRoutine(string source, string reason)
    {
        RefreshControllersNoReset();

        Atom moveAtom = containingAtom;
        if (moveAtom == null)
        {
            DebugMessage("[HumanBodyAction] Hand bonus knee local skipped / reason=" + reason + " / missing-move-atom" +
                " / source=" + source);
            yield break;
        }

        FreeControllerV3 moveLKnee = FindControllerByAliases("lKneeControl", "leftKneeControl", "lKnee", "leftKnee");
        FreeControllerV3 moveRKnee = FindControllerByAliases("rKneeControl", "rightKneeControl", "rKnee", "rightKnee");
        FreeControllerV3 moveLFoot = FindControllerByAliases("lFootControl", "leftFootControl", "lFoot", "leftFoot");
        FreeControllerV3 moveRFoot = FindControllerByAliases("rFootControl", "rightFootControl", "rFoot", "rightFoot");

        if (moveLKnee == null && moveRKnee == null)
        {
            DebugMessage("[HumanBodyAction] Hand bonus knee local skipped / reason=" + reason + " / missing-knee" +
                " / source=" + source +
                " / moveAtom=" + moveAtom.uid +
                " / moveLKnee=" + (moveLKnee != null ? "1" : "0") +
                " / moveRKnee=" + (moveRKnee != null ? "1" : "0"));
            yield break;
        }

        // Keep this fallback independent from the hand cover hold, but still restoreable by HBA_Cover_Restore/HBA_Reset.
        // v043: restart still cancels/restores the previous knee branch, but the new bonus snapshot is taken
        // only for the selected knee below. Capturing both knees/feet made unrelated knees look like they jumped.
        RestoreTargetKneeToSelfThighSnapshot("hand-bonus-knee-local-restart");
        int runSerial = ++targetKneeToSelfThighRunSerial;

        FreeControllerV3 moveKnee = null;
        string kneeLabel = "";
        int selectedSide = 0;
        string sideSelect = "fallback";
        if (moveLKnee != null && moveRKnee != null)
        {
            bool useLeft;
            float alternateRoll = UnityEngine.Random.Range(0.0f, 100.0f);
            if (handFallbackKneeLastSide == 0)
            {
                useLeft = UnityEngine.Random.Range(0, 2) == 0;
                sideSelect = "first-random";
            }
            else if (alternateRoll < 76.0f)
            {
                useLeft = handFallbackKneeLastSide > 0;
                sideSelect = "alternate";
            }
            else
            {
                useLeft = UnityEngine.Random.Range(0, 2) == 0;
                sideSelect = "random-break";
            }

            moveKnee = useLeft ? moveLKnee : moveRKnee;
            kneeLabel = useLeft ? "L Knee" : "R Knee";
            selectedSide = useLeft ? -1 : 1;
            handFallbackKneeLastSide = selectedSide;
        }
        else if (moveLKnee != null)
        {
            moveKnee = moveLKnee;
            kneeLabel = "L Knee";
            selectedSide = -1;
            handFallbackKneeLastSide = selectedSide;
            sideSelect = "only-left";
        }
        else
        {
            moveKnee = moveRKnee;
            kneeLabel = "R Knee";
            selectedSide = 1;
            handFallbackKneeLastSide = selectedSide;
            sideSelect = "only-right";
        }

        // v043: snapshot only the knee this bonus actually moves. Do not snapshot feet here.
        if (selectedSide < 0)
            CaptureTargetKneeToSelfThighSnapshot(moveAtom, moveKnee, null, null, null);
        else
            CaptureTargetKneeToSelfThighSnapshot(moveAtom, null, moveKnee, null, null);

        Vector3 startPos = GetControllerPosition(moveKnee);

        Vector3 localRight = moveAtom.transform != null ? Vector3.ProjectOnPlane(moveAtom.transform.right, Vector3.up) : Vector3.right;
        Vector3 localForward = moveAtom.transform != null ? Vector3.ProjectOnPlane(moveAtom.transform.forward, Vector3.up) : Vector3.forward;
        if (localRight.sqrMagnitude < 0.0001f) localRight = Vector3.right;
        if (localForward.sqrMagnitude < 0.0001f) localForward = Vector3.forward;
        localRight.Normalize();
        localForward.Normalize();

        Vector3 outward = selectedSide < 0 ? -localRight : localRight;
        Vector3 inward = -outward;
        float modeRoll = UnityEngine.Random.Range(0.0f, 100.0f);
        string guideMode;
        Vector3 dir;

        // v081: this is no longer a mini Knee->Thigh move. It is a body-space reaction only.
        // Most movements are outward/up or outward/forward. A few are mostly up or a small relax/back response.
        if (modeRoll < 45.0f)
        {
            guideMode = "local-out-up";
            dir = outward * UnityEngine.Random.Range(0.82f, 1.08f) + Vector3.up * UnityEngine.Random.Range(0.20f, 0.44f) + localForward * UnityEngine.Random.Range(-0.05f, 0.12f);
        }
        else if (modeRoll < 75.0f)
        {
            guideMode = "local-out-forward";
            dir = outward * UnityEngine.Random.Range(0.60f, 0.96f) + localForward * UnityEngine.Random.Range(0.22f, 0.52f) + Vector3.up * UnityEngine.Random.Range(0.05f, 0.24f);
        }
        else if (modeRoll < 90.0f)
        {
            guideMode = "local-up";
            dir = Vector3.up * UnityEngine.Random.Range(0.74f, 1.05f) + outward * UnityEngine.Random.Range(0.10f, 0.32f) + localForward * UnityEngine.Random.Range(-0.06f, 0.10f);
        }
        else
        {
            guideMode = "local-relax-back";
            dir = inward * UnityEngine.Random.Range(0.18f, 0.36f) - localForward * UnityEngine.Random.Range(0.18f, 0.42f) + Vector3.up * UnityEngine.Random.Range(0.06f, 0.20f);
        }

        if (dir.sqrMagnitude < 0.0001f)
            dir = outward;
        dir.Normalize();

        float nudgeAmount = UnityEngine.Random.Range(HandFailKneeNudgeAmountMin, HandFailKneeNudgeAmountMax);
        Vector3 peakPos = startPos + dir * nudgeAmount;

        Vector3 sideAxis = Vector3.Cross(Vector3.up, dir);
        if (sideAxis.sqrMagnitude < 0.0001f)
            sideAxis = outward;
        sideAxis.Normalize();

        float sideArc = UnityEngine.Random.Range(-HandFailKneeNudgeArcSideMax, HandFailKneeNudgeArcSideMax);
        float upArc = UnityEngine.Random.Range(HandFailKneeNudgeArcUpMin, HandFailKneeNudgeArcUpMax);
        Vector3 arcOffset = sideAxis * sideArc + Vector3.up * upArc;

        float preDelay = UnityEngine.Random.Range(0.04f, 0.16f);
        float moveDur = UnityEngine.Random.Range(HandFailKneeNudgeMoveSecondsMin, HandFailKneeNudgeMoveSecondsMax);
        float holdDur = UnityEngine.Random.Range(HandFailKneeNudgeHoldSecondsMin, HandFailKneeNudgeHoldSecondsMax);
        float settleBack = UnityEngine.Random.Range(HandFailKneeNudgeSettleBackMin, HandFailKneeNudgeSettleBackMax);
        float settleDur = UnityEngine.Random.Range(HandFailKneeNudgeSettleSecondsMin, HandFailKneeNudgeSettleSecondsMax);
        Vector3 settlePos = Vector3.Lerp(peakPos, startPos, settleBack);

        FreeControllerV3.PositionState originalKneePositionState = moveKnee.currentPositionState;
        FreeControllerV3.RotationState originalKneeRotationState = moveKnee.currentRotationState;
        Quaternion originalKneeRotation = GetControllerRotation(moveKnee);

        try { moveKnee.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { moveKnee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        hbaLastBlock = "Hand bonus knee local: " + kneeLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Hand bonus knee local start" +
            " / source=" + source +
            " / reason=" + reason +
            " / moveAtom=" + moveAtom.uid +
            " / knee=" + kneeLabel +
            " / sideSelect=" + sideSelect +
            " / guide=" + guideMode +
            " / start=" + V3(startPos) +
            " / peak=" + V3(peakPos) +
            " / settle=" + V3(settlePos) +
            " / amount=" + F3(nudgeAmount) +
            " / micro=1" +
            " / preDelay=" + F3(preDelay) +
            " / moveSeconds=" + F3(moveDur) +
            " / holdSeconds=" + F3(holdDur) +
            " / settleSeconds=" + F3(settleDur) +
            " / arc=" + V3(arcOffset) +
            " / footTouched=0" +
            " / noThighGuide=1" +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");

        float delayStart = Time.time;
        while (Time.time - delayStart < preDelay)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            SetControllerPosition(moveKnee, startPos);
            yield return null;
        }

        float startTime = Time.time;
        while (Time.time - startTime < moveDur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - startTime) / Mathf.Max(0.01f, moveDur));
            float s = Smooth01(t);
            float pulse = Mathf.Sin(s * Mathf.PI);
            float organic = s + pulse * HandFailKneeNudgeOvershoot * UnityEngine.Random.Range(0.55f, 1.00f);
            Vector3 pos = startPos + dir * (nudgeAmount * organic) + arcOffset * pulse;
            SetControllerPosition(moveKnee, pos);
            yield return null;
        }
        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        SetControllerPosition(moveKnee, peakPos);

        float settleStart = Time.time;
        while (Time.time - settleStart < settleDur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - settleStart) / Mathf.Max(0.01f, settleDur));
            float e = Smooth01(t);
            Vector3 pos = Vector3.Lerp(peakPos, settlePos, e) + arcOffset * 0.20f * Mathf.Sin(e * Mathf.PI);
            SetControllerPosition(moveKnee, pos);
            yield return null;
        }
        if (runSerial != targetKneeToSelfThighRunSerial) yield break;
        SetControllerPosition(moveKnee, settlePos);

        holdDur = Mathf.Max(0.01f, holdDur);
        float holdStart = Time.time;
        while (Time.time - holdStart < holdDur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            SetControllerPosition(moveKnee, settlePos);
            yield return null;
        }
        if (runSerial != targetKneeToSelfThighRunSerial) yield break;

        // v083: Comply -> Off caused a visible snap. For the hand-bonus knee reaction,
        // do not hard-drop the IK at the end. Ease the controller back to its start position,
        // then restore the knee controller's original state. This makes the nudge read as a
        // small body reaction instead of a forced IK release.
        float releaseBack = 1.0f;
        float releaseDur = UnityEngine.Random.Range(0.520f, 0.880f);
        Vector3 releasePos = startPos;

        float releaseStart = Time.time;
        while (Time.time - releaseStart < releaseDur)
        {
            if (runSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - releaseStart) / Mathf.Max(0.01f, releaseDur));
            float e = Smooth01(t);
            float breathe = Mathf.Sin(e * Mathf.PI);
            Vector3 pos = Vector3.Lerp(settlePos, releasePos, e) + arcOffset * 0.05f * breathe;
            SetControllerPosition(moveKnee, pos);
            SetControllerRotation(moveKnee, Quaternion.Slerp(GetControllerRotation(moveKnee), originalKneeRotation, e));
            yield return null;
        }
        if (runSerial != targetKneeToSelfThighRunSerial) yield break;

        SetControllerPosition(moveKnee, startPos);
        SetControllerRotation(moveKnee, originalKneeRotation);
        try { moveKnee.currentPositionState = originalKneePositionState; } catch { }
        try { moveKnee.currentRotationState = originalKneeRotationState; } catch { }

        hbaLastBlock = "Hand bonus knee local restored: " + kneeLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Hand bonus knee local restored" +
            " / source=" + source +
            " / reason=" + reason +
            " / moveAtom=" + moveAtom.uid +
            " / knee=" + kneeLabel +
            " / guide=" + guideMode +
            " / peak=" + V3(peakPos) +
            " / settle=" + V3(settlePos) +
            " / release=" + V3(releasePos) +
            " / releaseBack=" + F3(releaseBack) +
            " / releaseSeconds=" + F3(releaseDur) +
            " / positionState=restore-original:" + originalKneePositionState.ToString() +
            " / rotationState=restore-original:" + originalKneeRotationState.ToString() +
            " / footTouched=0" +
            " / noThighGuide=1" +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");
    }

    bool IsHandCoverFarTooFar(Vector3 startPosition, Vector3 requestedPosition, string targetLabel, bool kneeThighTestOnly, out float distance, out float threshold)
    {
        distance = Vector3.Distance(startPosition, requestedPosition);
        if (kneeThighTestOnly)
            threshold = HandCoverKneeThighTooFarDistance;
        else if (IsUpperHandCoverTargetLabel(targetLabel))
            threshold = HandCoverUpperTooFarDistance;
        else
            threshold = HandCoverTooFarDistance;
        return distance > threshold;
    }

    void SetHandCoverIkOffAfterFarReach(FreeControllerV3 hand, Quaternion lockedRotation, string source, string coverMode, string targetLabel, Vector3 requestedPosition, Vector3 reachedPosition, float requestedDistance, float threshold)
    {
        if (hand == null) return;

        SetControllerPosition(hand, reachedPosition);
        SetControllerRotation(hand, lockedRotation);
        try { hand.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
        try { hand.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        hbaLastBlock = coverMode + " reach-off: " + GetHandLabel(hand) + " -> " + targetLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Cover far reach IK off" +
            " / mode=" + coverMode +
            " / source=" + source +
            " / hand=" + GetHandLabel(hand) +
            " / target=" + targetLabel +
            " / requested=" + V3(requestedPosition) +
            " / reached=" + V3(reachedPosition) +
            " / requestDist=" + F3(requestedDistance) +
            " / tooFarLimit=" + F3(threshold) +
            " / positionState=Off / rotationState=Off" +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");
    }

    bool IsHandCoverRunStillCurrent(int runSerial)
    {
        return runSerial == handCoverRunSerial;
    }

    bool IsUpperHandCoverTargetLabel(string label)
    {
        if (string.IsNullOrEmpty(label)) return false;
        string n = label.ToLowerInvariant();
        return n.Contains("head") || n.Contains("neck") || n.Contains("chest");
    }

    Vector3 ClampHandCoverCommandPosition(Vector3 startPosition, Vector3 commandPosition, string coverMode, string targetLabel, bool kneeThighTestOnly)
    {
        float maxDistance = kneeThighTestOnly ? HandCoverKneeThighCommandMaxDistance : HandCoverCommandMaxDistance;
        if (!kneeThighTestOnly && IsUpperHandCoverTargetLabel(targetLabel))
        {
            maxDistance = HandCoverUpperCommandMaxDistance;
        }
        Vector3 delta = commandPosition - startPosition;
        float distance = delta.magnitude;
        if (distance <= maxDistance || distance < 0.0001f)
            return commandPosition;

        Vector3 clamped = startPosition + delta.normalized * maxDistance;
        DebugMessage("[HumanBodyAction] Cover command clamp / mode=" + coverMode + " / target=" + targetLabel + " / from=" + V3(startPosition) + " / requested=" + V3(commandPosition) + " / clamped=" + V3(clamped) + " / dist=" + F3(distance) + " / max=" + F3(maxDistance));
        return clamped;
    }

    IEnumerator MoveHandCoverPosition(FreeControllerV3 hand, Vector3 from, Vector3 to, float seconds, Quaternion lockedRotation, bool lockRotation)
    {
        if (hand == null) yield break;

        float dur = Mathf.Max(0.01f, seconds);
        float start = Time.time;
        while (Time.time - start < dur)
        {
            float t = Mathf.Clamp01((Time.time - start) / dur);
            float e = Smooth01(t);
            SetControllerPosition(hand, Vector3.Lerp(from, to, e));
            if (lockRotation) SetControllerRotation(hand, lockedRotation);
            yield return null;
        }
        SetControllerPosition(hand, to);
        if (lockRotation) SetControllerRotation(hand, lockedRotation);
    }

    IEnumerator MoveHandCoverPositionChestAvoid(FreeControllerV3 hand, Vector3 from, Vector3 to, float seconds, Quaternion lockedRotation, bool lockRotation, string targetLabel)
    {
        if (hand == null) yield break;

        Vector3 avoidArc;
        if (!TryBuildSelfFaceChestAvoidArc(hand, from, to, targetLabel, out avoidArc))
        {
            yield return StartCoroutine(MoveHandCoverPosition(hand, from, to, seconds, lockedRotation, lockRotation));
            yield break;
        }

        Vector3 delta = to - from;
        Vector3 c1 = from + delta * 0.28f + avoidArc;
        Vector3 c2 = to - delta * 0.22f + avoidArc * 0.62f;
        float dur = Mathf.Max(0.01f, seconds);
        float start = Time.time;
        DebugMessage("[HumanBodyAction] Cover chest avoid path / hand=" + GetHandLabel(hand) + " / target=" + targetLabel + " / arc=" + V3(avoidArc));

        while (Time.time - start < dur)
        {
            float t = Mathf.Clamp01((Time.time - start) / dur);
            float e = Smooth01(t);
            SetControllerPosition(hand, CubicBezier(from, c1, c2, to, e));
            if (lockRotation) SetControllerRotation(hand, lockedRotation);
            yield return null;
        }
        SetControllerPosition(hand, to);
        if (lockRotation) SetControllerRotation(hand, lockedRotation);
    }

    bool ShouldUseSelfFaceChestAvoidPath(FreeControllerV3 hand, Vector3 from, Vector3 to, string targetLabel)
    {
        Vector3 avoidArc;
        return TryBuildSelfFaceChestAvoidArc(hand, from, to, targetLabel, out avoidArc);
    }

    bool IsSelfFaceChestAvoidTargetLabel(string label)
    {
        if (string.IsNullOrEmpty(label)) return false;
        return string.Equals(label, "Head", StringComparison.OrdinalIgnoreCase)
            || string.Equals(label, "Neck", StringComparison.OrdinalIgnoreCase)
            || label.IndexOf("Head Side", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Self Head", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Self Face", StringComparison.OrdinalIgnoreCase) >= 0
            || label.IndexOf("Self Mouth", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    bool TryBuildSelfFaceChestAvoidArc(FreeControllerV3 hand, Vector3 from, Vector3 to, string targetLabel, out Vector3 avoidArc)
    {
        avoidArc = Vector3.zero;
        if (!IsSelfFaceChestAvoidTargetLabel(targetLabel)) return false;
        if (hand == null) return false;

        FreeControllerV3 chest = FindControllerByAliases("chestControl", "chest");
        if (chest == null) return false;

        Vector3 delta = to - from;
        if (delta.sqrMagnitude < 0.0025f) return false;

        Vector3 forward;
        Vector3 right;
        GetCoverBodyAxes(out forward, out right);
        forward.y = 0.0f;
        right.y = 0.0f;
        if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
        if (right.sqrMagnitude < 0.0001f) right = Vector3.right;
        forward.Normalize();
        right.Normalize();

        Vector3 chestPos = GetControllerPosition(chest);
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
                // Flat/small chest: keep the normal direct face-cover path.
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

        float along;
        float dist = DistancePointToSegment(chestProbe, from, to, out along);
        if (along < 0.08f || along > 0.92f) return false;
        if (dist > avoidRadius) return false;

        float closeness = Mathf.Clamp01((avoidRadius - dist) / Mathf.Max(0.001f, avoidRadius));
        float strength = breastMeasured
            ? Mathf.Lerp(0.55f, 1.20f, Mathf.Max(closeness, breastStrength))
            : Mathf.Lerp(0.45f, 0.80f, closeness);
        int side = GetHandSide(hand);
        if (side == 0) side = 1;
        avoidArc = right * side * SelfFaceChestAvoidSideOffset * strength
            + Vector3.up * SelfFaceChestAvoidUpOffset * strength
            + breastForward * SelfFaceChestAvoidForwardOffset * strength;

        DebugMessage("[HumanBodyAction] Cover chest avoid path"
            + " / hand=" + GetHandLabel(hand)
            + " / target=" + targetLabel
            + " / measured=" + (breastMeasured ? "1" : "0")
            + " / protrusion=" + F3(breastProtrusion)
            + " / halfWidth=" + F3(breastHalfWidth)
            + " / radius=" + F3(avoidRadius)
            + " / dist=" + F3(dist)
            + " / arc=" + V3(avoidArc));
        return avoidArc.sqrMagnitude > 0.0001f;
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

        FreeControllerV3 lNipple = FindControllerByAliases("lNippleControl", "leftNippleControl", "lNipple", "lnipple", "leftNipple", "LeftNipple", "nipple_l", "nippleL");
        FreeControllerV3 rNipple = FindControllerByAliases("rNippleControl", "rightNippleControl", "rNipple", "rnipple", "rightNipple", "RightNipple", "nipple_r", "nippleR");
        if (lNipple == null || rNipple == null) return false;

        left = GetControllerPosition(lNipple);
        right = GetControllerPosition(rNipple);
        return true;
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

    Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        float u = 1.0f - t;
        return u * u * u * a + 3.0f * u * u * t * b + 3.0f * u * t * t * c + t * t * t * d;
    }


    IEnumerator HoldHandCoverPosition(FreeControllerV3 hand, Vector3 position, float seconds, Quaternion lockedRotation, bool lockRotation)
    {
        if (hand == null) yield break;

        float dur = Mathf.Max(0.01f, seconds);
        float start = Time.time;
        while (Time.time - start < dur)
        {
            SetControllerPosition(hand, position);
            if (lockRotation) SetControllerRotation(hand, lockedRotation);
            yield return null;
        }
        SetControllerPosition(hand, position);
        if (lockRotation) SetControllerRotation(hand, lockedRotation);
    }

    Vector3 SoftSnapHandCoverPosition(FreeControllerV3 hand, Vector3 commandedPosition, Quaternion lockedRotation, string coverMode, string targetLabel)
    {
        if (hand == null) return commandedPosition;

        Vector3 bodyHandPosition;
        string bodyHandName;
        if (!TryGetBodyTransformPositionForController(hand, out bodyHandPosition, out bodyHandName))
        {
            DebugMessage("[HumanBodyAction] Cover soft snap skip / reason=no-body-transform / hand=" + GetHandLabel(hand) + " / mode=" + coverMode + " / target=" + targetLabel);
            return commandedPosition;
        }

        float distance = Vector3.Distance(commandedPosition, bodyHandPosition);
        if (distance < HandCoverSoftSnapMinDistance)
        {
            DebugMessage("[HumanBodyAction] Cover soft snap skip / reason=near / hand=" + GetHandLabel(hand) + " / mode=" + coverMode + " / target=" + targetLabel + " / dist=" + F3(distance));
            return commandedPosition;
        }

        Vector3 snappedPosition = bodyHandPosition;
        bool clamped = distance > HandCoverSoftSnapMaxDistance;
        // v048: For cover actions the snap is intended to stop the command from pushing through the body.
        // Holding the actual body-hand position is safer than holding a point still biased toward an unreachable command.
        // The max value is kept only as a diagnostic threshold in the log.

        SetControllerPosition(hand, snappedPosition);
        SetControllerRotation(hand, lockedRotation);

        DebugMessage("[HumanBodyAction] Cover soft snap / hand=" + GetHandLabel(hand) + " / body=" + bodyHandName + " / mode=" + coverMode + " / target=" + targetLabel + " / commanded=" + V3(commandedPosition) + " / bodyPos=" + V3(bodyHandPosition) + " / hold=" + V3(snappedPosition) + " / dist=" + F3(distance) + " / max=" + F3(HandCoverSoftSnapMaxDistance) + " / clamped=" + (clamped ? "1" : "0"));
        return snappedPosition;
    }

    bool TryGetBodyTransformPositionForController(FreeControllerV3 controller, out Vector3 position, out string transformName)
    {
        position = Vector3.zero;
        transformName = "";

        if (controller == null || containingAtom == null || string.IsNullOrEmpty(controller.name))
            return false;

        string keyword = controller.name.Replace("Control", "").Replace("control", "").ToLowerInvariant();
        if (string.IsNullOrEmpty(keyword))
            return false;

        Transform[] transforms = containingAtom.GetComponentsInChildren<Transform>(false);
        if (transforms == null)
            return false;

        Transform best = null;
        float bestDistance = float.MaxValue;
        Vector3 current = GetControllerPosition(controller);

        for (int i = 0; i < transforms.Length; i++)
        {
            Transform t = transforms[i];
            if (t == null || string.IsNullOrEmpty(t.name))
                continue;

            string n = t.name.ToLowerInvariant();
            if (n.Contains("control"))
                continue;
            if (!n.Contains(keyword))
                continue;

            float d = Vector3.Distance(current, t.position);
            if (d < bestDistance)
            {
                bestDistance = d;
                best = t;
            }
        }

        if (best == null)
            return false;

        position = best.position;
        transformName = best.name;
        return true;
    }

    bool IsHandFreeForCover(FreeControllerV3 hand)
    {
        // v077: Hand IK PositionState.On is allowed, but a TargetGrabber-held L/R target hand is not.
        // If TG holds only L Hand, R Hand remains selectable; if TG holds only R Hand, L Hand remains selectable.
        return hand != null && !IsHandBlockedByTargetGrabberHeldTargetHand(hand);
    }

    void CaptureHandCoverSnapshot(FreeControllerV3 hand)
    {
        if (hand == null) return;
        handCoverAutoRestoreArmed = HasHandCoverAutoRestoreSignal();
        activeHandCoverSnapshot = new HandCoverSnapshot();
        activeHandCoverSnapshot.hand = hand;
        activeHandCoverSnapshot.position = GetControllerPosition(hand);
        activeHandCoverSnapshot.rotation = GetControllerRotation(hand);
        activeHandCoverSnapshot.positionState = hand.currentPositionState;
        activeHandCoverSnapshot.rotationState = hand.currentRotationState;

        FreeControllerV3 elbow = FindMatchingElbowForHandCover(hand);
        if (elbow != null)
        {
            activeHandCoverSnapshot.elbow = elbow;
            activeHandCoverSnapshot.elbowPosition = GetControllerPosition(elbow);
            activeHandCoverSnapshot.elbowRotation = GetControllerRotation(elbow);
            activeHandCoverSnapshot.elbowPositionState = elbow.currentPositionState;
            activeHandCoverSnapshot.elbowRotationState = elbow.currentRotationState;
        }
    }

    void RequestHandCoverRestore(string reason, bool animate)
    {
        handCoverAutoRestoreArmed = false;
        handCoverRunSerial++;

        if (directHandCoverRoutine != null)
        {
            StopCoroutine(directHandCoverRoutine);
            directHandCoverRoutine = null;
        }
        if (handCoverRestoreRoutine != null)
        {
            StopCoroutine(handCoverRestoreRoutine);
            handCoverRestoreRoutine = null;
        }

        handCoverRestoreRoutine = StartCoroutine(RestoreHandCoverSnapshotRoutine(reason, animate));
    }

    IEnumerator RestoreHandCoverSnapshotRoutine(string reason, bool animate)
    {
        if (activeHandCoverSnapshot == null)
            yield break;

        HandCoverSnapshot snap = activeHandCoverSnapshot;
        activeHandCoverSnapshot = null;
        handCoverAutoRestoreArmed = false;

        if (!animate || IsCriticalHandCoverRestoreReason(reason))
        {
            RestoreHandCoverSnapshotImmediate(snap, reason);
            yield break;
        }

        yield return StartCoroutine(AnimateHandCoverRestore(snap, reason));
    }

    bool IsCriticalHandCoverRestoreReason(string reason)
    {
        if (string.IsNullOrEmpty(reason)) return false;
        string r = reason.ToLowerInvariant();
        return r.Contains("reset") || r.Contains("destroy") || r.Contains("disable");
    }

    void RestoreHandCoverSnapshot(string reason)
    {
        if (activeHandCoverSnapshot == null) return;

        HandCoverSnapshot snap = activeHandCoverSnapshot;
        activeHandCoverSnapshot = null;
        handCoverAutoRestoreArmed = false;
        RestoreHandCoverSnapshotImmediate(snap, reason);
    }

    void RestoreHandCoverSnapshotImmediate(HandCoverSnapshot snap, string reason)
    {
        if (snap == null) return;

        if (snap.hand != null)
        {
            SetControllerPosition(snap.hand, snap.position);
            SetControllerRotation(snap.hand, snap.rotation);
            try { snap.hand.currentPositionState = snap.positionState; } catch { }
            try { snap.hand.currentRotationState = snap.rotationState; } catch { }
        }

        if (snap.elbow != null)
        {
            SetControllerPosition(snap.elbow, snap.elbowPosition);
            SetControllerRotation(snap.elbow, snap.elbowRotation);
            try { snap.elbow.currentPositionState = snap.elbowPositionState; } catch { }
            try { snap.elbow.currentRotationState = snap.elbowRotationState; } catch { }
        }

        DebugMessage("[HumanBodyAction] Cover restore / reason=" + reason + " / hand=" + GetHandLabel(snap.hand) + " / elbow=" + GetControllerLabel(snap.elbow));
    }

    IEnumerator AnimateHandCoverRestore(HandCoverSnapshot snap, string reason)
    {
        if (snap == null)
            yield break;

        FreeControllerV3 hand = snap.hand;
        FreeControllerV3 elbow = snap.elbow;
        if (hand == null)
        {
            RestoreHandCoverSnapshotImmediate(snap, reason + ":no-hand");
            yield break;
        }

        Vector3 handFrom = GetControllerPosition(hand);
        Quaternion handRotFrom = GetControllerRotation(hand);
        Vector3 elbowFrom = elbow != null ? GetControllerPosition(elbow) : Vector3.zero;
        Quaternion elbowRotFrom = elbow != null ? GetControllerRotation(elbow) : Quaternion.identity;

        try { hand.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { hand.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        if (elbow != null)
        {
            try { elbow.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
            try { elbow.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }

        DebugMessage("[HumanBodyAction] Cover restore slow start / reason=" + reason +
            " / hand=" + GetHandLabel(hand) +
            " / from=" + V3(handFrom) +
            " / to=" + V3(snap.position) +
            " / seconds=" + F2(HandCoverReturnSeconds) +
            " / linger=" + F2(HandCoverReturnLingerSeconds));

        float linger = Mathf.Max(0.0f, HandCoverReturnLingerSeconds);
        float lingerStart = Time.time;
        while (Time.time - lingerStart < linger)
        {
            SetControllerPosition(hand, handFrom);
            SetControllerRotation(hand, handRotFrom);
            if (elbow != null)
            {
                SetControllerPosition(elbow, elbowFrom);
                SetControllerRotation(elbow, elbowRotFrom);
            }
            yield return null;
        }

        float dur = Mathf.Max(0.01f, HandCoverReturnSeconds);
        float start = Time.time;
        while (Time.time - start < dur)
        {
            float t = Mathf.Clamp01((Time.time - start) / dur);
            float e = Smooth01(t);
            SetControllerPosition(hand, Vector3.Lerp(handFrom, snap.position, e));
            SetControllerRotation(hand, Quaternion.Slerp(handRotFrom, snap.rotation, e));
            if (elbow != null)
            {
                SetControllerPosition(elbow, Vector3.Lerp(elbowFrom, snap.elbowPosition, e));
                SetControllerRotation(elbow, Quaternion.Slerp(elbowRotFrom, snap.elbowRotation, e));
            }
            yield return null;
        }

        RestoreHandCoverSnapshotImmediate(snap, reason + ":slow-done");
        handCoverRestoreRoutine = null;
    }



    void CaptureTargetKneeToSelfThighSnapshot(Atom targetAtom, FreeControllerV3 lKnee, FreeControllerV3 rKnee, FreeControllerV3 lFoot, FreeControllerV3 rFoot)
    {
        activeTargetKneeToSelfThighSnapshot = new TargetKneeToSelfThighSnapshot();
        activeTargetKneeToSelfThighSnapshot.targetAtom = targetAtom;
        activeTargetKneeToSelfThighSnapshot.lKnee = lKnee;
        activeTargetKneeToSelfThighSnapshot.rKnee = rKnee;
        activeTargetKneeToSelfThighSnapshot.lFoot = lFoot;
        activeTargetKneeToSelfThighSnapshot.rFoot = rFoot;

        if (lKnee != null)
        {
            activeTargetKneeToSelfThighSnapshot.lKneePosition = GetControllerPosition(lKnee);
            activeTargetKneeToSelfThighSnapshot.lKneeRotation = GetControllerRotation(lKnee);
            activeTargetKneeToSelfThighSnapshot.lKneePositionState = lKnee.currentPositionState;
            activeTargetKneeToSelfThighSnapshot.lKneeRotationState = lKnee.currentRotationState;
        }

        if (rKnee != null)
        {
            activeTargetKneeToSelfThighSnapshot.rKneePosition = GetControllerPosition(rKnee);
            activeTargetKneeToSelfThighSnapshot.rKneeRotation = GetControllerRotation(rKnee);
            activeTargetKneeToSelfThighSnapshot.rKneePositionState = rKnee.currentPositionState;
            activeTargetKneeToSelfThighSnapshot.rKneeRotationState = rKnee.currentRotationState;
        }

        if (lFoot != null)
        {
            activeTargetKneeToSelfThighSnapshot.lFootPosition = GetControllerPosition(lFoot);
            activeTargetKneeToSelfThighSnapshot.lFootRotation = GetControllerRotation(lFoot);
            activeTargetKneeToSelfThighSnapshot.lFootPositionState = lFoot.currentPositionState;
            activeTargetKneeToSelfThighSnapshot.lFootRotationState = lFoot.currentRotationState;
        }

        if (rFoot != null)
        {
            activeTargetKneeToSelfThighSnapshot.rFootPosition = GetControllerPosition(rFoot);
            activeTargetKneeToSelfThighSnapshot.rFootRotation = GetControllerRotation(rFoot);
            activeTargetKneeToSelfThighSnapshot.rFootPositionState = rFoot.currentPositionState;
            activeTargetKneeToSelfThighSnapshot.rFootRotationState = rFoot.currentRotationState;
        }

        DebugMessage("[HumanBodyAction] Random knee branch snapshot" +
            " / moveAtom=" + (targetAtom != null ? targetAtom.uid : "<none>") +
            " / lKneeSnap=" + (lKnee != null ? "1" : "0") +
            " / rKneeSnap=" + (rKnee != null ? "1" : "0") +
            " / lFootSnap=" + (lFoot != null ? "1" : "0") +
            " / rFootSnap=" + (rFoot != null ? "1" : "0"));
    }

    void RestoreTargetKneeToSelfThighSnapshot(string reason)
    {
        if (activeTargetKneeToSelfThighSnapshot == null) return;

        TargetKneeToSelfThighSnapshot snap = activeTargetKneeToSelfThighSnapshot;
        activeTargetKneeToSelfThighSnapshot = null;
        targetKneeToSelfThighRunSerial++;

        if (targetKneeRestoreRoutine != null)
        {
            StopCoroutine(targetKneeRestoreRoutine);
            targetKneeRestoreRoutine = null;
        }

        if (ShouldSmoothRestoreTargetKneeSnapshot(reason))
        {
            int restoreSerial = targetKneeToSelfThighRunSerial;
            targetKneeRestoreRoutine = StartCoroutine(RestoreTargetKneeToSelfThighSnapshotSmoothRoutine(snap, reason, restoreSerial));
            return;
        }

        RestoreTargetKneeToSelfThighSnapshotImmediate(snap, reason);
    }

    bool ShouldSmoothRestoreTargetKneeSnapshot(string reason)
    {
        if (string.IsNullOrEmpty(reason)) return false;
        return reason.Contains("HBA_Cover_Restore") || reason.Contains("HBR_Cover_Restore");
    }

    void RestoreTargetKneeToSelfThighSnapshotImmediate(TargetKneeToSelfThighSnapshot snap, string reason)
    {
        if (snap == null) return;

        if (snap.lKnee != null)
        {
            SetControllerPosition(snap.lKnee, snap.lKneePosition);
            SetControllerRotation(snap.lKnee, snap.lKneeRotation);
            try { snap.lKnee.currentPositionState = snap.lKneePositionState; } catch { }
            try { snap.lKnee.currentRotationState = snap.lKneeRotationState; } catch { }
        }

        if (snap.rKnee != null)
        {
            SetControllerPosition(snap.rKnee, snap.rKneePosition);
            SetControllerRotation(snap.rKnee, snap.rKneeRotation);
            try { snap.rKnee.currentPositionState = snap.rKneePositionState; } catch { }
            try { snap.rKnee.currentRotationState = snap.rKneeRotationState; } catch { }
        }

        // Foot IK is only temporarily relaxed while a knee is travelling. Do not snap foot transforms here.
        if (snap.lFoot != null)
        {
            try { snap.lFoot.currentPositionState = snap.lFootPositionState; } catch { }
            try { snap.lFoot.currentRotationState = snap.lFootRotationState; } catch { }
        }

        if (snap.rFoot != null)
        {
            try { snap.rFoot.currentPositionState = snap.rFootPositionState; } catch { }
            try { snap.rFoot.currentRotationState = snap.rFootRotationState; } catch { }
        }

        DebugMessage("[HumanBodyAction] Random knee branch restore immediate / reason=" + reason + " / moveAtom=" + (snap.targetAtom != null ? snap.targetAtom.uid : "<none>") +
            " / lFootStateOnly=" + (snap.lFoot != null ? "restore" : "none") +
            " / rFootStateOnly=" + (snap.rFoot != null ? "restore" : "none"));
    }

    IEnumerator RestoreTargetKneeToSelfThighSnapshotSmoothRoutine(TargetKneeToSelfThighSnapshot snap, string reason, int restoreSerial)
    {
        if (snap == null)
            yield break;

        Vector3 lStart = snap.lKnee != null ? GetControllerPosition(snap.lKnee) : Vector3.zero;
        Vector3 rStart = snap.rKnee != null ? GetControllerPosition(snap.rKnee) : Vector3.zero;
        Quaternion lStartRot = snap.lKnee != null ? GetControllerRotation(snap.lKnee) : Quaternion.identity;
        Quaternion rStartRot = snap.rKnee != null ? GetControllerRotation(snap.rKnee) : Quaternion.identity;

        if (snap.lKnee != null)
        {
            try { snap.lKnee.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
            try { snap.lKnee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }
        if (snap.rKnee != null)
        {
            try { snap.rKnee.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
            try { snap.rKnee.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
        }

        // Restore foot IK states only; never move foot transforms as part of a knee return.
        if (snap.lFoot != null)
        {
            try { snap.lFoot.currentPositionState = snap.lFootPositionState; } catch { }
            try { snap.lFoot.currentRotationState = snap.lFootRotationState; } catch { }
        }
        if (snap.rFoot != null)
        {
            try { snap.rFoot.currentPositionState = snap.rFootPositionState; } catch { }
            try { snap.rFoot.currentRotationState = snap.rFootRotationState; } catch { }
        }

        float returnDur = UnityEngine.Random.Range(RandomKneeReactionReturnSecondsMin, RandomKneeReactionReturnSecondsMax);
        returnDur = Mathf.Max(0.35f, returnDur);
        DebugMessage("[HumanBodyAction] Random knee branch restore smooth start" +
            " / reason=" + reason +
            " / moveAtom=" + (snap.targetAtom != null ? snap.targetAtom.uid : "<none>") +
            " / returnSeconds=" + F3(returnDur) +
            " / footRestore=state-only");

        float startTime = Time.time;
        while (Time.time - startTime < returnDur)
        {
            if (restoreSerial != targetKneeToSelfThighRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - startTime) / returnDur);
            float e = Smooth01(Smooth01(t));
            if (snap.lKnee != null)
            {
                SetControllerPosition(snap.lKnee, Vector3.Lerp(lStart, snap.lKneePosition, e));
                SetControllerRotation(snap.lKnee, Quaternion.Slerp(lStartRot, snap.lKneeRotation, e));
            }
            if (snap.rKnee != null)
            {
                SetControllerPosition(snap.rKnee, Vector3.Lerp(rStart, snap.rKneePosition, e));
                SetControllerRotation(snap.rKnee, Quaternion.Slerp(rStartRot, snap.rKneeRotation, e));
            }
            yield return null;
        }

        if (restoreSerial != targetKneeToSelfThighRunSerial) yield break;
        if (snap.lKnee != null)
        {
            SetControllerPosition(snap.lKnee, snap.lKneePosition);
            SetControllerRotation(snap.lKnee, snap.lKneeRotation);
        }
        if (snap.rKnee != null)
        {
            SetControllerPosition(snap.rKnee, snap.rKneePosition);
            SetControllerRotation(snap.rKnee, snap.rKneeRotation);
        }

        yield return new WaitForSeconds(RandomKneeReactionRestoreStabilizeSeconds);
        if (restoreSerial != targetKneeToSelfThighRunSerial) yield break;

        if (snap.lKnee != null)
        {
            try { snap.lKnee.currentPositionState = snap.lKneePositionState; } catch { }
            try { snap.lKnee.currentRotationState = snap.lKneeRotationState; } catch { }
        }
        if (snap.rKnee != null)
        {
            try { snap.rKnee.currentPositionState = snap.rKneePositionState; } catch { }
            try { snap.rKnee.currentRotationState = snap.rKneeRotationState; } catch { }
        }

        DebugMessage("[HumanBodyAction] Random knee branch restore smooth done" +
            " / reason=" + reason +
            " / moveAtom=" + (snap.targetAtom != null ? snap.targetAtom.uid : "<none>") +
            " / lKneeState=" + (snap.lKnee != null ? snap.lKneePositionState.ToString() : "none") +
            " / rKneeState=" + (snap.rKnee != null ? snap.rKneePositionState.ToString() : "none"));
        targetKneeRestoreRoutine = null;
    }


    FreeControllerV3 FindMatchingElbowForHandCover(FreeControllerV3 hand)
    {
        if (hand == null || string.IsNullOrEmpty(hand.name)) return null;
        string n = hand.name.ToLowerInvariant();
        if (n.Contains("lhand") || n.Contains("lefthand"))
            return FindControllerByAliases("lElbowControl", "leftElbowControl", "lElbow", "leftElbow");
        if (n.Contains("rhand") || n.Contains("righthand"))
            return FindControllerByAliases("rElbowControl", "rightElbowControl", "rElbow", "rightElbow");
        return null;
    }

    void RelaxHandCoverElbow(FreeControllerV3 hand)
    {
        FreeControllerV3 elbow = activeHandCoverSnapshot != null ? activeHandCoverSnapshot.elbow : FindMatchingElbowForHandCover(hand);
        if (elbow == null) return;
        try { elbow.currentPositionState = FreeControllerV3.PositionState.Comply; } catch { }
        try { elbow.currentRotationState = FreeControllerV3.RotationState.Comply; } catch { }
        DebugMessage("[HumanBodyAction] Cover elbow comply / hand=" + GetHandLabel(hand) + " / elbow=" + GetControllerLabel(elbow));
    }

    string GetControllerLabel(FreeControllerV3 fc)
    {
        return fc != null && !string.IsNullOrEmpty(fc.name) ? fc.name : "<none>";
    }

    string GetPositionStateLabel(FreeControllerV3 fc)
    {
        if (fc == null) return "<none>";
        try { return fc.currentPositionState.ToString(); }
        catch { return "<error>"; }
    }

    bool TrySelectRandomHandCoverTarget(string source, bool kneeThighTestOnly, bool forcePushAwayOnly, List<FreeControllerV3> freeHands, out FreeControllerV3 selectedHand, out HandCoverTarget selectedTarget, out string selectedCoverMode, out int targetCandidateCount)
    {
        selectedHand = null;
        selectedTarget = null;
        selectedCoverMode = kneeThighTestOnly ? "SelfThighTest" : forcePushAwayOnly ? "PushAway" : "Cover";
        targetCandidateCount = 0;

        if (freeHands == null || freeHands.Count == 0) return false;

        if (kneeThighTestOnly)
        {
            return TrySelectPushAwayHandTarget(source, "SelfThighTest", freeHands, true, out selectedHand, out selectedTarget, out selectedCoverMode, out targetCandidateCount);
        }

        if (forcePushAwayOnly)
        {
            bool ok = TrySelectPushAwayHandTarget(source, "PushAway", freeHands, false, out selectedHand, out selectedTarget, out selectedCoverMode, out targetCandidateCount);
            if (!ok) CoverHandLogMessage("[HumanBodyAction] PushAway force skipped / reason=no reachable PushAway target / person=" + GetPushAwayPersonLabelForLog() + " / reach=" + F3(GetEffectivePushAwayReach()));
            return ok;
        }

        string scope = GetHandCoverScope();
        if (scope == HandCoverScopeSelf)
        {
            return TrySelectPushAwayHandTarget(source, "PushAway", freeHands, false, out selectedHand, out selectedTarget, out selectedCoverMode, out targetCandidateCount);
        }

        if (scope == HandCoverScopeTarget)
        {
            return TrySelectCoverHandTarget(source, freeHands, out selectedHand, out selectedTarget, out selectedCoverMode, out targetCandidateCount);
        }

        float mix = handCoverPushAwayMix != null ? handCoverPushAwayMix.val : HandCoverPushAwayMixDefault;
        mix = Mathf.Clamp(mix, HandCoverPushAwayMixMin, HandCoverPushAwayMixMax);

        bool preferPushAway = false;
        if (mix >= 99.999f)
        {
            preferPushAway = true;
        }
        else if (mix > 0.001f)
        {
            float roll = UnityEngine.Random.Range(0.0f, 100.0f);
            preferPushAway = roll <= mix;
            CoverHandLogMessage("[HumanBodyAction] Cover mix roll / scope=All / roll=" + F1(roll) + " / mix=" + F1(mix) + "% / preferPushAway=" + (preferPushAway ? "1" : "0") + " / person=" + GetPushAwayPersonLabelForLog());
        }

        if (preferPushAway)
        {
            if (TrySelectPushAwayHandTarget(source, "PushAway", freeHands, false, out selectedHand, out selectedTarget, out selectedCoverMode, out targetCandidateCount))
                return true;
            CoverHandLogMessage("[HumanBodyAction] PushAway fallback to Cover / reason=no reachable PushAway target / person=" + GetPushAwayPersonLabelForLog() + " / reach=" + F3(GetEffectivePushAwayReach()));
            return TrySelectCoverHandTarget(source, freeHands, out selectedHand, out selectedTarget, out selectedCoverMode, out targetCandidateCount);
        }

        if (TrySelectCoverHandTarget(source, freeHands, out selectedHand, out selectedTarget, out selectedCoverMode, out targetCandidateCount))
            return true;
        return TrySelectPushAwayHandTarget(source, "PushAway", freeHands, false, out selectedHand, out selectedTarget, out selectedCoverMode, out targetCandidateCount);
    }

    bool TrySelectCoverHandTarget(string source, List<FreeControllerV3> freeHands, out FreeControllerV3 selectedHand, out HandCoverTarget selectedTarget, out string selectedCoverMode, out int targetCandidateCount)
    {
        selectedHand = null;
        selectedTarget = null;
        selectedCoverMode = "Cover";
        targetCandidateCount = 0;

        List<HandCoverTarget> allTargets = BuildHandCoverTargets();
        List<HandCoverTarget> compatibleTargets = new List<HandCoverTarget>();
        for (int i = 0; i < allTargets.Count; i++)
        {
            HandCoverTarget t = allTargets[i];
            if (HasCompatibleHandForCoverTarget(freeHands, t)) compatibleTargets.Add(t);
        }

        Atom behindOtherAtom;
        float behindDot;
        int upperBehindRemoved = FilterTargetUpperCoverWhenOtherPersonBehind(compatibleTargets, out behindOtherAtom, out behindDot);
        if (upperBehindRemoved > 0)
        {
            CoverHandLogMessage("[HumanBodyAction] Target upper cover blocked / build=v089 / reason=other-person-behind-self" +
                " / blocked=" + upperBehindRemoved +
                " / other=" + SafeAtomUid(behindOtherAtom) +
                " / backDot=" + F3(behindDot) +
                " / source=" + source);
        }

        targetCandidateCount = compatibleTargets.Count;
        if (compatibleTargets.Count == 0)
        {
            DebugMessage("[HumanBodyAction] Cover target filter / source=" + source + " / all=" + allTargets.Count + " / compatible=0 / upperBehindRemoved=" + upperBehindRemoved + " / behindOther=" + SafeAtomUid(behindOtherAtom) + " / backDot=" + F3(behindDot) + " / lFree=" + (HasHandSide(freeHands, -1) ? "1" : "0") + " / rFree=" + (HasHandSide(freeHands, 1) ? "1" : "0"));
            return false;
        }

        selectedTarget = compatibleTargets[UnityEngine.Random.Range(0, compatibleTargets.Count)];
        selectedHand = SelectHandForCoverTarget(freeHands, selectedTarget);
        if (selectedHand == null) return false;
        selectedCoverMode = selectedTarget.pushAway ? "PushAway" : "Cover";
        return true;
    }

    bool TrySelectPushAwayHandTarget(string source, string modeLabel, List<FreeControllerV3> freeHands, bool selfThighTestOnly, out FreeControllerV3 selectedHand, out HandCoverTarget selectedTarget, out string selectedCoverMode, out int targetCandidateCount)
    {
        selectedHand = null;
        selectedTarget = null;
        selectedCoverMode = modeLabel;
        targetCandidateCount = 0;

        List<FreeControllerV3> hands = new List<FreeControllerV3>(freeHands);
        while (hands.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, hands.Count);
            FreeControllerV3 hand = hands[index];
            hands.RemoveAt(index);

            Vector3 handStart = GetControllerPosition(hand);
            List<HandCoverTarget> targets = selfThighTestOnly
                ? BuildHandCoverSelfThighTestTargets(handStart)
                : BuildHandCoverPushAwayTargets(handStart);

            if (targets.Count == 0)
            {
                CoverHandLogMessage("[HumanBodyAction] PushAway candidates empty / hand=" + GetHandLabel(hand) + " / person=" + GetPushAwayPersonLabelForLog() + " / reach=" + F3(GetEffectivePushAwayReach()));
                continue;
            }

            selectedHand = hand;
            selectedTarget = targets[UnityEngine.Random.Range(0, targets.Count)];
            selectedCoverMode = modeLabel;
            targetCandidateCount = targets.Count;
            return true;
        }

        return false;
    }

    const float TargetUpperBehindBlockDot = -0.65f;

    int FilterTargetUpperCoverWhenOtherPersonBehind(List<HandCoverTarget> targets, out Atom otherAtom, out float backDot)
    {
        otherAtom = null;
        backDot = 1.0f;
        if (targets == null || targets.Count == 0) return 0;

        if (!IsOtherPersonBehindSelfForCover(out otherAtom, out backDot))
            return 0;

        int removed = 0;
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            HandCoverTarget t = targets[i];
            if (t == null) continue;
            if (!IsTargetUpperCoverBlockedWhenOtherBehind(t.label)) continue;
            targets.RemoveAt(i);
            removed++;
        }
        return removed;
    }

    bool IsOtherPersonBehindSelfForCover(out Atom otherAtom, out float backDot)
    {
        otherAtom = null;
        backDot = 1.0f;
        if (containingAtom == null) return false;

        otherAtom = ResolveHandCoverPushAwayAtom();
        if (otherAtom == null || otherAtom == containingAtom) return false;

        Vector3 selfPos = Vector3.zero;
        bool hasSelfPos = false;
        FreeControllerV3 selfHip = FindControllerByAliases("hipControl", "hip");
        FreeControllerV3 selfChest = FindControllerByAliases("chestControl", "chest");
        if (selfHip != null)
        {
            selfPos = GetControllerPosition(selfHip);
            hasSelfPos = true;
        }
        else if (selfChest != null)
        {
            selfPos = GetControllerPosition(selfChest);
            hasSelfPos = true;
        }
        else if (containingAtom.transform != null)
        {
            selfPos = containingAtom.transform.position;
            hasSelfPos = true;
        }
        if (!hasSelfPos) return false;

        Vector3 otherPos = Vector3.zero;
        bool hasOtherPos = false;
        FreeControllerV3 otherHip = FindControllerByAliasesOnAtom(otherAtom, "hipControl", "hip");
        FreeControllerV3 otherChest = FindControllerByAliasesOnAtom(otherAtom, "chestControl", "chest");
        if (otherHip != null)
        {
            otherPos = GetControllerPosition(otherHip);
            hasOtherPos = true;
        }
        else if (otherChest != null)
        {
            otherPos = GetControllerPosition(otherChest);
            hasOtherPos = true;
        }
        else if (otherAtom.transform != null)
        {
            otherPos = otherAtom.transform.position;
            hasOtherPos = true;
        }
        if (!hasOtherPos) return false;

        Vector3 toOther = otherPos - selfPos;
        toOther.y = 0.0f;
        if (toOther.sqrMagnitude < 0.0001f) return false;
        toOther.Normalize();

        Vector3 forward;
        Vector3 right;
        GetCoverBodyAxes(out forward, out right);
        forward.y = 0.0f;
        if (forward.sqrMagnitude < 0.0001f)
            forward = containingAtom.transform != null ? containingAtom.transform.forward : Vector3.forward;
        forward.y = 0.0f;
        if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
        forward.Normalize();

        backDot = Vector3.Dot(forward, toOther);
        return backDot <= TargetUpperBehindBlockDot;
    }

    bool IsTargetUpperCoverBlockedWhenOtherBehind(string label)
    {
        if (string.IsNullOrEmpty(label)) return false;
        string n = label.ToLowerInvariant();
        return n.Contains("head") ||
            n.Contains("neck") ||
            n.Contains("chest") ||
            n.Contains("nipple");
    }

    bool HasCompatibleHandForCoverTarget(List<FreeControllerV3> hands, HandCoverTarget target)
    {
        if (target == null) return false;
        if (target.requiredHandSide == 0) return hands != null && hands.Count > 0;
        return HasHandSide(hands, target.requiredHandSide);
    }

    bool HasHandSide(List<FreeControllerV3> hands, int side)
    {
        if (hands == null) return false;
        for (int i = 0; i < hands.Count; i++)
        {
            if (GetHandSide(hands[i]) == side) return true;
        }
        return false;
    }

    FreeControllerV3 SelectHandForCoverTarget(List<FreeControllerV3> hands, HandCoverTarget target)
    {
        if (hands == null || hands.Count == 0 || target == null) return null;

        if (target.requiredHandSide == 0)
            return hands[UnityEngine.Random.Range(0, hands.Count)];

        List<FreeControllerV3> candidates = new List<FreeControllerV3>();
        for (int i = 0; i < hands.Count; i++)
        {
            if (GetHandSide(hands[i]) == target.requiredHandSide) candidates.Add(hands[i]);
        }
        if (candidates.Count == 0) return null;
        return candidates[UnityEngine.Random.Range(0, candidates.Count)];
    }

    int GetHandSide(FreeControllerV3 hand)
    {
        if (hand == null || hand.name == null) return 0;
        string n = hand.name.ToLowerInvariant();
        if (n.Contains("lhand")) return -1;
        if (n.Contains("rhand")) return 1;
        return 0;
    }

    string GetRequiredHandSideLabel(int side)
    {
        if (side < 0) return "L";
        if (side > 0) return "R";
        return "Any";
    }

    string GetHandCoverTargetDisplayLabel(HandCoverTarget target)
    {
        if (target == null) return "<none>";
        if (target.requiredHandSide < 0) return target.label + " (L)";
        if (target.requiredHandSide > 0) return target.label + " (R)";
        return target.label;
    }

    List<HandCoverTarget> BuildHandCoverSelfThighTestTargets(Vector3 handStartPosition)
    {
        List<HandCoverTarget> targets = new List<HandCoverTarget>();

        Vector3 forward;
        Vector3 right;
        GetCoverBodyAxes(out forward, out right);

        // Test-only route: keep the main random Cover scope untouched and only sample from
        // reachable Self-side L/R Thigh push-away points. Target L/R Knee was intentionally removed.

        FreeControllerV3 hip = FindControllerByAliases("hipControl", "hip");
        FreeControllerV3 chest = FindControllerByAliases("chestControl", "chest");
        Vector3 bodyCenter = hip != null ? GetControllerPosition(hip) : chest != null ? GetControllerPosition(chest) : (containingAtom != null && containingAtom.transform != null ? containingAtom.transform.position : Vector3.zero);

        AddControlPushAwayTarget(targets, "Self L Thigh", FindControllerByAliases("lThighControl", "leftThighControl", "lThigh", "leftThigh"), handStartPosition, bodyCenter);
        AddControlPushAwayTarget(targets, "Self R Thigh", FindControllerByAliases("rThighControl", "rightThighControl", "rThigh", "rightThigh"), handStartPosition, bodyCenter);

        DebugMessage("[HumanBodyAction] Cover self thigh test targets / count=" + targets.Count);
        return targets;
    }

    List<HandCoverTarget> BuildMixedHandCoverTargets(Vector3 handStartPosition)
    {
        List<HandCoverTarget> coverTargets = BuildHandCoverTargets();
        List<HandCoverTarget> pushAwayTargets = BuildHandCoverPushAwayTargets(handStartPosition);
        string scope = GetHandCoverScope();

        if (scope == HandCoverScopeSelf)
        {
            DebugMessage("[HumanBodyAction] Cover scope / scope=Self / pushTargets=" + pushAwayTargets.Count);
            return pushAwayTargets;
        }

        if (scope == HandCoverScopeTarget)
        {
            DebugMessage("[HumanBodyAction] Cover scope / scope=Target / coverTargets=" + coverTargets.Count);
            return coverTargets;
        }

        float mix = handCoverPushAwayMix != null ? handCoverPushAwayMix.val : HandCoverPushAwayMixDefault;
        mix = Mathf.Clamp(mix, HandCoverPushAwayMixMin, HandCoverPushAwayMixMax);

        bool preferPushAway = false;
        if (mix >= 99.999f)
        {
            preferPushAway = true;
        }
        else if (mix > 0.001f)
        {
            float roll = UnityEngine.Random.Range(0.0f, 100.0f);
            preferPushAway = roll <= mix;
            DebugMessage("[HumanBodyAction] Cover mix roll / scope=All / roll=" + F1(roll) + " / mix=" + F1(mix) + "% / pushTargets=" + pushAwayTargets.Count + " / coverTargets=" + coverTargets.Count);
        }

        if (preferPushAway && pushAwayTargets.Count > 0)
            return pushAwayTargets;

        if (!preferPushAway && coverTargets.Count > 0)
            return coverTargets;

        // All mode fallback: if the selected branch has no valid targets, use the other branch instead.
        if (pushAwayTargets.Count > 0)
            return pushAwayTargets;

        return coverTargets;
    }

    string GetHandCoverScope()
    {
        if (handCoverScope == null || string.IsNullOrEmpty(handCoverScope.val)) return HandCoverScopeDefault;
        if (handCoverScope.val == HandCoverScopeSelf) return HandCoverScopeSelf;
        if (handCoverScope.val == HandCoverScopeTarget) return HandCoverScopeTarget;
        return HandCoverScopeAll;
    }

    List<HandCoverTarget> BuildHandCoverPushAwayTargets(Vector3 handStartPosition)
    {
        List<HandCoverTarget> targets = new List<HandCoverTarget>();

        Atom pushAwayAtom = ResolveHandCoverPushAwayAtom();
        if (pushAwayAtom == null)
        {
            CoverHandLogMessage("[HumanBodyAction] PushAway target missing / chooser=" + (handCoverPushAwayPerson != null ? handCoverPushAwayPerson.val : "<null>") + " / reason=no-other-person");
            return targets;
        }

        Vector3 forward;
        Vector3 right;
        GetCoverBodyAxesOnAtom(pushAwayAtom, out forward, out right);

        FreeControllerV3 hip = FindControllerByAliasesOnAtom(pushAwayAtom, "hipControl", "hip");
        FreeControllerV3 chest = FindControllerByAliasesOnAtom(pushAwayAtom, "chestControl", "chest");
        FreeControllerV3 head = FindControllerByAliasesOnAtom(pushAwayAtom, "headControl", "head");
        Vector3 bodyCenter = hip != null ? GetControllerPosition(hip) : chest != null ? GetControllerPosition(chest) : (pushAwayAtom.transform != null ? pushAwayAtom.transform.position : Vector3.zero);

        // v065: PushAway no longer means Self. It uses the selected non-self Person by default.
        // The moving hand remains the Person that owns this HumanBodyAction, but the target anchors
        // are on PushAway Person: Head/Chest/Hip/nearest reachable Thigh. All obey PushAway Reach.
        // v069: Weight PushAway Head as 3 slots so the selected other-Person head is not buried by
        // Chest/Hip/Thigh when PushAway finally wins the mix roll.
        AddControlPushAwayTarget(targets, "PushAway Head", head, handStartPosition, bodyCenter);
        AddControlPushAwayTarget(targets, "PushAway Head", head, handStartPosition, bodyCenter);
        AddControlPushAwayTarget(targets, "PushAway Head", head, handStartPosition, bodyCenter);
        AddControlPushAwayTarget(targets, "PushAway Chest", chest, handStartPosition, bodyCenter);
        AddControlPushAwayTarget(targets, "PushAway Hip", hip, handStartPosition, bodyCenter);
        AddNearestPushAwayPersonThighTarget(targets, pushAwayAtom, handStartPosition, bodyCenter);

        CoverHandLogMessage("[HumanBodyAction] PushAway person targets / atom=" + SafeAtomUid(pushAwayAtom) + " / targets=" + targets.Count + " / effectiveReach=" + F3(GetEffectivePushAwayReachForLog()));
        return targets;
    }

    void AddNearestPushAwayPersonThighTarget(List<HandCoverTarget> targets, Atom pushAwayAtom, Vector3 handStartPosition, Vector3 bodyCenter)
    {
        if (targets == null || pushAwayAtom == null) return;

        FreeControllerV3 lThigh = FindControllerByAliasesOnAtom(pushAwayAtom, "lThighControl", "leftThighControl", "lThigh", "leftThigh");
        FreeControllerV3 rThigh = FindControllerByAliasesOnAtom(pushAwayAtom, "rThighControl", "rightThighControl", "rThigh", "rightThigh");

        if (lThigh == null && rThigh == null) return;

        FreeControllerV3 first = null;
        string firstLabel = "";
        FreeControllerV3 second = null;
        string secondLabel = "";

        if (lThigh != null && rThigh != null)
        {
            float lDist = Vector3.Distance(handStartPosition, GetControllerPosition(lThigh));
            float rDist = Vector3.Distance(handStartPosition, GetControllerPosition(rThigh));
            if (lDist <= rDist)
            {
                first = lThigh;
                firstLabel = "PushAway L Thigh";
                second = rThigh;
                secondLabel = "PushAway R Thigh";
            }
            else
            {
                first = rThigh;
                firstLabel = "PushAway R Thigh";
                second = lThigh;
                secondLabel = "PushAway L Thigh";
            }
        }
        else if (lThigh != null)
        {
            first = lThigh;
            firstLabel = "PushAway L Thigh";
        }
        else
        {
            first = rThigh;
            firstLabel = "PushAway R Thigh";
        }

        int before = targets.Count;
        AddControlPushAwayTarget(targets, firstLabel, first, handStartPosition, bodyCenter);
        if (targets.Count > before) return;

        if (second != null)
        {
            AddControlPushAwayTarget(targets, secondLabel, second, handStartPosition, bodyCenter);
        }
    }

    void AddSelfNipplePushAwayTargets(List<HandCoverTarget> targets, FreeControllerV3 chest, Vector3 forward, Vector3 right, Vector3 handStartPosition, Vector3 bodyCenter)
    {
        if (targets == null) return;

        FreeControllerV3 lNipple = FindControllerByAliases("lNippleControl", "leftNippleControl", "lNipple", "leftNipple");
        FreeControllerV3 rNipple = FindControllerByAliases("rNippleControl", "rightNippleControl", "rNipple", "rightNipple");

        Vector3 outDir = forward;
        if (outDir.sqrMagnitude < 0.0001f) outDir = Vector3.forward;
        outDir.Normalize();

        if (lNipple != null || rNipple != null)
        {
            if (lNipple != null) AddPushAwayTargetNoReachLimit(targets, "Self L Nipple", GetControllerPosition(lNipple), outDir, handStartPosition, bodyCenter);
            if (rNipple != null) AddPushAwayTargetNoReachLimit(targets, "Self R Nipple", GetControllerPosition(rNipple), outDir, handStartPosition, bodyCenter);
            return;
        }

        if (chest == null) return;

        Vector3 chestPos = GetControllerPosition(chest);
        Vector3 rightDir = right;
        if (rightDir.sqrMagnitude < 0.0001f) rightDir = Vector3.right;
        rightDir.Normalize();

        // Fallback estimate when explicit nipple controls are not available.
        // Keep this as PushAway/no-reach-limit like the old Self Chest candidate.
        Vector3 up = Vector3.up;
        const float sideOffset = 0.115f;
        const float forwardOffset = 0.055f;
        const float downOffset = 0.045f;
        AddPushAwayTargetNoReachLimit(targets, "Self L Nipple", chestPos - rightDir * sideOffset + outDir * forwardOffset - up * downOffset, outDir, handStartPosition, bodyCenter);
        AddPushAwayTargetNoReachLimit(targets, "Self R Nipple", chestPos + rightDir * sideOffset + outDir * forwardOffset - up * downOffset, outDir, handStartPosition, bodyCenter);
    }

    void AddControlPushAwayTarget(List<HandCoverTarget> targets, string label, FreeControllerV3 control, Vector3 handStartPosition, Vector3 bodyCenter)
    {
        if (control == null) return;
        AddPushAwayTargetIfReachable(targets, label, GetControllerPosition(control), Vector3.zero, handStartPosition, bodyCenter);
    }

    void AddControlPushAwayTargetAlways(List<HandCoverTarget> targets, string label, FreeControllerV3 control, Vector3 handStartPosition, Vector3 bodyCenter)
    {
        if (control == null) return;
        AddPushAwayTargetNoReachLimit(targets, label, GetControllerPosition(control), Vector3.zero, handStartPosition, bodyCenter);
    }

    void AddPushAwayTargetNoReachLimit(List<HandCoverTarget> targets, string label, Vector3 position, Vector3 preferredOutward, Vector3 handStartPosition, Vector3 bodyCenter)
    {
        if (targets == null) return;

        Vector3 outward = preferredOutward;
        if (outward.sqrMagnitude < 0.0001f)
        {
            outward = position - bodyCenter;
            outward.y *= 0.35f;
        }
        if (outward.sqrMagnitude < 0.0001f)
        {
            Vector3 forward;
            Vector3 right;
            GetCoverBodyAxes(out forward, out right);
            outward = forward;
        }
        if (outward.sqrMagnitude < 0.0001f) outward = Vector3.forward;
        outward.Normalize();

        float distance = Vector3.Distance(handStartPosition, position);
        DebugMessage("[HumanBodyAction] PushAway include upper / target=" + label + " / dist=" + F3(distance) + " / reachFilter=off / commandClamp=on");
        targets.Add(new HandCoverTarget(label, position, outward, true));
    }

    float GetEffectivePushAwayReach()
    {
        float sliderReach = handCoverPushAwayReach != null ? handCoverPushAwayReach.val : HandCoverPushAwayReachDefault;
        // v069: Old scenes can keep the former 0.58 value after the script update. For PushAway Person
        // targets this made Person#2 Head/Chest/Hip unreachable and silently fell back to normal Cover.
        // Treat the 1.20 default as the effective minimum while still respecting the expanded max range.
        return Mathf.Clamp(Mathf.Max(sliderReach, HandCoverPushAwayReachDefault), HandCoverPushAwayReachMin, HandCoverPushAwayReachMax);
    }

    float GetEffectivePushAwayReachForLog()
    {
        return GetEffectivePushAwayReach();
    }

    void AddPushAwayTargetIfReachable(List<HandCoverTarget> targets, string label, Vector3 position, Vector3 preferredOutward, Vector3 handStartPosition, Vector3 bodyCenter)
    {
        if (targets == null) return;

        float reach = GetEffectivePushAwayReach();
        float distance = Vector3.Distance(handStartPosition, position);
        Vector3 commandPosition = position;
        bool stretchOnly = false;

        // v070: PushAway Person targets should still produce a useful motion even when the selected
        // Person#2 anchor is beyond the reach filter. Instead of dropping the target and falling back
        // to normal Cover, keep the same direction and place the command at a reachable stretch point.
        bool backLimited = false;
        float backComponent = 0.0f;
        float backLimit = HandCoverPushAwayBackStretchLimit;

        if (distance > reach)
        {
            Vector3 toTarget = position - handStartPosition;
            if (toTarget.sqrMagnitude < 0.0001f)
            {
                toTarget = position - bodyCenter;
                toTarget.y *= 0.35f;
            }
            if (toTarget.sqrMagnitude < 0.0001f) toTarget = Vector3.forward;
            toTarget.Normalize();

            float stretchDistance = GetPushAwayStretchDistanceForLabel(label, reach);
            commandPosition = handStartPosition + toTarget * stretchDistance;
            stretchOnly = true;
            Vector3 beforeBackLimit = commandPosition;
            backLimited = TryLimitPushAwayBackStretch(label, handStartPosition, ref commandPosition, out backComponent, out backLimit);
            CoverHandLogMessage("[HumanBodyAction] PushAway stretch to reach / target=" + label +
                " / dist=" + F3(distance) +
                " / reach=" + F3(reach) +
                " / stretch=" + F3(stretchDistance) +
                " / backLimited=" + (backLimited ? "1" : "0") +
                " / back=" + F3(backComponent) +
                " / backLimit=" + F3(backLimit) +
                " / before=" + V3(beforeBackLimit) +
                " / after=" + V3(commandPosition) +
                " / person=" + GetPushAwayPersonLabelForLog());
        }

        Vector3 outward = preferredOutward;
        if (outward.sqrMagnitude < 0.0001f)
        {
            outward = position - bodyCenter;
            outward.y *= 0.35f;
        }
        if (outward.sqrMagnitude < 0.0001f)
        {
            Vector3 forward;
            Vector3 right;
            GetCoverBodyAxes(out forward, out right);
            outward = forward;
        }
        if (outward.sqrMagnitude < 0.0001f) outward = Vector3.forward;
        outward.Normalize();

        string finalLabel = label;
        if (stretchOnly) finalLabel += " Reach";
        if (backLimited) finalLabel += " BackLimit";
        targets.Add(new HandCoverTarget(finalLabel, commandPosition, outward, true));
    }

    bool TryLimitPushAwayBackStretch(string label, Vector3 handStartPosition, ref Vector3 commandPosition, out float backComponent, out float backLimit)
    {
        backComponent = 0.0f;
        backLimit = HandCoverPushAwayBackStretchLimit;

        Vector3 forward;
        Vector3 right;
        GetCoverBodyAxes(out forward, out right);

        Vector3 back = -forward;
        back.y = 0.0f;
        if (back.sqrMagnitude < 0.0001f) return false;
        back.Normalize();

        Vector3 delta = commandPosition - handStartPosition;
        Vector3 flatDelta = new Vector3(delta.x, 0.0f, delta.z);
        if (flatDelta.sqrMagnitude < 0.0001f) return false;

        backComponent = Vector3.Dot(flatDelta, back);
        if (backComponent <= backLimit) return false;

        commandPosition -= back * (backComponent - backLimit);
        return true;
    }

    bool IsPushAwayBackLimitedTargetLabel(string label)
    {
        return !string.IsNullOrEmpty(label) && label.IndexOf("BackLimit", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    float GetPushAwayStretchDistanceForLabel(string label, float reach)
    {
        float commandMax = HandCoverCommandMaxDistance;
        if (IsUpperHandCoverTargetLabel(label)) commandMax = HandCoverUpperCommandMaxDistance;
        float usableReach = Mathf.Clamp(reach, HandCoverPushAwayReachMin, HandCoverPushAwayReachMax);
        float distance = Mathf.Min(usableReach, commandMax);
        // Keep a tiny bit of room for the PushAway outward offset that is added later, so far targets
        // do not immediately trip the far-too-far IK-Off path before the normal command clamp runs.
        float offset = Mathf.Clamp(handCoverPushAwayOffset != null ? handCoverPushAwayOffset.val : HandCoverPushAwayOffsetDefault, HandCoverPushAwayOffsetMin, HandCoverPushAwayOffsetMax);
        distance = Mathf.Max(0.05f, distance - offset * 0.50f);
        return distance;
    }


    List<HandCoverTarget> BuildHandCoverTargets()
    {
        List<HandCoverTarget> targets = new List<HandCoverTarget>();

        Vector3 forward;
        Vector3 right;
        GetCoverBodyAxes(out forward, out right);
        Vector3 up = Vector3.up;

        // v068: Keep upper-body targets visible in random selection again.
        // Lower-body targets are numerous and Hip Back is weighted, so give Head/Neck/Chest
        // one extra entry each without changing labels or UI.
        FreeControllerV3 head = FindControllerByAliases("headControl", "head");
        FreeControllerV3 chest = FindControllerByAliases("chestControl", "chest");
        FreeControllerV3 hip = FindControllerByAliases("hipControl", "hip");

        AddControlCoverTarget(targets, "Head", head, forward);
        AddControlCoverTarget(targets, "Head", head, forward);
        AddControlCoverTarget(targets, "Head", head, forward);
        AddHeadSideCoverTargets(targets, head, right, up);
        AddControlCoverTarget(targets, "Neck", FindControllerByAliases("neckControl", "neck"), forward);
        AddControlCoverTarget(targets, "Neck", FindControllerByAliases("neckControl", "neck"), forward);
        AddTargetNippleCoverTargets(targets, chest, forward, right, up);
        AddBellyAndGCoverTargets(targets, hip, chest, forward, up);

        if (hip != null)
        {
            Vector3 back = -forward;
            if (back.sqrMagnitude < 0.0001f) back = Vector3.back;
            back.Normalize();
            HandCoverTarget hipBackTarget = new HandCoverTarget("Hip Back", GetControllerPosition(hip) + back * HandCoverHipBackOffset + up * 0.035f, back);
            int hipBackWeight = Mathf.Max(1, HandCoverHipBackWeight);
            for (int i = 0; i < hipBackWeight; i++)
            {
                targets.Add(hipBackTarget);
            }
        }

        AddControlCoverTarget(targets, "L Thigh", FindControllerByAliases("lThighControl", "leftThighControl", "lThigh", "leftThigh"), forward, -1);
        AddControlCoverTarget(targets, "R Thigh", FindControllerByAliases("rThighControl", "rightThighControl", "rThigh", "rightThigh"), forward, 1);
        AddControlCoverTarget(targets, "L Knee", FindControllerByAliases("lKneeControl", "leftKneeControl", "lKnee", "leftKnee"), forward);
        AddControlCoverTarget(targets, "R Knee", FindControllerByAliases("rKneeControl", "rightKneeControl", "rKnee", "rightKnee"), forward);

        return targets;
    }

    void AddControlCoverTarget(List<HandCoverTarget> targets, string label, FreeControllerV3 control, Vector3 outward)
    {
        AddControlCoverTarget(targets, label, control, outward, 0);
    }

    void AddControlCoverTarget(List<HandCoverTarget> targets, string label, FreeControllerV3 control, Vector3 outward, int requiredHandSide)
    {
        if (targets == null || control == null) return;
        targets.Add(new HandCoverTarget(label, GetControllerPosition(control), outward, false, requiredHandSide));
    }


    void AddPointCoverTarget(List<HandCoverTarget> targets, string label, Vector3 position, Vector3 outward)
    {
        AddPointCoverTarget(targets, label, position, outward, 0);
    }

    void AddPointCoverTarget(List<HandCoverTarget> targets, string label, Vector3 position, Vector3 outward, int requiredHandSide)
    {
        if (targets == null) return;
        if (outward.sqrMagnitude < 0.0001f) outward = Vector3.forward;
        outward.Normalize();
        targets.Add(new HandCoverTarget(label, position, outward, false, requiredHandSide));
    }

    void AddTargetNippleCoverTargets(List<HandCoverTarget> targets, FreeControllerV3 chest, Vector3 forward, Vector3 right, Vector3 up)
    {
        if (targets == null) return;

        Vector3 outDir = forward;
        if (outDir.sqrMagnitude < 0.0001f) outDir = Vector3.forward;
        outDir.Normalize();

        FreeControllerV3 lNipple = FindControllerByAliases("lNippleControl", "leftNippleControl", "lNipple", "leftNipple");
        FreeControllerV3 rNipple = FindControllerByAliases("rNippleControl", "rightNippleControl", "rNipple", "rightNipple");
        if (lNipple != null || rNipple != null)
        {
            if (lNipple != null) AddControlCoverTarget(targets, "L Nipple", lNipple, outDir);
            if (rNipple != null) AddControlCoverTarget(targets, "R Nipple", rNipple, outDir);
            return;
        }

        if (chest == null) return;

        Vector3 chestPos = GetControllerPosition(chest);
        Vector3 rightDir = right;
        if (rightDir.sqrMagnitude < 0.0001f) rightDir = Vector3.right;
        rightDir.Normalize();

        const float sideOffset = 0.115f;
        const float forwardOffset = 0.055f;
        const float downOffset = 0.045f;
        AddPointCoverTarget(targets, "L Nipple", chestPos - rightDir * sideOffset + outDir * forwardOffset - up * downOffset, outDir);
        AddPointCoverTarget(targets, "R Nipple", chestPos + rightDir * sideOffset + outDir * forwardOffset - up * downOffset, outDir);
    }

    void AddBellyAndGCoverTargets(List<HandCoverTarget> targets, FreeControllerV3 hip, FreeControllerV3 chest, Vector3 forward, Vector3 up)
    {
        if (targets == null || hip == null) return;

        Vector3 hipPos = GetControllerPosition(hip);
        Vector3 chestPos = chest != null ? GetControllerPosition(chest) : hipPos + up * 0.42f;
        Vector3 outDir = forward;
        if (outDir.sqrMagnitude < 0.0001f) outDir = Vector3.forward;
        outDir.Normalize();

        // Computed cover points: no new UI/settings. Keep the existing position-only cover behavior.
        // Belly is placed between hip and chest, slightly toward the body front.
        Vector3 belly = Vector3.Lerp(hipPos, chestPos, 0.42f) + outDir * 0.045f;
        AddPointCoverTarget(targets, "Belly", belly, outDir);

        // G has no stable FreeControllerV3 on many scenes, so use a hip-based local estimate.
        // This keeps RandomHandCover independent from TargetLinePerson/trigger setup.
        Vector3 gPoint = hipPos + outDir * 0.095f - up * 0.105f;
        AddPointCoverTarget(targets, "G", gPoint, outDir);
    }

    void AddHeadSideCoverTargets(List<HandCoverTarget> targets, FreeControllerV3 head, Vector3 right, Vector3 up)
    {
        if (targets == null || head == null) return;

        Vector3 headPos = GetControllerPosition(head);
        Vector3 rightDir = right;
        if (rightDir.sqrMagnitude < 0.0001f) rightDir = Vector3.right;
        rightDir.Normalize();

        const float sideOffset = 0.180f;
        const float downOffset = 0.100f;
        AddPointCoverTarget(targets, "Head Side L", headPos - rightDir * sideOffset - up * downOffset, -rightDir, -1);
        AddPointCoverTarget(targets, "Head Side R", headPos + rightDir * sideOffset - up * downOffset, rightDir, 1);
    }


    void RefreshHandCoverPushAwayPersonChoices(bool keepCurrent)
    {
        string current = keepCurrent && handCoverPushAwayPerson != null ? handCoverPushAwayPerson.val : "";
        handCoverPushAwayPersonChoices.Clear();
        handCoverPushAwayPersonChoices.Add(HandCoverPushAwayPersonAuto);

        Atom nearestOther = FindNearestOtherPersonAtom();
        string nearestUid = nearestOther != null ? nearestOther.uid : "";

        if (SuperController.singleton != null)
        {
            List<Atom> atoms = SuperController.singleton.GetAtoms();
            if (atoms != null)
            {
                for (int i = 0; i < atoms.Count; i++)
                {
                    Atom atom = atoms[i];
                    if (atom == null || atom == containingAtom || atom.type != "Person" || string.IsNullOrEmpty(atom.uid)) continue;
                    if (!handCoverPushAwayPersonChoices.Contains(atom.uid)) handCoverPushAwayPersonChoices.Add(atom.uid);
                }
            }
        }

        if (handCoverPushAwayPersonChoices.Count > 2)
        {
            string auto = handCoverPushAwayPersonChoices[0];
            handCoverPushAwayPersonChoices.RemoveAt(0);
            handCoverPushAwayPersonChoices.Sort(StringComparer.OrdinalIgnoreCase);
            handCoverPushAwayPersonChoices.Insert(0, auto);
        }

        if (handCoverPushAwayPerson != null)
        {
            handCoverPushAwayPerson.choices = new List<string>(handCoverPushAwayPersonChoices);
            if (!string.IsNullOrEmpty(current) && handCoverPushAwayPersonChoices.Contains(current))
            {
                handCoverPushAwayPerson.val = current;
            }
            else if (!string.IsNullOrEmpty(nearestUid) && handCoverPushAwayPersonChoices.Contains(nearestUid))
            {
                handCoverPushAwayPerson.val = nearestUid;
            }
            else
            {
                handCoverPushAwayPerson.val = HandCoverPushAwayPersonAuto;
            }
        }
    }

    string GetDefaultHandCoverPushAwayPersonChoice()
    {
        Atom nearestOther = FindNearestOtherPersonAtom();
        if (nearestOther != null && !string.IsNullOrEmpty(nearestOther.uid)) return nearestOther.uid;
        return HandCoverPushAwayPersonAuto;
    }

    Atom ResolveHandCoverPushAwayAtom()
    {
        if (handCoverPushAwayPerson != null)
        {
            string uid = handCoverPushAwayPerson.val;
            if (!string.IsNullOrEmpty(uid) && uid != HandCoverPushAwayPersonAuto && SuperController.singleton != null)
            {
                Atom selected = SuperController.singleton.GetAtomByUid(uid);
                if (selected != null && selected != containingAtom && selected.type == "Person") return selected;
            }
        }
        return FindNearestOtherPersonAtom();
    }

    string SafeAtomUid(Atom atom)
    {
        if (atom == null) return "<none>";
        return string.IsNullOrEmpty(atom.uid) ? "<no-uid>" : atom.uid;
    }

    void GetCoverBodyAxesOnAtom(Atom atom, out Vector3 forward, out Vector3 right)
    {
        Quaternion rot = Quaternion.identity;
        FreeControllerV3 chest = FindControllerByAliasesOnAtom(atom, "chestControl", "chest");
        FreeControllerV3 hip = FindControllerByAliasesOnAtom(atom, "hipControl", "hip");
        if (chest != null) rot = GetControllerRotation(chest);
        else if (hip != null) rot = GetControllerRotation(hip);
        else if (atom != null && atom.transform != null) rot = atom.transform.rotation;

        forward = rot * Vector3.forward;
        forward.y = 0.0f;
        if (forward.sqrMagnitude < 0.0001f)
        {
            forward = atom != null && atom.transform != null ? atom.transform.forward : Vector3.forward;
            forward.y = 0.0f;
        }
        if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
        forward.Normalize();

        right = rot * Vector3.right;
        right.y = 0.0f;
        if (right.sqrMagnitude < 0.0001f) right = Vector3.Cross(Vector3.up, forward);
        if (right.sqrMagnitude < 0.0001f) right = Vector3.right;
        right.Normalize();
    }

    void GetCoverBodyAxes(out Vector3 forward, out Vector3 right)
    {
        Quaternion rot = Quaternion.identity;
        FreeControllerV3 chest = FindControllerByAliases("chestControl", "chest");
        FreeControllerV3 hip = FindControllerByAliases("hipControl", "hip");
        if (chest != null) rot = GetControllerRotation(chest);
        else if (hip != null) rot = GetControllerRotation(hip);
        else if (containingAtom != null && containingAtom.transform != null) rot = containingAtom.transform.rotation;

        forward = rot * Vector3.forward;
        forward.y = 0.0f;
        if (forward.sqrMagnitude < 0.0001f)
        {
            forward = containingAtom != null && containingAtom.transform != null ? containingAtom.transform.forward : Vector3.forward;
            forward.y = 0.0f;
        }
        if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
        forward.Normalize();

        right = rot * Vector3.right;
        right.y = 0.0f;
        if (right.sqrMagnitude < 0.0001f) right = Vector3.Cross(Vector3.up, forward);
        if (right.sqrMagnitude < 0.0001f) right = Vector3.right;
        right.Normalize();
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

    FreeControllerV3 FindControllerByAliases(params string[] aliases)
    {
        if (aliases == null) return null;

        for (int i = 0; i < aliases.Length; i++)
        {
            FreeControllerV3 found = FindControllerExactOnly(containingAtom, aliases[i]);
            if (found != null) return found;
        }

        for (int i = 0; i < aliases.Length; i++)
        {
            FreeControllerV3 found = FindControllerContains(containingAtom, aliases[i]);
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

    string GetHandLabel(FreeControllerV3 hand)
    {
        if (hand == null || hand.name == null) return "<none>";
        string n = hand.name.ToLowerInvariant();
        if (n.Contains("lhand")) return "L Hand";
        if (n.Contains("rhand")) return "R Hand";
        return hand.name;
    }

    string V3(Vector3 value)
    {
        return "(" + value.x.ToString("F3", CultureInfo.InvariantCulture) + "," + value.y.ToString("F3", CultureInfo.InvariantCulture) + "," + value.z.ToString("F3", CultureInfo.InvariantCulture) + ")";
    }

    string F1(float value)
    {
        return value.ToString("F1", CultureInfo.InvariantCulture);
    }

    string F2(float value)
    {
        return value.ToString("F2", CultureInfo.InvariantCulture);
    }

    string F3(float value)
    {
        return value.ToString("F3", CultureInfo.InvariantCulture);
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
        if (p.label == "L Hand" || p.label == "R Hand")
        {
            if (IsTwitchHandPartBlockedByTargetGrabber(p))
            {
                DebugMessage("[HumanBodyAction] Hand twitch skipped / part=" + p.label + " / reason=tg-held-target-hand-side");
                return false;
            }
            return twitchHands != null && twitchHands.val;
        }
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
        handCoverAutoRestoreArmed = false;
        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
            actionRoutine = null;
        }
        if (directHandCoverRoutine != null)
        {
            StopCoroutine(directHandCoverRoutine);
            directHandCoverRoutine = null;
        }
        if (directRandomKneeRoutine != null)
        {
            StopCoroutine(directRandomKneeRoutine);
            directRandomKneeRoutine = null;
        }
        if (gParallelChestHeadRoutine != null)
        {
            StopCoroutine(gParallelChestHeadRoutine);
            gParallelChestHeadRoutine = null;
            RestoreGParallelChestHeadSnapshots("reset:" + reason);
        }
        if (gProgressChestHeadActive || activeGProgressChestSnapshot != null || activeGProgressHeadSnapshot != null)
        {
            StopGProgressChestHeadFollow("reset:" + reason);
        }
        if (targetKneeRestoreRoutine != null)
        {
            StopCoroutine(targetKneeRestoreRoutine);
            targetKneeRestoreRoutine = null;
        }
        pendingRequest = null;
        suppressEventHandCoverUntil = -999.0f;
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

        RestoreHandCoverSnapshot("reset:" + reason);
        RestoreTargetKneeToSelfThighSnapshot("reset:" + reason);
        RestoreHandBonusElbowSnapshot("reset:" + reason);
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



    IEnumerator MaybeRandomHandElbowNudgeRoutine(string source, string reason)
    {
        bool forceManualRandomHandBonus = IsManualRandomHandElbowNudgeBonus(source, reason);
        float roll = forceManualRandomHandBonus ? 0.0f : UnityEngine.Random.Range(0.0f, 100.0f);
        if (!forceManualRandomHandBonus && roll > HandBonusElbowNudgeChance)
        {
            DebugMessage("[HumanBodyAction] Hand bonus elbow nudge chance skipped" +
                " / source=" + source +
                " / reason=" + reason +
                " / roll=" + F1(roll) +
                " / chance=" + F1(HandBonusElbowNudgeChance) + "%");
            yield break;
        }

        DebugMessage("[HumanBodyAction] Hand bonus elbow nudge chance hit" +
            " / source=" + source +
            " / reason=" + reason +
            " / roll=" + (forceManualRandomHandBonus ? "force" : F1(roll)) +
            " / chance=" + F1(HandBonusElbowNudgeChance) + "%" +
            " / force=" + (forceManualRandomHandBonus ? "1" : "0"));
        yield return StartCoroutine(RandomHandBonusElbowNudgeRoutine(source, reason));
    }

    bool IsManualRandomHandElbowNudgeBonus(string source, string reason)
    {
        if (reason != "random-hand-bonus")
            return false;
        if (string.IsNullOrEmpty(source))
            return false;
        return source.IndexOf("button:HBA_Cover_RandomHand", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    void RequestHandBonusElbowNudgeTest(string source)
    {
        if (!IsHbaEnabled())
        {
            hbaLastBlock = "Disabled: elbow nudge test skipped";
            DebugMessage("[HumanBodyAction] Elbow nudge test skipped because HBA Enable is OFF / source=" + source);
            UpdateHbaStatus(true);
            return;
        }

        StartCoroutine(DirectHandBonusElbowNudgeTestRoutine(source));
    }

    IEnumerator DirectHandBonusElbowNudgeTestRoutine(string source)
    {
        hbaLastBlock = "Elbow nudge test direct";
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] ACTION START DIRECT / source=" + source + " / preset=ElbowNudgeTest / head=Off");
        yield return StartCoroutine(RandomHandBonusElbowNudgeRoutine(source, "test-button"));
        DebugMessage("[HumanBodyAction] ACTION DONE DIRECT / source=" + source + " / preset=ElbowNudgeTest");
    }

    IEnumerator RandomHandBonusElbowNudgeRoutine(string source, string reason)
    {
        RefreshControllersNoReset();

        Atom moveAtom = containingAtom;
        if (moveAtom == null)
        {
            DebugMessage("[HumanBodyAction] Hand bonus elbow local skipped / reason=" + reason + " / missing-move-atom" +
                " / source=" + source);
            yield break;
        }

        FreeControllerV3 moveLElbow = FindControllerByAliases("lElbowControl", "leftElbowControl", "lElbow", "leftElbow");
        FreeControllerV3 moveRElbow = FindControllerByAliases("rElbowControl", "rightElbowControl", "rElbow", "rightElbow");

        if (moveLElbow == null && moveRElbow == null)
        {
            DebugMessage("[HumanBodyAction] Hand bonus elbow local skipped / reason=" + reason + " / missing-elbow" +
                " / source=" + source +
                " / moveAtom=" + moveAtom.uid +
                " / moveLElbow=" + (moveLElbow != null ? "1" : "0") +
                " / moveRElbow=" + (moveRElbow != null ? "1" : "0"));
            yield break;
        }

        RestoreHandBonusElbowSnapshot("hand-bonus-elbow-restart");
        int runSerial = ++handBonusElbowRunSerial;

        FreeControllerV3 moveElbow;
        string elbowLabel;
        string sideSelect;
        int selectedSide;

        if (moveLElbow != null && moveRElbow != null)
        {
            bool useLeft;
            if (handBonusElbowLastSide == 0)
            {
                useLeft = UnityEngine.Random.Range(0, 2) == 0;
                sideSelect = "first-random";
            }
            else
            {
                useLeft = handBonusElbowLastSide > 0;
                sideSelect = "alternate";
            }

            if (UnityEngine.Random.Range(0.0f, 100.0f) < 18.0f)
            {
                useLeft = UnityEngine.Random.Range(0, 2) == 0;
                sideSelect = "random-break";
            }

            moveElbow = useLeft ? moveLElbow : moveRElbow;
            elbowLabel = useLeft ? "L Elbow" : "R Elbow";
            selectedSide = useLeft ? -1 : 1;
            handBonusElbowLastSide = selectedSide;
        }
        else if (moveLElbow != null)
        {
            moveElbow = moveLElbow;
            elbowLabel = "L Elbow";
            selectedSide = -1;
            handBonusElbowLastSide = selectedSide;
            sideSelect = "only-left";
        }
        else
        {
            moveElbow = moveRElbow;
            elbowLabel = "R Elbow";
            selectedSide = 1;
            handBonusElbowLastSide = selectedSide;
            sideSelect = "only-right";
        }

        Vector3 startPos = GetControllerPosition(moveElbow);

        Vector3 localRight = moveAtom.transform != null ? Vector3.ProjectOnPlane(moveAtom.transform.right, Vector3.up) : Vector3.right;
        Vector3 localForward = moveAtom.transform != null ? Vector3.ProjectOnPlane(moveAtom.transform.forward, Vector3.up) : Vector3.forward;
        if (localRight.sqrMagnitude < 0.0001f) localRight = Vector3.right;
        if (localForward.sqrMagnitude < 0.0001f) localForward = Vector3.forward;
        localRight.Normalize();
        localForward.Normalize();

        Vector3 outward = selectedSide < 0 ? -localRight : localRight;
        Vector3 inward = -outward;
        float modeRoll = UnityEngine.Random.Range(0.0f, 100.0f);
        string guideMode;
        Vector3 dir;

        // Elbow bonus is even smaller than the knee bonus. It is not a target reach;
        // it is a tiny local body reaction that eases back to the original elbow state.
        if (modeRoll < 42.0f)
        {
            guideMode = "local-out-up";
            dir = outward * UnityEngine.Random.Range(0.62f, 0.92f) + Vector3.up * UnityEngine.Random.Range(0.22f, 0.52f) + localForward * UnityEngine.Random.Range(-0.04f, 0.14f);
        }
        else if (modeRoll < 72.0f)
        {
            guideMode = "local-out-forward";
            dir = outward * UnityEngine.Random.Range(0.45f, 0.82f) + localForward * UnityEngine.Random.Range(0.20f, 0.46f) + Vector3.up * UnityEngine.Random.Range(0.04f, 0.22f);
        }
        else if (modeRoll < 90.0f)
        {
            guideMode = "local-up";
            dir = Vector3.up * UnityEngine.Random.Range(0.70f, 1.00f) + outward * UnityEngine.Random.Range(0.08f, 0.26f) + localForward * UnityEngine.Random.Range(-0.06f, 0.12f);
        }
        else
        {
            guideMode = "local-relax-back";
            dir = inward * UnityEngine.Random.Range(0.12f, 0.30f) - localForward * UnityEngine.Random.Range(0.10f, 0.30f) + Vector3.up * UnityEngine.Random.Range(0.04f, 0.18f);
        }

        if (dir.sqrMagnitude < 0.0001f)
            dir = outward;
        dir.Normalize();

        float nudgeAmount = UnityEngine.Random.Range(HandBonusElbowNudgeAmountMin, HandBonusElbowNudgeAmountMax);
        Vector3 peakPos = startPos + dir * nudgeAmount;

        Vector3 sideAxis = Vector3.Cross(Vector3.up, dir);
        if (sideAxis.sqrMagnitude < 0.0001f)
            sideAxis = outward;
        sideAxis.Normalize();

        float sideArc = UnityEngine.Random.Range(-HandBonusElbowNudgeArcSideMax, HandBonusElbowNudgeArcSideMax);
        float upArc = UnityEngine.Random.Range(HandBonusElbowNudgeArcUpMin, HandBonusElbowNudgeArcUpMax);
        Vector3 arcOffset = sideAxis * sideArc + Vector3.up * upArc;

        float preDelay = UnityEngine.Random.Range(0.03f, 0.14f);
        float moveDur = UnityEngine.Random.Range(HandBonusElbowNudgeMoveSecondsMin, HandBonusElbowNudgeMoveSecondsMax);
        float holdDur = UnityEngine.Random.Range(HandBonusElbowNudgeHoldSecondsMin, HandBonusElbowNudgeHoldSecondsMax);
        float settleDur = UnityEngine.Random.Range(HandBonusElbowNudgeSettleSecondsMin, HandBonusElbowNudgeSettleSecondsMax);
        float settleBack = UnityEngine.Random.Range(0.25f, 0.52f);
        Vector3 settlePos = Vector3.Lerp(peakPos, startPos, settleBack);
        float releaseDur = UnityEngine.Random.Range(HandBonusElbowNudgeReleaseSecondsMin, HandBonusElbowNudgeReleaseSecondsMax);

        activeHandBonusElbowSnapshot = new HandBonusElbowSnapshot();
        activeHandBonusElbowSnapshot.elbow = moveElbow;
        activeHandBonusElbowSnapshot.position = startPos;
        activeHandBonusElbowSnapshot.rotation = GetControllerRotation(moveElbow);
        activeHandBonusElbowSnapshot.positionState = moveElbow.currentPositionState;
        activeHandBonusElbowSnapshot.rotationState = moveElbow.currentRotationState;

        try { moveElbow.currentPositionState = FreeControllerV3.PositionState.On; } catch { }
        try { moveElbow.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }

        hbaLastBlock = "Hand bonus elbow local: " + elbowLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Hand bonus elbow local start" +
            " / source=" + source +
            " / reason=" + reason +
            " / moveAtom=" + moveAtom.uid +
            " / elbow=" + elbowLabel +
            " / sideSelect=" + sideSelect +
            " / guide=" + guideMode +
            " / start=" + V3(startPos) +
            " / peak=" + V3(peakPos) +
            " / settle=" + V3(settlePos) +
            " / amount=" + F3(nudgeAmount) +
            " / preDelay=" + F3(preDelay) +
            " / moveSeconds=" + F3(moveDur) +
            " / holdSeconds=" + F3(holdDur) +
            " / settleSeconds=" + F3(settleDur) +
            " / releaseSeconds=" + F3(releaseDur) +
            " / arc=" + V3(arcOffset) +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");

        float delayStart = Time.time;
        while (Time.time - delayStart < preDelay)
        {
            if (runSerial != handBonusElbowRunSerial) yield break;
            SetControllerPosition(moveElbow, startPos);
            yield return null;
        }

        float startTime = Time.time;
        while (Time.time - startTime < moveDur)
        {
            if (runSerial != handBonusElbowRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - startTime) / Mathf.Max(0.01f, moveDur));
            float s = Smooth01(t);
            float pulse = Mathf.Sin(s * Mathf.PI);
            float organic = s + pulse * HandBonusElbowNudgeOvershoot * UnityEngine.Random.Range(0.45f, 0.90f);
            Vector3 pos = startPos + dir * (nudgeAmount * organic) + arcOffset * pulse;
            SetControllerPosition(moveElbow, pos);
            yield return null;
        }
        if (runSerial != handBonusElbowRunSerial) yield break;
        SetControllerPosition(moveElbow, peakPos);

        float settleStart = Time.time;
        while (Time.time - settleStart < settleDur)
        {
            if (runSerial != handBonusElbowRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - settleStart) / Mathf.Max(0.01f, settleDur));
            float e = Smooth01(t);
            Vector3 pos = Vector3.Lerp(peakPos, settlePos, e) + arcOffset * 0.16f * Mathf.Sin(e * Mathf.PI);
            SetControllerPosition(moveElbow, pos);
            yield return null;
        }
        if (runSerial != handBonusElbowRunSerial) yield break;
        SetControllerPosition(moveElbow, settlePos);

        float holdStart = Time.time;
        while (Time.time - holdStart < Mathf.Max(0.01f, holdDur))
        {
            if (runSerial != handBonusElbowRunSerial) yield break;
            SetControllerPosition(moveElbow, settlePos);
            yield return null;
        }
        if (runSerial != handBonusElbowRunSerial) yield break;

        HandBonusElbowSnapshot snap = activeHandBonusElbowSnapshot;
        if (snap == null || snap.elbow != moveElbow)
            yield break;

        float releaseStart = Time.time;
        while (Time.time - releaseStart < releaseDur)
        {
            if (runSerial != handBonusElbowRunSerial) yield break;
            float t = Mathf.Clamp01((Time.time - releaseStart) / Mathf.Max(0.01f, releaseDur));
            float e = Smooth01(t);
            float breathe = Mathf.Sin(e * Mathf.PI);
            Vector3 pos = Vector3.Lerp(settlePos, snap.position, e) + arcOffset * 0.04f * breathe;
            SetControllerPosition(moveElbow, pos);
            SetControllerRotation(moveElbow, Quaternion.Slerp(GetControllerRotation(moveElbow), snap.rotation, e));
            yield return null;
        }
        if (runSerial != handBonusElbowRunSerial) yield break;

        SetControllerPosition(moveElbow, snap.position);
        SetControllerRotation(moveElbow, snap.rotation);
        try { moveElbow.currentPositionState = snap.positionState; } catch { }
        try { moveElbow.currentRotationState = snap.rotationState; } catch { }
        activeHandBonusElbowSnapshot = null;

        hbaLastBlock = "Hand bonus elbow local restored: " + elbowLabel;
        UpdateHbaStatus(true);
        DebugMessage("[HumanBodyAction] Hand bonus elbow local restored" +
            " / source=" + source +
            " / reason=" + reason +
            " / moveAtom=" + moveAtom.uid +
            " / elbow=" + elbowLabel +
            " / guide=" + guideMode +
            " / peak=" + V3(peakPos) +
            " / settle=" + V3(settlePos) +
            " / release=" + V3(startPos) +
            " / releaseSeconds=" + F3(releaseDur) +
            " / positionState=restore-original:" + snap.positionState.ToString() +
            " / rotationState=restore-original:" + snap.rotationState.ToString() +
            " / restore=HBA_Cover_Restore,HBR_Cover_Restore,HBA_Reset");
    }

    void RestoreHandBonusElbowSnapshot(string reason)
    {
        if (activeHandBonusElbowSnapshot == null) return;

        HandBonusElbowSnapshot snap = activeHandBonusElbowSnapshot;
        activeHandBonusElbowSnapshot = null;
        handBonusElbowRunSerial++;

        if (snap.elbow != null)
        {
            SetControllerPosition(snap.elbow, snap.position);
            SetControllerRotation(snap.elbow, snap.rotation);
            try { snap.elbow.currentPositionState = snap.positionState; } catch { }
            try { snap.elbow.currentRotationState = snap.rotationState; } catch { }
        }

        DebugMessage("[HumanBodyAction] Hand bonus elbow restore" +
            " / reason=" + reason +
            " / elbow=" + (snap.elbow != null ? snap.elbow.name : "null") +
            " / positionState=" + snap.positionState.ToString() +
            " / rotationState=" + snap.rotationState.ToString());
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
            " / actions=HBA_Event_Start,HBA_Event_Inside,HBA_Event_Deep,HBA_Event_End,HBA_Twitch_Slow,HBA_Twitch_Weak,HBA_Twitch_Normal,HBA_Twitch_Strong,HBA_Cover_RandomHand,HBR_Cover_RandomHand,HBA_Bonus_KneeNudge,HBR_Bonus_KneeNudge,HBA_Cover_Restore,HBR_Cover_Restore,HBA_Cover_RandomKneeToThigh,HBR_Cover_RandomKneeToThigh,HBA_Cover_RandomKneeToThigh_Force,HBR_Cover_RandomKneeToThigh_Force,HBA_Head_Nod,HBA_Head_QuickNod,HBA_Head_IntenseShake"
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

    bool IsHandCoverComplyHoldEnabled()
    {
        return handCoverComplyHold != null && handCoverComplyHold.val;
    }

    void ApplyHandCoverComplyHoldIfNeeded(FreeControllerV3 hand, Vector3 holdPosition, Quaternion lockedRotation, string coverMode, string targetLabel)
    {
        if (!IsHandCoverComplyHoldEnabled()) return;
        if (hand == null) return;

        // Move/settle still uses PositionState.On. Only after the hand has reached the cover point,
        // reduce the position hold strength to Comply so the cover does not remain too rigid.
        SetControllerPosition(hand, holdPosition);
        SetControllerRotation(hand, lockedRotation);
        try { hand.currentPositionState = FreeControllerV3.PositionState.Comply; } catch { }
        DebugMessage("[HumanBodyAction] Cover comply hold / mode=" + coverMode +
            " / hand=" + GetHandLabel(hand) +
            " / target=" + targetLabel +
            " / hold=" + V3(holdPosition));
    }

    void CoverSelectionMessage(string message)
    {
        // v066: Compact Cover selection logs are controlled by Log Cover Hand, independent from Debug Log.
        if ((logCoverHand != null && logCoverHand.val) || IsDebug())
        {
            SuperController.LogMessage(message);
        }
    }

    void CoverHandLogMessage(string message)
    {
        if ((logCoverHand != null && logCoverHand.val) || IsDebug())
        {
            SuperController.LogMessage(message);
        }
    }

    string GetPushAwayPersonLabelForLog()
    {
        Atom atom = ResolveHandCoverPushAwayAtom();
        if (atom != null && !string.IsNullOrEmpty(atom.uid)) return atom.uid;
        if (handCoverPushAwayPerson != null && !string.IsNullOrEmpty(handCoverPushAwayPerson.val)) return handCoverPushAwayPerson.val;
        return "<none>";
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
