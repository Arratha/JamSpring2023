using UnityEngine;
using UnityEngine.U2D;


namespace SoftBody.Skin
{
    public class SoftBody_SkinController : MonoBehaviour
    {
        [SerializeField] private SpriteShapeController _spriteShape;
        [SerializeField] private Transform[] _shapePoints;

        private const float SplineOffset = 0.2f;
        private const float SpriteSizeMultiplier = 1.5f;

        private const float ShrinkChange = 0.05f;
        private float _shrinkMultiplier = 1;

        private void Update()
        {
            UpdateVerticies();
        }

        public void Shrink(float percent)
        {
            float min = 1 / SpriteSizeMultiplier;

            _shrinkMultiplier = SpriteSizeMultiplier - (SpriteSizeMultiplier - min) * percent;
        }

        [ContextMenu(nameof(UpdateVerticies))]
        private void UpdateVerticies()
        {
            for (int i = 0; i < _shapePoints.Length; i++)
            {
                Vector2 vertex = _shapePoints[i].localPosition;
                Vector2 towardsCenter = (Vector2.zero - vertex).normalized;

                float radiusMultiplier = SpriteSizeMultiplier * _shrinkMultiplier;
                float colliderRadius = _shapePoints[i].gameObject.GetComponent<CircleCollider2D>().radius * _shapePoints[i].localScale.x * radiusMultiplier;

                try
                {
                    _spriteShape.spline.SetPosition(i, (vertex - towardsCenter * colliderRadius));
                }
                catch
                {
                    _spriteShape.spline.SetPosition(i, (vertex - towardsCenter * (colliderRadius + SplineOffset)));
                }

                Vector2 leftTangent = _spriteShape.spline.GetLeftTangent(i);

                Vector2 newRightTangent = Vector2.Perpendicular(towardsCenter) * leftTangent.magnitude;
                Vector2 newLeftTangent = Vector2.zero - newRightTangent;

                _spriteShape.spline.SetRightTangent(i, newRightTangent);
                _spriteShape.spline.SetLeftTangent(i, newLeftTangent);
            }
        }
    }
}