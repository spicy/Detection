using UnityEngine.XR.Interaction.Toolkit;

public class SecondaryHold : BaseHold
{
    protected override void Grab(SelectEnterEventArgs args)
    {
        base.Grab(args);
        ObjectHeld.SetSecondaryHand(args);
    }

    protected override void Drop(SelectExitEventArgs args)
    {
        base.Drop(args);
        ObjectHeld.ClearSecondaryHand(args);
    }
}