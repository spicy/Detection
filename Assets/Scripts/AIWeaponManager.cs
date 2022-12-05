using Detection;
using static Detection.IDealsDamage;
using UnityEngine;

public class AIWeaponManager : MonoBehaviour
{
    [System.Serializable]
    public struct NecessaryUseConditions
    {
        public float minRange;
        public float maxRange;
        public float idealRange;
    }

    [SerializeField] private NecessaryUseConditions currentWeaponNecessaryUseConditions;
    private IDealsDamage dealsDamage;
    private IHasAIBehavior aiSpecificBehavior;
    private GameObject currentWeapon;
    private GameObject weaponToDrop;
    private WeaponInverseKinematics weaponInverse;
    private Vector3 weaponSpawnOffset = new(0, 2, 0);


    public void Awake()
    {
        dealsDamage = GetComponentInChildren<IDealsDamage>();
        aiSpecificBehavior = GetComponentInChildren<IHasAIBehavior>();
        weaponInverse = GetComponent<WeaponInverseKinematics>();
        SpawnWeaponToDrop();
    }

    public NecessaryUseConditions GetWeaponNecessaryUseConditions()
    {
        return currentWeaponNecessaryUseConditions;
    }

    public void DoAttack()
    {
        if (aiSpecificBehavior != null)
        {
            aiSpecificBehavior.DoAIBehavior();
        }   
        else
        {
            dealsDamage.Attack();
        }
    }

    private void SpawnWeaponToDrop()
    {
        Weapons weapon = dealsDamage.GetWeaponEnum();

        switch (weapon)
        {
            case Weapons.Rifle:
                currentWeapon = GetComponentInChildren<RaycastRifle>().gameObject;
                weaponToDrop = Instantiate(Resources.Load<GameObject>("Weapons/Rifle"), transform.position + weaponSpawnOffset, transform.rotation, transform);
                break;
            case Weapons.Pistol:
                currentWeapon = GetComponentInChildren<RaycastPistol>().gameObject;
                weaponToDrop = Instantiate(Resources.Load<GameObject>("Weapons/Pistol"), transform.position + weaponSpawnOffset, transform.rotation, transform);
                break;
            case Weapons.Shotgun:
                currentWeapon = GetComponentInChildren<DoubleBarrelShotgun>().gameObject;
                weaponToDrop = Instantiate(Resources.Load<GameObject>("Weapons/Sawed-Off-Shotgun"), transform.position + weaponSpawnOffset, transform.rotation, transform);
                break;
            case Weapons.Grenade:
                currentWeapon = GetComponentInChildren<Grenade>().gameObject;
                weaponToDrop = Instantiate(Resources.Load<GameObject>("Weapons/Grenade"), transform.position + weaponSpawnOffset, transform.rotation, transform);
                break;
            case Weapons.Knife:
                break;
        }

        weaponToDrop.SetActive(false);
    }

    public void LaunchWeapon()
    {
        currentWeapon.SetActive(false);
        weaponToDrop.transform.SetParent(null);
        weaponToDrop.SetActive(true);

        //  AI was not targeting anything --> drop weapon on floor
        if (weaponInverse.TargetTransform == null) return;

        float launchAngle = 45f;

        // launch direction
        Vector3 dirToLaunch = (weaponInverse.TargetTransform.position - weaponToDrop.transform.position);

        float temp = dirToLaunch.y;
        dirToLaunch.y = 0;

        // XZ length and angle of throw to radians
        float length = dirToLaunch.magnitude;
        float theta = launchAngle * Mathf.Deg2Rad;

        // height of throw
        dirToLaunch.y = length * Mathf.Tan(theta);
        length += temp / Mathf.Tan(theta);

        // force required to launch at distance
        float throwForce = Mathf.Sqrt(length * Physics.gravity.magnitude / Mathf.Sin(2 * theta));
        throwForce /= 1.65f;
        Vector3 velocity = throwForce * dirToLaunch.normalized;

        // Addforce to weapon and launch
        weaponToDrop.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
    }
}
