using System;

using UnityEngine;


namespace Drop
{
    public class Drop_ConditionController : MonoBehaviour
    {
        private int _waterCount = 10;

        public const int BaseWaterCount = 10;
        public const int MaxWaterCount = 30;

        public static Predicate<int> OnTryChangeWaterCount;
        public static event Action<int> OnWaterCountChanged;

        private void Awake()
        {
            _waterCount = BaseWaterCount;

            OnTryChangeWaterCount += TryChangeWaterCount;
        }

        public bool TryChangeWaterCount(int change)
        {
            if (_waterCount + change < 0 || _waterCount + change > MaxWaterCount)
                return false;

            _waterCount += change;

            OnWaterCountChanged?.Invoke(_waterCount);

            return true;
        }

        private void OnDestroy()
        {
            OnTryChangeWaterCount += TryChangeWaterCount;
        }
    }
}