using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

[CanSelectMultiple(true)]
public class TwoHandInteractable : XRGrabInteractable
{
    [SerializeField] private Transform secondGrabPoint;
    [SerializeField] private bool twoHandable;
    [SerializeField] private bool secondHandDeterminesRotation;
    private bool isHoldingWithBothHands = false;

    protected override void Awake()
    {
        base.Awake();
        if(!twoHandable)
            selectMode = InteractableSelectMode.Single;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (isHoldingWithBothHands && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (secondHandDeterminesRotation) HandleDoubleGripRotation();
        }
        else
        {
            base.ProcessInteractable(updatePhase);
        }
    }

    protected override void Grab()
    {
        IXRInteractable gun = (IXRInteractable)interactorsSelecting[0].interactablesSelected[0];
        IXRInteractor hand = (IXRInteractor)interactorsSelecting[0];

        if (interactorsSelecting.Count == 2)
        {
            //set the hand we just grabbed with to secondary
            interactorsSelecting[1].transform.GetComponentInChildren<HandBoneData>().poseType = HandBoneData.HandModelPose.Secondary;
            isHoldingWithBothHands = true;
        }
        else
        {
            //set the hand we just grabbed with to primary
            interactorsSelecting[0].transform.GetComponentInChildren<HandBoneData>().poseType = HandBoneData.HandModelPose.Primary;
            base.Grab();
        }

        GetComponentInParent<GrabPistolHandPose>().SetupPose(gun, hand);
    }

    protected override void Drop()
    {
        isHoldingWithBothHands = false;

        if (interactorsSelecting.Count == 1)
        {
            IXRInteractable gun = (IXRInteractable)interactorsSelecting[0].interactablesSelected[0];
            IXRInteractor hand = (IXRInteractor)interactorsSelecting[0];

            // set the now only hand holding the gun to primary and reset the hand animation
            hand.transform.GetComponentInChildren<HandBoneData>().poseType = HandBoneData.HandModelPose.Primary;
            GetComponentInParent<GrabPistolHandPose>().SetupPose(gun, hand);
        }

        if (!isSelected)
        {
            base.Drop();
        }
    }

    private void HandleDoubleGripRotation()
    {
        Transform firstAttach = GetAttachTransform(null);
        Transform firstHand = interactorsSelecting[0].transform;
        Transform secondAttach = secondGrabPoint;
        Transform secondHand = interactorsSelecting[1].transform;

        Vector3 handDir = secondHand.position - firstHand.position;

        Quaternion targetRotation = Quaternion.LookRotation(handDir, firstHand.up);

        Vector3 handBaseWorldDir = transform.position - firstAttach.position;
        Vector3 handBaseLocalDir = transform.InverseTransformDirection(handBaseWorldDir);

        Vector3 targetPosition = firstHand.position + targetRotation * handBaseLocalDir;

        transform.SetPositionAndRotation(targetPosition, targetRotation);
    }
}
