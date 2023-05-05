using System;

using UnityEngine;

using Drop;


namespace Pickables
{
    public class Pickable : MonoBehaviour, IPickable
    {
        public event Action<Drop_Controller, IPickable> OnPicked;

        public void Pick(Drop_Controller controller)
        {
            OnPicked?.Invoke(controller, this);
        }

        private void OnDestroy()
        {
            OnPicked = null;
        }
    }
}