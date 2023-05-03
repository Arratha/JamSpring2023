using UnityEngine;

using Drop;

using Shooting;
using Shooting.Weapon;

using Pickables;


namespace Items
{
    public enum ItemState { Pickable, Projectile, Item }

    public class Knife_Controller : MonoBehaviour
    {
        [SerializeField] private ItemState _currentState = ItemState.Pickable;

        private Rigidbody2D _knifeRigidBody;
        private Collider2D _knifeCollider;

        private SpringJoint2D _springJoint;
        private const float _minDistance = 0.2f;

        private Pickable_Knife _pickable;

        private float _pickableDelay;
        private const float PickableMaxDelay = 0.5f;

        private void Awake()
        {
            _knifeRigidBody = GetComponent<Rigidbody2D>();
            _knifeCollider = GetComponent<Collider2D>();

            _pickable = GetComponentInChildren<Pickable_Knife>();

            _pickable.OnPicked += Pick;
        }

        private void Update()
        {
            if (_springJoint != null && _springJoint.distance > _minDistance)
                _springJoint.distance = Mathf.Max(_springJoint.distance - Time.deltaTime * 5, _minDistance);

            if (_currentState == ItemState.Projectile)
                _pickableDelay = Mathf.Max(_pickableDelay - Time.deltaTime, 0);
        }

        public void Pick(Drop_Controller controller)
        {
            if (_currentState == ItemState.Pickable)
                ChangeState(controller);
        }

        public void Shoot()
        {
            if (_currentState == ItemState.Item)
                ChangeState();
        }

        private void AddSpring(Rigidbody2D rb)
        {
            _springJoint = gameObject.AddComponent<SpringJoint2D>();

            _springJoint.connectedBody = rb;
            _springJoint.anchor = new Vector2(Random.Range(-0.05f, 0.05f), 0);

            _springJoint.autoConfigureDistance = false;
            _springJoint.distance = Vector2.Distance(transform.position, rb.transform.position);

            _springJoint.frequency = 0f;
        }

        public void ChangeState(Drop_Controller controller = null)
        {
            switch (_currentState)
            {
                case ItemState.Pickable:
                    _currentState = ItemState.Item;

                    _knifeRigidBody.gravityScale = 0.5f;
                    _knifeRigidBody.angularVelocity = 0;

                    _knifeCollider.isTrigger = true;

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

                    _knifeCollider.isTrigger = false;

                    _knifeRigidBody.gravityScale = 1f;

                    Destroy(_springJoint);

                    _pickableDelay = PickableMaxDelay;
                    break;
            }
        }

        private void WallCollision(Collision2D collision)
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

        private void TargetCollision(Collider2D collision)
        {
            if (_currentState != ItemState.Item && _currentState != ItemState.Projectile)
                return;

            if (collision.gameObject.TryGetComponent(out IShootable shootable))
                shootable.Shoot();
        }

        public void OnCollisionStay2D(Collision2D collision)
        {
            WallCollision(collision);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            TargetCollision(collision);
        }

        private void OnDestroy()
        {
            _pickable.OnPicked -= Pick;
        }
    }
}