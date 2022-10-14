using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Weapon : TwoHandInteractable
{
    [SerializeField] protected GunData gunData;
    protected XRGrabInteractable weapon;
    protected Rigidbody rigidBody;

    protected override void Awake()
    {
        base.Awake();
        weapon = GetComponent<XRGrabInteractable>();
        rigidBody = GetComponent<Rigidbody>();
        SetupInteractions();
    }

    protected virtual void SetupInteractions()
    {
        weapon.activated.AddListener(StartAttacking);
        weapon.deactivated.AddListener(StopAttacking);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        weapon.activated.RemoveListener(StartAttacking);
        weapon.deactivated.RemoveListener(StopAttacking);
    }

    protected virtual void StartAttacking(ActivateEventArgs args)
    {

    }

    protected virtual void StopAttacking(DeactivateEventArgs args)
    {

    }
}
