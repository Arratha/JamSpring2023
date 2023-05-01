using System.Collections.Generic;

using UnityEngine;

using Settings.Tags;


namespace Drop.Movement
{
    [RequireComponent(typeof(Collider2D))]
    public class Drop_ShapePoint : MonoBehaviour
    {
        public bool IsGrounded { get; private set; }
        public bool IsInCollision => _groundColliders.Count > 0;
        private List<Collider2D> _groundColliders = new List<Collider2D>();

        public Vector2 Velocity { get => _pointRigidBody.velocity; set => _pointRigidBody.velocity = value; }

        private Collider2D _dropCollider;

        private Rigidbody2D _pointRigidBody;

        private float _groundedTimer;
        private const float GroundedExtraTime = 0.25f;

        private void Awake()
        {
            _pointRigidBody = GetComponent<Rigidbody2D>();

            Drop_MoveController dropMoveController = GetComponentInParent<Drop_MoveController>();
            dropMoveController.AddShapePoint(this);

            _dropCollider = dropMoveController.GetComponent<Collider2D>();
        }

        private void LateUpdate()
        {
            CheckGrounded();
        }

        private void CheckGrounded()
        {
            if (!IsInCollision)
                _groundedTimer = Mathf.Max(0, _groundedTimer - Time.deltaTime);
            else
                _groundedTimer = GroundedExtraTime;

            IsGrounded = _groundedTimer > 0;
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.GroundTag)
                && !_groundColliders.Contains(collision.collider))
                foreach (var currentPoint in collision.contacts)
                    if (currentPoint.point.y <= _dropCollider.bounds.min.y)
                    {
                        _groundColliders.Add(collision.collider);
                        break;
                    }
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.GroundTag)
                && _groundColliders.Contains(collision.collider))
                _groundColliders.Remove(collision.collider);
        }
    }
}