// V117_CHEST_HOLD_GRASP_FIX_DEBUG_LOGS_BUILD 2026-06-30: Based on v116 FIX. Keeps Front/Back Chest Hold L/R Hand Grasp boost (+0.35) with about-1-second delayed restore, but moves the Grasp diagnostic logs behind Debug Log so normal use stays quiet.
// V116_FRONT_BACK_LR_GRASP_HALF_DELAYED_RESTORE_BUILD 2026-06-30: Based on v115. Applies the same L/R Hand Grasp boost (+0.35) and about-1-second delayed restore to Chest Hold Back as well as Front. Back route keeps existing step2/step3, Wrist In, Wrist In2, near-wrist skip, and nipple follow behavior unchanged.
// V115_FRONT_LR_GRASP_HALF_DELAYED_RESTORE_BUILD 2026-06-30: Based on v114. Keeps Front Chest Hold L/R Hand Grasp boost at +0.35, but restores the original L/R grasp values automatically about 1 second after the boost. Release/defaults also restore immediately if still boosted.
// V114_FRONT_LR_GRASP_HALF_RESTORE_ON_RELEASE_BUILD 2026-06-30: Based on v113. Reduces Front Chest Hold L/R Hand Grasp boost from +0.70 to +0.35, keeps the grasp during the hold, and restores the original L/R grasp values only on Release/Target Release/Self IK Defaults/Self Load User Defaults. No wrist/final delayed restore.
// V113_FRONT_LR_GRASP_VERY_STRONG_WATCH_NO_RESTORE_BUILD 2026-06-30: Based on v112. For verification, increases Front Chest Hold L/R Hand Grasp boost to +0.70 so original 0.10 becomes 0.80 and visual change should be obvious. Keeps no automatic restore and post-apply watch logs.
// V112_FRONT_LR_GRASP_STRONG_WATCH_NO_RESTORE_BUILD 2026-06-30: Based on v111. For visual verification, increases Front Chest Hold L/R Hand Grasp boost from +0.10 to +0.40 and adds a short post-apply watch log to confirm whether another VaM morph/pose/plugin overwrites the value after TargetGrabber writes it. Still no automatic restore and no repeated stacking.
// V111_FRONT_LR_GRASP_ENSURE_NO_RESTORE_BUILD 2026-06-30: Based on v110. Fixes noRestore carry guard: each new Front Chest Hold Grab re-checks the current L/R Hand Grasp value and re-applies the desired boosted value if VaM/defaults/other plugins reset it. Still no automatic restore and no repeated stacking.
// V110_FRONT_LR_GRASP_NO_RESTORE_BUILD 2026-06-30: Based on v109. Chest Hold Front applies both Left Hand Grasp and Right Hand Grasp after step2. Temporary grasp boost is not automatically restored; Release/Target Release carry boosted state to avoid repeated stacking, while Self IK Defaults/Self Load User Defaults only clear internal state.
// V109_FRONT_LEFT_GRASP_KEEP_ACROSS_GRAB_BUILD 2026-06-30: Based on v108. Do not restore Left Hand Grasp at the next Grab start; keep the temporary boost across repeated Grab presses and restore only by delayed timer/release/defaults. Adds Update-based delayed restore so restore-pending can fire even after the grab route stops.
// V107_FRONT_LEFT_GRASP_SERIAL_ONCE_BUILD 2026-06-30: Based on v106/v96 stable. Chest Hold Front Left Hand Grasp uses a run serial from Grab start; boost and restore can each fire only once per run, even if L/R/front route code is evaluated multiple times.
// V106_FRONT_LEFT_GRASP_ONE_SHOT_BUILD 2026-06-30: Based on v105. Chest Hold Front Left Hand Grasp boost/restore is one-shot per Grab to prevent repeated t=1.0 boost/restore loops; reset only on new grab/release/defaults.
// V105_FRONT_LEFT_GRASP_GEOMETRY_ONLY_LOGS_BUILD 2026-06-30: Based on v104. Chest Hold Front Left Hand Grasp now targets only geometry JSON + DAZMorph, not other plugin sliders/storables, and logs per-target before/after to verify values.
// V104_FRONT_LEFT_GRASP_FROM_TESTER_BUILD 2026-06-30: Based on v103/v96 stable. Integrates the HandMorphTester_v001 working hand-morph resolver/writer into Chest Hold Front only: after step2, Left Hand Grasp +=0.1; after Wrist Up/final, restore original. Back and rotations unchanged.
// V103_REVERT_V96_STABLE_BUILD 2026-06-30: Reverts v97-v102 Left Hand Grasp experiments. Restores v96 FIX behavior exactly for Chest Hold Front/Back rotation stability.
// V95_BACK_FOLLOW_SKIP_NEAR_WRIST_RESET_BUILD 2026-06-30: Based on v94. Back Chest Hold also arms 3s nipple hand follow. When Back hand is already near its step3 nipple-side final position, Grab Hand skips the initial temporary wrist-rotation restore/off cycle for that hand; Wrist In and Wrist In2 final steps are preserved.
// V93_FRONT_STEP2_IN5_FOLLOW_NIPPLE_3S_BUILD 2026-06-29: Based on v92. Chest Hold Front step2 inward is 5cm while final inward stays 3cm. After Chest Hold Grab Hand final, self hands follow the assigned target nipple controls for 3 seconds, preserving the final hand-to-nipple offset.
// V92_CHEST_HOLD_NIPPLE_NO_LOCK_HALF_MOVE_BUILD 2026-06-29: Based on v91. Chest Hold Grab Hand never locks target nipple IK; if nipple IK was locked, release/forget it on grab start. Chest Hold nipple Pull/Push/Up/Down/Left/Right move nipples at 1/2 distance and chest drag uses actual moved nipple offset.
// V89_NIPPLE_RELEASE_CHEST_DRAG_BUILD 2026-06-29: Based on v88 FIX. Release/Self IK Default/Self Load User Defaults also release target nipple IK. Chest Hold nipple Pull/Push/Up/Down/Left/Right add chest drag feedback only when nipple controls actually move.
// V91_GEN_FINAL_WRIST_DOWN_BUILD 2026-06-29: Based on v90. Gen target applies Wrist Down as the final hand step after position movement; hand position stays locked and palm-auto is skipped for Gen.
// V88_CHEST_HOLD_FRONT_STEP2_STEP3_INWARD_BUILD 2026-06-29: Based on v87. Chest Hold Front only: step2 = assigned nipple 10cm down + 1cm hand-forward + 3cm inward; step3/final = assigned nipple 8cm down + 6cm hand-forward + 3cm inward. Back FIX unchanged.
// V87_CHEST_HOLD_FRONT_STABLE_BODY_AXIS_BUILD 2026-06-29: Front simple cross forward axis no longer uses current/grab-start hand position. Uses stable self body/chest anchor -> assigned nipple so repeated Grab does not drift. Based on v86.
// V86_CHEST_HOLD_FRONT_HAND_FORWARD_AXIS_BUILD 2026-06-29: Front simple cross uses each hand's actual reach direction (grab-start hand -> assigned nipple) for step2/final forward offsets. Based on v85.
// V85_CHEST_HOLD_FRONT_FINAL_ADJUST_BUILD 2026-06-29: Front simple cross step2=8cm down+1cm front, step3/final=8cm down+6cm front. Based on v84.
// V82_CHEST_HOLD_FRONT_SIMPLE_CROSS_BUILD 2026-06-29: Based on v81. Back is fixed/unchanged. Front face mode is isolated: R hand -> target left nipple, L hand -> target right nipple, with no OrderHoldTargets/palm/side remap.
// V84_CHEST_HOLD_FRONT_STEP2_BACK_STEP3_DOWN_FRONT_BUILD 2026-06-29: Based on v83. Chest Hold Front only: mutual-facing simple cross now uses step2 = nipple 5cm down + 3cm back, step3/final = nipple 5cm down + 3cm front. Back FIX remains unchanged.
// V81_CHEST_HOLD_BACK_WRIST_IN2_REVERSE_STRONGER_BUILD 2026-06-29: Based on v80. Reverses Wrist In2 bend direction and strengthens it. Chest Hold Back step5 uses Wrist In2; fixed Euler RotY remains removed.
// V80_CHEST_HOLD_BACK_WRIST_IN2_STEP5_BUILD 2026-06-29: Based on v79. Removes fixed Euler RotY step5; adds Wrist In2 button/action and uses Wrist In2 as Chest Hold Back step5. In2 is stronger inward than Wrist In.
// V77_CHEST_HOLD_BACK_STEP5_R20_BUILD 2026-06-29: Based on v76. Chest Hold Back step5 keeps L=300 and changes R from 180 to 20 (380 normalized).
// V73_CHEST_HOLD_BACK_STEP2_STEP3_WRIST_IN_BUILD 2026-06-29: Based on v72. Chest Hold Back is now staged: step2=current side reach at nipple offset 0.000, step3=Chest Hold Back Nipple Offset target (default -0.030), step4=apply Wrist In only after step3.
// V65_CHEST_HOLD_BACK_FRONT_ADJUST_3CM_BUILD 2026-06-29: Chest Hold Back reach test; based on v64. Only changes front adjust from 0.08m to 0.03m.
// V58_CHEST_HOLD_BACK_AXIS_LOG_BUILD 2026-06-29: Chest Hold log-only. Adds normal-time axis diagnostics for target right, nipple right, and reach right axes. No motion logic changes.
// V46_CHEST_HOLD_BACK_LEFT_DEPTH_3X_BUILD 2026-06-29: Chest Hold only, based on v45/v33. Only changes the L-hand back Chest Hold depth correction multiplier from 2.0f to 3.0f; R-hand route is unchanged.
// V44_CHEST_HOLD_BACK_LEFT_DEPTH_REVERSE_2X_BUILD 2026-06-29: Chest Hold only, based on v43/v33. Only the L-hand back Chest Hold depth correction is reversed and doubled from v43; R-hand route is unchanged.
// V31_CHEST_HOLD_TARGET_AXIS_MIDPOINTS_LOGS_BUILD 2026-06-29: Chest Hold only: middle/open points now use the actual target L-nipple to R-nipple axis and nipple-pair center for symmetric left/right routing; debug logs show start/mid/final and axis. Other routes unchanged.
// V29_CHEST_HOLD_MUTUAL_FRONT_EXACT_ONLY_BUILD 2026-06-29: Chest Hold only: adds an isolated mutual-facing front route using both self/target facing, assigns L hand->target R nipple and R hand->target L nipple, and moves hand IK exactly to nipple IK; non-mutual/self-front-target-back cases stay on legacy Chest Hold routes. Hug Body and all other routes are unchanged.
// V30_CHEST_HOLD_CURRENT_QUIET_LOGS_BUILD 2026-06-29: Keeps the current v29 Chest Hold behavior, but moves normal diagnostic logs behind Debug Log so routine use stays quiet.
// V28_HUG_BODY_WRAP_ACTUAL_HAND_SIDE_BUILD 2026-06-29: Hug Body wrapSide now aligns +side with the actual current R-hand side after actorRight projection, so unusual standing positions open hands outward instead of crossing.
// V24_HUG_BODY_BACK_CHEST_CENTER_BUILD 2026-06-29: Hug Body backside now keeps final handCenter on chestControl center instead of applying final-depth offset, so backside Hug Body converges to chest, not shifted handCenter/nipple-side.
// V21_CHEST_HOLD_BACK_LR_RESTORE_VIA_BUILD 2026-06-29: Reverts the v20 visual L/R swap; back-side Chest Hold uses L hand -> L nipple and R hand -> R nipple, keeping the v18 two-stage via route and v17 nipple stabilize.
// V19_CHEST_HOLD_BACK_FIXED_LR_NIPPLE_VIA_BUILD 2026-06-29: Back-side Chest Hold no longer uses ordered/cross hand targets; L hand stays on assigned L nipple, R hand stays on assigned R nipple, and via logs show raw/used targets.
// V18_CHEST_HOLD_BACK_TWO_STAGE_VIA_BUILD 2026-06-29: Back-side Chest Hold hand path now uses a two-stage route via target-front/outside before converging to nipple/palm target, keeping v17 nipple stabilization and v16 front=Up/back=In wrist fix.
// V15_CHEST_HOLD_TARGET_VISUAL_FRONT_SIDE_BUILD 2026-06-29: Chest Hold front/back now uses target visual side from target nipple/chest center to self position; visual front is dot<=0 for VaM root-forward convention. Front keeps Wrist Up, back keeps Wrist In.
// V16_CHEST_HOLD_FRONT_DOT_POSITIVE_UP_BUILD 2026-06-29: Chest Hold visual front判定を targetForward dot (targetPoint->self) >= 0 に修正。正面側は Wrist Up、背面側は Wrist In。
// V17_CHEST_HOLD_NIPPLE_STABILIZE_ON_GRAB_BUILD 2026-06-29: Chest Hold Grab Hand開始時だけ target L/R nipple PositionState を一時Onにして暴れを抑え、Release/Target reset/defaultsで元のIK状態へ戻す。Utility nipple moveでは新規Onしない。
// V13_CHEST_HOLD_FRONT_UP_BACK_IN_BUILD 2026-06-29: Chest Hold mutual-facing front/back remains unchanged, but final wrist mode is corrected to front=Up and back=In. Final logs now include wristMode.
// V12_CHEST_HOLD_FRONT_MUTUAL_FACING_BUILD 2026-06-29: Chest Hold front/back is now decided by mutual facing: front only when self faces target and target faces self; self faces target while target faces away is back. Wrist remains front=In/back=Up.
// V11_CHEST_HOLD_FRONT_IN_BACK_UP_BUILD 2026-06-29: Chest Hold keeps the shared front/back decision and nipple target offsets, but swaps final wrist assignment: front wrist is Wrist In, back wrist is Wrist Up.
// CHEST_HOLD_NIPPLE_NO_IK_ON_BUILD 2026-06-29: Chest Hold utility nipple moves no longer turn nipple IK PositionState/RotationState On; nipple control positions are moved/restored directly while preserving IK state.
// HUG_BODY_CHEST_HOLD_ISOLATION_BUILD 2026-06-29: Restores/clears Chest Hold nipple stabilize state as soon as IK Select leaves Chest Hold, and hard-gates nipple-pair grab logic so Hug Body can never run Chest Hold/R nipple routes.
// TARGET_RELEASE_UNBLOCK_HBA_HLA_HAND_COVER_BUILD 2026-06-28: Target Release/Target reset now restores held target-hand IK locks before clearing held state and forces TG Held Target Hand/L/R flags false early, so HBA/HLA hand cover unblocks on target-side release.
// HELD_TARGET_HAND_TARGET_UID_EXPORT_BUILD 2026-06-28: Exports TG Held Target Person UID so HBA/HLA on the target Person can find TargetGrabber even when TargetGrabber is installed on the grabbing/self Person.
// HELD_TARGET_HAND_FLAG_AGGREGATE_BUILD 2026-06-27: Adds hidden aggregate TG Held Target Hand flag and keeps L/R flags updated for HBA/HLA linkage diagnostics.
// HELD_TARGET_HAND_FOLLOW_LOCK_BUILD 2026-06-27: Target L/R Hand and Hand Hold grabs now keep the target hand IK locked as a self-hand-follow target. Utility moves may move the target hand, but AutoSnap/restore paths no longer drop or resnap the held target hand lock until Release/Target reset/defaults.
// TARGET_RUNTIME_RESET_ON_TARGET_RELEASE_DEFAULTS_BUILD 2026-06-27: Target Release, Target IK Default, and Target Load User Defaults now clear TargetGrabber target-side runtime lock/snap/held state before/after applying the requested reset.
// DEFAULT_IK_SELECT_HUG_BODY_BUILD 2026-06-26: IK Select defaults/falls back to Hug Body without auto-resetting to <none>, keeping HDU direct-route selection ownership.
// TARGET_IK_DEFAULT_SNAP_CURRENT_BUILD 2026-06-26: Target IK Default now treats the current target IK pose as the new snap/release baseline by clearing target snap/restore caches without changing IK Select, avoiding HDU_Commander route conflicts.
// FOOT_FRONTSIDE_PATH_FIX_BUILD 2026-06-26: Grab Foot now flips L/R path only on the target front-side route so the normal back-side foot direction remains unchanged.
// LOAD_USER_DEFAULTS_POSE_RESYNC_BUILD 2026-06-26: After Self/Target Load User Defaults, clears/rebases TargetGrabber pose-dependent snap/restore caches so old pose snapshots are not reused.
// TARGET_NONE_BODY_NUDGE_HIP_AXIS_BUILD 2026-06-25: None Body Nudge Pull/Push/Left/Right now use target hip/body visual horizontal axes; Atom transform root was often fixed to world +Z.
// TARGET_NONE_BODY_NUDGE_TARGET_ROOT_AXIS_BUILD 2026-06-25: Target Controller None Body Nudge Pull/Push/Left/Right now use target root horizontal forward/right axes instead of self-to-target position axis.
// TARGET_NONE_BODY_NUDGE_COMPLY_BUILD 2026-06-25: Target Controller None Body Nudge now temporarily sets non-moving target limb IK to Comply and restores it on Target Release.
// TARGET_NONE_BODY_NUDGE_BUILD 2026-06-25: Target Controller None makes Grab Hand Pull/Push/Up/Down/Left/Right nudge target torso/head controls as a body group.
// TARGET_NONE_BODY_NUDGE_HDU_ROUTE_BUILD 2026-06-25: Allows HDU direct routes to select Target Controller <none> so None Body Nudge works from HDU_Commander.
// TARGET_NONE_BODY_NUDGE_COMPLY_BUILD 2026-06-25: Target Controller <none> makes Grab Hand Pull/Push/Up/Down/Left/Right move target body while non-moving IK temporarily Comply.
// TARGET_RELEASE_HC_REAPPLY_BUILD 2026-06-25: After Target Release, safely calls target-side humanPoseControler/humanControler HC Reapply Current Pose when available.
// TARGET_PELVIS_ONESHOT_DEBUG_BUILD 2026-06-24: Makes TargetGrabber pelvis control one-shot only and adds verification logs/buttons. No LateUpdate hold.
// HDU_GRAB_HAND_ROUTES_BUILD 2026-06-24: Adds HDU direct target+GrabHand utility actions so HDU_Commander does not need to drive the IK Select popup.
// V5au: Moves Target Pelvis auto/test diagnostic logs behind Debug Log; behavior unchanged.
// ============================================================
// TargetGrabber.cs
// Version: V5h_hug_body_snap_position_clamp
// Date: 2026-06-23
// Base: TargetGrabber_v4_0de_chest_hold_button_release_only.cs
// Summary:
// - Keeps Hip Hold Auto Grab Width at 1.50.
// - Changes other wide person targets that previously auto-set Grab Width to 2.00 down to 0.80.
// - Keeps final wrist rotation fixed to Wrist In for single limb targets: L/R Hand, L/R Foot, and L/R Knee.
// - Keeps Hug Body final-point depth IN/OUT logic and the stabilized Hug Mode no-extra-deep-center behavior.
// - Keeps Pair Hand/Foot/Knee Hold Grab Width midpoint and fixed Wrist In.
// - Makes Hug Body Push/Pull use self-to-target depth axis so Push and Pull are guaranteed opposites.
// - Replaces the visible Grab Selected button with Target Swoon Drop: temporarily turns non-grab target IK off for 3 seconds; pressing again restores immediately.
// - v4.0cu: Target Swoon Drop keeps only the actually grabbed target control(s), and detects a held grab even after hasActiveGrab turns off at move completion.
// - v4.0cv: Tracks held target grab choice explicitly, includes target neck IK in Swoon Drop, adds one-hand Hug Body swoon twist, and doubles Hug Body Push/Pull pivot angle.
// - v4.0cw: Compile fix: comments the v4.0cv header history line correctly.
// - v4.0cx: Target Swoon Drop uses the current Target Controller first; Target Controller None forces keep=0/all target IK off.
// - v4.0cy: Adds Grab Hand Close/Left/Right and moves Release/Follow controls to the hidden Target Controller area.
// - v4.0cz: Restores the Target Controller popup, renames Release buttons, reorders Follow/Release/Default controls, and adds target shortcut buttons.
// - v4.0da: Moves Grab Follow below release/default buttons and places IK Select above target shortcut buttons.
// - v4.0db: Adds explicit Chest Hold routes for Grab Hand Pull/Push/Up/Down/Open/Close/Left/Right buttons.
// - v4.0de: Defers Grab Hand utility-button interactable refresh for Chest Hold button routes so Unity/VaM buttons visually release after click.
// - v4.0df: Compile fix: uses System.Collections.IEnumerator for the deferred button update coroutine.
// - V5: Chest Hold utility buttons move nipple IK controls directly, and Chest Hold keeps those buttons enabled.
// - V5b: Adds a strong Hug Body Wrist In bias by increasing the final-point Out threshold for Hug Body only.
// - V5c: Strengthens the Hug Body Wrist In bias further by raising the Hug Body Out threshold from 0.120m to 0.200m.
// - V5g: For Hug Body Auto Snap, self hand controls snap position only and keep the final Wrist In rotation.
// - V5b: Biases Hug Body final wrist strongly toward Wrist In; Out is used only when the final hand remains far on the actor/front side.
// V5j: Stabilizes target neck/head during Hug Body by excluding them from Hug Body pivot/autosnap target paths while preserving normal functions.
// V5af: Changes the guarded test slider from target abdomenControl Rot X to target pelvis Rot X.
//       Slider is VaM Move style 0..360: 270 = front/near side, 45 = back/far side.
// V5ag: Restricts target pelvis Rot X test range to 90..270 and adds Pelvis Auto On Grab.
//       Grab Hand frontSide sets 270 Near; backSide sets 90 Back. This is fixed assignment, not additive.
// V5ah: Moves Pelvis Auto On Grab after StartTimedGrab/ResolveControls so target Person is ready before applying pelvis Rot X.
// V5ap: Reverses TargetGrabber Pelvis Auto one-shot assignment only: frontSide -> 90 Back, backSide -> 270 Near.
// V5aq: Adds one-shot Target Pelvis Face Self yaw. Grab Hand Pelvis Auto now sets X plus Yaw toward self. Adds Face Self debug buttons.
// V5ar: Tried +180 yaw offset when detector backSide; rejected because TargetGrabber visual front/back is reversed for pelvis.
// V5as: Applies +180 yaw offset on detector frontSide instead, matching the visual back-side branch used by reversed pelvis X mapping.
//       The shared front/back detector is unchanged so hand routing is not affected.
// V5an: Removes TargetGrabber pelvis hold/manual test controls so TargetLinePerson can own persistent pelvis control. Keeps Grab Hand pelvis auto as one-shot only.
// V5ac: Hug Body Pull/Push uses HDC Hip-Upper on target hip/chest/head only; target hand/elbow/neck stay natural. Self hands follow chest-relative.
// - V5aa: Hug Body HDC Hip-Upper keeps target upper rotation, but self hands follow target chest-relative offsets instead of hip-pivot arcs.
// - V5ab: Hug Body HDC Pull/Push no longer drives target hand/elbow IK; target arms are left natural.
// - V5ac: Hug Body HDC Pull/Push drives target headControl with hip/chest while leaving target arms and neck natural.
// - V5ad: Increases one-hand Hug Body HDC yaw amount so single-hand Pull/Push twists more visibly.
// V5y: Rebuilds Hug Body Pull/Push from the stable v5n GrabHand route; only Pull/Push uses HDC-style Hip-Upper local Rot X/Y.
// V5m: For Hug Body Pull/Push, relaxes target neck/head IK during the pivot instead of freezing neck/head.
// V5n: Skips target neck/head AutoSnap for Head/Neck/Mouth grab routes to prevent target head rotation spin.
// V5m: For Hug Body Pull/Push, temporarily turns target neck/head IK OFF during the pivot so upper body can bend without head/neck IK resistance. Adds HDU Head route.
// V5at: Integrates Pelvis Auto one-shot into the Hip Hold grab route after Hip Hold target resolution, with per-grab guard/logs.
// ============================================================
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// FILE: TargetGrabber.cs
// VAM Target Grabber
//
// 指定Atomを手・足で掴む補助プラグイン
//
// Author : VAMT
// Version: v4.0da_ui_follow_lower_ik_select
// v3.0dh: Hug Body sends hands toward the actor's forward direction instead of target front/back.
//          Chest Hold / Hip Hold / pair hold routes are unchanged.
// v3.0di: Colors Release buttons when self/target restore state exists, and locks target thighs during Hip Hold.
// v3.0dj: Avoids VaM Mono KeyCollection foreach compiler crash in target lock restore.
// v4.0a: Adds Grab Head to move the actor headControl near the selected target part.
// v4.0b: Renames the head action button to Kiss, moves it to the top, and relaxes self chest IK during Kiss.
// v4.0c: Adds constrained Kiss face alignment for all target controllers.
// v4.0d: Adds Gen target using LabiaTrigger and renames the visible crotch-like target to Groin.
// v4.0e: Adds Self IK Default button for the selected person's basic IK state.
// v4.0f: Adds Anus target and a focused intimate target scan without inferred Gen/Anus fallbacks.
// v4.0g: Uses the existing _JointAl/Debug transform for Anus and removes broad intimate name scans.
// v4.0h: Caches the Anus transform so Follow/Grab frames do not rescan all target transforms.
// v4.0i: Adds wrist rotation test buttons for both hands: Straight/In/Out/Up/Down.
// v4.0j: Applies the checked wrist In/Out bend to Grab Hand so fingers bend toward the target center.
// v4.0k: Adds a default-ON wrist angle toggle and makes Wrist Test respect Left/Right Hand checks.
// v4.0l: Chooses Wrist In/Out by comparing which palm side faces the target IK more closely.
// v4.0m: Applies hand rotation after hand movement, using the moved hand position for palm-facing selection.
// v4.0n: Computes a continuous wrist bend angle so the palm side points toward the target IK.
// v4.0o: Adds wrist auto-angle trace logs for pass-through/back-side diagnosis.
// v4.0p: Adds wrist auto-angle skip/wait logs to confirm whether the after-move rotation path runs.
// v4.0r: Swaps Wrist In/Out test button direction labels to match the intended wrist-bend image.
// v4.0s: Reverses Hug Body wrist auto-angle target direction so near side opens and pass-through closes.
// v4.0t: Caches the Gen LabiaTrigger transform so Follow/Grab frames do not rescan all target transforms.
// v4.0u: Flips the palm-face axis only for Gen target to avoid approaching with the back of the hand.
// v4.0v: Uses a Gen-specific straight-wrist route: no automatic wrist bend, only fixed hand rotation plus Hand Palm Add Rot.
// v4.0w: Applies the Hug Body wrist target-direction flip only on the front-side route.
// v4.0x: Applies the Hug Body wrist target-direction flip on both front/back routes.
// v4.0y: For Hug Body, chooses OUT/IN/Straight from selected-person root depth: farther target=OUT, passed target=IN.
// v4.0z: Uses the Hug Body movement axis for OUT/IN/Straight depth instead of selected-person root forward.
// v4.0ab: Reverses the Hug Body wrist depth axis so pass-through is IN and farther target is OUT.
// v4.0ac: Keeps Hug Body depth mode, but swaps applied wrist Euler on crossed hand paths.
// v4.0ad: For Hug Body, decides OUT/IN from each hand's approach path: before target=OUT, passed target=IN.
// v4.0ae: Keeps v4.0ad Hug Body approach detection and flips only the applied wrist Euler on crossed paths.
// v4.0af: Limits crossed-path wrist Euler flip to clearly-before-target Hug Body cases only.
// v4.0ag: Sets Hug Body wrist bend to fixed IN for all Hug Body positions.
// v4.0ah: Sets Hug Body wrist bend to fixed OUT for all Hug Body positions.
// v4.0ai: Sends Hug Mode deep center along grabber-to-target direction instead of actor forward.
// v4.0aj: Enables Grab Hand Pull for Hip Hold by pulling target l/rThigh controls.
// v4.0ak: Scales Grab Hand Pull distance by target controller: Hand/Foot/Knee full, others half.
// v4.0al: Prevents Hip Hold Pull target thighs from auto-snap/self-following back after pull.
// v4.0am: Keeps the current grab width when re-grabbing after Pull instead of reopening to Grab Width.
// v4.0an: Re-grabs after Pull at Final Grab Width so Pull does not widen the hands.
// v4.0ao: Uses Final Grab Width for Pull re-grab even when no active grab was detected.
// v4.0ap: Temporarily turns hand rotation state off when grabbing hands with Align Hand Palm off.
// v4.0aq: Temporarily turns selected hand rotation state off at Grab Hand start before any route applies rotation.
// v4.0ar: Adds Target Controller choices for Peni Base / Peni Mid / Peni Tip.
// v4.0as: Adds Peni target point debug logs without changing target behavior.
// v4.0at: Forces Hand Center Offset to 0.0 for all targets.
// v4.0au: Gives Peni Base/Mid/Tip dedicated Auto Grab Width, Final Grab Width, and Auto Z Offset.
// v4.0av: Disables auto wrist bending for Peni Base/Mid/Tip and keeps the straight fixed hand rotation.
// v4.0aw: Keeps Peni Pull from snapping/following the Peni control back, and leaves Peni hand rotation off.
// v4.0ax: Sets Peni Base/Mid/Tip Auto Grab Width to 0.10.
// v4.0ay: Adds normal Grab/Hug Body hand target trace logs without changing movement behavior.
// v4.0bb: Caps only Hug Body hand path width so wrapping stays wide but avoids unreachable detours.
// v4.0bd: For Hug Body hands, uses approach-based actor-left side axis instead of target root side axis.
// v4.0bg: Resolves Hug Body hand layout by current hand side and movement cost, avoiding fixed L/R or simple forward flip.
// v4.0bh: Restores Hand/Foot/Knee Grab Hand Pull by moving target IK toward active self hand IK.
// v4.0bi: Uses a 0.01 internal minimum for Final Grab Width instead of allowing 0.000.
// v4.0bj: Hug Body layout prefers the candidate farther from the actor root instead of shortest-distance only.
// v4.0bk: Fades Hug Body far handCenter bias out near the end when Hug Mode is OFF.
// v4.0bm: Rebuilds Wrist test buttons to use the same fixed hand basis as Grab hand rotation.
// v4.0co: Forces final Wrist In for L/R Hand, L/R Foot, and L/R Knee final-point re-grabs.
// v4.0cp: Keeps Hip Hold Grab Width at 1.50 and lowers other wide person auto Grab Width from 2.00 to 0.80.
// v4.0cq: For Hug Body Push/Pull, bypasses reach/hand-position offset logic and pivots along self-to-target depth axis with opposite signs.
// v4.0cu: Target Swoon Drop preserves only the grabbed target control(s); no support-chain keep.
// v4.0cv: Held target grab state is explicit so Follow OFF completion does not make keep=0; Neck is also dropped; one-hand Hug Body can twist target toward the held hand.
// v4.0cx: Target Swoon Drop keeps the current Target Controller if selected; None ignores held grab and drops all target IK.
// v4.0bn: Uses a left/right symmetric visual base for Wrist test buttons instead of pathRight/layout basis.
// v4.0bo: Uses the verified Grab HAND ROT fixed presets for Wrist Straight and fixes the left/right preset swap.
// v4.0bp: Makes Wrist test buttons preset-only; no handRot offset, path/layout, target center, or current-pose basis is used.
// v4.0bq: Wrist buttons use captured arm poses: hand positions locked, hand rotations preset, elbows moved per mode.
// v4.0br: Clears pending wrist hand locks on wrist button start / grab start / release / defaults to avoid stale 8-frame locks.
// v4.0bx: Hug Body wrist picks Out only when the actual final hand position remains on the near/front side; near-center/back defaults to In.
// v4.0ca: Adds Grab Hand Up/Down, moves self defaults under Target Controller, and adds target-side IK/default buttons.
// v4.0cl: Pair Hand/Foot/Knee Hold uses Grab Width for hand midpoint spread and fixes final wrist to In.
// v4.0ck: Hug Body route snaps the IK control to the actual body hand at the end instead of forcing the control to finalPoint.
// v4.0cn: Hug Mode no longer applies old deep-center push on top of final-point hand routes; Hug Depth only affects Hug Body final depth lightly.
// v4.0ch: Hug Body uses actor-left/right midpoint from final point for the spread/open route.
// v4.0ci: Hug Body spread/open midpoint uses self-to-target view right, not target facing or self root facing.
// v4.0cj: Previous build force-snapped the hand control to finalPoint; superseded by v4.0ck IK snap.
// v4.0cg: Normal Grab Hand uses final-point-first route; Hug Body final center is placed slightly beyond target and wrist is final-depth only.
// v4.0by: Adds Grab Hand Push as the counterpart to Grab Hand Pull.
// v4.0bz: Upper-body Push/Pull uses target hip as pivot and rotates chest/head/related controls instead of plain translation.
// v4.0by: Adds Grab Hand Push to move movable target controls horizontally away from active self hands, then re-grab.
// v4.0bw: Hug Body wrist depth pick now stores the actual reach-limited hand target instead of unreachable desired.
// v4.0bv: Hug Body wrist uses stored depth reference: near/actor side = Wrist Out, far/unknown = Wrist In.
// v4.0bs: For Hug Body, chooses Wrist IN/OUT after hand arrival by comparing final palm-facing score; near center falls back to IN.
// v4.0bt: Compile fix: comment marker was missing in the summary header.
// v4.0bu: For Hug Body final wrist, skips IN/OUT scoring and applies the Wrist In button preset rotation directly.
//
// 機能
// ・Target Type排他選択（Atom / Person）
// ・Target Atom選択
// ・Target Person / Controller選択（FreeControllerV3全列挙、target/tool除外、Filter対応）
// ・Person選択
// ・Left / Right Hand Grab
// ・Left / Right Foot Grab
// ・Follow Target
// ・届かない場合は最大到達距離まで伸ばす
// ・左右で挟む配置
// ・手のひらをターゲット中心へ向ける
// ・足の裏をターゲット中心へ向ける
// ・手のひら回転は左右固定プリセット方式
// ・Hug Mode（いったん奥へ送ってから抱える）
// ・Release
//
// 想定用途
// ・ぬいぐるみを持つ
// ・ボールを持つ
// ・クッションを抱える
// ・足で物を挟む
//
// 次期拡張
// ・Hold / Push / Pull / Sandwich
// ・指制御
// ・足指制御
// ・追従スムージング強化
//
// Tested : VaM 1.x
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

public class TargetGrabber : MVRScript
{
    private const string NONE = "<none>";
    private const string LRNIPPLE = "lrnipple";
    private const string TC_CHEST_HOLD = "Chest Hold";
    private const string TC_HIP_HOLD = "Hip Hold";
    private const string TC_HUG_BODY = "Hug Body";
    private const string DEFAULT_TARGET_CONTROLLER = TC_HUG_BODY;
    private const string TC_CROTCH = "Crotch";
    private const string TC_GROIN = "Groin";
    private const string TC_GEN = "Gen";
    private const string TC_PENI_BASE = "Peni Base";
    private const string TC_PENI_MID = "Peni Mid";
    private const string TC_PENI_TIP = "Peni Tip";
    private const string TC_ANUS = "Anus";
    private const string TC_MOUTH = "Mouth";
    private const string TC_NECK = "Neck";
    private const string TC_ABDOMEN = "Abdomen";
    private const string TC_HIP = "Hip";
    private const string TC_HAND = "Hand Hold";
    private const string TC_FOOT = "Foot Hold";
    private const string TC_KNEE = "Knee Hold";
    private const string TC_HEAD = "Head";
    private const string TC_HEAD_TOP = "Head Top";
    private const string TC_L_NIPPLE = "L Nipple";
    private const string TC_R_NIPPLE = "R Nipple";
    private const string TC_L_HAND = "L Hand";
    private const string TC_R_HAND = "R Hand";
    private const string TC_L_FOOT = "L Foot";
    private const string TC_R_FOOT = "R Foot";
    private const string TC_L_KNEE = "L Knee";
    private const string TC_R_KNEE = "R Knee";
    private const string FOLLOW_OFF = "OFF";
    private const string FOLLOW_SELF = "Self";
    private const string FOLLOW_TARGET = "Target";
    private const string CUSTOM_TARGET_PREFIX = "Custom: ";

    private const int WRIST_HAND_LOCK_FRAMES = 8;

    private struct WristControlPose
    {
        public Vector3 LocalPos;
        public Quaternion LocalRot;

        public WristControlPose(float px, float py, float pz, float qx, float qy, float qz, float qw)
        {
            LocalPos = new Vector3(px, py, pz);
            LocalRot = NormalizeQuaternionRaw(new Quaternion(qx, qy, qz, qw));
        }
    }

    private class WristArmPose
    {
        public WristControlPose RHand;
        public WristControlPose LHand;
        public WristControlPose RElbow;
        public WristControlPose LElbow;

        public WristArmPose(WristControlPose rHand, WristControlPose lHand, WristControlPose rElbow, WristControlPose lElbow)
        {
            RHand = rHand;
            LHand = lHand;
            RElbow = rElbow;
            LElbow = lElbow;
        }
    }

    private class PendingWristHandLock
    {
        public string Mode;
        public string Label;
        public FreeControllerV3 Control;
        public Vector3 LockTransformWorldPos;
        public Vector3 LockControlWorldPos;
        public Quaternion TargetLocalRot;
        public bool HasControlTransform;
        public int FramesLeft;
    }
    private const float MIN_FINAL_GRAB_WIDTH = 0.01f;
    private const float HIP_HOLD_GRAB_WIDTH = 1.50f;
    private const float WIDE_PERSON_GRAB_WIDTH = 0.80f;
    private const float HIP_HOLD_FINAL_GRAB_WIDTH = 0.13f;
    private const float CROTCH_GRAB_WIDTH = 0.00f;
    private const float CROTCH_FINAL_GRAB_WIDTH = MIN_FINAL_GRAB_WIDTH;
    private const float PENI_GRAB_WIDTH = 0.10f;
    private const float PENI_FINAL_GRAB_WIDTH = 0.03f;
    private const float PENI_AUTO_Z_OFFSET = -0.03f;
    private const float HUG_BODY_HAND_WIDTH_CAP = 0.55f;
    private const float HUG_BODY_HAND_CENTER_OFFSET = 0.22f;
    private const float HUG_BODY_FINAL_POINT_DEPTH_OFFSET = 0.18f;
    private const float HUG_BODY_IK_SNAP_START_T = 0.985f;
    private const float FINAL_POINT_WRIST_DEPTH_THRESHOLD = 0.025f;
    private const float HUG_BODY_FINALPOINT_OUT_THRESHOLD = 0.200f;
    private const float HUG_BODY_IK_SNAP_MAX_OFFSET = 0.040f;
    private const float HUG_BODY_WRIST_NEAR_CENTER_DISTANCE = 0.03f;
    private const float HUG_BODY_WRIST_DEPTH_THRESHOLD = 0.03f;
    private const float CHEST_HOLD_PALM_CENTER_REACH_FRONT = 0.04f;
    private const float CHEST_HOLD_PALM_CENTER_REACH_BACK = 0.08f;
    private const float CHEST_HOLD_FRONT_NIPPLE_DOWN = 0.08f;
    private const float CHEST_HOLD_FRONT_NIPPLE_INWARD = 0.04f;
    private const float CHEST_HOLD_FRONT_SIMPLE_STEP2_DOWN = 0.13f; // v90: step2 is 13cm below assigned nipple
    private const float CHEST_HOLD_FRONT_SIMPLE_FINAL_DOWN = 0.11f; // v90: step3/final is 11cm below assigned nipple
    private const float CHEST_HOLD_FRONT_SIMPLE_STEP2_FORWARD = 0.06f; // v94: step2 is 6cm forward along stable hand route
    private const float CHEST_HOLD_FRONT_SIMPLE_FINAL_FORWARD = 0.01f; // v94: step3/final is 1cm forward along stable hand route
    private const float CHEST_HOLD_FRONT_SIMPLE_STEP2_INWARD = 0.05f; // v93: step2 moves 5cm inward toward nipple pair center
    private const float CHEST_HOLD_FRONT_SIMPLE_FINAL_INWARD = 0.03f; // v88/v93: step3/final keeps 3cm inward toward nipple pair center
    private const float CHEST_HOLD_NIPPLE_HAND_FOLLOW_SECONDS = 3.0f; // v95: after Chest Hold Grab Hand, hands follow assigned nipple controls briefly
    private const float CHEST_HOLD_FRONT_STEP2_SWITCH_T = 0.70f; // v104: same split as MoveChestHoldBackStep2Step3Control. Boost after step2 is reached.
    private const float CHEST_HOLD_FRONT_LEFT_GRASP_BOOST = 0.35f; // v115: half of v113 visual test. Front Chest Hold L/R Hand Grasp boost after step2.
    private const float CHEST_HOLD_FRONT_RIGHT_GRASP_BOOST = CHEST_HOLD_FRONT_LEFT_GRASP_BOOST; // v115: symmetric Right Hand Grasp boost after step2.
    private const float CHEST_HOLD_FRONT_LEFT_GRASP_RESTORE_DELAY = 1.00f; // v115: restore the temporary Front Chest Hold L/R Hand Grasp about 1 second after boost.
    private const float CHEST_HOLD_BACK_NEAR_WRIST_SKIP_DISTANCE = 0.085f; // v95: already-near Back hand skips initial wrist reset/off cycle
    private const float CHEST_HOLD_BACK_NEAR_WRIST_SKIP_LATERAL = 0.060f;
    private const float CHEST_HOLD_BACK_NEAR_WRIST_SKIP_VERTICAL = 0.080f;
    private const float CHEST_HOLD_BACK_NEAR_WRIST_SKIP_NIPPLE_DISTANCE = 0.140f;
    private const float CHEST_HOLD_BACK_VIA_OUTSIDE = 0.12f;
    private const float CHEST_HOLD_BACK_VIA_FORWARD = 0.18f;
    private const float CHEST_HOLD_BACK_VIA_UP = 0.00f;
    private const float CHEST_HOLD_BACK_VIA_SWITCH_T = 0.55f;
    private const float CHEST_HOLD_BACK_PASS_THROUGH = 0.35f; // back-side reach depth; keep v61 distance feeling
    private const float CHEST_HOLD_BACK_PASS_SIDE_OFFSET = 0.05f; // v71: side clearance 5cm. R hand right of R nipple, L hand left of L nipple
    private const float CHEST_HOLD_BACK_REACH_FRONT_ADJUST_DEFAULT = -0.03f; // v73: step3 default. Step2 is always nipple offset 0.000; step3 uses this slider value.
    private const float CHEST_HOLD_NIPPLE_DRAG_CHEST_PULL_PUSH_POS_SCALE = 0.35f; // v89: Pull/Push nipple movement drags chest position a little.
    private const float CHEST_HOLD_NIPPLE_DRAG_CHEST_DEGREES_PER_METER = 35.0f; // v89: nipple utility movement rotates chest for pulled feeling.
    private const float CHEST_HOLD_NIPPLE_UTILITY_MOVE_SCALE = 0.50f; // v92: Pull/Push/Up/Down/Left/Right nipple movement is half strength.
    private const float CHEST_HOLD_NIPPLE_DRAG_CHEST_MAX_DEGREES = 6.0f;
    private const float CHEST_HOLD_MUTUAL_FACE_DOT = 0.35f;
    private const float CHEST_HOLD_MUTUAL_ROOT_OPPOSITE_DOT = -0.35f;
    private const float HEAD_FINAL_GRAB_WIDTH = 0.15f;
    private const float HEAD_TARGET_UP_OFFSET = 0.130f;
    private const float HEAD_TOP_FORWARD_OFFSET = 0.180f;
    private const float HEAD_TOP_UP_OFFSET = 0.080f;
    private const float MOUTH_TARGET_FORWARD_OFFSET = 0.260f;
    private const float MOUTH_TARGET_UP_OFFSET = -0.020f;
    private const float MOUTH_FALLBACK_FORWARD_OFFSET = 0.340f;
    private const float MOUTH_FALLBACK_UP_OFFSET = -0.040f;
    private const float NECK_GRAB_WIDTH = 0.00f;
    private const float PAIR_HAND_MID_OUTSIDE_OFFSET = 0.070f;
    private const float PAIR_FINAL_OUTSIDE_OFFSET = 0.030f;

    private JSONStorableStringChooser personChooser;
    private JSONStorableStringChooser targetTypeChooser;
    private JSONStorableStringChooser targetAtomChooser;
    private JSONStorableStringChooser targetPersonChooser;
    private JSONStorableStringChooser targetPersonPartChooser;
    private JSONStorableStringChooser followModeChooser;
    private JSONStorableString targetControllerFilterJSON;

    private JSONStorableBool leftHandJSON;
    private JSONStorableBool rightHandJSON;
    private JSONStorableBool leftFootJSON;
    private JSONStorableBool rightFootJSON;
    private JSONStorableBool followTargetJSON;
    private JSONStorableBool autoSnapPullOpenIKJSON;
    private JSONStorableBool alignHandPalmJSON;
    private JSONStorableBool handWristAngleJSON;
    private JSONStorableBool alignFootSoleJSON;
    private JSONStorableBool debugLogJSON;
    private JSONStorableBool autoGrabWidthJSON;
    private JSONStorableBool hugModeJSON;
    private JSONStorableBool kissFaceAlignJSON;

    private JSONStorableFloat grabWidthJSON;
    private JSONStorableFloat grabCloseSpeedJSON;
    private JSONStorableFloat finalGrabWidthJSON;
    private JSONStorableFloat targetZOffsetJSON;
    private JSONStorableFloat autoZOffsetJSON;
    private JSONStorableFloat hugDepthJSON;
    private JSONStorableFloat maxHandReachJSON;
    private JSONStorableFloat maxFootReachJSON;
    private JSONStorableFloat maxHeadReachJSON;
    private JSONStorableFloat headTargetDistanceJSON;
    private JSONStorableFloat kissFaceStrengthJSON;
    private JSONStorableFloat handPalmOffsetJSON;
    private JSONStorableFloat handCenterOffsetJSON;
    private JSONStorableFloat chestHoldBackReachFrontAdjustJSON;
    private JSONStorableFloat footSoleOffsetJSON;
    private JSONStorableFloat kneeWidthMultiplierJSON;
    private JSONStorableFloat footArcWidthJSON;
    private JSONStorableFloat footArcDropJSON;
    private JSONStorableFloat moveTimeJSON;
    private JSONStorableFloat handRotXJSON;
    private JSONStorableFloat handRotYJSON;
    private JSONStorableFloat handRotZJSON;
    private JSONStorableFloat footRotXJSON;
    private JSONStorableFloat footRotYJSON;
    private JSONStorableFloat footRotZJSON;
    private JSONStorableBool targetPelvisRotXEnableJSON;
    private JSONStorableFloat targetPelvisRotXJSON;
    private JSONStorableBool targetPelvisAutoOnGrabJSON;
    private bool suppressTargetPelvisRotXCallback = false;
    private bool targetPelvisAutoOnGrabAppliedThisGrab = false;

    private JSONStorableString statusJSON;
    private UIDynamicButton grabHandPullButton;
    private UIDynamicButton grabHandPushButton;
    private UIDynamicButton grabHandUpButton;
    private UIDynamicButton grabHandDownButton;
    private UIDynamicButton grabHandOpenButton;
    private UIDynamicButton grabHandCloseButton;
    private UIDynamicButton grabHandLeftButton;
    private UIDynamicButton grabHandRightButton;
    private UIDynamicButton releaseTargetButton;
    private UIDynamicButton releaseButton;
    private UIDynamicButton swoonDropButton;
    private Coroutine deferredGrabHandUtilityButtonUpdateRoutine;
    private Coroutine poseLoadRuntimeResyncRoutine;

    private Atom selectedPerson;
    private Atom selectedTargetAtom;
    private Atom selectedTargetPerson;

    private FreeControllerV3 lHandControl;
    private FreeControllerV3 rHandControl;
    private FreeControllerV3 lElbowControl;
    private FreeControllerV3 rElbowControl;
    private FreeControllerV3 lFootControl;
    private FreeControllerV3 rFootControl;
    private FreeControllerV3 lKneeControl;
    private FreeControllerV3 rKneeControl;
    private FreeControllerV3 headControl;
    private FreeControllerV3 chestControl;
    private FreeControllerV3 hipControl;

    private bool hasActiveGrab = false;
    private bool hasHeldTargetGrab = false;
    private string heldTargetGrabChoice = null;
    private bool heldTargetGrabIncludeHands = false;
    private bool heldTargetGrabIncludeFeet = false;
    private bool heldTargetGrabIncludeHead = false;
    private bool heldTargetGrabLeftHand = false;
    private bool heldTargetGrabRightHand = false;
    private bool heldTargetGrabLeftFoot = false;
    private bool heldTargetGrabRightFoot = false;
    private bool heldTargetHandFollowLockLeft = false;
    private bool heldTargetHandFollowLockRight = false;
    private FreeControllerV3 heldTargetHandFollowLockLeftControl = null;
    private FreeControllerV3 heldTargetHandFollowLockRightControl = null;
    private JSONStorableBool tgHeldTargetHandJSON;
    private JSONStorableBool tgHeldTargetLHandJSON;
    private JSONStorableBool tgHeldTargetRHandJSON;
    private JSONStorableString tgHeldTargetPersonUidJSON;
    private bool suppressApply = false;

    // v1.8: Grab motion state.
    // Move Time Sec = 現在位置からターゲット位置まで到達する秒数。
    private bool activeIncludeHands = true;
    private bool activeIncludeFeet = true;
    private bool activeIncludeHead = false;
    private float activeMoveTimeMultiplier = 1.0f;
    private float grabElapsed = 0.0f;
    private float grabStartWidth = 0.0f;
    private float currentGrabWidth = 0.0f;
    private bool chestHoldFinalLeftLogged = false;
    private bool chestHoldFinalRightLogged = false;
    private bool chestHoldMovePointsLeftLogged = false;
    private bool chestHoldMovePointsRightLogged = false;
    private bool chestHoldEssentialTwoLineLogged = false;
    private bool chestHoldModeLogged = false;
    private readonly Dictionary<FreeControllerV3, Vector3> grabStartPositions = new Dictionary<FreeControllerV3, Vector3>();
    private readonly Dictionary<FreeControllerV3, Quaternion> grabStartRotations = new Dictionary<FreeControllerV3, Quaternion>();
    private readonly Dictionary<FreeControllerV3, Vector3> targetOriginalPositions = new Dictionary<FreeControllerV3, Vector3>();
    private readonly Dictionary<FreeControllerV3, Quaternion> targetOriginalRotations = new Dictionary<FreeControllerV3, Quaternion>();
    // v5bl: Chest Hold nipple utility moves must not turn nipple IK ON.
    // Controls in this set are restored by direct transform assignment, not MoveControl/LockTargetIKControl.
    private readonly HashSet<FreeControllerV3> chestHoldNoIkNippleMoveControls = new HashSet<FreeControllerV3>();
    private bool chestHoldNippleHandFollowPending = false;
    private bool chestHoldNippleHandFollowActive = false;
    private float chestHoldNippleHandFollowElapsed = 0.0f;
    private FreeControllerV3 chestHoldFollowLeftHand = null;
    private FreeControllerV3 chestHoldFollowRightHand = null;
    private FreeControllerV3 chestHoldFollowLeftNipple = null;
    private FreeControllerV3 chestHoldFollowRightNipple = null;
    private Vector3 chestHoldFollowLeftOffset = Vector3.zero;
    private Vector3 chestHoldFollowRightOffset = Vector3.zero;
    private string chestHoldNippleHandFollowRoute = "";
    private readonly List<HandMorphTarget> chestHoldFrontLeftGraspTargets = new List<HandMorphTarget>();
    private readonly List<HandMorphTarget> chestHoldFrontRightGraspTargets = new List<HandMorphTarget>();
    // v107/v110: run-serial guard. TargetGrabber can evaluate the Front route many times while one Grab is active.
    // Boost must be scoped to the Grab run, not to one function call or one hand branch.
    private int chestHoldFrontLeftGraspRunSerial = 0;
    private int chestHoldFrontLeftGraspBoostedSerial = -1;
    private int chestHoldFrontLeftGraspRestoredSerial = -1; // v115: delayed restore guard.
    private bool chestHoldFrontLeftGraspBoostActive = false;
    private bool chestHoldFrontLeftGraspHasOriginal = false;
    private float chestHoldFrontLeftGraspOriginal = 0.0f;
    private int chestHoldFrontLeftGraspRestorePendingSerial = -1; // v115: pending delayed restore serial.
    private float chestHoldFrontLeftGraspRestoreDueTime = -999.0f; // v115: Time.time when delayed restore should fire.
    private int chestHoldFrontRightGraspRunSerial = 0;
    private int chestHoldFrontRightGraspBoostedSerial = -1;
    private int chestHoldFrontRightGraspRestoredSerial = -1; // v115: delayed restore guard.
    private bool chestHoldFrontRightGraspBoostActive = false;
    private bool chestHoldFrontRightGraspHasOriginal = false;
    private float chestHoldFrontRightGraspOriginal = 0.0f;
    private int chestHoldFrontRightGraspRestorePendingSerial = -1; // v115: pending delayed restore serial.
    private float chestHoldFrontRightGraspRestoreDueTime = -999.0f; // v115: Time.time when delayed restore should fire.
    private int chestHoldFrontLeftGraspWatchSerial = -1;
    private int chestHoldFrontLeftGraspWatchFrames = 0;
    private float chestHoldFrontLeftGraspWatchDesired = 0.0f;
    private float chestHoldFrontLeftGraspWatchLast = -999.0f;
    private int chestHoldFrontRightGraspWatchSerial = -1;
    private int chestHoldFrontRightGraspWatchFrames = 0;
    private float chestHoldFrontRightGraspWatchDesired = 0.0f;
    private float chestHoldFrontRightGraspWatchLast = -999.0f;
    // v116: same grasp engine is shared by Front and Back Chest Hold.
    // Store the active route label only for clearer logs and delayed-restore/watch messages.
    private string chestHoldLeftGraspRouteLabel = "FRONT";
    private string chestHoldRightGraspRouteLabel = "FRONT";

    private readonly string[] chestHoldLeftHandGraspNames = new string[]
    {
        "Left Hand Grasp",
        "Items Left Hand Grasp",
        "Left hand Grasp",
        "Left hand grasp",
        "LeftHandGrasp",
        "leftHandGrasp",
        "lHandGrasp",
        "L Hand Grasp"
    };

    private readonly string[] chestHoldRightHandGraspNames = new string[]
    {
        "Right Hand Grasp",
        "Items Right Hand Grasp",
        "Right hand Grasp",
        "Right hand grasp",
        "RightHandGrasp",
        "rightHandGrasp",
        "rHandGrasp",
        "R Hand Grasp"
    };

    private class HandMorphTarget
    {
        public string label;
        public JSONStorableFloat jsonFloat;
        public DAZMorph morph;

        public bool IsJSON()
        {
            return jsonFloat != null;
        }

        public float ReadValue()
        {
            if (jsonFloat != null) return jsonFloat.val;
            if (morph != null) return morph.morphValue;
            return 0.0f;
        }

        public void WriteValue(float value)
        {
            if (jsonFloat != null)
            {
                try { jsonFloat.SetVal(value); }
                catch { jsonFloat.val = value; }
                return;
            }

            if (morph != null)
                morph.morphValue = value;
        }
    }

    // v17: Chest Hold Grab Hand開始時だけ、target nipple L/R のPositionStateを一時Onにする。
    // Utility nipple moveでは新規Onしない。Release/Target reset/defaultsで元の状態へ戻す。
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.PositionState> chestHoldNippleStabilizePositionStates = new Dictionary<FreeControllerV3, FreeControllerV3.PositionState>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.RotationState> chestHoldNippleStabilizeRotationStates = new Dictionary<FreeControllerV3, FreeControllerV3.RotationState>();
    private readonly List<FreeControllerV3> chestHoldNippleStabilizeControls = new List<FreeControllerV3>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.PositionState> targetLockPositionStates = new Dictionary<FreeControllerV3, FreeControllerV3.PositionState>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.RotationState> targetLockRotationStates = new Dictionary<FreeControllerV3, FreeControllerV3.RotationState>();
    private readonly List<FreeControllerV3> targetLockControls = new List<FreeControllerV3>();
    private readonly Dictionary<FreeControllerV3, Atom> pendingAutoSnapIKControls = new Dictionary<FreeControllerV3, Atom>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.PositionState> temporaryRelaxPositionStates = new Dictionary<FreeControllerV3, FreeControllerV3.PositionState>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.RotationState> temporaryRelaxRotationStates = new Dictionary<FreeControllerV3, FreeControllerV3.RotationState>();
    private readonly List<FreeControllerV3> temporaryRelaxControls = new List<FreeControllerV3>();
    // v5ay: Target Controller None Body Nudge専用。動かす体幹以外の手足IKを一時的にComplyへ逃がし、Target Releaseで戻す。
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.PositionState> targetNoneBodyRelaxPositionStates = new Dictionary<FreeControllerV3, FreeControllerV3.PositionState>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.RotationState> targetNoneBodyRelaxRotationStates = new Dictionary<FreeControllerV3, FreeControllerV3.RotationState>();
    private readonly List<FreeControllerV3> targetNoneBodyRelaxControls = new List<FreeControllerV3>();
    private Atom targetNoneBodyRelaxTargetAtom = null;
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.PositionState> swoonDropPositionStates = new Dictionary<FreeControllerV3, FreeControllerV3.PositionState>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.RotationState> swoonDropRotationStates = new Dictionary<FreeControllerV3, FreeControllerV3.RotationState>();
    private readonly List<FreeControllerV3> swoonDropControls = new List<FreeControllerV3>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.RotationState> temporaryHandRotationOffStates = new Dictionary<FreeControllerV3, FreeControllerV3.RotationState>();
    private readonly Dictionary<FreeControllerV3, PendingWristHandLock> pendingWristHandLocks = new Dictionary<FreeControllerV3, PendingWristHandLock>();
    private readonly Dictionary<FreeControllerV3, Vector3> hugBodyWristReferencePositions = new Dictionary<FreeControllerV3, Vector3>();
    private readonly Dictionary<FreeControllerV3, Vector3> hugBodyHandSnapAnchorPositions = new Dictionary<FreeControllerV3, Vector3>();
    private readonly List<FreeControllerV3> completedWristHandLocks = new List<FreeControllerV3>();
    private readonly List<FreeControllerV3> pendingSelfFollowTargets = new List<FreeControllerV3>();
    private readonly List<SelfFollowParentLink> activeSelfFollowParentLinks = new List<SelfFollowParentLink>();
    private readonly Dictionary<FreeControllerV3, SelfFollowLinkState> selfFollowOriginalLinkStates = new Dictionary<FreeControllerV3, SelfFollowLinkState>();
    private readonly Dictionary<string, FreeControllerV3> personControlCache = new Dictionary<string, FreeControllerV3>();
    private readonly Dictionary<string, FreeControllerV3> targetPersonControlCache = new Dictionary<string, FreeControllerV3>();
    private readonly HashSet<FreeControllerV3> positionStateOnControls = new HashSet<FreeControllerV3>();
    private readonly HashSet<FreeControllerV3> rotationStateOnControls = new HashSet<FreeControllerV3>();
    // Release時に、このプラグインが今回ONにしたIKだけOFF/復帰するための記録。
    // Grab Handなら手だけ、Grab Footなら足/膝だけを対象にし、未使用IKは触らない。
    private readonly HashSet<FreeControllerV3> releaseRestorePositionControls = new HashSet<FreeControllerV3>();
    private readonly HashSet<FreeControllerV3> releaseRestoreRotationControls = new HashSet<FreeControllerV3>();
    private Atom personControlCacheAtom = null;
    private Atom targetPersonControlCacheAtom = null;
    private bool personControlCacheValid = false;
    private bool targetPersonControlCacheValid = false;
    private Atom genTargetCacheAtom = null;
    private Transform genTargetCacheTransform = null;
    private bool genTargetCacheValid = false;
    private Atom anusTargetCacheAtom = null;
    private Transform anusTargetCacheTransform = null;
    private bool anusTargetCacheValid = false;
    private bool pufupufuActive = false;
    private float pufupufuElapsed = 0.0f;
    private const float PUFUPUFU_DURATION = 2.70f;
    private const float PUFUPUFU_AMPLITUDE = 0.070f;
    private Vector3 pufupufuLeftBase = Vector3.zero;
    private Vector3 pufupufuRightBase = Vector3.zero;
    private Vector3 pufupufuLeftAxis = Vector3.zero;
    private Vector3 pufupufuRightAxis = Vector3.zero;
    private bool jobActive = false;
    private float jobElapsed = 0.0f;
    private const float JOB_DURATION = 1.60f;
    private const float JOB_HAND_Y_AMPLITUDE = 0.05f;
    private const float JOB_HAND_Y_CYCLES = 4.0f;
    private const float GRAB_PULL_MAX_DISTANCE = 0.50f;
    private const float GRAB_PULL_MARGIN = 0.02f;
    private const float GRAB_HAND_OPEN_DISTANCE = 0.20f;
    private const float GRAB_HAND_PUSH_DISTANCE = 0.20f;
    private const float GRAB_HAND_VERTICAL_DISTANCE = 0.20f;
    private const float GRAB_HAND_HORIZONTAL_DISTANCE = 0.20f;
    private const float UPPER_BODY_PIVOT_MAX_DEGREES = 12.0f;
    private const float UPPER_BODY_PIVOT_DEGREES_PER_METER = 70.0f;
    private Vector3 jobLeftBase = Vector3.zero;
    private Vector3 jobRightBase = Vector3.zero;
    private float lastSideDebugTime = -10.0f;
    private float lastHandRotationDebugTime = -10.0f;
    private float lastHandTargetDebugTime = -10.0f;
    private float lastRouteCheckLogTime = -10.0f;
    private bool releaseRestoreIKPending = false;
    private float releaseRestoreIKTime = 0.0f;
    private ColorBlock releaseTargetDefaultColors;
    private ColorBlock releaseDefaultColors;
    private ColorBlock swoonDropDefaultColors;
    private bool releaseTargetColorsCaptured = false;
    private bool releaseColorsCaptured = false;
    private bool swoonDropColorsCaptured = false;
    private const float RELEASE_RESTORE_IK_DELAY = 3.00f;
    private const float SWOON_DROP_DURATION = 3.00f;
    private const float TARGET_SWOON_HUG_BODY_ONE_HAND_TWIST_OFFSET = 0.12f;
    private const float HUG_BODY_PUSH_PULL_PIVOT_ANGLE_MULTIPLIER = 2.00f;
    private const bool HUG_BODY_KEEP_TARGET_NECK_HEAD_STABLE = true;
    private const bool HUG_BODY_PULL_PUSH_RELAX_TARGET_NECK_HEAD_IK = true;
    private const float HUG_BODY_HDC_HIP_UPPER_LIMIT_X_DEGREES = 120.0f;
    private const float HUG_BODY_HDC_HIP_UPPER_ONE_HAND_YAW_SCALE = 0.85f;
    private bool hugBodyHdcHipUpperActive = false;
    private Atom hugBodyHdcHipUpperSelfAtom = null;
    private Atom hugBodyHdcHipUpperTargetAtom = null;
    private float hugBodyHdcHipUpperRotX = 0.0f;
    private float hugBodyHdcHipUpperRotY = 0.0f;
    private bool hugBodyHdcHipUpperBaseCaptured = false;
    private FreeControllerV3 hugBodyHdcHipUpperRoot = null;
    private Vector3 hugBodyHdcHipUpperRootBaseLocalPos = Vector3.zero;
    private Quaternion hugBodyHdcHipUpperRootBaseLocalRot = Quaternion.identity;
    private readonly Dictionary<FreeControllerV3, Vector3> hugBodyHdcHipUpperBaseLocalPositions = new Dictionary<FreeControllerV3, Vector3>();
    private readonly Dictionary<FreeControllerV3, Quaternion> hugBodyHdcHipUpperBaseLocalRotations = new Dictionary<FreeControllerV3, Quaternion>();
    private bool hugBodyHdcLeftHandBaseValid = false;
    private bool hugBodyHdcRightHandBaseValid = false;
    private Vector3 hugBodyHdcLeftHandBaseWorldPos = Vector3.zero;
    private Vector3 hugBodyHdcRightHandBaseWorldPos = Vector3.zero;
    private FreeControllerV3 hugBodyHdcHandFollowChest = null;
    private bool hugBodyHdcLeftHandChestLocalValid = false;
    private bool hugBodyHdcRightHandChestLocalValid = false;
    private Vector3 hugBodyHdcLeftHandChestLocalOffset = Vector3.zero;
    private Vector3 hugBodyHdcRightHandChestLocalOffset = Vector3.zero;
    private bool swoonDropActive = false;
    private float swoonDropEndTime = 0.0f;

    public override void Init()
    {
        personChooser = new JSONStorableStringChooser(
            "person",
            new List<string>(),
            NONE,
            "Person",
            (JSONStorableStringChooser.SetStringCallback)OnPersonChanged
        );
        RegisterStringChooser(personChooser);
        // v4.0cy: hide the top-left self Person popup. The chooser stays registered so saves/external changes still work.
        // UIDynamicPopup personPopup = CreateFilterablePopup(personChooser);
        // if (personPopup != null)
        //     personPopup.popup.onOpenPopupHandlers += UpdatePersonChoices;

        targetTypeChooser = new JSONStorableStringChooser(
            "targetType",
            new List<string> { "Atom", "Person" },
            "Person",
            "Target Type",
            (JSONStorableStringChooser.SetStringCallback)OnTargetTypeChanged
        );
        RegisterStringChooser(targetTypeChooser);
        CreatePopup(targetTypeChooser, false);

        targetAtomChooser = new JSONStorableStringChooser(
            "targetAtom",
            new List<string>(),
            NONE,
            "Target Atom",
            (JSONStorableStringChooser.SetStringCallback)OnTargetAtomChanged
        );
        RegisterStringChooser(targetAtomChooser);
        UIDynamicPopup atomPopup = CreateFilterablePopup(targetAtomChooser);
        if (atomPopup != null)
            atomPopup.popup.onOpenPopupHandlers += UpdateAtomChoices;

        targetPersonChooser = new JSONStorableStringChooser(
            "targetPerson",
            new List<string>(),
            NONE,
            "Target Person",
            (JSONStorableStringChooser.SetStringCallback)OnTargetPersonChanged
        );
        RegisterStringChooser(targetPersonChooser);
        UIDynamicPopup targetPersonPopup = CreateFilterablePopup(targetPersonChooser);
        if (targetPersonPopup != null)
            targetPersonPopup.popup.onOpenPopupHandlers += UpdateTargetPersonChoices;

        targetPersonPartChooser = new JSONStorableStringChooser(
            "targetPersonController",
            new List<string> { NONE, DEFAULT_TARGET_CONTROLLER },
            DEFAULT_TARGET_CONTROLLER,
            "IK Select",
            (JSONStorableStringChooser.SetStringCallback)OnTargetPersonPartChanged
        );
        RegisterStringChooser(targetPersonPartChooser);
        // v4.0da: keep the IK selection popup visible and place it above the shortcut buttons.
        UIDynamicPopup targetControllerPopup = CreateFilterablePopup(targetPersonPartChooser);
        if (targetControllerPopup != null)
            targetControllerPopup.popup.onOpenPopupHandlers += UpdateTargetPersonControllerChoices;

        // v4.0da: Target shortcut buttons under IK Select.
        CreateButton("Head", false).button.onClick.AddListener(delegate { SetTargetControllerShortcut(TC_HEAD); });
        CreateButton("Chest Hold", false).button.onClick.AddListener(delegate { SetTargetControllerShortcut(TC_CHEST_HOLD); });
        CreateButton("Hug Body", false).button.onClick.AddListener(delegate { SetTargetControllerShortcut(TC_HUG_BODY); });
        CreateButton("Hip Hold", false).button.onClick.AddListener(delegate { SetTargetControllerShortcut(TC_HIP_HOLD); });
        CreateButton("Hand Hold", false).button.onClick.AddListener(delegate { SetTargetControllerShortcut(TC_HAND); });
        CreateButton("Foot Hold", false).button.onClick.AddListener(delegate { SetTargetControllerShortcut(TC_FOOT); });

        targetControllerFilterJSON = new JSONStorableString("Target Ctrl Filter", "");
        RegisterString(targetControllerFilterJSON);
        // Target Ctrl Filter is kept for compatibility but no longer shown.

        // v4.0da: release/default controls first; the less-used follow selector is moved below them.
        followTargetJSON = new JSONStorableBool("Follow Target", false);
        RegisterBool(followTargetJSON);
        followTargetJSON.setCallbackFunction = OnLegacyFollowTargetChanged;

        releaseButton = CreateButton("Self Release", false);
        releaseButton.button.onClick.AddListener(Release);
        CaptureButtonDefaultColors(releaseButton, ref releaseDefaultColors, ref releaseColorsCaptured);

        CreateButton("Self IK Defaults", false).button.onClick.AddListener(SelfIKDefault);
        CreateButton("Self Load User Defaults", false).button.onClick.AddListener(LoadUserDefaults);

        releaseTargetButton = CreateButton("Target Release", false);
        releaseTargetButton.button.onClick.AddListener(ReleaseTarget);
        CaptureButtonDefaultColors(releaseTargetButton, ref releaseTargetDefaultColors, ref releaseTargetColorsCaptured);

        CreateButton("Target IK Default", false).button.onClick.AddListener(TargetIKDefault);
        CreateButton("Target Load User Defaults", false).button.onClick.AddListener(TargetLoadUserDefaults);

        followModeChooser = new JSONStorableStringChooser(
            "Follow Mode",
            new List<string> { FOLLOW_OFF, FOLLOW_SELF, FOLLOW_TARGET },
            FOLLOW_OFF,
            "Grab Follow",
            (JSONStorableStringChooser.SetStringCallback)OnFollowModeChanged
        );
        RegisterStringChooser(followModeChooser);
        CreatePopup(followModeChooser, false);

        debugLogJSON = CreateBool("Debug Log", false, false);

        tgHeldTargetHandJSON = new JSONStorableBool("TG Held Target Hand", false);
        RegisterBool(tgHeldTargetHandJSON);
        tgHeldTargetLHandJSON = new JSONStorableBool("TG Held Target L Hand", false);
        RegisterBool(tgHeldTargetLHandJSON);
        tgHeldTargetRHandJSON = new JSONStorableBool("TG Held Target R Hand", false);
        RegisterBool(tgHeldTargetRHandJSON);
        tgHeldTargetPersonUidJSON = new JSONStorableString("TG Held Target Person UID", "");
        RegisterString(tgHeldTargetPersonUidJSON);

        CreateButton("Refresh", false).button.onClick.AddListener(RefreshAll);
        CreateButton("Default", false).button.onClick.AddListener(ApplyDefaultSettings);
        CreateButton("Log Target Intimate Names", false).button.onClick.AddListener(LogTargetIntimateNames);

        // 左側UI: Target / Motion / Advanced。
        // Foot Sole系は左下へ寄せる。
        grabWidthJSON = CreateFloat("Grab Width", 1.60f, 0.0f, 2.00f, false);
        finalGrabWidthJSON = CreateFloat("Final Grab Width", 0.10f, MIN_FINAL_GRAB_WIDTH, 2.00f, false);
        autoGrabWidthJSON = CreateBool("Auto Grab Width", true, false);
        grabCloseSpeedJSON = CreateFloat("Grab Close Speed", 5.0f, 0.1f, 20.0f, false);
        moveTimeJSON = CreateFloat("Move Time Sec", 0.50f, 0.05f, 10.00f, false);
        CreateButton("Kiss", true).button.onClick.AddListener(GrabHead);
        hugModeJSON = CreateBool("Hug Mode", false, true);
        hugDepthJSON = CreateFloat("Hug Depth", -1.00f, -1.00f, 1.00f, false);

        targetZOffsetJSON = CreateFloat("Target Z Offset", 0.00f, -1.00f, 1.00f, false);
        autoZOffsetJSON = CreateFloat("Auto Z Offset", 0.00f, -1.00f, 1.00f, false);
        maxHandReachJSON = CreateFloat("Max Hand Reach", 0.70f, 0.10f, 2.00f, false);
        maxFootReachJSON = CreateFloat("Max Foot Reach", 0.80f, 0.10f, 2.00f, false);
        maxHeadReachJSON = CreateFloat("Max Head Reach", 0.45f, 0.10f, 1.50f, false);
        headTargetDistanceJSON = CreateFloat("Head Target Distance", 0.12f, 0.00f, 0.50f, false);
        kissFaceAlignJSON = CreateBool("Kiss Face Align", true, false);
        kissFaceStrengthJSON = CreateFloat("Kiss Face Strength", 0.70f, 0.00f, 1.00f, false);

        alignHandPalmJSON = CreateBool("Align Hand Palm", true, false);
        handPalmOffsetJSON = CreateFloat("Hand Palm Offset", 0.00f, -0.30f, 0.30f, false);
        handCenterOffsetJSON = CreateFloat("Hand Center Offset", 0.00f, -0.20f, 0.20f, false);
        chestHoldBackReachFrontAdjustJSON = CreateFloat("Chest Hold Back Nipple Offset", CHEST_HOLD_BACK_REACH_FRONT_ADJUST_DEFAULT, -0.100f, 0.100f, false);
        handRotXJSON = CreateFloat("Hand Palm Add Rot X", 0.0f, -180.0f, 180.0f, false);
        handRotYJSON = CreateFloat("Hand Palm Add Rot Y", 0.0f, -180.0f, 180.0f, false);
        handRotZJSON = CreateFloat("Hand Palm Add Rot Z", 0.0f, -180.0f, 180.0f, false);

        alignFootSoleJSON = CreateBool("Align Foot Sole", false, false);
        footSoleOffsetJSON = CreateFloat("Foot Sole Offset", 0.08f, -0.30f, 0.30f, false);
        footArcWidthJSON = CreateFloat("Foot Arc Width", 0.30f, 0.00f, 2.00f, false);
        footArcDropJSON = CreateFloat("Foot Arc Drop", 0.10f, 0.00f, 1.00f, false);
        kneeWidthMultiplierJSON = CreateFloat("Knee Width Multiplier", 1.50f, 0.00f, 4.00f, false);
        footRotXJSON = CreateFloat("Foot Sole Rot X", 0.0f, -180.0f, 180.0f, false);
        footRotYJSON = CreateFloat("Foot Sole Rot Y", 0.0f, -180.0f, 180.0f, false);
        footRotZJSON = CreateFloat("Foot Sole Rot Z", 0.0f, -180.0f, 180.0f, false);

        // v5an: keep only Grab Hand one-shot pelvis auto.
        // Persistent/manual pelvis hold was removed because TargetLinePerson owns pelvis hold during Axis Align.
        // These legacy storables stay registered only so old scene JSON does not break; they are not shown and are never held in LateUpdate.
        targetPelvisRotXEnableJSON = new JSONStorableBool("Target Pelvis Rot X ON", false);
        RegisterBool(targetPelvisRotXEnableJSON);

        targetPelvisRotXJSON = new JSONStorableFloat("Target Pelvis Rot X 90 Back / 180 Center / 270 Near", 180.0f, 90.0f, 270.0f, true, true);
        RegisterFloat(targetPelvisRotXJSON);

        targetPelvisAutoOnGrabJSON = new JSONStorableBool("Pelvis Auto On Grab", true);
        RegisterBool(targetPelvisAutoOnGrabJSON);
        CreateToggle(targetPelvisAutoOnGrabJSON, false);

        // v5ao: temporary verification buttons. All are one-shot and do not start any pelvis hold.
        CreateButton("Pelvis Auto Test", false).button.onClick.AddListener(TargetPelvisAutoTest);
        CreateButton("Pelvis Log", false).button.onClick.AddListener(TargetPelvisLog);
        CreateButton("Pelvis 90 Back Test", false).button.onClick.AddListener(TargetPelvis90BackTest);
        CreateButton("Pelvis 270 Near Test", false).button.onClick.AddListener(TargetPelvis270NearTest);
        CreateButton("Pelvis Face Self Test", false).button.onClick.AddListener(TargetPelvisFaceSelfTest);
        CreateButton("Pelvis Face Self +180 Test", false).button.onClick.AddListener(TargetPelvisFaceSelf180Test);

        // 右側UI: 操作系だけを上から順にまとめる。
        CreateButton("Grab Hand", true).button.onClick.AddListener(GrabHand);
        grabHandPullButton = CreateButton("Grab Hand Pull", true);
        grabHandPullButton.button.onClick.AddListener(GrabHandPull);
        grabHandPushButton = CreateButton("Grab Hand Push", true);
        grabHandPushButton.button.onClick.AddListener(GrabHandPush);
        grabHandUpButton = CreateButton("Grab Hand Up", true);
        grabHandUpButton.button.onClick.AddListener(GrabHandUp);
        grabHandDownButton = CreateButton("Grab Hand Down", true);
        grabHandDownButton.button.onClick.AddListener(GrabHandDown);
        grabHandOpenButton = CreateButton("Grab Hand Open", true);
        grabHandOpenButton.button.onClick.AddListener(GrabHandOpen);
        grabHandCloseButton = CreateButton("Grab Hand Close", true);
        grabHandCloseButton.button.onClick.AddListener(GrabHandClose);
        grabHandLeftButton = CreateButton("Grab Hand Left", true);
        grabHandLeftButton.button.onClick.AddListener(GrabHandLeft);
        grabHandRightButton = CreateButton("Grab Hand Right", true);
        grabHandRightButton.button.onClick.AddListener(GrabHandRight);
        leftHandJSON = CreateBool("Left Hand", true, true);
        rightHandJSON = CreateBool("Right Hand", true, true);

        CreateButton("Grab Foot", true).button.onClick.AddListener(GrabFoot);
        leftFootJSON = CreateBool("Left Foot", true, true);
        rightFootJSON = CreateBool("Right Foot", true, true);

        // Grab Selected remains as an external action, but the visible slot is now Target Swoon Drop.
        swoonDropButton = CreateButton("Target Swoon Drop", true);
        swoonDropButton.button.onClick.AddListener(ToggleSwoonDrop);
        CaptureButtonDefaultColors(swoonDropButton, ref swoonDropDefaultColors, ref swoonDropColorsCaptured);
        CreateButton("pufupufu", true).button.onClick.AddListener(Pufupufu);
        CreateButton("job", true).button.onClick.AddListener(Job);
        autoSnapPullOpenIKJSON = CreateBool("Auto Snap Pull/Open IK", true, true);

        handWristAngleJSON = CreateBool("Use Hand Wrist Angle", true, true);
        CreateButton("Wrist Straight", true).button.onClick.AddListener(delegate { ApplyBothHandWristTest("Straight"); });
        CreateButton("Wrist In", true).button.onClick.AddListener(delegate { ApplyBothHandWristTest("In"); });
        CreateButton("Wrist In2", true).button.onClick.AddListener(delegate { ApplyBothHandWristTest("In2"); });
        CreateButton("Wrist Out", true).button.onClick.AddListener(delegate { ApplyBothHandWristTest("Out"); });
        CreateButton("Wrist Up", true).button.onClick.AddListener(delegate { ApplyBothHandWristTest("Up"); });
        CreateButton("Wrist Down", true).button.onClick.AddListener(delegate { ApplyBothHandWristTest("Down"); });

        statusJSON = new JSONStorableString("Status", "Ready");
        RegisterString(statusJSON);
        UIDynamicTextField statusField = CreateTextField(statusJSON, false);
        if (statusField != null)
            statusField.height = 80.0f;

        RegisterExternalActions();

        RefreshAll();

        DebugLog("ready / v88_chest_hold_front_step2_step3_inward / based-on-v87 / back-fix-kept / front-step2-down10-front1-in3-step3-down8-front6-in3");
    }

    private void RegisterExternalActions()
    {
        RegisterAction(new JSONStorableAction("Refresh", RefreshAll));
        RegisterAction(new JSONStorableAction("Default", ApplyDefaultSettings));
        RegisterAction(new JSONStorableAction("Log Target Intimate Names", LogTargetIntimateNames));
        RegisterAction(new JSONStorableAction("Pelvis Auto On Grab ON", delegate { if (targetPelvisAutoOnGrabJSON != null) targetPelvisAutoOnGrabJSON.val = true; }));
        RegisterAction(new JSONStorableAction("Pelvis Auto On Grab OFF", delegate { if (targetPelvisAutoOnGrabJSON != null) targetPelvisAutoOnGrabJSON.val = false; }));
        RegisterAction(new JSONStorableAction("Pelvis Auto Test", TargetPelvisAutoTest));
        RegisterAction(new JSONStorableAction("Pelvis Log", TargetPelvisLog));
        RegisterAction(new JSONStorableAction("Pelvis 90 Back Test", TargetPelvis90BackTest));
        RegisterAction(new JSONStorableAction("Pelvis 270 Near Test", TargetPelvis270NearTest));
        RegisterAction(new JSONStorableAction("Pelvis Face Self Test", TargetPelvisFaceSelfTest));
        RegisterAction(new JSONStorableAction("Pelvis Face Self +180 Test", TargetPelvisFaceSelf180Test));
        RegisterAction(new JSONStorableAction("Kiss", GrabHead));
        RegisterAction(new JSONStorableAction("Grab Hand", GrabHand));
        RegisterAction(new JSONStorableAction("Wrist Straight", delegate { ApplyBothHandWristTest("Straight"); }));
        RegisterAction(new JSONStorableAction("Wrist In", delegate { ApplyBothHandWristTest("In"); }));
        RegisterAction(new JSONStorableAction("Wrist In2", delegate { ApplyBothHandWristTest("In2"); }));
        RegisterAction(new JSONStorableAction("Wrist Out", delegate { ApplyBothHandWristTest("Out"); }));
        RegisterAction(new JSONStorableAction("Wrist Up", delegate { ApplyBothHandWristTest("Up"); }));
        RegisterAction(new JSONStorableAction("Wrist Down", delegate { ApplyBothHandWristTest("Down"); }));
        RegisterAction(new JSONStorableAction("Grab Head", GrabHead));
        RegisterAction(new JSONStorableAction("Grab Hand Pull", GrabHandPull));
        RegisterAction(new JSONStorableAction("Grab Pull", GrabHandPull));
        RegisterAction(new JSONStorableAction("Grab Hand Push", GrabHandPush));
        RegisterAction(new JSONStorableAction("Grab Push", GrabHandPush));
        RegisterAction(new JSONStorableAction("Grab Hand Up", GrabHandUp));
        RegisterAction(new JSONStorableAction("Grab Up", GrabHandUp));
        RegisterAction(new JSONStorableAction("Grab Hand Down", GrabHandDown));
        RegisterAction(new JSONStorableAction("Grab Down", GrabHandDown));
        RegisterAction(new JSONStorableAction("Grab Hand Open", GrabHandOpen));
        RegisterAction(new JSONStorableAction("Grab Hand Close", GrabHandClose));
        RegisterAction(new JSONStorableAction("Grab Close", GrabHandClose));
        RegisterAction(new JSONStorableAction("Grab Hand Left", GrabHandLeft));
        RegisterAction(new JSONStorableAction("Grab Left", GrabHandLeft));
        RegisterAction(new JSONStorableAction("Grab Hand Right", GrabHandRight));
        RegisterAction(new JSONStorableAction("Grab Right", GrabHandRight));
        RegisterAction(new JSONStorableAction("Grab Left Hand", GrabLeftHand));
        RegisterAction(new JSONStorableAction("Grab Right Hand", GrabRightHand));
        RegisterAction(new JSONStorableAction("Grab Foot", GrabFoot));
        RegisterAction(new JSONStorableAction("Grab Left Foot", GrabLeftFoot));
        RegisterAction(new JSONStorableAction("Grab Right Foot", GrabRightFoot));
        RegisterAction(new JSONStorableAction("Grab Selected", GrabSelected));
        RegisterAction(new JSONStorableAction("Target Swoon Drop", ToggleSwoonDrop));
        RegisterAction(new JSONStorableAction("Target Swoon Stop", StopSwoonDropAction));
        RegisterAction(new JSONStorableAction("Swoon Drop", ToggleSwoonDrop));
        RegisterAction(new JSONStorableAction("Swoon Stop", StopSwoonDropAction));
        RegisterAction(new JSONStorableAction("pufupufu", Pufupufu));
        RegisterAction(new JSONStorableAction("job", Job));
        RegisterAction(new JSONStorableAction("Release Target", ReleaseTarget));
        RegisterAction(new JSONStorableAction("Target Release", ReleaseTarget));
        RegisterAction(new JSONStorableAction("Release", Release));
        RegisterAction(new JSONStorableAction("Self Release", Release));
        RegisterAction(new JSONStorableAction("Self IK Default", SelfIKDefault));
        RegisterAction(new JSONStorableAction("Self IK Defaults", SelfIKDefault));
        RegisterAction(new JSONStorableAction("Load User Defaults", LoadUserDefaults));
        RegisterAction(new JSONStorableAction("Self Load User Defaults", LoadUserDefaults));
        RegisterAction(new JSONStorableAction("LoadUserDefaults", LoadUserDefaults));
        RegisterAction(new JSONStorableAction("Target IK Default", TargetIKDefault));
        RegisterAction(new JSONStorableAction("Target Load User Defaults", TargetLoadUserDefaults));
        RegisterAction(new JSONStorableAction("Target Load Defaults", TargetLoadUserDefaults));
        RegisterAction(new JSONStorableAction("TargetLoadDefaults", TargetLoadUserDefaults));
        RegisterAction(new JSONStorableAction("Target Shortcut None", delegate { SetTargetControllerShortcut(NONE); }));
        RegisterAction(new JSONStorableAction("Target Shortcut Head", delegate { SetTargetControllerShortcut(TC_HEAD); }));
        RegisterAction(new JSONStorableAction("Target Shortcut Chest Hold", delegate { SetTargetControllerShortcut(TC_CHEST_HOLD); }));
        RegisterAction(new JSONStorableAction("Target Shortcut Hug Body", delegate { SetTargetControllerShortcut(TC_HUG_BODY); }));
        RegisterAction(new JSONStorableAction("Target Shortcut Hip Hold", delegate { SetTargetControllerShortcut(TC_HIP_HOLD); }));
        RegisterAction(new JSONStorableAction("Target Shortcut Hand Hold", delegate { SetTargetControllerShortcut(TC_HAND); }));
        RegisterAction(new JSONStorableAction("Target Shortcut Foot Hold", delegate { SetTargetControllerShortcut(TC_FOOT); }));
        RegisterAction(new JSONStorableAction("Target Shortcut Knee Hold", delegate { SetTargetControllerShortcut(TC_KNEE); }));
        RegisterHduGrabHandRouteActions();
    }

    private void RegisterHduGrabHandRouteActions()
    {
        // v5az: HDU_Commander can now route TargetGrabber=None into the same Body Nudge path
        // used by the visible TargetGrabber Grab Hand utility buttons.
        RegisterHduGrabHandRouteActionsForTarget("None", NONE);
        RegisterHduGrabHandRouteActionsForTarget("Head", TC_HEAD);
        RegisterHduGrabHandRouteActionsForTarget("Neck", TC_NECK);
        RegisterHduGrabHandRouteActionsForTarget("Chest Hold", TC_CHEST_HOLD);
        RegisterHduGrabHandRouteActionsForTarget("Hug Body", TC_HUG_BODY);
        RegisterHduGrabHandRouteActionsForTarget("Hip Hold", TC_HIP_HOLD);
        RegisterHduGrabHandRouteActionsForTarget("Hand Hold", TC_HAND);
        RegisterHduGrabHandRouteActionsForTarget("Foot Hold", TC_FOOT);
        RegisterHduGrabHandRouteActionsForTarget("Knee Hold", TC_KNEE);
    }

    private void RegisterHduGrabHandRouteActionsForTarget(string label, string targetChoice)
    {
        RegisterAction(new JSONStorableAction("HDU Grab Hand " + label, delegate { HduSelectTargetAndRun(targetChoice, GrabHand, "Grab Hand"); }));
        RegisterAction(new JSONStorableAction("HDU Grab Hand Pull " + label, delegate { HduSelectTargetAndRun(targetChoice, GrabHandPull, "Grab Hand Pull"); }));
        RegisterAction(new JSONStorableAction("HDU Grab Hand Push " + label, delegate { HduSelectTargetAndRun(targetChoice, GrabHandPush, "Grab Hand Push"); }));
        RegisterAction(new JSONStorableAction("HDU Grab Hand Up " + label, delegate { HduSelectTargetAndRun(targetChoice, GrabHandUp, "Grab Hand Up"); }));
        RegisterAction(new JSONStorableAction("HDU Grab Hand Down " + label, delegate { HduSelectTargetAndRun(targetChoice, GrabHandDown, "Grab Hand Down"); }));
        RegisterAction(new JSONStorableAction("HDU Grab Hand Open " + label, delegate { HduSelectTargetAndRun(targetChoice, GrabHandOpen, "Grab Hand Open"); }));
        RegisterAction(new JSONStorableAction("HDU Grab Hand Close " + label, delegate { HduSelectTargetAndRun(targetChoice, GrabHandClose, "Grab Hand Close"); }));
        RegisterAction(new JSONStorableAction("HDU Grab Hand Left " + label, delegate { HduSelectTargetAndRun(targetChoice, GrabHandLeft, "Grab Hand Left"); }));
        RegisterAction(new JSONStorableAction("HDU Grab Hand Right " + label, delegate { HduSelectTargetAndRun(targetChoice, GrabHandRight, "Grab Hand Right"); }));
    }

    private void HduSelectTargetAndRun(string targetChoice, Action action, string actionLabel)
    {
        if (!HduSelectTargetControllerInternal(targetChoice))
            return;

        if (action == null)
        {
            SetStatus("HDU action missing: " + actionLabel);
            return;
        }

        DebugLog("[HDU ROUTE] run / target=" + targetChoice + " / action=" + actionLabel);
        action();
    }

    private bool HduSelectTargetControllerInternal(string targetChoice)
    {
        if (string.IsNullOrEmpty(targetChoice))
        {
            SetStatus("HDU target missing");
            return false;
        }

        // v5az: Target Controller <none> is a valid HDU route.
        // It is required for None Body Nudge from HDU_Commander.
        if (targetChoice == "None")
            targetChoice = NONE;

        suppressApply = true;
        try
        {
            if (targetTypeChooser != null)
                targetTypeChooser.val = "Person";
        }
        finally
        {
            suppressApply = false;
        }

        UpdateTargetPersonChoices();
        UpdateTargetPersonControllerChoices();

        if (targetPersonPartChooser == null || targetPersonPartChooser.choices == null || !targetPersonPartChooser.choices.Contains(targetChoice))
        {
            SetStatus("HDU target unavailable: " + targetChoice);
            DebugLog("[HDU ROUTE] target unavailable / target=" + targetChoice);
            return false;
        }

        suppressApply = true;
        try
        {
            targetPersonPartChooser.val = targetChoice;
        }
        finally
        {
            suppressApply = false;
        }

        OnTargetPersonPartChanged(targetChoice);
        ResolveControls();
        ApplyAutoGrabWidthFromTargetPerson();
        UpdateGrabHandUtilityButtons();
        SetStatus("HDU Target: " + targetChoice);
        DebugLog("[HDU ROUTE] target selected / target=" + targetChoice);
        return true;
    }

    private void OnLegacyFollowTargetChanged(bool value)
    {
        if (suppressApply || !value || followModeChooser == null)
            return;

        followModeChooser.val = FOLLOW_TARGET;
    }

    private void OnFollowModeChanged(string value)
    {
        if (value != FOLLOW_SELF)
            RestoreSelfFollowParentLinks();
        else if (hasActiveGrab)
            QueueSelfFollowParentTargets(GetSelfFollowTargetControls());

        if (!suppressApply && IsFollowTargetMode() && hasActiveGrab)
            ApplyGrab(false, activeIncludeHands, activeIncludeFeet, activeIncludeHead);
    }

    private string GetFollowMode()
    {
        return followModeChooser != null && !string.IsNullOrEmpty(followModeChooser.val)
            ? followModeChooser.val
            : FOLLOW_OFF;
    }

    private bool IsFollowSelfMode()
    {
        return GetFollowMode() == FOLLOW_SELF;
    }

    private bool IsFollowTargetMode()
    {
        return GetFollowMode() == FOLLOW_TARGET;
    }

    private JSONStorableBool CreateBool(string name, bool def, bool rightSide)
    {
        JSONStorableBool js = new JSONStorableBool(name, def);
        RegisterBool(js);
        js.setCallbackFunction = delegate(bool v)
        {
            if (!suppressApply && name == "Auto Grab Width" && v)
            {
                ApplyAutoGrabWidthFromTargetAtom();
                ApplyAutoGrabWidthFromTargetPerson();
            }

            if (!suppressApply && IsFollowTargetMode() && hasActiveGrab)
                ApplyGrab(false);
        };
        CreateToggle(js, rightSide);
        return js;
    }

    private JSONStorableFloat CreateFloat(string name, float def, float min, float max, bool rightSide)
    {
        JSONStorableFloat js = new JSONStorableFloat(name, def, min, max, true, true);
        RegisterFloat(js);
        js.setCallbackFunction = delegate(float v)
        {
            if (!suppressApply && IsFollowTargetMode() && hasActiveGrab)
                ApplyGrab(false);
        };
        CreateSlider(js, rightSide);
        return js;
    }

    private void RefreshAll()
    {
        UpdatePersonChoices();
        UpdateAtomChoices();
        UpdateTargetPersonChoices();
        UpdateTargetPersonControllerChoices();
        ResolveControls();
        UpdateGrabHandUtilityButtons();
        SetStatus("Refreshed");
    }

    private void UpdatePersonChoices()
    {
        if (personChooser == null)
            return;

        string current = personChooser.val;
        List<string> choices = new List<string>();

        choices.AddRange(
            SuperController.singleton.GetAtoms()
                .Where(a => a != null && a.type == "Person")
                .Select(a => a.uid)
                .OrderBy(x => x)
        );

        if (choices.Count == 0)
            choices.Add(NONE);

        personChooser.choices = choices;

        string selfUid = containingAtom != null && containingAtom.type == "Person" ? containingAtom.uid : NONE;
        string next = choices.Contains(selfUid) && selfUid != NONE
            ? selfUid
            : choices.Contains(current) && current != NONE
                ? current
                : choices.FirstOrDefault(x => x != NONE) ?? NONE;

        personChooser.val = next;
        OnPersonChanged(next);
    }

    private void UpdateAtomChoices()
    {
        if (targetAtomChooser == null)
            return;

        string current = targetAtomChooser.val;
        List<string> choices = new List<string> { NONE };
        Vector3 selfPosition = GetSelfReferencePosition();

        choices.AddRange(
            SuperController.singleton.GetAtoms()
                .Where(a => a != null && a != containingAtom && a.type != "Person")
                .OrderBy(a => GetDistanceSqrToSelf(a, selfPosition))
                .ThenBy(a => a.uid)
                .Select(a => a.uid)
        );

        targetAtomChooser.choices = choices;

        string next = choices.Contains(current) && current != NONE
            ? current
            : choices.FirstOrDefault(x => x != NONE) ?? NONE;

        targetAtomChooser.val = next;
        OnTargetAtomChanged(next);
    }

    private Vector3 GetSelfReferencePosition()
    {
        if (selectedPerson != null && selectedPerson.transform != null)
            return selectedPerson.transform.position;

        if (containingAtom != null && containingAtom.transform != null)
            return containingAtom.transform.position;

        return Vector3.zero;
    }

    private float GetDistanceSqrToSelf(Atom atom, Vector3 selfPosition)
    {
        if (atom == null || atom.transform == null)
            return float.MaxValue;

        return (atom.transform.position - selfPosition).sqrMagnitude;
    }

    private void OnPersonChanged(string uid)
    {
        selectedPerson = string.IsNullOrEmpty(uid) || uid == NONE
            ? null
            : SuperController.singleton.GetAtomByUid(uid);

        InvalidatePersonControlCache();
        ResolveControls();
        UpdateTargetPersonChoices();
        UpdateGrabHandUtilityButtons();
    }

    private void OnTargetTypeChanged(string type)
    {
        UpdateTargetPersonControllerChoices();
        ApplyAutoGrabWidthFromTargetAtom();
        ApplyAutoGrabWidthFromTargetPerson();
        UpdateGrabHandUtilityButtons();

        if (!suppressApply && IsFollowTargetMode() && hasActiveGrab)
            ApplyGrab(false);
    }

    private bool IsTargetPersonMode()
    {
        return targetTypeChooser != null && targetTypeChooser.val == "Person";
    }

    private void OnTargetAtomChanged(string uid)
    {
        selectedTargetAtom = string.IsNullOrEmpty(uid) || uid == NONE
            ? null
            : SuperController.singleton.GetAtomByUid(uid);

        ApplyAutoGrabWidthFromTargetAtom();

        if (!suppressApply && IsFollowTargetMode() && hasActiveGrab)
            ApplyGrab(false);
    }

    private void UpdateTargetPersonChoices()
    {
        if (targetPersonChooser == null)
            return;

        string current = targetPersonChooser.val;
        string selfUid = selectedPerson != null ? selectedPerson.uid : containingAtom != null && containingAtom.type == "Person" ? containingAtom.uid : NONE;
        List<string> choices = new List<string>();

        choices.AddRange(
            SuperController.singleton.GetAtoms()
                .Where(a => a != null && a.type == "Person")
                .Select(a => a.uid)
                .OrderBy(x => x)
        );

        if (choices.Count == 0)
            choices.Add(NONE);

        targetPersonChooser.choices = choices;

        string next = choices.Contains(current) && current != NONE
            ? current
            : choices.FirstOrDefault(x => x != NONE && x != selfUid) ??
                (choices.Contains(selfUid) ? selfUid : NONE);

        targetPersonChooser.val = next;
        OnTargetPersonChanged(next);
    }

    private void OnTargetPersonChanged(string uid)
    {
        if (targetNoneBodyRelaxTargetAtom != null && (string.IsNullOrEmpty(uid) || uid == NONE || targetNoneBodyRelaxTargetAtom.uid != uid))
            RestoreTargetNoneBodyRelaxIK();

        selectedTargetPerson = string.IsNullOrEmpty(uid) || uid == NONE
            ? null
            : SuperController.singleton.GetAtomByUid(uid);

        InvalidateTargetPersonControlCache();
        UpdateTargetPersonControllerChoices();
        ApplyAutoGrabWidthFromTargetPerson();
        UpdateGrabHandUtilityButtons();

        if (!suppressApply && IsFollowTargetMode() && hasActiveGrab)
            ApplyGrab(false);
    }

    private void OnTargetPersonPartChanged(string part)
    {
        DebugLog("[TARGET CONTROLLER] raw=" + (part ?? "<null>") +
            " key=" + NormalizeControllerKey(part) +
            " nipple=" + Bool01(IsNipplePairControlName(part)));

        // v22: Chest Hold の nipple安定化/直接移動状態は、IK Select が Chest Hold を離れた時点で必ず解除する。
        // これを遅らせると、Hug Bodyへ切替後も nipple L/R の一時ONや移動キャッシュが残り、
        // Hug Body開始時に R nipple へ吸われるように見えることがある。
        if (part != TC_CHEST_HOLD)
        {
            int restored = RestoreChestHoldNippleIKStabilize("target-controller-change-to-" + (part ?? "<null>"));
            chestHoldNoIkNippleMoveControls.Clear();
            if (restored > 0 || IsDebugEnabled())
            {
                DebugLog("[CHEST HOLD ISOLATE] cleared on target change / next=" + (part ?? "<null>") +
                    " / restored=" + restored.ToString(CultureInfo.InvariantCulture));
            }
        }

        if (string.IsNullOrEmpty(part) || part == NONE)
            ClearHeldTargetGrabState();

        ApplyAutoGrabWidthFromTargetPerson();
        UpdateGrabHandUtilityButtons();

        if (!suppressApply && IsFollowTargetMode() && hasActiveGrab)
            ApplyGrab(false);
    }

    private void UpdateTargetPersonControllerChoices()
    {
        if (targetPersonPartChooser == null)
            return;

        string current = targetPersonPartChooser.val;
        List<string> choices = new List<string> { NONE };

        AddFixedTargetControllerChoice(choices, TC_HUG_BODY);
        AddFixedTargetControllerChoice(choices, TC_CHEST_HOLD);
        AddFixedTargetControllerChoice(choices, TC_HIP_HOLD);
        AddFixedTargetControllerChoice(choices, TC_HAND);
        AddFixedTargetControllerChoice(choices, TC_FOOT);
        AddFixedTargetControllerChoice(choices, TC_KNEE);
        AddFixedTargetControllerChoice(choices, TC_GEN);
        AddFixedTargetControllerChoice(choices, TC_PENI_BASE);
        AddFixedTargetControllerChoice(choices, TC_PENI_MID);
        AddFixedTargetControllerChoice(choices, TC_PENI_TIP);
        AddFixedTargetControllerChoice(choices, TC_ANUS);
        AddFixedTargetControllerChoice(choices, TC_GROIN);
        AddFixedTargetControllerChoice(choices, TC_HEAD);
        AddFixedTargetControllerChoice(choices, TC_HEAD_TOP);
        AddFixedTargetControllerChoice(choices, TC_MOUTH);
        AddFixedTargetControllerChoice(choices, TC_NECK);
        AddFixedTargetControllerChoice(choices, TC_ABDOMEN);
        AddFixedTargetControllerChoice(choices, TC_HIP);
        AddFixedTargetControllerChoice(choices, TC_L_NIPPLE);
        AddFixedTargetControllerChoice(choices, TC_R_NIPPLE);
        AddFixedTargetControllerChoice(choices, TC_L_HAND);
        AddFixedTargetControllerChoice(choices, TC_R_HAND);
        AddFixedTargetControllerChoice(choices, TC_L_FOOT);
        AddFixedTargetControllerChoice(choices, TC_R_FOOT);
        AddFixedTargetControllerChoice(choices, TC_L_KNEE);
        AddFixedTargetControllerChoice(choices, TC_R_KNEE);

        if (selectedTargetPerson != null && HasTargetControllerFilter())
        {
            choices.AddRange(
                selectedTargetPerson.GetComponentsInChildren<FreeControllerV3>(true)
                    .Where(fc => fc != null && IsAllowedTargetPersonControllerName(fc.name))
                    .Where(fc => MatchesTargetControllerFilter(fc.name))
                    .Select(fc => CUSTOM_TARGET_PREFIX + fc.name)
                    .Distinct()
                    .OrderBy(x => x)
            );
        }

        targetPersonPartChooser.choices = choices;

        if (current == TC_CROTCH)
            current = TC_GROIN;

        string next = choices.Contains(current) && current != NONE
            ? current
            : FirstExistingChoice(choices, DEFAULT_TARGET_CONTROLLER, TC_CHEST_HOLD, TC_HIP_HOLD, TC_HAND, TC_FOOT, TC_KNEE, TC_GEN, TC_PENI_BASE, TC_PENI_MID, TC_PENI_TIP, TC_ANUS, TC_GROIN, TC_HEAD, TC_HEAD_TOP, TC_MOUTH, TC_NECK, TC_ABDOMEN, TC_HIP, TC_L_NIPPLE, TC_R_NIPPLE) ?? NONE;

        targetPersonPartChooser.val = next;
        UpdateGrabHandUtilityButtons();
    }

    private void AddFixedTargetControllerChoice(List<string> choices, string label)
    {
        if (choices == null || string.IsNullOrEmpty(label))
            return;

        if (!choices.Contains(label))
            choices.Add(label);
    }

    private void SetTargetControllerShortcut(string choice)
    {
        if (targetPersonPartChooser == null || string.IsNullOrEmpty(choice))
            return;

        UpdateTargetPersonControllerChoices();

        if (targetPersonPartChooser.choices == null || !targetPersonPartChooser.choices.Contains(choice))
        {
            SetStatus("Target shortcut unavailable: " + choice);
            return;
        }

        targetPersonPartChooser.val = choice;
        OnTargetPersonPartChanged(choice);
        SetStatus("Target: " + choice);
    }

    private bool IsAllowedTargetPersonControllerName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;

        string n = name.ToLowerInvariant();
        bool explicitFilter = HasTargetControllerFilter();

        // lrnipple は通常掴みControlではなく、左右乳首ペア用の特殊キーとして通す。
        if (IsNipplePairControlName(name))
            return true;

        // 掴み先として使いにくい補助Controlは除外。
        // eyeTargetControl 等は target で落ちる。
        // ただしFilter指定時は、用意したIK/Target系Controlを明示選択できるよう通す。
        if (!explicitFilter && n.Contains("target"))
            return false;

        if (!explicitFilter && n.Contains("tool"))
            return false;

        return true;
    }

    private bool HasTargetControllerFilter()
    {
        return targetControllerFilterJSON != null && !string.IsNullOrEmpty(targetControllerFilterJSON.val);
    }

    private bool MatchesTargetControllerFilter(string name)
    {
        if (!HasTargetControllerFilter())
            return true;

        if (string.IsNullOrEmpty(name))
            return false;

        string filter = targetControllerFilterJSON.val.ToLowerInvariant();
        string lowerName = name.ToLowerInvariant();
        char[] separators = new char[] { ' ', ',', ';', '/', '|' };
        string[] tokens = filter.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        if (tokens == null || tokens.Length == 0)
            return true;

        foreach (string token in tokens)
        {
            if (lowerName.Contains(token))
                return true;
        }

        return false;
    }

    private string FirstExistingChoice(List<string> choices, params string[] candidates)
    {
        if (choices == null || candidates == null)
            return null;

        foreach (string c in candidates)
        {
            if (choices.Contains(c))
                return c;
        }

        return choices.FirstOrDefault(x => x != NONE);
    }

    private string GetTargetControllerActualName(string choice)
    {
        if (string.IsNullOrEmpty(choice) || choice == NONE)
            return choice;

        if (choice.StartsWith(CUSTOM_TARGET_PREFIX))
            return choice.Substring(CUSTOM_TARGET_PREFIX.Length);

        if (choice == TC_CHEST_HOLD)
            return "control";
        if (choice == TC_HIP_HOLD)
            return "hipHold";
        if (choice == TC_HUG_BODY)
            return "chestControl";
        if (choice == TC_GEN)
            return "LabiaTrigger";
        if (choice == TC_PENI_BASE)
            return FirstExistingTargetControlName("penisBaseControl", "penisBase", "penis base") ?? "penisBaseControl";
        if (choice == TC_PENI_MID)
            return FirstExistingTargetControlName("penisMidControl", "penisMid", "penis mid") ?? "penisMidControl";
        if (choice == TC_PENI_TIP)
            return FirstExistingTargetControlName("penisTipControl", "penisTip", "penis tip") ?? "penisTipControl";
        if (choice == TC_ANUS)
            return "_JointAl/Debug";
        if (choice == TC_CROTCH || choice == TC_GROIN)
            return "crotchTarget";
        if (choice == TC_HEAD)
            return "headControl";
        if (choice == TC_HEAD_TOP)
            return "headTopTarget";
        if (choice == TC_NECK)
            return FirstExistingTargetControlName("neckControl", "neck", "neckTarget") ?? "neckControl";
        if (choice == TC_MOUTH)
            return FirstExistingTargetControlName("mouthControl", "mouth", "mouthTarget") ?? "mouthControl";
        if (choice == TC_ABDOMEN)
            return "abdomenControl";
        if (choice == TC_HIP)
            return "hipControl";
        if (choice == TC_HAND)
            return "handPair";
        if (choice == TC_FOOT)
            return "footPair";
        if (choice == TC_KNEE)
            return "kneePair";
        if (choice == TC_L_NIPPLE)
            return FirstExistingTargetControlName("lNipple", "lnipple", "lNippleControl", "leftNipple") ?? "lNipple";
        if (choice == TC_R_NIPPLE)
            return FirstExistingTargetControlName("rNipple", "rnipple", "rNippleControl", "rightNipple") ?? "rNipple";
        if (choice == TC_L_HAND)
            return "lHandControl";
        if (choice == TC_R_HAND)
            return "rHandControl";
        if (choice == TC_L_FOOT)
            return "lFootControl";
        if (choice == TC_R_FOOT)
            return "rFootControl";
        if (choice == TC_L_KNEE)
            return "lKneeControl";
        if (choice == TC_R_KNEE)
            return "rKneeControl";

        return choice;
    }

    private string FirstExistingTargetControlName(params string[] names)
    {
        if (selectedTargetPerson == null || names == null)
            return null;

        foreach (string name in names)
        {
            if (string.IsNullOrEmpty(name))
                continue;

            if (GetControlFromAtom(selectedTargetPerson, name) != null)
                return name;
        }

        return null;
    }

    private void ApplyAutoGrabWidthFromTargetAtom()
    {
        if (suppressApply)
            return;

        if (autoGrabWidthJSON == null || !autoGrabWidthJSON.val)
            return;

        if (IsTargetPersonMode())
            return;

        if (selectedTargetAtom == null)
            return;

        float horizontalMin;
        float horizontalMax;
        if (!TryEstimateAtomHorizontalWidths(selectedTargetAtom, out horizontalMin, out horizontalMax))
        {
            DebugLog("[AUTO WIDTH] no bounds / atom=" + selectedTargetAtom.uid);
            return;
        }

        // v2.0p:
        // Grab Width は「対象物をまたぐための開き量」なので X/Z の大きい方 * 2 を使う。
        // Final Grab Width は「最後にどこまで閉じるか」なので X/Z の小さい方を使う。
        // UI値は常に正の値で扱う。左右配置の向きは GetTargetSideAxis 側で確定済み。
        bool specialLongObject = IsSpecialLongObjectAtom(selectedTargetAtom);

        float safeMin = Mathf.Max(0.0f, Mathf.Abs(horizontalMin));
        float safeMax = Mathf.Max(0.0f, Mathf.Abs(horizontalMax));

        float startWidth = Mathf.Clamp(Mathf.Max(0.50f, safeMax * 2.0f), 0.02f, 2.00f);
        float finalWidth = Mathf.Clamp(safeMin, 0.01f, 2.00f);
        float autoZ = 0.0f;

        if (specialLongObject)
        {
            // dildo/vibe系は細い棒として扱う。
            // 掴みに行く時は長さ側をまたぐため Grab Width は通常計算のまま。
            // 最終幅だけ細く閉じ、Z位置を少し前へ寄せる。
            autoZ = 0.14f;
            finalWidth = 0.01f;
        }

        suppressApply = true;
        try
        {
            if (autoZOffsetJSON != null)
                autoZOffsetJSON.val = autoZ;

            if (finalGrabWidthJSON != null)
                finalGrabWidthJSON.val = finalWidth;

            if (grabWidthJSON != null)
                grabWidthJSON.val = startWidth;
        }
        finally
        {
            suppressApply = false;
        }

        DebugLog("[AUTO WIDTH] atom=" + selectedTargetAtom.uid +
            " special=" + (specialLongObject ? "1" : "0") +
            " min=" + safeMin.ToString("F3", CultureInfo.InvariantCulture) +
            " max=" + safeMax.ToString("F3", CultureInfo.InvariantCulture) +
            " final=" + finalWidth.ToString("F3", CultureInfo.InvariantCulture) +
            " grab=" + startWidth.ToString("F3", CultureInfo.InvariantCulture) +
            " autoZ=" + autoZ.ToString("F3", CultureInfo.InvariantCulture));
    }

    private void ApplyAutoGrabWidthFromTargetPerson()
    {
        if (suppressApply)
            return;

        if (autoGrabWidthJSON == null || !autoGrabWidthJSON.val)
            return;

        if (!IsTargetPersonMode())
            return;

        string choice = targetPersonPartChooser != null
            ? targetPersonPartChooser.val
            : NONE;
        string controller = GetTargetControllerActualName(choice);

        if (string.IsNullOrEmpty(controller) || controller == NONE)
            return;

        string c = controller.ToLowerInvariant();
        bool hipHold = IsHipHoldMode();
        bool genMode = IsGenMode();
        bool peniMode = IsPeniMode();
        bool anusMode = IsAnusMode();
        bool groinMode = IsGroinMode();
        bool crotchLikeMode = genMode || anusMode || groinMode;
        float grabWidth = 0.40f;
        if (hipHold)
            grabWidth = HIP_HOLD_GRAB_WIDTH;
        else if (peniMode)
            grabWidth = PENI_GRAB_WIDTH;
        else if (crotchLikeMode)
            grabWidth = CROTCH_GRAB_WIDTH;
        else if (choice == TC_NECK)
            grabWidth = NECK_GRAB_WIDTH;
        else if (IsWidePersonController(c))
            grabWidth = WIDE_PERSON_GRAB_WIDTH;

        suppressApply = true;
        try
        {
            if (grabWidthJSON != null)
                grabWidthJSON.val = grabWidth;

            float finalWidth = MIN_FINAL_GRAB_WIDTH;
            if (hipHold)
                finalWidth = HIP_HOLD_FINAL_GRAB_WIDTH;
            else if (targetPersonPartChooser != null && targetPersonPartChooser.val == TC_HEAD)
                finalWidth = HEAD_FINAL_GRAB_WIDTH;
            else if (peniMode)
                finalWidth = PENI_FINAL_GRAB_WIDTH;
            else if (crotchLikeMode)
                finalWidth = CROTCH_FINAL_GRAB_WIDTH;
            else if (IsNipplePairMode())
                finalWidth = 0.10f;
            if (finalGrabWidthJSON != null)
                finalGrabWidthJSON.val = finalWidth;

            if (autoZOffsetJSON != null)
                autoZOffsetJSON.val = peniMode ? PENI_AUTO_Z_OFFSET : 0.00f;
        }
        finally
        {
            suppressApply = false;
        }

        DebugLog("[AUTO WIDTH PERSON] controller=" + choice +
            " actual=" + controller +
            " grab=" + grabWidth.ToString("F2", CultureInfo.InvariantCulture) +
            " final=" + (finalGrabWidthJSON != null ? finalGrabWidthJSON.val.ToString("F2", CultureInfo.InvariantCulture) : "0.00") +
            " autoZ=0.00");
    }

    private bool IsNipplePairMode()
    {
        if (!IsTargetPersonMode())
            return false;

        string choice = targetPersonPartChooser != null ? targetPersonPartChooser.val : NONE;
        string controlName = GetTargetControllerActualName(choice);
        return choice == TC_CHEST_HOLD || IsNipplePairControlName(controlName);
    }

    private bool IsHipHoldMode()
    {
        return IsTargetPersonMode() &&
               targetPersonPartChooser != null &&
               targetPersonPartChooser.val == TC_HIP_HOLD;
    }

    private bool IsGenMode()
    {
        return IsTargetPersonMode() &&
               targetPersonPartChooser != null &&
               targetPersonPartChooser.val == TC_GEN;
    }

    private bool IsPeniMode()
    {
        if (!IsTargetPersonMode() || targetPersonPartChooser == null)
            return false;

        string choice = targetPersonPartChooser.val;
        return choice == TC_PENI_BASE || choice == TC_PENI_MID || choice == TC_PENI_TIP;
    }

    private bool IsGenTarget()
    {
        return IsGenMode();
    }

    private bool IsAnusMode()
    {
        return IsTargetPersonMode() &&
               targetPersonPartChooser != null &&
               targetPersonPartChooser.val == TC_ANUS;
    }

    private bool IsGroinMode()
    {
        if (!IsTargetPersonMode() || targetPersonPartChooser == null)
            return false;

        string choice = targetPersonPartChooser.val;
        return choice == TC_GROIN || choice == TC_CROTCH;
    }

    private bool IsTargetPairMode()
    {
        if (!IsTargetPersonMode() || targetPersonPartChooser == null)
            return false;

        return targetPersonPartChooser.val == TC_HAND ||
               targetPersonPartChooser.val == TC_FOOT ||
               targetPersonPartChooser.val == TC_KNEE;
    }

    private bool IsNipplePairControlName(string name)
    {
        if (string.IsNullOrEmpty(name) || name == NONE)
            return false;

        string key = NormalizeControllerKey(name);

        // v3.0f:
        // IF仕様: Target Controller が通常名 "control" の場合は、
        // そのControl自体を掴まず、Nipple Pair特殊処理へ逃がす。
        // lrnipple系の名前も互換で特殊扱いにする。
        return key == "control" ||
               key.Contains(LRNIPPLE) ||
               key.Contains("nipplepair") ||
               key.Contains("pairnipple") ||
               key == "nipple";
    }

    private string NormalizeControllerKey(string name)
    {
        if (string.IsNullOrEmpty(name))
            return "";

        string n = name.ToLowerInvariant();
        string key = "";

        for (int i = 0; i < n.Length; i++)
        {
            char ch = n[i];
            if ((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9'))
                key += ch;
        }

        return key;
    }

    private string GetTargetControllerNameForDebug()
    {
        string choice = targetPersonPartChooser != null ? targetPersonPartChooser.val : NONE;
        string actual = GetTargetControllerActualName(choice);
        return choice + "=>" + actual;
    }

    private bool IsWidePersonController(string lowerControllerName)
    {
        if (string.IsNullOrEmpty(lowerControllerName))
            return false;

        return lowerControllerName == "chestcontrol" ||
               lowerControllerName == "abdomencontrol" ||
               lowerControllerName == "hipcontrol" ||
               lowerControllerName == "hiphold" ||
               lowerControllerName == "headcontrol";
    }

    private bool IsSpecialLongObjectAtom(Atom atom)
    {
        if (atom == null || string.IsNullOrEmpty(atom.uid))
            return false;

        string name = atom.uid.ToLowerInvariant();

        return name.Contains("dildo") ||
               name.Contains("vibe") ||
               name.Contains("vibrator");
    }

    private bool TryEstimateAtomHorizontalWidths(Atom atom, out float minWidth, out float maxWidth)
    {
        minWidth = 0.0f;
        maxWidth = 0.0f;

        if (atom == null)
            return false;

        Bounds bounds;
        bool hasBounds = TryGetAtomBounds(atom, out bounds);
        if (!hasBounds)
            return false;

        GetHorizontalMinMaxBoundsSize(bounds, out minWidth, out maxWidth);

        return maxWidth > 0.0001f;
    }

    private void GetHorizontalMinMaxBoundsSize(Bounds bounds, out float minWidth, out float maxWidth)
    {
        float x = Mathf.Abs(bounds.size.x);
        float z = Mathf.Abs(bounds.size.z);

        minWidth = Mathf.Max(0.0f, Mathf.Min(x, z));
        maxWidth = Mathf.Max(0.0f, Mathf.Max(x, z));
    }

    private bool TryGetAtomBounds(Atom atom, out Bounds bounds)
    {
        bounds = new Bounds();
        bool hasBounds = false;

        if (atom == null)
            return false;

        Renderer[] renderers = atom.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renderers)
        {
            if (r == null)
                continue;

            if (!hasBounds)
            {
                bounds = r.bounds;
                hasBounds = true;
            }
            else
            {
                bounds.Encapsulate(r.bounds);
            }
        }

        Collider[] colliders = atom.GetComponentsInChildren<Collider>(true);
        foreach (Collider c in colliders)
        {
            if (c == null)
                continue;

            if (!hasBounds)
            {
                bounds = c.bounds;
                hasBounds = true;
            }
            else
            {
                bounds.Encapsulate(c.bounds);
            }
        }

        return hasBounds;
    }

    private Vector3 GetAtomSideAxis(Atom atom)
    {
        if (atom != null && atom.mainController != null && atom.mainController.control != null)
        {
            Vector3 axis = atom.mainController.control.right;
            if (axis.sqrMagnitude > 0.0001f)
                return axis.normalized;
        }

        if (atom != null)
        {
            Vector3 axis = atom.transform.right;
            if (axis.sqrMagnitude > 0.0001f)
                return axis.normalized;
        }

        return Vector3.right;
    }

    private float GetBoundsSizeAlongAxis(Bounds bounds, Vector3 axis)
    {
        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.right;

        axis.Normalize();

        Vector3 c = bounds.center;
        Vector3 e = bounds.extents;

        float min = float.MaxValue;
        float max = float.MinValue;

        for (int xi = -1; xi <= 1; xi += 2)
        {
            for (int yi = -1; yi <= 1; yi += 2)
            {
                for (int zi = -1; zi <= 1; zi += 2)
                {
                    Vector3 p = c + new Vector3(e.x * xi, e.y * yi, e.z * zi);
                    float d = Vector3.Dot(p, axis);
                    if (d < min) min = d;
                    if (d > max) max = d;
                }
            }
        }

        return Mathf.Max(0.0f, max - min);
    }

    private void ApplyDefaultSettings()
    {
        suppressApply = true;

        try
        {
            if (grabWidthJSON != null) grabWidthJSON.val = 1.60f;
            if (grabCloseSpeedJSON != null) grabCloseSpeedJSON.val = 5.0f;
            if (finalGrabWidthJSON != null) finalGrabWidthJSON.val = 0.10f;
            if (targetZOffsetJSON != null) targetZOffsetJSON.val = 0.00f;
            if (autoZOffsetJSON != null) autoZOffsetJSON.val = 0.00f;
            if (maxHandReachJSON != null) maxHandReachJSON.val = 0.70f;
            if (maxFootReachJSON != null) maxFootReachJSON.val = 0.80f;
            if (maxHeadReachJSON != null) maxHeadReachJSON.val = 0.45f;
            if (headTargetDistanceJSON != null) headTargetDistanceJSON.val = 0.12f;
            if (kissFaceAlignJSON != null) kissFaceAlignJSON.val = true;
            if (kissFaceStrengthJSON != null) kissFaceStrengthJSON.val = 0.70f;
            if (handPalmOffsetJSON != null) handPalmOffsetJSON.val = 0.00f;
            if (handCenterOffsetJSON != null) handCenterOffsetJSON.val = 0.00f;
            if (footSoleOffsetJSON != null) footSoleOffsetJSON.val = 0.08f;
            if (footArcWidthJSON != null) footArcWidthJSON.val = 0.30f;
            if (footArcDropJSON != null) footArcDropJSON.val = 0.10f;
            if (kneeWidthMultiplierJSON != null) kneeWidthMultiplierJSON.val = 1.50f;
            if (moveTimeJSON != null) moveTimeJSON.val = 0.50f;
            if (hugModeJSON != null) hugModeJSON.val = false;
            if (hugDepthJSON != null) hugDepthJSON.val = 0.30f;
            if (autoGrabWidthJSON != null) autoGrabWidthJSON.val = true;
            if (followModeChooser != null) followModeChooser.val = FOLLOW_OFF;
            if (followTargetJSON != null) followTargetJSON.val = false;
            if (targetPersonPartChooser != null)
            {
                UpdateTargetPersonControllerChoices();
                if (targetPersonPartChooser.choices != null && targetPersonPartChooser.choices.Contains(DEFAULT_TARGET_CONTROLLER))
                    targetPersonPartChooser.val = DEFAULT_TARGET_CONTROLLER;
            }
            if (autoSnapPullOpenIKJSON != null) autoSnapPullOpenIKJSON.val = true;
            if (handWristAngleJSON != null) handWristAngleJSON.val = true;
            if (alignFootSoleJSON != null) alignFootSoleJSON.val = false;
            if (leftFootJSON != null) leftFootJSON.val = true;
            if (rightFootJSON != null) rightFootJSON.val = true;
        }
        finally
        {
            suppressApply = false;
        }

        if (targetPersonPartChooser != null && targetPersonPartChooser.val == DEFAULT_TARGET_CONTROLLER)
            OnTargetPersonPartChanged(DEFAULT_TARGET_CONTROLLER);

        if (IsFollowTargetMode() && hasActiveGrab)
            ApplyGrab(false);

        SetStatus("Default applied");
    }

    private void LoadUserDefaults()
    {
        RestoreHeldTargetHandFollowLocks("self-load-user-defaults");
        ClearPendingWristHandLocks();
        ResetChestHoldFrontLeftGraspBoostState("self-load-user-defaults");
        ResetChestHoldFrontRightGraspBoostState("self-load-user-defaults");
        ReleaseSelectedTargetNippleIK("self-load-user-defaults");

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
            QueuePostPoseLoadRuntimeResync(true, false, "self-load-user-defaults");
            SetStatus("Load User Defaults applied / snap resync");
            return;
        }

        SetStatus("Load User Defaults action not found");
        DebugLog("LOAD USER DEFAULTS: PosePresets action not found.");
    }

    private void SelfIKDefault()
    {
        ResolveControls();

        if (selectedPerson == null)
        {
            SetStatus("Self IK Default / no Person");
            return;
        }

        hasActiveGrab = false;
        RestoreHeldTargetHandFollowLocks("self-ik-default");
        ClearHeldTargetGrabState();
        ClearPendingWristHandLocks();
        ResetChestHoldFrontLeftGraspBoostState("self-ik-default");
        ResetChestHoldFrontRightGraspBoostState("self-ik-default");
        grabElapsed = 0.0f;
        activeMoveTimeMultiplier = 1.0f;
        activeIncludeHead = false;
        pufupufuActive = false;
        if (jobActive)
            RestoreJobHandPositions();
        jobActive = false;
        RestoreSelfFollowParentLinks();
        RestoreTemporaryRelaxLinkedIK();
        RestoreTargetNoneBodyRelaxIK();
        RestoreTemporaryHandRotationOffStates();
        RestoreChestHoldNippleIKStabilize("self-ik-default-nipple-stabilize");
        ReleaseSelectedTargetNippleIK("self-ik-default");
        StopSwoonDrop(true, "self-ik-default");
        pendingAutoSnapIKControls.Clear();

        int changed = 0;

        changed += SetIKState(hipControl, true, true);
        changed += SetIKState(chestControl, true, true);
        changed += SetIKState(headControl, true, true);
        changed += SetIKState(lFootControl, true, true);
        changed += SetIKState(rFootControl, true, true);

        changed += SetIKState(lHandControl, false, false);
        changed += SetIKState(rHandControl, false, false);
        changed += SetIKState(lKneeControl, false, false);
        changed += SetIKState(rKneeControl, false, false);

        changed += SetIKState(GetSelfControlByAliases("penisBaseControl", "penisBase", "penis base"), false, false);
        changed += SetIKState(GetSelfControlByAliases("penisMidControl", "penisMid", "penis mid"), false, false);
        changed += SetIKState(GetSelfControlByAliases("penisTipControl", "penisTip", "penis tip"), false, false);

        positionStateOnControls.Clear();
        rotationStateOnControls.Clear();
        releaseRestorePositionControls.Clear();
        releaseRestoreRotationControls.Clear();
        releaseRestoreIKPending = false;

        SetStatus("Self IK Default / controls=" + changed.ToString(CultureInfo.InvariantCulture));
    }

    private void TargetIKDefault()
    {
        string resetSummary = ResetTargetGrabberRuntimeState("target-ik-default", true);

        if (selectedTargetPerson == null)
        {
            SetStatus("Target IK Default / no Target Person / reset " + resetSummary);
            return;
        }

        InvalidateTargetPersonControlCache();
        int changed = ApplyPersonIKDefault(selectedTargetPerson, "TARGET IK DEFAULT");

        // v5be: Target IK Default is an explicit new baseline.
        // Do not move IK Select to <none> here. HDU_Commander direct routes select the same IK Select
        // immediately before running actions, so clearing the chooser here can race/conflict with HDU.
        QueueTargetIKDefaultRuntimeSnapResync("target-ik-default");

        SetStatus("Target IK Default / controls=" + changed.ToString(CultureInfo.InvariantCulture) +
            " / snap=current / reset " + resetSummary);
    }

    private void TargetLoadUserDefaults()
    {
        string resetSummary = ResetTargetGrabberRuntimeState("target-load-user-defaults", true);

        if (selectedTargetPerson == null)
        {
            SetStatus("Target Load User Defaults / no Target Person / reset " + resetSummary);
            return;
        }

        string[] actionNames =
        {
            "Load User Defaults",
            "LoadUserDefaults",
            "Load User Default",
            "LoadUserDefault",
            "Load Defaults",
            "LoadDefaults"
        };

        if (TryExecutePosePresetActionOnAtom(selectedTargetPerson, actionNames, "TARGET LOAD DEFAULTS"))
        {
            QueuePostPoseLoadRuntimeResync(false, true, "target-load-user-defaults");
            SetStatus("Target Load User Defaults applied / snap resync");
            return;
        }

        SetStatus("Target Load User Defaults action not found");
        DebugLog("TARGET LOAD DEFAULTS: PosePresets action not found on target person.");
    }

    private string ResetTargetGrabberRuntimeState(string reason, bool clearTargetSnapshots)
    {
        // Target Release / Target IK Default / Target Load User Defaults are explicit target-side reset points.
        // Clear every TargetGrabber runtime marker that can make a later route believe target hands/body are still held,
        // locked, pending snap, or waiting for delayed restore.  Do not change IK Select here; HDU direct routes own it.
        ResetHugBodyHdcHipUpperState(reason);
        hasActiveGrab = false;
        // v5bj: release target-side held-hand IK/state before clearing the flags/refs.
        // This avoids a state where HBA/HLA still treat the target hand as held or the target hand stays locked after Target Release.
        int restoredHeldHandLocks = RestoreHeldTargetHandFollowLocks(reason + "-held-target-hand");
        ClearHeldTargetGrabState();
        ClearPendingWristHandLocks();
        grabElapsed = 0.0f;
        activeMoveTimeMultiplier = 1.0f;
        activeIncludeHead = false;
        pufupufuActive = false;

        if (jobActive)
            RestoreJobHandPositions();
        jobActive = false;

        RestoreSelfFollowParentLinks();
        RestoreTemporaryRelaxLinkedIK();
        RestoreTemporaryHandRotationOffStates();
        int restoredNippleStabilize = RestoreChestHoldNippleIKStabilize(reason + "-nipple-stabilize");
        int releasedTargetNippleIK = ReleaseSelectedTargetNippleIK(reason + "-target-nipple-ik");
        StopSwoonDrop(true, reason);

        int restoredLocks = RestoreTargetLocks();
        int restoredComply = RestoreTargetNoneBodyRelaxIK();

        pendingAutoSnapIKControls.Clear();
        pendingSelfFollowTargets.Clear();
        hugBodyWristReferencePositions.Clear();
        hugBodyHandSnapAnchorPositions.Clear();

        releaseRestorePositionControls.Clear();
        releaseRestoreRotationControls.Clear();
        releaseRestoreIKPending = false;

        if (clearTargetSnapshots)
        {
            targetOriginalPositions.Clear();
            targetOriginalRotations.Clear();
            chestHoldNoIkNippleMoveControls.Clear();
            chestHoldNippleStabilizeControls.Clear();
            chestHoldNippleStabilizePositionStates.Clear();
            chestHoldNippleStabilizeRotationStates.Clear();
            targetLockPositionStates.Clear();
            targetLockRotationStates.Clear();
            targetLockControls.Clear();
        }

        DebugLog("[TARGET RUNTIME RESET] reason=" + reason +
            " / heldHandLocks=" + restoredHeldHandLocks.ToString(CultureInfo.InvariantCulture) +
            " / nippleStabilize=" + restoredNippleStabilize.ToString(CultureInfo.InvariantCulture) +
            " / nippleIK=" + releasedTargetNippleIK.ToString(CultureInfo.InvariantCulture) +
            " / locks=" + restoredLocks.ToString(CultureInfo.InvariantCulture) +
            " / comply=" + restoredComply.ToString(CultureInfo.InvariantCulture) +
            " / clearSnapshots=" + Bool01(clearTargetSnapshots));

        return "heldHandLocks=" + restoredHeldHandLocks.ToString(CultureInfo.InvariantCulture) +
            " / nippleStabilize=" + restoredNippleStabilize.ToString(CultureInfo.InvariantCulture) +
            " / nippleIK=" + releasedTargetNippleIK.ToString(CultureInfo.InvariantCulture) +
            " / locks=" + restoredLocks.ToString(CultureInfo.InvariantCulture) +
            " / comply=" + restoredComply.ToString(CultureInfo.InvariantCulture);
    }

    private int ApplyPersonIKDefault(Atom person, string logPrefix)
    {
        if (person == null)
            return 0;

        int changed = 0;

        changed += SetIKStateForAtom(person, "hipControl", true, true, logPrefix);
        changed += SetIKStateForAtom(person, "chestControl", true, true, logPrefix);
        changed += SetIKStateForAtom(person, "headControl", true, true, logPrefix);
        changed += SetIKStateForAtom(person, "lFootControl", true, true, logPrefix);
        changed += SetIKStateForAtom(person, "rFootControl", true, true, logPrefix);

        changed += SetIKStateForAtom(person, "lHandControl", false, false, logPrefix);
        changed += SetIKStateForAtom(person, "rHandControl", false, false, logPrefix);
        changed += SetIKStateForAtom(person, "lKneeControl", false, false, logPrefix);
        changed += SetIKStateForAtom(person, "rKneeControl", false, false, logPrefix);

        changed += SetIKStateForAtomByAliases(person, false, false, logPrefix, "penisBaseControl", "penisBase", "penis base");
        changed += SetIKStateForAtomByAliases(person, false, false, logPrefix, "penisMidControl", "penisMid", "penis mid");
        changed += SetIKStateForAtomByAliases(person, false, false, logPrefix, "penisTipControl", "penisTip", "penis tip");

        return changed;
    }

    private int SetIKStateForAtom(Atom atom, string controlName, bool positionOn, bool rotationOn, string logPrefix)
    {
        return SetIKStateForAtomByAliases(atom, positionOn, rotationOn, logPrefix, controlName);
    }

    private int SetIKStateForAtomByAliases(Atom atom, bool positionOn, bool rotationOn, string logPrefix, params string[] names)
    {
        if (atom == null || names == null)
            return 0;

        for (int i = 0; i < names.Length; i++)
        {
            FreeControllerV3 fc = GetControlFromAtom(atom, names[i]);
            if (fc != null)
                return SetIKStateWithPrefix(fc, positionOn, rotationOn, logPrefix);
        }

        return 0;
    }

    private int SetIKStateWithPrefix(FreeControllerV3 fc, bool positionOn, bool rotationOn, string logPrefix)
    {
        if (fc == null)
            return 0;

        try
        {
            fc.currentPositionState = positionOn ? FreeControllerV3.PositionState.On : FreeControllerV3.PositionState.Off;
        }
        catch { }

        try
        {
            fc.currentRotationState = rotationOn ? FreeControllerV3.RotationState.On : FreeControllerV3.RotationState.Off;
        }
        catch { }

        DebugLog("[" + logPrefix + "] " + fc.name +
            " pos=" + (positionOn ? "On" : "Off") +
            " rot=" + (rotationOn ? "On" : "Off"));
        return 1;
    }

    private int SetIKState(FreeControllerV3 fc, bool positionOn, bool rotationOn)
    {
        if (fc == null)
            return 0;

        try
        {
            fc.currentPositionState = positionOn ? FreeControllerV3.PositionState.On : FreeControllerV3.PositionState.Off;
        }
        catch { }

        try
        {
            fc.currentRotationState = rotationOn ? FreeControllerV3.RotationState.On : FreeControllerV3.RotationState.Off;
        }
        catch { }

        DebugLog("[SELF IK DEFAULT] " + fc.name +
            " pos=" + (positionOn ? "On" : "Off") +
            " rot=" + (rotationOn ? "On" : "Off"));
        return 1;
    }

    private FreeControllerV3 GetSelfControlByAliases(params string[] names)
    {
        if (selectedPerson == null || names == null)
            return null;

        for (int i = 0; i < names.Length; i++)
        {
            FreeControllerV3 fc = GetControl(names[i]);
            if (fc != null)
                return fc;
        }

        return null;
    }

    private bool TryExecutePosePresetAction(string[] actionNames)
    {
        return TryExecutePosePresetActionOnAtom(containingAtom, actionNames, "LOAD USER DEFAULTS");
    }

    private bool TryExecutePosePresetActionOnAtom(Atom atom, string[] actionNames, string logPrefix)
    {
        if (atom == null || actionNames == null)
            return false;

        foreach (string storableId in atom.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId))
                continue;

            if (storableId.IndexOf("PosePresets", StringComparison.OrdinalIgnoreCase) < 0)
                continue;

            JSONStorable storable = atom.GetStorableByID(storableId);
            if (storable == null)
                continue;

            for (int i = 0; i < actionNames.Length; i++)
            {
                JSONStorableAction action = storable.GetAction(actionNames[i]);
                if (action == null)
                    continue;

                action.actionCallback.Invoke();
                DebugLog(logPrefix + ": atom=" + atom.uid + " / pose action=" + storableId + " / " + actionNames[i]);
                return true;
            }
        }

        return false;
    }


    private void QueueTargetIKDefaultRuntimeSnapResync(string reason)
    {
        SnapTargetIKDefaultRuntimeStateToCurrent(reason + "/now");

        if (poseLoadRuntimeResyncRoutine != null)
        {
            try { StopCoroutine(poseLoadRuntimeResyncRoutine); }
            catch { }
            poseLoadRuntimeResyncRoutine = null;
        }

        poseLoadRuntimeResyncRoutine = StartCoroutine(TargetIKDefaultRuntimeSnapResyncRoutine(reason));
    }

    private System.Collections.IEnumerator TargetIKDefaultRuntimeSnapResyncRoutine(string reason)
    {
        // Target IK Default may be applied while VaM/control state settles for a frame.
        // Clear old TargetGrabber target-side snapshots again after a short delay so Release Target
        // and AutoSnap do not reuse pre-default positions.
        yield return null;
        yield return null;

        SnapTargetIKDefaultRuntimeStateToCurrent(reason + "/delayed");

        InvalidateTargetPersonControlCache();
        ResolveControls();
        UpdateGrabHandUtilityButtons();

        poseLoadRuntimeResyncRoutine = null;
        DebugLog("[TARGET IK DEFAULT SNAP CURRENT] done / reason=" + reason);
    }

    private void SnapTargetIKDefaultRuntimeStateToCurrent(string reason)
    {
        // Target IK Default means the currently visible target pose/IK state is now the baseline.
        // Clear old target-side restore/snap caches instead of restoring them.
        // This intentionally does not change targetPersonPartChooser / IK Select; HDU_Commander
        // direct route actions also use that chooser and should remain the owner of their selection.
        ClearHeldTargetGrabState();
        RestoreChestHoldNippleIKStabilize(reason + "-nipple-stabilize");
        ReleaseSelectedTargetNippleIK(reason + "-target-nipple-ik");
        pendingAutoSnapIKControls.Clear();
        hugBodyWristReferencePositions.Clear();
        hugBodyHandSnapAnchorPositions.Clear();

        targetOriginalPositions.Clear();
        targetOriginalRotations.Clear();
        chestHoldNoIkNippleMoveControls.Clear();
        chestHoldNippleStabilizeControls.Clear();
        chestHoldNippleStabilizePositionStates.Clear();
        chestHoldNippleStabilizeRotationStates.Clear();
        targetLockPositionStates.Clear();
        targetLockRotationStates.Clear();
        targetLockControls.Clear();

        DebugLog("[TARGET IK DEFAULT SNAP CURRENT] clear target snapshots / reason=" + reason +
            " / ikSelect=" + (targetPersonPartChooser != null ? targetPersonPartChooser.val : "<null>"));
    }

    private void QueuePostPoseLoadRuntimeResync(bool selfSide, bool targetSide, string reason)
    {
        ResetPoseDependentRuntimeStateAfterPoseLoad(selfSide, targetSide, reason + "/now");

        if (poseLoadRuntimeResyncRoutine != null)
        {
            try { StopCoroutine(poseLoadRuntimeResyncRoutine); }
            catch { }
            poseLoadRuntimeResyncRoutine = null;
        }

        poseLoadRuntimeResyncRoutine = StartCoroutine(PostPoseLoadRuntimeResyncRoutine(selfSide, targetSide, reason));
    }

    private System.Collections.IEnumerator PostPoseLoadRuntimeResyncRoutine(bool selfSide, bool targetSide, string reason)
    {
        // PosePresets can update controllers during/after the action callback.
        // Wait a couple of frames, then invalidate caches again so future Grab/Release uses the new pose as baseline.
        yield return null;
        yield return null;

        ResetPoseDependentRuntimeStateAfterPoseLoad(selfSide, targetSide, reason + "/delayed");

        if (selfSide)
            InvalidatePersonControlCache();
        if (targetSide)
            InvalidateTargetPersonControlCache();

        ResolveControls();
        UpdateGrabHandUtilityButtons();

        poseLoadRuntimeResyncRoutine = null;
        DebugLog("[POSE LOAD RESYNC] done / reason=" + reason +
            " / self=" + Bool01(selfSide) +
            " / target=" + Bool01(targetSide));
    }

    private void ResetPoseDependentRuntimeStateAfterPoseLoad(bool selfSide, bool targetSide, string reason)
    {
        // A pose load is an explicit new baseline.  Any old TargetGrabber move/release/snap snapshot
        // must not be reused, otherwise the next release/follow can pull controls back to the old pose.
        ClearPendingWristHandLocks();
        pendingAutoSnapIKControls.Clear();
        pendingSelfFollowTargets.Clear();
        hugBodyWristReferencePositions.Clear();
        hugBodyHandSnapAnchorPositions.Clear();

        if (selfSide)
        {
            ResetHugBodyHdcHipUpperState(reason);
            hasActiveGrab = false;
            ClearHeldTargetGrabState();
            grabElapsed = 0.0f;
            activeMoveTimeMultiplier = 1.0f;
            activeIncludeHead = false;
            pufupufuActive = false;

            // Do not restore job base positions here. A pose load is the new baseline,
            // and restoring the old job base can pull the freshly loaded pose backward.
            jobActive = false;
            RestoreSelfFollowParentLinks();
            RestoreTemporaryRelaxLinkedIK();
            RestoreTemporaryHandRotationOffStates();

            grabStartPositions.Clear();
            grabStartRotations.Clear();
            positionStateOnControls.Clear();
            rotationStateOnControls.Clear();
            releaseRestorePositionControls.Clear();
            releaseRestoreRotationControls.Clear();
            releaseRestoreIKPending = false;
        }

        if (targetSide)
        {
            ClearHeldTargetGrabState();
            StopSwoonDrop(true, reason);
            RestoreTargetNoneBodyRelaxIK();
            RestoreChestHoldNippleIKStabilize(reason + "-nipple-stabilize");
            RestoreTargetLocks();

            targetOriginalPositions.Clear();
            targetOriginalRotations.Clear();
            chestHoldNoIkNippleMoveControls.Clear();
            chestHoldNippleStabilizeControls.Clear();
            chestHoldNippleStabilizePositionStates.Clear();
            chestHoldNippleStabilizeRotationStates.Clear();
            targetLockPositionStates.Clear();
            targetLockRotationStates.Clear();
            targetLockControls.Clear();
        }

        if (selfSide || targetSide)
            ReleaseSelectedTargetNippleIK(reason + "-target-nipple-ik");

        DebugLog("[POSE LOAD RESYNC] clear snapshots / reason=" + reason +
            " / self=" + Bool01(selfSide) +
            " / target=" + Bool01(targetSide) +
            " / active=" + Bool01(hasActiveGrab));
    }

    private string TryReapplyTargetHumanPoseControllerAfterRelease()
    {
        // Safe optional bridge:
        // If the target Person has humanPoseControler/humanControler v038+,
        // reapply its current cycle/current pose after TargetGrabber restores target IK.
        // If the plugin/action is missing, do nothing. This keeps mutual/no-plugin setups safe.
        if (selectedTargetPerson == null)
        {
            DebugLog("[TARGET HC REAPPLY] skip / target person missing");
            return "none";
        }

        string actionName;
        string storableId;
        if (TryInvokeHumanPoseControllerActionOnAtom(
            selectedTargetPerson,
            new string[]
            {
                "HC Reapply Current Pose",
                "HC Reapply Current Cycle Pose"
            },
            out storableId,
            out actionName
        ))
        {
            DebugLog("[TARGET HC REAPPLY] ok / atom=" + selectedTargetPerson.uid +
                " / storable=" + storableId +
                " / action=" + actionName);
            return "reapply";
        }

        DebugLog("[TARGET HC REAPPLY] skip / action not found / atom=" + selectedTargetPerson.uid);
        return "none";
    }

    private bool TryInvokeHumanPoseControllerActionOnAtom(Atom atom, string[] actionNames, out string matchedStorableId, out string matchedActionName)
    {
        matchedStorableId = "";
        matchedActionName = "";

        if (atom == null || actionNames == null)
            return false;

        List<string> storableIds = atom.GetStorableIDs();
        if (storableIds == null)
            return false;

        for (int s = 0; s < storableIds.Count; s++)
        {
            string storableId = storableIds[s];
            if (string.IsNullOrEmpty(storableId))
                continue;

            if (!IsHumanPoseControllerStorableId(storableId))
                continue;

            JSONStorable storable = atom.GetStorableByID(storableId);
            if (storable == null)
                continue;

            for (int i = 0; i < actionNames.Length; i++)
            {
                string actionName = actionNames[i];
                if (string.IsNullOrEmpty(actionName))
                    continue;

                JSONStorableAction action = storable.GetAction(actionName);
                if (action == null || action.actionCallback == null)
                    continue;

                action.actionCallback.Invoke();
                matchedStorableId = storableId;
                matchedActionName = actionName;
                return true;
            }
        }

        return false;
    }

    private bool IsHumanPoseControllerStorableId(string storableId)
    {
        if (string.IsNullOrEmpty(storableId))
            return false;

        return storableId.IndexOf("humanPoseControler", StringComparison.OrdinalIgnoreCase) >= 0 ||
               storableId.IndexOf("humanControler", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private void InvalidatePersonControlCache()
    {
        personControlCache.Clear();
        personControlCacheAtom = null;
        personControlCacheValid = false;
    }

    private void InvalidateTargetPersonControlCache()
    {
        targetPersonControlCache.Clear();
        targetPersonControlCacheAtom = null;
        targetPersonControlCacheValid = false;
        InvalidateGenTargetCache();
        InvalidateAnusTargetCache();
    }

    private void InvalidateGenTargetCache()
    {
        genTargetCacheAtom = null;
        genTargetCacheTransform = null;
        genTargetCacheValid = false;
    }

    private void InvalidateAnusTargetCache()
    {
        anusTargetCacheAtom = null;
        anusTargetCacheTransform = null;
        anusTargetCacheValid = false;
    }

    private void RebuildControlCache(Atom atom, Dictionary<string, FreeControllerV3> cache)
    {
        cache.Clear();

        if (atom == null)
            return;

        foreach (FreeControllerV3 fc in atom.GetComponentsInChildren<FreeControllerV3>(true))
        {
            if (fc == null || string.IsNullOrEmpty(fc.name))
                continue;

            cache[fc.name] = fc;
        }
    }

    private void EnsurePersonControlCache()
    {
        if (personControlCacheValid && personControlCacheAtom == selectedPerson)
            return;

        RebuildControlCache(selectedPerson, personControlCache);
        personControlCacheAtom = selectedPerson;
        personControlCacheValid = true;
    }

    private void EnsureTargetPersonControlCache()
    {
        if (targetPersonControlCacheValid && targetPersonControlCacheAtom == selectedTargetPerson)
            return;

        RebuildControlCache(selectedTargetPerson, targetPersonControlCache);
        targetPersonControlCacheAtom = selectedTargetPerson;
        targetPersonControlCacheValid = true;
    }

    private void ResolveControls()
    {
        lHandControl = null;
        rHandControl = null;
        lElbowControl = null;
        rElbowControl = null;
        lFootControl = null;
        rFootControl = null;
        lKneeControl = null;
        rKneeControl = null;
        headControl = null;
        chestControl = null;
        hipControl = null;

        if (selectedPerson == null)
            return;

        lHandControl = GetControl("lHandControl");
        rHandControl = GetControl("rHandControl");
        lElbowControl = GetControl("lElbowControl");
        rElbowControl = GetControl("rElbowControl");
        lFootControl = GetControl("lFootControl");
        rFootControl = GetControl("rFootControl");
        lKneeControl = GetControl("lKneeControl");
        rKneeControl = GetControl("rKneeControl");
        headControl = GetControl("headControl");
        chestControl = GetControl("chestControl");
        hipControl = GetControl("hipControl");

        if (!hasActiveGrab)
        {
            DebugLog("[RESOLVE] lHand=" + Bool01(lHandControl != null) +
                " rHand=" + Bool01(rHandControl != null) +
                " lElbow=" + Bool01(lElbowControl != null) +
                " rElbow=" + Bool01(rElbowControl != null) +
                " lFoot=" + Bool01(lFootControl != null) +
                " rFoot=" + Bool01(rFootControl != null) +
                " lKnee=" + Bool01(lKneeControl != null) +
                " rKnee=" + Bool01(rKneeControl != null) +
                " head=" + Bool01(headControl != null) +
                " chest=" + Bool01(chestControl != null) +
                " hip=" + Bool01(hipControl != null));
        }
    }

    private FreeControllerV3 GetControl(string name)
    {
        if (selectedPerson == null || string.IsNullOrEmpty(name))
            return null;

        EnsurePersonControlCache();

        FreeControllerV3 fc;
        return personControlCache.TryGetValue(name, out fc) ? fc : null;
    }

    public void Update()
    {
        bool jobRunning = UpdateJobAnimation();
        UpdateReleaseRestoreIK();
        UpdateSwoonDrop();
        UpdateReleaseButtonColors();
        UpdateChestHoldNippleHandFollow();
        UpdateChestHoldFrontLeftGraspDelayedRestore();
        UpdateChestHoldFrontRightGraspDelayedRestore();
        UpdateChestHoldFrontLeftGraspWatch();
        UpdateChestHoldFrontRightGraspWatch();

        if (jobRunning)
        {
            UpdatePufupufuAnimation();
            return;
        }

        if (!hasActiveGrab)
        {
            UpdatePufupufuAnimation();
            return;
        }

        bool followTarget = IsFollowTargetMode();
        bool followSelf = IsFollowSelfMode();
        bool moving = GetMoveTLinear() < 1.0f;

        if (moving)
        {
            ApplyGrab(false, activeIncludeHands, activeIncludeFeet, activeIncludeHead);
            UpdatePufupufuAnimation();
            return;
        }

        ActivatePendingChestHoldNippleHandFollow();

        if (followTarget)
        {
            ApplyGrab(false, activeIncludeHands, activeIncludeFeet, activeIncludeHead);
            ExecutePendingAutoSnapPullOpenIK();
            RestoreTemporaryRelaxLinkedIK();
            RestoreSelfFollowParentLinks();
            UpdatePufupufuAnimation();
            return;
        }

        if (followSelf)
        {
            ExecutePendingAutoSnapPullOpenIK();
            RestoreTemporaryRelaxLinkedIK();
            ApplyPendingSelfFollowParentLinks();
            UpdateSelfFollowParentLinks();
            UpdatePufupufuAnimation();
            return;
        }

        if (UpdatePufupufuAnimation())
            return;

        ExecutePendingAutoSnapPullOpenIK();
        RestoreTemporaryRelaxLinkedIK();
        RestoreSelfFollowParentLinks();

        // Follow OFFなら到達後に更新だけ止める。ControlはONのまま保持。
        hasActiveGrab = false;
    }

    public void LateUpdate()
    {
        UpdatePendingWristHandLocks();
        // v5an: do not hold target pelvis here. TargetLinePerson may own persistent pelvis control.
    }

    private void ApplyTargetPelvisAutoOnGrab()
    {
        ApplyTargetPelvisAutoOnGrab("auto-grab", true, true);
    }

    private void ApplyTargetPelvisAutoOnGrab(string reason, bool respectToggle)
    {
        ApplyTargetPelvisAutoOnGrab(reason, respectToggle, false);
    }

    private void ApplyTargetPelvisAutoOnGrab(string reason, bool respectToggle, bool useGrabGuard)
    {
        if (useGrabGuard && targetPelvisAutoOnGrabAppliedThisGrab)
        {
            TargetPelvisLogAlways("[TARGET PELVIS AUTO GRAB] skipped=already-applied reason=" + reason);
            return;
        }

        if (respectToggle && targetPelvisAutoOnGrabJSON != null && !targetPelvisAutoOnGrabJSON.val)
        {
            TargetPelvisLogAlways("[TARGET PELVIS AUTO GRAB] skipped=toggle-off reason=" + reason);
            return;
        }

        if (!IsTargetPersonMode() || selectedTargetPerson == null)
        {
            TargetPelvisLogAlways("[TARGET PELVIS AUTO GRAB] skipped=no-target-person reason=" + reason +
                " targetMode=" + (IsTargetPersonMode() ? "Person" : "Other") +
                " target=" + (selectedTargetPerson != null ? selectedTargetPerson.uid : "<null>"));
            return;
        }

        ResolveControls();

        Vector3 center = GetTargetCenter();
        bool frontSide = IsGrabberInFrontOfTargetPerson(center);
        // v5ap: Pelvis Auto mapping was visually reversed in TargetGrabber.
        // Keep the existing front/back detector because it is used by hand routing; reverse only the pelvis 90/270 assignment.
        float x = frontSide ? 90.0f : 270.0f;

        suppressTargetPelvisRotXCallback = true;
        try
        {
            if (targetPelvisRotXJSON != null)
                targetPelvisRotXJSON.val = x;
        }
        finally
        {
            suppressTargetPelvisRotXCallback = false;
        }

        // v5ao: one-shot only. Never enable Target Pelvis Rot X ON and never hold in LateUpdate.
        bool okX = ApplyTargetPelvisRotX(x, reason + "-x-one-shot", false);

        // v5as: TargetGrabber's detector-side front/back is visually reversed for pelvis work.
        // The X mapping already uses detector frontSide as visual Back (X=90) and detector backSide as visual Near (X=270).
        // Therefore the yaw +180 correction must be applied on detector frontSide, not detector backSide.
        float yawOffset = frontSide ? 180.0f : 0.0f;
        bool okFace = ApplyTargetPelvisFaceSelfYaw(yawOffset, reason + "-face-self-one-shot", false);

        if (useGrabGuard)
            targetPelvisAutoOnGrabAppliedThisGrab = true;

        TargetPelvisLogAlways("[TARGET PELVIS AUTO GRAB] oneShot=1 reason=" + reason +
            " okX=" + Bool01(okX) +
            " okFace=" + Bool01(okFace) +
            " frontSide=" + Bool01(frontSide) +
            " backSide=" + Bool01(!frontSide) +
            " x=" + x.ToString("F1", CultureInfo.InvariantCulture) +
            " yawOffset=" + yawOffset.ToString("F1", CultureInfo.InvariantCulture) +
            " center=" + FormatVector3(center));
    }

    private void ApplyTargetPelvisRotXFromSlider(string reason)
    {
        if (targetPelvisRotXEnableJSON == null || !targetPelvisRotXEnableJSON.val)
            return;

        float value = targetPelvisRotXJSON != null ? targetPelvisRotXJSON.val : 0.0f;
        ApplyTargetPelvisRotX(value, reason, false);
    }

    private void ApplyTargetPelvisRotXHold()
    {
        if (targetPelvisRotXEnableJSON == null || !targetPelvisRotXEnableJSON.val || targetPelvisRotXJSON == null)
            return;

        ApplyTargetPelvisRotX(targetPelvisRotXJSON.val, "late", true);
    }

    private bool ApplyTargetPelvisRotX(float x, string reason, bool quiet)
    {
        if (!IsTargetPersonMode())
        {
            if (!quiet)
                SetStatus("Target Pelvis Rot X needs Target Type=Person");
            if (!quiet)
                TargetPelvisLogAlways("[TARGET PELVIS ROT X] skipped=target-type reason=" + reason);
            return false;
        }

        ResolveControls();

        FreeControllerV3 pelvis = GetTargetPersonControlByAliases("pelvisControl", "pelvis");
        if (pelvis == null || pelvis.control == null)
        {
            if (!quiet)
                SetStatus("Target Pelvis Rot X / pelvis missing");
            if (!quiet)
                TargetPelvisLogAlways("[TARGET PELVIS ROT X] skipped=pelvis-missing reason=" + reason);
            return false;
        }

        x = Mathf.Repeat(x, 360.0f);
        Vector3 before = pelvis.control.localRotation.eulerAngles;
        pelvis.control.localRotation = Quaternion.Euler(x, before.y, before.z);

        try
        {
            pelvis.currentRotationState = FreeControllerV3.RotationState.On;
        }
        catch { }

        Vector3 after = pelvis.control.localRotation.eulerAngles;

        if (!quiet)
            SetStatus("Target Pelvis Rot X / x=" + x.ToString("F1", CultureInfo.InvariantCulture));

        if (!quiet)
            TargetPelvisLogAlways("[TARGET PELVIS ROT X] reason=" + reason +
                " oneShot=1" +
                " x=" + x.ToString("F1", CultureInfo.InvariantCulture) +
                " rotState=" + pelvis.currentRotationState.ToString() +
                " before=" + FormatVector3(before) +
                " after=" + FormatVector3(after));

        return true;
    }

    private Vector3 GetTargetPelvisFaceSelfReferencePosition()
    {
        ResolveControls();

        if (chestControl != null && chestControl.control != null)
            return chestControl.control.position;

        if (hipControl != null && hipControl.control != null)
            return hipControl.control.position;

        if (selectedPerson != null && selectedPerson.transform != null)
            return selectedPerson.transform.position;

        if (containingAtom != null && containingAtom.transform != null)
            return containingAtom.transform.position;

        return Vector3.zero;
    }

    private bool ApplyTargetPelvisFaceSelfYaw(float yawOffsetDegrees, string reason, bool quiet)
    {
        if (!IsTargetPersonMode())
        {
            if (!quiet)
                SetStatus("Target Pelvis Face Self needs Target Type=Person");
            if (!quiet)
                TargetPelvisLogAlways("[TARGET PELVIS FACE SELF] skipped=target-type reason=" + reason);
            return false;
        }

        ResolveControls();

        FreeControllerV3 pelvis = GetTargetPersonControlByAliases("pelvisControl", "pelvis");
        if (pelvis == null || pelvis.control == null)
        {
            if (!quiet)
                SetStatus("Target Pelvis Face Self / pelvis missing");
            if (!quiet)
                TargetPelvisLogAlways("[TARGET PELVIS FACE SELF] skipped=pelvis-missing reason=" + reason);
            return false;
        }

        Vector3 selfRef = GetTargetPelvisFaceSelfReferencePosition();
        Vector3 pelvisPos = pelvis.control.position;
        Vector3 worldDir = selfRef - pelvisPos;
        worldDir.y = 0.0f;

        if (worldDir.sqrMagnitude < 0.000001f)
        {
            if (!quiet)
                SetStatus("Target Pelvis Face Self / direction too small");
            if (!quiet)
                TargetPelvisLogAlways("[TARGET PELVIS FACE SELF] skipped=dir-small reason=" + reason +
                    " pelvis=" + FormatVector3(pelvisPos) +
                    " self=" + FormatVector3(selfRef));
            return false;
        }

        worldDir.Normalize();

        Transform parent = pelvis.control.parent;
        Vector3 localDir = parent != null ? parent.InverseTransformDirection(worldDir) : worldDir;
        localDir.y = 0.0f;

        if (localDir.sqrMagnitude < 0.000001f)
        {
            if (!quiet)
                SetStatus("Target Pelvis Face Self / local direction too small");
            if (!quiet)
                TargetPelvisLogAlways("[TARGET PELVIS FACE SELF] skipped=local-dir-small reason=" + reason +
                    " worldDir=" + FormatVector3(worldDir));
            return false;
        }

        localDir.Normalize();

        float y = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;
        y = Mathf.Repeat(y + yawOffsetDegrees, 360.0f);

        Vector3 before = pelvis.control.localRotation.eulerAngles;
        pelvis.control.localRotation = Quaternion.Euler(before.x, y, before.z);

        try
        {
            pelvis.currentRotationState = FreeControllerV3.RotationState.On;
        }
        catch { }

        Vector3 after = pelvis.control.localRotation.eulerAngles;

        if (!quiet)
            SetStatus("Target Pelvis Face Self / y=" + y.ToString("F1", CultureInfo.InvariantCulture));

        if (!quiet)
            TargetPelvisLogAlways("[TARGET PELVIS FACE SELF] reason=" + reason +
                " oneShot=1" +
                " yawOffset=" + yawOffsetDegrees.ToString("F1", CultureInfo.InvariantCulture) +
                " y=" + y.ToString("F1", CultureInfo.InvariantCulture) +
                " rotState=" + pelvis.currentRotationState.ToString() +
                " self=" + FormatVector3(selfRef) +
                " pelvis=" + FormatVector3(pelvisPos) +
                " worldDir=" + FormatVector3(worldDir) +
                " localDir=" + FormatVector3(localDir) +
                " before=" + FormatVector3(before) +
                " after=" + FormatVector3(after));

        return true;
    }

    private void CaptureTargetPelvisRotX()
    {
        ResolveControls();

        FreeControllerV3 pelvis = GetTargetPersonControlByAliases("pelvisControl", "pelvis");
        if (pelvis == null || pelvis.control == null)
        {
            SetStatus("Capture Target Pelvis Rot X / pelvis missing");
            return;
        }

        float x = Mathf.Repeat(pelvis.control.localRotation.eulerAngles.x, 360.0f);
        suppressTargetPelvisRotXCallback = true;
        try
        {
            if (targetPelvisRotXJSON != null)
                targetPelvisRotXJSON.val = x;
        }
        finally
        {
            suppressTargetPelvisRotXCallback = false;
        }

        SetStatus("Captured Target Pelvis Rot X / x=" + x.ToString("F1", CultureInfo.InvariantCulture));
        if (IsDebugEnabled())
            DebugLog("[TARGET PELVIS ROT X CAPTURE] x=" + x.ToString("F1", CultureInfo.InvariantCulture));
    }

    private void TargetPelvisAutoTest()
    {
        TargetPelvisLogAlways("[TARGET PELVIS DEBUG BUTTON] pressed=auto");
        ApplyTargetPelvisAutoOnGrab("button-auto", false);
    }

    private void TargetPelvisLog()
    {
        TargetPelvisLogAlways("[TARGET PELVIS DEBUG BUTTON] pressed=log");
        if (!IsTargetPersonMode() || selectedTargetPerson == null)
        {
            TargetPelvisLogAlways("[TARGET PELVIS STATE] skipped=no-target-person");
            return;
        }

        ResolveControls();
        FreeControllerV3 pelvis = GetTargetPersonControlByAliases("pelvisControl", "pelvis");
        if (pelvis == null || pelvis.control == null)
        {
            TargetPelvisLogAlways("[TARGET PELVIS STATE] skipped=pelvis-missing");
            return;
        }

        TargetPelvisLogAlways("[TARGET PELVIS STATE] rotState=" + pelvis.currentRotationState.ToString() +
            " localEuler=" + FormatVector3(pelvis.control.localRotation.eulerAngles) +
            " worldEuler=" + FormatVector3(pelvis.control.rotation.eulerAngles) +
            " pos=" + FormatVector3(pelvis.control.position));
    }

    private void TargetPelvis90BackTest()
    {
        TargetPelvisLogAlways("[TARGET PELVIS DEBUG BUTTON] pressed=90");
        ApplyTargetPelvisRotX(90.0f, "button-90-one-shot", false);
    }

    private void TargetPelvis270NearTest()
    {
        TargetPelvisLogAlways("[TARGET PELVIS DEBUG BUTTON] pressed=270");
        ApplyTargetPelvisRotX(270.0f, "button-270-one-shot", false);
    }

    private void TargetPelvisFaceSelfTest()
    {
        TargetPelvisLogAlways("[TARGET PELVIS DEBUG BUTTON] pressed=face-self");
        ApplyTargetPelvisFaceSelfYaw(0.0f, "button-face-self-one-shot", false);
    }

    private void TargetPelvisFaceSelf180Test()
    {
        TargetPelvisLogAlways("[TARGET PELVIS DEBUG BUTTON] pressed=face-self-180");
        ApplyTargetPelvisFaceSelfYaw(180.0f, "button-face-self-180-one-shot", false);
    }

    private void TargetPelvisLogAlways(string text)
    {
        // v5au: Pelvis diagnostics are useful while tuning, but too noisy for normal use.
        // Route them through DebugLog so they only appear when Debug Log is ON.
        DebugLog(text);
    }

    private void GrabHand()
    {
        // V5y: keep normal GrabHand movement identical to the stable v5n route.
        // Only reset the Hug Body Pull/Push HDC base so the next Pull/Push captures the new pose.
        if (IsTargetPersonMode() && IsHugBodyTarget())
            ResetHugBodyHdcHipUpperState("grab-hand-start");

        StartTimedGrab(true, false);

        // v5ah: apply after StartTimedGrab(), because StartTimedGrab() calls ResolveControls()
        // and makes the target Person/controller state definite. v5ag could return early
        // before selectedTargetPerson was ready, so no [TARGET PELVIS AUTO GRAB] log appeared.
        // This is still not additive: repeated presses set pelvis X plus one-shot face-self yaw.
        ApplyTargetPelvisAutoOnGrab();
    }

    private void GrabHead()
    {
        ResolveControls();
        if (headControl == null)
        {
            SetStatus("Kiss needs headControl");
            return;
        }

        StartTimedGrab(false, false, false, true);
    }

    private void RelaxKissChestIK()
    {
        if (chestControl == null)
            return;

        RelaxTemporaryLinkedIK(chestControl);
        DebugLog("[KISS] self chest IK relaxed");
    }

    private void GrabHandPull()
    {
        ResolveControls();

        if (TryRunTargetNoneBodyMovePullPush(false))
            return;

        if (TryRunChestHoldGrabHandPull())
            return;

        // V5y: Hug Body Pull/Push is no longer based on hand reach.
        // Use a HumanDrivenController Hip-Upper style local Rot X/Y group transform,
        // while leaving Grab Hand itself on the stable original route.
        if (TryRunHugBodyHdcHipUpperPullPush(false))
            return;

        List<FreeControllerV3> pullControls = GetGrabHandPullTargetControls();
        if (pullControls.Count == 0)
        {
            SetStatus("Grab Hand Pull needs movable target control");
            return;
        }

        if (IsPullToHandTargetMode())
        {
            float maxDistance;
            int movedCount;
            int snappedHands;
            bool pulledToHand = TryPullTargetControlsToActiveHands(pullControls, out maxDistance, out movedCount, out snappedHands);

            UpdateGrabHandUtilityButtons();
            StartTimedGrab(true, false, pulledToHand, false, true);
            QueueAutoSnapPullOpenIK(null);

            if (pulledToHand)
            {
                SetStatus("Grab Hand Pull To Hand / moved=" + movedCount.ToString(CultureInfo.InvariantCulture) +
                    " / maxDist=" + maxDistance.ToString("F3", CultureInfo.InvariantCulture) +
                    " / handSnap=" + snappedHands.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                SetStatus("Grab Hand Pull To Hand / already close");
            }
            return;
        }

        bool upperBodyPivot = IsUpperBodyPivotTargetMode();
        List<FreeControllerV3> movedControls = upperBodyPivot ? GetUpperBodyPivotControls(pullControls) : pullControls;

        float shortage;
        Vector3 pullOffset;
        int hugDepthSnappedHands;
        bool pulled = upperBodyPivot && IsHugBodyTarget()
            ? TryGetHugBodyDepthPivotOffset(false, out pullOffset, out shortage, out hugDepthSnappedHands)
            : TryGetGrabPullOffset(out pullOffset, out shortage);
        if (pulled)
        {
            PrepareTemporaryRelaxLinkedIK(movedControls);
            if (upperBodyPivot && IsHugBodyTarget())
                RelaxHugBodyPullPushTargetNeckHeadIK("Pull");

            if (upperBodyPivot)
                ApplyUpperBodyPivotOffset(movedControls, pullOffset, "Pull");
            else
                ApplyGrabPullOffset(pullControls, pullOffset);
        }

        UpdateGrabHandUtilityButtons();
        StartTimedGrab(true, false, pulled, false, true);
        if (IsHipHoldMode() || IsPeniMode())
        {
            // Hip Hold Pull moves target l/rThighControl.
            // Peni Pull moves the selected Peni control.
            // Do not auto-snap or self-follow those target controls back after the pull.
            QueueAutoSnapPullOpenIK(null);
        }
        else
        {
            QueueAutoSnapPullOpenIK(movedControls);
            QueueSelfFollowParentTargets(movedControls);
        }

        if (pulled)
        {
            SetStatus("Grab Hand Pull" + (upperBodyPivot ? " Pivot" : "") + " / pulled=" + pullOffset.magnitude.ToString("F3", CultureInfo.InvariantCulture) +
                " / shortage=" + shortage.ToString("F3", CultureInfo.InvariantCulture));
        }
        else
        {
            SetStatus("Grab Hand Pull / reachable");
        }
    }

    private void GrabHandPush()
    {
        ResolveControls();

        if (TryRunTargetNoneBodyMovePullPush(true))
            return;

        if (TryRunChestHoldGrabHandPush())
            return;

        // V5y: Hug Body Pull/Push is no longer based on hand reach.
        // Use a HumanDrivenController Hip-Upper style local Rot X/Y group transform,
        // while leaving Grab Hand itself on the stable original route.
        if (TryRunHugBodyHdcHipUpperPullPush(true))
            return;

        List<FreeControllerV3> pushControls = GetGrabHandPullTargetControls();
        if (pushControls.Count == 0)
        {
            SetStatus("Grab Hand Push needs movable target control");
            return;
        }

        bool upperBodyPivot = IsUpperBodyPivotTargetMode();
        List<FreeControllerV3> movedControls = upperBodyPivot ? GetUpperBodyPivotControls(pushControls) : pushControls;
        PrepareTemporaryRelaxLinkedIK(movedControls);

        float maxDistance;
        int movedCount;
        int snappedHands;
        Vector3 pivotPushOffset;
        bool pushed;
        if (upperBodyPivot)
        {
            pushed = IsHugBodyTarget()
                ? TryGetHugBodyDepthPivotOffset(true, out pivotPushOffset, out maxDistance, out snappedHands)
                : TryGetUpperBodyPivotPushOffset(out pivotPushOffset, out maxDistance, out snappedHands);
            if (pushed)
            {
                if (IsHugBodyTarget())
                    RelaxHugBodyPullPushTargetNeckHeadIK("Push");

                ApplyUpperBodyPivotOffset(movedControls, pivotPushOffset, "Push");
                movedCount = movedControls.Count;
            }
            else
            {
                movedCount = 0;
            }
        }
        else
        {
            pushed = TryPushTargetControlsFromActiveHands(pushControls, out maxDistance, out movedCount, out snappedHands);
            pivotPushOffset = Vector3.zero;
        }

        UpdateGrabHandUtilityButtons();
        StartTimedGrab(true, false, pushed, false, true);

        if (IsHipHoldMode() || IsPeniMode())
        {
            // Hip Hold Push moves target l/rThighControl.
            // Peni Push moves the selected Peni control.
            // Do not auto-snap or self-follow those target controls back after the push.
            QueueAutoSnapPullOpenIK(null);
        }
        else
        {
            QueueAutoSnapPullOpenIK(movedControls);
            QueueSelfFollowParentTargets(movedControls);
        }

        if (pushed)
        {
            float pushAmount = upperBodyPivot ? pivotPushOffset.magnitude : GRAB_HAND_PUSH_DISTANCE * GetGrabPullDistanceScale();
            SetStatus("Grab Hand Push" + (upperBodyPivot ? " Pivot" : "") + " / moved=" + movedCount.ToString(CultureInfo.InvariantCulture) +
                " / push=" + pushAmount.ToString("F3", CultureInfo.InvariantCulture) +
                " / maxDist=" + maxDistance.ToString("F3", CultureInfo.InvariantCulture) +
                " / handSnap=" + snappedHands.ToString(CultureInfo.InvariantCulture));
        }
        else
        {
            SetStatus("Grab Hand Push / already away");
        }
    }

    private void GrabHandUp()
    {
        GrabHandVertical(true);
    }

    private void GrabHandDown()
    {
        GrabHandVertical(false);
    }

    private void GrabHandVertical(bool up)
    {
        ResolveControls();

        if (TryRunTargetNoneBodyMoveVertical(up))
            return;

        if (TryRunChestHoldGrabHandVertical(up))
            return;

        List<FreeControllerV3> baseControls = GetGrabHandPullTargetControls();
        if (baseControls.Count == 0)
        {
            SetStatus(up ? "Grab Hand Up needs movable target control" : "Grab Hand Down needs movable target control");
            return;
        }

        bool upperBodyGroup = IsUpperBodyPivotTargetMode();
        List<FreeControllerV3> movedControls = upperBodyGroup ? GetUpperBodyPivotControls(baseControls) : baseControls;
        PrepareTemporaryRelaxLinkedIK(movedControls);

        float moveDistance = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_VERTICAL_DISTANCE * GetGrabPullDistanceScale());
        Vector3 offset = new Vector3(0.0f, up ? moveDistance : -moveDistance, 0.0f);
        int movedCount = ApplyGrabHandVerticalOffset(movedControls, offset);
        bool moved = movedCount > 0;

        UpdateGrabHandUtilityButtons();
        StartTimedGrab(true, false, moved, false, true);

        if (IsHipHoldMode() || IsPeniMode())
        {
            QueueAutoSnapPullOpenIK(null);
        }
        else
        {
            QueueAutoSnapPullOpenIK(movedControls);
            QueueSelfFollowParentTargets(movedControls);
        }

        SetStatus((up ? "Grab Hand Up" : "Grab Hand Down") + (upperBodyGroup ? " Upper" : "") +
            " / moved=" + movedCount.ToString(CultureInfo.InvariantCulture) +
            " / y=" + offset.y.ToString("F3", CultureInfo.InvariantCulture));
    }

    private int ApplyGrabHandVerticalOffset(List<FreeControllerV3> controls, Vector3 offset)
    {
        int moved = 0;
        if (controls == null || controls.Count == 0 || offset.sqrMagnitude < 0.0001f)
            return moved;

        foreach (FreeControllerV3 fc in controls)
        {
            if (fc == null)
                continue;

            Vector3 pos = GetControlPosition(fc);
            Vector3 nextPos = pos + offset;
            MoveTargetControlToPosition(fc, nextPos);
            LockTargetIKControl(fc);
            moved++;

            DebugLog("[HAND VERTICAL] target=" + fc.name +
                " offset=" + FormatVector3(offset) +
                " from=" + FormatVector3(pos) +
                " to=" + FormatVector3(nextPos));
        }

        return moved;
    }

    private void GrabHandOpen()
    {
        ResolveControls();

        if (TryRunChestHoldGrabHandOpenClose(true))
            return;

        FreeControllerV3 left;
        FreeControllerV3 right;
        if (TryGetGrabHandOpenTargetControls(out left, out right))
        {
            PrepareTemporaryRelaxLinkedIK(new List<FreeControllerV3> { left, right });
            ApplyGrabHandOpenOffset(left, right);
            UpdateGrabHandUtilityButtons();
            StartTimedGrab(true, false, true);
            QueueAutoSnapPullOpenIK(new List<FreeControllerV3> { left, right });
            QueueSelfFollowParentTargets(new List<FreeControllerV3> { left, right });
            SetStatus("Grab Hand Open");
            return;
        }

        FreeControllerV3 single;
        bool singleRightSide;
        if (!TryGetGrabHandOpenSingleTargetControl(out single, out singleRightSide))
        {
            SetStatus("Grab Hand Open needs openable target");
            return;
        }

        PrepareTemporaryRelaxLinkedIK(new List<FreeControllerV3> { single });
        ApplyGrabHandOpenOffset(single, singleRightSide);
        UpdateGrabHandUtilityButtons();
        StartTimedGrab(true, false, true);
        QueueAutoSnapPullOpenIK(new List<FreeControllerV3> { single });
        QueueSelfFollowParentTargets(new List<FreeControllerV3> { single });
        SetStatus("Grab Hand Open");
    }

    private void GrabHandClose()
    {
        ResolveControls();

        if (TryRunChestHoldGrabHandOpenClose(false))
            return;

        FreeControllerV3 left;
        FreeControllerV3 right;
        if (TryGetGrabHandOpenTargetControls(out left, out right))
        {
            PrepareTemporaryRelaxLinkedIK(new List<FreeControllerV3> { left, right });
            ApplyGrabHandCloseOffset(left, right);
            UpdateGrabHandUtilityButtons();
            StartTimedGrab(true, false, true);
            QueueAutoSnapPullOpenIK(new List<FreeControllerV3> { left, right });
            QueueSelfFollowParentTargets(new List<FreeControllerV3> { left, right });
            SetStatus("Grab Hand Close");
            return;
        }

        FreeControllerV3 single;
        bool singleRightSide;
        if (!TryGetGrabHandOpenSingleTargetControl(out single, out singleRightSide))
        {
            SetStatus("Grab Hand Close needs closable target");
            return;
        }

        PrepareTemporaryRelaxLinkedIK(new List<FreeControllerV3> { single });
        ApplyGrabHandCloseOffset(single, singleRightSide);
        UpdateGrabHandUtilityButtons();
        StartTimedGrab(true, false, true);
        QueueAutoSnapPullOpenIK(new List<FreeControllerV3> { single });
        QueueSelfFollowParentTargets(new List<FreeControllerV3> { single });
        SetStatus("Grab Hand Close");
    }

    private void GrabHandLeft()
    {
        GrabHandHorizontal(false);
    }

    private void GrabHandRight()
    {
        GrabHandHorizontal(true);
    }

    private void GrabHandHorizontal(bool right)
    {
        ResolveControls();

        if (TryRunTargetNoneBodyMoveHorizontal(right))
            return;

        if (TryRunChestHoldGrabHandHorizontal(right))
            return;

        List<FreeControllerV3> baseControls = GetGrabHandPullTargetControls();
        if (baseControls.Count == 0)
        {
            SetStatus(right ? "Grab Hand Right needs movable target control" : "Grab Hand Left needs movable target control");
            return;
        }

        bool upperBodyGroup = IsUpperBodyPivotTargetMode();
        List<FreeControllerV3> movedControls = upperBodyGroup ? GetUpperBodyPivotControls(baseControls) : baseControls;
        PrepareTemporaryRelaxLinkedIK(movedControls);

        Vector3 side = GetActorViewSideAxisForControls(movedControls);
        if (side.sqrMagnitude < 0.0001f)
        {
            SetStatus(right ? "Grab Hand Right / no side axis" : "Grab Hand Left / no side axis");
            return;
        }

        float moveDistance = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_HORIZONTAL_DISTANCE * GetGrabPullDistanceScale());
        Vector3 offset = (right ? side : -side) * moveDistance;
        int movedCount = ApplyGrabHandHorizontalOffset(movedControls, offset);
        bool moved = movedCount > 0;

        UpdateGrabHandUtilityButtons();
        StartTimedGrab(true, false, moved, false, true);

        if (IsHipHoldMode() || IsPeniMode())
        {
            QueueAutoSnapPullOpenIK(null);
        }
        else
        {
            QueueAutoSnapPullOpenIK(movedControls);
            QueueSelfFollowParentTargets(movedControls);
        }

        SetStatus((right ? "Grab Hand Right" : "Grab Hand Left") + (upperBodyGroup ? " Upper" : "") +
            " / moved=" + movedCount.ToString(CultureInfo.InvariantCulture) +
            " / x=" + moveDistance.ToString("F3", CultureInfo.InvariantCulture));
    }

    private int ApplyGrabHandHorizontalOffset(List<FreeControllerV3> controls, Vector3 offset)
    {
        int moved = 0;
        if (controls == null || controls.Count == 0 || offset.sqrMagnitude < 0.0001f)
            return moved;

        foreach (FreeControllerV3 fc in controls)
        {
            if (fc == null)
                continue;

            MoveTargetControlByOffset(fc, offset);
            LockTargetIKControl(fc);
            moved++;

            DebugLog("[HAND HORIZONTAL] target=" + fc.name +
                " offset=" + FormatVector3(offset));
        }

        return moved;
    }

    private bool TryRunTargetNoneBodyMovePullPush(bool push)
    {
        if (!IsTargetNoneBodyMoveMode())
            return false;

        Vector3 depthAxis;
        if (!TryGetTargetNoneBodyMoveDepthAxis(out depthAxis))
        {
            SetStatus(push ? "Target Body Push / no target root forward axis" : "Target Body Pull / no target root forward axis");
            return true;
        }

        float moveDistance = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_PUSH_DISTANCE * GetGrabPullDistanceScale());
        Vector3 offset = (push ? depthAxis : -depthAxis) * moveDistance;
        RunTargetNoneBodyMove(push ? "Push" : "Pull", offset);
        return true;
    }

    private bool TryRunTargetNoneBodyMoveVertical(bool up)
    {
        if (!IsTargetNoneBodyMoveMode())
            return false;

        float moveDistance = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_VERTICAL_DISTANCE * GetGrabPullDistanceScale());
        Vector3 offset = new Vector3(0.0f, up ? moveDistance : -moveDistance, 0.0f);
        RunTargetNoneBodyMove(up ? "Up" : "Down", offset);
        return true;
    }

    private bool TryRunTargetNoneBodyMoveHorizontal(bool right)
    {
        if (!IsTargetNoneBodyMoveMode())
            return false;

        Vector3 sideAxis;
        if (!TryGetTargetNoneBodyMoveSideAxis(out sideAxis))
        {
            SetStatus(right ? "Target Body Right / no target root side axis" : "Target Body Left / no target root side axis");
            return true;
        }

        float moveDistance = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_HORIZONTAL_DISTANCE * GetGrabPullDistanceScale());
        Vector3 offset = (right ? sideAxis : -sideAxis) * moveDistance;
        RunTargetNoneBodyMove(right ? "Right" : "Left", offset);
        return true;
    }

    private bool IsTargetNoneBodyMoveMode()
    {
        return IsTargetPersonMode() &&
               selectedTargetPerson != null &&
               targetPersonPartChooser != null &&
               targetPersonPartChooser.val == NONE;
    }

    private void RunTargetNoneBodyMove(string label, Vector3 offset)
    {
        List<FreeControllerV3> controls = GetTargetNoneBodyMoveControls();
        if (controls.Count == 0)
        {
            SetStatus("Target Body " + label + " needs hip/chest/head control");
            DebugLog("[TARGET NONE BODY MOVE] label=" + label + " controls=0");
            return;
        }

        int relaxedCount = PrepareTargetNoneBodyMoveRelaxIK(controls, label);
        int movedCount = ApplyTargetNoneBodyMoveOffset(controls, offset, label);
        bool moved = movedCount > 0;

        UpdateGrabHandUtilityButtons();
        SetStatus("Target Body " + label +
            " / moved=" + movedCount.ToString(CultureInfo.InvariantCulture) +
            " / comply=" + relaxedCount.ToString(CultureInfo.InvariantCulture) +
            " / dist=" + offset.magnitude.ToString("F3", CultureInfo.InvariantCulture) +
            " / release=Target Release");

        if (!moved)
            DebugLog("[TARGET NONE BODY MOVE] label=" + label + " moved=0 offset=" + FormatVector3(offset));
    }

    private int ApplyTargetNoneBodyMoveOffset(List<FreeControllerV3> controls, Vector3 offset, string label)
    {
        int moved = 0;
        if (controls == null || controls.Count == 0 || offset.sqrMagnitude < 0.0001f)
            return moved;

        foreach (FreeControllerV3 fc in controls)
        {
            if (fc == null)
                continue;

            MoveTargetControlByOffset(fc, offset);
            LockTargetIKControl(fc);
            moved++;

            DebugLog("[TARGET NONE BODY MOVE] label=" + label +
                " target=" + fc.name +
                " offset=" + FormatVector3(offset));
        }

        return moved;
    }

    private List<FreeControllerV3> GetTargetNoneBodyMoveControls()
    {
        List<FreeControllerV3> controls = new List<FreeControllerV3>();
        if (selectedTargetPerson == null)
            return controls;

        AddControlIfNotNull(controls, GetTargetPersonControlByAliases("hipControl", "hip"));
        AddControlIfNotNull(controls, GetTargetPersonControlByAliases("abdomenControl", "abdomen"));
        AddControlIfNotNull(controls, GetTargetPersonControlByAliases("chestControl", "chest"));
        AddControlIfNotNull(controls, GetTargetPersonControlByAliases("neckControl", "neck"));
        AddControlIfNotNull(controls, GetTargetPersonControlByAliases("headControl", "head"));
        return controls;
    }

    private bool TryGetTargetNoneBodyMoveDepthAxis(out Vector3 axis)
    {
        string source;
        Vector3 side;
        return TryGetTargetNoneBodyMoveBodyAxes(out axis, out side, out source);
    }

    private bool TryGetTargetNoneBodyMoveSideAxis(out Vector3 axis)
    {
        string source;
        Vector3 forward;
        return TryGetTargetNoneBodyMoveBodyAxes(out forward, out axis, out source);
    }

    private bool TryGetTargetNoneBodyMoveBodyAxes(out Vector3 forwardAxis, out Vector3 rightAxis, out string source)
    {
        forwardAxis = Vector3.zero;
        rightAxis = Vector3.zero;
        source = "none";

        // v5bb:
        // selectedTargetPerson.transform.forward was often only the Atom transform direction and could stay world +Z,
        // while the visible/person pose direction was coming from the body controls.
        // For None Body Nudge, use the target body's visual/root-ish hip yaw first.
        if (TryGetHorizontalAxesFromTargetControl("hipControl", "hip", out forwardAxis, out rightAxis))
        {
            source = "hipControl";
            DebugLog("[TARGET NONE BODY AXIS] source=" + source +
                " forward=" + FormatVector3(forwardAxis) +
                " right=" + FormatVector3(rightAxis));
            return true;
        }

        // If hip is unavailable, chest still usually follows the current body yaw better than Atom transform.
        if (TryGetHorizontalAxesFromTargetControl("chestControl", "chest", out forwardAxis, out rightAxis))
        {
            source = "chestControl";
            DebugLog("[TARGET NONE BODY AXIS] source=" + source +
                " forward=" + FormatVector3(forwardAxis) +
                " right=" + FormatVector3(rightAxis));
            return true;
        }

        // VaM AtomControl/mainController is a better root fallback than selectedTargetPerson.transform in some scenes.
        if (selectedTargetPerson != null && selectedTargetPerson.mainController != null && selectedTargetPerson.mainController.control != null)
        {
            if (TryBuildHorizontalAxes(selectedTargetPerson.mainController.control.rotation, out forwardAxis, out rightAxis))
            {
                source = "targetMainControl";
                DebugLog("[TARGET NONE BODY AXIS] source=" + source +
                    " forward=" + FormatVector3(forwardAxis) +
                    " right=" + FormatVector3(rightAxis));
                return true;
            }
        }

        if (selectedTargetPerson != null && selectedTargetPerson.transform != null)
        {
            if (TryBuildHorizontalAxes(selectedTargetPerson.transform.rotation, out forwardAxis, out rightAxis))
            {
                source = "targetAtomTransform";
                DebugLog("[TARGET NONE BODY AXIS] source=" + source +
                    " forward=" + FormatVector3(forwardAxis) +
                    " right=" + FormatVector3(rightAxis));
                return true;
            }
        }

        if (selectedPerson != null && selectedPerson.transform != null)
        {
            if (TryBuildHorizontalAxes(selectedPerson.transform.rotation, out forwardAxis, out rightAxis))
            {
                source = "selfAtomTransform";
                DebugLog("[TARGET NONE BODY AXIS] source=" + source +
                    " forward=" + FormatVector3(forwardAxis) +
                    " right=" + FormatVector3(rightAxis));
                return true;
            }
        }

        return false;
    }

    private bool TryGetHorizontalAxesFromTargetControl(string primaryName, string fallbackName, out Vector3 forwardAxis, out Vector3 rightAxis)
    {
        forwardAxis = Vector3.zero;
        rightAxis = Vector3.zero;

        FreeControllerV3 fc = GetTargetPersonControlByAliases(primaryName, fallbackName);
        if (fc == null)
            return false;

        Quaternion rot = fc.control != null ? fc.control.rotation : fc.transform.rotation;
        return TryBuildHorizontalAxes(rot, out forwardAxis, out rightAxis);
    }

    private bool TryBuildHorizontalAxes(Quaternion rot, out Vector3 forwardAxis, out Vector3 rightAxis)
    {
        forwardAxis = rot * Vector3.forward;
        forwardAxis.y = 0.0f;

        rightAxis = rot * Vector3.right;
        rightAxis.y = 0.0f;

        if (forwardAxis.sqrMagnitude < 0.0001f && rightAxis.sqrMagnitude >= 0.0001f)
        {
            rightAxis.Normalize();
            forwardAxis = Vector3.Cross(rightAxis, Vector3.up);
        }
        else if (rightAxis.sqrMagnitude < 0.0001f && forwardAxis.sqrMagnitude >= 0.0001f)
        {
            forwardAxis.Normalize();
            rightAxis = Vector3.Cross(Vector3.up, forwardAxis);
        }

        if (forwardAxis.sqrMagnitude < 0.0001f || rightAxis.sqrMagnitude < 0.0001f)
            return false;

        forwardAxis.Normalize();
        rightAxis.Normalize();
        return true;
    }

    private bool IsPullToHandTargetMode()
    {
        if (!IsTargetPersonMode() || targetPersonPartChooser == null)
            return false;

        string choice = targetPersonPartChooser.val;
        return choice == TC_HAND ||
               choice == TC_L_HAND ||
               choice == TC_R_HAND ||
               choice == TC_FOOT ||
               choice == TC_L_FOOT ||
               choice == TC_R_FOOT ||
               choice == TC_KNEE ||
               choice == TC_L_KNEE ||
               choice == TC_R_KNEE;
    }

    private bool TryPullTargetControlsToActiveHands(List<FreeControllerV3> pullControls, out float maxDistance, out int movedCount, out int snappedHands)
    {
        maxDistance = 0.0f;
        movedCount = 0;
        snappedHands = 0;

        if (pullControls == null || pullControls.Count == 0 || selectedPerson == null)
            return false;

        bool leftActive = leftHandJSON != null && leftHandJSON.val && lHandControl != null;
        bool rightActive = rightHandJSON != null && rightHandJSON.val && rHandControl != null;
        if (!leftActive && !rightActive)
            return false;

        if (leftActive && SnapIKControlToBody(selectedPerson, lHandControl))
            snappedHands++;
        if (rightActive && SnapIKControlToBody(selectedPerson, rHandControl))
            snappedHands++;

        PrepareTemporaryRelaxLinkedIK(pullControls);

        bool moved = false;
        if (pullControls.Count == 2 && leftActive && rightActive)
        {
            FreeControllerV3 firstTarget = pullControls[0];
            FreeControllerV3 secondTarget = pullControls[1];

            float normalCost = GetControlDistanceSqr(firstTarget, lHandControl) + GetControlDistanceSqr(secondTarget, rHandControl);
            float swappedCost = GetControlDistanceSqr(firstTarget, rHandControl) + GetControlDistanceSqr(secondTarget, lHandControl);

            if (swappedCost < normalCost)
            {
                moved |= MoveTargetControlTowardHand(firstTarget, rHandControl, ref maxDistance, ref movedCount);
                moved |= MoveTargetControlTowardHand(secondTarget, lHandControl, ref maxDistance, ref movedCount);
            }
            else
            {
                moved |= MoveTargetControlTowardHand(firstTarget, lHandControl, ref maxDistance, ref movedCount);
                moved |= MoveTargetControlTowardHand(secondTarget, rHandControl, ref maxDistance, ref movedCount);
            }
        }
        else
        {
            foreach (FreeControllerV3 target in pullControls)
            {
                FreeControllerV3 hand = GetNearestActivePullHand(target, leftActive, rightActive);
                moved |= MoveTargetControlTowardHand(target, hand, ref maxDistance, ref movedCount);
            }
        }

        return moved;
    }

    private bool IsUpperBodyPivotTargetMode()
    {
        if (!IsTargetPersonMode() || targetPersonPartChooser == null)
            return false;

        string choice = targetPersonPartChooser.val;
        return choice == TC_HUG_BODY ||
               choice == TC_CHEST_HOLD ||
               choice == TC_HEAD ||
               choice == TC_HEAD_TOP ||
               choice == TC_MOUTH ||
               choice == TC_NECK ||
               choice == TC_ABDOMEN ||
               choice == TC_L_NIPPLE ||
               choice == TC_R_NIPPLE;
    }


    private bool TryRunHugBodyHdcHipUpperPullPush(bool push)
    {
        if (!IsTargetPersonMode() || !IsHugBodyTarget() || selectedTargetPerson == null)
            return false;

        List<FreeControllerV3> controls = GetHugBodyHdcHipUpperControls();
        if (controls.Count == 0)
        {
            SetStatus(push ? "Hug Body Push needs upper IK" : "Hug Body Pull needs upper IK");
            DebugLog("[HUG BODY HDC HIP-UPPER " + (push ? "Push" : "Pull") + "] no upper controls");
            return true;
        }

        if (!EnsureHugBodyHdcHipUpperBaseCaptured(controls))
        {
            SetStatus(push ? "Hug Body Push / HDC base missing" : "Hug Body Pull / HDC base missing");
            DebugLog("[HUG BODY HDC HIP-UPPER " + (push ? "Push" : "Pull") + "] base missing");
            return true;
        }

        float amount = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_PUSH_DISTANCE * GetGrabPullDistanceScale());
        float stepX = Mathf.Min(UPPER_BODY_PIVOT_MAX_DEGREES * HUG_BODY_PUSH_PULL_PIVOT_ANGLE_MULTIPLIER,
            amount * UPPER_BODY_PIVOT_DEGREES_PER_METER * HUG_BODY_PUSH_PULL_PIVOT_ANGLE_MULTIPLIER);
        if (stepX <= 0.0001f)
        {
            SetStatus(push ? "Hug Body Push / step=0" : "Hug Body Pull / step=0");
            return true;
        }

        bool leftActive = leftHandJSON != null && leftHandJSON.val && lHandControl != null;
        bool rightActive = rightHandJSON != null && rightHandJSON.val && rHandControl != null;
        bool oneHand = leftActive != rightActive;

        float prevX = hugBodyHdcHipUpperRotX;
        float signedStepX = push ? -stepX : stepX;
        hugBodyHdcHipUpperRotX = Mathf.Clamp(hugBodyHdcHipUpperRotX + signedStepX,
            -HUG_BODY_HDC_HIP_UPPER_LIMIT_X_DEGREES,
            HUG_BODY_HDC_HIP_UPPER_LIMIT_X_DEGREES);
        float appliedStepX = hugBodyHdcHipUpperRotX - prevX;

        if (oneHand)
        {
            float sideSign = leftActive ? -1.0f : 1.0f;
            hugBodyHdcHipUpperRotY = hugBodyHdcHipUpperRotX * HUG_BODY_HDC_HIP_UPPER_ONE_HAND_YAW_SCALE * sideSign;
        }
        else
        {
            hugBodyHdcHipUpperRotY = 0.0f;
        }

        Quaternion desiredRootLocalRot = BuildHugBodyHdcDesiredRootLocalRotation(hugBodyHdcHipUpperRotX, hugBodyHdcHipUpperRotY);
        Quaternion deltaRotLocal = desiredRootLocalRot * Quaternion.Inverse(hugBodyHdcHipUpperRootBaseLocalRot);

        int moved = ApplyHugBodyHdcHipUpperLocalTransform(controls, deltaRotLocal);
        int handFollow = FollowHugBodyHdcSelfHands(deltaRotLocal);

        UpdateGrabHandUtilityButtons();
        SetStatus((push ? "Hug Body Push HDC Hip-Upper" : "Hug Body Pull HDC Hip-Upper") +
            " / moved=" + moved.ToString(CultureInfo.InvariantCulture) +
            " / rotX=" + hugBodyHdcHipUpperRotX.ToString("F1", CultureInfo.InvariantCulture) +
            " / rotY=" + hugBodyHdcHipUpperRotY.ToString("F1", CultureInfo.InvariantCulture) +
            " / handFollow=" + handFollow.ToString(CultureInfo.InvariantCulture));

        DebugLog("[HUG BODY HDC HIP-UPPER " + (push ? "Push" : "Pull") + "]" +
            " moved=" + moved.ToString(CultureInfo.InvariantCulture) +
            " hands=" + handFollow.ToString(CultureInfo.InvariantCulture) +
            " stepX=" + appliedStepX.ToString("F2", CultureInfo.InvariantCulture) +
            " rotX=" + hugBodyHdcHipUpperRotX.ToString("F2", CultureInfo.InvariantCulture) +
            " rotY=" + hugBodyHdcHipUpperRotY.ToString("F2", CultureInfo.InvariantCulture) +
            " limitX=" + HUG_BODY_HDC_HIP_UPPER_LIMIT_X_DEGREES.ToString("F2", CultureInfo.InvariantCulture) +
            " oneHand=" + Bool01(oneHand) +
            " root=" + (hugBodyHdcHipUpperRoot != null ? hugBodyHdcHipUpperRoot.name : "<none>") +
            " hdcHipUpper=1 grabHandSafe=1");

        return true;
    }

    private List<FreeControllerV3> GetHugBodyHdcHipUpperControls()
    {
        // V5ac:
        // Keep target arms natural: do not drive target hand/elbow IK during Hug Body Pull/Push.
        // Drive headControl with the torso so the upper-body angle is visible, but do not touch neckControl.
        // Head auto-snap protection remains in the snap guards; this group only applies HDC-style local transform.
        List<FreeControllerV3> controls = new List<FreeControllerV3>();
        AddControlIfNotNull(controls, GetTargetPersonControlByAliases("hipControl", "hip"));
        AddControlIfNotNull(controls, GetTargetPersonControlByAliases("chestControl", "chest"));
        AddControlIfNotNull(controls, GetTargetPersonControlByAliases("headControl", "head"));
        return controls;
    }

    private void ResetHugBodyHdcHipUpperState(string reason)
    {
        hugBodyHdcHipUpperActive = false;
        hugBodyHdcHipUpperSelfAtom = selectedPerson;
        hugBodyHdcHipUpperTargetAtom = selectedTargetPerson;
        hugBodyHdcHipUpperRotX = 0.0f;
        hugBodyHdcHipUpperRotY = 0.0f;
        hugBodyHdcHipUpperBaseCaptured = false;
        hugBodyHdcHipUpperRoot = null;
        hugBodyHdcHipUpperRootBaseLocalPos = Vector3.zero;
        hugBodyHdcHipUpperRootBaseLocalRot = Quaternion.identity;
        hugBodyHdcHipUpperBaseLocalPositions.Clear();
        hugBodyHdcHipUpperBaseLocalRotations.Clear();
        hugBodyHdcLeftHandBaseValid = false;
        hugBodyHdcRightHandBaseValid = false;
        hugBodyHdcLeftHandBaseWorldPos = Vector3.zero;
        hugBodyHdcRightHandBaseWorldPos = Vector3.zero;
        hugBodyHdcHandFollowChest = null;
        hugBodyHdcLeftHandChestLocalValid = false;
        hugBodyHdcRightHandChestLocalValid = false;
        hugBodyHdcLeftHandChestLocalOffset = Vector3.zero;
        hugBodyHdcRightHandChestLocalOffset = Vector3.zero;

        if (IsDebugEnabled())
            DebugLog("[HUG BODY HDC STATE RESET] reason=" + reason);
    }

    private bool EnsureHugBodyHdcHipUpperBaseCaptured(List<FreeControllerV3> controls)
    {
        if (hugBodyHdcHipUpperBaseCaptured &&
            hugBodyHdcHipUpperSelfAtom == selectedPerson &&
            hugBodyHdcHipUpperTargetAtom == selectedTargetPerson &&
            hugBodyHdcHipUpperRoot != null &&
            hugBodyHdcHipUpperBaseLocalPositions.Count > 0)
        {
            return true;
        }

        ResetHugBodyHdcHipUpperState("capture-base");
        hugBodyHdcHipUpperSelfAtom = selectedPerson;
        hugBodyHdcHipUpperTargetAtom = selectedTargetPerson;
        hugBodyHdcHipUpperRoot = GetTargetPersonControlByAliases("hipControl", "hip");
        if (hugBodyHdcHipUpperRoot == null || hugBodyHdcHipUpperRoot.control == null)
            return false;

        hugBodyHdcHipUpperRootBaseLocalPos = hugBodyHdcHipUpperRoot.control.localPosition;
        hugBodyHdcHipUpperRootBaseLocalRot = hugBodyHdcHipUpperRoot.control.localRotation;

        for (int i = 0; i < controls.Count; i++)
        {
            FreeControllerV3 fc = controls[i];
            if (fc == null || fc.control == null)
                continue;

            hugBodyHdcHipUpperBaseLocalPositions[fc] = fc.control.localPosition;
            hugBodyHdcHipUpperBaseLocalRotations[fc] = fc.control.localRotation;
        }

        // V5aa: self hands should follow the target chest local frame, not a large hip-pivot world arc.
        // The body motion remains HDC Hip-Upper, but the actor hands keep their captured chest-relative offsets.
        hugBodyHdcHandFollowChest = GetTargetPersonControlByAliases("chestControl", "chest");

        if (lHandControl != null && lHandControl.control != null)
        {
            hugBodyHdcLeftHandBaseValid = true;
            hugBodyHdcLeftHandBaseWorldPos = lHandControl.control.position;
            if (hugBodyHdcHandFollowChest != null && hugBodyHdcHandFollowChest.control != null)
            {
                hugBodyHdcLeftHandChestLocalValid = true;
                hugBodyHdcLeftHandChestLocalOffset = hugBodyHdcHandFollowChest.control.InverseTransformPoint(lHandControl.control.position);
            }
        }

        if (rHandControl != null && rHandControl.control != null)
        {
            hugBodyHdcRightHandBaseValid = true;
            hugBodyHdcRightHandBaseWorldPos = rHandControl.control.position;
            if (hugBodyHdcHandFollowChest != null && hugBodyHdcHandFollowChest.control != null)
            {
                hugBodyHdcRightHandChestLocalValid = true;
                hugBodyHdcRightHandChestLocalOffset = hugBodyHdcHandFollowChest.control.InverseTransformPoint(rHandControl.control.position);
            }
        }

        hugBodyHdcHipUpperActive = true;
        hugBodyHdcHipUpperBaseCaptured = hugBodyHdcHipUpperBaseLocalPositions.Count > 0;

        DebugLog("[HUG BODY HDC BASE CAPTURE]" +
            " controls=" + hugBodyHdcHipUpperBaseLocalPositions.Count.ToString(CultureInfo.InvariantCulture) +
            " root=" + FormatVector3(hugBodyHdcHipUpperRootBaseLocalPos) +
            " rootRot=" + FormatVector3(NormalizeEulerSigned(hugBodyHdcHipUpperRootBaseLocalRot.eulerAngles)) +
            " selfHands=" + Bool01(hugBodyHdcLeftHandBaseValid || hugBodyHdcRightHandBaseValid) +
            " chestFollow=" + Bool01(hugBodyHdcHandFollowChest != null && hugBodyHdcHandFollowChest.control != null) +
            " hdcHipUpper=1");
        return hugBodyHdcHipUpperBaseCaptured;
    }

    private Quaternion BuildHugBodyHdcDesiredRootLocalRotation(float addX, float addY)
    {
        Vector3 baseEuler = NormalizeEulerSigned(hugBodyHdcHipUpperRootBaseLocalRot.eulerAngles);
        return Quaternion.Euler(baseEuler.x + addX, baseEuler.y + addY, baseEuler.z);
    }

    private Vector3 NormalizeEulerSigned(Vector3 e)
    {
        return new Vector3(NormalizeAngleSigned(e.x), NormalizeAngleSigned(e.y), NormalizeAngleSigned(e.z));
    }

    private float NormalizeAngleSigned(float a)
    {
        while (a > 180.0f) a -= 360.0f;
        while (a < -180.0f) a += 360.0f;
        return a;
    }

    private int ApplyHugBodyHdcHipUpperLocalTransform(List<FreeControllerV3> controls, Quaternion deltaRotLocal)
    {
        int moved = 0;
        for (int i = 0; i < controls.Count; i++)
        {
            FreeControllerV3 fc = controls[i];
            if (fc == null || fc.control == null)
                continue;

            Vector3 baseLocalPos;
            Quaternion baseLocalRot;
            if (!hugBodyHdcHipUpperBaseLocalPositions.TryGetValue(fc, out baseLocalPos) ||
                !hugBodyHdcHipUpperBaseLocalRotations.TryGetValue(fc, out baseLocalRot))
                continue;

            CaptureTargetOriginal(fc);
            Vector3 rel = baseLocalPos - hugBodyHdcHipUpperRootBaseLocalPos;
            Vector3 nextLocalPos = hugBodyHdcHipUpperRootBaseLocalPos + deltaRotLocal * rel;
            Quaternion nextLocalRot = deltaRotLocal * baseLocalRot;

            // Match HumanDrivenController: drive the FreeController control in local space.
            fc.control.localPosition = nextLocalPos;
            fc.control.localRotation = nextLocalRot;
            LockTargetIKControl(fc);
            moved++;
        }
        return moved;
    }

    private int FollowHugBodyHdcSelfHands(Quaternion deltaRotLocal)
    {
        int moved = 0;

        // V5aa: do not rotate the actor hands around target hip.
        // Hip-pivot follow makes the hands fly up/down because the actor hands are far from the target hip.
        // Keep the captured offset in the target chest frame instead. This follows upper-body bend without the big arc.
        if (hugBodyHdcHandFollowChest != null && hugBodyHdcHandFollowChest.control != null)
        {
            if (leftHandJSON != null && leftHandJSON.val &&
                lHandControl != null && lHandControl.control != null &&
                hugBodyHdcLeftHandChestLocalValid)
            {
                Quaternion currentRot = lHandControl.control.rotation;
                Vector3 nextPos = hugBodyHdcHandFollowChest.control.TransformPoint(hugBodyHdcLeftHandChestLocalOffset);
                MoveControl(lHandControl, nextPos, currentRot, false, true);
                DebugLog("[HUG BODY HDC HAND FOLLOW] hand=L mode=chest-relative pos=" +
                    FormatVector3(lHandControl.control.position) +
                    " next=" + FormatVector3(nextPos) +
                    " local=" + FormatVector3(hugBodyHdcLeftHandChestLocalOffset));
                moved++;
            }

            if (rightHandJSON != null && rightHandJSON.val &&
                rHandControl != null && rHandControl.control != null &&
                hugBodyHdcRightHandChestLocalValid)
            {
                Quaternion currentRot = rHandControl.control.rotation;
                Vector3 nextPos = hugBodyHdcHandFollowChest.control.TransformPoint(hugBodyHdcRightHandChestLocalOffset);
                MoveControl(rHandControl, nextPos, currentRot, false, true);
                DebugLog("[HUG BODY HDC HAND FOLLOW] hand=R mode=chest-relative pos=" +
                    FormatVector3(rHandControl.control.position) +
                    " next=" + FormatVector3(nextPos) +
                    " local=" + FormatVector3(hugBodyHdcRightHandChestLocalOffset));
                moved++;
            }

            return moved;
        }

        // Fallback: if chest is missing, keep the old hip-pivot follow rather than doing nothing.
        if (selectedTargetPerson == null || selectedTargetPerson.transform == null)
            return 0;

        Vector3 pivotWorld = selectedTargetPerson.transform.TransformPoint(hugBodyHdcHipUpperRootBaseLocalPos);
        Quaternion targetRootRot = selectedTargetPerson.transform.rotation;
        Quaternion deltaRotWorld = targetRootRot * deltaRotLocal * Quaternion.Inverse(targetRootRot);

        if (leftHandJSON != null && leftHandJSON.val && lHandControl != null && lHandControl.control != null && hugBodyHdcLeftHandBaseValid)
        {
            Quaternion currentRot = lHandControl.control.rotation;
            Vector3 nextPos = pivotWorld + deltaRotWorld * (hugBodyHdcLeftHandBaseWorldPos - pivotWorld);
            MoveControl(lHandControl, nextPos, currentRot, false, true);
            DebugLog("[HUG BODY HDC HAND FOLLOW] hand=L mode=hip-fallback pos=" + FormatVector3(lHandControl.control.position) + " next=" + FormatVector3(nextPos));
            moved++;
        }

        if (rightHandJSON != null && rightHandJSON.val && rHandControl != null && rHandControl.control != null && hugBodyHdcRightHandBaseValid)
        {
            Quaternion currentRot = rHandControl.control.rotation;
            Vector3 nextPos = pivotWorld + deltaRotWorld * (hugBodyHdcRightHandBaseWorldPos - pivotWorld);
            MoveControl(rHandControl, nextPos, currentRot, false, true);
            DebugLog("[HUG BODY HDC HAND FOLLOW] hand=R mode=hip-fallback pos=" + FormatVector3(rHandControl.control.position) + " next=" + FormatVector3(nextPos));
            moved++;
        }

        return moved;
    }

    private List<FreeControllerV3> GetUpperBodyPivotControls(List<FreeControllerV3> baseControls)
    {
        List<FreeControllerV3> controls = new List<FreeControllerV3>();

        AddControlIfNotNull(controls, GetTargetPersonControlByAliases("abdomenControl", "abdomen"));
        AddControlIfNotNull(controls, GetTargetPersonControlByAliases("chestControl", "chest"));

        // V5j:
        // Hug Body should not make target neck/head jump while the hands move.
        // Other upper-body target modes keep the old neck/head pivot behavior.
        if (!ShouldKeepTargetNeckHeadStableForHugBody())
        {
            AddControlIfNotNull(controls, GetTargetPersonControlByAliases("neckControl", "neck"));
            AddControlIfNotNull(controls, GetTargetPersonControlByAliases("headControl", "head"));
        }

        if (baseControls != null)
        {
            foreach (FreeControllerV3 fc in baseControls)
            {
                if (ShouldSkipTargetNeckHeadForHugBody(fc))
                    continue;

                AddControlIfNotNull(controls, fc);
            }
        }

        return controls;
    }

    private bool ShouldKeepTargetNeckHeadStableForHugBody()
    {
        return HUG_BODY_KEEP_TARGET_NECK_HEAD_STABLE && IsHugBodyTarget();
    }

    private bool ShouldSkipTargetNeckHeadForHugBody(FreeControllerV3 fc)
    {
        return ShouldKeepTargetNeckHeadStableForHugBody() && IsTargetNeckOrHeadControl(fc);
    }

    private bool IsHeadLikeTargetMode()
    {
        if (!IsTargetPersonMode() || targetPersonPartChooser == null)
            return false;

        string choice = targetPersonPartChooser.val;
        return choice == TC_HEAD ||
               choice == TC_HEAD_TOP ||
               choice == TC_NECK ||
               choice == TC_MOUTH;
    }

    private bool ShouldSkipTargetAutoSnapIK(FreeControllerV3 fc)
    {
        if (fc == null)
            return false;

        if (IsHeldTargetHandFollowLockControl(fc))
        {
            DebugLog("[TARGET SNAP SKIP] reason=held-target-hand-follow-lock target=" + fc.name);
            return true;
        }

        if (ShouldSkipTargetNeckHeadForHugBody(fc))
            return true;

        // V5n:
        // Head/Neck/Mouth grab routes use target head/neck as the center only.
        // AutoSnap copying the real head/neck bone rotation back into headControl/neckControl
        // can make the target head spin after the hands arrive.
        // Keep the target head/neck IK control untouched; only self hands are snapped/resynced.
        return IsHeadLikeTargetMode() && IsTargetNeckOrHeadControl(fc);
    }

    private void RelaxHugBodyPullPushTargetNeckHeadIK(string reason)
    {
        if (!HUG_BODY_PULL_PUSH_RELAX_TARGET_NECK_HEAD_IK || !IsHugBodyTarget() || selectedTargetPerson == null)
            return;

        int relaxed = 0;
        FreeControllerV3 neck = GetTargetPersonControlByAliases("neckControl", "neck");
        FreeControllerV3 head = GetTargetPersonControlByAliases("headControl", "head");

        if (neck != null)
        {
            RelaxTemporaryLinkedIK(neck);
            relaxed++;
        }

        if (head != null && head != neck)
        {
            RelaxTemporaryLinkedIK(head);
            relaxed++;
        }

        if (relaxed > 0)
        {
            DebugLog("[HUG BODY RELAX HEAD IK] reason=" + reason +
                " controls=" + relaxed.ToString(CultureInfo.InvariantCulture));
        }
    }

    private bool IsTargetNeckOrHeadControl(FreeControllerV3 fc)
    {
        if (fc == null || string.IsNullOrEmpty(fc.name))
            return false;

        string n = fc.name.ToLowerInvariant();
        return n.Contains("neck") || n.Contains("head");
    }

    private FreeControllerV3 GetUpperBodyPivotPrimaryControl()
    {
        if (!IsTargetPersonMode() || targetPersonPartChooser == null)
            return null;

        string choice = targetPersonPartChooser.val;
        if (choice == TC_HEAD || choice == TC_HEAD_TOP || choice == TC_MOUTH || choice == TC_NECK)
        {
            FreeControllerV3 head = GetTargetPersonControlByAliases("headControl", "head");
            if (head != null)
                return head;
        }

        if (choice == TC_ABDOMEN)
        {
            FreeControllerV3 abdomen = GetTargetPersonControlByAliases("abdomenControl", "abdomen");
            if (abdomen != null)
                return abdomen;
        }

        FreeControllerV3 chest = GetTargetPersonControlByAliases("chestControl", "chest");
        if (chest != null)
            return chest;

        return GetTargetPersonPartControl();
    }


    private bool IsChestHoldTarget()
    {
        return IsTargetPersonMode() &&
               targetPersonPartChooser != null &&
               targetPersonPartChooser.val == TC_CHEST_HOLD;
    }

    private bool TryGetChestHoldNippleIKControls(out List<FreeControllerV3> controls, out FreeControllerV3 left, out FreeControllerV3 right)
    {
        controls = new List<FreeControllerV3>();
        left = null;
        right = null;

        if (!IsChestHoldTarget() || selectedTargetPerson == null)
            return false;

        left = GetTargetPersonControlByAliases("lNippleControl", "leftNippleControl", "lNipple", "lnipple", "leftNipple", "LeftNipple", "nipple_l", "nippleL");
        right = GetTargetPersonControlByAliases("rNippleControl", "rightNippleControl", "rNipple", "rnipple", "rightNipple", "RightNipple", "nipple_r", "nippleR");

        AddControlIfNotNull(controls, left);
        AddControlIfNotNull(controls, right);

        if (controls.Count == 0)
            AddControlIfNotNull(controls, GetTargetPersonControlByAliases(LRNIPPLE, "lrNipple", "nipplePair", "pairNipple", "nipple"));

        return controls.Count > 0;
    }

    private bool TryGetChestHoldNippleMoveControls(out List<FreeControllerV3> controls)
    {
        FreeControllerV3 left;
        FreeControllerV3 right;
        return TryGetChestHoldNippleIKControls(out controls, out left, out right);
    }

    private bool ShouldStabilizeChestHoldNippleIKOnGrabStart(bool includeHands, bool useFinalGrabWidth)
    {
        // v92: 通常の Chest Hold Grab Hand 開始だけを検出する。
        // ここでは nipple IK をONにせず、逆にロック済みなら解除する。
        // Grab Hand Pull/Push/Open/Close等のUtility後の再Grabは useFinalGrabWidth=true なので対象外。
        return includeHands && !useFinalGrabWidth && IsChestHoldTarget();
    }

    private void UpdateChestHoldNippleIKStabilizeOnGrabStart(bool includeHands, bool useFinalGrabWidth)
    {
        bool chestHoldMode = IsChestHoldTarget();

        if (ShouldStabilizeChestHoldNippleIKOnGrabStart(includeHands, useFinalGrabWidth))
        {
            // v92: Chest Hold Grab Hand must not lock/stabilize nipple IK.
            // If an older route or previous state locked it, force-release target nipple IK now.
            RestoreChestHoldNippleIKStabilize("chest-hold-grab-no-nipple-lock-restore");
            int released = ReleaseSelectedTargetNippleIK("chest-hold-grab-no-nipple-lock");
            chestHoldNoIkNippleMoveControls.Clear();
            DebugLog("[CHEST HOLD NIPPLE NO LOCK] grab-start / released=" + released.ToString(CultureInfo.InvariantCulture));
            return;
        }

        // Chest Hold utility再Grabでは既存の直接移動状態を保持する。
        // 別ターゲット/別モードへ移る場合だけ戻す。
        if (!chestHoldMode)
            RestoreChestHoldNippleIKStabilize("grab-start-non-chest-hold");
    }

    private void StabilizeChestHoldNippleIKOnGrabStart()
    {
        // v92: Chest Hold must not turn target nipple IK On during Grab Hand.
        int released = ReleaseSelectedTargetNippleIK("chest-hold-stabilize-disabled");
        chestHoldNippleStabilizeControls.Clear();
        chestHoldNippleStabilizePositionStates.Clear();
        chestHoldNippleStabilizeRotationStates.Clear();
        DebugLog("[CHEST HOLD NIPPLE STABILIZE] disabled / released=" + released.ToString(CultureInfo.InvariantCulture));
    }

    private int RestoreChestHoldNippleIKStabilize(string reason)
    {
        int restored = 0;

        if (chestHoldNippleStabilizeControls.Count == 0)
            return 0;

        List<FreeControllerV3> controls = new List<FreeControllerV3>(chestHoldNippleStabilizeControls);
        for (int i = 0; i < controls.Count; i++)
        {
            FreeControllerV3 fc = controls[i];
            if (fc == null)
                continue;

            FreeControllerV3.PositionState posState;
            if (chestHoldNippleStabilizePositionStates.TryGetValue(fc, out posState))
            {
                try { fc.currentPositionState = posState; } catch { }
                restored++;
            }

            FreeControllerV3.RotationState rotState;
            if (chestHoldNippleStabilizeRotationStates.TryGetValue(fc, out rotState))
            {
                try { fc.currentRotationState = rotState; } catch { }
            }
        }

        chestHoldNippleStabilizeControls.Clear();
        chestHoldNippleStabilizePositionStates.Clear();
        chestHoldNippleStabilizeRotationStates.Clear();

        DebugLog("[CHEST HOLD NIPPLE STABILIZE] restore / reason=" + reason +
            " / restored=" + restored.ToString(CultureInfo.InvariantCulture));
        return restored;
    }

    private int ReleaseSelectedTargetNippleIK(string reason)
    {
        ClearChestHoldNippleHandFollow(reason + "-target-nipple-release");

        if (selectedTargetPerson == null)
            return 0;

        List<FreeControllerV3> controls = new List<FreeControllerV3>();
        HashSet<FreeControllerV3> seen = new HashSet<FreeControllerV3>();
        string[][] aliasGroups = new string[][]
        {
            new string[] { "lNippleControl", "leftNippleControl", "lNipple", "lnipple", "leftNipple", "LeftNipple", "nipple_l", "nippleL" },
            new string[] { "rNippleControl", "rightNippleControl", "rNipple", "rnipple", "rightNipple", "RightNipple", "nipple_r", "nippleR" },
            new string[] { LRNIPPLE, "lrNipple", "nipplePair", "pairNipple", "nipple" }
        };

        for (int g = 0; g < aliasGroups.Length; g++)
        {
            string[] names = aliasGroups[g];
            if (names == null)
                continue;

            for (int i = 0; i < names.Length; i++)
            {
                FreeControllerV3 fc = GetControlFromAtom(selectedTargetPerson, names[i]);
                if (fc == null || seen.Contains(fc))
                    continue;

                seen.Add(fc);
                controls.Add(fc);
                break;
            }
        }

        int released = 0;
        for (int i = 0; i < controls.Count; i++)
        {
            FreeControllerV3 fc = controls[i];
            if (fc == null)
                continue;

            // If this nipple was registered in any lock/auto-snap bookkeeping, forget that lock too.
            targetLockControls.Remove(fc);
            targetLockPositionStates.Remove(fc);
            targetLockRotationStates.Remove(fc);
            pendingAutoSnapIKControls.Remove(fc);
            temporaryRelaxPositionStates.Remove(fc);
            temporaryRelaxRotationStates.Remove(fc);

            try { fc.currentPositionState = FreeControllerV3.PositionState.Off; } catch { }
            try { fc.currentRotationState = FreeControllerV3.RotationState.Off; } catch { }
            released++;
        }

        if (released > 0)
            DebugLog("[TARGET NIPPLE IK RELEASE] reason=" + reason + " / released=" + released.ToString(CultureInfo.InvariantCulture));

        return released;
    }

    private List<FreeControllerV3> GetChestHoldUtilityControls()
    {
        List<FreeControllerV3> baseControls = new List<FreeControllerV3>();
        AddControlIfNotNull(baseControls, GetTargetPersonControlByAliases("chestControl", "chest"));
        return GetUpperBodyPivotControls(baseControls);
    }

    private FreeControllerV3 GetChestHoldPrimaryControl()
    {
        FreeControllerV3 chest = GetTargetPersonControlByAliases("chestControl", "chest");
        if (chest != null)
            return chest;
        return GetUpperBodyPivotPrimaryControl();
    }

    private bool TryGetChestHoldDepthOffset(bool push, out Vector3 offset, out float amount, out int snappedHands)
    {
        offset = Vector3.zero;
        amount = 0.0f;
        snappedHands = 0;

        if (!IsChestHoldTarget() || selectedPerson == null || selectedTargetPerson == null)
            return false;

        bool leftActive = leftHandJSON != null && leftHandJSON.val && lHandControl != null;
        bool rightActive = rightHandJSON != null && rightHandJSON.val && rHandControl != null;
        if (!leftActive && !rightActive)
            return false;

        if (leftActive && SnapIKControlToBody(selectedPerson, lHandControl))
            snappedHands++;
        if (rightActive && SnapIKControlToBody(selectedPerson, rHandControl))
            snappedHands++;

        FreeControllerV3 primary = GetChestHoldPrimaryControl();
        Vector3 targetPos = primary != null ? GetControlPosition(primary) : GetTargetCenter();
        Vector3 depthAxis = GetFinalPointDepthAxis(targetPos);
        depthAxis.y = 0.0f;
        if (depthAxis.sqrMagnitude < 0.0001f)
            return false;
        depthAxis.Normalize();

        amount = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_PUSH_DISTANCE * GetGrabPullDistanceScale());
        if (amount <= 0.0001f)
            return false;

        // Chest Hold uses the same clear pair semantics as Hug Body:
        // Push = away from the actor, Pull = toward the actor.
        offset = (push ? depthAxis : -depthAxis) * amount;

        DebugLog("[CHEST HOLD BUTTON " + (push ? "Push" : "Pull") + "]" +
            " amount=" + amount.ToString("F3", CultureInfo.InvariantCulture) +
            " offset=" + FormatVector3(offset) +
            " depthAxis=" + FormatVector3(depthAxis) +
            " target=" + FormatVector3(targetPos) +
            " snappedHands=" + snappedHands.ToString(CultureInfo.InvariantCulture));

        return offset.sqrMagnitude > 0.0001f;
    }

    private void MoveChestHoldNippleControlByOffsetNoIKOn(FreeControllerV3 fc, Vector3 offset)
    {
        if (fc == null || offset.sqrMagnitude < 0.0001f)
            return;

        CaptureTargetOriginal(fc);
        chestHoldNoIkNippleMoveControls.Add(fc);

        Vector3 pos = fc.control != null ? fc.control.position : fc.transform.position;
        Quaternion rot = fc.control != null ? fc.control.rotation : fc.transform.rotation;
        SetControlTransformNoIKStateChange(fc, pos + offset, rot, true);
    }

    private void ApplyChestHoldNippleOpenCloseOffsetNoIKOn(FreeControllerV3 left, FreeControllerV3 right, bool open)
    {
        if (left == null || right == null || left == right)
            return;

        Vector3 leftPos = GetControlPosition(left);
        Vector3 rightPos = GetControlPosition(right);
        Vector3 axis = rightPos - leftPos;

        if (axis.sqrMagnitude < 0.0001f)
            axis = GetActorViewSideAxisForControls(new List<FreeControllerV3> { left, right });

        if (axis.sqrMagnitude < 0.0001f)
            return;

        axis.Normalize();
        Vector3 leftOffset = (open ? -axis : axis) * GRAB_HAND_OPEN_DISTANCE;
        Vector3 rightOffset = -leftOffset;

        MoveChestHoldNippleControlByOffsetNoIKOn(left, leftOffset);
        MoveChestHoldNippleControlByOffsetNoIKOn(right, rightOffset);
    }

    private Vector3 ScaleChestHoldNippleUtilityOffset(Vector3 offset)
    {
        return offset * CHEST_HOLD_NIPPLE_UTILITY_MOVE_SCALE;
    }

    private int ApplyChestHoldNippleOffset(List<FreeControllerV3> controls, Vector3 offset)
    {
        Vector3 actualAverageOffset;
        return ApplyChestHoldNippleOffset(controls, offset, out actualAverageOffset);
    }

    private int ApplyChestHoldNippleOffset(List<FreeControllerV3> controls, Vector3 offset, out Vector3 actualAverageOffset)
    {
        actualAverageOffset = Vector3.zero;

        if (controls == null || controls.Count == 0 || offset.sqrMagnitude < 0.0001f)
            return 0;

        PrepareTemporaryRelaxLinkedIK(controls);

        int moved = 0;
        Vector3 actualTotalOffset = Vector3.zero;
        foreach (FreeControllerV3 fc in controls)
        {
            if (fc == null)
                continue;

            Vector3 before = GetControlPosition(fc);
            MoveChestHoldNippleControlByOffsetNoIKOn(fc, offset);
            Vector3 after = GetControlPosition(fc);
            Vector3 actualOffset = after - before;

            if (actualOffset.sqrMagnitude > 0.000001f)
            {
                actualTotalOffset += actualOffset;
                moved++;
            }

            if (IsDebugEnabled())
                DebugLog("[CHEST HOLD NIPPLE MOVE NO IK] control=" + fc.name +
                    " request=" + FormatVector3(offset) +
                    " actual=" + FormatVector3(actualOffset) +
                    " next=" + FormatVector3(after));
        }

        if (moved > 0)
            actualAverageOffset = actualTotalOffset / (float)moved;

        return moved;
    }

    private int ApplyChestHoldChestDragFeedback(Vector3 nippleOffset, string reason, bool moveChestPosition)
    {
        if (selectedTargetPerson == null || nippleOffset.sqrMagnitude < 0.0001f)
            return 0;

        FreeControllerV3 chest = GetTargetPersonControlByAliases("chestControl", "chest");
        if (chest == null)
            return 0;

        Vector3 pos = GetControlPosition(chest);
        Quaternion rot = chest.control != null ? chest.control.rotation : chest.transform.rotation;
        Vector3 chestMove = moveChestPosition ? nippleOffset * CHEST_HOLD_NIPPLE_DRAG_CHEST_PULL_PUSH_POS_SCALE : Vector3.zero;

        Vector3 dir = nippleOffset;
        if (dir.sqrMagnitude < 0.0001f)
            return 0;
        dir.Normalize();

        Vector3 targetForward = GetTargetPersonForwardAxis();
        targetForward.y = 0.0f;
        if (targetForward.sqrMagnitude < 0.0001f)
            targetForward = Vector3.forward;
        targetForward.Normalize();

        Vector3 targetRight = GetTargetPersonRightAxis();
        targetRight.y = 0.0f;
        if (targetRight.sqrMagnitude < 0.0001f)
            targetRight = Vector3.right;
        targetRight.Normalize();

        Vector3 rotAxis = Vector3.Cross(targetForward, dir);
        if (rotAxis.sqrMagnitude < 0.0001f)
        {
            float depth = Vector3.Dot(dir, targetForward);
            if (Mathf.Abs(depth) > 0.0001f)
                rotAxis = (depth >= 0.0f ? -targetRight : targetRight);
            else
                rotAxis = Vector3.up;
        }
        rotAxis.Normalize();

        float angle = Mathf.Min(CHEST_HOLD_NIPPLE_DRAG_CHEST_MAX_DEGREES,
            nippleOffset.magnitude * CHEST_HOLD_NIPPLE_DRAG_CHEST_DEGREES_PER_METER);
        if (angle <= 0.0001f && chestMove.sqrMagnitude < 0.0001f)
            return 0;

        Quaternion deltaRot = Quaternion.AngleAxis(angle, rotAxis);
        Quaternion nextRot = deltaRot * rot;
        Vector3 nextPos = pos + chestMove;

        CaptureTargetOriginal(chest);
        MoveControl(chest, nextPos, nextRot, false, true);
        LockTargetIKControl(chest);

        if (IsDebugEnabled())
            DebugLog("[CHEST HOLD CHEST DRAG] reason=" + reason +
                " nippleOffset=" + FormatVector3(nippleOffset) +
                " chestMove=" + FormatVector3(chestMove) +
                " angle=" + angle.ToString("F2", CultureInfo.InvariantCulture) +
                " axis=" + FormatVector3(rotAxis));

        return 1;
    }

    private void FinishChestHoldNippleButton(bool moved)
    {
        RequestDeferredGrabHandUtilityButtonUpdate();
        StartTimedGrab(true, false, moved, false, true);
        QueueAutoSnapPullOpenIK(null);
    }

    private bool TryRunChestHoldGrabHandPull()
    {
        Vector3 offset;
        float amount;
        int snappedHands;
        if (!TryGetChestHoldDepthOffset(false, out offset, out amount, out snappedHands))
            return false;

        List<FreeControllerV3> movedControls;
        if (!TryGetChestHoldNippleMoveControls(out movedControls))
        {
            SetStatus("Chest Hold Pull needs nipple control");
            return true;
        }

        Vector3 nippleOffset = ScaleChestHoldNippleUtilityOffset(offset);
        Vector3 actualNippleOffset;
        int movedCount = ApplyChestHoldNippleOffset(movedControls, nippleOffset, out actualNippleOffset);
        bool moved = movedCount > 0;
        int chestMoved = moved ? ApplyChestHoldChestDragFeedback(actualNippleOffset, "Pull", true) : 0;
        FinishChestHoldNippleButton(moved);

        SetStatus("Chest Hold Pull / amount=" + nippleOffset.magnitude.ToString("F3", CultureInfo.InvariantCulture) +
            " / nipple=" + movedCount.ToString(CultureInfo.InvariantCulture) +
            " / chest=" + chestMoved.ToString(CultureInfo.InvariantCulture) +
            " / handSnap=" + snappedHands.ToString(CultureInfo.InvariantCulture));
        return true;
    }

    private bool TryRunChestHoldGrabHandPush()
    {
        Vector3 offset;
        float amount;
        int snappedHands;
        if (!TryGetChestHoldDepthOffset(true, out offset, out amount, out snappedHands))
            return false;

        List<FreeControllerV3> movedControls;
        if (!TryGetChestHoldNippleMoveControls(out movedControls))
        {
            SetStatus("Chest Hold Push needs nipple control");
            return true;
        }

        Vector3 nippleOffset = ScaleChestHoldNippleUtilityOffset(offset);
        Vector3 actualNippleOffset;
        int movedCount = ApplyChestHoldNippleOffset(movedControls, nippleOffset, out actualNippleOffset);
        bool moved = movedCount > 0;
        int chestMoved = moved ? ApplyChestHoldChestDragFeedback(actualNippleOffset, "Push", true) : 0;
        FinishChestHoldNippleButton(moved);

        SetStatus("Chest Hold Push / amount=" + nippleOffset.magnitude.ToString("F3", CultureInfo.InvariantCulture) +
            " / nipple=" + movedCount.ToString(CultureInfo.InvariantCulture) +
            " / chest=" + chestMoved.ToString(CultureInfo.InvariantCulture) +
            " / handSnap=" + snappedHands.ToString(CultureInfo.InvariantCulture));
        return true;
    }

    private bool TryRunChestHoldGrabHandVertical(bool up)
    {
        if (!IsChestHoldTarget())
            return false;

        List<FreeControllerV3> movedControls;
        if (!TryGetChestHoldNippleMoveControls(out movedControls))
        {
            SetStatus(up ? "Chest Hold Up needs nipple control" : "Chest Hold Down needs nipple control");
            return true;
        }

        float moveDistance = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_VERTICAL_DISTANCE * GetGrabPullDistanceScale());
        Vector3 offset = new Vector3(0.0f, up ? moveDistance : -moveDistance, 0.0f);
        Vector3 nippleOffset = ScaleChestHoldNippleUtilityOffset(offset);
        Vector3 actualNippleOffset;
        int movedCount = ApplyChestHoldNippleOffset(movedControls, nippleOffset, out actualNippleOffset);
        bool moved = movedCount > 0;
        int chestMoved = moved ? ApplyChestHoldChestDragFeedback(actualNippleOffset, up ? "Up" : "Down", false) : 0;

        FinishChestHoldNippleButton(moved);

        SetStatus((up ? "Chest Hold Up" : "Chest Hold Down") +
            " / nipple=" + movedCount.ToString(CultureInfo.InvariantCulture) +
            " / chest=" + chestMoved.ToString(CultureInfo.InvariantCulture) +
            " / y=" + nippleOffset.y.ToString("F3", CultureInfo.InvariantCulture));
        return true;
    }

    private bool TryRunChestHoldGrabHandHorizontal(bool right)
    {
        if (!IsChestHoldTarget())
            return false;

        List<FreeControllerV3> movedControls;
        if (!TryGetChestHoldNippleMoveControls(out movedControls))
        {
            SetStatus(right ? "Chest Hold Right needs nipple control" : "Chest Hold Left needs nipple control");
            return true;
        }

        Vector3 side = GetActorViewSideAxisForControls(movedControls);
        if (side.sqrMagnitude < 0.0001f)
        {
            SetStatus(right ? "Chest Hold Right / no side axis" : "Chest Hold Left / no side axis");
            return true;
        }

        float moveDistance = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_HORIZONTAL_DISTANCE * GetGrabPullDistanceScale());
        Vector3 offset = (right ? side.normalized : -side.normalized) * moveDistance;
        Vector3 nippleOffset = ScaleChestHoldNippleUtilityOffset(offset);
        Vector3 actualNippleOffset;
        int movedCount = ApplyChestHoldNippleOffset(movedControls, nippleOffset, out actualNippleOffset);
        bool moved = movedCount > 0;
        int chestMoved = moved ? ApplyChestHoldChestDragFeedback(actualNippleOffset, right ? "Right" : "Left", false) : 0;

        FinishChestHoldNippleButton(moved);

        SetStatus((right ? "Chest Hold Right" : "Chest Hold Left") +
            " / nipple=" + movedCount.ToString(CultureInfo.InvariantCulture) +
            " / chest=" + chestMoved.ToString(CultureInfo.InvariantCulture) +
            " / x=" + nippleOffset.magnitude.ToString("F3", CultureInfo.InvariantCulture));
        return true;
    }

    private bool TryRunChestHoldGrabHandOpenClose(bool open)
    {
        if (!IsChestHoldTarget())
            return false;

        List<FreeControllerV3> movedControls;
        FreeControllerV3 left;
        FreeControllerV3 right;
        if (!TryGetChestHoldNippleIKControls(out movedControls, out left, out right))
        {
            SetStatus(open ? "Chest Hold Open needs nipple control" : "Chest Hold Close needs nipple control");
            return true;
        }

        if (left == null || right == null || left == right)
        {
            SetStatus(open ? "Chest Hold Open needs L/R nipple control" : "Chest Hold Close needs L/R nipple control");
            return true;
        }

        PrepareTemporaryRelaxLinkedIK(movedControls);
        if (open)
            ApplyChestHoldNippleOpenCloseOffsetNoIKOn(left, right, true);
        else
            ApplyChestHoldNippleOpenCloseOffsetNoIKOn(left, right, false);

        FinishChestHoldNippleButton(true);

        SetStatus((open ? "Chest Hold Open" : "Chest Hold Close") +
            " / nipple=2" +
            " / distance=" + GRAB_HAND_OPEN_DISTANCE.ToString("F3", CultureInfo.InvariantCulture));
        DebugLog("[CHEST HOLD BUTTON " + (open ? "Open" : "Close") + "]" +
            " nipple=2 distance=" + GRAB_HAND_OPEN_DISTANCE.ToString("F3", CultureInfo.InvariantCulture));
        return true;
    }

    private bool TryGetUpperBodyPivotPushOffset(out Vector3 pushOffset, out float maxDistance, out int snappedHands)
    {
        pushOffset = Vector3.zero;
        maxDistance = 0.0f;
        snappedHands = 0;

        if (!IsTargetPersonMode() || selectedPerson == null || selectedTargetPerson == null)
            return false;

        bool leftActive = leftHandJSON != null && leftHandJSON.val && lHandControl != null;
        bool rightActive = rightHandJSON != null && rightHandJSON.val && rHandControl != null;
        if (!leftActive && !rightActive)
            return false;

        if (leftActive && SnapIKControlToBody(selectedPerson, lHandControl))
            snappedHands++;
        if (rightActive && SnapIKControlToBody(selectedPerson, rHandControl))
            snappedHands++;

        FreeControllerV3 primary = GetUpperBodyPivotPrimaryControl();
        Vector3 targetPos = primary != null ? GetControlPosition(primary) : GetTargetCenter();

        Vector3 dirSum = Vector3.zero;
        int count = 0;
        AddUpperBodyPivotPushHand(lHandControl, leftActive, targetPos, ref dirSum, ref count, ref maxDistance);
        AddUpperBodyPivotPushHand(rHandControl, rightActive, targetPos, ref dirSum, ref count, ref maxDistance);

        if (count <= 0 || dirSum.sqrMagnitude < 0.0001f)
            dirSum = GetGrabPushFallbackDirection(targetPos);

        dirSum.y = 0.0f;
        if (dirSum.sqrMagnitude < 0.0001f)
            return false;

        float moveDistance = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_PUSH_DISTANCE * GetGrabPullDistanceScale());
        if (moveDistance <= 0.0001f)
            return false;

        pushOffset = dirSum.normalized * moveDistance;
        return pushOffset.sqrMagnitude > 0.0001f;
    }

    private void AddUpperBodyPivotPushHand(FreeControllerV3 hand, bool enabled, Vector3 targetPos, ref Vector3 dirSum, ref int count, ref float maxDistance)
    {
        if (!enabled || hand == null)
            return;

        Vector3 handPos = GetControlPosition(hand);
        Vector3 rawDir = targetPos - handPos;
        float distance = rawDir.magnitude;
        if (distance > maxDistance)
            maxDistance = distance;

        rawDir.y = 0.0f;
        if (rawDir.sqrMagnitude < 0.0001f)
            rawDir = GetGrabPushFallbackDirection(targetPos);

        if (rawDir.sqrMagnitude < 0.0001f)
            return;

        dirSum += rawDir.normalized;
        count++;
    }

    private bool TryGetHugBodyDepthPivotOffset(bool push, out Vector3 offset, out float amount, out int snappedHands)
    {
        offset = Vector3.zero;
        amount = 0.0f;
        snappedHands = 0;

        if (!IsTargetPersonMode() || selectedPerson == null || selectedTargetPerson == null || !IsHugBodyTarget())
            return false;

        bool leftActive = leftHandJSON != null && leftHandJSON.val && lHandControl != null;
        bool rightActive = rightHandJSON != null && rightHandJSON.val && rHandControl != null;
        if (!leftActive && !rightActive)
            return false;

        if (leftActive && SnapIKControlToBody(selectedPerson, lHandControl))
            snappedHands++;
        if (rightActive && SnapIKControlToBody(selectedPerson, rHandControl))
            snappedHands++;

        FreeControllerV3 primary = GetUpperBodyPivotPrimaryControl();
        Vector3 targetPos = primary != null ? GetControlPosition(primary) : GetTargetCenter();
        Vector3 depthAxis = GetFinalPointDepthAxis(targetPos);
        depthAxis.y = 0.0f;

        if (depthAxis.sqrMagnitude < 0.0001f)
            return false;

        depthAxis.Normalize();

        amount = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_PUSH_DISTANCE * GetGrabPullDistanceScale());
        if (amount <= 0.0001f)
            return false;

        // v4.0cq:
        // Hug Body Push/Pull must be a true pair. Do not use reach-shortage or current hand-position
        // directions here, because the Hug Body final-point route can place hands behind/around the body
        // and make those two heuristics collapse to the same pivot direction.
        // Push  = away from the actor, along self -> target.
        // Pull  = toward the actor, opposite self -> target.
        offset = (push ? depthAxis : -depthAxis) * amount;

        DebugLog("[HUG BODY DEPTH PIVOT " + (push ? "Push" : "Pull") + "]" +
            " amount=" + amount.ToString("F3", CultureInfo.InvariantCulture) +
            " offset=" + FormatVector3(offset) +
            " depthAxis=" + FormatVector3(depthAxis) +
            " target=" + FormatVector3(targetPos) +
            " snappedHands=" + snappedHands.ToString(CultureInfo.InvariantCulture));

        return offset.sqrMagnitude > 0.0001f;
    }

    private bool ApplyUpperBodyPivotOffset(List<FreeControllerV3> controls, Vector3 offset, string reason)
    {
        if (controls == null || controls.Count == 0 || offset.sqrMagnitude < 0.0001f)
            return false;

        FreeControllerV3 hip = GetTargetPersonControlByAliases("hipControl", "hip");
        FreeControllerV3 primary = GetUpperBodyPivotPrimaryControl();
        if (hip == null || primary == null)
        {
            ApplyGrabPullOffset(controls, offset);
            DebugLog("[UPPER PIVOT " + reason + "] fallback translation / hip=" + Bool01(hip != null) + " primary=" + Bool01(primary != null));
            return false;
        }

        Vector3 pivot = GetControlPosition(hip);
        Vector3 from = GetControlPosition(primary) - pivot;
        Vector3 flatOffset = offset;
        flatOffset.y = 0.0f;
        if (from.sqrMagnitude < 0.0001f || flatOffset.sqrMagnitude < 0.0001f)
            return false;

        Vector3 to = from + flatOffset;
        if (to.sqrMagnitude < 0.0001f)
            return false;

        Quaternion rawRot = Quaternion.FromToRotation(from.normalized, to.normalized);
        float angle;
        Vector3 axis;
        rawRot.ToAngleAxis(out angle, out axis);
        if (float.IsNaN(angle) || axis.sqrMagnitude < 0.0001f)
            return false;

        if (angle > 180.0f)
            angle -= 360.0f;

        float pivotMaxDegrees = UPPER_BODY_PIVOT_MAX_DEGREES;
        float pivotDegreesPerMeter = UPPER_BODY_PIVOT_DEGREES_PER_METER;
        if (IsHugBodyTarget() && (reason == "Push" || reason == "Pull"))
        {
            pivotMaxDegrees *= HUG_BODY_PUSH_PULL_PIVOT_ANGLE_MULTIPLIER;
            pivotDegreesPerMeter *= HUG_BODY_PUSH_PULL_PIVOT_ANGLE_MULTIPLIER;
        }

        float maxAngle = Mathf.Min(pivotMaxDegrees, flatOffset.magnitude * pivotDegreesPerMeter);
        if (maxAngle <= 0.0001f)
            return false;

        float clampedAngle = Mathf.Clamp(angle, -maxAngle, maxAngle);
        Quaternion rot = Quaternion.AngleAxis(clampedAngle, axis.normalized);
        int moved = 0;

        foreach (FreeControllerV3 fc in controls)
        {
            if (fc == null || fc == hip)
                continue;

            if (ShouldSkipTargetNeckHeadForHugBody(fc))
                continue;

            CaptureTargetOriginal(fc);
            Vector3 pos = GetControlPosition(fc);
            Quaternion currentRot = fc.control != null ? fc.control.rotation : fc.transform.rotation;
            Vector3 nextPos = pivot + rot * (pos - pivot);
            Quaternion nextRot = rot * currentRot;
            MoveControl(fc, nextPos, nextRot, false, true);
            LockTargetIKControl(fc);
            moved++;
        }

        DebugLog("[UPPER PIVOT " + reason + "] moved=" + moved.ToString(CultureInfo.InvariantCulture) +
            " angle=" + clampedAngle.ToString("F2", CultureInfo.InvariantCulture) +
            " maxAngle=" + maxAngle.ToString("F2", CultureInfo.InvariantCulture) +
            " offset=" + FormatVector3(flatOffset) +
            " pivot=" + FormatVector3(pivot) +
            " primary=" + (primary != null ? primary.name : "<none>"));

        return moved > 0;
    }

    private bool TryPushTargetControlsFromActiveHands(List<FreeControllerV3> pushControls, out float maxDistance, out int movedCount, out int snappedHands)
    {
        maxDistance = 0.0f;
        movedCount = 0;
        snappedHands = 0;

        if (pushControls == null || pushControls.Count == 0 || selectedPerson == null)
            return false;

        bool leftActive = leftHandJSON != null && leftHandJSON.val && lHandControl != null;
        bool rightActive = rightHandJSON != null && rightHandJSON.val && rHandControl != null;
        if (!leftActive && !rightActive)
            return false;

        if (leftActive && SnapIKControlToBody(selectedPerson, lHandControl))
            snappedHands++;
        if (rightActive && SnapIKControlToBody(selectedPerson, rHandControl))
            snappedHands++;

        bool moved = false;
        if (pushControls.Count == 2 && leftActive && rightActive)
        {
            FreeControllerV3 firstTarget = pushControls[0];
            FreeControllerV3 secondTarget = pushControls[1];

            float normalCost = GetControlDistanceSqr(firstTarget, lHandControl) + GetControlDistanceSqr(secondTarget, rHandControl);
            float swappedCost = GetControlDistanceSqr(firstTarget, rHandControl) + GetControlDistanceSqr(secondTarget, lHandControl);

            if (swappedCost < normalCost)
            {
                moved |= MoveTargetControlAwayFromHand(firstTarget, rHandControl, ref maxDistance, ref movedCount);
                moved |= MoveTargetControlAwayFromHand(secondTarget, lHandControl, ref maxDistance, ref movedCount);
            }
            else
            {
                moved |= MoveTargetControlAwayFromHand(firstTarget, lHandControl, ref maxDistance, ref movedCount);
                moved |= MoveTargetControlAwayFromHand(secondTarget, rHandControl, ref maxDistance, ref movedCount);
            }
        }
        else
        {
            foreach (FreeControllerV3 target in pushControls)
            {
                FreeControllerV3 hand = GetNearestActivePullHand(target, leftActive, rightActive);
                moved |= MoveTargetControlAwayFromHand(target, hand, ref maxDistance, ref movedCount);
            }
        }

        return moved;
    }

    private bool MoveTargetControlAwayFromHand(FreeControllerV3 target, FreeControllerV3 hand, ref float maxDistance, ref int movedCount)
    {
        if (target == null)
            return false;

        Vector3 targetPos = GetControlPosition(target);
        Vector3 handPos = hand != null ? GetControlPosition(hand) : GetSelfReferencePosition();
        Vector3 rawDir = targetPos - handPos;
        float distance = rawDir.magnitude;
        if (distance > maxDistance)
            maxDistance = distance;

        Vector3 pushDir = rawDir;
        pushDir.y = 0.0f;

        if (pushDir.sqrMagnitude < 0.0001f)
            pushDir = GetGrabPushFallbackDirection(targetPos);

        if (pushDir.sqrMagnitude < 0.0001f)
            return false;

        pushDir.Normalize();

        float moveDistance = Mathf.Min(GRAB_PULL_MAX_DISTANCE, GRAB_HAND_PUSH_DISTANCE * GetGrabPullDistanceScale());
        if (moveDistance <= 0.0001f)
            return false;

        Vector3 nextPos = targetPos + pushDir * moveDistance;
        MoveTargetControlToPosition(target, nextPos);
        LockTargetIKControl(target);
        movedCount++;

        DebugLog("[PUSH FROM HAND] target=" + target.name +
            " hand=" + (hand != null ? hand.name : "<none>") +
            " dist=" + distance.ToString("F3", CultureInfo.InvariantCulture) +
            " move=" + moveDistance.ToString("F3", CultureInfo.InvariantCulture) +
            " dir=" + FormatVector3(pushDir) +
            " from=" + FormatVector3(targetPos) +
            " to=" + FormatVector3(nextPos) +
            " handPos=" + FormatVector3(handPos));

        return true;
    }

    private Vector3 GetGrabPushFallbackDirection(Vector3 targetPos)
    {
        Vector3 selfPos = GetSelfReferencePosition();
        Vector3 dir = targetPos - selfPos;
        dir.y = 0.0f;

        if (dir.sqrMagnitude >= 0.0001f)
            return dir.normalized;

        if (selectedPerson != null && selectedPerson.transform != null)
        {
            dir = selectedPerson.transform.forward;
            dir.y = 0.0f;
            if (dir.sqrMagnitude >= 0.0001f)
                return dir.normalized;
        }

        if (containingAtom != null && containingAtom.transform != null)
        {
            dir = containingAtom.transform.forward;
            dir.y = 0.0f;
            if (dir.sqrMagnitude >= 0.0001f)
                return dir.normalized;
        }

        return Vector3.forward;
    }

    private float GetControlDistanceSqr(FreeControllerV3 a, FreeControllerV3 b)
    {
        if (a == null || b == null)
            return float.MaxValue * 0.25f;

        return (GetControlPosition(a) - GetControlPosition(b)).sqrMagnitude;
    }

    private FreeControllerV3 GetNearestActivePullHand(FreeControllerV3 target, bool leftActive, bool rightActive)
    {
        if (target == null)
            return null;

        if (leftActive && !rightActive)
            return lHandControl;
        if (rightActive && !leftActive)
            return rHandControl;

        Vector3 targetPos = GetControlPosition(target);
        float leftDist = lHandControl != null ? (GetControlPosition(lHandControl) - targetPos).sqrMagnitude : float.MaxValue;
        float rightDist = rHandControl != null ? (GetControlPosition(rHandControl) - targetPos).sqrMagnitude : float.MaxValue;
        return rightDist < leftDist ? rHandControl : lHandControl;
    }

    private bool MoveTargetControlTowardHand(FreeControllerV3 target, FreeControllerV3 hand, ref float maxDistance, ref int movedCount)
    {
        if (target == null || hand == null)
            return false;

        Vector3 targetPos = GetControlPosition(target);
        Vector3 handPos = GetControlPosition(hand);
        Vector3 delta = handPos - targetPos;
        float distance = delta.magnitude;
        if (distance > maxDistance)
            maxDistance = distance;

        if (distance < 0.005f)
        {
            LockTargetIKControl(target);
            return false;
        }

        float moveDistance = Mathf.Min(distance, GRAB_PULL_MAX_DISTANCE);
        Vector3 nextPos = targetPos + delta.normalized * moveDistance;
        MoveTargetControlToPosition(target, nextPos);
        LockTargetIKControl(target);
        movedCount++;

        DebugLog("[PULL TO HAND] target=" + target.name +
            " hand=" + hand.name +
            " dist=" + distance.ToString("F3", CultureInfo.InvariantCulture) +
            " move=" + moveDistance.ToString("F3", CultureInfo.InvariantCulture) +
            " from=" + FormatVector3(targetPos) +
            " to=" + FormatVector3(nextPos) +
            " handPos=" + FormatVector3(handPos));

        return true;
    }

    private void MoveTargetControlToPosition(FreeControllerV3 fc, Vector3 position)
    {
        if (fc == null)
            return;

        CaptureTargetOriginal(fc);
        Quaternion rot = fc.control != null ? fc.control.rotation : fc.transform.rotation;
        MoveControl(fc, position, rot, false, true);
        if (IsHeldTargetHandFollowLockControl(fc))
            ReapplyHeldTargetHandFollowLocks("move-target-to-position");
    }

    private bool TryGetGrabPullOffset(out Vector3 pullOffset, out float maxShortage)
    {
        pullOffset = Vector3.zero;
        maxShortage = 0.0f;

        if (!IsTargetPersonMode() || selectedTargetPerson == null)
            return false;

        Vector3 center = GetTargetCenter();
        Vector3 side = GetHandSideAxis(GetTargetSideAxis());
        bool swapSidePaths = ShouldSwapSidePaths(center);
        float width = GetGrabPullStartWidth();
        float maxReach = GetMaxHandReach();

        Vector3 pullSum = Vector3.zero;
        float weightSum = 0.0f;

        AddGrabPullHandShortage(false, leftHandJSON != null && leftHandJSON.val, !swapSidePaths, center, side, width, maxReach, ref pullSum, ref weightSum, ref maxShortage);
        AddGrabPullHandShortage(true, rightHandJSON != null && rightHandJSON.val, swapSidePaths, center, side, width, maxReach, ref pullSum, ref weightSum, ref maxShortage);

        if (weightSum <= 0.0f || pullSum.sqrMagnitude < 0.0001f)
            return false;

        pullOffset = pullSum / weightSum;
        float mag = pullOffset.magnitude;
        if (mag > GRAB_PULL_MAX_DISTANCE)
            pullOffset = pullOffset.normalized * GRAB_PULL_MAX_DISTANCE;

        pullOffset *= GetGrabPullDistanceScale();

        return pullOffset.sqrMagnitude > 0.0001f;
    }

    private float GetGrabPullDistanceScale()
    {
        if (!IsTargetPersonMode() || targetPersonPartChooser == null)
            return 1.0f;

        string choice = targetPersonPartChooser.val;
        if (choice == TC_HAND || choice == TC_FOOT || choice == TC_KNEE)
            return 1.0f;

        return 0.5f;
    }

    private void AddGrabPullHandShortage(bool rightHand, bool enabled, bool pathRightSide, Vector3 center, Vector3 side, float width, float maxReach, ref Vector3 pullSum, ref float weightSum, ref float maxShortage)
    {
        if (!enabled)
            return;

        FreeControllerV3 hand = rightHand ? rHandControl : lHandControl;
        if (hand == null)
            return;

        Vector3 root = GetHandRootPosition(pathRightSide);
        Vector3 desired = center + GetSideOffset(pathRightSide, side, width);
        Vector3 delta = desired - root;
        float dist = delta.magnitude;
        float shortage = dist - maxReach;

        if (shortage <= 0.0f || dist < 0.0001f)
            return;

        float amount = shortage + GRAB_PULL_MARGIN;
        pullSum += (-delta.normalized) * amount * amount;
        weightSum += amount;
        if (shortage > maxShortage)
            maxShortage = shortage;
    }

    private float GetGrabPullStartWidth()
    {
        if (IsHipHoldMode())
            return Mathf.Max(GetFinalGrabWidth(), HIP_HOLD_GRAB_WIDTH);

        return grabWidthJSON != null
            ? Mathf.Max(GetFinalGrabWidth(), grabWidthJSON.val)
            : Mathf.Max(0.10f, GetFinalGrabWidth());
    }

    private List<FreeControllerV3> GetGrabHandPullTargetControls()
    {
        List<FreeControllerV3> controls = new List<FreeControllerV3>();

        if (!IsTargetPersonMode() || selectedTargetPerson == null || targetPersonPartChooser == null)
            return controls;

        string choice = targetPersonPartChooser.val;

        FreeControllerV3 left;
        FreeControllerV3 right;
        if (TryGetGrabHandOpenTargetControls(out left, out right))
        {
            if (left != null)
                controls.Add(left);
            if (right != null && right != left)
                controls.Add(right);
            return controls;
        }

        if (choice == TC_HEAD || choice == TC_HEAD_TOP)
            AddControlIfNotNull(controls, GetControlFromAtom(selectedTargetPerson, "headControl"));

        else if (choice == TC_MOUTH)
            AddControlIfNotNull(controls, GetControlFromAtom(selectedTargetPerson, "mouthControl") ?? GetControlFromAtom(selectedTargetPerson, "headControl"));

        else if (choice == TC_NECK)
            AddControlIfNotNull(controls, GetControlFromAtom(selectedTargetPerson, "neckControl") ?? GetControlFromAtom(selectedTargetPerson, "headControl"));

        else if (choice == TC_L_NIPPLE || choice == TC_R_NIPPLE)
            AddControlIfNotNull(controls, GetControlFromAtom(selectedTargetPerson, "chestControl"));

        else if (choice == TC_HIP_HOLD)
        {
            AddControlIfNotNull(controls, GetTargetPersonControlByAliases("lThighControl", "leftThighControl", "lThigh", "leftThigh"));
            AddControlIfNotNull(controls, GetTargetPersonControlByAliases("rThighControl", "rightThighControl", "rThigh", "rightThigh"));
        }

        else
            AddControlIfNotNull(controls, GetTargetPersonPartControl());

        return controls;
    }

    private void AddControlIfNotNull(List<FreeControllerV3> controls, FreeControllerV3 fc)
    {
        if (controls == null || fc == null || controls.Contains(fc))
            return;

        controls.Add(fc);
    }

    private bool TryGetGrabHandOpenTargetControls(out FreeControllerV3 left, out FreeControllerV3 right)
    {
        left = null;
        right = null;

        if (!IsTargetPersonMode() || selectedTargetPerson == null || targetPersonPartChooser == null)
            return false;

        string choice = targetPersonPartChooser.val;

        if (choice == TC_HAND)
        {
            left = GetControlFromAtom(selectedTargetPerson, "lHandControl") ?? GetControlFromAtom(selectedTargetPerson, "leftHandControl");
            right = GetControlFromAtom(selectedTargetPerson, "rHandControl") ?? GetControlFromAtom(selectedTargetPerson, "rightHandControl");
        }
        else if (choice == TC_FOOT)
        {
            left = GetControlFromAtom(selectedTargetPerson, "lFootControl") ?? GetControlFromAtom(selectedTargetPerson, "leftFootControl");
            right = GetControlFromAtom(selectedTargetPerson, "rFootControl") ?? GetControlFromAtom(selectedTargetPerson, "rightFootControl");
        }
        else if (choice == TC_KNEE)
        {
            left = GetControlFromAtom(selectedTargetPerson, "lKneeControl") ?? GetControlFromAtom(selectedTargetPerson, "leftKneeControl");
            right = GetControlFromAtom(selectedTargetPerson, "rKneeControl") ?? GetControlFromAtom(selectedTargetPerson, "rightKneeControl");
        }
        else
        {
            return false;
        }

        return left != null && right != null;
    }

    private bool TryGetGrabHandOpenSingleTargetControl(out FreeControllerV3 control, out bool rightSide)
    {
        control = null;
        rightSide = false;

        if (!IsTargetPersonMode() || selectedTargetPerson == null || targetPersonPartChooser == null)
            return false;

        string choice = targetPersonPartChooser.val;

        if (choice == TC_L_HAND)
        {
            control = GetControlFromAtom(selectedTargetPerson, "lHandControl") ?? GetControlFromAtom(selectedTargetPerson, "leftHandControl");
            rightSide = false;
        }
        else if (choice == TC_R_HAND)
        {
            control = GetControlFromAtom(selectedTargetPerson, "rHandControl") ?? GetControlFromAtom(selectedTargetPerson, "rightHandControl");
            rightSide = true;
        }
        else if (choice == TC_L_FOOT)
        {
            control = GetControlFromAtom(selectedTargetPerson, "lFootControl") ?? GetControlFromAtom(selectedTargetPerson, "leftFootControl");
            rightSide = false;
        }
        else if (choice == TC_R_FOOT)
        {
            control = GetControlFromAtom(selectedTargetPerson, "rFootControl") ?? GetControlFromAtom(selectedTargetPerson, "rightFootControl");
            rightSide = true;
        }
        else if (choice == TC_L_KNEE)
        {
            control = GetControlFromAtom(selectedTargetPerson, "lKneeControl") ?? GetControlFromAtom(selectedTargetPerson, "leftKneeControl");
            rightSide = false;
        }
        else if (choice == TC_R_KNEE)
        {
            control = GetControlFromAtom(selectedTargetPerson, "rKneeControl") ?? GetControlFromAtom(selectedTargetPerson, "rightKneeControl");
            rightSide = true;
        }
        else
        {
            return false;
        }

        return control != null;
    }

    private void ApplyGrabPullOffset(List<FreeControllerV3> pullControls, Vector3 pullOffset)
    {
        if (pullControls == null || pullOffset.sqrMagnitude < 0.0001f)
            return;

        foreach (FreeControllerV3 fc in pullControls)
            MoveTargetControlByOffset(fc, pullOffset);
    }

    private void ApplyGrabHandOpenOffset(FreeControllerV3 left, FreeControllerV3 right)
    {
        Vector3 leftPos = GetControlPosition(left);
        Vector3 rightPos = GetControlPosition(right);
        Vector3 axis = rightPos - leftPos;

        if (axis.sqrMagnitude < 0.0001f)
            axis = GetTargetSideAxis();

        if (axis.sqrMagnitude < 0.0001f)
            return;

        axis.Normalize();
        MoveTargetControlByOffset(left, -axis * GRAB_HAND_OPEN_DISTANCE);
        MoveTargetControlByOffset(right, axis * GRAB_HAND_OPEN_DISTANCE);
    }

    private void ApplyGrabHandOpenOffset(FreeControllerV3 control, bool rightSide)
    {
        Vector3 side = GetTargetSideAxis();
        if (side.sqrMagnitude < 0.0001f)
            return;

        Vector3 offset = GetSideOffset(rightSide, side.normalized, GRAB_HAND_OPEN_DISTANCE);
        MoveTargetControlByOffset(control, offset);
    }

    private void ApplyGrabHandCloseOffset(FreeControllerV3 left, FreeControllerV3 right)
    {
        Vector3 leftPos = GetControlPosition(left);
        Vector3 rightPos = GetControlPosition(right);
        Vector3 axis = rightPos - leftPos;

        if (axis.sqrMagnitude < 0.0001f)
            axis = GetActorViewSideAxisForControls(new List<FreeControllerV3> { left, right });

        if (axis.sqrMagnitude < 0.0001f)
            return;

        axis.Normalize();
        MoveTargetControlByOffset(left, axis * GRAB_HAND_OPEN_DISTANCE);
        MoveTargetControlByOffset(right, -axis * GRAB_HAND_OPEN_DISTANCE);
    }

    private void ApplyGrabHandCloseOffset(FreeControllerV3 control, bool rightSide)
    {
        Vector3 side = GetActorViewSideAxisForControls(new List<FreeControllerV3> { control });
        if (side.sqrMagnitude < 0.0001f)
            return;

        Vector3 openOffset = GetSideOffset(rightSide, side.normalized, GRAB_HAND_OPEN_DISTANCE);
        MoveTargetControlByOffset(control, -openOffset);
    }

    private Vector3 GetActorViewSideAxisForControls(List<FreeControllerV3> controls)
    {
        Vector3 center = Vector3.zero;
        int count = 0;

        if (controls != null)
        {
            foreach (FreeControllerV3 fc in controls)
            {
                if (fc == null)
                    continue;

                center += GetControlPosition(fc);
                count++;
            }
        }

        if (count > 0)
            center /= (float)count;
        else
            center = GetTargetCenter();

        Vector3 depthAxis = GetFinalPointDepthAxis(center);
        return GetFinalPointSideAxis(depthAxis, GetTargetSideAxis());
    }

    private void MoveTargetControlByOffset(FreeControllerV3 fc, Vector3 offset)
    {
        if (fc == null || offset.sqrMagnitude < 0.0001f)
            return;

        CaptureTargetOriginal(fc);
        Vector3 pos = fc.control != null ? fc.control.position : fc.transform.position;
        Quaternion rot = fc.control != null ? fc.control.rotation : fc.transform.rotation;
        MoveControl(fc, pos + offset, rot, false, true);
        if (IsHeldTargetHandFollowLockControl(fc))
            ReapplyHeldTargetHandFollowLocks("move-target-by-offset");
    }

    private void CaptureTargetOriginal(FreeControllerV3 fc)
    {
        if (fc == null || targetOriginalPositions.ContainsKey(fc))
            return;

        targetOriginalPositions[fc] = fc.control != null ? fc.control.position : fc.transform.position;
        targetOriginalRotations[fc] = fc.control != null ? fc.control.rotation : fc.transform.rotation;
    }

    private void ReleaseTarget()
    {
        ResetChestHoldFrontLeftGraspBoostState("target-release");
        ResetChestHoldFrontRightGraspBoostState("target-release");
        RestoreSelfFollowParentLinks();
        // v5bj: Target-side release must also unblock HBA/HLA immediately.
        // Do this before restoring saved target positions, then ResetTargetGrabberRuntimeState cleans any remaining runtime state.
        int preRestoredHeldHandLocks = RestoreHeldTargetHandFollowLocks("target-release-pre");
        ClearHeldTargetGrabState();

        bool hadTargetReleaseState = HasTargetReleaseState();
        int restored = 0;

        if (hadTargetReleaseState)
        {
            List<FreeControllerV3> controls = targetOriginalPositions.Keys.ToList();

            foreach (FreeControllerV3 fc in controls)
            {
                if (fc == null)
                    continue;

                Vector3 pos;
                Quaternion rot;
                if (!targetOriginalPositions.TryGetValue(fc, out pos))
                    continue;
                if (!targetOriginalRotations.TryGetValue(fc, out rot))
                    rot = fc.control != null ? fc.control.rotation : fc.transform.rotation;

                if (chestHoldNoIkNippleMoveControls.Contains(fc))
                    SetControlTransformNoIKStateChange(fc, pos, rot, true);
                else
                    MoveControl(fc, pos, rot, false, true);
                restored++;
            }
        }

        string resetSummary = ResetTargetGrabberRuntimeState("target-release", true);
        string hcReapplyStatus = TryReapplyTargetHumanPoseControllerAfterRelease();
        UpdateGrabHandUtilityButtons();

        if (!hadTargetReleaseState)
        {
            SetStatus("Release Target / no saved target / heldHandLocks=" + preRestoredHeldHandLocks.ToString(CultureInfo.InvariantCulture) +
                " / reset " + resetSummary + " / hc=" + hcReapplyStatus);
            return;
        }

        SetStatus("Release Target / restored=" + restored.ToString(CultureInfo.InvariantCulture) +
            " / heldHandLocks=" + preRestoredHeldHandLocks.ToString(CultureInfo.InvariantCulture) +
            " / reset " + resetSummary +
            " / hc=" + hcReapplyStatus);
    }

    private void RequestDeferredGrabHandUtilityButtonUpdate()
    {
        if (deferredGrabHandUtilityButtonUpdateRoutine != null)
        {
            try { StopCoroutine(deferredGrabHandUtilityButtonUpdateRoutine); }
            catch { }
            deferredGrabHandUtilityButtonUpdateRoutine = null;
        }

        deferredGrabHandUtilityButtonUpdateRoutine = StartCoroutine(DeferredGrabHandUtilityButtonUpdateRoutine());
    }

    private System.Collections.IEnumerator DeferredGrabHandUtilityButtonUpdateRoutine()
    {
        // Do not change Button.interactable while the Unity/VaM button is still processing its click.
        // If interactable is toggled inside the onClick call stack, the button can remain visually pressed.
        yield return null;
        deferredGrabHandUtilityButtonUpdateRoutine = null;
        UpdateGrabHandUtilityButtons();
    }

    private void UpdateGrabHandUtilityButtons()
    {
        bool pullEnabled = GetGrabHandPullTargetControls().Count > 0;
        bool targetNoneBodyMoveEnabled = IsTargetNoneBodyMoveMode() && GetTargetNoneBodyMoveControls().Count > 0;
        if (targetNoneBodyMoveEnabled)
            pullEnabled = true;
        bool pushEnabled = pullEnabled;
        bool verticalEnabled = pullEnabled;
        bool horizontalEnabled = pullEnabled;
        bool openEnabled = false;

        if (targetNoneBodyMoveEnabled && IsDebugEnabled())
            DebugLog("[TARGET NONE BODY BUTTON ENABLE] pull=1 push=1 vertical=1 horizontal=1 openClose=0");

        if (IsChestHoldTarget())
        {
            pullEnabled = true;
            pushEnabled = true;
            verticalEnabled = true;
            horizontalEnabled = true;
            openEnabled = true;

            if (IsDebugEnabled())
                DebugLog("[CHEST HOLD BUTTON ENABLE V5] pull=1 push=1 vertical=1 horizontal=1 openClose=1");
        }

        FreeControllerV3 left;
        FreeControllerV3 right;
        if (!IsChestHoldTarget())
        {
            if (TryGetGrabHandOpenTargetControls(out left, out right))
                openEnabled = true;
            else
            {
                FreeControllerV3 single;
                bool singleRightSide;
                if (TryGetGrabHandOpenSingleTargetControl(out single, out singleRightSide))
                    openEnabled = true;
            }
        }

        if (grabHandPullButton != null && grabHandPullButton.button != null)
            grabHandPullButton.button.interactable = pullEnabled;

        if (grabHandPushButton != null && grabHandPushButton.button != null)
            grabHandPushButton.button.interactable = pushEnabled;

        if (grabHandUpButton != null && grabHandUpButton.button != null)
            grabHandUpButton.button.interactable = verticalEnabled;

        if (grabHandDownButton != null && grabHandDownButton.button != null)
            grabHandDownButton.button.interactable = verticalEnabled;

        if (grabHandOpenButton != null && grabHandOpenButton.button != null)
            grabHandOpenButton.button.interactable = openEnabled;

        if (grabHandCloseButton != null && grabHandCloseButton.button != null)
            grabHandCloseButton.button.interactable = openEnabled;

        if (grabHandLeftButton != null && grabHandLeftButton.button != null)
            grabHandLeftButton.button.interactable = horizontalEnabled;

        if (grabHandRightButton != null && grabHandRightButton.button != null)
            grabHandRightButton.button.interactable = horizontalEnabled;

        if (releaseTargetButton != null && releaseTargetButton.button != null)
            releaseTargetButton.button.interactable = HasTargetReleaseState();

        UpdateReleaseButtonColors();
    }

    private bool HasTargetReleaseState()
    {
        return targetOriginalPositions.Count > 0 ||
               targetLockPositionStates.Count > 0 ||
               targetLockRotationStates.Count > 0;
    }

    private bool HasSelfReleaseState()
    {
        return hasActiveGrab ||
               swoonDropActive ||
               pufupufuActive ||
               jobActive ||
               releaseRestoreIKPending ||
               positionStateOnControls.Count > 0 ||
               rotationStateOnControls.Count > 0 ||
               releaseRestorePositionControls.Count > 0 ||
               releaseRestoreRotationControls.Count > 0 ||
               pendingSelfFollowTargets.Count > 0 ||
               activeSelfFollowParentLinks.Count > 0 ||
               selfFollowOriginalLinkStates.Count > 0 ||
               temporaryRelaxControls.Count > 0 ||
               swoonDropControls.Count > 0;
    }


    private void ToggleSwoonDrop()
    {
        if (swoonDropActive)
        {
            StopSwoonDrop(true, "button");
            return;
        }

        StartSwoonDrop();
    }

    private void StopSwoonDropAction()
    {
        StopSwoonDrop(true, "action");
    }

    private void StartSwoonDrop()
    {
        ResolveControls();
        RestoreSwoonDropIK();

        List<FreeControllerV3> allControls = GetTargetSwoonDropControls();
        HashSet<FreeControllerV3> keepControls = GetSwoonDropKeepControls();
        ApplyTargetSwoonHugBodyOneHandTwist(keepControls);

        int dropped = 0;
        for (int i = 0; i < allControls.Count; i++)
        {
            FreeControllerV3 fc = allControls[i];
            if (fc == null || keepControls.Contains(fc))
                continue;

            if (!swoonDropControls.Contains(fc))
                swoonDropControls.Add(fc);

            if (!swoonDropPositionStates.ContainsKey(fc))
                swoonDropPositionStates[fc] = fc.currentPositionState;
            if (!swoonDropRotationStates.ContainsKey(fc))
                swoonDropRotationStates[fc] = fc.currentRotationState;

            try
            {
                fc.currentPositionState = FreeControllerV3.PositionState.Off;
            }
            catch { }

            try
            {
                fc.currentRotationState = FreeControllerV3.RotationState.Off;
            }
            catch { }

            dropped++;
        }

        if (dropped <= 0)
        {
            RestoreSwoonDropIK();
            SetStatus("Target Swoon Drop / no target IK changed");
            UpdateReleaseButtonColors();
            return;
        }

        swoonDropActive = true;
        swoonDropEndTime = Time.time + SWOON_DROP_DURATION;

        SetStatus("Target Swoon Drop / IK off=" + dropped.ToString(CultureInfo.InvariantCulture) + " / keep=" + keepControls.Count.ToString(CultureInfo.InvariantCulture));
        if (IsDebugEnabled())
            DebugLog("[TARGET SWOON DROP START] dropped=" + dropped.ToString(CultureInfo.InvariantCulture) +
                " keep=" + keepControls.Count.ToString(CultureInfo.InvariantCulture) +
                " activeGrab=" + Bool01(hasActiveGrab) +
                " heldGrab=" + Bool01(HasSwoonDropHeldGrabState()) +
                " currentChoice=" + GetSwoonDropCurrentTargetChoiceForLog() +
                " heldChoice=" + (heldTargetGrabChoice ?? "<none>") +
                " endTime=" + swoonDropEndTime.ToString("F3", CultureInfo.InvariantCulture));
        UpdateReleaseButtonColors();
    }

    private void UpdateSwoonDrop()
    {
        if (!swoonDropActive)
            return;

        if (Time.time < swoonDropEndTime)
            return;

        StopSwoonDrop(true, "timeout");
    }

    private void StopSwoonDrop(bool restore, string reason)
    {
        if (!swoonDropActive && swoonDropControls.Count == 0)
            return;

        int restored = restore ? RestoreSwoonDropIK() : ClearSwoonDropStateOnly();
        swoonDropActive = false;
        swoonDropEndTime = 0.0f;

        if (IsDebugEnabled())
            DebugLog("[TARGET SWOON DROP STOP] reason=" + reason +
                " restore=" + Bool01(restore) +
                " restored=" + restored.ToString(CultureInfo.InvariantCulture));

        if (restore)
            SetStatus("Target Swoon Drop stopped / restored=" + restored.ToString(CultureInfo.InvariantCulture));

        UpdateReleaseButtonColors();
    }

    private int RestoreSwoonDropIK()
    {
        int restored = 0;
        for (int i = 0; i < swoonDropControls.Count; i++)
        {
            FreeControllerV3 fc = swoonDropControls[i];
            if (fc == null)
                continue;

            FreeControllerV3.PositionState positionState;
            if (swoonDropPositionStates.TryGetValue(fc, out positionState))
            {
                try
                {
                    fc.currentPositionState = positionState;
                }
                catch { }
            }

            FreeControllerV3.RotationState rotationState;
            if (swoonDropRotationStates.TryGetValue(fc, out rotationState))
            {
                try
                {
                    fc.currentRotationState = rotationState;
                }
                catch { }
            }

            restored++;
        }

        ClearSwoonDropStateOnly();
        return restored;
    }

    private int ClearSwoonDropStateOnly()
    {
        int count = swoonDropControls.Count;
        swoonDropPositionStates.Clear();
        swoonDropRotationStates.Clear();
        swoonDropControls.Clear();
        return count;
    }

    private List<FreeControllerV3> GetTargetSwoonDropControls()
    {
        List<FreeControllerV3> controls = new List<FreeControllerV3>();

        if (selectedTargetPerson == null)
            return controls;

        AddUniqueControl(controls, GetTargetPersonControlByAliases("hipControl", "hip"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("abdomenControl", "abdomen"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("chestControl", "chest"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("neckControl", "neck"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("headControl", "head"));

        AddUniqueControl(controls, GetTargetPersonControlByAliases("lHandControl", "leftHandControl", "lHand", "leftHand"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("rHandControl", "rightHandControl", "rHand", "rightHand"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("lElbowControl", "leftElbowControl", "lElbow", "leftElbow"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("rElbowControl", "rightElbowControl", "rElbow", "rightElbow"));

        AddUniqueControl(controls, GetTargetPersonControlByAliases("lThighControl", "leftThighControl", "lThigh", "leftThigh"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("rThighControl", "rightThighControl", "rThigh", "rightThigh"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("lFootControl", "leftFootControl", "lFoot", "leftFoot"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("rFootControl", "rightFootControl", "rFoot", "rightFoot"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("lKneeControl", "leftKneeControl", "lKnee", "leftKnee"));
        AddUniqueControl(controls, GetTargetPersonControlByAliases("rKneeControl", "rightKneeControl", "rKnee", "rightKnee"));

        return controls;
    }

    private HashSet<FreeControllerV3> GetSwoonDropKeepControls()
    {
        HashSet<FreeControllerV3> keep = new HashSet<FreeControllerV3>();

        if (selectedTargetPerson == null)
            return keep;

        // v4.0cx:
        // Prefer the currently selected Target Controller. This is safer than relying only on
        // hasActiveGrab/heldGrab, because Follow OFF completion can leave no active grab flag.
        // If the user explicitly selects None, do not keep the last grabbed controller: drop all
        // target IK, including neck, for the Swoon duration.
        string currentChoice = GetSwoonDropCurrentTargetChoice();
        if (currentChoice == NONE)
            return keep;

        if (!string.IsNullOrEmpty(currentChoice))
        {
            AddSwoonDropGrabbedOnlyControlsForChoice(keep, currentChoice);
            keep.Remove(null);
            return keep;
        }

        if (HasSwoonDropHeldGrabState())
            AddSwoonDropGrabbedOnlyControlsForChoice(keep, heldTargetGrabChoice);

        keep.Remove(null);
        return keep;
    }

    private string GetSwoonDropCurrentTargetChoice()
    {
        if (!IsTargetPersonMode() || selectedTargetPerson == null || targetPersonPartChooser == null)
            return null;

        string choice = targetPersonPartChooser.val;
        if (string.IsNullOrEmpty(choice))
            return null;

        return choice;
    }

    private string GetSwoonDropCurrentTargetChoiceForLog()
    {
        string choice = GetSwoonDropCurrentTargetChoice();
        return string.IsNullOrEmpty(choice) ? "<none>" : choice;
    }

    private void CaptureHeldTargetGrabState(bool includeHands, bool includeFeet, bool includeHead)
    {
        string choice = targetPersonPartChooser != null ? targetPersonPartChooser.val : null;
        bool hasChoice = !string.IsNullOrEmpty(choice) && choice != NONE;

        hasHeldTargetGrab = IsTargetPersonMode() && selectedTargetPerson != null && targetPersonPartChooser != null && hasChoice && (includeHands || includeFeet || includeHead);
        heldTargetGrabChoice = hasHeldTargetGrab ? choice : null;
        heldTargetGrabIncludeHands = hasHeldTargetGrab && includeHands;
        heldTargetGrabIncludeFeet = hasHeldTargetGrab && includeFeet;
        heldTargetGrabIncludeHead = hasHeldTargetGrab && includeHead;
        heldTargetGrabLeftHand = heldTargetGrabIncludeHands && leftHandJSON != null && leftHandJSON.val;
        heldTargetGrabRightHand = heldTargetGrabIncludeHands && rightHandJSON != null && rightHandJSON.val;
        heldTargetGrabLeftFoot = heldTargetGrabIncludeFeet && leftFootJSON != null && leftFootJSON.val;
        heldTargetGrabRightFoot = heldTargetGrabIncludeFeet && rightFootJSON != null && rightFootJSON.val;

        CaptureHeldTargetHandFollowLockState(choice, includeHands);
    }

    private void ClearHeldTargetGrabState()
    {
        hasHeldTargetGrab = false;
        heldTargetGrabChoice = null;
        heldTargetGrabIncludeHands = false;
        heldTargetGrabIncludeFeet = false;
        heldTargetGrabIncludeHead = false;
        heldTargetGrabLeftHand = false;
        heldTargetGrabRightHand = false;
        heldTargetGrabLeftFoot = false;
        heldTargetGrabRightFoot = false;
        ClearHeldTargetHandFollowLockState("held-grab-clear");
    }

    private void CaptureHeldTargetHandFollowLockState(string choice, bool includeHands)
    {
        heldTargetHandFollowLockLeft = false;
        heldTargetHandFollowLockRight = false;
        heldTargetHandFollowLockLeftControl = null;
        heldTargetHandFollowLockRightControl = null;

        if (includeHands && IsTargetPersonMode() && selectedTargetPerson != null && !string.IsNullOrEmpty(choice) && choice != NONE)
        {
            if (choice == TC_HAND || choice == TC_L_HAND)
            {
                heldTargetHandFollowLockLeftControl = GetTargetPersonControlByAliases("lHandControl", "leftHandControl", "lHand", "leftHand");
                heldTargetHandFollowLockLeft = heldTargetHandFollowLockLeftControl != null;
                if (heldTargetHandFollowLockLeft)
                    LockTargetIKControl(heldTargetHandFollowLockLeftControl);
            }

            if (choice == TC_HAND || choice == TC_R_HAND)
            {
                heldTargetHandFollowLockRightControl = GetTargetPersonControlByAliases("rHandControl", "rightHandControl", "rHand", "rightHand");
                heldTargetHandFollowLockRight = heldTargetHandFollowLockRightControl != null;
                if (heldTargetHandFollowLockRight)
                    LockTargetIKControl(heldTargetHandFollowLockRightControl);
            }
        }

        UpdateHeldTargetHandFollowLockStorables();

        if ((heldTargetHandFollowLockLeft || heldTargetHandFollowLockRight) && IsDebugEnabled())
        {
            DebugLog("[HELD TARGET HAND LOCK] capture / choice=" + choice +
                " / L=" + Bool01(heldTargetHandFollowLockLeft) +
                " / R=" + Bool01(heldTargetHandFollowLockRight));
        }
    }

    private void ClearHeldTargetHandFollowLockState(string reason)
    {
        bool had = heldTargetHandFollowLockLeft || heldTargetHandFollowLockRight;
        heldTargetHandFollowLockLeft = false;
        heldTargetHandFollowLockRight = false;
        heldTargetHandFollowLockLeftControl = null;
        heldTargetHandFollowLockRightControl = null;
        UpdateHeldTargetHandFollowLockStorables();
        if (had && IsDebugEnabled())
            DebugLog("[HELD TARGET HAND LOCK] clear / reason=" + reason);
    }

    private void UpdateHeldTargetHandFollowLockStorables()
    {
        bool anyHeldTargetHand = heldTargetHandFollowLockLeft || heldTargetHandFollowLockRight;
        if (tgHeldTargetHandJSON != null) tgHeldTargetHandJSON.val = anyHeldTargetHand;
        if (tgHeldTargetLHandJSON != null) tgHeldTargetLHandJSON.val = heldTargetHandFollowLockLeft;
        if (tgHeldTargetRHandJSON != null) tgHeldTargetRHandJSON.val = heldTargetHandFollowLockRight;
        if (tgHeldTargetPersonUidJSON != null) tgHeldTargetPersonUidJSON.val = anyHeldTargetHand && selectedTargetPerson != null ? selectedTargetPerson.uid : "";
    }

    private bool IsHeldTargetHandFollowLockControl(FreeControllerV3 fc)
    {
        if (fc == null) return false;
        return (heldTargetHandFollowLockLeft && fc == heldTargetHandFollowLockLeftControl) ||
               (heldTargetHandFollowLockRight && fc == heldTargetHandFollowLockRightControl);
    }

    private void ReapplyHeldTargetHandFollowLocks(string reason)
    {
        int kept = 0;
        if (heldTargetHandFollowLockLeft && heldTargetHandFollowLockLeftControl != null)
        {
            LockTargetIKControl(heldTargetHandFollowLockLeftControl);
            kept++;
        }
        if (heldTargetHandFollowLockRight && heldTargetHandFollowLockRightControl != null)
        {
            LockTargetIKControl(heldTargetHandFollowLockRightControl);
            kept++;
        }
        UpdateHeldTargetHandFollowLockStorables();
        if (kept > 0 && IsDebugEnabled())
            DebugLog("[HELD TARGET HAND LOCK] kept / reason=" + reason + " / count=" + kept.ToString(CultureInfo.InvariantCulture));
    }

    private int RestoreHeldTargetHandFollowLocks(string reason)
    {
        int restored = 0;
        List<FreeControllerV3> controls = new List<FreeControllerV3>();
        if (heldTargetHandFollowLockLeft && heldTargetHandFollowLockLeftControl != null) controls.Add(heldTargetHandFollowLockLeftControl);
        if (heldTargetHandFollowLockRight && heldTargetHandFollowLockRightControl != null && heldTargetHandFollowLockRightControl != heldTargetHandFollowLockLeftControl) controls.Add(heldTargetHandFollowLockRightControl);

        foreach (FreeControllerV3 fc in controls)
        {
            if (fc == null) continue;

            FreeControllerV3.PositionState positionState;
            if (targetLockPositionStates.TryGetValue(fc, out positionState))
            {
                try { fc.currentPositionState = positionState; } catch { }
                targetLockPositionStates.Remove(fc);
            }

            FreeControllerV3.RotationState rotationState;
            if (targetLockRotationStates.TryGetValue(fc, out rotationState))
            {
                try { fc.currentRotationState = rotationState; } catch { }
                targetLockRotationStates.Remove(fc);
            }

            targetLockControls.Remove(fc);
            restored++;
        }

        if (restored > 0 && IsDebugEnabled())
            DebugLog("[HELD TARGET HAND LOCK] restored / reason=" + reason + " / count=" + restored.ToString(CultureInfo.InvariantCulture));

        ClearHeldTargetHandFollowLockState(reason);
        return restored;
    }

    private bool HasSwoonDropHeldGrabState()
    {
        return hasHeldTargetGrab;
    }

    private void AddSwoonDropGrabbedOnlyControlsForChoice(HashSet<FreeControllerV3> keep, string choice)
    {
        if (keep == null || string.IsNullOrEmpty(choice))
            return;

        if (choice == TC_HAND)
        {
            keep.Add(GetTargetPersonControlByAliases("lHandControl", "leftHandControl", "lHand", "leftHand"));
            keep.Add(GetTargetPersonControlByAliases("rHandControl", "rightHandControl", "rHand", "rightHand"));
        }
        else if (choice == TC_L_HAND)
            keep.Add(GetTargetPersonControlByAliases("lHandControl", "leftHandControl", "lHand", "leftHand"));
        else if (choice == TC_R_HAND)
            keep.Add(GetTargetPersonControlByAliases("rHandControl", "rightHandControl", "rHand", "rightHand"));
        else if (choice == TC_FOOT)
        {
            keep.Add(GetTargetPersonControlByAliases("lFootControl", "leftFootControl", "lFoot", "leftFoot"));
            keep.Add(GetTargetPersonControlByAliases("rFootControl", "rightFootControl", "rFoot", "rightFoot"));
        }
        else if (choice == TC_L_FOOT)
            keep.Add(GetTargetPersonControlByAliases("lFootControl", "leftFootControl", "lFoot", "leftFoot"));
        else if (choice == TC_R_FOOT)
            keep.Add(GetTargetPersonControlByAliases("rFootControl", "rightFootControl", "rFoot", "rightFoot"));
        else if (choice == TC_KNEE)
        {
            keep.Add(GetTargetPersonControlByAliases("lKneeControl", "leftKneeControl", "lKnee", "leftKnee"));
            keep.Add(GetTargetPersonControlByAliases("rKneeControl", "rightKneeControl", "rKnee", "rightKnee"));
        }
        else if (choice == TC_L_KNEE)
            keep.Add(GetTargetPersonControlByAliases("lKneeControl", "leftKneeControl", "lKnee", "leftKnee"));
        else if (choice == TC_R_KNEE)
            keep.Add(GetTargetPersonControlByAliases("rKneeControl", "rightKneeControl", "rKnee", "rightKnee"));
        else if (choice == TC_HIP_HOLD)
        {
            keep.Add(GetTargetPersonControlByAliases("lThighControl", "leftThighControl", "lThigh", "leftThigh"));
            keep.Add(GetTargetPersonControlByAliases("rThighControl", "rightThighControl", "rThigh", "rightThigh"));
        }
        else if (choice == TC_HUG_BODY || choice == TC_CHEST_HOLD || choice == TC_L_NIPPLE || choice == TC_R_NIPPLE)
            keep.Add(GetTargetPersonControlByAliases("chestControl", "chest"));
        else if (choice == TC_ABDOMEN)
            keep.Add(GetTargetPersonControlByAliases("abdomenControl", "abdomen"));
        else if (choice == TC_HIP)
            keep.Add(GetTargetPersonControlByAliases("hipControl", "hip"));
        else if (choice == TC_NECK)
            keep.Add(GetTargetPersonControlByAliases("neckControl", "neck"));
        else if (choice == TC_MOUTH)
            keep.Add(GetTargetPersonControlByAliases("mouthControl", "mouth") ?? GetTargetPersonControlByAliases("headControl", "head"));
        else if (choice == TC_HEAD || choice == TC_HEAD_TOP)
            keep.Add(GetTargetPersonControlByAliases("headControl", "head"));
        else
            keep.Add(GetTargetPersonPartControl());
    }

    private void AddSwoonDropGrabSupportChain(HashSet<FreeControllerV3> keep)
    {
        if (keep == null || !IsTargetPersonMode() || selectedTargetPerson == null || targetPersonPartChooser == null)
            return;

        string choice = targetPersonPartChooser.val;

        if (choice == TC_HAND)
        {
            AddSwoonDropHandChain(keep, false);
            AddSwoonDropHandChain(keep, true);
        }
        else if (choice == TC_L_HAND)
        {
            AddSwoonDropHandChain(keep, false);
        }
        else if (choice == TC_R_HAND)
        {
            AddSwoonDropHandChain(keep, true);
        }
        else if (choice == TC_FOOT)
        {
            AddSwoonDropFootChain(keep, false);
            AddSwoonDropFootChain(keep, true);
        }
        else if (choice == TC_L_FOOT)
        {
            AddSwoonDropFootChain(keep, false);
        }
        else if (choice == TC_R_FOOT)
        {
            AddSwoonDropFootChain(keep, true);
        }
        else if (choice == TC_KNEE)
        {
            AddSwoonDropKneeChain(keep, false);
            AddSwoonDropKneeChain(keep, true);
        }
        else if (choice == TC_L_KNEE)
        {
            AddSwoonDropKneeChain(keep, false);
        }
        else if (choice == TC_R_KNEE)
        {
            AddSwoonDropKneeChain(keep, true);
        }
        else if (choice == TC_HIP_HOLD)
        {
            AddSwoonDropHipHoldKeepControls(keep);
        }
        else if (IsUpperBodyPivotTargetMode() || choice == TC_HIP || choice == TC_CHEST_HOLD || choice == TC_HUG_BODY || choice == TC_ABDOMEN || choice == TC_HEAD || choice == TC_HEAD_TOP || choice == TC_MOUTH || choice == TC_NECK)
        {
            AddSwoonDropUpperBodyKeepControls(keep, true);
        }
    }

    private void AddSwoonDropHandChain(HashSet<FreeControllerV3> keep, bool right)
    {
        if (keep == null)
            return;

        if (right)
        {
            keep.Add(GetTargetPersonControlByAliases("rHandControl", "rightHandControl", "rHand", "rightHand"));
            keep.Add(GetTargetPersonControlByAliases("rElbowControl", "rightElbowControl", "rElbow", "rightElbow"));
        }
        else
        {
            keep.Add(GetTargetPersonControlByAliases("lHandControl", "leftHandControl", "lHand", "leftHand"));
            keep.Add(GetTargetPersonControlByAliases("lElbowControl", "leftElbowControl", "lElbow", "leftElbow"));
        }
    }

    private void AddSwoonDropFootChain(HashSet<FreeControllerV3> keep, bool right)
    {
        if (keep == null)
            return;

        if (right)
        {
            keep.Add(GetTargetPersonControlByAliases("rFootControl", "rightFootControl", "rFoot", "rightFoot"));
            keep.Add(GetTargetPersonControlByAliases("rKneeControl", "rightKneeControl", "rKnee", "rightKnee"));
            keep.Add(GetTargetPersonControlByAliases("rThighControl", "rightThighControl", "rThigh", "rightThigh"));
        }
        else
        {
            keep.Add(GetTargetPersonControlByAliases("lFootControl", "leftFootControl", "lFoot", "leftFoot"));
            keep.Add(GetTargetPersonControlByAliases("lKneeControl", "leftKneeControl", "lKnee", "leftKnee"));
            keep.Add(GetTargetPersonControlByAliases("lThighControl", "leftThighControl", "lThigh", "leftThigh"));
        }
    }

    private void AddSwoonDropKneeChain(HashSet<FreeControllerV3> keep, bool right)
    {
        if (keep == null)
            return;

        if (right)
        {
            keep.Add(GetTargetPersonControlByAliases("rKneeControl", "rightKneeControl", "rKnee", "rightKnee"));
            keep.Add(GetTargetPersonControlByAliases("rFootControl", "rightFootControl", "rFoot", "rightFoot"));
            keep.Add(GetTargetPersonControlByAliases("rThighControl", "rightThighControl", "rThigh", "rightThigh"));
        }
        else
        {
            keep.Add(GetTargetPersonControlByAliases("lKneeControl", "leftKneeControl", "lKnee", "leftKnee"));
            keep.Add(GetTargetPersonControlByAliases("lFootControl", "leftFootControl", "lFoot", "leftFoot"));
            keep.Add(GetTargetPersonControlByAliases("lThighControl", "leftThighControl", "lThigh", "leftThigh"));
        }
    }

    private void AddSwoonDropHipHoldKeepControls(HashSet<FreeControllerV3> keep)
    {
        if (keep == null)
            return;

        keep.Add(GetTargetPersonControlByAliases("hipControl", "hip"));
        keep.Add(GetTargetPersonControlByAliases("lThighControl", "leftThighControl", "lThigh", "leftThigh"));
        keep.Add(GetTargetPersonControlByAliases("rThighControl", "rightThighControl", "rThigh", "rightThigh"));
        keep.Add(GetTargetPersonControlByAliases("lKneeControl", "leftKneeControl", "lKnee", "leftKnee"));
        keep.Add(GetTargetPersonControlByAliases("rKneeControl", "rightKneeControl", "rKnee", "rightKnee"));
    }

    private void AddSwoonDropUpperBodyKeepControls(HashSet<FreeControllerV3> keep, bool includeHead)
    {
        if (keep == null)
            return;

        keep.Add(GetTargetPersonControlByAliases("hipControl", "hip"));
        keep.Add(GetTargetPersonControlByAliases("abdomenControl", "abdomen"));
        keep.Add(GetTargetPersonControlByAliases("chestControl", "chest"));
        if (includeHead)
            keep.Add(GetTargetPersonControlByAliases("headControl", "head"));
    }

    private void ApplyTargetSwoonHugBodyOneHandTwist(HashSet<FreeControllerV3> keepControls)
    {
        if (!hasHeldTargetGrab || heldTargetGrabChoice != TC_HUG_BODY || !heldTargetGrabIncludeHands)
            return;

        bool leftOnly = heldTargetGrabLeftHand && !heldTargetGrabRightHand;
        bool rightOnly = heldTargetGrabRightHand && !heldTargetGrabLeftHand;
        if (!leftOnly && !rightOnly)
            return;

        FreeControllerV3 hand = leftOnly ? lHandControl : rHandControl;
        FreeControllerV3 chest = GetTargetPersonControlByAliases("chestControl", "chest");
        if (hand == null || chest == null)
            return;

        Vector3 dir = GetControlPosition(hand) - GetControlPosition(chest);
        dir.y = 0.0f;
        if (dir.sqrMagnitude < 0.0001f)
            return;

        Vector3 offset = dir.normalized * TARGET_SWOON_HUG_BODY_ONE_HAND_TWIST_OFFSET;
        List<FreeControllerV3> controls = new List<FreeControllerV3>();
        AddControlIfNotNull(controls, chest);
        ApplyUpperBodyPivotOffset(controls, offset, leftOnly ? "TargetSwoon-L" : "TargetSwoon-R");

        if (keepControls != null)
            keepControls.Add(chest);

        DebugLog("[TARGET SWOON HUG TWIST] hand=" + (leftOnly ? "L" : "R") +
            " offset=" + FormatVector3(offset) +
            " hand=" + FormatVector3(GetControlPosition(hand)) +
            " chest=" + FormatVector3(GetControlPosition(chest)));
    }

    private void AddUniqueControl(List<FreeControllerV3> controls, FreeControllerV3 fc)
    {
        if (controls == null || fc == null || controls.Contains(fc))
            return;

        controls.Add(fc);
    }

    private void UpdateReleaseButtonColors()
    {
        SetButtonWarningColor(releaseTargetButton, releaseTargetDefaultColors, releaseTargetColorsCaptured, HasTargetReleaseState(), new Color(1.00f, 0.62f, 0.20f, 1.0f));
        SetButtonWarningColor(releaseButton, releaseDefaultColors, releaseColorsCaptured, HasSelfReleaseState(), new Color(0.32f, 0.70f, 1.00f, 1.0f));
        SetButtonWarningColor(swoonDropButton, swoonDropDefaultColors, swoonDropColorsCaptured, swoonDropActive, new Color(0.72f, 0.42f, 1.00f, 1.0f));
        SetButtonText(swoonDropButton, swoonDropActive ? "Target Swoon Stop" : "Target Swoon Drop");
    }

    private void CaptureButtonDefaultColors(UIDynamicButton dynamicButton, ref ColorBlock colors, ref bool captured)
    {
        if (captured || dynamicButton == null || dynamicButton.button == null)
        {
            return;
        }

        colors = dynamicButton.button.colors;
        captured = true;
    }

    private void SetButtonWarningColor(UIDynamicButton dynamicButton, ColorBlock defaultColors, bool hasDefault, bool active, Color normalColor)
    {
        if (dynamicButton == null || dynamicButton.button == null || !hasDefault)
        {
            return;
        }

        ColorBlock colors = defaultColors;
        if (active)
        {
            colors.normalColor = normalColor;
            colors.highlightedColor = Color.Lerp(normalColor, Color.white, 0.18f);
            colors.pressedColor = Color.Lerp(normalColor, Color.black, 0.18f);
            colors.disabledColor = new Color(normalColor.r, normalColor.g, normalColor.b, 0.45f);
        }

        dynamicButton.button.colors = colors;
    }

    private void SetButtonText(UIDynamicButton dynamicButton, string text)
    {
        if (dynamicButton == null || dynamicButton.button == null || string.IsNullOrEmpty(text))
            return;

        Text label = dynamicButton.button.GetComponentInChildren<Text>();
        if (label != null)
            label.text = text;
    }

    private void QueueAutoSnapPullOpenIK(List<FreeControllerV3> targetControls)
    {
        pendingAutoSnapIKControls.Clear();

        if (autoSnapPullOpenIKJSON == null || !autoSnapPullOpenIKJSON.val)
            return;

        if (selectedPerson != null)
        {
            if (leftHandJSON != null && leftHandJSON.val)
                AddPendingAutoSnapIK(selectedPerson, lHandControl);
            if (rightHandJSON != null && rightHandJSON.val)
                AddPendingAutoSnapIK(selectedPerson, rHandControl);
        }

        if (selectedTargetPerson != null && targetControls != null)
        {
            foreach (FreeControllerV3 fc in targetControls)
            {
                if (ShouldSkipTargetAutoSnapIK(fc))
                {
                    DebugLog("[TARGET SNAP SKIP] reason=head-neck-protect target=" + (fc != null ? fc.name : "<null>"));
                    continue;
                }

                AddPendingAutoSnapIK(selectedTargetPerson, fc);
            }
        }

        ReapplyHeldTargetHandFollowLocks("queue-auto-snap");
    }

    private void AddPendingAutoSnapIK(Atom atom, FreeControllerV3 fc)
    {
        if (atom == null || fc == null)
            return;

        pendingAutoSnapIKControls[fc] = atom;
    }

    private void ExecutePendingAutoSnapPullOpenIK()
    {
        if (pendingAutoSnapIKControls.Count == 0)
            return;

        List<KeyValuePair<FreeControllerV3, Atom>> controls = pendingAutoSnapIKControls.ToList();
        pendingAutoSnapIKControls.Clear();

        int snapped = 0;
        foreach (KeyValuePair<FreeControllerV3, Atom> item in controls)
        {
            if (SnapIKControlToBody(item.Value, item.Key))
                snapped++;
        }

        if (snapped > 0)
            SetStatus("Auto Snap Pull/Open IK / snapped=" + snapped.ToString(CultureInfo.InvariantCulture));
    }

    private bool SnapIKControlToBody(Atom atom, FreeControllerV3 fc)
    {
        if (atom == null || fc == null || string.IsNullOrEmpty(fc.name))
            return false;

        if (atom == selectedTargetPerson && ShouldSkipTargetAutoSnapIK(fc))
        {
            DebugLog("[TARGET SNAP SKIP] reason=snap-guard target=" + fc.name);
            return false;
        }

        string targetKeyword = fc.name.Replace("Control", "").Replace("control", "").ToLowerInvariant();
        if (string.IsNullOrEmpty(targetKeyword))
            return false;

        Transform[] transforms = atom.GetComponentsInChildren<Transform>(false);
        Transform best = null;
        float minDist = float.MaxValue;
        Vector3 current = GetControlPosition(fc);

        foreach (Transform t in transforms)
        {
            if (t == null || string.IsNullOrEmpty(t.name))
                continue;

            string name = t.name.ToLowerInvariant();
            if (name.Contains("control"))
                continue;
            if (!name.Contains(targetKeyword))
                continue;

            float d = Vector3.Distance(current, t.position);
            if (d < minDist)
            {
                minDist = d;
                best = t;
            }
        }

        if (best == null)
            return false;

        bool hugBodySelfHandPositionOnly = IsHugBodyTarget() && atom == selectedPerson && IsSelfHandControl(fc);

        Vector3 snapPosition = best.position;
        bool snapClamped = false;
        float rawSnapDistance = 0.0f;
        Vector3 snapAnchor = current;

        // V5h:
        // For Hug Body self hands, copying the real body hand position 100% can pull the IK
        // away from the chest-side final route when the body hand is still on the far/deep side.
        // Keep the final Wrist In rotation, but clamp the position snap from the route final
        // toward the real body hand. This preserves chest approach while still doing a small
        // VaM body-hand resync.
        if (hugBodySelfHandPositionOnly)
        {
            if (!hugBodyHandSnapAnchorPositions.TryGetValue(fc, out snapAnchor))
                snapAnchor = current;

            rawSnapDistance = Vector3.Distance(snapAnchor, best.position);
            if (rawSnapDistance > HUG_BODY_IK_SNAP_MAX_OFFSET && rawSnapDistance > 0.0001f)
            {
                snapPosition = snapAnchor + (best.position - snapAnchor).normalized * HUG_BODY_IK_SNAP_MAX_OFFSET;
                snapClamped = true;
            }
        }

        fc.transform.position = snapPosition;
        if (!hugBodySelfHandPositionOnly)
            fc.transform.rotation = best.rotation;

        if (fc.control != null)
        {
            fc.control.position = snapPosition;
            if (!hugBodySelfHandPositionOnly)
                fc.control.rotation = best.rotation;
        }

        if (hugBodySelfHandPositionOnly)
        {
            string snapMsg = "[HAND IK SNAP POS CLAMP] hand=" + (fc == rHandControl ? "R" : "L") +
                " reason=hug-body-keep-final-wrist-in" +
                " clamp=" + Bool01(snapClamped) +
                " max=" + HUG_BODY_IK_SNAP_MAX_OFFSET.ToString("F3", CultureInfo.InvariantCulture) +
                " rawDist=" + rawSnapDistance.ToString("F3", CultureInfo.InvariantCulture) +
                " anchor=" + FormatVector3(snapAnchor) +
                " body=" + FormatVector3(best.position) +
                " applied=" + FormatVector3(snapPosition) +
                " anchorToApplied=" + Vector3.Distance(snapAnchor, snapPosition).ToString("F3", CultureInfo.InvariantCulture);

            DebugLog(snapMsg);

            if (IsDebugEnabled())
            {
                Vector3 e = GetControlRotation(fc).eulerAngles;
                DebugLog(snapMsg +
                    " keepEuler=(" + e.x.ToString("F1", CultureInfo.InvariantCulture) + "," +
                        e.y.ToString("F1", CultureInfo.InvariantCulture) + "," +
                        e.z.ToString("F1", CultureInfo.InvariantCulture) + ")");
            }
        }

        return true;
    }

    private bool IsSelfHandControl(FreeControllerV3 fc)
    {
        return fc != null && (fc == lHandControl || fc == rHandControl);
    }

    private int PrepareTargetNoneBodyMoveRelaxIK(List<FreeControllerV3> movingControls, string label)
    {
        if (selectedTargetPerson == null)
            return 0;

        if (targetNoneBodyRelaxTargetAtom != null && targetNoneBodyRelaxTargetAtom != selectedTargetPerson)
            RestoreTargetNoneBodyRelaxIK();

        targetNoneBodyRelaxTargetAtom = selectedTargetPerson;

        List<FreeControllerV3> relaxControls = GetTargetNoneBodyMoveRelaxControls(movingControls);
        int changed = 0;
        foreach (FreeControllerV3 fc in relaxControls)
        {
            if (RelaxTargetNoneBodyMoveIKToComply(fc))
                changed++;
        }

        if (changed > 0)
            DebugLog("[TARGET NONE BODY RELAX] label=" + label +
                " newlyTracked=" + changed.ToString(CultureInfo.InvariantCulture) +
                " comply=" + relaxControls.Count.ToString(CultureInfo.InvariantCulture));

        return relaxControls.Count;
    }

    private List<FreeControllerV3> GetTargetNoneBodyMoveRelaxControls(List<FreeControllerV3> movingControls)
    {
        List<FreeControllerV3> controls = new List<FreeControllerV3>();
        if (selectedTargetPerson == null)
            return controls;

        AddControlIfNotMoving(controls, movingControls, GetTargetPersonControlByAliases("lHandControl", "leftHandControl", "lHand", "leftHand"));
        AddControlIfNotMoving(controls, movingControls, GetTargetPersonControlByAliases("rHandControl", "rightHandControl", "rHand", "rightHand"));
        AddControlIfNotMoving(controls, movingControls, GetTargetPersonControlByAliases("lElbowControl", "leftElbowControl", "lElbow", "leftElbow"));
        AddControlIfNotMoving(controls, movingControls, GetTargetPersonControlByAliases("rElbowControl", "rightElbowControl", "rElbow", "rightElbow"));
        AddControlIfNotMoving(controls, movingControls, GetTargetPersonControlByAliases("lFootControl", "leftFootControl", "lFoot", "leftFoot"));
        AddControlIfNotMoving(controls, movingControls, GetTargetPersonControlByAliases("rFootControl", "rightFootControl", "rFoot", "rightFoot"));
        AddControlIfNotMoving(controls, movingControls, GetTargetPersonControlByAliases("lKneeControl", "leftKneeControl", "lKnee", "leftKnee"));
        AddControlIfNotMoving(controls, movingControls, GetTargetPersonControlByAliases("rKneeControl", "rightKneeControl", "rKnee", "rightKnee"));
        AddControlIfNotMoving(controls, movingControls, GetTargetPersonControlByAliases("lThighControl", "leftThighControl", "lThigh", "leftThigh"));
        AddControlIfNotMoving(controls, movingControls, GetTargetPersonControlByAliases("rThighControl", "rightThighControl", "rThigh", "rightThigh"));

        return controls;
    }

    private void AddControlIfNotMoving(List<FreeControllerV3> controls, List<FreeControllerV3> movingControls, FreeControllerV3 fc)
    {
        if (fc == null)
            return;
        if (movingControls != null && movingControls.Contains(fc))
            return;
        AddControlIfNotNull(controls, fc);
    }

    private bool RelaxTargetNoneBodyMoveIKToComply(FreeControllerV3 fc)
    {
        if (fc == null)
            return false;

        bool newlyTracked = false;
        if (!targetNoneBodyRelaxControls.Contains(fc))
        {
            targetNoneBodyRelaxControls.Add(fc);
            newlyTracked = true;
        }

        if (!targetNoneBodyRelaxPositionStates.ContainsKey(fc))
            targetNoneBodyRelaxPositionStates[fc] = fc.currentPositionState;
        if (!targetNoneBodyRelaxRotationStates.ContainsKey(fc))
            targetNoneBodyRelaxRotationStates[fc] = fc.currentRotationState;

        try
        {
            fc.currentPositionState = FreeControllerV3.PositionState.Comply;
        }
        catch { }

        try
        {
            fc.currentRotationState = FreeControllerV3.RotationState.Comply;
        }
        catch { }

        return newlyTracked;
    }

    private int RestoreTargetNoneBodyRelaxIK()
    {
        if (targetNoneBodyRelaxControls.Count == 0)
        {
            targetNoneBodyRelaxPositionStates.Clear();
            targetNoneBodyRelaxRotationStates.Clear();
            targetNoneBodyRelaxTargetAtom = null;
            return 0;
        }

        int restored = 0;
        foreach (FreeControllerV3 fc in targetNoneBodyRelaxControls)
        {
            if (fc == null)
                continue;

            FreeControllerV3.PositionState positionState;
            if (targetNoneBodyRelaxPositionStates.TryGetValue(fc, out positionState))
            {
                try
                {
                    fc.currentPositionState = positionState;
                }
                catch { }
            }

            FreeControllerV3.RotationState rotationState;
            if (targetNoneBodyRelaxRotationStates.TryGetValue(fc, out rotationState))
            {
                try
                {
                    fc.currentRotationState = rotationState;
                }
                catch { }
            }

            restored++;
        }

        targetNoneBodyRelaxPositionStates.Clear();
        targetNoneBodyRelaxRotationStates.Clear();
        targetNoneBodyRelaxControls.Clear();
        targetNoneBodyRelaxTargetAtom = null;
        return restored;
    }

    private void PrepareTemporaryRelaxLinkedIK(List<FreeControllerV3> targetControls)
    {
        RestoreTemporaryRelaxLinkedIK();

        if (selectedTargetPerson == null || targetControls == null)
            return;

        foreach (FreeControllerV3 target in targetControls)
        {
            FreeControllerV3 linked = GetLinkedTemporaryRelaxIKControl(target);
            if (linked != null)
                RelaxTemporaryLinkedIK(linked);
        }
    }

    private FreeControllerV3 GetLinkedTemporaryRelaxIKControl(FreeControllerV3 target)
    {
        if (target == null || string.IsNullOrEmpty(target.name) || selectedTargetPerson == null)
            return null;

        string name = target.name.ToLowerInvariant();

        if (name.Contains("lhand") || name.Contains("lefthand"))
            return GetTargetPersonControlByAliases("lElbowControl", "leftElbowControl");
        if (name.Contains("rhand") || name.Contains("righthand"))
            return GetTargetPersonControlByAliases("rElbowControl", "rightElbowControl");
        if (name.Contains("lfoot") || name.Contains("leftfoot"))
            return GetTargetPersonControlByAliases("lKneeControl", "leftKneeControl");
        if (name.Contains("rfoot") || name.Contains("rightfoot"))
            return GetTargetPersonControlByAliases("rKneeControl", "rightKneeControl");

        return null;
    }

    private FreeControllerV3 GetTargetPersonControlByAliases(params string[] names)
    {
        if (selectedTargetPerson == null || names == null)
            return null;

        foreach (string name in names)
        {
            FreeControllerV3 fc = GetControlFromAtom(selectedTargetPerson, name);
            if (fc != null)
                return fc;
        }

        return null;
    }

    private void LockTargetHipHoldThighIK()
    {
        if (!IsTargetPersonMode() || selectedTargetPerson == null || !IsHipHoldMode())
            return;

        bool changed = false;
        changed |= LockTargetIKControl(GetTargetPersonControlByAliases("lThighControl", "leftThighControl", "lThigh", "leftThigh"));
        changed |= LockTargetIKControl(GetTargetPersonControlByAliases("rThighControl", "rightThighControl", "rThigh", "rightThigh"));

        if (changed)
            UpdateGrabHandUtilityButtons();
    }

    private bool LockTargetIKControl(FreeControllerV3 fc)
    {
        if (fc == null)
            return false;

        bool changed = false;

        if (!targetLockControls.Contains(fc))
            targetLockControls.Add(fc);

        if (!targetLockPositionStates.ContainsKey(fc))
        {
            targetLockPositionStates[fc] = fc.currentPositionState;
            changed = true;
        }

        if (!targetLockRotationStates.ContainsKey(fc))
        {
            targetLockRotationStates[fc] = fc.currentRotationState;
            changed = true;
        }

        try
        {
            fc.currentPositionState = FreeControllerV3.PositionState.On;
        }
        catch { }

        try
        {
            fc.currentRotationState = FreeControllerV3.RotationState.On;
        }
        catch { }

        return changed;
    }

    private int RestoreTargetLocks()
    {
        int restored = 0;
        for (int i = 0; i < targetLockControls.Count; i++)
        {
            FreeControllerV3 fc = targetLockControls[i];
            if (fc == null)
                continue;

            FreeControllerV3.PositionState positionState;
            if (targetLockPositionStates.TryGetValue(fc, out positionState))
            {
                try
                {
                    fc.currentPositionState = positionState;
                }
                catch { }
            }

            FreeControllerV3.RotationState rotationState;
            if (targetLockRotationStates.TryGetValue(fc, out rotationState))
            {
                try
                {
                    fc.currentRotationState = rotationState;
                }
                catch { }
            }

            restored++;
        }

        targetLockPositionStates.Clear();
        targetLockRotationStates.Clear();
        targetLockControls.Clear();
        return restored;
    }

    private void RelaxTemporaryLinkedIK(FreeControllerV3 fc)
    {
        if (fc == null)
            return;

        if (!temporaryRelaxControls.Contains(fc))
            temporaryRelaxControls.Add(fc);

        if (!temporaryRelaxPositionStates.ContainsKey(fc))
            temporaryRelaxPositionStates[fc] = fc.currentPositionState;
        if (!temporaryRelaxRotationStates.ContainsKey(fc))
            temporaryRelaxRotationStates[fc] = fc.currentRotationState;

        try
        {
            fc.currentPositionState = FreeControllerV3.PositionState.Off;
        }
        catch { }

        try
        {
            fc.currentRotationState = FreeControllerV3.RotationState.Off;
        }
        catch { }
    }

    private void RestoreTemporaryRelaxLinkedIK()
    {
        if (temporaryRelaxControls.Count == 0)
            return;

        foreach (FreeControllerV3 fc in temporaryRelaxControls)
        {
            if (fc == null)
                continue;

            FreeControllerV3.PositionState positionState;
            if (temporaryRelaxPositionStates.TryGetValue(fc, out positionState))
            {
                try
                {
                    fc.currentPositionState = positionState;
                }
                catch { }
            }

            FreeControllerV3.RotationState rotationState;
            if (temporaryRelaxRotationStates.TryGetValue(fc, out rotationState))
            {
                try
                {
                    fc.currentRotationState = rotationState;
                }
                catch { }
            }
        }

        temporaryRelaxPositionStates.Clear();
        temporaryRelaxRotationStates.Clear();
        temporaryRelaxControls.Clear();
    }

    private class SelfFollowParentLink
    {
        public FreeControllerV3 target;
        public FreeControllerV3 parent;
        public Vector3 localPosition;
        public Quaternion localRotation;
    }

    private class SelfFollowLinkState
    {
        public FreeControllerV3.PositionState positionState;
        public FreeControllerV3.RotationState rotationState;
    }

    private void QueueSelfFollowParentTargets(List<FreeControllerV3> targetControls)
    {
        pendingSelfFollowTargets.Clear();

        if (!IsFollowSelfMode() || targetControls == null || selectedPerson == null)
            return;

        foreach (FreeControllerV3 fc in targetControls)
        {
            if (fc != null && !pendingSelfFollowTargets.Contains(fc))
                pendingSelfFollowTargets.Add(fc);
        }
    }

    private List<FreeControllerV3> GetSelfFollowTargetControls()
    {
        List<FreeControllerV3> controls = new List<FreeControllerV3>();

        if (!IsTargetPersonMode() || selectedTargetPerson == null)
            return controls;

        FreeControllerV3 left;
        FreeControllerV3 right;
        if (TryGetGrabHandOpenTargetControls(out left, out right))
        {
            AddControlIfNotNull(controls, left);
            AddControlIfNotNull(controls, right);
            return controls;
        }

        bool rightSide;
        FreeControllerV3 single;
        if (TryGetGrabHandOpenSingleTargetControl(out single, out rightSide))
        {
            AddControlIfNotNull(controls, single);
            return controls;
        }

        AddControlIfNotNull(controls, GetTargetPersonPartControl());
        return controls;
    }

    private void ApplyPendingSelfFollowParentLinks()
    {
        if (pendingSelfFollowTargets.Count == 0)
            return;

        List<FreeControllerV3> targets = new List<FreeControllerV3>(pendingSelfFollowTargets);
        pendingSelfFollowTargets.Clear();

        foreach (FreeControllerV3 target in targets)
        {
            FreeControllerV3 parent = GetNearestActiveSelfFollowHand(target);
            if (target != null && parent != null)
                ApplySelfFollowParentLink(target, parent);
        }
    }

    private FreeControllerV3 GetNearestActiveSelfFollowHand(FreeControllerV3 target)
    {
        if (target == null)
            return null;

        FreeControllerV3 best = null;
        float bestDist = float.MaxValue;
        Vector3 targetPos = GetControlPosition(target);

        AddNearestSelfFollowHandCandidate(lHandControl, leftHandJSON != null && leftHandJSON.val, targetPos, ref best, ref bestDist);
        AddNearestSelfFollowHandCandidate(rHandControl, rightHandJSON != null && rightHandJSON.val, targetPos, ref best, ref bestDist);

        return best;
    }

    private void AddNearestSelfFollowHandCandidate(FreeControllerV3 hand, bool enabled, Vector3 targetPos, ref FreeControllerV3 best, ref float bestDist)
    {
        if (!enabled || hand == null)
            return;

        float dist = (GetControlPosition(hand) - targetPos).sqrMagnitude;
        if (dist < bestDist)
        {
            bestDist = dist;
            best = hand;
        }
    }

    private void ApplySelfFollowParentLink(FreeControllerV3 target, FreeControllerV3 parent)
    {
        if (target == null || parent == null)
            return;

        CaptureSelfFollowLinkState(target);
        try
        {
            target.currentPositionState = FreeControllerV3.PositionState.On;
        }
        catch { }

        SelfFollowParentLink link = new SelfFollowParentLink();
        link.target = target;
        link.parent = parent;
        Quaternion parentRot = GetControlRotation(parent);
        Vector3 parentPos = GetControlPosition(parent);
        link.localPosition = Quaternion.Inverse(parentRot) * (GetControlPosition(target) - parentPos);
        link.localRotation = Quaternion.Inverse(parentRot) * GetControlRotation(target);

        activeSelfFollowParentLinks.Add(link);
    }

    private void UpdateSelfFollowParentLinks()
    {
        if (activeSelfFollowParentLinks.Count == 0)
            return;

        foreach (SelfFollowParentLink link in activeSelfFollowParentLinks)
        {
            if (link == null || link.target == null || link.parent == null)
                continue;

            Quaternion parentRot = GetControlRotation(link.parent);
            Vector3 parentPos = GetControlPosition(link.parent);
            Vector3 nextPos = parentPos + parentRot * link.localPosition;
            Quaternion nextRot = parentRot * link.localRotation;

            link.target.transform.position = nextPos;
            link.target.transform.rotation = nextRot;
            if (link.target.control != null)
            {
                link.target.control.position = nextPos;
                link.target.control.rotation = nextRot;
            }
        }
    }

    private void CaptureSelfFollowLinkState(FreeControllerV3 target)
    {
        if (target == null || selfFollowOriginalLinkStates.ContainsKey(target))
            return;

        SelfFollowLinkState state = new SelfFollowLinkState();
        state.positionState = target.currentPositionState;
        state.rotationState = target.currentRotationState;
        selfFollowOriginalLinkStates[target] = state;
    }

    private void RestoreSelfFollowParentLinks()
    {
        pendingSelfFollowTargets.Clear();
        activeSelfFollowParentLinks.Clear();

        if (selfFollowOriginalLinkStates.Count == 0)
            return;

        List<FreeControllerV3> controls = selfFollowOriginalLinkStates.Keys.ToList();
        foreach (FreeControllerV3 target in controls)
        {
            if (target == null)
                continue;

            SelfFollowLinkState state;
            if (!selfFollowOriginalLinkStates.TryGetValue(target, out state) || state == null)
                continue;

            try
            {
                target.currentPositionState = state.positionState;
            }
            catch { }

            try
            {
                target.currentRotationState = state.rotationState;
            }
            catch { }
        }

        selfFollowOriginalLinkStates.Clear();
    }

    private void GrabFoot()
    {
        StartTimedGrab(false, true);
    }

    private void GrabLeftHand()
    {
        SetHandSelection(true, false);
        StartTimedGrab(true, false);
    }

    private void GrabRightHand()
    {
        SetHandSelection(false, true);
        StartTimedGrab(true, false);
    }

    private void GrabLeftFoot()
    {
        SetFootSelection(true, false);
        StartTimedGrab(false, true);
    }

    private void GrabRightFoot()
    {
        SetFootSelection(false, true);
        StartTimedGrab(false, true);
    }

    private void Pufupufu()
    {
        // v3.0aj:
        // pufupufu/control系は、Person切替直後でも手Control参照を取り直してから実行する。
        ResolveControls();

        Vector3 leftTarget;
        Vector3 rightTarget;
        string mode;

        if (!TryGetAssignedNippleTargets(out leftTarget, out rightTarget, out mode))
        {
            SetStatus("pufupufu needs control nipple target");
            return;
        }

        pufupufuLeftBase = GetControlPosition(lHandControl);
        pufupufuRightBase = GetControlPosition(rHandControl);
        pufupufuLeftAxis = GetNipplePairOutwardAxis(false);
        pufupufuRightAxis = GetNipplePairOutwardAxis(true);
        pufupufuElapsed = 0.0f;
        pufupufuActive = true;
        SetStatus("pufupufu");
    }

    private void Job()
    {
        // jobはGrabを呼ばず、現在の手位置を基準にY方向だけ動かす。
        ResolveControls();

        jobLeftBase = GetControlPosition(lHandControl);
        jobRightBase = GetControlPosition(rHandControl);
        jobElapsed = 0.0f;
        jobActive = true;
        SetStatus("job");
    }

    private void SetHandSelection(bool left, bool right)
    {
        suppressApply = true;
        try
        {
            if (leftHandJSON != null) leftHandJSON.val = left;
            if (rightHandJSON != null) rightHandJSON.val = right;
        }
        finally
        {
            suppressApply = false;
        }
    }

    private void SetFootSelection(bool left, bool right)
    {
        suppressApply = true;
        try
        {
            if (leftFootJSON != null) leftFootJSON.val = left;
            if (rightFootJSON != null) rightFootJSON.val = right;
        }
        finally
        {
            suppressApply = false;
        }
    }

    private void GrabSelected()
    {
        StartTimedGrab(true, true);
    }


    private HashSet<FreeControllerV3> BuildChestHoldBackNearWristResetSkipControls(bool includeHands, bool useFinalGrabWidth)
    {
        HashSet<FreeControllerV3> result = new HashSet<FreeControllerV3>();

        // v95: Only normal Chest Hold Grab Hand should suppress the initial wrist reset/off cycle.
        // Utility moves use useFinalGrabWidth=true and must keep their existing behavior.
        if (!includeHands || useFinalGrabWidth || !IsChestHoldTarget())
            return result;

        Vector3 targetLeftNipple;
        Vector3 targetRightNipple;
        if (!TryGetTargetNipplePositions(out targetLeftNipple, out targetRightNipple))
            return result;

        Vector3 leftSideTarget;
        Vector3 rightSideTarget;
        string mode;
        if (!TryGetAssignedNippleTargets(out leftSideTarget, out rightSideTarget, out mode))
            return result;

        if (mode != "back")
            return result;

        Vector3 zOffset = GetNipplePairZOffsetVector();
        targetLeftNipple += zOffset;
        targetRightNipple += zOffset;
        leftSideTarget += zOffset;
        rightSideTarget += zOffset;

        Vector3 pairCenter = (targetLeftNipple + targetRightNipple) * 0.5f;
        Vector3 sideAxis = GetChestHoldNippleSideAxis(targetLeftNipple, targetRightNipple, Vector3.right);
        if (sideAxis.sqrMagnitude < 0.0001f)
            sideAxis = GetTargetPersonRightAxis();
        sideAxis.y = 0.0f;
        if (sideAxis.sqrMagnitude < 0.0001f)
            sideAxis = Vector3.right;
        sideAxis.Normalize();

        if (leftHandJSON != null && leftHandJSON.val && lHandControl != null)
        {
            Vector3 leftFinal = GetChestHoldBackPassOffsetTarget(lHandControl, leftSideTarget, pairCenter, sideAxis, false);
            if (IsChestHoldBackHandNearFinalForWristResetSkip(lHandControl, leftSideTarget, leftFinal, sideAxis, false))
                result.Add(lHandControl);
        }

        if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
        {
            Vector3 rightFinal = GetChestHoldBackPassOffsetTarget(rHandControl, rightSideTarget, pairCenter, sideAxis, true);
            if (IsChestHoldBackHandNearFinalForWristResetSkip(rHandControl, rightSideTarget, rightFinal, sideAxis, true))
                result.Add(rHandControl);
        }

        return result;
    }

    private bool IsChestHoldBackHandNearFinalForWristResetSkip(FreeControllerV3 handControl, Vector3 nippleTarget, Vector3 finalTarget, Vector3 sideAxis, bool rightHand)
    {
        if (handControl == null)
            return false;

        Vector3 current = GetControlPosition(handControl);
        Vector3 delta = current - finalTarget;
        float distance = delta.magnitude;
        float vertical = Mathf.Abs(delta.y);
        float nippleDistance = Vector3.Distance(current, nippleTarget);

        Vector3 lateralAxis = sideAxis;
        lateralAxis.y = 0.0f;
        if (lateralAxis.sqrMagnitude < 0.0001f)
            lateralAxis = Vector3.right;
        lateralAxis.Normalize();
        if (!rightHand)
            lateralAxis = -lateralAxis;

        float lateral = Mathf.Abs(Vector3.Dot(delta, lateralAxis));

        bool near = distance <= CHEST_HOLD_BACK_NEAR_WRIST_SKIP_DISTANCE &&
            lateral <= CHEST_HOLD_BACK_NEAR_WRIST_SKIP_LATERAL &&
            vertical <= CHEST_HOLD_BACK_NEAR_WRIST_SKIP_VERTICAL &&
            nippleDistance <= CHEST_HOLD_BACK_NEAR_WRIST_SKIP_NIPPLE_DISTANCE;

        if (IsDebugEnabled())
        {
            DebugLog("[CHEST HOLD BACK NEAR WRIST CHECK] hand=" + (rightHand ? "R" : "L") +
                " / near=" + Bool01(near) +
                " / distance=" + distance.ToString("F3", CultureInfo.InvariantCulture) +
                " / lateral=" + lateral.ToString("F3", CultureInfo.InvariantCulture) +
                " / vertical=" + vertical.ToString("F3", CultureInfo.InvariantCulture) +
                " / nippleDistance=" + nippleDistance.ToString("F3", CultureInfo.InvariantCulture) +
                " / current=" + FormatVector3(current) +
                " / final=" + FormatVector3(finalTarget) +
                " / nipple=" + FormatVector3(nippleTarget));
        }

        return near;
    }

    private void StartTimedGrab(bool includeHands, bool includeFeet, bool keepTemporaryRelaxLinkedIK = false, bool includeHead = false, bool useFinalGrabWidth = false)
    {
        ResolveControls();
        StopSwoonDrop(true, "grab-start");
        ClearPendingWristHandLocks();
        ClearChestHoldNippleHandFollow("grab-start");
        BeginChestHoldFrontLeftGraspRun("grab-start");
        BeginChestHoldFrontRightGraspRun("grab-start");

        RestoreSelfFollowParentLinks();

        HashSet<FreeControllerV3> chestHoldBackNearWristSkipControls = BuildChestHoldBackNearWristResetSkipControls(includeHands, useFinalGrabWidth);

        if (!keepTemporaryRelaxLinkedIK)
            RestoreTemporaryRelaxLinkedIK();
        RestoreTargetNoneBodyRelaxIK();
        RestoreTemporaryHandRotationOffStates(chestHoldBackNearWristSkipControls);

        if (includeHead)
            RelaxKissChestIK();

        activeIncludeHands = includeHands;
        activeIncludeFeet = includeFeet;
        activeIncludeHead = includeHead;
        activeMoveTimeMultiplier = includeFeet && !includeHands ? 2.0f : 1.0f;
        CaptureHeldTargetGrabState(includeHands, includeFeet, includeHead);
        releaseRestoreIKPending = false;
        releaseRestorePositionControls.Clear();
        releaseRestoreRotationControls.Clear();
        pendingAutoSnapIKControls.Clear();
        pendingSelfFollowTargets.Clear();

        grabElapsed = 0.0f;
        if (useFinalGrabWidth)
        {
            grabStartWidth = GetFinalGrabWidth();
        }
        else
        {
            grabStartWidth = IsHipHoldMode()
                ? Mathf.Max(GetFinalGrabWidth(), HIP_HOLD_GRAB_WIDTH)
                : (grabWidthJSON != null ? Mathf.Max(GetFinalGrabWidth(), grabWidthJSON.val) : Mathf.Max(0.10f, GetFinalGrabWidth()));
        }
        currentGrabWidth = grabStartWidth;

        grabStartPositions.Clear();
        grabStartRotations.Clear();
        positionStateOnControls.Clear();
        rotationStateOnControls.Clear();
        chestHoldFinalLeftLogged = false;
        chestHoldFinalRightLogged = false;
        chestHoldMovePointsLeftLogged = false;
        chestHoldMovePointsRightLogged = false;
        chestHoldEssentialTwoLineLogged = false;
        chestHoldModeLogged = false;

        UpdateChestHoldNippleIKStabilizeOnGrabStart(includeHands, useFinalGrabWidth);

        ApplyTemporaryHandRotationOffIfNeeded(includeHands, chestHoldBackNearWristSkipControls);

        CaptureControlStart(lHandControl);
        CaptureControlStart(rHandControl);
        CaptureControlStart(lFootControl);
        CaptureControlStart(rFootControl);
        CaptureControlStart(lKneeControl);
        CaptureControlStart(rKneeControl);
        CaptureControlStart(headControl);

        hasActiveGrab = true;

        string controllerDebug = GetTargetControllerNameForDebug();
        string controllerActual = GetTargetControllerActualName(targetPersonPartChooser != null ? targetPersonPartChooser.val : NONE);
        DebugLog("[GRAB START] targetType=" + (targetTypeChooser != null ? targetTypeChooser.val : "<null>") +
            " controller=" + controllerDebug +
            " key=" + NormalizeControllerKey(controllerActual) +
            " nipple=" + Bool01(IsNipplePairControlName(controllerActual)));

        LogChestHoldHugBodyRouteAlways("grab-start", includeHands, includeFeet, includeHead, useFinalGrabWidth, GetTargetCenter(), IsChestHoldTarget(), IsHipHoldMode(), IsTargetPairMode(), true);

        targetPelvisAutoOnGrabAppliedThisGrab = false;

        ApplyGrab(false, activeIncludeHands, activeIncludeFeet, activeIncludeHead);

        if (IsFollowSelfMode() && activeIncludeHands)
            QueueSelfFollowParentTargets(GetSelfFollowTargetControls());
    }

    private void CaptureControlStart(FreeControllerV3 fc)
    {
        if (fc == null)
            return;

        Vector3 pos = fc.control != null ? fc.control.position : fc.transform.position;
        Quaternion rot = fc.control != null ? fc.control.rotation : fc.transform.rotation;

        grabStartPositions[fc] = pos;
        grabStartRotations[fc] = rot;
    }

    private Vector3 GetControlPosition(FreeControllerV3 fc)
    {
        if (fc == null)
            return Vector3.zero;

        return fc.control != null ? fc.control.position : fc.transform.position;
    }

    private Quaternion GetControlRotation(FreeControllerV3 fc)
    {
        if (fc == null)
            return Quaternion.identity;

        return fc.control != null ? fc.control.rotation : fc.transform.rotation;
    }

    private void SetControlPositionDirect(FreeControllerV3 fc, Vector3 pos)
    {
        if (fc == null)
            return;

        EnsurePositionStateOn(fc);
        fc.transform.position = pos;
        if (fc.control != null)
            fc.control.position = pos;
    }

    private bool UpdatePufupufuAnimation()
    {
        if (!pufupufuActive)
            return false;

        pufupufuElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(pufupufuElapsed / PUFUPUFU_DURATION);
        float wave = Mathf.Sin(t * Mathf.PI * 6.0f);

        if (lHandControl != null && leftHandJSON != null && leftHandJSON.val)
            SetControlPositionDirect(lHandControl, pufupufuLeftBase + pufupufuLeftAxis * PUFUPUFU_AMPLITUDE * wave);

        if (rHandControl != null && rightHandJSON != null && rightHandJSON.val)
            SetControlPositionDirect(rHandControl, pufupufuRightBase + pufupufuRightAxis * PUFUPUFU_AMPLITUDE * wave);

        if (t >= 1.0f)
        {
            pufupufuActive = false;
            SetControlPositionDirect(lHandControl, pufupufuLeftBase);
            SetControlPositionDirect(rHandControl, pufupufuRightBase);
        }

        return true;
    }

    private bool UpdateJobAnimation()
    {
        if (!jobActive)
            return false;

        jobElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(jobElapsed / JOB_DURATION);
        float wave = Mathf.Sin(t * Mathf.PI * 2.0f * JOB_HAND_Y_CYCLES);
        Vector3 offset = Vector3.up * (JOB_HAND_Y_AMPLITUDE * wave);

        if (lHandControl != null && leftHandJSON != null && leftHandJSON.val)
            SetControlPositionDirect(lHandControl, jobLeftBase + offset);

        if (rHandControl != null && rightHandJSON != null && rightHandJSON.val)
            SetControlPositionDirect(rHandControl, jobRightBase + offset);

        if (t >= 1.0f)
        {
            jobActive = false;
            RestoreJobHandPositions();
        }

        return true;
    }

    private void RestoreJobHandPositions()
    {
        if (lHandControl != null && leftHandJSON != null && leftHandJSON.val)
            SetControlPositionDirect(lHandControl, jobLeftBase);

        if (rHandControl != null && rightHandJSON != null && rightHandJSON.val)
            SetControlPositionDirect(rHandControl, jobRightBase);
    }


    private struct HandFinalPointRoute
    {
        public Vector3 handCenter;
        public Vector3 side;
        public Vector3 actorSpreadSide;
        public float pathWidth;
        public bool leftPathRightSide;
        public bool rightPathRightSide;
        public bool useActorMidpointRoute;
        public bool useHugBodyThreeStepRoute;
        public Vector3 depthAxis;
        public string mode;
    }

    private HandFinalPointRoute BuildHandFinalPointRoute(Vector3 targetCenter, Vector3 baseSide, bool swapSidePaths, bool log)
    {
        HandFinalPointRoute route = new HandFinalPointRoute();
        route.mode = "normal-final-point";
        // v4.0cn: final-point hand routes own the approach/depth logic.
        // Do not let old Hug Mode GetHugCenter() push the whole hand center forward again.
        route.handCenter = targetCenter;
        route.side = GetHandSideAxis(baseSide);
        route.actorSpreadSide = route.side;
        route.pathWidth = GetGrabWidth();
        route.leftPathRightSide = !swapSidePaths;
        route.rightPathRightSide = swapSidePaths;
        route.useActorMidpointRoute = false;
        route.useHugBodyThreeStepRoute = false;
        route.depthAxis = Vector3.zero;

        if (ShouldUseActorViewHandRoute())
        {
            Vector3 normalDepthAxis = GetFinalPointDepthAxis(targetCenter);
            Vector3 normalSideAxis = GetFinalPointSideAxis(normalDepthAxis, route.side);

            route.mode = "normal-final-point-view-axis";
            route.side = normalSideAxis;
            route.actorSpreadSide = normalSideAxis;
            route.leftPathRightSide = false;
            route.rightPathRightSide = true;
            route.useActorMidpointRoute = true;
        }

        if (IsHugBodyTarget())
        {
            Vector3 depthAxis = GetFinalPointDepthAxis(targetCenter);
            Vector3 sideAxis = GetHugBodyWrapSideAxis(depthAxis, route.side);
            bool backSide = IsTargetPersonMode() && selectedTargetPerson != null && !IsGrabberInFrontOfTargetPerson(targetCenter);

            // v27:
            // Hug Body is a wrap route, not a generic grab route.  The width itself can remain large,
            // but the width must be applied on the actor's left/right wrap axis, and the hand must move
            // in three explicit phases:
            //   step1: open L/R from the current depth,
            //   step2: move straight toward/past the chest while keeping that width,
            //   step3: close toward the chest-front final point.
            // Do not use the old approach-side/candidate layout to decide the spread direction.
            route.mode = backSide ? "hug-body-back-three-step-wrap" : "hug-body-front-three-step-wrap";
            route.handCenter = backSide ? targetCenter : targetCenter + depthAxis * GetHugBodyFinalPointDepthOffset();
            route.side = sideAxis;
            route.actorSpreadSide = sideAxis;
            route.pathWidth = Mathf.Min(GetGrabWidth(), HUG_BODY_HAND_WIDTH_CAP);
            route.useActorMidpointRoute = true;
            route.useHugBodyThreeStepRoute = true;
            route.depthAxis = depthAxis;

            // Actual hands stay on actual body sides: L=actor-left, R=actor-right.
            route.leftPathRightSide = false;
            route.rightPathRightSide = true;

            if (log)
            {
                DebugLog("[HUG BODY THREE STEP ROUTE]" +
                    " mode=" + route.mode +
                    " targetCenter=" + FormatVector3(targetCenter) +
                    " finalCenter=" + FormatVector3(route.handCenter) +
                    " depthAxis=" + FormatVector3(depthAxis) +
                    " wrapSide=" + FormatVector3(sideAxis) +
                    " pathWidth=" + route.pathWidth.ToString("F3", CultureInfo.InvariantCulture) +
                    " finalWidth=" + GetFinalGrabWidth().ToString("F3", CultureInfo.InvariantCulture) +
                    " backSide=" + Bool01(backSide));
            }
        }

        if (log && IsDebugEnabled())
        {
            Vector3 finalLeft = route.handCenter + GetSideOffset(route.leftPathRightSide, route.side, GetFinalGrabWidth());
            Vector3 finalRight = route.handCenter + GetSideOffset(route.rightPathRightSide, route.side, GetFinalGrabWidth());
            Vector3 midLeft = GetHandRouteMidPoint(route, route.leftPathRightSide, finalLeft);
            Vector3 midRight = GetHandRouteMidPoint(route, route.rightPathRightSide, finalRight);
            DebugLog("[HAND FINAL ROUTE] mode=" + route.mode +
                " targetCenter=" + FormatVector3(targetCenter) +
                " handCenter=" + FormatVector3(route.handCenter) +
                " side=" + FormatVector3(route.side) +
                " actorSpreadSide=" + FormatVector3(route.actorSpreadSide) +
                " depthAxis=" + FormatVector3(route.depthAxis) +
                " threeStep=" + Bool01(route.useHugBodyThreeStepRoute) +
                " pathWidth=" + route.pathWidth.ToString("F3", CultureInfo.InvariantCulture) +
                " finalWidth=" + GetFinalGrabWidth().ToString("F3", CultureInfo.InvariantCulture) +
                " hugMode=" + Bool01(IsHugMode()) +
                " hugDepthOffset=" + (IsHugBodyTarget() ? GetHugBodyFinalPointDepthOffset().ToString("F3", CultureInfo.InvariantCulture) : "0.000") +
                " leftPathRight=" + Bool01(route.leftPathRightSide) +
                " rightPathRight=" + Bool01(route.rightPathRightSide) +
                " finalL=" + FormatVector3(finalLeft) +
                " finalR=" + FormatVector3(finalRight) +
                " midL=" + FormatVector3(midLeft) +
                " midR=" + FormatVector3(midRight));
        }

        return route;
    }

    private float GetHugBodyFinalPointDepthOffset()
    {
        // v4.0cn:
        // Hug Mode used to push the moving hand center forward, and the final-point route also
        // placed the final center beyond the target.  That made Hug Mode overshoot.
        // Now Hug Depth only adjusts this final-depth offset lightly.
        float baseOffset = HUG_BODY_FINAL_POINT_DEPTH_OFFSET;
        if (!IsHugMode())
            return baseOffset;

        float hugDepth = hugDepthJSON != null ? Mathf.Max(0.0f, hugDepthJSON.val) : 0.0f;
        if (hugDepth <= 0.0f)
            return baseOffset;

        // Keep the effect intentionally small; this is an offset behind chest, not another travel path.
        return Mathf.Clamp(hugDepth * 0.18f, baseOffset, 0.22f);
    }

    private Vector3 GetFinalPointDepthAxis(Vector3 targetCenter)
    {
        Vector3 axis = targetCenter - GetHugOriginPosition(targetCenter);
        axis.y = 0.0f;

        if (axis.sqrMagnitude < 0.0001f && selectedPerson != null && selectedPerson.transform != null)
        {
            axis = selectedPerson.transform.forward;
            axis.y = 0.0f;
        }

        if (axis.sqrMagnitude < 0.0001f)
        {
            axis = GetSelectedPersonForwardAxis();
            axis.y = 0.0f;
        }

        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.forward;

        return axis.normalized;
    }

    private Vector3 GetFinalPointSideAxis(Vector3 depthAxis, Vector3 fallbackSide)
    {
        Vector3 side = Vector3.Cross(Vector3.up, depthAxis);
        side.y = 0.0f;

        if (side.sqrMagnitude < 0.0001f)
            side = fallbackSide;
        side.y = 0.0f;

        if (side.sqrMagnitude < 0.0001f && selectedPerson != null && selectedPerson.transform != null)
            side = selectedPerson.transform.right;
        side.y = 0.0f;

        if (side.sqrMagnitude < 0.0001f)
            side = Vector3.right;

        side.Normalize();

        // v4.0ci:
        // Spread/open direction must be independent of both target facing and self root facing.
        // The only intended frame is the actor-view frame built from self position -> target center:
        //   depthAxis = self -> target
        //   sideAxis  = Vector3.Cross(Vector3.up, depthAxis)
        // Therefore do not flip this axis using selectedPerson.transform.right.
        return side.normalized;
    }
    private Vector3 GetHugBodyWrapSideAxis(Vector3 depthAxis, Vector3 fallbackSide)
    {
        // v27:
        // Hug Body opens the actor's own hands left/right while the forward motion still follows
        // the actor-to-target chest direction.  Use actor-right projected onto the plane
        // perpendicular to that chest direction; this avoids world-X matching and keeps diagonal
        // standing positions valid, while preventing the old approach-side axis from sending an
        // elbow behind the actor.
        Vector3 depth = depthAxis;
        depth.y = 0.0f;
        if (depth.sqrMagnitude > 0.0001f)
            depth.Normalize();

        Vector3 actorRight = Vector3.zero;
        if (selectedPerson != null && selectedPerson.transform != null)
            actorRight = selectedPerson.transform.right;
        actorRight.y = 0.0f;

        Vector3 side = actorRight;
        if (depth.sqrMagnitude > 0.0001f && side.sqrMagnitude > 0.0001f)
            side = side - depth * Vector3.Dot(side, depth);

        side.y = 0.0f;
        if (side.sqrMagnitude < 0.0001f && depth.sqrMagnitude > 0.0001f)
            side = Vector3.Cross(Vector3.up, depth);

        side.y = 0.0f;
        if (side.sqrMagnitude < 0.0001f)
            side = fallbackSide;

        side.y = 0.0f;
        if (side.sqrMagnitude < 0.0001f)
            side = Vector3.right;

        side.Normalize();

        // +side must be the actor's right side because GetSideOffset(true)=+side and the
        // right hand uses pathRightSide=true.
        actorRight.y = 0.0f;
        if (actorRight.sqrMagnitude > 0.0001f && Vector3.Dot(side, actorRight.normalized) < 0.0f)
            side = -side;

        // v28:
        // In unusual placements, root.right can still be misleading compared with the visible hand
        // order.  Hug Body must open hands outward from their actual current body sides, so align
        // +side with the current R-hand side as the final guard.  This only chooses the wrap axis
        // sign; L/R assignment remains fixed as L=false, R=true in BuildHandFinalPointRoute().
        if (lHandControl != null && rHandControl != null)
        {
            Vector3 handRight = GetControlPosition(rHandControl) - GetControlPosition(lHandControl);
            handRight.y = 0.0f;
            if (handRight.sqrMagnitude > 0.0001f)
            {
                handRight.Normalize();
                if (Vector3.Dot(side, handRight) < 0.0f)
                    side = -side;
            }
        }

        return side.normalized;
    }


    private bool ShouldUseActorViewHandRoute()
    {
        if (!IsTargetPersonMode())
            return false;

        // These small/intimate routes keep their existing dedicated motion rules.
        if (IsPeniMode() || IsGenMode() || IsAnusMode() || IsGroinMode())
            return false;

        return true;
    }

    private bool IsSingleHandFootKneeTargetMode()
    {
        if (!IsTargetPersonMode() || targetPersonPartChooser == null)
            return false;

        string choice = targetPersonPartChooser.val;

        // v4.0co:
        // These are single-limb grab targets. Pull/Open/Push/Up/Down re-grabs should not
        // re-evaluate OUT/IN from depth for them; final wrist is always Wrist In.
        return choice == TC_L_HAND || choice == TC_R_HAND ||
               choice == TC_L_FOOT || choice == TC_R_FOOT ||
               choice == TC_L_KNEE || choice == TC_R_KNEE;
    }

    private Vector3 GetHandRouteFinalPoint(HandFinalPointRoute route, bool pathRightSide)
    {
        return route.handCenter + GetSideOffset(pathRightSide, route.side, GetFinalGrabWidth());
    }

    private Vector3 GetHandRouteMidPoint(HandFinalPointRoute route, bool pathRightSide, Vector3 finalPoint)
    {
        if (route.useHugBodyThreeStepRoute)
            return route.handCenter + GetSideOffset(pathRightSide, route.side, route.pathWidth);

        if (!route.useActorMidpointRoute)
            return route.handCenter + GetSideOffset(pathRightSide, route.side, route.pathWidth);

        // Actor-view route: expand from the already-decided final point using self-to-target view left/right.
        // Target facing and self root facing are ignored.
        // Left hand: finalPoint - viewRight * GrabWidth, Right hand: finalPoint + viewRight * GrabWidth.
        return finalPoint + GetSideOffset(pathRightSide, route.actorSpreadSide, route.pathWidth);
    }

    private void ApplyGrab(bool immediate)
    {
        if (hasActiveGrab)
            ApplyGrab(immediate, activeIncludeHands, activeIncludeFeet, activeIncludeHead);
        else
            ApplyGrab(immediate, true, true, false);
    }

    private void ApplyGrab(bool immediate, bool includeHands, bool includeFeet)
    {
        ApplyGrab(immediate, includeHands, includeFeet, false);
    }

    private void ApplyGrab(bool immediate, bool includeHands, bool includeFeet, bool includeHead)
    {
        if (selectedPerson == null)
        {
            SetStatus("No Person");
            return;
        }

        if (!HasValidTarget())
        {
            SetStatus(IsTargetPersonMode() ? "No Target Person" : "No Target Atom");
            return;
        }

        if (hasActiveGrab)
            grabElapsed += Time.deltaTime;

        AutoCloseGrabWidth();

        // v22: Chest Hold 専用処理を Hug Body へ漏らさない。
        // IsNipplePairMode() は古い互換で control 名を拾うため、移動分岐には使わない。
        bool nipplePairMode = IsChestHoldTarget();
        bool hipHoldMode = IsHipHoldMode();
        bool targetPairMode = IsTargetPairMode();

        Vector3 center = GetTargetCenter();
        Vector3 handCenter = (nipplePairMode || hipHoldMode || targetPairMode) ? center : GetHugCenter(center);
        Vector3 footCenter = (nipplePairMode || hipHoldMode || targetPairMode) ? center : GetHugCenter(ApplyFootPersonGrabOffset(center));
        Vector3 baseSide = GetTargetSideAxis();
        Vector3 handSide = GetHandSideAxis(baseSide);
        Vector3 footSide = GetFootSideAxis(baseSide);
        LogSideDebug(center, handSide, footSide);
        bool swapSidePaths = ShouldSwapSidePaths(center);
        LogHandRotationDebug(swapSidePaths, center);
        bool routeNormalLog = IsHugBodyTarget() || IsChestHoldTarget();
        bool logHandTargetsThisFrame = false;
        if ((IsDebugEnabled() || routeNormalLog) && Time.time - lastHandTargetDebugTime >= 0.50f)
        {
            logHandTargetsThisFrame = true;
            lastHandTargetDebugTime = Time.time;
        }

        LogChestHoldHugBodyRouteAlways("apply-grab", includeHands, includeFeet, includeHead, false, center, nipplePairMode, hipHoldMode, targetPairMode, false);

        int moved = 0;
        moved += ApplyHeadGrabIfNeeded(immediate, includeHead, center);

        if (nipplePairMode && (includeHands || includeFeet))
        {
            LogChestHoldHugBodyRouteAlways("branch-nipple-pair", includeHands, includeFeet, includeHead, false, center, nipplePairMode, hipHoldMode, targetPairMode, true);
            ApplyNipplePairGrab(immediate, includeHands, includeFeet, center, handCenter, footCenter, handSide);
            return;
        }

        if (hipHoldMode && (includeHands || includeFeet))
        {
            ApplyHipHoldGrab(immediate, includeHands, includeFeet, center, handCenter, footCenter, handSide);
            return;
        }

        if (targetPairMode && (includeHands || includeFeet))
        {
            ApplyTargetPairGrab(immediate, includeHands, includeFeet, center, handCenter, footCenter, handSide);
            return;
        }

        if (includeHands)
        {
            if (IsHugBodyTarget())
                LogChestHoldHugBodyRouteAlways("branch-hug-body-hand-route", includeHands, includeFeet, includeHead, false, center, nipplePairMode, hipHoldMode, targetPairMode, true);

            HandFinalPointRoute handRoute = BuildHandFinalPointRoute(center, baseSide, swapSidePaths, logHandTargetsThisFrame);
            handCenter = handRoute.handCenter;
            handSide = handRoute.side;
            float handPathWidth = handRoute.pathWidth;
            bool leftPathRightSideForHands = handRoute.leftPathRightSide;
            bool rightPathRightSideForHands = handRoute.rightPathRightSide;
            bool effectiveSwapSidePaths = handRoute.rightPathRightSide;

            if (leftHandJSON != null && leftHandJSON.val && lHandControl != null)
            {
                bool pathRightSide = leftPathRightSideForHands;
                Vector3 root = GetHandRootPosition(pathRightSide);
                Vector3 finalDesired = GetHandRouteFinalPoint(handRoute, pathRightSide);
                Vector3 midDesired = GetHandRouteMidPoint(handRoute, pathRightSide, finalDesired);
                Vector3 target = handRoute.useActorMidpointRoute
                    ? finalDesired
                    : GetReachLimitedPosition(root, finalDesired, GetMaxHandReach(), GetHandPalmOffset(), lHandControl, true, pathRightSide);
                Vector3 midTarget = handRoute.useActorMidpointRoute
                    ? midDesired
                    : GetReachLimitedPosition(root, midDesired, GetMaxHandReach(), GetHandPalmOffset(), lHandControl, true, pathRightSide);
                if (logHandTargetsThisFrame)
                    LogHandTargetDebug(false, lHandControl, root, midDesired, target, center, handCenter, handSide, handPathWidth, pathRightSide, effectiveSwapSidePaths, immediate);
                if (handRoute.useHugBodyThreeStepRoute)
                    MoveHugBodyHandControlThreeStep(lHandControl, handRoute, pathRightSide, false, immediate, logHandTargetsThisFrame);
                else
                    MoveHandControlThenRotateViaMidpoint(lHandControl, midTarget, target, center, pathRightSide, false, immediate, handRoute.useActorMidpointRoute);
                moved++;
            }

            if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
            {
                bool pathRightSide = rightPathRightSideForHands;
                Vector3 root = GetHandRootPosition(pathRightSide);
                Vector3 finalDesired = GetHandRouteFinalPoint(handRoute, pathRightSide);
                Vector3 midDesired = GetHandRouteMidPoint(handRoute, pathRightSide, finalDesired);
                Vector3 target = handRoute.useActorMidpointRoute
                    ? finalDesired
                    : GetReachLimitedPosition(root, finalDesired, GetMaxHandReach(), GetHandPalmOffset(), rHandControl, true, pathRightSide);
                Vector3 midTarget = handRoute.useActorMidpointRoute
                    ? midDesired
                    : GetReachLimitedPosition(root, midDesired, GetMaxHandReach(), GetHandPalmOffset(), rHandControl, true, pathRightSide);
                if (logHandTargetsThisFrame)
                    LogHandTargetDebug(true, rHandControl, root, midDesired, target, center, handCenter, handSide, handPathWidth, pathRightSide, effectiveSwapSidePaths, immediate);
                if (handRoute.useHugBodyThreeStepRoute)
                    MoveHugBodyHandControlThreeStep(rHandControl, handRoute, pathRightSide, true, immediate, logHandTargetsThisFrame);
                else
                    MoveHandControlThenRotateViaMidpoint(rHandControl, midTarget, target, center, pathRightSide, true, immediate, handRoute.useActorMidpointRoute);
                moved++;
            }

        }

        if (includeFeet)
        {
            bool frontSideForFeet = IsGrabberInFrontOfTargetPerson(center);

            if (logHandTargetsThisFrame)
            {
                DebugLog("[FOOT PATH] frontSide=" + Bool01(frontSideForFeet) +
                    " backSide=" + Bool01(!frontSideForFeet) +
                    " swapPaths=" + Bool01(swapSidePaths) +
                    " leftPathRight=" + Bool01(frontSideForFeet ? swapSidePaths : !swapSidePaths) +
                    " rightPathRight=" + Bool01(frontSideForFeet ? !swapSidePaths : swapSidePaths));
            }

            if (leftFootJSON != null && leftFootJSON.val)
            {
                // v5bd:
                // Foot path must keep the known-good back-side route, but flip L/R on the target front-side route.
                // Hands already use their final-point route; feet still use the legacy side-path route, so front-side needs its own correction.
                bool pathRightSide = frontSideForFeet ? swapSidePaths : !swapSidePaths;
                if (lKneeControl != null)
                {
                    Vector3 kneeTarget = GetKneeTargetPosition(pathRightSide, footCenter, footSide);
                    MoveControl(lKneeControl, kneeTarget, Quaternion.identity, false, immediate);
                    moved++;
                }

                if (lFootControl != null)
                {
                    Vector3 finalDesired = footCenter + GetFootLateralOffset(pathRightSide, footSide);
                    MoveFootControlWithArc(lFootControl, lKneeControl, GetFootRootPosition(pathRightSide), finalDesired, footSide, pathRightSide, immediate);
                    moved++;
                }
            }

            if (rightFootJSON != null && rightFootJSON.val)
            {
                // v5bd:
                // Foot path must keep the known-good back-side route, but flip L/R on the target front-side route.
                // Hands already use their final-point route; feet still use the legacy side-path route, so front-side needs its own correction.
                bool pathRightSide = frontSideForFeet ? !swapSidePaths : swapSidePaths;
                if (rKneeControl != null)
                {
                    Vector3 kneeTarget = GetKneeTargetPosition(pathRightSide, footCenter, footSide);
                    MoveControl(rKneeControl, kneeTarget, Quaternion.identity, false, immediate);
                    moved++;
                }

                if (rFootControl != null)
                {
                    Vector3 finalDesired = footCenter + GetFootLateralOffset(pathRightSide, footSide);
                    MoveFootControlWithArc(rFootControl, rKneeControl, GetFootRootPosition(pathRightSide), finalDesired, footSide, pathRightSide, immediate);
                    moved++;
                }
            }
        }

        if (IsDebugEnabled())
        {
            SetStatus("Grab applied / moved=" + moved.ToString(CultureInfo.InvariantCulture) +
                " / follow=" + GetFollowMode() +
                " / time=" + GetMoveTLinear().ToString("F2", CultureInfo.InvariantCulture) +
                " / width=" + currentGrabWidth.ToString("F3", CultureInfo.InvariantCulture) +
                " / finalWidth=" + GetFinalGrabWidth().ToString("F3", CultureInfo.InvariantCulture) +
                " / palmOffset=" + GetHandPalmOffset().ToString("F3", CultureInfo.InvariantCulture) +
                " / handCenter=" + GetHandCenterOffset().ToString("F3", CultureInfo.InvariantCulture) +
                " / handPalm=" + (ShouldAlignHandPalm() ? "ON" : "OFF") +
                " / head=" + (includeHead ? "ON" : "OFF") +
                " / headDist=" + GetHeadTargetDistance().ToString("F3", CultureInfo.InvariantCulture) +
                " / hug=" + (IsHugMode() ? "ON" : "OFF") +
                " / kneeMul=" + GetKneeWidthMultiplier().ToString("F2", CultureInfo.InvariantCulture) +
                " / footArc=" + GetFootArcWidth().ToString("F2", CultureInfo.InvariantCulture) +
                " / footDrop=" + GetFootArcDrop().ToString("F2", CultureInfo.InvariantCulture) +
                " / footSole=" + (ShouldAlignFootSole() ? "ON" : "OFF"));
        }
    }

    private int ApplyHeadGrabIfNeeded(bool immediate, bool includeHead, Vector3 targetCenter)
    {
        if (!includeHead)
            return 0;

        if (headControl == null)
        {
            SetStatus("Kiss needs headControl");
            return 0;
        }

        Vector3 target = GetHeadGrabTargetPosition(targetCenter);
        bool alignFace = ShouldKissFaceAlign();
        Quaternion rotation = alignFace ? GetKissFaceAlignedRotation(targetCenter) : GetControlRotation(headControl);
        MoveControl(headControl, target, rotation, alignFace, immediate);

        if (IsDebugEnabled())
        {
            DebugLog("[HEAD GRAB] target=" + FormatVector3(target) +
                " center=" + FormatVector3(targetCenter) +
                " distance=" + GetHeadTargetDistance().ToString("F3", CultureInfo.InvariantCulture) +
                " maxReach=" + GetMaxHeadReach().ToString("F3", CultureInfo.InvariantCulture) +
                " faceAlign=" + (alignFace ? "1" : "0") +
                " faceStrength=" + GetKissFaceStrength().ToString("F2", CultureInfo.InvariantCulture));
        }

        return 1;
    }

    private Vector3 GetHeadGrabTargetPosition(Vector3 targetCenter)
    {
        Vector3 headPos = GetControlPosition(headControl);
        Vector3 approach = headPos - targetCenter;

        if (approach.sqrMagnitude < 0.0001f)
            approach = GetSelectedPersonForwardAxis();

        if (approach.sqrMagnitude < 0.0001f)
            approach = Vector3.forward;

        Vector3 desired = targetCenter + approach.normalized * GetHeadTargetDistance();
        Vector3 root = GetHeadRootPosition(headPos);
        Vector3 delta = desired - root;
        float maxReach = GetMaxHeadReach();
        float dist = delta.magnitude;

        if (dist > maxReach && dist > 0.0001f)
            desired = root + delta.normalized * maxReach;

        return desired;
    }

    private Quaternion GetKissFaceAlignedRotation(Vector3 targetCenter)
    {
        Quaternion currentRot;
        if (!grabStartRotations.TryGetValue(headControl, out currentRot))
            currentRot = GetControlRotation(headControl);

        Vector3 headPos = GetControlPosition(headControl);
        Vector3 desiredDir = targetCenter - headPos;
        if (desiredDir.sqrMagnitude < 0.0001f)
            return currentRot;

        desiredDir.Normalize();

        Vector3 up = GetBodyUpAxis();
        if (up.sqrMagnitude < 0.0001f)
            up = Vector3.up;
        up.Normalize();

        Vector3 currentForward = currentRot * Vector3.forward;
        if (currentForward.sqrMagnitude < 0.0001f)
            currentForward = GetSelectedPersonForwardAxis();
        if (currentForward.sqrMagnitude < 0.0001f)
            currentForward = Vector3.forward;
        currentForward.Normalize();

        Vector3 currentFlat = Vector3.ProjectOnPlane(currentForward, up);
        Vector3 desiredFlat = Vector3.ProjectOnPlane(desiredDir, up);

        float strength = GetKissFaceStrength();
        float yaw = 0.0f;
        Quaternion yawRot = Quaternion.identity;

        if (currentFlat.sqrMagnitude > 0.0001f && desiredFlat.sqrMagnitude > 0.0001f)
        {
            currentFlat.Normalize();
            desiredFlat.Normalize();
            yaw = Mathf.Clamp(SignedAngleAroundAxis(currentFlat, desiredFlat, up), -45.0f, 45.0f) * strength;
            yawRot = Quaternion.AngleAxis(yaw, up);
        }

        Vector3 yawedForward = yawRot * currentForward;
        if (yawedForward.sqrMagnitude < 0.0001f)
            yawedForward = currentForward;
        yawedForward.Normalize();

        Vector3 right = Vector3.Cross(up, yawedForward);
        if (right.sqrMagnitude < 0.0001f)
            right = currentRot * Vector3.right;
        if (right.sqrMagnitude < 0.0001f)
            right = Vector3.right;
        right.Normalize();

        float pitch = Mathf.Clamp(SignedAngleAroundAxis(yawedForward, desiredDir, right), -35.0f, 35.0f) * strength;
        Quaternion pitchRot = Quaternion.AngleAxis(pitch, right);

        if (IsDebugEnabled())
        {
            DebugLog("[KISS FACE] yaw=" + yaw.ToString("F1", CultureInfo.InvariantCulture) +
                " pitch=" + pitch.ToString("F1", CultureInfo.InvariantCulture) +
                " strength=" + strength.ToString("F2", CultureInfo.InvariantCulture));
        }

        return pitchRot * yawRot * currentRot;
    }

    private float SignedAngleAroundAxis(Vector3 from, Vector3 to, Vector3 axis)
    {
        if (from.sqrMagnitude < 0.0001f || to.sqrMagnitude < 0.0001f || axis.sqrMagnitude < 0.0001f)
            return 0.0f;

        from.Normalize();
        to.Normalize();
        axis.Normalize();

        Vector3 cross = Vector3.Cross(from, to);
        float sin = Vector3.Dot(axis, cross);
        float cos = Mathf.Clamp(Vector3.Dot(from, to), -1.0f, 1.0f);
        return Mathf.Atan2(sin, cos) * Mathf.Rad2Deg;
    }

    private Vector3 GetHeadRootPosition(Vector3 fallback)
    {
        if (chestControl != null)
            return GetControlPosition(chestControl);

        if (selectedPerson != null && selectedPerson.transform != null)
            return selectedPerson.transform.position + GetBodyUpAxis() * 1.35f;

        return fallback;
    }

    private void ApplyNipplePairGrab(bool immediate, bool includeHands, bool includeFeet, Vector3 center, Vector3 handCenter, Vector3 footCenter, Vector3 side)
    {
        LogChestHoldHugBodyRouteAlways("enter-apply-nipple-pair", includeHands, includeFeet, false, false, center, true, IsHipHoldMode(), IsTargetPairMode(), true);

        // v22 hard gate: この関数は Chest Hold 専用。
        // Hug Body や他ターゲットから呼ばれても nipple route に入らないよう即returnする。
        if (!IsChestHoldTarget())
        {
            DebugLog("[CHEST HOLD ISOLATE ALERT] nipple-pair route blocked / controller=" +
                (targetPersonPartChooser != null ? targetPersonPartChooser.val : "<null>") +
                " / hugBody=" + Bool01(IsHugBodyTarget()) +
                " / strictChest=" + Bool01(IsChestHoldTarget()) +
                " / nippleCompat=" + Bool01(IsNipplePairMode()));
            return;
        }

        Vector3 targetLeftNipple;
        Vector3 targetRightNipple;
        if (!TryGetTargetNipplePositions(out targetLeftNipple, out targetRightNipple))
        {
            SetStatus("Chest Hold invalid / target nipples not ready");
            DebugLog("[CHEST HOLD] invalid nipples");
            return;
        }

        Vector3 zOffset = GetNipplePairZOffsetVector();
        targetLeftNipple += zOffset;
        targetRightNipple += zOffset;

        Vector3 chestHoldActualPairCenter = (targetLeftNipple + targetRightNipple) * 0.5f;
        Vector3 chestHoldTargetSideAxis = GetChestHoldNippleSideAxis(targetLeftNipple, targetRightNipple, side);
        bool relationValid;
        bool mutualFront;
        bool selfFacingTarget;
        bool targetFacingSelf;
        float selfLooksTarget;
        float targetLooksSelf;
        float rootDot;
        relationValid = TryGetChestHoldMutualFrontRelation(
            chestHoldActualPairCenter,
            out mutualFront,
            out selfFacingTarget,
            out targetFacingSelf,
            out selfLooksTarget,
            out targetLooksSelf,
            out rootDot
        );

        string relationRoute = GetChestHoldRelationRouteLabel(mutualFront, selfFacingTarget, targetFacingSelf, rootDot, relationValid);
        LogChestHoldRelationClass(relationRoute, relationValid, mutualFront, selfFacingTarget, targetFacingSelf, selfLooksTarget, targetLooksSelf, rootDot, chestHoldActualPairCenter);

        // v29: 正面どうし専用ルート。
        // ここだけは既存のtarget正面側判定 / OrderHoldTargets / palm offsetを通さない。
        // 目的はまず「向かい合わせ時に、相手nipple IKへ確実に届く」ことを確立すること。
        // 自分前・相手後ろ等の非mutual-frontは既存Chest Holdへ流し、影響を分離する。
        if (includeHands && relationValid && selfFacingTarget && targetFacingSelf)
        {
            if (!chestHoldModeLogged)
            {
                chestHoldModeLogged = true;
                DebugLog("[CHEST HOLD MODE] mode=face / modeFront=1 / modeBack=0 / relation=mutual-facing" +
                    " / selfFacingTarget=" + Bool01(selfFacingTarget) +
                    " / targetFacingSelf=" + Bool01(targetFacingSelf) +
                    " / selfLooksTarget=" + selfLooksTarget.ToString("F3", CultureInfo.InvariantCulture) +
                    " / targetLooksSelf=" + targetLooksSelf.ToString("F3", CultureInfo.InvariantCulture) +
                    " / rootDot=" + rootDot.ToString("F3", CultureInfo.InvariantCulture));
            }
            int mutualMoved = ApplyChestHoldFrontSimpleCrossHandGrab(immediate, targetLeftNipple, targetRightNipple, chestHoldActualPairCenter, chestHoldTargetSideAxis);
            if (IsDebugEnabled())
            {
                SetStatus("Chest Hold / route=front-simple-cross" +
                    " / moved=" + mutualMoved.ToString(CultureInfo.InvariantCulture) +
                    " / follow=" + GetFollowMode() +
                    " / time=" + GetMoveTLinear().ToString("F2", CultureInfo.InvariantCulture) +
                    " / hug=" + (IsHugMode() ? "ON" : "OFF"));
            }
            return;
        }

        Vector3 leftSideTarget;
        Vector3 rightSideTarget;
        string mode;

        if (!TryGetAssignedNippleTargets(out leftSideTarget, out rightSideTarget, out mode))
        {
            SetStatus("Chest Hold invalid / angle or target not ready");
            DebugLog("[CHEST HOLD] invalid");
            return;
        }

        leftSideTarget += zOffset;
        rightSideTarget += zOffset;

        // v82: Front(face) is deliberately simple and isolated.
        // Do not pass through OrderHoldTargets / palm offset / side remap.
        // Contract: R hand -> target left nipple, L hand -> target right nipple.
        // Back route remains unchanged below.
        if (includeHands && mode == "face")
        {
            int frontMoved = ApplyChestHoldFrontSimpleCrossHandGrab(immediate, targetLeftNipple, targetRightNipple, chestHoldActualPairCenter, chestHoldTargetSideAxis);
            if (IsDebugEnabled())
            {
                SetStatus("Chest Hold / route=front-simple-cross" +
                    " / moved=" + frontMoved.ToString(CultureInfo.InvariantCulture) +
                    " / follow=" + GetFollowMode() +
                    " / time=" + GetMoveTLinear().ToString("F2", CultureInfo.InvariantCulture) +
                    " / hug=" + (IsHugMode() ? "ON" : "OFF"));
            }
            return;
        }

        Vector3 footSide = GetFootSideAxis(side);
        Vector3 rawLeftSideTarget = leftSideTarget;
        Vector3 rawRightSideTarget = rightSideTarget;
        Vector3 leftHandTarget = leftSideTarget;
        Vector3 rightHandTarget = rightSideTarget;
        Vector3 leftFootTarget = leftSideTarget;
        Vector3 rightFootTarget = rightSideTarget;
        OrderHoldTargetsForHands(ref leftHandTarget, ref rightHandTarget, chestHoldActualPairCenter, chestHoldTargetSideAxis);
        OrderHoldTargetsForFeet(ref leftFootTarget, ref rightFootTarget, center, side);
        LogHoldTargetOrder("Chest Hold", mode, rawLeftSideTarget, rawRightSideTarget, leftHandTarget, rightHandTarget, chestHoldActualPairCenter, chestHoldTargetSideAxis);
        LogHoldFootTargetOrder("Chest Hold", mode, rawLeftSideTarget, rawRightSideTarget, leftFootTarget, rightFootTarget, center, side);

        int moved = 0;

        if (includeHands)
        {
            bool crossedTargets = mode == "face";
            Vector3 chestHoldPairCenter = (rawLeftSideTarget + rawRightSideTarget) * 0.5f;
            bool chestHoldVisualFrontSide = IsChestHoldFrontSideByTargetVisualSide(chestHoldPairCenter, mode);
            bool chestHoldFrontSide = IsChestHoldFrontSideFromMode(mode);
            bool chestHoldBackMode = !chestHoldFrontSide;

            // v64: Chest Holdの実処理/通常ログは位置(posFront)ではなく mode(face/back) を優先する。
            // 例: target-front-self-back は posFront=1 でも mode=back なので、Back reachログ/Back reach処理に入れる。
            // chestHoldVisualFrontSide はログ/確認用。
            if (!chestHoldModeLogged)
            {
                chestHoldModeLogged = true;
                DebugLog("[CHEST HOLD MODE]" +
                    " mode=" + (mode ?? "") +
                    " / modeFront=" + Bool01(chestHoldFrontSide) +
                    " / modeBack=" + Bool01(chestHoldBackMode) +
                    " / visualFront=" + Bool01(chestHoldVisualFrontSide) +
                    " / visualBack=" + Bool01(!chestHoldVisualFrontSide));
            }

            // v21: 背面Chest HoldはL hand->L nipple / R hand->R nipple固定。v20のLR swapは不採用。
            // ここで並べ替えを残すと、背面2段階viaの最終収束先が左右入れ替わり、
            // 右手が左nipple側へ行くように見えるケースがある。
            // front側は従来どおり、face時の見た目左右/近い手割当を維持する。
            if (chestHoldBackMode)
            {
                leftHandTarget = rawLeftSideTarget;
                rightHandTarget = rawRightSideTarget;

                if (IsDebugEnabled())
                {
                    DebugLog("[CHEST HOLD BACK LR RESTORE] mode=" + (mode ?? "") +
                        " / front=" + Bool01(chestHoldFrontSide) +
                        " / rawL=" + FormatVector3(rawLeftSideTarget) +
                        " / rawR=" + FormatVector3(rawRightSideTarget) +
                        " / usedL=" + FormatVector3(leftHandTarget) +
                        " / usedR=" + FormatVector3(rightHandTarget) +
                        " / pairCenter=" + FormatVector3(chestHoldPairCenter) +
                        " / targetSideAxis=" + FormatVector3(chestHoldTargetSideAxis));
                }
            }

            bool leftTargetRightSide = IsTargetOnPositiveSide(leftHandTarget, chestHoldPairCenter, chestHoldTargetSideAxis);
            bool rightTargetRightSide = IsTargetOnPositiveSide(rightHandTarget, chestHoldPairCenter, chestHoldTargetSideAxis);

            Vector3 chestHoldBackStep2Left = Vector3.zero;
            Vector3 chestHoldBackStep2Right = Vector3.zero;
            Vector3 chestHoldBackStep3Left = Vector3.zero;
            Vector3 chestHoldBackStep3Right = Vector3.zero;
            bool hasChestHoldBackReachTargets = false;
            bool chestHoldBackFollowLeftActive = false;
            bool chestHoldBackFollowRightActive = false;
            if (chestHoldBackMode)
            {
                // v73 steps:
                // step2 = current approved side reach position, nipple offset 0.000
                // step3 = same side reach position plus Chest Hold Back Nipple Offset slider, default -0.030
                // step4 = Wrist In after step3 is reached
                chestHoldBackStep2Left = GetChestHoldBackPassOffsetTargetWithOffset(lHandControl, leftHandTarget, chestHoldPairCenter, chestHoldTargetSideAxis, false, 0.0f);
                chestHoldBackStep2Right = GetChestHoldBackPassOffsetTargetWithOffset(rHandControl, rightHandTarget, chestHoldPairCenter, chestHoldTargetSideAxis, true, 0.0f);
                chestHoldBackStep3Left = GetChestHoldBackPassOffsetTarget(lHandControl, leftHandTarget, chestHoldPairCenter, chestHoldTargetSideAxis, false);
                chestHoldBackStep3Right = GetChestHoldBackPassOffsetTarget(rHandControl, rightHandTarget, chestHoldPairCenter, chestHoldTargetSideAxis, true);
                hasChestHoldBackReachTargets = true;
                // Existing log names map as:
                // reachBase = step2, reachAdj = step3, final = nipple.
                LogChestHoldEssentialTwoLineAlways(chestHoldBackStep3Right, chestHoldBackStep3Left, rightHandTarget, leftHandTarget);
            }

            if (leftHandJSON != null && leftHandJSON.val && lHandControl != null)
            {
                if (chestHoldBackMode)
                {
                    Vector3 step2 = hasChestHoldBackReachTargets ? chestHoldBackStep2Left : GetChestHoldBackPassOffsetTargetWithOffset(lHandControl, leftHandTarget, chestHoldPairCenter, chestHoldTargetSideAxis, false, 0.0f);
                    Vector3 step3 = hasChestHoldBackReachTargets ? chestHoldBackStep3Left : GetChestHoldBackPassOffsetTarget(lHandControl, leftHandTarget, chestHoldPairCenter, chestHoldTargetSideAxis, false);
                    MoveChestHoldBackStep2Step3Control(lHandControl, step2, step3, immediate);
                    MaybeBoostChestHoldBackLeftGrasp(immediate);
                    ApplyChestHoldBackWristInAtStep4(lHandControl, leftHandTarget, false, immediate);
                    chestHoldBackFollowLeftActive = true;
                    moved++;
                }
                else
                {
                    Vector3 leftRoot = GetHandRootPosition(false);
                    Vector3 leftSideGrabTarget = GetNipplePairSideGrabTarget(leftHandTarget, chestHoldTargetSideAxis, leftTargetRightSide);
                    Vector3 leftPalmTarget = GetChestHoldAdjustedPalmTarget(leftHandTarget, false, chestHoldFrontSide);
                    Vector3 target = GetChestHoldPalmCenteredWristTarget(lElbowControl, lHandControl, leftPalmTarget, false, chestHoldFrontSide);
                    LogHoldHandTarget("Chest Hold", mode, false, leftHandTarget, leftSideGrabTarget, target, leftRoot, chestHoldTargetSideAxis, leftTargetRightSide, chestHoldPairCenter, immediate);
                    MoveChestHoldHandControl(lHandControl, target, leftPalmTarget, false, immediate, chestHoldFrontSide, chestHoldPairCenter, chestHoldTargetSideAxis, leftTargetRightSide, leftHandTarget);
                    ApplyChestHoldFrontUpBackInWrist(lHandControl, leftPalmTarget, false, immediate, chestHoldFrontSide);
                    LogChestHoldFinalHandNipplePosition(lHandControl, leftHandTarget, leftPalmTarget, false, immediate, chestHoldFrontSide);
                    moved++;
                }
            }

            if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
            {
                if (chestHoldBackMode)
                {
                    Vector3 step2 = hasChestHoldBackReachTargets ? chestHoldBackStep2Right : GetChestHoldBackPassOffsetTargetWithOffset(rHandControl, rightHandTarget, chestHoldPairCenter, chestHoldTargetSideAxis, true, 0.0f);
                    Vector3 step3 = hasChestHoldBackReachTargets ? chestHoldBackStep3Right : GetChestHoldBackPassOffsetTarget(rHandControl, rightHandTarget, chestHoldPairCenter, chestHoldTargetSideAxis, true);
                    MoveChestHoldBackStep2Step3Control(rHandControl, step2, step3, immediate);
                    MaybeBoostChestHoldBackRightGrasp(immediate);
                    ApplyChestHoldBackWristInAtStep4(rHandControl, rightHandTarget, true, immediate);
                    chestHoldBackFollowRightActive = true;
                    moved++;
                }
                else
                {
                    Vector3 rightRoot = GetHandRootPosition(true);
                    Vector3 rightSideGrabTarget = GetNipplePairSideGrabTarget(rightHandTarget, chestHoldTargetSideAxis, rightTargetRightSide);
                    Vector3 rightPalmTarget = GetChestHoldAdjustedPalmTarget(rightHandTarget, true, chestHoldFrontSide);
                    Vector3 target = GetChestHoldPalmCenteredWristTarget(rElbowControl, rHandControl, rightPalmTarget, true, chestHoldFrontSide);
                    LogHoldHandTarget("Chest Hold", mode, true, rightHandTarget, rightSideGrabTarget, target, rightRoot, chestHoldTargetSideAxis, rightTargetRightSide, chestHoldPairCenter, immediate);
                    MoveChestHoldHandControl(rHandControl, target, rightPalmTarget, true, immediate, chestHoldFrontSide, chestHoldPairCenter, chestHoldTargetSideAxis, rightTargetRightSide, rightHandTarget);
                    ApplyChestHoldFrontUpBackInWrist(rHandControl, rightPalmTarget, true, immediate, chestHoldFrontSide);
                    LogChestHoldFinalHandNipplePosition(rHandControl, rightHandTarget, rightPalmTarget, true, immediate, chestHoldFrontSide);
                    moved++;
                }
            }

            if (chestHoldBackMode && (chestHoldBackFollowLeftActive || chestHoldBackFollowRightActive))
            {
                FreeControllerV3 targetLeftNippleControl;
                FreeControllerV3 targetRightNippleControl;
                List<FreeControllerV3> nippleControls;
                if (TryGetChestHoldNippleIKControls(out nippleControls, out targetLeftNippleControl, out targetRightNippleControl))
                {
                    ArmChestHoldNippleHandFollow(
                        chestHoldBackFollowLeftActive,
                        chestHoldBackFollowRightActive,
                        targetLeftNippleControl,
                        targetRightNippleControl,
                        chestHoldBackStep3Left,
                        chestHoldBackStep3Right,
                        immediate,
                        "back"
                    );
                }
            }
        }

        if (includeFeet)
        {
            if (leftFootJSON != null && leftFootJSON.val)
            {
                bool leftFootRightSide = IsTargetOnPositiveSide(leftFootTarget, center, side);
                if (lKneeControl != null)
                {
                    Vector3 kneeTarget = GetKneeTargetPosition(leftFootRightSide, footCenter, footSide);
                    MoveControl(lKneeControl, kneeTarget, Quaternion.identity, false, immediate);
                    moved++;
                }

                if (lFootControl != null)
                {
                    Vector3 root = GetFootRootPosition(leftFootRightSide);
                    LogHoldFootTarget("Chest Hold", mode, false, leftFootTarget, root, footSide, leftFootRightSide, center, immediate);
                    MoveFootControlWithArc(lFootControl, lKneeControl, root, leftFootTarget, footSide, leftFootRightSide, immediate);
                    moved++;
                }
            }

            if (rightFootJSON != null && rightFootJSON.val)
            {
                bool rightFootRightSide = IsTargetOnPositiveSide(rightFootTarget, center, side);
                if (rKneeControl != null)
                {
                    Vector3 kneeTarget = GetKneeTargetPosition(rightFootRightSide, footCenter, footSide);
                    MoveControl(rKneeControl, kneeTarget, Quaternion.identity, false, immediate);
                    moved++;
                }

                if (rFootControl != null)
                {
                    Vector3 root = GetFootRootPosition(rightFootRightSide);
                    LogHoldFootTarget("Chest Hold", mode, true, rightFootTarget, root, footSide, rightFootRightSide, center, immediate);
                    MoveFootControlWithArc(rFootControl, rKneeControl, root, rightFootTarget, footSide, rightFootRightSide, immediate);
                    moved++;
                }
            }
        }

        if (IsDebugEnabled())
        {
            SetStatus("Chest Hold / mode=" + mode +
                " / moved=" + moved.ToString(CultureInfo.InvariantCulture) +
                " / follow=" + GetFollowMode() +
                " / time=" + GetMoveTLinear().ToString("F2", CultureInfo.InvariantCulture) +
                " / hug=" + (IsHugMode() ? "ON" : "OFF"));
        }
    }

    private void ApplyHipHoldGrab(bool immediate, bool includeHands, bool includeFeet, Vector3 center, Vector3 handCenter, Vector3 footCenter, Vector3 side)
    {
        Vector3 leftSideTarget;
        Vector3 rightSideTarget;
        string mode;

        if (!TryGetAssignedHipHoldTargets(out leftSideTarget, out rightSideTarget, out mode))
        {
            SetStatus("Hip Hold invalid / target not ready");
            DebugLog("[HIP HOLD] invalid");
            return;
        }

        if (includeHands)
        {
            // V5at: Hip Hold has its own route and center/side calculation, so apply the one-shot pelvis auto here after Hip Hold target resolution.
            ApplyTargetPelvisAutoOnGrab("auto-grab-hip-hold", true, true);
        }

        LockTargetHipHoldThighIK();

        Vector3 handSide = GetHipHoldHandSideAxis(center, side);
        Vector3 footSide = GetFootSideAxis(side);
        Vector3 rawLeftSideTarget = leftSideTarget;
        Vector3 rawRightSideTarget = rightSideTarget;
        Vector3 leftHandTarget = leftSideTarget;
        Vector3 rightHandTarget = rightSideTarget;
        Vector3 leftFootTarget = leftSideTarget;
        Vector3 rightFootTarget = rightSideTarget;
        OrderHoldTargetsForHands(ref leftHandTarget, ref rightHandTarget, center, handSide);
        OrderHoldTargetsForFeet(ref leftFootTarget, ref rightFootTarget, center, footSide);
        LogHoldTargetOrder("Hip Hold", mode, rawLeftSideTarget, rawRightSideTarget, leftHandTarget, rightHandTarget, center, handSide);
        LogHoldFootTargetOrder("Hip Hold", mode, rawLeftSideTarget, rawRightSideTarget, leftFootTarget, rightFootTarget, center, footSide);
        int moved = 0;

        if (includeHands)
        {
            bool crossedTargets = mode == "face";
            bool leftTargetRightSide = IsTargetOnPositiveSide(leftHandTarget, center, handSide);
            bool rightTargetRightSide = IsTargetOnPositiveSide(rightHandTarget, center, handSide);

            if (leftHandJSON != null && leftHandJSON.val && lHandControl != null)
            {
                Vector3 leftRoot = GetHandRootPosition(false);
                Vector3 leftSideGrabTarget = GetNipplePairSideGrabTarget(leftHandTarget, handSide, leftTargetRightSide);
                Vector3 target = GetNipplePairHandTarget(leftRoot, leftSideGrabTarget, lHandControl, false);
                LogHoldHandTarget("Hip Hold", mode, false, leftHandTarget, leftSideGrabTarget, target, leftRoot, handSide, leftTargetRightSide, center, immediate);
                MoveControl(lHandControl, target, Quaternion.identity, false, immediate);
                moved++;
            }

            if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
            {
                Vector3 rightRoot = GetHandRootPosition(true);
                Vector3 rightSideGrabTarget = GetNipplePairSideGrabTarget(rightHandTarget, handSide, rightTargetRightSide);
                Vector3 target = GetNipplePairHandTarget(rightRoot, rightSideGrabTarget, rHandControl, true);
                LogHoldHandTarget("Hip Hold", mode, true, rightHandTarget, rightSideGrabTarget, target, rightRoot, handSide, rightTargetRightSide, center, immediate);
                MoveControl(rHandControl, target, Quaternion.identity, false, immediate);
                moved++;
            }
        }

        if (includeFeet)
        {
            if (leftFootJSON != null && leftFootJSON.val)
            {
                bool leftFootRightSide = IsTargetOnPositiveSide(leftFootTarget, center, side);
                if (lKneeControl != null)
                {
                    Vector3 kneeTarget = GetKneeTargetPosition(leftFootRightSide, footCenter, footSide);
                    MoveControl(lKneeControl, kneeTarget, Quaternion.identity, false, immediate);
                    moved++;
                }

                if (lFootControl != null)
                {
                    Vector3 root = GetFootRootPosition(leftFootRightSide);
                    LogHoldFootTarget("Hip Hold", mode, false, leftFootTarget, root, footSide, leftFootRightSide, center, immediate);
                    MoveFootControlWithArc(lFootControl, lKneeControl, root, leftFootTarget, footSide, leftFootRightSide, immediate);
                    moved++;
                }
            }

            if (rightFootJSON != null && rightFootJSON.val)
            {
                bool rightFootRightSide = IsTargetOnPositiveSide(rightFootTarget, center, side);
                if (rKneeControl != null)
                {
                    Vector3 kneeTarget = GetKneeTargetPosition(rightFootRightSide, footCenter, footSide);
                    MoveControl(rKneeControl, kneeTarget, Quaternion.identity, false, immediate);
                    moved++;
                }

                if (rFootControl != null)
                {
                    Vector3 root = GetFootRootPosition(rightFootRightSide);
                    LogHoldFootTarget("Hip Hold", mode, true, rightFootTarget, root, footSide, rightFootRightSide, center, immediate);
                    MoveFootControlWithArc(rFootControl, rKneeControl, root, rightFootTarget, footSide, rightFootRightSide, immediate);
                    moved++;
                }
            }
        }

        if (IsDebugEnabled())
        {
            SetStatus("Hip Hold / mode=" + mode +
                " / moved=" + moved.ToString(CultureInfo.InvariantCulture) +
                " / follow=" + GetFollowMode() +
                " / time=" + GetMoveTLinear().ToString("F2", CultureInfo.InvariantCulture));
        }
    }

    private Vector3 GetHipHoldHandSideAxis(Vector3 center, Vector3 fallbackSide)
    {
        Vector3 fallback = fallbackSide.sqrMagnitude > 0.0001f ? fallbackSide.normalized : Vector3.right;
        Vector3 up = GetTargetRootRotation() * Vector3.up;
        if (up.sqrMagnitude < 0.0001f)
            up = Vector3.up;
        up.Normalize();

        Vector3 approach = GetGrabberReferencePosition() - center;
        approach -= up * Vector3.Dot(approach, up);

        if (approach.sqrMagnitude < 0.0001f)
            return fallback;

        approach.Normalize();
        Vector3 axis = Vector3.Cross(up, approach);
        if (axis.sqrMagnitude < 0.0001f)
            return fallback;

        axis.Normalize();
        if (Vector3.Dot(axis, fallback) < 0.0f)
            axis = -axis;

        if (IsDebugEnabled())
        {
            DebugLog("[HIP HOLD SIDE] base=" + FormatVector3(fallback) +
                " up=" + FormatVector3(up) +
                " approach=" + FormatVector3(approach) +
                " handSide=" + FormatVector3(axis));
        }

        return axis;
    }

    private Vector3 GetGrabberReferencePosition()
    {
        if (chestControl != null)
            return chestControl.control != null ? chestControl.control.position : chestControl.transform.position;

        if (selectedPerson != null)
            return selectedPerson.transform.position;

        return Vector3.zero;
    }

    private void ApplyTargetPairGrab(bool immediate, bool includeHands, bool includeFeet, Vector3 center, Vector3 handCenter, Vector3 footCenter, Vector3 side)
    {
        Vector3 leftTarget;
        Vector3 rightTarget;
        string controller = targetPersonPartChooser != null ? targetPersonPartChooser.val : "Pair";
        string mode = "pair";

        if (!TryGetTargetPairPositions(out leftTarget, out rightTarget))
        {
            SetStatus(controller + " invalid / pair target not ready");
            DebugLog("[TARGET PAIR] invalid controller=" + controller);
            return;
        }

        Vector3 footSide = GetFootSideAxis(side);
        Vector3 leftHandTarget = leftTarget;
        Vector3 rightHandTarget = rightTarget;
        Vector3 leftFootTarget = leftTarget;
        Vector3 rightFootTarget = rightTarget;
        OrderHoldTargetsForHands(ref leftHandTarget, ref rightHandTarget, center, side);
        OrderHoldTargetsForFeet(ref leftFootTarget, ref rightFootTarget, center, side);
        LogHoldTargetOrder(controller, mode, leftTarget, rightTarget, leftHandTarget, rightHandTarget, center, side);
        LogHoldFootTargetOrder(controller, mode, leftTarget, rightTarget, leftFootTarget, rightFootTarget, center, side);

        int moved = 0;

        if (includeHands)
        {
            if (leftHandJSON != null && leftHandJSON.val && lHandControl != null)
            {
                Vector3 leftRoot = GetHandRootPosition(false);
                bool targetRightSide = IsTargetOnPositiveSide(leftHandTarget, center, side);
                Vector3 sideTarget = GetPairOutsidePoint(leftHandTarget, center, side, PAIR_FINAL_OUTSIDE_OFFSET);
                Vector3 target = GetNipplePairHandTarget(leftRoot, sideTarget, lHandControl, false);
                LogHoldHandTarget(controller, mode, false, leftHandTarget, sideTarget, target, leftRoot, side, targetRightSide, center, immediate);
                MovePairHandControlWithGrabWidthMidpoint(lHandControl, target, center, false, immediate);
                moved++;
            }

            if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
            {
                Vector3 rightRoot = GetHandRootPosition(true);
                bool targetRightSide = IsTargetOnPositiveSide(rightHandTarget, center, side);
                Vector3 sideTarget = GetPairOutsidePoint(rightHandTarget, center, side, PAIR_FINAL_OUTSIDE_OFFSET);
                Vector3 target = GetNipplePairHandTarget(rightRoot, sideTarget, rHandControl, true);
                LogHoldHandTarget(controller, mode, true, rightHandTarget, sideTarget, target, rightRoot, side, targetRightSide, center, immediate);
                MovePairHandControlWithGrabWidthMidpoint(rHandControl, target, center, true, immediate);
                moved++;
            }
        }

        if (includeFeet)
        {
            if (leftFootJSON != null && leftFootJSON.val)
            {
                bool leftFootRightSide = IsTargetOnPositiveSide(leftFootTarget, center, side);
                if (lKneeControl != null)
                {
                    Vector3 kneeTarget = GetKneeTargetPosition(leftFootRightSide, footCenter, footSide);
                    MoveControl(lKneeControl, kneeTarget, Quaternion.identity, false, immediate);
                    moved++;
                }

                if (lFootControl != null)
                {
                    Vector3 root = GetFootRootPosition(leftFootRightSide);
                    Vector3 finalTarget = GetPairOutsidePoint(leftFootTarget, center, footSide, PAIR_FINAL_OUTSIDE_OFFSET);
                    LogHoldFootTarget(controller, mode, false, finalTarget, root, footSide, leftFootRightSide, center, immediate);
                    MoveFootControlWithArc(lFootControl, lKneeControl, root, finalTarget, footSide, leftFootRightSide, immediate);
                    moved++;
                }
            }

            if (rightFootJSON != null && rightFootJSON.val)
            {
                bool rightFootRightSide = IsTargetOnPositiveSide(rightFootTarget, center, side);
                if (rKneeControl != null)
                {
                    Vector3 kneeTarget = GetKneeTargetPosition(rightFootRightSide, footCenter, footSide);
                    MoveControl(rKneeControl, kneeTarget, Quaternion.identity, false, immediate);
                    moved++;
                }

                if (rFootControl != null)
                {
                    Vector3 root = GetFootRootPosition(rightFootRightSide);
                    Vector3 finalTarget = GetPairOutsidePoint(rightFootTarget, center, footSide, PAIR_FINAL_OUTSIDE_OFFSET);
                    LogHoldFootTarget(controller, mode, true, finalTarget, root, footSide, rightFootRightSide, center, immediate);
                    MoveFootControlWithArc(rFootControl, rKneeControl, root, finalTarget, footSide, rightFootRightSide, immediate);
                    moved++;
                }
            }
        }

        if (IsDebugEnabled())
        {
            SetStatus(controller + " / mode=pair" +
                " / moved=" + moved.ToString(CultureInfo.InvariantCulture) +
                " / follow=" + GetFollowMode() +
                " / time=" + GetMoveTLinear().ToString("F2", CultureInfo.InvariantCulture));
        }
    }


    private bool TryGetChestHoldMutualFrontRelation(Vector3 targetPoint, out bool mutualFront, out bool selfFacingTarget, out bool targetFacingSelf, out float selfLooksTarget, out float targetLooksSelf, out float rootDot)
    {
        mutualFront = false;
        selfFacingTarget = false;
        targetFacingSelf = false;
        selfLooksTarget = 0.0f;
        targetLooksSelf = 0.0f;
        rootDot = 0.0f;

        if (selectedPerson == null || selectedPerson.transform == null || selectedTargetPerson == null || selectedTargetPerson.transform == null)
            return false;

        Vector3 selfPos = selectedPerson.transform.position;
        if (chestControl != null)
            selfPos = chestControl.control != null ? chestControl.control.position : chestControl.transform.position;

        Vector3 toTarget = targetPoint - selfPos;
        Vector3 toSelf = selfPos - targetPoint;
        // v83: Chest Hold front/back relation must use the body/chest facing used by nipple route,
        // not only Person root facing. Root can remain same-facing while the actual chest/front pose is face-to-face.
        Vector3 selfForward = GetNipplePairActorForwardAxis();
        Vector3 targetForward = GetNipplePairTargetForwardAxis();

        toTarget.y = 0.0f;
        toSelf.y = 0.0f;
        selfForward.y = 0.0f;
        targetForward.y = 0.0f;

        if (toTarget.sqrMagnitude < 0.0001f || toSelf.sqrMagnitude < 0.0001f || selfForward.sqrMagnitude < 0.0001f || targetForward.sqrMagnitude < 0.0001f)
            return false;

        toTarget.Normalize();
        toSelf.Normalize();
        selfForward.Normalize();
        targetForward.Normalize();

        selfLooksTarget = Vector3.Dot(selfForward, toTarget);
        targetLooksSelf = Vector3.Dot(targetForward, toSelf);
        rootDot = Vector3.Dot(selfForward, targetForward);

        selfFacingTarget = selfLooksTarget >= CHEST_HOLD_MUTUAL_FACE_DOT;
        targetFacingSelf = targetLooksSelf >= CHEST_HOLD_MUTUAL_FACE_DOT;
        mutualFront = selfFacingTarget && targetFacingSelf && rootDot <= CHEST_HOLD_MUTUAL_ROOT_OPPOSITE_DOT;
        return true;
    }

    private string GetChestHoldRelationRouteLabel(bool mutualFront, bool selfFacingTarget, bool targetFacingSelf, float rootDot, bool relationValid)
    {
        if (!relationValid)
            return "relation-invalid-legacy";
        if (mutualFront)
            return "mutual-front-exact";
        if (selfFacingTarget && !targetFacingSelf)
            return "self-front-target-back-legacy";
        if (!selfFacingTarget && targetFacingSelf)
            return "target-front-self-back-legacy";
        if (rootDot > 0.35f)
            return "same-facing-legacy";
        return "diagonal-legacy";
    }

    private void LogChestHoldRelationClass(string route, bool relationValid, bool mutualFront, bool selfFacingTarget, bool targetFacingSelf, float selfLooksTarget, float targetLooksSelf, float rootDot, Vector3 targetPoint)
    {
        if (!IsDebugEnabled())
            return;

        DebugLog("[CHEST HOLD RELATION] route=" + (route ?? "") +
            " / valid=" + Bool01(relationValid) +
            " / mutualFront=" + Bool01(mutualFront) +
            " / selfFacingTarget=" + Bool01(selfFacingTarget) +
            " / targetFacingSelf=" + Bool01(targetFacingSelf) +
            " / selfLooksTarget=" + selfLooksTarget.ToString("F3", CultureInfo.InvariantCulture) +
            " / targetLooksSelf=" + targetLooksSelf.ToString("F3", CultureInfo.InvariantCulture) +
            " / rootDot=" + rootDot.ToString("F3", CultureInfo.InvariantCulture) +
            " / targetPoint=" + FormatVector3(targetPoint));
    }

    private Vector3 GetChestHoldNippleSideAxis(Vector3 targetLeftNipple, Vector3 targetRightNipple, Vector3 fallbackSide)
    {
        Vector3 side = targetRightNipple - targetLeftNipple;
        side.y = 0.0f;
        if (side.sqrMagnitude < 0.0001f)
        {
            side = fallbackSide;
            side.y = 0.0f;
        }
        if (side.sqrMagnitude < 0.0001f)
            side = selectedTargetPerson != null && selectedTargetPerson.transform != null ? selectedTargetPerson.transform.right : Vector3.right;
        side.y = 0.0f;
        if (side.sqrMagnitude < 0.0001f)
            side = Vector3.right;
        return side.normalized;
    }

    private Vector3 GetChestHoldFrontHandForwardAxis(bool rightHand, Vector3 assignedNippleWithDown, Vector3 fallbackAxis)
    {
        // v87:
        // Do NOT use current hand IK/grab-start hand position here.
        // After one Grab, the hand position is already moved, so pressing Grab again changes the axis.
        // Use a stable body/chest anchor instead: self body -> assigned nipple route.
        Vector3 anchor = GetHandRootPosition(rightHand);
        Vector3 axis = assignedNippleWithDown - anchor;
        axis.y = 0.0f;

        if (axis.sqrMagnitude < 0.0001f)
        {
            axis = fallbackAxis;
            axis.y = 0.0f;
        }
        if (axis.sqrMagnitude < 0.0001f && selectedTargetPerson != null && selectedTargetPerson.transform != null)
        {
            axis = selectedTargetPerson.transform.forward;
            axis.y = 0.0f;
        }
        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.forward;
        return axis.normalized;
    }

    private void BeginChestHoldFrontLeftGraspRun(string reason)
    {
        BeginChestHoldFrontHandGraspRun(
            false,
            reason,
            ref chestHoldFrontLeftGraspRunSerial,
            ref chestHoldFrontLeftGraspBoostedSerial,
            ref chestHoldFrontLeftGraspRestoredSerial,
            ref chestHoldFrontLeftGraspBoostActive,
            ref chestHoldFrontLeftGraspHasOriginal,
            ref chestHoldFrontLeftGraspOriginal,
            ref chestHoldFrontLeftGraspRestorePendingSerial,
            ref chestHoldFrontLeftGraspRestoreDueTime,
            chestHoldFrontLeftGraspTargets
        );
    }

    private void BeginChestHoldFrontRightGraspRun(string reason)
    {
        BeginChestHoldFrontHandGraspRun(
            true,
            reason,
            ref chestHoldFrontRightGraspRunSerial,
            ref chestHoldFrontRightGraspBoostedSerial,
            ref chestHoldFrontRightGraspRestoredSerial,
            ref chestHoldFrontRightGraspBoostActive,
            ref chestHoldFrontRightGraspHasOriginal,
            ref chestHoldFrontRightGraspOriginal,
            ref chestHoldFrontRightGraspRestorePendingSerial,
            ref chestHoldFrontRightGraspRestoreDueTime,
            chestHoldFrontRightGraspTargets
        );
    }

    private void BeginChestHoldFrontHandGraspRun(bool rightHand, string reason, ref int runSerial, ref int boostedSerial, ref int restoredSerial, ref bool boostActive, ref bool hasOriginal, ref float original, ref int restorePendingSerial, ref float restoreDueTime, List<HandMorphTarget> targets)
    {
        // v115: Do not restore at Grab start. If a previous boost is still active, keep the
        // original baseline, but do NOT mark this new serial as boosted yet. MaybeBoost will
        // re-check the actual morph value after step2 and re-apply the desired boost if VaM,
        // defaults, or another plugin reset the morph value, then schedule a fresh delayed restore.
        bool carryActiveBoost = boostActive && hasOriginal;

        runSerial++;
        int serial = runSerial;
        restoredSerial = -1;
        boostedSerial = -1;
        restorePendingSerial = -1;
        restoreDueTime = -999.0f;

        if (carryActiveBoost)
        {
            LogChestHoldFrontHandGrasp(rightHand, "carry / serial=" + serial.ToString(CultureInfo.InvariantCulture) +
                " / reason=" + (reason ?? "") +
                " / original=" + original.ToString("F3", CultureInfo.InvariantCulture) +
                " / willEnsureAfterStep2=1" +
                " / restoreDelay=" + CHEST_HOLD_FRONT_LEFT_GRASP_RESTORE_DELAY.ToString("F3", CultureInfo.InvariantCulture));
            return;
        }

        boostActive = false;
        hasOriginal = false;
        original = 0.0f;
        if (targets != null)
            targets.Clear();
    }

    private void MaybeBoostChestHoldFrontLeftGrasp(bool immediate)
    {
        chestHoldLeftGraspRouteLabel = "FRONT";
        MaybeBoostChestHoldFrontHandGrasp(
            false,
            immediate,
            chestHoldFrontLeftGraspTargets,
            ref chestHoldFrontLeftGraspRunSerial,
            ref chestHoldFrontLeftGraspBoostedSerial,
            ref chestHoldFrontLeftGraspRestoredSerial,
            ref chestHoldFrontLeftGraspBoostActive,
            ref chestHoldFrontLeftGraspHasOriginal,
            ref chestHoldFrontLeftGraspOriginal,
            ref chestHoldFrontLeftGraspRestorePendingSerial,
            ref chestHoldFrontLeftGraspRestoreDueTime
        );
    }

    private void MaybeBoostChestHoldFrontRightGrasp(bool immediate)
    {
        chestHoldRightGraspRouteLabel = "FRONT";
        MaybeBoostChestHoldFrontHandGrasp(
            true,
            immediate,
            chestHoldFrontRightGraspTargets,
            ref chestHoldFrontRightGraspRunSerial,
            ref chestHoldFrontRightGraspBoostedSerial,
            ref chestHoldFrontRightGraspRestoredSerial,
            ref chestHoldFrontRightGraspBoostActive,
            ref chestHoldFrontRightGraspHasOriginal,
            ref chestHoldFrontRightGraspOriginal,
            ref chestHoldFrontRightGraspRestorePendingSerial,
            ref chestHoldFrontRightGraspRestoreDueTime
        );
    }

    private void MaybeBoostChestHoldBackLeftGrasp(bool immediate)
    {
        chestHoldLeftGraspRouteLabel = "BACK";
        MaybeBoostChestHoldFrontHandGrasp(
            false,
            immediate,
            chestHoldFrontLeftGraspTargets,
            ref chestHoldFrontLeftGraspRunSerial,
            ref chestHoldFrontLeftGraspBoostedSerial,
            ref chestHoldFrontLeftGraspRestoredSerial,
            ref chestHoldFrontLeftGraspBoostActive,
            ref chestHoldFrontLeftGraspHasOriginal,
            ref chestHoldFrontLeftGraspOriginal,
            ref chestHoldFrontLeftGraspRestorePendingSerial,
            ref chestHoldFrontLeftGraspRestoreDueTime
        );
    }

    private void MaybeBoostChestHoldBackRightGrasp(bool immediate)
    {
        chestHoldRightGraspRouteLabel = "BACK";
        MaybeBoostChestHoldFrontHandGrasp(
            true,
            immediate,
            chestHoldFrontRightGraspTargets,
            ref chestHoldFrontRightGraspRunSerial,
            ref chestHoldFrontRightGraspBoostedSerial,
            ref chestHoldFrontRightGraspRestoredSerial,
            ref chestHoldFrontRightGraspBoostActive,
            ref chestHoldFrontRightGraspHasOriginal,
            ref chestHoldFrontRightGraspOriginal,
            ref chestHoldFrontRightGraspRestorePendingSerial,
            ref chestHoldFrontRightGraspRestoreDueTime
        );
    }

    private void MaybeBoostChestHoldFrontHandGrasp(bool rightHand, bool immediate, List<HandMorphTarget> targets, ref int runSerial, ref int boostedSerial, ref int restoredSerial, ref bool boostActive, ref bool hasOriginal, ref float original, ref int restorePendingSerial, ref float restoreDueTime)
    {
        int serial = runSerial;
        if (serial <= 0) return;
        if (boostedSerial == serial) return;
        if (restoredSerial == serial) return; // legacy guard; v111 does not restore automatically.

        float t = immediate ? 1.0f : GetMoveTLinear();
        if (t < CHEST_HOLD_FRONT_STEP2_SWITCH_T)
            return;

        ResolveChestHoldFrontHandGraspTargets(rightHand, targets);
        if (targets == null || targets.Count == 0)
        {
            LogChestHoldFrontHandGrasp(rightHand, "boost-miss / serial=" + serial.ToString(CultureInfo.InvariantCulture) +
                " / reason=no-target / t=" + t.ToString("F3", CultureInfo.InvariantCulture));
            return;
        }

        float add = rightHand ? CHEST_HOLD_FRONT_RIGHT_GRASP_BOOST : CHEST_HOLD_FRONT_LEFT_GRASP_BOOST;

        if (boostActive && hasOriginal)
        {
            // v111: restoreOnRelease carry means the morph should remain boosted, but in practice VaM
            // defaults, pose loads, or another plugin can reset the visible morph while our guard
            // is still active. Re-apply original+add without stacking.
            float desired = Mathf.Clamp01(original + add);
            float current = ReadPreferredHandMorphValue(targets, original);
            ApplyHandMorphTargets(targets, desired, "ensure-" + (rightHand ? "R" : "L"), rightHand);
            ArmChestHoldFrontHandGraspWatch(rightHand, serial, desired);
            ScheduleChestHoldFrontHandGraspDelayedRestore(rightHand, serial, ref restorePendingSerial, ref restoreDueTime, "ensure");
            boostedSerial = serial;
            LogChestHoldFrontHandGrasp(rightHand, "ensure / serial=" + serial.ToString(CultureInfo.InvariantCulture) +
                " / t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
                " / original=" + original.ToString("F3", CultureInfo.InvariantCulture) +
                " / add=" + add.ToString("F3", CultureInfo.InvariantCulture) +
                " / current=" + current.ToString("F3", CultureInfo.InvariantCulture) +
                " / desired=" + desired.ToString("F3", CultureInfo.InvariantCulture) +
                " / targets=" + targets.Count.ToString(CultureInfo.InvariantCulture) +
                " / atom=" + (selectedPerson != null ? selectedPerson.uid : containingAtom != null ? containingAtom.uid : "<null>") +
                " / first=" + GetFirstHandMorphTargetLabel(targets) +
                " / delayedRestore=1");
            return;
        }

        original = ReadPreferredHandMorphValue(targets, 0.0f);
        hasOriginal = true;
        float next = Mathf.Clamp01(original + add);
        ApplyHandMorphTargets(targets, next, "boost-" + (rightHand ? "R" : "L"), rightHand);
        ArmChestHoldFrontHandGraspWatch(rightHand, serial, next);
        ScheduleChestHoldFrontHandGraspDelayedRestore(rightHand, serial, ref restorePendingSerial, ref restoreDueTime, "boost");
        boostActive = true;
        boostedSerial = serial;

        LogChestHoldFrontHandGrasp(rightHand, "boost / serial=" + serial.ToString(CultureInfo.InvariantCulture) +
            " / t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
            " / original=" + original.ToString("F3", CultureInfo.InvariantCulture) +
            " / add=" + add.ToString("F3", CultureInfo.InvariantCulture) +
            " / next=" + next.ToString("F3", CultureInfo.InvariantCulture) +
            " / targets=" + targets.Count.ToString(CultureInfo.InvariantCulture) +
            " / atom=" + (selectedPerson != null ? selectedPerson.uid : containingAtom != null ? containingAtom.uid : "<null>") +
            " / first=" + GetFirstHandMorphTargetLabel(targets) +
            " / delayedRestore=1");
    }

    private void MaybeRestoreChestHoldFrontLeftGraspAfterWrist(bool immediate)
    {
        // v115: restore is scheduled by MaybeBoost/Ensure after step2. Do not reschedule every wrist/final frame.
    }

    private void MaybeRestoreChestHoldFrontRightGraspAfterWrist(bool immediate)
    {
        // v115: restore is scheduled by MaybeBoost/Ensure after step2. Do not reschedule every wrist/final frame.
    }

    private void ScheduleChestHoldFrontHandGraspDelayedRestore(bool rightHand, int serial, ref int restorePendingSerial, ref float restoreDueTime, string reason)
    {
        if (serial <= 0) return;

        restorePendingSerial = serial;
        restoreDueTime = Time.time + CHEST_HOLD_FRONT_LEFT_GRASP_RESTORE_DELAY;

        LogChestHoldFrontHandGrasp(rightHand, "restore-scheduled / serial=" + serial.ToString(CultureInfo.InvariantCulture) +
            " / reason=" + (reason ?? "") +
            " / dueIn=" + CHEST_HOLD_FRONT_LEFT_GRASP_RESTORE_DELAY.ToString("F3", CultureInfo.InvariantCulture) +
            " / dueTime=" + restoreDueTime.ToString("F3", CultureInfo.InvariantCulture));
    }

    private void UpdateChestHoldFrontLeftGraspDelayedRestore()
    {
        UpdateChestHoldFrontHandGraspDelayedRestore(
            false,
            ref chestHoldFrontLeftGraspRestorePendingSerial,
            ref chestHoldFrontLeftGraspRestoreDueTime,
            ref chestHoldFrontLeftGraspRunSerial,
            ref chestHoldFrontLeftGraspBoostedSerial,
            ref chestHoldFrontLeftGraspRestoredSerial,
            ref chestHoldFrontLeftGraspBoostActive,
            ref chestHoldFrontLeftGraspHasOriginal,
            ref chestHoldFrontLeftGraspOriginal,
            ref chestHoldFrontLeftGraspWatchSerial,
            ref chestHoldFrontLeftGraspWatchFrames,
            chestHoldFrontLeftGraspTargets
        );
    }

    private void UpdateChestHoldFrontRightGraspDelayedRestore()
    {
        UpdateChestHoldFrontHandGraspDelayedRestore(
            true,
            ref chestHoldFrontRightGraspRestorePendingSerial,
            ref chestHoldFrontRightGraspRestoreDueTime,
            ref chestHoldFrontRightGraspRunSerial,
            ref chestHoldFrontRightGraspBoostedSerial,
            ref chestHoldFrontRightGraspRestoredSerial,
            ref chestHoldFrontRightGraspBoostActive,
            ref chestHoldFrontRightGraspHasOriginal,
            ref chestHoldFrontRightGraspOriginal,
            ref chestHoldFrontRightGraspWatchSerial,
            ref chestHoldFrontRightGraspWatchFrames,
            chestHoldFrontRightGraspTargets
        );
    }

    private void UpdateChestHoldFrontHandGraspDelayedRestore(bool rightHand, ref int restorePendingSerial, ref float restoreDueTime, ref int runSerial, ref int boostedSerial, ref int restoredSerial, ref bool boostActive, ref bool hasOriginal, ref float original, ref int watchSerial, ref int watchFrames, List<HandMorphTarget> targets)
    {
        if (restorePendingSerial <= 0) return;
        if (restoreDueTime <= -100.0f)
        {
            restorePendingSerial = -1;
            return;
        }
        if (Time.time < restoreDueTime) return;

        int pendingSerial = restorePendingSerial;
        RestoreChestHoldFrontHandGraspBoost(
            rightHand,
            "delayed-1s",
            ref runSerial,
            ref boostedSerial,
            ref restoredSerial,
            ref boostActive,
            ref hasOriginal,
            ref original,
            ref restorePendingSerial,
            ref restoreDueTime,
            ref watchSerial,
            ref watchFrames,
            targets
        );

        LogChestHoldFrontHandGrasp(rightHand, "delayed-restore-fired / serial=" + pendingSerial.ToString(CultureInfo.InvariantCulture));
    }

    private void ArmChestHoldFrontHandGraspWatch(bool rightHand, int serial, float desired)
    {
        if (rightHand)
        {
            chestHoldFrontRightGraspWatchSerial = serial;
            chestHoldFrontRightGraspWatchFrames = 20;
            chestHoldFrontRightGraspWatchDesired = Mathf.Clamp01(desired);
            chestHoldFrontRightGraspWatchLast = -999.0f;
        }
        else
        {
            chestHoldFrontLeftGraspWatchSerial = serial;
            chestHoldFrontLeftGraspWatchFrames = 20;
            chestHoldFrontLeftGraspWatchDesired = Mathf.Clamp01(desired);
            chestHoldFrontLeftGraspWatchLast = -999.0f;
        }
    }

    private void UpdateChestHoldFrontLeftGraspWatch()
    {
        UpdateChestHoldFrontHandGraspWatch(
            false,
            ref chestHoldFrontLeftGraspWatchSerial,
            ref chestHoldFrontLeftGraspWatchFrames,
            ref chestHoldFrontLeftGraspWatchDesired,
            ref chestHoldFrontLeftGraspWatchLast,
            chestHoldFrontLeftGraspTargets
        );
    }

    private void UpdateChestHoldFrontRightGraspWatch()
    {
        UpdateChestHoldFrontHandGraspWatch(
            true,
            ref chestHoldFrontRightGraspWatchSerial,
            ref chestHoldFrontRightGraspWatchFrames,
            ref chestHoldFrontRightGraspWatchDesired,
            ref chestHoldFrontRightGraspWatchLast,
            chestHoldFrontRightGraspTargets
        );
    }

    private void UpdateChestHoldFrontHandGraspWatch(bool rightHand, ref int watchSerial, ref int watchFrames, ref float watchDesired, ref float watchLast, List<HandMorphTarget> targets)
    {
        if (watchFrames <= 0) return;
        if (watchSerial <= 0)
        {
            watchFrames = 0;
            return;
        }

        ResolveChestHoldFrontHandGraspTargets(rightHand, targets);
        float current = ReadPreferredHandMorphValue(targets, -1.0f);
        bool changed = Mathf.Abs(current - watchLast) > 0.001f;
        bool milestone = watchFrames == 20 || watchFrames == 15 || watchFrames == 10 || watchFrames == 5 || watchFrames == 1;
        if (changed || milestone)
        {
            LogChestHoldFrontHandGrasp(rightHand, "watch / serial=" + watchSerial.ToString(CultureInfo.InvariantCulture) +
                " / framesLeft=" + watchFrames.ToString(CultureInfo.InvariantCulture) +
                " / current=" + current.ToString("F3", CultureInfo.InvariantCulture) +
                " / desired=" + watchDesired.ToString("F3", CultureInfo.InvariantCulture) +
                " / delta=" + (current - watchDesired).ToString("F3", CultureInfo.InvariantCulture) +
                " / targets=" + (targets != null ? targets.Count : 0).ToString(CultureInfo.InvariantCulture) +
                " / first=" + GetFirstHandMorphTargetLabel(targets));
            watchLast = current;
        }

        watchFrames--;
        if (watchFrames <= 0)
        {
            LogChestHoldFrontHandGrasp(rightHand, "watch-end / serial=" + watchSerial.ToString(CultureInfo.InvariantCulture) +
                " / current=" + current.ToString("F3", CultureInfo.InvariantCulture) +
                " / desired=" + watchDesired.ToString("F3", CultureInfo.InvariantCulture) +
                " / overwritten=" + Bool01(Mathf.Abs(current - watchDesired) > 0.001f));
        }
    }

    private void RestoreChestHoldFrontLeftGraspBoost(string reason)
    {
        RestoreChestHoldFrontHandGraspBoost(
            false,
            reason,
            ref chestHoldFrontLeftGraspRunSerial,
            ref chestHoldFrontLeftGraspBoostedSerial,
            ref chestHoldFrontLeftGraspRestoredSerial,
            ref chestHoldFrontLeftGraspBoostActive,
            ref chestHoldFrontLeftGraspHasOriginal,
            ref chestHoldFrontLeftGraspOriginal,
            ref chestHoldFrontLeftGraspRestorePendingSerial,
            ref chestHoldFrontLeftGraspRestoreDueTime,
            ref chestHoldFrontLeftGraspWatchSerial,
            ref chestHoldFrontLeftGraspWatchFrames,
            chestHoldFrontLeftGraspTargets
        );
    }

    private void RestoreChestHoldFrontRightGraspBoost(string reason)
    {
        RestoreChestHoldFrontHandGraspBoost(
            true,
            reason,
            ref chestHoldFrontRightGraspRunSerial,
            ref chestHoldFrontRightGraspBoostedSerial,
            ref chestHoldFrontRightGraspRestoredSerial,
            ref chestHoldFrontRightGraspBoostActive,
            ref chestHoldFrontRightGraspHasOriginal,
            ref chestHoldFrontRightGraspOriginal,
            ref chestHoldFrontRightGraspRestorePendingSerial,
            ref chestHoldFrontRightGraspRestoreDueTime,
            ref chestHoldFrontRightGraspWatchSerial,
            ref chestHoldFrontRightGraspWatchFrames,
            chestHoldFrontRightGraspTargets
        );
    }

    private void RestoreChestHoldFrontHandGraspBoost(bool rightHand, string reason, ref int runSerial, ref int boostedSerial, ref int restoredSerial, ref bool boostActive, ref bool hasOriginal, ref float original, ref int restorePendingSerial, ref float restoreDueTime, ref int watchSerial, ref int watchFrames, List<HandMorphTarget> targets)
    {
        int serial = runSerial;

        if (!boostActive && !hasOriginal)
        {
            LogChestHoldFrontHandGrasp(rightHand, "restore-skip / reason=" + (reason ?? "") + " / reason2=no-active-boost / delayedRestore=1");
            return;
        }

        if (!hasOriginal)
        {
            boostActive = false;
            restorePendingSerial = -1;
            restoreDueTime = -999.0f;
            watchSerial = -1;
            watchFrames = 0;
            if (targets != null) targets.Clear();
            LogChestHoldFrontHandGrasp(rightHand, "restore-skip / reason=" + (reason ?? "") + " / reason2=no-original / delayedRestore=1");
            return;
        }

        float restore = Mathf.Clamp01(original);
        ResolveChestHoldFrontHandGraspTargets(rightHand, targets);
        ApplyHandMorphTargets(targets, restore, "restore-" + (reason ?? ""), rightHand);

        LogChestHoldFrontHandGrasp(rightHand, "restore / serial=" + serial.ToString(CultureInfo.InvariantCulture) +
            " / reason=" + (reason ?? "") +
            " / value=" + restore.ToString("F3", CultureInfo.InvariantCulture) +
            " / targets=" + (targets != null ? targets.Count : 0).ToString(CultureInfo.InvariantCulture) +
            " / atom=" + (selectedPerson != null ? selectedPerson.uid : containingAtom != null ? containingAtom.uid : "<null>") +
            " / first=" + GetFirstHandMorphTargetLabel(targets) +
            " / delayedRestore=1");

        boostActive = false;
        hasOriginal = false;
        original = 0.0f;
        restorePendingSerial = -1;
        restoreDueTime = -999.0f;
        watchSerial = -1;
        watchFrames = 0;
        if (serial > 0)
            restoredSerial = serial;
        if (targets != null)
            targets.Clear();
    }

    private void ResetChestHoldFrontLeftGraspBoostState(string reason)
    {
        ResetChestHoldFrontHandGraspBoostState(
            false,
            reason,
            ref chestHoldFrontLeftGraspBoostActive,
            ref chestHoldFrontLeftGraspHasOriginal,
            ref chestHoldFrontLeftGraspRestorePendingSerial,
            ref chestHoldFrontLeftGraspRestoreDueTime,
            chestHoldFrontLeftGraspTargets
        );
    }

    private void ResetChestHoldFrontRightGraspBoostState(string reason)
    {
        ResetChestHoldFrontHandGraspBoostState(
            true,
            reason,
            ref chestHoldFrontRightGraspBoostActive,
            ref chestHoldFrontRightGraspHasOriginal,
            ref chestHoldFrontRightGraspRestorePendingSerial,
            ref chestHoldFrontRightGraspRestoreDueTime,
            chestHoldFrontRightGraspTargets
        );
    }

    private void ResetChestHoldFrontHandGraspBoostState(bool rightHand, string reason, ref bool boostActive, ref bool hasOriginal, ref int restorePendingSerial, ref float restoreDueTime, List<HandMorphTarget> targets)
    {
        // v115: normal restore is delayed about 1 second after boost.
        // Release/default buttons still restore immediately if the delayed restore has not fired yet.
        bool restoreForExit = string.Equals(reason, "release", StringComparison.Ordinal) ||
            string.Equals(reason, "target-release", StringComparison.Ordinal) ||
            string.Equals(reason, "self-load-user-defaults", StringComparison.Ordinal) ||
            string.Equals(reason, "self-ik-default", StringComparison.Ordinal);

        if (restoreForExit)
        {
            if (rightHand)
                RestoreChestHoldFrontRightGraspBoost(reason);
            else
                RestoreChestHoldFrontLeftGraspBoost(reason);
            return;
        }

        restorePendingSerial = -1;
        restoreDueTime = -999.0f;
        LogChestHoldFrontHandGrasp(rightHand, "reset-state / reason=" + (reason ?? "") + " / carry=1 / restore=0 / delayedRestore=1");
    }

    private void ResolveChestHoldFrontLeftGraspTargets()
    {
        ResolveChestHoldFrontHandGraspTargets(false, chestHoldFrontLeftGraspTargets);
    }

    private void ResolveChestHoldFrontRightGraspTargets()
    {
        ResolveChestHoldFrontHandGraspTargets(true, chestHoldFrontRightGraspTargets);
    }

    private void ResolveChestHoldFrontHandGraspTargets(bool rightHand, List<HandMorphTarget> targets)
    {
        if (targets == null) return;
        targets.Clear();

        Atom atom = selectedPerson != null ? selectedPerson : containingAtom;
        string[] names = rightHand ? chestHoldRightHandGraspNames : chestHoldLeftHandGraspNames;
        // v105/v110: do NOT scan all storables here. If HandMorphTester is installed on the same Person,
        // scanning all storables finds its slider JSONStorableFloat and SetVal triggers tester callbacks.
        // Chest Hold should write only the actual geometry hand-grasp controls.
        AddHandMorphJSONTargets(atom, names, targets, false);
        AddHandMorphDAZMorphTargets(atom, names, targets);
    }

    private void AddHandMorphJSONTargets(Atom atom, string[] names, List<HandMorphTarget> output, bool scanAllStorables)
    {
        if (atom == null || names == null || output == null) return;

        JSONStorable geometry = null;
        try { geometry = atom.GetStorableByID("geometry"); } catch { geometry = null; }
        AddHandMorphJSONTargetsFromStorable(geometry, "geometry", names, output);

        if (!scanAllStorables) return;

        List<string> ids = null;
        try { ids = atom.GetStorableIDs(); } catch { ids = null; }
        if (ids == null) return;

        for (int i = 0; i < ids.Count; i++)
        {
            string sid = ids[i];
            if (string.IsNullOrEmpty(sid)) continue;
            if (sid == "geometry") continue;

            JSONStorable st = null;
            try { st = atom.GetStorableByID(sid); } catch { st = null; }
            if (st == null || st == this) continue;
            AddHandMorphJSONTargetsFromStorable(st, sid, names, output);
        }
    }

    private void AddHandMorphJSONTargetsFromStorable(JSONStorable storable, string storableId, string[] names, List<HandMorphTarget> output)
    {
        if (storable == null || names == null || output == null) return;

        for (int n = 0; n < names.Length; n++)
        {
            string name = names[n];
            if (string.IsNullOrEmpty(name)) continue;

            JSONStorableFloat f = null;
            try { f = storable.GetFloatJSONParam(name); } catch { f = null; }
            if (f == null) continue;
            if (ContainsHandMorphJSONTarget(output, f)) continue;

            HandMorphTarget target = new HandMorphTarget();
            target.label = (storableId ?? "") + ":" + name;
            target.jsonFloat = f;
            output.Add(target);
        }
    }

    private void AddHandMorphDAZMorphTargets(Atom atom, string[] names, List<HandMorphTarget> output)
    {
        if (atom == null || names == null || output == null) return;

        DAZCharacterSelector dcs = null;
        try { dcs = atom.GetStorableByID("geometry") as DAZCharacterSelector; } catch { dcs = null; }
        if (dcs == null || dcs.morphsControlUI == null) return;

        GenerateDAZMorphsControlUI morphUI = dcs.morphsControlUI;
        for (int n = 0; n < names.Length; n++)
        {
            string name = names[n];
            if (string.IsNullOrEmpty(name)) continue;

            DAZMorph morph = null;
            try { morph = morphUI.GetMorphByDisplayName(name); } catch { morph = null; }
            if (morph == null) continue;
            if (ContainsHandMorphDAZMorphTarget(output, morph)) continue;

            HandMorphTarget target = new HandMorphTarget();
            target.label = "DAZMorph:" + name;
            target.morph = morph;
            output.Add(target);
        }
    }

    private bool ContainsHandMorphJSONTarget(List<HandMorphTarget> list, JSONStorableFloat f)
    {
        if (list == null || f == null) return false;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null && list[i].jsonFloat == f) return true;
        }
        return false;
    }

    private bool ContainsHandMorphDAZMorphTarget(List<HandMorphTarget> list, DAZMorph morph)
    {
        if (list == null || morph == null) return false;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null && list[i].morph == morph) return true;
        }
        return false;
    }

    private float ReadPreferredHandMorphValue(List<HandMorphTarget> targets, float fallback)
    {
        if (targets == null || targets.Count == 0) return fallback;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null && targets[i].jsonFloat != null)
                return Mathf.Clamp01(targets[i].jsonFloat.val);
        }

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null && targets[i].morph != null)
                return Mathf.Clamp01(targets[i].morph.morphValue);
        }

        return fallback;
    }

    private void ApplyHandMorphTargets(List<HandMorphTarget> targets, float value, string reason)
    {
        ApplyHandMorphTargets(targets, value, reason, false);
    }

    private void ApplyHandMorphTargets(List<HandMorphTarget> targets, float value, string reason, bool rightHand)
    {
        if (targets == null) return;
        float v = Mathf.Clamp01(value);
        for (int i = 0; i < targets.Count; i++)
        {
            HandMorphTarget target = targets[i];
            if (target == null) continue;

            float before = 0.0f;
            float after = 0.0f;
            bool ok = true;
            try { before = target.ReadValue(); } catch { before = -999.0f; ok = false; }
            try { target.WriteValue(v); } catch { ok = false; }
            try { after = target.ReadValue(); } catch { after = -999.0f; ok = false; }

            // v105/v110: normal one-shot diagnostic for this temporary Front grasp experiment.
            // No reflection/GetType; type is determined from stored fields only.
            LogChestHoldFrontHandGrasp(rightHand, "target / reason=" + (reason ?? "") +
                " / index=" + i.ToString(CultureInfo.InvariantCulture) +
                " / label=" + SafeHandMorphLabel(target.label) +
                " / type=" + (target.IsJSON() ? "JSONStorableFloat" : "DAZMorph") +
                " / before=" + before.ToString("F3", CultureInfo.InvariantCulture) +
                " / requested=" + v.ToString("F3", CultureInfo.InvariantCulture) +
                " / after=" + after.ToString("F3", CultureInfo.InvariantCulture) +
                " / ok=" + Bool01(ok));
        }
    }

    private string GetFirstHandMorphTargetLabel(List<HandMorphTarget> targets)
    {
        if (targets == null || targets.Count == 0) return "none";
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null && !string.IsNullOrEmpty(targets[i].label))
                return targets[i].label.Replace("\n", " ").Replace("\r", " ");
        }
        return "none";
    }

    private string SafeHandMorphLabel(string label)
    {
        if (string.IsNullOrEmpty(label)) return "";
        return label.Replace("\n", " ").Replace("\r", " ");
    }

    private void LogChestHoldFrontLeftGrasp(string message)
    {
        LogChestHoldFrontHandGrasp(false, message);
    }

    private void LogChestHoldFrontRightGrasp(string message)
    {
        LogChestHoldFrontHandGrasp(true, message);
    }

    private void LogChestHoldFrontHandGrasp(bool rightHand, string message)
    {
        // v117 FIX: Chest Hold Grasp diagnostics are noisy during normal use.
        // Keep them available only when Debug Log is ON.
        if (debugLogJSON == null || !debugLogJSON.val) return;

        string route = rightHand ? chestHoldRightGraspRouteLabel : chestHoldLeftGraspRouteLabel;
        if (string.IsNullOrEmpty(route)) route = "FRONT";
        SuperController.LogMessage("[TargetGrabber] [CHEST HOLD " + route + " " + (rightHand ? "RIGHT" : "LEFT") + " GRASP] " + (message ?? ""));
    }

    private int ApplyChestHoldFrontSimpleCrossHandGrab(bool immediate, Vector3 targetLeftNipple, Vector3 targetRightNipple, Vector3 center, Vector3 side)
    {
        int moved = 0;

        Vector3 fallbackFrontAxis = Vector3.forward;
        if (selectedTargetPerson != null && selectedTargetPerson.transform != null)
            fallbackFrontAxis = selectedTargetPerson.transform.forward;
        fallbackFrontAxis.y = 0.0f;
        if (fallbackFrontAxis.sqrMagnitude < 0.0001f)
            fallbackFrontAxis = Vector3.forward;
        fallbackFrontAxis.Normalize();

        Vector3 step2DownOffset = Vector3.down * CHEST_HOLD_FRONT_SIMPLE_STEP2_DOWN;
        Vector3 finalDownOffset = Vector3.down * CHEST_HOLD_FRONT_SIMPLE_FINAL_DOWN;

        // v93 Front contract:
        // R hand -> target left nipple. L hand -> target right nipple.
        // step2: assigned nipple + 13cm down + 1cm stable hand-forward + 5cm inward.
        // step3/final: assigned nipple + 11cm down + 6cm stable hand-forward + 3cm inward.
        // "front" is stable self-body/chest anchor -> assigned nipple route.
        // It must not use current hand IK position, or repeated Grab changes the axis.
        Vector3 frontNippleSideAxis = GetChestHoldNippleSideAxis(targetLeftNipple, targetRightNipple, side);
        Vector3 rightStep2Inward = frontNippleSideAxis * CHEST_HOLD_FRONT_SIMPLE_STEP2_INWARD;   // target left nipple -> pair center/right nipple
        Vector3 leftStep2Inward = -frontNippleSideAxis * CHEST_HOLD_FRONT_SIMPLE_STEP2_INWARD;   // target right nipple -> pair center/left nipple
        Vector3 rightFinalInward = frontNippleSideAxis * CHEST_HOLD_FRONT_SIMPLE_FINAL_INWARD;   // target left nipple -> pair center/right nipple
        Vector3 leftFinalInward = -frontNippleSideAxis * CHEST_HOLD_FRONT_SIMPLE_FINAL_INWARD;   // target right nipple -> pair center/left nipple

        Vector3 leftStep2Base = targetRightNipple + step2DownOffset + leftStep2Inward;
        Vector3 leftFinalBase = targetRightNipple + finalDownOffset + leftFinalInward;
        Vector3 rightStep2Base = targetLeftNipple + step2DownOffset + rightStep2Inward;
        Vector3 rightFinalBase = targetLeftNipple + finalDownOffset + rightFinalInward;

        Vector3 leftFrontAxis = GetChestHoldFrontHandForwardAxis(false, leftFinalBase, fallbackFrontAxis);
        Vector3 rightFrontAxis = GetChestHoldFrontHandForwardAxis(true, rightFinalBase, fallbackFrontAxis);

        Vector3 leftStep2 = leftStep2Base + leftFrontAxis * CHEST_HOLD_FRONT_SIMPLE_STEP2_FORWARD;
        Vector3 leftFinal = leftFinalBase + leftFrontAxis * CHEST_HOLD_FRONT_SIMPLE_FINAL_FORWARD;
        Vector3 rightStep2 = rightStep2Base + rightFrontAxis * CHEST_HOLD_FRONT_SIMPLE_STEP2_FORWARD;
        Vector3 rightFinal = rightFinalBase + rightFrontAxis * CHEST_HOLD_FRONT_SIMPLE_FINAL_FORWARD;

        if (IsDebugEnabled())
        {
            DebugLog("[CHEST HOLD FRONT SIMPLE] assign=R->targetL/L->targetR / route=stable-body-axis-step2-down13-front6-in5-step3-down11-front1-in3-follow3s" +
                " / center=" + FormatVector3(center) +
                " / targetL=" + FormatVector3(targetLeftNipple) +
                " / targetR=" + FormatVector3(targetRightNipple) +
                " / side=" + FormatVector3(side) +
                " / frontNippleSideAxis=" + FormatVector3(frontNippleSideAxis) +
                " / rightStep2Inward=" + FormatVector3(rightStep2Inward) +
                " / leftStep2Inward=" + FormatVector3(leftStep2Inward) +
                " / rightFinalInward=" + FormatVector3(rightFinalInward) +
                " / leftFinalInward=" + FormatVector3(leftFinalInward) +
                " / axisSource=self-body-anchor / fallbackFrontAxis=" + FormatVector3(fallbackFrontAxis) +
                " / rightFrontAxis=" + FormatVector3(rightFrontAxis) +
                " / leftFrontAxis=" + FormatVector3(leftFrontAxis) +
                " / step2Down=" + CHEST_HOLD_FRONT_SIMPLE_STEP2_DOWN.ToString("F3", CultureInfo.InvariantCulture) +
                " / finalDown=" + CHEST_HOLD_FRONT_SIMPLE_FINAL_DOWN.ToString("F3", CultureInfo.InvariantCulture) +
                " / step2Inward=" + CHEST_HOLD_FRONT_SIMPLE_STEP2_INWARD.ToString("F3", CultureInfo.InvariantCulture) +
                " / finalInward=" + CHEST_HOLD_FRONT_SIMPLE_FINAL_INWARD.ToString("F3", CultureInfo.InvariantCulture) +
                " / step2Front=" + CHEST_HOLD_FRONT_SIMPLE_STEP2_FORWARD.ToString("F3", CultureInfo.InvariantCulture) +
                " / finalFront=" + CHEST_HOLD_FRONT_SIMPLE_FINAL_FORWARD.ToString("F3", CultureInfo.InvariantCulture) +
                " / rightStep2=" + FormatVector3(rightStep2) +
                " / rightFinal=" + FormatVector3(rightFinal) +
                " / leftStep2=" + FormatVector3(leftStep2) +
                " / leftFinal=" + FormatVector3(leftFinal));
        }

        bool leftFollowActive = false;
        bool rightFollowActive = false;

        // L hand goes to target right nipple route.
        if (leftHandJSON != null && leftHandJSON.val && lHandControl != null)
        {
            Vector3 leftRoot = GetHandRootPosition(false);
            LogHoldHandTarget("Chest Hold", "front-simple-cross", false, leftFinal, leftStep2, leftFinal, leftRoot, side, true, center, immediate);
            MoveChestHoldBackStep2Step3Control(lHandControl, leftStep2, leftFinal, immediate);
            MaybeBoostChestHoldFrontLeftGrasp(immediate);
            ApplyChestHoldFrontUpBackInWrist(lHandControl, leftFinal, false, immediate, true);
            MaybeRestoreChestHoldFrontLeftGraspAfterWrist(immediate);
            LogChestHoldFinalHandNipplePosition(lHandControl, leftFinal, leftFinal, false, immediate, true);
            leftFollowActive = true;
            moved++;
        }

        // R hand goes to target left nipple route.
        if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
        {
            Vector3 rightRoot = GetHandRootPosition(true);
            LogHoldHandTarget("Chest Hold", "front-simple-cross", true, rightFinal, rightStep2, rightFinal, rightRoot, side, false, center, immediate);
            MoveChestHoldBackStep2Step3Control(rHandControl, rightStep2, rightFinal, immediate);
            MaybeBoostChestHoldFrontRightGrasp(immediate);
            ApplyChestHoldFrontUpBackInWrist(rHandControl, rightFinal, true, immediate, true);
            MaybeRestoreChestHoldFrontRightGraspAfterWrist(immediate);
            LogChestHoldFinalHandNipplePosition(rHandControl, rightFinal, rightFinal, true, immediate, true);
            rightFollowActive = true;
            moved++;
        }

        if (moved > 0)
            ArmChestHoldNippleHandFollow(leftFollowActive, rightFollowActive, leftFinal, rightFinal, immediate);

        return moved;
    }

    private void ArmChestHoldNippleHandFollow(bool leftActive, bool rightActive, Vector3 leftFinal, Vector3 rightFinal, bool immediate)
    {
        FreeControllerV3 targetLeftNippleControl;
        FreeControllerV3 targetRightNippleControl;
        List<FreeControllerV3> nippleControls;
        if (!TryGetChestHoldNippleIKControls(out nippleControls, out targetLeftNippleControl, out targetRightNippleControl))
            return;

        // Front route: L hand follows target R nipple, R hand follows target L nipple.
        ArmChestHoldNippleHandFollow(leftActive, rightActive, targetRightNippleControl, targetLeftNippleControl, leftFinal, rightFinal, immediate, "front");
    }

    private void ArmChestHoldNippleHandFollow(bool leftActive, bool rightActive, FreeControllerV3 leftFollowNippleControl, FreeControllerV3 rightFollowNippleControl, Vector3 leftFinal, Vector3 rightFinal, bool immediate, string route)
    {
        bool any = false;
        if (leftActive && lHandControl != null && leftFollowNippleControl != null)
        {
            chestHoldFollowLeftHand = lHandControl;
            chestHoldFollowLeftNipple = leftFollowNippleControl;
            chestHoldFollowLeftOffset = leftFinal - GetControlPosition(leftFollowNippleControl);
            any = true;
        }
        else
        {
            chestHoldFollowLeftHand = null;
            chestHoldFollowLeftNipple = null;
            chestHoldFollowLeftOffset = Vector3.zero;
        }

        if (rightActive && rHandControl != null && rightFollowNippleControl != null)
        {
            chestHoldFollowRightHand = rHandControl;
            chestHoldFollowRightNipple = rightFollowNippleControl;
            chestHoldFollowRightOffset = rightFinal - GetControlPosition(rightFollowNippleControl);
            any = true;
        }
        else
        {
            chestHoldFollowRightHand = null;
            chestHoldFollowRightNipple = null;
            chestHoldFollowRightOffset = Vector3.zero;
        }

        if (!any)
            return;

        chestHoldNippleHandFollowPending = !immediate;
        chestHoldNippleHandFollowActive = immediate;
        chestHoldNippleHandFollowElapsed = 0.0f;
        chestHoldNippleHandFollowRoute = route ?? "";

        if (IsDebugEnabled())
            DebugLog("[CHEST HOLD NIPPLE HAND FOLLOW] arm / route=" + chestHoldNippleHandFollowRoute +
                " / pending=" + Bool01(chestHoldNippleHandFollowPending) +
                " / active=" + Bool01(chestHoldNippleHandFollowActive) +
                " / seconds=" + CHEST_HOLD_NIPPLE_HAND_FOLLOW_SECONDS.ToString("F2", CultureInfo.InvariantCulture) +
                " / left=" + Bool01(chestHoldFollowLeftHand != null && chestHoldFollowLeftNipple != null) +
                " / right=" + Bool01(chestHoldFollowRightHand != null && chestHoldFollowRightNipple != null));
    }

    private void ActivatePendingChestHoldNippleHandFollow()
    {
        if (!chestHoldNippleHandFollowPending)
            return;

        chestHoldNippleHandFollowPending = false;
        chestHoldNippleHandFollowActive = true;
        chestHoldNippleHandFollowElapsed = 0.0f;

        if (IsDebugEnabled())
            DebugLog("[CHEST HOLD NIPPLE HAND FOLLOW] start / route=" + chestHoldNippleHandFollowRoute + " / seconds=" + CHEST_HOLD_NIPPLE_HAND_FOLLOW_SECONDS.ToString("F2", CultureInfo.InvariantCulture));
    }

    private void ClearChestHoldNippleHandFollow(string reason)
    {
        if (!chestHoldNippleHandFollowPending && !chestHoldNippleHandFollowActive &&
            chestHoldFollowLeftHand == null && chestHoldFollowRightHand == null &&
            chestHoldFollowLeftNipple == null && chestHoldFollowRightNipple == null)
            return;

        string oldRoute = chestHoldNippleHandFollowRoute;

        chestHoldNippleHandFollowPending = false;
        chestHoldNippleHandFollowActive = false;
        chestHoldNippleHandFollowElapsed = 0.0f;
        chestHoldFollowLeftHand = null;
        chestHoldFollowRightHand = null;
        chestHoldFollowLeftNipple = null;
        chestHoldFollowRightNipple = null;
        chestHoldFollowLeftOffset = Vector3.zero;
        chestHoldFollowRightOffset = Vector3.zero;
        chestHoldNippleHandFollowRoute = "";

        if (IsDebugEnabled())
            DebugLog("[CHEST HOLD NIPPLE HAND FOLLOW] clear / route=" + oldRoute + " / reason=" + reason);
    }

    private bool UpdateChestHoldNippleHandFollow()
    {
        if (!chestHoldNippleHandFollowActive)
            return false;

        chestHoldNippleHandFollowElapsed += Time.deltaTime;
        bool any = false;

        if (chestHoldFollowLeftHand != null && chestHoldFollowLeftNipple != null)
        {
            Vector3 next = GetControlPosition(chestHoldFollowLeftNipple) + chestHoldFollowLeftOffset;
            SetControlPositionDirect(chestHoldFollowLeftHand, next);
            any = true;
        }

        if (chestHoldFollowRightHand != null && chestHoldFollowRightNipple != null)
        {
            Vector3 next = GetControlPosition(chestHoldFollowRightNipple) + chestHoldFollowRightOffset;
            SetControlPositionDirect(chestHoldFollowRightHand, next);
            any = true;
        }

        if (!any || chestHoldNippleHandFollowElapsed >= CHEST_HOLD_NIPPLE_HAND_FOLLOW_SECONDS)
            ClearChestHoldNippleHandFollow(any ? "timeout" : "lost-control");

        return any;
    }

    private int ApplyChestHoldMutualFrontExactHandGrab(bool immediate, Vector3 targetLeftNipple, Vector3 targetRightNipple, Vector3 center, Vector3 fallbackSide)
    {
        int moved = 0;
        Vector3 side = GetChestHoldNippleSideAxis(targetLeftNipple, targetRightNipple, fallbackSide);

        if (IsDebugEnabled())
        {
            DebugLog("[CHEST HOLD MUTUAL FRONT ROUTE] assign=L->targetR/R->targetL / exactNipple=1" +
                " / center=" + FormatVector3(center) +
                " / targetL=" + FormatVector3(targetLeftNipple) +
                " / targetR=" + FormatVector3(targetRightNipple) +
                " / side=" + FormatVector3(side));
        }

        if (leftHandJSON != null && leftHandJSON.val && lHandControl != null)
        {
            Vector3 leftTarget = targetRightNipple;
            Vector3 leftRoot = GetHandRootPosition(false);
            LogHoldHandTarget("Chest Hold", "mutual-front-exact", false, leftTarget, leftTarget, leftTarget, leftRoot, side, true, center, immediate);
            MoveChestHoldHandControl(lHandControl, leftTarget, leftTarget, false, immediate, true, center, side, true, leftTarget);
            ApplyChestHoldFrontUpBackInWrist(lHandControl, leftTarget, false, immediate, true);
            LogChestHoldFinalHandNipplePosition(lHandControl, leftTarget, leftTarget, false, immediate, true);
            moved++;
        }

        if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
        {
            Vector3 rightTarget = targetLeftNipple;
            Vector3 rightRoot = GetHandRootPosition(true);
            LogHoldHandTarget("Chest Hold", "mutual-front-exact", true, rightTarget, rightTarget, rightTarget, rightRoot, side, false, center, immediate);
            MoveChestHoldHandControl(rHandControl, rightTarget, rightTarget, true, immediate, true, center, side, false, rightTarget);
            ApplyChestHoldFrontUpBackInWrist(rHandControl, rightTarget, true, immediate, true);
            LogChestHoldFinalHandNipplePosition(rHandControl, rightTarget, rightTarget, true, immediate, true);
            moved++;
        }

        return moved;
    }

    private bool TryGetAssignedNippleTargets(out Vector3 leftSideTarget, out Vector3 rightSideTarget, out string mode)
    {
        leftSideTarget = Vector3.zero;
        rightSideTarget = Vector3.zero;
        mode = "none";

        if (selectedPerson == null || selectedTargetPerson == null)
            return false;

        Vector3 targetLeftNipple;
        Vector3 targetRightNipple;
        if (!TryGetTargetNipplePositions(out targetLeftNipple, out targetRightNipple))
            return false;

        // v83: Chest Hold face/back is a relation problem, not a root-dot-only problem.
        // Prefer chest/nipple facing axes so face-to-face does not fall into Back when Person roots happen to be same-facing.
        Vector3 pairCenter = (targetLeftNipple + targetRightNipple) * 0.5f;
        bool relationValid;
        bool mutualFront;
        bool selfFacingTarget;
        bool targetFacingSelf;
        float selfLooksTarget;
        float targetLooksSelf;
        float rootDot;
        relationValid = TryGetChestHoldMutualFrontRelation(
            pairCenter,
            out mutualFront,
            out selfFacingTarget,
            out targetFacingSelf,
            out selfLooksTarget,
            out targetLooksSelf,
            out rootDot
        );

        if (relationValid && selfFacingTarget && targetFacingSelf)
        {
            // 向かい合い: R hand -> target L nipple, L hand -> target R nipple.
            leftSideTarget = targetRightNipple;
            rightSideTarget = targetLeftNipple;
            mode = "face";
            DebugLog("[CHEST HOLD] mode=face relation=mutual-facing" +
                " selfLooksTarget=" + selfLooksTarget.ToString("F3", CultureInfo.InvariantCulture) +
                " targetLooksSelf=" + targetLooksSelf.ToString("F3", CultureInfo.InvariantCulture) +
                " rootDot=" + rootDot.ToString("F3", CultureInfo.InvariantCulture));
            return true;
        }

        Vector3 actorForward = GetNipplePairActorForwardAxis();
        Vector3 targetForward = GetNipplePairTargetForwardAxis();

        if (actorForward.sqrMagnitude < 0.0001f || targetForward.sqrMagnitude < 0.0001f)
            return false;

        actorForward.Normalize();
        targetForward.Normalize();

        float dot = Vector3.Dot(actorForward, targetForward);

        if (dot > 0.70f)
        {
            leftSideTarget = targetLeftNipple;
            rightSideTarget = targetRightNipple;
            mode = "back";
            DebugLog("[CHEST HOLD] mode=back dot=" + dot.ToString("F3", CultureInfo.InvariantCulture));
            return true;
        }

        if (dot < -0.70f)
        {
            leftSideTarget = targetRightNipple;
            rightSideTarget = targetLeftNipple;
            mode = "face";
            DebugLog("[CHEST HOLD] mode=face dot=" + dot.ToString("F3", CultureInfo.InvariantCulture));
            return true;
        }

        DebugLog("[CHEST HOLD] invalid angle dot=" + dot.ToString("F3", CultureInfo.InvariantCulture));
        return false;
    }

    private bool TryGetTargetPairPositions(out Vector3 leftTarget, out Vector3 rightTarget)
    {
        leftTarget = Vector3.zero;
        rightTarget = Vector3.zero;

        if (selectedTargetPerson == null || targetPersonPartChooser == null)
            return false;

        string choice = targetPersonPartChooser.val;
        string[] leftNames = null;
        string[] rightNames = null;

        if (choice == TC_HAND)
        {
            leftNames = new string[] { "lHandControl", "leftHandControl", "lHand", "leftHand" };
            rightNames = new string[] { "rHandControl", "rightHandControl", "rHand", "rightHand" };
        }
        else if (choice == TC_FOOT)
        {
            leftNames = new string[] { "lFootControl", "leftFootControl", "lFoot", "leftFoot" };
            rightNames = new string[] { "rFootControl", "rightFootControl", "rFoot", "rightFoot" };
        }
        else if (choice == TC_KNEE)
        {
            leftNames = new string[] { "lKneeControl", "leftKneeControl", "lKnee", "leftKnee" };
            rightNames = new string[] { "rKneeControl", "rightKneeControl", "rKnee", "rightKnee" };
        }
        else
        {
            return false;
        }

        bool hasLeft = TryFindPointOnAtom(selectedTargetPerson, leftNames, out leftTarget);
        bool hasRight = TryFindPointOnAtom(selectedTargetPerson, rightNames, out rightTarget);

        if (IsDebugEnabled())
        {
            DebugLog("[TARGET PAIR] controller=" + choice +
                " left=" + Bool01(hasLeft) +
                " right=" + Bool01(hasRight) +
                " leftPos=" + FormatVector3(leftTarget) +
                " rightPos=" + FormatVector3(rightTarget));
        }

        return hasLeft && hasRight;
    }

    private bool TryGetAssignedHipHoldTargets(out Vector3 leftSideTarget, out Vector3 rightSideTarget, out string mode)
    {
        return TryGetAssignedHipHoldTargets(0.0f, true, out leftSideTarget, out rightSideTarget, out mode);
    }

    private bool TryGetAssignedHipHoldTargets(float openWidth, bool log, out Vector3 leftSideTarget, out Vector3 rightSideTarget, out string mode)
    {
        leftSideTarget = Vector3.zero;
        rightSideTarget = Vector3.zero;
        mode = "none";

        if (selectedPerson == null || selectedTargetPerson == null)
            return false;

        Vector3 targetLeft;
        Vector3 targetRight;
        if (!TryGetHipHoldSideTargets(openWidth, log, out targetLeft, out targetRight))
            return false;

        Vector3 actorForward = GetSelectedPersonForwardAxis();
        Vector3 targetForward = GetTargetPersonForwardAxis();

        if (actorForward.sqrMagnitude < 0.0001f || targetForward.sqrMagnitude < 0.0001f)
            return false;

        actorForward.Normalize();
        targetForward.Normalize();

        float dot = Vector3.Dot(actorForward, targetForward);

        if (dot > 0.70f)
        {
            leftSideTarget = targetLeft;
            rightSideTarget = targetRight;
            mode = "back";
            if (log)
                DebugLog("[HIP HOLD] mode=back dot=" + dot.ToString("F3", CultureInfo.InvariantCulture));
            return true;
        }

        if (dot < -0.70f)
        {
            leftSideTarget = targetRight;
            rightSideTarget = targetLeft;
            mode = "face";
            if (log)
                DebugLog("[HIP HOLD] mode=face dot=" + dot.ToString("F3", CultureInfo.InvariantCulture));
            return true;
        }

        if (log)
            DebugLog("[HIP HOLD] invalid angle dot=" + dot.ToString("F3", CultureInfo.InvariantCulture));
        return false;
    }

    private bool TryGetHipHoldSideTargets(out Vector3 leftTarget, out Vector3 rightTarget)
    {
        return TryGetHipHoldSideTargets(0.0f, true, out leftTarget, out rightTarget);
    }

    private bool TryGetHipHoldSideTargets(float openWidth, out Vector3 leftTarget, out Vector3 rightTarget)
    {
        return TryGetHipHoldSideTargets(openWidth, true, out leftTarget, out rightTarget);
    }

    private bool TryGetHipHoldSideTargets(float openWidth, bool log, out Vector3 leftTarget, out Vector3 rightTarget)
    {
        leftTarget = Vector3.zero;
        rightTarget = Vector3.zero;

        if (selectedTargetPerson == null)
            return false;

        Quaternion rot = selectedTargetPerson.transform != null ? selectedTargetPerson.transform.rotation : Quaternion.identity;
        Vector3 right = rot * Vector3.right;
        Vector3 up = rot * Vector3.up;

        if (right.sqrMagnitude < 0.0001f) right = Vector3.right;
        if (up.sqrMagnitude < 0.0001f) up = Vector3.up;

        right.Normalize();
        up.Normalize();

        Vector3 leftThigh;
        Vector3 rightThigh;
        bool hasLeft = TryFindPointOnAtom(selectedTargetPerson, new string[] {
            "lThighControl", "leftThighControl", "lThigh", "leftThigh",
            "lThighBend", "leftThighBend", "lUpperLeg", "leftUpperLeg"
        }, out leftThigh);
        bool hasRight = TryFindPointOnAtom(selectedTargetPerson, new string[] {
            "rThighControl", "rightThighControl", "rThigh", "rightThigh",
            "rThighBend", "rightThighBend", "rUpperLeg", "rightUpperLeg"
        }, out rightThigh);

        float upOffset = 0.050f;
        if (hasLeft && hasRight)
        {
            Vector3 leftFinal = leftThigh + up * upOffset;
            Vector3 rightFinal = rightThigh + up * upOffset;
            leftFinal.x = leftThigh.x;
            rightFinal.x = rightThigh.x;
            leftTarget = leftFinal - right * openWidth;
            rightTarget = rightFinal + right * openWidth;
            if (log)
            {
                DebugLog("[HIP HOLD TARGET] thighs=1 left=(" +
                    leftThigh.x.ToString("F3", CultureInfo.InvariantCulture) + "," +
                    leftThigh.y.ToString("F3", CultureInfo.InvariantCulture) + "," +
                    leftThigh.z.ToString("F3", CultureInfo.InvariantCulture) + ") right=(" +
                    rightThigh.x.ToString("F3", CultureInfo.InvariantCulture) + "," +
                    rightThigh.y.ToString("F3", CultureInfo.InvariantCulture) + "," +
                    rightThigh.z.ToString("F3", CultureInfo.InvariantCulture) + ") open=" +
                    openWidth.ToString("F3", CultureInfo.InvariantCulture));
            }
            return true;
        }

        FreeControllerV3 hip = GetControlFromAtom(selectedTargetPerson, "hipControl");
        Vector3 hipCenter = hip != null
            ? (hip.control != null ? hip.control.position : hip.transform.position)
            : selectedTargetPerson.transform.position + up * 0.85f;

        leftTarget = hipCenter - right * (0.180f + openWidth) + up * 0.020f;
        rightTarget = hipCenter + right * (0.180f + openWidth) + up * 0.020f;
        if (log)
            DebugLog("[HIP HOLD TARGET] thighs=0 fallback=hip open=" + openWidth.ToString("F3", CultureInfo.InvariantCulture));
        return true;
    }

    private bool TryGetTargetNipplePositions(out Vector3 leftNipple, out Vector3 rightNipple)
    {
        leftNipple = Vector3.zero;
        rightNipple = Vector3.zero;

        if (selectedTargetPerson == null)
            return false;

        bool hasLeft = TryFindPointOnAtom(selectedTargetPerson, new string[] {
            "lNipple", "lnipple", "lNippleControl", "leftNipple", "LeftNipple", "nipple_l", "nippleL"
        }, out leftNipple);

        bool hasRight = TryFindPointOnAtom(selectedTargetPerson, new string[] {
            "rNipple", "rnipple", "rNippleControl", "rightNipple", "RightNipple", "nipple_r", "nippleR"
        }, out rightNipple);

        if (hasLeft && hasRight)
            return true;

        // 実Controlが無い環境向けの安全な概算。
        // lrnipple があれば左右乳首ペア中心として使い、無ければ chestControl を基準にする。
        Vector3 center;
        Quaternion rot;
        if (!TryGetApproxNippleCenterAndRotation(out center, out rot))
            return false;

        Vector3 right = rot * Vector3.right;
        Vector3 left = -right;
        Vector3 up = rot * Vector3.up;
        Vector3 forward = rot * Vector3.forward;

        float halfWidth = 0.115f;
        float upOffset = 0.015f;
        float forwardOffset = 0.035f;

        leftNipple = center + left.normalized * halfWidth + up.normalized * upOffset + forward.normalized * forwardOffset;
        rightNipple = center + right.normalized * halfWidth + up.normalized * upOffset + forward.normalized * forwardOffset;
        return true;
    }

    private bool TryGetApproxNippleCenterAndRotation(out Vector3 center, out Quaternion rot)
    {
        center = Vector3.zero;
        rot = Quaternion.identity;

        if (selectedTargetPerson == null)
            return false;

        FreeControllerV3 pairControl = GetControlFromAtom(selectedTargetPerson, LRNIPPLE);
        if (pairControl != null)
        {
            center = pairControl.control != null ? pairControl.control.position : pairControl.transform.position;
            rot = pairControl.control != null ? pairControl.control.rotation : pairControl.transform.rotation;
            return true;
        }

        FreeControllerV3 chest = GetControlFromAtom(selectedTargetPerson, "chestControl");
        if (chest != null)
        {
            center = chest.control != null ? chest.control.position : chest.transform.position;
            rot = chest.control != null ? chest.control.rotation : chest.transform.rotation;
            return true;
        }

        center = selectedTargetPerson.transform.position + selectedTargetPerson.transform.up * 1.25f;
        rot = selectedTargetPerson.transform.rotation;
        return true;
    }

    private Vector3 GetTargetPersonForwardAxis()
    {
        // v3.0u:
        // 相手側の向き判定は chestControl/head/選択Controller ではなく、
        // Target Person の root 向きで固定する。
        // 前屈や頭の向きで左右判定が反転するのを防ぐ。
        if (selectedTargetPerson == null || selectedTargetPerson.transform == null)
            return Vector3.forward;

        Vector3 forward = selectedTargetPerson.transform.forward;
        if (forward.sqrMagnitude > 0.0001f)
            return forward.normalized;

        return Vector3.forward;
    }

    private Vector3 GetTargetPersonRightAxis()
    {
        // v52: Target-local right axis for Chest Hold back pass test.
        // Root basis only; do not use self/hand position.
        if (selectedTargetPerson == null || selectedTargetPerson.transform == null)
            return Vector3.right;

        Vector3 right = selectedTargetPerson.transform.right;
        if (right.sqrMagnitude > 0.0001f)
            return right.normalized;

        return Vector3.right;
    }

    private Vector3 GetSelectedPersonForwardAxis()
    {
        // v3.0u:
        // 自分側の向き判定は chestControl ではなく、
        // Selected Person の root 向きで固定する。
        // 前屈・胸の回転・頭の向きで同方向/対面判定がブレるのを防ぐ。
        if (selectedPerson == null || selectedPerson.transform == null)
            return Vector3.forward;

        Vector3 forward = selectedPerson.transform.forward;
        if (forward.sqrMagnitude > 0.0001f)
            return forward.normalized;

        return Vector3.forward;
    }

    private Vector3 GetNipplePairTargetForwardAxis()
    {
        // v3.0aj:
        // Nipple Pair/control/pufupufu 専用。旧版と同じく chestControl を優先する。
        // 通常Grab用の GetTargetPersonForwardAxis() はroot基準のまま維持する。
        if (selectedTargetPerson == null)
            return Vector3.forward;

        FreeControllerV3 chest = GetControlFromAtom(selectedTargetPerson, "chestControl");
        if (chest != null)
        {
            Quaternion rot = chest.control != null ? chest.control.rotation : chest.transform.rotation;
            Vector3 f = rot * Vector3.forward;
            if (f.sqrMagnitude > 0.0001f)
                return f.normalized;
        }

        Vector3 forward = selectedTargetPerson.transform.forward;
        if (forward.sqrMagnitude > 0.0001f)
            return forward.normalized;

        return Vector3.forward;
    }

    private Vector3 GetNipplePairActorForwardAxis()
    {
        // v3.0aj:
        // Nipple Pair/control/pufupufu 専用。旧版と同じく自分側 chestControl を優先する。
        if (selectedPerson == null)
            return Vector3.forward;

        if (chestControl != null)
        {
            Quaternion rot = chestControl.control != null ? chestControl.control.rotation : chestControl.transform.rotation;
            Vector3 f = rot * Vector3.forward;
            if (f.sqrMagnitude > 0.0001f)
                return f.normalized;
        }

        Vector3 forward = selectedPerson.transform.forward;
        if (forward.sqrMagnitude > 0.0001f)
            return forward.normalized;

        return Vector3.forward;
    }

    private bool IsSameFacingTargetPersonForNipplePair()
    {
        if (!IsTargetPersonMode())
            return false;

        Vector3 actorForward = GetSelectedPersonForwardAxis();
        Vector3 targetForward = GetTargetPersonForwardAxis();

        if (actorForward.sqrMagnitude < 0.0001f || targetForward.sqrMagnitude < 0.0001f)
            return false;

        return Vector3.Dot(actorForward.normalized, targetForward.normalized) > 0.70f;
    }

    private bool IsGrabberInFrontOfTargetPersonForNipplePair(Vector3 targetPoint)
    {
        if (selectedPerson == null || selectedTargetPerson == null)
            return true;

        Vector3 grabberPos = selectedPerson.transform.position;
        if (chestControl != null)
            grabberPos = chestControl.control != null ? chestControl.control.position : chestControl.transform.position;

        Vector3 toGrabber = grabberPos - targetPoint;
        if (toGrabber.sqrMagnitude < 0.0001f)
            return true;

        Vector3 targetForward = GetTargetPersonForwardAxis();
        if (targetForward.sqrMagnitude < 0.0001f)
            return true;

        return Vector3.Dot(toGrabber.normalized, targetForward.normalized) >= 0.0f;
    }

    private Quaternion GetNipplePairRotation()
    {
        Vector3 center;
        Quaternion rot;
        if (TryGetApproxNippleCenterAndRotation(out center, out rot))
            return rot;

        return selectedTargetPerson != null && selectedTargetPerson.transform != null
            ? selectedTargetPerson.transform.rotation
            : Quaternion.identity;
    }

    private Vector3 GetNipplePairSideAxis()
    {
        // v3.0aj:
        // Nipple Pair/control/pufupufu の左右軸は、旧版同様に胸/乳首ペアControl由来にする。
        // 通常Grabの手足左右軸は GetTargetSideAxis() のroot基準を維持。
        Vector3 axis = GetNipplePairRotation() * Vector3.right;
        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.right;

        return -axis.normalized;
    }

    private Vector3 GetNipplePairZOffsetVector()
    {
        Vector3 axis = GetTargetPersonForwardAxis();
        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.forward;

        axis.Normalize();

        if (IsSameFacingTargetPerson())
            axis = -axis;

        return axis * GetTargetZOffsetValue();
    }

    private bool TryFindPointOnAtom(Atom atom, string[] names, out Vector3 pos)
    {
        pos = Vector3.zero;

        if (atom == null || names == null)
            return false;

        foreach (string name in names)
        {
            FreeControllerV3 fc = GetControlFromAtom(atom, name);
            if (fc != null)
            {
                pos = fc.control != null ? fc.control.position : fc.transform.position;
                return true;
            }
        }

        Transform[] transforms = atom.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in transforms)
        {
            if (t == null || string.IsNullOrEmpty(t.name))
                continue;

            foreach (string name in names)
            {
                if (string.Equals(t.name, name, StringComparison.OrdinalIgnoreCase))
                {
                    pos = t.position;
                    return true;
                }
            }
        }

        return false;
    }

    private bool TryFindControlPointOnAtom(Atom atom, string[] names, out Vector3 pos)
    {
        pos = Vector3.zero;

        if (atom == null || names == null)
            return false;

        foreach (string name in names)
        {
            FreeControllerV3 fc = GetControlFromAtom(atom, name);
            if (fc != null)
            {
                pos = fc.control != null ? fc.control.position : fc.transform.position;
                return true;
            }
        }

        return false;
    }

    private Transform FindChildTransformOnAtom(Atom atom, string childName)
    {
        if (atom == null || string.IsNullOrEmpty(childName))
            return null;

        Transform[] transforms = atom.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in transforms)
        {
            if (t != null && string.Equals(t.name, childName, StringComparison.OrdinalIgnoreCase))
                return t;
        }

        return null;
    }

    private Transform FindFirstChildTransformOnAtom(Atom atom, string[] childNames)
    {
        if (atom == null || childNames == null)
            return null;

        Transform[] transforms = atom.GetComponentsInChildren<Transform>(true);
        foreach (string childName in childNames)
        {
            if (string.IsNullOrEmpty(childName))
                continue;

            foreach (Transform t in transforms)
            {
                if (t != null && string.Equals(t.name, childName, StringComparison.OrdinalIgnoreCase))
                    return t;
            }
        }

        return null;
    }

    private Transform FindGenTargetTransform(Atom atom)
    {
        if (atom == null)
            return null;

        if (genTargetCacheValid && genTargetCacheAtom == atom)
            return genTargetCacheTransform;

        genTargetCacheAtom = atom;
        genTargetCacheTransform = FindChildTransformOnAtom(atom, "LabiaTrigger");
        genTargetCacheValid = true;

        if (IsDebugEnabled())
            DebugLog("[GEN TARGET CACHE] " + (genTargetCacheTransform != null
                ? "found path=" + GetTransformPath(genTargetCacheTransform)
                : "not found"));

        return genTargetCacheTransform;
    }

    private Transform FindAnusTargetTransform(Atom atom)
    {
        if (atom == null)
            return null;

        if (anusTargetCacheValid && anusTargetCacheAtom == atom)
            return anusTargetCacheTransform;

        anusTargetCacheAtom = atom;
        anusTargetCacheTransform = FindChildTransformByPathSuffix(atom, "_JointAl/Debug");
        anusTargetCacheValid = true;

        if (IsDebugEnabled())
            DebugLog("[ANUS TARGET CACHE] " + (anusTargetCacheTransform != null
                ? "found path=" + GetTransformPath(anusTargetCacheTransform)
                : "not found"));

        return anusTargetCacheTransform;
    }

    private Transform FindChildTransformByPathSuffix(Atom atom, string pathSuffix)
    {
        if (atom == null || string.IsNullOrEmpty(pathSuffix))
            return null;

        Transform[] transforms = atom.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in transforms)
        {
            if (t == null)
                continue;

            string path = GetTransformPath(t);
            if (path.EndsWith(pathSuffix, StringComparison.OrdinalIgnoreCase))
                return t;
        }

        return null;
    }

    private void LogTargetIntimateNames()
    {
        if (selectedTargetPerson == null)
        {
            SetStatus("Intimate scan needs Target Person");
            DebugLog("[INTIMATE SCAN] no Target Person");
            return;
        }

        Transform gen = FindFirstChildTransformOnAtom(selectedTargetPerson, new string[] {
            "LabiaTrigger"
        });
        Transform vagina = FindFirstChildTransformOnAtom(selectedTargetPerson, new string[] {
            "VaginaTrigger"
        });
        Transform deepVagina = FindFirstChildTransformOnAtom(selectedTargetPerson, new string[] {
            "DeepVaginaTrigger"
        });
        Transform deeperVagina = FindFirstChildTransformOnAtom(selectedTargetPerson, new string[] {
            "DeeperVaginaTrigger"
        });
        Transform anus = FindAnusTargetTransform(selectedTargetPerson);

        DebugLog("[INTIMATE SCAN] target=" + selectedTargetPerson.uid +
            " / focused=pelvis triggers + _JointAl/Debug");
        DebugLog("[INTIMATE SCAN] Gen candidate=" +
            FormatTransformCandidate(gen));
        DebugLog("[INTIMATE SCAN] Vagina candidate=" +
            FormatTransformCandidate(vagina));
        DebugLog("[INTIMATE SCAN] Deep Vagina candidate=" +
            FormatTransformCandidate(deepVagina));
        DebugLog("[INTIMATE SCAN] Deeper Vagina candidate=" +
            FormatTransformCandidate(deeperVagina));
        DebugLog("[INTIMATE SCAN] Anus candidate=" +
            FormatTransformCandidate(anus));

        SetStatus("Intimate scan / Gen=" + Bool01(gen != null) +
            " Anus=" + Bool01(anus != null));
    }

    private string FormatTransformCandidate(Transform t)
    {
        if (t == null)
            return "<not found>";

        return t.name + " path=" + GetTransformPath(t) + " pos=" + FormatVector3(t.position);
    }

    private string GetTransformPath(Transform t)
    {
        if (t == null)
            return "<null>";

        string path = t.name;
        Transform parent = t.parent;
        int guard = 0;
        while (parent != null && guard < 80)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
            guard++;
        }

        return path;
    }

    private bool TryGetCuratedTargetPoint(out Vector3 center, out Quaternion rot)
    {
        center = Vector3.zero;
        rot = Quaternion.identity;

        if (selectedTargetPerson == null || targetPersonPartChooser == null)
            return false;

        string choice = targetPersonPartChooser.val;

        if (choice == TC_HIP_HOLD)
        {
            Vector3 left;
            Vector3 right;
            if (!TryGetHipHoldSideTargets(out left, out right))
                return false;

            center = (left + right) * 0.5f;
            rot = selectedTargetPerson.transform != null ? selectedTargetPerson.transform.rotation : Quaternion.identity;
            return true;
        }

        if (choice == TC_HEAD)
        {
            FreeControllerV3 head = GetControlFromAtom(selectedTargetPerson, "headControl");
            if (head != null)
            {
                Vector3 headPos = head.control != null ? head.control.position : head.transform.position;
                Quaternion headRot = head.control != null ? head.control.rotation : head.transform.rotation;
                center = headPos + (headRot * Vector3.up) * HEAD_TARGET_UP_OFFSET;
                rot = headRot;
                if (IsDebugEnabled())
                    DebugLog("[HEAD TARGET] adjusted=headControl center=" + FormatVector3(center));
                return true;
            }
        }

        if (choice == TC_HEAD_TOP)
        {
            FreeControllerV3 head = GetControlFromAtom(selectedTargetPerson, "headControl");
            if (head != null)
            {
                Vector3 headPos = head.control != null ? head.control.position : head.transform.position;
                Quaternion headRot = head.control != null ? head.control.rotation : head.transform.rotation;
                center = headPos + (headRot * Vector3.forward) * HEAD_TOP_FORWARD_OFFSET + (headRot * Vector3.up) * HEAD_TOP_UP_OFFSET;
                rot = headRot;
                if (IsDebugEnabled())
                    DebugLog("[HEAD TOP TARGET] fallback=headControl center=" + FormatVector3(center));
                return true;
            }
        }

        if (choice == TC_L_NIPPLE || choice == TC_R_NIPPLE)
        {
            Vector3 left;
            Vector3 right;
            if (!TryGetTargetNipplePositions(out left, out right))
                return false;

            center = choice == TC_L_NIPPLE ? left : right;
            rot = GetPointFallbackRotation("chestControl");
            return true;
        }

        if (choice == TC_NECK)
        {
            if (TryFindControlPointOnAtom(selectedTargetPerson, new string[] {
                "neckControl", "neck", "neckTarget"
            }, out center))
            {
                rot = GetPointFallbackRotation("chestControl");
                return true;
            }

            FreeControllerV3 head = GetControlFromAtom(selectedTargetPerson, "headControl");
            FreeControllerV3 chest = GetControlFromAtom(selectedTargetPerson, "chestControl");
            if (head != null && chest != null)
            {
                Vector3 headPos = head.control != null ? head.control.position : head.transform.position;
                Vector3 chestPos = chest.control != null ? chest.control.position : chest.transform.position;
                center = Vector3.Lerp(chestPos, headPos, 0.68f);
                rot = GetPointFallbackRotation("chestControl");
                return true;
            }
        }

        if (choice == TC_MOUTH)
        {
            if (TryFindControlPointOnAtom(selectedTargetPerson, new string[] {
                "mouthControl", "mouthTarget"
            }, out center))
            {
                rot = GetPointFallbackRotation("headControl");
                center += (rot * Vector3.forward) * MOUTH_TARGET_FORWARD_OFFSET + (rot * Vector3.up) * MOUTH_TARGET_UP_OFFSET;
                if (IsDebugEnabled())
                    DebugLog("[MOUTH TARGET] faceFront=mouthControl center=" + FormatVector3(center));
                return true;
            }

            FreeControllerV3 head = GetControlFromAtom(selectedTargetPerson, "headControl");
            if (head != null)
            {
                Vector3 headPos = head.control != null ? head.control.position : head.transform.position;
                Quaternion headRot = head.control != null ? head.control.rotation : head.transform.rotation;
                center = headPos + (headRot * Vector3.forward) * MOUTH_FALLBACK_FORWARD_OFFSET + (headRot * Vector3.up) * MOUTH_FALLBACK_UP_OFFSET;
                rot = headRot;
                if (IsDebugEnabled())
                    DebugLog("[MOUTH TARGET] faceFrontFallback=headControl center=" + FormatVector3(center));
                return true;
            }
        }

        if (choice == TC_GEN)
        {
            if (TryGetGenTargetPoint(out center, out rot))
                return true;
        }

        if (choice == TC_ANUS)
        {
            if (TryGetAnusTargetPoint(out center, out rot))
                return true;
        }

        if (choice == TC_CROTCH || choice == TC_GROIN)
        {
            if (TryGetGroinTargetPoint(out center, out rot))
                return true;
        }

        return false;
    }

    private bool TryGetGenTargetPoint(out Vector3 center, out Quaternion rot)
    {
        center = Vector3.zero;
        rot = Quaternion.identity;

        Transform labia = FindGenTargetTransform(selectedTargetPerson);
        if (labia != null)
        {
            center = labia.position;
            rot = labia.rotation;
            if (IsDebugEnabled())
                DebugLog("[GEN TARGET] use=" + labia.name + " path=" + GetTransformPath(labia) + " center=" + FormatVector3(center));
            return true;
        }

        if (IsDebugEnabled())
            DebugLog("[GEN TARGET] not found / candidates=LabiaTrigger");
        return false;
    }

    private bool TryGetAnusTargetPoint(out Vector3 center, out Quaternion rot)
    {
        center = Vector3.zero;
        rot = Quaternion.identity;

        Transform anus = FindAnusTargetTransform(selectedTargetPerson);
        if (anus != null)
        {
            center = anus.position;
            rot = anus.rotation;
            if (IsDebugEnabled())
                DebugLog("[ANUS TARGET] use=" + anus.name + " path=" + GetTransformPath(anus) + " center=" + FormatVector3(center));
            return true;
        }

        if (IsDebugEnabled())
            DebugLog("[ANUS TARGET] not found / candidate=_JointAl/Debug");
        return false;
    }

    private bool TryGetGroinTargetPoint(out Vector3 center, out Quaternion rot)
    {
        center = Vector3.zero;
        rot = Quaternion.identity;

        Vector3 leftThigh;
        Vector3 rightThigh;
        bool hasLeft = TryFindPointOnAtom(selectedTargetPerson, new string[] {
            "lThighControl", "leftThighControl", "lThigh", "leftThigh",
            "lThighBend", "leftThighBend", "lUpperLeg", "leftUpperLeg"
        }, out leftThigh);
        bool hasRight = TryFindPointOnAtom(selectedTargetPerson, new string[] {
            "rThighControl", "rightThighControl", "rThigh", "rightThigh",
            "rThighBend", "rightThighBend", "rUpperLeg", "rightUpperLeg"
        }, out rightThigh);

        if (hasLeft && hasRight)
        {
            center = Vector3.Lerp(leftThigh, rightThigh, 0.50f);
            rot = GetPointFallbackRotation("hipControl");
            return true;
        }

        FreeControllerV3 hip = GetControlFromAtom(selectedTargetPerson, "hipControl");
        if (hip != null)
        {
            Vector3 hipPos = hip.control != null ? hip.control.position : hip.transform.position;
            Quaternion hipRot = hip.control != null ? hip.control.rotation : hip.transform.rotation;
            center = hipPos - (hipRot * Vector3.up) * 0.200f;
            rot = hipRot;
            return true;
        }

        return false;
    }

    private Quaternion GetPointFallbackRotation(string controllerName)
    {
        FreeControllerV3 fc = GetControlFromAtom(selectedTargetPerson, controllerName);
        if (fc != null)
            return fc.control != null ? fc.control.rotation : fc.transform.rotation;

        return selectedTargetPerson != null && selectedTargetPerson.transform != null
            ? selectedTargetPerson.transform.rotation
            : Quaternion.identity;
    }

    private bool HasValidTarget()
    {
        if (IsTargetPersonMode())
            return selectedTargetPerson != null;

        return selectedTargetAtom != null;
    }

    private Vector3 GetTargetCenter()
    {
        if (IsTargetPersonMode())
        {
            if (IsNipplePairMode())
            {
                Vector3 nippleCenter;
                Quaternion nippleRot;
                if (TryGetApproxNippleCenterAndRotation(out nippleCenter, out nippleRot))
                    return ApplyTargetZOffset(nippleCenter, nippleRot);
            }

            if (IsHipHoldMode())
            {
                Vector3 hipHoldCenter;
                Quaternion hipHoldRot;
                if (TryGetCuratedTargetPoint(out hipHoldCenter, out hipHoldRot))
                    return ApplyTargetZOffset(hipHoldCenter, hipHoldRot);
            }

            if (IsTargetPairMode())
            {
                Vector3 pairLeft;
                Vector3 pairRight;
                if (TryGetTargetPairPositions(out pairLeft, out pairRight))
                    return ApplyTargetZOffset(Vector3.Lerp(pairLeft, pairRight, 0.50f), GetTargetRootRotation());
            }

            if (ShouldPreferCuratedTargetPoint())
            {
                Vector3 preferredCenter;
                Quaternion preferredRot;
                if (TryGetCuratedTargetPoint(out preferredCenter, out preferredRot))
                    return ApplyTargetZOffset(preferredCenter, preferredRot);
            }

            FreeControllerV3 part = GetTargetPersonPartControl();
            if (part != null)
            {
                Vector3 partCenter = part.control != null ? part.control.position : part.transform.position;
                Quaternion partRot = part.control != null ? part.control.rotation : part.transform.rotation;
                Vector3 rawCenter = ApplyChestBackGrabOffset(partCenter);
                Vector3 finalCenter = ApplyTargetZOffset(rawCenter, partRot);
                if (IsPeniMode() && IsDebugEnabled())
                {
                    string choice = targetPersonPartChooser != null ? targetPersonPartChooser.val : NONE;
                    string actual = GetTargetControllerActualName(choice);
                    DebugLog("[PENI TARGET] choice=" + choice +
                        " actual=" + actual +
                        " use=" + part.name +
                        " path=" + (part.transform != null ? GetTransformPath(part.transform) : "") +
                        " rawCenter=" + FormatVector3(rawCenter) +
                        " finalCenter=" + FormatVector3(finalCenter) +
                        " zOffset=" + GetTargetZOffsetValue().ToString("F3", CultureInfo.InvariantCulture));
                }
                return finalCenter;
            }

            Vector3 pointCenter;
            Quaternion pointRot;
            if (TryGetCuratedTargetPoint(out pointCenter, out pointRot))
                return ApplyTargetZOffset(pointCenter, pointRot);

            if (selectedTargetPerson != null)
                return ApplyTargetZOffset(selectedTargetPerson.transform.position + Vector3.up * 1.0f, selectedTargetPerson.transform.rotation);

            return Vector3.zero;
        }

        if (selectedTargetAtom == null)
            return Vector3.zero;

        Vector3 center = selectedTargetAtom.transform.position;
        Quaternion rot = selectedTargetAtom.transform.rotation;

        // Atom自体の transform ではなく、mainController.control を優先する。
        // VaMではAtom本体TransformよりControlの回転が実際の見た目に近いことがある。
        if (selectedTargetAtom.mainController != null && selectedTargetAtom.mainController.control != null)
        {
            center = selectedTargetAtom.mainController.control.position;
            rot = selectedTargetAtom.mainController.control.rotation;
        }

        // Target Z Offset:
        // Atom/Controlから見たローカルZ直線上の+-地点をターゲットにする。
        // + は control forward 側、- は control backward 側。
        return ApplyTargetZOffset(center, rot);
    }

    private float GetTargetZOffsetValue()
    {
        float z = targetZOffsetJSON != null ? targetZOffsetJSON.val : 0.0f;
        z += autoZOffsetJSON != null ? autoZOffsetJSON.val : 0.0f;
        return z;
    }

    private Vector3 ApplyTargetZOffset(Vector3 center, Quaternion rot)
    {
        return center + GetTargetForwardAxis(rot) * GetTargetZOffsetValue();
    }

    private Vector3 ApplyChestBackGrabOffset(Vector3 center)
    {
        if (!IsChestTargetController())
            return center;

        if (IsGrabberInFrontOfTargetPerson(center))
            return center;

        Vector3 forward = GetTargetPersonForwardAxis();
        if (forward.sqrMagnitude < 0.0001f)
            return center;

        return center + forward.normalized * 0.18f;
    }

    private Vector3 ApplyFootPersonGrabOffset(Vector3 center)
    {
        if (!IsTargetPersonMode())
            return center;

        Vector3 forward = GetTargetPersonForwardAxis();
        if (forward.sqrMagnitude < 0.0001f)
            return center;

        forward.Normalize();

        // 掴む側がターゲットPersonの正面側か背面側か
        bool front = IsGrabberInFrontOfTargetPerson(center);

        // 正面側からFoot Grabするときの補正
        float frontOffset = 0.03f;

        // 背面側からFoot Grabするときの補正
        float backOffset = 0.08f;

        return center + forward * (front ? frontOffset : backOffset);
    }

    private bool IsChestTargetController()
    {
        string choice = targetPersonPartChooser != null ? targetPersonPartChooser.val : NONE;
        string controlName = GetTargetControllerActualName(choice);
        return NormalizeControllerKey(controlName) == "chestcontrol";
    }

    private Vector3 GetTargetZOffsetVector()
    {
        return GetTargetForwardAxis(GetTargetRotation()) * GetTargetZOffsetValue();
    }

    private Quaternion GetTargetRotation()
    {
        if (IsTargetPersonMode())
        {
            if (IsNipplePairMode())
            {
                Vector3 nippleCenter;
                Quaternion nippleRot;
                if (TryGetApproxNippleCenterAndRotation(out nippleCenter, out nippleRot))
                    return nippleRot;
            }

            if (ShouldPreferCuratedTargetPoint())
            {
                Vector3 preferredCenter;
                Quaternion preferredRot;
                if (TryGetCuratedTargetPoint(out preferredCenter, out preferredRot))
                    return preferredRot;
            }

            FreeControllerV3 part = GetTargetPersonPartControl();
            if (part != null)
                return part.control != null ? part.control.rotation : part.transform.rotation;

            return selectedTargetPerson != null ? selectedTargetPerson.transform.rotation : Quaternion.identity;
        }

        if (selectedTargetAtom == null)
            return Quaternion.identity;

        if (selectedTargetAtom.mainController != null && selectedTargetAtom.mainController.control != null)
            return selectedTargetAtom.mainController.control.rotation;

        return selectedTargetAtom.transform.rotation;
    }

    private bool ShouldPreferCuratedTargetPoint()
    {
        if (!IsTargetPersonMode() || targetPersonPartChooser == null)
            return false;

        string choice = targetPersonPartChooser.val;
        return choice == TC_GEN || choice == TC_ANUS || choice == TC_GROIN || choice == TC_CROTCH || choice == TC_HEAD || choice == TC_HEAD_TOP || choice == TC_MOUTH;
    }

    private Vector3 GetTargetSideAxis()
    {
        // v3.0u:
        // 手足の左右配置に使う横方向軸は、Target Person mode では
        // 胸・頭・選択Controller・乳首ペアControlではなく、
        // Target Person root の rotation から取る。
        // Atom mode は従来通り mainController/control の回転を優先する。
        Vector3 axis = GetTargetRootRotation() * Vector3.right;
        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.right;

        // v2.0i:
        // UIの Grab Width / Final Grab Width は正の値のまま扱う。
        // Left/Right の手が反対側へ行く問題は、幅の符号ではなく
        // ターゲット横軸の向きが実運用上逆だったため、ここで一括反転する。
        return -axis.normalized;
    }

    private Quaternion GetTargetRootRotation()
    {
        if (IsTargetPersonMode())
            return selectedTargetPerson != null && selectedTargetPerson.transform != null
                ? selectedTargetPerson.transform.rotation
                : Quaternion.identity;

        if (selectedTargetAtom == null)
            return Quaternion.identity;

        if (selectedTargetAtom.mainController != null && selectedTargetAtom.mainController.control != null)
            return selectedTargetAtom.mainController.control.rotation;

        return selectedTargetAtom.transform.rotation;
    }

    private Vector3 GetHandSideAxis(Vector3 baseSide)
    {
        return baseSide;
    }

    private Vector3 GetHugBodyApproachSideAxis(Vector3 center, Vector3 fallbackSide)
    {
        // Hug Body の手は「相手rootの横軸」ではなく、掴む側から見た接近方向の左右へ広げる。
        // target root の向きだけに依存すると、位置関係によって横幅が前後方向へ化ける。
        Vector3 origin = GetHugOriginPosition(center);
        Vector3 approach = center - origin;
        approach.y = 0.0f;

        if (approach.sqrMagnitude < 0.0001f)
        {
            approach = GetSelectedPersonForwardAxis();
            approach.y = 0.0f;
        }

        if (approach.sqrMagnitude < 0.0001f)
        {
            approach = GetTargetPersonForwardAxis();
            approach.y = 0.0f;
        }

        Vector3 side = Vector3.zero;
        if (approach.sqrMagnitude > 0.0001f)
            side = Vector3.Cross(Vector3.up, approach.normalized);

        if (side.sqrMagnitude < 0.0001f)
            side = fallbackSide;

        if (side.sqrMagnitude < 0.0001f)
            side = Vector3.right;

        side.Normalize();

        // side が「自分の左側」を向くように揃える。
        // GetSideOffset(true, side, width) を左手用に使うため、+side = 左手側に固定する。
        Vector3 actorRight = selectedPerson != null && selectedPerson.transform != null
            ? selectedPerson.transform.right
            : Vector3.right;
        actorRight.y = 0.0f;
        if (actorRight.sqrMagnitude > 0.0001f)
        {
            Vector3 actorLeft = -actorRight.normalized;
            if (Vector3.Dot(side, actorLeft) < 0.0f)
                side = -side;
        }

        return side.normalized;
    }

    private struct HugBodyHandLayout
    {
        public Vector3 handCenter;
        public bool leftPathRightSide;
        public bool rightPathRightSide;
        public float score;
        public float leftStartSide;
        public float rightStartSide;
        public string mode;
    }

    private HugBodyHandLayout ResolveHugBodyHandLayout(
        Vector3 targetCenter,
        Vector3 baseHandCenter,
        Vector3 handSide,
        float handPathWidth,
        bool log)
    {
        HugBodyHandLayout baseLayout = BuildHugBodyHandLayout("base", targetCenter, baseHandCenter, handSide, handPathWidth);
        HugBodyHandLayout best = baseLayout;
        float plusScore = -1.0f;
        float minusScore = -1.0f;
        float plusAwayDot = 0.0f;
        float minusAwayDot = 0.0f;
        float centerBias = 0.0f;
        bool farCandidateUsed = false;

        if (!IsHugMode())
        {
            Vector3 axis = GetHugForwardAxis(targetCenter);
            axis.y = 0.0f;
            if (axis.sqrMagnitude > 0.0001f)
            {
                axis.Normalize();

                // v4.0bk:
                // Hug Body + Hug Mode OFF では、奥側 handCenter 補正は「通過経路用」。
                // 最終地点まで補正を残すと手が奥へ出過ぎるため、終盤で補正量を0へ戻す。
                centerBias = GetHugBodyCenterBiasFade();
                Vector3 centerOffset = axis * HUG_BODY_HAND_CENTER_OFFSET * centerBias;

                HugBodyHandLayout plus = BuildHugBodyHandLayout("axis+", targetCenter, baseHandCenter + centerOffset, handSide, handPathWidth);
                HugBodyHandLayout minus = BuildHugBodyHandLayout("axis-", targetCenter, baseHandCenter - centerOffset, handSide, handPathWidth);
                plusScore = plus.score;
                minusScore = minus.score;

                // v4.0bj:
                // Hug Body の handCenter 補正は「手の移動距離が短い候補」だけで選ぶと、
                // 胸中心より自分root側へ戻る候補を選び、抱きに行く位置が手前に来ることがある。
                // targetCenter から actor/root へ向かう方向との dot が負の候補を「奥側」として優先する。
                Vector3 actorDir = GetHugOriginPosition(targetCenter) - targetCenter;
                actorDir.y = 0.0f;
                if (actorDir.sqrMagnitude > 0.0001f)
                {
                    actorDir.Normalize();
                    plusAwayDot = Vector3.Dot(plus.handCenter - targetCenter, actorDir);
                    minusAwayDot = Vector3.Dot(minus.handCenter - targetCenter, actorDir);

                    bool plusFar = plusAwayDot < -0.0001f;
                    bool minusFar = minusAwayDot < -0.0001f;

                    if (plusFar && minusFar)
                    {
                        best = plus.score <= minus.score ? plus : minus;
                        farCandidateUsed = true;
                    }
                    else if (plusFar)
                    {
                        best = plus;
                        farCandidateUsed = true;
                    }
                    else if (minusFar)
                    {
                        best = minus;
                        farCandidateUsed = true;
                    }
                }

                if (!farCandidateUsed)
                {
                    if (plus.score < best.score)
                        best = plus;
                    if (minus.score < best.score)
                        best = minus;
                }
            }
        }

        if (log && IsDebugEnabled())
        {
            DebugLog("[HUG BODY HAND LAYOUT] mode=" + best.mode +
                " score=" + best.score.ToString("F3", CultureInfo.InvariantCulture) +
                " baseScore=" + baseLayout.score.ToString("F3", CultureInfo.InvariantCulture) +
                " axisPlusScore=" + plusScore.ToString("F3", CultureInfo.InvariantCulture) +
                " axisMinusScore=" + minusScore.ToString("F3", CultureInfo.InvariantCulture) +
                " plusAwayDot=" + plusAwayDot.ToString("F3", CultureInfo.InvariantCulture) +
                " minusAwayDot=" + minusAwayDot.ToString("F3", CultureInfo.InvariantCulture) +
                " centerBias=" + centerBias.ToString("F3", CultureInfo.InvariantCulture) +
                " farPick=" + Bool01(farCandidateUsed) +
                " leftStartSide=" + best.leftStartSide.ToString("F3", CultureInfo.InvariantCulture) +
                " rightStartSide=" + best.rightStartSide.ToString("F3", CultureInfo.InvariantCulture) +
                " leftPathRight=" + Bool01(best.leftPathRightSide) +
                " rightPathRight=" + Bool01(best.rightPathRightSide) +
                " width=" + handPathWidth.ToString("F3", CultureInfo.InvariantCulture) +
                " targetCenter=" + FormatVector3(targetCenter) +
                " handCenter=" + FormatVector3(best.handCenter) +
                " side=" + FormatVector3(handSide));
        }

        return best;
    }

    private float GetHugBodyCenterBiasFade()
    {
        // v4.0bk:
        // 0.0 - 0.65 : 奥側補正を維持
        // 0.65 - 1.0 : smoothstepで補正を0へ戻す
        // 最終地点は補正なしの target center 基準にする。
        float t = GetMoveTLinear();
        if (t <= 0.65f)
            return 1.0f;

        float u = Mathf.Clamp01((t - 0.65f) / 0.35f);
        float smooth = u * u * (3.0f - 2.0f * u);
        return Mathf.Clamp01(1.0f - smooth);
    }

    private HugBodyHandLayout BuildHugBodyHandLayout(
        string mode,
        Vector3 targetCenter,
        Vector3 candidateHandCenter,
        Vector3 handSide,
        float handPathWidth)
    {
        HugBodyHandLayout layout = new HugBodyHandLayout();
        layout.mode = mode;
        layout.handCenter = candidateHandCenter;
        layout.leftPathRightSide = true;
        layout.rightPathRightSide = false;
        layout.score = 0.0f;
        layout.leftStartSide = 0.0f;
        layout.rightStartSide = 0.0f;

        Vector3 sideAxis = handSide.sqrMagnitude > 0.0001f ? handSide.normalized : Vector3.right;
        bool leftEnabled = leftHandJSON != null && leftHandJSON.val && lHandControl != null;
        bool rightEnabled = rightHandJSON != null && rightHandJSON.val && rHandControl != null;

        if (leftEnabled)
            layout.leftStartSide = GetHugBodyHandStartSide(lHandControl, candidateHandCenter, sideAxis);
        if (rightEnabled)
            layout.rightStartSide = GetHugBodyHandStartSide(rHandControl, candidateHandCenter, sideAxis);

        if (leftEnabled && rightEnabled)
        {
            // 現在 + 側にいる手は + 側へ、- 側にいる手は - 側へ送る。
            // これで L/R 固定によるクロスを避ける。
            if (layout.leftStartSide >= layout.rightStartSide)
            {
                layout.leftPathRightSide = true;
                layout.rightPathRightSide = false;
            }
            else
            {
                layout.leftPathRightSide = false;
                layout.rightPathRightSide = true;
            }
        }
        else if (leftEnabled)
        {
            layout.leftPathRightSide = layout.leftStartSide >= 0.0f;
            layout.rightPathRightSide = !layout.leftPathRightSide;
        }
        else if (rightEnabled)
        {
            layout.rightPathRightSide = layout.rightStartSide >= 0.0f;
            layout.leftPathRightSide = !layout.rightPathRightSide;
        }

        int count = 0;
        if (leftEnabled)
        {
            layout.score += ScoreHugBodyHandLayout(lHandControl, layout.leftPathRightSide, candidateHandCenter, handSide, handPathWidth);
            count++;
        }
        if (rightEnabled)
        {
            layout.score += ScoreHugBodyHandLayout(rHandControl, layout.rightPathRightSide, candidateHandCenter, handSide, handPathWidth);
            count++;
        }

        if (count > 0)
            layout.score /= (float)count;
        else
            layout.score = 9999.0f;

        return layout;
    }

    private float GetHugBodyHandStartSide(FreeControllerV3 fc, Vector3 handCenter, Vector3 sideAxis)
    {
        if (fc == null)
            return 0.0f;

        Vector3 start = Vector3.zero;
        if (!grabStartPositions.TryGetValue(fc, out start))
            start = GetControlPosition(fc);

        return Vector3.Dot(start - handCenter, sideAxis);
    }

    private float ScoreHugBodyHandLayout(
        FreeControllerV3 fc,
        bool pathRightSide,
        Vector3 handCenter,
        Vector3 handSide,
        float handPathWidth)
    {
        if (fc == null)
            return 9999.0f;

        Vector3 start = Vector3.zero;
        if (!grabStartPositions.TryGetValue(fc, out start))
            start = GetControlPosition(fc);

        Vector3 root = GetHandRootPosition(pathRightSide);
        Vector3 desired = handCenter + GetSideOffset(pathRightSide, handSide, handPathWidth);
        Vector3 finalTarget = GetReachLimitedPosition(root, desired, GetMaxHandReach(), GetHandPalmOffset(), fc, true, pathRightSide);
        float score = Vector3.Distance(start, finalTarget);

        if (handSide.sqrMagnitude > 0.0001f)
        {
            Vector3 sideAxis = handSide.normalized;
            float startSide = Vector3.Dot(start - handCenter, sideAxis);
            float targetSide = pathRightSide ? handPathWidth : -handPathWidth;
            if (Mathf.Abs(startSide) > 0.03f && startSide * targetSide < 0.0f)
                score += 0.50f;
        }

        return score;
    }

    private Vector3 GetFootSideAxis(Vector3 baseSide)
    {
        return baseSide;
    }

    private bool ShouldSwapSidePaths(Vector3 targetPoint)
    {
        if (!IsTargetPersonMode())
            return false;

        // v3.0v:
        // 以前は同方向判定だけで左右パスを入れ替えていた。
        // ただし、相手が背面を向いている＝掴む側がTarget Personの背面側にいる場合、
        // root forward同士は同方向になりやすく、ここで入れ替えると左手/右手の動きが逆になる。
        // 背面側から掴む場合は入れ替えない。
        if (!IsGrabberInFrontOfTargetPerson(targetPoint))
            return false;

        return IsSameFacingTargetPerson();
    }

    private bool IsSameFacingTargetPerson()
    {
        if (!IsTargetPersonMode())
            return false;

        Vector3 actorForward = GetSelectedPersonForwardAxis();
        Vector3 targetForward = GetTargetPersonForwardAxis();

        if (actorForward.sqrMagnitude < 0.0001f || targetForward.sqrMagnitude < 0.0001f)
            return false;

        return Vector3.Dot(actorForward.normalized, targetForward.normalized) > 0.70f;
    }

    private void LogSideDebug(Vector3 center, Vector3 handSide, Vector3 footSide)
    {
        if (!IsDebugEnabled())
            return;

        if (Time.time - lastSideDebugTime < 0.50f)
            return;
        lastSideDebugTime = Time.time;

        Vector3 actorForward = GetSelectedPersonForwardAxis();
        Vector3 targetForward = GetTargetPersonForwardAxis();
        Vector3 hugForward = IsHugMode() ? GetHugForwardAxis(center) : Vector3.zero;
        float facingDot = 0.0f;

        if (actorForward.sqrMagnitude > 0.0001f && targetForward.sqrMagnitude > 0.0001f)
            facingDot = Vector3.Dot(actorForward.normalized, targetForward.normalized);

        DebugLog("[SIDE] targetMode=" + (IsTargetPersonMode() ? "Person" : "Atom") +
            " controller=" + (targetPersonPartChooser != null ? targetPersonPartChooser.val : "<null>") +
            " dot=" + facingDot.ToString("F3", CultureInfo.InvariantCulture) +
            " sameFacing=" + (IsSameFacingTargetPerson() ? "1" : "0") +
            " backSide=" + (!IsGrabberInFrontOfTargetPerson(center) ? "1" : "0") +
            " swapPaths=" + (ShouldSwapSidePaths(center) ? "1" : "0") +
            " handPalm=" + (ShouldAlignHandPalm() ? "1" : "0") +
            " handSide=(" + handSide.x.ToString("F3", CultureInfo.InvariantCulture) + "," +
                handSide.y.ToString("F3", CultureInfo.InvariantCulture) + "," +
                handSide.z.ToString("F3", CultureInfo.InvariantCulture) + ")" +
            " footSide=(" + footSide.x.ToString("F3", CultureInfo.InvariantCulture) + "," +
                footSide.y.ToString("F3", CultureInfo.InvariantCulture) + "," +
                footSide.z.ToString("F3", CultureInfo.InvariantCulture) + ")" +
            " hugForward=(" + hugForward.x.ToString("F3", CultureInfo.InvariantCulture) + "," +
                hugForward.y.ToString("F3", CultureInfo.InvariantCulture) + "," +
                hugForward.z.ToString("F3", CultureInfo.InvariantCulture) + ")" +
            " center=(" + center.x.ToString("F3", CultureInfo.InvariantCulture) + "," +
                center.y.ToString("F3", CultureInfo.InvariantCulture) + "," +
                center.z.ToString("F3", CultureInfo.InvariantCulture) + ")");
    }

    private void LogHandRotationDebug(bool swapSidePaths, Vector3 center)
    {
        if (!IsDebugEnabled())
            return;

        if (Time.time - lastHandRotationDebugTime < 0.50f)
            return;
        lastHandRotationDebugTime = Time.time;

        // v3.0al: ApplyGrab の手LR割当と同じ条件でログを出す。
        bool frontSide = IsGrabberInFrontOfTargetPerson(center);
        bool leftPathRightSide = !swapSidePaths;
        bool rightPathRightSide = swapSidePaths;
        Vector3 rotOffset = GetHandRotationOffset();
        Quaternion leftRotation = GetPalmOrSoleRotation(Vector3.zero, center, rotOffset, true, leftPathRightSide, false);
        Quaternion rightRotation = GetPalmOrSoleRotation(Vector3.zero, center, rotOffset, true, rightPathRightSide, true);
        Vector3 leftEuler = leftRotation.eulerAngles;
        Vector3 rightEuler = rightRotation.eulerAngles;

        DebugLog("[HAND ROT] align=" + (ShouldAlignHandPalm() ? "1" : "0") +
            " sameFacing=" + (IsSameFacingTargetPerson() ? "1" : "0") +
            " frontSide=" + Bool01(frontSide) +
            " leftPathRight=" + Bool01(leftPathRightSide) +
            " rightPathRight=" + Bool01(rightPathRightSide) +
            " leftEuler=(" + leftEuler.x.ToString("F1", CultureInfo.InvariantCulture) + "," +
                leftEuler.y.ToString("F1", CultureInfo.InvariantCulture) + "," +
                leftEuler.z.ToString("F1", CultureInfo.InvariantCulture) + ")" +
            " rightEuler=(" + rightEuler.x.ToString("F1", CultureInfo.InvariantCulture) + "," +
                rightEuler.y.ToString("F1", CultureInfo.InvariantCulture) + "," +
                rightEuler.z.ToString("F1", CultureInfo.InvariantCulture) + ")");
    }

    private void LogHoldHandTarget(
        string controller,
        string mode,
        bool rightHand,
        Vector3 baseTarget,
        Vector3 sideTarget,
        Vector3 finalTarget,
        Vector3 root,
        Vector3 side,
        bool offsetRightSide,
        Vector3 center,
        bool immediate)
    {
        if (!IsDebugEnabled())
            return;

        FreeControllerV3 fc = rightHand ? rHandControl : lHandControl;
        Vector3 start = Vector3.zero;
        if (fc != null && !grabStartPositions.TryGetValue(fc, out start))
            start = fc.control != null ? fc.control.position : fc.transform.position;

        float t = immediate ? 1.0f : GetMoveTLinear();
        Vector3 next = Vector3.Lerp(start, finalTarget, t);

        Vector3 actorForward = GetSelectedPersonForwardAxis();
        Vector3 targetForward = GetTargetPersonForwardAxis();
        float facingDot = 0.0f;
        if (actorForward.sqrMagnitude > 0.0001f && targetForward.sqrMagnitude > 0.0001f)
            facingDot = Vector3.Dot(actorForward.normalized, targetForward.normalized);

        bool sameFacing = IsSameFacingTargetPerson();
        bool positionFront = IsGrabberInFrontOfTargetPerson(center);
        bool swapPaths = ShouldSwapSidePaths(center);
        bool crossedTargets = mode == "face";
        float startSideCoord = Vector3.Dot(start - center, side);
        float endSideCoord = Vector3.Dot(finalTarget - center, side);
        float baseSideCoord = Vector3.Dot(baseTarget - center, side);
        float sideTargetSideCoord = Vector3.Dot(sideTarget - center, side);
        float maxReach = GetMaxHandReach();
        float distance = (sideTarget - root).magnitude;
        bool reachClamp = (finalTarget - sideTarget).sqrMagnitude > 0.000001f;
        float finalError = (finalTarget - sideTarget).magnitude;

        string holdHandTargetMessage = "[HOLD HAND TARGET] controller=" + controller +
            " mode=" + mode +
            " hand=" + (rightHand ? "R" : "L") +
            " immediate=" + Bool01(immediate) +
            " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
            " rootDot=" + facingDot.ToString("F3", CultureInfo.InvariantCulture) +
            " sameFacing=" + Bool01(sameFacing) +
            " posFront=" + Bool01(positionFront) +
            " backSide=" + Bool01(!positionFront) +
            " swapPaths=" + Bool01(swapPaths) +
            " crossed=" + Bool01(crossedTargets) +
            " offsetRight=" + Bool01(offsetRightSide) +
            " startSide=" + startSideCoord.ToString("F3", CultureInfo.InvariantCulture) +
            " endSide=" + endSideCoord.ToString("F3", CultureInfo.InvariantCulture) +
            " baseSide=" + baseSideCoord.ToString("F3", CultureInfo.InvariantCulture) +
            " sideTargetSide=" + sideTargetSideCoord.ToString("F3", CultureInfo.InvariantCulture) +
            " follow=" + GetFollowMode() +
            " hug=" + Bool01(IsHugMode()) +
            " grabWidth=" + GetGrabWidth().ToString("F3", CultureInfo.InvariantCulture) +
            " finalWidth=" + GetFinalGrabWidth().ToString("F3", CultureInfo.InvariantCulture) +
            " maxReach=" + maxReach.ToString("F3", CultureInfo.InvariantCulture) +
            " distRootToSide=" + distance.ToString("F3", CultureInfo.InvariantCulture) +
            " finalError=" + finalError.ToString("F3", CultureInfo.InvariantCulture) +
            " reachClamp=" + Bool01(reachClamp) +
            " side=" + FormatVector3(side) +
            " center=" + FormatVector3(center) +
            " root=" + FormatVector3(root) +
            " base=" + FormatVector3(baseTarget) +
            " sideTarget=" + FormatVector3(sideTarget) +
            " pathStart=" + FormatVector3(start) +
            " pathNext=" + FormatVector3(next) +
            " pathEnd=" + FormatVector3(finalTarget);

        DebugLog(holdHandTargetMessage);
    }

    private void LogHandTargetDebug(
        bool rightHand,
        FreeControllerV3 fc,
        Vector3 root,
        Vector3 desired,
        Vector3 finalTarget,
        Vector3 center,
        Vector3 handCenter,
        Vector3 side,
        float handPathWidth,
        bool pathRightSide,
        bool swapSidePaths,
        bool immediate)
    {
        bool forceNormal = IsHugBodyTarget() || IsChestHoldTarget();
        if (!IsDebugEnabled() && !forceNormal)
            return;

        Vector3 start = Vector3.zero;
        if (fc != null && !grabStartPositions.TryGetValue(fc, out start))
            start = GetControlPosition(fc);

        Vector3 current = GetControlPosition(fc);
        float t = immediate ? 1.0f : GetMoveTLinear();
        Vector3 next = Vector3.Lerp(start, finalTarget, t);

        Vector3 actorForward = GetSelectedPersonForwardAxis();
        Vector3 targetForward = GetTargetPersonForwardAxis();
        float facingDot = 0.0f;
        if (actorForward.sqrMagnitude > 0.0001f && targetForward.sqrMagnitude > 0.0001f)
            facingDot = Vector3.Dot(actorForward.normalized, targetForward.normalized);

        bool positionFront = IsTargetPersonMode() && selectedTargetPerson != null
            ? IsGrabberInFrontOfTargetPerson(center)
            : false;

        float startSideCoord = 0.0f;
        float currentSideCoord = 0.0f;
        float desiredSideCoord = 0.0f;
        float endSideCoord = 0.0f;
        if (side.sqrMagnitude > 0.0001f)
        {
            Vector3 sideAxis = side.normalized;
            startSideCoord = Vector3.Dot(start - center, sideAxis);
            currentSideCoord = Vector3.Dot(current - center, sideAxis);
            desiredSideCoord = Vector3.Dot(desired - center, sideAxis);
            endSideCoord = Vector3.Dot(finalTarget - center, sideAxis);
        }

        float maxReach = GetMaxHandReach();
        float rootToDesired = (desired - root).magnitude;
        float rootToFinal = (finalTarget - root).magnitude;
        float finalError = (finalTarget - desired).magnitude;
        bool targetChanged = (finalTarget - desired).sqrMagnitude > 0.000001f;
        bool reachClamp = targetChanged && maxReach > 0.0001f && rootToFinal >= maxReach - 0.0005f;

        string handTargetMessage = "[HAND TARGET] controller=" + (targetPersonPartChooser != null ? targetPersonPartChooser.val : "<null>") +
            " hand=" + (rightHand ? "R" : "L") +
            " immediate=" + Bool01(immediate) +
            " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
            " rootDot=" + facingDot.ToString("F3", CultureInfo.InvariantCulture) +
            " sameFacing=" + Bool01(IsSameFacingTargetPerson()) +
            " posFront=" + Bool01(positionFront) +
            " backSide=" + Bool01(!positionFront) +
            " swapPaths=" + Bool01(swapSidePaths) +
            " pathRight=" + Bool01(pathRightSide) +
            " hug=" + Bool01(IsHugMode()) +
            " hugBody=" + Bool01(IsHugBodyTarget()) +
            " follow=" + GetFollowMode() +
            " grabWidth=" + GetGrabWidth().ToString("F3", CultureInfo.InvariantCulture) +
            " handWidth=" + handPathWidth.ToString("F3", CultureInfo.InvariantCulture) +
            " finalWidth=" + GetFinalGrabWidth().ToString("F3", CultureInfo.InvariantCulture) +
            " maxReach=" + maxReach.ToString("F3", CultureInfo.InvariantCulture) +
            " rootToDesired=" + rootToDesired.ToString("F3", CultureInfo.InvariantCulture) +
            " rootToFinal=" + rootToFinal.ToString("F3", CultureInfo.InvariantCulture) +
            " finalError=" + finalError.ToString("F3", CultureInfo.InvariantCulture) +
            " targetChanged=" + Bool01(targetChanged) +
            " reachClamp=" + Bool01(reachClamp) +
            " startSide=" + startSideCoord.ToString("F3", CultureInfo.InvariantCulture) +
            " currentSide=" + currentSideCoord.ToString("F3", CultureInfo.InvariantCulture) +
            " desiredSide=" + desiredSideCoord.ToString("F3", CultureInfo.InvariantCulture) +
            " endSide=" + endSideCoord.ToString("F3", CultureInfo.InvariantCulture) +
            " side=" + FormatVector3(side) +
            " center=" + FormatVector3(center) +
            " handCenter=" + FormatVector3(handCenter) +
            " root=" + FormatVector3(root) +
            " desired=" + FormatVector3(desired) +
            " pathStart=" + FormatVector3(start) +
            " pathCurrent=" + FormatVector3(current) +
            " pathNext=" + FormatVector3(next) +
            " pathEnd=" + FormatVector3(finalTarget);

        DebugLog(handTargetMessage);
    }

    private void LogChestHoldHugBodyRouteAlways(
        string stage,
        bool includeHands,
        bool includeFeet,
        bool includeHead,
        bool useFinalGrabWidth,
        Vector3 center,
        bool nipplePairRoute,
        bool hipHoldMode,
        bool targetPairMode,
        bool force)
    {
        bool hugBody = IsHugBodyTarget();
        bool strictChest = IsChestHoldTarget();
        bool nippleCompat = IsNipplePairMode();
        if (!force && !hugBody && !strictChest && !nippleCompat)
            return;

        if (!force && Time.time - lastRouteCheckLogTime < 0.50f)
            return;
        lastRouteCheckLogTime = Time.time;

        string choice = targetPersonPartChooser != null ? targetPersonPartChooser.val : "<null>";
        string actual = GetTargetControllerActualName(choice);
        string targetUid = selectedTargetPerson != null ? selectedTargetPerson.uid : "<null>";
        string selfUid = selectedPerson != null ? selectedPerson.uid : "<null>";

        FreeControllerV3 targetChest = null;
        FreeControllerV3 lNipple = null;
        FreeControllerV3 rNipple = null;
        if (selectedTargetPerson != null)
        {
            targetChest = GetTargetPersonControlByAliases("chestControl", "chest");
            lNipple = GetTargetPersonControlByAliases("lNippleControl", "leftNippleControl", "lNipple", "lnipple", "leftNipple", "LeftNipple", "nipple_l", "nippleL");
            rNipple = GetTargetPersonControlByAliases("rNippleControl", "rightNippleControl", "rNipple", "rnipple", "rightNipple", "RightNipple", "nipple_r", "nippleR");
        }

        Vector3 chestPos = GetControlPositionSafe(targetChest);
        Vector3 lPos = GetControlPositionSafe(lNipple);
        Vector3 rPos = GetControlPositionSafe(rNipple);
        float chestDelta = targetChest != null ? (center - chestPos).magnitude : -1.0f;
        float lDist = lNipple != null ? (center - lPos).magnitude : -1.0f;
        float rDist = rNipple != null ? (center - rPos).magnitude : -1.0f;

        DebugLog("[ROUTE CHECK] stage=" + stage +
            " / choice=" + choice +
            " / actual=" + actual +
            " / self=" + selfUid +
            " / target=" + targetUid +
            " / hugBody=" + Bool01(hugBody) +
            " / strictChest=" + Bool01(strictChest) +
            " / nippleCompat=" + Bool01(nippleCompat) +
            " / nippleRoute=" + Bool01(nipplePairRoute) +
            " / hipHold=" + Bool01(hipHoldMode) +
            " / targetPair=" + Bool01(targetPairMode) +
            " / includeHands=" + Bool01(includeHands) +
            " / includeFeet=" + Bool01(includeFeet) +
            " / includeHead=" + Bool01(includeHead) +
            " / useFinalWidth=" + Bool01(useFinalGrabWidth) +
            " / follow=" + GetFollowMode() +
            " / active=" + Bool01(hasActiveGrab) +
            " / t=" + GetMoveTLinear().ToString("F3", CultureInfo.InvariantCulture) +
            " / center=" + FormatVector3(center) +
            " / targetChest=" + (targetChest != null ? FormatVector3(chestPos) : "<none>") +
            " / centerToTargetChest=" + chestDelta.ToString("F3", CultureInfo.InvariantCulture) +
            " / lNipple=" + (lNipple != null ? FormatVector3(lPos) : "<none>") +
            " / rNipple=" + (rNipple != null ? FormatVector3(rPos) : "<none>") +
            " / centerToL=" + lDist.ToString("F3", CultureInfo.InvariantCulture) +
            " / centerToR=" + rDist.ToString("F3", CultureInfo.InvariantCulture));
    }

    private Vector3 GetControlPositionSafe(FreeControllerV3 fc)
    {
        if (fc == null)
            return Vector3.zero;

        try
        {
            if (fc.control != null)
                return fc.control.position;
            return fc.transform.position;
        }
        catch
        {
            return Vector3.zero;
        }
    }

    private string FormatVector3(Vector3 value)
    {
        return "(" +
            value.x.ToString("F3", CultureInfo.InvariantCulture) + "," +
            value.y.ToString("F3", CultureInfo.InvariantCulture) + "," +
            value.z.ToString("F3", CultureInfo.InvariantCulture) + ")";
    }


    private string FormatQuaternion(Quaternion value)
    {
        return "(" +
            value.x.ToString("F3", CultureInfo.InvariantCulture) + "," +
            value.y.ToString("F3", CultureInfo.InvariantCulture) + "," +
            value.z.ToString("F3", CultureInfo.InvariantCulture) + "," +
            value.w.ToString("F3", CultureInfo.InvariantCulture) + ")";
    }

    private static Quaternion NormalizeQuaternionRaw(Quaternion q)
    {
        float m = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
        if (m <= 0.0001f)
            return Quaternion.identity;

        return new Quaternion(q.x / m, q.y / m, q.z / m, q.w / m);
    }

    private Quaternion NormalizeQuaternion(Quaternion q)
    {
        return NormalizeQuaternionRaw(q);
    }

    private bool IsTargetOnPositiveSide(Vector3 target, Vector3 center, Vector3 side)
    {
        if (side.sqrMagnitude < 0.0001f)
            return true;

        return Vector3.Dot(target - center, side.normalized) >= 0.0f;
    }

    private Vector3 GetPairOutsidePoint(Vector3 target, Vector3 center, Vector3 side, float offset)
    {
        if (side.sqrMagnitude < 0.0001f || offset <= 0.0f)
            return target;

        Vector3 sideAxis = side.normalized;
        bool positiveSide = Vector3.Dot(target - center, sideAxis) >= 0.0f;
        return target + (positiveSide ? sideAxis : -sideAxis) * offset;
    }

    private void LogHoldTargetOrder(string controller, string mode, Vector3 rawLeft, Vector3 rawRight, Vector3 orderedLeft, Vector3 orderedRight, Vector3 center, Vector3 side)
    {
        if (!IsDebugEnabled())
            return;

        Vector3 sideAxis = side.sqrMagnitude > 0.0001f ? side.normalized : Vector3.right;
        float rawLeftSide = Vector3.Dot(rawLeft - center, sideAxis);
        float rawRightSide = Vector3.Dot(rawRight - center, sideAxis);
        float orderedLeftSide = Vector3.Dot(orderedLeft - center, sideAxis);
        float orderedRightSide = Vector3.Dot(orderedRight - center, sideAxis);
        float leftStartSide = GetHoldHandStartSide(false, center, sideAxis);
        float rightStartSide = GetHoldHandStartSide(true, center, sideAxis);

        DebugLog("[HOLD ORDER] controller=" + controller +
            " mode=" + mode +
            " leftStartSide=" + leftStartSide.ToString("F3", CultureInfo.InvariantCulture) +
            " rightStartSide=" + rightStartSide.ToString("F3", CultureInfo.InvariantCulture) +
            " rawLeftSide=" + rawLeftSide.ToString("F3", CultureInfo.InvariantCulture) +
            " rawRightSide=" + rawRightSide.ToString("F3", CultureInfo.InvariantCulture) +
            " orderedLeftSide=" + orderedLeftSide.ToString("F3", CultureInfo.InvariantCulture) +
            " orderedRightSide=" + orderedRightSide.ToString("F3", CultureInfo.InvariantCulture) +
            " rawLeft=" + FormatVector3(rawLeft) +
            " rawRight=" + FormatVector3(rawRight) +
            " orderedLeft=" + FormatVector3(orderedLeft) +
            " orderedRight=" + FormatVector3(orderedRight));
    }

    private void LogHoldFootTargetOrder(string controller, string mode, Vector3 rawLeft, Vector3 rawRight, Vector3 orderedLeft, Vector3 orderedRight, Vector3 center, Vector3 side)
    {
        if (!IsDebugEnabled())
            return;

        Vector3 sideAxis = side.sqrMagnitude > 0.0001f ? side.normalized : Vector3.right;
        float rawLeftSide = Vector3.Dot(rawLeft - center, sideAxis);
        float rawRightSide = Vector3.Dot(rawRight - center, sideAxis);
        float orderedLeftSide = Vector3.Dot(orderedLeft - center, sideAxis);
        float orderedRightSide = Vector3.Dot(orderedRight - center, sideAxis);
        float leftStartSide = GetHoldFootStartSide(false, center, sideAxis);
        float rightStartSide = GetHoldFootStartSide(true, center, sideAxis);

        DebugLog("[HOLD FOOT ORDER] controller=" + controller +
            " mode=" + mode +
            " leftStartSide=" + leftStartSide.ToString("F3", CultureInfo.InvariantCulture) +
            " rightStartSide=" + rightStartSide.ToString("F3", CultureInfo.InvariantCulture) +
            " rawLeftSide=" + rawLeftSide.ToString("F3", CultureInfo.InvariantCulture) +
            " rawRightSide=" + rawRightSide.ToString("F3", CultureInfo.InvariantCulture) +
            " orderedLeftSide=" + orderedLeftSide.ToString("F3", CultureInfo.InvariantCulture) +
            " orderedRightSide=" + orderedRightSide.ToString("F3", CultureInfo.InvariantCulture) +
            " rawLeft=" + FormatVector3(rawLeft) +
            " rawRight=" + FormatVector3(rawRight) +
            " orderedLeft=" + FormatVector3(orderedLeft) +
            " orderedRight=" + FormatVector3(orderedRight));
    }

    private void LogHoldFootTarget(string controller, string mode, bool rightFoot, Vector3 desired, Vector3 root, Vector3 side, bool pathRightSide, Vector3 center, bool immediate)
    {
        if (!IsDebugEnabled())
            return;

        FreeControllerV3 fc = rightFoot ? rFootControl : lFootControl;
        Vector3 start = Vector3.zero;
        if (fc != null && !grabStartPositions.TryGetValue(fc, out start))
            start = fc.control != null ? fc.control.position : fc.transform.position;

        Vector3 finalTarget = GetReachLimitedPosition(root, desired, GetMaxFootReach(), GetFootSoleOffset(), fc, false, pathRightSide);
        float t = immediate ? 1.0f : GetMoveTLinear();
        float startSide = Vector3.Dot(start - center, side);
        float desiredSide = Vector3.Dot(desired - center, side);
        float finalSide = Vector3.Dot(finalTarget - center, side);
        float finalError = (finalTarget - desired).magnitude;

        DebugLog("[HOLD FOOT TARGET] controller=" + controller +
            " mode=" + mode +
            " foot=" + (rightFoot ? "R" : "L") +
            " pathRight=" + Bool01(pathRightSide) +
            " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
            " startSide=" + startSide.ToString("F3", CultureInfo.InvariantCulture) +
            " desiredSide=" + desiredSide.ToString("F3", CultureInfo.InvariantCulture) +
            " finalSide=" + finalSide.ToString("F3", CultureInfo.InvariantCulture) +
            " finalError=" + finalError.ToString("F3", CultureInfo.InvariantCulture) +
            " maxReach=" + GetMaxFootReach().ToString("F3", CultureInfo.InvariantCulture) +
            " side=" + FormatVector3(side) +
            " center=" + FormatVector3(center) +
            " root=" + FormatVector3(root) +
            " desired=" + FormatVector3(desired) +
            " final=" + FormatVector3(finalTarget) +
            " pathStart=" + FormatVector3(start));
    }

    private void OrderHoldTargetsForHands(ref Vector3 leftHandTarget, ref Vector3 rightHandTarget, Vector3 center, Vector3 side)
    {
        if (side.sqrMagnitude < 0.0001f)
            return;

        Vector3 sideAxis = side.normalized;
        float leftSide = Vector3.Dot(leftHandTarget - center, sideAxis);
        float rightSide = Vector3.Dot(rightHandTarget - center, sideAxis);
        float leftStartSide = GetHoldHandStartSide(false, center, sideAxis);
        float rightStartSide = GetHoldHandStartSide(true, center, sideAxis);

        Vector3 highTarget = leftSide >= rightSide ? leftHandTarget : rightHandTarget;
        Vector3 lowTarget = leftSide >= rightSide ? rightHandTarget : leftHandTarget;

        if (leftStartSide >= rightStartSide)
        {
            leftHandTarget = highTarget;
            rightHandTarget = lowTarget;
        }
        else
        {
            leftHandTarget = lowTarget;
            rightHandTarget = highTarget;
        }
    }

    private void OrderHoldTargetsForFeet(ref Vector3 leftFootTarget, ref Vector3 rightFootTarget, Vector3 center, Vector3 side)
    {
        if (side.sqrMagnitude < 0.0001f)
            return;

        Vector3 sideAxis = side.normalized;
        float leftSide = Vector3.Dot(leftFootTarget - center, sideAxis);
        float rightSide = Vector3.Dot(rightFootTarget - center, sideAxis);
        float leftStartSide = GetHoldFootStartSide(false, center, sideAxis);
        float rightStartSide = GetHoldFootStartSide(true, center, sideAxis);

        Vector3 highTarget = leftSide >= rightSide ? leftFootTarget : rightFootTarget;
        Vector3 lowTarget = leftSide >= rightSide ? rightFootTarget : leftFootTarget;

        if (leftStartSide >= rightStartSide)
        {
            leftFootTarget = highTarget;
            rightFootTarget = lowTarget;
        }
        else
        {
            leftFootTarget = lowTarget;
            rightFootTarget = highTarget;
        }
    }

    private float GetHoldHandStartSide(bool rightHand, Vector3 center, Vector3 sideAxis)
    {
        FreeControllerV3 fc = rightHand ? rHandControl : lHandControl;
        Vector3 pos = Vector3.zero;
        if (fc != null && !grabStartPositions.TryGetValue(fc, out pos))
            pos = fc.control != null ? fc.control.position : fc.transform.position;

        return Vector3.Dot(pos - center, sideAxis);
    }

    private float GetHoldFootStartSide(bool rightFoot, Vector3 center, Vector3 sideAxis)
    {
        FreeControllerV3 fc = rightFoot ? rFootControl : lFootControl;
        Vector3 pos = Vector3.zero;
        if (fc != null && !grabStartPositions.TryGetValue(fc, out pos))
            pos = fc.control != null ? fc.control.position : fc.transform.position;

        return Vector3.Dot(pos - center, sideAxis);
    }

    private FreeControllerV3 GetTargetPersonPartControl()
    {
        if (selectedTargetPerson == null)
            return null;

        // lrnipple は通常Controllerではなく、左右乳首ペア用の特殊キーとして扱う。
        // ここで通常FreeControllerとして返すと、中心ターゲット扱いになってしまうため除外する。
        if (IsNipplePairMode())
            return null;

        string choice = targetPersonPartChooser != null ? targetPersonPartChooser.val : NONE;
        string controlName = GetTargetControllerActualName(choice);
        if (string.IsNullOrEmpty(controlName) || controlName == NONE)
            return null;

        return GetControlFromAtom(selectedTargetPerson, controlName);
    }

    private FreeControllerV3 GetControlFromAtom(Atom atom, string name)
    {
        if (atom == null || string.IsNullOrEmpty(name))
            return null;

        if (atom == selectedPerson)
        {
            EnsurePersonControlCache();
            FreeControllerV3 personFc;
            return personControlCache.TryGetValue(name, out personFc) ? personFc : null;
        }

        if (atom == selectedTargetPerson)
        {
            EnsureTargetPersonControlCache();
            FreeControllerV3 targetFc;
            return targetPersonControlCache.TryGetValue(name, out targetFc) ? targetFc : null;
        }

        foreach (FreeControllerV3 fc in atom.GetComponentsInChildren<FreeControllerV3>(true))
        {
            if (fc != null && fc.name == name)
                return fc;
        }

        return null;
    }

    private float GetGrabWidth()
    {
        // v2.0g:
        // スライダー表示値は必ずプラスの幅として扱う。
        // ここでは符号反転しない。
        // 左右の反転は ApplyGrab の配置式だけで行う。
        return Mathf.Max(0.0f, currentGrabWidth);
    }

    private bool IsHugMode()
    {
        return hugModeJSON != null && hugModeJSON.val;
    }

    private bool IsHugBodyTarget()
    {
        return IsTargetPersonMode() &&
               targetPersonPartChooser != null &&
               targetPersonPartChooser.val == TC_HUG_BODY;
    }

    private float GetFinalGrabWidth()
    {
        // 0.000 はIK/配置計算で潰れやすいので、内部の最終幅は必ず少し残す。
        // Hug Mode でも完全ゼロには閉じず、ログ上も finalWidth=0.010 になる。
        if (IsHugMode())
            return MIN_FINAL_GRAB_WIDTH;

        // UI上の Final Grab Width は「左右の実距離」として扱う。
        // 配置計算は center ± width の半幅方式なので、内部値は半分にする。
        // ただし UI値が0/極小の場合も、内部値は MIN_FINAL_GRAB_WIDTH を下限にする。
        float width = finalGrabWidthJSON != null
            ? Mathf.Max(0.0f, finalGrabWidthJSON.val)
            : 0.10f;

        return Mathf.Max(MIN_FINAL_GRAB_WIDTH, width * 0.5f);
    }

    private float GetHugDepth()
    {
        return hugDepthJSON != null ? hugDepthJSON.val : 0.30f;
    }

    private Vector3 GetTargetForwardAxis()
    {
        return GetTargetForwardAxis(GetTargetRotation());
    }

    private Vector3 GetTargetForwardAxis(Quaternion rot)
    {
        Vector3 axis = rot * Vector3.forward;
        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.forward;

        axis.Normalize();

        if (IsSameFacingTargetPerson())
            axis = -axis;

        return axis;
    }

    private Vector3 GetHugCenter(Vector3 center)
    {
        if (!IsHugMode())
            return center;

        float t = GetMoveTLinear();

        // v4.0ai:
        // Hug Body の一度奥へ送る方向は、向きではなく現在の位置関係で決める。
        // 自分側の胸/腰/root から target center へ向かう水平ベクトルの延長へ送る。
        // これで立ち位置によって「一旦前へ出す」が逆に見えるケースを避ける。
        Vector3 deepCenter = center + GetHugForwardAxis(center) * Mathf.Abs(GetHugDepth());

        // 前半は対象物の奥を狙い、後半で通常中心へ戻す。
        if (t < 0.50f)
            return deepCenter;

        float u = Mathf.Clamp01((t - 0.50f) / 0.50f);
        return Vector3.Lerp(deepCenter, center, u);
    }

    private Vector3 GetHugForwardAxis(Vector3 center)
    {
        Vector3 origin = GetHugOriginPosition(center);
        Vector3 axis = center - origin;
        axis.y = 0.0f;

        if (axis.sqrMagnitude < 0.0001f)
        {
            axis = GetSelectedPersonForwardAxis();
            axis.y = 0.0f;
        }

        if (axis.sqrMagnitude < 0.0001f)
        {
            axis = GetTargetForwardAxis();
            axis.y = 0.0f;
        }

        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.forward;

        return axis.normalized;
    }

    private Vector3 GetHugOriginPosition(Vector3 center)
    {
        if (chestControl != null)
            return chestControl.control != null ? chestControl.control.position : chestControl.transform.position;

        if (hipControl != null)
            return hipControl.control != null ? hipControl.control.position : hipControl.transform.position;

        if (selectedPerson != null && selectedPerson.transform != null)
            return selectedPerson.transform.position;

        return center - Vector3.forward;
    }

    private void AutoCloseGrabWidth()
    {
        // Grab Width はUI値を維持し、内部の currentGrabWidth だけを閉じる。
        // Hug Mode時も完全0には閉じず、GetFinalGrabWidth() の最小幅へ閉じる。
        currentGrabWidth = Mathf.Lerp(grabStartWidth, GetFinalGrabWidth(), GetMoveTLinear());
    }

    private float GetMoveTLinear()
    {
        float duration = moveTimeJSON != null ? Mathf.Max(0.05f, moveTimeJSON.val) : 1.0f;
        duration *= Mathf.Max(0.05f, activeMoveTimeMultiplier);
        return Mathf.Clamp01(grabElapsed / duration);
    }

    private float GetMaxHandReach()
    {
        return maxHandReachJSON != null ? Mathf.Max(0.05f, maxHandReachJSON.val) : 0.70f;
    }

    private float GetMaxFootReach()
    {
        return maxFootReachJSON != null ? Mathf.Max(0.05f, maxFootReachJSON.val) : 0.80f;
    }

    private float GetMaxHeadReach()
    {
        return maxHeadReachJSON != null ? Mathf.Max(0.05f, maxHeadReachJSON.val) : 0.45f;
    }

    private float GetHeadTargetDistance()
    {
        return headTargetDistanceJSON != null ? Mathf.Max(0.0f, headTargetDistanceJSON.val) : 0.12f;
    }

    private bool ShouldKissFaceAlign()
    {
        return kissFaceAlignJSON != null && kissFaceAlignJSON.val;
    }

    private float GetKissFaceStrength()
    {
        return kissFaceStrengthJSON != null ? Mathf.Clamp01(kissFaceStrengthJSON.val) : 0.70f;
    }

    private float GetHandPalmOffset()
    {
        return handPalmOffsetJSON != null ? handPalmOffsetJSON.val : 0.08f;
    }

    private float GetHandCenterOffset()
    {
        return 0.0f;
    }

    private float GetFootSoleOffset()
    {
        return footSoleOffsetJSON != null ? footSoleOffsetJSON.val : 0.08f;
    }

    private float GetKneeWidthMultiplier()
    {
        return kneeWidthMultiplierJSON != null ? Mathf.Max(0.0f, kneeWidthMultiplierJSON.val) : 2.00f;
    }

    private float GetFootArcWidth()
    {
        return footArcWidthJSON != null ? Mathf.Max(0.0f, footArcWidthJSON.val) : 0.30f;
    }

    private float GetFootArcDrop()
    {
        return footArcDropJSON != null ? Mathf.Max(0.0f, footArcDropJSON.val) : 0.10f;
    }

    private Vector3 GetBodyUpAxis()
    {
        if (selectedPerson != null && selectedPerson.transform != null)
        {
            Vector3 up = selectedPerson.transform.up;
            if (up.sqrMagnitude > 0.0001f)
                return up.normalized;
        }

        return Vector3.up;
    }

    private Vector3 GetBodyDownAxis()
    {
        return -GetBodyUpAxis();
    }

    private Vector3 ClampFootArcBelowKnee(Vector3 pos, FreeControllerV3 kneeControl, float margin)
    {
        if (kneeControl == null)
            return pos;

        Vector3 kneePos = kneeControl.control != null ? kneeControl.control.position : kneeControl.transform.position;
        Vector3 bodyUp = GetBodyUpAxis();

        float footHeight = Vector3.Dot(pos, bodyUp);
        float kneeHeight = Vector3.Dot(kneePos, bodyUp);
        float maxFootHeight = kneeHeight - Mathf.Max(0.0f, margin);

        if (footHeight > maxFootHeight)
            pos -= bodyUp * (footHeight - maxFootHeight);

        return pos;
    }

    private Vector3 GetFootOutwardDirection(bool rightSide, Vector3 side)
    {
        // Reverse Foot Sideは廃止。通常方向に固定する。
        Vector3 dir = GetSideOffset(rightSide, side, 1.0f);
        if (dir.sqrMagnitude > 0.0001f)
            return dir.normalized;
        return Vector3.zero;
    }

    private void MoveFootControlWithArc(FreeControllerV3 fc, FreeControllerV3 kneeControl, Vector3 root, Vector3 finalDesired, Vector3 side, bool rightSide, bool immediate)
    {
        if (fc == null)
            return;

        Vector3 outward = GetFootOutwardDirection(rightSide, side);
        Vector3 midDesired = finalDesired + outward * GetFootArcWidth() + GetBodyDownAxis() * GetFootArcDrop();
        midDesired = ClampFootArcBelowKnee(midDesired, kneeControl, 0.05f);

        Vector3 finalTarget = GetReachLimitedPosition(root, finalDesired, GetMaxFootReach(), GetFootSoleOffset(), fc, false, rightSide);
        Vector3 midTarget = GetReachLimitedPosition(root, midDesired, GetMaxFootReach(), GetFootSoleOffset(), fc, false, rightSide);
        midTarget = ClampFootArcBelowKnee(midTarget, kneeControl, 0.05f);

        if (immediate)
        {
            MoveControl(fc, finalTarget, Quaternion.identity, false, true);
            return;
        }

        EnsurePositionStateOn(fc);

        float t = GetMoveTLinear();

        Vector3 start;
        if (!grabStartPositions.TryGetValue(fc, out start))
            start = fc.control != null ? fc.control.position : fc.transform.position;

        Vector3 next;
        if (t < 0.50f)
        {
            float u = Mathf.Clamp01(t / 0.50f);
            next = Vector3.Lerp(start, midTarget, u);
        }
        else
        {
            float u = Mathf.Clamp01((t - 0.50f) / 0.50f);
            next = Vector3.Lerp(midTarget, finalTarget, u);
        }

        fc.transform.position = next;
        if (fc.control != null)
            fc.control.position = next;
    }

    private void MovePairHandControlWithGrabWidthMidpoint(FreeControllerV3 fc, Vector3 finalTarget, Vector3 center, bool actualRightHand, bool immediate)
    {
        if (fc == null)
            return;

        Vector3 depthAxis = GetFinalPointDepthAxis(center);
        Vector3 viewRight = GetFinalPointSideAxis(depthAxis, Vector3.right);
        float openWidth = GetGrabWidth();
        Vector3 midTarget = finalTarget + (actualRightHand ? viewRight : -viewRight) * openWidth;

        if (immediate)
        {
            MoveControl(fc, finalTarget, Quaternion.identity, false, true);
            ApplyPairFinalWristIn(fc, center, actualRightHand, true);
            return;
        }

        EnsurePositionStateOn(fc);

        float t = GetMoveTLinear();

        Vector3 start;
        if (!grabStartPositions.TryGetValue(fc, out start))
            start = fc.control != null ? fc.control.position : fc.transform.position;

        Vector3 next;
        if (t < 0.50f)
        {
            float u = Mathf.Clamp01(t / 0.50f);
            next = Vector3.Lerp(start, midTarget, u);
        }
        else
        {
            float u = Mathf.Clamp01((t - 0.50f) / 0.50f);
            next = Vector3.Lerp(midTarget, finalTarget, u);
        }

        fc.transform.position = next;
        if (fc.control != null)
            fc.control.position = next;

        if (IsDebugEnabled())
        {
            DebugLog("[PAIR HAND MID ROUTE] hand=" + (actualRightHand ? "R" : "L") +
                " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
                " openWidth=" + openWidth.ToString("F3", CultureInfo.InvariantCulture) +
                " viewRight=" + FormatVector3(viewRight) +
                " mid=" + FormatVector3(midTarget) +
                " final=" + FormatVector3(finalTarget) +
                " current=" + FormatVector3(GetControlPosition(fc)));
        }

        if (t >= 1.0f)
            ApplyPairFinalWristIn(fc, center, actualRightHand, false);
    }

    private void ApplyPairFinalWristIn(FreeControllerV3 fc, Vector3 center, bool actualRightHand, bool immediate)
    {
        if (fc == null)
            return;

        if (!ShouldAlignHandPalm())
        {
            if (IsDebugEnabled())
                DebugLog("[PAIR WRIST IN SKIP] reason=align-hand-palm-off hand=" + (actualRightHand ? "R" : "L"));
            return;
        }

        Vector3 movedPosition = GetControlPosition(fc);
        bool frontSide = IsTargetPersonMode() && selectedTargetPerson != null
            ? IsGrabberInFrontOfTargetPerson(center)
            : false;
        Quaternion baseRotation = GetFixedHandBaseRotation(GetHandRotationOffset(), actualRightHand, actualRightHand, frontSide);
        Quaternion fallbackRotation = ApplyHandWristMode(baseRotation, actualRightHand, "In");
        Quaternion finalRotation = GetWristButtonHandWorldRotation(actualRightHand, "In", fallbackRotation);

        MoveControl(fc, movedPosition, finalRotation, true, true);

        if (IsDebugEnabled())
        {
            Vector3 finalEuler = finalRotation.eulerAngles;
            DebugLog("[PAIR WRIST IN] hand=" + (actualRightHand ? "R" : "L") +
                " apply=button-preset" +
                " immediate=" + Bool01(immediate) +
                " pos=" + FormatVector3(movedPosition) +
                " center=" + FormatVector3(center) +
                " finalEuler=(" + finalEuler.x.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    finalEuler.y.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    finalEuler.z.ToString("F1", CultureInfo.InvariantCulture) + ")");
        }
    }

    private Vector3 GetSideOffset(bool rightSide, Vector3 side, float width)
    {
        // 通常: Left=-side / Right=+side
        return (rightSide ? side : -side) * width;
    }

    private Vector3 GetFootLateralOffset(bool rightSide, Vector3 side)
    {
        // Reverse Foot Sideは廃止。通常方向に固定する。
        return GetSideOffset(rightSide, side, GetGrabWidth());
    }

    private Vector3 GetKneeTargetPosition(bool rightSide, Vector3 center, Vector3 side)
    {
        // v2.0u:
        // 膝幅は currentGrabWidth に追従するため、Grab Width から Final Grab Width へ閉じる動きと同期する。
        // Reverse系はv2.0t検証で不要だったため、通常方向に固定する。
        float kneeWidth = GetGrabWidth() * GetKneeWidthMultiplier();
        Vector3 lateral = GetSideOffset(rightSide, side, kneeWidth);

        // 膝の高さは、腰側rootとtarget中心の中間に置く。
        // これで足首より外側へ逃げる「ひし形」に近い形になる。
        Vector3 root = GetFootRootPosition(rightSide);
        Vector3 basePos = Vector3.Lerp(root, center, 0.50f);

        return basePos + lateral;
    }

    private Vector3 GetHandRootPosition(bool right)
    {
        if (chestControl != null)
            return chestControl.transform.position;

        if (hipControl != null)
            return hipControl.transform.position + Vector3.up * 0.35f;

        return selectedPerson != null ? selectedPerson.transform.position + Vector3.up * 1.2f : Vector3.zero;
    }

    private Vector3 GetFootRootPosition(bool right)
    {
        if (hipControl != null)
            return hipControl.transform.position;

        return selectedPerson != null ? selectedPerson.transform.position + Vector3.up * 0.8f : Vector3.zero;
    }

    private Vector3 GetReachLimitedPosition(Vector3 root, Vector3 desired, float maxReach, float contactOffset, FreeControllerV3 fc, bool hand, bool rightSide)
    {
        // v1.9c:
        // v1.9bの contactOffset は root→target方向だったため、
        // 腕の位置によって縦方向へ逃げることがあった。
        //
        // ここでは手首/足首Controlのローカル軸基準で逃がす。
        // 手: 固定手首回転プリセットから palm normal を作る。
        // 足: 現在のControl回転から sole normal を作る。
        Vector3 offsetAxis = GetContactOffsetAxis(fc, hand, rightSide);

        if (offsetAxis.sqrMagnitude > 0.0001f && Mathf.Abs(contactOffset) > 0.0001f)
            desired += offsetAxis.normalized * contactOffset;

        // v1.9j:
        // Hand Palm Offset はターゲットY方向の面合わせ用。
        // Hand Center Offset は手首IK中心から掌中央へ寄せる補正。
        // 左右の掴み位置計算は触らず、固定手首回転から同じローカル軸を取る。
        if (hand)
        {
            float centerOffset = GetHandCenterOffset();
            Vector3 centerAxis = GetHandCenterOffsetAxis(rightSide);

            if (centerAxis.sqrMagnitude > 0.0001f && Mathf.Abs(centerOffset) > 0.0001f)
                desired += centerAxis.normalized * centerOffset;
        }

        Vector3 delta = desired - root;
        float dist = delta.magnitude;

        if (dist <= maxReach || dist < 0.0001f)
            return desired;

        return root + delta.normalized * maxReach;
    }

    private Vector3 GetContactOffsetAxis(FreeControllerV3 fc, bool hand, bool rightSide)
    {
        if (hand)
        {
            // v1.9i:
            // Hand Palm Offset を左右の開きではなく、Target Atom の local Y 方向へ効かせる。
            // 左右の手で別軸を使わないため、v1.9d のような左右ズレを避ける。
            // mainController.control がある場合は、Target Z Offset と同じく Control 回転を優先する。
            Quaternion rot = GetTargetRotation();

            Vector3 axis = rot * Vector3.up;

            if (axis.sqrMagnitude > 0.0001f)
                return axis.normalized;
        }

        if (fc != null)
        {
            Quaternion rot = fc.control != null ? fc.control.rotation : fc.transform.rotation;
            Vector3 axis = -(rot * Vector3.up);

            if (axis.sqrMagnitude > 0.0001f)
                return axis.normalized;
        }

        return Vector3.zero;
    }


    private Vector3 GetHandCenterOffsetAxis(bool rightSide)
    {
        // v1.9n:
        // v1.9j の左右別固定回転 local up は、Hand Center Offset が
        // 横幅の広がり/狭まりとして出やすかったため不採用。
        // 左右対称性を壊さないよう、左手固定角度を共通基準にして local X(right) を使う。
        // rightSide は互換のため受け取るが、軸計算では使わない。
        Quaternion palmRef = Quaternion.Euler(298.76f, 51.38f, 25.61f);
        Vector3 axis = palmRef * Vector3.right;

        if (axis.sqrMagnitude > 0.0001f)
            return axis.normalized;

        return Vector3.zero;
    }


    private bool ShouldAlignHandPalm()
    {
        return alignHandPalmJSON != null && alignHandPalmJSON.val;
    }

    private bool ShouldUseHandWristAngle()
    {
        return handWristAngleJSON == null || handWristAngleJSON.val;
    }

    private bool ShouldAlignFootSole()
    {
        return alignFootSoleJSON != null && alignFootSoleJSON.val;
    }

    private void ClearPendingWristHandLocks()
    {
        pendingWristHandLocks.Clear();
        completedWristHandLocks.Clear();
        hugBodyWristReferencePositions.Clear();
        hugBodyHandSnapAnchorPositions.Clear();
    }

    private void ApplyBothHandWristTest(string mode)
    {
        ResolveControls();
        ClearPendingWristHandLocks();

        WristArmPose pose = GetWristButtonArmPose(mode);
        if (pose == null)
        {
            SetStatus("Wrist Test unknown mode: " + mode);
            return;
        }

        bool useLeft = leftHandJSON == null || leftHandJSON.val;
        bool useRight = rightHandJSON == null || rightHandJSON.val;

        if (!useLeft && !useRight)
        {
            SetStatus("Wrist Test needs checked L/R Hand");
            return;
        }

        int hands = 0;
        int elbows = 0;

        // Apply elbows first, then lock current hand positions and rotate hands.
        if (useRight)
        {
            elbows += ApplyWristButtonElbowPose(rElbowControl, true, mode, pose.RElbow);
            hands += ApplyWristButtonHandLocked(rHandControl, true, mode, pose.RHand.LocalRot);
        }

        if (useLeft)
        {
            elbows += ApplyWristButtonElbowPose(lElbowControl, false, mode, pose.LElbow);
            hands += ApplyWristButtonHandLocked(lHandControl, false, mode, pose.LHand.LocalRot);
        }

        if (hands <= 0)
        {
            SetStatus("Wrist Test needs hand controls");
            return;
        }

        SetStatus("Wrist Test / " + mode + " / hands=" + hands.ToString(CultureInfo.InvariantCulture) +
            " elbows=" + elbows.ToString(CultureInfo.InvariantCulture) + " handPosition=LOCKED");

        DebugLog("[WRIST TEST ARM] mode=" + mode +
            " hands=" + hands.ToString(CultureInfo.InvariantCulture) +
            " elbows=" + elbows.ToString(CultureInfo.InvariantCulture) +
            " handPosition=LOCKED");
    }

    private int ApplyHandWristTest(FreeControllerV3 handControl, bool actualRightHand, string mode)
    {
        if (handControl == null)
        {
            return 0;
        }

        Vector3 pos = GetControlPosition(handControl);
        Quaternion rot = GetHandWristButtonRotation(handControl, actualRightHand, mode, pos);
        MoveControl(handControl, pos, rot, true, true);
        return 1;
    }


    private WristArmPose GetWristButtonArmPose(string mode)
    {
        if (mode == "In2")
        {
            // Wrist In2: use the verified Wrist In arm/elbow pose, but bend the hand rotations
            // in the opposite direction from v80 and a little stronger.  This keeps the button/test
            // independent from chest-hold position and avoids fixed world Euler angles.
            WristArmPose inPose = GetWristButtonArmPose("In");
            inPose.RHand.LocalRot = NormalizeQuaternionRaw(inPose.RHand.LocalRot * Quaternion.Euler(0.0f, 0.0f, -45.0f));
            inPose.LHand.LocalRot = NormalizeQuaternionRaw(inPose.LHand.LocalRot * Quaternion.Euler(0.0f, 0.0f, 45.0f));
            return inPose;
        }

        if (mode == "In")
        {
            return new WristArmPose(
                new WristControlPose( 0.096f, 1.219f, 0.295f, -0.230f, -0.718f, -0.603f, 0.261f),
                new WristControlPose(-0.096f, 1.219f, 0.295f, -0.230f,  0.718f,  0.603f, 0.261f),
                new WristControlPose( 0.232f, 1.188f, 0.107f, -0.275f, -0.754f, -0.468f, 0.370f),
                new WristControlPose(-0.228f, 1.188f, 0.111f, -0.285f,  0.749f,  0.466f, 0.374f)
            );
        }

        if (mode == "Out")
        {
            return new WristArmPose(
                new WristControlPose( 0.106f, 1.219f, 0.287f, -0.754f, -0.012f,  0.054f, 0.654f),
                new WristControlPose(-0.106f, 1.219f, 0.287f, -0.754f,  0.012f, -0.054f, 0.654f),
                new WristControlPose( 0.166f, 1.148f, 0.075f, -0.454f, -0.660f, -0.399f, 0.445f),
                new WristControlPose(-0.166f, 1.148f, 0.076f, -0.455f,  0.660f,  0.399f, 0.445f)
            );
        }

        if (mode == "Up")
        {
            return new WristArmPose(
                new WristControlPose( 0.106f, 1.219f, 0.287f, -0.581f, -0.415f,  0.514f, 0.474f),
                new WristControlPose(-0.106f, 1.219f, 0.287f, -0.581f,  0.415f, -0.514f, 0.474f),
                new WristControlPose( 0.179f, 1.140f, 0.082f, -0.203f, -0.801f,  0.028f, 0.563f),
                new WristControlPose(-0.180f, 1.140f, 0.082f, -0.203f,  0.801f, -0.029f, 0.562f)
            );
        }

        if (mode == "Down")
        {
            return new WristArmPose(
                new WristControlPose( 0.106f, 1.219f, 0.287f, -0.601f,  0.392f, -0.425f, 0.551f),
                new WristControlPose(-0.106f, 1.219f, 0.287f, -0.601f, -0.392f,  0.425f, 0.551f),
                new WristControlPose( 0.185f, 1.174f, 0.080f, -0.552f, -0.342f, -0.732f, 0.206f),
                new WristControlPose(-0.186f, 1.173f, 0.080f, -0.552f,  0.344f,  0.731f, 0.205f)
            );
        }

        // Straight is the default.
        return new WristArmPose(
            new WristControlPose( 0.096f, 1.219f, 0.295f, -0.512f, -0.554f, -0.436f, 0.491f),
            new WristControlPose(-0.096f, 1.219f, 0.295f, -0.512f,  0.554f,  0.436f, 0.491f),
            new WristControlPose( 0.185f, 1.167f, 0.089f, -0.384f, -0.691f, -0.450f, 0.415f),
            new WristControlPose(-0.185f, 1.167f, 0.089f, -0.385f,  0.692f,  0.449f, 0.415f)
        );
    }

    private Transform GetSelectedPersonPoseRootTransform()
    {
        if (selectedPerson == null)
            return null;

        if (selectedPerson.mainController != null && selectedPerson.mainController.transform != null)
            return selectedPerson.mainController.transform;

        return selectedPerson.transform;
    }

    private int ApplyWristButtonElbowPose(FreeControllerV3 elbowControl, bool actualRightHand, string mode, WristControlPose pose)
    {
        if (elbowControl == null)
        {
            DebugLog("[WRIST ELBOW MISS] hand=" + (actualRightHand ? "R" : "L") + " mode=" + mode);
            return 0;
        }

        Transform rootT = GetSelectedPersonPoseRootTransform();
        if (rootT == null)
            return 0;

        Vector3 beforeWorldPos = elbowControl.control != null ? elbowControl.control.position : elbowControl.transform.position;
        Vector3 beforeLocalPos = rootT.InverseTransformPoint(beforeWorldPos);
        Quaternion beforeLocalRot = Quaternion.Inverse(rootT.rotation) * (elbowControl.control != null ? elbowControl.control.rotation : elbowControl.transform.rotation);

        Vector3 worldPos = rootT.TransformPoint(pose.LocalPos);
        Quaternion worldRot = rootT.rotation * pose.LocalRot;

        EnsurePositionStateOn(elbowControl);
        EnsureRotationStateOn(elbowControl);

        elbowControl.transform.position = worldPos;
        elbowControl.transform.rotation = worldRot;
        if (elbowControl.control != null)
        {
            elbowControl.control.position = worldPos;
            elbowControl.control.rotation = worldRot;
        }

        if (IsDebugEnabled())
        {
            Vector3 afterLocalPos = rootT.InverseTransformPoint(elbowControl.control != null ? elbowControl.control.position : elbowControl.transform.position);
            Quaternion afterLocalRot = Quaternion.Inverse(rootT.rotation) * (elbowControl.control != null ? elbowControl.control.rotation : elbowControl.transform.rotation);
            DebugLog("[WRIST ELBOW] hand=" + (actualRightHand ? "R" : "L") +
                " mode=" + mode +
                " beforeLocalPos=" + FormatVector3(beforeLocalPos) +
                " targetLocalPos=" + FormatVector3(pose.LocalPos) +
                " afterLocalPos=" + FormatVector3(afterLocalPos) +
                " posDelta=" + Vector3.Distance(beforeWorldPos, elbowControl.control != null ? elbowControl.control.position : elbowControl.transform.position).ToString("F6", CultureInfo.InvariantCulture) +
                " beforeLocalQuat=" + FormatQuaternion(beforeLocalRot) +
                " targetLocalQuat=" + FormatQuaternion(pose.LocalRot) +
                " afterLocalQuat=" + FormatQuaternion(afterLocalRot) +
                " rotErrDeg=" + Quaternion.Angle(pose.LocalRot, afterLocalRot).ToString("F3", CultureInfo.InvariantCulture));
        }

        return 1;
    }

    private int ApplyWristButtonHandLocked(FreeControllerV3 handControl, bool actualRightHand, string mode, Quaternion targetLocalRot)
    {
        if (handControl == null)
            return 0;

        Transform rootT = GetSelectedPersonPoseRootTransform();
        if (rootT == null)
            return 0;

        targetLocalRot = NormalizeQuaternion(targetLocalRot);

        Vector3 lockTransformPos = handControl.transform.position;
        Vector3 lockControlPos = handControl.control != null ? handControl.control.position : handControl.transform.position;
        Vector3 beforeLocalPos = rootT.InverseTransformPoint(handControl.control != null ? handControl.control.position : handControl.transform.position);
        Quaternion beforeLocalRot = Quaternion.Inverse(rootT.rotation) * (handControl.control != null ? handControl.control.rotation : handControl.transform.rotation);
        Quaternion worldRot = rootT.rotation * targetLocalRot;

        EnsurePositionStateOn(handControl);
        EnsureRotationStateOn(handControl);

        handControl.transform.rotation = worldRot;
        handControl.transform.position = lockTransformPos;
        if (handControl.control != null)
        {
            handControl.control.rotation = worldRot;
            handControl.control.position = lockControlPos;
        }

        PendingWristHandLock pending = new PendingWristHandLock();
        pending.Mode = mode;
        pending.Label = actualRightHand ? "RHand" : "LHand";
        pending.Control = handControl;
        pending.LockTransformWorldPos = lockTransformPos;
        pending.LockControlWorldPos = lockControlPos;
        pending.TargetLocalRot = targetLocalRot;
        pending.HasControlTransform = handControl.control != null;
        pending.FramesLeft = WRIST_HAND_LOCK_FRAMES;
        pendingWristHandLocks[handControl] = pending;

        if (IsDebugEnabled())
        {
            Vector3 afterLocalPos = rootT.InverseTransformPoint(handControl.control != null ? handControl.control.position : handControl.transform.position);
            Quaternion afterLocalRot = Quaternion.Inverse(rootT.rotation) * (handControl.control != null ? handControl.control.rotation : handControl.transform.rotation);
            DebugLog("[WRIST HAND BEGIN] hand=" + (actualRightHand ? "R" : "L") +
                " mode=" + mode +
                " beforeLocalPos=" + FormatVector3(beforeLocalPos) +
                " afterLocalPos=" + FormatVector3(afterLocalPos) +
                " posDeltaNow=" + Vector3.Distance(lockControlPos, handControl.control != null ? handControl.control.position : handControl.transform.position).ToString("F6", CultureInfo.InvariantCulture) +
                " beforeLocalQuat=" + FormatQuaternion(beforeLocalRot) +
                " targetLocalQuat=" + FormatQuaternion(targetLocalRot) +
                " afterLocalQuat=" + FormatQuaternion(afterLocalRot) +
                " rotErrDegNow=" + Quaternion.Angle(targetLocalRot, afterLocalRot).ToString("F3", CultureInfo.InvariantCulture));
        }

        return 1;
    }

    private void UpdatePendingWristHandLocks()
    {
        if (pendingWristHandLocks.Count == 0)
            return;

        Transform rootT = GetSelectedPersonPoseRootTransform();
        if (rootT == null)
        {
            ClearPendingWristHandLocks();
            return;
        }

        completedWristHandLocks.Clear();

        foreach (KeyValuePair<FreeControllerV3, PendingWristHandLock> item in pendingWristHandLocks)
        {
            PendingWristHandLock pending = item.Value;
            if (pending == null || pending.Control == null)
            {
                completedWristHandLocks.Add(item.Key);
                continue;
            }

            Quaternion worldRot = rootT.rotation * pending.TargetLocalRot;

            EnsurePositionStateOn(pending.Control);
            EnsureRotationStateOn(pending.Control);

            pending.Control.transform.rotation = worldRot;
            pending.Control.transform.position = pending.LockTransformWorldPos;
            if (pending.Control.control != null)
            {
                pending.Control.control.rotation = worldRot;
                pending.Control.control.position = pending.LockControlWorldPos;
            }

            pending.FramesLeft--;
            if (pending.FramesLeft <= 0)
                completedWristHandLocks.Add(item.Key);
        }

        for (int i = 0; i < completedWristHandLocks.Count; i++)
        {
            FreeControllerV3 key = completedWristHandLocks[i];
            PendingWristHandLock pending;
            if (pendingWristHandLocks.TryGetValue(key, out pending) && pending != null)
                LogWristHandLockDone(rootT, pending);

            pendingWristHandLocks.Remove(key);
        }
    }

    private void LogWristHandLockDone(Transform rootT, PendingWristHandLock pending)
    {
        if (!IsDebugEnabled() || pending == null || pending.Control == null)
            return;

        Quaternion afterTransformLocalRot = Quaternion.Inverse(rootT.rotation) * pending.Control.transform.rotation;
        Quaternion afterControlLocalRot = pending.Control.control != null
            ? Quaternion.Inverse(rootT.rotation) * pending.Control.control.rotation
            : Quaternion.identity;
        Vector3 afterTransformLocalPos = rootT.InverseTransformPoint(pending.Control.transform.position);
        Vector3 afterControlLocalPos = pending.Control.control != null
            ? rootT.InverseTransformPoint(pending.Control.control.position)
            : Vector3.zero;

        float transformPosDelta = Vector3.Distance(pending.LockTransformWorldPos, pending.Control.transform.position);
        float controlPosDelta = pending.Control.control != null
            ? Vector3.Distance(pending.LockControlWorldPos, pending.Control.control.position)
            : 0.0f;

        float transformAngleError = Quaternion.Angle(pending.TargetLocalRot, afterTransformLocalRot);
        float controlAngleError = pending.Control.control != null
            ? Quaternion.Angle(pending.TargetLocalRot, afterControlLocalRot)
            : -1.0f;

        DebugLog("[WRIST HAND DONE] label=" + pending.Label +
            " mode=" + pending.Mode +
            " targetLocalQuat=" + FormatQuaternion(pending.TargetLocalRot) +
            " afterTransformLocalQuat=" + FormatQuaternion(afterTransformLocalRot) +
            " afterControlLocalQuat=" + (pending.Control.control != null ? FormatQuaternion(afterControlLocalRot) : "(null)") +
            " transformErrDeg=" + transformAngleError.ToString("F3", CultureInfo.InvariantCulture) +
            " controlErrDeg=" + controlAngleError.ToString("F3", CultureInfo.InvariantCulture) +
            " afterTransformLocalPos=" + FormatVector3(afterTransformLocalPos) +
            " afterControlLocalPos=" + (pending.Control.control != null ? FormatVector3(afterControlLocalPos) : "(null)") +
            " transformPosDelta=" + transformPosDelta.ToString("F6", CultureInfo.InvariantCulture) +
            " controlPosDelta=" + controlPosDelta.ToString("F6", CultureInfo.InvariantCulture));
    }

    private Quaternion GetHandWristButtonRotation(FreeControllerV3 handControl, bool actualRightHand, string mode, Vector3 controlPosition)
    {
        // v4.0bp:
        // Wrist test buttons are a pure preset test. Do not mix in:
        // - Hand Palm Add Rot X/Y/Z
        // - Hug Body layout/pathRight/frontSide
        // - target center
        // - current hand rotation
        // Straight must be exactly the supplied preset.
        Quaternion baseRotation = GetHandWristButtonSymmetricBaseRotation(actualRightHand);
        Quaternion finalRotation = ApplyHandWristMode(baseRotation, actualRightHand, mode);

        if (IsDebugEnabled())
        {
            Vector3 baseEuler = baseRotation.eulerAngles;
            Vector3 modeEuler = GetHandWristModeEuler(actualRightHand, mode);
            Vector3 finalEuler = finalRotation.eulerAngles;
            DebugLog("[WRIST TEST PRESET] hand=" + (actualRightHand ? "R" : "L") +
                " mode=" + mode +
                " basis=preset-only" +
                " pos=" + FormatVector3(controlPosition) +
                " baseEuler=(" + baseEuler.x.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    baseEuler.y.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    baseEuler.z.ToString("F1", CultureInfo.InvariantCulture) + ")" +
                " modeEuler=(" + modeEuler.x.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    modeEuler.y.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    modeEuler.z.ToString("F1", CultureInfo.InvariantCulture) + ")" +
                " finalEuler=(" + finalEuler.x.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    finalEuler.y.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    finalEuler.z.ToString("F1", CultureInfo.InvariantCulture) + ")");
        }

        return finalRotation;
    }

    private Quaternion GetHandWristButtonBaseRotation(
        FreeControllerV3 handControl,
        bool actualRightHand,
        Vector3 controlPosition,
        out bool pathRightSide,
        out bool frontSide,
        out Vector3 center,
        out string basis)
    {
        pathRightSide = actualRightHand;
        frontSide = false;
        center = Vector3.zero;
        basis = "visual-symmetric";

        // v4.0bn:
        // Wrist test buttons are used to verify the actual bend definitions.
        // Do not use Hug Body pathRight/layout as the base here, because that can swap
        // the left/right starting rotation before Straight/In/Out/Up/Down is tested.
        // Keep pathRight/front only as debug context.
        if (selectedPerson != null && HasValidTarget())
        {
            center = GetTargetCenter();
            frontSide = IsTargetPersonMode() && selectedTargetPerson != null
                ? IsGrabberInFrontOfTargetPerson(center)
                : false;

            string contextBasis;
            if (TryGetGrabHandPathRightSideForWristButton(actualRightHand, center, out pathRightSide, out contextBasis))
                basis = "visual-symmetric/" + contextBasis;
        }

        return GetHandWristButtonSymmetricBaseRotation(actualRightHand);
    }

    private Quaternion GetHandWristButtonSymmetricBaseRotation(bool actualRightHand)
    {
        // v4.0bp: preset-only. Do not apply Hand Palm Add Rot X/Y/Z to test buttons.
        return Quaternion.Euler(GetHandWristSymmetricBaseEuler(actualRightHand));
    }

    private bool TryGetGrabHandPathRightSideForWristButton(bool actualRightHand, Vector3 center, out bool pathRightSide, out string basis)
    {
        pathRightSide = actualRightHand;
        basis = "none";

        if (selectedPerson == null || !HasValidTarget())
            return false;

        // v22: Chest Hold 専用処理を Hug Body へ漏らさない。
        // IsNipplePairMode() は古い互換で control 名を拾うため、移動分岐には使わない。
        bool nipplePairMode = IsChestHoldTarget();
        bool hipHoldMode = IsHipHoldMode();
        bool targetPairMode = IsTargetPairMode();

        Vector3 handCenter = (nipplePairMode || hipHoldMode || targetPairMode) ? center : GetHugCenter(center);
        Vector3 baseSide = GetTargetSideAxis();
        Vector3 handSide = GetHandSideAxis(baseSide);
        if (IsHugBodyTarget())
            handSide = GetHugBodyApproachSideAxis(center, handSide);

        bool swapSidePaths = ShouldSwapSidePaths(center);
        bool leftPathRightSide = !swapSidePaths;
        bool rightPathRightSide = swapSidePaths;
        basis = "grab-swap";

        if (IsHugBodyTarget())
        {
            float handPathWidth = Mathf.Min(GetGrabWidth(), HUG_BODY_HAND_WIDTH_CAP);
            HugBodyHandLayout layout = ResolveHugBodyHandLayout(center, handCenter, handSide, handPathWidth, false);
            leftPathRightSide = layout.leftPathRightSide;
            rightPathRightSide = layout.rightPathRightSide;
            basis = "hug-body-" + layout.mode;
        }

        pathRightSide = actualRightHand ? rightPathRightSide : leftPathRightSide;
        return true;
    }

    private Quaternion ApplyHandWristMode(Quaternion baseRotation, bool actualRightHand, string mode)
    {
        return baseRotation * Quaternion.Euler(GetHandWristModeEuler(actualRightHand, mode));
    }

    private Quaternion GetHandWristRotation(bool actualRightHand, string mode)
    {
        return ApplyHandWristMode(GetHandWristStraightRotation(actualRightHand), actualRightHand, mode);
    }

    private Quaternion GetHandWristStraightRotation(bool actualRightHand)
    {
        return Quaternion.Euler(GetHandWristSymmetricBaseEuler(actualRightHand));
    }

    private Vector3 GetHandWristSymmetricBaseEuler(bool actualRightHand)
    {
        // v4.0bo:
        // Fixed Straight presets from the verified Grab HAND ROT log:
        //   leftEuler  = (298.8, 308.6, 334.4)
        //   rightEuler = (298.8,  51.4,  25.6)
        // Do not derive these from pathRight/layout here; buttons must be stable for retesting.
        Vector3 leftPreset = new Vector3(298.76f, 308.62f, 334.39f);
        Vector3 rightPreset = new Vector3(298.76f, 51.38f, 25.61f);
        return actualRightHand ? rightPreset : leftPreset;
    }

    private Vector3 GetHandWristModeEuler(bool actualRightHand, string mode)
    {
        const float angle = 90.0f;

        if (mode == "In2")
        {
            // IN2 = stronger inward bend than Wrist In, reversed from v80.
            // Wrist In is 90 degrees; In2 uses 150 degrees in the opposite sign.
            const float in2Angle = 150.0f;
            return new Vector3(0.0f, 0.0f, actualRightHand ? -in2Angle : in2Angle);
        }

        if (mode == "In")
        {
            // IN = both wrists bend toward the actor center side when hands are extended.
            return new Vector3(0.0f, 0.0f, actualRightHand ? angle : -angle);
        }

        if (mode == "Out")
        {
            // OUT = opposite side from IN.
            return new Vector3(0.0f, 0.0f, actualRightHand ? -angle : angle);
        }

        if (mode == "Up")
        {
            return new Vector3(-angle, 0.0f, 0.0f);
        }

        if (mode == "Down")
        {
            return new Vector3(angle, 0.0f, 0.0f);
        }

        return Vector3.zero;
    }

    private Vector3 GetHandWristTestEuler(bool actualRightHand, string mode)
    {
        return GetHandWristModeEuler(actualRightHand, mode);
    }

    private Vector3 GetHandRotationOffset()
    {
        return new Vector3(
            handRotXJSON != null ? handRotXJSON.val : 0.0f,
            handRotYJSON != null ? handRotYJSON.val : 0.0f,
            handRotZJSON != null ? handRotZJSON.val : 0.0f
        );
    }

    private Vector3 GetFootRotationOffset()
    {
        return new Vector3(
            footRotXJSON != null ? footRotXJSON.val : 0.0f,
            footRotYJSON != null ? footRotYJSON.val : 0.0f,
            footRotZJSON != null ? footRotZJSON.val : 0.0f
        );
    }

    private Vector3 GetHandPalmFaceAxis(Quaternion rotation)
    {
        Vector3 axis = rotation * Vector3.up;
        if (IsGenTarget())
            axis = -axis;

        if (axis.sqrMagnitude > 0.0001f)
            return axis.normalized;

        return Vector3.forward;
    }

    private Vector3 GetHandWristBendAxis(Quaternion rotation)
    {
        Vector3 axis = rotation * Vector3.forward;
        if (axis.sqrMagnitude > 0.0001f)
            return axis.normalized;

        return Vector3.forward;
    }

    private Quaternion ApplyAutoPalmFacingWristRotation(Quaternion baseRotation, Vector3 controlPosition, Vector3 startPosition, Vector3 center, bool pathRightSide, bool actualRightHand, bool frontSide)
    {
        if (ShouldUseFinalPointWristRoute())
            return ApplyFinalPointDepthWristRotation(baseRotation, controlPosition, startPosition, center, pathRightSide, actualRightHand, frontSide);

        Vector3 targetDir = center - controlPosition;
        if (targetDir.sqrMagnitude < 0.0001f)
        {
            LogWristAutoSkipDebug("zero-target-dir", controlPosition, center, pathRightSide, actualRightHand, frontSide);
            return baseRotation;
        }

        targetDir.Normalize();

        bool hugBodyFlip = false;

        Vector3 palmAxis = GetHandPalmFaceAxis(baseRotation);
        Vector3 bendAxis = GetHandWristBendAxis(baseRotation);

        Vector3 palmOnPlane = Vector3.ProjectOnPlane(palmAxis, bendAxis);
        Vector3 targetOnPlane = Vector3.ProjectOnPlane(targetDir, bendAxis);

        if (palmOnPlane.sqrMagnitude < 0.0001f || targetOnPlane.sqrMagnitude < 0.0001f)
        {
            LogWristAutoSkipDebug("bad-plane", controlPosition, center, pathRightSide, actualRightHand, frontSide);
            return baseRotation;
        }

        float angle = Mathf.Clamp(SignedAngleAroundAxis(palmOnPlane, targetOnPlane, bendAxis), -90.0f, 90.0f);
        LogWristAutoAngleDebug(controlPosition, center, targetDir, palmAxis, bendAxis, angle, pathRightSide, actualRightHand, frontSide, hugBodyFlip);
        return baseRotation * Quaternion.Euler(0.0f, 0.0f, angle);
    }

    private void StoreHugBodyWristReference(FreeControllerV3 handControl, Vector3 referencePoint, Vector3 center)
    {
        if (handControl == null || !IsHugBodyTarget())
            return;

        Vector3 actorSideAxis = GetHugOriginPosition(center) - center;
        actorSideAxis.y = 0.0f;
        if (actorSideAxis.sqrMagnitude < 0.0001f)
            actorSideAxis = -GetHugForwardAxis(center);
        actorSideAxis.y = 0.0f;

        if (actorSideAxis.sqrMagnitude < 0.0001f)
            return;

        actorSideAxis.Normalize();
        float depth = Vector3.Dot(referencePoint - center, actorSideAxis);

        // Keep the last meaningful near/far reference from the actual reach-limited target.
        // Do not use the raw desired point: when the desired point is unreachable behind the
        // target, the hand may actually stop on the front/near side and should pick Wrist Out.
        if (Mathf.Abs(depth) >= HUG_BODY_WRIST_DEPTH_THRESHOLD)
            hugBodyWristReferencePositions[handControl] = referencePoint;
    }

    private bool TryGetHugBodyWristReference(FreeControllerV3 handControl, Vector3 fallbackPoint, Vector3 center, out Vector3 referencePoint, out float depth, out string source)
    {
        referencePoint = fallbackPoint;
        depth = 0.0f;
        source = "fallback";

        Vector3 actorSideAxis = GetHugOriginPosition(center) - center;
        actorSideAxis.y = 0.0f;
        if (actorSideAxis.sqrMagnitude < 0.0001f)
            actorSideAxis = -GetHugForwardAxis(center);
        actorSideAxis.y = 0.0f;

        if (actorSideAxis.sqrMagnitude < 0.0001f)
        {
            source = "no-axis";
            return false;
        }

        actorSideAxis.Normalize();

        if (handControl != null && hugBodyWristReferencePositions.TryGetValue(handControl, out referencePoint))
            source = "stored";

        depth = Vector3.Dot(referencePoint - center, actorSideAxis);
        return Mathf.Abs(depth) >= HUG_BODY_WRIST_DEPTH_THRESHOLD;
    }

    private Quaternion GetWristButtonHandWorldRotation(bool actualRightHand, string mode, Quaternion fallbackRotation)
    {
        WristArmPose pose = GetWristButtonArmPose(mode);
        Transform rootT = GetSelectedPersonPoseRootTransform();
        if (pose == null || rootT == null)
            return fallbackRotation;

        Quaternion targetLocalRot = actualRightHand ? pose.RHand.LocalRot : pose.LHand.LocalRot;
        targetLocalRot = NormalizeQuaternion(targetLocalRot);
        return rootT.rotation * targetLocalRot;
    }


    private bool ShouldUseFinalPointWristRoute()
    {
        if (!IsTargetPersonMode())
            return false;

        // Keep these specialized intimate/small-target routes as-is.  They have their own palm/rotation rules.
        if (IsPeniMode() || IsGenMode() || IsAnusMode() || IsGroinMode())
            return false;

        return true;
    }

    private Quaternion ApplyFinalPointDepthWristRotation(Quaternion baseRotation, Vector3 handPos, Vector3 startPosition, Vector3 center, bool pathRightSide, bool actualRightHand, bool frontSide)
    {
        // LogHandRotationDebug calls rotation calculation with Vector3.zero.
        // Do not let that debug probe decide the final-point wrist direction.
        if (handPos.sqrMagnitude < 0.0001f)
            return baseRotation;

        Vector3 depthAxis = GetFinalPointDepthAxis(center);
        float depth = Vector3.Dot(handPos - center, depthAxis);

        // depthAxis is actor/self -> target.
        //   depth < -threshold : hand is still before the target from actor view => Wrist Out.
        //   otherwise          : hand reached target center or beyond => Wrist In.
        // V5b:
        //   Hug Body closes around the torso and often ends near the center line.
        //   The generic 0.025m threshold was too eager and made the final wrist pick Out.
        //   For Hug Body, keep Wrist In unless the final hand is very clearly far on the actor/front side.
        bool hugBodyWristBias = IsHugBodyTarget();
        float outThreshold = hugBodyWristBias ? HUG_BODY_FINALPOINT_OUT_THRESHOLD : FINAL_POINT_WRIST_DEPTH_THRESHOLD;
        string mode = "In";
        string reason = hugBodyWristBias ? "hug-body-in-biased" : "center-or-beyond-in";
        if (IsSingleHandFootKneeTargetMode())
        {
            mode = "In";
            reason = "single-limb-hand-foot-knee-fixed-in";
        }
        else if (depth < -outThreshold)
        {
            mode = "Out";
            reason = hugBodyWristBias ? "hug-body-far-front-out" : "front-before-target-out";
        }

        Quaternion fallbackRotation = ApplyHandWristMode(baseRotation, actualRightHand, mode);
        Quaternion finalRotation = GetWristButtonHandWorldRotation(actualRightHand, mode, fallbackRotation);

        if (IsDebugEnabled())
        {
            Vector3 finalEuler = finalRotation.eulerAngles;
            DebugLog("[WRIST FINALPOINT] hand=" + (actualRightHand ? "R" : "L") +
                " mode=" + mode +
                " apply=button-preset" +
                " reason=" + reason +
                " depth=" + depth.ToString("F3", CultureInfo.InvariantCulture) +
                " threshold=" + outThreshold.ToString("F3", CultureInfo.InvariantCulture) +
                " hugBias=" + Bool01(hugBodyWristBias) +
                " pathRight=" + Bool01(pathRightSide) +
                " front=" + Bool01(frontSide) +
                " finalEuler=(" + finalEuler.x.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    finalEuler.y.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    finalEuler.z.ToString("F1", CultureInfo.InvariantCulture) + ")" +
                " start=" + FormatVector3(startPosition) +
                " handPos=" + FormatVector3(handPos) +
                " center=" + FormatVector3(center) +
                " depthAxis=" + FormatVector3(depthAxis));
        }

        return finalRotation;
    }

    private Quaternion ApplyHugBodyDirectionalWristRotation(Quaternion baseRotation, Vector3 handPos, Vector3 startPosition, Vector3 center, bool pathRightSide, bool actualRightHand, bool frontSide)
    {
        // LogHandRotationDebug calls rotation calculation with Vector3.zero.
        // Do not let that debug probe decide the Hug Body wrist direction.
        if (handPos.sqrMagnitude < 0.0001f)
            return baseRotation;

        bool crossedPath = pathRightSide != actualRightHand;

        // Important:
        // Decide from the actual final hand position only.
        // Older versions cached an earlier reach-limited target. During Hug Body the hand opens wide,
        // then closes back toward center, so that cached early point could say "front => Out" even
        // after the hand finally returned near center. That was the wrong reference.
        Vector3 actorSideAxis = GetHugOriginPosition(center) - center;
        actorSideAxis.y = 0.0f;
        if (actorSideAxis.sqrMagnitude < 0.0001f)
            actorSideAxis = -GetHugForwardAxis(center);
        actorSideAxis.y = 0.0f;

        bool hasAxis = actorSideAxis.sqrMagnitude >= 0.0001f;
        if (hasAxis)
            actorSideAxis.Normalize();

        Vector3 centerToHand = handPos - center;
        Vector3 centerToHandFlat = centerToHand;
        centerToHandFlat.y = 0.0f;
        float flatDistance = centerToHandFlat.magnitude;
        float depth = hasAxis ? Vector3.Dot(centerToHand, actorSideAxis) : 0.0f;

        // depth is measured along target-center -> actor/self side.
        //   actual final depth > threshold : final hand is still on near/front/actor side => Wrist Out.
        //   actual final depth <= threshold: center/back/unknown => Wrist In.
        // This deliberately defaults to In for near-center, because Hug Body closes to Final Grab Width
        // and the final hand position is often only 1 cm from center.
        string mode = "In";
        string reason = "default-in";
        if (!hasAxis)
        {
            reason = "no-axis-in";
        }
        else if (flatDistance < HUG_BODY_WRIST_NEAR_CENTER_DISTANCE)
        {
            reason = "near-center-in";
        }
        else if (depth > HUG_BODY_WRIST_DEPTH_THRESHOLD)
        {
            mode = "Out";
            reason = "final-front-out";
        }
        else if (depth < -HUG_BODY_WRIST_DEPTH_THRESHOLD)
        {
            reason = "final-back-in";
        }
        else
        {
            reason = "depth-small-in";
        }

        Quaternion fallbackRotation = ApplyHandWristMode(baseRotation, actualRightHand, mode);
        Quaternion finalRotation = GetWristButtonHandWorldRotation(actualRightHand, mode, fallbackRotation);

        if (IsDebugEnabled())
        {
            Vector3 finalEuler = finalRotation.eulerAngles;
            DebugLog("[WRIST HUG FINALPOS] hand=" + (actualRightHand ? "R" : "L") +
                " mode=" + mode +
                " apply=button-preset" +
                " reason=" + reason +
                " cross=" + Bool01(crossedPath) +
                " pathRight=" + Bool01(pathRightSide) +
                " front=" + Bool01(frontSide) +
                " depth=" + depth.ToString("F3", CultureInfo.InvariantCulture) +
                " flatDist=" + flatDistance.ToString("F3", CultureInfo.InvariantCulture) +
                " finalEuler=(" + finalEuler.x.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    finalEuler.y.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    finalEuler.z.ToString("F1", CultureInfo.InvariantCulture) + ")" +
                " start=" + FormatVector3(startPosition) +
                " handPos=" + FormatVector3(handPos) +
                " center=" + FormatVector3(center));
        }

        return finalRotation;
    }

    private void LogWristAutoAngleDebug(Vector3 handPos, Vector3 center, Vector3 targetDir, Vector3 palmAxis, Vector3 bendAxis, float angle, bool pathRightSide, bool actualRightHand, bool frontSide, bool hugBodyFlip)
    {
        if (!IsDebugEnabled())
            return;

        Vector3 targetForward = GetTargetPersonForwardAxis();
        float handForwardDot = targetForward.sqrMagnitude > 0.0001f
            ? Vector3.Dot((handPos - center).normalized, targetForward.normalized)
            : 0.0f;
        float targetDirForwardDot = targetForward.sqrMagnitude > 0.0001f
            ? Vector3.Dot(targetDir.normalized, targetForward.normalized)
            : 0.0f;
        float palmScore = Vector3.Dot(palmAxis.normalized, targetDir.normalized);

        DebugLog("[WRIST AUTO] hand=" + (actualRightHand ? "R" : "L") +
            " pathRight=" + Bool01(pathRightSide) +
            " front=" + Bool01(frontSide) +
            " hugBody=" + Bool01(IsHugBodyTarget()) +
            " hugFlip=" + Bool01(hugBodyFlip) +
            " gen=" + Bool01(IsGenTarget()) +
            " peni=" + Bool01(IsPeniMode()) +
            " angle=" + angle.ToString("F1", CultureInfo.InvariantCulture) +
            " handPos=" + FormatVector3(handPos) +
            " center=" + FormatVector3(center) +
            " targetDir=" + FormatVector3(targetDir) +
            " palm=" + FormatVector3(palmAxis) +
            " bend=" + FormatVector3(bendAxis) +
            " handFwdDot=" + handForwardDot.ToString("F3", CultureInfo.InvariantCulture) +
            " dirFwdDot=" + targetDirForwardDot.ToString("F3", CultureInfo.InvariantCulture) +
            " palmScore=" + palmScore.ToString("F3", CultureInfo.InvariantCulture));
    }

    private void LogWristAutoSkipDebug(string reason, Vector3 handPos, Vector3 center, bool pathRightSide, bool actualRightHand, bool frontSide)
    {
        if (!IsDebugEnabled())
            return;

        DebugLog("[WRIST AUTO SKIP] reason=" + reason +
            " hand=" + (actualRightHand ? "R" : "L") +
            " pathRight=" + Bool01(pathRightSide) +
            " front=" + Bool01(frontSide) +
            " handPos=" + FormatVector3(handPos) +
            " center=" + FormatVector3(center));
    }

    private Quaternion GetFixedHandBaseRotation(Vector3 eulerOffset, bool pathRightSide, bool actualRightHand, bool frontSide)
    {
        Vector3 baseEuler = GetFixedHandBaseEuler(pathRightSide, actualRightHand, frontSide);
        return Quaternion.Euler(
            baseEuler.x + eulerOffset.x,
            baseEuler.y + eulerOffset.y,
            baseEuler.z + eulerOffset.z
        );
    }

    private Vector3 GetFixedHandBaseEuler(bool pathRightSide, bool actualRightHand, bool frontSide)
    {
        Vector3 leftPreset = new Vector3(298.76f, 51.38f, 25.61f);
        Vector3 rightPreset = new Vector3(298.76f, 308.62f, 334.39f);

        // v3.0ag:
        // v3.0afで正面右手は復旧済み。
        // ただしHug中はhandCenterが奥へ動くため、handCenter基準でfrontSideを判定すると
        // 正面右手まで背面扱いになって壊れることがある。
        // 呼び出し側で元のTarget centerを渡すようにし、ここではTarget Person右手だけ
        // 正面=leftPreset、背面=rightPresetへ明示分岐する。
        // 左手とAtom modeは従来式のまま。
        if (IsTargetPersonMode() && actualRightHand)
            return frontSide ? leftPreset : rightPreset;

        return pathRightSide == actualRightHand ? rightPreset : leftPreset;
    }

    private Quaternion GetFixedHandRotation(Vector3 controlPosition, Vector3 startPosition, Vector3 center, Vector3 eulerOffset, bool pathRightSide, bool actualRightHand, bool frontSide)
    {
        Quaternion baseRotation = GetFixedHandBaseRotation(eulerOffset, pathRightSide, actualRightHand, frontSide);

        if (IsGenTarget())
        {
            if (IsDebugEnabled())
                DebugLog("[WRIST GEN] straight=1 hand=" + (actualRightHand ? "R" : "L") +
                    " pathRight=" + Bool01(pathRightSide) +
                    " front=" + Bool01(frontSide) +
                    " pos=" + FormatVector3(controlPosition) +
                    " center=" + FormatVector3(center));
            return baseRotation;
        }

        if (ShouldUseHandWristAngle())
            return ApplyAutoPalmFacingWristRotation(baseRotation, controlPosition, startPosition, center, pathRightSide, actualRightHand, frontSide);

        LogWristAutoSkipDebug("toggle-off", controlPosition, center, pathRightSide, actualRightHand, frontSide);
        return baseRotation;
    }

    private bool IsChestHoldFrontSideByTargetVisualSide(Vector3 targetPoint, string mode)
    {
        // v15: Chest Hold専用の正面/背面判定。
        // 目的: 「targetの正面側に自分がいるか」だけで決める。
        // v16: Chest Holdの正面/背面は、target forward と targetPoint->self の向きで判定する。
        // dot >= 0 = targetがself側を向いている = 正面側: Wrist Up + front palm offset
        // dot <  0 = targetがselfと反対側を向いている = 背面側: Wrist In
        if (selectedPerson == null || selectedTargetPerson == null)
            return IsChestHoldFrontSideFromMode(mode);

        Vector3 grabberPos = selectedPerson.transform != null ? selectedPerson.transform.position : Vector3.zero;
        if (chestControl != null)
            grabberPos = chestControl.control != null ? chestControl.control.position : chestControl.transform.position;

        Vector3 toGrabber = grabberPos - targetPoint;
        toGrabber.y = 0.0f;

        Vector3 targetForward = GetTargetPersonForwardAxis();
        targetForward.y = 0.0f;

        if (toGrabber.sqrMagnitude < 0.0001f || targetForward.sqrMagnitude < 0.0001f)
            return IsChestHoldFrontSideFromMode(mode);

        toGrabber.Normalize();
        targetForward.Normalize();

        float dot = Vector3.Dot(toGrabber, targetForward);
        bool front = dot >= 0.0f;

        if (IsDebugEnabled())
        {
            DebugLog("[CHEST HOLD FRONT] target-dot-positive / mode=" + (mode ?? "") +
                " / front=" + Bool01(front) +
                " / back=" + Bool01(!front) +
                " / dot=" + dot.ToString("F3", CultureInfo.InvariantCulture) +
                " / targetPoint=" + FormatVector3(targetPoint) +
                " / self=" + FormatVector3(grabberPos) +
                " / targetForward=" + FormatVector3(targetForward));
        }

        return front;
    }

    private bool IsChestHoldFrontSideFromMode(string mode)
    {
        // v14: Chest Hold already classifies the pair as "face" or "back" in TryGetAssignedNippleTargets().
        // Use that same classification for Chest Hold wrist/front palm behavior.
        // face = mutual-facing front side => Wrist Up.
        // back = target faces away/back side => Wrist In.
        bool front = string.Equals(mode, "face", StringComparison.OrdinalIgnoreCase);

        if (IsDebugEnabled())
        {
            DebugLog("[CHEST HOLD FRONT] mode-based / mode=" + (mode ?? "") +
                " / front=" + Bool01(front) +
                " / back=" + Bool01(!front));
        }

        return front;
    }

    private bool IsChestHoldFrontSideByMutualFacing(Vector3 targetPoint)
    {
        // v12: Chest Hold専用の正面/背面判定。
        // front = 自分がtargetを向いていて、targetも自分を向いている（向かい合い）。
        // back  = 自分がtargetを向いているが、targetが反対側を向いている（背面側）。
        // 通常Grab/Hug/Foot/Pelvis側の既存front/back判定は触らない。
        if (selectedPerson == null || selectedPerson.transform == null || selectedTargetPerson == null || selectedTargetPerson.transform == null)
            return IsGrabberInFrontOfTargetPerson(targetPoint);

        Vector3 selfPos = selectedPerson.transform.position;
        if (chestControl != null)
            selfPos = chestControl.control != null ? chestControl.control.position : chestControl.transform.position;

        Vector3 toTarget = targetPoint - selfPos;
        Vector3 toSelf = selfPos - targetPoint;
        Vector3 selfForward = GetSelectedPersonForwardAxis();
        Vector3 targetForward = GetTargetPersonForwardAxis();

        toTarget.y = 0.0f;
        toSelf.y = 0.0f;
        selfForward.y = 0.0f;
        targetForward.y = 0.0f;

        if (toTarget.sqrMagnitude < 0.0001f || toSelf.sqrMagnitude < 0.0001f || selfForward.sqrMagnitude < 0.0001f || targetForward.sqrMagnitude < 0.0001f)
            return IsGrabberInFrontOfTargetPerson(targetPoint);

        toTarget.Normalize();
        toSelf.Normalize();
        selfForward.Normalize();
        targetForward.Normalize();

        float selfLooksTarget = Vector3.Dot(selfForward, toTarget);
        float targetLooksSelf = Vector3.Dot(targetForward, toSelf);
        bool front = selfLooksTarget >= 0.0f && targetLooksSelf >= 0.0f;

        if (IsDebugEnabled())
        {
            DebugLog("[CHEST HOLD FRONT] mutual-facing / front=" + Bool01(front) +
                " back=" + Bool01(!front) +
                " selfLooksTarget=" + selfLooksTarget.ToString("F3", CultureInfo.InvariantCulture) +
                " targetLooksSelf=" + targetLooksSelf.ToString("F3", CultureInfo.InvariantCulture));
        }

        return front;
    }

    private bool IsGrabberInFrontOfTargetPerson(Vector3 targetPoint)
    {
        if (selectedPerson == null || selectedTargetPerson == null)
            return true;

        Vector3 grabberPos = selectedPerson.transform.position;
        if (chestControl != null)
            grabberPos = chestControl.control != null ? chestControl.control.position : chestControl.transform.position;

        Vector3 toGrabber = grabberPos - targetPoint;
        if (toGrabber.sqrMagnitude < 0.0001f)
            return true;

        Vector3 targetForward = GetTargetPersonForwardAxis();
        if (targetForward.sqrMagnitude < 0.0001f)
            return true;

        return Vector3.Dot(toGrabber.normalized, targetForward.normalized) >= 0.0f;
    }

    private Vector3 GetNipplePairHandTarget(Vector3 root, Vector3 nippleTarget, FreeControllerV3 fc, bool rightSide)
    {
        // Hold系は指定した左右ターゲットを最終地点にする。
        // reach clamp を掛けると、人物間距離が Max Hand Reach を超えた時に胸/腰まで届かず手前で止まる。
        return nippleTarget;
    }

    private void MoveChestHoldHandControl(FreeControllerV3 handControl, Vector3 finalTarget, Vector3 palmTarget, bool actualRightHand, bool immediate, bool frontSide, Vector3 pairCenter, Vector3 sideAxis, bool targetRightSide, Vector3 nippleTarget)
    {
        if (handControl == null)
            return;

        Vector3 start = GetChestHoldMoveStartPosition(handControl);
        Vector3 mid = (start + finalTarget) * 0.5f;
        string route = frontSide || immediate ? "front-linear" : "back-symmetric-mid";

        // v18/v31: 正面側は従来どおり直線。
        // 背面側だけ、左右それぞれのpalmTarget基準ではなく、target nippleペア中心基準で対称の中間点を作る。
        if (frontSide || immediate)
        {
            LogChestHoldMovePoints(handControl, actualRightHand, route, start, mid, finalTarget, palmTarget, nippleTarget, pairCenter, sideAxis, sideAxis, targetRightSide, frontSide, immediate);
            MoveControl(handControl, finalTarget, Quaternion.identity, false, immediate);
            return;
        }

        Vector3 openAxis = GetChestHoldBackOpenAxis(sideAxis);
        Vector3 via = GetChestHoldBackViaTarget(finalTarget, palmTarget, pairCenter, openAxis, targetRightSide);
        LogChestHoldMovePoints(handControl, actualRightHand, route, start, via, finalTarget, palmTarget, nippleTarget, pairCenter, sideAxis, openAxis, targetRightSide, frontSide, immediate);
        MoveControlTwoStage(handControl, via, finalTarget, CHEST_HOLD_BACK_VIA_SWITCH_T, false);
    }

    private float GetChestHoldBackReachFrontAdjust()
    {
        return chestHoldBackReachFrontAdjustJSON != null
            ? chestHoldBackReachFrontAdjustJSON.val
            : CHEST_HOLD_BACK_REACH_FRONT_ADJUST_DEFAULT;
    }

    private Vector3 GetChestHoldBackPassOffsetTarget(FreeControllerV3 handControl, Vector3 nippleTarget, Vector3 pairCenter, Vector3 sideAxis, bool rightHand)
    {
        return GetChestHoldBackPassOffsetTargetWithOffset(handControl, nippleTarget, pairCenter, sideAxis, rightHand, GetChestHoldBackReachFrontAdjust());
    }

    private Vector3 GetChestHoldBackPassOffsetTargetWithOffset(FreeControllerV3 handControl, Vector3 nippleTarget, Vector3 pairCenter, Vector3 sideAxis, bool rightHand, float nippleOffset)
    {
        Vector3 targetForward = GetTargetPersonForwardAxis();
        targetForward.y = 0.0f;
        if (targetForward.sqrMagnitude < 0.0001f)
            targetForward = Vector3.forward;
        targetForward.Normalize();

        Vector3 targetBackAxis = -targetForward;

        Vector3 measuredSideAxis = sideAxis;
        measuredSideAxis.y = 0.0f;
        if (measuredSideAxis.sqrMagnitude < 0.0001f)
        {
            measuredSideAxis = GetTargetPersonRightAxis();
            measuredSideAxis.y = 0.0f;
        }
        if (measuredSideAxis.sqrMagnitude < 0.0001f)
            measuredSideAxis = Vector3.right;
        measuredSideAxis.Normalize();

        Vector3 depthAxis = Vector3.Cross(Vector3.up, measuredSideAxis);
        depthAxis.y = 0.0f;
        if (depthAxis.sqrMagnitude < 0.0001f)
            depthAxis = targetBackAxis;
        if (Vector3.Dot(depthAxis, targetBackAxis) < 0.0f)
            depthAxis = -depthAxis;
        if (depthAxis.sqrMagnitude < 0.0001f)
            depthAxis = targetBackAxis;
        depthAxis.Normalize();

        Vector3 targetSideAxis = rightHand ? measuredSideAxis : -measuredSideAxis;

        // v73:
        // step2 uses nippleOffset 0.000.
        // step3 uses the Chest Hold Back Nipple Offset slider, default -0.030.
        // The side promise remains: R hand goes right of R nipple, L hand goes left of L nipple.
        return nippleTarget
            + targetSideAxis * CHEST_HOLD_BACK_PASS_SIDE_OFFSET
            + depthAxis * nippleOffset;
    }

    private void MoveChestHoldBackStep2Step3Control(FreeControllerV3 fc, Vector3 step2Target, Vector3 step3Target, bool immediate)
    {
        if (fc == null)
            return;

        EnsurePositionStateOn(fc);

        float t = immediate ? 1.0f : GetMoveTLinear();

        Vector3 start;
        if (!grabStartPositions.TryGetValue(fc, out start))
            start = fc.control != null ? fc.control.position : fc.transform.position;

        Vector3 next;
        if (t <= CHEST_HOLD_FRONT_STEP2_SWITCH_T)
        {
            float a = Mathf.Clamp01(t / CHEST_HOLD_FRONT_STEP2_SWITCH_T);
            next = Vector3.Lerp(start, step2Target, a);
        }
        else
        {
            float b = Mathf.Clamp01((t - CHEST_HOLD_FRONT_STEP2_SWITCH_T) / Mathf.Max(0.001f, 1.0f - CHEST_HOLD_FRONT_STEP2_SWITCH_T));
            next = Vector3.Lerp(step2Target, step3Target, b);
        }

        fc.transform.position = next;
        if (fc.control != null)
            fc.control.position = next;
    }

    private void ApplyChestHoldBackWristInAtStep4(FreeControllerV3 fc, Vector3 nippleTarget, bool actualRightHand, bool immediate)
    {
        if (fc == null)
            return;

        float t = immediate ? 1.0f : GetMoveTLinear();
        if (t < 0.999f)
            return;

        // step4: existing Wrist In.
        ApplyChestHoldFrontUpBackInWrist(fc, nippleTarget, actualRightHand, true, false);

        // step5: separate Wrist In2 feature.  Do not force world Euler angles here;
        // use the wrist preset so self/target placement does not directly change a fixed RotY.
        ApplyChestHoldBackWristIn2AtStep5(fc, actualRightHand);
    }

    private void ApplyChestHoldBackWristIn2AtStep5(FreeControllerV3 fc, bool actualRightHand)
    {
        if (fc == null || !ShouldAlignHandPalm())
            return;

        Vector3 movedPosition = GetControlPosition(fc);
        Quaternion baseRotation = GetFixedHandBaseRotation(GetHandRotationOffset(), actualRightHand, actualRightHand, false);
        Quaternion fallbackRotation = ApplyHandWristMode(baseRotation, actualRightHand, "In2");
        Quaternion finalRotation = GetWristButtonHandWorldRotation(actualRightHand, "In2", fallbackRotation);
        MoveControl(fc, movedPosition, finalRotation, true, true);

        if (IsDebugEnabled())
        {
            Vector3 finalEuler = finalRotation.eulerAngles;
            DebugLog("[CHEST HOLD STEP5 WRIST IN2] hand=" + (actualRightHand ? "R" : "L") +
                " source=wrist-in2-preset" +
                " pos=" + FormatVector3(movedPosition) +
                " finalEuler=(" + finalEuler.x.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    finalEuler.y.ToString("F1", CultureInfo.InvariantCulture) + "," +
                    finalEuler.z.ToString("F1", CultureInfo.InvariantCulture) + ")");
        }
    }

    private Vector3 GetChestHoldBackPassSelfReference()
    {
        Vector3 selfRef = Vector3.zero;
        bool hasSelfRef = false;

        if (selectedPerson != null && selectedPerson.transform != null)
        {
            selfRef = selectedPerson.transform.position;
            hasSelfRef = true;
        }

        if (chestControl != null)
        {
            if (chestControl.control != null)
            {
                selfRef = chestControl.control.position;
                hasSelfRef = true;
            }
            else if (chestControl.transform != null)
            {
                selfRef = chestControl.transform.position;
                hasSelfRef = true;
            }
        }

        return hasSelfRef ? selfRef : Vector3.zero;
    }

    private Vector3 GetChestHoldBackCommonPassDirection(Vector3 pairCenter)
    {
        Vector3 selfRef = GetChestHoldBackPassSelfReference();
        Vector3 passDir = pairCenter - selfRef;
        passDir.y = 0.0f;

        if (passDir.sqrMagnitude < 0.0001f)
        {
            passDir = GetTargetPersonForwardAxis();
            passDir.y = 0.0f;
        }

        if (passDir.sqrMagnitude < 0.0001f)
            passDir = Vector3.forward;

        passDir.Normalize();
        return passDir;
    }


    private Vector3 GetChestHoldSelfRootLogPosition()
    {
        if (hipControl != null && hipControl.control != null)
            return hipControl.control.position;
        if (chestControl != null && chestControl.control != null)
            return chestControl.control.position;
        if (headControl != null && headControl.control != null)
            return headControl.control.position;
        if (containingAtom != null && containingAtom.mainController != null)
            return containingAtom.mainController.control.position;
        if (selectedPerson != null && selectedPerson.transform != null)
            return selectedPerson.transform.position;
        return Vector3.zero;
    }

    private Vector3 GetChestHoldTargetRootLogPosition()
    {
        FreeControllerV3 hip = GetTargetPersonControlByAliases("hipControl", "hip", "pelvisControl", "pelvis");
        if (hip != null && hip.control != null)
            return hip.control.position;

        FreeControllerV3 chest = GetTargetPersonControlByAliases("chestControl", "chest");
        if (chest != null && chest.control != null)
            return chest.control.position;

        FreeControllerV3 head = GetTargetPersonControlByAliases("headControl", "head");
        if (head != null && head.control != null)
            return head.control.position;

        if (selectedTargetPerson != null && selectedTargetPerson.mainController != null)
            return selectedTargetPerson.mainController.control.position;
        if (selectedTargetPerson != null && selectedTargetPerson.transform != null)
            return selectedTargetPerson.transform.position;
        return Vector3.zero;
    }

    private string FormatDistance(float value)
    {
        return value.ToString("F3", CultureInfo.InvariantCulture);
    }

    private void LogChestHoldEssentialTwoLineAlways(Vector3 reachRightTarget, Vector3 reachLeftTarget, Vector3 finalRightTarget, Vector3 finalLeftTarget)
    {
        if (chestHoldEssentialTwoLineLogged)
            return;
        chestHoldEssentialTwoLineLogged = true;

        Vector3 selfRoot = GetChestHoldSelfRootLogPosition();
        Vector3 targetRoot = GetChestHoldTargetRootLogPosition();
        Vector3 rightHandPos = GetControlPositionSafe(rHandControl);
        Vector3 leftHandPos = GetControlPositionSafe(lHandControl);
        Vector3 pairCenter = (finalRightTarget + finalLeftTarget) * 0.5f;

        Vector3 targetForwardAxisForAdjust = GetTargetPersonForwardAxis();
        targetForwardAxisForAdjust.y = 0.0f;
        if (targetForwardAxisForAdjust.sqrMagnitude < 0.0001f)
            targetForwardAxisForAdjust = Vector3.forward;
        targetForwardAxisForAdjust.Normalize();

        Vector3 targetBackAxisForAdjust = -targetForwardAxisForAdjust;
        Vector3 nippleSideAxisForAdjust = finalRightTarget - finalLeftTarget;
        nippleSideAxisForAdjust.y = 0.0f;
        if (nippleSideAxisForAdjust.sqrMagnitude < 0.0001f)
        {
            nippleSideAxisForAdjust = GetTargetPersonRightAxis();
            nippleSideAxisForAdjust.y = 0.0f;
        }
        if (nippleSideAxisForAdjust.sqrMagnitude < 0.0001f)
            nippleSideAxisForAdjust = Vector3.right;
        nippleSideAxisForAdjust.Normalize();

        Vector3 sliderAxisForAdjust = Vector3.Cross(Vector3.up, nippleSideAxisForAdjust);
        sliderAxisForAdjust.y = 0.0f;
        if (sliderAxisForAdjust.sqrMagnitude < 0.0001f)
            sliderAxisForAdjust = targetBackAxisForAdjust;
        if (Vector3.Dot(sliderAxisForAdjust, targetBackAxisForAdjust) < 0.0f)
            sliderAxisForAdjust = -sliderAxisForAdjust;
        if (sliderAxisForAdjust.sqrMagnitude < 0.0001f)
            sliderAxisForAdjust = targetBackAxisForAdjust;
        sliderAxisForAdjust.Normalize();

        float frontAdjust = GetChestHoldBackReachFrontAdjust();
        // v69: base is each nipple plus its promised side clearance. The slider moves only front/back.
        Vector3 baseReachRightTarget = finalRightTarget + nippleSideAxisForAdjust * CHEST_HOLD_BACK_PASS_SIDE_OFFSET;
        Vector3 baseReachLeftTarget = finalLeftTarget - nippleSideAxisForAdjust * CHEST_HOLD_BACK_PASS_SIDE_OFFSET;
        Vector3 baseReachCenter = (baseReachRightTarget + baseReachLeftTarget) * 0.5f;
        Vector3 adjustedReachCenter = (reachRightTarget + reachLeftTarget) * 0.5f;
        float baseCenterForward = Vector3.Dot(baseReachCenter - pairCenter, targetForwardAxisForAdjust);
        float adjustedCenterForward = Vector3.Dot(adjustedReachCenter - pairCenter, targetForwardAxisForAdjust);
        float visibleFrontDelta = Vector3.Dot(adjustedReachCenter - baseReachCenter, sliderAxisForAdjust);
        float sliderSideDelta = Vector3.Dot(adjustedReachCenter - baseReachCenter, nippleSideAxisForAdjust);
        float nippleToReachR = Vector3.Dot(reachRightTarget - finalRightTarget, sliderAxisForAdjust);
        float nippleToReachL = Vector3.Dot(reachLeftTarget - finalLeftTarget, sliderAxisForAdjust);
        float nippleSideDeltaR = Vector3.Dot(reachRightTarget - finalRightTarget, nippleSideAxisForAdjust);
        float nippleSideDeltaL = Vector3.Dot(reachLeftTarget - finalLeftTarget, nippleSideAxisForAdjust);

        float rightHandToReach = Vector3.Distance(rightHandPos, reachRightTarget);
        float leftHandToReach = Vector3.Distance(leftHandPos, reachLeftTarget);
        float rightReachToFinal = Vector3.Distance(reachRightTarget, finalRightTarget);
        float leftReachToFinal = Vector3.Distance(reachLeftTarget, finalLeftTarget);
        float rightFinalFromPair = Vector3.Distance(finalRightTarget, pairCenter);
        float leftFinalFromPair = Vector3.Distance(finalLeftTarget, pairCenter);
        float rightReachFromPair = Vector3.Distance(reachRightTarget, pairCenter);
        float leftReachFromPair = Vector3.Distance(reachLeftTarget, pairCenter);

        DebugLog("[CHEST HOLD INPUT]" +
            " selfRoot=" + FormatVector3(selfRoot) +
            " / rHand=" + FormatVector3(rightHandPos) +
            " / lHand=" + FormatVector3(leftHandPos) +
            " / targetRoot=" + FormatVector3(targetRoot) +
            " / rNipple=" + FormatVector3(finalRightTarget) +
            " / lNipple=" + FormatVector3(finalLeftTarget) +
            " / pairCenter=" + FormatVector3(pairCenter));

        DebugLog("[CHEST HOLD TARGETS]" +
            " reachBaseR=" + FormatVector3(baseReachRightTarget) +
            " / reachBaseL=" + FormatVector3(baseReachLeftTarget) +
            " / reachAdjR=" + FormatVector3(reachRightTarget) +
            " / reachAdjL=" + FormatVector3(reachLeftTarget) +
            " / finalR=" + FormatVector3(finalRightTarget) +
            " / finalL=" + FormatVector3(finalLeftTarget) +
            " / nippleOffset=" + frontAdjust.ToString("F3", CultureInfo.InvariantCulture) +
            " / sideOffset=" + CHEST_HOLD_BACK_PASS_SIDE_OFFSET.ToString("F3", CultureInfo.InvariantCulture) +
            " / nippleToReachR=" + nippleToReachR.ToString("F3", CultureInfo.InvariantCulture) +
            " / nippleToReachL=" + nippleToReachL.ToString("F3", CultureInfo.InvariantCulture) +
            " / nippleSideDeltaR=" + nippleSideDeltaR.ToString("F3", CultureInfo.InvariantCulture) +
            " / nippleSideDeltaL=" + nippleSideDeltaL.ToString("F3", CultureInfo.InvariantCulture) +
            " / baseCenterForward=" + baseCenterForward.ToString("F3", CultureInfo.InvariantCulture) +
            " / adjustedCenterForward=" + adjustedCenterForward.ToString("F3", CultureInfo.InvariantCulture) +
            " / visibleFrontDelta=" + visibleFrontDelta.ToString("F3", CultureInfo.InvariantCulture) +
            " / sliderAxis=" + FormatVector3(sliderAxisForAdjust) +
            " / sliderSideDelta=" + sliderSideDelta.ToString("F3", CultureInfo.InvariantCulture) +
            " / dHandReachR=" + FormatDistance(rightHandToReach) +
            " / dHandReachL=" + FormatDistance(leftHandToReach) +
            " / dReachFinalR=" + FormatDistance(rightReachToFinal) +
            " / dReachFinalL=" + FormatDistance(leftReachToFinal) +
            " / dReachPairR=" + FormatDistance(rightReachFromPair) +
            " / dReachPairL=" + FormatDistance(leftReachFromPair) +
            " / dFinalPairR=" + FormatDistance(rightFinalFromPair) +
            " / dFinalPairL=" + FormatDistance(leftFinalFromPair));

        Vector3 rightElbowPos = GetControlPositionSafe(rElbowControl);
        Vector3 leftElbowPos = GetControlPositionSafe(lElbowControl);
        Vector3 rightElbowFromHand = rightElbowPos - rightHandPos;
        Vector3 leftElbowFromHand = leftElbowPos - leftHandPos;
        Vector3 rightElbowFromReach = rightElbowPos - reachRightTarget;
        Vector3 leftElbowFromReach = leftElbowPos - reachLeftTarget;
        float rightElbowOutFromHand = Vector3.Dot(rightElbowFromHand, nippleSideAxisForAdjust);
        float leftElbowOutFromHand = Vector3.Dot(leftElbowFromHand, -nippleSideAxisForAdjust);
        float rightElbowOutFromReach = Vector3.Dot(rightElbowFromReach, nippleSideAxisForAdjust);
        float leftElbowOutFromReach = Vector3.Dot(leftElbowFromReach, -nippleSideAxisForAdjust);
        float rightElbowSideFromPair = Vector3.Dot(rightElbowPos - pairCenter, nippleSideAxisForAdjust);
        float leftElbowSideFromPair = Vector3.Dot(leftElbowPos - pairCenter, nippleSideAxisForAdjust);
        float rightHandSideFromPair = Vector3.Dot(rightHandPos - pairCenter, nippleSideAxisForAdjust);
        float leftHandSideFromPair = Vector3.Dot(leftHandPos - pairCenter, nippleSideAxisForAdjust);
        float rightReachSideFromPair = Vector3.Dot(reachRightTarget - pairCenter, nippleSideAxisForAdjust);
        float leftReachSideFromPair = Vector3.Dot(reachLeftTarget - pairCenter, nippleSideAxisForAdjust);

        DebugLog("[CHEST HOLD ELBOW]" +
            " rElbow=" + FormatVector3(rightElbowPos) +
            " / lElbow=" + FormatVector3(leftElbowPos) +
            " / rElbowOutFromHand=" + rightElbowOutFromHand.ToString("F3", CultureInfo.InvariantCulture) +
            " / lElbowOutFromHand=" + leftElbowOutFromHand.ToString("F3", CultureInfo.InvariantCulture) +
            " / rElbowOutFromReach=" + rightElbowOutFromReach.ToString("F3", CultureInfo.InvariantCulture) +
            " / lElbowOutFromReach=" + leftElbowOutFromReach.ToString("F3", CultureInfo.InvariantCulture) +
            " / rElbowSidePair=" + rightElbowSideFromPair.ToString("F3", CultureInfo.InvariantCulture) +
            " / lElbowSidePair=" + leftElbowSideFromPair.ToString("F3", CultureInfo.InvariantCulture) +
            " / rHandSidePair=" + rightHandSideFromPair.ToString("F3", CultureInfo.InvariantCulture) +
            " / lHandSidePair=" + leftHandSideFromPair.ToString("F3", CultureInfo.InvariantCulture) +
            " / rReachSidePair=" + rightReachSideFromPair.ToString("F3", CultureInfo.InvariantCulture) +
            " / lReachSidePair=" + leftReachSideFromPair.ToString("F3", CultureInfo.InvariantCulture));

        DebugLog("[CHEST HOLD FRONT ADJUST]" +
            " targetForward=" + FormatVector3(targetForwardAxisForAdjust) +
            " / sliderAxis=" + FormatVector3(sliderAxisForAdjust) +
            " / nippleSideAxis=" + FormatVector3(nippleSideAxisForAdjust) +
            " / baseCenter=" + FormatVector3(baseReachCenter) +
            " / adjustedCenter=" + FormatVector3(adjustedReachCenter) +
            " / baseR=" + FormatVector3(baseReachRightTarget) +
            " / adjustedR=" + FormatVector3(reachRightTarget) +
            " / baseL=" + FormatVector3(baseReachLeftTarget) +
            " / adjustedL=" + FormatVector3(reachLeftTarget) +
            " / nippleOffset=" + frontAdjust.ToString("F3", CultureInfo.InvariantCulture) +
            " / sideOffset=" + CHEST_HOLD_BACK_PASS_SIDE_OFFSET.ToString("F3", CultureInfo.InvariantCulture) +
            " / nippleToReachR=" + nippleToReachR.ToString("F3", CultureInfo.InvariantCulture) +
            " / nippleToReachL=" + nippleToReachL.ToString("F3", CultureInfo.InvariantCulture) +
            " / nippleSideDeltaR=" + nippleSideDeltaR.ToString("F3", CultureInfo.InvariantCulture) +
            " / nippleSideDeltaL=" + nippleSideDeltaL.ToString("F3", CultureInfo.InvariantCulture) +
            " / sliderDelta=" + visibleFrontDelta.ToString("F3", CultureInfo.InvariantCulture) +
            " / sliderSideDelta=" + sliderSideDelta.ToString("F3", CultureInfo.InvariantCulture));

        Vector3 targetRightAxis = GetTargetPersonRightAxis();
        targetRightAxis.y = 0.0f;
        if (targetRightAxis.sqrMagnitude < 0.0001f)
            targetRightAxis = Vector3.right;
        targetRightAxis.Normalize();

        Vector3 targetForwardAxis = GetTargetPersonForwardAxis();
        targetForwardAxis.y = 0.0f;
        if (targetForwardAxis.sqrMagnitude < 0.0001f)
            targetForwardAxis = Vector3.forward;
        targetForwardAxis.Normalize();

        Vector3 nippleRightAxis = finalRightTarget - finalLeftTarget;
        nippleRightAxis.y = 0.0f;
        float nippleAxisLen = nippleRightAxis.magnitude;
        if (nippleRightAxis.sqrMagnitude < 0.0001f)
            nippleRightAxis = targetRightAxis;
        else
            nippleRightAxis.Normalize();

        Vector3 reachRightAxis = reachRightTarget - reachLeftTarget;
        reachRightAxis.y = 0.0f;
        float reachAxisLen = reachRightAxis.magnitude;
        if (reachRightAxis.sqrMagnitude < 0.0001f)
            reachRightAxis = targetRightAxis;
        else
            reachRightAxis.Normalize();

        float targetVsNippleDot = Vector3.Dot(targetRightAxis, nippleRightAxis);
        float reachVsNippleDot = Vector3.Dot(reachRightAxis, nippleRightAxis);
        float targetVsReachDot = Vector3.Dot(targetRightAxis, reachRightAxis);
        float nippleForwardDepth = Vector3.Dot((finalRightTarget - finalLeftTarget), targetForwardAxis);
        float reachForwardDepth = Vector3.Dot((reachRightTarget - reachLeftTarget), targetForwardAxis);
        float nippleRightDepth = Vector3.Dot((finalRightTarget - finalLeftTarget), targetRightAxis);
        float reachRightDepth = Vector3.Dot((reachRightTarget - reachLeftTarget), targetRightAxis);
        Vector3 pairToReachCenter = ((reachRightTarget + reachLeftTarget) * 0.5f) - pairCenter;
        float reachCenterForward = Vector3.Dot(pairToReachCenter, targetForwardAxis);
        float reachCenterBack = Vector3.Dot(pairToReachCenter, -targetForwardAxis);

        DebugLog("[CHEST HOLD AXIS]" +
            " targetRight=" + FormatVector3(targetRightAxis) +
            " / targetForward=" + FormatVector3(targetForwardAxis) +
            " / nippleRight=" + FormatVector3(nippleRightAxis) +
            " / reachRight=" + FormatVector3(reachRightAxis) +
            " / dotTargetNipple=" + targetVsNippleDot.ToString("F3", CultureInfo.InvariantCulture) +
            " / dotReachNipple=" + reachVsNippleDot.ToString("F3", CultureInfo.InvariantCulture) +
            " / dotTargetReach=" + targetVsReachDot.ToString("F3", CultureInfo.InvariantCulture) +
            " / nippleAxisLen=" + nippleAxisLen.ToString("F3", CultureInfo.InvariantCulture) +
            " / reachAxisLen=" + reachAxisLen.ToString("F3", CultureInfo.InvariantCulture) +
            " / nippleForwardDepth=" + nippleForwardDepth.ToString("F3", CultureInfo.InvariantCulture) +
            " / reachForwardDepth=" + reachForwardDepth.ToString("F3", CultureInfo.InvariantCulture) +
            " / nippleRightDepth=" + nippleRightDepth.ToString("F3", CultureInfo.InvariantCulture) +
            " / reachRightDepth=" + reachRightDepth.ToString("F3", CultureInfo.InvariantCulture) +
            " / reachCenterForward=" + reachCenterForward.ToString("F3", CultureInfo.InvariantCulture) +
            " / reachCenterBack=" + reachCenterBack.ToString("F3", CultureInfo.InvariantCulture));
    }

    private void DebugChestHoldBackPassOffsetTarget(string handLabel, Vector3 nippleTarget, Vector3 finalTarget, Vector3 pairCenter, Vector3 sideAxis, bool immediate)
    {
        if (!IsDebugEnabled())
            return;

        Vector3 targetForward = GetTargetPersonForwardAxis();
        targetForward.y = 0.0f;
        if (targetForward.sqrMagnitude < 0.0001f)
            targetForward = Vector3.forward;
        targetForward.Normalize();
        Vector3 targetBackAxis = -targetForward;

        Vector3 targetRightAxis = GetTargetPersonRightAxis();
        targetRightAxis.y = 0.0f;
        if (targetRightAxis.sqrMagnitude < 0.0001f)
            targetRightAxis = Vector3.right;
        targetRightAxis.Normalize();

        Vector3 targetSideAxis = (handLabel == "R") ? targetRightAxis : -targetRightAxis;
        Vector3 deltaFromNipple = finalTarget - nippleTarget;
        Vector3 deltaFromPairCenter = finalTarget - pairCenter;
        float backDepth = Vector3.Dot(deltaFromPairCenter, targetBackAxis);
        float sideDepth = Vector3.Dot(deltaFromPairCenter, targetSideAxis);
        float forwardDepth = Vector3.Dot(deltaFromPairCenter, targetForward);
        float distanceFromPairCenter = deltaFromPairCenter.magnitude;
        float distanceFromNipple = deltaFromNipple.magnitude;
        float frontAdjust = GetChestHoldBackReachFrontAdjust();

        DebugLog("[CHEST HOLD BACK PASS TEST] hand=" + (handLabel ?? "") +
            " / mode=orthogonal-nipple-axis-front-adjust-reach" +
            " / nipple=" + FormatVector3(nippleTarget) +
            " / final=" + FormatVector3(finalTarget) +
            " / pairCenter=" + FormatVector3(pairCenter) +
            " / targetBackAxis=" + FormatVector3(targetBackAxis) +
            " / targetSideAxis=" + FormatVector3(targetSideAxis) +
            " / targetForward=" + FormatVector3(targetForward) +
            " / reachBack=" + CHEST_HOLD_BACK_PASS_THROUGH.ToString("F2", CultureInfo.InvariantCulture) +
            " / sideClearance=" + CHEST_HOLD_BACK_PASS_SIDE_OFFSET.ToString("F2", CultureInfo.InvariantCulture) +
            " / frontAdjust=" + frontAdjust.ToString("F3", CultureInfo.InvariantCulture) +
            " / backDepthFromPair=" + backDepth.ToString("F3", CultureInfo.InvariantCulture) +
            " / sideDepthFromPair=" + sideDepth.ToString("F3", CultureInfo.InvariantCulture) +
            " / forwardDepthFromPair=" + forwardDepth.ToString("F3", CultureInfo.InvariantCulture) +
            " / distanceFromPair=" + distanceFromPairCenter.ToString("F3", CultureInfo.InvariantCulture) +
            " / distanceFromNipple=" + distanceFromNipple.ToString("F3", CultureInfo.InvariantCulture) +
            " / deltaFromNipple=" + FormatVector3(deltaFromNipple) +
            " / immediate=" + Bool01(immediate));
    }

    private Vector3 GetChestHoldMoveStartPosition(FreeControllerV3 handControl)
    {
        Vector3 start;
        if (handControl != null && grabStartPositions.TryGetValue(handControl, out start))
            return start;
        return GetControlPosition(handControl);
    }

    private Vector3 GetChestHoldBackOpenAxis(Vector3 sideAxis)
    {
        Vector3 openAxis = Vector3.zero;
        if (selectedTargetPerson != null && selectedTargetPerson.transform != null)
            openAxis = selectedTargetPerson.transform.right;

        openAxis.y = 0.0f;
        if (openAxis.sqrMagnitude < 0.0001f)
            openAxis = sideAxis;
        openAxis.y = 0.0f;
        if (openAxis.sqrMagnitude < 0.0001f)
            openAxis = Vector3.right;
        openAxis.Normalize();

        Vector3 signAxis = sideAxis;
        signAxis.y = 0.0f;
        if (signAxis.sqrMagnitude >= 0.0001f)
        {
            signAxis.Normalize();
            if (Vector3.Dot(openAxis, signAxis) < 0.0f)
                openAxis = -openAxis;
        }

        return openAxis;
    }

    private Vector3 GetChestHoldBackViaTarget(Vector3 finalTarget, Vector3 palmTarget, Vector3 pairCenter, Vector3 openAxis, bool targetRightSide)
    {
        Vector3 lateralAxis = openAxis;
        lateralAxis.y = 0.0f;
        if (lateralAxis.sqrMagnitude < 0.0001f)
            lateralAxis = selectedTargetPerson != null && selectedTargetPerson.transform != null ? selectedTargetPerson.transform.right : Vector3.right;
        lateralAxis.y = 0.0f;
        if (lateralAxis.sqrMagnitude < 0.0001f)
            lateralAxis = Vector3.right;
        lateralAxis.Normalize();
        if (!targetRightSide)
            lateralAxis = -lateralAxis;

        Vector3 upAxis = selectedTargetPerson != null && selectedTargetPerson.transform != null
            ? selectedTargetPerson.transform.up
            : Vector3.up;
        if (upAxis.sqrMagnitude < 0.0001f)
            upAxis = Vector3.up;
        upAxis.Normalize();

        // v33: 背面Chest Holdの中間点は、pairCenter + forward ではなく、
        // 各手のfinalTargetから左右へ開く。
        // L/R nippleの奥行き差が大きい時、pairCenter基準 + forward via だと片手だけ大きく動くため。
        Vector3 via = finalTarget
            + lateralAxis * CHEST_HOLD_BACK_VIA_OUTSIDE
            + upAxis * CHEST_HOLD_BACK_VIA_UP;

        // v44: v43 の L hand depth 補正が逆だったため、余計な変更はせず、
        // その補正量だけを反対方向へ 2 倍にする。R hand は v33/v43 のまま。
        if (!targetRightSide)
        {
            Vector3 depthAxis = GetTargetPersonForwardAxis();
            depthAxis.y = 0.0f;
            if (depthAxis.sqrMagnitude >= 0.0001f)
            {
                depthAxis.Normalize();
                float palmDepth = Vector3.Dot(palmTarget - pairCenter, depthAxis);
                if (palmDepth > 0.0001f)
                    via += depthAxis * (2.5f * palmDepth);
            }
        }

        return via;
    }

    private void LogChestHoldMovePoints(FreeControllerV3 handControl, bool actualRightHand, string route, Vector3 start, Vector3 mid, Vector3 finalTarget, Vector3 palmTarget, Vector3 nippleTarget, Vector3 pairCenter, Vector3 sideAxis, Vector3 openAxis, bool targetRightSide, bool frontSide, bool immediate)
    {
        if (!IsDebugEnabled() || handControl == null)
            return;

        if (actualRightHand ? chestHoldMovePointsRightLogged : chestHoldMovePointsLeftLogged)
            return;

        float t = immediate ? 1.0f : GetMoveTLinear();
        if (!immediate && t > 0.075f)
            return;

        DebugLog("[CHEST HOLD POINTS] hand=" + (actualRightHand ? "R" : "L") +
            " route=" + (route ?? "") +
            " front=" + Bool01(frontSide) +
            " targetRightSide=" + Bool01(targetRightSide) +
            " start=" + FormatVector3(start) +
            " mid=" + FormatVector3(mid) +
            " final=" + FormatVector3(finalTarget) +
            " nipple=" + FormatVector3(nippleTarget) +
            " palmTarget=" + FormatVector3(palmTarget) +
            " pairCenter=" + FormatVector3(pairCenter) +
            " side=" + FormatVector3(sideAxis) +
            " openAxis=" + FormatVector3(openAxis) +
            " midDepth=" + GetChestHoldDebugDepth(mid, pairCenter).ToString("F3", CultureInfo.InvariantCulture) +
            " finalDepth=" + GetChestHoldDebugDepth(finalTarget, pairCenter).ToString("F3", CultureInfo.InvariantCulture) +
            " palmDepth=" + GetChestHoldDebugDepth(palmTarget, pairCenter).ToString("F3", CultureInfo.InvariantCulture) +
            " openFromFinal=" + Vector3.Distance(mid, finalTarget).ToString("F3", CultureInfo.InvariantCulture) +
            " startToMid=" + Vector3.Distance(start, mid).ToString("F3", CultureInfo.InvariantCulture) +
            " midToFinal=" + Vector3.Distance(mid, finalTarget).ToString("F3", CultureInfo.InvariantCulture) +
            " t=" + t.ToString("F3", CultureInfo.InvariantCulture));

        if (actualRightHand)
            chestHoldMovePointsRightLogged = true;
        else
            chestHoldMovePointsLeftLogged = true;
    }

    private float GetChestHoldDebugDepth(Vector3 point, Vector3 pairCenter)
    {
        Vector3 depthAxis = GetTargetPersonForwardAxis();
        depthAxis.y = 0.0f;
        if (depthAxis.sqrMagnitude < 0.0001f)
            return 0.0f;
        depthAxis.Normalize();
        return Vector3.Dot(point - pairCenter, depthAxis);
    }

    private void MoveControlTwoStage(FreeControllerV3 fc, Vector3 viaTarget, Vector3 finalTarget, float switchT, bool immediate)
    {
        if (fc == null)
            return;

        EnsurePositionStateOn(fc);

        float t = immediate ? 1.0f : GetMoveTLinear();
        switchT = Mathf.Clamp(switchT, 0.05f, 0.95f);

        Vector3 start;
        if (!grabStartPositions.TryGetValue(fc, out start))
            start = fc.control != null ? fc.control.position : fc.transform.position;

        Vector3 next;
        if (t <= switchT)
        {
            float a = Mathf.Clamp01(t / switchT);
            next = Vector3.Lerp(start, viaTarget, a);
        }
        else
        {
            float b = Mathf.Clamp01((t - switchT) / Mathf.Max(0.001f, 1.0f - switchT));
            next = Vector3.Lerp(viaTarget, finalTarget, b);
        }

        fc.transform.position = next;
        if (fc.control != null)
            fc.control.position = next;

        if (IsDebugEnabled() && t <= 0.05f)
        {
            DebugLog("[CHEST HOLD BACK VIA] hand=" + (fc == rHandControl ? "R" : "L") +
                " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
                " via=" + FormatVector3(viaTarget) +
                " final=" + FormatVector3(finalTarget) +
                " switch=" + switchT.ToString("F2", CultureInfo.InvariantCulture));
        }
    }

    private Vector3 GetChestHoldAdjustedPalmTarget(Vector3 nippleTarget, bool actualRightHand, bool frontSide)
    {
        if (!frontSide)
            return nippleTarget;

        Vector3 downAxis = selectedTargetPerson != null ? -selectedTargetPerson.transform.up : Vector3.down;
        if (downAxis.sqrMagnitude < 0.0001f)
            downAxis = Vector3.down;

        Vector3 inwardAxis = -GetNipplePairOutwardAxis(actualRightHand);
        if (inwardAxis.sqrMagnitude < 0.0001f)
            inwardAxis = actualRightHand ? Vector3.left : Vector3.right;

        return nippleTarget
            + downAxis.normalized * CHEST_HOLD_FRONT_NIPPLE_DOWN
            + inwardAxis.normalized * CHEST_HOLD_FRONT_NIPPLE_INWARD;
    }

    private Vector3 GetChestHoldPalmCenteredWristTarget(FreeControllerV3 elbowControl, FreeControllerV3 handControl, Vector3 palmTarget, bool actualRightHand, bool frontSide)
    {
        if (elbowControl == null || handControl == null)
            return palmTarget;

        Vector3 elbowPos = GetControlPosition(elbowControl);
        Vector3 wristPos = GetControlPosition(handControl);
        Vector3 forearmDir = wristPos - elbowPos;
        if (forearmDir.sqrMagnitude < 0.0001f)
            return palmTarget;

        forearmDir.Normalize();
        float reach = frontSide ? CHEST_HOLD_PALM_CENTER_REACH_FRONT : CHEST_HOLD_PALM_CENTER_REACH_BACK;
        return palmTarget - forearmDir * reach;
    }

    private string GetChestHoldFrontBackWristMode(bool frontSide)
    {
        // v16: frontSide comes from target-dot-positive判定。
        // front side = Wrist Up, back side = Wrist In.
        return frontSide ? "Up" : "In";
    }

    private void ApplyChestHoldFrontUpBackInWrist(FreeControllerV3 fc, Vector3 center, bool actualRightHand, bool immediate, bool frontSide)
    {
        if (fc == null || !ShouldAlignHandPalm())
            return;

        Vector3 movedPosition = GetControlPosition(fc);
        string mode = GetChestHoldFrontBackWristMode(frontSide);

        Quaternion baseRotation = GetFixedHandBaseRotation(GetHandRotationOffset(), actualRightHand, actualRightHand, frontSide);
        Quaternion fallbackRotation = ApplyHandWristMode(baseRotation, actualRightHand, mode);
        Quaternion finalRotation = GetWristButtonHandWorldRotation(actualRightHand, mode, fallbackRotation);
        MoveControl(fc, movedPosition, finalRotation, true, true);
    }

    private void LogChestHoldFinalHandNipplePosition(FreeControllerV3 handControl, Vector3 nippleTarget, Vector3 palmTarget, bool actualRightHand, bool immediate, bool frontSide)
    {
        if (handControl == null)
            return;

        if (actualRightHand ? chestHoldFinalRightLogged : chestHoldFinalLeftLogged)
            return;

        float t = immediate ? 1.0f : GetMoveTLinear();
        if (t < 0.999f)
            return;

        Vector3 handPos = GetControlPosition(handControl);
        DebugLog("[CHEST HOLD FINAL] hand=" + (actualRightHand ? "R" : "L") +
            " front=" + Bool01(frontSide) +
            " wristMode=" + GetChestHoldFrontBackWristMode(frontSide) +
            " handPos=" + FormatVector3(handPos) +
            " nipple=" + FormatVector3(nippleTarget) +
            " palmTarget=" + FormatVector3(palmTarget) +
            " nippleDistance=" + Vector3.Distance(handPos, nippleTarget).ToString("F3", CultureInfo.InvariantCulture) +
            " palmDistance=" + Vector3.Distance(handPos, palmTarget).ToString("F3", CultureInfo.InvariantCulture) +
            " t=" + t.ToString("F3", CultureInfo.InvariantCulture));

        if (actualRightHand)
            chestHoldFinalRightLogged = true;
        else
            chestHoldFinalLeftLogged = true;
    }

    private Vector3 GetNipplePairSideGrabTarget(Vector3 nippleTarget, Vector3 side, bool rightSide)
    {
        Vector3 lateral = GetSideOffset(rightSide, side, GetGrabWidth());
        return nippleTarget + lateral;
    }

    private Vector3 GetNipplePairOutwardAxis(bool rightSide)
    {
        // v3.0aj:
        // pufupufu の押し出し方向は通常Grab用のroot横軸ではなく、
        // 旧版と同じく lrnipple/chestControl の回転から取る。
        Vector3 side = GetNipplePairSideAxis();
        if (side.sqrMagnitude > 0.0001f)
            return GetSideOffset(rightSide, side.normalized, 1.0f).normalized;

        return rightSide ? Vector3.right : Vector3.left;
    }

    private Quaternion GetPalmOrSoleRotation(Vector3 controlPosition, Vector3 center, Vector3 eulerOffset, bool hand, bool rightSide)
    {
        return GetPalmOrSoleRotation(controlPosition, center, eulerOffset, hand, rightSide, rightSide);
    }

    private Quaternion GetPalmOrSoleRotation(Vector3 controlPosition, Vector3 center, Vector3 eulerOffset, bool hand, bool rightSide, bool actualRightHand)
    {
        return GetPalmOrSoleRotation(controlPosition, Vector3.zero, center, eulerOffset, hand, rightSide, actualRightHand);
    }

    private Quaternion GetPalmOrSoleRotation(Vector3 controlPosition, Vector3 startPosition, Vector3 center, Vector3 eulerOffset, bool hand, bool rightSide, bool actualRightHand)
    {
        // v1.4:
        // 手のひら合わせは LookRotation でターゲット中心へ向けると、手首が内側へ折れやすい。
        // そのため手だけはVaM上で確認した左右の握り角度をベースにする。
        // Hand Palm Add Rot X/Y/Z は、この固定角度への微調整として使う。
        if (hand)
        {
            bool frontSide = IsTargetPersonMode() && selectedTargetPerson != null
                ? IsGrabberInFrontOfTargetPerson(center)
                : false;
            return GetFixedHandRotation(controlPosition, startPosition, center, eulerOffset, rightSide, actualRightHand, frontSide);
        }

        // 足裏は従来通り、ターゲット中心へ向ける方式を維持する。
        Vector3 faceAxis = center - controlPosition;
        if (faceAxis.sqrMagnitude < 0.0001f)
            faceAxis = selectedTargetAtom != null ? -selectedTargetAtom.transform.forward : Vector3.forward;

        faceAxis.Normalize();

        Vector3 upAxis = Vector3.up;
        if (selectedPerson != null)
            upAxis = selectedPerson.transform.up;

        if (Mathf.Abs(Vector3.Dot(faceAxis, upAxis)) > 0.95f)
            upAxis = selectedPerson != null ? selectedPerson.transform.forward : Vector3.forward;

        Quaternion baseRot = Quaternion.LookRotation(faceAxis, upAxis);

        float xOffset = eulerOffset.x;
        float yOffset = eulerOffset.y;
        float zOffset = eulerOffset.z;

        if (!rightSide)
            yOffset = -yOffset;

        return baseRot * Quaternion.Euler(xOffset, yOffset, zOffset);
    }


    private void SnapHandControlPosition(FreeControllerV3 handControl, Vector3 target)
    {
        if (handControl == null)
            return;

        EnsurePositionStateOn(handControl);
        handControl.transform.position = target;
        if (handControl.control != null)
            handControl.control.position = target;
    }

    private Vector3 GetHugBodyThreeStepOpenPoint(HandFinalPointRoute route, bool pathRightSide, Vector3 start)
    {
        Vector3 side = route.side.sqrMagnitude > 0.0001f ? route.side.normalized : Vector3.right;
        Vector3 depth = route.depthAxis.sqrMagnitude > 0.0001f ? route.depthAxis.normalized : GetFinalPointDepthAxis(route.handCenter);
        depth.y = 0.0f;
        if (depth.sqrMagnitude > 0.0001f)
            depth.Normalize();

        float startDepth = 0.0f;
        if (depth.sqrMagnitude > 0.0001f)
            startDepth = Vector3.Dot(start - route.handCenter, depth);

        Vector3 openPoint = route.handCenter + depth * startDepth + GetSideOffset(pathRightSide, side, route.pathWidth);
        // Step1 is only an open-left/right phase.  Keep the current hand height so the elbow does
        // not solve by diving behind before the forward extension begins.
        openPoint.y = start.y;
        return openPoint;
    }

    private void MoveHugBodyHandControlThreeStep(FreeControllerV3 handControl, HandFinalPointRoute route, bool pathRightSide, bool actualRightHand, bool immediate, bool logRoute)
    {
        if (handControl == null)
            return;

        Vector3 finalTarget = GetHandRouteFinalPoint(route, pathRightSide);
        Vector3 forwardWideTarget = route.handCenter + GetSideOffset(pathRightSide, route.side, route.pathWidth);

        if (immediate)
        {
            MoveHandControlThenRotate(handControl, finalTarget, route.handCenter, pathRightSide, actualRightHand, true);
            return;
        }

        EnsurePositionStateOn(handControl);

        float t = GetMoveTLinear();
        Vector3 start;
        if (!grabStartPositions.TryGetValue(handControl, out start))
            start = GetControlPosition(handControl);

        Vector3 openTarget = GetHugBodyThreeStepOpenPoint(route, pathRightSide, start);

        Vector3 routePosition;
        string phase;
        if (t < 0.333333f)
        {
            float u = Mathf.Clamp01(t / 0.333333f);
            routePosition = Vector3.Lerp(start, openTarget, u);
            phase = "step1-open";
        }
        else if (t < 0.666667f)
        {
            float u = Mathf.Clamp01((t - 0.333333f) / 0.333334f);
            routePosition = Vector3.Lerp(openTarget, forwardWideTarget, u);
            phase = "step2-forward-wide";
        }
        else
        {
            float u = Mathf.Clamp01((t - 0.666667f) / 0.333333f);
            routePosition = Vector3.Lerp(forwardWideTarget, finalTarget, u);
            phase = "step3-close";
        }

        handControl.transform.position = routePosition;
        if (handControl.control != null)
            handControl.control.position = routePosition;

        bool doIkSnap = t >= HUG_BODY_IK_SNAP_START_T;
        if (IsHugBodyTarget() && IsSelfHandControl(handControl))
            hugBodyHandSnapAnchorPositions[handControl] = finalTarget;

        if (doIkSnap)
        {
            Vector3 beforeSnap = GetControlPosition(handControl);
            bool snapped = SnapIKControlToBody(selectedPerson, handControl);
            if (IsDebugEnabled())
                DebugLog("[HAND IK SNAP] hand=" + (actualRightHand ? "R" : "L") +
                    " snapped=" + Bool01(snapped) +
                    " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
                    " snapStart=" + HUG_BODY_IK_SNAP_START_T.ToString("F3", CultureInfo.InvariantCulture) +
                    " route=" + FormatVector3(beforeSnap) +
                    " ik=" + FormatVector3(GetControlPosition(handControl)) +
                    " final=" + FormatVector3(finalTarget));

            Vector3 afterSnap = GetControlPosition(handControl);
            DebugLog("[HUG BODY SNAP FINAL CHECK] hand=" + (actualRightHand ? "R" : "L") +
                " snapped=" + Bool01(snapped) +
                " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
                " center=" + FormatVector3(route.handCenter) +
                " final=" + FormatVector3(finalTarget) +
                " routeBeforeSnap=" + FormatVector3(beforeSnap) +
                " ikAfterSnap=" + FormatVector3(afterSnap) +
                " finalToIk=" + Vector3.Distance(finalTarget, afterSnap).ToString("F3", CultureInfo.InvariantCulture) +
                " centerToIk=" + Vector3.Distance(route.handCenter, afterSnap).ToString("F3", CultureInfo.InvariantCulture) +
                " centerToFinal=" + Vector3.Distance(route.handCenter, finalTarget).ToString("F3", CultureInfo.InvariantCulture));
        }

        if (logRoute)
        {
            DebugLog("[HUG BODY THREE STEP]" +
                " hand=" + (actualRightHand ? "R" : "L") +
                " phase=" + phase +
                " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
                " pathRight=" + Bool01(pathRightSide) +
                " center=" + FormatVector3(route.handCenter) +
                " depthAxis=" + FormatVector3(route.depthAxis) +
                " side=" + FormatVector3(route.side) +
                " start=" + FormatVector3(start) +
                " step1Open=" + FormatVector3(openTarget) +
                " step2Forward=" + FormatVector3(forwardWideTarget) +
                " final=" + FormatVector3(finalTarget) +
                " current=" + FormatVector3(GetControlPosition(handControl)) +
                " pathWidth=" + route.pathWidth.ToString("F3", CultureInfo.InvariantCulture) +
                " finalWidth=" + GetFinalGrabWidth().ToString("F3", CultureInfo.InvariantCulture));
        }

        if (t < 1.0f && !doIkSnap)
        {
            if (IsDebugEnabled())
                DebugLog("[WRIST AUTO WAIT] hand=" + (actualRightHand ? "R" : "L") +
                    " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
                    " target=" + FormatVector3(finalTarget) +
                    " current=" + FormatVector3(GetControlPosition(handControl)));
            return;
        }

        if (IsPeniMode())
            return;

        if (!ShouldAlignHandPalm())
        {
            LogWristAutoSkipDebug("align-hand-palm-off", GetControlPosition(handControl), route.handCenter, pathRightSide, actualRightHand, false);
            return;
        }

        Vector3 movedPosition = GetControlPosition(handControl);
        Vector3 startPosition;
        if (!grabStartPositions.TryGetValue(handControl, out startPosition))
            startPosition = movedPosition;

        Quaternion rotation = GetPalmOrSoleRotation(movedPosition, startPosition, route.handCenter, GetHandRotationOffset(), true, pathRightSide, actualRightHand);
        MoveControl(handControl, movedPosition, rotation, true, true);
    }


    private bool ApplyGenFinalWristDownAtFinalStep(FreeControllerV3 handControl, bool actualRightHand, Vector3 target, Vector3 center)
    {
        if (!IsGenTarget())
            return false;

        if (handControl == null)
            return true;

        WristArmPose pose = GetWristButtonArmPose("Down");
        if (pose == null)
            return true;

        Quaternion targetLocalRot = actualRightHand ? pose.RHand.LocalRot : pose.LHand.LocalRot;
        int applied = ApplyWristButtonHandLocked(handControl, actualRightHand, "Down", targetLocalRot);

        if (IsDebugEnabled())
        {
            DebugLog("[GEN FINAL WRIST DOWN] hand=" + (actualRightHand ? "R" : "L") +
                " applied=" + applied.ToString(CultureInfo.InvariantCulture) +
                " positionLocked=1" +
                " target=" + FormatVector3(target) +
                " current=" + FormatVector3(GetControlPosition(handControl)) +
                " center=" + FormatVector3(center));
        }

        // Gen uses this fixed final wrist step instead of the generic palm-auto rotation.
        return true;
    }

    private void MoveHandControlThenRotateViaMidpoint(FreeControllerV3 handControl, Vector3 midTarget, Vector3 finalTarget, Vector3 center, bool pathRightSide, bool actualRightHand, bool immediate, bool useMidpointRoute)
    {
        if (handControl == null)
            return;

        if (!useMidpointRoute)
        {
            MoveHandControlThenRotate(handControl, finalTarget, center, pathRightSide, actualRightHand, immediate);
            return;
        }

        if (immediate)
        {
            MoveHandControlThenRotate(handControl, finalTarget, center, pathRightSide, actualRightHand, true);
            return;
        }

        EnsurePositionStateOn(handControl);

        float t = GetMoveTLinear();
        Vector3 start;
        if (!grabStartPositions.TryGetValue(handControl, out start))
            start = GetControlPosition(handControl);

        Vector3 routePosition;
        if (t < 0.50f)
        {
            float u = Mathf.Clamp01(t / 0.50f);
            routePosition = Vector3.Lerp(start, midTarget, u);
        }
        else
        {
            float u = Mathf.Clamp01((t - 0.50f) / 0.50f);
            routePosition = Vector3.Lerp(midTarget, finalTarget, u);
        }

        handControl.transform.position = routePosition;
        if (handControl.control != null)
            handControl.control.position = routePosition;

        bool doIkSnap = t >= HUG_BODY_IK_SNAP_START_T;
        if (IsHugBodyTarget() && IsSelfHandControl(handControl))
            hugBodyHandSnapAnchorPositions[handControl] = finalTarget;
        if (doIkSnap)
        {
            Vector3 beforeSnap = GetControlPosition(handControl);
            bool snapped = SnapIKControlToBody(selectedPerson, handControl);
            if (IsDebugEnabled())
                DebugLog("[HAND IK SNAP] hand=" + (actualRightHand ? "R" : "L") +
                    " snapped=" + Bool01(snapped) +
                    " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
                    " snapStart=" + HUG_BODY_IK_SNAP_START_T.ToString("F3", CultureInfo.InvariantCulture) +
                    " route=" + FormatVector3(beforeSnap) +
                    " ik=" + FormatVector3(GetControlPosition(handControl)) +
                    " final=" + FormatVector3(finalTarget));

            if (IsHugBodyTarget())
            {
                Vector3 afterSnap = GetControlPosition(handControl);
                DebugLog("[HUG BODY SNAP FINAL CHECK] hand=" + (actualRightHand ? "R" : "L") +
                    " snapped=" + Bool01(snapped) +
                    " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
                    " center=" + FormatVector3(center) +
                    " final=" + FormatVector3(finalTarget) +
                    " routeBeforeSnap=" + FormatVector3(beforeSnap) +
                    " ikAfterSnap=" + FormatVector3(afterSnap) +
                    " finalToIk=" + Vector3.Distance(finalTarget, afterSnap).ToString("F3", CultureInfo.InvariantCulture) +
                    " centerToIk=" + Vector3.Distance(center, afterSnap).ToString("F3", CultureInfo.InvariantCulture) +
                    " centerToFinal=" + Vector3.Distance(center, finalTarget).ToString("F3", CultureInfo.InvariantCulture));
            }
        }

        if (IsDebugEnabled())
            DebugLog("[HAND MID ROUTE] hand=" + (actualRightHand ? "R" : "L") +
                " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
                " pathRight=" + Bool01(pathRightSide) +
                " mid=" + FormatVector3(midTarget) +
                " final=" + FormatVector3(finalTarget) +
                " current=" + FormatVector3(GetControlPosition(handControl)));

        if (t < 1.0f && !doIkSnap)
        {
            if (IsDebugEnabled())
                DebugLog("[WRIST AUTO WAIT] hand=" + (actualRightHand ? "R" : "L") +
                    " t=" + t.ToString("F3", CultureInfo.InvariantCulture) +
                    " target=" + FormatVector3(finalTarget) +
                    " current=" + FormatVector3(GetControlPosition(handControl)));
            return;
        }

        if (IsPeniMode())
            return;

        if (ApplyGenFinalWristDownAtFinalStep(handControl, actualRightHand, finalTarget, center))
            return;

        if (!ShouldAlignHandPalm())
        {
            LogWristAutoSkipDebug("align-hand-palm-off", GetControlPosition(handControl), center, pathRightSide, actualRightHand, false);
            return;
        }

        Vector3 movedPosition = GetControlPosition(handControl);
        Vector3 startPosition;
        if (!grabStartPositions.TryGetValue(handControl, out startPosition))
            startPosition = movedPosition;

        Quaternion rotation = GetPalmOrSoleRotation(movedPosition, startPosition, center, GetHandRotationOffset(), true, pathRightSide, actualRightHand);
        MoveControl(handControl, movedPosition, rotation, true, true);
    }

    private void MoveHandControlThenRotate(FreeControllerV3 handControl, Vector3 target, Vector3 center, bool pathRightSide, bool actualRightHand, bool immediate)
    {
        if (handControl == null)
            return;

        // First move only. Wrist/palm rotation is decided after the hand reaches the moved position.
        MoveControl(handControl, target, Quaternion.identity, false, immediate);

        if (IsPeniMode())
        {
            if (IsDebugEnabled())
                DebugLog("[WRIST PENI SKIP] rotation-off=1 hand=" + (actualRightHand ? "R" : "L") +
                    " target=" + FormatVector3(target) +
                    " center=" + FormatVector3(center));
            return;
        }

        if (IsGenTarget())
        {
            if (!immediate && GetMoveTLinear() < 1.0f)
            {
                if (IsDebugEnabled())
                    DebugLog("[WRIST AUTO WAIT] hand=" + (actualRightHand ? "R" : "L") +
                        " mode=gen-final-down" +
                        " t=" + GetMoveTLinear().ToString("F3", CultureInfo.InvariantCulture) +
                        " target=" + FormatVector3(target) +
                        " current=" + FormatVector3(GetControlPosition(handControl)));
                return;
            }

            ApplyGenFinalWristDownAtFinalStep(handControl, actualRightHand, target, center);
            return;
        }

        if (!ShouldAlignHandPalm())
        {
            LogWristAutoSkipDebug("align-hand-palm-off", GetControlPosition(handControl), center, pathRightSide, actualRightHand, false);
            return;
        }

        if (!immediate && GetMoveTLinear() < 1.0f)
        {
            if (IsDebugEnabled())
                DebugLog("[WRIST AUTO WAIT] hand=" + (actualRightHand ? "R" : "L") +
                    " t=" + GetMoveTLinear().ToString("F3", CultureInfo.InvariantCulture) +
                    " target=" + FormatVector3(target) +
                    " current=" + FormatVector3(GetControlPosition(handControl)));
            return;
        }

        // Final IK snap means snapping the IK control to the actual body hand, not forcing the
        // control onto the requested target point.  Only do this for Hug Body fallback paths.
        if ((immediate || GetMoveTLinear() >= 1.0f) && IsHugBodyTarget())
        {
            if (IsSelfHandControl(handControl))
                hugBodyHandSnapAnchorPositions[handControl] = target;
            SnapIKControlToBody(selectedPerson, handControl);
        }

        Vector3 movedPosition = GetControlPosition(handControl);
        Vector3 startPosition;
        if (!grabStartPositions.TryGetValue(handControl, out startPosition))
            startPosition = movedPosition;

        Quaternion rotation = GetPalmOrSoleRotation(movedPosition, startPosition, center, GetHandRotationOffset(), true, pathRightSide, actualRightHand);
        MoveControl(handControl, movedPosition, rotation, true, true);
    }

    private void SetControlTransformNoIKStateChange(FreeControllerV3 fc, Vector3 position, Quaternion rotation, bool applyRotation)
    {
        if (fc == null)
            return;

        // Do not call EnsurePositionStateOn / EnsureRotationStateOn here.
        // This is used by Chest Hold nipple utility moves so nipple IK state is preserved.
        try
        {
            fc.transform.position = position;
            if (fc.control != null)
                fc.control.position = position;
        }
        catch { }

        if (applyRotation)
        {
            try
            {
                fc.transform.rotation = rotation;
                if (fc.control != null)
                    fc.control.rotation = rotation;
            }
            catch { }
        }
    }

    private void MoveControl(FreeControllerV3 fc, Vector3 target, bool immediate)
    {
        MoveControl(fc, target, Quaternion.identity, false, immediate);
    }

    private void EnsurePositionStateOn(FreeControllerV3 fc)
    {
        if (fc == null || positionStateOnControls.Contains(fc))
            return;

        try
        {
            fc.currentPositionState = FreeControllerV3.PositionState.On;
            positionStateOnControls.Add(fc);
        }
        catch { }
    }

    private void EnsureRotationStateOn(FreeControllerV3 fc)
    {
        if (fc == null || rotationStateOnControls.Contains(fc))
            return;

        try
        {
            fc.currentRotationState = FreeControllerV3.RotationState.On;
            rotationStateOnControls.Add(fc);
        }
        catch { }
    }

    private void ApplyTemporaryHandRotationOffIfNeeded(bool includeHands, HashSet<FreeControllerV3> skipControls = null)
    {
        if (!includeHands)
            return;

        if (leftHandJSON != null && leftHandJSON.val)
        {
            if (skipControls != null && lHandControl != null && skipControls.Contains(lHandControl))
            {
                if (IsDebugEnabled())
                    DebugLog("[CHEST HOLD BACK NEAR WRIST SKIP] hand=L / skip=apply-rot-off");
            }
            else
            {
                TemporarilyTurnHandRotationOff(lHandControl, "L");
            }
        }

        if (rightHandJSON != null && rightHandJSON.val)
        {
            if (skipControls != null && rHandControl != null && skipControls.Contains(rHandControl))
            {
                if (IsDebugEnabled())
                    DebugLog("[CHEST HOLD BACK NEAR WRIST SKIP] hand=R / skip=apply-rot-off");
            }
            else
            {
                TemporarilyTurnHandRotationOff(rHandControl, "R");
            }
        }
    }

    private void TemporarilyTurnHandRotationOff(FreeControllerV3 handControl, string label)
    {
        if (handControl == null)
            return;

        if (!temporaryHandRotationOffStates.ContainsKey(handControl))
            temporaryHandRotationOffStates[handControl] = handControl.currentRotationState;

        try
        {
            handControl.currentRotationState = FreeControllerV3.RotationState.Off;
            if (IsDebugEnabled())
                DebugLog("[HAND ROT OFF] hand=" + label + " reason=grab-start");
        }
        catch { }
    }

    private void RestoreTemporaryHandRotationOffStates(HashSet<FreeControllerV3> skipControls = null)
    {
        if (temporaryHandRotationOffStates.Count == 0)
            return;

        List<KeyValuePair<FreeControllerV3, FreeControllerV3.RotationState>> states = temporaryHandRotationOffStates.ToList();
        temporaryHandRotationOffStates.Clear();

        foreach (KeyValuePair<FreeControllerV3, FreeControllerV3.RotationState> item in states)
        {
            if (item.Key == null)
                continue;

            if (skipControls != null && skipControls.Contains(item.Key))
            {
                temporaryHandRotationOffStates[item.Key] = item.Value;
                if (IsDebugEnabled())
                    DebugLog("[CHEST HOLD BACK NEAR WRIST SKIP] hand=" + (item.Key == rHandControl ? "R" : item.Key == lHandControl ? "L" : "?") + " / skip=restore-rot-state");
                continue;
            }

            try
            {
                item.Key.currentRotationState = item.Value;
            }
            catch { }
        }
    }

    private void MoveControl(FreeControllerV3 fc, Vector3 target, Quaternion targetRotation, bool applyRotation, bool immediate)
    {
        if (fc == null)
            return;

        EnsurePositionStateOn(fc);

        if (applyRotation)
            EnsureRotationStateOn(fc);

        float t = immediate ? 1.0f : GetMoveTLinear();

        Vector3 start;
        if (!grabStartPositions.TryGetValue(fc, out start))
            start = fc.control != null ? fc.control.position : fc.transform.position;

        Vector3 next = Vector3.Lerp(start, target, t);

        fc.transform.position = next;
        if (fc.control != null)
            fc.control.position = next;

        if (applyRotation)
        {
            Quaternion startRot;
            if (!grabStartRotations.TryGetValue(fc, out startRot))
                startRot = fc.control != null ? fc.control.rotation : fc.transform.rotation;

            Quaternion nextRot = Quaternion.Slerp(startRot, targetRotation, t);

            fc.transform.rotation = nextRot;
            if (fc.control != null)
                fc.control.rotation = nextRot;
        }
    }

    private void Release()
    {
        ResetHugBodyHdcHipUpperState("release");
        hasActiveGrab = false;
        ClearChestHoldNippleHandFollow("release");
        ResetChestHoldFrontLeftGraspBoostState("release");
        ResetChestHoldFrontRightGraspBoostState("release");
        RestoreHeldTargetHandFollowLocks("release");
        ClearHeldTargetGrabState();
        ClearPendingWristHandLocks();
        grabElapsed = 0.0f;
        activeMoveTimeMultiplier = 1.0f;
        activeIncludeHead = false;
        pufupufuActive = false;

        if (jobActive)
            RestoreJobHandPositions();

        jobActive = false;
        RestoreSelfFollowParentLinks();
        RestoreTemporaryRelaxLinkedIK();
        RestoreTargetNoneBodyRelaxIK();
        RestoreTemporaryHandRotationOffStates();
        RestoreChestHoldNippleIKStabilize("release");
        ReleaseSelectedTargetNippleIK("release");
        StopSwoonDrop(true, "release");

        // 今回このプラグインがONにしたControlだけを、Release/復帰対象として退避する。
        // ここで退避してから作業用HashSetをClearするのが重要。
        releaseRestorePositionControls.Clear();
        releaseRestoreRotationControls.Clear();

        foreach (FreeControllerV3 fc in positionStateOnControls)
        {
            if (fc != null)
                releaseRestorePositionControls.Add(fc);
        }

        foreach (FreeControllerV3 fc in rotationStateOnControls)
        {
            if (fc != null)
                releaseRestoreRotationControls.Add(fc);
        }

        ReleaseTrackedControls();

        grabStartPositions.Clear();
        grabStartRotations.Clear();
        positionStateOnControls.Clear();
        rotationStateOnControls.Clear();
        pendingAutoSnapIKControls.Clear();

        releaseRestoreIKPending = true;
        releaseRestoreIKTime = Time.time + RELEASE_RESTORE_IK_DELAY;

        SetStatus("Released");
        UpdateReleaseButtonColors();
    }

    private void UpdateReleaseRestoreIK()
    {
        if (!releaseRestoreIKPending)
            return;

        if (hasActiveGrab)
        {
            releaseRestoreIKPending = false;
            releaseRestorePositionControls.Clear();
            releaseRestoreRotationControls.Clear();
            return;
        }

        if (Time.time < releaseRestoreIKTime)
            return;

        releaseRestoreIKPending = false;

        RestoreTrackedControls();

        releaseRestorePositionControls.Clear();
        releaseRestoreRotationControls.Clear();

        SetStatus("Released / IK restored");
        UpdateReleaseButtonColors();
    }

    private void ReleaseTrackedControls()
    {
        HashSet<FreeControllerV3> all = new HashSet<FreeControllerV3>();

        foreach (FreeControllerV3 fc in releaseRestorePositionControls)
        {
            if (fc != null)
                all.Add(fc);
        }

        foreach (FreeControllerV3 fc in releaseRestoreRotationControls)
        {
            if (fc != null)
                all.Add(fc);
        }

        foreach (FreeControllerV3 fc in all)
        {
            bool releasePosition = releaseRestorePositionControls.Contains(fc);
            bool releaseRotation = releaseRestoreRotationControls.Contains(fc);
            ReleaseControlSelective(fc, releasePosition, releaseRotation);
        }
    }

    private void RestoreTrackedControls()
    {
        HashSet<FreeControllerV3> all = new HashSet<FreeControllerV3>();

        foreach (FreeControllerV3 fc in releaseRestorePositionControls)
        {
            if (fc != null)
                all.Add(fc);
        }

        foreach (FreeControllerV3 fc in releaseRestoreRotationControls)
        {
            if (fc != null)
                all.Add(fc);
        }

        foreach (FreeControllerV3 fc in all)
        {
            bool restorePosition = releaseRestorePositionControls.Contains(fc);
            bool restoreRotation = releaseRestoreRotationControls.Contains(fc);
            RestoreIKControlSelective(fc, restorePosition, restoreRotation);
        }
    }

    private void RestoreIKControlSelective(FreeControllerV3 fc, bool restorePosition, bool restoreRotation)
    {
        if (fc == null)
            return;

        if (restorePosition)
        {
            try
            {
                fc.currentPositionState = FreeControllerV3.PositionState.On;
            }
            catch { }
        }

        if (restoreRotation)
        {
            try
            {
                fc.currentRotationState = FreeControllerV3.RotationState.On;
            }
            catch { }
        }
    }

    private void ReleaseControlSelective(FreeControllerV3 fc, bool releasePosition, bool releaseRotation)
    {
        if (fc == null)
            return;

        if (releasePosition)
        {
            try
            {
                fc.currentPositionState = FreeControllerV3.PositionState.Off;
            }
            catch { }
        }

        if (releaseRotation)
        {
            try
            {
                fc.currentRotationState = FreeControllerV3.RotationState.Off;
            }
            catch
            {
                // VaM環境差異対策。Offが効かない場合も現在値で維持される。
            }
        }
    }

    private void SetStatus(string text)
    {
        if (statusJSON != null)
            statusJSON.val = text;

        // Grab中は毎フレーム呼ばれるため、Debug ONでもログ洪水を避ける。
        // エラー・無効系だけログに残す。
        if (string.IsNullOrEmpty(text))
            return;

        if (text.StartsWith("Grab applied"))
            return;

        DebugLog("[STATUS] " + text);
    }

    private bool IsDebugEnabled()
    {
        return debugLogJSON != null && debugLogJSON.val;
    }

    private void DebugLog(string text)
    {
        if (IsDebugEnabled())
            SuperController.LogMessage("[TargetGrabber] " + text);
    }

    private string Bool01(bool value)
    {
        return value ? "1" : "0";
    }
}
