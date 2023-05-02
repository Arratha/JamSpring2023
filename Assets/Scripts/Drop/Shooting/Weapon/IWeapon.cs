using UnityEngine;


namespace Drop.Shooting.Weapon
{
    public delegate void OnRanOutOfAmmunition();

    public interface IWeapon
    {
        public void Shoot(Vector2 target, OnRanOutOfAmmunition callback);
    }
}