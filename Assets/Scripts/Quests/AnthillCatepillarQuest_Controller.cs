using UnityEngine;

using Enemy;


namespace Quest
{
    public class AnthillCatepillarQuest_Controller : MonoBehaviour
    {
        [SerializeField] private GameObject _catepillar;

        [Space(10)]
        [SerializeField] private Enemy_Controller[] _ants;

        private int _antsCount;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _antsCount = _ants.Length;

            foreach (var ant in _ants)
                ant.OnDeath += KillAnt;
        }

        private void KillAnt()
        {
            _antsCount--;

            if (_antsCount == 0)
                _catepillar.SetActive(true);
        }
    }
}