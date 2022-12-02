using UnityEngine;
namespace Detection
{
    public interface IDealsDamage
    {
        public enum Weapons
        {
            Knife,
            Pistol,
            Shotgun,
            Rifle,
            Grenade
        }

        public void Attack();

        public Weapons GetWeaponEnum();
    }
}

