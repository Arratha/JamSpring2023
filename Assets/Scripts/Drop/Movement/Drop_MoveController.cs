using System.Collections.Generic;

using UnityEngine;

using Drop.Liquids;
using Drop.Liquids.Models;


namespace Drop.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(Drop_LiquidsController))]
    public class Drop_MoveController : MonoBehaviour
    {
        private Transform _dropTransform;
        private Rigidbody2D _dropRigidbody;

        private const float MoveForce = 5;
        private const float JumpForce = 700;

        private bool _isGrounded => _shapePoints.FindAll(x => x.IsGrounded).Count != 0;
        private bool _isInCollision => _shapePoints.FindAll(x => x.IsInCollision).Count != 0;
        private List<Drop_ShapePoint> _shapePoints = new List<Drop_ShapePoint>();

        private float _factorOfFriction;

        private Drop_LiquidsController _liquidController;

        private void Awake()
        {
            _dropTransform = transform;
            _dropRigidbody = GetComponent<Rigidbody2D>();

            _liquidController = GetComponent<Drop_LiquidsController>();
            _liquidController.OnLiquidChanged += ChangeLiquid;
        }

        private void ChangeLiquid(BaseLiquid liquid)
        {
            _factorOfFriction = liquid.FactorOfFriction;
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

        public void Move(Vector2 mousePosition)
        {
            if (!_isGrounded)
                return;

            Vector2 targetVector = Camera.main.ScreenToWorldPoint(mousePosition) - _dropTransform.position;

            _dropRigidbody.AddForce(targetVector.normalized * MoveForce);
        }

        public void Stop()
        {
            if (!_isInCollision)
                return;

            _dropRigidbody.velocity /= _factorOfFriction;
            _shapePoints.ForEach(x => x.Velocity /= _factorOfFriction);
        }

        private void OnDestroy()
        {
            _liquidController.OnLiquidChanged -= ChangeLiquid;
        }
    }
}