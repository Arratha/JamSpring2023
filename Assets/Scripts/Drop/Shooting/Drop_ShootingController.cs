using System.Collections.Generic;

using UnityEngine;

using Drop.Shooting.Weapon;


namespace Drop.Shooting
{
    public class Drop_ShootingController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _baseProjcetilePrefab;

        private List<IWeapon> _currentWeapon = new List<IWeapon>();

        private Transform _shootingPointTransform;

        private void Awake()
        {
            _shootingPointTransform = transform;

            _currentWeapon.Add(new Weapon_Water(_baseProjcetilePrefab, _shootingPointTransform));
        }

        public void Shoot(Vector2 mousePosition)
        {
            Vector2 modifiedPosition = new Vector2(mousePosition.x * Random.Range(0.95f, 1.05f), mousePosition.y * Random.Range(0.95f, 1.05f));
            Vector2 targetVector = (Camera.main.ScreenToWorldPoint(modifiedPosition) - _shootingPointTransform.position).normalized;

            _currentWeapon[0].Shoot(targetVector, () => _currentWeapon.RemoveAt(0));
        }
    }
}