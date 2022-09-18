using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;



public class Weapon : MonoBehaviour
{
    private XRGrabInteractable weapon;
    [SerializeField] protected GunData gunData;

    protected virtual void Awake()
    {
        weapon = GetComponent<XRGrabInteractable>();
        SetupInteractions();
    }

    protected virtual void SetupInteractions()
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

    protected virtual void Recoil()
    {

    }
    
}
