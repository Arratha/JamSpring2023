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
        [SerializeField] private Vector3 _rewardCreationPoint;
        [SerializeField] private GameObject _drop;

        private GameObject _reward;

        private List<IPickable> _points;
        private int _pointCount;

        private bool _isEarned;

        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            if (_isEarned && _reward == null)
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
            {
                _isEarned = true;
                CreateReward();
            }
        }

        private void CreateReward()
        {
            _rewardCreationPoint = _drop.transform.position + new Vector3(0,0,10);
            _reward = Instantiate(_rewardPrefab, _rewardCreationPoint, new Quaternion(0, 0, 0, 0));
        }
    }
}