using UnityEngine;

using Drop;

using Shooting;
using Shooting.Weapon;

using Pickables;


namespace Items
{
    public enum ItemState { Pickable, Projectile, Item }

    public abstract class PickableItem_Controller : MonoBehaviour
    {
        protected virtual ProjectileType _projectileType { get; }

        [SerializeField]protected ItemState _currentState = ItemState.Pickable;

        protected Rigidbody2D _rigidBody;
        protected Collider2D _collider;

        protected SpringJoint2D _springJoint;
        private const float _minDistance = 0.2f;

        protected IPickable _pickable;

        protected float _pickableDelay;
        protected const float PickableMaxDelay = 0.5f;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();

            _pickable = GetComponentInChildren<IPickable>();

            _pickable.OnPicked += Pick;
        }

        private void Update()
        {
            SpringReinforcement();
        }

        protected virtual void FixedUpdate()
        {
            ProjectileDelay();
        }

        public void Pick(Drop_Controller controller)
        {
            if (_currentState == ItemState.Pickable)
                ChangeState(controller);
        }

        public virtual void Shoot()
        {
            if (_currentState == ItemState.Item)
                ChangeState();
        }

        protected abstract void WallCollision(Collision2D collision);

        protected abstract void TargetCollision(Collider2D collision);

        protected void AddSpring(Rigidbody2D rb)
        {
            _springJoint = gameObject.AddComponent<SpringJoint2D>();

            _springJoint.connectedBody = rb;
            _springJoint.anchor = new Vector2(Random.Range(-0.05f, 0.05f), 0);

            _springJoint.autoConfigureDistance = false;
            _springJoint.distance = Vector2.Distance(transform.position, rb.transform.position);

            _springJoint.frequency = 0f;
        }

        protected abstract void ChangeState(Drop_Controller controller = null);

        protected void SpringReinforcement()
        {
            if (_springJoint == null || _springJoint.distance <= _minDistance)
                return;

            _springJoint.distance = Mathf.Max(_springJoint.distance - Time.deltaTime * 5, _minDistance);
        }

        protected virtual void ProjectileDelay()
        {
            if (_currentState != ItemState.Projectile)
                return;

            _pickableDelay = Mathf.Max(_pickableDelay - Time.deltaTime, 0);
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