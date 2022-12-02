using UnityEngine;
using Detection;
using static Detection.IDealsDamage;

public class RaycastPistol : TwoHandInteractable, IShootable, IShootsParticle, IDealsDamage
{
    [SerializeField] private Color bulletColor = new Color(240, 208, 81);
    [SerializeField] private float bulletLifetime = 0.5f;
    [SerializeField] private float bulletSize = 0.15f;

    [SerializeField] private GunData gunData;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private ParticleSystem _particleSystem;
    private int currentAmmo;

    private void Start()
    {
        currentAmmo = gunData.startingAmmo;
    }

    public override void StartObjectAction()
    {
        Attack();
    } 
    
    public void Attack()
    {
        Shoot();
    }

    public Weapons GetWeaponEnum()
    {
        return Weapons.Pistol;
    }

    public void Shoot()
    {
        if(currentAmmo > 0)
        {
            Ray ray = new(bulletSpawn.position, bulletSpawn.forward);
            ShootAndEmitParticle(ray);
            AudioManager.manager.Play("beretta_shot");
            --currentAmmo;
        }
        else
        {
            AudioManager.manager.Play("gun_empty");
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

            VFXEmitArgs overrideArgs = new VFXEmitArgs(bulletColor, bulletSize, bulletLifetime);
            scannableObject.EmitParticle(hit, overrideArgs);
        }
    }
}
