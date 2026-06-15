// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// FILE: TargetGrabber.cs
// VAM Target Grabber
//
// 指定Atomを手・足で掴む補助プラグイン
//
// Author : VAMT
// Version: v3.0cq-pair-mid-final-offset
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
    private const string CUSTOM_TARGET_PREFIX = "Custom: ";
    private const float HIP_HOLD_GRAB_WIDTH = 1.50f;
    private const float HIP_HOLD_FINAL_GRAB_WIDTH = 0.13f;
    private const float CROTCH_GRAB_WIDTH = 0.00f;
    private const float CROTCH_FINAL_GRAB_WIDTH = 0.00f;
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
    private JSONStorableString targetControllerFilterJSON;

    private JSONStorableBool leftHandJSON;
    private JSONStorableBool rightHandJSON;
    private JSONStorableBool leftFootJSON;
    private JSONStorableBool rightFootJSON;
    private JSONStorableBool followTargetJSON;
    private JSONStorableBool alignHandPalmJSON;
    private JSONStorableBool alignFootSoleJSON;
    private JSONStorableBool debugLogJSON;
    private JSONStorableBool autoGrabWidthJSON;
    private JSONStorableBool hugModeJSON;

    private JSONStorableFloat grabWidthJSON;
    private JSONStorableFloat grabCloseSpeedJSON;
    private JSONStorableFloat finalGrabWidthJSON;
    private JSONStorableFloat targetZOffsetJSON;
    private JSONStorableFloat autoZOffsetJSON;
    private JSONStorableFloat hugDepthJSON;
    private JSONStorableFloat maxHandReachJSON;
    private JSONStorableFloat maxFootReachJSON;
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

    private Atom selectedPerson;
    private Atom selectedTargetAtom;
    private Atom selectedTargetPerson;

    private FreeControllerV3 lHandControl;
    private FreeControllerV3 rHandControl;
    private FreeControllerV3 lFootControl;
    private FreeControllerV3 rFootControl;
    private FreeControllerV3 lKneeControl;
    private FreeControllerV3 rKneeControl;
    private FreeControllerV3 chestControl;
    private FreeControllerV3 hipControl;

    private bool hasActiveGrab = false;
    private bool suppressApply = false;

    // v1.8: Grab motion state.
    // Move Time Sec = 現在位置からターゲット位置まで到達する秒数。
    private bool activeIncludeHands = true;
    private bool activeIncludeFeet = true;
    private float activeMoveTimeMultiplier = 1.0f;
    private float grabElapsed = 0.0f;
    private float grabStartWidth = 0.0f;
    private float currentGrabWidth = 0.0f;
    private readonly Dictionary<FreeControllerV3, Vector3> grabStartPositions = new Dictionary<FreeControllerV3, Vector3>();
    private readonly Dictionary<FreeControllerV3, Quaternion> grabStartRotations = new Dictionary<FreeControllerV3, Quaternion>();
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
    private float jobOriginalZOffset = 0.0f;
    private const float JOB_DURATION = 0.90f;
    private float lastSideDebugTime = -10.0f;
    private float lastHandRotationDebugTime = -10.0f;
    private bool releaseRestoreIKPending = false;
    private float releaseRestoreIKTime = 0.0f;
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

        // 左側UI: Target / Motion / Advanced。
        // Foot Sole系は左下へ寄せる。
        grabWidthJSON = CreateFloat("Grab Width", 1.60f, 0.0f, 2.00f, false);
        finalGrabWidthJSON = CreateFloat("Final Grab Width", 0.10f, 0.0f, 2.00f, false);
        autoGrabWidthJSON = CreateBool("Auto Grab Width", true, false);
        grabCloseSpeedJSON = CreateFloat("Grab Close Speed", 5.0f, 0.1f, 20.0f, false);
        moveTimeJSON = CreateFloat("Move Time Sec", 0.50f, 0.05f, 10.00f, false);
        hugModeJSON = CreateBool("Hug Mode", false, true);
        hugDepthJSON = CreateFloat("Hug Depth", -1.00f, -1.00f, 1.00f, false);

        targetZOffsetJSON = CreateFloat("Target Z Offset", 0.00f, -1.00f, 1.00f, false);
        autoZOffsetJSON = CreateFloat("Auto Z Offset", 0.00f, -1.00f, 1.00f, false);
        maxHandReachJSON = CreateFloat("Max Hand Reach", 0.70f, 0.10f, 2.00f, false);
        maxFootReachJSON = CreateFloat("Max Foot Reach", 0.80f, 0.10f, 2.00f, false);

        alignHandPalmJSON = CreateBool("Align Hand Palm", true, false);
        handPalmOffsetJSON = CreateFloat("Hand Palm Offset", 0.00f, -0.30f, 0.30f, false);
        handCenterOffsetJSON = CreateFloat("Hand Center Offset", 0.10f, -0.20f, 0.20f, false);
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
        leftHandJSON = CreateBool("Left Hand", true, true);
        rightHandJSON = CreateBool("Right Hand", true, true);

        CreateButton("Grab Foot", true).button.onClick.AddListener(GrabFoot);
        leftFootJSON = CreateBool("Left Foot", true, true);
        rightFootJSON = CreateBool("Right Foot", true, true);

        CreateButton("Grab Selected", true).button.onClick.AddListener(GrabSelected);
        CreateButton("pufupufu", true).button.onClick.AddListener(Pufupufu);
        CreateButton("job", true).button.onClick.AddListener(Job);
        CreateButton("Release", true).button.onClick.AddListener(Release);

        followTargetJSON = CreateBool("Follow Target", false, true);

        statusJSON = new JSONStorableString("Status", "Ready");
        RegisterString(statusJSON);
        UIDynamicTextField statusField = CreateTextField(statusJSON, false);
        if (statusField != null)
            statusField.height = 80.0f;

        RegisterExternalActions();

        RefreshAll();

        DebugLog("ready / v3.0al-root-orientation-lr-stable-filter / stable-lr-paths-filter");
    }

    private void RegisterExternalActions()
    {
        RegisterAction(new JSONStorableAction("Refresh", RefreshAll));
        RegisterAction(new JSONStorableAction("Default", ApplyDefaultSettings));
        RegisterAction(new JSONStorableAction("Grab Hand", GrabHand));
        RegisterAction(new JSONStorableAction("Grab Left Hand", GrabLeftHand));
        RegisterAction(new JSONStorableAction("Grab Right Hand", GrabRightHand));
        RegisterAction(new JSONStorableAction("Grab Foot", GrabFoot));
        RegisterAction(new JSONStorableAction("Grab Left Foot", GrabLeftFoot));
        RegisterAction(new JSONStorableAction("Grab Right Foot", GrabRightFoot));
        RegisterAction(new JSONStorableAction("Grab Selected", GrabSelected));
        RegisterAction(new JSONStorableAction("pufupufu", Pufupufu));
        RegisterAction(new JSONStorableAction("job", Job));
        RegisterAction(new JSONStorableAction("Release", Release));
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

            if (!suppressApply && followTargetJSON != null && followTargetJSON.val && hasActiveGrab)
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
            if (!suppressApply && followTargetJSON != null && followTargetJSON.val && hasActiveGrab)
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
    }

    private void OnTargetTypeChanged(string type)
    {
        UpdateTargetPersonControllerChoices();
        ApplyAutoGrabWidthFromTargetAtom();
        ApplyAutoGrabWidthFromTargetPerson();

        if (!suppressApply && followTargetJSON != null && followTargetJSON.val && hasActiveGrab)
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

        if (!suppressApply && followTargetJSON != null && followTargetJSON.val && hasActiveGrab)
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

        if (!suppressApply && followTargetJSON != null && followTargetJSON.val && hasActiveGrab)
            ApplyGrab(false);
    }

    private void OnTargetPersonPartChanged(string part)
    {
        DebugLog("[TARGET CONTROLLER] raw=" + (part ?? "<null>") +
            " key=" + NormalizeControllerKey(part) +
            " nipple=" + Bool01(IsNipplePairControlName(part)));

        ApplyAutoGrabWidthFromTargetPerson();

        if (!suppressApply && followTargetJSON != null && followTargetJSON.val && hasActiveGrab)
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
        AddFixedTargetControllerChoice(choices, TC_CROTCH);
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

        string next = choices.Contains(current) && current != NONE
            ? current
            : FirstExistingChoice(choices, TC_HUG_BODY, TC_CHEST_HOLD, TC_HIP_HOLD, TC_HAND, TC_FOOT, TC_KNEE, TC_CROTCH, TC_HEAD, TC_HEAD_TOP, TC_MOUTH, TC_NECK, TC_ABDOMEN, TC_HIP, TC_L_NIPPLE, TC_R_NIPPLE) ?? NONE;

        targetPersonPartChooser.val = next;
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
        if (choice == TC_CROTCH)
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
        float grabWidth = hipHold
            ? HIP_HOLD_GRAB_WIDTH
            : (choice == TC_CROTCH
                ? CROTCH_GRAB_WIDTH
                : (choice == TC_NECK ? NECK_GRAB_WIDTH : (IsWidePersonController(c) ? 2.00f : 0.40f)));

        suppressApply = true;
        try
        {
            if (grabWidthJSON != null)
                grabWidthJSON.val = grabWidth;

            float finalWidth = hipHold
                ? HIP_HOLD_FINAL_GRAB_WIDTH
                : (targetPersonPartChooser != null && targetPersonPartChooser.val == TC_HEAD
                    ? HEAD_FINAL_GRAB_WIDTH
                    : (choice == TC_CROTCH
                        ? CROTCH_FINAL_GRAB_WIDTH
                        : (IsNipplePairMode() ? 0.10f : 0.00f)));
            if (finalGrabWidthJSON != null)
                finalGrabWidthJSON.val = finalWidth;

            if (autoZOffsetJSON != null)
                autoZOffsetJSON.val = 0.00f;
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
            if (handPalmOffsetJSON != null) handPalmOffsetJSON.val = 0.00f;
            if (handCenterOffsetJSON != null) handCenterOffsetJSON.val = 0.10f;
            if (footSoleOffsetJSON != null) footSoleOffsetJSON.val = 0.08f;
            if (footArcWidthJSON != null) footArcWidthJSON.val = 0.30f;
            if (footArcDropJSON != null) footArcDropJSON.val = 0.10f;
            if (kneeWidthMultiplierJSON != null) kneeWidthMultiplierJSON.val = 1.50f;
            if (moveTimeJSON != null) moveTimeJSON.val = 0.50f;
            if (hugModeJSON != null) hugModeJSON.val = false;
            if (hugDepthJSON != null) hugDepthJSON.val = 0.30f;
            if (autoGrabWidthJSON != null) autoGrabWidthJSON.val = true;
            if (alignFootSoleJSON != null) alignFootSoleJSON.val = false;
            if (leftFootJSON != null) leftFootJSON.val = true;
            if (rightFootJSON != null) rightFootJSON.val = true;
        }
        finally
        {
            suppressApply = false;
        }

        if (followTargetJSON != null && followTargetJSON.val && hasActiveGrab)
            ApplyGrab(false);

        SetStatus("Default applied");
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
        lFootControl = null;
        rFootControl = null;
        lKneeControl = null;
        rKneeControl = null;
        chestControl = null;
        hipControl = null;

        if (selectedPerson == null)
            return;

        lHandControl = GetControl("lHandControl");
        rHandControl = GetControl("rHandControl");
        lFootControl = GetControl("lFootControl");
        rFootControl = GetControl("rFootControl");
        lKneeControl = GetControl("lKneeControl");
        rKneeControl = GetControl("rKneeControl");
        chestControl = GetControl("chestControl");
        hipControl = GetControl("hipControl");

        if (!hasActiveGrab)
        {
            DebugLog("[RESOLVE] lHand=" + Bool01(lHandControl != null) +
                " rHand=" + Bool01(rHandControl != null) +
                " lFoot=" + Bool01(lFootControl != null) +
                " rFoot=" + Bool01(rFootControl != null) +
                " lKnee=" + Bool01(lKneeControl != null) +
                " rKnee=" + Bool01(rKneeControl != null) +
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

        if (!hasActiveGrab)
        {
            if (jobRunning)
                ApplyGrab(true, activeIncludeHands, activeIncludeFeet);
            UpdatePufupufuAnimation();
            return;
        }

        bool follow = followTargetJSON != null && followTargetJSON.val;
        bool moving = GetMoveTLinear() < 1.0f;

        if (follow || moving || jobRunning)
        {
            ApplyGrab(false, activeIncludeHands, activeIncludeFeet);
            UpdatePufupufuAnimation();
            return;
        }

        if (UpdatePufupufuAnimation())
            return;

        // Follow OFFなら到達後に更新だけ止める。ControlはONのまま保持。
        hasActiveGrab = false;
    }

    private void GrabHand()
    {
        StartTimedGrab(true, false);
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
        // v3.0aj:
        // 実行前にControl参照を更新する。既存job動作自体は旧版のまま。
        ResolveControls();

        jobOriginalZOffset = targetZOffsetJSON != null ? targetZOffsetJSON.val : 0.0f;
        jobElapsed = 0.0f;
        jobActive = true;
        hasActiveGrab = true;
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

    private void StartTimedGrab(bool includeHands, bool includeFeet)
    {
        ResolveControls();

        activeIncludeHands = includeHands;
        activeIncludeFeet = includeFeet;
        activeMoveTimeMultiplier = includeFeet && !includeHands ? 2.0f : 1.0f;
        releaseRestoreIKPending = false;
        releaseRestorePositionControls.Clear();
        releaseRestoreRotationControls.Clear();

        grabElapsed = 0.0f;
        grabStartWidth = IsHipHoldMode()
            ? Mathf.Max(GetFinalGrabWidth(), HIP_HOLD_GRAB_WIDTH)
            : (grabWidthJSON != null ? Mathf.Max(GetFinalGrabWidth(), grabWidthJSON.val) : Mathf.Max(0.10f, GetFinalGrabWidth()));
        currentGrabWidth = grabStartWidth;

        grabStartPositions.Clear();
        grabStartRotations.Clear();
        positionStateOnControls.Clear();
        rotationStateOnControls.Clear();

        CaptureControlStart(lHandControl);
        CaptureControlStart(rHandControl);
        CaptureControlStart(lFootControl);
        CaptureControlStart(rFootControl);
        CaptureControlStart(lKneeControl);
        CaptureControlStart(rKneeControl);

        hasActiveGrab = true;

        string controllerDebug = GetTargetControllerNameForDebug();
        string controllerActual = GetTargetControllerActualName(targetPersonPartChooser != null ? targetPersonPartChooser.val : NONE);
        DebugLog("[GRAB START] targetType=" + (targetTypeChooser != null ? targetTypeChooser.val : "<null>") +
            " controller=" + controllerDebug +
            " key=" + NormalizeControllerKey(controllerActual) +
            " nipple=" + Bool01(IsNipplePairControlName(controllerActual)));

        ApplyGrab(false, activeIncludeHands, activeIncludeFeet);
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
        float wave01 = 0.5f + 0.5f * Mathf.Sin(t * Mathf.PI * 6.0f - Mathf.PI * 0.5f);
        float z = Mathf.Lerp(0.03f, 0.07f, wave01);

        bool oldSuppress = suppressApply;
        suppressApply = true;
        try
        {
            if (targetZOffsetJSON != null)
                targetZOffsetJSON.val = z;
        }
        finally
        {
            suppressApply = oldSuppress;
        }

        if (t >= 1.0f)
        {
            jobActive = false;
            oldSuppress = suppressApply;
            suppressApply = true;
            try
            {
                if (targetZOffsetJSON != null)
                    targetZOffsetJSON.val = jobOriginalZOffset;
            }
            finally
            {
                suppressApply = oldSuppress;
            }
        }

        return true;
    }

    private void ApplyGrab(bool immediate)
    {
        ApplyGrab(immediate, true, true);
    }

    private void ApplyGrab(bool immediate, bool includeHands, bool includeFeet)
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
        Vector3 footSide = GetFootSideAxis(baseSide);
        LogSideDebug(center, handSide, footSide);
        bool swapSidePaths = ShouldSwapSidePaths(center);
        LogHandRotationDebug(swapSidePaths, center);
        int moved = 0;

        if (nipplePairMode)
        {
            ApplyNipplePairGrab(immediate, includeHands, includeFeet, center, handCenter, footCenter, handSide);
            return;
        }

        if (hipHoldMode)
        {
            ApplyHipHoldGrab(immediate, includeHands, includeFeet, center, handCenter, footCenter, handSide);
            return;
        }

        if (targetPairMode)
        {
            ApplyTargetPairGrab(immediate, includeHands, includeFeet, center, handCenter, footCenter, handSide);
            return;
        }

        if (includeHands)
        {
            if (leftHandJSON != null && leftHandJSON.val && lHandControl != null)
            {
                // v3.0al:
                // 左右の行き先は正面/背面のワールド位置で反転しない。
                // 正面/背面判定は回転やHug方向にだけ使い、手の実Control割当は固定する。
                bool pathRightSide = !swapSidePaths;
                Vector3 target = GetReachLimitedPosition(GetHandRootPosition(pathRightSide), handCenter + GetSideOffset(pathRightSide, handSide, GetGrabWidth()), GetMaxHandReach(), GetHandPalmOffset(), lHandControl, true, pathRightSide);
                // v3.0ag:
                // 回転の正面/背面判定は Hug で動く handCenter ではなく、元のTarget centerで固定する。
                // Hug中にhandCenterが奥へ送られても、正面右手の当たり回転が背面扱いに化けないようにする。
                Quaternion rotation = GetPalmOrSoleRotation(target, center, GetHandRotationOffset(), true, pathRightSide, false);
                MoveControl(lHandControl, target, rotation, ShouldAlignHandPalm(), immediate);
                moved++;
            }

            if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
            {
                bool pathRightSide = swapSidePaths;
                Vector3 target = GetReachLimitedPosition(GetHandRootPosition(pathRightSide), handCenter + GetSideOffset(pathRightSide, handSide, GetGrabWidth()), GetMaxHandReach(), GetHandPalmOffset(), rHandControl, true, pathRightSide);
                // v3.0ag:
                // 右手回転の正面/背面判定も元のTarget centerで固定する。
                // 正面右手はv3.0ab/afの当たりを維持し、背面右手だけ別プリセットへ切り替えられるようにする。
                Quaternion rotation = GetPalmOrSoleRotation(target, center, GetHandRotationOffset(), true, pathRightSide, true);
                MoveControl(rHandControl, target, rotation, ShouldAlignHandPalm(), immediate);
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
                " / follow=" + (followTargetJSON != null && followTargetJSON.val ? "ON" : "OFF") +
                " / time=" + GetMoveTLinear().ToString("F2", CultureInfo.InvariantCulture) +
                " / width=" + currentGrabWidth.ToString("F3", CultureInfo.InvariantCulture) +
                " / finalWidth=" + GetFinalGrabWidth().ToString("F3", CultureInfo.InvariantCulture) +
                " / palmOffset=" + GetHandPalmOffset().ToString("F3", CultureInfo.InvariantCulture) +
                " / handCenter=" + GetHandCenterOffset().ToString("F3", CultureInfo.InvariantCulture) +
                " / handPalm=" + (ShouldAlignHandPalm() ? "ON" : "OFF") +
                " / hug=" + (IsHugMode() ? "ON" : "OFF") +
                " / kneeMul=" + GetKneeWidthMultiplier().ToString("F2", CultureInfo.InvariantCulture) +
                " / footArc=" + GetFootArcWidth().ToString("F2", CultureInfo.InvariantCulture) +
                " / footDrop=" + GetFootArcDrop().ToString("F2", CultureInfo.InvariantCulture) +
                " / footSole=" + (ShouldAlignFootSole() ? "ON" : "OFF"));
        }
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
                " / follow=" + (followTargetJSON != null && followTargetJSON.val ? "ON" : "OFF") +
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

        Vector3 footSide = GetFootSideAxis(side);
        Vector3 rawLeftSideTarget = leftSideTarget;
        Vector3 rawRightSideTarget = rightSideTarget;
        Vector3 leftHandTarget = leftSideTarget;
        Vector3 rightHandTarget = rightSideTarget;
        Vector3 leftFootTarget = leftSideTarget;
        Vector3 rightFootTarget = rightSideTarget;
        OrderHoldTargetsForHands(ref leftHandTarget, ref rightHandTarget, center, side);
        OrderHoldTargetsForFeet(ref leftFootTarget, ref rightFootTarget, center, side);
        LogHoldTargetOrder("Hip Hold", mode, rawLeftSideTarget, rawRightSideTarget, leftHandTarget, rightHandTarget, center, side);
        LogHoldFootTargetOrder("Hip Hold", mode, rawLeftSideTarget, rawRightSideTarget, leftFootTarget, rightFootTarget, center, side);
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
                LogHoldHandTarget("Hip Hold", mode, false, leftHandTarget, leftSideGrabTarget, target, leftRoot, side, leftTargetRightSide, center, immediate);
                MoveControl(lHandControl, target, Quaternion.identity, false, immediate);
                moved++;
            }

            if (rightHandJSON != null && rightHandJSON.val && rHandControl != null)
            {
                Vector3 rightRoot = GetHandRootPosition(true);
                Vector3 rightSideGrabTarget = GetNipplePairSideGrabTarget(rightHandTarget, side, rightTargetRightSide);
                Vector3 target = GetNipplePairHandTarget(rightRoot, rightSideGrabTarget, rHandControl, true);
                LogHoldHandTarget("Hip Hold", mode, true, rightHandTarget, rightSideGrabTarget, target, rightRoot, side, rightTargetRightSide, center, immediate);
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
                " / follow=" + (followTargetJSON != null && followTargetJSON.val ? "ON" : "OFF") +
                " / time=" + GetMoveTLinear().ToString("F2", CultureInfo.InvariantCulture));
        }
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
                " / follow=" + (followTargetJSON != null && followTargetJSON.val ? "ON" : "OFF") +
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

        if (choice == TC_CROTCH)
        {
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
                return ApplyTargetZOffset(ApplyChestBackGrabOffset(partCenter), partRot);
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
        return choice == TC_HEAD || choice == TC_HEAD_TOP || choice == TC_MOUTH;
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
            " follow=" + Bool01(followTargetJSON != null && followTargetJSON.val) +
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

    private string FormatVector3(Vector3 value)
    {
        return "(" +
            value.x.ToString("F3", CultureInfo.InvariantCulture) + "," +
            value.y.ToString("F3", CultureInfo.InvariantCulture) + "," +
            value.z.ToString("F3", CultureInfo.InvariantCulture) + ")";
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

    private float GetFinalGrabWidth()
    {
        if (IsHugMode())
            return 0.0f;

        // UI上の Final Grab Width は「左右の実距離」として扱う。
        // 配置計算は center ± width の半幅方式なので、内部値は半分にする。
        float width = finalGrabWidthJSON != null
            ? Mathf.Max(0.0f, finalGrabWidthJSON.val)
            : 0.10f;

        return width * 0.5f;
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

        // v3.0aa:
        // Hug Mode の一度奥へ送る方向は、通常Grab用の GetTargetForwardAxis() から分離する。
        // 通常Grab用の軸は sameFacing 補正で反転するため、背面側では Hug の奥方向まで逆になりやすい。
        // Person相手では「掴む側から見て奥」へ送るため、Target Person root の forward と
        // 自分が正面側/背面側のどちらにいるかだけで決める。
        Vector3 deepCenter = center + GetHugForwardAxis(center) * Mathf.Abs(GetHugDepth());

        // 前半は対象物の奥を狙い、後半で通常中心へ戻す。
        if (t < 0.50f)
            return deepCenter;

        float u = Mathf.Clamp01((t - 0.50f) / 0.50f);
        return Vector3.Lerp(deepCenter, center, u);
    }

    private Vector3 GetHugForwardAxis(Vector3 center)
    {
        if (IsTargetPersonMode() && selectedTargetPerson != null)
        {
            Vector3 targetForward = GetTargetPersonForwardAxis();
            if (targetForward.sqrMagnitude > 0.0001f)
            {
                targetForward.Normalize();

                // 正面側から掴むなら、奥はTarget Personの背面側。
                // 背面側から掴むなら、奥はTarget Personの正面側。
                return IsGrabberInFrontOfTargetPerson(center) ? -targetForward : targetForward;
            }
        }

        Vector3 axis = GetTargetForwardAxis();
        if (axis.sqrMagnitude < 0.0001f)
            axis = Vector3.forward;

        return axis.normalized;
    }

    private void AutoCloseGrabWidth()
    {
        // Grab Width はUI値を維持し、内部の currentGrabWidth だけを閉じる。
        // Hug Mode時は Final Grab Width を強制0にする。
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

    private float GetHandPalmOffset()
    {
        return handPalmOffsetJSON != null ? handPalmOffsetJSON.val : 0.08f;
    }

    private float GetHandCenterOffset()
    {
        return handCenterOffsetJSON != null ? handCenterOffsetJSON.val : 0.04f;
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

    private bool ShouldAlignFootSole()
    {
        return alignFootSoleJSON != null && alignFootSoleJSON.val;
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

    private Quaternion GetFixedHandRotation(Vector3 eulerOffset, bool pathRightSide, bool actualRightHand, bool frontSide)
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
        Vector3 baseEuler;
        if (IsTargetPersonMode() && actualRightHand)
            baseEuler = frontSide ? leftPreset : rightPreset;
        else
            baseEuler = pathRightSide == actualRightHand ? rightPreset : leftPreset;

        Vector3 add = eulerOffset;

        return Quaternion.Euler(
            baseEuler.x + add.x,
            baseEuler.y + add.y,
            baseEuler.z + add.z
        );
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
        // v1.4:
        // 手のひら合わせは LookRotation でターゲット中心へ向けると、手首が内側へ折れやすい。
        // そのため手だけはVaM上で確認した左右の握り角度をベースにする。
        // Hand Palm Add Rot X/Y/Z は、この固定角度への微調整として使う。
        if (hand)
        {
            bool frontSide = IsTargetPersonMode() && selectedTargetPerson != null
                ? IsGrabberInFrontOfTargetPerson(center)
                : false;
            return GetFixedHandRotation(eulerOffset, rightSide, actualRightHand, frontSide);
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
        grabElapsed = 0.0f;
        activeMoveTimeMultiplier = 1.0f;
        pufupufuActive = false;

        if (jobActive && targetZOffsetJSON != null)
            targetZOffsetJSON.val = jobOriginalZOffset;

        jobActive = false;

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

        releaseRestoreIKPending = true;
        releaseRestoreIKTime = Time.time + RELEASE_RESTORE_IK_DELAY;

        SetStatus("Released");
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
