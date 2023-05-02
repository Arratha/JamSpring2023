using UnityEngine;


namespace Drop.Shooting.Weapon
{
    public class Weapon_Water : IWeapon
    {
        private const float ShootForce = 30;
        private readonly Rigidbody2D _projectilePrefab;

        private Transform _shootPointTransform;

        public Weapon_Water(Rigidbody2D prefab, Transform shootPointTransform)
        {
            _projectilePrefab = prefab;

            _shootPointTransform = shootPointTransform;
        }

        public void Shoot(Vector2 target, OnRanOutOfAmmunition callback)
        {
            if (Drop_ConditionController.OnTryChangeWaterCount == null ||
                !Drop_ConditionController.OnTryChangeWaterCount.Invoke(-1))
                return;

            Rigidbody2D projectile = GameObject.Instantiate(_projectilePrefab, _shootPointTransform.position, new Quaternion(0, 0, 0, 0));

            float forceModifier = Random.Range(0.8f, 1.3f);
            projectile.AddForce(target.normalized * ShootForce * forceModifier);
        }
    }
}