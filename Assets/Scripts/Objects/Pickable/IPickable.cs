using System;

using UnityEngine;

using Drop;


namespace Pickables
{
    public interface IPickable
    {
        public void Pick(Drop_Controller controller);

        public event Action<Drop_Controller, IPickable> OnPicked;
        public GameObject gameObject { get; }
    }
}