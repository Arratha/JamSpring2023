using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

using Settings.Tags;
using Shooting;


namespace Quest
{
    public class BugQuest_Controller : MonoBehaviour
    {
        [SerializeField] private GameObject _rewardPrefab;
        [SerializeField] private Transform _rewardCreationPoint;

        private GameObject _reward;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _messageField;
        private GameObject _messageBackground;

        private IShootable _shootable;

        private string[][] _messages = new string[][]
        {
            new string[]
            {
                "Господь жучий, как же жарко…",
                "Ооо, здравствуй мой жидкий друг! Возможно, ты мне и сможешь помочь…",
                "Да-да, катающаяся желейка, именно ты! Выполни мою просьбу, облей меня водой, и за это, я дам тебе нечто очень ценное…"
            } ,
            new string[]
            {
                "О дааа, спасибо тебе, прыгающий пузырик",
                "Как и обещал, самое ценное, что у меня есть - знание! Я его получил от мимо пробегающего волка, так что слушай внимательно. Звучит оно так кхм:”- В какой форме вода приобретает самую большую силу? - В форме женских слёз. Ауф",
                "Что? Тебе этого недостаточно?! Ну, возьми тогда этот листик, больше мне все равно нечего тебе дать, может пригодится."
            }
        };

        private int _indexI = 0;
        private int _indexJ = 0;

        private bool _isDropInRange => _dropColliders.Count > 0;
        private List<Collider2D> _dropColliders = new List<Collider2D>();

        private bool _isShooted;
        private bool _isQuestDone;
        
        private void Awake()
        {
            _messageBackground = _messageField.transform.parent.gameObject;

            _shootable = GetComponentInChildren<IShootable>();

            _shootable.OnShooted += Shooted;
        }

        private void Update()
        {
            ChangeMessage();
            CreateReward();
        }

        private void Shooted(ProjectileType type, Vector2 projectilePosition, OnShootCallback callback)
        {
            if (_isQuestDone)
                return;

            if (_isShooted)
                return;

            if (type != ProjectileType.DropOfWater)
                return;

            _isShooted = true;

            callback?.Invoke();

            _indexI = _messages.Length - 1;
            _indexJ = 0;

            ShowMessage(_messages[_indexI][_indexJ]);
        }

        private void QuestDone()
        {
            _isQuestDone = true;
        }

        private void CreateReward()
        {
            if (_isQuestDone && _reward == null)
                _reward = Instantiate(_rewardPrefab, _rewardCreationPoint.position, new Quaternion(0, 0, 0, 0));
        }

        private void ShowMessage(string message)
        {
            if (!_isDropInRange)
                StartCoroutine(ShowMessageTemporary());

            _messageField.text = message;
        }

        private IEnumerator ShowMessageTemporary()
        {
            _messageBackground.SetActive(true);

            yield return new WaitForSeconds(3);

            _messageBackground.SetActive(_isDropInRange);
        }

        private void ChangeMessage()
        {
            if (_isQuestDone)
                return;

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

            if (_indexI == _messages.Length - 1
                && _indexJ + change == _messages[_indexI].Length - 1)
                QuestDone();

            _indexJ += change;

            ShowMessage(_messages[_indexI][_indexJ]);
        }

        private void ChangeDropRange()
        {
            _messageBackground.SetActive(_isDropInRange);

            if (!_isDropInRange)
                return;

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