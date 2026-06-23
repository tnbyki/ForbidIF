// DEPTH_AUTO_RAWHUD_RESTORE_BUILD 2026-06-24: Renames Depth Probe Rate ON to Auto, keeps Performance Mode-linked probe pacing, and restores internal Raw HUD Probe for the HUD graph/FX while keeping debug toggles hidden.
// HUD_GRAPH_FX_DEFAULT_ON_BUILD 2026-06-24: Keeps Performance Mode and Depth Probe Rate visible, keeps Depth HUD graph and HUD FX visible, and defaults HUD graph/FX ON for screen-following HUD operation.
// DEPTH_RATE_ON_HUD_SYNC_BUILD 2026-06-24: Adds Depth Probe Rate=Auto mode driven by Performance Mode; Quality/Balanced update HUD/probe pacing fast enough for screen-following HUD, while Light remains throttled.
// UI_CLEANUP_RUNTIME_PERF_TOGGLES_BUILD 2026-06-24: Hides runtime perf feature toggles and keeps those features always enabled; Depth Probe Rate and Perf Probe Timing Log remain visible. Performance Mode remains active.
// UI_FINAL_DEPTH_RATE_BUILD 2026-06-24: Keeps existing probe/HUD/reaction features, hides ineffective debug toggles from UI, keeps Depth Probe Rate and Perf Probe Timing Log visible; Transform Cache remains always ON.
// PROBE_RAW_TOGGLES_RATE_BUILD 2026-06-24: Adds lower probe rates and separate Raw HUD/Event probe toggles to isolate duplicate heavy sampling.
// PERF_PROBE_DEEP_DIVE_BUILD 2026-06-24: Removes ineffective raw-event toggle, adds transform-cache/body-gate/bookkeeping switches, and logs main probe sub-timings.
// DEPTH_PROBE_RATE_TIMING_BUILD 2026-06-24: Adds 5/2/1 FPS depth-probe rates plus Perf Probe Timing Log for coarse block timing.
// DEPTH_PROBE_RATE_BUILD 2026-06-24: Adds Depth Probe Rate chooser to throttle expensive probe/event/HUD sampling without disabling motion.
// RUNTIME_FEATURE_TOGGLE_SPLIT_BUILD 2026-06-24: Splits broad Perf Motion Update into Placement/Upper Lower/P Follow/Depth Probe toggles for performance isolation.
// RUNTIME_FEATURE_TOGGLES_BUILD 2026-06-24: Replaces Feature Cut Mode combo with individual runtime feature toggles for performance isolation.
// FEATURE_CUT_MODES_BUILD 2026-06-24: Adds Feature Cut Mode combo (Full/No HUD/No Reactions/Motion Only/Docking Only) to isolate heavy runtime features.
// PROBE_UI_CLEANUP_BUILD 2026-06-24: Hides ineffective/developer perf toggles, forces transform cache ON, keeps only useful probe controls.
// P_MID_G_ALIGN_BUTTON_BUILD 2026-06-23: Adds a PUSH-adjacent button that places P Mid and P Tip onto the live genital G line.
// P_MID_AXIS_ASSIST_TRIGGER_CONFIRM_BUILD 2026-06-22: Uses BodyTouchTriggerProbe Gen contact to confirm/scale P Mid Axis Assist.
// P_MID_AXIS_ASSIST_BUILD 2026-06-22: Nudges P Mid/Base toward the Gen axis when Tip is already entering, reducing side-entry look from behind.
// TARGET_SWITCH_KEEP_DISTANCE_BUILD 2026-06-22: Keeps Distance when switching directly between Gen and Anus targets.
// QUALITY_HUD_FAST_BUILD 2026-06-22: Raises Quality HUD/debug-line pacing for a more screen-attached feel.
// INSERT_REACTION_FRAME_SPREAD_BUILD 2026-06-22: Defers first Inside TG/HBA action fire by one frame to reduce insertion-frame spikes.
// PERFORMANCE_MODE_BUILD 2026-06-22: Adds Quality/Balanced/Light update pacing for HUD, debug lines, and HBA bridge writes.
// PUSH_LINE_LIGHT_WRITE_BUILD 2026-06-22: Skips redundant controller state/control writes during Auto Line/PUSH motion without changing targets.
// SWITCH_RETRACT_LIGHT_WRITE_BUILD 2026-06-22: Skips redundant controller state/position writes during Switch Retract without changing motion.
// SWITCH_RETRACT_HORIZONTAL_HBA_CLEAR_BUILD 2026-06-22: Horizontalizes target-switch hip/root retreat direction and silently clears old reaction state so switch-generated zero progress never fires End.
// SWITCH_RETRACT_NO_CAPTURE_GATE_BUILD 2026-06-22: Allows Gen<->Anus Switch Retract even when capture state is false, using live target inside axis and keeping HBA End gated during the switch.
// SWITCH_RETRACT_HBA_GATE_BUILD 2026-06-22: Suppresses HBA End/zero-progress during Gen<->Anus Switch Retract, adds skip diagnostics, and weakens P-controller follow so hip motion is visible.
// SWITCH_RETRACT_COMPILEFIX_BUILD 2026-06-22: Initializes inside-line locals before compound condition so VaM Mono definite-assignment accepts Switch Retract.
// SWITCH_RETRACT_BUILD 2026-06-22: Adds a brief withdrawal motion when switching between Gen and Anus targets, using the previous target insertion axis.
// ANUS_HBA_COMMON_EVENT_BUILD 2026-06-22: Routes Anus HUD depth into the same HBA_Event_* bridge as Gen while keeping TargetId distinct.
// LINE_RENDER_LIGHT_BUILD 2026-06-22: Throttles Debug View line drawing and caches HUD LineRenderer lookups without changing Auto Line motion.
// AUTO_LINE_SLOW_LINEAR_BUILD 2026-06-22: Makes Auto Line Slow use a dedicated constant-speed in/out motion with no easing, while Auto Line/Fast stay unchanged.
// HBA_RELINK_CURRENT_INSTANCE_BUILD 2026-06-22: Refreshes HBA shared-status storable cache and prefers HumanBodyAction instances exposing HBA_BridgeVersion to avoid stale links after swapping HBA scripts.
// HBA_PROGRESS_HUD_SOURCE_FIX_BUILD 2026-06-22: Uses HUD/raw gen depth as HBA/event/UI progress when control depth is angle-gated at 0%.
// HBA_ONLY_NO_TG_HEAD_UI_BUILD 2026-06-22: Removes TargetLinePerson TG/Head reaction UI and keeps only hidden HBA event/status notification bridge.
// HBA_EVENT_STATUS_NOTIFY_BUILD 2026-06-22: Sends HBA_TargetId/HBA_Progress/HBA_Active to HumanBodyAction and defaults reactions to HBA_Event_* while TG_ defaults move to HumanBodyAction.
// HBA_REACTION_ACTIONS_BUILD 2026-06-22: Replaces Tw_/Head reaction choices with HumanBodyAction HBA_ actions and targets HumanBodyAction as the preferred action plugin.
// PERF_V176_DEPTH_FRAME_CACHE_BUILD 2026-06-22: Shares live depth results inside a frame to avoid duplicate Gen/Anus/Mouth projection work.
// PERF_V175_LOG_TW_HUD_OPT_BUILD 2026-06-22: Adds Debug Log gating, caches Gen Head action lookup, and lets HUD FX stay off for lighter runtime.
// PERF_V174_HUD_20FPS_BUILD 2026-06-22: Renders Gen/Anus HUD at 20fps while keeping depth detection and triggers per-frame.
// PERF_V173_HUD_LIGHTEN_BUILD 2026-06-22: Avoids non-active anus HUD sampling, caches G contact dot renderer, and suppresses redundant SetActive calls.
// TW_HEAD_HUD_DEPTH_FALLBACK_BUILD 2026-06-21: Uses HUD/raw depth for Gen Head/Tw reactions when control GenDepth is angle-gated, so Tw can fire on P Tip contact.
// TWITCH_TARGET_FIXED_TRACE_BUILD 2026-06-21: Fixes Head Plugin Atom to Target Person and logs Tw_ calls even when only Target atom is used.
// TWITCH_DEBUG_TRACE_BUILD 2026-06-21: Adds Debug View traces and test buttons for Tw_ BodyTwitcher reactions.
// TWITCH_REACTION_ACTIONS_BUILD 2026-06-21: Adds Tw_ BodyTwitcher actions to Gen Head reaction action choosers.
// MOUTH_PUSH_Y_SCALE_050_BUILD 2026-06-21: Relaxes mouth vertical damping from 0.25 to 0.50 so the line returns closer to mouthPhysicsMeshPredictionPoint.forward.
// MOUTH_PUSH_AUTO_BUILD 2026-06-21: Enables PUSH/Auto PUSH for mouth with a dedicated mouth inside line and no mouth yellow fallback.
// ANUS_NO_YELLOW_FALLBACK_AXIS_FIX_BUILD 2026-06-21: Keeps Anus P follow on the live anus axis instead of falling back to Yellow Guide, and derives Anus inside direction from the current red-line direction.
// ANUS_MARKER_THIN_RIGHT_STABLE_DIR_BUILD 2026-06-21: Nudges Anus */circle marker slightly right, thins its lines, and stabilizes Anus depth direction using own root/hip approach side instead of P controllers.
// ANUS_MARKER_MICRO_RIGHT_BUILD 2026-06-21: Nudges the Anus HUD star/circle marker a tiny bit right while keeping size and circle behavior unchanged.
// ANUS_MARKER_2THIRD_RIGHT_CIRCLE_FLIP_BUILD 2026-06-21: Restores Anus HUD marker size from half to about two-thirds, nudges it slightly right, and flips circle draw order vertically.
// ANUS_STAR_HALF_LEFT_UP_CIRCLE_BUILD 2026-06-21: Moves Anus HUD marker one marker left/up, halves size again, and switches * to circle when fully open.
// ANUS_STAR_SMALLER_RIGHT_BUILD 2026-06-21: Shrinks Anus HUD star so new max equals previous min and nudges the star slightly right.
// ANUS_DEPTH_DIRECTION_STAR_BUILD 2026-06-21: Auto-flips Anus depth/P-follow direction so own P stays on the approach side, and adds a growing asterisk marker under the Anus HUD bar.
// ANUS_DEPTH_HUD_AND_P_FOLLOW_BUILD 2026-06-21: Adds Anus depth HUD to the right of Gen HUD and uses Anus inside-line P follow near target.
// G_CONTACT_DOT_MICRO_LEFT_DOWN_2_BUILD 2026-06-21: Micro-adjusts the always-visible G contact dot slightly further left and down from v155.
// G_CONTACT_DOT_INITIAL_RED_RANGE_BUILD 2026-06-21: Shows the P Tip/G HUD dot from the start, nudges it half-dot right/down, and keeps it red only during the existing P Tip on G contact range.
// G_CONTACT_DOT_HALF_COLOR_BUILD 2026-06-21: Halves the P Tip/G HUD dot, nudges it slightly lower, and changes proximity by light-to-deep pink color without hiding alpha.
// P_TIP_G_CONTACT_DOT_TUNE_BUILD 2026-06-21: Moves the P Tip/G HUD dot left/up and fades alpha from 50% to dense pink by contact closeness.
// G_CONTACT_DOT_MICRO_LEFT_DOWN_BUILD 2026-06-21: Micro-adjusts the always-visible G contact dot slightly left and down from v154.
// G_CONTACT_DOT_SHIFT_WIDE_COLOR_BUILD 2026-06-21: Shifts the always-visible G contact dot slightly right overall and widens the pale-pink to deep-pink proximity color range.
// G_CONTACT_DOT_ALWAYS_RED_BUILD 2026-06-21: Keeps the G contact dot visible, shifts it left/down, and colors it from pale pink to red on contact.
// P_TIP_G_CONTACT_HUD_DOT_BUILD 2026-06-21: Shows a pink contact dot under the Gen Depth HUD bar while P Tip is on G.
// ANUS_DEPTH_HUD_AND_P_FOLLOW_BUILD 2026-06-21: Adds Anus depth HUD to the right of Gen HUD and uses Anus inside-line P follow near target.
// ANUS_NO_100_LEFT_GEN_COLOR_BUILD 2026-06-21: Removes anus 100% horizontal marker, shifts anus HUD left by one bar width, and applies the clearer pink depth color ramp to Gen fill.
// ANUS_PUSH_AUTO_ENABLED_BUILD 2026-06-21: Enables PUSH and PUSH Auto for anus target by using the current genital/anus inside line and keeps G-contact dot logs genital-only.
// MOUTH_PUSH_AUTO_BUILD 2026-06-21: Enables PUSH/Auto PUSH for mouth with a dedicated mouth inside line and no mouth yellow fallback.
// P_TIP_G_CONTACT_LOG_BUILD 2026-06-21: Allows PUSH Auto from P Tip on G contact without root-distance gating and logs on/off even when Debug View is off.
// LIE_SAFE_DISTANCE_PUSH_GUIDE_BUILD 2026-06-21: Moves Lie docking to a safer 1.30 distance and lets near-zero G contact guide PUSH inward.
// LIE_NEAR_NOW_DOCKING_STABLE_BUILD 2026-06-21: Keeps body yaw unchanged for Lie Now Docking at near-zero distance and only micro-adjusts XZ.
// PERF_CACHE_HUD_BUILD 2026-06-21: Caches atom/controller lookups and throttles visual-only Gen Depth HUD sampling.
// LIE_DOCKING_YAW_FLIP_BUILD 2026-06-20: Flips Lie Docking front/back preference after yaw lock selection.
// LIE_DOCKING_YAW_LOCK_BUILD 2026-06-20: In Lie pose, locks Docking yaw to target root same/opposite direction instead of Labia/red-line angle.
// LIE_PUSH_AUTO_FIX_BUILD 2026-06-20: Allows PUSH/PUSH Auto during Lie pose and uses Lie-compensated push depth for auto trigger.
// STANDING_KEEP_LEG_IK_LOG_TRIM_BUILD 2026-06-19: Keeps leg IK during standing docking and trims normal PUSH logs behind Debug View.
// HUD_IGNORE_ANGLE_GATE_BUILD 2026-06-19: Keeps control GenDepth angle-gated but lets the right HUD bar display raw depth through the angle gate.
// ANGLE_GATE_NO_BASE_LIFT_BUILD 2026-06-19: Keeps yellow P2 fallback but disables dynamic P Base lift when live G Depth angle is outside the 45 degree gate.
// G_DEPTH_GUIDE_ANGLE_GATE_BUILD 2026-06-19: Hides cyan G Depth guide when G Depth angle is outside the 45 degree gate.
// GEN_DEPTH_ANGLE_GATE_LOG_TRIM_BUILD 2026-06-19: Gates GenDepth by G Depth angle and makes PUSH cycle logs Debug View only.
// PUSH_AUTO_STOP_SIMPLE_TIP_DISTANCE_BUILD 2026-06-19: Stops PUSH auto only by Distance delta or Tip rawDepth threshold.
// PUSH_AUTO_STOP_DISTANCE_DELTA_BUILD 2026-06-19: Stops PUSH auto when Distance increases from the auto-start value.
// PUSH_AUTO_TIP_START_BASE_STOP_BUILD 2026-06-19: Uses Tip depth for auto start and P Base depth for auto stop.
// PUSH_AUTO_BASE_DEPTH_BUILD 2026-06-19: Uses P Base depth for PUSH auto start/stop so pushing base starts and pulling base stops.
// PUSH_AUTO_EXIT_DECREASING_BUILD 2026-06-19: Allows earlier PUSH auto exit when rawDepth is shallow and decreasing.
// PUSH_AUTO_EXIT_RAW_019_BUILD 2026-06-19: Uses rawDepth < 0.019 as PUSH auto G Depth exit timing.
// PUSH_AUTO_G_TIMING_TUNE_BUILD 2026-06-19: Delays PUSH auto start and speeds up G Depth exit stop timing.
// PUSH_UI_REORDER_BUILD 2026-06-19: Moves Distance above Now Docking and moves PUSH Depth Scale / Auto G Trigger lower in the UI.
// PUSH_AUTO_EXIT_RELEASE_P_IK_BUILD 2026-06-19: Releases P Base/Mid/Tip IK after PUSH auto stops by G Depth exit.
// PUSH_AUTO_G_DEPTH_TRIGGER_BUILD 2026-06-19: Starts PUSH auto modes when G Depth is entered and stops them when leaving.
// NOW_DOCKING_NEAR_KEEP_LOG_TRIM_BUILD 2026-06-19: Keeps current root placement for near Now Docking and trims PUSH cycle/target logs unless useful.
// HUD_DROP_LEFT_008_BUILD 2026-06-17: Increases lower drop marker left correction to 0.008 while keeping soft uninserted alpha.
// PUSH_NONE_SPIRAL_RANDOM_BUILD 2026-06-19: Adds None single-shot PUSH mode, makes it default, and randomizes Spiral direction per cycle.
// PUSH_AUTO_LOOP_STOP_BUILD 2026-06-19: Makes PUSH auto modes loop until pressed again, colors PUSH button while active, strengthens Spiral, and adds Deep Stop wobble.
// PUSH_AUTO_MODES_BUILD 2026-06-19: Adds PUSH Auto Mode combo, caps repeated PUSH to one-push max, and adds line/slow/fast/spiral/deep-stop/random modes.
// PUSH_CAP_SPEED_BUILD 2026-06-19: Caps repeated PUSH depth by GenDepthMax*PUSH Depth Scale and speeds PUSH motion by about 1.3x.
// PUSH_DEPTH_SCALE_EXTEND_BUILD 2026-06-19: Adds PUSH Depth Scale slider and lets repeated PUSH while moving extend deeper before restoring to original.
// PUSH_G_DEPTH_DELTA_BUILD 2026-06-19: Changes PUSH to move current P Base/Mid/Tip along live G Depth axis to random depth ratio 1.30-1.50 and restore.
// PUSH_P_LINE_RANDOM_BUILD 2026-06-19: Adds PUSH button above Now Docking to move P Base/Mid/Tip deeper along yellow line to random 1.30-1.50 and restore.
// NOW_DOCKING_LINE_FIT_NO_EXTRA_DROP_BUILD 2026-06-18: Now Docking projects current position onto the docking line, starts from a mid distance, and does not pre-apply yellow dip height.
// NOW_DOCKING_SMART_SHAPE_BUILD 2026-06-18: Now Docking keeps current placement but builds the yellow guide with a Smart Docking-like virtual distance.
// NOW_DOCKING_FRONT_GATE_BUILD 2026-06-18: Uses current-distance Now Docking only when already near the docking front/back line; side starts fall back to the old safe distance.
// NOW_DOCKING_YELLOW_SHAPE_MIN_BUILD 2026-06-18: Keeps the yellow dip trapezoid from shrinking too much when Now Docking starts close to the target.
// NOW_DOCKING_CURRENT_LINE_FIT_BUILD 2026-06-18: Now Docking keeps the current horizontal distance and fits the body onto the selected docking line instead of forcing Distance=1.0.
// HUD_DROP_FRONT_OFFSET_ALPHA_BUILD 2026-06-17: Offsets the lower drop marker after front projection and softens its uninserted alpha.
// HUD_DROP_FRONT_GRADIENT_BUILD 2026-06-17: Pulls the lower drop marker forward and adds a subtle outline gradient.
// HUD_PULSE_ROD_ACTION_FIX_BUILD 2026-06-17: Tunes idle/insert pulse and makes Rod Width scale insertion burst in the intuitive direction.
// HUD_ROD_WIDTH_TUNE_BUILD 2026-06-17: Renames Rod Width, narrows the drop marker, warms the fill bar, and strengthens pulse subtly.
// HUD_AUTO_BODY_SUBTLE_BUILD 2026-06-17: Adds subtle target-thigh body scaling, manual drop max width, and insertion pulse.
// HUD_TUNED_PULSE_BUILD 2026-06-17: Tunes GenDepth HUD colors, peak depth layering, lower marker pulse, and Load Defaults P IK release.
// HUD_SMOOTH_BAR_BUILD 2026-06-17: Rounds GenDepth bars, improves contrast, and smooths the lower drop marker.
// HUD_DROP_MARKER_BUILD 2026-06-17: Uses one lower drop marker shape and opens it from 1% to 15% GenDepth.
// HUD_INSERT_STATE_BUILD 2026-06-17: Defaults TG on, Yellow Butt 1.5, and changes Gen HUD markers for uninserted/inserted states.
// GEN_BODY_GATE_BUILD 2026-06-17: Gates GenDepth percent by own hip distance and can release P IK when body is far.
// GEN_TG_START_THRESHOLD_BUILD 2026-06-17: Splits Start from Inside so TG_Start fires on the first shallow positive depth.
// GEN_TG_TRIGGERS_BUILD 2026-06-17: Adds selectable TG_ UIToggle atom actions for Start/Inside/Deep/End GenDepth states.
// GEN_TG_LIST_REFRESH_FIX_BUILD 2026-06-17: Refreshes TG chooser copies and matches TG_ by atom uid or atom name.
// GEN_HEAD_ACTION_DIRECT_BUILD 2026-06-17: Calls HumanHeadOpenControl actions directly from GenDepth Start/Inside/Deep/End.
// GEN_HEAD_INSIDE_RANDOM_BUILD 2026-06-17: Adds Inside-only Head Random action with 3-10s irregular firing.
// EXTERNAL_ACTIONS_BUILD 2026-06-17: Registers visible non-TG/non-Head buttons as external actions.
// ANUS_TARGET_BUILD 2026-06-17: Adds Anus docking target using _JointAl/Debug and keeps GenDepth on genital only.
// GEN_DEPTH_LOG_THROTTLE_BUILD 2026-06-17: Reduces GenDepth probe debug logs to gate/zone changes plus a slow heartbeat.
// UI_CLEANUP_BUILD 2026-06-17: Hides old Apply Once and Cuddle buttons from the main UI.
// LOW_TARGET_ACTION_BUILD 2026-06-16: Replaces low-target toggles with Off / Leg Unlock / Sit Ground Pose action combo.
// LOW_TARGET_LEG_UNLOCK_BUILD 2026-06-16: Renames leg IK option and unlocks foot/knee only when target height would lower own hip significantly.
// NOW_SMART_LABIA_BUILD 2026-06-16: Renames Auto Docking to Now Docking; Smart uses Labia-facing side, Reverse uses the opposite.
// DEBUG_VIEW_BUILD 2026-06-16: Renames Show Lines to Debug View, defaults it OFF, and gates detailed logs behind it.
// GEN_DEPTH_PROBE_LOG_BUILD 2026-06-16: Logs rawDepth/lateralDistance/percent/gate state for insertion false-positive checks.
// GEN_DEPTH_LATERAL_GATE_BUILD 2026-06-16: Gen depth counts only when P tip is close to the Labia axis, preventing far Reverse-side false insertion.
// DOCKING_GENITAL_ROOT_FORWARD_BUILD 2026-06-16: Uses LabiaTrigger position for genital origin, but target root forward for Docking horizontal direction.
// DOCKING_TARGET_PROBE_NO_HIP_FALLBACK_BUILD 2026-06-16: Logs Labia/Mouth target detection and stops Docking instead of falling back to hip direction when special target is missing.
// GEN_DEPTH_METER_TUNE_NO_TESTES_BUILD 2026-06-16: Retunes Gen meter visuals and removes Testes Forward waist-motion test from TargetLinePerson.
// GEN_DEPTH_DOUBLE_HEART_BURST_BUILD 2026-06-16: Adds upper/lower hearts, removes HUD percent text, and adds rainbow threshold bursts.
// GEN_DEPTH_HEART_MARKER_BUILD 2026-06-16: Replaces 100% dot marker with PunchBallGame-style heart marker.
// GEN_DEPTH_MAX_SLIDER_BUILD 2026-06-16: Adds Gen Depth Max slider and retunes meter colors/marker/peak hold.
// GEN_DEPTH_METER_PEAK_BUILD 2026-06-16: Retunes 100%, shortens HUD bar, adds 100 marker and 3s peak hold.
// GEN_DEPTH_BAR_CUBE_BUILD 2026-06-16: Draws right-side Gen depth HUD with cube bars and integer percent text.
// GEN_DEPTH_RIGHT_BAR_COMPILEFIX_BUILD 2026-06-16: Initializes HUD depth locals for VaM Mono definite-assignment analysis.
// GEN_DEPTH_RIGHT_BAR_BUILD 2026-06-16: Replaces front text with a right-side live Gen depth HUD bar.
// GEN_DEPTH_PERCENT_ONLY_BUILD 2026-06-16: Shows only live Gen depth numerator/denominator and percent.
// LIVE_GEN_DEPTH_TEXT_BUILD 2026-06-16: Adds live Purple Gen-depth projection values without changing existing guide lines.
// INSERT_DEBUG_TEXT_BUILD 2026-06-16: Shows trial P-tip insertion projection values in UI and in front of the camera.
// DOCKING_BUTTONS_TARGET_RESET_BUILD 2026-06-15: Adds Auto/Smart/Reverse Smart Docking buttons and resets capture state when Target changes.
// PUBLIC_UI_NAMES_BUILD 2026-06-15: Public-facing Auto/Auto Reverse names and locks placement sliders until first Auto capture.
// CAPTURE_CALLBACK_GUARD_BUILD 2026-06-15: Prevents slider callbacks from applying placement during Capture initialization.
// NO_P_FOLLOW_IN_LIE_BUILD 2026-06-15: Lie poses skip penisBase/Mid/Tip yellow-guide control.
// AUTO_LIE_VISIBLE_ON_BUILD 2026-06-15: Auto Lie On Ride Pose is visible and enabled by default.
// TRANSPARENT_LINES_BUILD 2026-06-15: Makes all guide/debug lines semi-transparent to reduce visual flicker.
// LOCKED_GUIDE_SCALE_BUILD 2026-06-15: Distance/orbit sliders keep the captured guide scale; only capture/guide-scale changes rebuild the guide.
// DELAYED_GUIDE_REFRESH_BUILD 2026-06-15: Slider/toggle apply now moves root first, waits 2 frames, then applies Upper/P follow.
// UPPER_BODY_LOWER_DELTA_CHEST_HEAD_BUILD 2026-06-15: Upper Body Lower uses delta movement only; no saved localPosition reapply; hand/elbow excluded.
// UPPER_BODY_LOWER_RESTORE_STATE_BUILD 2026-06-15: Restores original PositionState after Upper Body Lower finishes/releases.
// SLIDER_ONLY_APPLY_BUILD 2026-06-15: Adds Apply On Slider Change Only mode; stops per-frame placement/upper/P follow when enabled.
// YELLOW_BUTT_GUIDE_SCALE_BUILD 2026-06-14: Adds one slider to scale yellow guide dip clearance for larger hips/butt.
// YELLOW_GUIDE_FLAT_START_040_LINE_UNCHANGED_BUILD 2026-06-14: Same Yellow/Green guide build+draw as YELLOW_GUIDE_ALWAYS_LIE_FLAT. Only no-dip P follow start gate changes to Distance<=0.40.
// Based on GREEN70_UPPER_BODY_LOWER_CHECKED: yellow 6-point green70 flat path preserved for display only.
// RIDE_IGNORE_GEN_DIR_BUILD 2026-06-13: Ride mode ignores genital direction and uses targetHip->ownHip direction.
// RIDE_BACK_FIXED_BUILD 2026-06-13: Ride pose auto priority / Lie On Back fixed / Sit Ground default ON.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetLinePerson : MVRScript
{
    JSONStorableStringChooser targetPersonChooser;
    JSONStorableStringChooser targetControllerChooser;
    JSONStorableStringChooser performanceModeChooser;
    JSONStorableBool runtimePlacement;
    JSONStorableBool runtimeUpperLower;
    JSONStorableBool runtimePFollow;
    JSONStorableBool runtimeDepthProbe;
    JSONStorableStringChooser depthProbeRateChooser;
    JSONStorableBool perfProbeTimingLog;
    JSONStorableBool perfRawHudProbe;
    JSONStorableBool perfRawEventProbe; // hidden/forced off in v199: raw event probe had no measurable effect
    JSONStorableBool perfReactionFallbackProbe;
    JSONStorableBool perfTransformCache;
    JSONStorableBool perfMainBodyGate;
    JSONStorableBool perfMainBookkeeping;
    float lastPerfProbeTimingLogTime = -999.0f;
    float lastDepthProbeUpdateTime = -999.0f;
    int lastDepthProbeDecisionFrame = -1;
    bool lastDepthProbeDecisionRun = true;
    float lastPerfMainLineMs = 0.0f;
    float lastPerfMainCalcMs = 0.0f;
    float lastPerfMainGateMs = 0.0f;
    float lastPerfMainBookMs = 0.0f;
    JSONStorableBool runtimeHud;
    JSONStorableBool runtimeReactions;
    JSONStorableBool runtimePushAuto;
    JSONStorableBool runtimeDynamicVisuals;

    JSONStorableFloat distance;
    JSONStorableFloat orbitAngle;
    JSONStorableFloat hipYOffset;
    JSONStorableFloat cuddleDepth;
    JSONStorableFloat avoidCaptureDuration;
    JSONStorableFloat avoidRadius;
    JSONStorableFloat avoidSideAngle;
    JSONStorableFloat tiltTriggerAngle;
    JSONStorableFloat upperTiltAngle;
    JSONStorableBool autoUpperTilt;
    JSONStorableBool allowDownTilt;
    JSONStorableBool avoidTargetOnCapture;
    JSONStorableBool followTarget;
    JSONStorableBool applyOnSliderChangeOnly;
    JSONStorableBool showLines;
    JSONStorableBool debugLog;
    JSONStorableBool dynamicRedLine;
    JSONStorableFloat redLineUpdateSec;
    JSONStorableBool dynamicYellowEnd;
    JSONStorableBool showInsertDebug;
    JSONStorableBool genDepthHudFx;
    JSONStorableString insertDebugText;
    JSONStorableFloat genDepthMax;
    JSONStorableFloat genHudDropMaxWidth;
    JSONStorableBool genBodyGate;
    JSONStorableFloat genBodyGateMaxDistance;
    JSONStorableBool pIkOffOnGenBodyGate;
    JSONStorableBool genTgTriggers;
    JSONStorableString genTgPrefix;
    JSONStorableStringChooser genTgStartAtom;
    JSONStorableStringChooser genTgInsideAtom;
    JSONStorableStringChooser genTgDeepAtom;
    JSONStorableStringChooser genTgEndAtom;
    JSONStorableStringChooser genTgStartMode;
    JSONStorableStringChooser genTgInsideMode;
    JSONStorableStringChooser genTgDeepMode;
    JSONStorableStringChooser genTgEndMode;
    JSONStorableBool genHeadActions;
    JSONStorableStringChooser genHeadAtom;
    JSONStorableStringChooser genHeadStartAction;
    JSONStorableStringChooser genHeadInsideAction;
    JSONStorableStringChooser genHeadDeepAction;
    JSONStorableStringChooser genHeadEndAction;
    JSONStorableFloat genHeadInsideCooldown;
    JSONStorableStringChooser lowTargetAction;
    bool lowTargetActionReached;
    float lowTargetActionHipDrop;
    JSONStorableBool autoLieOnRidePose;

    JSONStorableFloat sitGroundYThreshold;

    JSONStorableFloat limbRestoreDelay;
    JSONStorableFloat kneeAfterFootDelay;
    JSONStorableBool pYellowPathAlign;
    JSONStorableFloat pYellowPathAdvance;
    JSONStorableBool hipLowerByYellowPath;
    JSONStorableFloat hipLowerByYellowScale;
    JSONStorableBool pPathSealed;
    JSONStorableBool upperBodyLowerByDistance;
    JSONStorableFloat upperBodyDistanceLowerScale;
    JSONStorableBool upperBodyLowerByYellowPath;
    JSONStorableFloat upperBodyYellowLowerScale;
    JSONStorableBool pAngleAtYellowP3;
    JSONStorableFloat yellowButtGuideScale;
    JSONStorableFloat pushDepthScale;
    JSONStorableStringChooser pushAutoMode;
    JSONStorableBool pushAutoGDepthTrigger;
    JSONStorableBool switchRetractOnTargetChange;
    JSONStorableFloat switchRetractDistance;
    JSONStorableFloat switchRetractTime;

    UIDynamicSlider orbitAngleSlider;
    UIDynamicSlider distanceSlider;
    UIDynamicSlider hipYOffsetSlider;
    UIDynamicSlider yellowButtGuideScaleSlider;
    UIDynamicButton pushButton;

    readonly Dictionary<string, FreeControllerV3.PositionState> upperBodyLowerBasePositionStates = new Dictionary<string, FreeControllerV3.PositionState>();
    bool upperBodyLowerBaseCaptured;
    float upperBodyLowerReferenceDistance;
    float lastAppliedUpperBodyLower;
    bool delayedGuideRefreshPending;
    int delayedGuideRefreshFrames;
    bool delayedGuideRefreshRebuildGuide;
    string delayedGuideRefreshReason = "";
    string lastUpperBodyYellowLowerPhase = "";
    float lastLoggedUpperBodyYellowProgress = -1f;
    bool pAngleAtYellowP3Applied;
    bool pDynamicBaseYApplied;
    Vector3 lastDynamicPBaseOffset;
    float lastPAngleDebugLogTime = -999f;
    Coroutine pushPRoutine;
    Coroutine switchRetractRoutine;
    bool targetSwitchRetractBusy;
    float targetSwitchRetractHbaGateUntil = -1.0f;
    float lastSwitchRetractHbaGateLogTime = -999.0f;
    string lastTargetControllerMode = "genital";
    bool pushPStateCaptured;
    bool pushStopRequested;
    bool pushAutoLoopActive;
    bool pushAutoGDepthSuppressUntilExit;
    bool pushReleasePIkOnDone;
    bool pushButtonUiCaptured;
    ColorBlock pushButtonNormalColors;
    Vector3 pushActiveDir;
    float pushCurrentMoveDistance;
    float pushTargetMoveDistance;
    float pushLastPressTime;
    string pushResolvedMode = "";
    float pushModeFollowSpeed;
    float pushModeReturnSeconds;
    float pushModeHoldSeconds;
    bool pushModeLinearSlow;
    float pushModeSpiralAngle;
    float pushModeSpiralStartAngle;
    float pushAutoGDepthEnterSince = -1.0f;
    float pushAutoGDepthExitSince = -1.0f;
    float pushAutoGDepthStartDistance;
    float pushStartRawDepth;
    float pushStartLateral;
    Vector3 pushSavedPBasePosition;
    Vector3 pushSavedPMidPosition;
    Vector3 pushSavedPTipPosition;
    Quaternion pushSavedPBaseRotation;
    Quaternion pushSavedPMidRotation;
    Quaternion pushSavedPTipRotation;
    FreeControllerV3.PositionState pushSavedPBasePositionState;
    FreeControllerV3.PositionState pushSavedPMidPositionState;
    FreeControllerV3.PositionState pushSavedPTipPositionState;
    FreeControllerV3.RotationState pushSavedPBaseRotationState;
    FreeControllerV3.RotationState pushSavedPMidRotationState;
    FreeControllerV3.RotationState pushSavedPTipRotationState;

    const float PushPDepthScaleDefault = 1.50f;
    const float PushPDepthScaleMin = 1.00f;
    const float PushPDepthScaleMax = 2.00f;
    const float PushPMinMoveDistance = 0.010f;
    const float PushPFollowSpeed = 10.5f;
    const float PushPReturnSeconds = 0.22f;
    const float PushPHoldSeconds = 0.00f;
    const float PushPLineSlowLinearSpeed = 0.12f;
    const string PushModeNone = "None";
    const string PushModeAutoLine = "Auto Line";
    const string PushModeAutoLineSlow = "Auto Line Slow";
    const string PushModeAutoLineFast = "Auto Line Fast";
    const string PushModeAutoSpiral = "Auto Spiral";
    const string PushModeAutoDeepStop = "Auto Deep Stop";
    const string PushModeAutoRandom = "Auto Random";
    const float PushPSpiralDegreesMin = 100.0f;
    const float PushPSpiralDegreesMax = 220.0f;
    const float PushPSpiralStartMinDegrees = -45.0f;
    const float PushPSpiralStartMaxDegrees = 45.0f;
    const float PushPDeepStopSeconds = 1.00f;
    const float PushPDeepStopWobbleSpeed = 13.0f;
    const float PushPDeepStopWobbleScale = 0.18f;
    const float PushAutoGDepthEnterRawDepth = 0.020f;
    const float PushAutoGDepthEnterLateralMax = 0.070f;
    const float PushAutoGDepthEnterHoldSeconds = 0.25f;
    const float PushAutoGDepthExitRawDepth = 0.019f;
    const float PushAutoGDepthExitHoldSeconds = 0.08f;
    const float PushAutoGDepthExitDistanceDelta = 0.005f;
    const float PushAutoNearZeroBackDepth = 0.020f;
    const float PushNearZeroGuideDepth = 0.045f;
    const float SwitchRetractDistanceDefault = 0.060f;
    const float SwitchRetractDistanceMin = 0.000f;
    const float SwitchRetractDistanceMax = 0.180f;
    const float SwitchRetractTimeDefault = 0.180f;
    const float SwitchRetractTimeMin = 0.050f;
    const float SwitchRetractTimeMax = 0.600f;
    const float SwitchRetractSettleSeconds = 0.050f;
    const float SwitchRetractHbaGateResumeDelaySeconds = 0.150f;
    const float SwitchRetractPControllerFollowScale = 0.25f;
    const float SwitchRetractMinHorizontalDirSqr = 0.0001f;

    FreeControllerV3.PositionState savedRKneePosState;
    FreeControllerV3.RotationState savedRKneeRotState;
    FreeControllerV3.PositionState savedLKneePosState;
    FreeControllerV3.RotationState savedLKneeRotState;
    FreeControllerV3.PositionState savedRFootPosState;
    FreeControllerV3.RotationState savedRFootRotState;
    FreeControllerV3.PositionState savedLFootPosState;
    FreeControllerV3.RotationState savedLFootRotState;

    bool limbStateCaptured;

    readonly List<string> personChoices = new List<string>();
    readonly List<string> genTgAtomChoices = new List<string>();
    readonly List<string> genHeadAtomChoices = new List<string>();
    readonly List<string> genHeadActionChoices = new List<string>()
    {
        "Off",
        "HBA_Event_Start",
        "HBA_Event_Inside",
        "HBA_Event_Deep",
        "HBA_Event_End",
        "HBA_Gen_Start",
        "HBA_Gen_Inside",
        "HBA_Gen_Deep",
        "HBA_Gen_End",
        "HBA_Anus_Start",
        "HBA_Anus_Inside",
        "HBA_Anus_Deep",
        "HBA_Anus_End",
        "HBA_Mouth_Start",
        "HBA_Mouth_Inside",
        "HBA_Mouth_Deep",
        "HBA_Mouth_End",
        "HBA_Twitch_Weak",
        "HBA_Twitch_Normal",
        "HBA_Twitch_Strong",
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
    readonly List<string> genHeadInsideActionChoices = new List<string>()
    {
        "Off",
        "HBA_Event_Start",
        "HBA_Event_Inside",
        "HBA_Event_Deep",
        "HBA_Event_End",
        "HBA_Gen_Start",
        "HBA_Gen_Inside",
        "HBA_Gen_Deep",
        "HBA_Gen_End",
        "HBA_Anus_Start",
        "HBA_Anus_Inside",
        "HBA_Anus_Deep",
        "HBA_Anus_End",
        "HBA_Mouth_Start",
        "HBA_Mouth_Inside",
        "HBA_Mouth_Deep",
        "HBA_Mouth_End",
        "HBA_Twitch_Weak",
        "HBA_Twitch_Normal",
        "HBA_Twitch_Strong",
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
    readonly List<string> genTgModeChoices = new List<string>()
    {
        "Off",
        "State",
        "Button Pulse",
        "Timer 1s",
        "Timer 5s"
    };
    readonly string[] genTgFallbackStorableIds =
    {
        "Trigger",
        "UIToggle",
        "Toggle",
        "Control",
        "plugin#0",
        "plugin#1",
        "plugin#2"
    };
    readonly string[] genTgFallbackBoolNames =
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

    Vector3 capturedOrigin;
    Vector3 capturedDir;
    Vector3 capturedLineDir;
    bool hasDynamicRedLineDisplay;
    Vector3 dynamicRedLineOrigin;
    Vector3 dynamicRedLineDir;
    float lastDynamicRedLineUpdateTime = -999f;
    bool dynamicYellowEndFrozen;
    bool lastGenDepthSampleKnown;
    float lastGenDepthRawDepth;
    float lastGenDepthPushEffectiveDepth;
    bool lastGenDepthPushLieCompensated;
    float lastGenDepthLateral;
    float lastGenDepthBodyDistance;
    float lastGenDepthPercent;
    string lastDynamicYellowEndSkipReason = "";
    float lastDynamicYellowEndSkipLogTime = -999f;

    bool captured;
    bool isCapturing;
    bool isAvoidMoving;
    bool nowDockingLineFitActive;
    bool nowDockingKeepCurrentPlacement;
    bool nowDockingLieNearKeepOrientation;
    float nowDockingLineFitPBaseY;
    float appliedHipYOffset;
    float appliedUpperTiltAngle;
    bool hasAppliedUpperTilt;
    bool rideLieActive;
    bool lieDockingYawLockActive;
    Vector3 lieDockingYawLockForward;
    bool lieDockingYawLockOpposite;

    Coroutine avoidCaptureRoutine;
    Coroutine delayedLineLockRoutine;
    Coroutine delayedInsideReactionRoutine;

    const float LineLockDelaySeconds = 0.20f;
    const int DelayedGuideRefreshFrameCount = 2;
    const float GuideLineAlpha = 0.35f;
    const float PAngleAtYellowP3BaseDegrees = 15.0f;
    const float PAngleGenMaxDegrees = 90.0f;
    const float HoleTowardOwnTiltDegThreshold = 10.0f;
    const float HoleTowardOwnDotThreshold = 0.25f;
    const float PAngleDebugLogIntervalSeconds = 999999f; // minimal log: first apply only
    const float PDynamicBaseYMinLift = 0.005f;
    const float PDynamicBaseYLiftScale = 1.0f;
    const float PDynamicBaseForwardOnDipMin = 0.003f;
    const float PDynamicBaseForwardOnDipMax = 0.060f;
    const float PDynamicBaseForwardOnDipScale = 0.35f;
    const float PDynamicForwardKeepShapeLeadScale = 1.25f;
    const float PDynamicForwardKeepShapeMinLead = 0.16f;
    const float PDynamicForwardKeepShapeStartProgress = 0.985f;
    const float PMidAxisAssistTipBackDepth = -0.030f;
    const float PMidAxisAssistTipForwardDepth = 0.260f;
    const float PMidAxisAssistTipLateralMax = 0.110f;
    const float PMidAxisAssistMidLateralMin = 0.012f;
    const float PMidAxisAssistMidMax = 0.045f;
    const float PMidAxisAssistBaseMax = 0.026f;
    const float PMidAxisAssistMidScale = 0.72f;
    const float PMidAxisAssistBaseScale = 0.38f;
    const float PMidAxisAssistUnconfirmedScale = 0.35f;
    const float PMidAxisAssistLogInterval = 2.5f;
    const float PMidGAlignBaseFollowScale = 0.35f;
    const float PMidGAlignBaseMaxMove = 0.035f;
    const float PDynamicForwardKeepShapeMinUpAngleDegrees = 8.0f;
    const float PTipYellowGuideTangentSmoothDistance = 0.055f;
    const float PTipYellowGuideEndExtendMax = 0.45f;
    const int PTipYellowParallelLockPointIndex = 3;
    const float PTipYellowParallelLockSlack = 0.0005f;
    const float POwnTiltGuardGreenLengthThreshold = 0.25f;
    const float POwnTiltGuardMidAngleRatio = 0.55f;
    const float POwnTiltGuardMidAngleMinDegrees = 25.0f;
    const float POwnTiltGuardMidAngleMaxDegrees = 50.0f;
    const float POwnTiltGuardTipMinUpDegrees = 60.0f;

    const float YellowGuideDipAngleMinDegrees = 75.0f;
    const float YellowGuideDipAngleMaxDegrees = 115.0f;
    const float YellowGuideLieVerticalDotThreshold = 0.55f;
    const float PFlatGuideStartDistance = 0.40f; // no-dip guide: start P follow when Distance reaches this value
    const float LowTargetLegUnlockMinDrop = 0.100f;
    const float StandingLegIkKeepHipRootYMin = 0.550f;
    const string LowTargetActionOff = "Off";
    const string LowTargetActionLegUnlock = "Leg Unlock";
    const string LowTargetActionSitGroundPose = "Sit Ground Pose";
    const string GenTgModeOff = "Off";
    const string GenTgModeState = "State";
    const string GenTgModeButtonPulse = "Button Pulse";
    const string GenTgModeTimer1 = "Timer 1s";
    const string GenTgModeTimer5 = "Timer 5s";
    const float GenTgButtonPulseSeconds = 0.10f;
    const float GenTgTimer1Seconds = 1.00f;
    const float GenTgTimer5Seconds = 5.00f;
    const float GenTgEventCooldownSeconds = 0.50f;
    const float GenTgStateOffDelaySeconds = 0.50f;
    const float GenTgStartEnterPercent = 0.001f;
    const float GenTgStartExitPercent = 0.001f;
    const float GenTgInsideEnterPercent = 0.050f;
    const float GenTgInsideExitPercent = 0.001f;
    const float GenTgDeepEnterPercent = 1.000f;
    const float GenTgDeepExitPercent = 0.950f;
    const string GenHeadAtomTargetPerson = "Target Person";
    const string GenHeadActionOff = "Off";
    const string GenHeadActionRandom = "Head Random";
    const string GenHeadPreferredPlugin = "HumanBodyAction";
    const float GenHeadEventCooldownSeconds = 0.50f;
    const float GenHeadInsideCooldownDefault = 3.00f;
    const float GenHeadRandomIntervalMin = 3.00f;
    const float GenHeadRandomIntervalMax = 10.00f;
    const float GenDepthMaxDefault = 0.200f;
    const float GenDepthMaxMin = 0.020f;
    const float GenDepthMaxMax = 0.300f;
    const float GenBodyGateMaxDistanceDefault = 0.650f;
    const float GenBodyGateMaxDistanceMin = 0.100f;
    const float GenBodyGateMaxDistanceMax = 2.000f;
    const float GenDepthHudCameraDistance = 1.25f;
    const float GenDepthHudX = 0.91f;
    const float GenDepthHudBottomY = 0.36f;
    const float GenDepthHudTopY = 0.58f;
    const float GenDepthHudBarWidth = 0.028f;
    const float GenDepthHudBarDepth = 0.006f;
    const float GenDepthHudDisplayMaxPercent = 1.20f;
    const float GenDepthHudPeakHoldSeconds = 1.0f;
    const float GenDepthHudUpperHeartPercent = 1.13f;
    const float GenDepthHudLowerHeartOffset = 0.014f;
    const float GenDepthHudDropOpenStartPercent = 0.010f;
    const float GenDepthHudDropOpenEndPercent = 0.150f;
    const float GenDepthHudDropClosedWidthScale = 0.26f;
    const float GenDepthHudDropOpenWidthScale = 0.76f;
    const float GenDepthHudDropOpenWidthMin = 0.45f;
    const float GenDepthHudDropOpenWidthMax = 1.05f;
    const float GenDepthHudDropHeightScale = 1.34f;
    const float GenDepthHudDropSmoothTime = 0.18f;
    const float GenDepthHudIdlePulseSpeed = 3.0f;
    const float GenDepthHudInsertPulseSpeed = 9.0f;
    const float GenDepthHudIdlePulseStrength = 0.45f;
    const float GenDepthHudDropPulseNarrowScale = 0.040f;
    const float GenDepthHudHeartPulseNarrowScale = 0.040f;
    const float GenDepthHudHeartPulseMultiplier = 2.0f;
    const float GenDepthHudPeakBackOffset = 0.018f;
    const float GenDepthHudDropFrontOffset = 0.014f;
    const float GenDepthHudDropLeftOffset = 0.008f;
    const float GenDepthHudTargetThighBaseline = 0.420f;
    const float GenDepthHudTargetThighMin = 0.250f;
    const float GenDepthHudTargetThighMax = 0.650f;
    const float GenDepthHudBodyScaleMin = 0.880f;
    const float GenDepthHudBodyScaleMax = 1.120f;
    const float GenDepthHudBodyScaleInfluence = 0.35f;
    const float GenDepthHudBurstRatioInfluence = 0.45f;
    const float GenDepthHudBurstRatioMin = 0.85f;
    const float GenDepthHudBurstRatioMax = 1.16f;
    const float GenDepthMaxLateralDistance = 0.120f;
    const float GenDepthAngleGateLimitDegrees = 45.0f;
    const float NowDockingCurrentDistanceMin = 0.400f;
    const float NowDockingCurrentDistanceMax = 3.000f;
    const float NowDockingCurrentFitSideDotMin = 0.900f;
    const float NowDockingNearKeepPlacementDistance = 0.450f;
    const float NowDockingLieNearKeepOrientationDistance = 0.220f;
    const float NowDockingLieNearMicroAdjustMax = 0.080f;
    const float LieDockingSafeDistance = 1.300f;
    const float NowDockingYellowSmartShapeDistance = 1.000f;
    const float GenDepthProbeLogMinInterval = 0.20f;
    const float GenDepthProbeLogHeartbeatInterval = 5.00f;
    const int GenDepthBurstParticleCount = 12;
    const float GenDepthBurstLifetime = 0.70f;
    const float GenDepthBurstCooldownZero = 0.45f;
    const float GenDepthBurstCooldownMax = 0.85f;
    const float GenDepthBurstSize = 0.010f;
    const string PerformanceModeQuality = "Quality";
    const string PerformanceModeBalanced = "Balanced";
    const string PerformanceModeLight = "Light";
    const string DepthProbeRateOn = "Auto";
    const string DepthProbeRateEveryFrame = "Every Frame";
    const string DepthProbeRate30Fps = "30 FPS";
    const string DepthProbeRate20Fps = "20 FPS";
    const string DepthProbeRate10Fps = "10 FPS";
    const string DepthProbeRate5Fps = "5 FPS";
    const string DepthProbeRate2Fps = "2 FPS";
    const string DepthProbeRate1Fps = "1 FPS";
    const string DepthProbeRateHalfFps = "0.5 FPS";
    const string DepthProbeRateQuarterFps = "0.25 FPS";
    const string DepthProbeRateTenthFps = "0.1 FPS";
    const string DepthProbeRateTwentiethFps = "0.05 FPS";
    const string DepthProbeRateOff = "Off";
    const float PerfProbeTimingLogInterval = 1.00f;
    const string FeatureCutModeFull = "Full";
    const string FeatureCutModeNoHud = "No HUD";
    const string FeatureCutModeNoReactions = "No Reactions";
    const string FeatureCutModeMotionOnly = "Motion Only";
    const string FeatureCutModeDockingOnly = "Docking Only";
    const float GenDepthHudSampleInterval = 0.050f;
    const float DebugLineRenderInterval = 0.050f;
    const float GenDepthHudGContactDotBelowScale = 1.45f;
    const float GenDepthHudGContactDotSizeScale = 0.29f;
    const float GenDepthHudGContactDotLeftDotScale = 1.1567f; // v156: micro left from v155 by about 0.12 dot
    const float GenDepthHudGContactDotUpDotScale = 7.76f; // v156: micro down from v155 by about 0.12 dot
    const float GenDepthHudGContactDotForwardOffset = 0.016f;
    const float GenDepthHudGContactDotPulseSpeed = 8.0f;
    const float GenDepthHudGContactDotPulseStrength = 0.06f;
    const float GenDepthHudGContactDotMinAlpha = 1.00f;
    const float GenDepthHudGContactDotMaxAlpha = 1.00f;
    const float GenDepthHudGContactColorLateralRangeScale = 2.20f;
    const float GenDepthHudGContactColorBackDepthRangeScale = 4.00f;
    const float GenDepthHudGContactColorCurvePower = 0.55f;
    const float AnusDepthHudXOffset = 0.045f;
    const float AnusDepthHudWholeLeftBarScale = 1.00f; // v163: move entire anus HUD left by one anus bar width
    const float AnusDepthHudBarWidthScale = 0.78f;
    const float AnusDepthHudMarkerWidthScale = 1.05f;
    const float AnusDepthHudMarkerYPercent = 1.00f;
    const float AnusDepthHudStarBelowScale = 0.35f; // v160: one marker-height up from v159
    const float AnusDepthHudStarForwardOffset = 0.017f;
    const float AnusDepthHudStarClosedSizeScale = 0.20f; // v161: about 2/3 of v159 idle star
    const float AnusDepthHudStarOpenSizeScale = 0.41f; // v161: about 2/3 of v159 maximum
    const float AnusDepthHudStarOpenStartPercent = 0.010f;
    const float AnusDepthHudStarOpenEndPercent = 0.150f;
    const float AnusDepthHudStarPulseSpeed = 8.0f;
    const float AnusDepthHudStarPulseStrength = 0.07f;
    const float AnusDepthHudStarRightScale = -0.47f; // v164: tiny right nudge from v162
    const float AnusDepthPFollowBackDepth = -0.300f; // v165: keep Anus axis follow before contact instead of falling back to yellow
    const float AnusDepthPFollowLateralMax = 0.180f; // v165: wider Anus axis capture zone; genital remains unchanged
    const float MouthDepthPFollowBackDepth = -0.250f; // v167: keep Mouth axis follow before contact instead of falling back to yellow
    const float MouthDepthPFollowLateralMax = 0.160f; // v167: wider Mouth axis capture zone; genital remains unchanged
    const float MouthInsideVerticalScale = 0.50f; // v168: relax mouth vertical damping from 0.25 toward original forward
    const float AnusDepthDirectionFlipDeadZone = 0.030f; // legacy v164 constant; Anus axis no longer flips from own/P side

    GameObject forwardLineObj;
    GameObject moveLineObj;
    GameObject penisPathLineObj;
    GameObject bendMarkerLineObj;
    GameObject gDepthGuideLineObj;
    GameObject genDepthHudBackObj;
    GameObject genDepthHudFillObj;
    GameObject genDepthHudMarkerObj;
    GameObject genDepthHudBottomMarkerObj;
    GameObject genDepthHudPeakObj;
    GameObject genDepthHudGContactDotObj;
    GameObject anusDepthHudBackObj;
    GameObject anusDepthHudFillObj;
    GameObject anusDepthHudMarkerObj;
    GameObject anusDepthHudStarObj;

    LineRenderer forwardLine;
    LineRenderer moveLine;
    LineRenderer penisPathLine;
    LineRenderer bendMarkerLine;
    LineRenderer gDepthGuideLine;
    LineRenderer genDepthHudMarkerLine;
    LineRenderer genDepthHudBottomMarkerLine;
    Material genDepthHudBackMaterial;
    Material genDepthHudFillMaterial;
    Material genDepthHudMarkerMaterial;
    Material genDepthHudPeakMaterial;
    Material genDepthHudGContactDotMaterial;
    Material anusDepthHudBackMaterial;
    Material anusDepthHudFillMaterial;
    Material anusDepthHudMarkerMaterial;
    Material anusDepthHudStarMaterial;
    Material[] genDepthBurstMaterials;
    float genDepthPeakPercent;
    float genDepthPeakUntil;
    float genDepthHudLowerVisualOpenT;
    float genDepthHudLowerVisualOpenVelocity;
    float genDepthInsertedMaxPercent;
    float anusDepthInsertedMaxPercent;
    float previousGenDepthPercent;
    float nextZeroBurstTime;
    float nextMaxBurstTime;
    float lastGenDepthUiTextTime = -999f;
    float lastGenDepthHudSampleTime = -999f;
    float lastGenDepthHudRenderTime = -999f;
    bool cachedHudDepthKnown;
    bool cachedHudHasDepth;
    float cachedHudDepth;
    float cachedHudLength;
    float cachedHudPercent;
    bool cachedAnusHudDepthKnown;
    bool cachedAnusHudHasDepth;
    float cachedAnusHudDepth;
    float cachedAnusHudLength;
    float cachedAnusHudPercent;
    bool genDepthHudActiveKnown;
    bool genDepthHudActive;
    bool genDepthHudVisibleKnown;
    bool genDepthHudVisible;
    bool anusDepthHudActiveKnown;
    bool anusDepthHudActive;
    bool genDepthHudGContactDotActiveKnown;
    bool genDepthHudGContactDotActive;
    Renderer genDepthHudGContactDotRenderer;
    Color genDepthHudGContactDotLastColor;
    bool genDepthHudGContactDotColorKnown;
    float lastDebugLineRenderTime = -999f;
    bool debugLinesEnabledKnown;
    bool debugLinesEnabled;
    readonly Dictionary<GameObject, LineRenderer> hudLineRendererCache = new Dictionary<GameObject, LineRenderer>();
    float lastGenDepthProbeLogTime = -999f;
    bool lastGenDepthProbeLogKnown;
    bool lastGenDepthProbeLateralGated;
    bool lastGenDepthProbeBodyGated;
    bool lastGenDepthProbeAngleGated;
    string genHeadActionCacheAtomUid = "";
    Dictionary<string, string> genHeadActionLocationCache = new Dictionary<string, string>();
    string hbaLinkCacheAtomUid = "";
    JSONStorableFloat hbaTargetIdParam;
    JSONStorableFloat hbaProgressParam;
    JSONStorableBool hbaActiveParam;
    string bodyTouchProbeCacheAtomUid = "";
    JSONStorableBool bodyTouchLabiaParam;
    JSONStorableBool bodyTouchVaginaParam;
    JSONStorableBool bodyTouchDeepVaginaParam;
    JSONStorableBool bodyTouchDeeperVaginaParam;
    float lastHbaSharedStatusTime = -999.0f;
    float hbaLinkLastResolveTime = -999.0f;
    const float HbaSharedStatusInterval = 0.05f;
    const float HbaLinkRefreshInterval = 1.00f;
    int lastGenDepthProbeZone = -999;
    float lastGDepthGuideLineLogTime = -999f;
    bool lastGDepthGuideAngleGateBlocked;
    bool gDepthPFollowApplied;
    float lastGDepthPFollowLogTime = -999f;
    bool lastGDepthAngleGateBlocked;
    bool pMidAxisAssistApplied;
    float lastPMidAxisAssistLogTime = -999f;
    bool pTipOnGContactKnown;
    bool pTipOnGContact;
    bool pBaseLiftAngleGateBlocked;
    float lastPBaseLiftAngleGateLogTime = -999f;
    bool genTgStartActive;
    bool genTgInsideActive;
    bool genTgDeepActive;
    bool genTgHadInside;
    bool genHeadDepthStartActive;
    bool genHeadDepthInsideActive;
    bool genHeadDepthDeepActive;
    bool genHeadDepthHadInside;
    float lastGenHeadDepthSourceLogTime = -999.0f;
    string lastFeatureCutClearKey = "";
    string lastFeatureCutReactionClearKey = "";

    class GenTgRuntime
    {
        public bool lastWrittenKnown;
        public bool lastWrittenValue;
        public string lastAtomUid = "";
        public float timedOffAt = -1.0f;
        public float stateOffAt = -1.0f;
        public float lastFireTime = -999.0f;
    }

    GenTgRuntime genTgStartRuntime = new GenTgRuntime();
    GenTgRuntime genTgInsideRuntime = new GenTgRuntime();
    GenTgRuntime genTgDeepRuntime = new GenTgRuntime();
    GenTgRuntime genTgEndRuntime = new GenTgRuntime();

    float genHeadStartLastFireTime = -999.0f;
    float genHeadInsideLastFireTime = -999.0f;
    float genHeadInsideNextRandomTime = -1.0f;
    float genHeadDeepLastFireTime = -999.0f;
    float genHeadEndLastFireTime = -999.0f;
    readonly Dictionary<string, Atom> atomCache = new Dictionary<string, Atom>();
    readonly Dictionary<string, FreeControllerV3> controllerContainsCache = new Dictionary<string, FreeControllerV3>();
    readonly Dictionary<string, FreeControllerV3> controllerExactCache = new Dictionary<string, FreeControllerV3>();
    readonly Dictionary<string, Transform> childTransformExactCache = new Dictionary<string, Transform>();
    readonly Dictionary<string, Transform> childTransformSuffixCache = new Dictionary<string, Transform>();

    class GenDepthBurstParticle
    {
        public GameObject obj;
        public Vector3 velocity;
        public float startTime;
        public float endTime;
        public float size;
    }

    readonly List<GenDepthBurstParticle> genDepthBurstParticles = new List<GenDepthBurstParticle>();

    class DepthFrameCache
    {
        public int frame = -1;
        public bool known;
        public bool ok;
        public float depth;
        public float length;
        public float percent;
    }

    readonly DepthFrameCache genDepthFrameCache = new DepthFrameCache();
    readonly DepthFrameCache genHudDepthFrameCache = new DepthFrameCache();
    readonly DepthFrameCache anusHudDepthFrameCache = new DepthFrameCache();
    readonly DepthFrameCache anusPushDepthFrameCache = new DepthFrameCache();
    readonly DepthFrameCache mouthPushDepthFrameCache = new DepthFrameCache();

    const int YellowPPathPointCount = 6;
    Vector3[] yellowPPathPoints = new Vector3[YellowPPathPointCount];
    float[] yellowPPathLengths = new float[YellowPPathPointCount];
    float yellowPPathTotalLength;
    float yellowBaseToMidLength;
    float yellowMidToTipLength;
    bool hasYellowPPath;

    bool yellowGuideHasDip;
    bool yellowGuideOwnLieFlat;
    float yellowGuideTargetAxisAngleDeg;

    Vector3 capturedMoveLineStart;
    Vector3 capturedMoveLineEnd;
    bool hasCapturedMoveLine;

    float capturedGreenBaseY;
    bool hasCapturedGreenBaseY;

    Vector3 capturedBodyRootPosition;
    Quaternion capturedBodyRootRotation;
    bool hasCapturedBodyRoot;
    Vector3 capturedBodyHipPosition;
    bool hasCapturedBodyHip;

    bool pYellowCapturePending;
    bool pYellowOriginalCaptured;
    bool pTipYellowShapeLocked;
    Vector3 pTipYellowLockedMidOffset;
    Vector3 pTipYellowLockedTipOffset;
    Vector3 pTipYellowLockedMidTangent;
    Vector3 pTipYellowLockedTipTangent;
    Vector3 pTipYellowLockedBaseTangent;
    float pTipYellowLockedProgress;
    Vector3 savedPBasePosition;
    Vector3 savedPMidPosition;
    Vector3 savedPTipPosition;
    Quaternion savedPBaseRotation;
    Quaternion savedPMidRotation;
    Quaternion savedPTipRotation;
    FreeControllerV3.PositionState savedPBasePositionState;
    FreeControllerV3.PositionState savedPMidPositionState;
    FreeControllerV3.PositionState savedPTipPositionState;
    FreeControllerV3.RotationState savedPBaseRotationState;
    FreeControllerV3.RotationState savedPMidRotationState;
    FreeControllerV3.RotationState savedPTipRotationState;

    public override void Init()
    {
        RefreshPersonList();

        pushButton = CreateButton("PUSH", true);
        CapturePushButtonUi();
        pushButton.button.onClick.AddListener(delegate
        {
            ActionPushP();
        });

        CreateButton("P Mid G Align", true).button.onClick.AddListener(delegate
        {
            ActionPMidGAlign();
        });

        pushAutoMode = new JSONStorableStringChooser(
            "PUSH Auto Mode",
            new List<string>()
            {
                PushModeNone,
                PushModeAutoLine,
                PushModeAutoLineSlow,
                PushModeAutoLineFast,
                PushModeAutoSpiral,
                PushModeAutoDeepStop,
                PushModeAutoRandom
            },
            PushModeNone,
            "PUSH Auto Mode"
        );
        RegisterStringChooser(pushAutoMode);
        CreateScrollablePopup(pushAutoMode, true);

        distance = new JSONStorableFloat(
            "Distance",
            1.0f,
            -1.5f,
            3.0f
        );
        distance.setCallbackFunction = OnPlacementSliderChanged;
        RegisterFloat(distance);
        distanceSlider = CreateSlider(distance, true);

        CreateButton("Now Docking", true).button.onClick.AddListener(delegate
        {
            ActionNowDocking();
        });

        CreateButton("Smart Docking", true).button.onClick.AddListener(delegate
        {
            ActionSmartDocking();
        });

        CreateButton("Reverse Smart Docking", true).button.onClick.AddListener(delegate
        {
            ActionReverseSmartDocking();
        });

        targetPersonChooser = new JSONStorableStringChooser(
            "Target Person",
            personChoices,
            personChoices.Count > 0 ? personChoices[0] : "",
            "Target Person"
        );
        targetPersonChooser.setCallbackFunction = OnTargetPersonChanged;
        RegisterStringChooser(targetPersonChooser);
        CreateScrollablePopup(targetPersonChooser);

        targetControllerChooser = new JSONStorableStringChooser(
            "Target",
            new List<string>()
            {
                "genital",
                "anus",
                "mouth"
            },
            "genital",
            "Target"
        );
        targetControllerChooser.setCallbackFunction = OnTargetControllerChanged;
        RegisterStringChooser(targetControllerChooser);
        CreateScrollablePopup(targetControllerChooser);
        lastTargetControllerMode = targetControllerChooser.val;

        performanceModeChooser = new JSONStorableStringChooser(
            "Performance Mode",
            new List<string>()
            {
                PerformanceModeQuality,
                PerformanceModeBalanced,
                PerformanceModeLight
            },
            PerformanceModeBalanced,
            "Performance Mode"
        );
        RegisterStringChooser(performanceModeChooser);
        CreateScrollablePopup(performanceModeChooser);

        runtimePlacement = new JSONStorableBool("Perf Placement", true);
        RegisterBool(runtimePlacement);

        runtimeUpperLower = new JSONStorableBool("Perf Upper Lower", true);
        RegisterBool(runtimeUpperLower);

        runtimePFollow = new JSONStorableBool("Perf P Follow", true);
        RegisterBool(runtimePFollow);

        runtimeDepthProbe = new JSONStorableBool("Perf Depth Probe", true);
        RegisterBool(runtimeDepthProbe);
        // v202: runtime feature toggles are hidden and forced ON in IsRuntime*Enabled().
        // Depth Probe Rate remains the visible control for probe pacing/off.

        depthProbeRateChooser = new JSONStorableStringChooser(
            "Depth Probe Rate",
            new List<string>()
            {
                DepthProbeRateOn,
                DepthProbeRateEveryFrame,
                DepthProbeRate30Fps,
                DepthProbeRate20Fps,
                DepthProbeRate10Fps,
                DepthProbeRate5Fps,
                DepthProbeRate2Fps,
                DepthProbeRate1Fps,
                DepthProbeRateHalfFps,
                DepthProbeRateQuarterFps,
                DepthProbeRateTenthFps,
                DepthProbeRateTwentiethFps,
                DepthProbeRateOff
            },
            DepthProbeRateOn,
            "Depth Probe Rate"
        );
        depthProbeRateChooser.setCallbackFunction = OnDepthProbeRateChanged;
        RegisterStringChooser(depthProbeRateChooser);
        CreateScrollablePopup(depthProbeRateChooser);

        perfProbeTimingLog = new JSONStorableBool("Perf Probe Timing Log", false);
        RegisterBool(perfProbeTimingLog);
        CreateToggle(perfProbeTimingLog);

        // v201: keep the feature paths, but hide debug toggles that showed no practical runtime value.
        // v205: keep this hidden, but restore it internally so the HUD graph/FX value path stays live.
        // Transform Cache stays forced ON, so this no longer takes the old heavy transform-scan path.
        perfRawHudProbe = new JSONStorableBool("Perf Raw HUD Probe", true);
        RegisterBool(perfRawHudProbe);

        // Raw Event Probe stayed near 0ms in testing, so it is hidden and forced off.
        perfRawEventProbe = new JSONStorableBool("Perf Raw Event Probe", false);
        RegisterBool(perfRawEventProbe);

        // Reaction fallback path is retained, but hidden and normally OFF.
        perfReactionFallbackProbe = new JSONStorableBool("Perf Reaction Fallback Probe", false);
        RegisterBool(perfReactionFallbackProbe);

        // Transform Cache was the decisive win in testing, so keep it always ON and hide the toggle.
        perfTransformCache = new JSONStorableBool("Perf Transform Cache", true);
        RegisterBool(perfTransformCache);

        // Body Gate / Bookkeeping are retained with normal behavior and hidden.
        perfMainBodyGate = new JSONStorableBool("Perf Main Body Gate", true);
        RegisterBool(perfMainBodyGate);

        perfMainBookkeeping = new JSONStorableBool("Perf Main Bookkeeping", true);
        RegisterBool(perfMainBookkeeping);

        runtimeHud = new JSONStorableBool("Perf HUD", true);
        RegisterBool(runtimeHud);

        runtimeReactions = new JSONStorableBool("Perf Reactions", true);
        RegisterBool(runtimeReactions);

        runtimePushAuto = new JSONStorableBool("Perf PUSH Auto", true);
        RegisterBool(runtimePushAuto);

        runtimeDynamicVisuals = new JSONStorableBool("Perf Dynamic Visuals", true);
        RegisterBool(runtimeDynamicVisuals);
        // v202: hidden; keep dynamic visuals enabled under normal operation.

        switchRetractOnTargetChange = new JSONStorableBool(
            "Switch Retract",
            true
        );
        RegisterBool(switchRetractOnTargetChange);
        CreateToggle(switchRetractOnTargetChange, true);

        switchRetractDistance = new JSONStorableFloat(
            "Switch Retract Distance",
            SwitchRetractDistanceDefault,
            SwitchRetractDistanceMin,
            SwitchRetractDistanceMax
        );
        RegisterFloat(switchRetractDistance);
        CreateSlider(switchRetractDistance, true);

        switchRetractTime = new JSONStorableFloat(
            "Switch Retract Time",
            SwitchRetractTimeDefault,
            SwitchRetractTimeMin,
            SwitchRetractTimeMax
        );
        RegisterFloat(switchRetractTime);
        CreateSlider(switchRetractTime, true);

        CreateButton("Refresh Person List").button.onClick.AddListener(delegate
        {
            ActionRefreshPersonList();
        });

        orbitAngle = new JSONStorableFloat(
            "Orbit Angle",
            0.0f,
            -180.0f,
            180.0f
        );
        orbitAngle.setCallbackFunction = OnPlacementSliderChanged;
        RegisterFloat(orbitAngle);
        orbitAngleSlider = CreateSlider(orbitAngle);

        followTarget = new JSONStorableBool(
            "Follow Line",
            true
        );
        followTarget.setCallbackFunction = OnFollowTargetChanged;
        RegisterBool(followTarget);
        CreateToggle(followTarget);

        applyOnSliderChangeOnly = new JSONStorableBool(
            "Apply On Change",
            true
        );
        applyOnSliderChangeOnly.setCallbackFunction = OnApplyModeChanged;
        RegisterBool(applyOnSliderChangeOnly);
        CreateToggle(applyOnSliderChangeOnly);

        yellowButtGuideScale = new JSONStorableFloat(
            "Yellow Butt Guide Scale",
            1.5f,
            0.50f,
            3.00f
        );
        yellowButtGuideScale.setCallbackFunction = OnYellowButtGuideScaleChanged;
        RegisterFloat(yellowButtGuideScale);
        yellowButtGuideScaleSlider = CreateSlider(yellowButtGuideScale, true);

        hipYOffset = new JSONStorableFloat(
            "Hip Y Offset",
            0.0f,
            -0.5f,
            0.5f
        );
        hipYOffset.setCallbackFunction = OnHipYOffsetChanged;
        RegisterFloat(hipYOffset);
        hipYOffsetSlider = CreateSlider(hipYOffset);

        cuddleDepth = new JSONStorableFloat(
            "Cuddle Depth",
            0.6f,
            0.0f,
            2.0f
        );
        RegisterFloat(cuddleDepth);
        // hidden UI: CreateSlider(cuddleDepth, true);

        avoidTargetOnCapture = new JSONStorableBool(
            "Avoid Target On Capture",
            true
        );
        RegisterBool(avoidTargetOnCapture);
        CreateToggle(avoidTargetOnCapture);

        // Avoid sliders are grouped under Avoid Target On Capture.
        avoidCaptureDuration = new JSONStorableFloat(
            "Avoid Duration",
            1.0f,
            0.1f,
            5.0f
        );
        RegisterFloat(avoidCaptureDuration);
        // hidden UI: CreateSlider(avoidCaptureDuration);

        avoidRadius = new JSONStorableFloat(
            "Avoid Radius",
            0.35f,
            0.0f,
            1.5f
        );
        RegisterFloat(avoidRadius);
        // hidden UI: CreateSlider(avoidRadius);

        // Side angle used for two-step avoid movement.
        avoidSideAngle = new JSONStorableFloat(
            "Avoid Side Angle",
            90.0f,
            0.0f,
            180.0f
        );
        RegisterFloat(avoidSideAngle);
        // hidden UI: CreateSlider(avoidSideAngle);

        CreateButton("Upper Body Direction", true).button.onClick.AddListener(delegate
        {
            ActionUpperBodyDirection();
        });

        CreateButton("Load Pose USER Defaults", true).button.onClick.AddListener(delegate
        {
            ActionLoadPoseUserDefaults();
        });

        autoLieOnRidePose = new JSONStorableBool(
            "Auto Lie On Ride Pose",
            true
        );
        RegisterBool(autoLieOnRidePose);
        CreateToggle(autoLieOnRidePose, true);

        autoUpperTilt = new JSONStorableBool(
            "Auto Upper Tilt",
            false
        );
        RegisterBool(autoUpperTilt);
        // hidden UI: CreateToggle(autoUpperTilt, true);

        tiltTriggerAngle = new JSONStorableFloat(
            "Tilt Trigger Angle",
            30.0f,
            0.0f,
            80.0f
        );
        RegisterFloat(tiltTriggerAngle);
        // hidden UI: CreateSlider(tiltTriggerAngle, true);

        upperTiltAngle = new JSONStorableFloat(
            "Upper Tilt Angle",
            30.0f,
            -60.0f,
            60.0f
        );
        RegisterFloat(upperTiltAngle);
        // hidden UI: CreateSlider(upperTiltAngle, true);

        allowDownTilt = new JSONStorableBool(
            "Allow Down Tilt",
            false
        );
        RegisterBool(allowDownTilt);
        // hidden UI: CreateToggle(allowDownTilt, true);

        sitGroundYThreshold = new JSONStorableFloat(
            "Sit Ground Y",
            0.35f,
            0.00f,
            1.00f
        );
        RegisterFloat(sitGroundYThreshold);
        // hidden UI: CreateSlider(sitGroundYThreshold, true);

        showLines = new JSONStorableBool(
            "Debug View",
            false
        );
        RegisterBool(showLines);
        CreateToggle(showLines);

        debugLog = new JSONStorableBool(
            "Debug Log",
            false
        );
        RegisterBool(debugLog);
        CreateToggle(debugLog);

        dynamicRedLine = new JSONStorableBool(
            "Dynamic Red Line",
            true
        );
        RegisterBool(dynamicRedLine);
        CreateToggle(dynamicRedLine);

        redLineUpdateSec = new JSONStorableFloat(
            "Red Line Update Sec",
            0.50f,
            0.10f,
            3.00f
        );
        RegisterFloat(redLineUpdateSec);
        CreateSlider(redLineUpdateSec);

        dynamicYellowEnd = new JSONStorableBool(
            "Dynamic Yellow End",
            false
        );
        RegisterBool(dynamicYellowEnd);
        CreateToggle(dynamicYellowEnd);

        showInsertDebug = new JSONStorableBool(
            "Gen Depth HUD",
            true
        );
        RegisterBool(showInsertDebug);
        CreateToggle(showInsertDebug);

        genDepthHudFx = new JSONStorableBool(
            "HUD FX",
            true
        );
        RegisterBool(genDepthHudFx);
        CreateToggle(genDepthHudFx);

        genDepthMax = new JSONStorableFloat(
            "Gen Depth Max",
            GenDepthMaxDefault,
            GenDepthMaxMin,
            GenDepthMaxMax
        );
        RegisterFloat(genDepthMax);
        CreateSlider(genDepthMax);

        genHudDropMaxWidth = new JSONStorableFloat(
            "Rod Width",
            GenDepthHudDropOpenWidthScale,
            GenDepthHudDropOpenWidthMin,
            GenDepthHudDropOpenWidthMax
        );
        RegisterFloat(genHudDropMaxWidth);
        CreateSlider(genHudDropMaxWidth);

        genBodyGate = new JSONStorableBool(
            "Gen Body Gate",
            true
        );
        RegisterBool(genBodyGate);
        CreateToggle(genBodyGate);

        genBodyGateMaxDistance = new JSONStorableFloat(
            "Gen Body Max Distance",
            GenBodyGateMaxDistanceDefault,
            GenBodyGateMaxDistanceMin,
            GenBodyGateMaxDistanceMax
        );
        RegisterFloat(genBodyGateMaxDistance);
        CreateSlider(genBodyGateMaxDistance);

        pIkOffOnGenBodyGate = new JSONStorableBool(
            "P IK OFF on Body Gate",
            true
        );
        RegisterBool(pIkOffOnGenBodyGate);
        CreateToggle(pIkOffOnGenBodyGate);

        pushDepthScale = new JSONStorableFloat(
            "PUSH Depth Scale",
            PushPDepthScaleDefault,
            PushPDepthScaleMin,
            PushPDepthScaleMax
        );
        RegisterFloat(pushDepthScale);
        CreateSlider(pushDepthScale, true);

        pushAutoGDepthTrigger = new JSONStorableBool("PUSH Auto G Trigger", true);
        pushAutoGDepthTrigger.setCallbackFunction = OnPushAutoGDepthTriggerChanged;
        RegisterBool(pushAutoGDepthTrigger);
        CreateToggle(pushAutoGDepthTrigger, true);

        SetupHbaEventBridgeNoUi();

        insertDebugText = new JSONStorableString(
            "Insert Debug",
            "Gen --% / Max --%"
        );
        RegisterString(insertDebugText);
        UIDynamicTextField insertDebugField = CreateTextField(insertDebugText, true);
        if (insertDebugField != null)
        {
            insertDebugField.height = 45.0f;
        }

        // New test: Distance still moves the root normally, but the upper body
        // lowering amount is sampled from the yellow path Y only.
        upperBodyLowerByYellowPath = new JSONStorableBool(
            "Upper Body Lower By Yellow Guide",
            true
        );
        upperBodyLowerByYellowPath.setCallbackFunction = OnUpperBodyLowerToggleChanged;
        RegisterBool(upperBodyLowerByYellowPath);
        CreateToggle(upperBodyLowerByYellowPath, true);

        upperBodyYellowLowerScale = new JSONStorableFloat(
            "Upper Yellow Lower Scale",
            1.0f,
            0.0f,
            3.0f
        );
        RegisterFloat(upperBodyYellowLowerScale);
        // hidden UI: CreateSlider(upperBodyYellowLowerScale, true);

        pAngleAtYellowP3 = new JSONStorableBool(
            "P Follow Yellow Guide",
            true
        );
        pAngleAtYellowP3.setCallbackFunction = OnPFollowToggleChanged;
        RegisterBool(pAngleAtYellowP3);
        CreateToggle(pAngleAtYellowP3);

        // P Path move UI remains sealed in this build.
        pPathSealed = new JSONStorableBool("P Path Sealed", true);
        RegisterBool(pPathSealed);

        pYellowPathAlign = new JSONStorableBool("P Yellow Path Align", false);
        RegisterBool(pYellowPathAlign);

        pYellowPathAdvance = new JSONStorableFloat("P Yellow Path Advance", 0.0f, 0.0f, 2.00f);
        RegisterFloat(pYellowPathAdvance);

        upperBodyLowerByDistance = new JSONStorableBool("Upper Body Lower By Distance", false);
        RegisterBool(upperBodyLowerByDistance);

        hipLowerByYellowPath = new JSONStorableBool("Hip Lower By Yellow Path", false);
        RegisterBool(hipLowerByYellowPath);

        lowTargetAction = new JSONStorableStringChooser(
            "Low Target Action",
            new List<string>()
            {
                LowTargetActionOff,
                LowTargetActionLegUnlock,
                LowTargetActionSitGroundPose
            },
            LowTargetActionLegUnlock,
            "Low Target Action"
        );
        RegisterStringChooser(lowTargetAction);
        CreateScrollablePopup(lowTargetAction, true);

        limbRestoreDelay = new JSONStorableFloat(
            "IK Restore Delay",
            3.0f,
            0.0f,
            10.0f
        );
        RegisterFloat(limbRestoreDelay);
        // hidden UI: CreateSlider(limbRestoreDelay);

        kneeAfterFootDelay = new JSONStorableFloat(
            "Knee After Foot Delay",
            0.5f,
            0.0f,
            3.0f
        );
        RegisterFloat(kneeAfterFootDelay);
        // hidden UI: CreateSlider(kneeAfterFootDelay);

        CreateButton("Copy Body Direction", true).button.onClick.AddListener(delegate
        {
            ActionCopyBodyDirection();
        });

        CreateButton("Mirror Pose", true).button.onClick.AddListener(delegate
        {
            ActionMirrorPose();
        });

        CreateButton("Sit Ground Pose", true).button.onClick.AddListener(delegate
        {
            ActionSitGroundPose();
        });

        CreateButton("Lie On Back", true).button.onClick.AddListener(delegate
        {
            ActionLieOnBack();
        });

        CreateButton("Lie On Front", true).button.onClick.AddListener(delegate
        {
            ActionLieOnFront();
        });

        CreateDebugLines();
        CreateInsertDebugOverlay();

        SetPlacementControlsInteractable(false);

        RegisterExternalActions();

        LogMessageIfDebug("[TargetLinePerson] Ready / v194 runtime feature split toggles / P Mid G Align button");
    }


    void SetupHbaEventBridgeNoUi()
    {
        // TargetLinePerson should only notify HumanBodyAction.
        // No TG/Head reaction UI is created here; TG_ and reaction routing live in HumanBodyAction.
        genTgTriggers = new JSONStorableBool("Gen TG Triggers", false);
        RegisterBool(genTgTriggers);

        genTgPrefix = new JSONStorableString("TG Prefix", "TG_");
        RegisterString(genTgPrefix);

        genHeadActions = new JSONStorableBool("HBA Events", true);
        RegisterBool(genHeadActions);

        genHeadStartAction = CreateHiddenGenHeadActionChooser("HBA Start Event", "HBA_Event_Start", false);
        genHeadInsideAction = CreateHiddenGenHeadActionChooser("HBA Inside Event", "HBA_Event_Inside", true);
        genHeadDeepAction = CreateHiddenGenHeadActionChooser("HBA Deep Event", "HBA_Event_Deep", false);
        genHeadEndAction = CreateHiddenGenHeadActionChooser("HBA End Event", "HBA_Event_End", false);

        genHeadInsideCooldown = new JSONStorableFloat(
            "HBA Inside Cooldown",
            GenHeadInsideCooldownDefault,
            0.50f,
            10.00f
        );
        RegisterFloat(genHeadInsideCooldown);

        RefreshGenTgAtomList();
        RefreshGenHeadAtomList();
    }

    JSONStorableStringChooser CreateHiddenGenHeadActionChooser(string name, string defaultAction, bool insideChoices)
    {
        JSONStorableStringChooser chooser = new JSONStorableStringChooser(
            name,
            insideChoices ? genHeadInsideActionChoices : genHeadActionChoices,
            defaultAction,
            name
        );
        RegisterStringChooser(chooser);
        return chooser;
    }

    void RegisterExternalActions()
    {
        RegisterAction(new JSONStorableAction("PUSH", ActionPushP));
        RegisterAction(new JSONStorableAction("P Mid G Align", ActionPMidGAlign));
        RegisterAction(new JSONStorableAction("Now Docking", ActionNowDocking));
        RegisterAction(new JSONStorableAction("Smart Docking", ActionSmartDocking));
        RegisterAction(new JSONStorableAction("Reverse Smart Docking", ActionReverseSmartDocking));
        RegisterAction(new JSONStorableAction("Refresh Person List", ActionRefreshPersonList));
        RegisterAction(new JSONStorableAction("Upper Body Direction", ActionUpperBodyDirection));
        RegisterAction(new JSONStorableAction("Load Pose USER Defaults", ActionLoadPoseUserDefaults));
        RegisterAction(new JSONStorableAction("Load Pose User Defaults", ActionLoadPoseUserDefaults));
        RegisterAction(new JSONStorableAction("Copy Body Direction", ActionCopyBodyDirection));
        RegisterAction(new JSONStorableAction("Mirror Pose", ActionMirrorPose));
        RegisterAction(new JSONStorableAction("Sit Ground Pose", ActionSitGroundPose));
        RegisterAction(new JSONStorableAction("Lie On Back", ActionLieOnBack));
        RegisterAction(new JSONStorableAction("Lie On Front", ActionLieOnFront));
    }

    void ActionNowDocking()
    {
        CaptureHorizontalCurrentSide(false);
    }

    void ActionSmartDocking()
    {
        CaptureHorizontalBaseline(false);
    }

    void ActionReverseSmartDocking()
    {
        CaptureHorizontalBaseline(true);
    }

    void ActionRefreshPersonList()
    {
        RefreshPersonList();

        if (targetPersonChooser != null)
        {
            targetPersonChooser.choices = new List<string>(personChoices);

            if (personChoices.Count > 0 && string.IsNullOrEmpty(targetPersonChooser.val))
                targetPersonChooser.val = personChoices[0];
        }

        RefreshGenHeadAtomList();
    }

    void ActionUpperBodyDirection()
    {
        ApplyUpperBodyDirection();
    }

    void ActionLoadPoseUserDefaults()
    {
        LoadPoseUserDefaults();
    }

    void ActionCopyBodyDirection()
    {
        CopyBodyDirectionFromTarget();
    }

    void ActionMirrorPose()
    {
        MirrorPoseLeftRight();
    }

    void ActionSitGroundPose()
    {
        ApplySitGroundPresetPose();
    }

    void ActionLieOnBack()
    {
        ApplyLieOnBackPresetPose();
    }

    void ActionLieOnFront()
    {
        ApplyLieOnFrontPresetPose();
    }

    void CreateGenTgUi()
    {
        genTgTriggers = new JSONStorableBool("Gen TG Triggers", false);
        RegisterBool(genTgTriggers);
        CreateToggle(genTgTriggers, true);

        genTgPrefix = new JSONStorableString("TG Prefix", "TG_");
        genTgPrefix.setCallbackFunction = delegate(string value)
        {
            RefreshGenTgAtomList();
        };
        RegisterString(genTgPrefix);

        CreateButton("Refresh TG List", true).button.onClick.AddListener(delegate
        {
            RefreshGenTgAtomList();
        });

        genTgStartAtom = CreateGenTgAtomChooser("Start TG Atom");
        genTgStartMode = CreateGenTgModeChooser("Start Mode", GenTgModeButtonPulse);

        genTgInsideAtom = CreateGenTgAtomChooser("Inside TG Atom");
        genTgInsideMode = CreateGenTgModeChooser("Inside Mode", GenTgModeState);

        genTgDeepAtom = CreateGenTgAtomChooser("Deep TG Atom");
        genTgDeepMode = CreateGenTgModeChooser("Deep Mode", GenTgModeTimer5);

        genTgEndAtom = CreateGenTgAtomChooser("End TG Atom");
        genTgEndMode = CreateGenTgModeChooser("End Mode", GenTgModeTimer5);

        CreateButton("Log TG Status", true).button.onClick.AddListener(delegate
        {
            LogGenTgStatus();
        });

        RefreshGenTgAtomList();
    }

    JSONStorableStringChooser CreateGenTgAtomChooser(string name)
    {
        JSONStorableStringChooser chooser = new JSONStorableStringChooser(
            name,
            new List<string>(genTgAtomChoices),
            "",
            name
        );
        RegisterStringChooser(chooser);
        CreateScrollablePopup(chooser, true);
        return chooser;
    }

    JSONStorableStringChooser CreateGenTgModeChooser(string name, string defaultMode)
    {
        JSONStorableStringChooser chooser = new JSONStorableStringChooser(
            name,
            genTgModeChoices,
            defaultMode,
            name
        );
        RegisterStringChooser(chooser);
        CreateScrollablePopup(chooser, true);
        return chooser;
    }

    void RefreshGenTgAtomList()
    {
        string startCurrent = genTgStartAtom != null ? genTgStartAtom.val : "";
        string insideCurrent = genTgInsideAtom != null ? genTgInsideAtom.val : "";
        string deepCurrent = genTgDeepAtom != null ? genTgDeepAtom.val : "";
        string endCurrent = genTgEndAtom != null ? genTgEndAtom.val : "";

        ForceAllGenTgOff();
        genTgStartActive = false;
        genTgInsideActive = false;
        genTgDeepActive = false;
        genTgHadInside = false;
        ResetGenHeadReactionState("tg-list-refresh");

        genTgAtomChoices.Clear();
        genTgAtomChoices.Add("");

        string prefix = GetGenTgPrefix();
        int atomCount = 0;
        int matchCount = 0;
        foreach (Atom atom in SuperController.singleton.GetAtoms())
        {
            if (atom == null || string.IsNullOrEmpty(atom.uid))
            {
                continue;
            }

            atomCount++;
            if (IsGenTgAtomCandidate(atom, prefix) && !genTgAtomChoices.Contains(atom.uid))
            {
                genTgAtomChoices.Add(atom.uid);
                matchCount++;
            }
        }

        genTgAtomChoices.Sort(StringComparer.OrdinalIgnoreCase);

        ApplyGenTgAtomChoices(genTgStartAtom, startCurrent, "Start");
        ApplyGenTgAtomChoices(genTgInsideAtom, insideCurrent, "Inside");
        ApplyGenTgAtomChoices(genTgDeepAtom, deepCurrent, "Deep");
        ApplyGenTgAtomChoices(genTgEndAtom, endCurrent, "End");

        LogMessageIfDebug(
            "[TargetLinePerson] Gen TG list refreshed" +
            " / prefix=" + prefix +
            " / found=" + matchCount +
            " / allAtoms=" + atomCount
        );
    }

    void ApplyGenTgAtomChoices(JSONStorableStringChooser chooser, string current, string suffix)
    {
        if (chooser == null)
        {
            return;
        }

        chooser.choices = new List<string>(genTgAtomChoices);

        if (!string.IsNullOrEmpty(current) && genTgAtomChoices.Contains(current))
        {
            chooser.valNoCallback = current;
            return;
        }

        string defaultName = GetGenTgPrefix() + suffix;
        if (genTgAtomChoices.Contains(defaultName))
        {
            chooser.valNoCallback = defaultName;
            return;
        }

        chooser.valNoCallback = "";
    }

    bool IsGenTgAtomCandidate(Atom atom, string prefix)
    {
        if (atom == null || string.IsNullOrEmpty(prefix))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(atom.uid) && atom.uid.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (!string.IsNullOrEmpty(atom.name) && atom.name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    string GetGenTgPrefix()
    {
        return genTgPrefix != null && !string.IsNullOrEmpty(genTgPrefix.val) ? genTgPrefix.val : "TG_";
    }

    void CreateGenHeadUi()
    {
        genHeadActions = new JSONStorableBool("Gen Head Actions", true);
        RegisterBool(genHeadActions);
        CreateToggle(genHeadActions, true);

        RefreshGenHeadAtomChoices();

        genHeadAtom = new JSONStorableStringChooser(
            "Head Plugin Atom",
            new List<string>(genHeadAtomChoices),
            GenHeadAtomTargetPerson,
            "Head Plugin Atom"
        );
        RegisterStringChooser(genHeadAtom);
        CreateScrollablePopup(genHeadAtom, true);

        genHeadStartAction = CreateGenHeadActionChooser("Start Head Action", "HBA_Event_Start");
        genHeadInsideAction = CreateGenHeadInsideActionChooser("Inside Head Action", "HBA_Event_Inside");
        genHeadDeepAction = CreateGenHeadActionChooser("Deep Head Action", "HBA_Event_Deep");
        genHeadEndAction = CreateGenHeadActionChooser("End Head Action", "HBA_Event_End");

        genHeadInsideCooldown = new JSONStorableFloat(
            "Inside Head Cooldown",
            GenHeadInsideCooldownDefault,
            0.50f,
            10.00f
        );
        RegisterFloat(genHeadInsideCooldown);
        CreateSlider(genHeadInsideCooldown, true);

        CreateButton("Refresh Head List", true).button.onClick.AddListener(delegate
        {
            RefreshGenHeadAtomList();
        });

        CreateButton("Log Head Status", true).button.onClick.AddListener(delegate
        {
            LogGenHeadStatus();
        });

        CreateButton("Test HBA_Event_Inside", true).button.onClick.AddListener(delegate
        {
            TestGenHeadAction("HBA_Event_Inside");
        });

        CreateButton("Test HBA_Event_Deep", true).button.onClick.AddListener(delegate
        {
            TestGenHeadAction("HBA_Event_Deep");
        });

        RefreshGenHeadAtomList();
    }

    JSONStorableStringChooser CreateGenHeadActionChooser(string name, string defaultAction)
    {
        JSONStorableStringChooser chooser = new JSONStorableStringChooser(
            name,
            genHeadActionChoices,
            defaultAction,
            name
        );
        RegisterStringChooser(chooser);
        CreateScrollablePopup(chooser, true);
        return chooser;
    }

    JSONStorableStringChooser CreateGenHeadInsideActionChooser(string name, string defaultAction)
    {
        JSONStorableStringChooser chooser = new JSONStorableStringChooser(
            name,
            genHeadInsideActionChoices,
            defaultAction,
            name
        );
        RegisterStringChooser(chooser);
        CreateScrollablePopup(chooser, true);
        return chooser;
    }

    void RefreshGenHeadAtomChoices()
    {
        genHeadAtomChoices.Clear();
        genHeadAtomChoices.Add(GenHeadAtomTargetPerson);

        foreach (string personUid in personChoices)
        {
            if (!string.IsNullOrEmpty(personUid) && !genHeadAtomChoices.Contains(personUid))
            {
                genHeadAtomChoices.Add(personUid);
            }
        }
    }

    void RefreshGenHeadAtomList()
    {
        string current = genHeadAtom != null ? genHeadAtom.val : GenHeadAtomTargetPerson;
        RefreshGenHeadAtomChoices();

        if (genHeadAtom != null)
        {
            genHeadAtom.choices = new List<string>(genHeadAtomChoices);
            genHeadAtom.valNoCallback = genHeadAtomChoices.Contains(current) ? current : GenHeadAtomTargetPerson;
        }

        Atom atom = GetGenHeadTargetAtom();
        bool found = atom != null && HasGenHeadActionPlugin(atom);
        LogMessageIfDebug(
            "[TargetLinePerson] Gen Head list refreshed" +
            " / atom=" + (atom != null ? atom.uid : "") +
            " / pluginFound=" + found
        );
    }

    Atom GetGenHeadTargetAtom()
    {
        // v171: BodyTwitcher reactions are resolved against the current Target Person by default.
        // This avoids confusion where Head Plugin Atom points somewhere else and Tw_ actions never fire.
        if (targetPersonChooser != null && !string.IsNullOrEmpty(targetPersonChooser.val))
        {
            return FindAtom(targetPersonChooser.val);
        }

        if (genHeadAtom == null || string.IsNullOrEmpty(genHeadAtom.val) || genHeadAtom.val == GenHeadAtomTargetPerson)
        {
            return null;
        }

        return FindAtom(genHeadAtom.val);
    }

    bool HasGenHeadActionPlugin(Atom atom)
    {
        if (atom == null)
        {
            return false;
        }

        foreach (string storableId in atom.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId))
            {
                continue;
            }

            JSONStorable storable = atom.GetStorableByID(storableId);
            if (storable == null)
            {
                continue;
            }

            for (int i = 0; i < genHeadActionChoices.Count; i++)
            {
                string actionName = genHeadActionChoices[i];
                if (actionName == GenHeadActionOff)
                {
                    continue;
                }

                if (storable.GetAction(actionName) != null)
                {
                    return true;
                }
            }

            if (storable.GetAction(GenHeadActionRandom) != null)
            {
                return true;
            }
        }

        return false;
    }

    void Update()
    {
        if (!captured)
        {
            CancelDelayedGuideRefresh("not captured");
            hasDynamicRedLineDisplay = false;
            dynamicYellowEndFrozen = false;
            ResetPushAutoGDepthTriggerState("not captured");
            UpdateDebugLines(false);
            UpdateInsertDebugText();
            return;
        }

        if (IsApplyOnSliderChangeOnly())
        {
            if (followTarget == null || !followTarget.val || isAvoidMoving || targetSwitchRetractBusy)
            {
                ResetUpperBodyLowerIfApplied("follow off or avoid or switch retract");
                ResetPAngleAtYellowP3IfApplied("follow off or avoid or switch retract");
                CancelDelayedGuideRefresh("follow off or avoid or switch retract");
            }
            else
            {
                ProcessDelayedGuideRefresh();
            }

            if (!IsFeatureCutNoDynamicVisuals())
            {
                UpdateDynamicRedLineDisplayIfNeeded();
            }
            else
            {
                hasDynamicRedLineDisplay = false;
                dynamicYellowEndFrozen = false;
            }

            if (!IsFeatureCutNoPushAuto())
            {
                if (ShouldRunDepthProbeNow())
                {
                    UpdatePushAutoGDepthTrigger();
                }
            }
            else
            {
                ResetPushAutoGDepthTriggerState("feature cut no push auto");
            }

            UpdateDebugLines(!IsFeatureCutNoDynamicVisuals() && showLines.val);
            UpdateInsertDebugText();
            return;
        }

        CancelDelayedGuideRefresh("continuous apply mode");

        if (followTarget.val && !isAvoidMoving && !targetSwitchRetractBusy)
        {
            if (IsRuntimePlacementEnabled())
            {
                ApplyPlacement();
            }

            if (IsRuntimeUpperLowerEnabled())
            {
                ApplyUpperBodyLowerByYellowPathIfNeeded("update");
            }
            else
            {
                ResetUpperBodyLowerIfApplied("perf upper lower off");
            }

            if (IsRuntimePFollowEnabled())
            {
                ApplyPAngleAtYellowP3IfNeeded("update");
            }
            else
            {
                ResetPAngleAtYellowP3IfApplied("perf p follow off");
            }
        }
        else
        {
            ResetUpperBodyLowerIfApplied("follow off or avoid or switch retract");
            ResetPAngleAtYellowP3IfApplied("follow off or avoid or switch retract");
        }

        if (!IsFeatureCutNoDynamicVisuals())
        {
            UpdateDynamicRedLineDisplayIfNeeded();
        }
        else
        {
            hasDynamicRedLineDisplay = false;
            dynamicYellowEndFrozen = false;
        }

        if (!IsFeatureCutNoPushAuto())
        {
            if (ShouldRunDepthProbeNow())
            {
                UpdatePushAutoGDepthTrigger();
            }
        }
        else
        {
            ResetPushAutoGDepthTriggerState("feature cut no push auto");
        }

        UpdateDebugLines(!IsFeatureCutNoDynamicVisuals() && showLines.val);
        UpdateInsertDebugText();
    }

    bool IsApplyOnSliderChangeOnly()
    {
        return applyOnSliderChangeOnly != null && applyOnSliderChangeOnly.val;
    }

    string GetPerformanceMode()
    {
        if (performanceModeChooser == null || string.IsNullOrEmpty(performanceModeChooser.val))
        {
            return PerformanceModeBalanced;
        }

        return performanceModeChooser.val;
    }

    bool IsRuntimePlacementEnabled()
    {
        // v202: hidden runtime perf toggle; normal placement remains enabled.
        return true;
    }

    bool IsRuntimeUpperLowerEnabled()
    {
        // v202: hidden runtime perf toggle; normal upper-lower processing remains enabled.
        return true;
    }

    bool IsRuntimePFollowEnabled()
    {
        // v202: hidden runtime perf toggle; normal P-follow processing remains enabled.
        return true;
    }

    bool IsRuntimeDepthProbeEnabled()
    {
        // v202: hidden runtime perf toggle; Depth Probe Rate is the visible on/off/pacing control.
        return !IsDepthProbeRateOff();
    }

    bool IsDepthProbeRateOff()
    {
        return depthProbeRateChooser != null && depthProbeRateChooser.val == DepthProbeRateOff;
    }

    float GetDepthProbeInterval()
    {
        if (depthProbeRateChooser == null || string.IsNullOrEmpty(depthProbeRateChooser.val))
        {
            return GetAutoDepthProbeIntervalFromPerformanceMode();
        }

        string mode = depthProbeRateChooser.val;
        if (mode == DepthProbeRateOn || mode == "ON") return GetAutoDepthProbeIntervalFromPerformanceMode();
        if (mode == DepthProbeRateEveryFrame) return 0.0f;
        if (mode == DepthProbeRate30Fps) return 1.0f / 30.0f;
        if (mode == DepthProbeRate20Fps) return 1.0f / 20.0f;
        if (mode == DepthProbeRate10Fps) return 1.0f / 10.0f;
        if (mode == DepthProbeRate5Fps) return 1.0f / 5.0f;
        if (mode == DepthProbeRate2Fps) return 1.0f / 2.0f;
        if (mode == DepthProbeRate1Fps) return 1.0f;
        if (mode == DepthProbeRateHalfFps) return 2.0f;
        if (mode == DepthProbeRateQuarterFps) return 4.0f;
        if (mode == DepthProbeRateTenthFps) return 10.0f;
        if (mode == DepthProbeRateTwentiethFps) return 20.0f;
        if (mode == DepthProbeRateOff) return 999999.0f;
        return GetAutoDepthProbeIntervalFromPerformanceMode();
    }

    float GetAutoDepthProbeIntervalFromPerformanceMode()
    {
        string mode = GetPerformanceMode();
        if (mode == PerformanceModeQuality)
        {
            return 0.0f; // follow the rendered frame as closely as possible
        }
        if (mode == PerformanceModeLight)
        {
            return 1.0f / 5.0f;
        }
        return 1.0f / 30.0f; // Balanced: fast enough for HUD/screen-following without every-frame probing
    }

    string GetDepthProbeRateLogLabel()
    {
        if (depthProbeRateChooser == null || string.IsNullOrEmpty(depthProbeRateChooser.val))
        {
            return DepthProbeRateOn + "(" + GetPerformanceMode() + ")";
        }
        if (depthProbeRateChooser.val == DepthProbeRateOn || depthProbeRateChooser.val == "ON")
        {
            return DepthProbeRateOn + "(" + GetPerformanceMode() + ")";
        }
        return depthProbeRateChooser.val;
    }

    bool ShouldRunDepthProbeNow()
    {
        if (lastDepthProbeDecisionFrame == Time.frameCount)
        {
            return lastDepthProbeDecisionRun;
        }

        lastDepthProbeDecisionFrame = Time.frameCount;

        float interval = GetDepthProbeInterval();
        if (interval <= 0.0001f)
        {
            lastDepthProbeUpdateTime = Time.time;
            lastDepthProbeDecisionRun = true;
            return true;
        }

        if (Time.time - lastDepthProbeUpdateTime >= interval)
        {
            lastDepthProbeUpdateTime = Time.time;
            lastDepthProbeDecisionRun = true;
            return true;
        }

        lastDepthProbeDecisionRun = false;
        return false;
    }

    void OnDepthProbeRateChanged(string value)
    {
        lastDepthProbeUpdateTime = -999.0f;
        lastDepthProbeDecisionFrame = -1;
        lastDepthProbeDecisionRun = true;
        ClearRuntimeFeatureOutputs("depth-probe-rate-changed");
        if (IsDebugLogEnabled())
        {
            SuperController.LogMessage("[TargetLinePerson] Depth Probe Rate = " + value);
        }
    }

    bool IsPerfProbeTimingLogEnabled()
    {
        return perfProbeTimingLog != null && perfProbeTimingLog.val;
    }

    bool IsRuntimeRawHudProbeEnabled()
    {
        // v205: hidden and forced ON so the HUD graph / HUD FX sample path stays active.
        // Perf Transform Cache remains forced ON, which avoids the old expensive transform scan.
        return true;
    }

    bool IsRuntimeRawEventProbeEnabled()
    {
        // v199: hidden and forced off because it did not move eventProbe timing in tests.
        return false;
    }

    bool IsRuntimeReactionFallbackProbeEnabled()
    {
        // v205: hidden and normally OFF. Keep fallback code available, but do not run it in normal operation.
        return false;
    }

    bool IsPerfTransformCacheEnabled()
    {
        // v200: always ON. Disabling it caused the heavy transform scan path.
        return true;
    }

    bool IsPerfMainBodyGateEnabled()
    {
        // v200: keep normal behavior; no useful perf difference was observed.
        return true;
    }

    bool IsPerfMainBookkeepingEnabled()
    {
        // v200: keep normal behavior; no useful perf difference was observed.
        return true;
    }

    double PerfNow()
    {
        return (double)System.Diagnostics.Stopwatch.GetTimestamp() / (double)System.Diagnostics.Stopwatch.Frequency;
    }

    float PerfMs(double start)
    {
        return Mathf.Max(0.0f, (float)((PerfNow() - start) * 1000.0));
    }

    void LogDepthProbePerfTiming(float totalMs, float mainProbeMs, float hudSampleMs, float eventProbeMs, float hudRenderMs, float reactionMs)
    {
        if (!IsPerfProbeTimingLogEnabled())
        {
            return;
        }

        if (Time.time - lastPerfProbeTimingLogTime < PerfProbeTimingLogInterval)
        {
            return;
        }
        lastPerfProbeTimingLogTime = Time.time;

        string rate = GetDepthProbeRateLogLabel();
        SuperController.LogMessage(
            "[TargetLinePerson] [PERF PROBE]" +
            " rate=" + rate +
            " / rawHud=" + (IsRuntimeRawHudProbeEnabled() ? "1" : "0") +
            " / fallback=" + (IsRuntimeReactionFallbackProbeEnabled() ? "1" : "0") +
            " / tCache=1" +
            " / total=" + totalMs.ToString("F2") + "ms" +
            " / main=" + mainProbeMs.ToString("F2") + "ms" +
            " / mainLine=" + lastPerfMainLineMs.ToString("F2") + "ms" +
            " / mainCalc=" + lastPerfMainCalcMs.ToString("F2") + "ms" +
            " / mainGate=" + lastPerfMainGateMs.ToString("F2") + "ms" +
            " / mainBook=" + lastPerfMainBookMs.ToString("F2") + "ms" +
            " / hudSample=" + hudSampleMs.ToString("F2") + "ms" +
            " / eventProbe=" + eventProbeMs.ToString("F2") + "ms" +
            " / hudRender=" + hudRenderMs.ToString("F2") + "ms" +
            " / reactions=" + reactionMs.ToString("F2") + "ms"
        );
    }

    bool IsRuntimeHudEnabled()
    {
        // v202: hidden runtime perf toggle; HUD remains enabled.
        return true;
    }

    bool IsRuntimeReactionsEnabled()
    {
        // v202: hidden runtime perf toggle; reactions remain enabled.
        return true;
    }

    bool IsRuntimePushAutoEnabled()
    {
        // v202: hidden runtime perf toggle; PUSH Auto remains enabled.
        return true;
    }

    bool IsRuntimeDynamicVisualsEnabled()
    {
        // v202: hidden runtime perf toggle; dynamic visuals remain enabled.
        return true;
    }

    string GetFeatureCutMode()
    {
        // Legacy name kept internally so the existing clear/log keys remain simple.
        // UI is now split into individual Perf toggles instead of a Feature Cut Mode combo.
        return "PL" + (IsRuntimePlacementEnabled() ? "1" : "0")
            + "UL" + (IsRuntimeUpperLowerEnabled() ? "1" : "0")
            + "PF" + (IsRuntimePFollowEnabled() ? "1" : "0")
            + "DP" + (IsRuntimeDepthProbeEnabled() ? "1" : "0")
            + "H" + (IsRuntimeHudEnabled() ? "1" : "0")
            + "R" + (IsRuntimeReactionsEnabled() ? "1" : "0")
            + "PA" + (IsRuntimePushAutoEnabled() ? "1" : "0")
            + "V" + (IsRuntimeDynamicVisualsEnabled() ? "1" : "0");
    }

    bool IsFeatureCutDockingOnly()
    {
        return !IsRuntimePlacementEnabled()
            && !IsRuntimeUpperLowerEnabled()
            && !IsRuntimePFollowEnabled()
            && !IsRuntimeDepthProbeEnabled()
            && !IsRuntimeHudEnabled()
            && !IsRuntimeReactionsEnabled()
            && !IsRuntimePushAutoEnabled()
            && !IsRuntimeDynamicVisualsEnabled();
    }

    bool IsFeatureCutMotionOnly()
    {
        return !IsRuntimeHudEnabled()
            && !IsRuntimeReactionsEnabled()
            && !IsRuntimePushAutoEnabled()
            && !IsRuntimeDepthProbeEnabled();
    }

    bool IsFeatureCutNoHud()
    {
        return !IsRuntimeHudEnabled() || !IsRuntimeDepthProbeEnabled();
    }

    bool IsFeatureCutNoReactions()
    {
        return !IsRuntimeReactionsEnabled() || !IsRuntimeDepthProbeEnabled();
    }

    bool IsFeatureCutNoPushAuto()
    {
        return !IsRuntimePushAutoEnabled() || !IsRuntimeDepthProbeEnabled();
    }

    bool IsFeatureCutNoDynamicVisuals()
    {
        return !IsRuntimeDynamicVisualsEnabled();
    }

    bool IsFeatureCutNoDepthProbe()
    {
        return !IsRuntimeDepthProbeEnabled();
    }

    void OnRuntimeFeatureToggleChanged(bool value)
    {
        lastDepthProbeUpdateTime = -999.0f;
        lastDepthProbeDecisionFrame = -1;
        lastDepthProbeDecisionRun = true;
        lastFeatureCutClearKey = "";
        lastFeatureCutReactionClearKey = "";
        ClearRuntimeFeatureOutputs("runtime-feature-toggle-changed");
        if (IsDebugLogEnabled())
        {
            LogMessageIfDebug("[TargetLinePerson] Runtime feature toggles changed / key=" + GetFeatureCutMode());
        }
    }

    void ClearRuntimeFeatureOutputs(string reason)
    {
        string key = GetFeatureCutMode() + "/" + reason;
        if (lastFeatureCutClearKey == key)
        {
            return;
        }
        lastFeatureCutClearKey = key;

        ForceAllGenTgOff();
        genTgStartActive = false;
        genTgInsideActive = false;
        genTgDeepActive = false;
        genTgHadInside = false;
        ResetGenHeadReactionState(reason);
        ResetPushAutoGDepthTriggerState(reason);
        cachedHudDepthKnown = false;
        cachedAnusHudDepthKnown = false;
        if (IsFeatureCutNoHud())
        {
            UpdateDepthHudThrottled(0.0f, GetGenDepthMax(), 0.0f, 0.0f, GetGenDepthMax(), 0.0f, false);
        }
        if (insertDebugText != null && IsFeatureCutMotionOnly())
        {
            string text = "Perf Toggles: " + GetFeatureCutMode();
            if (insertDebugText.val != text)
            {
                insertDebugText.val = text;
            }
        }
    }

    void ClearReactionOutputsOnce(string reason)
    {
        string key = GetFeatureCutMode() + "/" + reason;
        if (lastFeatureCutReactionClearKey == key)
        {
            return;
        }
        lastFeatureCutReactionClearKey = key;

        ForceAllGenTgOff();
        genTgStartActive = false;
        genTgInsideActive = false;
        genTgDeepActive = false;
        genTgHadInside = false;
        ResetGenHeadReactionState(reason);
    }

    float GetHudSampleInterval()
    {
        string mode = GetPerformanceMode();
        if (mode == PerformanceModeQuality)
        {
            return 0.016f;
        }
        if (mode == PerformanceModeLight)
        {
            return 0.120f;
        }
        return 0.033f; // Balanced: HUD should keep up with the visible screen
    }

    float GetDebugLineRenderInterval()
    {
        string mode = GetPerformanceMode();
        if (mode == PerformanceModeQuality)
        {
            return 0.016f;
        }
        if (mode == PerformanceModeLight)
        {
            return 0.150f;
        }
        return 0.033f;
    }

    float GetDynamicRedLineMinInterval()
    {
        string mode = GetPerformanceMode();
        if (mode == PerformanceModeQuality)
        {
            return 0.033f;
        }
        if (mode == PerformanceModeLight)
        {
            return 0.350f;
        }
        return 0.050f;
    }

    float GetGenDepthUiTextInterval()
    {
        string mode = GetPerformanceMode();
        if (mode == PerformanceModeQuality)
        {
            return 0.050f;
        }
        if (mode == PerformanceModeLight)
        {
            return 0.500f;
        }
        return 0.075f;
    }

    float GetHbaSharedStatusInterval()
    {
        string mode = GetPerformanceMode();
        if (mode == PerformanceModeQuality)
        {
            return 0.033f;
        }
        if (mode == PerformanceModeLight)
        {
            return 0.120f;
        }
        return 0.050f;
    }

    void SetPlacementControlsInteractable(bool interactable)
    {
        if (orbitAngleSlider != null && orbitAngleSlider.slider != null)
        {
            orbitAngleSlider.slider.interactable = interactable;
        }

        if (distanceSlider != null && distanceSlider.slider != null)
        {
            distanceSlider.slider.interactable = interactable;
        }

        if (hipYOffsetSlider != null && hipYOffsetSlider.slider != null)
        {
            hipYOffsetSlider.slider.interactable = interactable;
        }

        if (yellowButtGuideScaleSlider != null && yellowButtGuideScaleSlider.slider != null)
        {
            yellowButtGuideScaleSlider.slider.interactable = interactable;
        }
    }

    void ApplyCapturedPlacementOnce(string reason)
    {
        ApplyCapturedPlacementOnce(reason, false);
    }

    void ApplyCapturedPlacementOnce(string reason, bool rebuildGuide)
    {
        if (isCapturing)
        {
            return;
        }

        if (!captured)
        {
            return;
        }

        if (followTarget == null || !followTarget.val || isAvoidMoving || targetSwitchRetractBusy)
        {
            ResetUpperBodyLowerIfApplied(reason + " inactive");
            ResetPAngleAtYellowP3IfApplied(reason + " inactive");
            CancelDelayedGuideRefresh(reason + " inactive");
            UpdateDebugLines(showLines != null && showLines.val);
            return;
        }
        ApplyPlacement();
        RequestDelayedGuideRefresh(reason, rebuildGuide);
        UpdateDebugLines(showLines != null && showLines.val);
    }

    void RequestDelayedGuideRefresh(string reason, bool rebuildGuide)
    {
        if (!captured)
        {
            return;
        }

        if (rebuildGuide && delayedLineLockRoutine != null)
        {
            StopCoroutine(delayedLineLockRoutine);
            delayedLineLockRoutine = null;
        }

        delayedGuideRefreshPending = true;
        delayedGuideRefreshFrames = DelayedGuideRefreshFrameCount;
        delayedGuideRefreshRebuildGuide = rebuildGuide;
        delayedGuideRefreshReason = reason;

        pYellowCapturePending = true;

        if (rebuildGuide)
        {
            hasYellowPPath = false;
            hasCapturedMoveLine = false;
            hasCapturedGreenBaseY = false;
            ClearTipYellowParallelLock();
            lastUpperBodyYellowLowerPhase = "";
            lastLoggedUpperBodyYellowProgress = -1f;
        }
    }

    void CancelDelayedGuideRefresh(string reason)
    {
        if (!delayedGuideRefreshPending)
        {
            return;
        }

        delayedGuideRefreshPending = false;
        delayedGuideRefreshFrames = 0;
        delayedGuideRefreshRebuildGuide = false;
        delayedGuideRefreshReason = "";
        pYellowCapturePending = false;
    }

    void ProcessDelayedGuideRefresh()
    {
        if (!delayedGuideRefreshPending)
        {
            return;
        }

        if (!captured || followTarget == null || !followTarget.val || isAvoidMoving)
        {
            ResetUpperBodyLowerIfApplied("delayed guide inactive");
            ResetPAngleAtYellowP3IfApplied("delayed guide inactive");
            CancelDelayedGuideRefresh("inactive");
            return;
        }

        delayedGuideRefreshFrames--;
        if (delayedGuideRefreshFrames > 0)
        {
            return;
        }

        if (delayedLineLockRoutine != null && !delayedGuideRefreshRebuildGuide)
        {
            return;
        }

        string reason = delayedGuideRefreshReason;

        if (delayedGuideRefreshRebuildGuide || !hasYellowPPath || !hasCapturedMoveLine)
        {
            if (delayedGuideRefreshRebuildGuide)
            {
                hasYellowPPath = false;
                hasCapturedMoveLine = false;
                hasCapturedGreenBaseY = false;
            }

            BuildCapturedYellowPPath();
        }

        pYellowCapturePending = false;
        delayedGuideRefreshPending = false;
        delayedGuideRefreshFrames = 0;
        delayedGuideRefreshRebuildGuide = false;
        delayedGuideRefreshReason = "";

        if (hasYellowPPath && hasCapturedMoveLine)
        {
            if (IsRuntimeUpperLowerEnabled())
            {
                ApplyUpperBodyLowerByYellowPathIfNeeded("delayed " + reason);
            }
            else
            {
                ResetUpperBodyLowerIfApplied("perf upper lower off");
            }

            if (IsRuntimePFollowEnabled())
            {
                ApplyPAngleAtYellowP3IfNeeded("delayed " + reason);
            }
            else
            {
                ResetPAngleAtYellowP3IfApplied("perf p follow off");
            }
        }
        else
        {
            ResetUpperBodyLowerIfApplied("delayed guide failed");
            ResetPAngleAtYellowP3IfApplied("delayed guide failed");
        }
    }

    void OnPlacementSliderChanged(float value)
    {
        ApplyCapturedPlacementOnce("slider changed");
    }

    void OnFollowTargetChanged(bool value)
    {
        if (value)
        {
            ApplyCapturedPlacementOnce("follow on");
        }
        else
        {
            ResetUpperBodyLowerIfApplied("follow off");
            ResetPAngleAtYellowP3IfApplied("follow off");
            CancelDelayedGuideRefresh("follow off");
            UpdateDebugLines(showLines != null && showLines.val);
        }
    }

    void OnApplyModeChanged(bool value)
    {
        if (value)
        {
            ApplyCapturedPlacementOnce("slider-only mode on");
        }
        else
        {
            CancelDelayedGuideRefresh("slider-only mode off");
        }
    }

    void OnPushAutoGDepthTriggerChanged(bool value)
    {
        ResetPushAutoGDepthTriggerState("toggle changed");
    }

    void OnUpperBodyLowerToggleChanged(bool value)
    {
        if (value)
        {
            ApplyCapturedPlacementOnce("upper body lower on");
        }
        else
        {
            ResetUpperBodyLowerIfApplied("upper body lower off");
            CancelDelayedGuideRefresh("upper body lower off");
        }
    }

    void OnPFollowToggleChanged(bool value)
    {
        if (value)
        {
            ApplyCapturedPlacementOnce("p follow on");
        }
        else
        {
            ResetPAngleAtYellowP3IfApplied("p follow off");
            CancelDelayedGuideRefresh("p follow off");
        }
    }

    void RefreshPersonList()
    {
        ClearLookupCaches();
        personChoices.Clear();

        foreach (Atom a in SuperController.singleton.GetAtoms())
        {
            if (a == null) continue;
            if (a.type != "Person") continue;
            if (a == containingAtom) continue;

            personChoices.Add(a.uid);
        }
    }

    Atom FindAtom(string uid)
    {
        if (string.IsNullOrEmpty(uid))
        {
            return null;
        }

        Atom cached;
        if (atomCache.TryGetValue(uid, out cached))
        {
            if (cached != null && cached.uid == uid)
            {
                return cached;
            }
            atomCache.Remove(uid);
        }

        foreach (Atom a in SuperController.singleton.GetAtoms())
        {
            if (a != null && a.uid == uid)
            {
                atomCache[uid] = a;
                return a;
            }
        }

        return null;
    }

    FreeControllerV3 FindController(Atom atom, string keyword)
    {
        if (atom == null || atom.freeControllers == null)
        {
            return null;
        }

        string key = keyword.ToLower();
        string cacheKey = atom.uid + "|" + key;
        FreeControllerV3 cached;
        if (controllerContainsCache.TryGetValue(cacheKey, out cached))
        {
            if (cached != null && cached.name != null && cached.name.ToLower().Contains(key))
            {
                return cached;
            }
            controllerContainsCache.Remove(cacheKey);
        }

        foreach (FreeControllerV3 fc in atom.freeControllers)
        {
            if (fc == null || fc.name == null) continue;

            if (fc.name.ToLower().Contains(key))
            {
                controllerContainsCache[cacheKey] = fc;
                return fc;
            }
        }

        return null;
    }

    FreeControllerV3 FindControllerExact(Atom atom, string controllerName)
    {
        if (atom == null || atom.freeControllers == null)
        {
            return null;
        }

        string cacheKey = atom.uid + "|" + controllerName;
        FreeControllerV3 cached;
        if (controllerExactCache.TryGetValue(cacheKey, out cached))
        {
            if (cached != null && cached.name == controllerName)
            {
                return cached;
            }
            controllerExactCache.Remove(cacheKey);
        }

        foreach (FreeControllerV3 fc in atom.freeControllers)
        {
            if (fc != null && fc.name == controllerName)
            {
                controllerExactCache[cacheKey] = fc;
                return fc;
            }
        }

        return null;
    }

    FreeControllerV3 GetTargetController()
    {
        Atom target = FindAtom(targetPersonChooser.val);

        if (target == null)
        {
            return null;
        }

        FreeControllerV3 fc = FindController(target, targetControllerChooser.val);

        if (fc != null)
        {
            return fc;
        }

        fc = FindController(target, "pelvis");
        if (fc != null)
        {
            return fc;
        }

        fc = FindController(target, "hip");
        if (fc != null)
        {
            return fc;
        }

        return target.mainController;
    }

    FreeControllerV3 GetOwnHip()
    {
        FreeControllerV3 fc;

        fc = FindControllerExact(containingAtom, "hipControl");
        if (fc != null) return fc;

        fc = FindControllerExact(containingAtom, "pelvisControl");
        if (fc != null) return fc;

        fc = FindController(containingAtom, "hip");
        if (fc != null) return fc;

        fc = FindController(containingAtom, "pelvis");
        if (fc != null) return fc;

        return containingAtom.mainController;
    }

    FreeControllerV3 GetOwnChest()
    {
        FreeControllerV3 fc;

        fc = FindControllerExact(containingAtom, "chestControl");
        if (fc != null) return fc;

        fc = FindController(containingAtom, "chest");
        if (fc != null) return fc;

        return null;
    }

    FreeControllerV3 GetOwnHead()
    {
        FreeControllerV3 fc;

        fc = FindControllerExact(containingAtom, "headControl");
        if (fc != null) return fc;

        fc = FindController(containingAtom, "head");
        if (fc != null) return fc;

        return null;
    }

    FreeControllerV3 GetTargetHipController(Atom target)
    {
        FreeControllerV3 fc;

        fc = FindControllerExact(target, "hipControl");
        if (fc != null) return fc;

        fc = FindControllerExact(target, "pelvisControl");
        if (fc != null) return fc;

        fc = FindController(target, "hip");
        if (fc != null) return fc;

        fc = FindController(target, "pelvis");
        if (fc != null) return fc;

        return target != null ? target.mainController : null;
    }

    FreeControllerV3 GetTargetChestController(Atom target)
    {
        FreeControllerV3 fc;

        fc = FindControllerExact(target, "chestControl");
        if (fc != null) return fc;

        fc = FindController(target, "chest");
        if (fc != null) return fc;

        return null;
    }

    FreeControllerV3 GetTargetHeadController(Atom target)
    {
        FreeControllerV3 fc;

        fc = FindControllerExact(target, "headControl");
        if (fc != null) return fc;

        fc = FindController(target, "head");
        if (fc != null) return fc;

        return null;
    }

    FreeControllerV3 GetTargetThighController(Atom target, bool right)
    {
        FreeControllerV3 fc;

        fc = FindControllerExact(target, right ? "rThighControl" : "lThighControl");
        if (fc != null) return fc;

        fc = FindController(target, right ? "rthigh" : "lthigh");
        if (fc != null) return fc;

        fc = FindController(target, right ? "r thigh" : "l thigh");
        if (fc != null) return fc;

        return null;
    }

    FreeControllerV3 GetTargetKneeController(Atom target, bool right)
    {
        FreeControllerV3 fc;

        fc = FindControllerExact(target, right ? "rKneeControl" : "lKneeControl");
        if (fc != null) return fc;

        fc = FindController(target, right ? "rknee" : "lknee");
        if (fc != null) return fc;

        fc = FindController(target, right ? "r knee" : "l knee");
        if (fc != null) return fc;

        return null;
    }

    bool TryGetTargetThighLength(Atom target, bool right, out float length)
    {
        length = 0.0f;
        FreeControllerV3 thigh = GetTargetThighController(target, right);
        FreeControllerV3 knee = GetTargetKneeController(target, right);
        if (thigh == null || knee == null || thigh.transform == null || knee.transform == null)
        {
            return false;
        }

        length = Vector3.Distance(thigh.transform.position, knee.transform.position);
        return length >= GenDepthHudTargetThighMin && length <= GenDepthHudTargetThighMax;
    }

    float GetGenHudTargetBodyScale()
    {
        if (targetPersonChooser == null)
        {
            return 1.0f;
        }

        Atom target = FindAtom(targetPersonChooser.val);
        if (target == null)
        {
            return 1.0f;
        }

        float rightLength;
        float leftLength;
        bool hasRight = TryGetTargetThighLength(target, true, out rightLength);
        bool hasLeft = TryGetTargetThighLength(target, false, out leftLength);
        if (!hasRight && !hasLeft)
        {
            return 1.0f;
        }

        float average = hasRight && hasLeft ? (rightLength + leftLength) * 0.5f : (hasRight ? rightLength : leftLength);
        float rawScale = average / GenDepthHudTargetThighBaseline;
        return Mathf.Clamp(rawScale, GenDepthHudBodyScaleMin, GenDepthHudBodyScaleMax);
    }

    float GetGenHudTargetBodyVisualScale()
    {
        return Mathf.Lerp(1.0f, GetGenHudTargetBodyScale(), GenDepthHudBodyScaleInfluence);
    }

    float GetGenHudDropMaxWidth()
    {
        if (genHudDropMaxWidth == null)
        {
            return GenDepthHudDropOpenWidthScale;
        }

        return Mathf.Clamp(genHudDropMaxWidth.val, GenDepthHudDropOpenWidthMin, GenDepthHudDropOpenWidthMax);
    }

    float GetGenHudBurstSizeScale()
    {
        float targetScale = GetGenHudTargetBodyScale();
        float ownWidthScale = GetGenHudDropMaxWidth() / GenDepthHudDropOpenWidthScale;
        float ratio = targetScale * ownWidthScale;
        float subtleRatio = Mathf.Lerp(1.0f, ratio, GenDepthHudBurstRatioInfluence);
        return Mathf.Clamp(subtleRatio, GenDepthHudBurstRatioMin, GenDepthHudBurstRatioMax);
    }

    FreeControllerV3 GetOwnPenisBase()
    {
        FreeControllerV3 fc;

        fc = FindControllerExact(containingAtom, "penisBaseControl");
        if (fc != null) return fc;

        fc = FindController(containingAtom, "penisbase");
        if (fc != null) return fc;

        fc = FindController(containingAtom, "penis base");
        if (fc != null) return fc;

        return null;
    }

    FreeControllerV3 GetOwnPenisMid()
    {
        FreeControllerV3 fc;

        fc = FindControllerExact(containingAtom, "penisMidControl");
        if (fc != null) return fc;

        fc = FindController(containingAtom, "penismid");
        if (fc != null) return fc;

        fc = FindController(containingAtom, "penis mid");
        if (fc != null) return fc;

        return null;
    }

    FreeControllerV3 GetOwnPenisTip()
    {
        FreeControllerV3 fc;

        fc = FindControllerExact(containingAtom, "penisTipControl");
        if (fc != null) return fc;

        fc = FindController(containingAtom, "penistip");
        if (fc != null) return fc;

        fc = FindController(containingAtom, "penis tip");
        if (fc != null) return fc;

        return null;
    }

    Transform FindChildTransform(Atom atom, string childName)
    {
        if (atom == null || string.IsNullOrEmpty(childName))
        {
            return null;
        }

        string key = atom.uid + "|exact|" + childName;
        if (IsPerfTransformCacheEnabled())
        {
            Transform cached;
            if (childTransformExactCache.TryGetValue(key, out cached))
            {
                if (cached != null)
                {
                    return cached;
                }
                childTransformExactCache.Remove(key);
            }
        }

        foreach (Transform t in atom.GetComponentsInChildren<Transform>(true))
        {
            if (t != null && t.name == childName)
            {
                if (IsPerfTransformCacheEnabled())
                {
                    childTransformExactCache[key] = t;
                }
                return t;
            }
        }

        return null;
    }

    Transform FindAnusTargetTransform(Atom atom)
    {
        return FindChildTransformByPathSuffix(atom, "_JointAl/Debug");
    }

    Transform FindChildTransformByPathSuffix(Atom atom, string pathSuffix)
    {
        if (atom == null || string.IsNullOrEmpty(pathSuffix))
        {
            return null;
        }

        string key = atom.uid + "|suffix|" + pathSuffix;
        if (IsPerfTransformCacheEnabled())
        {
            Transform cached;
            if (childTransformSuffixCache.TryGetValue(key, out cached))
            {
                if (cached != null)
                {
                    return cached;
                }
                childTransformSuffixCache.Remove(key);
            }
        }

        foreach (Transform t in atom.GetComponentsInChildren<Transform>(true))
        {
            if (t == null)
            {
                continue;
            }

            if (TransformPathEndsWith(t, pathSuffix))
            {
                if (IsPerfTransformCacheEnabled())
                {
                    childTransformSuffixCache[key] = t;
                }
                return t;
            }
        }

        return null;
    }

    bool TransformPathEndsWith(Transform t, string pathSuffix)
    {
        if (t == null || string.IsNullOrEmpty(pathSuffix))
        {
            return false;
        }

        string[] parts = pathSuffix.Split('/');
        Transform current = t;

        for (int i = parts.Length - 1; i >= 0; i--)
        {
            if (current == null || !string.Equals(current.name, parts[i], StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            current = current.parent;
        }

        return true;
    }

    void CaptureHorizontalBaseline(bool hardMode)
    {
        ReleasePControllerLocksOnly("before baseline cap");
        CaptureHorizontalWithLimbRestore(hardMode);
    }

    void CaptureHorizontalCurrentSide(bool reverseCurrentSide)
    {
        ReleasePControllerLocksOnly("before current-side cap");
        CaptureHorizontalWithLimbRestoreCurrentSide(reverseCurrentSide);
    }

    void OnTargetControllerChanged(string value)
    {
        string oldMode = string.IsNullOrEmpty(lastTargetControllerMode) ? "" : lastTargetControllerMode;
        string newMode = string.IsNullOrEmpty(value) ? "" : value;
        lastTargetControllerMode = newMode;
        bool preserveDistance = oldMode != newMode && IsGenOrAnusTarget(oldMode) && IsGenOrAnusTarget(newMode);

        ClearLookupCaches();

        Vector3 origin = Vector3.zero;
        Vector3 insideDir = Vector3.zero;
        float length = 0.0f;
        bool shouldRetract = false;
        string skipReason = "";

        if (switchRetractOnTargetChange == null || !switchRetractOnTargetChange.val)
        {
            skipReason = "disabled";
        }
        else if (!IsGenOrAnusTarget(oldMode) || !IsGenOrAnusTarget(newMode))
        {
            skipReason = "old/new not gen-anus";
        }
        else if (oldMode == newMode)
        {
            skipReason = "same-target";
        }
        else if (!TryGetInsideLineForTargetMode(oldMode, out origin, out insideDir, out length))
        {
            skipReason = "insideDir missing";
        }
        else if (insideDir.sqrMagnitude < 0.0001f)
        {
            skipReason = "insideDir zero";
        }
        else if (GetSwitchRetractDistance() <= 0.0001f)
        {
            skipReason = "distance zero";
        }
        else
        {
            shouldRetract = true;
        }

        if (switchRetractRoutine != null)
        {
            StopCoroutine(switchRetractRoutine);
            switchRetractRoutine = null;
            targetSwitchRetractBusy = false;
        }

        if (shouldRetract)
        {
            StartSwitchRetractHbaGate();
            switchRetractRoutine = StartCoroutine(RunSwitchRetractThenReset(oldMode, newMode, origin, insideDir.normalized, preserveDistance));
            return;
        }

        if (oldMode != newMode && IsDebugLogEnabled())
        {
            LogMessageIfDebug(
                "[TargetLinePerson] Switch Retract skipped" +
                " / old=" + oldMode +
                " / new=" + newMode +
                " / reason=" + skipReason +
                " / captured=" + captured
            );
        }

        ResetCaptureStateForTargetChange("target changed to " + value, preserveDistance);
    }

    bool IsGenOrAnusTarget(string mode)
    {
        return mode == "genital" || mode == "anus";
    }
    bool IsSwitchRetractHbaGateActive()
    {
        return targetSwitchRetractBusy || Time.time < targetSwitchRetractHbaGateUntil;
    }

    void StartSwitchRetractHbaGate()
    {
        targetSwitchRetractHbaGateUntil = Time.time + SwitchRetractHbaGateResumeDelaySeconds;
        lastSwitchRetractHbaGateLogTime = -999.0f;
    }

    void ExtendSwitchRetractHbaGateAfterMotion()
    {
        float until = Time.time + SwitchRetractHbaGateResumeDelaySeconds;
        if (until > targetSwitchRetractHbaGateUntil)
        {
            targetSwitchRetractHbaGateUntil = until;
        }
    }

    void LogSwitchRetractHbaGateIfNeeded(string depthSource)
    {
        if (!IsDebugLogEnabled())
        {
            return;
        }

        if (Time.time - lastSwitchRetractHbaGateLogTime < 0.50f)
        {
            return;
        }

        lastSwitchRetractHbaGateLogTime = Time.time;
        LogMessageIfDebug(
            "[TargetLinePerson] Switch Retract HBA gate" +
            " / busy=" + targetSwitchRetractBusy +
            " / holdLeft=" + Mathf.Max(0.0f, targetSwitchRetractHbaGateUntil - Time.time).ToString("F2") +
            " / suppressEnd=True" +
            " / source=" + depthSource
        );
    }


    float GetSwitchRetractDistance()
    {
        if (switchRetractDistance == null)
        {
            return SwitchRetractDistanceDefault;
        }

        return Mathf.Clamp(switchRetractDistance.val, SwitchRetractDistanceMin, SwitchRetractDistanceMax);
    }

    float GetSwitchRetractTime()
    {
        if (switchRetractTime == null)
        {
            return SwitchRetractTimeDefault;
        }

        return Mathf.Clamp(switchRetractTime.val, SwitchRetractTimeMin, SwitchRetractTimeMax);
    }

    bool TryGetInsideLineForTargetMode(string targetMode, out Vector3 origin, out Vector3 dir, out float length)
    {
        origin = Vector3.zero;
        dir = Vector3.zero;
        length = GetGenDepthMax();

        if (targetPersonChooser == null)
        {
            return false;
        }

        Atom targetAtom = FindAtom(targetPersonChooser.val);
        if (targetAtom == null)
        {
            return false;
        }

        if (targetMode == "genital")
        {
            Transform genitalLine = FindChildTransform(targetAtom, "LabiaTrigger");
            if (genitalLine == null)
            {
                return false;
            }

            origin = genitalLine.position;
            dir = genitalLine.up;
        }
        else if (targetMode == "anus")
        {
            Transform anusLine = FindAnusTargetTransform(targetAtom);
            if (anusLine == null)
            {
                return false;
            }

            origin = anusLine.position;
            dir = GetAnusInsideDirectionForDepth(targetAtom, anusLine);
        }
        else
        {
            return false;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        dir.Normalize();
        return true;
    }

    Vector3 ResolveSwitchRetractHorizontalRetreatDir(Vector3 oldOrigin, Vector3 oldInsideDir)
    {
        // Retreat direction is the opposite of the old target's approach axis,
        // but hip/root animation should stay horizontal. If the old axis is
        // nearly vertical, fall back to "away from old target origin" on XZ.
        Vector3 retreatDir = -oldInsideDir;
        retreatDir.y = 0.0f;

        if (retreatDir.sqrMagnitude < SwitchRetractMinHorizontalDirSqr)
        {
            FreeControllerV3 hip = GetOwnHip();
            if (hip != null && hip.transform != null)
            {
                retreatDir = hip.transform.position - oldOrigin;
                retreatDir.y = 0.0f;
            }
        }

        if (retreatDir.sqrMagnitude < SwitchRetractMinHorizontalDirSqr)
        {
            if (containingAtom != null && containingAtom.transform != null)
            {
                retreatDir = -containingAtom.transform.forward;
                retreatDir.y = 0.0f;
            }
        }

        if (retreatDir.sqrMagnitude < SwitchRetractMinHorizontalDirSqr)
        {
            retreatDir = Vector3.back;
        }

        return retreatDir.normalized;
    }

    void ClearReactionStateForTargetSwitchSilently(string reason)
    {
        ForceAllGenTgOff();

        genTgStartActive = false;
        genTgInsideActive = false;
        genTgDeepActive = false;
        genTgHadInside = false;

        genHeadDepthStartActive = false;
        genHeadDepthInsideActive = false;
        genHeadDepthDeepActive = false;
        genHeadDepthHadInside = false;
        genHeadInsideNextRandomTime = -1.0f;

        UpdateHbaSharedStatus(0.0f, false);

        if (IsDebugLogEnabled())
        {
            LogMessageIfDebug("[TargetLinePerson] Switch Retract silent reaction clear / reason=" + reason);
        }
    }

    IEnumerator RunSwitchRetractThenReset(string oldMode, string newMode, Vector3 oldOrigin, Vector3 oldInsideDir, bool preserveDistance)
    {
        targetSwitchRetractBusy = true;
        CancelDelayedGuideRefresh("switch retract");
        ResetUpperBodyLowerIfApplied("switch retract");
        ResetPAngleAtYellowP3IfApplied("switch retract");

        if (pushPRoutine != null)
        {
            pushStopRequested = true;
        }

        FreeControllerV3 root = containingAtom != null ? containingAtom.mainController : null;
        FreeControllerV3 hip = GetOwnHip();
        FreeControllerV3 penisBase = GetOwnPenisBase();
        FreeControllerV3 penisMid = GetOwnPenisMid();
        FreeControllerV3 penisTip = GetOwnPenisTip();

        if (hip == null || hip.transform == null || oldInsideDir.sqrMagnitude < 0.0001f)
        {
            targetSwitchRetractBusy = false;
            ExtendSwitchRetractHbaGateAfterMotion();
            ClearReactionStateForTargetSwitchSilently("switch retract skipped");
            switchRetractRoutine = null;
            ResetCaptureStateForTargetChange("target changed to " + newMode + " / switch retract skipped", preserveDistance);
            yield break;
        }

        Vector3 horizontalRetreatDir = ResolveSwitchRetractHorizontalRetreatDir(oldOrigin, oldInsideDir);
        if (horizontalRetreatDir.sqrMagnitude < 0.0001f)
        {
            targetSwitchRetractBusy = false;
            ExtendSwitchRetractHbaGateAfterMotion();
            ClearReactionStateForTargetSwitchSilently("switch retract direction missing");
            switchRetractRoutine = null;
            ResetCaptureStateForTargetChange("target changed to " + newMode + " / switch retract direction missing", preserveDistance);
            yield break;
        }

        Vector3 retractDelta = horizontalRetreatDir * GetSwitchRetractDistance();
        Vector3 pRetractDelta = retractDelta * SwitchRetractPControllerFollowScale;
        Vector3 rootStart = root != null && root.transform != null ? root.transform.position : Vector3.zero;
        Vector3 hipStart = hip.transform.position;
        Vector3 baseStart = penisBase != null && penisBase.transform != null ? penisBase.transform.position : Vector3.zero;
        Vector3 midStart = penisMid != null && penisMid.transform != null ? penisMid.transform.position : Vector3.zero;
        Vector3 tipStart = penisTip != null && penisTip.transform != null ? penisTip.transform.position : Vector3.zero;

        float duration = Mathf.Max(0.01f, GetSwitchRetractTime());
        float startTime = Time.time;

        DebugLog(
            "[TargetLinePerson] Switch Retract start" +
            " / old=" + oldMode +
            " / new=" + newMode +
            " / distance=" + GetSwitchRetractDistance().ToString("F3") +
            " / time=" + duration.ToString("F2") +
            " / pFollowScale=" + SwitchRetractPControllerFollowScale.ToString("F2") +
            " / rawDir=(" + FormatVector3((-oldInsideDir.normalized)) + ")" +
            " / horizontalDir=(" + FormatVector3(horizontalRetreatDir) + ")"
        );

        while (Time.time - startTime < duration)
        {
            float t = Mathf.Clamp01((Time.time - startTime) / duration);
            float eased = t * t * (3.0f - 2.0f * t);
            Vector3 delta = retractDelta * eased;

            Vector3 pDelta = pRetractDelta * eased;
            ApplySwitchRetractControllerPosition(root, rootStart + delta);
            ApplySwitchRetractControllerPosition(hip, hipStart + delta);
            ApplySwitchRetractControllerPosition(penisBase, baseStart + pDelta);
            ApplySwitchRetractControllerPosition(penisMid, midStart + pDelta);
            ApplySwitchRetractControllerPosition(penisTip, tipStart + pDelta);

            yield return null;
        }

        ApplySwitchRetractControllerPosition(root, rootStart + retractDelta);
        ApplySwitchRetractControllerPosition(hip, hipStart + retractDelta);
        ApplySwitchRetractControllerPosition(penisBase, baseStart + pRetractDelta);
        ApplySwitchRetractControllerPosition(penisMid, midStart + pRetractDelta);
        ApplySwitchRetractControllerPosition(penisTip, tipStart + pRetractDelta);

        if (SwitchRetractSettleSeconds > 0.0001f)
        {
            yield return new WaitForSeconds(SwitchRetractSettleSeconds);
        }

        ExtendSwitchRetractHbaGateAfterMotion();
        ClearReactionStateForTargetSwitchSilently("switch retract from " + oldMode + " to " + newMode);
        targetSwitchRetractBusy = false;
        switchRetractRoutine = null;
        ResetCaptureStateForTargetChange("target changed to " + newMode + " / switch retract from " + oldMode, preserveDistance);
    }

    void ApplySwitchRetractControllerPosition(FreeControllerV3 fc, Vector3 position)
    {
        if (fc == null || fc.transform == null)
        {
            return;
        }

        if (fc.currentPositionState != FreeControllerV3.PositionState.On)
        {
            fc.currentPositionState = FreeControllerV3.PositionState.On;
        }

        if ((fc.transform.position - position).sqrMagnitude > 0.00000001f)
        {
            fc.transform.position = position;
        }

        if (fc.control != null)
        {
            if ((fc.control.position - position).sqrMagnitude > 0.00000001f)
            {
                fc.control.position = position;
            }
        }
    }

    void ClearLookupCaches()
    {
        atomCache.Clear();
        controllerContainsCache.Clear();
        controllerExactCache.Clear();
        ClearBodyTouchProbeCache();
    }

    void OnTargetPersonChanged(string value)
    {
        ClearLookupCaches();
        RefreshGenHeadAtomList();
        ResetCaptureStateForTargetChange("person changed to " + value);
    }

    void ResetCaptureStateForTargetChange(string reason)
    {
        ResetCaptureStateForTargetChange(reason, false);
    }

    void ResetCaptureStateForTargetChange(string reason, bool preserveDistance)
    {
        float preservedDistance = distance != null ? distance.val : 1.0f;
        if (delayedLineLockRoutine != null)
        {
            StopCoroutine(delayedLineLockRoutine);
            delayedLineLockRoutine = null;
        }

        if (avoidCaptureRoutine != null)
        {
            StopCoroutine(avoidCaptureRoutine);
            avoidCaptureRoutine = null;
        }

        if (switchRetractRoutine != null)
        {
            StopCoroutine(switchRetractRoutine);
            switchRetractRoutine = null;
        }
        if (delayedInsideReactionRoutine != null)
        {
            StopCoroutine(delayedInsideReactionRoutine);
            delayedInsideReactionRoutine = null;
        }
        targetSwitchRetractBusy = false;

        if (limbStateCaptured)
        {
            RestoreFootStateOnly();
            RestoreKneeStateOnly();
            limbStateCaptured = false;
        }

        isCapturing = false;
        isAvoidMoving = false;

        ResetUpperBodyLowerIfApplied(reason);
        ResetPAngleAtYellowP3IfApplied(reason);
        ReleasePControllerLocksOnly(reason);

        captured = false;
        capturedOrigin = Vector3.zero;
        capturedDir = Vector3.zero;
        capturedLineDir = Vector3.zero;
        capturedMoveLineStart = Vector3.zero;
        capturedMoveLineEnd = Vector3.zero;
        hasYellowPPath = false;
        hasCapturedMoveLine = false;
        hasCapturedGreenBaseY = false;
        hasCapturedBodyRoot = false;
        hasCapturedBodyHip = false;
        pYellowCapturePending = false;
        upperBodyLowerBaseCaptured = false;
        lastAppliedUpperBodyLower = 0f;
        rideLieActive = false;
        lieDockingYawLockActive = false;
        lieDockingYawLockForward = Vector3.zero;
        lieDockingYawLockOpposite = false;
        pAngleAtYellowP3Applied = false;
        pDynamicBaseYApplied = false;
        pMidAxisAssistApplied = false;
        lastDynamicPBaseOffset = Vector3.zero;
        ClearTipYellowParallelLock();
        CancelDelayedGuideRefresh(reason);

        if (distance != null) distance.valNoCallback = preserveDistance ? preservedDistance : 1.0f;
        if (orbitAngle != null) orbitAngle.valNoCallback = 0.0f;
        if (hipYOffset != null) hipYOffset.valNoCallback = 0.0f;

        SetPlacementControlsInteractable(false);
        UpdateDebugLines(false);

        LogMessageIfDebug("[TargetLinePerson] Target reset / reason=" + reason + " / press Now Docking again");
    }

    void ReleasePControllerLocksOnly(string reason)
    {
        ReleasePYellowController(GetOwnPenisBase());
        ReleasePYellowController(GetOwnPenisMid());
        ReleasePYellowController(GetOwnPenisTip());

        if (pYellowPathAdvance != null)
        {
            pYellowPathAdvance.valNoCallback = 0.0f;
        }

        if (pYellowPathAlign != null)
        {
            pYellowPathAlign.val = false;
        }

        pYellowOriginalCaptured = false;
        pDynamicBaseYApplied = false;
        lastDynamicPBaseOffset = Vector3.zero;
        ClearTipYellowParallelLock();
        hasCapturedBodyRoot = false;
        hasCapturedBodyHip = false;
        hasCapturedGreenBaseY = false;

        // Quiet build: P control release is normal capture housekeeping; no log needed.
    }

    void CaptureHorizontalWithLimbRestore(bool hardMode)
    {
        CaptureLimbState();

        CaptureHorizontal(hardMode);

        if (avoidCaptureRoutine == null)
        {
            StartCoroutine(RestoreLimbStateDelayed());
        }
    }

    void CaptureHorizontalWithLimbRestoreCurrentSide(bool reverseCurrentSide)
    {
        CaptureLimbState();

        CaptureHorizontalCurrentSideInternal(reverseCurrentSide);

        if (avoidCaptureRoutine == null)
        {
            StartCoroutine(RestoreLimbStateDelayed());
        }
    }

    void ApplyPlacementWithLimbRestore()
    {
        CaptureLimbState();
        RefreshLowTargetActionState("apply once");
        SetOwnKneeAndFootIkOffIfNeeded("apply once");

        ApplyPlacement();

        StartCoroutine(RestoreLimbStateDelayed());
    }

    void CaptureLimbState()
    {
        limbStateCaptured = false;

        FreeControllerV3 rKnee = FindControllerExact(containingAtom, "rKneeControl");
        FreeControllerV3 lKnee = FindControllerExact(containingAtom, "lKneeControl");
        FreeControllerV3 rFoot = FindControllerExact(containingAtom, "rFootControl");
        FreeControllerV3 lFoot = FindControllerExact(containingAtom, "lFootControl");

        if (rKnee == null) rKnee = FindController(containingAtom, "rknee");
        if (lKnee == null) lKnee = FindController(containingAtom, "lknee");
        if (rFoot == null) rFoot = FindController(containingAtom, "rfoot");
        if (lFoot == null) lFoot = FindController(containingAtom, "lfoot");

        if (rKnee != null)
        {
            savedRKneePosState = rKnee.currentPositionState;
            savedRKneeRotState = rKnee.currentRotationState;
        }

        if (lKnee != null)
        {
            savedLKneePosState = lKnee.currentPositionState;
            savedLKneeRotState = lKnee.currentRotationState;
        }

        if (rFoot != null)
        {
            savedRFootPosState = rFoot.currentPositionState;
            savedRFootRotState = rFoot.currentRotationState;
        }

        if (lFoot != null)
        {
            savedLFootPosState = lFoot.currentPositionState;
            savedLFootRotState = lFoot.currentRotationState;
        }

        limbStateCaptured = true;
    }

    void SetOwnKneeAndFootIkOffIfNeeded(string reason)
    {
        if (GetLowTargetAction() != LowTargetActionLegUnlock)
        {
            return;
        }

        if (!lowTargetActionReached)
        {
            DebugLog("[TargetLinePerson] Low Target Leg Unlock skipped / reason=" + reason + " / hipDrop=" + lowTargetActionHipDrop.ToString("F3") + " / threshold=" + LowTargetLegUnlockMinDrop.ToString("F3"));
            return;
        }

        float standingHipRootY;
        if (IsOwnStandingForLegIkKeep(out standingHipRootY))
        {
            DebugLog(
                "[TargetLinePerson] Low Target Leg Unlock skipped" +
                " / reason=standing-docking" +
                " / call=" + reason +
                " / hipRootY=" + standingHipRootY.ToString("F3") +
                " / threshold=" + StandingLegIkKeepHipRootYMin.ToString("F3") +
                " / hipDrop=" + lowTargetActionHipDrop.ToString("F3")
            );
            return;
        }

        DebugLog("[TargetLinePerson] Low Target Leg Unlock applied / reason=" + reason + " / hipDrop=" + lowTargetActionHipDrop.ToString("F3") + " / threshold=" + LowTargetLegUnlockMinDrop.ToString("F3"));
        SetOwnKneeAndFootIkOff();
    }

    bool IsOwnStandingForLegIkKeep(out float hipRootY)
    {
        hipRootY = 0.0f;

        if (rideLieActive || IsOwnLiePoseForYellowGuide())
        {
            return false;
        }

        FreeControllerV3 ownHip = GetOwnHip();
        if (ownHip == null || containingAtom == null || containingAtom.mainController == null)
        {
            return false;
        }

        hipRootY = ownHip.transform.position.y - containingAtom.mainController.transform.position.y;
        return hipRootY >= StandingLegIkKeepHipRootYMin;
    }

    string GetLowTargetAction()
    {
        return lowTargetAction != null ? lowTargetAction.val : LowTargetActionLegUnlock;
    }

    void RefreshLowTargetActionState(string reason)
    {
        lowTargetActionReached = ShouldUseLowTargetAction(out lowTargetActionHipDrop);
        DebugLog("[TargetLinePerson] Low Target Action check / reason=" + reason + " / action=" + GetLowTargetAction() + " / reached=" + lowTargetActionReached + " / hipDrop=" + lowTargetActionHipDrop.ToString("F3") + " / threshold=" + LowTargetLegUnlockMinDrop.ToString("F3"));
    }

    bool ShouldUseLowTargetAction(out float hipDrop)
    {
        hipDrop = 0.0f;

        if (!captured)
        {
            return false;
        }

        FreeControllerV3 ownHip = GetOwnHip();
        if (ownHip == null)
        {
            return false;
        }

        FreeControllerV3 penisBase = GetOwnPenisBase();
        float targetHipY;

        if (penisBase != null)
        {
            float hipToPBaseY = ownHip.transform.position.y - penisBase.transform.position.y;
            targetHipY = capturedOrigin.y + hipToPBaseY;
        }
        else
        {
            targetHipY = capturedOrigin.y + GetHipYOffset();
        }

        hipDrop = ownHip.transform.position.y - targetHipY;
        return hipDrop >= LowTargetLegUnlockMinDrop;
    }

    IEnumerator RestoreLimbStateDelayed()
    {
        float delay = limbRestoreDelay != null ? limbRestoreDelay.val : 3.0f;

        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        RestoreFootStateOnly();

        float kneeDelay = kneeAfterFootDelay != null ? kneeAfterFootDelay.val : 0.5f;

        if (kneeDelay > 0f)
        {
            yield return new WaitForSeconds(kneeDelay);
        }

        RestoreKneeStateOnly();

        limbStateCaptured = false;
    }

    void RestoreFootStateOnly()
    {
        if (!limbStateCaptured)
        {
            return;
        }

        FreeControllerV3 rFoot = FindControllerExact(containingAtom, "rFootControl");
        FreeControllerV3 lFoot = FindControllerExact(containingAtom, "lFootControl");

        if (rFoot == null) rFoot = FindController(containingAtom, "rfoot");
        if (lFoot == null) lFoot = FindController(containingAtom, "lfoot");

        if (rFoot != null)
        {
            rFoot.currentPositionState = savedRFootPosState;
            rFoot.currentRotationState = savedRFootRotState;
        }

        if (lFoot != null)
        {
            lFoot.currentPositionState = savedLFootPosState;
            lFoot.currentRotationState = savedLFootRotState;
        }
    }

    void RestoreKneeStateOnly()
    {
        if (!limbStateCaptured)
        {
            return;
        }

        FreeControllerV3 rKnee = FindControllerExact(containingAtom, "rKneeControl");
        FreeControllerV3 lKnee = FindControllerExact(containingAtom, "lKneeControl");

        if (rKnee == null) rKnee = FindController(containingAtom, "rknee");
        if (lKnee == null) lKnee = FindController(containingAtom, "lknee");

        if (rKnee != null)
        {
            rKnee.currentPositionState = savedRKneePosState;
            rKnee.currentRotationState = savedRKneeRotState;
        }

        if (lKnee != null)
        {
            lKnee.currentPositionState = savedLKneePosState;
            lKnee.currentRotationState = savedLKneeRotState;
        }
    }

    void CaptureHorizontal(bool hardMode)
    {
        CaptureHorizontalCore(hardMode, false, false);
    }

    void CaptureHorizontalCurrentSideInternal(bool reverseCurrentSide)
    {
        CaptureHorizontalCore(false, true, reverseCurrentSide);
    }

    void CaptureHorizontalCore(bool hardMode, bool chooseCurrentSide, bool reverseCurrentSide)
    {
        isCapturing = true;
        ClearAppliedUpperTilt();
        nowDockingLineFitActive = false;
        nowDockingKeepCurrentPlacement = false;
        nowDockingLieNearKeepOrientation = false;
        nowDockingLineFitPBaseY = 0f;
        lieDockingYawLockActive = false;
        lieDockingYawLockForward = Vector3.zero;
        lieDockingYawLockOpposite = false;

        captured = true;
        distance.valNoCallback = 1.0f;
        orbitAngle.valNoCallback = 0.0f;

        if (!UpdateLine())
        {
            captured = false;
            isCapturing = false;
            SetPlacementControlsInteractable(false);
            return;
        }
        bool reverseDirection = hardMode;
        float currentSideDot = 1.0f;
        if (chooseCurrentSide)
        {
            reverseDirection = ShouldReverseForCurrentSideDocking(reverseCurrentSide, out currentSideDot);
        }

        if (reverseDirection)
        {
            ReverseCapturedDirection();
        }

        if (chooseCurrentSide)
        {
            FitNowDockingDistanceToCurrentLine(currentSideDot);
        }

        hasYellowPPath = false;
        hasCapturedMoveLine = false;
        hasCapturedGreenBaseY = false;
        if (delayedLineLockRoutine != null)
        {
            StopCoroutine(delayedLineLockRoutine);
            delayedLineLockRoutine = null;
        }
        pYellowCapturePending = true;
        ReleasePControllerLocksOnly("capture");
        RefreshLowTargetActionState("capture");
        SetOwnKneeAndFootIkOffIfNeeded("capture low target check");

// Restore the original P_YELLOW_PATH height flow:
//   1. Match the Person root height roughly to the target root.
//   2. Capture Hip Y Offset as ownHip.y - ownPBase.y.
//   3. Move hip-relative body controllers so own P Base Y matches capturedOrigin.y.
//
// This is intentionally done BEFORE ApplyPlacement(), because ApplyPlacement() only moves
// the root in X/Z.  If this step is skipped, the body can be placed horizontally while
// the own P Base is left at the wrong height.
if (!nowDockingLieNearKeepOrientation)
{
    AlignRootHeightOnce();
    SetHipYOffsetFromPenisBase();
    ApplyAutoUpperTiltYOffsetOnce();
    AlignHipRelativeControllersHeightToCapturedOriginOnce();
    ApplyAutoUpperTiltOnce();
    if (!ApplyAutoLieOnRidePoseIfNeeded())
    {
        ReleaseRideLieIfNeeded();
        ApplySitGroundPoseIfNeeded();
    }
    ApplyLieDockingYawLockIfNeeded(hardMode, chooseCurrentSide, reverseCurrentSide);
}
else
{
    SetHipYOffsetFromPenisBase();
    DebugLog(
        "[TargetLinePerson] Lie near Now Docking: keep orientation" +
        " / distance=" + (distance != null ? distance.val.ToString("F3") : "n/a") +
        " / microMax=" + NowDockingLieNearMicroAdjustMax.ToString("F3")
    );
}

        if (avoidCaptureRoutine != null)
        {
            StopCoroutine(avoidCaptureRoutine);
            avoidCaptureRoutine = null;
            isAvoidMoving = false;
        }

        // Capture/line baselines must be rebuilt after the body has reached its capture position.
        // The vertical P-base alignment above is preserved because the avoid/placement movement is X/Z only.
        upperBodyLowerBaseCaptured = false;
        lastAppliedUpperBodyLower = 0f;

        if (!nowDockingKeepCurrentPlacement && avoidTargetOnCapture != null && avoidTargetOnCapture.val && ShouldAvoidTargetOnCapture())
        {
            avoidCaptureRoutine = StartCoroutine(AvoidCaptureMoveRoutine());
        }
        else
        {
            if (nowDockingKeepCurrentPlacement)
            {
                DebugLog(
                    "[TargetLinePerson] Now Docking keep current placement" +
                    " / distance=" + (distance != null ? distance.val.ToString("F3") : "n/a") +
                    " / threshold=" + NowDockingNearKeepPlacementDistance.ToString("F3") +
                    " / lieNearKeepYaw=" + (nowDockingLieNearKeepOrientation ? "1" : "0")
                );
                if (nowDockingLieNearKeepOrientation)
                {
                    ApplyPlacementMicroNoRotate(NowDockingLieNearMicroAdjustMax);
                }
            }
            else
            {
                ApplyPlacement(true);
            }
            ScheduleDelayedLineLock(nowDockingKeepCurrentPlacement ? "now-docking-keep-current" : "capture-after-placement");
        }

        isCapturing = false;
        SetPlacementControlsInteractable(true);
        DebugLog("[TargetLinePerson] Captured orbit / PBaseY aligned by original hip-offset flow");
    }

    void SetHipYOffsetFromPenisBase()
    {
        FreeControllerV3 ownHip = GetOwnHip();
        FreeControllerV3 penisBase = GetOwnPenisBase();

        float value = 0f;

        if (ownHip != null && penisBase != null)
        {
            value = ownHip.transform.position.y - penisBase.transform.position.y;
        }

        appliedHipYOffset = value;

        if (hipYOffset != null)
        {
            hipYOffset.valNoCallback = value;
        }
    }

    void AlignRootHeightOnce()
    {
        Atom targetAtom = FindAtom(targetPersonChooser.val);

        if (targetAtom == null || targetAtom.mainController == null || containingAtom == null || containingAtom.mainController == null)
        {
            return;
        }

        Transform ownRoot = containingAtom.mainController.transform;
        Transform targetRoot = targetAtom.mainController.transform;

        Vector3 ownRootPos = ownRoot.position;
        ownRootPos.y = targetRoot.position.y;
        ownRoot.position = ownRootPos;
    }

    void AlignHipRelativeControllersHeightToCapturedOriginOnce()
    {
        FreeControllerV3 ownHip = GetOwnHip();

        if (ownHip == null || containingAtom == null || containingAtom.mainController == null)
        {
            return;
        }

        float targetPBaseY = nowDockingLineFitActive ? nowDockingLineFitPBaseY : capturedOrigin.y;
        float deltaY = targetPBaseY + GetHipYOffset() - ownHip.transform.position.y;
        MoveHipRelativeControllersVertical(deltaY);
    }

    float GetHipYOffset()
    {
        return hipYOffset != null ? hipYOffset.val : 0f;
    }

    void OnHipYOffsetChanged(float value)
    {
        if (!captured)
        {
            appliedHipYOffset = value;
            return;
        }

        float deltaY = value - appliedHipYOffset;
        MoveHipRelativeControllersVertical(deltaY);
        appliedHipYOffset = value;
        ApplyCapturedPlacementOnce("hip y offset slider");
    }

    void MoveHipRelativeControllersVertical(float deltaY)
    {
        if (Mathf.Abs(deltaY) < 0.0001f)
        {
            return;
        }

        List<FreeControllerV3> controllers = GetBodyDirectionControllers();

        foreach (FreeControllerV3 fc in controllers)
        {
            Vector3 pos = fc.transform.position;
            pos.y += deltaY;
            fc.transform.position = pos;
        }
    }

    void ApplyAutoUpperTiltYOffsetOnce()
    {
        if (autoUpperTilt == null || !autoUpperTilt.val)
        {
            return;
        }

        float trigger = tiltTriggerAngle != null ? tiltTriggerAngle.val : 30f;
        float pitch = GetCapturedLinePitch();

        if (pitch < trigger)
        {
            return;
        }

        float add = GetPenisMidToTipDistance();

        if (add <= 0f)
        {
            return;
        }

        float next = GetHipYOffset() + add;
        appliedHipYOffset = next;

        if (hipYOffset != null)
        {
            hipYOffset.valNoCallback = next;
        }
    }

    float GetPenisMidToTipDistance()
    {
        FreeControllerV3 penisMid = GetOwnPenisMid();
        FreeControllerV3 penisTip = GetOwnPenisTip();

        if (penisMid == null || penisTip == null)
        {
            return 0f;
        }

        return Vector3.Distance(penisMid.transform.position, penisTip.transform.position);
    }

    void ApplyAutoUpperTiltOnce()
    {
        if (autoUpperTilt == null || !autoUpperTilt.val)
        {
            return;
        }

        if (capturedLineDir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        float trigger = tiltTriggerAngle != null ? tiltTriggerAngle.val : 30f;
        float pitch = GetCapturedLinePitch();
        float angle = 0f;
        float tiltAmount = Mathf.Abs(GetUpperTiltAngle());

        if (pitch >= trigger)
        {
            angle = tiltAmount;
        }
        else if (pitch <= -trigger && allowDownTilt != null && allowDownTilt.val)
        {
            angle = -tiltAmount;
        }
        else
        {
            return;
        }

        TiltHipRelativeControllersOnce(angle);
        appliedUpperTiltAngle = angle;
        hasAppliedUpperTilt = true;
    }

    void ClearAppliedUpperTilt()
    {
        if (!hasAppliedUpperTilt)
        {
            return;
        }

        TiltHipRelativeControllersOnce(-appliedUpperTiltAngle);
        appliedUpperTiltAngle = 0f;
        hasAppliedUpperTilt = false;
    }

    float GetCapturedLinePitch()
    {
        if (capturedLineDir.sqrMagnitude < 0.0001f)
        {
            return 0f;
        }

        return Mathf.Asin(Mathf.Clamp(capturedLineDir.normalized.y, -1f, 1f)) * Mathf.Rad2Deg;
    }

    float GetUpperTiltAngle()
    {
        return upperTiltAngle != null ? upperTiltAngle.val : 30f;
    }

    void TiltHipRelativeControllersOnce(float angle)
    {
        if (Mathf.Abs(angle) < 0.001f)
        {
            return;
        }

        FreeControllerV3 ownHip = GetOwnHip();

        if (ownHip == null || containingAtom == null || containingAtom.mainController == null)
        {
            return;
        }

        Vector3 pivot = ownHip.transform.position;
        Vector3 axis = containingAtom.mainController.transform.right;

        if (axis.sqrMagnitude < 0.0001f)
        {
            axis = Vector3.right;
        }

        Quaternion rotation = Quaternion.AngleAxis(angle, axis.normalized);
        List<FreeControllerV3> controllers = GetHipRelativeControllers();

        foreach (FreeControllerV3 fc in controllers)
        {
            Vector3 offset = fc.transform.position - pivot;
            fc.transform.position = pivot + rotation * offset;
            fc.transform.rotation = rotation * fc.transform.rotation;
        }
    }

    List<FreeControllerV3> GetHipRelativeControllers()
    {
        List<FreeControllerV3> controllers = new List<FreeControllerV3>();

        if (containingAtom == null || containingAtom.freeControllers == null)
        {
            return controllers;
        }

        foreach (FreeControllerV3 fc in containingAtom.freeControllers)
        {
            if (IsHipRelativeController(fc))
            {
                controllers.Add(fc);
            }
        }

        return controllers;
    }

    bool IsHipRelativeController(FreeControllerV3 fc)
    {
        if (fc == null || fc.name == null)
        {
            return false;
        }

        if (containingAtom != null && containingAtom.mainController == fc)
        {
            return false;
        }

        string name = fc.name.ToLower();

        if (name.Contains("foot") ||
            name.Contains("toe") ||
            name.Contains("heel"))
        {
            return false;
        }

        return true;
    }

    bool UpdateLine()
    {
        FreeControllerV3 target = GetTargetController();
        Atom targetAtom = FindAtom(targetPersonChooser.val);

        if (targetAtom == null)
        {
            LogMessageIfDebug("[TargetLinePerson] Docking target probe / targetAtom=MISSING / docking=SKIP");
            return false;
        }

        if (target == null)
        {
            LogMessageIfDebug("[TargetLinePerson] Docking target probe / target=" + GetTargetModeName() + " / controller=MISSING / docking=SKIP");
            return false;
        }

        Transform genitalLine = null;
        Transform anusLine = null;
        string targetMode = GetTargetModeName();

        if (targetMode == "genital")
        {
            genitalLine = FindChildTransform(targetAtom, "LabiaTrigger");
            if (genitalLine == null)
            {
                LogMessageIfDebug("[TargetLinePerson] Docking target probe / target=genital / LabiaTrigger=MISSING / hipFallback=REMOVED / docking=SKIP");
                return false;
            }
        }

        if (targetMode == "anus")
        {
            anusLine = FindAnusTargetTransform(targetAtom);
            if (anusLine == null)
            {
                LogMessageIfDebug("[TargetLinePerson] Docking target probe / target=anus / _JointAl/Debug=MISSING / docking=SKIP");
                return false;
            }
        }

        Transform mouthLine = null;

        if (targetMode == "mouth")
        {
            mouthLine = FindMouthTargetTransform(targetAtom);
            if (mouthLine == null)
            {
                LogMessageIfDebug("[TargetLinePerson] Docking target probe / target=mouth / mouthPhysicsMeshPredictionPoint=MISSING / hipFallback=REMOVED / docking=SKIP");
                return false;
            }
        }

        Transform specialLine = genitalLine != null ? genitalLine : (anusLine != null ? anusLine : mouthLine);
        Vector3 lineDir = GetTargetLineDirection(target, targetAtom, genitalLine, anusLine, mouthLine);
        string dockingDirSource = "lineDir";
        Vector3 forward = lineDir;
        if (genitalLine != null)
        {
            Vector3 flatLabia = lineDir;
            flatLabia.y = 0f;
            if (flatLabia.sqrMagnitude >= 0.0001f)
            {
                forward = lineDir;
                dockingDirSource = "LabiaTrigger.-up";
            }
            else
            {
                forward = GetTargetRootForward(targetAtom, lineDir);
                dockingDirSource = "targetRoot.forward fallback";
                DebugLog("[TargetLinePerson] Labia smart direction fallback: LabiaTrigger axis is nearly vertical; using target root forward.");
            }
        }
        else if (anusLine != null)
        {
            forward = -GetAnusInsideDirection(targetAtom, anusLine);
            dockingDirSource = "anus targetRoot.-forward";
        }
        forward.y = 0f;

        if (forward.sqrMagnitude < 0.0001f)
        {
            forward = Vector3.forward;
        }

        if (lineDir.sqrMagnitude < 0.0001f)
        {
            lineDir = forward;
        }

        forward.Normalize();
        lineDir.Normalize();

        capturedOrigin = specialLine != null ? specialLine.position : target.transform.position;
        capturedDir = forward;
        capturedLineDir = lineDir;
        hasDynamicRedLineDisplay = false;
        dynamicYellowEndFrozen = false;
        lastDynamicRedLineUpdateTime = -999f;

        DebugLog(
            "[TargetLinePerson] Docking target probe / target=" + targetMode +
            GetDockingTargetFoundText(genitalLine, anusLine, mouthLine) +
            " / source=" + GetDockingTargetSourceName(genitalLine, anusLine, mouthLine, specialLine) +
            " / dockingDirSource=" + dockingDirSource +
            " / origin=(" + FormatVector3(capturedOrigin) + ")" +
            " / dir=(" + FormatVector3(capturedDir) + ")" +
            " / lineDir=(" + FormatVector3(capturedLineDir) + ")"
        );

        return true;
    }

    void UpdateDynamicRedLineDisplayIfNeeded()
    {
        if (dynamicRedLine == null || !dynamicRedLine.val)
        {
            hasDynamicRedLineDisplay = false;
            return;
        }

        bool yellowEndNeedsAxis = dynamicYellowEnd != null && dynamicYellowEnd.val;
        bool debugView = IsDebugViewEnabled();

        if (!debugView && !yellowEndNeedsAxis)
        {
            return;
        }

        float interval = redLineUpdateSec != null ? Mathf.Max(GetDynamicRedLineMinInterval(), redLineUpdateSec.val) : 0.50f;
        if (Time.time - lastDynamicRedLineUpdateTime < interval)
        {
            return;
        }

        FreeControllerV3 target = GetTargetController();
        Atom targetAtom = FindAtom(targetPersonChooser.val);
        if (target == null || targetAtom == null)
        {
            return;
        }

        Transform genitalLine = null;
        Transform anusLine = null;
        Transform mouthLine = null;
        string targetMode = GetTargetModeName();

        if (targetMode == "genital")
        {
            genitalLine = FindChildTransform(targetAtom, "LabiaTrigger");
            if (genitalLine == null)
            {
                return;
            }
        }
        else if (targetMode == "anus")
        {
            anusLine = FindAnusTargetTransform(targetAtom);
            if (anusLine == null)
            {
                return;
            }
        }
        else if (targetMode == "mouth")
        {
            mouthLine = FindMouthTargetTransform(targetAtom);
            if (mouthLine == null)
            {
                return;
            }
        }

        Transform specialLine = genitalLine != null ? genitalLine : (anusLine != null ? anusLine : mouthLine);
        Vector3 newOrigin = specialLine != null ? specialLine.position : target.transform.position;
        Vector3 newLineDir = GetTargetLineDirection(target, targetAtom, genitalLine, anusLine, mouthLine);

        if (newLineDir.sqrMagnitude < 0.0001f)
        {
            newLineDir = capturedLineDir.sqrMagnitude >= 0.0001f ? capturedLineDir : capturedDir;
        }

        if (newLineDir.sqrMagnitude < 0.0001f)
        {
            newLineDir = Vector3.forward;
        }

        newLineDir.Normalize();

        Vector3 oldOrigin = hasDynamicRedLineDisplay ? dynamicRedLineOrigin : capturedOrigin;
        Vector3 oldLineDir = hasDynamicRedLineDisplay ? dynamicRedLineDir : capturedLineDir;
        float angle = oldLineDir.sqrMagnitude >= 0.0001f ? Vector3.Angle(oldLineDir.normalized, newLineDir) : 0f;

        dynamicRedLineOrigin = newOrigin;
        dynamicRedLineDir = newLineDir;
        hasDynamicRedLineDisplay = true;
        lastDynamicRedLineUpdateTime = Time.time;

        if (debugView)
        {
            DebugLog(
                "[TargetLinePerson] Dynamic Red Line update" +
                " / source=" + GetDockingTargetSourceName(genitalLine, anusLine, mouthLine, specialLine) +
                " / interval=" + interval.ToString("F2") +
                " / originOld=(" + FormatVector3(oldOrigin) + ")" +
                " / originNew=(" + FormatVector3(dynamicRedLineOrigin) + ")" +
                " / dirOld=(" + FormatVector3(oldLineDir) + ")" +
                " / dirNew=(" + FormatVector3(dynamicRedLineDir) + ")" +
                " / angle=" + angle.ToString("F1")
            );
        }

        ApplyDynamicYellowEndIfNeeded("axis-update");
    }

    void ApplyDynamicYellowEndIfNeeded(string reason)
    {
        if (dynamicYellowEnd == null || !dynamicYellowEnd.val)
        {
            LogDynamicYellowEndSkip("toggle-off", reason);
            return;
        }

        if (dynamicYellowEndFrozen)
        {
            LogDynamicYellowEndSkip("frozen", reason);
            return;
        }

        if (!hasYellowPPath || YellowPPathPointCount < 6)
        {
            LogDynamicYellowEndSkip("no-yellow-path", reason);
            return;
        }

        if (!hasDynamicRedLineDisplay)
        {
            LogDynamicYellowEndSkip("no-dynamic-red", reason);
            return;
        }

        if (targetControllerChooser != null && targetControllerChooser.val != "genital")
        {
            LogDynamicYellowEndSkip("target-" + targetControllerChooser.val, reason);
            return;
        }

        float liveDepth;
        float liveLength;
        float livePercent;
        TryGetLiveGenDepth(out liveDepth, out liveLength, out livePercent);

        string freezeReason;
        if (ShouldFreezeDynamicYellowEnd(out freezeReason))
        {
            dynamicYellowEndFrozen = true;
            if (IsDebugViewEnabled())
            {
                DebugLog(
                    "[TargetLinePerson] Dynamic Yellow End frozen" +
                    " / reason=" + freezeReason +
                    " / rawDepth=" + lastGenDepthRawDepth.ToString("F3") +
                    " / lateral=" + lastGenDepthLateral.ToString("F3") +
                    " / bodyDist=" + (lastGenDepthBodyDistance >= 0.0f ? lastGenDepthBodyDistance.ToString("F3") : "n/a") +
                    " / percent=" + lastGenDepthPercent.ToString("F3")
                );
            }
            return;
        }

        Vector3 redDir = dynamicRedLineDir;
        if (redDir.sqrMagnitude < 0.0001f)
        {
            redDir = capturedLineDir.sqrMagnitude >= 0.0001f ? capturedLineDir : capturedDir;
        }
        if (redDir.sqrMagnitude < 0.0001f)
        {
            redDir = Vector3.forward;
        }
        redDir.Normalize();

        Vector3 redUpDir = GetYellowEndDirection(redDir);
        Vector3 oldP4 = yellowPPathPoints[4];
        Vector3 oldP5 = yellowPPathPoints[5];
        Vector3 newP4 = dynamicRedLineOrigin;
        Vector3 newP5 = dynamicRedLineOrigin + redUpDir * 1.00f;
        Vector3 oldEndDir = oldP5 - oldP4;
        Vector3 newEndDir = newP5 - newP4;
        float endAngle = 0f;

        if (oldEndDir.sqrMagnitude >= 0.0001f && newEndDir.sqrMagnitude >= 0.0001f)
        {
            endAngle = Vector3.Angle(oldEndDir.normalized, newEndDir.normalized);
        }

        if (Vector3.Distance(oldP4, newP4) < 0.0005f && endAngle < 0.1f)
        {
            LogDynamicYellowEndSkip("unchanged", reason);
            return;
        }

        yellowPPathPoints[4] = newP4;
        yellowPPathPoints[5] = newP5;
        RecalculateYellowPPathLengths();

        if (IsDebugViewEnabled())
        {
            DebugLog(
                "[TargetLinePerson] Dynamic Yellow End update" +
                " / reason=" + reason +
                " / p4Old=(" + FormatVector3(oldP4) + ")" +
                " / p4New=(" + FormatVector3(newP4) + ")" +
                " / p5Old=(" + FormatVector3(oldP5) + ")" +
                " / p5New=(" + FormatVector3(newP5) + ")" +
                " / endDir=(" + FormatVector3(redUpDir) + ")" +
                " / total=" + yellowPPathTotalLength.ToString("F3")
            );
        }
    }

    Vector3 GetYellowEndDirection(Vector3 redDir)
    {
        Vector3 dir = redDir;
        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = capturedLineDir.sqrMagnitude >= 0.0001f ? capturedLineDir : capturedDir;
        }
        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = Vector3.forward;
        }
        dir.Normalize();

        // GenDepth uses LabiaTrigger.up as its positive depth direction.
        // GetTargetLineDirection() uses -LabiaTrigger.up for docking/red-line display.
        // Yellow p4->p5 is the insertion/depth guide, so genital must use the GenDepth direction.
        if (targetControllerChooser != null && targetControllerChooser.val == "genital")
        {
            return -dir;
        }

        return dir.y >= 0f ? dir : -dir;
    }

    void LogDynamicYellowEndSkip(string skipReason, string reason)
    {
        if (!IsDebugViewEnabled())
        {
            return;
        }

        if (skipReason == "toggle-off" || skipReason == "unchanged")
        {
            return;
        }

        bool changed = lastDynamicYellowEndSkipReason != skipReason;
        bool heartbeat = Time.time - lastDynamicYellowEndSkipLogTime >= 2.0f;
        if (!changed && !heartbeat)
        {
            return;
        }

        lastDynamicYellowEndSkipReason = skipReason;
        lastDynamicYellowEndSkipLogTime = Time.time;

        DebugLog(
            "[TargetLinePerson] Dynamic Yellow End skip" +
            " / reason=" + skipReason +
            " / call=" + reason +
            " / hasYellow=" + (hasYellowPPath ? "1" : "0") +
            " / frozen=" + (dynamicYellowEndFrozen ? "1" : "0") +
            " / dynRed=" + (hasDynamicRedLineDisplay ? "1" : "0") +
            " / target=" + (targetControllerChooser != null ? targetControllerChooser.val : "null") +
            " / rawDepth=" + (lastGenDepthSampleKnown ? lastGenDepthRawDepth.ToString("F3") : "n/a") +
            " / lateral=" + (lastGenDepthSampleKnown ? lastGenDepthLateral.ToString("F3") : "n/a") +
            " / bodyDist=" + (lastGenDepthSampleKnown && lastGenDepthBodyDistance >= 0.0f ? lastGenDepthBodyDistance.ToString("F3") : "n/a") +
            " / percent=" + (lastGenDepthSampleKnown ? lastGenDepthPercent.ToString("F3") : "n/a")
        );
    }

    void RecalculateYellowPPathLengths()
    {
        yellowPPathLengths[0] = 0f;
        yellowPPathTotalLength = 0f;

        for (int i = 1; i < YellowPPathPointCount; i++)
        {
            yellowPPathTotalLength += Vector3.Distance(yellowPPathPoints[i - 1], yellowPPathPoints[i]);
            yellowPPathLengths[i] = yellowPPathTotalLength;
        }

        hasYellowPPath = yellowPPathTotalLength > 0.0001f;
    }

    Vector3 GetTargetRootForward(Atom targetAtom, Vector3 fallback)
    {
        if (targetAtom != null && targetAtom.mainController != null)
        {
            Vector3 rootForward = targetAtom.mainController.transform.forward;
            if (rootForward.sqrMagnitude >= 0.0001f)
            {
                return rootForward;
            }
        }

        if (targetAtom != null && targetAtom.transform != null)
        {
            Vector3 atomForward = targetAtom.transform.forward;
            if (atomForward.sqrMagnitude >= 0.0001f)
            {
                return atomForward;
            }
        }

        return fallback;
    }

    Vector3 GetAnusInsideDirection(Atom targetAtom, Transform anusLine)
    {
        Vector3 fallback = anusLine != null ? anusLine.forward : Vector3.forward;
        Vector3 dir = GetTargetRootForward(targetAtom, fallback);
        if (dir.sqrMagnitude < 0.0001f && anusLine != null)
        {
            dir = anusLine.forward;
        }
        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = Vector3.forward;
        }
        dir.Normalize();
        return dir;
    }

    Transform FindMouthTargetTransform(Atom atom)
    {
        return FindChildTransform(atom, "mouthPhysicsMeshPredictionPoint");
    }

    Vector3 ApplyMouthVerticalDamp(Vector3 dir)
    {
        if (dir.sqrMagnitude < 0.0001f)
        {
            return dir;
        }

        dir.y *= MouthInsideVerticalScale;
        if (dir.sqrMagnitude < 0.0001f)
        {
            return dir;
        }

        return dir.normalized;
    }

    Vector3 GetMouthApproachDirection(Atom targetAtom, Transform mouthLine)
    {
        Vector3 fallback = mouthLine != null ? mouthLine.forward : Vector3.forward;
        Vector3 dir = fallback;

        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = GetTargetRootForward(targetAtom, Vector3.forward);
        }

        dir = ApplyMouthVerticalDamp(dir);
        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = Vector3.forward;
        }

        return dir.normalized;
    }

    Vector3 GetMouthInsideDirectionForDepth(Atom targetAtom, Transform mouthLine)
    {
        // Like genital/anus, PUSH depth goes opposite of the approach/red line.
        // Mouth prediction points often tilt downward, so damp Y before using it.
        Vector3 redLineDir = Vector3.zero;
        if (GetTargetModeName() == "mouth" && hasDynamicRedLineDisplay && dynamicRedLineDir.sqrMagnitude >= 0.0001f)
        {
            redLineDir = dynamicRedLineDir;
        }
        else if (GetTargetModeName() == "mouth" && capturedLineDir.sqrMagnitude >= 0.0001f)
        {
            redLineDir = capturedLineDir;
        }

        if (redLineDir.sqrMagnitude >= 0.0001f)
        {
            Vector3 dampedRed = ApplyMouthVerticalDamp(redLineDir);
            if (dampedRed.sqrMagnitude >= 0.0001f)
            {
                return -dampedRed.normalized;
            }
        }

        Vector3 approach = GetMouthApproachDirection(targetAtom, mouthLine);
        if (approach.sqrMagnitude < 0.0001f)
        {
            return approach;
        }

        return -approach.normalized;
    }

    string GetDockingTargetFoundText(Transform genitalLine, Transform anusLine, Transform mouthLine)
    {
        if (genitalLine != null)
        {
            return " / LabiaTrigger=FOUND";
        }

        if (anusLine != null)
        {
            return " / _JointAl/Debug=FOUND";
        }

        if (mouthLine != null)
        {
            return " / mouthPhysicsMeshPredictionPoint=FOUND";
        }

        return "";
    }

    string GetTargetModeName()
    {
        return targetControllerChooser != null ? targetControllerChooser.val : "";
    }

    string GetDockingTargetSourceName(Transform genitalLine, Transform anusLine, Transform mouthLine, Transform specialLine)
    {
        if (genitalLine != null)
        {
            return "LabiaTrigger";
        }

        if (anusLine != null)
        {
            return "_JointAl/Debug";
        }

        if (mouthLine != null)
        {
            return "mouthPhysicsMeshPredictionPoint";
        }

        return specialLine != null ? specialLine.name : "controller";
    }

    string FormatVector3(Vector3 v)
    {
        return v.x.ToString("F3") + "," + v.y.ToString("F3") + "," + v.z.ToString("F3");
    }

    bool IsDebugViewEnabled()
    {
        return showLines != null && showLines.val;
    }

    bool IsDebugLogEnabled()
    {
        return debugLog != null && debugLog.val;
    }

    void DebugLog(string message)
    {
        if (IsDebugLogEnabled())
        {
            LogMessageIfDebug(message);
        }
    }

    void LogMessageIfDebug(string message)
    {
        if (IsDebugLogEnabled())
        {
            SuperController.LogMessage(message);
        }
    }

    bool IsHbaActionName(string actionName)
    {
        return !string.IsNullOrEmpty(actionName) && actionName.IndexOf("HBA_", StringComparison.OrdinalIgnoreCase) == 0;
    }

    void TraceHeadAction(string message, string actionName)
    {
        DebugLog(message);
    }

    Vector3 GetTargetLineDirection(FreeControllerV3 target, Atom targetAtom, Transform genitalLine, Transform anusLine, Transform mouthLine)
    {
        if (genitalLine != null)
        {
            return -genitalLine.up;
        }

        if (anusLine != null)
        {
            return -GetAnusInsideDirection(targetAtom, anusLine);
        }

        if (mouthLine != null)
        {
            return GetMouthApproachDirection(targetAtom, mouthLine);
        }

        if (target == null)
        {
            return Vector3.forward;
        }

        return target.transform.forward;
    }

    void ReverseCapturedDirection()
    {
        if (capturedDir.sqrMagnitude >= 0.0001f)
        {
            capturedDir = -capturedDir.normalized;
        }

        DebugLog("[TargetLinePerson] Reverse Smart Docking: captured direction reversed.");
    }

    bool ShouldReverseForCurrentSideDocking(bool reverseCurrentSide, out float sideDot)
    {
        sideDot = 0f;
        FreeControllerV3 ownHip = GetOwnHip();

        if (ownHip == null || capturedDir.sqrMagnitude < 0.0001f)
        {
            return reverseCurrentSide;
        }

        Vector3 toOwn = ownHip.transform.position - capturedOrigin;
        toOwn.y = 0f;

        Vector3 flatDir = capturedDir;
        flatDir.y = 0f;

        if (toOwn.sqrMagnitude < 0.0001f || flatDir.sqrMagnitude < 0.0001f)
        {
            return reverseCurrentSide;
        }

        sideDot = Vector3.Dot(toOwn.normalized, flatDir.normalized);
        bool normalAlreadyOnCurrentSide = sideDot >= 0f;
        bool reverseNeeded = !normalAlreadyOnCurrentSide;

        if (reverseCurrentSide)
        {
            reverseNeeded = !reverseNeeded;
        }

        DebugLog(
            "[TargetLinePerson] Now Docking side selected" +
            " / sideDot=" + sideDot.ToString("F3") +
            " / reverse=" + reverseNeeded +
            " / reverseCurrentSide=" + reverseCurrentSide
        );

        return reverseNeeded;
    }

    void FitNowDockingDistanceToCurrentLine(float sideDot)
    {
        FreeControllerV3 ownHip = GetOwnHip();

        if (ownHip == null || capturedDir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        bool currentFit = Mathf.Abs(sideDot) >= NowDockingCurrentFitSideDotMin;
        if (!currentFit)
        {
            nowDockingLineFitActive = false;
            nowDockingKeepCurrentPlacement = false;
            nowDockingLieNearKeepOrientation = false;
            nowDockingLineFitPBaseY = 0f;

            if (distance != null)
            {
                distance.valNoCallback = 1.0f;
            }

            DebugLog(
                "[TargetLinePerson] Now Docking current line fit skipped" +
                " / currentFit=0" +
                " / sideDot=" + sideDot.ToString("F3") +
                " / threshold=" + NowDockingCurrentFitSideDotMin.ToString("F3") +
                " / distance=1.000" +
                " / reason=side-start"
            );
            return;
        }

        Vector3 fromOrigin = ownHip.transform.position - capturedOrigin;
        fromOrigin.y = 0f;

        float currentHorizontalDistance = fromOrigin.magnitude;
        Vector3 lineDir = capturedDir;
        lineDir.y = 0f;
        if (lineDir.sqrMagnitude < 0.0001f)
        {
            return;
        }
        lineDir.Normalize();

        // Use the distance along the selected Now Docking line, not the raw radial
        // distance from the target.  Side starts can otherwise feel too far away.
        // If the body is already very close, keep the current root placement instead
        // of pushing it back to the safe minimum distance.
        float currentProjectedDistance = Mathf.Max(0.0f, Vector3.Dot(fromOrigin, lineDir));
        bool nearKeepPlacement = currentHorizontalDistance <= NowDockingNearKeepPlacementDistance;
        bool lieNearKeepOrientation = nearKeepPlacement &&
            currentHorizontalDistance <= NowDockingLieNearKeepOrientationDistance &&
            (rideLieActive || IsOwnLiePoseForYellowGuide());
        float fittedDistance = nearKeepPlacement
            ? Mathf.Clamp(currentProjectedDistance, 0.0f, NowDockingCurrentDistanceMax)
            : Mathf.Clamp(
                Mathf.Max(currentProjectedDistance, NowDockingCurrentDistanceMin),
                0.0f,
                NowDockingCurrentDistanceMax
            );

        if (distance != null)
        {
            distance.valNoCallback = fittedDistance;
        }

        float dipDown;
        float dipAngle;
        bool yellowDipExpected = ShouldUseNowDockingYellowDipHeight(fittedDistance, out dipDown, out dipAngle);

        nowDockingLineFitActive = true;
        nowDockingKeepCurrentPlacement = nearKeepPlacement;
        nowDockingLieNearKeepOrientation = lieNearKeepOrientation;
        // Do not lower P base here.  BuildCapturedYellowPPath() already creates the
        // yellow dip from the captured green plane.  Pre-lowering here double-applies
        // the yellow drop and makes the body look too low.
        nowDockingLineFitPBaseY = capturedOrigin.y;

        DebugLog(
            "[TargetLinePerson] Now Docking current line fit" +
            " / currentFit=1" +
            " / sideDot=" + sideDot.ToString("F3") +
            " / radialDist=" + currentHorizontalDistance.ToString("F3") +
            " / projectedDist=" + currentProjectedDistance.ToString("F3") +
            " / distance=" + fittedDistance.ToString("F3") +
            " / keepCurrentPlacement=" + (nowDockingKeepCurrentPlacement ? "1" : "0") +
            " / lieNearKeepYaw=" + (nowDockingLieNearKeepOrientation ? "1" : "0") +
            " / keepThreshold=" + NowDockingNearKeepPlacementDistance.ToString("F3") +
            " / lieNearThreshold=" + NowDockingLieNearKeepOrientationDistance.ToString("F3") +
            " / yellowDipExpected=" + (yellowDipExpected ? "1" : "0") +
            " / preDrop=0" +
            " / dipDown=" + dipDown.ToString("F3") +
            " / dipAngle=" + dipAngle.ToString("F1") +
            " / targetPBaseY=" + nowDockingLineFitPBaseY.ToString("F3") +
            " / origin=(" + FormatVector3(capturedOrigin) + ")" +
            " / dir=(" + FormatVector3(capturedDir) + ")"
        );
    }

    bool ShouldUseNowDockingYellowDipHeight(float fittedDistance, out float dipDown, out float dipAngle)
    {
        dipDown = 0.08f * GetYellowButtGuideScale();
        dipAngle = 0f;

        if (fittedDistance < 0.10f)
        {
            return false;
        }

        if (IsOwnLiePoseForYellowGuide())
        {
            return false;
        }

        Vector3 approachFlat = -capturedDir;
        approachFlat.y = 0f;
        if (approachFlat.sqrMagnitude < 0.0001f)
        {
            return false;
        }
        approachFlat.Normalize();

        Vector3 redDir = capturedLineDir;
        if (redDir.sqrMagnitude < 0.0001f)
        {
            redDir = capturedDir.sqrMagnitude >= 0.0001f ? capturedDir : Vector3.forward;
        }
        if (redDir.sqrMagnitude < 0.0001f)
        {
            return false;
        }
        redDir.Normalize();

        Vector3 redUpDir = redDir.y >= 0f ? redDir : -redDir;
        dipAngle = GetTargetAxisAngleFromOwnDegrees(approachFlat, redUpDir);

        return dipAngle >= YellowGuideDipAngleMinDegrees &&
            dipAngle <= YellowGuideDipAngleMaxDegrees;
    }

    Vector3 GetOrbitDirection()
    {
        Quaternion rotation = Quaternion.AngleAxis(orbitAngle.val, Vector3.up);
        Vector3 dir = rotation * capturedDir;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
        {
            return Vector3.forward;
        }

        return dir.normalized;
    }

    Vector3 GetOrbitDirectionAtAngle(float angle)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 dir = rotation * capturedDir;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
        {
            return Vector3.forward;
        }

        return dir.normalized;
    }

    float GetAvoidSideAngle(FreeControllerV3 ownHip)
    {
        float angle = avoidSideAngle != null ? Mathf.Abs(avoidSideAngle.val) : 90f;

        if (ownHip == null)
        {
            return angle;
        }

        Vector3 fromCenter = ownHip.transform.position - capturedOrigin;
        fromCenter.y = 0f;

        Vector3 right = Vector3.Cross(Vector3.up, capturedDir);

        if (right.sqrMagnitude < 0.0001f)
        {
            return angle;
        }

        float side = Vector3.Dot(fromCenter, right.normalized);
        return side >= 0f ? angle : -angle;
    }

    bool ShouldAvoidTargetOnCapture()
    {
        FreeControllerV3 ownHip = GetOwnHip();

        if (ownHip == null)
        {
            return false;
        }

        Vector3 start = ownHip.transform.position;
        Vector3 end = GetTargetHipPosition(ownHip);
        Vector3 center = capturedOrigin;

        start.y = 0f;
        end.y = 0f;
        center.y = 0f;

        float radius = avoidRadius != null ? avoidRadius.val : 0.35f;
        float distanceToPath = DistancePointToSegment(center, start, end);

        return distanceToPath < radius;
    }

    float DistancePointToSegment(Vector3 point, Vector3 a, Vector3 b)
    {
        Vector3 ab = b - a;

        if (ab.sqrMagnitude < 0.0001f)
        {
            return Vector3.Distance(point, a);
        }

        float t = Vector3.Dot(point - a, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        Vector3 closest = a + ab * t;
        return Vector3.Distance(point, closest);
    }

    Vector3 GetTargetHipPosition(FreeControllerV3 ownHip)
    {
        Vector3 orbitDir = GetOrbitDirectionAtAngle(orbitAngle.val);
        return GetTargetHipPositionAtDirection(ownHip, orbitDir);
    }

    Vector3 GetTargetHipPositionAtAngle(FreeControllerV3 ownHip, float angle)
    {
        Vector3 orbitDir = GetOrbitDirectionAtAngle(angle);
        return GetTargetHipPositionAtDirection(ownHip, orbitDir);
    }

    Vector3 GetTargetHipPositionAtDirection(FreeControllerV3 ownHip, Vector3 orbitDir)
    {
        Vector3 targetHipPos = capturedOrigin + orbitDir * distance.val;
        targetHipPos.y = capturedOrigin.y + GetHipYOffset();
        return targetHipPos;
    }

    Vector3 GetTargetRootPosition(FreeControllerV3 ownHip)
    {
        if (ownHip == null || containingAtom == null || containingAtom.mainController == null)
        {
            return Vector3.zero;
        }

        Vector3 delta = GetTargetHipPosition(ownHip) - ownHip.transform.position;
        delta.y = 0f;
        return containingAtom.mainController.transform.position + delta;
    }

void ApplyPlacement()
{
    ApplyPlacement(false);
}

void ApplyPlacement(bool rotateToCapturedOrigin)
{
    if (!captured)
    {
        return;
    }

    FreeControllerV3 ownHip = GetOwnHip();

    if (ownHip == null || containingAtom == null || containingAtom.mainController == null)
    {
        return;
    }

    Vector3 targetHipPos = GetTargetHipPosition(ownHip);

    Vector3 delta = targetHipPos - ownHip.transform.position;
    delta.y = 0f;

    containingAtom.mainController.transform.position += delta;

    if (rotateToCapturedOrigin)
    {
        FaceCapturedOriginFromOwnHip(ownHip);
    }
}

void ApplyPlacementMicroNoRotate(float maxDelta)
{
    if (!captured)
    {
        return;
    }

    FreeControllerV3 ownHip = GetOwnHip();

    if (ownHip == null || containingAtom == null || containingAtom.mainController == null)
    {
        return;
    }

    Vector3 targetHipPos = GetTargetHipPosition(ownHip);
    Vector3 delta = targetHipPos - ownHip.transform.position;
    delta.y = 0f;

    float max = Mathf.Max(0.0f, maxDelta);
    if (max > 0.0f && delta.magnitude > max)
    {
        delta = delta.normalized * max;
    }

    if (delta.sqrMagnitude < 0.000001f)
    {
        return;
    }

    containingAtom.mainController.transform.position += delta;
    DebugLog(
        "[TargetLinePerson] Lie near Now Docking micro adjust" +
        " / delta=(" + FormatVector3(delta) + ")" +
        " / max=" + max.ToString("F3")
    );
}

    string[] GetUpperBodyLowerControllerNames()
    {
        // Keep hands/elbows out of Upper Body Lower.
        // Moving r/lHandControl or r/lElbowControl here makes VaM re-solve arm IK
        // and can look like an old hand pose is being reapplied.
        return new string[]
        {
            "chestControl",
            "headControl"
        };
    }

    Vector3 GetControllerLocalPositionSafe(FreeControllerV3 fc)
    {
        if (fc == null)
        {
            return Vector3.zero;
        }

        if (fc.control != null)
        {
            return fc.control.localPosition;
        }

        return fc.transform.localPosition;
    }

    void SetControllerLocalPositionNoStateSafe(FreeControllerV3 fc, Vector3 localPosition)
    {
        if (fc == null)
        {
            return;
        }

        if (fc.control != null)
        {
            fc.control.localPosition = localPosition;
        }

        if (fc.transform != null)
        {
            fc.transform.localPosition = localPosition;
        }
    }

    void MoveControllerLocalYByDelta(FreeControllerV3 fc, float deltaY, bool forcePositionOn)
    {
        if (fc == null || Mathf.Abs(deltaY) < 0.0001f)
        {
            return;
        }

        if (forcePositionOn)
        {
            fc.currentPositionState = FreeControllerV3.PositionState.On;
        }

        Vector3 localPosition = GetControllerLocalPositionSafe(fc);
        localPosition.y += deltaY;
        SetControllerLocalPositionNoStateSafe(fc, localPosition);
    }

    void CaptureUpperBodyLowerBase(string reason)
    {
        // Do not capture localPosition anymore.
        // The previous build reapplied saved localPosition every time the yellow guide lowered,
        // which could pull hands/arms toward an old pose.  This build stores only the
        // PositionState so it can be restored after the delta lower is released.
        upperBodyLowerBasePositionStates.Clear();

        string[] names = GetUpperBodyLowerControllerNames();
        for (int i = 0; i < names.Length; i++)
        {
            FreeControllerV3 fc = FindControllerExact(containingAtom, names[i]);
            if (fc == null)
            {
                continue;
            }

            upperBodyLowerBasePositionStates[names[i]] = fc.currentPositionState;
        }

        upperBodyLowerReferenceDistance = distance != null ? distance.val : 1.0f;
        upperBodyLowerBaseCaptured = upperBodyLowerBasePositionStates.Count > 0;
        lastAppliedUpperBodyLower = 0f;

        DebugLog("[TargetLinePerson] Upper Body Lower state captured / reason=" + reason + " / controllers=" + upperBodyLowerBasePositionStates.Count + " / refDistance=" + upperBodyLowerReferenceDistance.ToString("F3") + " / mode=delta-no-pose-reapply");
    }

    void ResetUpperBodyLower(string reason)
    {
        if (!upperBodyLowerBaseCaptured)
        {
            lastAppliedUpperBodyLower = 0f;
            return;
        }

        string[] names = GetUpperBodyLowerControllerNames();
        for (int i = 0; i < names.Length; i++)
        {
            FreeControllerV3 fc = FindControllerExact(containingAtom, names[i]);
            if (fc == null)
            {
                continue;
            }

            // Undo only the amount this routine actually applied.  Do not restore a
            // saved localPosition, because that replays an old pose.
            if (Mathf.Abs(lastAppliedUpperBodyLower) > 0.0001f)
            {
                MoveControllerLocalYByDelta(fc, lastAppliedUpperBodyLower, false);
            }

            FreeControllerV3.PositionState savedState;
            if (upperBodyLowerBasePositionStates.TryGetValue(names[i], out savedState))
            {
                fc.currentPositionState = savedState;
            }
        }

        lastAppliedUpperBodyLower = 0f;
        DebugLog("[TargetLinePerson] Upper Body Lower reset / reason=" + reason + " / delta undone / positionState restored / no pose reapply");
    }

    void ApplyUpperBodyLowerByDistanceIfNeeded(string reason)
    {
        if (upperBodyLowerByDistance == null || !upperBodyLowerByDistance.val)
        {
            if (Mathf.Abs(lastAppliedUpperBodyLower) > 0.0001f)
            {
                ResetUpperBodyLower("disabled");
            }
            return;
        }

        if (!upperBodyLowerBaseCaptured)
        {
            CaptureUpperBodyLowerBase(reason + " auto");
        }

        float reference = upperBodyLowerReferenceDistance;
        float current = distance != null ? distance.val : reference;
        float scale = upperBodyDistanceLowerScale != null ? upperBodyDistanceLowerScale.val : 0.20f;
        float lower = Mathf.Max(0f, reference - current) * scale;

        if (lower <= 0.0001f)
        {
            ResetUpperBodyLowerIfApplied("distance lower zero");
            return;
        }

        ApplyUpperBodyLowerAmount(lower);
    }


    void ResetUpperBodyLowerIfApplied(string reason)
    {
        if (upperBodyLowerBaseCaptured && Mathf.Abs(lastAppliedUpperBodyLower) > 0.0001f)
        {
            ResetUpperBodyLower(reason);
        }
    }

    void ApplyUpperBodyLowerByYellowPathIfNeeded(string reason)
    {
        if (upperBodyLowerByYellowPath == null || !upperBodyLowerByYellowPath.val)
        {
            ResetUpperBodyLowerIfApplied("yellow lower disabled");
            return;
        }

        if (!captured || followTarget == null || !followTarget.val || isAvoidMoving)
        {
            ResetUpperBodyLowerIfApplied("yellow lower inactive");
            return;
        }

        if (!hasYellowPPath && !pYellowCapturePending)
        {
            BuildCapturedYellowPPath();
        }

        if (!hasYellowPPath || yellowPPathTotalLength <= 0.0001f || !hasCapturedMoveLine)
        {
            ResetUpperBodyLowerIfApplied("no yellow path");
            return;
        }

        if (!yellowGuideHasDip)
        {
            ResetUpperBodyLowerIfApplied("yellow guide has no dip");
            return;
        }

        if (!upperBodyLowerBaseCaptured)
        {
            CaptureUpperBodyLowerBase(reason + " yellow auto");
        }

        float current = distance != null ? distance.val : 0f;

        // Do not remap 70%->100% to the entire yellow path length.
        // That made the visible dip timing lie.  The lower amount must be sampled
        // from the visible yellow line at the same horizontal position on the
        // saved flat green guide.
        float progress = GetYellowProgressFromCurrentPBaseProjection();

        Vector3 samplePos;
        int segmentIndex;
        float segmentT;
        bool sampled = SampleYellowPPathByGreenProjection(progress, out samplePos, out segmentIndex, out segmentT);

        if (!sampled)
        {
            ResetUpperBodyLowerIfApplied("yellow projection failed");
            return;
        }

        float rawLower = Mathf.Max(0f, yellowPPathPoints[1].y - samplePos.y);
        if (rawLower < 0.005f)
        {
            rawLower = 0f;
        }

        string phase;
        if (segmentIndex <= 1 && rawLower <= 0f)
        {
            phase = "green";
        }
        else if (rawLower <= 0f)
        {
            phase = "yellow-flat";
        }
        else
        {
            phase = "yellow";
        }

        float scale = upperBodyYellowLowerScale != null ? upperBodyYellowLowerScale.val : 1.0f;
        float lower = rawLower * scale;

        if (lower <= 0.0001f)
        {
            ResetUpperBodyLowerIfApplied("yellow lower zero");
            lastUpperBodyYellowLowerPhase = phase;
            lastLoggedUpperBodyYellowProgress = progress;
            return;
        }

        ApplyUpperBodyLowerAmount(lower);

        // Quiet build: yellow-path lowering progress is intentionally not logged per update.
        lastUpperBodyYellowLowerPhase = phase;
        lastLoggedUpperBodyYellowProgress = progress;
    }

    bool SampleYellowPPathByGreenProjection(float progress, out Vector3 samplePos, out int segmentIndex, out float segmentT)
    {
        samplePos = Vector3.zero;
        segmentIndex = -1;
        segmentT = 0f;

        if (!hasYellowPPath || !hasCapturedMoveLine)
        {
            return false;
        }

        Vector3 start = capturedMoveLineStart;
        Vector3 end = capturedMoveLineEnd;
        Vector3 axis = end - start;
        axis.y = 0f;

        float axisLength = axis.magnitude;
        if (axisLength < 0.0001f)
        {
            samplePos = yellowPPathPoints[0];
            segmentIndex = 0;
            return true;
        }

        Vector3 axisDir = axis / axisLength;
        float targetS = Mathf.Clamp01(progress) * axisLength;
        const float epsilon = 0.002f;

        bool found = false;
        float bestLower = -999f;
        Vector3 bestPos = yellowPPathPoints[0];
        int bestSegment = 0;
        float bestT = 0f;

        for (int i = 1; i < YellowPPathPointCount; i++)
        {
            Vector3 a = yellowPPathPoints[i - 1];
            Vector3 b = yellowPPathPoints[i];

            Vector3 af = a - start;
            Vector3 bf = b - start;
            af.y = 0f;
            bf.y = 0f;

            float aS = Vector3.Dot(af, axisDir);
            float bS = Vector3.Dot(bf, axisDir);
            float span = bS - aS;

            if (Mathf.Abs(span) < 0.0001f)
            {
                continue;
            }

            float minS = Mathf.Min(aS, bS) - epsilon;
            float maxS = Mathf.Max(aS, bS) + epsilon;

            if (targetS < minS || targetS > maxS)
            {
                continue;
            }

            float t = Mathf.Clamp01((targetS - aS) / span);
            Vector3 p = Vector3.Lerp(a, b, t);
            float lower = yellowPPathPoints[1].y - p.y;

            // If multiple yellow segments overlap on the same flat guide point,
            // use the lowest visible point.  This keeps the lower amount tied to
            // the drawn path, not to a hidden segment order.
            if (!found || lower > bestLower)
            {
                found = true;
                bestLower = lower;
                bestPos = p;
                bestSegment = i;
                bestT = t;
            }
        }

        if (!found)
        {
            if (targetS <= 0f)
            {
                samplePos = yellowPPathPoints[0];
                segmentIndex = 0;
                segmentT = 0f;
                return true;
            }

            samplePos = yellowPPathPoints[YellowPPathPointCount - 1];
            segmentIndex = YellowPPathPointCount - 1;
            segmentT = 1f;
            return true;
        }

        samplePos = bestPos;
        segmentIndex = bestSegment;
        segmentT = bestT;
        return true;
    }

    float GetYellowProgressFromCurrentPBaseProjection()
    {
        return Mathf.Clamp01(GetYellowProgressFromCurrentPBaseProjectionRaw());
    }

    float GetYellowProgressFromCurrentPBaseProjectionRaw()
    {
        if (!hasCapturedMoveLine)
        {
            return 0f;
        }

        Vector3 start = capturedMoveLineStart;
        Vector3 end = capturedMoveLineEnd;
        Vector3 axis = end - start;
        axis.y = 0f;

        float len = axis.magnitude;
        if (len < 0.0001f)
        {
            return 0f;
        }

        Vector3 current = GetYellowProgressReferencePosition();
        Vector3 diff = current - start;
        diff.y = 0f;

        float projected = Vector3.Dot(diff, axis.normalized);
        return projected / len;
    }

    Vector3 GetYellowProgressReferencePosition()
    {
        FreeControllerV3 penisBase = GetOwnPenisBase();
        if (penisBase != null && penisBase.transform != null)
        {
            return penisBase.transform.position;
        }

        FreeControllerV3 ownHip = GetOwnHip();
        if (ownHip != null && ownHip.transform != null)
        {
            return ownHip.transform.position;
        }

        if (containingAtom != null && containingAtom.mainController != null)
        {
            return containingAtom.mainController.transform.position;
        }

        return capturedMoveLineStart;
    }

    void ApplyUpperBodyLowerAmount(float lower)
    {
        if (!upperBodyLowerBaseCaptured)
        {
            CaptureUpperBodyLowerBase("apply upper lower");
        }

        if (!upperBodyLowerBaseCaptured)
        {
            return;
        }

        float targetLower = Mathf.Max(0f, lower);
        float deltaLower = targetLower - lastAppliedUpperBodyLower;

        if (Mathf.Abs(deltaLower) < 0.0001f)
        {
            return;
        }

        // lower grows downward, so local Y moves by -deltaLower.
        float deltaY = -deltaLower;
        string[] names = GetUpperBodyLowerControllerNames();
        for (int i = 0; i < names.Length; i++)
        {
            FreeControllerV3 fc = FindControllerExact(containingAtom, names[i]);
            if (fc == null)
            {
                continue;
            }

            MoveControllerLocalYByDelta(fc, deltaY, true);
        }

        lastAppliedUpperBodyLower = targetLower;
    }

    void ApplyPAngleAtYellowP3IfNeeded(string reason)
    {
        if (pAngleAtYellowP3 == null || !pAngleAtYellowP3.val)
        {
            ResetPAngleAtYellowP3IfApplied("tip yellow guide p2 follow disabled");
            return;
        }

        if (pushPRoutine != null)
        {
            return;
        }

        // P Follow is independent from Upper Body Lower.
        // The Yellow Guide is still the single source of truth.  This routine must not
        // rebuild or alter yellowPPathPoints except through BuildCapturedYellowPPath().

        if (IsPControlBlockedByLiePose())
        {
            ResetPFollowForLiePose("lie pose active / " + reason);
            return;
        }

        if (!captured || followTarget == null || !followTarget.val || isAvoidMoving)
        {
            ResetPAngleAtYellowP3IfApplied("tip yellow guide p2 follow inactive");
            return;
        }

        if (!hasYellowPPath && !pYellowCapturePending)
        {
            BuildCapturedYellowPPath();
        }

        if (!hasYellowPPath || !hasCapturedMoveLine || !pYellowOriginalCaptured)
        {
            ResetPAngleAtYellowP3IfApplied("tip yellow guide p2 follow no yellow/base");
            return;
        }

        float rawProgress = GetYellowProgressFromCurrentPBaseProjectionRaw();
        float progress = Mathf.Clamp01(rawProgress);
        float p2Progress = GetYellowPointGreenProjectionRatio(2);
        float lockProgress = GetYellowPointGreenProjectionRatio(PTipYellowParallelLockPointIndex);

        // Start trigger only.  Do not rebuild or alter the yellow/green guide here.
        // Dip guide keeps the old P2 trigger.
        // No-dip guide starts earlier at Distance<=0.40 to correct downward P droop.
        string pFollowStartMode;
        if (!IsPYellowGuideStartReached(progress, p2Progress, out pFollowStartMode))
        {
            ClearTipYellowParallelLock();
            ResetPAngleAtYellowP3IfApplied("before yellow guide start / " + pFollowStartMode);
            return;
        }

        FreeControllerV3 penisBase = GetOwnPenisBase();
        FreeControllerV3 penisMid = GetOwnPenisMid();
        FreeControllerV3 penisTip = GetOwnPenisTip();

        if (penisBase == null || penisMid == null || penisTip == null)
        {
            ResetPAngleAtYellowP3IfApplied("missing p controller");
            return;
        }

        if (ApplyGDepthPFollowIfNeeded(penisBase, penisMid, penisTip, reason))
        {
            return;
        }

        string currentDepthTargetMode = GetTargetModeName();
        if (currentDepthTargetMode == "anus" && TryHasLiveAnusInsideLine())
        {
            // v165: Do not let Anus mode fall through to the generic Yellow Guide shape near the target.
            // The Yellow Guide is built for genital docking and can rotate P in the opposite direction for anus.
            ResetPAngleAtYellowP3IfApplied("anus depth line not in wide capture zone / no yellow fallback");
            return;
        }

        if (currentDepthTargetMode == "mouth" && TryHasLiveMouthInsideLine())
        {
            // v167: Mouth uses its own inside line. The generic Yellow Guide can aim slightly downward
            // from mouthPhysicsMeshPredictionPoint and can push/rotate P away from the intended mouth axis.
            ResetPAngleAtYellowP3IfApplied("mouth depth line not in wide capture zone / no yellow fallback");
            return;
        }

        float liveGDepthAngle;
        bool suppressBaseLiftByAngle = IsLiveGDepthAngleBlocked(out liveGDepthAngle);
        float baseYLift = GetDynamicPBaseYLiftFromYellowProgress(progress);
        float rawBaseYLift = baseYLift;
        if (suppressBaseLiftByAngle)
        {
            baseYLift = 0.0f;
            bool shouldLogBaseLiftGate = !pBaseLiftAngleGateBlocked || (IsDebugViewEnabled() && Time.time - lastPBaseLiftAngleGateLogTime >= 2.5f);
            if (shouldLogBaseLiftGate)
            {
                lastPBaseLiftAngleGateLogTime = Time.time;
                DebugLog(
                    "[TargetLinePerson] P Base lift gate" +
                    " / reason=g-depth-angle" +
                    " / angle=" + liveGDepthAngle.ToString("F1") +
                    " / limit=" + GenDepthAngleGateLimitDegrees.ToString("F1") +
                    " / rawBaseYLift=" + rawBaseYLift.ToString("F3") +
                    " / baseYLift=0.000" +
                    " / yellow-p2=keep"
                );
            }
            pBaseLiftAngleGateBlocked = true;
        }
        else if (pBaseLiftAngleGateBlocked)
        {
            DebugLog(
                "[TargetLinePerson] P Base lift gate clear" +
                " / reason=g-depth-angle-clear" +
                " / angle=" + liveGDepthAngle.ToString("F1") +
                " / limit=" + GenDepthAngleGateLimitDegrees.ToString("F1")
            );
            pBaseLiftAngleGateBlocked = false;
        }

        float baseForwardPush = GetDynamicPBaseForwardPushFromYellowProgress(progress);
        Vector3 baseForwardDir = GetDynamicPBaseForwardPushDirection();
        ApplyDynamicPBaseAdjustIfNeeded(penisBase, baseYLift, baseForwardPush, baseForwardDir);

        Vector3 currentBasePos = penisBase.transform.position;

        float ownTiltDeg;
        float ownTiltDot;
        float greenGuideLength;
        bool yellowFoldedOnGreen;
        bool ownTiltShortGuard = ShouldUseOwnTiltShortDistanceGuard(currentBasePos, out ownTiltDeg, out ownTiltDot, out greenGuideLength, out yellowFoldedOnGreen);
        bool parallelMode = !ownTiltShortGuard && progress + PTipYellowParallelLockSlack >= lockProgress;

        Vector3 baseTan;
        Vector3 midTarget;
        Vector3 midTan;
        Vector3 tipTarget;
        Vector3 tipTan;
        float anchorDistance = 0f;
        float midGuideDistance = 0f;
        float tipGuideDistance = 0f;
        bool baseExtended = false;
        bool midExtended = false;
        bool tipExtended = false;

        if (ownTiltShortGuard)
        {
            ClearTipYellowParallelLock();

            if (!BuildOwnTiltShortDistanceGuardShape(currentBasePos, out baseTan, out midTarget, out midTan, out tipTarget, out tipTan))
            {
                ResetPAngleAtYellowP3IfApplied("own tilt short-distance guard shape failed");
                return;
            }

            // In guard mode, Mid/Tip position must not be sampled farther along the yellow path.
            // The unchanged yellow guide is used for angle intent only; physical positions are
            // rebuilt from the current Base with captured segment lengths.
            midGuideDistance = yellowBaseToMidLength;
            tipGuideDistance = yellowBaseToMidLength + yellowMidToTipLength;
        }
        else if (parallelMode)
        {
            if (!pTipYellowShapeLocked)
            {
                if (!CaptureTipYellowParallelShape(currentBasePos, lockProgress, out anchorDistance, out midGuideDistance, out tipGuideDistance, out baseExtended, out midExtended, out tipExtended))
                {
                    ResetPAngleAtYellowP3IfApplied("yellow parallel shape capture failed");
                    return;
                }
            }

            baseTan = pTipYellowLockedBaseTangent;
            midTarget = currentBasePos + pTipYellowLockedMidOffset;
            tipTarget = currentBasePos + pTipYellowLockedTipOffset;
            midTan = pTipYellowLockedMidTangent;
            tipTan = pTipYellowLockedTipTangent;
        }
        else
        {
            ClearTipYellowParallelLock();

            Vector3 anchorPos;
            Vector3 anchorTan;
            int anchorSegment;
            float anchorSegmentT;
            if (!SampleYellowPPathByGreenProjectionWithDistance(progress, out anchorPos, out anchorTan, out anchorSegment, out anchorSegmentT, out anchorDistance))
            {
                ResetPAngleAtYellowP3IfApplied("yellow guide anchor failed");
                return;
            }

            if (!BuildYellowGuideThreePointShape(currentBasePos, anchorDistance, out baseTan, out midTarget, out midTan, out tipTarget, out tipTan, out midGuideDistance, out tipGuideDistance, out baseExtended, out midExtended, out tipExtended))
            {
                ResetPAngleAtYellowP3IfApplied("yellow three-point shape failed");
                return;
            }
        }

        Vector3 midBefore = penisMid.transform.position;
        Vector3 tipBefore = penisTip.transform.position;

        // Base position is still only handled by ApplyDynamicPBaseAdjustIfNeeded().
        // Here Base gets rotation only.  Its tangent is intentionally the flat
        // yellow approach line so the first circled part stays straight.
        SetPYellowRotationOnly(penisBase, GetYellowPPathRotation(baseTan));

        // Mid and Tip are no longer placed on a Bezier bridge.  Their positions
        // and rotations are copied from the corresponding yellow-guide shape:
        // Base = straight section, Mid = middle guide angle, Tip = steep guide angle.
        ApplyControllerToYellowPathRelative(penisMid, midTarget, midTan);
        ApplyControllerToYellowPathRelative(penisTip, tipTarget, tipTan);
        ApplyPMidAxisAssistIfNeeded(penisBase, penisMid, penisTip, reason);

        bool shouldLog = !pAngleAtYellowP3Applied && IsDebugViewEnabled();
        if (shouldLog)
        {
            DebugLog(
                "[TargetLinePerson] P yellow guide three-angle shape at P2 applied" +
                " / reason=" + reason +
                " / progress=" + progress.ToString("F3") +
                " / rawProgress=" + rawProgress.ToString("F3") +
                " / p2=" + p2Progress.ToString("F3") +
                " / startMode=" + pFollowStartMode +
                " / lock=" + lockProgress.ToString("F3") +
                " / parallel=" + parallelMode +
                " / anchorDist=" + anchorDistance.ToString("F3") +
                " / midGuideDist=" + midGuideDistance.ToString("F3") +
                " / tipGuideDist=" + tipGuideDistance.ToString("F3") +
                " / baseExt=" + baseExtended +
                " / midExt=" + midExtended +
                " / tipExt=" + tipExtended +
                " / baseYLift=" + baseYLift.ToString("F3") +
                " / baseForward=" + baseForwardPush.ToString("F3") +
                " / ownTiltGuard=" + ownTiltShortGuard +
                " / ownTiltDeg=" + ownTiltDeg.ToString("F1") +
                " / ownTiltDot=" + ownTiltDot.ToString("F2") +
                " / greenLen=" + greenGuideLength.ToString("F3") +
                " / folded=" + yellowFoldedOnGreen +
                " / yellowLine=unchanged" +
                " / mode=base-straight-mid-tip-yellow-angle-parallel-own-tilt-guard" +
                " / midMove=" + Vector3.Distance(midBefore, midTarget).ToString("F3") +
                " / tipMove=" + Vector3.Distance(tipBefore, tipTarget).ToString("F3") +
                " / position mid+tip / rotation base+mid+tip"
            );
        }

        pAngleAtYellowP3Applied = true;
    }

    bool IsLiveGDepthAngleBlocked(out float angle)
    {
        angle = 0.0f;

        Vector3 origin;
        Vector3 dir;
        float length;
        if (!TryGetLiveGenitalInsideLine(out origin, out dir, out length))
        {
            return false;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        dir.Normalize();
        angle = Mathf.Abs(Mathf.Asin(Mathf.Clamp(dir.y, -1.0f, 1.0f)) * Mathf.Rad2Deg);
        return angle > GenDepthAngleGateLimitDegrees;
    }

    bool ApplyGDepthPFollowIfNeeded(FreeControllerV3 penisBase, FreeControllerV3 penisMid, FreeControllerV3 penisTip, string reason)
    {
        if (penisBase == null || penisMid == null || penisTip == null)
        {
            return false;
        }

        Vector3 origin;
        Vector3 dir;
        float length;
        string depthTargetMode;
        if (!TryGetLiveCurrentInsideLine(out origin, out dir, out length, out depthTargetMode))
        {
            gDepthPFollowApplied = false;
            return false;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            gDepthPFollowApplied = false;
            return false;
        }

        dir.Normalize();
        float gDepthAngle = Mathf.Abs(Mathf.Asin(Mathf.Clamp(dir.y, -1f, 1f)) * Mathf.Rad2Deg);
        bool angleBlocked = gDepthAngle > 45.0f;

        if (angleBlocked)
        {
            if (gDepthPFollowApplied || !lastGDepthAngleGateBlocked)
            {
                DebugLog(
                    "[TargetLinePerson] Depth P Follow gate" +
                    " / target=" + depthTargetMode +
                    " / reason=angle" +
                    " / angle=" + gDepthAngle.ToString("F1") +
                    " / limit=45.0" +
                    " / fallback=yellow-p2"
                );
            }
            lastGDepthAngleGateBlocked = true;
            gDepthPFollowApplied = false;
            return false;
        }

        if (lastGDepthAngleGateBlocked)
        {
            DebugLog(
                "[TargetLinePerson] Depth P Follow gate clear" +
                " / target=" + depthTargetMode +
                " / angle=" + gDepthAngle.ToString("F1") +
                " / limit=45.0"
            );
        }
        lastGDepthAngleGateBlocked = false;

        Vector3 tipPos = penisTip.transform.position;
        Vector3 fromOrigin = tipPos - origin;
        float rawDepth = Vector3.Dot(fromOrigin, dir);
        Vector3 closestOnAxis = origin + dir * rawDepth;
        float lateral = Vector3.Distance(tipPos, closestOnAxis);

        bool isAnusDepthTarget = depthTargetMode == "anus";
        bool isMouthDepthTarget = depthTargetMode == "mouth";
        float backDepthLimit = isAnusDepthTarget ? AnusDepthPFollowBackDepth : (isMouthDepthTarget ? MouthDepthPFollowBackDepth : -0.080f);
        float lateralLimit = isAnusDepthTarget ? AnusDepthPFollowLateralMax : (isMouthDepthTarget ? MouthDepthPFollowLateralMax : 0.080f);
        bool nearGDepthLine =
            rawDepth > backDepthLimit &&
            lateral < lateralLimit;

        if (!nearGDepthLine)
        {
            if (gDepthPFollowApplied || ((isAnusDepthTarget || isMouthDepthTarget) && IsDebugViewEnabled()))
            {
                DebugLog(
                    "[TargetLinePerson] Depth P Follow OFF" +
                    " / target=" + depthTargetMode +
                    " / reason=left-zone" +
                    " / rawDepth=" + rawDepth.ToString("F3") +
                    " / lateral=" + lateral.ToString("F3") +
                    " / backLimit=" + backDepthLimit.ToString("F3") +
                    " / lateralLimit=" + lateralLimit.ToString("F3") +
                    ((isAnusDepthTarget || isMouthDepthTarget) ? " / fallback=yellow-disabled-until-outside-wide-zone" : "")
                );
            }
            gDepthPFollowApplied = false;
            return false;
        }

        float baseLen = Mathf.Max(0.02f, yellowBaseToMidLength);
        float midLen = Mathf.Max(0.02f, yellowMidToTipLength);
        float targetDepth = Mathf.Clamp(rawDepth, 0.0f, 1.00f);

        Vector3 tipTarget = origin + dir * targetDepth;
        Vector3 midTarget = tipTarget - dir * midLen;
        Vector3 baseTarget = midTarget - dir * baseLen;

        SetPYellowRotationOnly(penisBase, GetYellowPPathRotation(dir));
        ApplyControllerToYellowPathRelative(penisMid, midTarget, dir);
        ApplyControllerToYellowPathRelative(penisTip, tipTarget, dir);
        ApplyPMidAxisAssistIfNeeded(penisBase, penisMid, penisTip, reason);

        bool shouldLog = !gDepthPFollowApplied || (IsDebugViewEnabled() && Time.time - lastGDepthPFollowLogTime >= 2.5f);
        if (shouldLog)
        {
            lastGDepthPFollowLogTime = Time.time;
            DebugLog(
                "[TargetLinePerson] Depth P Follow applied" +
                " / target=" + depthTargetMode +
                " / reason=" + reason +
                " / angle=" + gDepthAngle.ToString("F1") +
                " / rawDepth=" + rawDepth.ToString("F3") +
                " / targetDepth=" + targetDepth.ToString("F3") +
                " / lateral=" + lateral.ToString("F3") +
                " / origin=(" + FormatVector3(origin) + ")" +
                " / dir=(" + FormatVector3(dir) + ")" +
                " / baseLen=" + baseLen.ToString("F3") +
                " / midLen=" + midLen.ToString("F3") +
                " / baseTarget=(" + FormatVector3(baseTarget) + ")" +
                " / midTarget=(" + FormatVector3(midTarget) + ")" +
                " / tipTarget=(" + FormatVector3(tipTarget) + ")"
            );
        }

        gDepthPFollowApplied = true;
        pAngleAtYellowP3Applied = true;
        return true;
    }

    void ApplyPMidAxisAssistIfNeeded(FreeControllerV3 penisBase, FreeControllerV3 penisMid, FreeControllerV3 penisTip, string reason)
    {
        if (penisBase == null || penisMid == null || penisTip == null)
        {
            pMidAxisAssistApplied = false;
            return;
        }

        if (GetTargetModeName() != "genital")
        {
            pMidAxisAssistApplied = false;
            return;
        }

        Vector3 origin;
        Vector3 dir;
        float length;
        if (!TryGetLiveGenitalInsideLine(out origin, out dir, out length))
        {
            pMidAxisAssistApplied = false;
            return;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            pMidAxisAssistApplied = false;
            return;
        }

        bool hasProbe;
        bool triggerConfirmed = IsGenBodyTouchConfirmed(out hasProbe);
        dir.Normalize();
        Vector3 tipPos = penisTip.transform.position;
        Vector3 tipFromOrigin = tipPos - origin;
        float tipDepth = Vector3.Dot(tipFromOrigin, dir);
        Vector3 tipClosest = origin + dir * tipDepth;
        Vector3 tipLateral = Vector3.ProjectOnPlane(tipPos - tipClosest, dir);
        float tipLateralDistance = tipLateral.magnitude;

        if (tipDepth < PMidAxisAssistTipBackDepth || tipDepth > PMidAxisAssistTipForwardDepth || tipLateralDistance > PMidAxisAssistTipLateralMax)
        {
            pMidAxisAssistApplied = false;
            return;
        }

        Vector3 midPos = penisMid.transform.position;
        float midDepth = Vector3.Dot(midPos - origin, dir);
        Vector3 midClosest = origin + dir * midDepth;
        Vector3 midLateral = Vector3.ProjectOnPlane(midPos - midClosest, dir);
        float midLateralDistance = midLateral.magnitude;

        if (midLateralDistance < PMidAxisAssistMidLateralMin)
        {
            pMidAxisAssistApplied = false;
            return;
        }

        float confirmScale = hasProbe && !triggerConfirmed ? PMidAxisAssistUnconfirmedScale : 1.0f;
        Vector3 midCorrection = Vector3.ClampMagnitude(-midLateral * PMidAxisAssistMidScale * confirmScale, PMidAxisAssistMidMax * confirmScale);
        Vector3 baseCorrection = Vector3.ClampMagnitude(-midLateral * PMidAxisAssistBaseScale * confirmScale, PMidAxisAssistBaseMax * confirmScale);

        ApplyControllerPositionOffsetIfChanged(penisMid, midCorrection);
        ApplyControllerPositionOffsetIfChanged(penisBase, baseCorrection);

        bool shouldLog = !pMidAxisAssistApplied || (IsDebugViewEnabled() && Time.time - lastPMidAxisAssistLogTime >= PMidAxisAssistLogInterval);
        if (shouldLog)
        {
            lastPMidAxisAssistLogTime = Time.time;
            DebugLog(
                "[TargetLinePerson] P Mid Axis Assist" +
                " / reason=" + reason +
                " / triggerProbe=" + (hasProbe ? "1" : "0") +
                " / triggerConfirmed=" + (triggerConfirmed ? "1" : "0") +
                " / scale=" + confirmScale.ToString("F2") +
                " / tipDepth=" + tipDepth.ToString("F3") +
                " / tipLat=" + tipLateralDistance.ToString("F3") +
                " / midLat=" + midLateralDistance.ToString("F3") +
                " / midMove=" + midCorrection.magnitude.ToString("F3") +
                " / baseMove=" + baseCorrection.magnitude.ToString("F3")
            );
        }

        pMidAxisAssistApplied = true;
    }

    void ApplyControllerPositionOffsetIfChanged(FreeControllerV3 fc, Vector3 offset)
    {
        if (fc == null || offset.sqrMagnitude < 0.00000001f)
        {
            return;
        }

        Vector3 pos = fc.transform.position + offset;
        if (fc.currentPositionState != FreeControllerV3.PositionState.On)
        {
            fc.currentPositionState = FreeControllerV3.PositionState.On;
        }
        if ((fc.transform.position - pos).sqrMagnitude > 0.00000001f)
        {
            fc.transform.position = pos;
        }
        if (fc.control != null && (fc.control.position - pos).sqrMagnitude > 0.00000001f)
        {
            fc.control.position = pos;
        }
    }

    bool IsGenBodyTouchConfirmed(out bool hasProbe)
    {
        hasProbe = false;
        Atom targetAtom = targetPersonChooser != null ? FindAtom(targetPersonChooser.val) : null;
        if (!ResolveBodyTouchProbeStorables(targetAtom))
        {
            return false;
        }

        hasProbe = true;
        return IsBoolParamOn(bodyTouchLabiaParam) ||
            IsBoolParamOn(bodyTouchVaginaParam) ||
            IsBoolParamOn(bodyTouchDeepVaginaParam) ||
            IsBoolParamOn(bodyTouchDeeperVaginaParam);
    }

    bool IsBoolParamOn(JSONStorableBool param)
    {
        return param != null && param.val;
    }

    bool ResolveBodyTouchProbeStorables(Atom atom)
    {
        if (atom == null)
        {
            ClearBodyTouchProbeCache();
            return false;
        }

        if (bodyTouchProbeCacheAtomUid == atom.uid && bodyTouchLabiaParam != null)
        {
            return true;
        }

        ClearBodyTouchProbeCache();

        foreach (string storableId in atom.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId))
            {
                continue;
            }

            JSONStorable storable = atom.GetStorableByID(storableId);
            if (storable == null)
            {
                continue;
            }

            JSONStorableBool labia = storable.GetBoolJSONParam("On LabiaTrigger");
            JSONStorableBool vagina = storable.GetBoolJSONParam("On VaginaTrigger");
            JSONStorableBool deep = storable.GetBoolJSONParam("On DeepVaginaTrigger");
            JSONStorableBool deeper = storable.GetBoolJSONParam("On DeeperVaginaTrigger");

            if (labia != null || vagina != null || deep != null || deeper != null)
            {
                bodyTouchProbeCacheAtomUid = atom.uid;
                bodyTouchLabiaParam = labia;
                bodyTouchVaginaParam = vagina;
                bodyTouchDeepVaginaParam = deep;
                bodyTouchDeeperVaginaParam = deeper;
                return true;
            }
        }

        return false;
    }

    void ClearBodyTouchProbeCache()
    {
        bodyTouchProbeCacheAtomUid = "";
        bodyTouchLabiaParam = null;
        bodyTouchVaginaParam = null;
        bodyTouchDeepVaginaParam = null;
        bodyTouchDeeperVaginaParam = null;
    }

    bool IsPYellowGuideStartReached(float progress, float p2Progress, out string startMode)
    {
        if (yellowGuideHasDip)
        {
            startMode = "dip-p2";
            return progress + 0.0001f >= p2Progress;
        }

        float currentDistance = distance != null ? distance.val : 999f;
        startMode = "flat-distance-040";
        return currentDistance <= PFlatGuideStartDistance;
    }

    bool CaptureTipYellowParallelShape(Vector3 currentBasePos, float lockProgress, out float anchorDistance, out float midGuideDistance, out float tipGuideDistance, out bool baseExtended, out bool midExtended, out bool tipExtended)
    {
        anchorDistance = 0f;
        midGuideDistance = 0f;
        tipGuideDistance = 0f;
        baseExtended = false;
        midExtended = false;
        tipExtended = false;

        Vector3 anchorPos;
        Vector3 anchorTan;
        int anchorSegment;
        float anchorSegmentT;
        if (!SampleYellowPPathByGreenProjectionWithDistance(lockProgress, out anchorPos, out anchorTan, out anchorSegment, out anchorSegmentT, out anchorDistance))
        {
            return false;
        }

        Vector3 baseTan;
        Vector3 midTarget;
        Vector3 midTan;
        Vector3 tipTarget;
        Vector3 tipTan;
        if (!BuildYellowGuideThreePointShape(currentBasePos, anchorDistance, out baseTan, out midTarget, out midTan, out tipTarget, out tipTan, out midGuideDistance, out tipGuideDistance, out baseExtended, out midExtended, out tipExtended))
        {
            return false;
        }

        pTipYellowLockedMidOffset = midTarget - currentBasePos;
        pTipYellowLockedTipOffset = tipTarget - currentBasePos;
        pTipYellowLockedBaseTangent = baseTan.sqrMagnitude > 0.0001f ? baseTan.normalized : Vector3.forward;
        pTipYellowLockedMidTangent = midTan.sqrMagnitude > 0.0001f ? midTan.normalized : Vector3.forward;
        pTipYellowLockedTipTangent = tipTan.sqrMagnitude > 0.0001f ? tipTan.normalized : Vector3.forward;
        pTipYellowLockedProgress = lockProgress;
        pTipYellowShapeLocked = true;
        return true;
    }

    bool BuildYellowGuideThreePointShape(Vector3 currentBasePos, float baseGuideDistance, out Vector3 baseTan, out Vector3 midTarget, out Vector3 midTan, out Vector3 tipTarget, out Vector3 tipTan, out float midGuideDistance, out float tipGuideDistance, out bool baseExtended, out bool midExtended, out bool tipExtended)
    {
        baseTan = Vector3.forward;
        midTarget = currentBasePos;
        midTan = Vector3.forward;
        tipTarget = currentBasePos;
        tipTan = Vector3.forward;
        midGuideDistance = baseGuideDistance + yellowBaseToMidLength;
        tipGuideDistance = midGuideDistance + yellowMidToTipLength;
        baseExtended = false;
        midExtended = false;
        tipExtended = false;

        Vector3 baseGuidePos;
        Vector3 baseGuideTan;
        Vector3 midGuidePos;
        Vector3 tipGuidePos;

        if (!SampleYellowPPathExtendedSmooth(baseGuideDistance, out baseGuidePos, out baseGuideTan, out baseExtended))
        {
            return false;
        }

        if (!SampleYellowPPathExtendedSmooth(midGuideDistance, out midGuidePos, out midTan, out midExtended))
        {
            return false;
        }

        if (!SampleYellowPPathExtendedSmooth(tipGuideDistance, out tipGuidePos, out tipTan, out tipExtended))
        {
            return false;
        }

        // Position follows the unchanged yellow guide shape, but it is re-anchored
        // to the current Base so the shape can move forward with the body.
        midTarget = currentBasePos + (midGuidePos - baseGuidePos);
        tipTarget = currentBasePos + (tipGuidePos - baseGuidePos);

        // Base should stay visually straight.  Use the first flat yellow/green
        // approach direction instead of the local dip tangent, otherwise Base can
        // suddenly inherit the down-slope angle at the P2 corner.
        baseTan = GetYellowGuideFlatBaseTangent();

        if (midTan.sqrMagnitude < 0.0001f)
        {
            midTan = midGuidePos - baseGuidePos;
        }
        if (tipTan.sqrMagnitude < 0.0001f)
        {
            tipTan = tipGuidePos - midGuidePos;
        }

        if (midTan.sqrMagnitude < 0.0001f)
        {
            midTan = baseTan;
        }
        if (tipTan.sqrMagnitude < 0.0001f)
        {
            tipTan = midTan;
        }

        midTan.Normalize();
        tipTan.Normalize();
        return true;
    }

    Vector3 GetYellowGuideFlatBaseTangent()
    {
        Vector3 dir = Vector3.zero;

        if (hasYellowPPath && YellowPPathPointCount >= 2)
        {
            dir = yellowPPathPoints[1] - yellowPPathPoints[0];
        }

        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f && hasCapturedMoveLine)
        {
            dir = capturedMoveLineEnd - capturedMoveLineStart;
            dir.y = 0f;
        }
        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = capturedOrigin - currentBaseFallbackPosition();
            dir.y = 0f;
        }
        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = capturedDir;
            dir.y = 0f;
        }
        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = Vector3.forward;
        }

        return dir.normalized;
    }

    Vector3 currentBaseFallbackPosition()
    {
        FreeControllerV3 penisBase = GetOwnPenisBase();
        if (penisBase != null && penisBase.transform != null)
        {
            return penisBase.transform.position;
        }
        FreeControllerV3 ownHip = GetOwnHip();
        if (ownHip != null && ownHip.transform != null)
        {
            return ownHip.transform.position;
        }
        return capturedMoveLineStart;
    }

    bool ShouldUseOwnTiltShortDistanceGuard(Vector3 currentBasePos, out float tiltDeg, out float towardDot, out float greenGuideLength, out bool yellowFoldedOnGreen)
    {
        tiltDeg = 0f;
        towardDot = 0f;
        greenGuideLength = GetCapturedGreenGuideFlatLength();
        yellowFoldedOnGreen = false;

        if (!hasYellowPPath || !hasCapturedMoveLine)
        {
            return false;
        }

        bool tiltedTowardOwn = IsCapturedLineTiltedTowardOwn(currentBasePos, out tiltDeg, out towardDot);
        if (!tiltedTowardOwn)
        {
            return false;
        }

        float p2 = GetYellowPointGreenProjectionRatio(2);
        float p3 = GetYellowPointGreenProjectionRatio(3);
        yellowFoldedOnGreen = p3 + 0.0001f < p2;

        // The dangerous case is not simply tilted Gen.  It is tilted-toward-own
        // plus a short/overlapped green guide.  In that situation, adding
        // Base->Mid and Mid->Tip lengths to yellow path distance jumps too early
        // onto the p4->p5 upward segment, so Mid/Tip fly up.
        bool nearZeroDistance = greenGuideLength < POwnTiltGuardGreenLengthThreshold;
        return nearZeroDistance || yellowFoldedOnGreen;
    }

    float GetCapturedGreenGuideFlatLength()
    {
        if (!hasCapturedMoveLine)
        {
            return 999f;
        }

        Vector3 axis = capturedMoveLineEnd - capturedMoveLineStart;
        axis.y = 0f;
        return axis.magnitude;
    }

    bool BuildOwnTiltShortDistanceGuardShape(Vector3 currentBasePos, out Vector3 baseTan, out Vector3 midTarget, out Vector3 midTan, out Vector3 tipTarget, out Vector3 tipTan)
    {
        baseTan = GetYellowGuideFlatBaseTangent();
        if (baseTan.sqrMagnitude < 0.0001f)
        {
            baseTan = Vector3.forward;
        }
        baseTan.Normalize();

        tipTan = GetOwnTiltGuardTipTangent();
        if (tipTan.sqrMagnitude < 0.0001f)
        {
            tipTan = MakeUpAngleDirection(baseTan, 80f);
        }
        tipTan.Normalize();

        Vector3 midFlat = baseTan;
        Vector3 tipFlat = tipTan;
        tipFlat.y = 0f;
        if (tipFlat.sqrMagnitude >= 0.0001f)
        {
            tipFlat.Normalize();

            // If the target tilt still has a forward component, let Mid share it.
            // If the target tilt points back toward this person, keep Mid forward;
            // otherwise the whole shaft hooks backward too early in near-zero distance.
            if (Vector3.Dot(baseTan, tipFlat) > 0.25f)
            {
                Vector3 blendedFlat = baseTan + tipFlat;
                if (blendedFlat.sqrMagnitude >= 0.0001f)
                {
                    midFlat = blendedFlat.normalized;
                }
            }
        }

        float tipAngle = GetUpAngleDegrees(tipTan);
        float midAngle = Mathf.Clamp(tipAngle * POwnTiltGuardMidAngleRatio, POwnTiltGuardMidAngleMinDegrees, POwnTiltGuardMidAngleMaxDegrees);
        midTan = MakeUpAngleDirection(midFlat, midAngle);
        if (midTan.sqrMagnitude < 0.0001f)
        {
            midTan = baseTan;
        }
        midTan.Normalize();

        // Guard mode keeps the captured segment lengths but stops using yellow-path
        // distance for positions.  This prevents the near-zero, own-tilted p4->p5
        // segment from pulling Mid/Tip upward in one frame.
        midTarget = currentBasePos + midTan * yellowBaseToMidLength;
        tipTarget = midTarget + tipTan * yellowMidToTipLength;
        return true;
    }

    Vector3 GetOwnTiltGuardTipTangent()
    {
        Vector3 dir = Vector3.zero;

        if (hasYellowPPath && YellowPPathPointCount >= 6)
        {
            // Use the unchanged guide's final insertion direction as angle intent.
            dir = yellowPPathPoints[5] - yellowPPathPoints[4];
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = capturedLineDir;
        }
        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = capturedDir;
        }
        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = MakeUpAngleDirection(GetYellowGuideFlatBaseTangent(), 80f);
        }

        // The yellow guide represents insertion from below to above.  Use the
        // upward-facing side of the Gen axis for the Tip tangent.
        if (dir.y < 0f)
        {
            dir = -dir;
        }

        Vector3 flat = dir;
        flat.y = 0f;
        if (flat.sqrMagnitude < 0.0001f)
        {
            flat = GetYellowGuideFlatBaseTangent();
        }
        if (flat.sqrMagnitude < 0.0001f)
        {
            flat = Vector3.forward;
        }
        flat.Normalize();

        float upAngle = GetUpAngleDegrees(dir);
        if (upAngle < POwnTiltGuardTipMinUpDegrees)
        {
            dir = MakeUpAngleDirection(flat, POwnTiltGuardTipMinUpDegrees);
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = MakeUpAngleDirection(flat, 80f);
        }

        return dir.normalized;
    }

    float GetUpAngleDegrees(Vector3 dir)
    {
        if (dir.sqrMagnitude < 0.0001f)
        {
            return 0f;
        }

        dir.Normalize();
        Vector3 flat = dir;
        flat.y = 0f;
        return Mathf.Atan2(Mathf.Max(0f, dir.y), flat.magnitude) * Mathf.Rad2Deg;
    }

    void ClearTipYellowParallelLock()
    {
        pTipYellowShapeLocked = false;
        pTipYellowLockedMidOffset = Vector3.zero;
        pTipYellowLockedTipOffset = Vector3.zero;
        pTipYellowLockedBaseTangent = Vector3.forward;
        pTipYellowLockedMidTangent = Vector3.forward;
        pTipYellowLockedTipTangent = Vector3.forward;
        pTipYellowLockedProgress = 0f;
    }

    float GetDynamicPBaseYLiftFromYellowProgress(float progress)
    {
        if (!hasYellowPPath || !hasCapturedMoveLine || !hasCapturedGreenBaseY || !yellowGuideHasDip)
        {
            return 0f;
        }

        Vector3 samplePos;
        int segmentIndex;
        float segmentT;
        if (!SampleYellowPPathByGreenProjectionHighest(progress, out samplePos, out segmentIndex, out segmentT))
        {
            return 0f;
        }

        // Only the yellow upward section may lift Base.
        // The dip/down section is intentionally ignored to avoid the old
        // Mid/Tip -> Base downward dragging problem.
        float lift = samplePos.y - capturedGreenBaseY;
        if (lift < PDynamicBaseYMinLift)
        {
            return 0f;
        }

        return lift * PDynamicBaseYLiftScale;
    }

    float GetDynamicPBaseForwardPushFromYellowProgress(float progress)
    {
        if (!hasYellowPPath || !hasCapturedMoveLine || !yellowGuideHasDip)
        {
            return 0f;
        }

        Vector3 samplePos;
        int segmentIndex;
        float segmentT;
        if (!SampleYellowPPathByGreenProjection(progress, out samplePos, out segmentIndex, out segmentT))
        {
            return 0f;
        }

        // Push Base forward only while the visible yellow guide is in its dip.
        // This is intentionally XZ-only and small.  It helps Base enter the dip
        // before Mid/Tip start pulling, without recreating the old downward drag.
        float dip = yellowPPathPoints[1].y - samplePos.y;
        if (dip < PDynamicBaseForwardOnDipMin)
        {
            return 0f;
        }

        return Mathf.Clamp(dip * PDynamicBaseForwardOnDipScale, 0f, PDynamicBaseForwardOnDipMax);
    }

    Vector3 GetDynamicPBaseForwardPushDirection()
    {
        Vector3 dir = Vector3.zero;

        if (hasCapturedMoveLine)
        {
            dir = capturedMoveLineEnd - capturedMoveLineStart;
            dir.y = 0f;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = capturedDir;
            dir.y = 0f;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = capturedLineDir;
            dir.y = 0f;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = Vector3.forward;
        }

        return dir.normalized;
    }

    void ApplyDynamicPBaseAdjustIfNeeded(FreeControllerV3 penisBase, float lift, float forwardPush, Vector3 forwardDir)
    {
        if (penisBase == null)
        {
            return;
        }

        bool hasLift = lift > 0.0001f;
        bool hasForward = forwardPush > 0.0001f && forwardDir.sqrMagnitude > 0.0001f;

        if (!hasLift && !hasForward)
        {
            RestoreDynamicPBaseYStateIfApplied();
            return;
        }

        Vector3 neutralPos = penisBase.transform.position;
        if (pDynamicBaseYApplied)
        {
            neutralPos -= lastDynamicPBaseOffset;
        }

        Vector3 targetPos = neutralPos;

        if (hasLift)
        {
            targetPos.y = capturedGreenBaseY + lift;
        }

        if (hasForward)
        {
            Vector3 flatDir = forwardDir;
            flatDir.y = 0f;
            if (flatDir.sqrMagnitude > 0.0001f)
            {
                targetPos += flatDir.normalized * forwardPush;
            }
        }

        penisBase.currentPositionState = FreeControllerV3.PositionState.On;
        penisBase.transform.position = targetPos;

        if (penisBase.control != null)
        {
            penisBase.control.position = targetPos;
        }

        lastDynamicPBaseOffset = targetPos - neutralPos;
        pDynamicBaseYApplied = true;
    }

    void RestoreDynamicPBaseYStateIfApplied()
    {
        if (!pDynamicBaseYApplied)
        {
            return;
        }

        FreeControllerV3 penisBase = GetOwnPenisBase();
        if (penisBase != null && pYellowOriginalCaptured)
        {
            penisBase.currentPositionState = savedPBasePositionState;
        }
        else if (penisBase != null)
        {
            penisBase.currentPositionState = FreeControllerV3.PositionState.Off;
        }

        pDynamicBaseYApplied = false;
        lastDynamicPBaseOffset = Vector3.zero;
    }

    bool SampleYellowPPathByGreenProjectionHighest(float progress, out Vector3 samplePos, out int segmentIndex, out float segmentT)
    {
        samplePos = Vector3.zero;
        segmentIndex = -1;
        segmentT = 0f;

        if (!hasYellowPPath || !hasCapturedMoveLine)
        {
            return false;
        }

        Vector3 start = capturedMoveLineStart;
        Vector3 end = capturedMoveLineEnd;
        Vector3 axis = end - start;
        axis.y = 0f;

        float axisLength = axis.magnitude;
        if (axisLength < 0.0001f)
        {
            samplePos = yellowPPathPoints[0];
            segmentIndex = 0;
            return true;
        }

        Vector3 axisDir = axis / axisLength;
        float targetS = Mathf.Clamp01(progress) * axisLength;
        const float epsilon = 0.002f;

        bool found = false;
        float bestY = -999999f;
        Vector3 bestPos = yellowPPathPoints[0];
        int bestSegment = 0;
        float bestT = 0f;

        for (int i = 1; i < YellowPPathPointCount; i++)
        {
            Vector3 a = yellowPPathPoints[i - 1];
            Vector3 b = yellowPPathPoints[i];

            Vector3 af = a - start;
            Vector3 bf = b - start;
            af.y = 0f;
            bf.y = 0f;

            float aS = Vector3.Dot(af, axisDir);
            float bS = Vector3.Dot(bf, axisDir);
            float span = bS - aS;

            if (Mathf.Abs(span) < 0.0001f)
            {
                continue;
            }

            float minS = Mathf.Min(aS, bS) - epsilon;
            float maxS = Mathf.Max(aS, bS) + epsilon;

            if (targetS < minS || targetS > maxS)
            {
                continue;
            }

            float t = Mathf.Clamp01((targetS - aS) / span);
            Vector3 p = Vector3.Lerp(a, b, t);

            // This differs from SampleYellowPPathByGreenProjection(), which
            // intentionally picks the lowest visible point for upper-body lowering.
            // Base Y lift needs the highest visible point instead.
            if (!found || p.y > bestY)
            {
                found = true;
                bestY = p.y;
                bestPos = p;
                bestSegment = i;
                bestT = t;
            }
        }

        if (!found)
        {
            if (targetS <= 0f)
            {
                samplePos = yellowPPathPoints[0];
                segmentIndex = 0;
                segmentT = 0f;
                return true;
            }

            samplePos = yellowPPathPoints[YellowPPathPointCount - 1];
            segmentIndex = YellowPPathPointCount - 1;
            segmentT = 1f;
            return true;
        }

        samplePos = bestPos;
        segmentIndex = bestSegment;
        segmentT = bestT;
        return true;
    }

    bool SampleYellowPPathExtendedSmooth(float distance, out Vector3 pos, out Vector3 tangent, out bool extended)
    {
        pos = Vector3.zero;
        tangent = Vector3.forward;
        extended = false;

        if (!hasYellowPPath || YellowPPathPointCount < 2)
        {
            return false;
        }

        float d = distance;
        if (d < 0f)
        {
            d = 0f;
        }

        if (d <= yellowPPathTotalLength)
        {
            SampleYellowPPath(d, out pos, out tangent);
        }
        else
        {
            extended = true;
            float extra = Mathf.Min(d - yellowPPathTotalLength, PTipYellowGuideEndExtendMax);
            Vector3 endTan = yellowPPathPoints[YellowPPathPointCount - 1] - yellowPPathPoints[YellowPPathPointCount - 2];
            if (endTan.sqrMagnitude < 0.0001f)
            {
                endTan = capturedLineDir;
            }
            if (endTan.sqrMagnitude < 0.0001f)
            {
                endTan = capturedDir;
            }
            if (endTan.sqrMagnitude < 0.0001f)
            {
                endTan = Vector3.forward;
            }
            endTan.Normalize();

            pos = yellowPPathPoints[YellowPPathPointCount - 1] + endTan * extra;
            tangent = endTan;
        }

        // Smooth only the rotation tangent.  Position still stays on the yellow
        // guide, but sharp yellow corners do not snap Tip rotation in one frame.
        Vector3 before;
        Vector3 beforeTan;
        Vector3 after;
        Vector3 afterTan;
        bool beforeExt;
        bool afterExt;
        float smooth = Mathf.Max(0.005f, PTipYellowGuideTangentSmoothDistance);
        SampleYellowPPathExtendedRaw(d - smooth, out before, out beforeTan, out beforeExt);
        SampleYellowPPathExtendedRaw(d + smooth, out after, out afterTan, out afterExt);

        Vector3 smoothTan = after - before;
        if (smoothTan.sqrMagnitude >= 0.0001f)
        {
            tangent = smoothTan.normalized;
        }
        else if (tangent.sqrMagnitude >= 0.0001f)
        {
            tangent.Normalize();
        }
        else
        {
            tangent = Vector3.forward;
        }

        return true;
    }

    bool SampleYellowPPathExtendedRaw(float distance, out Vector3 pos, out Vector3 tangent, out bool extended)
    {
        pos = Vector3.zero;
        tangent = Vector3.forward;
        extended = false;

        if (!hasYellowPPath || YellowPPathPointCount < 2)
        {
            return false;
        }

        float d = Mathf.Max(0f, distance);
        if (d <= yellowPPathTotalLength)
        {
            SampleYellowPPath(d, out pos, out tangent);
            return true;
        }

        extended = true;
        float extra = Mathf.Min(d - yellowPPathTotalLength, PTipYellowGuideEndExtendMax);
        Vector3 endTan = yellowPPathPoints[YellowPPathPointCount - 1] - yellowPPathPoints[YellowPPathPointCount - 2];
        if (endTan.sqrMagnitude < 0.0001f)
        {
            endTan = capturedLineDir;
        }
        if (endTan.sqrMagnitude < 0.0001f)
        {
            endTan = capturedDir;
        }
        if (endTan.sqrMagnitude < 0.0001f)
        {
            endTan = Vector3.forward;
        }
        endTan.Normalize();

        pos = yellowPPathPoints[YellowPPathPointCount - 1] + endTan * extra;
        tangent = endTan;
        return true;
    }

    bool BuildDynamicPCurveFromCurrentBaseToGuideTip(Vector3 currentBasePos, FreeControllerV3 penisMid, Vector3 tipTarget, Vector3 tipTan, out Vector3[] points, out float[] lengths, out float totalLength)
    {
        const int sampleCount = 16;
        points = new Vector3[sampleCount];
        lengths = new float[sampleCount];
        totalLength = 0f;

        Vector3 toTip = tipTarget - currentBasePos;
        if (toTip.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        Vector3 startDir = Vector3.zero;
        if (penisMid != null && penisMid.transform != null)
        {
            startDir = penisMid.transform.position - currentBasePos;
        }
        if (startDir.sqrMagnitude < 0.0001f)
        {
            startDir = toTip;
        }
        if (startDir.sqrMagnitude < 0.0001f)
        {
            startDir = capturedDir;
        }
        if (startDir.sqrMagnitude < 0.0001f)
        {
            startDir = Vector3.forward;
        }
        startDir.Normalize();

        Vector3 endDir = tipTan;
        if (endDir.sqrMagnitude < 0.0001f)
        {
            endDir = toTip;
        }
        if (endDir.sqrMagnitude < 0.0001f)
        {
            endDir = Vector3.forward;
        }
        endDir.Normalize();

        float directLen = toTip.magnitude;
        float pLen = Mathf.Max(0.02f, yellowBaseToMidLength + yellowMidToTipLength);
        float handle = Mathf.Clamp(Mathf.Min(directLen, pLen) * 0.42f, 0.035f, 0.22f);

        Vector3 c0 = currentBasePos;
        Vector3 c3 = tipTarget;
        Vector3 c1 = c0 + startDir * handle;
        Vector3 c2 = c3 - endDir * handle;

        // Avoid a sudden vertical decision at the bottom of the yellow dip.
        // The Tip position follows the guide exactly; these handle heights only
        // smooth the bridge used by Mid.
        c1.y = Mathf.Lerp(c0.y, c3.y, 0.25f);
        c2.y = Mathf.Lerp(c0.y, c3.y, 0.75f);

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (sampleCount <= 1) ? 0f : ((float)i / (float)(sampleCount - 1));
            points[i] = CubicBezier(c0, c1, c2, c3, t);

            if (i == 0)
            {
                lengths[i] = 0f;
            }
            else
            {
                totalLength += Vector3.Distance(points[i - 1], points[i]);
                lengths[i] = totalLength;
            }
        }

        return totalLength > 0.0001f;
    }

    float GetDynamicPForwardKeepShapeLead()
    {
        float pTotalLength = Mathf.Max(0.02f, yellowBaseToMidLength + yellowMidToTipLength);
        return Mathf.Max(PDynamicForwardKeepShapeMinLead, pTotalLength * PDynamicForwardKeepShapeLeadScale);
    }

    Vector3 GetDynamicPFlatForwardDirection(Vector3 currentBasePos)
    {
        Vector3 axis = Vector3.zero;
        float axisLength = 0f;

        if (hasCapturedMoveLine)
        {
            axis = capturedMoveLineEnd - capturedMoveLineStart;
            axis.y = 0f;
            axisLength = axis.magnitude;
        }

        if (axisLength < 0.0001f)
        {
            axis = capturedDir;
            axis.y = 0f;
            axisLength = axis.magnitude;
        }

        if (axisLength < 0.0001f)
        {
            axis = capturedLineDir;
            axis.y = 0f;
            axisLength = axis.magnitude;
        }

        if (axisLength < 0.0001f)
        {
            axis = currentBasePos - capturedOrigin;
            axis.y = 0f;
            axisLength = axis.magnitude;
        }

        if (axisLength < 0.0001f)
        {
            axis = Vector3.forward;
            axisLength = 1f;
        }

        return axis / axisLength;
    }

    bool IsDynamicForwardKeepShapeMode(Vector3 currentBasePos, float rawProgress)
    {
        float lead = GetDynamicPForwardKeepShapeLead();
        Vector3 flatToNormalEnd = capturedOrigin - currentBasePos;
        flatToNormalEnd.y = 0f;

        bool nearGreenEnd = rawProgress >= PDynamicForwardKeepShapeStartProgress;
        bool curveTooShortSoon = flatToNormalEnd.magnitude < lead;
        return nearGreenEnd || curveTooShortSoon;
    }

    Vector3 GetDynamicPForwardKeepShapeDirection(Vector3 currentBasePos)
    {
        Vector3 flatForward = GetDynamicPFlatForwardDirection(currentBasePos);

        Vector3 holeDir = capturedLineDir;
        if (holeDir.sqrMagnitude < 0.0001f)
        {
            holeDir = capturedDir;
        }

        float upAngle = PDynamicForwardKeepShapeMinUpAngleDegrees * Mathf.Deg2Rad;
        if (holeDir.sqrMagnitude >= 0.0001f)
        {
            holeDir.Normalize();
            Vector3 holeFlat = holeDir;
            holeFlat.y = 0f;
            float flatLen = holeFlat.magnitude;
            upAngle = Mathf.Atan2(Mathf.Abs(holeDir.y), Mathf.Max(flatLen, 0.0001f));
            float minAngle = PDynamicForwardKeepShapeMinUpAngleDegrees * Mathf.Deg2Rad;
            if (upAngle < minAngle)
            {
                upAngle = minAngle;
            }
        }

        // Keep horizontal travel in the already-captured green-line direction,
        // but keep the vertical angle from the captured hole axis.
        // This prevents the purple/end section from flattening to horizontal.
        Vector3 dir = flatForward.normalized * Mathf.Cos(upAngle) + Vector3.up * Mathf.Sin(upAngle);
        if (dir.sqrMagnitude < 0.0001f)
        {
            return Vector3.up;
        }

        return dir.normalized;
    }

    Vector3 GetDynamicPCurveEndFromCurrentBase(Vector3 currentBasePos, float rawProgress)
    {
        Vector3 normalEnd = capturedOrigin;
        float lead = GetDynamicPForwardKeepShapeLead();

        if (!IsDynamicForwardKeepShapeMode(currentBasePos, rawProgress))
        {
            return normalEnd;
        }

        // Once the Base reaches the end of the flat green guide, capturedOrigin is no
        // longer a useful curve end.  Previously the virtual end was carried forward
        // on a flat XZ line, so the P became horizontal at the purple/end point.
        // Carry it along the captured hole angle instead: forward AND upward.
        Vector3 keepDir = GetDynamicPForwardKeepShapeDirection(currentBasePos);
        return currentBasePos + keepDir * lead;
    }

    bool BuildDynamicPCurveFromCurrentBase(Vector3 currentBasePos, FreeControllerV3 penisMid, float rawProgress, out Vector3[] points, out float[] lengths, out float totalLength)
    {
        const int sampleCount = 14;
        points = new Vector3[sampleCount];
        lengths = new float[sampleCount];
        totalLength = 0f;

        bool keepShapeMode = IsDynamicForwardKeepShapeMode(currentBasePos, rawProgress);
        Vector3 end = GetDynamicPCurveEndFromCurrentBase(currentBasePos, rawProgress);
        Vector3 toEnd = end - currentBasePos;

        if (toEnd.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        Vector3 flatToEnd = toEnd;
        flatToEnd.y = 0f;

        Vector3 startDir = flatToEnd;
        if (startDir.sqrMagnitude < 0.0001f && penisMid != null)
        {
            startDir = penisMid.transform.position - currentBasePos;
        }
        if (startDir.sqrMagnitude < 0.0001f)
        {
            startDir = capturedDir;
        }
        if (startDir.sqrMagnitude < 0.0001f)
        {
            startDir = Vector3.forward;
        }
        startDir.Normalize();
        if (keepShapeMode)
        {
            startDir = GetDynamicPForwardKeepShapeDirection(currentBasePos);
        }

        Vector3 endDir = capturedLineDir;
        if (endDir.sqrMagnitude < 0.0001f)
        {
            endDir = capturedDir;
        }
        if (endDir.sqrMagnitude < 0.0001f)
        {
            endDir = toEnd;
        }
        if (endDir.sqrMagnitude < 0.0001f)
        {
            endDir = Vector3.forward;
        }
        endDir.Normalize();

        // Use the upward-facing copy of the captured axis, matching the old
        // yellow p4->p5 tangent idea, but rebuild the curve from current Base.
        if (endDir.y < 0f)
        {
            endDir = -endDir;
        }
        if (keepShapeMode)
        {
            endDir = GetDynamicPForwardKeepShapeDirection(currentBasePos);
        }

        float flatLen = flatToEnd.magnitude;
        float directLen = toEnd.magnitude;
        float handle = Mathf.Clamp(directLen * 0.35f, 0.04f, 0.35f);

        Vector3 c0 = currentBasePos;
        Vector3 c3 = end;
        Vector3 c1 = c0 + startDir * handle;
        Vector3 c2 = c3 - endDir * handle;

        // Keep the first handle from diving too aggressively when the body/root
        // is being pushed by VaM physics.  The target height is still reached by
        // the Bezier itself near the end.
        if (!keepShapeMode)
        {
            c1.y = Mathf.Lerp(c0.y, c3.y, 0.20f);
            c2.y = Mathf.Lerp(c0.y, c3.y, 0.80f);
        }

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (sampleCount <= 1) ? 0f : ((float)i / (float)(sampleCount - 1));
            points[i] = CubicBezier(c0, c1, c2, c3, t);

            if (i == 0)
            {
                lengths[i] = 0f;
            }
            else
            {
                totalLength += Vector3.Distance(points[i - 1], points[i]);
                lengths[i] = totalLength;
            }
        }

        return totalLength > 0.0001f;
    }

    Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        float u = 1f - t;
        return
            (u * u * u) * a +
            (3f * u * u * t) * b +
            (3f * u * t * t) * c +
            (t * t * t) * d;
    }

    bool TrySampleDynamicPCurve(Vector3[] points, float[] lengths, float totalLength, float distance, out Vector3 pos, out Vector3 tangent)
    {
        pos = Vector3.zero;
        tangent = Vector3.forward;

        if (points == null || lengths == null || points.Length < 2 || lengths.Length != points.Length)
        {
            return false;
        }

        if (distance < 0f || distance > totalLength)
        {
            return false;
        }

        for (int i = 1; i < points.Length; i++)
        {
            if (distance <= lengths[i])
            {
                float prevLen = lengths[i - 1];
                float segLen = lengths[i] - prevLen;
                Vector3 a = points[i - 1];
                Vector3 b = points[i];

                if (segLen < 0.0001f)
                {
                    pos = b;
                    tangent = (b - a).sqrMagnitude > 0.0001f ? (b - a).normalized : Vector3.forward;
                    return true;
                }

                float t = Mathf.Clamp01((distance - prevLen) / segLen);
                pos = Vector3.Lerp(a, b, t);
                tangent = (b - a).sqrMagnitude > 0.0001f ? (b - a).normalized : Vector3.forward;
                return true;
            }
        }

        return false;
    }

    void ResetPAngleAtYellowP3IfApplied(string reason)
    {
        if (!pAngleAtYellowP3Applied)
        {
            return;
        }

        FreeControllerV3 penisBase = GetOwnPenisBase();
        FreeControllerV3 penisMid = GetOwnPenisMid();
        FreeControllerV3 penisTip = GetOwnPenisTip();

        RestoreDynamicPBaseYStateIfApplied();
        ClearTipYellowParallelLock();

        if (pYellowOriginalCaptured)
        {
            // Do not snap world positions back to capture coordinates while the body/root may have moved.
            // Restore only controller states so VaM can resume natural/off control.
            RestorePYellowControllerStateOnly(penisBase, savedPBasePositionState, savedPBaseRotationState);
            RestorePYellowControllerStateOnly(penisMid, savedPMidPositionState, savedPMidRotationState);
            RestorePYellowControllerStateOnly(penisTip, savedPTipPositionState, savedPTipRotationState);
        }
        else
        {
            ReleasePYellowController(penisBase);
            ReleasePYellowController(penisMid);
            ReleasePYellowController(penisTip);
        }

        pAngleAtYellowP3Applied = false;
        gDepthPFollowApplied = false;
        lastGDepthAngleGateBlocked = false;
        pBaseLiftAngleGateBlocked = false;
        lastPAngleDebugLogTime = -999f;
        DebugLog("[TargetLinePerson] P yellow guide three-angle shape at P2 reset / reason=" + reason + " / base state restored / mid+tip state restored");
    }

    bool SampleYellowPPathByGreenProjectionWithDistance(float progress, out Vector3 samplePos, out Vector3 tangent, out int segmentIndex, out float segmentT, out float pathDistance)
    {
        tangent = Vector3.forward;
        pathDistance = 0f;

        if (!SampleYellowPPathByGreenProjection(progress, out samplePos, out segmentIndex, out segmentT))
        {
            return false;
        }

        if (!hasYellowPPath)
        {
            return false;
        }

        if (segmentIndex <= 0)
        {
            pathDistance = 0f;
            tangent = yellowPPathPoints[1] - yellowPPathPoints[0];
        }
        else if (segmentIndex >= YellowPPathPointCount)
        {
            pathDistance = yellowPPathTotalLength;
            tangent = yellowPPathPoints[YellowPPathPointCount - 1] - yellowPPathPoints[YellowPPathPointCount - 2];
        }
        else
        {
            float prevLen = yellowPPathLengths[segmentIndex - 1];
            float nextLen = yellowPPathLengths[segmentIndex];
            pathDistance = Mathf.Lerp(prevLen, nextLen, Mathf.Clamp01(segmentT));
            tangent = yellowPPathPoints[segmentIndex] - yellowPPathPoints[segmentIndex - 1];
        }

        if (tangent.sqrMagnitude < 0.0001f)
        {
            tangent = Vector3.forward;
        }
        else
        {
            tangent.Normalize();
        }

        pathDistance = Mathf.Clamp(pathDistance, 0f, yellowPPathTotalLength);
        return true;
    }

    void RestorePYellowControllerStateOnly(FreeControllerV3 fc, FreeControllerV3.PositionState positionState, FreeControllerV3.RotationState rotationState)
    {
        if (fc == null)
        {
            return;
        }

        fc.currentPositionState = positionState;
        fc.currentRotationState = rotationState;
    }

    bool IsCapturedLineTiltedTowardOwn(Vector3 pBasePos, out float tiltDeg, out float towardDot)
    {
        tiltDeg = 0f;
        towardDot = 0f;

        Vector3 axis = capturedLineDir;
        if (axis.sqrMagnitude < 0.0001f)
        {
            axis = capturedDir;
        }
        if (axis.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        axis.Normalize();

        // Judge the visible/captured axis as an UPWARD leaning axis.
        // "Hole tilted toward own" means the upper side of the target axis leans back toward this person.
        // The previous DOWNWARD normalization made the horizontal sign opposite in front-facing cases.
        if (axis.y < 0f)
        {
            axis = -axis;
        }

        Vector3 flatOrigin = capturedOrigin;
        flatOrigin.y = pBasePos.y;

        Vector3 approachDir = flatOrigin - pBasePos;
        approachDir.y = 0f;
        if (approachDir.sqrMagnitude < 0.0001f)
        {
            return false;
        }
        approachDir.Normalize();

        // towardOwn means from the captured target/origin back toward this person.
        Vector3 towardOwn = -approachDir;

        Vector3 axisFlat = axis;
        axisFlat.y = 0f;
        float flatLen = axisFlat.magnitude;
        if (flatLen < 0.0001f)
        {
            return false;
        }
        axisFlat.Normalize();

        tiltDeg = Mathf.Atan2(flatLen, Mathf.Abs(axis.y)) * Mathf.Rad2Deg;
        towardDot = Vector3.Dot(axisFlat, towardOwn);

        return tiltDeg >= HoleTowardOwnTiltDegThreshold && towardDot >= HoleTowardOwnDotThreshold;
    }

    Vector3 MakeUpAngleDirection(Vector3 flatDir, float degrees)
    {
        flatDir.y = 0f;

        if (flatDir.sqrMagnitude < 0.0001f)
        {
            flatDir = Vector3.forward;
        }

        flatDir.Normalize();

        float rad = degrees * Mathf.Deg2Rad;
        Vector3 dir = flatDir * Mathf.Cos(rad) + Vector3.up * Mathf.Sin(rad);

        if (dir.sqrMagnitude < 0.0001f)
        {
            return flatDir;
        }

        return dir.normalized;
    }

    void SetPYellowRotationOnly(FreeControllerV3 fc, Quaternion rotation)
    {
        if (fc == null)
        {
            return;
        }

        fc.currentRotationState = FreeControllerV3.RotationState.On;

        if (fc.transform != null)
        {
            fc.transform.rotation = rotation;
        }

        if (fc.control != null)
        {
            fc.control.rotation = rotation;
        }
    }

    void RestorePYellowRotationOnly(FreeControllerV3 fc, Quaternion rotation, FreeControllerV3.RotationState rotationState)
    {
        if (fc == null)
        {
            return;
        }

        if (fc.transform != null)
        {
            fc.transform.rotation = rotation;
        }

        if (fc.control != null)
        {
            fc.control.rotation = rotation;
        }

        fc.currentRotationState = rotationState;
    }

    float GetYellowPointGreenProjectionRatio(int index)
    {
        if (!hasYellowPPath || !hasCapturedMoveLine || index < 0 || index >= YellowPPathPointCount)
        {
            return 1f;
        }

        Vector3 start = capturedMoveLineStart;
        Vector3 end = capturedMoveLineEnd;
        Vector3 axis = end - start;
        axis.y = 0f;

        float len = axis.magnitude;
        if (len < 0.0001f)
        {
            return 1f;
        }

        Vector3 diff = yellowPPathPoints[index] - start;
        diff.y = 0f;

        float projected = Vector3.Dot(diff, axis.normalized);
        return Mathf.Clamp01(projected / len);
    }

    void ApplyCuddleMode(bool mirrorFront)
    {
        Atom targetAtom = FindAtom(targetPersonChooser.val);

        if (targetAtom == null || targetAtom.mainController == null || containingAtom == null || containingAtom.mainController == null)
        {
            return;
        }

        PlaceRootForCuddle(targetAtom, mirrorFront);
        CopyTargetPoseExceptRoot(targetAtom, mirrorFront);
    }

    void PlaceRootForCuddle(Atom targetAtom, bool up)
    {
        Transform targetRoot = targetAtom.mainController.transform;
        Transform ownRoot = containingAtom.mainController.transform;

        Vector3 forward = targetRoot.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude < 0.0001f)
        {
            forward = Vector3.forward;
        }

        forward.Normalize();

        float depth = cuddleDepth != null ? cuddleDepth.val : 0.6f;

        if (up)
        {
            ownRoot.position = targetRoot.position + forward * depth;
        }
        else
        {
            ownRoot.position = targetRoot.position - forward * depth;
        }
    }

    void RotateBodyTowardTargetRootYaw(Atom targetAtom)
    {
        if (targetAtom == null || targetAtom.mainController == null || containingAtom == null || containingAtom.mainController == null)
        {
            return;
        }

        FreeControllerV3 ownHip = GetOwnHip();

        if (ownHip == null)
        {
            return;
        }

        Vector3 ownForward = containingAtom.mainController.transform.forward;
        Vector3 targetForward = targetAtom.mainController.transform.forward;

        ownForward.y = 0f;
        targetForward.y = 0f;

        if (ownForward.sqrMagnitude < 0.0001f || targetForward.sqrMagnitude < 0.0001f)
        {
            return;
        }

        ownForward.Normalize();
        targetForward.Normalize();

        Quaternion delta = Quaternion.FromToRotation(ownForward, targetForward);
        Vector3 pivot = ownHip.transform.position;

        // Copy Body Direction must include feet.
        // GetHipRelativeControllers() excludes foot/toe/heel for tilt safety,
        // so use GetBodyDirectionControllers() here.
        List<FreeControllerV3> controllers = GetBodyDirectionControllers();

        foreach (FreeControllerV3 fc in controllers)
        {
            if (fc == null || fc.transform == null) continue;

            Vector3 offset = fc.transform.position - pivot;
            fc.transform.position = pivot + delta * offset;
            fc.transform.rotation = delta * fc.transform.rotation;
        }
    }


    List<FreeControllerV3> GetBodyDirectionControllers()
    {
        List<FreeControllerV3> controllers = new List<FreeControllerV3>();

        if (containingAtom == null || containingAtom.freeControllers == null)
        {
            return controllers;
        }

        foreach (FreeControllerV3 fc in containingAtom.freeControllers)
        {
            if (fc == null || IsRootController(containingAtom, fc))
            {
                continue;
            }

            controllers.Add(fc);
        }

        return controllers;
    }

    void CopyBodyDirectionFromTarget()
    {
        Atom targetAtom = FindAtom(targetPersonChooser.val);

        if (targetAtom == null || targetAtom.mainController == null || containingAtom == null || containingAtom.mainController == null)
        {
            return;
        }

        SetOwnKneeAndFootIkOn();
        CopyTargetPoseExceptRoot(targetAtom, false);
        RotateBodyTowardTargetRootYaw(targetAtom);
    }

    void FaceTargetAtom(Atom targetAtom)
    {
        if (targetAtom == null || containingAtom == null || containingAtom.mainController == null)
        {
            return;
        }

        Vector3 targetPos = targetAtom.mainController != null
            ? targetAtom.mainController.transform.position
            : targetAtom.transform.position;

        FreeControllerV3 ownHip = GetOwnHip();
        Vector3 ownPos = ownHip != null
            ? ownHip.transform.position
            : containingAtom.mainController.transform.position;

        Vector3 dir = targetPos - ownPos;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        containingAtom.mainController.transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
    }

    void CopyTargetPoseExceptRoot(Atom targetAtom, bool mirrorFront)
    {
        if (targetAtom == null || targetAtom.freeControllers == null || containingAtom == null || containingAtom.freeControllers == null)
        {
            return;
        }

        Transform targetRoot = targetAtom.mainController != null ? targetAtom.mainController.transform : targetAtom.transform;
        Transform ownRoot = containingAtom.mainController.transform;
        Vector3 mirrorNormal = targetRoot.forward;
        mirrorNormal.y = 0f;

        if (mirrorNormal.sqrMagnitude < 0.0001f)
        {
            mirrorNormal = Vector3.forward;
        }

        mirrorNormal.Normalize();

        foreach (FreeControllerV3 ownController in containingAtom.freeControllers)
        {
            if (ownController == null || IsRootController(containingAtom, ownController))
            {
                continue;
            }

            string targetName = mirrorFront ? GetMirroredControllerName(ownController.name) : ownController.name;
            FreeControllerV3 targetController = FindControllerExact(targetAtom, targetName);

            if (targetController == null && mirrorFront)
            {
                targetController = FindControllerExact(targetAtom, ownController.name);
            }

            if (targetController == null || IsRootController(targetAtom, targetController))
            {
                continue;
            }

            if (mirrorFront)
            {
                Vector3 relative = targetController.transform.position - targetRoot.position;
                relative = MirrorVectorAcrossPlane(relative, mirrorNormal);
                ownController.transform.position = ownRoot.position + relative;
                ownController.transform.rotation = MirrorRotationForCuddle(targetController.transform.rotation, targetRoot.rotation, ownRoot.rotation);
            }
            else
            {
                ownController.transform.localPosition = targetController.transform.localPosition;
                ownController.transform.localRotation = targetController.transform.localRotation;
            }
        }
    }

    string GetMirroredControllerName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        if (name.Length > 1 && name[0] == 'r' && char.IsUpper(name[1]))
        {
            return "l" + name.Substring(1);
        }

        if (name.Length > 1 && name[0] == 'l' && char.IsUpper(name[1]))
        {
            return "r" + name.Substring(1);
        }

        if (name.StartsWith("r "))
        {
            return "l " + name.Substring(2);
        }

        if (name.StartsWith("l "))
        {
            return "r " + name.Substring(2);
        }

        if (name.StartsWith("right", StringComparison.OrdinalIgnoreCase))
        {
            return "left" + name.Substring(5);
        }

        if (name.StartsWith("left", StringComparison.OrdinalIgnoreCase))
        {
            return "right" + name.Substring(4);
        }

        return name;
    }

    Vector3 MirrorVectorAcrossPlane(Vector3 vector, Vector3 normal)
    {
        return vector - 2f * Vector3.Dot(vector, normal) * normal;
    }

    Quaternion MirrorRotationForCuddle(Quaternion targetRotation, Quaternion targetRootRotation, Quaternion ownRootRotation)
    {
        Quaternion relativeRotation = Quaternion.Inverse(targetRootRotation) * targetRotation;
        return ownRootRotation * relativeRotation;
    }

    bool IsRootController(Atom atom, FreeControllerV3 controller)
    {
        return atom != null && atom.mainController == controller;
    }

    Vector3 GetTargetFacingDirection(Atom targetAtom)
    {
        if (targetAtom != null && targetAtom.mainController != null)
        {
            Vector3 dir = targetAtom.mainController.transform.forward;
            dir.y = 0f;

            if (dir.sqrMagnitude >= 0.0001f)
            {
                return dir.normalized;
            }
        }

        return Vector3.forward;
    }

    void FacePointFromHip(Vector3 point)
    {
        FreeControllerV3 ownHip = GetOwnHip();

        if (ownHip == null || containingAtom == null || containingAtom.mainController == null)
        {
            return;
        }

        Vector3 dir = point - ownHip.transform.position;
        dir.y = 0f;
        FaceDirection(dir);
    }

    void FaceDirection(Vector3 dir)
    {
        if (containingAtom == null || containingAtom.mainController == null)
        {
            return;
        }

        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        containingAtom.mainController.transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
    }

    void ApplySitGroundPoseIfNeeded()
    {
        if (GetLowTargetAction() != LowTargetActionSitGroundPose)
        {
            return;
        }

        if (!lowTargetActionReached)
        {
            DebugLog("[TargetLinePerson] Sit Ground Pose skipped / reason=low target action / hipDrop=" + lowTargetActionHipDrop.ToString("F3") + " / threshold=" + LowTargetLegUnlockMinDrop.ToString("F3"));
            return;
        }

        DebugLog("[TargetLinePerson] Sit Ground Pose applied by Low Target Action / hipDrop=" + lowTargetActionHipDrop.ToString("F3"));
        ApplySitGroundPresetPose();
    }

    bool IsSitGroundYReached()
    {
        float threshold = sitGroundYThreshold != null ? sitGroundYThreshold.val : 0.35f;
        FreeControllerV3 ownHip = GetOwnHip();
        float y = ownHip != null ? ownHip.transform.position.y : capturedOrigin.y;
        return y <= threshold;
    }

    void ApplySitGroundPresetPose()
    {
        // Captured sitting/low-ground pose.
        // This button only applies local pose values.
        // It does NOT move/rotate Person root.
        ApplyLocalPose("hipControl",    new Vector3( 0.001f, 0.275f, -0.003f), new Quaternion( 0.000f, -0.115f,  0.000f,  0.993f));
        ApplyLocalPose("chestControl",  new Vector3( 0.007f, 0.483f, -0.031f), new Quaternion( 0.001f,  0.115f,  0.000f, -0.993f));
        ApplyLocalPose("headControl",   new Vector3( 0.004f, 0.822f, -0.019f), new Quaternion( 0.009f, -0.115f,  0.001f,  0.993f));

        ApplyLocalPose("rHandControl",  new Vector3( 0.356f, 0.161f, -0.043f), new Quaternion( 0.207f,  0.213f,  0.640f, -0.709f));
        ApplyLocalPose("lHandControl",  new Vector3(-0.300f, 0.161f, -0.197f), new Quaternion( 0.348f, -0.046f, -0.576f, -0.739f));

        ApplyLocalPose("rFootControl",  new Vector3( 0.374f, 0.055f, -0.256f), new Quaternion( 0.826f,  0.116f,  0.125f,  0.537f));
        ApplyLocalPose("lFootControl",  new Vector3(-0.221f, 0.055f, -0.396f), new Quaternion( 0.833f, -0.235f,  0.068f,  0.496f));

        ApplyLocalPose("rKneeControl",  new Vector3( 0.466f,-0.018f,  0.118f), new Quaternion( 0.756f,  0.040f,  0.075f,  0.649f));
        ApplyLocalPose("lKneeControl",  new Vector3(-0.470f,-0.018f, -0.101f), new Quaternion( 0.753f, -0.187f,  0.100f,  0.623f));

        ApplyLocalPose("rElbowControl", new Vector3( 0.270f, 0.388f, -0.075f), new Quaternion(-0.009f,  0.110f,  0.568f, -0.815f));
        ApplyLocalPose("lElbowControl", new Vector3(-0.208f, 0.388f, -0.187f), new Quaternion( 0.120f,  0.079f, -0.556f, -0.819f));

        DebugLog("[TargetLinePerson] Sit Ground pose applied.");
    }
bool ApplyAutoLieOnRidePoseIfNeeded()
{
    if (autoLieOnRidePose == null || !autoLieOnRidePose.val)
    {
        return false;
    }

    if (!IsTargetRideLikePose())
    {
        return false;
    }

    // Ride-like poses keep the current horizontal direction.
    OverrideCapturedDirectionForRidePose();

    // Ride-like poses use the fixed Lie On Back preset.
    ApplyLieOnBackPresetPose();
    rideLieActive = true;
    EnsureLieDockingSafeDistance("auto-lie");
    DebugLog("[TargetLinePerson] Auto Lie On Ride Pose applied: Lie On Back fixed / genital dir ignored.");
    return true;
}

void EnsureLieDockingSafeDistance(string reason)
{
    if (distance == null)
    {
        return;
    }

    if (distance.val >= LieDockingSafeDistance - 0.0001f)
    {
        return;
    }

    distance.valNoCallback = LieDockingSafeDistance;
    nowDockingKeepCurrentPlacement = false;
    nowDockingLieNearKeepOrientation = false;
    nowDockingLineFitActive = true;
    DebugLog(
        "[TargetLinePerson] Lie docking safe distance" +
        " / reason=" + reason +
        " / distance=" + LieDockingSafeDistance.ToString("F3")
    );
}

void ReleaseRideLieIfNeeded()
{
    if (!rideLieActive)
    {
        return;
    }

    rideLieActive = false;

    // Ride邵ｺ・ｧ騾包ｽｷ隲､・ｧ陋幢ｽｴ郢ｧ魍・e On Back邵ｺ・ｫ邵ｺ蜉ｱ笳・募ｾ個竏ｵ・ｬ・｡邵ｺ・ｮCapture邵ｺ・ｧRide隴夲ｽ｡闔会ｽｶ邵ｺ謔滂ｽ､謔ｶ・檎ｸｺ貅假ｽ・    // Re-apply upper body direction after ride lie is released.
    ApplyUpperBodyDirection();
    DebugLog("[TargetLinePerson] Ride Lie released: Upper Body Direction + head aligned applied.");
}

void OverrideCapturedDirectionForRidePose()
{
    Atom targetAtom = FindAtom(targetPersonChooser.val);
    if (targetAtom == null)
    {
        return;
    }

    FreeControllerV3 ownHip = GetOwnHip();
    FreeControllerV3 targetHip = GetTargetHipController(targetAtom);

    if (ownHip == null || targetHip == null)
    {
        return;
    }

    Vector3 dir = ownHip.transform.position - targetHip.transform.position;
    dir.y = 0f;

    if (dir.sqrMagnitude < 0.0001f)
    {
        return;
    }

    dir.Normalize();

    capturedDir = dir;
    capturedLineDir = dir;
}

void ApplyLieDockingYawLockIfNeeded(bool hardMode, bool chooseCurrentSide, bool reverseCurrentSide)
{
    if (!rideLieActive && !IsOwnLiePoseForYellowGuide())
    {
        return;
    }

    Atom targetAtom = FindAtom(targetPersonChooser.val);
    if (targetAtom == null)
    {
        return;
    }

    Vector3 targetForward = GetTargetRootForward(targetAtom, capturedDir);
    targetForward.y = 0f;

    if (targetForward.sqrMagnitude < 0.0001f && capturedDir.sqrMagnitude >= 0.0001f)
    {
        targetForward = capturedDir;
        targetForward.y = 0f;
    }

    if (targetForward.sqrMagnitude < 0.0001f)
    {
        return;
    }

    targetForward.Normalize();

    bool opposite = hardMode;
    float currentSideDot = 0f;
    string selectMode = hardMode ? "reverse-smart" : "smart";

    if (chooseCurrentSide)
    {
        FreeControllerV3 ownHip = GetOwnHip();
        Vector3 toOwn = Vector3.zero;

        if (ownHip != null)
        {
            toOwn = ownHip.transform.position - capturedOrigin;
            toOwn.y = 0f;
        }

        if (toOwn.sqrMagnitude >= 0.0001f)
        {
            currentSideDot = Vector3.Dot(toOwn.normalized, targetForward);
            // Pick the opposite side when the owner is behind target root forward.
            opposite = currentSideDot > 0f;
        }
        else if (containingAtom != null && containingAtom.mainController != null)
        {
            Vector3 ownForward = containingAtom.mainController.transform.forward;
            ownForward.y = 0f;
            if (ownForward.sqrMagnitude >= 0.0001f)
            {
                currentSideDot = Vector3.Dot(ownForward.normalized, targetForward);
                opposite = currentSideDot < 0f;
            }
        }

        if (reverseCurrentSide)
        {
            opposite = !opposite;
        }

        selectMode = reverseCurrentSide ? "now-reverse-current-side" : "now-current-side";
    }

    // Preference flip: keep the stable same/opposite 2-choice lock,
    // but use the other front/back side for Lie docking.
    opposite = !opposite;

    Vector3 desiredRootForward = opposite ? -targetForward : targetForward;
    desiredRootForward.y = 0f;

    if (desiredRootForward.sqrMagnitude < 0.0001f)
    {
        return;
    }

    desiredRootForward.Normalize();

    lieDockingYawLockActive = true;
    lieDockingYawLockForward = desiredRootForward;
    lieDockingYawLockOpposite = opposite;

    // In normal standing docking, capturedDir is the position direction from target to own hip,
    // while root yaw faces back toward the target.  For Lie, keep that relationship stable:
    // root yaw = same/opposite target root yaw, position line = the opposite side of that yaw.
    capturedDir = -desiredRootForward;

    if (capturedLineDir.sqrMagnitude < 0.0001f)
    {
        capturedLineDir = capturedDir;
    }

    if (chooseCurrentSide)
    {
        FreeControllerV3 ownHip = GetOwnHip();
        float lieFitSideDot = 1f;

        if (ownHip != null)
        {
            Vector3 toOwn = ownHip.transform.position - capturedOrigin;
            toOwn.y = 0f;

            if (toOwn.sqrMagnitude >= 0.0001f)
            {
                lieFitSideDot = Vector3.Dot(toOwn.normalized, capturedDir.normalized);
            }
        }

        FitNowDockingDistanceToCurrentLine(lieFitSideDot);
    }

    if (chooseCurrentSide)
    {
        EnsureLieDockingSafeDistance("lie-yaw-lock");
    }

    DebugLog(
        "[TargetLinePerson] LIE DOCKING YAW LOCK" +
        " / mode=" + selectMode +
        " / yaw=" + (opposite ? "opposite" : "same") +
        " / frontBackFlip=1" +
        " / currentSideDot=" + currentSideDot.ToString("F3") +
        " / targetForward=(" + FormatVector3(targetForward) + ")" +
        " / rootForward=(" + FormatVector3(lieDockingYawLockForward) + ")" +
        " / capturedDir=(" + FormatVector3(capturedDir) + ")" +
        " / lineDir=(" + FormatVector3(capturedLineDir) + ")"
    );
}

bool IsTargetRideLikePose()
{
    Atom targetAtom = FindAtom(targetPersonChooser.val);
    if (targetAtom == null)
    {
        return false;
    }

    FreeControllerV3 hip = GetTargetHipController(targetAtom);
    FreeControllerV3 chest = GetTargetChestController(targetAtom);
    FreeControllerV3 rKnee = FindControllerExact(targetAtom, "rKneeControl");
    FreeControllerV3 lKnee = FindControllerExact(targetAtom, "lKneeControl");

    if (rKnee == null) rKnee = FindController(targetAtom, "rknee");
    if (lKnee == null) lKnee = FindController(targetAtom, "lknee");

    if (hip == null || chest == null || rKnee == null || lKnee == null)
    {
        return false;
    }

    Vector3 hipPos = hip.transform.position;
    Vector3 chestPos = chest.transform.position;
    Vector3 rKneePos = rKnee.transform.position;
    Vector3 lKneePos = lKnee.transform.position;

    Vector3 upper = chestPos - hipPos;
    if (upper.sqrMagnitude < 0.0001f)
    {
        return false;
    }

    // 1. 闕ｳ髮∵ｿ髴・ｽｫ邵ｺ蠕娯括邵ｺ・ｼ陜吶ｉ蟲ｩ
    // 1. Upper body verticality.
    float upperVerticalDot = Mathf.Abs(Vector3.Dot(upper.normalized, Vector3.up));

    // 2. Hip height.
    float hipY = hipPos.y;

    // 3. Knee height.
    float kneeY = (rKneePos.y + lKneePos.y) * 0.5f;
    float hipKneeDiff = Mathf.Abs(hipY - kneeY);

    bool upperAlmostVertical = upperVerticalDot >= 0.80f;
    bool hipLow = hipY <= 0.65f;
    bool hipNearKneeHeight = hipKneeDiff <= 0.20f;

    return upperAlmostVertical && hipLow && hipNearKneeHeight;
}
    void ApplyLieOnBackPresetPose()
    {
        ResetPFollowForLiePose("lie on back preset");

        // Height-adjusted preset from captured VaM pose.
        // This button only applies local pose values.
        // It does NOT move/rotate Person root.

        ApplyLocalPose("hipControl",   new Vector3( 0.000f, 0.195f, -0.136f), new Quaternion( 0.707f,  0.000f,  0.000f, -0.707f));
        ApplyLocalPose("chestControl", new Vector3( 0.000f, 0.163f, -0.350f), new Quaternion(-0.708f,  0.000f,  0.000f,  0.706f));
        ApplyLocalPose("headControl",  new Vector3( 0.000f, 0.177f, -0.699f), new Quaternion(-0.701f,  0.000f,  0.000f,  0.713f));

        ApplyLocalPose("rHandControl", new Vector3( 0.293f, 0.243f, -0.009f), new Quaternion(-0.647f, -0.603f, -0.302f,  0.355f));
        ApplyLocalPose("lHandControl", new Vector3(-0.277f, 0.248f, -0.004f), new Quaternion(-0.645f,  0.601f,  0.297f,  0.367f));

        ApplyLocalPose("rFootControl", new Vector3( 0.109f, 0.170f,  0.859f), new Quaternion(-0.578f,  0.088f, -0.088f,  0.806f));
        ApplyLocalPose("lFootControl", new Vector3(-0.109f, 0.170f,  0.859f), new Quaternion(-0.578f, -0.088f,  0.088f,  0.806f));

        ApplyLocalPose("rKneeControl", new Vector3( 0.098f, 0.160f,  0.381f), new Quaternion( 0.706f, -0.039f,  0.032f, -0.706f));
        ApplyLocalPose("lKneeControl", new Vector3(-0.098f, 0.160f,  0.381f), new Quaternion( 0.706f,  0.039f, -0.032f, -0.706f));

        ApplyLocalPose("rElbowControl", new Vector3( 0.252f, 0.188f, -0.261f), new Quaternion(-0.546f, -0.549f, -0.347f,  0.529f));
        ApplyLocalPose("lElbowControl", new Vector3(-0.248f, 0.180f, -0.260f), new Quaternion(-0.542f,  0.574f,  0.337f,  0.513f));

        DebugLog("[TargetLinePerson] Lie On Back preset pose applied.");
    }

    void ApplyLieOnFrontPresetPose()
    {
        ResetPFollowForLiePose("lie on front preset");

        // Real captured Front pose.
        // No Z inversion. No root rotation. No root movement.
        ApplyLocalPose("hipControl",   new Vector3( 0.000f, 0.195f,  0.136f), new Quaternion( 0.000f,  0.707f,  0.707f,  0.000f));
        ApplyLocalPose("chestControl", new Vector3( 0.000f, 0.163f,  0.350f), new Quaternion( 0.000f, -0.706f, -0.708f,  0.000f));
        ApplyLocalPose("headControl",  new Vector3( 0.000f, 0.177f,  0.699f), new Quaternion( 0.000f, -0.713f, -0.701f,  0.000f));

        ApplyLocalPose("rHandControl", new Vector3(-0.293f, 0.243f,  0.009f), new Quaternion( 0.302f, -0.355f, -0.647f, -0.603f));
        ApplyLocalPose("lHandControl", new Vector3( 0.277f, 0.248f,  0.004f), new Quaternion(-0.297f, -0.367f, -0.645f,  0.601f));

        ApplyLocalPose("rFootControl", new Vector3(-0.109f, 0.170f, -0.859f), new Quaternion( 0.088f, -0.806f, -0.578f,  0.088f));
        ApplyLocalPose("lFootControl", new Vector3( 0.109f, 0.170f, -0.859f), new Quaternion(-0.088f, -0.806f, -0.578f, -0.088f));

        ApplyLocalPose("rKneeControl", new Vector3(-0.098f, 0.160f, -0.381f), new Quaternion(-0.032f,  0.706f,  0.706f, -0.039f));
        ApplyLocalPose("lKneeControl", new Vector3( 0.098f, 0.160f, -0.381f), new Quaternion( 0.032f,  0.706f,  0.706f,  0.039f));

        ApplyLocalPose("rElbowControl", new Vector3(-0.252f, 0.188f,  0.261f), new Quaternion( 0.347f, -0.529f, -0.546f, -0.549f));
        ApplyLocalPose("lElbowControl", new Vector3( 0.248f, 0.180f,  0.260f), new Quaternion(-0.337f, -0.513f, -0.542f,  0.574f));

        DebugLog("[TargetLinePerson] Lie On Front captured preset pose applied.");
    }



    void ApplyLocalPose(string controllerName, Vector3 localPosition, Quaternion localRotation)
    {
        FreeControllerV3 fc = FindControllerExact(containingAtom, controllerName);

        if (fc == null)
        {
            return;
        }

        fc.currentPositionState = FreeControllerV3.PositionState.On;
        fc.currentRotationState = FreeControllerV3.RotationState.On;

        if (fc.control != null)
        {
            fc.control.localPosition = localPosition;
            fc.control.localRotation = localRotation;
        }
        else
        {
            fc.transform.localPosition = localPosition;
            fc.transform.localRotation = localRotation;
        }
    }

    void MirrorPoseLeftRight()
    {
        if (containingAtom == null || containingAtom.mainController == null || containingAtom.freeControllers == null)
        {
            return;
        }

        Transform root = containingAtom.mainController.transform;

        // Mirror around the root right plane.
        Vector3 mirrorCenter = root.position;
        Vector3 mirrorNormal = root.right;
        mirrorNormal.y = 0f;

        if (mirrorNormal.sqrMagnitude < 0.0001f)
        {
            mirrorNormal = Vector3.right;
        }

        mirrorNormal.Normalize();

        Dictionary<string, PoseSnapshot> before = CaptureControllerSnapshots();

        ApplyMirroredPair(before, "lHandControl", "rHandControl", mirrorCenter, mirrorNormal);
        ApplyMirroredPair(before, "lElbowControl", "rElbowControl", mirrorCenter, mirrorNormal);
        ApplyMirroredPair(before, "lKneeControl", "rKneeControl", mirrorCenter, mirrorNormal);
        ApplyMirroredPair(before, "lFootControl", "rFootControl", mirrorCenter, mirrorNormal);
        ApplyMirroredPair(before, "lToeControl", "rToeControl", mirrorCenter, mirrorNormal);
        ApplyMirroredPair(before, "lHeelControl", "rHeelControl", mirrorCenter, mirrorNormal);

        ApplyMirroredSingle(before, "hipControl", mirrorCenter, mirrorNormal);
        ApplyMirroredSingle(before, "pelvisControl", mirrorCenter, mirrorNormal);
        ApplyMirroredSingle(before, "abdomenControl", mirrorCenter, mirrorNormal);
        ApplyMirroredSingle(before, "chestControl", mirrorCenter, mirrorNormal);
        ApplyMirroredSingle(before, "neckControl", mirrorCenter, mirrorNormal);
        ApplyMirroredSingle(before, "headControl", mirrorCenter, mirrorNormal);

        DebugLog("[TargetLinePerson] Mirror Pose applied.");
    }

    struct PoseSnapshot
    {
        public Vector3 position;
        public Quaternion rotation;

        public PoseSnapshot(Vector3 p, Quaternion r)
        {
            position = p;
            rotation = r;
        }
    }

    Dictionary<string, PoseSnapshot> CaptureControllerSnapshots()
    {
        Dictionary<string, PoseSnapshot> result = new Dictionary<string, PoseSnapshot>();

        if (containingAtom == null || containingAtom.freeControllers == null)
        {
            return result;
        }

        foreach (FreeControllerV3 fc in containingAtom.freeControllers)
        {
            if (fc == null || fc.name == null || fc.transform == null) continue;
            if (IsRootController(containingAtom, fc)) continue;

            result[fc.name] = new PoseSnapshot(fc.transform.position, fc.transform.rotation);
        }

        return result;
    }

    void ApplyMirroredPair(Dictionary<string, PoseSnapshot> before, string leftName, string rightName, Vector3 mirrorCenter, Vector3 mirrorNormal)
    {
        FreeControllerV3 left = FindControllerExact(containingAtom, leftName);
        FreeControllerV3 right = FindControllerExact(containingAtom, rightName);

        bool hasLeft = left != null && before.ContainsKey(leftName);
        bool hasRight = right != null && before.ContainsKey(rightName);

        if (hasLeft && hasRight)
        {
            PoseSnapshot leftSnap = before[leftName];
            PoseSnapshot rightSnap = before[rightName];

            ApplySnapshotMirrored(left, rightSnap, mirrorCenter, mirrorNormal);
            ApplySnapshotMirrored(right, leftSnap, mirrorCenter, mirrorNormal);
        }
        else if (hasLeft)
        {
            ApplySnapshotMirrored(left, before[leftName], mirrorCenter, mirrorNormal);
        }
        else if (hasRight)
        {
            ApplySnapshotMirrored(right, before[rightName], mirrorCenter, mirrorNormal);
        }
    }

    void ApplyMirroredSingle(Dictionary<string, PoseSnapshot> before, string controllerName, Vector3 mirrorCenter, Vector3 mirrorNormal)
    {
        FreeControllerV3 fc = FindControllerExact(containingAtom, controllerName);

        if (fc == null || !before.ContainsKey(controllerName))
        {
            return;
        }

        ApplySnapshotMirrored(fc, before[controllerName], mirrorCenter, mirrorNormal);
    }

    void ApplySnapshotMirrored(FreeControllerV3 fc, PoseSnapshot snap, Vector3 mirrorCenter, Vector3 mirrorNormal)
    {
        if (fc == null || fc.transform == null)
        {
            return;
        }

        Vector3 mirroredPosition = MirrorPointAcrossPlane(snap.position, mirrorCenter, mirrorNormal);
        Quaternion mirroredRotation = MirrorRotationAcrossPlane(snap.rotation, mirrorNormal);

        fc.transform.position = mirroredPosition;
        fc.transform.rotation = mirroredRotation;

        if (fc.control != null)
        {
            fc.control.position = mirroredPosition;
            fc.control.rotation = mirroredRotation;
        }
    }

    Vector3 MirrorPointAcrossPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
    {
        Vector3 relative = point - planePoint;
        return planePoint + MirrorVectorAcrossPlane(relative, planeNormal);
    }

    Quaternion MirrorRotationAcrossPlane(Quaternion rotation, Vector3 planeNormal)
    {
        Vector3 forward = MirrorVectorAcrossPlane(rotation * Vector3.forward, planeNormal);
        Vector3 up = MirrorVectorAcrossPlane(rotation * Vector3.up, planeNormal);

        if (forward.sqrMagnitude < 0.0001f)
        {
            forward = Vector3.forward;
        }

        if (up.sqrMagnitude < 0.0001f)
        {
            up = Vector3.up;
        }

        return Quaternion.LookRotation(forward.normalized, up.normalized);
    }

    void UprightUpperBody()
    {
        FreeControllerV3 ownHip = GetOwnHip();
        FreeControllerV3 ownChest = GetOwnChest();
        FreeControllerV3 ownHead = GetOwnHead();

        if (ownHip == null)
        {
            return;
        }

        if (ownChest != null)
        {
            Vector3 chestOffset = ownChest.transform.position - ownHip.transform.position;
            float chestHeight = Mathf.Max(0.15f, chestOffset.magnitude);
            ownChest.transform.position = ownHip.transform.position + Vector3.up * chestHeight;
        }

        if (ownHead != null)
        {
            Vector3 headOffset = ownHead.transform.position - ownHip.transform.position;
            float headHeight = Mathf.Max(0.35f, headOffset.magnitude);
            ownHead.transform.position = ownHip.transform.position + Vector3.up * headHeight;
        }
    }

    void ApplyUpperBodyDirection()
    {
        UprightUpperBody();
        AlignOwnHeadToUpperBody();
    }

    void AlignOwnHeadToUpperBody()
    {
        FreeControllerV3 ownHead = GetOwnHead();
        FreeControllerV3 ownChest = GetOwnChest();
        FreeControllerV3 ownHip = GetOwnHip();

        if (ownHead == null || ownChest == null || ownHip == null)
        {
            return;
        }

        // Use hip-to-chest as body up direction.
        Vector3 up = ownChest.transform.position - ownHip.transform.position;

        if (up.sqrMagnitude < 0.0001f)
        {
            up = Vector3.up;
        }

        up.Normalize();

        // Align head control to body forward without rotating the root.
        Vector3 forward = containingAtom != null && containingAtom.mainController != null
            ? containingAtom.mainController.transform.forward
            : Vector3.forward;

        forward = Vector3.ProjectOnPlane(forward, up);

        if (forward.sqrMagnitude < 0.0001f)
        {
            forward = Vector3.ProjectOnPlane(ownChest.transform.forward, up);
        }

        if (forward.sqrMagnitude < 0.0001f)
        {
            forward = Vector3.forward;
        }

        forward.Normalize();

        Quaternion alignedRotation = Quaternion.LookRotation(forward, up);

        ownHead.currentRotationState = FreeControllerV3.RotationState.On;

        if (ownHead.control != null)
        {
            ownHead.control.rotation = alignedRotation;
        }
        else
        {
            ownHead.transform.rotation = alignedRotation;
        }
    }

    IEnumerator AvoidCaptureMoveRoutine()
    {
        FreeControllerV3 ownHip = GetOwnHip();

        if (ownHip == null || containingAtom == null || containingAtom.mainController == null)
        {
            yield break;
        }

        isAvoidMoving = true;

        float sideAngle = GetAvoidSideAngle(ownHip);
        Vector3 startRoot = containingAtom.mainController.transform.position;
        Vector3 sideHipTarget = GetTargetHipPositionAtAngle(ownHip, sideAngle);
        Vector3 finalHipTarget = GetTargetHipPositionAtAngle(ownHip, orbitAngle.val);

        Vector3 sideRoot = startRoot + HorizontalDelta(ownHip.transform.position, sideHipTarget);
        Vector3 finalRoot = sideRoot + HorizontalDelta(sideHipTarget, finalHipTarget);

        float duration = avoidCaptureDuration != null ? avoidCaptureDuration.val : 1.0f;
        float halfDuration = Mathf.Max(0.05f, duration * 0.5f);

        yield return MoveRootRoutine(startRoot, sideRoot, halfDuration);
        yield return MoveRootRoutine(sideRoot, finalRoot, halfDuration);

        isAvoidMoving = false;
        avoidCaptureRoutine = null;
        FaceCapturedOriginFromOwnHip(GetOwnHip());

        // Avoid movement also changes the final body/P-base position, so lock the
        // green/yellow guide only after the final avoid placement is complete and settled.
        ScheduleDelayedLineLock("avoid-after-placement");

        StartCoroutine(RestoreLimbStateDelayed());
    }

    IEnumerator MoveRootRoutine(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float eased = Mathf.SmoothStep(0f, 1f, t);

            containingAtom.mainController.transform.position = Vector3.Lerp(from, to, eased);
            FaceCapturedOriginFromOwnHip(GetOwnHip());

            yield return null;
        }

        containingAtom.mainController.transform.position = to;
        FaceCapturedOriginFromOwnHip(GetOwnHip());
    }

    Vector3 HorizontalDelta(Vector3 from, Vector3 to)
    {
        Vector3 delta = to - from;
        delta.y = 0f;
        return delta;
    }

    void FaceCapturedOriginFromOwnHip(FreeControllerV3 ownHip)
    {
        if (lieDockingYawLockActive && lieDockingYawLockForward.sqrMagnitude >= 0.0001f)
        {
            FaceDirection(lieDockingYawLockForward);
            return;
        }

        if (ownHip == null || containingAtom == null || containingAtom.mainController == null)
        {
            return;
        }

        Vector3 lookDir = capturedOrigin - ownHip.transform.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        containingAtom.mainController.transform.rotation = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
    }

    void LoadPoseUserDefaults()
    {
        string[] actionNames =
        {
            "Load User Defaults",
            "LoadUserDefaults",
            "Load User Default",
            "LoadUserDefault",
            "Load Defaults",
            "LoadDefaults"
        };

        if (TryExecutePosePresetAction(actionNames))
        {
            ReleasePYellowController(GetOwnPenisBase());
            ReleasePYellowController(GetOwnPenisMid());
            ReleasePYellowController(GetOwnPenisTip());
            DebugLog("[TargetLinePerson] LOAD VaM USER DEF: executed.");
            return;
        }

        LogMessageIfDebug("[TargetLinePerson] LOAD VaM USER DEF: PosePresets action not found.");
    }

    bool TryExecutePosePresetAction(string[] actionNames)
    {
        if (containingAtom == null) return false;

        foreach (string storableId in containingAtom.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId)) continue;
            if (storableId.IndexOf("PosePresets", StringComparison.OrdinalIgnoreCase) < 0) continue;

            JSONStorable storable = containingAtom.GetStorableByID(storableId);
            if (storable == null) continue;

            for (int i = 0; i < actionNames.Length; i++)
            {
                JSONStorableAction action = storable.GetAction(actionNames[i]);
                if (action == null) continue;

                action.actionCallback.Invoke();
                DebugLog("[TargetLinePerson] LOAD VaM USER DEF: pose action=" + storableId + " / " + actionNames[i]);
                return true;
            }
        }

        return false;
    }

    void SetOwnKneeIkOffIfNeeded()
    {
        SetControllerOff(FindController(containingAtom, "rknee"));
        SetControllerOff(FindController(containingAtom, "lknee"));
        SetControllerOff(FindController(containingAtom, "r knee"));
        SetControllerOff(FindController(containingAtom, "l knee"));
    }

    void SetOwnKneeAndFootIkOff()
    {
        SetOwnKneeIkOffIfNeeded();
        SetControllerOff(FindControllerExact(containingAtom, "rFootControl"));
        SetControllerOff(FindControllerExact(containingAtom, "lFootControl"));
        SetControllerOff(FindController(containingAtom, "rfoot"));
        SetControllerOff(FindController(containingAtom, "lfoot"));
        SetControllerOff(FindController(containingAtom, "r foot"));
        SetControllerOff(FindController(containingAtom, "l foot"));
    }

    void SetOwnKneeAndFootIkOn()
    {
        SetControllerOn(FindControllerExact(containingAtom, "rKneeControl"));
        SetControllerOn(FindControllerExact(containingAtom, "lKneeControl"));
        SetControllerOn(FindControllerExact(containingAtom, "rFootControl"));
        SetControllerOn(FindControllerExact(containingAtom, "lFootControl"));
        SetControllerOn(FindController(containingAtom, "rknee"));
        SetControllerOn(FindController(containingAtom, "lknee"));
        SetControllerOn(FindController(containingAtom, "r knee"));
        SetControllerOn(FindController(containingAtom, "l knee"));
        SetControllerOn(FindController(containingAtom, "rfoot"));
        SetControllerOn(FindController(containingAtom, "lfoot"));
        SetControllerOn(FindController(containingAtom, "r foot"));
        SetControllerOn(FindController(containingAtom, "l foot"));
    }

    void SetControllerOn(FreeControllerV3 fc)
    {
        if (fc == null)
        {
            return;
        }

        fc.currentPositionState = FreeControllerV3.PositionState.On;
        fc.currentRotationState = FreeControllerV3.RotationState.On;
    }

    void SetControllerOff(FreeControllerV3 fc)
    {
        if (fc == null)
        {
            return;
        }

        fc.currentPositionState = FreeControllerV3.PositionState.Off;
        fc.currentRotationState = FreeControllerV3.RotationState.Off;
    }

    void CreateDebugLines()
    {
        forwardLineObj = new GameObject("TargetLinePerson_Forward");
        moveLineObj = new GameObject("TargetLinePerson_Move");
        penisPathLineObj = new GameObject("TargetLinePerson_PenisPath_Yellow");
        bendMarkerLineObj = new GameObject("TargetLinePerson_BendMarker_Purple");
        gDepthGuideLineObj = new GameObject("TargetLinePerson_GDepthGuide_Cyan");

        forwardLine = forwardLineObj.AddComponent<LineRenderer>();
        moveLine = moveLineObj.AddComponent<LineRenderer>();
        penisPathLine = penisPathLineObj.AddComponent<LineRenderer>();
        bendMarkerLine = bendMarkerLineObj.AddComponent<LineRenderer>();
        gDepthGuideLine = gDepthGuideLineObj.AddComponent<LineRenderer>();

        // Original debug lines restored.
        SetupLine(forwardLine, Color.red);
        SetupLine(moveLine, Color.green);

        // Extra debug lines: yellow shows the intended P path, purple marks the bend point.
        SetupLine(penisPathLine, Color.yellow);
        SetupLine(bendMarkerLine, new Color(1f, 0f, 1f, 1f));
        SetupLine(gDepthGuideLine, Color.cyan);

        if (penisPathLine != null)
        {
            penisPathLine.startWidth = 0.025f;
            penisPathLine.endWidth = 0.025f;
        }

        if (bendMarkerLine != null)
        {
            bendMarkerLine.startWidth = 0.035f;
            bendMarkerLine.endWidth = 0.035f;
        }

        if (gDepthGuideLine != null)
        {
            Color solidCyan = new Color(0.0f, 1.0f, 1.0f, 1.0f);
            gDepthGuideLine.startWidth = 0.055f;
            gDepthGuideLine.endWidth = 0.055f;
            gDepthGuideLine.startColor = solidCyan;
            gDepthGuideLine.endColor = solidCyan;
            if (gDepthGuideLine.material != null)
            {
                gDepthGuideLine.material.color = solidCyan;
            }
        }
    }

    void SetupLine(LineRenderer lr, Color color)
    {
        Color transparentColor = new Color(color.r, color.g, color.b, GuideLineAlpha);

        lr.positionCount = 2;
        lr.startWidth = 0.015f;
        lr.endWidth = 0.015f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.material.color = transparentColor;
        lr.startColor = transparentColor;
        lr.endColor = transparentColor;
        lr.enabled = false;
        lr.useWorldSpace = true;
    }

    void CreateInsertDebugOverlay()
    {
        CleanupLegacyInsertDebugObjects();

        genDepthHudBackMaterial = CreateGenDepthHudMaterial(new Color(0.55f, 0.0f, 0.42f, 0.90f));
        genDepthHudFillMaterial = CreateGenDepthHudMaterial(new Color(1.0f, 0.0f, 0.82f, 0.85f));
        genDepthHudMarkerMaterial = CreateGenDepthHudMaterial(new Color(1.0f, 1.0f, 1.0f, 0.95f));
        genDepthHudPeakMaterial = CreateGenDepthHudMaterial(new Color(1.0f, 0.92f, 0.05f, 0.45f));
        genDepthHudGContactDotMaterial = CreateGenDepthHudMaterial(new Color(1.0f, 0.45f, 0.84f, GenDepthHudGContactDotMinAlpha));
        anusDepthHudBackMaterial = CreateGenDepthHudMaterial(new Color(0.42f, 0.05f, 0.22f, 0.82f));
        anusDepthHudFillMaterial = CreateGenDepthHudMaterial(new Color(1.0f, 0.32f, 0.58f, 0.92f));
        anusDepthHudMarkerMaterial = CreateGenDepthHudMaterial(new Color(1.0f, 0.78f, 0.90f, 0.95f));
        anusDepthHudStarMaterial = CreateGenDepthHudMaterial(new Color(1.0f, 0.58f, 0.78f, 0.96f));
        genDepthBurstMaterials = CreateGenDepthBurstMaterials();

        genDepthHudBackObj = CreateGenDepthHudBarObject("TargetLinePerson_GenDepthHud_Back", genDepthHudBackMaterial);
        genDepthHudFillObj = CreateGenDepthHudBarObject("TargetLinePerson_GenDepthHud_Fill", genDepthHudFillMaterial);
        genDepthHudMarkerObj = CreateGenDepthHudMarkerObject("TargetLinePerson_GenDepthHud_100", genDepthHudMarkerMaterial);
        genDepthHudMarkerLine = genDepthHudMarkerObj != null ? genDepthHudMarkerObj.GetComponent<LineRenderer>() : null;
        genDepthHudBottomMarkerObj = CreateGenDepthHudMarkerObject("TargetLinePerson_GenDepthHud_0", genDepthHudMarkerMaterial);
        genDepthHudBottomMarkerLine = genDepthHudBottomMarkerObj != null ? genDepthHudBottomMarkerObj.GetComponent<LineRenderer>() : null;
        genDepthHudPeakObj = CreateGenDepthHudBarObject("TargetLinePerson_GenDepthHud_Peak", genDepthHudPeakMaterial);
        genDepthHudGContactDotObj = CreateGenDepthHudDotObject("TargetLinePerson_GenDepthHud_GContactDot", genDepthHudGContactDotMaterial);
        anusDepthHudBackObj = CreateGenDepthHudBarObject("TargetLinePerson_AnusDepthHud_Back", anusDepthHudBackMaterial);
        anusDepthHudFillObj = CreateGenDepthHudBarObject("TargetLinePerson_AnusDepthHud_Fill", anusDepthHudFillMaterial);
        anusDepthHudMarkerObj = CreateGenDepthHudBarObject("TargetLinePerson_AnusDepthHud_100", anusDepthHudMarkerMaterial);
        anusDepthHudStarObj = CreateAnusDepthHudStarObject("TargetLinePerson_AnusDepthHud_Star", anusDepthHudStarMaterial);
        genDepthPeakPercent = 0.0f;
        genDepthPeakUntil = 0.0f;
        previousGenDepthPercent = 0.0f;
        anusDepthInsertedMaxPercent = 0.0f;
        nextZeroBurstTime = 0.0f;
        nextMaxBurstTime = 0.0f;

        SetGenDepthHudActive(false);
    }

    void CleanupLegacyInsertDebugObjects()
    {
        DestroyNamedObject("TargetLinePerson_InsertDebugText");
        DestroyNamedObject("TargetLinePerson_GenDepthHud_Back");
        DestroyNamedObject("TargetLinePerson_GenDepthHud_Fill");
        DestroyNamedObject("TargetLinePerson_GenDepthHud_Text");
        DestroyNamedObject("TargetLinePerson_GenDepthHud_100");
        DestroyNamedObject("TargetLinePerson_GenDepthHud_0");
        DestroyNamedObject("TargetLinePerson_GenDepthHud_Peak");
        DestroyNamedObject("TargetLinePerson_GenDepthHud_GContactDot");
        DestroyNamedObject("TargetLinePerson_AnusDepthHud_Back");
        DestroyNamedObject("TargetLinePerson_AnusDepthHud_Fill");
        DestroyNamedObject("TargetLinePerson_AnusDepthHud_100");
        DestroyNamedObject("TargetLinePerson_AnusDepthHud_Star");
        DestroyNamedObject("TargetLinePerson_GenDepthHud_Burst");
    }

    void DestroyNamedObject(string objectName)
    {
        for (int i = 0; i < 128; i++)
        {
            GameObject go = GameObject.Find(objectName);
            if (go == null)
            {
                return;
            }

            go.name = objectName + "_Destroyed_" + i.ToString();
            Destroy(go);
        }
    }

    Material CreateGenDepthHudMaterial(Color color)
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

    Material[] CreateGenDepthBurstMaterials()
    {
        Color[] colors = new Color[]
        {
            new Color(1.0f, 0.05f, 0.05f, 0.95f),
            new Color(1.0f, 0.45f, 0.02f, 0.95f),
            new Color(1.0f, 0.95f, 0.02f, 0.95f),
            new Color(0.05f, 1.0f, 0.20f, 0.95f),
            new Color(0.05f, 0.55f, 1.0f, 0.95f),
            new Color(0.75f, 0.10f, 1.0f, 0.95f)
        };

        Material[] mats = new Material[colors.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            mats[i] = CreateGenDepthHudMaterial(colors[i]);
        }
        return mats;
    }

    GameObject CreateGenDepthHudBarObject(string objectName, Material mat)
    {
        GameObject bar = new GameObject(objectName);
        bar.name = objectName;

        LineRenderer line = bar.AddComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.loop = false;
        line.positionCount = 2;
        line.numCapVertices = 8;
        line.numCornerVertices = 4;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        if (mat != null)
        {
            line.material = new Material(mat);
            line.startColor = mat.color;
            line.endColor = mat.color;
        }

        return bar;
    }

    void SetGenDepthHudBarLine(GameObject obj, Vector3 start, Vector3 end, float width, Color color)
    {
        if (obj == null)
        {
            return;
        }

        LineRenderer line = GetCachedHudLineRenderer(obj);
        if (line == null)
        {
            return;
        }

        line.positionCount = 2;
        line.loop = false;
        line.startWidth = width;
        line.endWidth = width;
        line.startColor = color;
        line.endColor = color;
        if (line.material != null)
        {
            line.material.color = color;
        }

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    LineRenderer GetCachedHudLineRenderer(GameObject obj)
    {
        if (obj == null)
        {
            return null;
        }

        LineRenderer line;
        if (hudLineRendererCache.TryGetValue(obj, out line) && line != null)
        {
            return line;
        }

        line = obj.GetComponent<LineRenderer>();
        if (line != null)
        {
            hudLineRendererCache[obj] = line;
        }
        return line;
    }

    GameObject CreateGenDepthHudMarkerObject(string objectName, Material mat)
    {
        GameObject marker = new GameObject(objectName);
        marker.name = objectName;

        LineRenderer line = marker.AddComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.loop = true;
        line.positionCount = 48;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        if (mat != null)
        {
            line.material = new Material(mat);
        }

        return marker;
    }

    GameObject CreateGenDepthHudDotObject(string objectName, Material mat)
    {
        GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        dot.name = objectName;

        Collider col = dot.GetComponent<Collider>();
        if (col != null)
        {
            Destroy(col);
        }

        Renderer renderer = dot.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (mat != null)
            {
                renderer.material = new Material(mat);
            }
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        dot.SetActive(false);
        return dot;
    }

    GameObject CreateAnusDepthHudStarObject(string objectName, Material mat)
    {
        GameObject root = new GameObject(objectName);
        root.name = objectName;

        for (int i = 0; i < 3; i++)
        {
            GameObject child = new GameObject(objectName + "_Line" + i.ToString());
            child.transform.SetParent(root.transform, false);
            LineRenderer line = child.AddComponent<LineRenderer>();
            line.useWorldSpace = true;
            line.loop = false;
            line.positionCount = 2;
            line.numCapVertices = 6;
            line.numCornerVertices = 2;
            line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            line.receiveShadows = false;
            if (mat != null)
            {
                line.material = new Material(mat);
                line.startColor = mat.color;
                line.endColor = mat.color;
            }
        }

        root.SetActive(false);
        return root;
    }

    void SetAnusDepthHudStar(Vector3 center, Vector3 right, Vector3 up, float radius, float width, Color color, bool circleMode)
    {
        if (anusDepthHudStarObj == null)
        {
            return;
        }

        if (circleMode)
        {
            for (int i = 0; i < 3; i++)
            {
                Transform child = anusDepthHudStarObj.transform.Find("TargetLinePerson_AnusDepthHud_Star_Line" + i.ToString());
                if (child == null)
                {
                    continue;
                }

                child.gameObject.SetActive(i == 0);
                LineRenderer line = child.GetComponent<LineRenderer>();
                if (line == null || i != 0)
                {
                    continue;
                }

                line.positionCount = 48;
                line.loop = true;
                line.startWidth = width;
                line.endWidth = width;
                line.startColor = color;
                line.endColor = color;
                if (line.material != null)
                {
                    line.material.color = color;
                }

                for (int p = 0; p < line.positionCount; p++)
                {
                    float a = (Mathf.PI * 2.0f * p) / line.positionCount;
                    Vector3 pos = center + right * (Mathf.Cos(a) * radius) + up * (-Mathf.Sin(a) * radius); // v161: vertical flip draw order
                    line.SetPosition(p, pos);
                }
            }
            return;
        }

        Vector3[] dirs = new Vector3[]
        {
            up,
            (up + right).normalized,
            (up - right).normalized
        };

        for (int i = 0; i < 3; i++)
        {
            Transform child = anusDepthHudStarObj.transform.Find("TargetLinePerson_AnusDepthHud_Star_Line" + i.ToString());
            if (child == null)
            {
                continue;
            }

            child.gameObject.SetActive(true);
            LineRenderer line = child.GetComponent<LineRenderer>();
            if (line == null)
            {
                continue;
            }

            line.positionCount = 2;
            line.loop = false;
            line.startWidth = width;
            line.endWidth = width;
            line.startColor = color;
            line.endColor = color;
            if (line.material != null)
            {
                line.material.color = color;
            }
            line.SetPosition(0, center - dirs[i] * radius);
            line.SetPosition(1, center + dirs[i] * radius);
        }
    }

    void SetGenDepthHudHeart(LineRenderer line, Vector3 center, Vector3 right, Vector3 up, float radius, float width, bool flipY, Color color)
    {
        if (line == null)
        {
            return;
        }

        line.positionCount = 48;
        line.loop = true;
        line.startColor = color;
        line.endColor = color;
        line.startWidth = width;
        line.endWidth = width;
        if (line.material != null)
        {
            line.material.color = color;
        }

        float scale = radius / 18.0f;
        for (int i = 0; i < line.positionCount; i++)
        {
            float a = (Mathf.PI * 2.0f * i) / line.positionCount;
            float x = 16.0f * Mathf.Pow(Mathf.Sin(a), 3.0f);
            float y = 13.0f * Mathf.Cos(a) - 5.0f * Mathf.Cos(2.0f * a) - 2.0f * Mathf.Cos(3.0f * a) - Mathf.Cos(4.0f * a);
            if (flipY)
            {
                y = -y;
            }
            Vector3 p = center + right * (x * scale) + up * (y * scale - radius * 0.10f);
            line.SetPosition(i, p);
        }
    }

    void SetGenDepthHudDropMarker(LineRenderer line, Vector3 center, Vector3 right, Vector3 up, float radius, float shapeWidthScale, float width, Color color)
    {
        if (line == null)
        {
            return;
        }

        line.positionCount = 64;
        line.loop = true;
        line.startColor = color;
        line.endColor = color;
        line.startWidth = width;
        line.endWidth = width;
        if (line.material != null)
        {
            line.material.color = Color.white;
        }
        ApplyGenDepthHudDropGradient(line, color);

        float xRadius = radius * shapeWidthScale;
        float yRadius = radius * GenDepthHudDropHeightScale;
        for (int i = 0; i < line.positionCount; i++)
        {
            float a = (Mathf.PI * 2.0f * i) / line.positionCount;
            float y = Mathf.Cos(a);
            float top = Mathf.Clamp01(y);
            float bottom = Mathf.Clamp01(-y);
            float sideScale = 1.0f - top * top * 0.55f - bottom * bottom * 0.12f;
            Vector3 p = center + right * (Mathf.Sin(a) * xRadius * sideScale) + up * (y * yRadius);
            line.SetPosition(i, p);
        }
    }

    void ApplyGenDepthHudDropGradient(LineRenderer line, Color baseColor)
    {
        if (line == null)
        {
            return;
        }

        Color soft = Color.Lerp(baseColor, Color.white, 0.28f);
        soft.a = baseColor.a;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(baseColor, 0.0f),
                new GradientColorKey(soft, 0.50f),
                new GradientColorKey(baseColor, 1.0f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(baseColor.a, 0.0f),
                new GradientAlphaKey(baseColor.a, 1.0f)
            }
        );
        line.colorGradient = gradient;
    }

    void UpdateInsertDebugText()
    {
        double perfTotalStart = PerfNow();
        float perfMainProbeMs = 0.0f;
        float perfHudSampleMs = 0.0f;
        float perfEventProbeMs = 0.0f;
        float perfHudRenderMs = 0.0f;
        float perfReactionMs = 0.0f;
        double perfSectionStart = 0.0;

        if (IsFeatureCutNoDepthProbe())
        {
            ClearRuntimeFeatureOutputs("perf depth probe off");
            UpdateDepthHudThrottled(0.0f, GetGenDepthMax(), 0.0f, 0.0f, GetGenDepthMax(), 0.0f, false);
            if (insertDebugText != null)
            {
                string text = "Perf Depth Probe OFF";
                if (insertDebugText.val != text)
                {
                    insertDebugText.val = text;
                }
            }
            return;
        }

        if (!ShouldRunDepthProbeNow())
        {
            // Keep the previous HUD/reaction state until the next scheduled probe tick.
            // This throttles the expensive live projection/sampling group without changing placement motion.
            return;
        }

        if (!IsFeatureCutNoReactions())
        {
            lastFeatureCutReactionClearKey = "";
        }

        float depth = 0.0f;
        float length = GetGenDepthMax();
        float percent = 0.0f;
        lastPerfMainLineMs = 0.0f;
        lastPerfMainCalcMs = 0.0f;
        lastPerfMainGateMs = 0.0f;
        lastPerfMainBookMs = 0.0f;
        perfSectionStart = PerfNow();
        bool hasDepth = TryGetLiveGenDepth(out depth, out length, out percent);
        perfMainProbeMs += PerfMs(perfSectionStart);
        float hudDepth = depth;
        float hudLength = length;
        float hudPercent = percent;
        bool visible = showInsertDebug != null && showInsertDebug.val && !IsFeatureCutNoHud();
        bool hasHudDepth = hasDepth;
        float anusHudDepth = 0.0f;
        float anusHudLength = GetGenDepthMax();
        float anusHudPercent = 0.0f;
        bool hasAnusHudDepth = false;
        bool anusHudVisible = visible && targetControllerChooser != null && targetControllerChooser.val == "anus";
        perfSectionStart = PerfNow();
        if (visible && (!cachedHudDepthKnown || Time.time - lastGenDepthHudSampleTime >= GetHudSampleInterval()))
        {
            cachedHudDepth = hudDepth;
            cachedHudLength = hudLength;
            cachedHudPercent = hudPercent;
            cachedHudHasDepth = hasDepth;
            cachedAnusHudDepth = anusHudDepth;
            cachedAnusHudLength = anusHudLength;
            cachedAnusHudPercent = anusHudPercent;
            cachedAnusHudHasDepth = false;

            if (IsRuntimeRawHudProbeEnabled())
            {
                cachedHudHasDepth = TryGetLiveGenDepthForHud(out cachedHudDepth, out cachedHudLength, out cachedHudPercent);
                cachedAnusHudHasDepth = anusHudVisible && TryGetLiveAnusDepthForHud(out cachedAnusHudDepth, out cachedAnusHudLength, out cachedAnusHudPercent);
            }

            cachedHudDepthKnown = true;
            cachedAnusHudDepthKnown = true;
            lastGenDepthHudSampleTime = Time.time;
        }
        perfHudSampleMs += PerfMs(perfSectionStart);
        if (visible && cachedHudDepthKnown)
        {
            hudDepth = cachedHudDepth;
            hudLength = cachedHudLength;
            hudPercent = cachedHudPercent;
            hasHudDepth = cachedHudHasDepth;
        }
        if (visible && cachedAnusHudDepthKnown)
        {
            anusHudDepth = cachedAnusHudDepth;
            anusHudLength = cachedAnusHudLength;
            anusHudPercent = cachedAnusHudPercent;
            hasAnusHudDepth = cachedAnusHudHasDepth;
        }
        if (!hasDepth)
        {
            depth = 0.0f;
            length = GetGenDepthMax();
            percent = 0.0f;
        }
        if (!hasHudDepth)
        {
            hudDepth = 0.0f;
            hudLength = GetGenDepthMax();
            hudPercent = 0.0f;
        }
        if (!hasAnusHudDepth)
        {
            anusHudDepth = 0.0f;
            anusHudLength = GetGenDepthMax();
            anusHudPercent = 0.0f;
        }

        // Use target-specific HUD/raw depth as the public HBA progress source.
        // TargetLinePerson stays a detector/bridge only: Gen and Anus both flow into
        // the same HBA_Event_* path, while HBA_TargetId tells HumanBodyAction which target it is.
        string eventTargetMode = GetTargetModeName();
        string eventDepthSource = "none";
        float eventPercent = 0.0f;
        bool hasEventDepth = false;

        perfSectionStart = PerfNow();
        if (eventTargetMode == "anus")
        {
            if (IsRuntimeRawEventProbeEnabled())
            {
                if (cachedAnusHudDepthKnown && cachedAnusHudHasDepth)
                {
                    eventPercent = cachedAnusHudPercent;
                    hasEventDepth = true;
                    eventDepthSource = "anus-hud-depth-cache";
                }
                else
                {
                    float rawAnusDepth;
                    float rawAnusLength;
                    float rawAnusPercent;
                    if (TryGetLiveAnusDepthForHud(out rawAnusDepth, out rawAnusLength, out rawAnusPercent))
                    {
                        eventPercent = rawAnusPercent;
                        hasEventDepth = true;
                        eventDepthSource = "anus-hud-depth";
                    }
                }
            }
        }
        else
        {
            // Genital keeps the previous control-depth source, with optional raw/HUD fallback.
            if (eventTargetMode == "genital")
            {
                eventPercent = percent;
                hasEventDepth = hasDepth;
                eventDepthSource = hasEventDepth ? "gen-control-depth" : "gen-no-depth";

                if (IsRuntimeRawEventProbeEnabled())
                {
                    float rawHudPercent = -1.0f;
                    bool hasRawHud = false;

                    if (cachedHudDepthKnown && cachedHudHasDepth)
                    {
                        rawHudPercent = cachedHudPercent;
                        hasRawHud = true;
                    }
                    else
                    {
                        float rawHudDepth;
                        float rawHudLength;
                        if (TryGetLiveGenDepthForHud(out rawHudDepth, out rawHudLength, out rawHudPercent))
                        {
                            hasRawHud = true;
                        }
                    }

                    if (hasRawHud && (!hasEventDepth || rawHudPercent > eventPercent + 0.0005f))
                    {
                        eventPercent = rawHudPercent;
                        hasEventDepth = true;
                        eventDepthSource = "genital-hud-raw-depth";
                    }
                }
            }
        }
        perfEventProbeMs += PerfMs(perfSectionStart);

        perfSectionStart = PerfNow();
        UpdateDepthHudThrottled(hudDepth, hudLength, hudPercent, anusHudDepth, anusHudLength, anusHudPercent, visible);
        perfHudRenderMs += PerfMs(perfSectionStart);
        if (!IsFeatureCutNoHud())
        {
            UpdateGenDepthUiText(hasEventDepth, eventPercent, eventTargetMode);
        }
        perfSectionStart = PerfNow();
        if (IsFeatureCutNoReactions())
        {
            ClearReactionOutputsOnce("feature cut no reactions");
        }
        else
        {
            UpdateGenTgTriggers(hasEventDepth, eventPercent, eventDepthSource);
        }
        perfReactionMs += PerfMs(perfSectionStart);

        LogDepthProbePerfTiming(PerfMs(perfTotalStart), perfMainProbeMs, perfHudSampleMs, perfEventProbeMs, perfHudRenderMs, perfReactionMs);
    }

    void UpdateDepthHudThrottled(float hudDepth, float hudLength, float hudPercent, float anusHudDepth, float anusHudLength, float anusHudPercent, bool visible)
    {
        if (!visible)
        {
            if (!genDepthHudVisibleKnown || genDepthHudVisible)
            {
                UpdateGenDepthHud(0.0f, GetGenDepthMax(), 0.0f, false);
                UpdateAnusDepthHud(0.0f, GetGenDepthMax(), 0.0f, false);
                genDepthHudVisible = false;
                genDepthHudVisibleKnown = true;
            }
            return;
        }

        if (genDepthHudVisibleKnown && genDepthHudVisible && Time.time - lastGenDepthHudRenderTime < GetHudSampleInterval())
        {
            return;
        }

        UpdateGenDepthHud(hudDepth, hudLength, hudPercent, true);
        UpdateAnusDepthHud(anusHudDepth, anusHudLength, anusHudPercent, true);
        lastGenDepthHudRenderTime = Time.time;
        genDepthHudVisible = true;
        genDepthHudVisibleKnown = true;
    }

    void UpdateGenDepthUiText(bool hasDepth, float percent, string targetMode)
    {
        if (insertDebugText == null || Time.time - lastGenDepthUiTextTime < GetGenDepthUiTextInterval())
        {
            return;
        }

        bool anusMode = targetMode == "anus";
        string label = anusMode ? "Anus" : "Gen";
        float maxPercent = anusMode ? anusDepthInsertedMaxPercent : genDepthInsertedMaxPercent;
        string maxText = maxPercent > 0.001f ? BuildDepthPercentText(0.0f, 1.0f, maxPercent) : "--%";
        string text = hasDepth ? (label + " " + BuildDepthPercentText(0.0f, 1.0f, percent) + " / Max " + maxText) : (label + " --% / Max --%");

        if (insertDebugText.val != text)
        {
            insertDebugText.val = text;
        }
        lastGenDepthUiTextTime = Time.time;
    }

    void UpdateGenTgTriggers(bool hasDepth, float percent, string depthSource)
    {
        bool tgEnabled = genTgTriggers != null && genTgTriggers.val;
        bool headEnabled = genHeadActions != null && genHeadActions.val;

        if (!tgEnabled)
        {
            ForceAllGenTgOff();
        }

        if (IsSwitchRetractHbaGateActive())
        {
            LogSwitchRetractHbaGateIfNeeded(depthSource);
            return;
        }

        if (!hasDepth || (!tgEnabled && !headEnabled))
        {
            if (tgEnabled)
            {
                ForceAllGenTgOff();
            }
            genTgStartActive = false;
            genTgInsideActive = false;
            genTgDeepActive = false;
            genTgHadInside = false;
            ResetGenHeadReactionState(!hasDepth ? "no-depth" : "disabled");
            return;
        }

        bool wasStart = genTgStartActive;
        bool wasInside = genTgInsideActive;
        bool wasDeep = genTgDeepActive;

        if (genTgStartActive)
        {
            if (percent <= GenTgStartExitPercent)
            {
                genTgStartActive = false;
            }
        }
        else if (percent > GenTgStartEnterPercent)
        {
            genTgStartActive = true;
        }

        if (genTgInsideActive)
        {
            if (percent <= GenTgInsideExitPercent)
            {
                genTgInsideActive = false;
            }
        }
        else if (percent > GenTgInsideEnterPercent)
        {
            genTgInsideActive = true;
        }

        if (genTgDeepActive)
        {
            if (percent < GenTgDeepExitPercent)
            {
                genTgDeepActive = false;
            }
        }
        else if (percent >= GenTgDeepEnterPercent)
        {
            genTgDeepActive = true;
            genTgInsideActive = true;
        }

        bool started = genTgStartActive;
        bool inserted = genTgInsideActive;
        bool deep = genTgDeepActive;
        bool startEvent = !wasStart && started;
        bool insideEvent = !wasInside && inserted;
        bool deepEvent = !wasDeep && deep;
        bool endEvent = wasInside && !inserted;
        bool delayInsideReaction = insideEvent;

        if (inserted)
        {
            genTgHadInside = true;
        }

        if (delayInsideReaction)
        {
            ScheduleDelayedInsideReaction(tgEnabled, headEnabled);
        }

        if (tgEnabled)
        {
            ProcessGenTgSlot("Start", genTgStartAtom, genTgStartMode, genTgStartRuntime, started, startEvent);
            if (!delayInsideReaction)
            {
                ProcessGenTgSlot("Inside", genTgInsideAtom, genTgInsideMode, genTgInsideRuntime, inserted, insideEvent);
            }
            ProcessGenTgSlot("Deep", genTgDeepAtom, genTgDeepMode, genTgDeepRuntime, deep, deepEvent);
            ProcessGenTgSlot("End", genTgEndAtom, genTgEndMode, genTgEndRuntime, !inserted && genTgHadInside, endEvent);
        }

        if (headEnabled)
        {
            UpdateGenHeadReactionTriggers(percent, string.IsNullOrEmpty(depthSource) ? "control-depth" : depthSource, delayInsideReaction);
        }
        else
        {
            ResetGenHeadReactionState("head-disabled");
        }
    }

    void ScheduleDelayedInsideReaction(bool tgEnabledAtStart, bool headEnabledAtStart)
    {
        if (delayedInsideReactionRoutine != null)
        {
            StopCoroutine(delayedInsideReactionRoutine);
            delayedInsideReactionRoutine = null;
        }

        delayedInsideReactionRoutine = StartCoroutine(DelayedInsideReactionRoutine(tgEnabledAtStart, headEnabledAtStart));
    }

    IEnumerator DelayedInsideReactionRoutine(bool tgEnabledAtStart, bool headEnabledAtStart)
    {
        yield return null;
        delayedInsideReactionRoutine = null;

        if (!genTgInsideActive || IsSwitchRetractHbaGateActive())
        {
            yield break;
        }

        bool tgStillEnabled = tgEnabledAtStart && genTgTriggers != null && genTgTriggers.val;
        bool headStillEnabled = headEnabledAtStart && genHeadActions != null && genHeadActions.val && genHeadDepthInsideActive;

        if (tgStillEnabled)
        {
            ProcessGenTgSlot("Inside", genTgInsideAtom, genTgInsideMode, genTgInsideRuntime, true, true);
        }

        if (headStillEnabled)
        {
            ProcessGenHeadInside(true, true);
        }
    }

    void ResetGenHeadReactionState(string reason)
    {
        if (!genHeadDepthStartActive && !genHeadDepthInsideActive && !genHeadDepthDeepActive && !genHeadDepthHadInside)
        {
            return;
        }

        if (genHeadDepthInsideActive)
        {
            ProcessGenHeadActions(false, false, false, false, true);
        }

        genHeadDepthStartActive = false;
        genHeadDepthInsideActive = false;
        genHeadDepthDeepActive = false;
        genHeadDepthHadInside = false;
        genHeadInsideNextRandomTime = -1.0f;
        UpdateHbaSharedStatus(0.0f, false);

        if (IsDebugViewEnabled())
        {
            DebugLog("[TargetLinePerson] Gen Head reaction state reset / reason=" + reason);
        }
    }

    void UpdateGenHeadReactionTriggers(float controlPercent, string controlSource)
    {
        UpdateGenHeadReactionTriggers(controlPercent, controlSource, false);
    }

    void UpdateGenHeadReactionTriggers(float controlPercent, string controlSource, bool delayInsideReaction)
    {
        float reactionPercent = Mathf.Clamp(controlPercent, 0.0f, GenDepthHudDisplayMaxPercent);
        string source = controlSource;
        float fallbackPercent;
        string fallbackSource;

        if (TryGetGenHeadReactionFallbackPercent(out fallbackPercent, out fallbackSource) && fallbackPercent > reactionPercent + 0.0005f)
        {
            reactionPercent = Mathf.Clamp(fallbackPercent, 0.0f, GenDepthHudDisplayMaxPercent);
            source = fallbackSource;
        }

        bool wasStart = genHeadDepthStartActive;
        bool wasInside = genHeadDepthInsideActive;
        bool wasDeep = genHeadDepthDeepActive;

        if (genHeadDepthStartActive)
        {
            if (reactionPercent <= GenTgStartExitPercent)
            {
                genHeadDepthStartActive = false;
            }
        }
        else if (reactionPercent > GenTgStartEnterPercent)
        {
            genHeadDepthStartActive = true;
        }

        if (genHeadDepthInsideActive)
        {
            if (reactionPercent <= GenTgInsideExitPercent)
            {
                genHeadDepthInsideActive = false;
            }
        }
        else if (reactionPercent > GenTgInsideEnterPercent)
        {
            genHeadDepthInsideActive = true;
        }

        if (genHeadDepthDeepActive)
        {
            if (reactionPercent < GenTgDeepExitPercent)
            {
                genHeadDepthDeepActive = false;
            }
        }
        else if (reactionPercent >= GenTgDeepEnterPercent)
        {
            genHeadDepthDeepActive = true;
            genHeadDepthInsideActive = true;
        }

        bool started = genHeadDepthStartActive;
        bool inserted = genHeadDepthInsideActive;
        bool deep = genHeadDepthDeepActive;
        bool startEvent = !wasStart && started;
        bool insideEvent = !wasInside && inserted;
        bool deepEvent = !wasDeep && deep;
        bool endEvent = wasInside && !inserted;

        if (inserted)
        {
            genHeadDepthHadInside = true;
        }

        UpdateHbaSharedStatus(reactionPercent, inserted || started || deep);

        bool important = startEvent || insideEvent || deepEvent || endEvent || Time.time - lastGenHeadDepthSourceLogTime >= 3.0f;
        if (important && IsDebugLogEnabled() && HasHbaActionSelected())
        {
            lastGenHeadDepthSourceLogTime = Time.time;
            LogMessageIfDebug(
                "[TargetLinePerson] HBA depth source" +
                " / source=" + source +
                " / controlPercent=" + controlPercent.ToString("F3") +
                " / reactionPercent=" + reactionPercent.ToString("F3") +
                " / start=" + started +
                " / inside=" + inserted +
                " / deep=" + deep +
                " / startEvent=" + startEvent +
                " / insideEvent=" + insideEvent +
                " / deepEvent=" + deepEvent +
                " / endEvent=" + endEvent +
                " / rawDepth=" + (lastGenDepthSampleKnown ? lastGenDepthRawDepth.ToString("F3") : "n/a") +
                " / lateral=" + (lastGenDepthSampleKnown ? lastGenDepthLateral.ToString("F3") : "n/a")
            );
        }

        ProcessGenHeadActions(inserted, startEvent, delayInsideReaction ? false : insideEvent, deepEvent, endEvent);
    }

    bool HasHbaActionSelected()
    {
        return IsHbaActionName(GetGenHeadActionValue(genHeadStartAction)) ||
            IsHbaActionName(GetGenHeadActionValue(genHeadInsideAction)) ||
            IsHbaActionName(GetGenHeadActionValue(genHeadDeepAction)) ||
            IsHbaActionName(GetGenHeadActionValue(genHeadEndAction));
    }

    bool TryGetGenHeadReactionFallbackPercent(out float percent, out string source)
    {
        percent = 0.0f;
        source = "none";

        if (!IsRuntimeReactionFallbackProbeEnabled())
        {
            source = "fallback-probe-off";
            return false;
        }

        string targetMode = GetTargetModeName();
        float depth;
        float length;
        float rawPercent;

        if (targetMode == "genital")
        {
            if (TryGetLiveGenDepthForHud(out depth, out length, out rawPercent))
            {
                percent = rawPercent;
                source = "genital-hud-raw-depth";
                return true;
            }
            return false;
        }

        if (targetMode == "anus")
        {
            if (TryGetLiveAnusDepthForHud(out depth, out length, out rawPercent))
            {
                percent = rawPercent;
                source = "anus-hud-depth";
                return true;
            }
            return false;
        }

        return false;
    }


    void UpdateHbaSharedStatus(float percent, bool active)
    {
        if (Time.time - lastHbaSharedStatusTime < GetHbaSharedStatusInterval())
        {
            return;
        }

        Atom atom = GetGenHeadTargetAtom();
        if (!ResolveHbaStatusStorables(atom))
        {
            return;
        }

        if (hbaTargetIdParam != null)
        {
            float targetId = GetHbaTargetId();
            if (Mathf.Abs(hbaTargetIdParam.val - targetId) > 0.0001f)
            {
                hbaTargetIdParam.val = targetId;
            }
        }
        if (hbaProgressParam != null)
        {
            float nextProgress = Mathf.Clamp(percent, 0.0f, GenDepthHudDisplayMaxPercent);
            if (Mathf.Abs(hbaProgressParam.val - nextProgress) > 0.0005f)
            {
                hbaProgressParam.val = nextProgress;
            }
        }
        if (hbaActiveParam != null && hbaActiveParam.val != active) hbaActiveParam.val = active;
        lastHbaSharedStatusTime = Time.time;
    }

    bool ResolveHbaStatusStorables(Atom atom)
    {
        if (atom == null)
        {
            ClearHbaStatusCache();
            return false;
        }

        // During testing the HBA script may be swapped/reloaded while this plugin keeps
        // old JSONStorable references.  Re-scan periodically and prefer the current
        // HumanBodyAction bridge marker when available.
        if (hbaLinkCacheAtomUid == atom.uid && hbaTargetIdParam != null && hbaProgressParam != null && hbaActiveParam != null)
        {
            if (Time.time - hbaLinkLastResolveTime < HbaLinkRefreshInterval)
            {
                return true;
            }
        }

        ClearHbaStatusCache();
        hbaLinkCacheAtomUid = atom.uid;

        JSONStorable fallbackStorable = null;
        JSONStorableFloat fallbackTargetId = null;
        JSONStorableFloat fallbackProgress = null;
        JSONStorableBool fallbackActive = null;

        foreach (string sid in atom.GetStorableIDs())
        {
            JSONStorable storable = atom.GetStorableByID(sid);
            if (storable == null)
            {
                continue;
            }

            JSONStorableFloat targetId = storable.GetFloatJSONParam("HBA_TargetId");
            JSONStorableFloat progress = storable.GetFloatJSONParam("HBA_Progress");
            JSONStorableBool active = storable.GetBoolJSONParam("HBA_Active");
            if (targetId == null || progress == null || active == null)
            {
                continue;
            }

            JSONStorableFloat bridgeVersion = storable.GetFloatJSONParam("HBA_BridgeVersion");
            bool looksLikeHba = sid.IndexOf("HumanBodyAction", StringComparison.OrdinalIgnoreCase) >= 0;

            // v011+ exposes HBA_BridgeVersion. Prefer it so we do not write to an old HBA
            // instance left on the same Person while iterating scripts.
            if (bridgeVersion != null || looksLikeHba)
            {
                hbaTargetIdParam = targetId;
                hbaProgressParam = progress;
                hbaActiveParam = active;
                hbaLinkLastResolveTime = Time.time;
                return true;
            }

            if (fallbackStorable == null)
            {
                fallbackStorable = storable;
                fallbackTargetId = targetId;
                fallbackProgress = progress;
                fallbackActive = active;
            }
        }

        if (fallbackStorable != null)
        {
            hbaTargetIdParam = fallbackTargetId;
            hbaProgressParam = fallbackProgress;
            hbaActiveParam = fallbackActive;
            hbaLinkLastResolveTime = Time.time;
            return true;
        }

        return false;
    }

    void ClearHbaStatusCache()
    {
        hbaLinkCacheAtomUid = "";
        hbaTargetIdParam = null;
        hbaProgressParam = null;
        hbaActiveParam = null;
        hbaLinkLastResolveTime = -999.0f;
    }

    float GetHbaTargetId()
    {
        string mode = GetTargetModeName();
        if (mode == "genital") return 1.0f;
        if (mode == "anus") return 2.0f;
        if (mode == "mouth") return 3.0f;
        return 0.0f;
    }

    void ProcessGenTgSlot(
        string label,
        JSONStorableStringChooser atomChooser,
        JSONStorableStringChooser modeChooser,
        GenTgRuntime runtime,
        bool stateDesired,
        bool eventTriggered
    )
    {
        if (runtime == null)
        {
            return;
        }

        string mode = modeChooser != null && !string.IsNullOrEmpty(modeChooser.val) ? modeChooser.val : GenTgModeOff;

        if (runtime.timedOffAt >= 0.0f && Time.time >= runtime.timedOffAt)
        {
            SetGenTgSlotValue(label, atomChooser, runtime, false, "timer-off");
            runtime.timedOffAt = -1.0f;
        }

        if (mode == GenTgModeOff)
        {
            runtime.timedOffAt = -1.0f;
            runtime.stateOffAt = -1.0f;
            SetGenTgSlotValue(label, atomChooser, runtime, false, "mode-off");
            return;
        }

        if (mode == GenTgModeState)
        {
            runtime.timedOffAt = -1.0f;

            if (stateDesired)
            {
                runtime.stateOffAt = -1.0f;
                SetGenTgSlotValue(label, atomChooser, runtime, true, "state-on");
                return;
            }

            if (!runtime.lastWrittenKnown || !runtime.lastWrittenValue)
            {
                runtime.stateOffAt = -1.0f;
                return;
            }

            if (runtime.stateOffAt < 0.0f)
            {
                runtime.stateOffAt = Time.time + GenTgStateOffDelaySeconds;
                return;
            }

            if (Time.time >= runtime.stateOffAt)
            {
                runtime.stateOffAt = -1.0f;
                SetGenTgSlotValue(label, atomChooser, runtime, false, "state-off-delay");
            }
            return;
        }

        runtime.stateOffAt = -1.0f;
        if (!eventTriggered)
        {
            return;
        }

        if (runtime.timedOffAt >= 0.0f)
        {
            return;
        }

        if (Time.time - runtime.lastFireTime < GenTgEventCooldownSeconds)
        {
            DebugLog("[TargetLinePerson] Gen TG skipped by cooldown / slot=" + label + " / mode=" + mode);
            return;
        }

        float duration = GetGenTgTimedDuration(mode);
        if (duration <= 0.0f)
        {
            return;
        }

        runtime.lastFireTime = Time.time;
        runtime.timedOffAt = Time.time + duration;
        SetGenTgSlotValue(label, atomChooser, runtime, true, "event-on/" + mode);
    }

    void ProcessGenHeadActions(
        bool inserted,
        bool startEvent,
        bool insideEvent,
        bool deepEvent,
        bool endEvent
    )
    {
        ProcessGenHeadSlot("Start", genHeadStartAction, startEvent, ref genHeadStartLastFireTime, GenHeadEventCooldownSeconds);
        ProcessGenHeadInside(inserted, insideEvent);
        ProcessGenHeadSlot("Deep", genHeadDeepAction, deepEvent, ref genHeadDeepLastFireTime, GenHeadEventCooldownSeconds);
        ProcessGenHeadSlot("End", genHeadEndAction, endEvent, ref genHeadEndLastFireTime, GenHeadEventCooldownSeconds);
    }

    void ProcessGenHeadInside(bool inserted, bool insideEvent)
    {
        if (!inserted)
        {
            genHeadInsideNextRandomTime = -1.0f;
            return;
        }

        string actionName = genHeadInsideAction != null && !string.IsNullOrEmpty(genHeadInsideAction.val) ? genHeadInsideAction.val : GenHeadActionOff;
        if (actionName == GenHeadActionRandom)
        {
            ProcessGenHeadInsideRandom(insideEvent);
            return;
        }

        genHeadInsideNextRandomTime = -1.0f;
        float cooldown = genHeadInsideCooldown != null ? Mathf.Max(0.50f, genHeadInsideCooldown.val) : GenHeadInsideCooldownDefault;
        bool shouldFire = insideEvent || Time.time - genHeadInsideLastFireTime >= cooldown;
        ProcessGenHeadSlot("Inside", genHeadInsideAction, shouldFire, ref genHeadInsideLastFireTime, cooldown);
    }

    void ProcessGenHeadInsideRandom(bool insideEvent)
    {
        if (insideEvent || genHeadInsideNextRandomTime < 0.0f)
        {
            ScheduleNextGenHeadInsideRandom();
            return;
        }

        if (Time.time < genHeadInsideNextRandomTime)
        {
            return;
        }

        genHeadInsideLastFireTime = Time.time;
        bool ok = TryFireGenHeadAction(GenHeadActionRandom);
        DebugLog(
            "[TargetLinePerson] Gen Head random" +
            " / action=" + GenHeadActionRandom +
            " / ok=" + ok
        );
        ScheduleNextGenHeadInsideRandom();
    }

    void ScheduleNextGenHeadInsideRandom()
    {
        genHeadInsideNextRandomTime = Time.time + UnityEngine.Random.Range(GenHeadRandomIntervalMin, GenHeadRandomIntervalMax);
    }

    void ProcessGenHeadSlot(
        string label,
        JSONStorableStringChooser actionChooser,
        bool eventTriggered,
        ref float lastFireTime,
        float cooldown
    )
    {
        if (!eventTriggered)
        {
            return;
        }

        string actionName = actionChooser != null && !string.IsNullOrEmpty(actionChooser.val) ? actionChooser.val : GenHeadActionOff;
        if (actionName == GenHeadActionOff)
        {
            return;
        }

        Atom atom = GetGenHeadTargetAtom();
        bool isHba = IsHbaActionName(actionName);
        if (isHba && IsDebugLogEnabled())
        {
            LogMessageIfDebug(
                "[TargetLinePerson] HBA slot trigger" +
                " / slot=" + label +
                " / action=" + actionName +
                " / targetAtom=" + (atom != null ? atom.uid : "") +
                " / actionFound=" + FindGenHeadActionLocation(atom, actionName)
            );
        }

        if (Time.time - lastFireTime < cooldown)
        {
            if (isHba && IsDebugLogEnabled())
            {
                LogMessageIfDebug(
                    "[TargetLinePerson] HBA skipped by cooldown" +
                    " / slot=" + label +
                    " / action=" + actionName +
                    " / remain=" + Mathf.Max(0.0f, cooldown - (Time.time - lastFireTime)).ToString("F2")
                );
            }
            return;
        }

        lastFireTime = Time.time;
        bool ok = TryFireGenHeadAction(actionName);
        TraceHeadAction(
            "[TargetLinePerson] Gen Head action" +
            " / slot=" + label +
            " / action=" + actionName +
            " / ok=" + ok,
            actionName
        );
    }

    bool TryFireGenHeadAction(string actionName)
    {
        Atom atom = GetGenHeadTargetAtom();
        if (atom == null || string.IsNullOrEmpty(actionName) || actionName == GenHeadActionOff)
        {
            TraceHeadAction(
                "[TargetLinePerson] Gen Head fire skipped" +
                " / action=" + actionName +
                " / atom=" + (atom != null ? atom.uid : "") +
                " / reason=atom-or-action-empty",
                actionName
            );
            return false;
        }

        if (IsDebugLogEnabled())
        {
            TraceHeadAction(
                "[TargetLinePerson] Gen Head fire try" +
                " / action=" + actionName +
                " / atom=" + atom.uid +
                " / pluginFound=" + HasGenHeadActionPlugin(atom) +
                " / actionFound=" + FindGenHeadActionLocation(atom, actionName),
                actionName
            );
        }

        if (TryExecuteGenHeadAction(atom, actionName, true))
        {
            return true;
        }

        return TryExecuteGenHeadAction(atom, actionName, false);
    }

    bool TryExecuteGenHeadAction(Atom atom, string actionName, bool preferredOnly)
    {
        JSONStorableAction action;
        string storableId;
        if (!TryGetCachedGenHeadAction(atom, actionName, preferredOnly, out action, out storableId))
        {
            if (IsDebugLogEnabled())
            {
                TraceHeadAction(
                    "[TargetLinePerson] Gen Head action not found in pass" +
                    " / action=" + actionName +
                    " / atom=" + (atom != null ? atom.uid : "") +
                    " / preferredOnly=" + preferredOnly,
                    actionName
                );
            }
            return false;
        }

        if (IsDebugLogEnabled())
        {
            TraceHeadAction(
                "[TargetLinePerson] Gen Head execute" +
                " / action=" + actionName +
                " / atom=" + atom.uid +
                " / storable=" + storableId +
                " / preferredOnly=" + preferredOnly,
                actionName
            );
        }
        action.actionCallback.Invoke();
        return true;
    }

    float GetGenTgTimedDuration(string mode)
    {
        if (mode == GenTgModeButtonPulse) return GenTgButtonPulseSeconds;
        if (mode == GenTgModeTimer1) return GenTgTimer1Seconds;
        if (mode == GenTgModeTimer5) return GenTgTimer5Seconds;
        return 0.0f;
    }

    void ForceAllGenTgOff()
    {
        ForceGenTgSlotOff("Start", genTgStartAtom, genTgStartRuntime);
        ForceGenTgSlotOff("Inside", genTgInsideAtom, genTgInsideRuntime);
        ForceGenTgSlotOff("Deep", genTgDeepAtom, genTgDeepRuntime);
        ForceGenTgSlotOff("End", genTgEndAtom, genTgEndRuntime);
    }

    void ForceGenTgSlotOff(string label, JSONStorableStringChooser atomChooser, GenTgRuntime runtime)
    {
        if (runtime == null)
        {
            return;
        }

        runtime.timedOffAt = -1.0f;
        runtime.stateOffAt = -1.0f;
        SetGenTgSlotValue(label, atomChooser, runtime, false, "force-off");
    }

    void SetGenTgSlotValue(string label, JSONStorableStringChooser atomChooser, GenTgRuntime runtime, bool value, string reason)
    {
        string atomUid = atomChooser != null ? atomChooser.val : "";
        if (string.IsNullOrEmpty(atomUid))
        {
            return;
        }

        if (
            runtime != null &&
            runtime.lastWrittenKnown &&
            runtime.lastWrittenValue == value &&
            runtime.lastAtomUid == atomUid
        )
        {
            return;
        }

        Atom atom = SuperController.singleton.GetAtomByUid(atomUid);
        if (atom == null)
        {
            DebugLog("[TargetLinePerson] Gen TG atom missing / slot=" + label + " / atom=" + atomUid);
            return;
        }

        bool unityOk = SetGenTgUnityToggle(atom, value);
        string boolInfo;
        bool boolOk = SetGenTgBoolParam(atom, value, out boolInfo);

        if (runtime != null)
        {
            runtime.lastWrittenKnown = true;
            runtime.lastWrittenValue = value;
            runtime.lastAtomUid = atomUid;
        }

        DebugLog(
            "[TargetLinePerson] Gen TG set" +
            " / slot=" + label +
            " / atom=" + atomUid +
            " / value=" + value +
            " / unityToggle=" + unityOk +
            " / boolParam=" + boolOk +
            boolInfo +
            " / reason=" + reason
        );
    }

    bool SetGenTgUnityToggle(Atom atom, bool value)
    {
        if (atom == null)
        {
            return false;
        }

        Toggle toggle = atom.GetComponentInChildren<Toggle>(true);
        if (toggle == null)
        {
            return false;
        }

        toggle.isOn = value;
        return true;
    }

    bool SetGenTgBoolParam(Atom atom, bool value, out string info)
    {
        info = "";
        if (atom == null)
        {
            return false;
        }

        if (TrySetGenTgBoolParam(atom, "Trigger", "value", value, out info))
        {
            return true;
        }

        for (int i = 0; i < genTgFallbackStorableIds.Length; i++)
        {
            for (int j = 0; j < genTgFallbackBoolNames.Length; j++)
            {
                if (TrySetGenTgBoolParam(atom, genTgFallbackStorableIds[i], genTgFallbackBoolNames[j], value, out info))
                {
                    return true;
                }
            }
        }

        foreach (string sid in atom.GetStorableIDs())
        {
            for (int j = 0; j < genTgFallbackBoolNames.Length; j++)
            {
                if (TrySetGenTgBoolParam(atom, sid, genTgFallbackBoolNames[j], value, out info))
                {
                    return true;
                }
            }
        }

        info = " / bool not found";
        return false;
    }

    bool TrySetGenTgBoolParam(Atom atom, string sid, string param, bool value, out string info)
    {
        info = "";
        if (atom == null || string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(param))
        {
            return false;
        }

        JSONStorable storable = atom.GetStorableByID(sid);
        if (storable == null)
        {
            return false;
        }

        JSONStorableBool boolParam = storable.GetBoolJSONParam(param);
        if (boolParam == null)
        {
            return false;
        }

        boolParam.val = value;
        info = " / storable=" + sid + " / param=" + param;
        return true;
    }

    void LogGenTgStatus()
    {
        LogMessageIfDebug(
            "[TargetLinePerson] Gen TG status" +
            " / enabled=" + (genTgTriggers != null && genTgTriggers.val) +
            " / prefix=" + GetGenTgPrefix() +
            " / start=" + genTgStartActive +
            " / inside=" + genTgInsideActive +
            " / deep=" + genTgDeepActive
        );
        LogGenTgSlotStatus("Start", genTgStartAtom, genTgStartMode);
        LogGenTgSlotStatus("Inside", genTgInsideAtom, genTgInsideMode);
        LogGenTgSlotStatus("Deep", genTgDeepAtom, genTgDeepMode);
        LogGenTgSlotStatus("End", genTgEndAtom, genTgEndMode);
    }

    void LogGenTgSlotStatus(string label, JSONStorableStringChooser atomChooser, JSONStorableStringChooser modeChooser)
    {
        string atomUid = atomChooser != null ? atomChooser.val : "";
        string mode = modeChooser != null ? modeChooser.val : "";
        Atom atom = !string.IsNullOrEmpty(atomUid) ? SuperController.singleton.GetAtomByUid(atomUid) : null;
        bool hasUnityToggle = atom != null && atom.GetComponentInChildren<Toggle>(true) != null;
        bool hasTriggerValue = false;
        if (atom != null)
        {
            JSONStorable storable = atom.GetStorableByID("Trigger");
            hasTriggerValue = storable != null && storable.GetBoolJSONParam("value") != null;
        }

        LogMessageIfDebug(
            "[TargetLinePerson] Gen TG slot" +
            " / slot=" + label +
            " / atom=" + atomUid +
            " / found=" + (atom != null) +
            " / mode=" + mode +
            " / unityToggle=" + hasUnityToggle +
            " / triggerValue=" + hasTriggerValue
        );
    }

    void LogGenHeadStatus()
    {
        Atom atom = GetGenHeadTargetAtom();
        string startAction = GetGenHeadActionValue(genHeadStartAction);
        string insideAction = GetGenHeadActionValue(genHeadInsideAction);
        string deepAction = GetGenHeadActionValue(genHeadDeepAction);
        string endAction = GetGenHeadActionValue(genHeadEndAction);

        LogMessageIfDebug(
            "[TargetLinePerson] Gen Head status" +
            " / enabled=" + (genHeadActions != null && genHeadActions.val) +
            " / headPluginAtomChooser=" + (genHeadAtom != null ? genHeadAtom.val : "") +
            " / resolvedMode=TargetPersonFixed" +
            " / resolvedAtom=" + (atom != null ? atom.uid : "") +
            " / pluginFound=" + HasGenHeadActionPlugin(atom) +
            " / start=" + startAction + "@" + FindGenHeadActionLocation(atom, startAction) +
            " / inside=" + insideAction + "@" + FindGenHeadActionLocation(atom, insideAction) +
            " / deep=" + deepAction + "@" + FindGenHeadActionLocation(atom, deepAction) +
            " / end=" + endAction + "@" + FindGenHeadActionLocation(atom, endAction) +
            " / HBA_Twitch_Normal=" + FindGenHeadActionLocation(atom, "HBA_Twitch_Normal") +
            " / HBA_Twitch_Strong=" + FindGenHeadActionLocation(atom, "HBA_Twitch_Strong")
        );
    }

    void TestGenHeadAction(string actionName)
    {
        bool ok = TryFireGenHeadAction(actionName);
        LogMessageIfDebug(
            "[TargetLinePerson] Gen Head TEST" +
            " / action=" + actionName +
            " / ok=" + ok
        );
    }

    string FindGenHeadActionLocation(Atom atom, string actionName)
    {
        JSONStorableAction action;
        string storableId;
        if (TryGetCachedGenHeadAction(atom, actionName, false, out action, out storableId))
        {
            return storableId;
        }
        return atom == null ? "no-atom" : (string.IsNullOrEmpty(actionName) || actionName == GenHeadActionOff ? "off" : "not-found");
    }

    void ResetGenHeadActionCacheIfNeeded(Atom atom)
    {
        string uid = atom != null ? atom.uid : "";
        if (genHeadActionCacheAtomUid == uid)
        {
            return;
        }

        genHeadActionCacheAtomUid = uid;
        genHeadActionLocationCache.Clear();
    }

    bool TryGetCachedGenHeadAction(Atom atom, string actionName, bool preferredOnly, out JSONStorableAction action, out string storableId)
    {
        action = null;
        storableId = "";
        if (atom == null || string.IsNullOrEmpty(actionName) || actionName == GenHeadActionOff)
        {
            return false;
        }

        ResetGenHeadActionCacheIfNeeded(atom);
        string key = (preferredOnly ? "P|" : "A|") + actionName;
        if (genHeadActionLocationCache.TryGetValue(key, out storableId))
        {
            if (storableId == "not-found")
            {
                return false;
            }

            JSONStorable cachedStorable = atom.GetStorableByID(storableId);
            action = cachedStorable != null ? cachedStorable.GetAction(actionName) : null;
            if (action != null)
            {
                return true;
            }

            genHeadActionLocationCache.Remove(key);
        }

        foreach (string sid in atom.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(sid))
            {
                continue;
            }

            bool preferred = sid.IndexOf(GenHeadPreferredPlugin, StringComparison.OrdinalIgnoreCase) >= 0;
            if (preferredOnly && !preferred)
            {
                continue;
            }

            JSONStorable storable = atom.GetStorableByID(sid);
            if (storable == null)
            {
                continue;
            }

            action = storable.GetAction(actionName);
            if (action != null)
            {
                storableId = sid;
                genHeadActionLocationCache[key] = sid;
                return true;
            }
        }

        storableId = "not-found";
        genHeadActionLocationCache[key] = storableId;
        action = null;
        return false;
    }

    string GetGenHeadActionValue(JSONStorableStringChooser chooser)
    {
        return chooser != null && !string.IsNullOrEmpty(chooser.val) ? chooser.val : GenHeadActionOff;
    }

    string BuildInsertDebugText()
    {
        float depth;
        float length;
        float percent;
        if (!TryGetLiveGenDepth(out depth, out length, out percent))
        {
            return "--%";
        }

        return BuildDepthPercentText(depth, length, percent);
    }

    void ReadDepthFrameCache(DepthFrameCache cache, out float depth, out float length, out float percent)
    {
        depth = cache.depth;
        length = cache.length;
        percent = cache.percent;
    }

    void WriteDepthFrameCache(DepthFrameCache cache, bool ok, float depth, float length, float percent)
    {
        cache.frame = Time.frameCount;
        cache.known = true;
        cache.ok = ok;
        cache.depth = depth;
        cache.length = length;
        cache.percent = percent;
    }

    bool IsDepthFrameCacheFresh(DepthFrameCache cache)
    {
        return cache.known && cache.frame == Time.frameCount;
    }

    bool TryGetLiveGenDepth(out float depth, out float length, out float percent)
    {
        if (IsDepthFrameCacheFresh(genDepthFrameCache))
        {
            ReadDepthFrameCache(genDepthFrameCache, out depth, out length, out percent);
            return genDepthFrameCache.ok;
        }

        bool ok = ComputeLiveGenDepth(out depth, out length, out percent);
        WriteDepthFrameCache(genDepthFrameCache, ok, depth, length, percent);
        return ok;
    }

    bool TryGetLiveGenDepthForHud(out float depth, out float length, out float percent)
    {
        if (IsDepthFrameCacheFresh(genHudDepthFrameCache))
        {
            ReadDepthFrameCache(genHudDepthFrameCache, out depth, out length, out percent);
            return genHudDepthFrameCache.ok;
        }

        bool ok = ComputeLiveGenDepthForHud(out depth, out length, out percent);
        WriteDepthFrameCache(genHudDepthFrameCache, ok, depth, length, percent);
        return ok;
    }

    bool TryGetLiveAnusDepthForHud(out float depth, out float length, out float percent)
    {
        if (IsDepthFrameCacheFresh(anusHudDepthFrameCache))
        {
            ReadDepthFrameCache(anusHudDepthFrameCache, out depth, out length, out percent);
            return anusHudDepthFrameCache.ok;
        }

        bool ok = ComputeLiveAnusDepthForHud(out depth, out length, out percent);
        WriteDepthFrameCache(anusHudDepthFrameCache, ok, depth, length, percent);
        return ok;
    }

    bool TryGetLiveAnusDepthForPush(out float depth, out float length, out float percent)
    {
        if (IsDepthFrameCacheFresh(anusPushDepthFrameCache))
        {
            ReadDepthFrameCache(anusPushDepthFrameCache, out depth, out length, out percent);
            return anusPushDepthFrameCache.ok;
        }

        bool ok = ComputeLiveAnusDepthForPush(out depth, out length, out percent);
        WriteDepthFrameCache(anusPushDepthFrameCache, ok, depth, length, percent);
        return ok;
    }

    bool TryGetLiveMouthDepthForPush(out float depth, out float length, out float percent)
    {
        if (IsDepthFrameCacheFresh(mouthPushDepthFrameCache))
        {
            ReadDepthFrameCache(mouthPushDepthFrameCache, out depth, out length, out percent);
            return mouthPushDepthFrameCache.ok;
        }

        bool ok = ComputeLiveMouthDepthForPush(out depth, out length, out percent);
        WriteDepthFrameCache(mouthPushDepthFrameCache, ok, depth, length, percent);
        return ok;
    }

    bool ComputeLiveGenDepth(out float depth, out float length, out float percent)
    {
        depth = 0.0f;
        length = GetGenDepthMax();
        percent = 0.0f;

        double sectionStart = PerfNow();
        FreeControllerV3 tip = GetOwnPenisTip();
        if (tip == null || tip.transform == null)
        {
            lastPerfMainLineMs += PerfMs(sectionStart);
            return false;
        }

        Vector3 tipPos = tip.transform.position;
        Vector3 purpleOrigin;
        Vector3 purpleDir;
        float purpleLength;
        if (!TryGetLiveGenitalInsideLine(out purpleOrigin, out purpleDir, out purpleLength))
        {
            lastPerfMainLineMs += PerfMs(sectionStart);
            return false;
        }

        if (purpleDir.sqrMagnitude < 0.0001f)
        {
            lastPerfMainLineMs += PerfMs(sectionStart);
            return false;
        }
        lastPerfMainLineMs += PerfMs(sectionStart);

        sectionStart = PerfNow();
        length = Mathf.Max(0.0001f, purpleLength);
        Vector3 purpleDirNorm = purpleDir.normalized;
        float gDepthAngle = Mathf.Abs(Mathf.Asin(Mathf.Clamp(purpleDirNorm.y, -1.0f, 1.0f)) * Mathf.Rad2Deg);
        Vector3 tipFromOrigin = tipPos - purpleOrigin;
        float rawDepth = Vector3.Dot(tipFromOrigin, purpleDirNorm);
        Vector3 closestOnAxis = purpleOrigin + purpleDirNorm * rawDepth;
        float lateralDistance = Vector3.Distance(tipPos, closestOnAxis);
        lastPerfMainCalcMs += PerfMs(sectionStart);

        if (gDepthAngle > GenDepthAngleGateLimitDegrees)
        {
            depth = 0.0f;
            percent = 0.0f;
            sectionStart = PerfNow();
            if (IsPerfMainBookkeepingEnabled())
            {
                RememberGenDepthSample(rawDepth, lateralDistance, -1.0f, percent);
                LogGenDepthProbe(rawDepth, lateralDistance, -1.0f, length, percent, false, false, true, gDepthAngle);
                ApplyGenBodyGatePReleaseIfNeeded(false);
            }
            lastPerfMainBookMs += PerfMs(sectionStart);
            return true;
        }

        if (lateralDistance > GenDepthMaxLateralDistance)
        {
            depth = 0.0f;
            percent = 0.0f;
            sectionStart = PerfNow();
            if (IsPerfMainBookkeepingEnabled())
            {
                RememberGenDepthSample(rawDepth, lateralDistance, -1.0f, percent);
                LogGenDepthProbe(rawDepth, lateralDistance, -1.0f, length, percent, true, false, false, gDepthAngle);
                ApplyGenBodyGatePReleaseIfNeeded(false);
            }
            lastPerfMainBookMs += PerfMs(sectionStart);
            return true;
        }

        float bodyDistance = -1.0f;
        bool bodyGated = false;
        sectionStart = PerfNow();
        if (IsPerfMainBodyGateEnabled())
        {
            bodyGated = IsGenBodyGated(purpleOrigin, out bodyDistance);
        }
        lastPerfMainGateMs += PerfMs(sectionStart);

        if (bodyGated)
        {
            depth = 0.0f;
            percent = 0.0f;
            sectionStart = PerfNow();
            if (IsPerfMainBookkeepingEnabled())
            {
                RememberGenDepthSample(rawDepth, lateralDistance, bodyDistance, percent);
                LogGenDepthProbe(rawDepth, lateralDistance, bodyDistance, length, percent, false, true, false, gDepthAngle);
                ApplyGenBodyGatePReleaseIfNeeded(true);
            }
            lastPerfMainBookMs += PerfMs(sectionStart);
            return true;
        }

        sectionStart = PerfNow();
        percent = Mathf.Clamp(rawDepth / length, 0.0f, GenDepthHudDisplayMaxPercent);
        depth = Mathf.Clamp(rawDepth, 0.0f, length * GenDepthHudDisplayMaxPercent);
        lastPerfMainCalcMs += PerfMs(sectionStart);

        sectionStart = PerfNow();
        if (IsPerfMainBookkeepingEnabled())
        {
            RememberGenDepthSample(rawDepth, lateralDistance, bodyDistance, percent);
            LogGenDepthProbe(rawDepth, lateralDistance, bodyDistance, length, percent, false, false, false, gDepthAngle);
            ApplyGenBodyGatePReleaseIfNeeded(false);
        }
        lastPerfMainBookMs += PerfMs(sectionStart);

        return true;
    }

    bool ComputeLiveGenDepthForHud(out float depth, out float length, out float percent)
    {
        depth = 0.0f;
        length = GetGenDepthMax();
        percent = 0.0f;

        FreeControllerV3 tip = GetOwnPenisTip();
        if (tip == null || tip.transform == null)
        {
            return false;
        }

        Vector3 tipPos = tip.transform.position;
        Vector3 purpleOrigin;
        Vector3 purpleDir;
        float purpleLength;
        if (!TryGetLiveGenitalInsideLine(out purpleOrigin, out purpleDir, out purpleLength))
        {
            return false;
        }

        if (purpleDir.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        length = Mathf.Max(0.0001f, purpleLength);
        Vector3 purpleDirNorm = purpleDir.normalized;
        Vector3 tipFromOrigin = tipPos - purpleOrigin;
        float rawDepth = Vector3.Dot(tipFromOrigin, purpleDirNorm);
        Vector3 closestOnAxis = purpleOrigin + purpleDirNorm * rawDepth;
        float lateralDistance = Vector3.Distance(tipPos, closestOnAxis);

        if (lateralDistance > GenDepthMaxLateralDistance)
        {
            depth = 0.0f;
            percent = 0.0f;
            return true;
        }

        float bodyDistance;
        if (IsGenBodyGated(purpleOrigin, out bodyDistance))
        {
            depth = 0.0f;
            percent = 0.0f;
            return true;
        }

        percent = Mathf.Clamp(rawDepth / length, 0.0f, GenDepthHudDisplayMaxPercent);
        depth = Mathf.Clamp(rawDepth, 0.0f, length * GenDepthHudDisplayMaxPercent);
        return true;
    }

    bool ComputeLiveAnusDepthForHud(out float depth, out float length, out float percent)
    {
        depth = 0.0f;
        length = GetGenDepthMax();
        percent = 0.0f;

        FreeControllerV3 tip = GetOwnPenisTip();
        if (tip == null || tip.transform == null)
        {
            return false;
        }

        Vector3 origin;
        Vector3 dir;
        float insideLength;
        if (!TryGetLiveAnusInsideLine(out origin, out dir, out insideLength))
        {
            return false;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        length = Mathf.Max(0.0001f, insideLength);
        Vector3 dirNorm = dir.normalized;
        Vector3 tipFromOrigin = tip.transform.position - origin;
        float rawDepth = Vector3.Dot(tipFromOrigin, dirNorm);
        Vector3 closestOnAxis = origin + dirNorm * rawDepth;
        float lateralDistance = Vector3.Distance(tip.transform.position, closestOnAxis);

        if (lateralDistance > GenDepthMaxLateralDistance)
        {
            depth = 0.0f;
            percent = 0.0f;
            return true;
        }

        float bodyDistance;
        if (IsGenBodyGated(origin, out bodyDistance))
        {
            depth = 0.0f;
            percent = 0.0f;
            return true;
        }

        percent = Mathf.Clamp(rawDepth / length, 0.0f, GenDepthHudDisplayMaxPercent);
        depth = Mathf.Clamp(rawDepth, 0.0f, length * GenDepthHudDisplayMaxPercent);
        return true;
    }

    bool TryGetLiveCurrentPushDepth(out float depth, out float length, out float percent, out string targetMode)
    {
        targetMode = GetTargetModeName();

        if (targetMode == "genital")
        {
            return TryGetLiveGenDepth(out depth, out length, out percent);
        }

        if (targetMode == "anus")
        {
            return TryGetLiveAnusDepthForPush(out depth, out length, out percent);
        }

        if (targetMode == "mouth")
        {
            return TryGetLiveMouthDepthForPush(out depth, out length, out percent);
        }

        depth = 0.0f;
        length = GetGenDepthMax();
        percent = 0.0f;
        return false;
    }

    bool ComputeLiveAnusDepthForPush(out float depth, out float length, out float percent)
    {
        depth = 0.0f;
        length = GetGenDepthMax();
        percent = 0.0f;

        FreeControllerV3 tip = GetOwnPenisTip();
        if (tip == null || tip.transform == null)
        {
            return false;
        }

        Vector3 origin;
        Vector3 dir;
        float insideLength;
        if (!TryGetLiveAnusInsideLine(out origin, out dir, out insideLength))
        {
            return false;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        length = Mathf.Max(0.0001f, insideLength);
        Vector3 dirNorm = dir.normalized;
        Vector3 tipFromOrigin = tip.transform.position - origin;
        float rawDepth = Vector3.Dot(tipFromOrigin, dirNorm);
        Vector3 closestOnAxis = origin + dirNorm * rawDepth;
        float lateralDistance = Vector3.Distance(tip.transform.position, closestOnAxis);
        float bodyDistance;

        if (lateralDistance > GenDepthMaxLateralDistance)
        {
            depth = 0.0f;
            percent = 0.0f;
            RememberPushDepthSample(rawDepth, lateralDistance, -1.0f, percent);
            return true;
        }

        if (IsGenBodyGated(origin, out bodyDistance))
        {
            depth = 0.0f;
            percent = 0.0f;
            RememberPushDepthSample(rawDepth, lateralDistance, bodyDistance, percent);
            ApplyGenBodyGatePReleaseIfNeeded(true);
            return true;
        }

        percent = Mathf.Clamp(rawDepth / length, 0.0f, GenDepthHudDisplayMaxPercent);
        depth = Mathf.Clamp(rawDepth, 0.0f, length * GenDepthHudDisplayMaxPercent);
        RememberPushDepthSample(rawDepth, lateralDistance, bodyDistance, percent);
        ApplyGenBodyGatePReleaseIfNeeded(false);
        return true;
    }

    bool ComputeLiveMouthDepthForPush(out float depth, out float length, out float percent)
    {
        depth = 0.0f;
        length = GetGenDepthMax();
        percent = 0.0f;

        FreeControllerV3 tip = GetOwnPenisTip();
        if (tip == null || tip.transform == null)
        {
            return false;
        }

        Vector3 origin;
        Vector3 dir;
        float insideLength;
        if (!TryGetLiveMouthInsideLine(out origin, out dir, out insideLength))
        {
            return false;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        length = Mathf.Max(0.0001f, insideLength);
        Vector3 dirNorm = dir.normalized;
        Vector3 tipFromOrigin = tip.transform.position - origin;
        float rawDepth = Vector3.Dot(tipFromOrigin, dirNorm);
        Vector3 closestOnAxis = origin + dirNorm * rawDepth;
        float lateralDistance = Vector3.Distance(tip.transform.position, closestOnAxis);

        if (lateralDistance > GenDepthMaxLateralDistance)
        {
            depth = 0.0f;
            percent = 0.0f;
            RememberPushDepthSample(rawDepth, lateralDistance, -1.0f, percent);
            return true;
        }

        // Mouth target is naturally far from the own hip/root, so the genital body gate would
        // suppress Auto PUSH even when P Tip is correctly near the mouth axis. Keep mouth push
        // governed by tip depth/lateral only.
        percent = Mathf.Clamp(rawDepth / length, 0.0f, GenDepthHudDisplayMaxPercent);
        depth = Mathf.Clamp(rawDepth, 0.0f, length * GenDepthHudDisplayMaxPercent);
        RememberPushDepthSample(rawDepth, lateralDistance, -1.0f, percent);
        return true;
    }

    void RememberPushDepthSample(float rawDepth, float lateralDistance, float bodyDistance, float percent)
    {
        lastGenDepthSampleKnown = true;
        lastGenDepthRawDepth = rawDepth;
        lastGenDepthPushLieCompensated = IsPushAutoLieDepthCompensationActive();
        lastGenDepthPushEffectiveDepth = lastGenDepthPushLieCompensated ? Mathf.Abs(rawDepth) : rawDepth;
        lastGenDepthLateral = lateralDistance;
        lastGenDepthBodyDistance = bodyDistance;
        lastGenDepthPercent = percent;
    }

    void RememberGenDepthSample(float rawDepth, float lateralDistance, float bodyDistance, float percent)
    {
        lastGenDepthSampleKnown = true;
        lastGenDepthRawDepth = rawDepth;
        lastGenDepthPushLieCompensated = IsPushAutoLieDepthCompensationActive();
        lastGenDepthPushEffectiveDepth = lastGenDepthPushLieCompensated ? Mathf.Abs(rawDepth) : rawDepth;
        lastGenDepthLateral = lateralDistance;
        lastGenDepthBodyDistance = bodyDistance;
        lastGenDepthPercent = percent;
        UpdatePTipOnGContactState(IsNearZeroGContact(rawDepth, lateralDistance), rawDepth, lateralDistance);
    }

    void UpdatePTipOnGContactState(bool onG, float rawDepth, float lateralDistance)
    {
        if (pTipOnGContactKnown && pTipOnGContact == onG)
        {
            return;
        }

        pTipOnGContactKnown = true;
        pTipOnGContact = onG;

        LogMessageIfDebug(
            "[TargetLinePerson] " + (onG ? "P Tip on G" : "P Tip off G") +
            " / rawDepth=" + rawDepth.ToString("F3") +
            " / lateral=" + lateralDistance.ToString("F3") +
            " / lateralMax=" + PushAutoGDepthEnterLateralMax.ToString("F3")
        );
    }

    bool ShouldFreezeDynamicYellowEnd(out string freezeReason)
    {
        freezeReason = "";

        if (!lastGenDepthSampleKnown)
        {
            return false;
        }

        if (lastGenDepthPercent > 0.001f || lastGenDepthRawDepth > 0.0f)
        {
            freezeReason = "depth";
            return true;
        }

        if (lastGenDepthRawDepth > -0.030f &&
            lastGenDepthBodyDistance >= 0.0f &&
            lastGenDepthBodyDistance < 0.150f &&
            lastGenDepthLateral < 0.050f)
        {
            freezeReason = "near-axis-depth-gate";
            return true;
        }

        return false;
    }

    bool IsGenBodyGated(Vector3 origin, out float bodyDistance)
    {
        bodyDistance = 0.0f;
        if (genBodyGate == null || !genBodyGate.val)
        {
            return false;
        }

        FreeControllerV3 ownHip = GetOwnHip();
        if (ownHip == null || ownHip.transform == null)
        {
            return false;
        }

        Vector3 delta = ownHip.transform.position - origin;
        delta.y = 0.0f;
        bodyDistance = delta.magnitude;
        return bodyDistance > GetGenBodyGateMaxDistance();
    }

    float GetGenBodyGateMaxDistance()
    {
        if (genBodyGateMaxDistance == null)
        {
            return GenBodyGateMaxDistanceDefault;
        }

        return Mathf.Clamp(genBodyGateMaxDistance.val, GenBodyGateMaxDistanceMin, GenBodyGateMaxDistanceMax);
    }

    void ApplyGenBodyGatePReleaseIfNeeded(bool bodyGated)
    {
        if (!bodyGated || pIkOffOnGenBodyGate == null || !pIkOffOnGenBodyGate.val)
        {
            return;
        }

        ResetPAngleAtYellowP3IfApplied("gen body gate");
        ReleasePYellowController(GetOwnPenisBase());
        ReleasePYellowController(GetOwnPenisMid());
        ReleasePYellowController(GetOwnPenisTip());
    }

    void LogGenDepthProbe(float rawDepth, float lateralDistance, float bodyDistance, float length, float percent, bool lateralGated, bool bodyGated)
    {
        LogGenDepthProbe(rawDepth, lateralDistance, bodyDistance, length, percent, lateralGated, bodyGated, false, 0.0f);
    }

    void LogGenDepthProbe(float rawDepth, float lateralDistance, float bodyDistance, float length, float percent, bool lateralGated, bool bodyGated, bool angleGated, float angle)
    {
        if (!IsDebugViewEnabled())
        {
            return;
        }

        int zone = GetGenDepthProbeZone(percent, lateralGated, bodyGated, angleGated);
        bool changed =
            !lastGenDepthProbeLogKnown ||
            lastGenDepthProbeLateralGated != lateralGated ||
            lastGenDepthProbeBodyGated != bodyGated ||
            lastGenDepthProbeAngleGated != angleGated ||
            lastGenDepthProbeZone != zone;
        bool heartbeat = Time.time - lastGenDepthProbeLogTime >= GenDepthProbeLogHeartbeatInterval;

        if (!changed && !heartbeat)
        {
            return;
        }

        if (Time.time - lastGenDepthProbeLogTime < GenDepthProbeLogMinInterval)
        {
            return;
        }

        lastGenDepthProbeLogTime = Time.time;
        lastGenDepthProbeLogKnown = true;
        lastGenDepthProbeLateralGated = lateralGated;
        lastGenDepthProbeBodyGated = bodyGated;
        lastGenDepthProbeAngleGated = angleGated;
        lastGenDepthProbeZone = zone;

        DebugLog(
            "[TargetLinePerson] GenDepth probe" +
            " / reason=" + (changed ? "state-change" : "heartbeat") +
            " / zone=" + GetGenDepthProbeZoneName(zone) +
            " / rawDepth=" + rawDepth.ToString("F3") +
            " / lateral=" + lateralDistance.ToString("F3") +
            " / lateralMax=" + GenDepthMaxLateralDistance.ToString("F3") +
            " / bodyDist=" + (bodyDistance >= 0.0f ? bodyDistance.ToString("F3") : "n/a") +
            " / bodyMax=" + GetGenBodyGateMaxDistance().ToString("F3") +
            " / length=" + length.ToString("F3") +
            " / percent=" + percent.ToString("F3") +
            " / angle=" + angle.ToString("F1") +
            " / angleLimit=" + GenDepthAngleGateLimitDegrees.ToString("F1") +
            " / lateralGated=" + lateralGated +
            " / bodyGated=" + bodyGated +
            " / angleGated=" + angleGated
        );
    }

    int GetGenDepthProbeZone(float percent, bool lateralGated, bool bodyGated)
    {
        return GetGenDepthProbeZone(percent, lateralGated, bodyGated, false);
    }

    int GetGenDepthProbeZone(float percent, bool lateralGated, bool bodyGated, bool angleGated)
    {
        if (angleGated)
        {
            return -3;
        }

        if (bodyGated)
        {
            return -2;
        }

        if (lateralGated)
        {
            return -1;
        }

        if (percent <= 0.001f)
        {
            return 0;
        }

        if (percent < 0.50f)
        {
            return 1;
        }

        if (percent < 1.00f)
        {
            return 2;
        }

        return 3;
    }

    string GetGenDepthProbeZoneName(int zone)
    {
        if (zone == -3) return "angle-gated";
        if (zone == -2) return "body-gated";
        if (zone < 0) return "gated";
        if (zone == 0) return "zero";
        if (zone == 1) return "under50";
        if (zone == 2) return "under100";
        return "over100";
    }

    string BuildDepthPercentText(float depth, float length, float percent)
    {
        return Mathf.RoundToInt(percent * 100.0f).ToString() + "%";
    }

    bool TryGetLiveGenitalInsideLine(out Vector3 origin, out Vector3 dir, out float length)
    {
        origin = Vector3.zero;
        dir = Vector3.zero;
        length = GetGenDepthMax();

        if (targetControllerChooser == null || targetControllerChooser.val != "genital")
        {
            return false;
        }

        if (targetPersonChooser == null)
        {
            return false;
        }

        Atom targetAtom = FindAtom(targetPersonChooser.val);
        if (targetAtom == null)
        {
            return false;
        }

        Transform genitalLine = FindChildTransform(targetAtom, "LabiaTrigger");
        if (genitalLine == null)
        {
            return false;
        }

        origin = genitalLine.position;
        dir = genitalLine.up;

        if (dir.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        dir.Normalize();
        return true;
    }

    bool TryGetLiveCurrentInsideLine(out Vector3 origin, out Vector3 dir, out float length, out string targetMode)
    {
        origin = Vector3.zero;
        dir = Vector3.zero;
        length = GetGenDepthMax();
        targetMode = GetTargetModeName();

        if (targetMode == "genital")
        {
            return TryGetLiveGenitalInsideLine(out origin, out dir, out length);
        }

        if (targetMode == "anus")
        {
            return TryGetLiveAnusInsideLine(out origin, out dir, out length);
        }

        if (targetMode == "mouth")
        {
            return TryGetLiveMouthInsideLine(out origin, out dir, out length);
        }

        return false;
    }

    bool TryHasLiveMouthInsideLine()
    {
        Vector3 origin;
        Vector3 dir;
        float length;
        return TryGetLiveMouthInsideLine(out origin, out dir, out length);
    }

    bool TryHasLiveAnusInsideLine()
    {
        Vector3 origin;
        Vector3 dir;
        float length;
        return TryGetLiveAnusInsideLine(out origin, out dir, out length);
    }

    bool TryGetLiveAnusInsideLine(out Vector3 origin, out Vector3 dir, out float length)
    {
        origin = Vector3.zero;
        dir = Vector3.zero;
        length = GetGenDepthMax();

        if (targetControllerChooser == null || targetControllerChooser.val != "anus")
        {
            return false;
        }

        if (targetPersonChooser == null)
        {
            return false;
        }

        Atom targetAtom = FindAtom(targetPersonChooser.val);
        if (targetAtom == null)
        {
            return false;
        }

        Transform anusLine = FindAnusTargetTransform(targetAtom);
        if (anusLine == null)
        {
            return false;
        }

        origin = anusLine.position;
        dir = GetAnusInsideDirectionForDepth(targetAtom, anusLine);

        if (dir.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        dir.Normalize();
        return true;
    }

    bool TryGetLiveMouthInsideLine(out Vector3 origin, out Vector3 dir, out float length)
    {
        origin = Vector3.zero;
        dir = Vector3.zero;
        length = GetGenDepthMax();

        if (targetControllerChooser == null || targetControllerChooser.val != "mouth")
        {
            return false;
        }

        if (targetPersonChooser == null)
        {
            return false;
        }

        Atom targetAtom = FindAtom(targetPersonChooser.val);
        if (targetAtom == null)
        {
            return false;
        }

        Transform mouthLine = FindMouthTargetTransform(targetAtom);
        if (mouthLine == null)
        {
            return false;
        }

        origin = mouthLine.position;
        dir = GetMouthInsideDirectionForDepth(targetAtom, mouthLine);

        if (dir.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        dir.Normalize();
        return true;
    }

    Vector3 GetAnusInsideDirectionForDepth(Atom targetAtom, Transform anusLine)
    {
        // v165: Anus depth/P-follow direction must be the exact opposite of the red approach line.
        // The v164 approach-side auto flip could invert the depth axis when the body/P controls moved
        // across the hole. That made the fallback Yellow Guide point P away from the anus.
        Vector3 redLineDir = Vector3.zero;
        if (hasDynamicRedLineDisplay && dynamicRedLineDir.sqrMagnitude >= 0.0001f)
        {
            redLineDir = dynamicRedLineDir;
        }
        else if (capturedLineDir.sqrMagnitude >= 0.0001f)
        {
            redLineDir = capturedLineDir;
        }

        if (redLineDir.sqrMagnitude >= 0.0001f)
        {
            return -redLineDir.normalized;
        }

        Vector3 dir = GetAnusInsideDirection(targetAtom, anusLine);
        if (dir.sqrMagnitude < 0.0001f)
        {
            return dir;
        }

        return dir.normalized;
    }

    bool TryGetOwnAnusDepthReferencePosition(out Vector3 position)
    {
        if (containingAtom != null && containingAtom.mainController != null && containingAtom.mainController.transform != null)
        {
            position = containingAtom.mainController.transform.position;
            return true;
        }

        FreeControllerV3 hipCtrl = GetOwnHip();
        if (hipCtrl != null && hipCtrl.transform != null)
        {
            position = hipCtrl.transform.position;
            return true;
        }

        FreeControllerV3 baseCtrl = GetOwnPenisBase();
        if (baseCtrl != null && baseCtrl.transform != null)
        {
            position = baseCtrl.transform.position;
            return true;
        }

        FreeControllerV3 tipCtrl = GetOwnPenisTip();
        if (tipCtrl != null && tipCtrl.transform != null)
        {
            position = tipCtrl.transform.position;
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    bool IsGenDepthHudFxEnabled()
    {
        return genDepthHudFx != null && genDepthHudFx.val;
    }

    float GetGenDepthMax()
    {
        if (genDepthMax == null)
        {
            return GenDepthMaxDefault;
        }

        return Mathf.Clamp(genDepthMax.val, GenDepthMaxMin, GenDepthMaxMax);
    }

    void UpdateGenDepthHud(float depth, float length, float percent, bool visible)
    {
        if (!visible)
        {
            SetGenDepthHudActive(false);
            genDepthPeakPercent = 0.0f;
            genDepthPeakUntil = 0.0f;
            genDepthHudLowerVisualOpenT = 0.0f;
            genDepthHudLowerVisualOpenVelocity = 0.0f;
            genDepthInsertedMaxPercent = 0.0f;
            previousGenDepthPercent = 0.0f;
            ClearGenDepthBurstParticles();
            return;
        }

        Camera cam = Camera.main;
        if (cam == null || genDepthHudBackObj == null || genDepthHudFillObj == null || genDepthHudMarkerObj == null || genDepthHudBottomMarkerObj == null || genDepthHudPeakObj == null)
        {
            SetGenDepthHudActive(false);
            genDepthPeakPercent = 0.0f;
            genDepthPeakUntil = 0.0f;
            genDepthHudLowerVisualOpenT = 0.0f;
            genDepthHudLowerVisualOpenVelocity = 0.0f;
            genDepthInsertedMaxPercent = 0.0f;
            previousGenDepthPercent = 0.0f;
            ClearGenDepthBurstParticles();
            return;
        }

        Vector3 bottom = cam.ViewportToWorldPoint(new Vector3(GenDepthHudX, GenDepthHudBottomY, GenDepthHudCameraDistance));
        Vector3 top = cam.ViewportToWorldPoint(new Vector3(GenDepthHudX, GenDepthHudTopY, GenDepthHudCameraDistance));
        float fullHeight = Vector3.Distance(bottom, top);
        Vector3 upDir = (top - bottom).normalized;
        float fillRatio = Mathf.Clamp(percent / GenDepthHudDisplayMaxPercent, 0.0f, 1.0f);
        float markerRatio = Mathf.Clamp(GenDepthHudUpperHeartPercent / GenDepthHudDisplayMaxPercent, 0.0f, 1.0f);
        bool inserted = percent > 0.001f;
        float fillHeight = inserted ? Mathf.Max(0.001f, fullHeight * fillRatio) : 0.0f;
        Vector3 fillTop = bottom + upDir * fillHeight;
        Vector3 markerCenter = bottom + upDir * (fullHeight * markerRatio);
        Vector3 lowerMarkerCenter =
            bottom
            - upDir * (fullHeight * GenDepthHudLowerHeartOffset)
            - cam.transform.forward * GenDepthHudDropFrontOffset
            - cam.transform.right * GenDepthHudDropLeftOffset;
        float dropMaxWidth = GetGenHudDropMaxWidth();
        float manualWidthT = Mathf.InverseLerp(GenDepthHudDropOpenWidthMin, GenDepthHudDropOpenWidthMax, dropMaxWidth);
        float bodyVisualScale = GetGenHudTargetBodyVisualScale();
        float lowerTargetOpenT = Mathf.Clamp01(Mathf.InverseLerp(GenDepthHudDropOpenStartPercent, GenDepthHudDropOpenEndPercent, percent));
        genDepthHudLowerVisualOpenT = Mathf.SmoothDamp(genDepthHudLowerVisualOpenT, lowerTargetOpenT, ref genDepthHudLowerVisualOpenVelocity, GenDepthHudDropSmoothTime);
        float pulseSpeed = inserted ? GenDepthHudInsertPulseSpeed : GenDepthHudIdlePulseSpeed;
        float pulseT = (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) * 0.5f;
        float pulseStrength = inserted ? Mathf.Lerp(0.35f, 1.0f, genDepthHudLowerVisualOpenT) : GenDepthHudIdlePulseStrength;
        float lowerShapeWidth = Mathf.Lerp(GenDepthHudDropClosedWidthScale, dropMaxWidth, genDepthHudLowerVisualOpenT);
        lowerShapeWidth *= 1.0f - pulseT * GenDepthHudDropPulseNarrowScale * pulseStrength;
        float heartPulseScale = inserted ? GenDepthHudHeartPulseMultiplier : 1.0f;
        float heartRadiusScale = 1.0f - pulseT * GenDepthHudHeartPulseNarrowScale * pulseStrength * heartPulseScale;
        float fillWidthScale = Mathf.Lerp(0.58f, 0.88f, manualWidthT) * Mathf.Lerp(1.0f, bodyVisualScale, 0.60f);
        float dropRadius = GenDepthHudBarWidth * 0.78f * bodyVisualScale;
        if (inserted && percent > genDepthInsertedMaxPercent)
        {
            genDepthInsertedMaxPercent = percent;
        }
        if (!inserted)
        {
            genDepthInsertedMaxPercent = 0.0f;
        }
        Color backColor = new Color(0.55f, 0.0f, 0.42f, 0.90f);
        float genFillColorT = Mathf.Clamp01(percent);
        Color fillColor = Color.Lerp(
            new Color(1.0f, 0.72f, 0.86f, 0.90f),
            new Color(1.0f, 0.05f, 0.32f, 0.95f),
            genFillColorT
        );
        Color peakColor = new Color(1.0f, 0.92f, 0.05f, 0.45f);
        Color upperInsertedColor = new Color(1.0f, 0.05f, 0.42f, 0.95f);
        Color lowerInsertedColor = new Color(1.0f, 0.38f, 0.88f, 0.95f);
        Color uninsertedColor = new Color(1.0f, 0.88f, 0.96f, 0.78f);
        Color lowerUninsertedColor = new Color(1.0f, 0.88f, 0.96f, 0.68f);
        Color markerColor = inserted ? upperInsertedColor : uninsertedColor;
        Color lowerMarkerColor = Color.Lerp(lowerUninsertedColor, lowerInsertedColor, genDepthHudLowerVisualOpenT);

        SetGenDepthHudBarLine(genDepthHudBackObj, bottom, top, GenDepthHudBarWidth, backColor);
        if (inserted)
        {
            SetGenDepthHudBarLine(genDepthHudFillObj, bottom, fillTop, GenDepthHudBarWidth * fillWidthScale, fillColor);
        }

        SetGenDepthHudHeart(
            genDepthHudMarkerLine,
            markerCenter,
            cam.transform.right,
            cam.transform.up,
            GenDepthHudBarWidth * 1.10f * heartRadiusScale,
            GenDepthHudBarWidth * 0.16f,
            false,
            markerColor
        );

        SetGenDepthHudDropMarker(
            genDepthHudBottomMarkerLine,
            lowerMarkerCenter,
            cam.transform.right,
            cam.transform.up,
            dropRadius,
            lowerShapeWidth,
            GenDepthHudBarWidth * 0.12f,
            lowerMarkerColor
        );

        UpdateGenDepthHudGContactDot(bottom, upDir, cam, IsGenDepthHudGContactDotActive());

        if (IsGenDepthHudFxEnabled())
        {
            TriggerGenDepthBursts(percent, lowerMarkerCenter, markerCenter, cam.transform.right, cam.transform.up, cam.transform.forward, GetGenHudBurstSizeScale());
            UpdateGenDepthBurstParticles();
        }
        else if (genDepthBurstParticles.Count > 0)
        {
            ClearGenDepthBurstParticles();
        }

        if (inserted)
        {
            UpdateGenDepthPeak(percent);
            float peakRatio = Mathf.Clamp(genDepthPeakPercent / GenDepthHudDisplayMaxPercent, 0.0f, 1.0f);
            Vector3 peakCenter = bottom + upDir * (fullHeight * peakRatio) + cam.transform.forward * GenDepthHudPeakBackOffset;
            Vector3 peakLeft = peakCenter - cam.transform.right * (GenDepthHudBarWidth * 0.72f);
            Vector3 peakRight = peakCenter + cam.transform.right * (GenDepthHudBarWidth * 0.72f);
            SetGenDepthHudBarLine(genDepthHudPeakObj, peakLeft, peakRight, GenDepthHudBarDepth * 1.7f, peakColor);
            genDepthHudPeakObj.SetActive(true);
        }
        else
        {
            genDepthPeakPercent = 0.0f;
            genDepthPeakUntil = 0.0f;
            genDepthHudPeakObj.SetActive(false);
        }

        SetGenDepthHudActive(true);
        if (genDepthHudFillObj != null)
        {
            genDepthHudFillObj.SetActive(inserted);
        }
        if (!inserted && genDepthHudPeakObj != null)
        {
            genDepthHudPeakObj.SetActive(false);
        }
    }

    void UpdateAnusDepthHud(float depth, float length, float percent, bool visible)
    {
        bool active = visible && targetControllerChooser != null && targetControllerChooser.val == "anus";
        Camera cam = Camera.main;
        if (!active || cam == null || anusDepthHudBackObj == null || anusDepthHudFillObj == null || anusDepthHudMarkerObj == null || anusDepthHudStarObj == null)
        {
            SetAnusDepthHudActive(false);
            anusDepthInsertedMaxPercent = 0.0f;
            return;
        }

        Vector3 bottom = cam.ViewportToWorldPoint(new Vector3(GenDepthHudX + AnusDepthHudXOffset, GenDepthHudBottomY, GenDepthHudCameraDistance));
        Vector3 top = cam.ViewportToWorldPoint(new Vector3(GenDepthHudX + AnusDepthHudXOffset, GenDepthHudTopY, GenDepthHudCameraDistance));
        float barWidth = GenDepthHudBarWidth * AnusDepthHudBarWidthScale;
        Vector3 anusHudLeftOffset = -cam.transform.right * (barWidth * AnusDepthHudWholeLeftBarScale);
        bottom += anusHudLeftOffset;
        top += anusHudLeftOffset;
        float fullHeight = Vector3.Distance(bottom, top);
        Vector3 upDir = (top - bottom).normalized;
        float fillRatio = Mathf.Clamp(percent / GenDepthHudDisplayMaxPercent, 0.0f, 1.0f);
        bool inserted = percent > 0.001f;
        float fillHeight = inserted ? Mathf.Max(0.001f, fullHeight * fillRatio) : 0.0f;
        Vector3 fillTop = bottom + upDir * fillHeight;

        if (inserted && percent > anusDepthInsertedMaxPercent)
        {
            anusDepthInsertedMaxPercent = percent;
        }
        if (!inserted)
        {
            anusDepthInsertedMaxPercent = 0.0f;
        }

        Color backColor = new Color(0.42f, 0.05f, 0.22f, 0.82f);
        Color fillColor = Color.Lerp(new Color(1.0f, 0.72f, 0.86f, 0.90f), new Color(1.0f, 0.05f, 0.32f, 0.95f), Mathf.Clamp01(percent));
        SetGenDepthHudBarLine(anusDepthHudBackObj, bottom, top, barWidth, backColor);
        if (inserted)
        {
            SetGenDepthHudBarLine(anusDepthHudFillObj, bottom, fillTop, barWidth * 0.72f, fillColor);
        }

        // v163: no anus 100% horizontal marker; the bar and */隨ｳ繝ｻmarker are enough.
        if (anusDepthHudMarkerObj != null)
        {
            anusDepthHudMarkerObj.SetActive(false);
        }

        float starOpenT = Mathf.InverseLerp(AnusDepthHudStarOpenStartPercent, AnusDepthHudStarOpenEndPercent, Mathf.Clamp01(percent));
        float starPulse = 1.0f + Mathf.Sin(Time.time * AnusDepthHudStarPulseSpeed) * AnusDepthHudStarPulseStrength * Mathf.Lerp(0.35f, 1.0f, starOpenT);
        float starRadius = barWidth * Mathf.Lerp(AnusDepthHudStarClosedSizeScale, AnusDepthHudStarOpenSizeScale, starOpenT) * starPulse;
        float starWidth = GenDepthHudBarDepth * Mathf.Lerp(0.70f, 1.25f, starOpenT);
        Vector3 starCenter = bottom + cam.transform.right * (barWidth * AnusDepthHudStarRightScale) - upDir * (barWidth * AnusDepthHudStarBelowScale) - cam.transform.forward * AnusDepthHudStarForwardOffset;
        Color starColor = Color.Lerp(new Color(1.0f, 0.70f, 0.84f, 0.92f), new Color(1.0f, 0.05f, 0.32f, 0.98f), Mathf.Clamp01(percent));
        bool starCircleMode = starOpenT >= 0.995f;
        SetAnusDepthHudStar(starCenter, cam.transform.right, upDir, starRadius, starWidth, starColor, starCircleMode);

        SetAnusDepthHudActive(true);
        if (anusDepthHudFillObj != null)
        {
            anusDepthHudFillObj.SetActive(inserted);
        }
    }

    void SetAnusDepthHudActive(bool active)
    {
        if (anusDepthHudActiveKnown && anusDepthHudActive == active)
        {
            return;
        }

        anusDepthHudActiveKnown = true;
        anusDepthHudActive = active;

        if (anusDepthHudBackObj != null) anusDepthHudBackObj.SetActive(active);
        if (anusDepthHudFillObj != null) anusDepthHudFillObj.SetActive(active);
        if (anusDepthHudMarkerObj != null) anusDepthHudMarkerObj.SetActive(false);
        if (anusDepthHudStarObj != null) anusDepthHudStarObj.SetActive(active);
    }

    void UpdateGenDepthHudGContactDot(Vector3 bottom, Vector3 upDir, Camera cam, bool active)
    {
        if (genDepthHudGContactDotObj == null || cam == null)
        {
            return;
        }

        if (!active)
        {
            SetGenDepthHudGContactDotActive(false);
            return;
        }

        float dotSize = GenDepthHudBarWidth * GenDepthHudGContactDotSizeScale;
        float pulse = 1.0f + Mathf.Sin(Time.time * GenDepthHudGContactDotPulseSpeed) * GenDepthHudGContactDotPulseStrength;
        Vector3 center =
            bottom
            - upDir * (GenDepthHudBarWidth * GenDepthHudGContactDotBelowScale)
            + upDir * (dotSize * GenDepthHudGContactDotUpDotScale)
            - cam.transform.right * (dotSize * GenDepthHudGContactDotLeftDotScale)
            - cam.transform.forward * GenDepthHudGContactDotForwardOffset;

        Color dotColor = GetGenDepthHudGContactDotColor();
        Material dotMaterial = genDepthHudGContactDotMaterial;
        if (dotMaterial == null)
        {
            Renderer renderer = GetGenDepthHudGContactDotRenderer();
            if (renderer != null)
            {
                dotMaterial = renderer.material;
            }
        }
        if (dotMaterial != null && (!genDepthHudGContactDotColorKnown || genDepthHudGContactDotLastColor != dotColor))
        {
            dotMaterial.color = dotColor;
            genDepthHudGContactDotLastColor = dotColor;
            genDepthHudGContactDotColorKnown = true;
        }

        genDepthHudGContactDotObj.transform.position = center;
        genDepthHudGContactDotObj.transform.rotation = cam.transform.rotation;
        genDepthHudGContactDotObj.transform.localScale = Vector3.one * (dotSize * pulse);
        SetGenDepthHudGContactDotActive(true);
    }

    Renderer GetGenDepthHudGContactDotRenderer()
    {
        if (genDepthHudGContactDotRenderer == null && genDepthHudGContactDotObj != null)
        {
            genDepthHudGContactDotRenderer = genDepthHudGContactDotObj.GetComponent<Renderer>();
        }
        return genDepthHudGContactDotRenderer;
    }

    void SetGenDepthHudGContactDotActive(bool active)
    {
        if (genDepthHudGContactDotObj == null)
        {
            return;
        }

        if (genDepthHudGContactDotActiveKnown && genDepthHudGContactDotActive == active)
        {
            return;
        }

        genDepthHudGContactDotActiveKnown = true;
        genDepthHudGContactDotActive = active;
        genDepthHudGContactDotObj.SetActive(active);
    }

    Color GetGenDepthHudGContactDotColor()
    {
        if (pTipOnGContactKnown && pTipOnGContact)
        {
            return new Color(1.0f, 0.0f, 0.0f, GenDepthHudGContactDotMaxAlpha);
        }

        float nearT = GetGenDepthHudGContactNearT();
        Color far = new Color(1.0f, 0.78f, 0.92f, GenDepthHudGContactDotMinAlpha);
        Color near = new Color(1.0f, 0.12f, 0.55f, GenDepthHudGContactDotMaxAlpha);
        return Color.Lerp(far, near, nearT);
    }

    float GetGenDepthHudGContactNearT()
    {
        if (!lastGenDepthSampleKnown)
        {
            return 0.0f;
        }

        // Wider than the actual contact gate so the always-visible dot shows
        // approach as a clear pale-pink -> deep-pink change before it turns red.
        float lateralMax = Mathf.Max(0.0001f, PushAutoGDepthEnterLateralMax * GenDepthHudGContactColorLateralRangeScale);
        float backDepthMax = Mathf.Max(0.0001f, PushAutoNearZeroBackDepth * GenDepthHudGContactColorBackDepthRangeScale);
        float lateralRatio = Mathf.Clamp01(lastGenDepthLateral / lateralMax);
        float backDepthRatio = Mathf.Clamp01(Mathf.Max(0.0f, -lastGenDepthRawDepth) / backDepthMax);
        float normalizedDistance = Mathf.Sqrt(lateralRatio * lateralRatio + backDepthRatio * backDepthRatio);
        float nearT = 1.0f - Mathf.Clamp01(normalizedDistance);
        return Mathf.Pow(Mathf.Clamp01(nearT), GenDepthHudGContactColorCurvePower);
    }

    bool IsGenDepthHudGContactDotActive()
    {
        return targetControllerChooser == null || targetControllerChooser.val == "genital";
    }

    void UpdateGenDepthPeak(float percent)
    {
        if (percent >= genDepthPeakPercent || Time.time >= genDepthPeakUntil)
        {
            genDepthPeakPercent = percent;
            genDepthPeakUntil = Time.time + GenDepthHudPeakHoldSeconds;
        }
    }

    void TriggerGenDepthBursts(float percent, Vector3 zeroCenter, Vector3 maxCenter, Vector3 right, Vector3 up, Vector3 forward, float zeroBurstScale)
    {
        if (previousGenDepthPercent <= 0.001f && percent > 0.001f && Time.time >= nextZeroBurstTime)
        {
            SpawnGenDepthBurst(zeroCenter, right, up, forward, 0.75f, true, zeroBurstScale);
            nextZeroBurstTime = Time.time + GenDepthBurstCooldownZero;
        }

        if (previousGenDepthPercent < 1.0f && percent >= 1.0f && Time.time >= nextMaxBurstTime)
        {
            SpawnGenDepthBurst(maxCenter, right, up, forward, 1.0f, false, 1.0f);
            nextMaxBurstTime = Time.time + GenDepthBurstCooldownMax;
        }

        previousGenDepthPercent = percent;
    }

    void SpawnGenDepthBurst(Vector3 center, Vector3 right, Vector3 up, Vector3 forward, float power, bool triangle, float sizeScale)
    {
        if (genDepthBurstMaterials == null || genDepthBurstMaterials.Length == 0)
        {
            return;
        }

        for (int i = 0; i < GenDepthBurstParticleCount; i++)
        {
            GameObject particle = triangle ? CreateGenDepthBurstTriangleObject() : GameObject.CreatePrimitive(PrimitiveType.Sphere);
            particle.name = "TargetLinePerson_GenDepthHud_Burst";

            Collider col = particle.GetComponent<Collider>();
            if (col != null)
            {
                Destroy(col);
            }

            Renderer renderer = particle.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = genDepthBurstMaterials[i % genDepthBurstMaterials.Length];
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }

            float angle = ((Mathf.PI * 2.0f) * i) / GenDepthBurstParticleCount;
            float speed = (0.075f + UnityEngine.Random.value * 0.065f) * power * sizeScale;
            Vector3 dir =
                right * Mathf.Cos(angle) +
                up * Mathf.Sin(angle) +
                forward * (0.10f + UnityEngine.Random.value * 0.12f);
            if (dir.sqrMagnitude < 0.0001f)
            {
                dir = up;
            }
            dir.Normalize();

            float size = GenDepthBurstSize * (0.75f + UnityEngine.Random.value * 0.65f) * sizeScale;
            particle.transform.position = center;
            particle.transform.rotation = Quaternion.LookRotation(forward, up);
            particle.transform.localScale = Vector3.one * size;

            GenDepthBurstParticle burst = new GenDepthBurstParticle();
            burst.obj = particle;
            burst.velocity = dir * speed;
            burst.startTime = Time.time;
            burst.endTime = Time.time + GenDepthBurstLifetime;
            burst.size = size;
            genDepthBurstParticles.Add(burst);
        }
    }

    GameObject CreateGenDepthBurstTriangleObject()
    {
        GameObject obj = new GameObject("TargetLinePerson_GenDepthHud_Burst_Triangle");
        MeshFilter filter = obj.AddComponent<MeshFilter>();
        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[]
        {
            new Vector3(0.0f, 0.75f, 0.0f),
            new Vector3(-0.65f, -0.45f, 0.0f),
            new Vector3(0.65f, -0.45f, 0.0f)
        };
        mesh.triangles = new int[]
        {
            0, 1, 2,
            0, 2, 1
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        filter.mesh = mesh;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        return obj;
    }

    void UpdateGenDepthBurstParticles()
    {
        for (int i = genDepthBurstParticles.Count - 1; i >= 0; i--)
        {
            GenDepthBurstParticle particle = genDepthBurstParticles[i];
            if (particle == null || particle.obj == null || Time.time >= particle.endTime)
            {
                if (particle != null && particle.obj != null)
                {
                    Destroy(particle.obj);
                }
                genDepthBurstParticles.RemoveAt(i);
                continue;
            }

            float age = Time.time - particle.startTime;
            float t = Mathf.Clamp01(age / GenDepthBurstLifetime);
            particle.obj.transform.position += particle.velocity * Time.deltaTime;
            particle.obj.transform.localScale = Vector3.one * (particle.size * (1.0f - t * 0.65f));
        }
    }

    void ClearGenDepthBurstParticles()
    {
        for (int i = genDepthBurstParticles.Count - 1; i >= 0; i--)
        {
            GenDepthBurstParticle particle = genDepthBurstParticles[i];
            if (particle != null && particle.obj != null)
            {
                Destroy(particle.obj);
            }
            genDepthBurstParticles.RemoveAt(i);
        }
    }

    void SetGenDepthHudActive(bool active)
    {
        if (!active)
        {
            SetGenDepthHudGContactDotActive(false);
            SetAnusDepthHudActive(false);
        }

        if (genDepthHudActiveKnown && genDepthHudActive == active)
        {
            return;
        }

        genDepthHudActiveKnown = true;
        genDepthHudActive = active;

        if (genDepthHudBackObj != null) genDepthHudBackObj.SetActive(active);
        if (genDepthHudFillObj != null) genDepthHudFillObj.SetActive(active);
        if (genDepthHudMarkerObj != null) genDepthHudMarkerObj.SetActive(active);
        if (genDepthHudBottomMarkerObj != null) genDepthHudBottomMarkerObj.SetActive(active);
        if (genDepthHudPeakObj != null) genDepthHudPeakObj.SetActive(active);
    }

    void UpdateDebugLines(bool visible)
    {
        // Show Lines is visual only.  Yellow Guide itself is still built and used
        // internally even when the yellow LineRenderer is hidden.
        if (!captured)
        {
            SetDebugLineRenderersEnabled(false);
            return;
        }

        if (!hasYellowPPath && !isAvoidMoving && !pYellowCapturePending)
        {
            BuildCapturedYellowPPath();
        }

        bool draw = visible && captured;

        if (!draw)
        {
            SetDebugLineRenderersEnabled(false);
            lastDebugLineRenderTime = -999f;
            return;
        }

        SetDebugLineRenderersEnabled(true);
        if (Time.time - lastDebugLineRenderTime < GetDebugLineRenderInterval())
        {
            return;
        }
        lastDebugLineRenderTime = Time.time;

        DrawOriginalRedGreenLines();
        DrawGDepthGuideLine();
        DrawYellowPPathAndPurpleBendMarker();
    }

    void SetDebugLineRenderersEnabled(bool enabled)
    {
        if (debugLinesEnabledKnown && debugLinesEnabled == enabled)
        {
            if (enabled)
            {
                if (penisPathLine != null) penisPathLine.enabled = hasYellowPPath;
                if (bendMarkerLine != null) bendMarkerLine.enabled = hasYellowPPath;
            }
            return;
        }

        debugLinesEnabledKnown = true;
        debugLinesEnabled = enabled;

        if (forwardLine != null) forwardLine.enabled = enabled;
        if (moveLine != null) moveLine.enabled = enabled;
        if (penisPathLine != null) penisPathLine.enabled = enabled && hasYellowPPath;
        if (bendMarkerLine != null) bendMarkerLine.enabled = enabled && hasYellowPPath;
        if (gDepthGuideLine != null) gDepthGuideLine.enabled = enabled;
    }

    void ScheduleDelayedLineLock(string reason)
    {
        if (delayedLineLockRoutine != null)
        {
            StopCoroutine(delayedLineLockRoutine);
            delayedLineLockRoutine = null;
        }

        hasYellowPPath = false;
        hasCapturedMoveLine = false;
        hasCapturedGreenBaseY = false;
        pYellowCapturePending = true;
        delayedLineLockRoutine = StartCoroutine(DelayedLineLockRoutine(reason));
    }

    IEnumerator DelayedLineLockRoutine(string reason)
    {
        // Wait for VaM/physics/controller transforms to settle after root placement.
        yield return null;
        yield return new WaitForSeconds(LineLockDelaySeconds);

        hasYellowPPath = false;
        hasCapturedMoveLine = false;
        // BuildCapturedYellowPPath locks greenY once from the settled P Base/hip Y.
        BuildCapturedYellowPPath();
        LogCapturedGreenGuideLength(reason + "+delay" + LineLockDelaySeconds.ToString("F2"));

        if (hasYellowPPath && hasCapturedMoveLine)
        {
            CaptureUpperBodyLowerBase(reason + "+delay");
        }

        pYellowCapturePending = false;
        delayedLineLockRoutine = null;
    }

    void LogCapturedGreenGuideLength(string reason)
    {
        if (!hasCapturedMoveLine)
        {
            DebugLog("[TargetLinePerson] Green guide lock skipped / reason=" + reason + " / no line");
            return;
        }

        float len = Vector3.Distance(capturedMoveLineStart, capturedMoveLineEnd);
        string yInfo = hasCapturedGreenBaseY ? (" / greenY=" + capturedGreenBaseY.ToString("F3")) : "";
        DebugLog("[TargetLinePerson] Green guide locked AFTER placement / reason=" + reason + " / len=" + len.ToString("F3") + yInfo);
    }

    void DrawOriginalRedGreenLines()
    {
        Vector3 redOrigin = hasDynamicRedLineDisplay ? dynamicRedLineOrigin : capturedOrigin;
        Vector3 forwardDir = hasDynamicRedLineDisplay ? dynamicRedLineDir : capturedLineDir;

        if (forwardDir.sqrMagnitude < 0.0001f)
        {
            forwardDir = capturedDir.sqrMagnitude >= 0.0001f ? capturedDir : Vector3.forward;
        }

        forwardDir.Normalize();

        if (forwardLine != null)
        {
            forwardLine.positionCount = 2;
            forwardLine.SetPosition(0, redOrigin);
            forwardLine.SetPosition(1, redOrigin + forwardDir * 1.5f);
        }

        if (moveLine == null)
        {
            return;
        }

        if (pYellowCapturePending && !hasCapturedMoveLine)
        {
            moveLine.enabled = false;
            return;
        }

        // Green line is fixed at the same time as the yellow P path is built.
        // Do not use the current P/base position here, because advancing P would
        // otherwise move the green guide itself.
        if (!hasCapturedMoveLine && !isAvoidMoving && !pYellowCapturePending)
        {
            BuildCapturedYellowPPath();
        }

        if (hasCapturedMoveLine)
        {
            moveLine.positionCount = 2;
            moveLine.SetPosition(0, capturedMoveLineStart);
            moveLine.SetPosition(1, capturedMoveLineEnd);
        }
    }

    void DrawGDepthGuideLine()
    {
        if (gDepthGuideLine == null)
        {
            return;
        }

        Vector3 origin;
        Vector3 dir;
        float length;
        string depthTargetMode;
        if (!TryGetLiveCurrentInsideLine(out origin, out dir, out length, out depthTargetMode))
        {
            gDepthGuideLine.enabled = false;
            return;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            gDepthGuideLine.enabled = false;
            return;
        }

        dir.Normalize();
        float gDepthAngle = Mathf.Abs(Mathf.Asin(Mathf.Clamp(dir.y, -1.0f, 1.0f)) * Mathf.Rad2Deg);
        if (gDepthAngle > GenDepthAngleGateLimitDegrees)
        {
            gDepthGuideLine.enabled = false;
            if (!lastGDepthGuideAngleGateBlocked)
            {
                DebugLog(
                    "[TargetLinePerson] G Depth Guide hidden" +
                    " / reason=angle" +
                    " / angle=" + gDepthAngle.ToString("F1") +
                    " / limit=" + GenDepthAngleGateLimitDegrees.ToString("F1")
                );
            }
            lastGDepthGuideAngleGateBlocked = true;
            return;
        }

        if (lastGDepthGuideAngleGateBlocked)
        {
            DebugLog(
                "[TargetLinePerson] G Depth Guide visible" +
                " / reason=angle-clear" +
                " / angle=" + gDepthAngle.ToString("F1") +
                " / limit=" + GenDepthAngleGateLimitDegrees.ToString("F1")
            );
        }
        lastGDepthGuideAngleGateBlocked = false;
        gDepthGuideLine.enabled = true;

        float drawLength = 1.00f;
        float backStub = 0.12f;
        gDepthGuideLine.positionCount = 3;
        gDepthGuideLine.SetPosition(0, origin - dir * backStub);
        gDepthGuideLine.SetPosition(1, origin);
        gDepthGuideLine.SetPosition(2, origin + dir * drawLength);

        if (Time.time - lastGDepthGuideLineLogTime >= 5.0f)
        {
            lastGDepthGuideLineLogTime = Time.time;
            DebugLog(
                "[TargetLinePerson] G Depth Guide line" +
                " / origin=(" + FormatVector3(origin) + ")" +
                " / dir=(" + FormatVector3(dir) + ")" +
                " / angle=" + gDepthAngle.ToString("F1") +
                " / limit=" + GenDepthAngleGateLimitDegrees.ToString("F1") +
                " / backStub=" + backStub.ToString("F3") +
                " / len=" + drawLength.ToString("F3")
            );
        }
    }

    void DrawYellowPPathAndPurpleBendMarker()
    {
        if (penisPathLine == null || bendMarkerLine == null)
        {
            return;
        }

        if (pYellowCapturePending)
        {
            penisPathLine.enabled = false;
            bendMarkerLine.enabled = false;
            return;
        }

        if (!hasYellowPPath && !isAvoidMoving)
        {
            BuildCapturedYellowPPath();
        }

        if (!hasYellowPPath)
        {
            penisPathLine.enabled = false;
            bendMarkerLine.enabled = false;
            return;
        }

        penisPathLine.positionCount = YellowPPathPointCount;
        for (int i = 0; i < YellowPPathPointCount; i++)
        {
            penisPathLine.SetPosition(i, yellowPPathPoints[i]);
        }

        // Purple marker: short horizontal marker at captured/origin bend point p4.
        Vector3 markerDir = yellowPPathPoints[5] - yellowPPathPoints[4];
        markerDir.y = 0f;
        if (markerDir.sqrMagnitude < 0.0001f)
        {
            markerDir = yellowPPathPoints[4] - yellowPPathPoints[3];
            markerDir.y = 0f;
        }
        if (markerDir.sqrMagnitude < 0.0001f)
        {
            markerDir = Vector3.forward;
        }
        markerDir.Normalize();

        Vector3 markerSide = Vector3.Cross(Vector3.up, markerDir);
        if (markerSide.sqrMagnitude < 0.0001f)
        {
            markerSide = Vector3.right;
        }
        markerSide.Normalize();

        Vector3 p4 = yellowPPathPoints[4];
        bendMarkerLine.positionCount = 2;
        bendMarkerLine.SetPosition(0, p4 - markerSide * 0.07f);
        bendMarkerLine.SetPosition(1, p4 + markerSide * 0.07f);

        // Drawing only. Baseline build never applies P movement from yellow path.
    }

    bool IsOwnLiePoseForYellowGuide()
    {
        FreeControllerV3 ownHip = GetOwnHip();
        FreeControllerV3 ownChest = GetOwnChest();

        if (ownHip == null || ownChest == null)
        {
            return false;
        }

        Vector3 upper = ownChest.transform.position - ownHip.transform.position;
        if (upper.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        float verticalDot = Mathf.Abs(Vector3.Dot(upper.normalized, Vector3.up));
        return verticalDot <= YellowGuideLieVerticalDotThreshold;
    }

    bool IsPControlBlockedByLiePose()
    {
        return rideLieActive || IsOwnLiePoseForYellowGuide();
    }

    void ResetPFollowForLiePose(string reason)
    {
        ClearTipYellowParallelLock();

        if (pAngleAtYellowP3Applied)
        {
            ResetPAngleAtYellowP3IfApplied(reason);
        }
        else
        {
            RestoreDynamicPBaseYStateIfApplied();
        }
    }

    float GetTargetAxisAngleFromOwnDegrees(Vector3 flatApproachDir, Vector3 axisUpDir)
    {
        flatApproachDir.y = 0f;
        if (flatApproachDir.sqrMagnitude < 0.0001f)
        {
            flatApproachDir = capturedDir;
            flatApproachDir.y = 0f;
        }
        if (flatApproachDir.sqrMagnitude < 0.0001f)
        {
            flatApproachDir = Vector3.forward;
        }
        flatApproachDir.Normalize();

        if (axisUpDir.sqrMagnitude < 0.0001f)
        {
            axisUpDir = capturedLineDir;
        }
        if (axisUpDir.sqrMagnitude < 0.0001f)
        {
            axisUpDir = Vector3.forward;
        }
        axisUpDir.Normalize();

        if (axisUpDir.y < 0f)
        {
            axisUpDir = -axisUpDir;
        }

        float along = Vector3.Dot(axisUpDir, flatApproachDir);
        float up = Mathf.Max(0f, axisUpDir.y);
        float angle = Mathf.Atan2(up, along) * Mathf.Rad2Deg;

        if (angle < 0f)
        {
            angle += 180f;
        }

        return Mathf.Clamp(angle, 0f, 180f);
    }

    float GetYellowButtGuideScale()
    {
        return yellowButtGuideScale != null ? Mathf.Clamp(yellowButtGuideScale.val, 0.50f, 3.00f) : 1.0f;
    }

    void OnYellowButtGuideScaleChanged(float value)
    {
        if (!captured)
        {
            return;
        }

        // Rebuild only the guide shape.  The actual capture target remains the same,
        // but yellowPPathPoints must be recalculated because the dip clearance changed.
        hasYellowPPath = false;
        hasCapturedMoveLine = false;
        ClearTipYellowParallelLock();
        lastUpperBodyYellowLowerPhase = "";
        lastLoggedUpperBodyYellowProgress = -1f;

        ApplyCapturedPlacementOnce("yellow butt guide scale slider", true);

        DebugLog("[TargetLinePerson] Yellow Butt Guide Scale changed / value=" + GetYellowButtGuideScale().ToString("F2") + " / guide will rebuild");
    }

    void BuildCapturedYellowPPath()
    {
        hasYellowPPath = false;
        hasCapturedMoveLine = false;
        yellowGuideHasDip = false;
        yellowGuideOwnLieFlat = false;
        yellowGuideTargetAxisAngleDeg = 0f;

        FreeControllerV3 penisBase = GetOwnPenisBase();
        FreeControllerV3 penisMid = GetOwnPenisMid();
        FreeControllerV3 penisTip = GetOwnPenisTip();
        FreeControllerV3 ownHip = GetOwnHip();

        CapturePYellowOriginalState(penisBase, penisMid, penisTip);

        Vector3 p0;
        Vector3 p1;

        if (penisBase != null)
        {
            p0 = penisBase.transform.position;
        }
        else if (ownHip != null)
        {
            p0 = ownHip.transform.position;
        }
        else if (containingAtom != null && containingAtom.mainController != null)
        {
            p0 = containingAtom.mainController.transform.position;
        }
        else
        {
            return;
        }

        if (!hasCapturedGreenBaseY)
        {
            capturedGreenBaseY = p0.y;
            hasCapturedGreenBaseY = true;
        }

        // The green/yellow guide height must not follow later P physics.
        // Lock the guide plane once per capture, then build p0/p1/p2/p3 on that plane.
        p0.y = capturedGreenBaseY;

        if (nowDockingLineFitActive)
        {
            Vector3 smartShapeDir = capturedDir;
            smartShapeDir.y = 0f;

            if (smartShapeDir.sqrMagnitude >= 0.0001f)
            {
                smartShapeDir.Normalize();

                Vector3 realP0 = p0;
                Vector3 flatOriginForSmartShape = capturedOrigin;
                flatOriginForSmartShape.y = capturedGreenBaseY;
                p0 = flatOriginForSmartShape + smartShapeDir * NowDockingYellowSmartShapeDistance;

                DebugLog("[TargetLinePerson] Now Docking smart yellow shape" +
                    " / realP0=(" + FormatVector3(realP0) + ")" +
                    " / virtualP0=(" + FormatVector3(p0) + ")" +
                    " / smartDistance=" + NowDockingYellowSmartShapeDistance.ToString("F3"));
            }
        }

        Vector3 approachDir = capturedLineDir;
        approachDir.y = 0f;
        if (approachDir.sqrMagnitude < 0.0001f)
        {
            approachDir = capturedDir;
            approachDir.y = 0f;
        }
        if (approachDir.sqrMagnitude < 0.0001f)
        {
            approachDir = Vector3.forward;
        }
        approachDir.Normalize();

        // GREEN70_THEN_DIP_BUILD:
        // First follow the original green guide most of the way.
        // Only after reaching about 70% of the P-base -> capturedOrigin line
        // does the safer dip/yellow curve begin.
        float greenStartRatio = 0.70f;

        // Keep the green guide horizontal.  The original target point may be
        // higher/lower than P base, but the green guide is only the flat
        // approach direction.  The yellow/dip section handles vertical change.
        Vector3 flatCapturedOrigin = capturedOrigin;
        flatCapturedOrigin.y = capturedGreenBaseY;
        Vector3 greenToOrigin = flatCapturedOrigin - p0;

        if (greenToOrigin.sqrMagnitude > 0.0001f)
        {
            p1 = Vector3.Lerp(p0, flatCapturedOrigin, greenStartRatio);
        }
        else if (penisTip != null)
        {
            p1 = penisTip.transform.position;
            p1.y = capturedGreenBaseY;
        }
        else
        {
            p1 = p0 + approachDir * 0.20f;
        }

        Vector3 redDir = capturedLineDir;
        if (redDir.sqrMagnitude < 0.0001f)
        {
            redDir = capturedDir.sqrMagnitude >= 0.0001f ? capturedDir : Vector3.forward;
        }
        if (redDir.sqrMagnitude < 0.0001f)
        {
            redDir = Vector3.forward;
        }
        redDir.Normalize();

        Vector3 redUpDir = redDir.y >= 0f ? redDir : -redDir;

        Vector3 baseToOrigin = capturedOrigin - p1;
        Vector3 approachFlat = baseToOrigin;
        approachFlat.y = 0f;
        if (approachFlat.sqrMagnitude < 0.0001f)
        {
            approachFlat = approachDir;
        }

        float remainingFlat = approachFlat.magnitude;

        if (approachFlat.sqrMagnitude < 0.0001f)
        {
            approachFlat = approachDir;
            remainingFlat = 0.20f;
        }

        approachFlat.Normalize();

        // Yellow Guide is always the internal Own P route.
        // Only the guide SHAPE changes:
        //   - non-lie + Target Axis 75-115 degrees: dip guide
        //   - lie or other axis angles: flat green-parallel guide
        yellowGuideOwnLieFlat = IsOwnLiePoseForYellowGuide();
        yellowGuideTargetAxisAngleDeg = GetTargetAxisAngleFromOwnDegrees(approachFlat, redUpDir);
        yellowGuideHasDip = !yellowGuideOwnLieFlat &&
            yellowGuideTargetAxisAngleDeg >= YellowGuideDipAngleMinDegrees &&
            yellowGuideTargetAxisAngleDeg <= YellowGuideDipAngleMaxDegrees;

        Vector3 p2;
        Vector3 p3;

        if (yellowGuideHasDip)
        {
            // Yellow Butt Guide Scale:
            // 1 slider expands the visible yellow trapezoid for larger hips/butt.
            // - Height: deeper dip.
            // - Width: longer low/clearance section by shortening the two ramp-in/out margins.
            float buttScale = GetYellowButtGuideScale();
            float dipDown = 0.08f * buttScale;
            float shapeRemainingFlat = remainingFlat;

            float dipForwardBase = Mathf.Clamp(shapeRemainingFlat * 0.25f, 0.04f, 0.10f);
            float originBackBase = Mathf.Clamp(shapeRemainingFlat * 0.55f, 0.04f, 0.12f);
            float rampScale = 1.0f / Mathf.Max(0.01f, buttScale);

            float dipForward = dipForwardBase * rampScale;
            float originBack = originBackBase * rampScale;

            float minRamp = Mathf.Min(0.015f, shapeRemainingFlat * 0.10f);
            float maxRamp = Mathf.Max(minRamp, shapeRemainingFlat * 0.45f);
            dipForward = Mathf.Clamp(dipForward, minRamp, maxRamp);
            originBack = Mathf.Clamp(originBack, minRamp, maxRamp);

            // Keep at least a small horizontal bottom section.  This prevents the
            // trapezoid from collapsing when the remaining distance is short.
            float minBottom = Mathf.Min(0.03f * buttScale, shapeRemainingFlat * 0.20f);
            float bottom = shapeRemainingFlat - dipForward - originBack;
            if (shapeRemainingFlat > 0.0001f && bottom < minBottom)
            {
                float reduce = (minBottom - bottom) * 0.5f;
                dipForward = Mathf.Max(minRamp, dipForward - reduce);
                originBack = Mathf.Max(minRamp, originBack - reduce);
            }

            if (nowDockingLineFitActive)
            {
                DebugLog("[TargetLinePerson] Now Docking yellow smart shape dip" +
                    " / remainingFlat=" + remainingFlat.ToString("F3") +
                    " / shapeRemainingFlat=" + shapeRemainingFlat.ToString("F3") +
                    " / dipForward=" + dipForward.ToString("F3") +
                    " / originBack=" + originBack.ToString("F3") +
                    " / dipDown=" + dipDown.ToString("F3"));
            }

            p2 = p1 + approachFlat * dipForward + Vector3.down * dipDown;

            // Keep the dipped approach section on the same flat green-plane.
            // Only p4/p5 are allowed to rise to the actual captured target height.
            p3 = flatCapturedOrigin - approachFlat * originBack + Vector3.down * dipDown;
        }
        else
        {
            Vector3 p1ToOrigin = flatCapturedOrigin - p1;
            p1ToOrigin.y = 0f;

            if (p1ToOrigin.sqrMagnitude > 0.0001f)
            {
                p2 = Vector3.Lerp(p1, flatCapturedOrigin, 0.45f);
                p3 = Vector3.Lerp(p1, flatCapturedOrigin, 0.85f);
            }
            else
            {
                p2 = p1 + approachFlat * 0.04f;
                p3 = p1 + approachFlat * 0.08f;
            }

            // Lie/flat guide: keep the guide on the green plane and parallel to green.
            p2.y = capturedGreenBaseY;
            p3.y = capturedGreenBaseY;
        }

        Vector3 p4 = capturedOrigin;
        Vector3 p5 = capturedOrigin + GetYellowEndDirection(redDir) * 1.00f;

        yellowPPathPoints[0] = p0;
        yellowPPathPoints[1] = p1;
        yellowPPathPoints[2] = p2;
        yellowPPathPoints[3] = p3;
        yellowPPathPoints[4] = p4;
        yellowPPathPoints[5] = p5;

        // Freeze the original green guide at path-build time.
        // Original meaning: green line is P base -> capturedOrigin.
        // It is saved here so later P movement cannot drag the guide line.
        capturedMoveLineStart = p0;
        capturedMoveLineEnd = flatCapturedOrigin;
        hasCapturedMoveLine = true;

        CaptureBodyRootForYellowPath();

        yellowPPathLengths[0] = 0f;
        yellowPPathTotalLength = 0f;
        for (int i = 1; i < YellowPPathPointCount; i++)
        {
            yellowPPathTotalLength += Vector3.Distance(yellowPPathPoints[i - 1], yellowPPathPoints[i]);
            yellowPPathLengths[i] = yellowPPathTotalLength;
        }

        if (penisBase != null && penisMid != null)
        {
            yellowBaseToMidLength = Vector3.Distance(penisBase.transform.position, penisMid.transform.position);
        }
        else
        {
            yellowBaseToMidLength = 0.08f;
        }

        if (penisMid != null && penisTip != null)
        {
            yellowMidToTipLength = Vector3.Distance(penisMid.transform.position, penisTip.transform.position);
        }
        else if (penisBase != null && penisTip != null)
        {
            yellowMidToTipLength = Mathf.Max(0.02f, Vector3.Distance(penisBase.transform.position, penisTip.transform.position) - yellowBaseToMidLength);
        }
        else
        {
            yellowMidToTipLength = 0.08f;
        }

        hasYellowPPath = yellowPPathTotalLength > 0.0001f;
    }


    void CaptureBodyRootForYellowPath()
    {
        if (containingAtom == null || containingAtom.mainController == null)
        {
            hasCapturedBodyRoot = false;
            hasCapturedBodyHip = false;
            return;
        }

        capturedBodyRootPosition = containingAtom.mainController.transform.position;
        capturedBodyRootRotation = containingAtom.mainController.transform.rotation;
        hasCapturedBodyRoot = true;

        FreeControllerV3 ownHip = GetOwnHip();
        if (ownHip != null)
        {
            capturedBodyHipPosition = ownHip.transform.position;
            hasCapturedBodyHip = true;
        }
        else
        {
            hasCapturedBodyHip = false;
        }
    }

    void RestoreCapturedBodyRootForYellowPath()
    {
        if (!hasCapturedBodyRoot || containingAtom == null || containingAtom.mainController == null)
        {
            return;
        }

        containingAtom.mainController.transform.position = capturedBodyRootPosition;
        containingAtom.mainController.transform.rotation = capturedBodyRootRotation;
    }

    void ReleasePYellowPathControl(string reason)
    {
        FreeControllerV3 penisBase = GetOwnPenisBase();
        FreeControllerV3 penisMid = GetOwnPenisMid();
        FreeControllerV3 penisTip = GetOwnPenisTip();

        bool restored = RestorePYellowOriginalState(penisBase, penisMid, penisTip);

        if (!restored)
        {
            ReleasePYellowController(penisBase);
            ReleasePYellowController(penisMid);
            ReleasePYellowController(penisTip);
        }

        hasYellowPPath = false;
        hasCapturedMoveLine = false;
        hasCapturedGreenBaseY = false;
        yellowPPathTotalLength = 0f;
        hasCapturedBodyRoot = false;
        hasCapturedBodyHip = false;
        pAngleAtYellowP3Applied = false;
        pDynamicBaseYApplied = false;
        lastDynamicPBaseOffset = Vector3.zero;

        if (pYellowPathAdvance != null)
        {
            pYellowPathAdvance.valNoCallback = 0.0f;
        }

        if (pYellowPathAlign != null)
        {
            pYellowPathAlign.val = false;
        }

        DebugLog("[TargetLinePerson] P Yellow Path released / reason=" + reason + " / restored=" + restored + " / advance=0.000 / align=OFF");
    }

    void CapturePYellowOriginalState(FreeControllerV3 baseFc, FreeControllerV3 midFc, FreeControllerV3 tipFc)
    {
        if (baseFc == null || midFc == null || tipFc == null)
        {
            pYellowOriginalCaptured = false;
            return;
        }

        savedPBasePosition = baseFc.transform.position;
        savedPMidPosition = midFc.transform.position;
        savedPTipPosition = tipFc.transform.position;
        savedPBaseRotation = baseFc.transform.rotation;
        savedPMidRotation = midFc.transform.rotation;
        savedPTipRotation = tipFc.transform.rotation;

        savedPBasePositionState = baseFc.currentPositionState;
        savedPMidPositionState = midFc.currentPositionState;
        savedPTipPositionState = tipFc.currentPositionState;
        savedPBaseRotationState = baseFc.currentRotationState;
        savedPMidRotationState = midFc.currentRotationState;
        savedPTipRotationState = tipFc.currentRotationState;

        pYellowOriginalCaptured = true;
    }

    bool RestorePYellowOriginalState(FreeControllerV3 baseFc, FreeControllerV3 midFc, FreeControllerV3 tipFc)
    {
        if (!pYellowOriginalCaptured)
        {
            return false;
        }

        if (baseFc == null || midFc == null || tipFc == null)
        {
            pYellowOriginalCaptured = false;
            return false;
        }

        RestorePYellowController(baseFc, savedPBasePosition, savedPBaseRotation, savedPBasePositionState, savedPBaseRotationState);
        RestorePYellowController(midFc, savedPMidPosition, savedPMidRotation, savedPMidPositionState, savedPMidRotationState);
        RestorePYellowController(tipFc, savedPTipPosition, savedPTipRotation, savedPTipPositionState, savedPTipRotationState);

        pYellowOriginalCaptured = false;
        return true;
    }

    void RestorePYellowController(FreeControllerV3 fc, Vector3 position, Quaternion rotation, FreeControllerV3.PositionState positionState, FreeControllerV3.RotationState rotationState)
    {
        if (fc == null)
        {
            return;
        }

        fc.transform.position = position;
        fc.transform.rotation = rotation;

        if (fc.control != null)
        {
            fc.control.position = position;
            fc.control.rotation = rotation;
        }

        fc.currentPositionState = positionState;
        fc.currentRotationState = rotationState;
    }

    void ReleasePYellowController(FreeControllerV3 fc)
    {
        if (fc == null)
        {
            return;
        }

        fc.currentPositionState = FreeControllerV3.PositionState.Off;
        fc.currentRotationState = FreeControllerV3.RotationState.Off;
    }

    void AddYellowPathAdvance(float delta)
    {
        float now = pYellowPathAdvance != null ? pYellowPathAdvance.val : 0f;
        SetYellowPathAdvance(now + delta, delta >= 0f ? "advance + button" : "advance - button");
    }

    void SetYellowPathAdvance(float value, string reason)
    {
        float next = Mathf.Clamp(value, 0.0f, GetYellowPathAdvanceMax());

        if (pYellowPathAlign != null)
        {
            pYellowPathAlign.val = true;
        }

        if (pYellowPathAdvance != null)
        {
            pYellowPathAdvance.valNoCallback = next;
        }

        DebugLog("[TargetLinePerson] P Yellow Path advance set / reason=" + reason + " / advance=" + next.ToString("F3"));
        ApplyYellowPPathAlignmentDebug(reason);
    }

    float GetYellowPathAdvanceMax()
    {
        if (!hasYellowPPath)
        {
            BuildCapturedYellowPPath();
        }

        if (!hasYellowPPath)
        {
            return 2.0f;
        }

        // Test build: allow Yellow Path Advance to reach the full yellow guide.
        // v108 stopped slightly before the end with shaftLen * 0.25.
        float max = yellowPPathTotalLength;
        return Mathf.Clamp(max, 0.0f, 2.0f);
    }

    void OnYellowPathAdvanceChanged(float value)
    {
        float clamped = Mathf.Clamp(value, 0.0f, GetYellowPathAdvanceMax());
        if (pYellowPathAdvance != null && Mathf.Abs(clamped - value) > 0.0001f)
        {
            pYellowPathAdvance.valNoCallback = clamped;
        }

        // Slider callback: make it obvious whether the value is reaching the apply code.
        ApplyYellowPPathAlignmentDebug("slider");
    }

    void ApplyYellowPPathAlignmentIfNeeded()
    {
        ApplyYellowPPathAlignmentCore(false, "update");
    }

    void ApplyYellowPPathAlignmentDebug(string reason)
    {
        ApplyYellowPPathAlignmentCore(true, reason);
    }

    bool ApplyYellowPPathAlignmentCore(bool log, string reason)
    {
        log = log && IsDebugViewEnabled();

        if (pushPRoutine != null)
        {
            if (log) LogMessageIfDebug("[TargetLinePerson] P Yellow Path apply skipped: PUSH running / reason=" + reason);
            return false;
        }

        if (pPathSealed != null && pPathSealed.val)
        {
            if (log) LogMessageIfDebug("[TargetLinePerson] P Yellow Path apply skipped: P Path Sealed ON / reason=" + reason);
            return false;
        }

        if (pYellowPathAlign == null || !pYellowPathAlign.val)
        {
            if (log) LogMessageIfDebug("[TargetLinePerson] P Yellow Path apply skipped: Align OFF / reason=" + reason);
            return false;
        }

        if (!hasYellowPPath && !pYellowCapturePending)
        {
            BuildCapturedYellowPPath();
        }

        if (!hasYellowPPath)
        {
            if (log) LogMessageIfDebug("[TargetLinePerson] P Yellow Path apply skipped: no yellow path / reason=" + reason);
            return false;
        }

        if (IsPControlBlockedByLiePose())
        {
            if (log) LogMessageIfDebug("[TargetLinePerson] P Yellow Path apply skipped: lie pose active / reason=" + reason);
            return false;
        }

        if (isAvoidMoving)
        {
            if (log) LogMessageIfDebug("[TargetLinePerson] P Yellow Path apply skipped: avoid moving / reason=" + reason);
            return false;
        }

        if (targetControllerChooser != null && targetControllerChooser.val != "genital")
        {
            if (log) LogMessageIfDebug("[TargetLinePerson] P Yellow Path apply skipped: targetController=" + targetControllerChooser.val + " / reason=" + reason);
            return false;
        }

        FreeControllerV3 penisBase = GetOwnPenisBase();
        FreeControllerV3 penisMid = GetOwnPenisMid();
        FreeControllerV3 penisTip = GetOwnPenisTip();

        if (penisBase == null || penisMid == null || penisTip == null)
        {
            if (log)
            {
                LogMessageIfDebug(
                    "[TargetLinePerson] P Yellow Path apply skipped: missing controller" +
                    " / base=" + (penisBase != null) +
                    " / mid=" + (penisMid != null) +
                    " / tip=" + (penisTip != null) +
                    " / reason=" + reason
                );
            }
            return false;
        }

        float advance = pYellowPathAdvance != null ? pYellowPathAdvance.val : 0f;

        Vector3 baseBefore = penisBase.transform.position;
        Vector3 midBefore = penisMid.transform.position;
        Vector3 tipBefore = penisTip.transform.position;

        Vector3 baseTarget;
        Vector3 midTarget;
        Vector3 tipTarget;
        Vector3 baseTan;
        Vector3 midTan;
        Vector3 tipTan;

        SampleYellowPPath(advance, out baseTarget, out baseTan);
        SampleYellowPPath(advance + yellowBaseToMidLength, out midTarget, out midTan);
        SampleYellowPPath(advance + yellowBaseToMidLength + yellowMidToTipLength, out tipTarget, out tipTan);

        RestoreCapturedBodyRootForYellowPath();
        ApplyHipLowerByYellowPathIfNeeded(advance);

        ApplyControllerToYellowPath(penisBase, advance);
        ApplyControllerToYellowPath(penisMid, advance + yellowBaseToMidLength);
        ApplyControllerToYellowPath(penisTip, advance + yellowBaseToMidLength + yellowMidToTipLength);

        RestoreCapturedBodyRootForYellowPath();
        ApplyHipLowerByYellowPathIfNeeded(advance);

        if (log)
        {
            LogMessageIfDebug(
                "[TargetLinePerson] P Yellow Path apply" +
                " / reason=" + reason +
                " / advance=" + advance.ToString("F3") +
                " / total=" + yellowPPathTotalLength.ToString("F3") +
                " / max=" + GetYellowPathAdvanceMax().ToString("F3") +
                " / hipLower=" + GetCurrentYellowHipLower(advance).ToString("F3") +
                " / p01=" + yellowPPathLengths[1].ToString("F3") +
                " / p02=" + yellowPPathLengths[2].ToString("F3") +
                " / p03=" + yellowPPathLengths[3].ToString("F3") +
                " / p04=" + yellowPPathLengths[4].ToString("F3") +
                " / p05=" + yellowPPathLengths[5].ToString("F3") +
                " / baseLen=" + yellowBaseToMidLength.ToString("F3") +
                " / midLen=" + yellowMidToTipLength.ToString("F3") +
                " / baseMove=" + Vector3.Distance(baseBefore, baseTarget).ToString("F3") +
                " / midMove=" + Vector3.Distance(midBefore, midTarget).ToString("F3") +
                " / tipMove=" + Vector3.Distance(tipBefore, tipTarget).ToString("F3")
            );
        }

        return true;
    }

    bool IsPushAutoGDepthTriggerEnabled()
    {
        return pushAutoGDepthTrigger != null &&
            pushAutoGDepthTrigger.val &&
            GetPushAutoMode() != PushModeNone;
    }

    void ResetPushAutoGDepthTriggerState(string reason)
    {
        pushAutoGDepthEnterSince = -1.0f;
        pushAutoGDepthExitSince = -1.0f;
        pushAutoGDepthSuppressUntilExit = false;
        pushAutoGDepthStartDistance = 0.0f;
    }

    bool IsPushAutoGDepthBodyGated()
    {
        return lastGenDepthSampleKnown &&
            lastGenDepthBodyDistance >= 0.0f &&
            lastGenDepthBodyDistance > GetGenBodyGateMaxDistance();
    }

    bool IsPushAutoLieDepthCompensationActive()
    {
        // Mouth PUSH should not inherit genital/anus lie compensation.
        if (GetTargetModeName() == "mouth")
        {
            return false;
        }

        return rideLieActive || IsOwnLiePoseForYellowGuide();
    }

    bool IsPushAutoGDepthEnterCandidate()
    {
        if (!lastGenDepthSampleKnown)
        {
            return false;
        }

        if (IsNearZeroGContact(lastGenDepthRawDepth, lastGenDepthLateral))
        {
            return true;
        }

        return
            lastGenDepthPushEffectiveDepth >= PushAutoGDepthEnterRawDepth &&
            lastGenDepthLateral <= PushAutoGDepthEnterLateralMax &&
            !IsPushAutoGDepthBodyGated();
    }

    bool IsNearZeroGContact(float rawDepth, float lateral)
    {
        return lateral <= PushAutoGDepthEnterLateralMax &&
            rawDepth >= -PushAutoNearZeroBackDepth &&
            !IsPushAutoGDepthBodyGated();
    }

    bool IsPushAutoGDepthExitCandidate()
    {
        if (!lastGenDepthSampleKnown)
        {
            return true;
        }

        bool tipShallow = lastGenDepthPushEffectiveDepth < PushAutoGDepthExitRawDepth;
        float currentDistance = distance != null ? distance.val : pushAutoGDepthStartDistance;
        bool distanceIncreased = currentDistance > pushAutoGDepthStartDistance + PushAutoGDepthExitDistanceDelta;

        return distanceIncreased || tipShallow;
    }

    bool IsPushDepthTargetMode(string targetMode)
    {
        return targetMode == "genital" || targetMode == "anus" || targetMode == "mouth";
    }

    string GetPushAutoGDepthLogTail()
    {
        return " / autoMode=" + GetPushAutoMode() +
            " / rawDepth=" + (lastGenDepthSampleKnown ? lastGenDepthRawDepth.ToString("F3") : "n/a") +
            " / pushDepth=" + (lastGenDepthSampleKnown ? lastGenDepthPushEffectiveDepth.ToString("F3") : "n/a") +
            " / lieDepth=" + (lastGenDepthSampleKnown ? (lastGenDepthPushLieCompensated ? "1" : "0") : "n/a") +
            " / lateral=" + (lastGenDepthSampleKnown ? lastGenDepthLateral.ToString("F3") : "n/a") +
            " / distance=" + (distance != null ? distance.val.ToString("F3") : "n/a") +
            " / startDistance=" + pushAutoGDepthStartDistance.ToString("F3") +
            " / distanceDelta=" + (distance != null ? (distance.val - pushAutoGDepthStartDistance).ToString("F3") : "n/a") +
            " / lateralEnterMax=" + PushAutoGDepthEnterLateralMax.ToString("F3") +
            " / bodyDist=" + (lastGenDepthSampleKnown && lastGenDepthBodyDistance >= 0.0f ? lastGenDepthBodyDistance.ToString("F3") : "n/a") +
            " / percent=" + (lastGenDepthSampleKnown ? lastGenDepthPercent.ToString("F3") : "n/a");
    }

    void UpdatePushAutoGDepthTrigger()
    {
        if (!IsPushAutoGDepthTriggerEnabled())
        {
            ResetPushAutoGDepthTriggerState("disabled");
            return;
        }

        string pushTargetMode = GetTargetModeName();
        if (!IsPushDepthTargetMode(pushTargetMode))
        {
            if (pushPRoutine != null && !pushStopRequested)
            {
                pushStopRequested = true;
                UpdatePushButtonUi();
                DebugLog("[TargetLinePerson] PUSH auto trigger stop / reason=target-not-push-depth / target=" + pushTargetMode);
            }
            ResetPushAutoGDepthTriggerState("target-not-push-depth");
            return;
        }

        float depth;
        float length;
        float percent;
        string depthTargetMode;
        if (!TryGetLiveCurrentPushDepth(out depth, out length, out percent, out depthTargetMode))
        {
            ResetPushAutoGDepthTriggerState("no-push-depth");
            return;
        }

        bool enterCandidate = IsPushAutoGDepthEnterCandidate();
        bool exitCandidate = IsPushAutoGDepthExitCandidate();

        if (pushAutoGDepthSuppressUntilExit)
        {
            if (!enterCandidate || exitCandidate)
            {
                pushAutoGDepthSuppressUntilExit = false;
                pushAutoGDepthEnterSince = -1.0f;
                pushAutoGDepthExitSince = -1.0f;
                DebugLog("[TargetLinePerson] PUSH auto trigger manual suppress cleared" + GetPushAutoGDepthLogTail());
            }
            return;
        }

        if (pushPRoutine != null)
        {
            pushAutoGDepthEnterSince = -1.0f;

            if (exitCandidate)
            {
                if (pushAutoGDepthExitSince < 0.0f)
                {
                    pushAutoGDepthExitSince = Time.time;
                }

                if (Time.time - pushAutoGDepthExitSince >= PushAutoGDepthExitHoldSeconds)
                {
                    if (!pushStopRequested)
                    {
                        pushReleasePIkOnDone = true;
                        pushStopRequested = true;
                        UpdatePushButtonUi();
                        DebugLog("[TargetLinePerson] PUSH auto trigger stop / reason=g-depth-exit" + GetPushAutoGDepthLogTail());
                    }
                    pushAutoGDepthExitSince = -1.0f;
                }
            }
            else
            {
                pushAutoGDepthExitSince = -1.0f;
            }

            return;
        }

        pushAutoGDepthExitSince = -1.0f;

        if (enterCandidate)
        {
            if (pushAutoGDepthEnterSince < 0.0f)
            {
                pushAutoGDepthEnterSince = Time.time;
            }

            if (Time.time - pushAutoGDepthEnterSince >= PushAutoGDepthEnterHoldSeconds)
            {
                pushAutoGDepthEnterSince = -1.0f;
                pushStopRequested = false;
                pushReleasePIkOnDone = false;
                pushAutoGDepthStartDistance = distance != null ? distance.val : 0.0f;
                DebugLog("[TargetLinePerson] PUSH auto trigger start / reason=g-depth-enter" + GetPushAutoGDepthLogTail());
                pushPRoutine = StartCoroutine(PushPCoroutine());
                UpdatePushButtonUi();
            }
        }
        else
        {
            pushAutoGDepthEnterSince = -1.0f;
        }
    }

    void ActionPushP()
    {
        if (pushPRoutine != null)
        {
            if (IsPushAutoGDepthTriggerEnabled())
            {
                pushAutoGDepthSuppressUntilExit = true;
                pushAutoGDepthEnterSince = -1.0f;
                pushAutoGDepthExitSince = -1.0f;
            }

            pushStopRequested = true;
            UpdatePushButtonUi();
            DebugLog("[TargetLinePerson] PUSH stop requested");
            return;
        }

        pushAutoGDepthSuppressUntilExit = false;
        pushStopRequested = false;
        pushReleasePIkOnDone = false;
        pushPRoutine = StartCoroutine(PushPCoroutine());
        UpdatePushButtonUi();
    }

    void ActionPMidGAlign()
    {
        if (pushPRoutine != null)
        {
            DebugLog("[TargetLinePerson] P Mid G Align skipped / reason=PUSH running");
            return;
        }

        FreeControllerV3 penisBase = GetOwnPenisBase();
        FreeControllerV3 penisMid = GetOwnPenisMid();
        FreeControllerV3 penisTip = GetOwnPenisTip();

        if (penisMid == null || penisTip == null)
        {
            DebugLog(
                "[TargetLinePerson] P Mid G Align skipped / reason=missing P controller" +
                " / base=" + (penisBase != null ? "1" : "0") +
                " / mid=" + (penisMid != null ? "1" : "0") +
                " / tip=" + (penisTip != null ? "1" : "0")
            );
            return;
        }

        Vector3 origin;
        Vector3 dir;
        float length;
        if (!TryGetLiveGenitalInsideLine(out origin, out dir, out length))
        {
            DebugLog("[TargetLinePerson] P Mid G Align skipped / reason=no genital G line");
            return;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            DebugLog("[TargetLinePerson] P Mid G Align skipped / reason=bad G direction");
            return;
        }

        dir.Normalize();

        float midDepth;
        float midLateral;
        Vector3 midCorrection = GetLateralCorrectionToInsideLine(penisMid.transform.position, origin, dir, out midDepth, out midLateral);

        float tipDepth;
        float tipLateral;
        Vector3 tipCorrection = GetLateralCorrectionToInsideLine(penisTip.transform.position, origin, dir, out tipDepth, out tipLateral);

        Vector3 baseCorrection = Vector3.zero;
        if (penisBase != null)
        {
            baseCorrection = Vector3.ClampMagnitude(midCorrection * PMidGAlignBaseFollowScale, PMidGAlignBaseMaxMove);
            ApplyControllerPositionOffsetIfChanged(penisBase, baseCorrection);
        }

        ApplyControllerPositionOffsetIfChanged(penisMid, midCorrection);
        ApplyControllerPositionOffsetIfChanged(penisTip, tipCorrection);
        pMidAxisAssistApplied = true;

        DebugLog(
            "[TargetLinePerson] P Mid G Align" +
            " / midMove=" + midCorrection.magnitude.ToString("F3") +
            " / tipMove=" + tipCorrection.magnitude.ToString("F3") +
            " / baseMove=" + baseCorrection.magnitude.ToString("F3") +
            " / midDepth=" + midDepth.ToString("F3") +
            " / tipDepth=" + tipDepth.ToString("F3") +
            " / midLat=" + midLateral.ToString("F3") +
            " / tipLat=" + tipLateral.ToString("F3")
        );
    }

    Vector3 GetLateralCorrectionToInsideLine(Vector3 position, Vector3 origin, Vector3 dir, out float depth, out float lateralDistance)
    {
        depth = Vector3.Dot(position - origin, dir);
        Vector3 closest = origin + dir * depth;
        Vector3 correction = closest - position;
        lateralDistance = correction.magnitude;
        return correction;
    }

    void CapturePushButtonUi()
    {
        if (pushButton == null || pushButton.button == null || pushButtonUiCaptured)
        {
            return;
        }

        pushButtonNormalColors = pushButton.button.colors;
        pushButtonUiCaptured = true;
        UpdatePushButtonUi();
    }

    void UpdatePushButtonUi()
    {
        if (pushButton == null || pushButton.button == null)
        {
            return;
        }

        if (!pushButtonUiCaptured)
        {
            pushButtonNormalColors = pushButton.button.colors;
            pushButtonUiCaptured = true;
        }

        Text text = pushButton.button.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = pushAutoLoopActive ? (pushStopRequested ? "PUSH..." : "PUSH STOP") : "PUSH";
        }

        if (pushAutoLoopActive)
        {
            ColorBlock colors = pushButtonNormalColors;
            if (pushStopRequested)
            {
                colors.normalColor = new Color(1.0f, 0.86f, 0.22f, 1.0f);
                colors.highlightedColor = new Color(1.0f, 0.92f, 0.38f, 1.0f);
                colors.pressedColor = new Color(0.90f, 0.70f, 0.12f, 1.0f);
            }
            else
            {
                colors.normalColor = new Color(1.0f, 0.35f, 0.15f, 1.0f);
                colors.highlightedColor = new Color(1.0f, 0.48f, 0.25f, 1.0f);
                colors.pressedColor = new Color(0.85f, 0.22f, 0.10f, 1.0f);
            }
            pushButton.button.colors = colors;
        }
        else if (pushButtonUiCaptured)
        {
            pushButton.button.colors = pushButtonNormalColors;
        }
    }

    IEnumerator PushPCoroutine()
    {
        FreeControllerV3 penisBase = GetOwnPenisBase();
        FreeControllerV3 penisMid = GetOwnPenisMid();
        FreeControllerV3 penisTip = GetOwnPenisTip();

        if (penisBase == null || penisMid == null || penisTip == null)
        {
            pushReleasePIkOnDone = false;
            LogMessageIfDebug(
                "[TargetLinePerson] PUSH skipped / reason=missing-controller" +
                " / base=" + (penisBase != null) +
                " / mid=" + (penisMid != null) +
                " / tip=" + (penisTip != null)
            );
            pushPRoutine = null;
            pushAutoLoopActive = false;
            UpdatePushButtonUi();
            yield break;
        }

        if (isAvoidMoving)
        {
            pushReleasePIkOnDone = false;
            LogMessageIfDebug("[TargetLinePerson] PUSH skipped / reason=avoid-moving");
            pushPRoutine = null;
            pushAutoLoopActive = false;
            UpdatePushButtonUi();
            yield break;
        }

        string pushTargetMode = GetTargetModeName();
        if (!IsPushDepthTargetMode(pushTargetMode))
        {
            pushReleasePIkOnDone = false;
            LogMessageIfDebug("[TargetLinePerson] PUSH skipped / reason=target-not-push-depth / target=" + pushTargetMode);
            pushPRoutine = null;
            pushAutoLoopActive = false;
            UpdatePushButtonUi();
            yield break;
        }

        Vector3 origin;
        Vector3 dir;
        float length;
        string depthTargetMode;
        if (!TryGetLiveCurrentInsideLine(out origin, out dir, out length, out depthTargetMode))
        {
            pushReleasePIkOnDone = false;
            LogMessageIfDebug("[TargetLinePerson] PUSH skipped / reason=no-push-depth-line / target=" + pushTargetMode);
            pushPRoutine = null;
            pushAutoLoopActive = false;
            UpdatePushButtonUi();
            yield break;
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            pushReleasePIkOnDone = false;
            LogMessageIfDebug("[TargetLinePerson] PUSH skipped / reason=bad-push-depth-dir / target=" + pushTargetMode);
            pushPRoutine = null;
            pushAutoLoopActive = false;
            UpdatePushButtonUi();
            yield break;
        }

        dir.Normalize();
        length = Mathf.Max(0.0001f, length);

        Vector3 tipFromOrigin = penisTip.transform.position - origin;
        float rawDepth = Vector3.Dot(tipFromOrigin, dir);
        bool pushLieDepth = IsPushAutoLieDepthCompensationActive();
        float pushDepth = pushLieDepth ? Mathf.Abs(rawDepth) : rawDepth;
        Vector3 closestOnAxis = origin + dir * rawDepth;
        float lateral = Vector3.Distance(penisTip.transform.position, closestOnAxis);
        pushStartRawDepth = rawDepth;
        pushStartLateral = lateral;

        CapturePushPState(penisBase, penisMid, penisTip);
        pushActiveDir = dir;
        pushCurrentMoveDistance = 0.0f;
        pushTargetMoveDistance = 0.0f;
        pushLastPressTime = Time.time;
        pushAutoLoopActive = GetPushAutoMode() != PushModeNone;
        UpdatePushButtonUi();

        DebugLog(
            "[TargetLinePerson] PUSH start" +
            " / mode=" + (pushAutoLoopActive ? "auto-loop" : "single") +
            " / autoMode=" + GetPushAutoMode() +
            " / target=" + depthTargetMode +
            " / scale=" + GetPushDepthScale().ToString("F2") +
            " / add=" + GetPushPAddDistance().ToString("F3") +
            " / maxMove=" + GetPushPMaxMoveDistance().ToString("F3") +
            " / rawDepth=" + rawDepth.ToString("F3") +
            " / pushDepth=" + pushDepth.ToString("F3") +
            " / lieDepth=" + (pushLieDepth ? "1" : "0") +
            " / lateral=" + lateral.ToString("F3") +
            " / origin=(" + FormatVector3(origin) + ")" +
            " / dir=(" + FormatVector3(dir) + ")"
        );

        bool keepLooping = true;
        while (!pushStopRequested && keepLooping)
        {
            ConfigurePushMode();
            pushCurrentMoveDistance = 0.0f;
            pushTargetMoveDistance = 0.0f;
            pushLastPressTime = Time.time;
            AddPushPTargetDistance("cycle");

            if (IsDebugViewEnabled())
            {
                LogMessageIfDebug(
                    "[TargetLinePerson] PUSH cycle" +
                    " / autoMode=" + pushResolvedMode +
                    " / loop=" + pushAutoLoopActive +
                    " / targetMove=" + pushTargetMoveDistance.ToString("F3") +
                    " / spiralStart=" + pushModeSpiralStartAngle.ToString("F1") +
                    " / spiralAngle=" + pushModeSpiralAngle.ToString("F1")
                );
            }

            yield return StartCoroutine(MovePushPToDistance(penisBase, penisMid, penisTip, pushTargetMoveDistance, pushModeFollowSpeed));

            if (pushStopRequested)
            {
                break;
            }

            if (pushResolvedMode == PushModeAutoDeepStop)
            {
                yield return StartCoroutine(DeepStopWobblePushP(penisBase, penisMid, penisTip));
                if (pushStopRequested)
                {
                    break;
                }
            }

            yield return StartCoroutine(ReturnPushPToHome(penisBase, penisMid, penisTip, pushModeReturnSeconds));

            keepLooping = pushAutoLoopActive;
        }

        yield return StartCoroutine(ReturnPushPToHome(penisBase, penisMid, penisTip, pushModeReturnSeconds));

        RestorePushPState(penisBase, penisMid, penisTip);
        ReleasePushPIkOnDoneIfNeeded(penisBase, penisMid, penisTip, "g-depth-exit");
        pushPRoutine = null;
        pushStopRequested = false;
        pushAutoLoopActive = false;
        UpdatePushButtonUi();
        DebugLog("[TargetLinePerson] PUSH done / restored=1");
    }

    void AddPushPTargetDistance(string reason)
    {
        if (pushPRoutine == null && reason != "start" && reason != "cycle")
        {
            return;
        }

        float add = GetPushPAddDistance();
        if (IsNearZeroGContact(pushStartRawDepth, pushStartLateral))
        {
            float guideAdd = Mathf.Clamp(PushNearZeroGuideDepth - Mathf.Max(0.0f, pushStartRawDepth), 0.0f, PushNearZeroGuideDepth);
            add = Mathf.Max(add, guideAdd);
        }
        float before = pushTargetMoveDistance;
        float maxMove = GetPushPMaxMoveDistance();
        pushTargetMoveDistance = Mathf.Min(pushTargetMoveDistance + add, maxMove);
        bool capped = pushTargetMoveDistance >= maxMove - 0.0001f && before + add > maxMove + 0.0001f;
        pushLastPressTime = Time.time;

        DebugLog(
            "[TargetLinePerson] PUSH target" +
            " / reason=" + reason +
            " / autoMode=" + pushResolvedMode +
            " / scale=" + GetPushDepthScale().ToString("F2") +
            " / add=" + add.ToString("F3") +
            " / nearGContact=" + (IsNearZeroGContact(pushStartRawDepth, pushStartLateral) ? "1" : "0") +
            " / currentMove=" + pushCurrentMoveDistance.ToString("F3") +
            " / targetMove=" + pushTargetMoveDistance.ToString("F3") +
            " / maxMove=" + maxMove.ToString("F3") +
            " / capped=" + capped
        );
    }

    IEnumerator MovePushPToDistance(FreeControllerV3 penisBase, FreeControllerV3 penisMid, FreeControllerV3 penisTip, float targetDistance, float speed)
    {
        if (pushModeLinearSlow)
        {
            speed = Mathf.Max(0.005f, speed);
            while (!pushStopRequested && Mathf.Abs(targetDistance - pushCurrentMoveDistance) > 0.001f)
            {
                pushCurrentMoveDistance = Mathf.MoveTowards(pushCurrentMoveDistance, targetDistance, speed * Time.deltaTime);
                ApplyPushPDelta(penisBase, penisMid, penisTip, pushActiveDir * pushCurrentMoveDistance);
                yield return null;
            }

            if (!pushStopRequested)
            {
                pushCurrentMoveDistance = targetDistance;
                ApplyPushPDelta(penisBase, penisMid, penisTip, pushActiveDir * pushCurrentMoveDistance);
            }
            yield break;
        }

        speed = Mathf.Max(0.01f, speed);
        while (!pushStopRequested && Mathf.Abs(targetDistance - pushCurrentMoveDistance) > 0.001f)
        {
            float t = 1.0f - Mathf.Exp(-speed * Time.deltaTime);
            pushCurrentMoveDistance = Mathf.Lerp(pushCurrentMoveDistance, targetDistance, t);
            ApplyPushPDelta(penisBase, penisMid, penisTip, pushActiveDir * pushCurrentMoveDistance);
            yield return null;
        }

        if (!pushStopRequested)
        {
            pushCurrentMoveDistance = targetDistance;
            ApplyPushPDelta(penisBase, penisMid, penisTip, pushActiveDir * pushCurrentMoveDistance);
        }
    }

    IEnumerator ReturnPushPToHome(FreeControllerV3 penisBase, FreeControllerV3 penisMid, FreeControllerV3 penisTip, float seconds)
    {
        if (pushModeLinearSlow)
        {
            float speed = Mathf.Max(0.005f, pushModeFollowSpeed);
            while (pushCurrentMoveDistance > 0.001f)
            {
                pushCurrentMoveDistance = Mathf.MoveTowards(pushCurrentMoveDistance, 0.0f, speed * Time.deltaTime);
                ApplyPushPDelta(penisBase, penisMid, penisTip, pushActiveDir * pushCurrentMoveDistance);
                yield return null;
            }

            pushCurrentMoveDistance = 0.0f;
            ApplyPushPDelta(penisBase, penisMid, penisTip, Vector3.zero);
            yield break;
        }

        float startDistance = pushCurrentMoveDistance;
        float elapsed = 0.0f;
        seconds = Mathf.Max(0.01f, seconds);

        while (elapsed < seconds)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / seconds);
            float eased = t * t * (3.0f - 2.0f * t);
            pushCurrentMoveDistance = Mathf.Lerp(startDistance, 0.0f, eased);
            ApplyPushPDelta(penisBase, penisMid, penisTip, pushActiveDir * pushCurrentMoveDistance);
            yield return null;
        }

        pushCurrentMoveDistance = 0.0f;
        ApplyPushPDelta(penisBase, penisMid, penisTip, Vector3.zero);
    }

    IEnumerator DeepStopWobblePushP(FreeControllerV3 penisBase, FreeControllerV3 penisMid, FreeControllerV3 penisTip)
    {
        float elapsed = 0.0f;
        float baseDistance = pushTargetMoveDistance;
        float wobble = Mathf.Clamp(GetPushPMaxMoveDistance() * PushPDeepStopWobbleScale, 0.006f, 0.030f);

        while (!pushStopRequested && elapsed < PushPDeepStopSeconds)
        {
            elapsed += Time.deltaTime;
            float pulse = (Mathf.Sin(elapsed * PushPDeepStopWobbleSpeed) + 1.0f) * 0.5f;
            pushCurrentMoveDistance = Mathf.Max(0.0f, baseDistance - wobble * pulse);
            ApplyPushPDelta(penisBase, penisMid, penisTip, pushActiveDir * pushCurrentMoveDistance);
            yield return null;
        }

        if (!pushStopRequested)
        {
            pushCurrentMoveDistance = baseDistance;
            ApplyPushPDelta(penisBase, penisMid, penisTip, pushActiveDir * pushCurrentMoveDistance);
        }
    }

    float GetPushDepthScale()
    {
        return pushDepthScale != null ? Mathf.Clamp(pushDepthScale.val, PushPDepthScaleMin, PushPDepthScaleMax) : PushPDepthScaleDefault;
    }

    float GetPushPAddDistance()
    {
        return Mathf.Max(PushPMinMoveDistance, GetGenDepthMax() * (GetPushDepthScale() - 1.0f));
    }

    float GetPushPMaxMoveDistance()
    {
        float maxMove = GetPushPAddDistance();
        if (IsNearZeroGContact(pushStartRawDepth, pushStartLateral))
        {
            maxMove = Mathf.Max(maxMove, PushNearZeroGuideDepth);
        }
        return maxMove;
    }

    string GetPushAutoMode()
    {
        return pushAutoMode != null && !string.IsNullOrEmpty(pushAutoMode.val) ? pushAutoMode.val : PushModeNone;
    }

    string ResolvePushAutoMode()
    {
        string mode = GetPushAutoMode();
        if (mode != PushModeAutoRandom)
        {
            return mode;
        }

        string[] modes = new string[]
        {
            PushModeAutoLine,
            PushModeAutoLineSlow,
            PushModeAutoLineFast,
            PushModeAutoSpiral,
            PushModeAutoDeepStop
        };
        int index = UnityEngine.Random.Range(0, modes.Length);
        return modes[index];
    }

    void ConfigurePushMode()
    {
        pushResolvedMode = ResolvePushAutoMode();
        pushModeFollowSpeed = PushPFollowSpeed;
        pushModeReturnSeconds = PushPReturnSeconds;
        pushModeHoldSeconds = PushPHoldSeconds;
        pushModeLinearSlow = false;
        pushModeSpiralAngle = 0.0f;
        pushModeSpiralStartAngle = 0.0f;

        if (pushResolvedMode == PushModeNone)
        {
            pushModeFollowSpeed = 15.0f;
            pushModeReturnSeconds = 0.16f;
            pushModeHoldSeconds = 0.0f;
        }
        else if (pushResolvedMode == PushModeAutoLineSlow)
        {
            // Slow is intentionally different from the normal exponential line move:
            // constant-speed in/out, no quick ease-in, no fast return.
            pushModeLinearSlow = true;
            pushModeFollowSpeed = PushPLineSlowLinearSpeed;
            pushModeReturnSeconds = 0.0f;
            pushModeHoldSeconds = 0.0f;
        }
        else if (pushResolvedMode == PushModeAutoLineFast)
        {
            pushModeFollowSpeed = 15.0f;
            pushModeReturnSeconds = 0.16f;
            pushModeHoldSeconds = 0.0f;
        }
        else if (pushResolvedMode == PushModeAutoSpiral)
        {
            pushModeFollowSpeed = PushPFollowSpeed;
            pushModeReturnSeconds = PushPReturnSeconds;
            pushModeHoldSeconds = PushPHoldSeconds;
            float spiralSign = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
            pushModeSpiralStartAngle = UnityEngine.Random.Range(PushPSpiralStartMinDegrees, PushPSpiralStartMaxDegrees);
            pushModeSpiralAngle = UnityEngine.Random.Range(PushPSpiralDegreesMin, PushPSpiralDegreesMax) * spiralSign;
        }
        else if (pushResolvedMode == PushModeAutoDeepStop)
        {
            pushModeFollowSpeed = PushPFollowSpeed;
            pushModeReturnSeconds = PushPReturnSeconds;
            pushModeHoldSeconds = 0.0f;
        }
    }

    void ApplyPushPDelta(FreeControllerV3 penisBase, FreeControllerV3 penisMid, FreeControllerV3 penisTip, Vector3 delta)
    {
        if (penisBase == null || penisMid == null || penisTip == null)
        {
            return;
        }

        float maxMove = Mathf.Max(0.0001f, GetPushPMaxMoveDistance());
        float moveRatio = Mathf.Clamp01(delta.magnitude / maxMove);
        float twist = pushModeSpiralAngle * moveRatio + pushModeSpiralStartAngle * Mathf.Sin(moveRatio * Mathf.PI);

        ApplyPushPControllerDelta(penisBase, pushSavedPBasePosition, pushSavedPBaseRotation, delta, twist * 0.75f);
        ApplyPushPControllerDelta(penisMid, pushSavedPMidPosition, pushSavedPMidRotation, delta, twist * 1.20f);
        ApplyPushPControllerDelta(penisTip, pushSavedPTipPosition, pushSavedPTipRotation, delta, twist * 1.80f);
    }

    void ApplyPushPControllerDelta(FreeControllerV3 fc, Vector3 basePosition, Quaternion baseRotation, Vector3 delta, float twistDegrees)
    {
        if (fc == null)
        {
            return;
        }

        Vector3 pos = basePosition + delta;
        Quaternion rot = baseRotation;
        if (Mathf.Abs(twistDegrees) > 0.0001f && pushActiveDir.sqrMagnitude > 0.0001f)
        {
            rot = Quaternion.AngleAxis(twistDegrees, pushActiveDir.normalized) * baseRotation;
        }

        if (fc.currentPositionState != FreeControllerV3.PositionState.On)
        {
            fc.currentPositionState = FreeControllerV3.PositionState.On;
        }
        if (fc.currentRotationState != FreeControllerV3.RotationState.On)
        {
            fc.currentRotationState = FreeControllerV3.RotationState.On;
        }

        if ((fc.transform.position - pos).sqrMagnitude > 0.00000001f)
        {
            fc.transform.position = pos;
        }
        if (Quaternion.Angle(fc.transform.rotation, rot) > 0.001f)
        {
            fc.transform.rotation = rot;
        }

        if (fc.control != null)
        {
            if ((fc.control.position - pos).sqrMagnitude > 0.00000001f)
            {
                fc.control.position = pos;
            }
            if (Quaternion.Angle(fc.control.rotation, rot) > 0.001f)
            {
                fc.control.rotation = rot;
            }
        }
    }

    void CapturePushPState(FreeControllerV3 baseFc, FreeControllerV3 midFc, FreeControllerV3 tipFc)
    {
        if (baseFc == null || midFc == null || tipFc == null)
        {
            pushPStateCaptured = false;
            return;
        }

        pushSavedPBasePosition = baseFc.transform.position;
        pushSavedPMidPosition = midFc.transform.position;
        pushSavedPTipPosition = tipFc.transform.position;
        pushSavedPBaseRotation = baseFc.transform.rotation;
        pushSavedPMidRotation = midFc.transform.rotation;
        pushSavedPTipRotation = tipFc.transform.rotation;
        pushSavedPBasePositionState = baseFc.currentPositionState;
        pushSavedPMidPositionState = midFc.currentPositionState;
        pushSavedPTipPositionState = tipFc.currentPositionState;
        pushSavedPBaseRotationState = baseFc.currentRotationState;
        pushSavedPMidRotationState = midFc.currentRotationState;
        pushSavedPTipRotationState = tipFc.currentRotationState;
        pushPStateCaptured = true;
    }

    void RestorePushPState(FreeControllerV3 baseFc, FreeControllerV3 midFc, FreeControllerV3 tipFc)
    {
        if (!pushPStateCaptured || baseFc == null || midFc == null || tipFc == null)
        {
            pushPStateCaptured = false;
            return;
        }

        RestorePushPController(baseFc, pushSavedPBasePosition, pushSavedPBaseRotation, pushSavedPBasePositionState, pushSavedPBaseRotationState);
        RestorePushPController(midFc, pushSavedPMidPosition, pushSavedPMidRotation, pushSavedPMidPositionState, pushSavedPMidRotationState);
        RestorePushPController(tipFc, pushSavedPTipPosition, pushSavedPTipRotation, pushSavedPTipPositionState, pushSavedPTipRotationState);
        pushPStateCaptured = false;
    }

    void RestorePushPController(FreeControllerV3 fc, Vector3 position, Quaternion rotation, FreeControllerV3.PositionState positionState, FreeControllerV3.RotationState rotationState)
    {
        if (fc == null)
        {
            return;
        }

        fc.currentPositionState = FreeControllerV3.PositionState.On;
        fc.currentRotationState = FreeControllerV3.RotationState.On;
        fc.transform.position = position;
        fc.transform.rotation = rotation;

        if (fc.control != null)
        {
            fc.control.position = position;
            fc.control.rotation = rotation;
        }

        fc.currentPositionState = positionState;
        fc.currentRotationState = rotationState;
    }

    void ReleasePushPIkOnDoneIfNeeded(FreeControllerV3 baseFc, FreeControllerV3 midFc, FreeControllerV3 tipFc, string reason)
    {
        if (!pushReleasePIkOnDone)
        {
            return;
        }

        ReleasePYellowController(baseFc);
        ReleasePYellowController(midFc);
        ReleasePYellowController(tipFc);
        pushReleasePIkOnDone = false;

        DebugLog("[TargetLinePerson] PUSH auto trigger P IK release / reason=" + reason);
    }

    float GetCurrentYellowHipLower(float advance)
    {
        if (!hasYellowPPath)
        {
            return 0f;
        }

        Vector3 samplePos;
        Vector3 sampleTan;
        SampleYellowPPath(advance, out samplePos, out sampleTan);
        float scale = hipLowerByYellowScale != null ? hipLowerByYellowScale.val : 1.0f;
        return Mathf.Min(0f, samplePos.y - yellowPPathPoints[0].y) * scale;
    }

    void ApplyHipLowerByYellowPathIfNeeded(float advance)
    {
        if (hipLowerByYellowPath == null || !hipLowerByYellowPath.val)
        {
            return;
        }

        if (!hasCapturedBodyHip || !hasYellowPPath)
        {
            return;
        }

        FreeControllerV3 ownHip = GetOwnHip();
        if (ownHip == null)
        {
            return;
        }

        Vector3 samplePos;
        Vector3 sampleTan;
        SampleYellowPPath(advance, out samplePos, out sampleTan);

        float scale = hipLowerByYellowScale != null ? hipLowerByYellowScale.val : 1.0f;

        // Test only: use the yellow path's downward dip as a hipControl lowering offset.
        // Root stays locked; only the body hip Y is lowered.
        // Positive Y changes are ignored so the hip does not lift upward.
        float lower = Mathf.Min(0f, samplePos.y - yellowPPathPoints[0].y) * scale;

        Vector3 hipPos = capturedBodyHipPosition;
        hipPos.y += lower;

        ownHip.currentPositionState = FreeControllerV3.PositionState.On;

        if (ownHip.control != null)
        {
            ownHip.control.position = hipPos;
        }

        if (ownHip.transform != null)
        {
            ownHip.transform.position = hipPos;
        }
    }

    void ApplyControllerToYellowPath(FreeControllerV3 fc, float distance)
    {
        if (fc == null)
        {
            return;
        }

        Vector3 pos;
        Vector3 tangent;
        SampleYellowPPath(distance, out pos, out tangent);

        Quaternion rot = GetYellowPPathRotation(tangent);

        fc.currentPositionState = FreeControllerV3.PositionState.On;
        fc.currentRotationState = FreeControllerV3.RotationState.On;

        // Keep transform and control in sync.
        fc.transform.position = pos;
        fc.transform.rotation = rot;

        if (fc.control != null)
        {
            fc.control.position = pos;
            fc.control.rotation = rot;
        }
    }

    void ApplyControllerToYellowPathRelative(FreeControllerV3 fc, Vector3 pos, Vector3 tangent)
    {
        if (fc == null)
        {
            return;
        }

        Quaternion rot = GetYellowPPathRotation(tangent);

        fc.currentPositionState = FreeControllerV3.PositionState.On;
        fc.currentRotationState = FreeControllerV3.RotationState.On;

        fc.transform.position = pos;
        fc.transform.rotation = rot;

        if (fc.control != null)
        {
            fc.control.position = pos;
            fc.control.rotation = rot;
        }
    }


    void SampleYellowPPath(float distance, out Vector3 pos, out Vector3 tangent)
    {
        if (!hasYellowPPath)
        {
            pos = Vector3.zero;
            tangent = Vector3.forward;
            return;
        }

        distance = Mathf.Clamp(distance, 0f, yellowPPathTotalLength);

        for (int i = 1; i < YellowPPathPointCount; i++)
        {
            if (distance <= yellowPPathLengths[i])
            {
                float prevLen = yellowPPathLengths[i - 1];
                float segLen = yellowPPathLengths[i] - prevLen;
                Vector3 a = yellowPPathPoints[i - 1];
                Vector3 b = yellowPPathPoints[i];

                if (segLen < 0.0001f)
                {
                    pos = b;
                    tangent = (b - a).sqrMagnitude > 0.0001f ? (b - a).normalized : Vector3.forward;
                    return;
                }

                float t = Mathf.Clamp01((distance - prevLen) / segLen);
                pos = Vector3.Lerp(a, b, t);
                tangent = (b - a).normalized;
                return;
            }
        }

        pos = yellowPPathPoints[YellowPPathPointCount - 1];
        tangent = yellowPPathPoints[YellowPPathPointCount - 1] - yellowPPathPoints[YellowPPathPointCount - 2];
        if (tangent.sqrMagnitude < 0.0001f)
        {
            tangent = Vector3.forward;
        }
        else
        {
            tangent.Normalize();
        }
    }

    Quaternion GetYellowPPathRotation(Vector3 tangent)
    {
        if (tangent.sqrMagnitude < 0.0001f)
        {
            tangent = Vector3.forward;
        }
        tangent.Normalize();

        FreeControllerV3 penisBase = GetOwnPenisBase();
        Vector3 up = Vector3.up;

        if (penisBase != null)
        {
            up = Vector3.ProjectOnPlane(penisBase.transform.up, tangent);
            if (up.sqrMagnitude < 0.0001f)
            {
                up = Vector3.ProjectOnPlane(penisBase.transform.right, tangent);
            }
        }

        if (up.sqrMagnitude < 0.0001f)
        {
            up = Vector3.up;
        }

        return Quaternion.LookRotation(tangent, up.normalized);
    }

    void OnDestroy()
    {
        if (pushPRoutine != null)
        {
            StopCoroutine(pushPRoutine);
            pushPRoutine = null;
        }
        pushStopRequested = false;
        pushAutoLoopActive = false;
        UpdatePushButtonUi();
        if (pushPStateCaptured)
        {
            RestorePushPState(GetOwnPenisBase(), GetOwnPenisMid(), GetOwnPenisTip());
            ReleasePushPIkOnDoneIfNeeded(GetOwnPenisBase(), GetOwnPenisMid(), GetOwnPenisTip(), "destroy");
        }
        pushReleasePIkOnDone = false;

        if (delayedLineLockRoutine != null)
        {
            StopCoroutine(delayedLineLockRoutine);
            delayedLineLockRoutine = null;
        }
        if (delayedInsideReactionRoutine != null)
        {
            StopCoroutine(delayedInsideReactionRoutine);
            delayedInsideReactionRoutine = null;
        }
        if (forwardLineObj != null)
        {
            Destroy(forwardLineObj);
        }

        if (moveLineObj != null)
        {
            Destroy(moveLineObj);
        }

        if (penisPathLineObj != null)
        {
            Destroy(penisPathLineObj);
        }

        if (bendMarkerLineObj != null)
        {
            Destroy(bendMarkerLineObj);
        }

        if (gDepthGuideLineObj != null)
        {
            Destroy(gDepthGuideLineObj);
        }

        if (genDepthHudBackObj != null)
        {
            Destroy(genDepthHudBackObj);
        }

        if (genDepthHudFillObj != null)
        {
            Destroy(genDepthHudFillObj);
        }

        if (genDepthHudMarkerObj != null)
        {
            Destroy(genDepthHudMarkerObj);
        }

        if (genDepthHudBottomMarkerObj != null)
        {
            Destroy(genDepthHudBottomMarkerObj);
        }

        if (genDepthHudPeakObj != null)
        {
            Destroy(genDepthHudPeakObj);
        }

        if (genDepthHudGContactDotObj != null)
        {
            Destroy(genDepthHudGContactDotObj);
        }

        if (anusDepthHudBackObj != null)
        {
            Destroy(anusDepthHudBackObj);
        }

        if (anusDepthHudFillObj != null)
        {
            Destroy(anusDepthHudFillObj);
        }

        if (anusDepthHudMarkerObj != null)
        {
            Destroy(anusDepthHudMarkerObj);
        }

        if (anusDepthHudStarObj != null)
        {
            Destroy(anusDepthHudStarObj);
        }

        if (genDepthHudBackMaterial != null)
        {
            Destroy(genDepthHudBackMaterial);
        }

        if (genDepthHudFillMaterial != null)
        {
            Destroy(genDepthHudFillMaterial);
        }

        if (genDepthHudMarkerMaterial != null)
        {
            Destroy(genDepthHudMarkerMaterial);
        }

        if (genDepthHudPeakMaterial != null)
        {
            Destroy(genDepthHudPeakMaterial);
        }

        if (genDepthHudGContactDotMaterial != null)
        {
            Destroy(genDepthHudGContactDotMaterial);
        }

        if (anusDepthHudBackMaterial != null)
        {
            Destroy(anusDepthHudBackMaterial);
        }

        if (anusDepthHudFillMaterial != null)
        {
            Destroy(anusDepthHudFillMaterial);
        }

        if (anusDepthHudMarkerMaterial != null)
        {
            Destroy(anusDepthHudMarkerMaterial);
        }

        if (anusDepthHudStarMaterial != null)
        {
            Destroy(anusDepthHudStarMaterial);
        }

        ClearGenDepthBurstParticles();

        if (genDepthBurstMaterials != null)
        {
            for (int i = 0; i < genDepthBurstMaterials.Length; i++)
            {
                if (genDepthBurstMaterials[i] != null)
                {
                    Destroy(genDepthBurstMaterials[i]);
                }
            }
        }
    }
}
