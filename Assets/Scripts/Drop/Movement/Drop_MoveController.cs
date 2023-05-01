using System.Collections.Generic;

using UnityEngine;

namespace Drop.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Transform))]
    public class Drop_MoveController : MonoBehaviour
    {
        private Rigidbody2D _dropRigidbody;

        private const float MoveForce = 5;
        private const float JumpForce = 400;

        private bool _isInCollision => _shapePoints.FindAll(x => x.IsInCollision).Count != 0;
        private List<Drop_ShapePoint> _shapePoints = new List<Drop_ShapePoint>();

        private const float MaxSpeed = 5f;

        private float _factorOfFriction = 1.05f;

        private void Awake()
        {
            _dropRigidbody = GetComponent<Rigidbody2D>();
        }

        public void AddShapePoint(Drop_ShapePoint point)
        {
            _shapePoints.Add(point);
        }

        public void Jump()
        {
            if (!_isInCollision)
                return;

            _dropRigidbody.AddForce(Vector2.up * JumpForce);
        }

        public void Move(float targetVector)
        {
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
    }
}