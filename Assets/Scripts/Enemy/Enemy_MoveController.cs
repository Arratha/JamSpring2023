using System.Collections.Generic;

using UnityEngine;

using Settings.Tags;


namespace Enemy
{
    public delegate void OnPoinCallback();

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Transform))]
    public class Enemy_MoveController : MonoBehaviour
    {
        [SerializeField] private float MoveForce = 5;
        [SerializeField] private float JumpForce = 200;

        [SerializeField] private float MaxSpeed = 3f;
        [SerializeField] private float MinSpeed = 0.001f;

        private Rigidbody2D _enemyRigidbody;
        private Collider2D _enemyCollider;

        public Collider2D collider { get => GetComponent<Collider2D>(); }

        private bool _isGrounded;
        private bool _isInCollision => _groundColliders.Count > 0;
        private List<Collider2D> _groundColliders = new List<Collider2D>();

        private float _groundedTimer;
        private const float GroundedExtraTime = 0.25f;
        
        private void Awake()
        {
            _enemyRigidbody = GetComponent<Rigidbody2D>();
            _enemyCollider = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            CheckGrounded();
        }

        public void MoveTo(Vector2 targetPosition, OnPoinCallback callback = null)
        {
            if (!_isGrounded)
                return;

            if (Mathf.Abs(targetPosition.x - transform.position.x) < _enemyCollider.bounds.size.x / 2)
            {
                _enemyRigidbody.velocity = Vector2.zero;
                callback?.Invoke();
                return;
            }

            float targetVector = Mathf.Sign(targetPosition.x - transform.position.x);

            _enemyRigidbody.AddForce(Vector2.right * targetVector * MoveForce * 1.5f);

            if (Mathf.Sign(targetVector) != Mathf.Sign(_enemyRigidbody.velocity.x))
                _enemyRigidbody.velocity = new Vector2(0, _enemyRigidbody.velocity.y);

            if (_enemyRigidbody.velocity.x > MaxSpeed)
                _enemyRigidbody.velocity = new Vector2(Mathf.Sign(_enemyRigidbody.velocity.x) * MaxSpeed,
                    _enemyRigidbody.velocity.y);

            SetFacing();
        }

        public void JumpOut(Vector2 targetPosition)
        {
            Vector2 dropPosition = _enemyRigidbody.position;
            Vector2 targetVector = -1 * (targetPosition - dropPosition);

            _enemyRigidbody.velocity = Vector2.zero;
            _enemyRigidbody.AddForce(targetVector.normalized * JumpForce);
        }

        public void Stop()
        {
            if (!_isGrounded)
                return;

            _enemyRigidbody.velocity = Vector2.zero;
        }

        private void CheckGrounded()
        {
            if (!_isInCollision)
                _groundedTimer = Mathf.Max(0, _groundedTimer - Time.deltaTime);
            else
                _groundedTimer = GroundedExtraTime;

            _isGrounded = _groundedTimer > 0;
        }

        private void SetFacing()
        {
            if (Mathf.Abs(_enemyRigidbody.velocity.x) < MinSpeed)
                return;

            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(_enemyRigidbody.velocity.x),
                 transform.localScale.y, transform.localScale.z);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.GroundTag)
                && !_groundColliders.Contains(collision.collider))
                _groundColliders.Add(collision.collider);
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.GroundTag)
                && _groundColliders.Contains(collision.collider))
                _groundColliders.Remove(collision.collider);
        }
    }
}