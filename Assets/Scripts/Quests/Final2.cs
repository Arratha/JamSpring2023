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

        private string _message = "Тут могла быть ваща реклама";

        private Collider2D _collider;

        private bool _isQuestDone;

        private void Awake()
        {
            _messageBackground = _messageField.transform.parent.gameObject;

            _collider = GetComponent<Collider2D>();
        }

        private void Final()
        {
            if (_isQuestDone)
                return;

            _isQuestDone = true;

            _messageBackground.SetActive(true);
            _messageField.text = _message;
        }

        public void SetActive()
        {
            _collider.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Collision");

            if (collision.gameObject.CompareTag(Tags.DropTag))
                Final();
        }
    }
}