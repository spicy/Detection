using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class GrenadeRing : XRGrabInteractable
{

    [HideInInspector] public bool isConnected = true;

    protected override void Awake()
    {
        base.Awake();
        selectEntered.AddListener(PullPin);
    }

    private void PullPin(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRSocketInteractor) return;

        isConnected = false;
        AudioManager.manager.Play("grenade_pin");
    }
}