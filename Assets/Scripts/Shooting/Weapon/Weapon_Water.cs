using UnityEngine;

using Drop;
using Items;


namespace Shooting.Weapon
{
    public class Weapon_Water : IWeapon
    {
        private const float ShootForce = 300;
        private readonly Rigidbody2D _projectilePrefab;

        private readonly Transform _shootingPointTransform;
        private readonly Transform _dropTransform;
        private readonly Drop_ConditionController _dropCondition;

        public Weapon_Water(Rigidbody2D prefab, Transform shootPointTransform, Drop_ConditionController dropCondition)
        {
            _projectilePrefab = prefab;

            _shootingPointTransform = shootPointTransform;
            _dropTransform = dropCondition.transform;
            _dropCondition = dropCondition;
        }

        public void Shoot(Vector2 mousePosition, OnRanOutOfAmmunition callback)
        {
            if (!_dropCondition.TryChangeWaterCount(-1))
                return;

            Vector2 modifiedPosition = new Vector2(mousePosition.x * Random.Range(0.95f, 1.05f), mousePosition.y * Random.Range(0.95f, 1.05f));
            Vector2 targetVector = (Camera.main.ScreenToWorldPoint(modifiedPosition) - _shootingPointTransform.position);

            Rigidbody2D projectile = Object.Instantiate(_projectilePrefab, _shootingPointTransform.position, new Quaternion(0, 0, 0, 0));
            projectile.GetComponent<DropOfWater_Controller>().Shoot();

            projectile.transform.parent = _dropTransform;

            float forceModifier = Random.Range(0.8f, 1.3f);
            projectile.AddForce(targetVector.normalized * ShootForce * forceModifier);
        }
    }
}