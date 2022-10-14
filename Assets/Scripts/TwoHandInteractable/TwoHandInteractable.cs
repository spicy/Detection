using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// <summary>
/// 
/// This class requires that you have a PrimaryHold and SecondaryHold on the object as children in order to function
/// The Primary and Secondary holds must have colliders set to isTrigger = true
/// You must set the Interaction Layer Mask to Nothing on the main object or else it will not work properly
/// You must also have GrabPistolHandPose on the main object
///
/// </summary>

public class TwoHandInteractable : XRGrabInteractable
{
    private PrimaryHold pHold = null;
    protected IXRInteractor PrimaryInteractor { get; private set; } = null;
    private SecondaryHold sHold = null;
    protected IXRInteractor SecondaryInteractor { get; private set; } = null;
    private bool isHoldingWithBothHands = false;
    private Quaternion initalAttachRotation;
    private GrabPistolHandPose handPoseInstance;
    public enum ZAxisRotationType { None, First, Second }
    public ZAxisRotationType rotationType;
    
    private Transform primaryHandModel;
    private Quaternion initialPrimaryHandRot;
    private Transform secondaryHandModel;
    private Quaternion initialSecondaryHandRot;

    protected override void Awake()
    {
        base.Awake();
        SetupHolds();
        handPoseInstance = GetComponentInParent<GrabPistolHandPose>();
        selectEntered.AddListener(SetInitialRotation);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        selectEntered.RemoveListener(SetInitialRotation);
    }

    private void SetupHolds()
    {
        pHold = GetComponentInChildren<PrimaryHold>();
        pHold.Init(this);

        sHold = GetComponentInChildren<SecondaryHold>();
        sHold.Init(this);
        sHold.gameObject.SetActive(false);
    }

    private void SetInitialRotation(SelectEnterEventArgs args)
    {
        initalAttachRotation = PrimaryInteractor.GetAttachTransform(pHold.ObjectHeld).localRotation;
    }

    public void SetPrimaryHand(SelectEnterEventArgs args)
    {
        PrimaryInteractor = args.interactorObject;
        initalAttachRotation = pHold.ObjectHeld.transform.localRotation;
        ManualSelect(args);

        // Set primary hand pose
        PrimaryInteractor.transform.GetComponentInChildren<HandBoneData>().poseType = HandBoneData.HandModelPose.Primary;
        handPoseInstance.SetupPose(pHold.ObjectHeld, PrimaryInteractor);

        // Parent the hand model to the Hold to maintain position on object
        primaryHandModel = PrimaryInteractor.transform.Find("Hand Model");
        primaryHandModel.transform.parent = pHold.transform;
        initialPrimaryHandRot = primaryHandModel.transform.rotation;

        // enable second grab point
        sHold.gameObject.SetActive(true);
    }

    public void ClearPrimaryHand(SelectExitEventArgs args)
    {
        // Reset the hand model parent and rotation
        primaryHandModel.transform.parent = PrimaryInteractor.transform;
        primaryHandModel.transform.rotation = initialPrimaryHandRot;

        // Clear primary hand pose
        handPoseInstance.UnsetPose(pHold.ObjectHeld, PrimaryInteractor);
        ManualDeSelect(args);

        // Reset parent of second hand model if both holding
        if (isHoldingWithBothHands)
        {
            secondaryHandModel.transform.parent = SecondaryInteractor.transform;
            secondaryHandModel.transform.rotation = initialSecondaryHandRot;
        }

        sHold.gameObject.SetActive(false);
        isHoldingWithBothHands = false;
        PrimaryInteractor = null;
    }

    public void SetSecondaryHand(SelectEnterEventArgs args)
    {
        SecondaryInteractor = args.interactorObject;
        isHoldingWithBothHands = true;

        // Set secondary hand pose
        SecondaryInteractor.transform.GetComponentInChildren<HandBoneData>().poseType = HandBoneData.HandModelPose.Secondary;
        handPoseInstance.SetupPose(sHold.ObjectHeld, SecondaryInteractor);

        // Set the hand model as child to the Hold to maintain position on object
        secondaryHandModel = SecondaryInteractor.transform.Find("Hand Model");
        secondaryHandModel.transform.parent = sHold.transform;
        initialSecondaryHandRot = secondaryHandModel.transform.rotation;
    }

    public void ClearSecondaryHand(SelectExitEventArgs args)
    {
        // Reset the hand model parent and rotation
        secondaryHandModel.transform.parent = SecondaryInteractor.transform;
        secondaryHandModel.transform.rotation = initialSecondaryHandRot;

        // Clear secondary hand pose
        handPoseInstance.UnsetPose(sHold.ObjectHeld, SecondaryInteractor);

        // Reset the rotation of the gun to initial state
        PrimaryInteractor.GetAttachTransform(pHold.ObjectHeld).localRotation = initalAttachRotation;


        SecondaryInteractor = null;
        isHoldingWithBothHands = false;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if(isHoldingWithBothHands)
        {
            PrimaryInteractor.GetAttachTransform(pHold.ObjectHeld).rotation = GetRotation();
        }
            
    }

    private Quaternion GetRotation()
    {
        return rotationType switch
        {
            // No rotation about the forward axis
            ZAxisRotationType.None => Quaternion.LookRotation(SecondaryInteractor.GetAttachTransform(sHold.ObjectHeld).position - PrimaryInteractor.GetAttachTransform(pHold.ObjectHeld).position),

            // Primary hand determines rotation about forward axis
            ZAxisRotationType.First => Quaternion.LookRotation(SecondaryInteractor.GetAttachTransform(sHold.ObjectHeld).position - PrimaryInteractor.GetAttachTransform(pHold.ObjectHeld).position, PrimaryInteractor.transform.up),

            // Secondary hand determines rotation about forward axis
            ZAxisRotationType.Second => Quaternion.LookRotation(SecondaryInteractor.GetAttachTransform(sHold.ObjectHeld).position - PrimaryInteractor.GetAttachTransform(pHold.ObjectHeld).position, SecondaryInteractor.transform.up),

            // default second hand determines rotation
            _ => Quaternion.LookRotation(SecondaryInteractor.GetAttachTransform(sHold.ObjectHeld).position - PrimaryInteractor.GetAttachTransform(pHold.ObjectHeld).position, SecondaryInteractor.transform.up),
        };
    }

    private void ManualSelect(SelectEnterEventArgs args)
    {
        OnSelectEntering(args);
        OnSelectEntered(args);
    }

    private void ManualDeSelect(SelectExitEventArgs args)
    {
        OnSelectExiting(args);
        OnSelectExited(args);
    }

    public virtual void StartObjectAction()
    {

    }

    public virtual void StopObjectAction()
    {

    }
}