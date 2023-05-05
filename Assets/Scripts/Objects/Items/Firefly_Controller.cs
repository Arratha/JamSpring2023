using UnityEngine;

using Drop;

using Shooting;
using Shooting.Weapon;


namespace Items
{
    public class Firefly_Controller : PickableItem_Controller
    {
        protected override ProjectileType _projectileType { get; } = ProjectileType.Firefly;

        private float _wanderingDelay;
        private const float WanderingMaxDelay = 2f;

        private readonly Vector2 MaxOffset = new Vector2(0.25f, 0.25f);
        private Vector2? _centralPosition;
        private float? _wanderingTime;

        private const float Slowdown = 1.03f;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            Wandering();
            ProjectileSlowdown();
        }

        private void Wandering()
        {
            if (_currentState != ItemState.Pickable)
                return;

            if (_centralPosition == null)
                _centralPosition = transform.position;

            if (_wanderingTime == null || _wanderingTime <= 0)
            {
                Vector2 currentPosition = transform.position;
                Vector2 offset = new Vector2(MaxOffset.x * Random.Range(-1f, 1f), MaxOffset.y * Random.Range(-1f, 1f));
                Vector2 newPosition = (Vector2)_centralPosition + offset;

                _wanderingTime = Random.Range(0.25f, 0.5f);

                _rigidBody.velocity = (newPosition - currentPosition) / ((float)_wanderingTime * 2);
            }

            _wanderingTime -= Time.deltaTime;
        }

        private void ProjectileSlowdown()
        {
            if (_currentState != ItemState.Projectile)
                return;

            _rigidBody.velocity /= Slowdown;
        }

        protected override void ChangeState(Drop_Controller controller = null)
        {
            switch (_currentState)
            {
                case ItemState.Pickable:
                    _currentState = ItemState.Item;

                    _collider.isTrigger = true;

                    _pickable.gameObject.SetActive(false);

                    transform.parent = controller.transform;

                    AddSpring(controller.DropShooting.GetComponent<Rigidbody2D>());
                    controller.DropShooting.AddWeapon(new Weapon(this, 200, 0, true));
                    break;
                case ItemState.Projectile:
                    _currentState = ItemState.Pickable;

                    _pickable.gameObject.SetActive(true);
                    break;
                case ItemState.Item:
                    _currentState = ItemState.Projectile;

                    _collider.isTrigger = false;

                    Destroy(_springJoint);

                    _pickableDelay = PickableMaxDelay;
                    _wanderingDelay = WanderingMaxDelay;

                    _centralPosition = null;
                    _wanderingTime = null;
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
            if (_currentState != ItemState.Item && _currentState != ItemState.Projectile)
                return;

            if (collision.gameObject.TryGetComponent(out IShootable shootable))
                shootable.Shoot(_projectileType, transform.position);
        }

        protected override void ProjectileDelay()
        {
            base.ProjectileDelay();

            if (_currentState != ItemState.Projectile)
                return;

            _wanderingDelay = Mathf.Max(_wanderingDelay - Time.deltaTime, 0);

            if (_wanderingDelay <= 0)
                ChangeState();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            if (_centralPosition == null)
                Gizmos.DrawWireCube(transform.position, MaxOffset);
            else
                Gizmos.DrawWireCube((Vector2)_centralPosition, MaxOffset);
        }
    }
}