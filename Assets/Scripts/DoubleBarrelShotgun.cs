using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using Detection;

public class DoubleBarrelShotgun : Weapon, IShootable, IShootsParticle
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private int pelletsPerShot = 7;
    [SerializeField] private float spread;
    private int currentAmmo;

    private void Start()
    {
        currentAmmo = gunData.startingAmmo;
    }

    protected override void StartAttacking(ActivateEventArgs args)
    {
        Shoot();
    }
    
    public void Shoot()
    {
        if(currentAmmo > 0)
        {
            for(int i = 0; i < pelletsPerShot; ++i)
            {
                Ray ray = new(bulletSpawn.position, GetPelletDirection());
                ShootAndEmitParticle(ray);
            }

            AudioManager.manager.Play("shotgun_shot");
            --currentAmmo;
        }
        else
        {
            AudioManager.manager.Play("gun_empty");
        }
    }

    private Vector3 GetPelletDirection()
    {
        Vector3 target = bulletSpawn.position + bulletSpawn.forward * gunData.range;
        target = new Vector3(
            target.x + Random.Range(-spread, spread),
            target.y + Random.Range(-spread, spread),
            target.z + Random.Range(-spread, spread)
        );

        Vector3 dir = target - bulletSpawn.position;
        return dir.normalized;
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
