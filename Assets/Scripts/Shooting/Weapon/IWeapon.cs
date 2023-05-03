using UnityEngine;


namespace Shooting.Weapon
{
    public delegate void OnRanOutOfAmmunition();

    public interface IWeapon
    {
        public void Shoot(Vector2 mousePosition, OnRanOutOfAmmunition callback);
    }
}