using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using Detection;
using static Detection.IDealsDamage;

public class DoubleBarrelShotgun : Weapon, IShootable, IShootsParticle, IDealsDamage
{
    [SerializeField] private Color bulletColor = new Color(240, 208, 81);
    [SerializeField] private float bulletLifetime = 0.5f;
    [SerializeField] private float bulletSize = 0.2f;

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
        Attack();
    }

    public void Attack()
    {
        Shoot();
    }

    public Weapons GetWeaponEnum()
    {
        return Weapons.Shotgun;
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

            VFXEmitArgs overrideArgs = new VFXEmitArgs(bulletColor, bulletSize, bulletLifetime);
            scannableObject.EmitParticle(hit, overrideArgs);
        }
    }
}
