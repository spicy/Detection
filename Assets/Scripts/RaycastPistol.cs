using System.Collections;
using UnityEngine;
using Detection;
using UnityEngine.XR.Interaction.Toolkit;


public class RaycastPistol : Weapon, IShootable, IReloadable, IShootsParticle
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
        base.StartAttacking(args);
        StartCoroutine(ShootingRoutine());
    }


    protected override void StopAttacking(DeactivateEventArgs args)
    {
        base.StopAttacking(args);
        StopAllCoroutines();
    }

    public void Shoot()
    {
        if(gunData.currentAmmo > 0)
        {
            Ray ray = new(bulletSpawn.position, bulletSpawn.forward);
            ShootAndEmitParticle(ray);
            //Recoil();
        }
    }

    // TODO: implement recoil
    protected override void Recoil()
    {

    }

    // TODO: implement reloading
    public void Reload()
    {

    }
   
    private IEnumerator ShootingRoutine()
    {
        while(true)
        { 
            Shoot();
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

            // create a particle where the ray lands
            var scannableObject = hit.collider.GetComponent<IScannable>();
            if (scannableObject == null) return;
            // scannableObject.EmitParticle(hit.point, null);
            scannableObject.EmitParticle(hit.point, _particleSystem);
        }
    }
}
