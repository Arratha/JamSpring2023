using UnityEngine;

using Drop;


namespace Pickables
{
    public class Pickable_DropOfWater : MonoBehaviour, IPickable
    {
        public void Pick(Drop_Controller controller)
        {
            controller.DropCondition.TryChangeWaterCount(1);

            Destroy(gameObject);
        }
    }
}