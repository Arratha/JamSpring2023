using UnityEngine;

using Items;



namespace Shooting.Weapon
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        private readonly float ShootForce;
        private readonly float TorqueForce;

        private readonly bool IsRandom;

        private readonly PickableItem_Controller ItemController;

        public Weapon(PickableItem_Controller tadpole, float shootForce, float torqueForce = 0, bool isRandom = false)
        {
            ShootForce = shootForce;
            TorqueForce = torqueForce;

            IsRandom = isRandom;

            ItemController = tadpole;
        }

        public void Shoot(Vector2 mousePosition, OnRanOutOfAmmunition callback)
        {
            ItemController.Shoot();

            Rigidbody2D projectile = ItemController.GetComponent<Rigidbody2D>();

            Vector2 vectorModifier = (IsRandom) ? new Vector2(Random.Range(0.95f, 1.05f), Random.Range(0.95f, 1.05f)) : Vector2.one;

            Vector2 modifiedPosition = new Vector2(mousePosition.x * vectorModifier.x, mousePosition.y * vectorModifier.y);
            Vector2 targetVector = (Camera.main.ScreenToWorldPoint(modifiedPosition) - ItemController.transform.position);

            float shootForceModifier = (IsRandom) ? Random.Range(0.8f, 1.3f) : 1;
            float torqueForceModifier = (IsRandom) ? Random.Range(0.8f, 1.3f) : 1;

            projectile.AddForce(targetVector.normalized * ShootForce * shootForceModifier);
            projectile.AddTorque(TorqueForce * torqueForceModifier);

            callback.Invoke();
        }
    }
}