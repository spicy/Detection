using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;


[CanSelectMultiple(true)]
public class TwoHandInteractable : XRGrabInteractable
{
    [SerializeField] private Transform secondGrabPoint;
    [SerializeField] private bool twoHand;
    

    protected override void Awake()
    {
        base.Awake();
        if(!twoHand)
            selectMode = InteractableSelectMode.Single;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(interactorsSelecting.Count == 1)
        {
            isTwoHanded = false;
            base.ProcessInteractable(updatePhase);
        }
        else if(interactorsSelecting.Count == 2 && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            isTwoHanded = true;
            HandleDoubleGrip();
        }
    }

    protected override void Grab()
    {
        if(interactorsSelecting.Count == 1)
        {
            base.Grab();
        }
    }

    protected override void Drop()
    {
        if(!isSelected)
        {
            base.Drop();
        }
    }

    private void HandleDoubleGrip()
    {
        Transform firstAttach = GetAttachTransform(null);
        Transform firstHand = interactorsSelecting[0].transform;
        Transform secondAttach = secondGrabPoint;
        Transform secondHand = interactorsSelecting[1].transform;

        Vector3 handDir = secondHand.position - firstHand.position;

        Quaternion targetRotation = Quaternion.LookRotation(handDir, firstHand.up);

        Vector3 HandBaseWorldDir = transform.position - firstAttach.position;
        Vector3 HandBaseLocalDir = transform.InverseTransformDirection(HandBaseWorldDir);

        Vector3 targetPosition = firstHand.position + targetRotation * HandBaseLocalDir;

        transform.SetPositionAndRotation(targetPosition, targetRotation);
    }
}
