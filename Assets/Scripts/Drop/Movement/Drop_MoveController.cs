using System.Collections.Generic;

using UnityEngine;


namespace Drop.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Transform))]
    public class Drop_MoveController : MonoBehaviour
    {
        [SerializeField] private float MoveForce = 5;
        [SerializeField] private float JumpForce = 400;

        [SerializeField] private float MaxSpeed = 5f;

        [SerializeField] private float _factorOfFriction = 1.05f;

        private bool _isGrounded => _shapePoints.FindAll(x => x.IsGrounded).Count != 0;

        private bool _isInCollision => _shapePoints.FindAll(x => x.IsInCollision).Count != 0;
        private List<Drop_ShapePoint> _shapePoints = new List<Drop_ShapePoint>();

        private Rigidbody2D _dropRigidbody;
        private Transform _dropBodyTransform;

        private void Awake()
        {
            _dropRigidbody = GetComponent<Rigidbody2D>();
            _dropBodyTransform = transform;
        }

        public void AddShapePoint(Drop_ShapePoint point)
        {
            _shapePoints.Add(point);
        }

        public void Jump(Vector2 mousePosition, float prepTime)
        {
            if (!_isGrounded)
                return;

            Vector2 targetVector = Camera.main.ScreenToWorldPoint(mousePosition) - _dropBodyTransform.position;

            _dropRigidbody.AddForce(targetVector.normalized * JumpForce * prepTime);
        }

        public void JumpOut(Vector2 targetPosition)
        {
            Vector2 dropPosition = _dropBodyTransform.position;
            Vector2 targetVector = -1 * (targetPosition - dropPosition);

            _dropRigidbody.velocity = Vector2.zero;
            _dropRigidbody.AddForce(targetVector.normalized * JumpForce);
        }

        public void Move(float targetVector)
        {
            if (!_isGrounded)
                return;

            _dropRigidbody.AddForce(Vector2.right * targetVector * MoveForce * 1.5f);

            if (Mathf.Sign(targetVector) != Mathf.Sign(_dropRigidbody.velocity.x))
                Stop();

            if (_dropRigidbody.velocity.x > MaxSpeed)
                _dropRigidbody.velocity = new Vector2(Mathf.Sign(_dropRigidbody.velocity.x) * MaxSpeed,
                    _dropRigidbody.velocity.y);
        }

        public void Stop()
        {
            if (!_isInCollision)
                return;

            _dropRigidbody.velocity /= _factorOfFriction;
            _shapePoints.ForEach(x => x.Velocity /= _factorOfFriction);
        }

        public void SetGravity(float gravity)
        {
            _dropRigidbody.gravityScale = gravity;

            foreach (var currentPoint in _shapePoints)
                currentPoint.GetComponent<Rigidbody2D>().gravityScale = gravity;
        }

        public void RemovePhysics()
        {
            _dropRigidbody.bodyType = RigidbodyType2D.Kinematic;
            _dropRigidbody.velocity = Vector2.zero;
            GetComponent<Collider2D>().isTrigger = true;

            foreach (var currentPoint in _shapePoints)
            {
                currentPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                currentPoint.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                currentPoint.GetComponent<Collider2D>().isTrigger = true;
            }
        }
    }
}