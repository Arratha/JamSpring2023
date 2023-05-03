using UnityEngine;

using Items;


namespace Shooting.Weapon
{
    public class Weapon_Knife : IWeapon
    {
        private const float ShootForce = 1000;

        private readonly Knife_Controller _knifeController;

        public Weapon_Knife(Knife_Controller knife)
        {
            _knifeController = knife;
        }

        public void Shoot(Vector2 mousePosition, OnRanOutOfAmmunition callback)
        {
            _knifeController.Shoot();

            Rigidbody2D projectile = _knifeController.GetComponent<Rigidbody2D>();

            Vector2 targetVector = Camera.main.ScreenToWorldPoint(mousePosition) - _knifeController.transform.position;
            projectile.AddForce(targetVector.normalized * ShootForce);

            projectile.AddTorque(-500);

            callback.Invoke();
        }
    }
}