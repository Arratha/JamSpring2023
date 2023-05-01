using System;
using System.Collections.Generic;

using UnityEngine;

using Drop.Liquids.Models;


namespace Drop.Liquids
{
    public enum LiquidType { Water, Oil, Pitch }

    public class Drop_LiquidsController : MonoBehaviour
    {
        public BaseLiquid CurrentLiquid { get; private set; }
        public event Action<BaseLiquid> OnLiquidChanged;

        private Dictionary<LiquidType, LiquidState> _liquids;

        private void Awake()
        {
            _liquids = new Dictionary<LiquidType, LiquidState>() {
               { LiquidType.Water, new LiquidState(new Water(), true) },
               { LiquidType.Oil, new LiquidState(new Oil(), true) },
               { LiquidType.Pitch, new LiquidState(new Pitch(), true) }
            };
        }

        public void ChangeLiquidAvailability(LiquidType type, bool availability)
        {
            _liquids[type].IsAwailable = availability;
        }

        public void ChangeLiquid(int id)
        {
            if (_liquids[(LiquidType)id].IsAwailable)
                CurrentLiquid = _liquids[(LiquidType)id].Liquid;

            OnLiquidChanged?.Invoke(CurrentLiquid);
        }

        private class LiquidState
        {
            public BaseLiquid Liquid;
            public bool IsAwailable;

            public LiquidState(BaseLiquid liquid, bool isAwailable)
            {
                Liquid = liquid;
                IsAwailable = isAwailable;
            }
        }
    }
}