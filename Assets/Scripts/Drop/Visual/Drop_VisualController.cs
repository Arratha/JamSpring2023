using UnityEngine;
using UnityEngine.U2D;

using Drop.Liquids;
using Drop.Liquids.Models;
using SoftBody.Skin;


namespace Drop.Visuals
{
    [RequireComponent(typeof(SoftBody_SkinController))]
    [RequireComponent(typeof(Drop_LiquidsController))]
    public class Drop_VisualController : MonoBehaviour
    {
        [SerializeField] private SpriteShapeRenderer _skin;

        private Drop_LiquidsController _liquidController;
        private SoftBody_SkinController _softSkinController;

        private void Awake()
        {
            _softSkinController = GetComponent<SoftBody_SkinController>();

            _liquidController = GetComponent<Drop_LiquidsController>();
            _liquidController.OnLiquidChanged += ChangeLiquid;
        }

        public void Shrink(float percent)
        {
            _softSkinController.Shrink(percent);
        }

        private void ChangeLiquid(BaseLiquid liquid)
        {
            _skin.color = liquid.LiquidColor;
        }

        private void OnDestroy()
        {
            _liquidController.OnLiquidChanged -= ChangeLiquid;
        }
    }
}