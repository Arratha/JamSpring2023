using UnityEngine;

using Drop.Movement;
using SoftBody.Skin;


namespace Drop.Visuals
{
    [RequireComponent(typeof(SoftBody_SkinController))]
    public class Drop_VisualController : MonoBehaviour
    {
        private SoftBody_SkinController _softSkinController;
        private Transform _dropBodyTransform;

        private Vector2 _baseScale;

        private Drop_ShapePoint[] _shapePoints;

        private void Awake()
        {
            _dropBodyTransform = transform;
            _softSkinController = GetComponent<SoftBody_SkinController>();

            _baseScale = _dropBodyTransform.localScale;

            _shapePoints = GetComponentsInChildren<Drop_ShapePoint>();

            Drop_ConditionController.OnWaterCountChanged += Decrease;
        }

        public void GroupUp(float percent)
        {
            _softSkinController.GroupUp(percent);
        }

        private void Decrease(int currentWaterCount)
        {
            float newPercent;

            if (currentWaterCount <= Drop_ConditionController.BaseWaterCount)
                newPercent = (0.5f
                    + 0.5f * (float)currentWaterCount / (float)Drop_ConditionController.BaseWaterCount);
            else
                newPercent = (1
                    + 0.5f * (float)(currentWaterCount - Drop_ConditionController.BaseWaterCount) / (float)Drop_ConditionController.MaxWaterCount);

            _dropBodyTransform.localScale = _baseScale * newPercent;

            foreach (var currentPoint in _shapePoints)
                currentPoint.ChangeJointsDistance(newPercent);
        }

        private void OnDestroy()
        {
            Drop_ConditionController.OnWaterCountChanged -= Decrease;
        }
    }
}