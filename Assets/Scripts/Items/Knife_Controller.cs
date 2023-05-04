using UnityEngine;

using Drop;

using Shooting;
using Shooting.Weapon;


namespace Items
{
    public class Knife_Controller : PickableItem_Controller
    {
        protected override void ChangeState(Drop_Controller controller = null)
        {
            switch (_currentState)
            {
                case ItemState.Pickable:
                    _currentState = ItemState.Item;

                    _rigidBody.gravityScale = 0.5f;
                    _rigidBody.angularVelocity = 0;

                    _collider.isTrigger = true;

                    _pickable.gameObject.SetActive(false);

                    AddSpring(controller.DropShooting.GetComponent<Rigidbody2D>());
                    controller.DropShooting.AddWeapon(new Weapon_Knife(this));
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

        protected override void WallCollision(Collision2D collision)
        {
            if (_pickableDelay > 0)
                return;

            switch (_currentState)
            {
                case ItemState.Projectile:
                    ChangeState();
                    goto case ItemState.Item;
                case ItemState.Item:
                    if (collision.gameObject.TryGetComponent(out IShootable shootable))
                        shootable.Shoot();
                    break;
            }
        }

        protected override void TargetCollision(Collider2D collision)
        {
            if (_currentState != ItemState.Item && _currentState != ItemState.Projectile)
                return;

            if (collision.gameObject.TryGetComponent(out IShootable shootable))
                shootable.Shoot();
        }
    }
}