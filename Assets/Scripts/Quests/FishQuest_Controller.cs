using UnityEngine;

using Shooting;


namespace Quest
{
    public class FishQuest_Controller : MonoBehaviour
    {
        [SerializeField] private GameObject _rewardPrefab;
        [SerializeField] private Transform _rewardCreationPoint;

        [SerializeField] private GameObject _tadpole;

        [Space(10)]
        [SerializeField] private SunFlowerQuest_Controller _flowerQuestController;

        private IShootable _shootable;

        private bool _isQuestDone;

        [SerializeField] private GameObject _fish;
        private Animator _animFish;
        private Animator _animAquarium;

        private void Awake()
        {
            _shootable = GetComponentInChildren<IShootable>();

            _shootable.OnShooted += Shooted;

            _animAquarium = GetComponent<Animator>();
            _animFish = _fish.GetComponent<Animator>();
        }

        private void Shooted(ProjectileType type, Vector2 projectilePosition, OnShootCallback callback)
        {
            if (_isQuestDone)
                return;

            if (type != ProjectileType.Knife)
                return;

            QuestDone();
        }

        private void CreateReward()
        {
            Instantiate(_rewardPrefab, _rewardCreationPoint.position, new Quaternion(0, 0, 0, 0));
        }

        private void QuestDone()
        {
            _isQuestDone = true;
            _flowerQuestController.ForcedQuestDone();

            TurnColliderOffRecursively(transform);

            _tadpole.SetActive(false);

            CreateReward();

            _animAquarium.Play("Broken");
            _animFish.Play("FishStand1");
            //запуск рыбы. По окончанию анимации должна включить объект, на котором final 1
        }

        private void TurnColliderOffRecursively(Transform obj)
        {
            for (int i = 0; i < obj.childCount; i++)
                TurnColliderOffRecursively(obj.GetChild(i));

            if (TryGetComponent(out Collider2D collider))
                collider.enabled = false;
        }

        private void OnDestroy()
        {
            _shootable.OnShooted -= Shooted;
        }
    }
}