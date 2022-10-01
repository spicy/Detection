using System.Collections;
using UnityEngine;
using Detection;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


public class RaycastPistol : Weapon, IShootable, IShootsParticle
{
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private XRBaseInteractor socketInteractor;
    private WaitForSeconds waitTime;
    private GunMagazine magazine;
    protected XRIDefaultInputActions playerControls;
    protected InputAction ejectMag;


    protected override void Awake()
    {
        base.Awake();
        playerControls = new XRIDefaultInputActions();
    }
    private void Start()
    {
        waitTime = new WaitForSeconds(1f / gunData.fireRate);
        magazine = null;
        socketInteractor.selectEntered.AddListener(AttachMagazine);
        socketInteractor.selectExited.AddListener(RemoveMagazine);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        ejectMag = playerControls.XRIRightHandInteraction.Eject;
        ejectMag.Enable();
        ejectMag.performed += EjectMag;
    }

    private void EjectMag(InputAction.CallbackContext context)
    {
        if(interactorsSelecting.Count != 0)
        {
            socketInteractor.allowSelect = false;
            Invoke("EnableSocket", 1f);
        }
    }

    private void EnableSocket()
    {
        socketInteractor.allowSelect = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ejectMag.Disable();
    }

    private void AttachMagazine(SelectEnterEventArgs args)
    {
        magazine = args.interactableObject.transform.GetComponent<GunMagazine>();
    }

    private void RemoveMagazine(SelectExitEventArgs args)
    {
        magazine = null;
    }

    protected override void StartAttacking(ActivateEventArgs args)
    {
        if(interactorsSelecting[0] == args.interactorObject)
        {
            StartCoroutine(ShootingRoutine());
        }
    }


    protected override void StopAttacking(DeactivateEventArgs args)
    {
        if(interactorsSelecting[0] == args.interactorObject)
        {
            StopAllCoroutines();
        }
    }

    public void Shoot()
    {
        Ray ray = new(bulletSpawn.position, bulletSpawn.forward);
        ShootAndEmitParticle(ray);
        FindObjectOfType<AudioManager>().Play("beretta_shot");
        --magazine.bullets;
    }

    public void Recoil()
    {
        rigidBody.AddRelativeForce(Vector3.back * gunData.recoilForce, ForceMode.Impulse);
        rigidBody.AddRelativeTorque(Vector3.left * gunData.recoilForce, ForceMode.Impulse);
    }
   
    private IEnumerator ShootingRoutine()
    {
        while(true)
        { 
            if(magazine != null && magazine.bullets > 0)
            {
                Shoot();
                Recoil();
            }
            yield return waitTime;
        }
    }

    public void ShootAndEmitParticle(Ray ray)
    {
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, gunData.range))
        {
            ITakeDamage damageTaker = hit.collider.GetComponent<ITakeDamage>();
            if(damageTaker != null)
            {
                damageTaker.TakeDamage(gunData.damage);
            }

            var scannableObject = hit.collider.GetComponent<IScannable>();
            if (scannableObject == null) return;

            scannableObject.EmitParticle(hit.point, _particleSystem);
        }
    }
}
