using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

using Settings.Tags;
using Shooting;


namespace Quest
{
    public abstract class Dialog_Controller : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageField;
        private GameObject _messageBackground;

        protected virtual string[][] _messages => new string[][] { };

        private int _indexI = 0;
        private int _indexJ = 0;

        private bool _isReaded;
        private bool _isDropInRange;
        private List<Collider2D> _dropColliders = new List<Collider2D>();

        private void Awake()
        {
            _messageBackground = _messageField.transform.parent.gameObject;
        }

        private void Update()
        {
            ChangeMessage();
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

        private void ChangeDropRange()
        {
            _messageBackground.SetActive(_isDropInRange);

            if (!_isDropInRange)
                return;

            if (_isReaded)
            {
                _isReaded = false;

                _indexI = Mathf.Min(_indexI + 1, _messages.Length - 1);
                _indexJ = 0;
            }

            ShowMessage(_messages[_indexI][_indexJ]);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.DropTag)
                && !_dropColliders.Contains(collision))
                _dropColliders.Add(collision);

            if (_isDropInRange != _dropColliders.Count > 0)
            {
                _isDropInRange = _dropColliders.Count > 0;
                ChangeDropRange();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.DropTag)
                && _dropColliders.Contains(collision))
                _dropColliders.Remove(collision);

            if (_isDropInRange != _dropColliders.Count > 0)
            {
                _isDropInRange = _dropColliders.Count > 0;
                ChangeDropRange();
            }
        }
    }
}