// ============================================================
// TargetGrabber.cs
// Version: v4.0bx_hug_wrist_finalpos_pick
// Date: 2026-06-21
// Base: TargetGrabber_v4_0bw_hug_wrist_depth_target_pick.cs
// Summary:
// - Fixes Hug Body wrist depth pick to use the actual final hand position only, not any earlier cached open-path target.
// - Hug Body final wrist: actual final front/actor side = Wrist Out; near-center/back/unknown = Wrist In.
// - Uses the same fixed wrist-button preset rotations for final Hug Body In/Out application.
// - Keeps Hug Body handCenter far/inside correction fade-out when Hug Mode is OFF.
// - Keeps Final Grab Width zero/default values clamped to 0.01 internally.
// ============================================================
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// FILE: TargetGrabber.cs
// VAM Target Grabber
//
// 指定Atomを手・足で掴む補助プラグイン
//
// Author : VAMT
// Version: v4.0bx_hug_wrist_finalpos_pick
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
// v4.0bn: Uses a left/right symmetric visual base for Wrist test buttons instead of pathRight/layout basis.
// v4.0bo: Uses the verified Grab HAND ROT fixed presets for Wrist Straight and fixes the left/right preset swap.
// v4.0bp: Makes Wrist test buttons preset-only; no handRot offset, path/layout, target center, or current-pose basis is used.
// v4.0bq: Wrist buttons use captured arm poses: hand positions locked, hand rotations preset, elbows moved per mode.
// v4.0br: Clears pending wrist hand locks on wrist button start / grab start / release / defaults to avoid stale 8-frame locks.
// v4.0bx: Hug Body wrist picks Out only when the actual final hand position remains on the near/front side; near-center/back defaults to In.
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
    private const float HIP_HOLD_FINAL_GRAB_WIDTH = 0.13f;
    private const float CROTCH_GRAB_WIDTH = 0.00f;
    private const float CROTCH_FINAL_GRAB_WIDTH = MIN_FINAL_GRAB_WIDTH;
    private const float PENI_GRAB_WIDTH = 0.10f;
    private const float PENI_FINAL_GRAB_WIDTH = 0.03f;
    private const float PENI_AUTO_Z_OFFSET = -0.03f;
    private const float HUG_BODY_HAND_WIDTH_CAP = 0.55f;
    private const float HUG_BODY_HAND_CENTER_OFFSET = 0.22f;
    private const float HUG_BODY_WRIST_NEAR_CENTER_DISTANCE = 0.03f;
    private const float HUG_BODY_WRIST_DEPTH_THRESHOLD = 0.03f;
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

    private JSONStorableString statusJSON;
    private UIDynamicButton grabHandPullButton;
    private UIDynamicButton grabHandOpenButton;
    private UIDynamicButton releaseTargetButton;
    private UIDynamicButton releaseButton;

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
    private readonly Dictionary<FreeControllerV3, Vector3> grabStartPositions = new Dictionary<FreeControllerV3, Vector3>();
    private readonly Dictionary<FreeControllerV3, Quaternion> grabStartRotations = new Dictionary<FreeControllerV3, Quaternion>();
    private readonly Dictionary<FreeControllerV3, Vector3> targetOriginalPositions = new Dictionary<FreeControllerV3, Vector3>();
    private readonly Dictionary<FreeControllerV3, Quaternion> targetOriginalRotations = new Dictionary<FreeControllerV3, Quaternion>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.PositionState> targetLockPositionStates = new Dictionary<FreeControllerV3, FreeControllerV3.PositionState>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.RotationState> targetLockRotationStates = new Dictionary<FreeControllerV3, FreeControllerV3.RotationState>();
    private readonly List<FreeControllerV3> targetLockControls = new List<FreeControllerV3>();
    private readonly Dictionary<FreeControllerV3, Atom> pendingAutoSnapIKControls = new Dictionary<FreeControllerV3, Atom>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.PositionState> temporaryRelaxPositionStates = new Dictionary<FreeControllerV3, FreeControllerV3.PositionState>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.RotationState> temporaryRelaxRotationStates = new Dictionary<FreeControllerV3, FreeControllerV3.RotationState>();
    private readonly List<FreeControllerV3> temporaryRelaxControls = new List<FreeControllerV3>();
    private readonly Dictionary<FreeControllerV3, FreeControllerV3.RotationState> temporaryHandRotationOffStates = new Dictionary<FreeControllerV3, FreeControllerV3.RotationState>();
    private readonly Dictionary<FreeControllerV3, PendingWristHandLock> pendingWristHandLocks = new Dictionary<FreeControllerV3, PendingWristHandLock>();
    private readonly Dictionary<FreeControllerV3, Vector3> hugBodyWristReferencePositions = new Dictionary<FreeControllerV3, Vector3>();
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
    private Vector3 jobLeftBase = Vector3.zero;
    private Vector3 jobRightBase = Vector3.zero;
    private float lastSideDebugTime = -10.0f;
    private float lastHandRotationDebugTime = -10.0f;
    private float lastHandTargetDebugTime = -10.0f;
    private bool releaseRestoreIKPending = false;
    private float releaseRestoreIKTime = 0.0f;
    private ColorBlock releaseTargetDefaultColors;
    private ColorBlock releaseDefaultColors;
    private bool releaseTargetColorsCaptured = false;
    private bool releaseColorsCaptured = false;
    private const float RELEASE_RESTORE_IK_DELAY = 3.00f;

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
        UIDynamicPopup personPopup = CreateFilterablePopup(personChooser);
        if (personPopup != null)
            personPopup.popup.onOpenPopupHandlers += UpdatePersonChoices;

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
            new List<string> { NONE },
            NONE,
            "Target Controller",
            (JSONStorableStringChooser.SetStringCallback)OnTargetPersonPartChanged
        );
        RegisterStringChooser(targetPersonPartChooser);
        UIDynamicPopup targetControllerPopup = CreateFilterablePopup(targetPersonPartChooser);
        if (targetControllerPopup != null)
            targetControllerPopup.popup.onOpenPopupHandlers += UpdateTargetPersonControllerChoices;

        targetControllerFilterJSON = new JSONStorableString("Target Ctrl Filter", "");
        RegisterString(targetControllerFilterJSON);
        CreateTextField(targetControllerFilterJSON, false);

        debugLogJSON = CreateBool("Debug Log", false, false);

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

        // 右側UI: 操作系だけを上から順にまとめる。
        CreateButton("Grab Hand", true).button.onClick.AddListener(GrabHand);
        grabHandPullButton = CreateButton("Grab Hand Pull", true);
        grabHandPullButton.button.onClick.AddListener(GrabHandPull);
        grabHandOpenButton = CreateButton("Grab Hand Open", true);
        grabHandOpenButton.button.onClick.AddListener(GrabHandOpen);
        leftHandJSON = CreateBool("Left Hand", true, true);
        rightHandJSON = CreateBool("Right Hand", true, true);

        CreateButton("Grab Foot", true).button.onClick.AddListener(GrabFoot);
        leftFootJSON = CreateBool("Left Foot", true, true);
        rightFootJSON = CreateBool("Right Foot", true, true);

        CreateButton("Grab Selected", true).button.onClick.AddListener(GrabSelected);
        CreateButton("pufupufu", true).button.onClick.AddListener(Pufupufu);
        CreateButton("job", true).button.onClick.AddListener(Job);
        releaseTargetButton = CreateButton("Release Target", true);
        releaseTargetButton.button.onClick.AddListener(ReleaseTarget);
        CaptureButtonDefaultColors(releaseTargetButton, ref releaseTargetDefaultColors, ref releaseTargetColorsCaptured);

        releaseButton = CreateButton("Release", true);
        releaseButton.button.onClick.AddListener(Release);
        CaptureButtonDefaultColors(releaseButton, ref releaseDefaultColors, ref releaseColorsCaptured);

        followTargetJSON = new JSONStorableBool("Follow Target", false);
        RegisterBool(followTargetJSON);
        followTargetJSON.setCallbackFunction = OnLegacyFollowTargetChanged;

        followModeChooser = new JSONStorableStringChooser(
            "Follow Mode",
            new List<string> { FOLLOW_OFF, FOLLOW_SELF, FOLLOW_TARGET },
            FOLLOW_OFF,
            "Follow Mode",
            (JSONStorableStringChooser.SetStringCallback)OnFollowModeChanged
        );
        RegisterStringChooser(followModeChooser);
        CreatePopup(followModeChooser, true);

        autoSnapPullOpenIKJSON = CreateBool("Auto Snap Pull/Open IK", true, true);
        CreateButton("Self IK Default", true).button.onClick.AddListener(SelfIKDefault);
        CreateButton("Load User Defaults", true).button.onClick.AddListener(LoadUserDefaults);

        handWristAngleJSON = CreateBool("Use Hand Wrist Angle", true, true);
        CreateButton("Wrist Straight", true).button.onClick.AddListener(delegate { ApplyBothHandWristTest("Straight"); });
        CreateButton("Wrist In", true).button.onClick.AddListener(delegate { ApplyBothHandWristTest("In"); });
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

        DebugLog("ready / v4.0bx_hug_wrist_finalpos_pick / hug-body-finalpos-out-only / v4.0bp-preset-only");
    }

    private void RegisterExternalActions()
    {
        RegisterAction(new JSONStorableAction("Refresh", RefreshAll));
        RegisterAction(new JSONStorableAction("Default", ApplyDefaultSettings));
        RegisterAction(new JSONStorableAction("Log Target Intimate Names", LogTargetIntimateNames));
        RegisterAction(new JSONStorableAction("Kiss", GrabHead));
        RegisterAction(new JSONStorableAction("Grab Hand", GrabHand));
        RegisterAction(new JSONStorableAction("Wrist Straight", delegate { ApplyBothHandWristTest("Straight"); }));
        RegisterAction(new JSONStorableAction("Wrist In", delegate { ApplyBothHandWristTest("In"); }));
        RegisterAction(new JSONStorableAction("Wrist Out", delegate { ApplyBothHandWristTest("Out"); }));
        RegisterAction(new JSONStorableAction("Wrist Up", delegate { ApplyBothHandWristTest("Up"); }));
        RegisterAction(new JSONStorableAction("Wrist Down", delegate { ApplyBothHandWristTest("Down"); }));
        RegisterAction(new JSONStorableAction("Grab Head", GrabHead));
        RegisterAction(new JSONStorableAction("Grab Hand Pull", GrabHandPull));
        RegisterAction(new JSONStorableAction("Grab Pull", GrabHandPull));
        RegisterAction(new JSONStorableAction("Grab Hand Open", GrabHandOpen));
        RegisterAction(new JSONStorableAction("Grab Left Hand", GrabLeftHand));
        RegisterAction(new JSONStorableAction("Grab Right Hand", GrabRightHand));
        RegisterAction(new JSONStorableAction("Grab Foot", GrabFoot));
        RegisterAction(new JSONStorableAction("Grab Left Foot", GrabLeftFoot));
        RegisterAction(new JSONStorableAction("Grab Right Foot", GrabRightFoot));
        RegisterAction(new JSONStorableAction("Grab Selected", GrabSelected));
        RegisterAction(new JSONStorableAction("pufupufu", Pufupufu));
        RegisterAction(new JSONStorableAction("job", Job));
        RegisterAction(new JSONStorableAction("Release Target", ReleaseTarget));
        RegisterAction(new JSONStorableAction("Release", Release));
        RegisterAction(new JSONStorableAction("Self IK Default", SelfIKDefault));
        RegisterAction(new JSONStorableAction("Load User Defaults", LoadUserDefaults));
        RegisterAction(new JSONStorableAction("LoadUserDefaults", LoadUserDefaults));
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
            : FirstExistingChoice(choices, TC_HUG_BODY, TC_CHEST_HOLD, TC_HIP_HOLD, TC_HAND, TC_FOOT, TC_KNEE, TC_GEN, TC_PENI_BASE, TC_PENI_MID, TC_PENI_TIP, TC_ANUS, TC_GROIN, TC_HEAD, TC_HEAD_TOP, TC_MOUTH, TC_NECK, TC_ABDOMEN, TC_HIP, TC_L_NIPPLE, TC_R_NIPPLE) ?? NONE;

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
            grabWidth = 2.00f;

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

        if (IsFollowTargetMode() && hasActiveGrab)
            ApplyGrab(false);

        SetStatus("Default applied");
    }

    private void LoadUserDefaults()
    {
        ClearPendingWristHandLocks();

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
            SetStatus("Load User Defaults applied");
            return;
        }

        SetStatus("Load User Defaults action not found");
        SuperController.LogMessage("[TargetGrabber] LOAD USER DEFAULTS: PosePresets action not found.");
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
        if (containingAtom == null)
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
                JSONStorableAction action = storable.GetAction(actionNames[i]);
                if (action == null)
                    continue;

                action.actionCallback.Invoke();
                SuperController.LogMessage("[TargetGrabber] LOAD USER DEFAULTS: pose action=" + storableId + " / " + actionNames[i]);
                return true;
            }
        }

        return false;
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
        UpdateReleaseButtonColors();

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
    }

    private void GrabHand()
    {
        StartTimedGrab(true, false);
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

        float shortage;
        Vector3 pullOffset;
        bool pulled = TryGetGrabPullOffset(out pullOffset, out shortage);
        if (pulled)
        {
            PrepareTemporaryRelaxLinkedIK(pullControls);
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
            QueueAutoSnapPullOpenIK(pullControls);
            QueueSelfFollowParentTargets(pullControls);
        }

        if (pulled)
        {
            SetStatus("Grab Hand Pull / pulled=" + pullOffset.magnitude.ToString("F3", CultureInfo.InvariantCulture) +
                " / shortage=" + shortage.ToString("F3", CultureInfo.InvariantCulture));
        }
        else
        {
            SetStatus("Grab Hand Pull / reachable");
        }
    }

    private void GrabHandOpen()
    {
        ResolveControls();

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

    private void MoveTargetControlByOffset(FreeControllerV3 fc, Vector3 offset)
    {
        if (fc == null || offset.sqrMagnitude < 0.0001f)
            return;

        CaptureTargetOriginal(fc);
        Vector3 pos = fc.control != null ? fc.control.position : fc.transform.position;
        Quaternion rot = fc.control != null ? fc.control.rotation : fc.transform.rotation;
        MoveControl(fc, pos + offset, rot, false, true);
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
        RestoreSelfFollowParentLinks();

        if (!HasTargetReleaseState())
        {
            SetStatus("Release Target / no saved target");
            UpdateGrabHandUtilityButtons();
            return;
        }

        List<FreeControllerV3> controls = targetOriginalPositions.Keys.ToList();
        int restored = 0;

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

            MoveControl(fc, pos, rot, false, true);
            restored++;
        }

        targetOriginalPositions.Clear();
        targetOriginalRotations.Clear();
        int restoredLocks = RestoreTargetLocks();
        UpdateGrabHandUtilityButtons();
        SetStatus("Release Target / restored=" + restored.ToString(CultureInfo.InvariantCulture) +
            " / locks=" + restoredLocks.ToString(CultureInfo.InvariantCulture));
    }

    private void UpdateGrabHandUtilityButtons()
    {
        bool pullEnabled = GetGrabHandPullTargetControls().Count > 0;
        bool openEnabled = false;
        FreeControllerV3 left;
        FreeControllerV3 right;
        if (TryGetGrabHandOpenTargetControls(out left, out right))
            openEnabled = true;
        else
        {
            FreeControllerV3 single;
            bool singleRightSide;
            if (TryGetGrabHandOpenSingleTargetControl(out single, out singleRightSide))
                openEnabled = true;
        }

        if (grabHandPullButton != null && grabHandPullButton.button != null)
            grabHandPullButton.button.interactable = pullEnabled;

        if (grabHandOpenButton != null && grabHandOpenButton.button != null)
            grabHandOpenButton.button.interactable = openEnabled;

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
               temporaryRelaxControls.Count > 0;
    }

    private void UpdateReleaseButtonColors()
    {
        SetButtonWarningColor(releaseTargetButton, releaseTargetDefaultColors, releaseTargetColorsCaptured, HasTargetReleaseState(), new Color(1.00f, 0.62f, 0.20f, 1.0f));
        SetButtonWarningColor(releaseButton, releaseDefaultColors, releaseColorsCaptured, HasSelfReleaseState(), new Color(0.32f, 0.70f, 1.00f, 1.0f));
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
                AddPendingAutoSnapIK(selectedTargetPerson, fc);
        }
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

        fc.transform.position = best.position;
        fc.transform.rotation = best.rotation;
        if (fc.control != null)
        {
            fc.control.position = best.position;
            fc.control.rotation = best.rotation;
        }

        return true;
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

    private void StartTimedGrab(bool includeHands, bool includeFeet, bool keepTemporaryRelaxLinkedIK = false, bool includeHead = false, bool useFinalGrabWidth = false)
    {
        ResolveControls();
        ClearPendingWristHandLocks();

        RestoreSelfFollowParentLinks();

        if (!keepTemporaryRelaxLinkedIK)
            RestoreTemporaryRelaxLinkedIK();
        RestoreTemporaryHandRotationOffStates();

        if (includeHead)
            RelaxKissChestIK();

        activeIncludeHands = includeHands;
        activeIncludeFeet = includeFeet;
        activeIncludeHead = includeHead;
        activeMoveTimeMultiplier = includeFeet && !includeHands ? 2.0f : 1.0f;
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

        ApplyTemporaryHandRotationOffIfNeeded(includeHands);

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

        bool nipplePairMode = IsNipplePairMode();
        bool hipHoldMode = IsHipHoldMode();
        bool targetPairMode = IsTargetPairMode();

        Vector3 center = GetTargetCenter();
        Vector3 handCenter = (nipplePairMode || hipHoldMode || targetPairMode) ? center : GetHugCenter(center);
        Vector3 footCenter = (nipplePairMode || hipHoldMode || targetPairMode) ? center : GetHugCenter(ApplyFootPersonGrabOffset(center));
        Vector3 baseSide = GetTargetSideAxis();
        Vector3 handSide = GetHandSideAxis(baseSide);
        if (IsHugBodyTarget())
            handSide = GetHugBodyApproachSideAxis(center, handSide);
        Vector3 footSide = GetFootSideAxis(baseSide);
        LogSideDebug(center, handSide, footSide);
        bool swapSidePaths = ShouldSwapSidePaths(center);
        LogHandRotationDebug(swapSidePaths, center);
        bool logHandTargetsThisFrame = false;
        if (IsDebugEnabled() && Time.time - lastHandTargetDebugTime >= 0.50f)
        {
            logHandTargetsThisFrame = true;
            lastHandTargetDebugTime = Time.time;
        }
        int moved = 0;
        moved += ApplyHeadGrabIfNeeded(immediate, includeHead, center);

        if (nipplePairMode && (includeHands || includeFeet))
        {
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
            bool hugBodyTarget = IsHugBodyTarget();
            // v4.0bb:
            // Hug Body は体を抱えるため左右幅が必要だが、Auto Grab Width の 1m 超をそのまま使うと
            // maxReach で外側へクランプされ、片手が遠回りしてから中心へ戻る。
            // 通常Grab/Hold系は触らず、Hug Body の通常手ルートだけ実効幅を控えめに上限化する。
            float handPathWidth = hugBodyTarget
                ? Mathf.Min(GetGrabWidth(), HUG_BODY_HAND_WIDTH_CAP)
                : GetGrabWidth();
            bool leftPathRightSideForHands = !swapSidePaths;
            bool rightPathRightSideForHands = swapSidePaths;
            bool effectiveSwapSidePaths = swapSidePaths;
            if (hugBodyTarget)
            {
                HugBodyHandLayout layout = ResolveHugBodyHandLayout(center, handCenter, handSide, handPathWidth, logHandTargetsThisFrame);
                handCenter = layout.handCenter;
                leftPathRightSideForHands = layout.leftPathRightSide;
                rightPathRightSideForHands = layout.rightPathRightSide;
                effectiveSwapSidePaths = layout.rightPathRightSide;
            }

            if (leftHandJSON != null && leftHandJSON.val && lHandControl != null)
            {
                // v3.0al:
                // 左右の行き先は正面/背面のワールド位置で反転しない。
                // 正面/背面判定は回転やHug方向にだけ使い、手の実Control割当は固定する。
                bool pathRightSide = leftPathRightSideForHands;
                Vector3 root = GetHandRootPosition(pathRightSide);
                Vector3 desired = handCenter + GetSideOffset(pathRightSide, handSide, handPathWidth);
                Vector3 target = GetReachLimitedPosition(root, desired, GetMaxHandReach(), GetHandPalmOffset(), lHandControl, true, pathRightSide);
                if (logHandTargetsThisFrame)
                    LogHandTargetDebug(false, lHandControl, root, desired, target, center, handCenter, handSide, handPathWidth, pathRightSide, effectiveSwapSidePaths, immediate);
                // v3.0ag:
                // 回転の正面/背面判定は Hug で動く handCenter ではなく、元のTarget centerで固定する。
                // Hug中にhandCenterが奥へ送られても、正面右手の当たり回転が背面扱いに化けないようにする。
                MoveHandControlThenRotate(lHandControl, target, center, pathRightSide, false, immediate);
                moved++;
            }

            if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
            {
                bool pathRightSide = rightPathRightSideForHands;
                Vector3 root = GetHandRootPosition(pathRightSide);
                Vector3 desired = handCenter + GetSideOffset(pathRightSide, handSide, handPathWidth);
                Vector3 target = GetReachLimitedPosition(root, desired, GetMaxHandReach(), GetHandPalmOffset(), rHandControl, true, pathRightSide);
                if (logHandTargetsThisFrame)
                    LogHandTargetDebug(true, rHandControl, root, desired, target, center, handCenter, handSide, handPathWidth, pathRightSide, effectiveSwapSidePaths, immediate);
                // v3.0ag:
                // 右手回転の正面/背面判定も元のTarget centerで固定する。
                // 正面右手はv3.0ab/afの当たりを維持し、背面右手だけ別プリセットへ切り替えられるようにする。
                MoveHandControlThenRotate(rHandControl, target, center, pathRightSide, true, immediate);
                moved++;
            }
        }

        if (includeFeet)
        {
            if (leftFootJSON != null && leftFootJSON.val)
            {
                // v3.0al:
                // 足も手と同じく、正面/背面のワールド位置で左右行き先を反転しない。
                bool pathRightSide = !swapSidePaths;
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
                // v3.0al:
                // 足も手と同じく、正面/背面のワールド位置で左右行き先を反転しない。
                bool pathRightSide = swapSidePaths;
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
        Vector3 leftSideTarget;
        Vector3 rightSideTarget;
        string mode;

        if (!TryGetAssignedNippleTargets(out leftSideTarget, out rightSideTarget, out mode))
        {
            SetStatus("Chest Hold invalid / angle or target not ready");
            DebugLog("[CHEST HOLD] invalid");
            return;
        }

        Vector3 zOffset = GetNipplePairZOffsetVector();
        leftSideTarget += zOffset;
        rightSideTarget += zOffset;
        Vector3 footSide = GetFootSideAxis(side);
        Vector3 rawLeftSideTarget = leftSideTarget;
        Vector3 rawRightSideTarget = rightSideTarget;
        Vector3 leftHandTarget = leftSideTarget;
        Vector3 rightHandTarget = rightSideTarget;
        Vector3 leftFootTarget = leftSideTarget;
        Vector3 rightFootTarget = rightSideTarget;
        OrderHoldTargetsForHands(ref leftHandTarget, ref rightHandTarget, center, side);
        OrderHoldTargetsForFeet(ref leftFootTarget, ref rightFootTarget, center, side);
        LogHoldTargetOrder("Chest Hold", mode, rawLeftSideTarget, rawRightSideTarget, leftHandTarget, rightHandTarget, center, side);
        LogHoldFootTargetOrder("Chest Hold", mode, rawLeftSideTarget, rawRightSideTarget, leftFootTarget, rightFootTarget, center, side);

        int moved = 0;

        if (includeHands)
        {
            bool crossedTargets = mode == "face";
            bool leftTargetRightSide = IsTargetOnPositiveSide(leftHandTarget, center, side);
            bool rightTargetRightSide = IsTargetOnPositiveSide(rightHandTarget, center, side);

            if (leftHandJSON != null && leftHandJSON.val && lHandControl != null)
            {
                Vector3 leftRoot = GetHandRootPosition(false);
                Vector3 leftSideGrabTarget = GetNipplePairSideGrabTarget(leftHandTarget, side, leftTargetRightSide);
                Vector3 target = GetNipplePairHandTarget(leftRoot, leftSideGrabTarget, lHandControl, false);
                LogHoldHandTarget("Chest Hold", mode, false, leftHandTarget, leftSideGrabTarget, target, leftRoot, side, leftTargetRightSide, center, immediate);
                MoveControl(lHandControl, target, Quaternion.identity, false, immediate);
                moved++;
            }

            if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
            {
                Vector3 rightRoot = GetHandRootPosition(true);
                Vector3 rightSideGrabTarget = GetNipplePairSideGrabTarget(rightHandTarget, side, rightTargetRightSide);
                Vector3 target = GetNipplePairHandTarget(rightRoot, rightSideGrabTarget, rHandControl, true);
                LogHoldHandTarget("Chest Hold", mode, true, rightHandTarget, rightSideGrabTarget, target, rightRoot, side, rightTargetRightSide, center, immediate);
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
                MovePairHandControlWithMidpoint(lHandControl, target, center, side, immediate);
                moved++;
            }

            if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
            {
                Vector3 rightRoot = GetHandRootPosition(true);
                bool targetRightSide = IsTargetOnPositiveSide(rightHandTarget, center, side);
                Vector3 sideTarget = GetPairOutsidePoint(rightHandTarget, center, side, PAIR_FINAL_OUTSIDE_OFFSET);
                Vector3 target = GetNipplePairHandTarget(rightRoot, sideTarget, rHandControl, true);
                LogHoldHandTarget(controller, mode, true, rightHandTarget, sideTarget, target, rightRoot, side, targetRightSide, center, immediate);
                MovePairHandControlWithMidpoint(rHandControl, target, center, side, immediate);
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

        Vector3 actorForward = GetSelectedPersonForwardAxis();
        Vector3 targetForward = GetTargetPersonForwardAxis();

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
            // 向かい合い: 見た目上の左右に合わせるためクロス割当。
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
            SuperController.LogMessage("[TargetGrabber] [INTIMATE SCAN] no Target Person");
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

        SuperController.LogMessage("[TargetGrabber] [INTIMATE SCAN] target=" + selectedTargetPerson.uid +
            " / focused=pelvis triggers + _JointAl/Debug");
        SuperController.LogMessage("[TargetGrabber] [INTIMATE SCAN] Gen candidate=" +
            FormatTransformCandidate(gen));
        SuperController.LogMessage("[TargetGrabber] [INTIMATE SCAN] Vagina candidate=" +
            FormatTransformCandidate(vagina));
        SuperController.LogMessage("[TargetGrabber] [INTIMATE SCAN] Deep Vagina candidate=" +
            FormatTransformCandidate(deepVagina));
        SuperController.LogMessage("[TargetGrabber] [INTIMATE SCAN] Deeper Vagina candidate=" +
            FormatTransformCandidate(deeperVagina));
        SuperController.LogMessage("[TargetGrabber] [INTIMATE SCAN] Anus candidate=" +
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

        DebugLog("[HOLD HAND TARGET] controller=" + controller +
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
            " pathEnd=" + FormatVector3(finalTarget));
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
        if (!IsDebugEnabled())
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

        DebugLog("[HAND TARGET] controller=" + (targetPersonPartChooser != null ? targetPersonPartChooser.val : "<null>") +
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
            " pathEnd=" + FormatVector3(finalTarget));
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

    private void MovePairHandControlWithMidpoint(FreeControllerV3 fc, Vector3 finalTarget, Vector3 center, Vector3 side, bool immediate)
    {
        if (fc == null)
            return;

        if (immediate || side.sqrMagnitude < 0.0001f)
        {
            MoveControl(fc, finalTarget, Quaternion.identity, false, immediate);
            return;
        }

        EnsurePositionStateOn(fc);

        Vector3 midTarget = GetPairOutsidePoint(finalTarget, center, side, PAIR_HAND_MID_OUTSIDE_OFFSET);

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

        bool nipplePairMode = IsNipplePairMode();
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
        if (IsHugBodyTarget())
            return ApplyHugBodyDirectionalWristRotation(baseRotation, controlPosition, startPosition, center, pathRightSide, actualRightHand, frontSide);

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

        Vector3 movedPosition = GetControlPosition(handControl);
        Vector3 startPosition;
        if (!grabStartPositions.TryGetValue(handControl, out startPosition))
            startPosition = movedPosition;

        Quaternion rotation = GetPalmOrSoleRotation(movedPosition, startPosition, center, GetHandRotationOffset(), true, pathRightSide, actualRightHand);
        MoveControl(handControl, movedPosition, rotation, true, true);
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

    private void ApplyTemporaryHandRotationOffIfNeeded(bool includeHands)
    {
        if (!includeHands)
            return;

        if (leftHandJSON != null && leftHandJSON.val)
            TemporarilyTurnHandRotationOff(lHandControl, "L");
        if (rightHandJSON != null && rightHandJSON.val)
            TemporarilyTurnHandRotationOff(rHandControl, "R");
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

    private void RestoreTemporaryHandRotationOffStates()
    {
        if (temporaryHandRotationOffStates.Count == 0)
            return;

        List<KeyValuePair<FreeControllerV3, FreeControllerV3.RotationState>> states = temporaryHandRotationOffStates.ToList();
        temporaryHandRotationOffStates.Clear();

        foreach (KeyValuePair<FreeControllerV3, FreeControllerV3.RotationState> item in states)
        {
            if (item.Key == null)
                continue;

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
        hasActiveGrab = false;
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
