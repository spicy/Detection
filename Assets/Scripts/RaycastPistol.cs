using UnityEngine;
using Detection;

public class RaycastPistol : TwoHandInteractable, IShootable, IShootsParticle
{
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
        Shoot();
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

    // Recoil does not work
    //public void Recoil()
    //{
    //    rigidBody.AddRelativeForce(Vector3.back * gunData.recoilForce, ForceMode.Impulse);
    //    rigidBody.AddRelativeTorque(Vector3.left * gunData.recoilForce, ForceMode.Impulse);
    //}

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
