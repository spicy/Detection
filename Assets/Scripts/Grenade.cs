using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;
using Detection;

public class Grenade : XRGrabInteractable
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explodeRadius;
    [SerializeField] private float damage;
    [SerializeField] private float grenadeTimer;
    private GrenadeRing grenadeRing;
    private Collider ringCollider;
    private bool exploded = false;

    protected override void Awake()
    {
        base.Awake();
        selectEntered.AddListener(EnableRing);
        selectExited.AddListener(DisableRing);
        grenadeRing = GetComponentInChildren<GrenadeRing>();
        ringCollider = grenadeRing.GetComponent<Collider>();

        // Ignore collision between ring and grenade
        Physics.IgnoreCollision(GetComponent<Collider>(), ringCollider);
        ringCollider.enabled = false;
    }

    private void EnableRing(SelectEnterEventArgs args)
    {
        // Allow the ring to be pulled after the grenade has been picked up
        ringCollider.enabled = true;
    }

    private void DisableRing(SelectExitEventArgs args)
    {
        if(grenadeRing.isConnected)
        {
            ringCollider.enabled = false;
        }
    }

    private void Update()
    {
        if(!grenadeRing.isConnected && !exploded)
        {
            exploded = true;
            StartCoroutine(TimerRoutine());
        }
    }

    private IEnumerator TimerRoutine()
    {
        yield return new WaitForSecondsRealtime(grenadeTimer);

        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider collider in colliders)
        {
            ITakeDamage damageTaker = collider.GetComponent<ITakeDamage>();
            if (damageTaker == null) continue;
            damageTaker.TakeDamage(damage);
        }

        Instantiate(explosionPrefab, transform.position, transform.rotation);
        AudioManager.manager.Play("small_explosion");

        Destroy(gameObject);
    }

    //public override bool IsSelectableBy(IXRSelectInteractor interactor)
    //{
    //    bool isGrabbed = firstInteractorSelecting != null && !interactor.Equals(firstInteractorSelecting);
    //    return base.IsSelectableBy(interactor) && !isGrabbed;
    //}
}
