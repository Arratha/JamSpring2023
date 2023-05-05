using UnityEngine;

using Drop;

using Shooting;
using Shooting.Weapon;


namespace Items
{
    public class Tadpole_Controller : PickableItem_Controller
    {
        protected override ProjectileType _projectileType { get; } = ProjectileType.Tadpole;

        private float _escapeTimer;
        private const float EscapeTimerMin = 1;
        private const float EscapeTimerMax = 3;
        private readonly float EscapeForce = 400f;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            TryEscape();
        }

        private void TryEscape()
        {
            if (_currentState != ItemState.Item)
                return;

            _escapeTimer = Mathf.Max(_escapeTimer - Time.deltaTime, 0);

            if (_escapeTimer <= 0)
            {
                _escapeTimer = Random.Range(EscapeTimerMin, EscapeTimerMax);

                Vector2 escapeVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

                _rigidBody.AddForce(escapeVector * EscapeForce);
                _rigidBody.AddTorque(Random.Range(-1, 1) * Random.Range(100, 300));
            }
        }

        protected override void ChangeState(Drop_Controller controller = null)
        {
            switch (_currentState)
            {
                case ItemState.Pickable:
                    _currentState = ItemState.Item;

                    _rigidBody.gravityScale = 0f;

                    _collider.isTrigger = true;

                    _pickable.gameObject.SetActive(false);

                    transform.parent = controller.transform;

                    AddSpring(controller.DropShooting.GetComponent<Rigidbody2D>());
                    controller.DropShooting.AddWeapon(new Weapon(this, 600));

                    _escapeTimer = Random.Range(EscapeTimerMin, EscapeTimerMax);
                    break;
                case ItemState.Projectile:
                    _currentState = ItemState.Pickable;

                    _pickable.gameObject.SetActive(true);
                    break;
                case ItemState.Item:
                    _currentState = ItemState.Projectile;

                    _rigidBody.gravityScale = 1f;

                    _collider.isTrigger = false;

                    Destroy(_springJoint);

                    _pickableDelay = PickableMaxDelay;
                    break;
            }
        }

        protected override void WallCollision(Collision2D collision = null)
        {
            if (_pickableDelay > 0)
                return;

            if (_currentState != ItemState.Projectile)
                return;

            ChangeState();
        }

        protected override void TargetCollision(Collider2D collision)
        {
            if (_currentState != ItemState.Projectile)
                return;

            if (collision.gameObject.TryGetComponent(out IShootable shootable))
                shootable.Shoot(_projectileType, transform.position, () => Destroy(gameObject));
        }
    }
}