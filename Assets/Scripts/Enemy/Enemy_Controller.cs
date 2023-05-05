using UnityEngine;

using Settings.Tags;
using Drop;
using Shooting;


namespace Enemy
{
    public enum AIState { Patrool, Follow, Attack, Return, StandingAngry, Wandering, Stunned }

    public class Enemy_Controller : MonoBehaviour
    {
        private static bool _isAgressive;

        private AIState _currentState = AIState.Patrool;
        private AIState _previousState;

        [SerializeField] private Enemy_MoveController _enemyMove;
        private IShootable _shootable;

        [Space(10)]
        [SerializeField] private ZoneOfPatrool _zoneOfPatrool;

        [Space(10)]
        [SerializeField] private Vector2 _zoneOfSight;
        private float _zoneOfSightMultiplier = 1;
        private const float ZoneOfSightMaxMultiplier = 3;

        [Space(10)]
        [SerializeField] private Vector2 _zoneOfAttack;

        private float _stunTimer;
        private const float StunMaxTimer = 1f;

        private float _attackTimer;
        private const float AttackMaxTimer = 0.5f;

        private float _returnTimer = ReturnMaxTimer;
        private const float ReturnMaxTimer = 5f;

        private Vector2 _followPoint;
        private Drop_Controller _drop;

        private float _wanderingTimer = WanderingMaxTimer;
        private const float WanderingMaxTimer = 1f;
        private Vector2 _wanderingPosition;
        private Vector2? _wanderingTargetPoint;
        private float _wanderingRange = 2;

        private void Awake()
        {
            Inititalize();
        }

        private void Inititalize()
        {
            _shootable = GetComponentInChildren<IShootable>();

            _shootable.OnShooted += Shoot;
        }

        private void FixedUpdate()
        {
            State();
        }

        private void State()
        {
            switch (_currentState)
            {
                case AIState.Patrool:

                    if (!_zoneOfPatrool.IsInZone(_enemyMove.transform.position))
                    {
                        GoReturn();
                        break;
                    }

                    if (CheckForDrop())
                    {
                        GoFollow();
                        break;
                    }

                    _enemyMove.MoveTo(_zoneOfPatrool.GetPoint(_enemyMove.transform.position.y), _zoneOfPatrool.ChangePoint);

                    break;
                case AIState.Follow:

                    if (CheckForDropInAttackRange())
                    {
                        GoAttack();
                        break;
                    }

                    if (!_zoneOfPatrool.IsInZone(_enemyMove.transform.position))
                    {
                        GoAngry();
                        break;
                    }

                    if (!CheckForDrop())
                    {
                        GoPatrool();
                        break;
                    }

                    _enemyMove.MoveTo(_followPoint);

                    break;
                case AIState.Attack:

                    _enemyMove.Stop();

                    _attackTimer = Mathf.Max(_attackTimer - Time.deltaTime, 0);

                    if (_stunTimer <= 0)
                    {
                        if (CheckForDropInAttackRange())
                            DoDamage();

                        ChangeState(_previousState);
                    }

                    break;
                case AIState.Return:

                    if (_zoneOfPatrool.IsInZone(_enemyMove.transform.position))
                    {
                        GoPatrool();
                        break;
                    }

                    if (CheckForDrop())
                    {
                        GoFollow();
                        break;
                    }

                    _returnTimer = Mathf.Max(_returnTimer - Time.deltaTime, 0);

                    if (_returnTimer <= 0)
                    {
                        GoWander();
                        break;
                    }

                    _enemyMove.MoveTo(_zoneOfPatrool.GetZoneCenter(_enemyMove.transform.position.y));

                    break;
                case AIState.StandingAngry:

                    if (CheckForDropInAttackRange())
                    {
                        GoAttack();
                        break;
                    }

                    if (!CheckForDrop())
                    {
                        GoReturn();
                        break;
                    }

                    _enemyMove.Stop();

                    break;
                case AIState.Wandering:

                    if (CheckForDropInAttackRange())
                    {
                        GoAttack();
                        break;
                    }

                    if (_wanderingTargetPoint == null)
                    {
                        Vector2 offset = Vector2.right * _wanderingRange * Random.Range(-1f, 1f);
                        _wanderingTargetPoint = _wanderingPosition + offset;
                                
                        _wanderingTimer = WanderingMaxTimer;
                    }

                    _wanderingTimer = Mathf.Max(_wanderingTimer - Time.deltaTime, 0);

                    _enemyMove.MoveTo((Vector2)_wanderingTargetPoint, () => _wanderingTargetPoint = null );

                    if (_wanderingTimer <= 0)
                        _wanderingTargetPoint = null;

                    break;
                case AIState.Stunned:

                    _stunTimer = Mathf.Max(_stunTimer - Time.deltaTime, 0);

                    if (_stunTimer <= 0)
                        ChangeState(_previousState);

                    break;
            }
        }

        private void ChangeState(AIState state)
        {
            _previousState = _currentState;
            _currentState = state;
        }

        private void Shoot(ProjectileType type, Vector2 projectilePosition, OnShootCallback callback = null)
        {
            callback?.Invoke();

            switch (type)
            {
                case ProjectileType.DropOfWater:

                    Stun(projectilePosition);

                    break;
                case ProjectileType.Knife:

                    _isAgressive = true;

                    Destroy(gameObject);

                    break;
            }
        }

        private void Stun(Vector2 projectilePosition)
        {
            if (_currentState == AIState.Stunned)
                return;

            ChangeState(AIState.Stunned);

            _stunTimer = StunMaxTimer;

            _enemyMove.JumpOut(projectilePosition);
        }

        private void DoDamage()
        {
            _drop.DropCondition.DoDamage(_enemyMove.transform.position);
        }

        private bool CheckForDrop()
        {
            if (!_isAgressive)
                return false;

            RaycastHit2D[] hit = Physics2D.BoxCastAll(_enemyMove.transform.position,
               _zoneOfSight * _zoneOfSightMultiplier, 0, Vector2.right, 0);

            int index = -1;
            float distance = Mathf.Infinity;

            for (int i = 0; i < hit.Length; i++)
                if (hit[i].collider.CompareTag(Tags.DropTag))
                    if (Vector2.Distance(_enemyMove.transform.position, hit[i].point) < distance)
                    {
                        distance = Vector2.Distance(_enemyMove.transform.position, hit[i].point);
                        index = i;
                    }

            if (index != -1)
            {
                _followPoint = hit[index].point;

                return true;
            }

            return false;
        }

        private bool CheckForDropInAttackRange()
        {
            if (!_isAgressive)
                return false;

            RaycastHit2D[] hit = Physics2D.BoxCastAll(_enemyMove.transform.position + Vector3.right
                * (_enemyMove.collider.bounds.size.x / 2 + _zoneOfAttack.x / 2) * Mathf.Sign(_enemyMove.transform.localScale.x), _zoneOfAttack, 0, Vector2.right, 0);

            int index = -1;
            float distance = Mathf.Infinity;

            for (int i = 0; i < hit.Length; i++)
                if (hit[i].collider.CompareTag(Tags.DropTag))
                    if (Vector2.Distance(_enemyMove.transform.position, hit[i].point) < distance)
                    {
                        distance = Vector2.Distance(_enemyMove.transform.position, hit[i].point);
                        index = i;
                    }

            if (index != -1)
            {
                _drop = hit[index].collider.gameObject.GetComponentInParent<Drop_Controller>();
                return true;
            }

            return false;
        }

        private void GoPatrool()
        {
            _zoneOfSightMultiplier = 1;
            _returnTimer = ReturnMaxTimer;

            ChangeState(AIState.Patrool);
        }

        private void GoFollow()
        {
            _zoneOfSightMultiplier = ZoneOfSightMaxMultiplier;

            ChangeState(AIState.Follow);
        }

        private void GoReturn()
        {
            _zoneOfSightMultiplier = 1;

            ChangeState(AIState.Return);
        }

        private void GoAngry()
        {
            ChangeState(AIState.StandingAngry);
        }

        private void GoWander()
        {
            _zoneOfSightMultiplier = 1;

            _wanderingTimer = WanderingMaxTimer;
            _wanderingPosition = _enemyMove.transform.position;

            ChangeState(AIState.Wandering);
        }

        private void GoAttack()
        {
            _attackTimer = AttackMaxTimer;

            ChangeState(AIState.Attack);
        }

        private void OnDestroy()
        {
            _shootable.OnShooted -= Shoot;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_zoneOfPatrool.GetZoneCenter(), _zoneOfPatrool.GetZoneSize());

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_enemyMove.transform.position, _zoneOfSight * _zoneOfSightMultiplier);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_enemyMove.transform.position + Vector3.right
                * (_enemyMove.collider.bounds.size.x / 2 + _zoneOfAttack.x / 2) * Mathf.Sign(_enemyMove.transform.localScale.x), _zoneOfAttack);
        }

        [System.Serializable]
        private class ZoneOfPatrool
        {
            [SerializeField] private Transform _pointA;
            [SerializeField] private Transform _pointB;

            private bool _isPointA = true;

            public Vector2 GetPoint(float y)
            {
                Vector2 result = (_isPointA) ? _pointA.transform.position : _pointB.transform.position;
                result.y = y;

                return result;
            }

            public void ChangePoint()
            {
                _isPointA = !_isPointA;
            }

            public Vector2 GetZoneCenter(float? y = null)
            {
                Vector2 result = Vector2.zero;
                result.x = _pointA.transform.position.x - (_pointA.transform.position.x - _pointB.transform.position.x) / 2;
                result.y = _pointA.transform.position.y - (_pointA.transform.position.y - _pointB.transform.position.y) / 2;

                if (y != null)
                    result.y = (float)y;

                return result;
            }

            public Vector3 GetZoneSize()
            {
                Vector2 result = Vector2.zero;
                result.x = Mathf.Abs(_pointA.transform.position.x - _pointB.transform.position.x);
                result.y = Mathf.Abs(_pointA.transform.position.y - _pointB.transform.position.y);

                return result;
            }

            public bool IsInZone(Vector2 position)
            {
                Vector2 minPoint = new Vector2(Mathf.Min(_pointA.transform.position.x, _pointB.transform.position.x), Mathf.Min(_pointA.transform.position.y, _pointB.transform.position.y));
                Vector2 maxPoint = new Vector2(Mathf.Max(_pointA.transform.position.x, _pointB.transform.position.x), Mathf.Max(_pointA.transform.position.y, _pointB.transform.position.y));

                if (minPoint.x - position.x > 0 || minPoint.y - position.y > 0)
                    return false;

                if (maxPoint.x - position.x < 0 || maxPoint.y - position.y < 0)
                    return false;

                return true;
            }
        }
    }
}