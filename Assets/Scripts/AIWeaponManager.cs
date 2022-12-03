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
    //private GameObject weaponPrefab;
    //private GameObject weaponToDrop;
    //WeaponInverseKinematics weaponInverse;

    //public Transform target;

    public void Awake()
    {
        dealsDamage = GetComponentInChildren<IDealsDamage>();
        aiSpecificBehavior = GetComponentInChildren<IHasAIBehavior>();
        //weaponPrefab = GetWeaponPrefab();
        // instantiate with a 2f up offset so the new weapon does not instantiate under the floor
        //weaponToDrop = Instantiate(weaponPrefab, transform.position + Vector3.up * 2f, transform.rotation, transform);
        //weaponToDrop.SetActive(false);
        //weaponInverse = GetComponent<WeaponInverseKinematics>();
        //weaponInverse.SetTargetTransform(target);
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

    private GameObject GetWeaponPrefab()
    {
        Weapons weapon = dealsDamage.GetWeaponEnum();

        switch (weapon)
        {
            case Weapons.Rifle:
                return GetComponentInChildren<RaycastRifle>().gameObject;
            case Weapons.Pistol:
                return GetComponentInChildren<RaycastPistol>().gameObject;
            case Weapons.Shotgun:
                return GetComponentInChildren<DoubleBarrelShotgun>().gameObject;
            case Weapons.Grenade:
                return GetComponentInChildren<Grenade>().gameObject;
            case Weapons.Knife:
                break;
        }
        return null;
    }
    /*
    public void LaunchWeapon()
    {
        weaponToDrop.transform.SetParent(null);
        weaponToDrop.SetActive(true);
        weaponToDrop.AddComponent<Outline>();

        float launchAngle = 45f;

        // launch direction
        Vector3 dirToLaunch = -(weaponToDrop.transform.position - weaponInverse.TargetTransform.position);

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
    }*/
}
