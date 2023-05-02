using System.Collections.Generic;

using UnityEngine;


namespace Drop.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Transform))]
    public class Drop_MoveController : MonoBehaviour
    {
        private Rigidbody2D _dropRigidbody;

        //Сила воздействия движения, от нее зависит скорость разгона и скорость разварота
        [SerializeField] private float MoveForce = 5;
        //Сила, с которой капля отталкивается от поверхности при прыжек
        [SerializeField] private float JumpForce = 400;

        //Максимальная горизонтальная скорость движения
        [SerializeField] private float MaxSpeed = 5f;

        //Угасание движения при остановке капли
        [SerializeField] private float _factorOfFriction = 1.05f;

        private bool _isGrounded => _shapePoints.FindAll(x => x.IsGrounded).Count != 0;

        private bool _isInCollision => _shapePoints.FindAll(x => x.IsInCollision).Count != 0;
        private List<Drop_ShapePoint> _shapePoints = new List<Drop_ShapePoint>();

        private Transform _dropTransform;

        private void Awake()
        {
            _dropRigidbody = GetComponent<Rigidbody2D>();
            _dropTransform = transform;
        }

        public void AddShapePoint(Drop_ShapePoint point)
        {
            _shapePoints.Add(point);
        }

        public void Jump(Vector2 mousePosition, float prepTime)
        {
            if (!_isGrounded)
                return;

            Vector2 targetVector = Camera.main.ScreenToWorldPoint(mousePosition) - _dropTransform.position;

            _dropRigidbody.AddForce(targetVector.normalized * JumpForce * prepTime);
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
    }
}