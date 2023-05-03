using UnityEngine;

using Items;


namespace Shooting.Weapon
{
    public class Weapon_Firefly : IWeapon
    {
        private const float ShootForce = 200;

        private readonly Firefly_Controller _fireflyController;

        public Weapon_Firefly(Firefly_Controller firefly)
        {
            _fireflyController = firefly;
        }

        public void Shoot(Vector2 mousePosition, OnRanOutOfAmmunition callback)
        {
            _fireflyController.Shoot();


            Rigidbody2D projectile = _fireflyController.GetComponent<Rigidbody2D>();

            Vector2 modifiedPosition = new Vector2(mousePosition.x * Random.Range(0.95f, 1.05f), mousePosition.y * Random.Range(0.95f, 1.05f));
            Vector2 targetVector = (Camera.main.ScreenToWorldPoint(modifiedPosition) - _fireflyController.transform.position);

            float forceModifier = Random.Range(0.8f, 1.3f);
            projectile.AddForce(targetVector.normalized * ShootForce * forceModifier);

            callback.Invoke();
        }
    }
}
