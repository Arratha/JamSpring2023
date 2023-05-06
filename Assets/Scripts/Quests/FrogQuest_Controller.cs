using System.Collections.Generic;

using UnityEngine;

using TMPro;

using Settings.Tags;
using Shooting;


namespace Quest
{
    public class FrogQuest_Controller : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageField;
        [SerializeField] private GameObject _dialogueSprite;

        private IShootable _shootable;

        private string[][] _messages = new string[][] { new string[] 
        { 
            "������, ������� �����! � ����, ��� �� ��������� �� ���� ������.", 
            "� ��������, ��� �� ����� ����� � �������, � ��� ������, ��� �� ������� ������ ��� � ���������� �������!", 
            "��� ����� ������� ������������, � � �����, ��� �� ������ ����� ���� ������ ����������.",
            "�� ������ � ����� ������?",
            "...",
            "��*, Chat GPT ������ �������������",
            "������, �����, � ���� ������ � � ������������ ������ �� ����: ��������� ��� ���� ������� � � �� � ����������",
            "��, �����, �� �� ������ ��������, ������ ����� ���� ��������� �� ����� ����� ����������� ����",
            "����, A - �������� �����, D - ������",
            "����� ����, �� ������ �������! ����� ��� � ������ ����� �� ����������� ����� �����.",
            "��� �� �� ���� ��� ��������?! ��, ������ ��� ������� ����� � ����� ���� ��������� ��������.",
            "���, ������ ������ � ����� ��� ���� ������������!..���"
        }, 
            new string[] 
            { 
                "���",
            } 
        };
        private string[] _getTadpoleMessage = new string[] { "��� ���� ��������! �������, ������ ����� ��� ����������!", "����! ������� ���, �� ��� �� ���������?", "�, ������ �� �������! ������� ����, �����, ������ �� �����!..���" };

        private int _indexI = 0;
        private int _indexJ = 0;

        private bool _isReaded;
        private bool _isDropInRange => _dropColliders.Count > 0;
        private List<Collider2D> _dropColliders = new List<Collider2D>();


        private void Awake()
        {
            _shootable = GetComponentInChildren<IShootable>();

            _shootable.OnShooted += Shooted;
        }

        private void Update()
        {
            ChangeMessage();
        }

        private void Shooted(ProjectileType type, Vector2 projectilePosition, OnShootCallback callback)
        {
            if (type != ProjectileType.Tadpole)
                return;

            callback?.Invoke();

            ShowMessage(_getTadpoleMessage[Random.Range(0, _getTadpoleMessage.Length)]);
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
            _messageField.gameObject.SetActive(_isDropInRange);
            _dialogueSprite.gameObject.SetActive(_isDropInRange);

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

            ChangeDropRange();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.DropTag)
                && _dropColliders.Contains(collision))
                _dropColliders.Remove(collision);

            ChangeDropRange();
        }

        private void OnDestroy()
        {
            _shootable.OnShooted -= Shooted;
        }
    }
}