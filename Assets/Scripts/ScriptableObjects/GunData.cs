using UnityEngine;

[CreateAssetMenu(fileName="Gun", menuName="Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float range;
    public float fireRate;
    public float recoilForce;

    [Header("Reloading")]
    public int      startingAmmo;
    public int      maxAmmo;
}
