using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class BaseHold : XRBaseInteractable
{
    public TwoHandInteractable ObjectHeld { get; private set; } = null;

    public void Init(TwoHandInteractable obj)
    {
        ObjectHeld = obj;
    }

    protected override void Awake()
    {
        base.Awake();
        activated.AddListener(StartAction);
        deactivated.AddListener(StopAction);
        selectEntered.AddListener(Grab);
        selectExited.AddListener(Drop);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        activated.RemoveListener(StartAction);
        deactivated.RemoveListener(StopAction);
        selectEntered.RemoveListener(Grab);
        selectExited.RemoveListener(Drop);
    }
    protected virtual void Grab(SelectEnterEventArgs args)
    {

    }

    protected virtual void Drop(SelectExitEventArgs args)
    {
        
    }

    protected virtual void StartAction(ActivateEventArgs args)
    {
        
    }

    protected virtual void StopAction(DeactivateEventArgs args)
    {

    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isGrabbed = firstInteractorSelecting != null && !interactor.Equals(firstInteractorSelecting);
        return base.IsSelectableBy(interactor) && !isGrabbed;
    }
}
