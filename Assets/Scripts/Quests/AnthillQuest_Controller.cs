using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Drop;
using Pickables;


namespace Quest
{
    public class AnthillQuest_Controller : MonoBehaviour
    {
        [SerializeField] private GameObject _rewardPrefab;
        [SerializeField] private Transform _rewardCreationPoint;

        private GameObject _reward;

        private List<IPickable> _points;
        private int _pointCount;

        private bool _isQuestDone;

        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            CreateReward();
        }

        private void Initialize()
        {
            _points = GetComponentsInChildren<IPickable>().ToList();
            _pointCount = _points.Count;

            foreach (var currentPoint in _points)
                currentPoint.OnPicked += GetPoint;
        }

        private void GetPoint(Drop_Controller drop, IPickable sender)
        {
            _points.Remove(sender);
            Destroy(sender.gameObject);

            drop.DropMessage.ShowMessage($"{_pointCount - _points.Count}/{_pointCount}");

            if (_points.Count == 0)
                _isQuestDone = true;
        }

        private void CreateReward()
        {
            if (_isQuestDone && _reward == null)
                _reward = Instantiate(_rewardPrefab, _rewardCreationPoint.position, new Quaternion(0, 0, 0, 0));
        }
    }
}