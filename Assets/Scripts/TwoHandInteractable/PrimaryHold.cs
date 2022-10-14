using UnityEngine.XR.Interaction.Toolkit;
public class PrimaryHold : BaseHold
{
    protected override void StartAction(ActivateEventArgs args)
    {
        ObjectHeld.StartObjectAction();
    }

    protected override void StopAction(DeactivateEventArgs args)
    {
        ObjectHeld.StopObjectAction();
    }

    protected override void Grab(SelectEnterEventArgs args)
    {
        base.Grab(args);
        ObjectHeld.SetPrimaryHand(args);
    }

    protected override void Drop(SelectExitEventArgs args)
    {
        base.Drop(args);
        ObjectHeld.ClearPrimaryHand(args);
    }
}
