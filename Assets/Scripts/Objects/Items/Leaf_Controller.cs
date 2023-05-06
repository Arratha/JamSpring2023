using UnityEngine;

using Drop;


namespace Items
{
    public class Leaf_Controller : PickableItem_Controller
    {
        protected override float _minDistance => 0;
        protected override Vector2? _springAnchor => new Vector2(-1.1f, -0.4f);

        protected override void ChangeState(Drop_Controller controller = null)
        {
            switch (_currentState)
            {
                case ItemState.Pickable:
                    _currentState = ItemState.Item;

                    _collider.isTrigger = true;

                    transform.eulerAngles = Vector3.zero;
                    _pickable.gameObject.SetActive(false);
                    _rigidBody.freezeRotation = true;

                    transform.parent = controller.transform;

                    AddSpring(controller.DropShooting.GetComponent<Rigidbody2D>());
                    controller.DropMove.SetGravity(0.5f);
                    break;
                case ItemState.Projectile:
                    _currentState = ItemState.Pickable;

                    _pickable.gameObject.SetActive(true);
                    _rigidBody.freezeRotation = false;

                    controller.DropMove.SetGravity(1);
                    break;
                case ItemState.Item:
                    _currentState = ItemState.Projectile;

                    _collider.isTrigger = false;

                    Destroy(_springJoint);
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

        }
    }
}