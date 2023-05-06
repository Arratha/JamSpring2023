using UnityEngine;

using TMPro;

using Settings.Tags;
using Drop;
using Items;


namespace Quest
{
    [RequireComponent(typeof(Collider2D))]
    public class Final2 : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageField;
        private GameObject _messageBackground;

        private string _message = "“ут должен быть босс файт, но мы его не сделали :)";

        private Collider2D _collider;

        private bool _isQuestDone;
        private Drop_Controller _drop;

        private void Awake()
        {
            _messageBackground = _messageField.transform.parent.gameObject;

            _collider = GetComponent<Collider2D>();
        }

        private void Final(Collider2D collision)
        {
            if (_isQuestDone)
                return;

            _isQuestDone = true;
            _drop = collision.GetComponentInParent<Drop_Controller>();
            _messageBackground.SetActive(true);
            _messageField.text = _message;

            GetComponent<Animator>().enabled = false;

            _drop.RemoveControl();
        }

        public void SetActive(Drop_Controller drop)
        {
            _collider.enabled = true;
            _drop = drop;
            GetComponent<Animator>().speed = 0.4f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.DropTag))
                Final(collision);
        }
    }
}