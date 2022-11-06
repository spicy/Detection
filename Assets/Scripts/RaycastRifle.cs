using System.Collections;
using UnityEngine;
using Detection;

public class RaycastRifle : TwoHandInteractable, IShootable, IShootsParticle
{
    [SerializeField] protected GunData gunData;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private ParticleSystem _particleSystem;
    private int currentAmmo;
    private WaitForSeconds waitTime;

    private void Start()
    {
        waitTime = new WaitForSeconds(1f / gunData.fireRate);
        currentAmmo = gunData.startingAmmo;
    }

    public override void StartObjectAction()
    {
        StartCoroutine(ShootingRoutine());
    }

    public override void StopObjectAction()
    {
        StopAllCoroutines();
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            Ray ray = new(bulletSpawn.position, bulletSpawn.forward);
            ShootAndEmitParticle(ray);
            AudioManager.manager.Play("ak47_shot");
            --currentAmmo;
        }
        else
        {
            AudioManager.manager.Play("gun_empty");
        }
    }

    private IEnumerator ShootingRoutine()
    {
        while (true)
        {
            if (PrimaryInteractor == null) yield break;

            Shoot();
            yield return waitTime;
            
        }
    }

    // Recoil does not work
    //public void Recoil()
    //{
    //    rigidBody.AddRelativeForce(Vector3.back * gunData.recoilForce, ForceMode.Impulse);
    //    rigidBody.AddRelativeTorque(Vector3.left * gunData.recoilForce, ForceMode.Impulse);
    //}

    public void ShootAndEmitParticle(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, gunData.range))
        {
            ITakeDamage damageTaker = hit.collider.GetComponent<ITakeDamage>();
            if (damageTaker != null)
            {
                damageTaker.TakeDamage(gunData.damage);
            }

            var scannableObject = hit.collider.GetComponent<IScannable>();
            if (scannableObject == null) return;

            scannableObject.EmitParticle(hit, _particleSystem);
        }
    }
}