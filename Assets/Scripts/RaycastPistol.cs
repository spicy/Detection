using System.Collections;
using UnityEngine;
using Detection;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastPistol : Weapon, IShootable, IShootsParticle
{
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private ParticleSystem _particleSystem;
    private WaitForSeconds waitTime;

    private void Start()
    {
        waitTime = new WaitForSeconds(1f / gunData.fireRate); 
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
        //--gunData.currentAmmo;
    }

    // Recoil does not work
    public void Recoil()
    {
        rigidBody.AddRelativeForce(Vector3.back * gunData.recoilForce, ForceMode.Impulse);
        rigidBody.AddRelativeTorque(Vector3.left * gunData.recoilForce, ForceMode.Impulse);
    }
   
    private IEnumerator ShootingRoutine()
    {
        while(true)
        { 
            if(gunData.currentAmmo > 0)
            {
                Shoot();
                // Recoil();
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
