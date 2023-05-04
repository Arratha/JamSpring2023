using System.Collections.Generic;

using UnityEngine;

using Drop;

using Shooting;


namespace Items
{
    public class DropOfWater_Controller : PickableItem_Controller
    {
        private float _projectilestateDelay;
        private const float ProjectilestateMaxDelay = 0.5f;

        private bool _isInContacnt => _colliders.Count > 0;
        private List<Collider2D> _colliders = new List<Collider2D>();

        private float _dryDelay = DryMaxDelay;
        private const float DryMaxDelay = 5f;

        private Vector2? _baseScale;

        protected override ProjectileType _projectileType { get; } = ProjectileType.DropOfWater;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            DryOut();
        }

        private void DryOut()
        {
            if (_currentState != ItemState.Pickable)
                return;

            if (!_isInContacnt)
                return;

            if (_baseScale == null)
                _baseScale = transform.localScale;

            if (_dryDelay <= 0)
                Destroy(gameObject);

            transform.localScale = (Vector2)_baseScale * (0.5f + 0.5f * (_dryDelay / DryMaxDelay));

            _dryDelay -= Time.deltaTime;
        }

        public override void Shoot()
        {
            _currentState = ItemState.Item;
            _pickable.gameObject.SetActive(false);
            _projectilestateDelay = ProjectilestateMaxDelay;

            base.Shoot();
        }

        protected override void ChangeState(Drop_Controller controller = null)
        {
            switch (_currentState)
            {
                case ItemState.Pickable:
                    _currentState = ItemState.Item;

                    controller.DropCondition.TryChangeWaterCount(1);

                    Destroy(gameObject);
                    break;
                case ItemState.Projectile:
                    _currentState = ItemState.Pickable;

                    _pickable.gameObject.SetActive(true);
                    break;
                case ItemState.Item:
                    _currentState = ItemState.Projectile;

                    _pickableDelay = PickableMaxDelay;
                    break;
            }
        }

        protected override void WallCollision(Collision2D collision)
        {
            if (_currentState != ItemState.Projectile && _currentState != ItemState.Pickable)
                return;

            if (collision.gameObject.TryGetComponent(out IShootable shootable))
                shootable.Shoot(_projectileType, () => Destroy(gameObject));

            if (_pickableDelay > 0)
                return;

            if (_currentState != ItemState.Projectile)
                return;

            ChangeState();
        }

        protected override void TargetCollision(Collider2D collision)
        {
            if (_currentState != ItemState.Projectile && _currentState != ItemState.Pickable)
                return;

            if (collision.gameObject.TryGetComponent(out IShootable shootable))
                shootable.Shoot(_projectileType, () => Destroy(gameObject));
        }

        protected override void ProjectileDelay()
        {
            base.ProjectileDelay();

            if (_currentState != ItemState.Projectile)
                return;

            _projectilestateDelay = Mathf.Max(_projectilestateDelay - Time.deltaTime, 0);

            if (_projectilestateDelay <= 0)
                ChangeState();
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_colliders.Contains(collision.collider))
                _colliders.Add(collision.collider);
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            if (_colliders.Contains(collision.collider))
                _colliders.Remove(collision.collider);
        }
    }
}