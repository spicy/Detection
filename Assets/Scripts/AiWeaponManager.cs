using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AiWeaponManager : MonoBehaviour
{
    public enum WeaponType { Grenade, DBShotgun, Rifle, Pistol };
    public WeaponType weaponType;
    public List<GameObject> weaponPrefabs = new List<GameObject>();
    public List<Transform> weaponOffsets = new List<Transform>();
    private XRGrabInteractable weapon;
    WeaponIK weaponIk;
    RaycastRifle rifle;

    private void Awake()
    {
        weaponIk = GetComponent<WeaponIK>();
        SpawnWeapon();
        rifle = weapon.GetComponent<RaycastRifle>();
        weaponIk.aimTransform = rifle.bulletSpawn;
    }

    private void SpawnWeapon()
    {
        weapon = Instantiate(weaponPrefabs[(int)weaponType], weaponOffsets[(int)weaponType], false).GetComponent<XRGrabInteractable>();
        weapon.GetComponent<Rigidbody>().isKinematic = true;
    }
}
