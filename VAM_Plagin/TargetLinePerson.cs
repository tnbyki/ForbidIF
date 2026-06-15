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

public class TargetLinePerson : MVRScript
{
    JSONStorableStringChooser targetPersonChooser;
    JSONStorableStringChooser targetControllerChooser;

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
    JSONStorableBool kneeIkOffOnApply;
    JSONStorableBool sitGroundAuto;
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

    UIDynamicSlider orbitAngleSlider;
    UIDynamicSlider distanceSlider;
    UIDynamicSlider hipYOffsetSlider;
    UIDynamicSlider yellowButtGuideScaleSlider;

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

    Vector3 capturedOrigin;
    Vector3 capturedDir;
    Vector3 capturedLineDir;

    bool captured;
    bool isCapturing;
    bool isAvoidMoving;
    float appliedHipYOffset;
    float appliedUpperTiltAngle;
    bool hasAppliedUpperTilt;
    bool rideLieActive;

    Coroutine avoidCaptureRoutine;
    Coroutine delayedLineLockRoutine;

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

    GameObject forwardLineObj;
    GameObject moveLineObj;
    GameObject penisPathLineObj;
    GameObject bendMarkerLineObj;

    LineRenderer forwardLine;
    LineRenderer moveLine;
    LineRenderer penisPathLine;
    LineRenderer bendMarkerLine;

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

        CreateButton("Auto Docking", true).button.onClick.AddListener(delegate
        {
            CaptureHorizontalCurrentSide(false);
        });

        CreateButton("Smart Docking", true).button.onClick.AddListener(delegate
        {
            CaptureHorizontalBaseline(false);
        });

        CreateButton("Reverse Smart Docking", true).button.onClick.AddListener(delegate
        {
            CaptureHorizontalBaseline(true);
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
                "mouth"
            },
            "genital",
            "Target"
        );
        targetControllerChooser.setCallbackFunction = OnTargetControllerChanged;
        RegisterStringChooser(targetControllerChooser);
        CreateScrollablePopup(targetControllerChooser);

        CreateButton("Refresh Person List").button.onClick.AddListener(delegate
        {
            RefreshPersonList();
            targetPersonChooser.choices = personChoices;

            if (personChoices.Count > 0 && string.IsNullOrEmpty(targetPersonChooser.val))
            {
                targetPersonChooser.val = personChoices[0];
            }
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

        distance = new JSONStorableFloat(
            "Distance",
            1.0f,
            -1.5f,
            3.0f
        );
        distance.setCallbackFunction = OnPlacementSliderChanged;
        RegisterFloat(distance);
        distanceSlider = CreateSlider(distance, true);

        yellowButtGuideScale = new JSONStorableFloat(
            "Yellow Butt Guide Scale",
            1.0f,
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

        // Avoid 系スライダーは Avoid Target On Capture の下、左側へまとめる。
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

        // Avoid時の2段階移動で、横へ逃げる角度。
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
            ApplyUpperBodyDirection();
        });

        CreateButton("Load Pose USER Defaults", true).button.onClick.AddListener(delegate
        {
            LoadPoseUserDefaults();
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

        sitGroundAuto = new JSONStorableBool(
            "Sit Ground Auto",
            false
        );
        RegisterBool(sitGroundAuto);
        // hidden UI: CreateToggle(sitGroundAuto, true);

        sitGroundYThreshold = new JSONStorableFloat(
            "Sit Ground Y",
            0.35f,
            0.00f,
            1.00f
        );
        RegisterFloat(sitGroundYThreshold);
        // hidden UI: CreateSlider(sitGroundYThreshold, true);

        showLines = new JSONStorableBool(
            "Show Lines",
            true
        );
        RegisterBool(showLines);
        CreateToggle(showLines);

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

        kneeIkOffOnApply = new JSONStorableBool(
            "Knee IK OFF on Apply",
            true
        );
        RegisterBool(kneeIkOffOnApply);
        CreateToggle(kneeIkOffOnApply);

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

        CreateButton("Apply Once", true).button.onClick.AddListener(delegate
        {
            ApplyPlacementWithLimbRestore();
        });

        CreateButton("Cuddle Up", true).button.onClick.AddListener(delegate
        {
            ApplyCuddleMode(true);
        });

        CreateButton("Cuddle Down", true).button.onClick.AddListener(delegate
        {
            ApplyCuddleMode(false);
        });


        CreateButton("Copy Body Direction", true).button.onClick.AddListener(delegate
        {
            CopyBodyDirectionFromTarget();
        });

        CreateButton("Mirror Pose", true).button.onClick.AddListener(delegate
        {
            MirrorPoseLeftRight();
        });

        CreateButton("Sit Ground Pose", true).button.onClick.AddListener(delegate
        {
            ApplySitGroundPresetPose();
        });

        CreateButton("Lie On Back", true).button.onClick.AddListener(delegate
        {
            ApplyLieOnBackPresetPose();
        });

        CreateButton("Lie On Front", true).button.onClick.AddListener(delegate
        {
            ApplyLieOnFrontPresetPose();
        });

        CreateDebugLines();

        SetPlacementControlsInteractable(false);

        SuperController.LogMessage("[TargetLinePerson] Ready / v060 docking buttons target reset / public UI names / capture callback guard / no P follow in lie / auto lie visible on / transparent lines / locked guide scale / delayed guide refresh");
    }

    void Update()
    {
        if (!captured)
        {
            CancelDelayedGuideRefresh("not captured");
            UpdateDebugLines(false);
            return;
        }

        if (IsApplyOnSliderChangeOnly())
        {
            if (followTarget == null || !followTarget.val || isAvoidMoving)
            {
                ResetUpperBodyLowerIfApplied("follow off or avoid");
                ResetPAngleAtYellowP3IfApplied("follow off or avoid");
                CancelDelayedGuideRefresh("follow off or avoid");
            }
            else
            {
                ProcessDelayedGuideRefresh();
            }

            UpdateDebugLines(showLines.val);
            return;
        }

        CancelDelayedGuideRefresh("continuous apply mode");

        if (followTarget.val && !isAvoidMoving)
        {
            ApplyPlacement();
            ApplyUpperBodyLowerByYellowPathIfNeeded("update");
            ApplyPAngleAtYellowP3IfNeeded("update");
        }
        else
        {
            ResetUpperBodyLowerIfApplied("follow off or avoid");
            ResetPAngleAtYellowP3IfApplied("follow off or avoid");
        }

        UpdateDebugLines(showLines.val);
    }

    bool IsApplyOnSliderChangeOnly()
    {
        return applyOnSliderChangeOnly != null && applyOnSliderChangeOnly.val;
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

        if (followTarget == null || !followTarget.val || isAvoidMoving)
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
            ApplyUpperBodyLowerByYellowPathIfNeeded("delayed " + reason);
            ApplyPAngleAtYellowP3IfNeeded("delayed " + reason);
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
        foreach (Atom a in SuperController.singleton.GetAtoms())
        {
            if (a != null && a.uid == uid)
            {
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

        foreach (FreeControllerV3 fc in atom.freeControllers)
        {
            if (fc == null || fc.name == null) continue;

            if (fc.name.ToLower().Contains(key))
            {
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

        foreach (FreeControllerV3 fc in atom.freeControllers)
        {
            if (fc != null && fc.name == controllerName)
            {
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
        if (atom == null)
        {
            return null;
        }

        foreach (Transform t in atom.GetComponentsInChildren<Transform>(true))
        {
            if (t != null && t.name == childName)
            {
                return t;
            }
        }

        return null;
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
        ResetCaptureStateForTargetChange("target changed to " + value);
    }

    void OnTargetPersonChanged(string value)
    {
        ResetCaptureStateForTargetChange("person changed to " + value);
    }

    void ResetCaptureStateForTargetChange(string reason)
    {
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
        pAngleAtYellowP3Applied = false;
        pDynamicBaseYApplied = false;
        lastDynamicPBaseOffset = Vector3.zero;
        ClearTipYellowParallelLock();
        CancelDelayedGuideRefresh(reason);

        if (distance != null) distance.valNoCallback = 1.0f;
        if (orbitAngle != null) orbitAngle.valNoCallback = 0.0f;
        if (hipYOffset != null) hipYOffset.valNoCallback = 0.0f;

        SetPlacementControlsInteractable(false);
        UpdateDebugLines(false);

        SuperController.LogMessage("[TargetLinePerson] Target reset / reason=" + reason + " / press Auto Docking again");
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
        SetOwnKneeAndFootIkOffIfNeeded();

        CaptureHorizontal(hardMode);

        if (avoidCaptureRoutine == null)
        {
            StartCoroutine(RestoreLimbStateDelayed());
        }
    }

    void CaptureHorizontalWithLimbRestoreCurrentSide(bool reverseCurrentSide)
    {
        CaptureLimbState();
        SetOwnKneeAndFootIkOffIfNeeded();

        CaptureHorizontalCurrentSideInternal(reverseCurrentSide);

        if (avoidCaptureRoutine == null)
        {
            StartCoroutine(RestoreLimbStateDelayed());
        }
    }

    void ApplyPlacementWithLimbRestore()
    {
        CaptureLimbState();
        SetOwnKneeAndFootIkOffIfNeeded();

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

    void SetOwnKneeAndFootIkOffIfNeeded()
    {
        if (kneeIkOffOnApply == null || !kneeIkOffOnApply.val)
        {
            return;
        }

        SetOwnKneeAndFootIkOff();
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

        captured = true;
        distance.valNoCallback = 1.0f;
        orbitAngle.valNoCallback = 0.0f;

UpdateLine();
bool reverseDirection = hardMode;
if (chooseCurrentSide)
{
    reverseDirection = ShouldReverseForCurrentSideDocking(reverseCurrentSide);
}

if (reverseDirection)
{
    ReverseCapturedDirection();
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

// Restore the original P_YELLOW_PATH height flow:
//   1. Match the Person root height roughly to the target root.
//   2. Capture Hip Y Offset as ownHip.y - ownPBase.y.
//   3. Move hip-relative body controllers so own P Base Y matches capturedOrigin.y.
//
// This is intentionally done BEFORE ApplyPlacement(), because ApplyPlacement() only moves
// the root in X/Z.  If this step is skipped, the body can be placed horizontally while
// the own P Base is left at the wrong height.
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

        if (avoidTargetOnCapture != null && avoidTargetOnCapture.val && ShouldAvoidTargetOnCapture())
        {
            avoidCaptureRoutine = StartCoroutine(AvoidCaptureMoveRoutine());
        }
        else
        {
               ApplyPlacement(true);
               ScheduleDelayedLineLock("capture-after-placement");
        }

        isCapturing = false;
        SetPlacementControlsInteractable(true);
        SuperController.LogMessage("[TargetLinePerson] Captured orbit / PBaseY aligned by original hip-offset flow");
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

        float deltaY = capturedOrigin.y + GetHipYOffset() - ownHip.transform.position.y;
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
            SetOwnKneeAndFootIkOff();
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

    void UpdateLine()
    {
        FreeControllerV3 target = GetTargetController();
        Atom targetAtom = FindAtom(targetPersonChooser.val);

        if (target == null)
        {
            return;
        }

        Transform genitalLine = null;

        if (targetControllerChooser != null && targetControllerChooser.val == "genital")
        {
            genitalLine = FindChildTransform(targetAtom, "LabiaTrigger");
        }

        Transform mouthLine = null;

        if (targetControllerChooser != null && targetControllerChooser.val == "mouth")
        {
            mouthLine = FindChildTransform(targetAtom, "mouthPhysicsMeshPredictionPoint");
        }

        Transform specialLine = genitalLine != null ? genitalLine : mouthLine;
        Vector3 lineDir = GetTargetLineDirection(target, genitalLine, mouthLine);
        Vector3 forward = lineDir;
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
    }

    Vector3 GetTargetLineDirection(FreeControllerV3 target, Transform genitalLine, Transform mouthLine)
    {
        if (genitalLine != null)
        {
            return -genitalLine.up;
        }

        if (mouthLine != null)
        {
            return mouthLine.forward;
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

        if (capturedLineDir.sqrMagnitude >= 0.0001f)
        {
            capturedLineDir = -capturedLineDir.normalized;
        }

        SuperController.LogMessage("[TargetLinePerson] Reverse Smart Docking: captured direction reversed.");
    }

    bool ShouldReverseForCurrentSideDocking(bool reverseCurrentSide)
    {
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

        float sideDot = Vector3.Dot(toOwn.normalized, flatDir.normalized);
        bool normalAlreadyOnCurrentSide = sideDot >= 0f;
        bool reverseNeeded = !normalAlreadyOnCurrentSide;

        if (reverseCurrentSide)
        {
            reverseNeeded = !reverseNeeded;
        }

        SuperController.LogMessage(
            "[TargetLinePerson] Auto Docking side selected" +
            " / sideDot=" + sideDot.ToString("F3") +
            " / reverse=" + reverseNeeded +
            " / reverseCurrentSide=" + reverseCurrentSide
        );

        return reverseNeeded;
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

        SuperController.LogMessage("[TargetLinePerson] Upper Body Lower state captured / reason=" + reason + " / controllers=" + upperBodyLowerBasePositionStates.Count + " / refDistance=" + upperBodyLowerReferenceDistance.ToString("F3") + " / mode=delta-no-pose-reapply");
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
        SuperController.LogMessage("[TargetLinePerson] Upper Body Lower reset / reason=" + reason + " / delta undone / positionState restored / no pose reapply");
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

        float baseYLift = GetDynamicPBaseYLiftFromYellowProgress(progress);
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

        bool shouldLog = !pAngleAtYellowP3Applied;
        if (shouldLog)
        {
            SuperController.LogMessage(
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
        lastPAngleDebugLogTime = -999f;
        SuperController.LogMessage("[TargetLinePerson] P yellow guide three-angle shape at P2 reset / reason=" + reason + " / base state restored / mid+tip state restored");
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
        if (sitGroundAuto == null || !sitGroundAuto.val)
        {
            return;
        }

        if (!IsSitGroundYReached())
        {
            return;
        }

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

        SuperController.LogMessage("[TargetLinePerson] Sit Ground pose applied.");
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

    // Ride成立時は genital の向きを使わない。
    // 現在の「相手hip -> 自分hip」の水平方向を配置方向として使う。
    OverrideCapturedDirectionForRidePose();

    // Ride成立時は前後位置に関係なく Lie On Back 固定。
    // RootはCapture後の配置処理で相手方向へ向くため、Front/Back分岐はしない。
    ApplyLieOnBackPresetPose();
    rideLieActive = true;
    SuperController.LogMessage("[TargetLinePerson] Auto Lie On Ride Pose applied: Lie On Back fixed / genital dir ignored.");
    return true;
}

void ReleaseRideLieIfNeeded()
{
    if (!rideLieActive)
    {
        return;
    }

    rideLieActive = false;

    // Rideで男性側をLie On Backにした後、次のCaptureでRide条件が外れたら
    // Upper Body Direction共通処理だけ呼ぶ。全身初期化はしない。
    ApplyUpperBodyDirection();
    SuperController.LogMessage("[TargetLinePerson] Ride Lie released: Upper Body Direction + head aligned applied.");
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

    // 1. 上半身がほぼ垂直
    // 1.0に近いほど垂直。0.0に近いほど水平。
    float upperVerticalDot = Mathf.Abs(Vector3.Dot(upper.normalized, Vector3.up));

    // 2. hipが低い
    float hipY = hipPos.y;

    // 3. hipが膝ぐらいの高さ
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

        SuperController.LogMessage("[TargetLinePerson] Lie On Back preset pose applied.");
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

        SuperController.LogMessage("[TargetLinePerson] Lie On Front captured preset pose applied.");
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

        // 左右ミラー用。root.forward だと前後反転になって180度回転っぽくなるため、
        // root.right を鏡面法線にする。
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

        SuperController.LogMessage("[TargetLinePerson] Mirror Pose applied.");
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

        // hip -> chest を背骨方向として使う。
        Vector3 up = ownChest.transform.position - ownHip.transform.position;

        if (up.sqrMagnitude < 0.0001f)
        {
            up = Vector3.up;
        }

        up.Normalize();

        // 体の正面方向を使う。胸やRootは回さず、headControlだけを整える。
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
            SuperController.LogMessage("[TargetLinePerson] LOAD VaM USER DEF: executed.");
            return;
        }

        SuperController.LogMessage("[TargetLinePerson] LOAD VaM USER DEF: PosePresets action not found.");
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
                SuperController.LogMessage("[TargetLinePerson] LOAD VaM USER DEF: pose action=" + storableId + " / " + actionNames[i]);
                return true;
            }
        }

        return false;
    }

    void SetOwnKneeIkOffIfNeeded()
    {
        if (!kneeIkOffOnApply.val)
        {
            return;
        }

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

        forwardLine = forwardLineObj.AddComponent<LineRenderer>();
        moveLine = moveLineObj.AddComponent<LineRenderer>();
        penisPathLine = penisPathLineObj.AddComponent<LineRenderer>();
        bendMarkerLine = bendMarkerLineObj.AddComponent<LineRenderer>();

        // Original debug lines restored.
        SetupLine(forwardLine, Color.red);
        SetupLine(moveLine, Color.green);

        // Extra debug lines: yellow shows the intended P path, purple marks the bend point.
        SetupLine(penisPathLine, Color.yellow);
        SetupLine(bendMarkerLine, new Color(1f, 0f, 1f, 1f));

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

    void UpdateDebugLines(bool visible)
    {
        // Show Lines is visual only.  Yellow Guide itself is still built and used
        // internally even when the yellow LineRenderer is hidden.
        if (!captured)
        {
            if (forwardLine != null) forwardLine.enabled = false;
            if (moveLine != null) moveLine.enabled = false;
            if (penisPathLine != null) penisPathLine.enabled = false;
            if (bendMarkerLine != null) bendMarkerLine.enabled = false;
            return;
        }

        if (!hasYellowPPath && !isAvoidMoving && !pYellowCapturePending)
        {
            BuildCapturedYellowPPath();
        }

        bool draw = visible && captured;
        if (forwardLine != null) forwardLine.enabled = draw;
        if (moveLine != null) moveLine.enabled = draw;
        if (penisPathLine != null) penisPathLine.enabled = draw && hasYellowPPath;
        if (bendMarkerLine != null) bendMarkerLine.enabled = draw && hasYellowPPath;

        if (!draw)
        {
            return;
        }

        DrawOriginalRedGreenLines();
        DrawYellowPPathAndPurpleBendMarker();
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
            SuperController.LogMessage("[TargetLinePerson] Green guide lock skipped / reason=" + reason + " / no line");
            return;
        }

        float len = Vector3.Distance(capturedMoveLineStart, capturedMoveLineEnd);
        string yInfo = hasCapturedGreenBaseY ? (" / greenY=" + capturedGreenBaseY.ToString("F3")) : "";
        SuperController.LogMessage("[TargetLinePerson] Green guide locked AFTER placement / reason=" + reason + " / len=" + len.ToString("F3") + yInfo);
    }

    void DrawOriginalRedGreenLines()
    {
        Vector3 forwardDir = capturedLineDir;

        if (forwardDir.sqrMagnitude < 0.0001f)
        {
            forwardDir = capturedDir.sqrMagnitude >= 0.0001f ? capturedDir : Vector3.forward;
        }

        forwardDir.Normalize();

        if (forwardLine != null)
        {
            forwardLine.positionCount = 2;
            forwardLine.SetPosition(0, capturedOrigin);
            forwardLine.SetPosition(1, capturedOrigin + forwardDir * 1.5f);
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

        SuperController.LogMessage("[TargetLinePerson] Yellow Butt Guide Scale changed / value=" + GetYellowButtGuideScale().ToString("F2") + " / guide will rebuild");
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

            float dipForwardBase = Mathf.Clamp(remainingFlat * 0.25f, 0.04f, 0.10f);
            float originBackBase = Mathf.Clamp(remainingFlat * 0.55f, 0.04f, 0.12f);
            float rampScale = 1.0f / Mathf.Max(0.01f, buttScale);

            float dipForward = dipForwardBase * rampScale;
            float originBack = originBackBase * rampScale;

            float minRamp = Mathf.Min(0.015f, remainingFlat * 0.10f);
            float maxRamp = Mathf.Max(minRamp, remainingFlat * 0.45f);
            dipForward = Mathf.Clamp(dipForward, minRamp, maxRamp);
            originBack = Mathf.Clamp(originBack, minRamp, maxRamp);

            // Keep at least a small horizontal bottom section.  This prevents the
            // trapezoid from collapsing when the remaining distance is short.
            float minBottom = Mathf.Min(0.03f * buttScale, remainingFlat * 0.20f);
            float bottom = remainingFlat - dipForward - originBack;
            if (remainingFlat > 0.0001f && bottom < minBottom)
            {
                float reduce = (minBottom - bottom) * 0.5f;
                dipForward = Mathf.Max(minRamp, dipForward - reduce);
                originBack = Mathf.Max(minRamp, originBack - reduce);
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
        Vector3 p5 = capturedOrigin + redUpDir * 0.45f;

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

        SuperController.LogMessage("[TargetLinePerson] P Yellow Path released / reason=" + reason + " / restored=" + restored + " / advance=0.000 / align=OFF");
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

        SuperController.LogMessage("[TargetLinePerson] P Yellow Path advance set / reason=" + reason + " / advance=" + next.ToString("F3"));
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

        // Allow the tip to climb onto the red/up extension, but avoid endlessly
        // pushing all three P controls into the final endpoint.
        float shaftLen = Mathf.Max(0.0f, yellowBaseToMidLength + yellowMidToTipLength);
        float max = yellowPPathTotalLength - shaftLen * 0.25f;
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
        if (pPathSealed != null && pPathSealed.val)
        {
            if (log) SuperController.LogMessage("[TargetLinePerson] P Yellow Path apply skipped: P Path Sealed ON / reason=" + reason);
            return false;
        }

        if (pYellowPathAlign == null || !pYellowPathAlign.val)
        {
            if (log) SuperController.LogMessage("[TargetLinePerson] P Yellow Path apply skipped: Align OFF / reason=" + reason);
            return false;
        }

        if (!hasYellowPPath && !pYellowCapturePending)
        {
            BuildCapturedYellowPPath();
        }

        if (!hasYellowPPath)
        {
            if (log) SuperController.LogMessage("[TargetLinePerson] P Yellow Path apply skipped: no yellow path / reason=" + reason);
            return false;
        }

        if (IsPControlBlockedByLiePose())
        {
            if (log) SuperController.LogMessage("[TargetLinePerson] P Yellow Path apply skipped: lie pose active / reason=" + reason);
            return false;
        }

        if (isAvoidMoving)
        {
            if (log) SuperController.LogMessage("[TargetLinePerson] P Yellow Path apply skipped: avoid moving / reason=" + reason);
            return false;
        }

        if (targetControllerChooser != null && targetControllerChooser.val != "genital")
        {
            if (log) SuperController.LogMessage("[TargetLinePerson] P Yellow Path apply skipped: targetController=" + targetControllerChooser.val + " / reason=" + reason);
            return false;
        }

        FreeControllerV3 penisBase = GetOwnPenisBase();
        FreeControllerV3 penisMid = GetOwnPenisMid();
        FreeControllerV3 penisTip = GetOwnPenisTip();

        if (penisBase == null || penisMid == null || penisTip == null)
        {
            if (log)
            {
                SuperController.LogMessage(
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
            SuperController.LogMessage(
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

        // VaM/FreeControllerV3 は transform だけだと UI control 側に負ける場合があるので、
        // サンプル方式を維持しつつ control も同時に更新する。
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
        if (delayedLineLockRoutine != null)
        {
            StopCoroutine(delayedLineLockRoutine);
            delayedLineLockRoutine = null;
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
    }
}
