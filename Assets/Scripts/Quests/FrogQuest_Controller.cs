using UnityEngine;

using TMPro;

using Settings.Tags;
using Drop;
using Shooting;


namespace Quest
{
    public class FrogQuest_Controller : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageField;

        private IShootable _pickable;

        private string[][] _messages = new string[][] { new string[] { "0", "1", "2" }, new string[] { "3", "4", "5" } };
        private string[] _getTodpoleMessage = new string[] { "Tod1" };

        private int _indexI = 0;
        private int _indexJ = 0;

        private bool _isReaded;
        private bool _isDropInRange;

        private void Awake()
        {
            _pickable = GetComponentInChildren<IShootable>();

            _pickable.OnShooted += GetTodpole;
        }

        private void Update()
        {
            ChangeMessage();
        }

        private void GetTodpole(ProjectileType type, Vector2 projectilePosition, OnShootCallback callback)
        {
            if (type != ProjectileType.Tadpole)
                return;

            callback?.Invoke();

            ShowMessage(_getTodpoleMessage[Random.Range(0, _getTodpoleMessage.Length)]);
        }

        private void ShowMessage(string message)
        {
            _messageField.text = message;
        }

        private void ChangeMessage()
        {
            if (!_isDropInRange)
                return;

            if (Input.GetKeyDown(KeyCode.Q))
                TryChangeMessage(-1);

            if (Input.GetKeyDown(KeyCode.E))
                TryChangeMessage(1);
        }

        private void TryChangeMessage(int change)
        {
            if (_indexJ + change < 0 || _indexJ + change > _messages[_indexI].Length - 1)
                return;

            if (_indexJ + change == _messages[_indexI].Length - 1)
                _isReaded = transform;

            _indexJ += change;

            ShowMessage(_messages[_indexI][_indexJ]);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.DropTag))
            {
                _isDropInRange = true;
                _messageField.gameObject.SetActive(true);

                if (_isReaded)
                {
                    _isReaded = false;

                    _indexI = Mathf.Min(_indexI + 1, _messages.Length - 1);
                    _indexJ = 0;
                }

                ShowMessage(_messages[_indexI][_indexJ]);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.DropTag))
            {
                _isDropInRange = false;
                _messageField.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _pickable.OnShooted -= GetTodpole;
        }
    }
}