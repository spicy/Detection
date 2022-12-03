using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;
using Detection;
using static Detection.IDealsDamage;

public class Grenade : XRGrabInteractable, IHasAIBehavior, IDealsDamage
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explodeRadius;
    [SerializeField] private float damage;
    [SerializeField] private float grenadeTimer;
    private GrenadeRing grenadeRing;
    private Collider ringCollider;
    private bool exploded = false;
    private int count = 0;

    protected override void Awake()
    {
        base.Awake();
        selectEntered.AddListener(EnableRing);
        selectExited.AddListener(DisableRing);
        grenadeRing = GetComponentInChildren<GrenadeRing>();
        XRSocketInteractor socket = GetComponentInChildren<XRSocketInteractor>();
        grenadeRing.transform.SetParent(socket.transform);
        ringCollider = grenadeRing.GetComponent<Collider>();

        // Ignore collision between ring and grenade
        Physics.IgnoreCollision(GetComponent<Collider>(), ringCollider);
        ringCollider.enabled = false;
    }
    private void Update()
    {
        if (!grenadeRing.isConnected && !exploded)
        {
            exploded = true;
            StartCoroutine(TimerRoutine());
        }
    }

    private void EnableRing(SelectEnterEventArgs args)
    {
        // Allow the ring to be pulled after the grenade has been picked up
        ringCollider.enabled = true;
    }

    private void DisableRing(SelectExitEventArgs args)
    {
        if (grenadeRing.isConnected)
        {
            ringCollider.enabled = false;
        }
    }

    public void DoAIBehavior()
    {
        if(count == 0)
        {
            StartCoroutine(ThrowRoutine());
            ++count;
        }
    }

    // This function directly moves the transform of the grenade. The grenade stops when it reaches the target position
    IEnumerator ThrowRoutine()
    {
        GameObject throwGrenade = Instantiate(gameObject, transform.position, transform.rotation, null);
        Physics.IgnoreCollision(throwGrenade.GetComponent<Collider>(), transform.GetComponent<Collider>());
        GetComponentInChildren<MeshRenderer>().enabled = false;
        throwGrenade.GetComponentInChildren<GrenadeRing>().isConnected = false;

        float gravity = Physics.gravity.magnitude;
        WeaponInverseKinematics weaponIk = GetComponentInParent<WeaponInverseKinematics>();
        float throwAngle = 45f;

        // Projectile motion: simulating gravity, ignoring physics
        float distance = Vector3.Distance(throwGrenade.transform.position, weaponIk.TargetTransform.position);

        float velocity = distance / (Mathf.Sin(2 * throwAngle * Mathf.Deg2Rad) / gravity);

        float Vx = Mathf.Sqrt(velocity) * Mathf.Cos(throwAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(velocity) * Mathf.Sin(throwAngle * Mathf.Deg2Rad);

        // time to target
        float duration = distance / Vx;

        throwGrenade.transform.rotation = Quaternion.LookRotation(weaponIk.TargetTransform.position - throwGrenade.transform.position);

        // time movement of grenade
        float t = 0f;
        while (t < duration)
        {
            throwGrenade.transform.Translate(0, (Vy - (gravity * t)) * Time.deltaTime, Vx * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
    
    private IEnumerator TimerRoutine()
    {
        yield return new WaitForSecondsRealtime(grenadeTimer);

        Explode();
    }

    public void Attack()
    {
        StartCoroutine(TimerRoutine());
    }

    public Weapons GetWeaponEnum()
    {
        return Weapons.Grenade;
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

        Instantiate(explosionPrefab, transform.position, transform.rotation, transform);
        AudioManager.manager.Play("small_explosion");

        Destroy(gameObject);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isGrabbed = firstInteractorSelecting != null && !interactor.Equals(firstInteractorSelecting);
        return base.IsSelectableBy(interactor) && !isGrabbed;
    }
}
