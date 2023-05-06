using System.Collections;
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
        private GameObject _messageBackground;
        [SerializeField] private Animator anim;

        [Space(10)]
        private int _neededTadpoleCount = 3;
        private int _tadpoleCount = 0;

        private IShootable _shootable;

        private string[][] _messages = new string[][] { new string[] 
        { 
            "Привет, дорогая Капля! Я рада, что ты оказалась на моей голове.", 
            "Я заметила, что ты очень живая и быстрая, а это значит, что ты сможешь помочь мне в выполнении задания!", 
            "Мне нужно поймать головастиков, и я думаю, что ты можешь стать моим лучшим помощником.",
            "Ты готова к этому вызову?",
            "...",
            "Бл*, Chat GPT забыла выключить…КВА",
            "Короче, Капля, я тебя спасла и в благородство играть не буду: выполнишь для меня задание — и мы в расчете…КВА",
            "Ах, точно, ты же только родилась, сейчас научу тебя двигаться по этому стра… прекрасному миру",
            "Итак, A - катиться влево, D - вправо",
            "Кроме того, ты можешь прыгать! Зажми ПКМ и сделай рывок по направлению своей мышки.",
            "Что ты на меня так смотришь?! Да, именно так прыгают капли в нашем мире вырванных пробелов.",
            "Все, катись отсюда и верни мне моих головастиков!..КВА"
        }, 
            new string[] 
            { 
                "КВА",
            } 
        };

        private string[] _getTadpoleMessage = new string[]
        {
            "Мой Биба вернулся! Спасибо, скорее верни мне оставшихся!",
            "Боба! Радость моя, но где же последний?",
            "О, похоже на триплет! Спасибо тебе, Капля, теперь мы квиты! Ква" 
        };

        private int _indexI = 0;
        private int _indexJ = 0;

        private bool _isReaded;
        private bool _isDropInRange;
        private List<Collider2D> _dropColliders = new List<Collider2D>();

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
        }

        private void Shooted(ProjectileType type, Vector2 projectilePosition, OnShootCallback callback)
        {
            if (_isQuestDone)
                return;

            if (type != ProjectileType.Tadpole)
                return;

            callback?.Invoke();


            if (_tadpoleCount == _neededTadpoleCount - 1)
                StartCoroutine(QuestDone());
            else
            {
                ShowMessage(_getTadpoleMessage[_tadpoleCount]);
                _tadpoleCount++;
            }
        }

        private IEnumerator QuestDone()
        {
            _isQuestDone = true;

            ShowMessage(_getTadpoleMessage[_tadpoleCount]);
            yield return ShowMessageTemporary();

            TurnColliderOffRecursively(transform);

            anim.Play("FrogDown_Anim");
        }

        private void ShowMessage(string message)
        {
            if (!_isDropInRange)
                StartCoroutine(ShowMessageTemporary());

            _messageField.text = message;
        }

        private void TurnColliderOffRecursively(Transform obj)
        {
            for (int i = 0; i < obj.childCount; i++)
                TurnColliderOffRecursively(obj.GetChild(i));

            if (TryGetComponent(out Collider2D collider))
                collider.enabled = false;
        }

        private IEnumerator ShowMessageTemporary()
        {
            _messageBackground.SetActive(true);

            yield return new WaitForSeconds(3);

            _messageBackground.SetActive(_isDropInRange && !_isQuestDone);
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

            if (_indexJ + change == _messages[_indexI].Length - 1)
                _isReaded = transform;

            _indexJ += change;

            ShowMessage(_messages[_indexI][_indexJ]);
        }

        private void ChangeDropRange()
        {
            _messageBackground.SetActive(_isDropInRange && !_isQuestDone);

            if (!_isDropInRange || _isQuestDone)
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

        private void OnDestroy()
        {
            _shootable.OnShooted -= Shooted;
        }
    }
}