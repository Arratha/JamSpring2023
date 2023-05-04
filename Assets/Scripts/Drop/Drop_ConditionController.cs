using System;

using UnityEngine;

using Drop.Movement;


namespace Drop
{
    public class Drop_ConditionController : MonoBehaviour
    {
        private int _waterCount = BaseWaterCount;

        public const int BaseWaterCount = 10;
        public const int MaxWaterCount = 30;

        public static event Action<int> OnWaterCountChanged;

        private float _invulnerabilitySeconds = 3f;
        private const float InvulnerabilityMaxSeconds = 1f;

        private Drop_Controller _dropController;

        private void Awake()
        {
            _dropController = GetComponentInParent<Drop_Controller>();
        }

        private void FixedUpdate()
        {
            Invulnerability();
        }

        public bool TryChangeWaterCount(int change, bool isFored = false)
        {
            if (!isFored && (_waterCount + change < 1 || _waterCount + change > MaxWaterCount))
                return false;

            if (_waterCount + change <= 0)
            {
                Death();
                return false;
            }

            _waterCount += change;

            OnWaterCountChanged?.Invoke(_waterCount);

            return true;
        }

        public void Invulnerability()
        {
            _invulnerabilitySeconds = Math.Max(_invulnerabilitySeconds - Time.deltaTime, 0);
        }

        public void DoDamage(Vector2 damageDealerPosition)
        {
            if (_invulnerabilitySeconds > 0)
                return;

            _dropController.DropMove.JumpOut(damageDealerPosition);
            _invulnerabilitySeconds = InvulnerabilityMaxSeconds;

            TryChangeWaterCount(-1, true);
        }

        private void Death()
        {
            Destroy(_dropController.gameObject);
        }
    }
}