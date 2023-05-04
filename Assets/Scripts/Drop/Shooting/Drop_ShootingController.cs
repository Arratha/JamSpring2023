using System;
using System.Collections.Generic;

using UnityEngine;

using Shooting.Weapon;


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

            Drop_ConditionController dropCondition = GetComponentInParent<Drop_Controller>().DropCondition;
            AddWeapon(new Weapon_Water(_baseProjcetilePrefab, _shootingPointTransform, dropCondition));
        }

        public void Shoot(Vector2 mousePosition)
        {
            _currentWeapon[0].Shoot(mousePosition, RemoveWeapon);
        }

        public void AddWeapon(IWeapon weapon)
        {
            _currentWeapon.Insert(0, weapon);
        }

        private void RemoveWeapon()
        {
            _currentWeapon[0] = null;
            _currentWeapon.RemoveAt(0);
        }
    }
}