using UnityEngine;

using SoftBody.Skin;


namespace Drop.Visuals
{
    [RequireComponent(typeof(SoftBody_SkinController))]
    public class Drop_VisualController : MonoBehaviour
    {
        private SoftBody_SkinController _softSkinController;

        private void Awake()
        {
            _softSkinController = GetComponent<SoftBody_SkinController>();
        }

        public void Shrink(float percent)
        {
            _softSkinController.Shrink(percent);
        }
    }
}