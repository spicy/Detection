using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Weapon : MonoBehaviour
{
    [SerializeField] protected GunData gunData;
    protected XRGrabInteractable weapon;

    private void Awake()
    {
        weapon = GetComponent<XRGrabInteractable>();
        SetupInteractions();
    }

    protected void SetupInteractions()
    {
        weapon.activated.AddListener(StartAttacking);
        weapon.deactivated.AddListener(StopAttacking);
    }

    protected virtual void StartAttacking(ActivateEventArgs args)
    {

    }

    protected virtual void StopAttacking(DeactivateEventArgs args)
    {

    }
}
