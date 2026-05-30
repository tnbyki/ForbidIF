// ============================================================
// Human Grab Hand  Tracker control plugin for VaM
// File   : HumanGrabTracker.cs
// Version: 1.0.0 by VAMT
// ============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class HumanGrabTracker : MVRScript
{
    private const int UDP_PORT = 5006;
    private const string NONE_ATOM = "None";
    private const string CLOTH_GRAB_LEFT_UID = "ClothGrab_L";
    private const string CLOTH_GRAB_RIGHT_UID = "ClothGrab_R";
    private const float HAND_LOST_SECONDS = 0.5f;
    private const float RELEASE_OPEN_SECONDS = 0.7f;
    private const float CLOTH_GRAB_HIDE_Y = -10.0f;
    private const float INTERNAL_PULL_SMOOTHNESS = 0.12f;
    private const float BODY_PULL_DEPTH_GATE = 0.12f;
    private const float HAND_ACTION_RELEASE_SECONDS = 0.35f;
    private const float HAND_ACTION_SUCCESS_SECONDS = 0.70f;
    private const float HAND_GRAB_SUCCESS_ROTATION = 20.0f;
    private const float HAND_GRAB_RELEASE_ROTATION = -20.0f;
    private const float HAND_GRAB_HOLD_INWARD_ROTATION = 15.0f;
    private const float HAND_ROTATION_JUMP_LIMIT = 120.0f;
    private const float HAND_ROTATION_SMOOTH_SECONDS = 0.12f;

    private JSONStorableBool enableHand;
    private JSONStorableStringChooser leftHandAtomChooser;
    private JSONStorableStringChooser rightHandAtomChooser;
    private JSONStorableBool enableBodyPull;
    private JSONStorableBool pullNipple;
    private JSONStorableBool pullHand;
    private JSONStorableBool pullKnee;
    private JSONStorableBool pullHead;
    private JSONStorableBool pullHip;
    private JSONStorableBool pullFoot;
    private JSONStorableFloat bodyPullDistance;
    private JSONStorableFloat bodyPullStrength;
    private JSONStorableFloat handSmoothness;
    private JSONStorableFloat baseOffsetY;
    private JSONStorableFloat baseOffsetZ;
    private JSONStorableFloat handXScale;
    private JSONStorableBool enableHandXRotation;
    private JSONStorableFloat handXNeutralAngle;
    private JSONStorableFloat handXRotationScale;
    private JSONStorableFloat handXRotationOffset;
    private JSONStorableFloat handGrabForwardOffset;
    private JSONStorableBool enableClothGrab;
    private JSONStorableFloat clothGrabForwardOffset;
    private JSONStorableFloat depthScale;
    private JSONStorableFloat depthDeadZone;
    private JSONStorableFloat depthLimit;
    private JSONStorableFloat leftDepthBias;
    private JSONStorableFloat rightDepthBias;
    private JSONStorableBool invertDepth;

    private Atom leftHandAtom;
    private Atom rightHandAtom;
    private Atom leftClothGrabAtom;
    private Atom rightClothGrabAtom;

    private FreeControllerV3 lNippleControl;
    private FreeControllerV3 rNippleControl;
    private FreeControllerV3 lHandControl;
    private FreeControllerV3 rHandControl;
    private FreeControllerV3 lKneeControl;
    private FreeControllerV3 rKneeControl;
    private FreeControllerV3 lFootControl;
    private FreeControllerV3 rFootControl;
    private FreeControllerV3 headControl;
    private FreeControllerV3 hipControl;

    private Transform headRoot;

    private UdpClient udp;
    private readonly object packetLock = new object();
    private string pendingLeftPacket = "";
    private string pendingRightPacket = "";

    private Vector3 idleLeft = new Vector3(-0.25f, -0.35f, 0.5f);
    private Vector3 idleRight = new Vector3(0.25f, -0.35f, 0.5f);
    private Vector3 targetLeft;
    private Vector3 targetRight;
    private Vector3 currentLeft;
    private Vector3 currentRight;
    private float targetLeftXRotation;
    private float targetRightXRotation;
    private float currentLeftXRotation;
    private float currentRightXRotation;
    private bool hasLeftXRotation;
    private bool hasRightXRotation;
    private float leftActionRotation;
    private float rightActionRotation;
    private float leftActionStartedAt = -1f;
    private float rightActionStartedAt = -1f;
    private bool leftWasClosed;
    private bool rightWasClosed;
    private bool leftClothGrabActive;
    private bool rightClothGrabActive;
    private int leftClothGrabGeneration;
    private int rightClothGrabGeneration;
    private bool leftGrabbedThisFrame;
    private bool rightGrabbedThisFrame;
    private Quaternion leftHandBaseRotation = Quaternion.identity;
    private Quaternion rightHandBaseRotation = Quaternion.identity;

    private string leftHandState = "HAND_OPEN";
    private string rightHandState = "HAND_OPEN";
    private float leftLastReceiveTime = -100f;
    private float rightLastReceiveTime = -100f;
    private float leftOpenStartedAt = -1f;
    private float rightOpenStartedAt = -1f;

    private float leftNeutralZ;
    private float rightNeutralZ;
    private bool hasLeftNeutralZ;
    private bool hasRightNeutralZ;
    private bool wasEnabled;

    private class PullTargetState
    {
        public string sourceHand = "";
        public Vector3 grabHandPosition = Vector3.zero;
        public Vector3 grabControlPosition = Vector3.zero;
    }

    private PullTargetState lNippleState = new PullTargetState();
    private PullTargetState rNippleState = new PullTargetState();
    private PullTargetState lHandState = new PullTargetState();
    private PullTargetState rHandState = new PullTargetState();
    private PullTargetState lKneeState = new PullTargetState();
    private PullTargetState rKneeState = new PullTargetState();
    private PullTargetState lFootState = new PullTargetState();
    private PullTargetState rFootState = new PullTargetState();
    private PullTargetState headState = new PullTargetState();
    private PullTargetState hipState = new PullTargetState();

    public override void Init()
    {
        targetLeft = idleLeft;
        targetRight = idleRight;
        currentLeft = idleLeft;
        currentRight = idleRight;

        enableHand = new JSONStorableBool("Enable Hand Tracking", false);
        RegisterBool(enableHand);
        CreateToggle(enableHand, false);

        List<string> handChoices = BuildHandAtomChoices();

        leftHandAtomChooser = new JSONStorableStringChooser(
            "Left Hand Atom",
            handChoices,
            NONE_ATOM,
            "Left Hand Atom",
            delegate(string value) { RefreshHandAtoms(); }
        );
        RegisterStringChooser(leftHandAtomChooser);
        CreatePopup(leftHandAtomChooser, false);

        rightHandAtomChooser = new JSONStorableStringChooser(
            "Right Hand Atom",
            handChoices,
            NONE_ATOM,
            "Right Hand Atom",
            delegate(string value) { RefreshHandAtoms(); }
        );
        RegisterStringChooser(rightHandAtomChooser);
        CreatePopup(rightHandAtomChooser, false);

        CreateButton("Refresh Hand Atom List", false)
            .button.onClick.AddListener(delegate
            {
                RefreshHandAtomChoices();
                RefreshHandAtoms();
            });

        bodyPullDistance = new JSONStorableFloat("Body Pull Distance", 0.07f, 0.02f, 0.30f);
        RegisterFloat(bodyPullDistance);
        CreateSlider(bodyPullDistance, false);

        handGrabForwardOffset = new JSONStorableFloat("Grab Forward Offset", 0.20f, -0.20f, 0.30f);
        RegisterFloat(handGrabForwardOffset);
        CreateSlider(handGrabForwardOffset, false);

        enableClothGrab = new JSONStorableBool("Enable Cloth Grab", true);
        RegisterBool(enableClothGrab);
        CreateToggle(enableClothGrab, false);

        clothGrabForwardOffset = new JSONStorableFloat("Cloth Grab Forward Offset", 0.20f, -0.20f, 0.50f);
        RegisterFloat(clothGrabForwardOffset);
        CreateSlider(clothGrabForwardOffset, false);

        bodyPullStrength = new JSONStorableFloat("Body Pull Strength", 1.0f, 0.0f, 10.0f);
        RegisterFloat(bodyPullStrength);
        CreateSlider(bodyPullStrength, false);

        CreateButton("Reset Pull Targets", false)
            .button.onClick.AddListener(ResetPullTargets);

        enableBodyPull = new JSONStorableBool("Enable Body Pull", true);
        RegisterBool(enableBodyPull);
        CreateToggle(enableBodyPull, false);

        pullNipple = new JSONStorableBool("Pull Nipple", true);
        RegisterBool(pullNipple);
        CreateToggle(pullNipple, false);

        pullHand = new JSONStorableBool("Pull Hand", true);
        RegisterBool(pullHand);
        CreateToggle(pullHand, false);

        pullKnee = new JSONStorableBool("Pull Knee", true);
        RegisterBool(pullKnee);
        CreateToggle(pullKnee, false);

        pullHead = new JSONStorableBool("Pull Head", true);
        RegisterBool(pullHead);
        CreateToggle(pullHead, false);

        pullHip = new JSONStorableBool("Pull Hip", true);
        RegisterBool(pullHip);
        CreateToggle(pullHip, false);

        pullFoot = new JSONStorableBool("Pull Foot", true);
        RegisterBool(pullFoot);
        CreateToggle(pullFoot, false);

        handSmoothness = new JSONStorableFloat("Hand Smoothness", 0.18f, 0.0f, 1.0f);
        RegisterFloat(handSmoothness);
        CreateSlider(handSmoothness, true);

        baseOffsetY = new JSONStorableFloat("Base Offset Y", 0.2f, -1.0f, 1.0f);
        RegisterFloat(baseOffsetY);
        CreateSlider(baseOffsetY, true);

        baseOffsetZ = new JSONStorableFloat("Base Offset Z", 1.0f, -1.0f, 2.0f);
        RegisterFloat(baseOffsetZ);
        CreateSlider(baseOffsetZ, true);

        handXScale = new JSONStorableFloat("Hand X Scale", 2.0f, 0.5f, 3.0f);
        RegisterFloat(handXScale);
        CreateSlider(handXScale, true);

        enableHandXRotation = new JSONStorableBool("Enable Hand X Rotation", false);
        RegisterBool(enableHandXRotation);
        CreateToggle(enableHandXRotation, true);

        handXNeutralAngle = new JSONStorableFloat("Hand X Neutral Angle", 90.0f, 0.0f, 180.0f);
        RegisterFloat(handXNeutralAngle);
        CreateSlider(handXNeutralAngle, true);

        handXRotationScale = new JSONStorableFloat("Hand X Rotation Scale", 1.0f, -2.0f, 2.0f);
        RegisterFloat(handXRotationScale);
        CreateSlider(handXRotationScale, true);

        handXRotationOffset = new JSONStorableFloat("Hand X Rotation Offset", 0.0f, -90.0f, 90.0f);
        RegisterFloat(handXRotationOffset);
        CreateSlider(handXRotationOffset, true);

        depthScale = new JSONStorableFloat("Hand Depth Scale", 25.0f, 0.0f, 40.0f);
        RegisterFloat(depthScale);
        CreateSlider(depthScale, true);

        depthDeadZone = new JSONStorableFloat("Hand Depth Dead Zone", 0.01f, 0.0f, 0.20f);
        RegisterFloat(depthDeadZone);
        CreateSlider(depthDeadZone, true);

        depthLimit = new JSONStorableFloat("Hand Depth Limit", 1.0f, 0.1f, 2.0f);
        RegisterFloat(depthLimit);
        CreateSlider(depthLimit, true);

        leftDepthBias = new JSONStorableFloat("Left Depth Bias", 0.0f, -1.0f, 1.0f);
        RegisterFloat(leftDepthBias);
        CreateSlider(leftDepthBias, true);

        rightDepthBias = new JSONStorableFloat("Right Depth Bias", 0.0f, -1.0f, 1.0f);
        RegisterFloat(rightDepthBias);
        CreateSlider(rightDepthBias, true);

        invertDepth = new JSONStorableBool("Invert Hand Depth", true);
        RegisterBool(invertDepth);
        CreateToggle(invertDepth, true);

        CreateButton("Reset Depth Center", true)
            .button.onClick.AddListener(ResetDepthCenter);

        CreateButton("Refresh Body Controls", true)
            .button.onClick.AddListener(RefreshBodyControls);

        ResolveCameraRoot();
        RefreshHandAtoms();
        RefreshBodyControls();
        StartCoroutine(EnsureClothGrabAtomsCo());
        StartUDP();
    }

    private void ResolveCameraRoot()
    {
        headRoot = null;

        if (SuperController.singleton != null &&
            SuperController.singleton.centerCameraTarget != null)
        {
            headRoot = SuperController.singleton.centerCameraTarget.transform;
        }

        if (headRoot == null && containingAtom != null)
        {
            headRoot = containingAtom.transform;
        }

        SuperController.LogMessage(headRoot != null ? "Camera Root OK" : "Camera Root not found");
    }

    private void ResetDepthCenter()
    {
        hasLeftNeutralZ = false;
        hasRightNeutralZ = false;
        SuperController.LogMessage("Hand depth center reset");
    }

    private float ParseFloat(string value)
    {
        float result;
        if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
        {
            return result;
        }
        if (float.TryParse(value, out result))
        {
            return result;
        }
        return 0f;
    }

    private List<string> BuildHandAtomChoices()
    {
        List<string> choices = new List<string>();
        choices.Add(NONE_ATOM);

        if (SuperController.singleton == null)
        {
            return choices;
        }

        foreach (Atom atom in SuperController.singleton.GetAtoms())
        {
            if (atom == null || string.IsNullOrEmpty(atom.uid))
            {
                continue;
            }

            if (atom.uid.IndexOf("hand", StringComparison.OrdinalIgnoreCase) < 0)
            {
                continue;
            }

            if (!choices.Contains(atom.uid))
            {
                choices.Add(atom.uid);
            }
        }

        return choices;
    }

    private void RefreshHandAtomChoices()
    {
        List<string> choices = BuildHandAtomChoices();
        UpdateChooserChoices(leftHandAtomChooser, choices);
        UpdateChooserChoices(rightHandAtomChooser, choices);
    }

    private void UpdateChooserChoices(JSONStorableStringChooser chooser, List<string> choices)
    {
        if (chooser == null)
        {
            return;
        }

        string current = chooser.val;
        chooser.choices = choices;

        if (choices.Contains(current))
        {
            chooser.val = current;
        }
        else
        {
            chooser.val = NONE_ATOM;
        }
    }

    private Atom FindAtomByUid(string uid)
    {
        if (SuperController.singleton == null || string.IsNullOrEmpty(uid))
        {
            return null;
        }

        return SuperController.singleton.GetAtomByUid(uid);
    }

    private Atom FindHandAtom(JSONStorableStringChooser chooser, string exactUid, string sideWord)
    {
        if (chooser == null || chooser.val == NONE_ATOM)
        {
            return null;
        }

        if (!string.IsNullOrEmpty(chooser.val))
        {
            return FindAtomByUid(chooser.val);
        }

        return null;
    }

    private void RefreshHandAtoms()
    {
        leftHandAtom = FindHandAtom(leftHandAtomChooser, "LeftHandEmpty", "Left");
        rightHandAtom = FindHandAtom(rightHandAtomChooser, "RightHandEmpty", "Right");

        if (enableHand != null && enableHand.val)
        {
            ParentHandAtomToCameraRoot(leftHandAtom);
            ParentHandAtomToCameraRoot(rightHandAtom);
        }
        else
        {
            DetachHandAtomFromCameraRoot(leftHandAtom);
            DetachHandAtomFromCameraRoot(rightHandAtom);
        }

        CacheHandBaseRotation(leftHandAtom, ref leftHandBaseRotation);
        CacheHandBaseRotation(rightHandAtom, ref rightHandBaseRotation);
        ResetHandXRotationState();

        SuperController.LogMessage(
            "Hand atoms - Left:" + (leftHandAtom != null) +
            " Right:" + (rightHandAtom != null)
        );
    }

    private void CacheHandBaseRotation(Atom atom, ref Quaternion baseRotation)
    {
        if (atom == null ||
            atom.mainController == null ||
            atom.mainController.transform == null)
        {
            return;
        }

        baseRotation = atom.mainController.transform.localRotation;
    }

    private void ResetHandXRotationState()
    {
        hasLeftXRotation = false;
        hasRightXRotation = false;
        currentLeftXRotation = 0.0f;
        currentRightXRotation = 0.0f;
        targetLeftXRotation = 0.0f;
        targetRightXRotation = 0.0f;
        leftActionRotation = 0.0f;
        rightActionRotation = 0.0f;
        leftActionStartedAt = -1f;
        rightActionStartedAt = -1f;
    }

    private void ParentHandAtomToCameraRoot(Atom atom)
    {
        if (atom == null ||
            atom.mainController == null ||
            atom.mainController.transform == null ||
            headRoot == null)
        {
            return;
        }

        atom.mainController.transform.SetParent(
            headRoot,
            true
        );
    }

    private void DetachHandAtomFromCameraRoot(Atom atom)
    {
        if (atom == null ||
            atom.mainController == null ||
            atom.mainController.transform == null)
        {
            return;
        }

        atom.mainController.transform.SetParent(null, true);
    }

    private IEnumerator EnsureClothGrabAtomsCo()
    {
        RefreshClothGrabAtoms();

        if (leftClothGrabAtom == null)
        {
            yield return CreateClothGrabAtomCo(CLOTH_GRAB_LEFT_UID);
        }

        if (rightClothGrabAtom == null)
        {
            yield return CreateClothGrabAtomCo(CLOTH_GRAB_RIGHT_UID);
        }

        RefreshClothGrabAtoms();
        HideClothGrab("LEFT");
        HideClothGrab("RIGHT");
        yield return ReinitializeClothGrabAtomCo(leftClothGrabAtom);
        yield return ReinitializeClothGrabAtomCo(rightClothGrabAtom);
        HideClothGrab("LEFT");
        HideClothGrab("RIGHT");
    }

    private IEnumerator CreateClothGrabAtomCo(string uid)
    {
        if (FindAtomByUid(uid) != null)
        {
            yield break;
        }

        List<string> before = GetAllAtomUids();

        SuperController.singleton.AddAtomByType(
            "ClothGrabSphere",
            false,
            false,
            false
        );

        yield return null;
        yield return null;
        yield return null;

        Atom newAtom = FindNewAtom(before);
        if (newAtom == null)
        {
            SuperController.LogError("[ClothGrab] Create failed: " + uid);
            yield break;
        }

        SuperController.singleton.RenameAtom(newAtom, uid);
        newAtom.SetOn(true);
        newAtom.transform.localScale = Vector3.one * 0.3f;

        if (newAtom.mainController != null && newAtom.mainController.transform != null)
        {
            newAtom.mainController.transform.position = GetClothGrabHiddenPosition();
        }

        SuperController.LogMessage("[ClothGrab] Created " + uid);
    }

    private IEnumerator ReinitializeClothGrabAtomCo(Atom atom)
    {
        if (atom == null)
        {
            yield break;
        }

        atom.SetOn(false);
        yield return null;
        yield return null;
        atom.SetOn(true);
    }

    private void RefreshClothGrabAtoms()
    {
        leftClothGrabAtom = FindAtomByUid(CLOTH_GRAB_LEFT_UID);
        rightClothGrabAtom = FindAtomByUid(CLOTH_GRAB_RIGHT_UID);

        if (leftClothGrabAtom != null)
        {
            leftClothGrabAtom.SetOn(true);
        }

        if (rightClothGrabAtom != null)
        {
            rightClothGrabAtom.SetOn(true);
        }

        SuperController.LogMessage(
            "ClothGrab atoms - Left:" + (leftClothGrabAtom != null) +
            " Right:" + (rightClothGrabAtom != null)
        );
    }

    private List<string> GetAllAtomUids()
    {
        List<string> list = new List<string>();

        foreach (Atom atom in SuperController.singleton.GetAtoms())
        {
            if (atom != null)
            {
                list.Add(atom.uid);
            }
        }

        return list;
    }

    private Atom FindNewAtom(List<string> before)
    {
        foreach (Atom atom in SuperController.singleton.GetAtoms())
        {
            if (atom == null)
            {
                continue;
            }

            if (!before.Contains(atom.uid))
            {
                return atom;
            }
        }

        return null;
    }

    private Vector3 GetClothGrabHiddenPosition()
    {
        Vector3 origin =
            containingAtom != null
                ? containingAtom.transform.position
                : Vector3.zero;

        origin.y += CLOTH_GRAB_HIDE_Y;
        return origin;
    }

    private void RefreshBodyControls()
    {
        if (containingAtom == null)
        {
            return;
        }

        lNippleControl = containingAtom.GetStorableByID("lNippleControl") as FreeControllerV3;
        rNippleControl = containingAtom.GetStorableByID("rNippleControl") as FreeControllerV3;
        lHandControl = containingAtom.GetStorableByID("lHandControl") as FreeControllerV3;
        rHandControl = containingAtom.GetStorableByID("rHandControl") as FreeControllerV3;
        lKneeControl = containingAtom.GetStorableByID("lKneeControl") as FreeControllerV3;
        rKneeControl = containingAtom.GetStorableByID("rKneeControl") as FreeControllerV3;
        lFootControl = containingAtom.GetStorableByID("lFootControl") as FreeControllerV3;
        rFootControl = containingAtom.GetStorableByID("rFootControl") as FreeControllerV3;
        headControl = containingAtom.GetStorableByID("headControl") as FreeControllerV3;
        hipControl = containingAtom.GetStorableByID("hipControl") as FreeControllerV3;

        SuperController.LogMessage(
            "Body controls - " +
            "Nipple L:" + (lNippleControl != null) + " R:" + (rNippleControl != null) + " / " +
            "Hand L:" + (lHandControl != null) + " R:" + (rHandControl != null) + " / " +
            "Knee L:" + (lKneeControl != null) + " R:" + (rKneeControl != null) + " / " +
            "Foot L:" + (lFootControl != null) + " R:" + (rFootControl != null) + " / " +
            "Head:" + (headControl != null) + " Hip:" + (hipControl != null)
        );
    }

    private void SnapHandIKToVisibleHand(string side)
    {
        FreeControllerV3 handControl = side == "LEFT" ? lHandControl : rHandControl;
        string sidePrefix = side == "LEFT" ? "l" : "r";
        Transform visibleHand = FindVisibleHandTransform(sidePrefix, handControl);

        if (handControl == null || visibleHand == null)
        {
            SuperController.LogMessage(
                "[HandIKResync] Skip " +
                side +
                " handControl:" +
                (handControl != null) +
                " visibleHand:" +
                GetPath(visibleHand)
            );
            return;
        }

        Vector3 before = handControl.transform.position;

        handControl.currentPositionState = FreeControllerV3.PositionState.On;
        handControl.currentRotationState = FreeControllerV3.RotationState.On;
        handControl.transform.position = visibleHand.position;
        handControl.transform.rotation = visibleHand.rotation;

        SuperController.LogMessage(
            "[HandIKResync] Snap " +
            side +
            " IK to " +
            visibleHand.name +
            " move=" +
            Vector3.Distance(before, handControl.transform.position).ToString("F4") +
            " path=" +
            GetPath(visibleHand)
        );
    }

    private void SnapKneeIKToVisibleKnee(string side)
    {
        FreeControllerV3 kneeControl = side == "LEFT" ? lKneeControl : rKneeControl;
        string sidePrefix = side == "LEFT" ? "l" : "r";
        Transform visibleKnee = FindVisibleKneeTransform(sidePrefix, kneeControl);

        if (kneeControl == null || visibleKnee == null)
        {
            SuperController.LogMessage(
                "[KneeIKResync] Skip " +
                side +
                " kneeControl:" +
                (kneeControl != null) +
                " visibleKnee:" +
                GetPath(visibleKnee)
            );
            return;
        }

        Vector3 before = kneeControl.transform.position;

        kneeControl.currentPositionState = FreeControllerV3.PositionState.On;
        kneeControl.currentRotationState = FreeControllerV3.RotationState.On;
        kneeControl.transform.position = visibleKnee.position;
        kneeControl.transform.rotation = visibleKnee.rotation;

        SuperController.LogMessage(
            "[KneeIKResync] Snap " +
            side +
            " IK to " +
            visibleKnee.name +
            " move=" +
            Vector3.Distance(before, kneeControl.transform.position).ToString("F4") +
            " path=" +
            GetPath(visibleKnee)
        );
    }

    private void SnapFootIKToVisibleFoot(string side)
    {
        FreeControllerV3 footControl = side == "LEFT" ? lFootControl : rFootControl;
        string sidePrefix = side == "LEFT" ? "l" : "r";
        Transform visibleFoot = FindVisibleFootTransform(sidePrefix, footControl);

        if (footControl == null || visibleFoot == null)
        {
            SuperController.LogMessage(
                "[FootIKResync] Skip " +
                side +
                " footControl:" +
                (footControl != null) +
                " visibleFoot:" +
                GetPath(visibleFoot)
            );
            return;
        }

        Vector3 before = footControl.transform.position;

        footControl.currentPositionState = FreeControllerV3.PositionState.On;
        footControl.currentRotationState = FreeControllerV3.RotationState.On;
        footControl.transform.position = visibleFoot.position;
        footControl.transform.rotation = visibleFoot.rotation;

        SuperController.LogMessage(
            "[FootIKResync] Snap " +
            side +
            " IK to " +
            visibleFoot.name +
            " move=" +
            Vector3.Distance(before, footControl.transform.position).ToString("F4") +
            " path=" +
            GetPath(visibleFoot)
        );
    }

    private void SnapSingleIKToVisiblePart(FreeControllerV3 control, string label)
    {
        Transform visible = FindSinglePartTransform(label, control);

        if (control == null || visible == null)
        {
            SuperController.LogMessage(
                "[" + label + "IKResync] Skip control:" +
                (control != null) +
                " visible:" +
                GetPath(visible)
            );
            return;
        }

        Vector3 before = control.transform.position;

        control.currentPositionState = FreeControllerV3.PositionState.On;
        control.currentRotationState = FreeControllerV3.RotationState.On;
        control.transform.position = visible.position;
        control.transform.rotation = visible.rotation;

        SuperController.LogMessage(
            "[" + label + "IKResync] Snap IK to " +
            visible.name +
            " move=" +
            Vector3.Distance(before, control.transform.position).ToString("F4") +
            " path=" +
            GetPath(visible)
        );
    }

    private Transform FindVisibleHandTransform(string sidePrefix, FreeControllerV3 handControl)
    {
        List<Transform> candidates = FindHandCandidates(sidePrefix);

        if (candidates.Count == 0)
        {
            return null;
        }

        if (handControl == null)
        {
            return candidates[0];
        }

        Transform best = null;
        float bestDist = float.MaxValue;

        foreach (Transform t in candidates)
        {
            float d = Vector3.Distance(handControl.transform.position, t.position);

            if (d < bestDist)
            {
                bestDist = d;
                best = t;
            }
        }

        return best;
    }

    private Transform FindVisibleKneeTransform(string sidePrefix, FreeControllerV3 kneeControl)
    {
        List<Transform> candidates = FindKneeCandidates(sidePrefix);

        if (candidates.Count == 0)
        {
            return null;
        }

        if (kneeControl == null)
        {
            return candidates[0];
        }

        Transform best = null;
        float bestDist = float.MaxValue;

        foreach (Transform t in candidates)
        {
            float d = Vector3.Distance(kneeControl.transform.position, t.position);

            if (d < bestDist)
            {
                bestDist = d;
                best = t;
            }
        }

        return best;
    }

    private Transform FindVisibleFootTransform(string sidePrefix, FreeControllerV3 footControl)
    {
        List<Transform> candidates = FindFootCandidates(sidePrefix);

        if (candidates.Count == 0)
        {
            return null;
        }

        if (footControl == null)
        {
            return candidates[0];
        }

        Transform best = null;
        float bestDist = float.MaxValue;

        foreach (Transform t in candidates)
        {
            float d = Vector3.Distance(footControl.transform.position, t.position);

            if (d < bestDist)
            {
                bestDist = d;
                best = t;
            }
        }

        return best;
    }

    private Transform FindSinglePartTransform(string label, FreeControllerV3 control)
    {
        List<Transform> candidates = FindSinglePartCandidates(label);

        if (candidates.Count == 0)
        {
            return null;
        }

        if (control == null)
        {
            return candidates[0];
        }

        Transform best = null;
        float bestDist = float.MaxValue;

        foreach (Transform t in candidates)
        {
            float d = Vector3.Distance(control.transform.position, t.position);

            if (d < bestDist)
            {
                bestDist = d;
                best = t;
            }
        }

        return best;
    }

    private List<Transform> FindHandCandidates(string sidePrefix)
    {
        List<Transform> result = new List<Transform>();

        if (containingAtom == null)
        {
            return result;
        }

        Transform[] transforms = containingAtom.GetComponentsInChildren<Transform>(false);
        string lowerPrefix = sidePrefix.ToLowerInvariant();

        foreach (Transform t in transforms)
        {
            if (t == null || string.IsNullOrEmpty(t.name))
            {
                continue;
            }

            if (!t.gameObject.activeInHierarchy)
            {
                continue;
            }

            string lower = t.name.ToLowerInvariant();

            if (!lower.StartsWith(lowerPrefix))
            {
                continue;
            }

            if (lower.IndexOf("control") >= 0)
            {
                continue;
            }

            if (lower.IndexOf("hand") >= 0 ||
                lower.IndexOf("carpal") >= 0 ||
                lower.IndexOf("metacarpal") >= 0)
            {
                if (!result.Contains(t))
                {
                    result.Add(t);
                }
            }
        }

        return result;
    }

    private List<Transform> FindKneeCandidates(string sidePrefix)
    {
        List<Transform> result = new List<Transform>();

        if (containingAtom == null)
        {
            return result;
        }

        Transform[] transforms = containingAtom.GetComponentsInChildren<Transform>(false);
        string lowerPrefix = sidePrefix.ToLowerInvariant();

        foreach (Transform t in transforms)
        {
            if (t == null || string.IsNullOrEmpty(t.name))
            {
                continue;
            }

            if (!t.gameObject.activeInHierarchy)
            {
                continue;
            }

            string lower = t.name.ToLowerInvariant();

            if (!lower.StartsWith(lowerPrefix))
            {
                continue;
            }

            if (lower.IndexOf("control") >= 0)
            {
                continue;
            }

            if (lower.IndexOf("knee") >= 0 ||
                lower.IndexOf("shin") >= 0 ||
                lower.IndexOf("thigh") >= 0)
            {
                if (!result.Contains(t))
                {
                    result.Add(t);
                }
            }
        }

        return result;
    }

    private List<Transform> FindFootCandidates(string sidePrefix)
    {
        List<Transform> result = new List<Transform>();

        if (containingAtom == null)
        {
            return result;
        }

        Transform[] transforms = containingAtom.GetComponentsInChildren<Transform>(false);
        string lowerPrefix = sidePrefix.ToLowerInvariant();

        foreach (Transform t in transforms)
        {
            if (t == null || string.IsNullOrEmpty(t.name))
            {
                continue;
            }

            if (!t.gameObject.activeInHierarchy)
            {
                continue;
            }

            string lower = t.name.ToLowerInvariant();

            if (!lower.StartsWith(lowerPrefix))
            {
                continue;
            }

            if (lower.IndexOf("control") >= 0)
            {
                continue;
            }

            if (lower.IndexOf("foot") >= 0 ||
                lower.IndexOf("toe") >= 0 ||
                lower.IndexOf("ankle") >= 0)
            {
                if (!result.Contains(t))
                {
                    result.Add(t);
                }
            }
        }

        return result;
    }

    private List<Transform> FindSinglePartCandidates(string label)
    {
        List<Transform> result = new List<Transform>();

        if (containingAtom == null)
        {
            return result;
        }

        Transform[] transforms = containingAtom.GetComponentsInChildren<Transform>(false);
        string lowerLabel = label.ToLowerInvariant();

        foreach (Transform t in transforms)
        {
            if (t == null || string.IsNullOrEmpty(t.name))
            {
                continue;
            }

            if (!t.gameObject.activeInHierarchy)
            {
                continue;
            }

            string lower = t.name.ToLowerInvariant();

            if (lower.IndexOf("control") >= 0)
            {
                continue;
            }

            bool match =
                lowerLabel == "head"
                    ? lower.IndexOf("head") >= 0 || lower.IndexOf("neck") >= 0 || lower.IndexOf("skull") >= 0
                    : lower.IndexOf("hip") >= 0 || lower.IndexOf("pelvis") >= 0;

            if (match && !result.Contains(t))
            {
                result.Add(t);
            }
        }

        return result;
    }

    private string GetPath(Transform t)
    {
        if (t == null)
        {
            return "null";
        }

        string path = t.name;
        Transform p = t.parent;

        while (p != null)
        {
            path = p.name + "/" + path;
            p = p.parent;
        }

        return path;
    }

    private void StartUDP()
    {
        try
        {
            udp = new UdpClient(UDP_PORT);
            udp.BeginReceive(ReceiveCallback, null);
            SuperController.LogMessage("Hand UDP " + UDP_PORT + " started");
        }
        catch (Exception e)
        {
            SuperController.LogMessage("Hand UDP not started: " + e.Message);
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udp.EndReceive(ar, ref ep);
            string message = Encoding.UTF8.GetString(data);

            lock (packetLock)
            {
                if (message.StartsWith("LEFT", StringComparison.OrdinalIgnoreCase))
                {
                    pendingLeftPacket = message;
                }
                else if (message.StartsWith("RIGHT", StringComparison.OrdinalIgnoreCase))
                {
                    pendingRightPacket = message;
                }
            }

            if (udp != null)
            {
                udp.BeginReceive(ReceiveCallback, null);
            }
        }
        catch (Exception e)
        {
            if (udp != null)
            {
                SuperController.LogMessage("Hand UDP receive error: " + e.Message);
            }
        }
    }

    private float GetDepthOffset(string side, float z)
    {
        bool isLeft = side == "LEFT";

        if (isLeft && !hasLeftNeutralZ)
        {
            leftNeutralZ = z;
            hasLeftNeutralZ = true;
        }
        else if (!isLeft && !hasRightNeutralZ)
        {
            rightNeutralZ = z;
            hasRightNeutralZ = true;
        }

        float neutral = isLeft ? leftNeutralZ : rightNeutralZ;
        float depth = z - neutral;

        if (Mathf.Abs(depth) < depthDeadZone.val)
        {
            depth = 0f;
        }

        if (invertDepth.val)
        {
            depth = -depth;
        }

        float offset = depth * depthScale.val;
        offset += isLeft ? leftDepthBias.val : rightDepthBias.val;

        return Mathf.Clamp(offset, -depthLimit.val, depthLimit.val);
    }

    private void ProcessHandPacket(string packet)
    {
        if (string.IsNullOrEmpty(packet))
        {
            return;
        }

        try
        {
            string[] parts = packet.Split(',');
            if (parts.Length < 4)
            {
                return;
            }

            string side = parts[0].Trim().ToUpper();
            float x = ParseFloat(parts[1]);
            float y = ParseFloat(parts[2]);
            float z = ParseFloat(parts[3]);
            float angle = parts.Length >= 5 ? ParseFloat(parts[4]) : 90.0f;
            string state = parts.Length >= 9 ? parts[8].Trim().ToUpper() : "HAND_OPEN";

            if (state == "HAND_HALF")
            {
                state = "HAND_CLOSED";
            }
            if (state != "HAND_CLOSED" && state != "HAND_OPEN")
            {
                state = "HAND_OPEN";
            }

            Vector3 target = new Vector3(
                (0.5f - x) * handXScale.val,
                baseOffsetY.val + ((0.5f - y) * 1.6f),
                baseOffsetZ.val + GetDepthOffset(side, z)
            );

            if (side == "LEFT")
            {
                targetLeft = target;
                targetLeftXRotation = GetHandXRotation("LEFT", angle);
                leftHandState = state;
                leftLastReceiveTime = Time.time;
            }
            else if (side == "RIGHT")
            {
                targetRight = target;
                targetRightXRotation = GetHandXRotation("RIGHT", angle);
                rightHandState = state;
                rightLastReceiveTime = Time.time;
            }
        }
        catch (Exception e)
        {
            SuperController.LogMessage("Hand packet parse failed: " + e.Message);
        }
    }

    private bool IsLeftVisible()
    {
        return Time.time - leftLastReceiveTime <= HAND_LOST_SECONDS &&
               leftHandAtom != null &&
               leftHandAtom.mainController != null &&
               leftHandAtom.mainController.transform != null;
    }

    private float GetHandXRotation(string side, float angle)
    {
        // face.py reports a screen-space hand direction angle.
        // The neutral angle is user-adjustable because webcam hand baselines vary.
        float raw = Mathf.Clamp(angle - handXNeutralAngle.val, -90.0f, 90.0f);
        bool hasValue = side == "LEFT" ? hasLeftXRotation : hasRightXRotation;
        float current = side == "LEFT" ? currentLeftXRotation : currentRightXRotation;

        if (!hasValue)
        {
            if (side == "LEFT")
            {
                hasLeftXRotation = true;
                currentLeftXRotation = raw;
            }
            else
            {
                hasRightXRotation = true;
                currentRightXRotation = raw;
            }

            return raw;
        }

        float delta = Mathf.Abs(Mathf.DeltaAngle(current, raw));
        if (delta > HAND_ROTATION_JUMP_LIMIT)
        {
            return current;
        }

        float t = 1.0f - Mathf.Exp(-Time.deltaTime / HAND_ROTATION_SMOOTH_SECONDS);
        float smoothed = Mathf.LerpAngle(current, raw, t);

        if (side == "LEFT")
        {
            currentLeftXRotation = smoothed;
        }
        else
        {
            currentRightXRotation = smoothed;
        }

        return smoothed;
    }

    private bool IsRightVisible()
    {
        return Time.time - rightLastReceiveTime <= HAND_LOST_SECONDS &&
               rightHandAtom != null &&
               rightHandAtom.mainController != null &&
               rightHandAtom.mainController.transform != null;
    }

    private bool IsHandVisible(string side)
    {
        return side == "LEFT" ? IsLeftVisible() : IsRightVisible();
    }

    private bool IsHandClosed(string side)
    {
        return side == "LEFT" ? leftHandState == "HAND_CLOSED" : rightHandState == "HAND_CLOSED";
    }

    private bool IsHandOpen(string side)
    {
        return side == "LEFT" ? leftHandState == "HAND_OPEN" : rightHandState == "HAND_OPEN";
    }

    private void BeginGrabActionFrame()
    {
        leftGrabbedThisFrame = false;
        rightGrabbedThisFrame = false;
    }

    private void FinishGrabActionFrame()
    {
        bool leftClosed = IsLeftVisible() && IsHandClosed("LEFT");
        bool rightClosed = IsRightVisible() && IsHandClosed("RIGHT");

        if (leftClosed && !leftWasClosed && !leftGrabbedThisFrame)
        {
            StartHandActionRotation("LEFT", HAND_GRAB_RELEASE_ROTATION);
            SuperController.LogMessage("[Grab Miss] LEFT");
        }

        if (leftClosed && !leftWasClosed)
        {
            ActivateClothGrab("LEFT");
        }

        if (rightClosed && !rightWasClosed && !rightGrabbedThisFrame)
        {
            StartHandActionRotation("RIGHT", HAND_GRAB_RELEASE_ROTATION);
            SuperController.LogMessage("[Grab Miss] RIGHT");
        }

        if (rightClosed && !rightWasClosed)
        {
            ActivateClothGrab("RIGHT");
        }

        leftWasClosed = leftClosed;
        rightWasClosed = rightClosed;
    }

    private void MarkGrabbedThisFrame(string side)
    {
        if (side == "LEFT")
        {
            leftGrabbedThisFrame = true;
        }
        else if (side == "RIGHT")
        {
            rightGrabbedThisFrame = true;
        }
    }

    private void StartHandActionRotation(string side, float rotation)
    {
        if (side == "LEFT")
        {
            leftActionRotation = rotation;
            leftActionStartedAt = Time.time;
        }
        else if (side == "RIGHT")
        {
            rightActionRotation = rotation;
            rightActionStartedAt = Time.time;
        }
    }

    private float GetHandActionRotation(string side)
    {
        float startedAt = side == "LEFT" ? leftActionStartedAt : rightActionStartedAt;
        if (startedAt < 0f)
        {
            return 0.0f;
        }

        float elapsed = Time.time - startedAt;
        float rotation = side == "LEFT" ? leftActionRotation : rightActionRotation;
        float duration =
            rotation > 0.0f
                ? HAND_ACTION_SUCCESS_SECONDS
                : HAND_ACTION_RELEASE_SECONDS;

        if (elapsed >= duration)
        {
            if (side == "LEFT")
            {
                leftActionRotation = 0.0f;
                leftActionStartedAt = -1f;
            }
            else if (side == "RIGHT")
            {
                rightActionRotation = 0.0f;
                rightActionStartedAt = -1f;
            }

            return 0.0f;
        }

        float t = Mathf.Clamp01(elapsed / duration);

        if (rotation > 0.0f)
        {
            float pulse = Mathf.Sin(t * Mathf.PI * 4.0f);
            return Mathf.Max(0.0f, pulse) * rotation;
        }

        return Mathf.Lerp(rotation, 0.0f, t);
    }

    private bool IsHandHoldingTarget(string side)
    {
        return lNippleState.sourceHand == side ||
               rNippleState.sourceHand == side ||
               lHandState.sourceHand == side ||
               rHandState.sourceHand == side ||
               lKneeState.sourceHand == side ||
               rKneeState.sourceHand == side ||
               lFootState.sourceHand == side ||
               rFootState.sourceHand == side ||
               headState.sourceHand == side ||
               hipState.sourceHand == side;
    }

    private float GetHandHoldInwardRotation(string side)
    {
        if (!IsHandHoldingTarget(side))
        {
            return 0.0f;
        }

        return side == "LEFT"
            ? HAND_GRAB_HOLD_INWARD_ROTATION
            : -HAND_GRAB_HOLD_INWARD_ROTATION;
    }

    private Transform GetHandTransform(string side)
    {
        if (side == "LEFT" && IsLeftVisible())
        {
            return leftHandAtom.mainController.transform;
        }

        if (side == "RIGHT" && IsRightVisible())
        {
            return rightHandAtom.mainController.transform;
        }

        return null;
    }

    private Vector3 GetGrabHandPosition(Transform hand)
    {
        if (hand == null)
        {
            return Vector3.zero;
        }

        Vector3 direction =
            headRoot != null
                ? headRoot.forward
                : hand.forward;

        return hand.position + (direction * handGrabForwardOffset.val);
    }

    private Vector3 GetClothGrabPosition(Transform hand)
    {
        if (hand == null)
        {
            return GetClothGrabHiddenPosition();
        }

        Vector3 direction =
            headRoot != null
                ? headRoot.forward
                : hand.forward;

        return hand.position + (direction * clothGrabForwardOffset.val);
    }

    private Atom GetClothGrabAtom(string side)
    {
        return side == "LEFT" ? leftClothGrabAtom : rightClothGrabAtom;
    }

    private void SetClothGrabActive(string side, bool active)
    {
        if (side == "LEFT")
        {
            leftClothGrabActive = active;
        }
        else if (side == "RIGHT")
        {
            rightClothGrabActive = active;
        }
    }

    private int NextClothGrabGeneration(string side)
    {
        if (side == "LEFT")
        {
            leftClothGrabGeneration++;
            return leftClothGrabGeneration;
        }

        rightClothGrabGeneration++;
        return rightClothGrabGeneration;
    }

    private bool IsCurrentClothGrabGeneration(string side, int generation)
    {
        return side == "LEFT"
            ? leftClothGrabGeneration == generation
            : rightClothGrabGeneration == generation;
    }

    private bool IsClothGrabActive(string side)
    {
        return side == "LEFT" ? leftClothGrabActive : rightClothGrabActive;
    }

    private void ActivateClothGrab(string side)
    {
        if (enableClothGrab == null || !enableClothGrab.val)
        {
            HideClothGrab(side);
            return;
        }

        SetClothGrabActive(side, true);
        StartCoroutine(ActivateClothGrabCo(side, NextClothGrabGeneration(side)));
    }

    private IEnumerator ActivateClothGrabCo(string side, int generation)
    {
        Atom atom = GetClothGrabAtom(side);

        if (atom == null ||
            atom.mainController == null ||
            atom.mainController.transform == null)
        {
            yield break;
        }

        atom.SetOn(false);
        MoveClothGrabToHand(side);
        yield return null;
        yield return null;
        if (!IsCurrentClothGrabGeneration(side, generation))
        {
            yield break;
        }

        atom.SetOn(true);
        MoveClothGrabToHand(side);
    }

    private void HideClothGrab(string side)
    {
        SetClothGrabActive(side, false);
        StartCoroutine(HideClothGrabCo(side, NextClothGrabGeneration(side)));
    }

    private IEnumerator HideClothGrabCo(string side, int generation)
    {
        Atom atom = GetClothGrabAtom(side);

        if (atom == null || atom.mainController == null || atom.mainController.transform == null)
        {
            yield break;
        }

        atom.SetOn(false);
        yield return null;
        yield return null;
        if (!IsCurrentClothGrabGeneration(side, generation))
        {
            yield break;
        }

        atom.mainController.transform.position = GetClothGrabHiddenPosition();
        yield return null;
        yield return null;
        if (!IsCurrentClothGrabGeneration(side, generation))
        {
            yield break;
        }

        atom.SetOn(true);
    }

    private void MoveClothGrabToHand(string side)
    {
        Atom atom = GetClothGrabAtom(side);
        Transform hand = GetHandTransform(side);

        if (atom == null ||
            atom.mainController == null ||
            atom.mainController.transform == null ||
            hand == null)
        {
            return;
        }

        atom.mainController.transform.position = GetClothGrabPosition(hand);
    }

    private void UpdateClothGrab()
    {
        if (enableClothGrab == null || !enableClothGrab.val)
        {
            HideClothGrab("LEFT");
            HideClothGrab("RIGHT");
            return;
        }

        if (IsClothGrabActive("LEFT"))
        {
            if (IsLeftVisible())
            {
                MoveClothGrabToHand("LEFT");
            }
            else
            {
                HideClothGrab("LEFT");
            }
        }

        if (IsClothGrabActive("RIGHT"))
        {
            if (IsRightVisible())
            {
                MoveClothGrabToHand("RIGHT");
            }
            else
            {
                HideClothGrab("RIGHT");
            }
        }
    }

    private void ApplyHandAtom(
        Atom atom,
        Vector3 localPosition,
        float localXRotation,
        string side,
        Quaternion baseRotation
    )
    {
        if (atom == null || atom.mainController == null || atom.mainController.transform == null)
        {
            return;
        }

        atom.mainController.transform.localPosition = localPosition;

        float trackedRotation = 0.0f;
        if (enableHandXRotation != null && enableHandXRotation.val)
        {
            trackedRotation =
                (localXRotation * handXRotationScale.val) +
                handXRotationOffset.val;
        }

        Quaternion extraRotation = Quaternion.Euler(
            trackedRotation + GetHandActionRotation(side),
            0.0f,
            GetHandHoldInwardRotation(side)
        );

        atom.mainController.transform.localRotation = baseRotation * extraRotation;
    }

    private bool IsTargetEnabled(FreeControllerV3 control)
    {
        if (control == null)
        {
            return false;
        }

        if (control == lNippleControl || control == rNippleControl)
        {
            return pullNipple.val;
        }
        if (control == lHandControl || control == rHandControl)
        {
            return pullHand.val;
        }
        if (control == lKneeControl || control == rKneeControl)
        {
            return pullKnee.val;
        }
        if (control == lFootControl || control == rFootControl)
        {
            return pullFoot.val;
        }
        if (control == headControl)
        {
            return pullHead.val;
        }
        if (control == hipControl)
        {
            return pullHip.val;
        }

        return false;
    }

    private string GetControlName(FreeControllerV3 control)
    {
        if (control == lNippleControl) return "lNippleControl";
        if (control == rNippleControl) return "rNippleControl";
        if (control == lHandControl) return "lHandControl";
        if (control == rHandControl) return "rHandControl";
        if (control == lKneeControl) return "lKneeControl";
        if (control == rKneeControl) return "rKneeControl";
        if (control == lFootControl) return "lFootControl";
        if (control == rFootControl) return "rFootControl";
        if (control == headControl) return "headControl";
        if (control == hipControl) return "hipControl";
        return "unknown";
    }

    private bool ShouldReleaseOff(FreeControllerV3 control)
    {
        return control == lNippleControl || control == rNippleControl;
    }

    private bool ReleaseTarget(FreeControllerV3 control, PullTargetState state)
    {
        bool hadSource = state != null && !string.IsNullOrEmpty(state.sourceHand);

        if (control != null)
        {
            control.currentPositionState =
                ShouldReleaseOff(control)
                    ? FreeControllerV3.PositionState.Off
                    : FreeControllerV3.PositionState.On;
        }

        if (state != null)
        {
            state.sourceHand = "";
        }

        if (hadSource)
        {
            SnapReleasedControlToVisibleBody(control);
        }

        return hadSource;
    }

    private void SnapReleasedControlToVisibleBody(FreeControllerV3 control)
    {
        if (control == lHandControl)
        {
            SnapHandIKToVisibleHand("LEFT");
            return;
        }

        if (control == rHandControl)
        {
            SnapHandIKToVisibleHand("RIGHT");
            return;
        }

        if (control == lKneeControl)
        {
            SnapKneeIKToVisibleKnee("LEFT");
            return;
        }

        if (control == rKneeControl)
        {
            SnapKneeIKToVisibleKnee("RIGHT");
            return;
        }

        if (control == lFootControl)
        {
            SnapFootIKToVisibleFoot("LEFT");
            return;
        }

        if (control == rFootControl)
        {
            SnapFootIKToVisibleFoot("RIGHT");
            return;
        }

        if (control == headControl)
        {
            SnapSingleIKToVisiblePart(headControl, "Head");
            return;
        }

        if (control == hipControl)
        {
            SnapSingleIKToVisiblePart(hipControl, "Hip");
        }
    }

    private void ReleaseTargetAndSnapSourceHand(FreeControllerV3 control, PullTargetState state)
    {
        string side = state != null ? state.sourceHand : "";
        bool released = ReleaseTarget(control, state);

        if (released && !string.IsNullOrEmpty(side))
        {
            StartHandActionRotation(side, HAND_GRAB_RELEASE_ROTATION);
            SnapHandIKToVisibleHand(side);
        }
    }

    private void ReleaseTargetsForHand(string side)
    {
        bool released = false;

        if (lNippleState.sourceHand == side) released |= ReleaseTarget(lNippleControl, lNippleState);
        if (rNippleState.sourceHand == side) released |= ReleaseTarget(rNippleControl, rNippleState);
        if (lHandState.sourceHand == side) released |= ReleaseTarget(lHandControl, lHandState);
        if (rHandState.sourceHand == side) released |= ReleaseTarget(rHandControl, rHandState);
        if (lKneeState.sourceHand == side) released |= ReleaseTarget(lKneeControl, lKneeState);
        if (rKneeState.sourceHand == side) released |= ReleaseTarget(rKneeControl, rKneeState);
        if (lFootState.sourceHand == side) released |= ReleaseTarget(lFootControl, lFootState);
        if (rFootState.sourceHand == side) released |= ReleaseTarget(rFootControl, rFootState);
        if (headState.sourceHand == side) released |= ReleaseTarget(headControl, headState);
        if (hipState.sourceHand == side) released |= ReleaseTarget(hipControl, hipState);

        if (released)
        {
            StartHandActionRotation(side, HAND_GRAB_RELEASE_ROTATION);
            SnapHandIKToVisibleHand(side);
        }
    }

    private void ReleaseAllTargets()
    {
        bool releasedLeft = false;
        bool releasedRight = false;

        TrackReleaseSide(lNippleControl, lNippleState, ref releasedLeft, ref releasedRight);
        TrackReleaseSide(rNippleControl, rNippleState, ref releasedLeft, ref releasedRight);
        TrackReleaseSide(lHandControl, lHandState, ref releasedLeft, ref releasedRight);
        TrackReleaseSide(rHandControl, rHandState, ref releasedLeft, ref releasedRight);
        TrackReleaseSide(lKneeControl, lKneeState, ref releasedLeft, ref releasedRight);
        TrackReleaseSide(rKneeControl, rKneeState, ref releasedLeft, ref releasedRight);
        TrackReleaseSide(lFootControl, lFootState, ref releasedLeft, ref releasedRight);
        TrackReleaseSide(rFootControl, rFootState, ref releasedLeft, ref releasedRight);
        TrackReleaseSide(headControl, headState, ref releasedLeft, ref releasedRight);
        TrackReleaseSide(hipControl, hipState, ref releasedLeft, ref releasedRight);

        if (!releasedLeft && !releasedRight)
        {
            ReleaseTarget(lNippleControl, lNippleState);
            ReleaseTarget(rNippleControl, rNippleState);
            ReleaseTarget(lHandControl, lHandState);
            ReleaseTarget(rHandControl, rHandState);
            ReleaseTarget(lKneeControl, lKneeState);
            ReleaseTarget(rKneeControl, rKneeState);
            ReleaseTarget(lFootControl, lFootState);
            ReleaseTarget(rFootControl, rFootState);
            ReleaseTarget(headControl, headState);
            ReleaseTarget(hipControl, hipState);
            return;
        }

        if (releasedLeft)
        {
            StartHandActionRotation("LEFT", HAND_GRAB_RELEASE_ROTATION);
            SnapHandIKToVisibleHand("LEFT");
        }
        if (releasedRight)
        {
            StartHandActionRotation("RIGHT", HAND_GRAB_RELEASE_ROTATION);
            SnapHandIKToVisibleHand("RIGHT");
        }
    }

    private void ResetPullTargets()
    {
        ReleaseAllTargets();
        SnapHandIKToVisibleHand("LEFT");
        SnapHandIKToVisibleHand("RIGHT");
        SnapKneeIKToVisibleKnee("LEFT");
        SnapKneeIKToVisibleKnee("RIGHT");
        SnapFootIKToVisibleFoot("LEFT");
        SnapFootIKToVisibleFoot("RIGHT");
        SnapSingleIKToVisiblePart(headControl, "Head");
        SnapSingleIKToVisiblePart(hipControl, "Hip");
        SuperController.LogMessage("[Hand] Pull targets reset");
    }

    private void TrackReleaseSide(
        FreeControllerV3 control,
        PullTargetState state,
        ref bool releasedLeft,
        ref bool releasedRight
    )
    {
        if (state == null || string.IsNullOrEmpty(state.sourceHand))
        {
            ReleaseTarget(control, state);
            return;
        }

        string side = state.sourceHand;
        bool released = ReleaseTarget(control, state);

        if (!released)
        {
            return;
        }

        if (side == "LEFT")
        {
            releasedLeft = true;
        }
        else if (side == "RIGHT")
        {
            releasedRight = true;
        }
    }

    private void UpdateReleaseTimer(string side)
    {
        bool isOpen = IsHandOpen(side);

        if (side == "LEFT")
        {
            if (isOpen)
            {
                if (leftOpenStartedAt < 0f) leftOpenStartedAt = Time.time;
                if (Time.time - leftOpenStartedAt >= RELEASE_OPEN_SECONDS)
                {
                    HideClothGrab(side);
                    ReleaseTargetsForHand(side);
                }
            }
            else
            {
                leftOpenStartedAt = -1f;
            }
            return;
        }

        if (isOpen)
        {
            if (rightOpenStartedAt < 0f) rightOpenStartedAt = Time.time;
            if (Time.time - rightOpenStartedAt >= RELEASE_OPEN_SECONDS)
            {
                HideClothGrab(side);
                ReleaseTargetsForHand(side);
            }
        }
        else
        {
            rightOpenStartedAt = -1f;
        }
    }

    private void UpdateLostHandRelease()
    {
        if (!IsLeftVisible())
        {
            leftOpenStartedAt = -1f;
            HideClothGrab("LEFT");
            ReleaseTargetsForHand("LEFT");
        }

        if (!IsRightVisible())
        {
            rightOpenStartedAt = -1f;
            HideClothGrab("RIGHT");
            ReleaseTargetsForHand("RIGHT");
        }
    }

    private bool TryFindGrabHand(FreeControllerV3 control, out string side, out Transform hand)
    {
        side = "";
        hand = null;

        float bestDistance = bodyPullDistance.val;

        if (IsLeftVisible() && IsHandClosed("LEFT"))
        {
            Transform left = leftHandAtom.mainController.transform;
            Vector3 leftGrabPosition = GetGrabHandPosition(left);
            float distance;
            if (IsGrabCandidate(leftGrabPosition, control.transform.position, bestDistance, out distance))
            {
                bestDistance = distance;
                side = "LEFT";
                hand = left;
            }
        }

        if (IsRightVisible() && IsHandClosed("RIGHT"))
        {
            Transform right = rightHandAtom.mainController.transform;
            Vector3 rightGrabPosition = GetGrabHandPosition(right);
            float distance;
            if (IsGrabCandidate(rightGrabPosition, control.transform.position, bestDistance, out distance))
            {
                bestDistance = distance;
                side = "RIGHT";
                hand = right;
            }
        }

        return hand != null;
    }

    private bool IsGrabCandidate(
        Vector3 handPosition,
        Vector3 controlPosition,
        float maxViewDistance,
        out float viewDistance
    )
    {
        viewDistance = GetViewPlaneDistance(handPosition, controlPosition);
        if (viewDistance > maxViewDistance)
        {
            return false;
        }

        return GetViewDepthDelta(handPosition, controlPosition) <= BODY_PULL_DEPTH_GATE;
    }

    private float GetViewPlaneDistance(Vector3 a, Vector3 b)
    {
        if (headRoot != null)
        {
            Vector3 localA = headRoot.InverseTransformPoint(a);
            Vector3 localB = headRoot.InverseTransformPoint(b);

            return Vector2.Distance(
                new Vector2(localA.x, localA.y),
                new Vector2(localB.x, localB.y)
            );
        }

        return Vector2.Distance(
            new Vector2(a.x, a.y),
            new Vector2(b.x, b.y)
        );
    }

    private float GetViewDepthDelta(Vector3 a, Vector3 b)
    {
        if (headRoot != null)
        {
            Vector3 localA = headRoot.InverseTransformPoint(a);
            Vector3 localB = headRoot.InverseTransformPoint(b);
            return Mathf.Abs(localA.z - localB.z);
        }

        return Mathf.Abs(a.z - b.z);
    }

    private void UpdatePullTarget(FreeControllerV3 control, PullTargetState state)
    {
        if (control == null || state == null)
        {
            return;
        }

        if (!enableBodyPull.val || !IsTargetEnabled(control))
        {
            ReleaseTargetAndSnapSourceHand(control, state);
            return;
        }

        Transform source = null;
        Vector3 sourceGrabPosition = Vector3.zero;

        if (!string.IsNullOrEmpty(state.sourceHand))
        {
            source = GetHandTransform(state.sourceHand);
            if (source == null)
            {
                ReleaseTargetAndSnapSourceHand(control, state);
                return;
            }

            sourceGrabPosition = GetGrabHandPosition(source);
        }

        if (source == null)
        {
            string side;
            if (!TryFindGrabHand(control, out side, out source))
            {
                return;
            }

            state.sourceHand = side;
            sourceGrabPosition = GetGrabHandPosition(source);
            state.grabHandPosition = sourceGrabPosition;
            state.grabControlPosition = control.transform.position;
            StartHandActionRotation(side, HAND_GRAB_SUCCESS_ROTATION);
            MarkGrabbedThisFrame(side);
            SuperController.LogMessage("[Grab] " + side + " -> " + GetControlName(control));
        }

        control.currentPositionState = FreeControllerV3.PositionState.On;

        float strength = bodyPullStrength.val;
        if (control == lNippleControl || control == rNippleControl)
        {
            strength *= 3.0f;
        }

        Vector3 desired =
            state.grabControlPosition +
            ((sourceGrabPosition - state.grabHandPosition) * strength);

        if (INTERNAL_PULL_SMOOTHNESS <= 0.001f)
        {
            control.transform.position = desired;
            return;
        }

        float t = 1.0f - Mathf.Exp(-Time.deltaTime / INTERNAL_PULL_SMOOTHNESS);
        control.transform.position = Vector3.Lerp(control.transform.position, desired, t);
    }

    private void UpdateBodyPull()
    {
        UpdateLostHandRelease();
        UpdateReleaseTimer("LEFT");
        UpdateReleaseTimer("RIGHT");

        UpdatePullTarget(lNippleControl, lNippleState);
        UpdatePullTarget(rNippleControl, rNippleState);
        UpdatePullTarget(lHandControl, lHandState);
        UpdatePullTarget(rHandControl, rHandState);
        UpdatePullTarget(lKneeControl, lKneeState);
        UpdatePullTarget(rKneeControl, rKneeState);
        UpdatePullTarget(lFootControl, lFootState);
        UpdatePullTarget(rFootControl, rFootState);
        UpdatePullTarget(headControl, headState);
        UpdatePullTarget(hipControl, hipState);
    }

    private void Update()
    {
        if (enableHand == null || !enableHand.val)
        {
            if (wasEnabled)
            {
                ReleaseAllTargets();
                HideClothGrab("LEFT");
                HideClothGrab("RIGHT");
                DetachHandAtomFromCameraRoot(leftHandAtom);
                DetachHandAtomFromCameraRoot(rightHandAtom);
                ResetHandXRotationState();
                wasEnabled = false;
            }
            return;
        }

        if (!wasEnabled)
        {
            RefreshHandAtoms();
            RefreshBodyControls();
            ResetDepthCenter();
            wasEnabled = true;
        }

        string leftPacket;
        string rightPacket;

        lock (packetLock)
        {
            leftPacket = pendingLeftPacket;
            rightPacket = pendingRightPacket;
            pendingLeftPacket = "";
            pendingRightPacket = "";
        }

        ProcessHandPacket(leftPacket);
        ProcessHandPacket(rightPacket);

        float t =
            handSmoothness.val <= 0.001f
                ? 1f
                : 1.0f - Mathf.Exp(-Time.deltaTime / Mathf.Max(0.001f, handSmoothness.val));

        currentLeft = Vector3.Lerp(currentLeft, targetLeft, t);
        currentRight = Vector3.Lerp(currentRight, targetRight, t);

        ApplyHandAtom(leftHandAtom, currentLeft, targetLeftXRotation, "LEFT", leftHandBaseRotation);
        ApplyHandAtom(rightHandAtom, currentRight, targetRightXRotation, "RIGHT", rightHandBaseRotation);

        BeginGrabActionFrame();
        UpdateBodyPull();
        FinishGrabActionFrame();
        UpdateClothGrab();
    }

    private void OnDestroy()
    {
        ReleaseAllTargets();
        HideClothGrab("LEFT");
        HideClothGrab("RIGHT");
        DetachHandAtomFromCameraRoot(leftHandAtom);
        DetachHandAtomFromCameraRoot(rightHandAtom);

        if (udp != null)
        {
            udp.Close();
            udp = null;
        }
    }
}
