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
    private HandBoneData primaryHand;
    private SecondaryHold sHold = null;
    protected IXRInteractor SecondaryInteractor { get; private set; } = null;
    private HandBoneData secondaryHand;

    private bool isHoldingWithBothHands = false;
    private Quaternion initalAttachRotation;
    private GrabPistolHandPose handPoseInstance;
    public enum ZAxisRotationType { None, First, Second }
    public ZAxisRotationType rotationType;
    
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

        // Get Hand Model and HandBoneData
        primaryHand = PrimaryInteractor.transform.GetComponentInChildren<HandBoneData>();
        primaryHand.poseType = HandBoneData.HandModelPose.Primary;
        handPoseInstance.SetupPose(pHold.ObjectHeld, PrimaryInteractor);

        // Parent the hand model to hold to maintain position
        primaryHand.transform.parent = pHold.transform;
        
        // enable second grab point
        sHold.gameObject.SetActive(true);
    }

    public void ClearPrimaryHand(SelectExitEventArgs args)
    {
        // Reset primary hand parent
        primaryHand.transform.parent = PrimaryInteractor.transform;

        // Clear primary hand pose
        handPoseInstance.UnsetPose(pHold.ObjectHeld, PrimaryInteractor);

        ManualDeSelect(args);

        // Reset parent of second hand if both hands were holding
        if (isHoldingWithBothHands)
        {
            secondaryHand.transform.parent = SecondaryInteractor.transform;
            handPoseInstance.UnsetPose(pHold.ObjectHeld, SecondaryInteractor);
        }

        sHold.gameObject.SetActive(false);
        isHoldingWithBothHands = false;
        PrimaryInteractor = null;
    }

    public void SetSecondaryHand(SelectEnterEventArgs args)
    {
        SecondaryInteractor = args.interactorObject;
        isHoldingWithBothHands = true;

        // Get secondary hand and set hand pose
        secondaryHand = SecondaryInteractor.transform.GetComponentInChildren<HandBoneData>();
        secondaryHand.poseType = HandBoneData.HandModelPose.Secondary;
        handPoseInstance.SetupPose(sHold.ObjectHeld, SecondaryInteractor);

        // Parent secondHand to hold to maintain position
        secondaryHand.transform.parent = sHold.transform;
    }

    public void ClearSecondaryHand(SelectExitEventArgs args)
    {
        // Reset the hand parent
        secondaryHand.transform.parent = SecondaryInteractor.transform;

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
            PrimaryInteractor.GetAttachTransform(pHold.ObjectHeld).rotation = GetRotation();

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