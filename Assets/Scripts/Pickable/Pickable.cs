using System;

using UnityEngine;

using Drop;


namespace Pickables
{
    public class Pickable : MonoBehaviour, IPickable
    {
        public event Action<Drop_Controller> OnPicked;

        public void Pick(Drop_Controller controller)
        {
            OnPicked?.Invoke(controller);
        }
    }
}