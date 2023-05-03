using UnityEngine;

using Drop;


namespace Shooting.Weapon
{
    public class Weapon_Water : IWeapon
    {
        private const float ShootForce = 30;
        private readonly Rigidbody2D _projectilePrefab;

        private readonly Transform _shootingPointTransform;

        public Weapon_Water(Rigidbody2D prefab, Transform shootPointTransform)
        {
            _projectilePrefab = prefab;

            _shootingPointTransform = shootPointTransform;
        }

        public void Shoot(Vector2 mousePosition, OnRanOutOfAmmunition callback)
        {
            if (Drop_ConditionController.OnTryChangeWaterCount == null ||
                !Drop_ConditionController.OnTryChangeWaterCount.Invoke(-1))
                return;

            Vector2 modifiedPosition = new Vector2(mousePosition.x * Random.Range(0.95f, 1.05f), mousePosition.y * Random.Range(0.95f, 1.05f));
            Vector2 targetVector = (Camera.main.ScreenToWorldPoint(modifiedPosition) - _shootingPointTransform.position);

            Rigidbody2D projectile = Object.Instantiate(_projectilePrefab, _shootingPointTransform.position, new Quaternion(0, 0, 0, 0));

            float forceModifier = Random.Range(0.8f, 1.3f);
            projectile.AddForce(targetVector.normalized * ShootForce * forceModifier);
        }
    }
}