using UnityEngine;

using Settings.Tags;
using Drop;
using Items;


namespace Quest
{
    [RequireComponent(typeof(Collider2D))]
    public class Final1 : MonoBehaviour
    {
        [SerializeField] private Final2 _final2;

        private bool _isQuestDone;

        private Drop_Controller _drop;

        private void Final(Collider2D collision)
        {
            if (_isQuestDone)
                return;

            _drop = collision.GetComponentInParent<Drop_Controller>();

            if (_drop == null)
                return;

            if (!CheckForLeaf())
                return;

            _isQuestDone = true;

            _final2.SetActive(_drop);
        }

        private bool CheckForLeaf()
        {
            Leaf_Controller leaf = _drop.GetComponentInChildren<Leaf_Controller>();

            return leaf != null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.DropTag))
                Final(collision);
        }
    }
}