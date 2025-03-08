using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.State;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefence.Module.Characters {
    public abstract class Enemy : Character
    {
        protected IEnemyState _currentState;
        public IEnemyState CurrentCharacterState { get => _currentState; }
        public EnemyStandbyState StandbyState = new EnemyStandbyState();
        public EnemyPatrolState PatrolState = new EnemyPatrolState();

        [SerializeField] private float speed;
        private PathFinder pathFinder;

        protected bool _isBlocked;
        public bool IsBlocked { get => _isBlocked; set => _isBlocked = value; }

        private new void Awake()
        {
            _currentState = StandbyState;
            _currentState.EnemyEnterState(this);
        }

        protected override void Start()
        {
            base.Start();
            _isBlocked = false;
            if (pathFinder == null)
            {
                pathFinder = transform.parent.parent.GetComponent<PathFinder>();
            }

            SwitchState(PatrolState);
        }

        private void Update()
        {
            if (_currentState != null)
            {
                _currentState.EnemyUpdateState(this);
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.CompareTag("Operator"))
            {
                Operator op = collision.gameObject.GetComponentInChildren<Operator>();
                this.Targets.Remove(op);

                _isBlocked = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Operator"))
            {
                if (!collision.gameObject.GetComponentInChildren<Operator>().MaxedOutBlock())
                {
                    _isBlocked = true;
                }
                Debug.Log("ENEMY HIT AN OPERATOR");
            }


            if (collision.gameObject.CompareTag("PlayerBase"))
            {
                Debug.Log("Hit Base");
                NotifyStageEvents(StageEvents.TakeBaseDamage);
                NotifyStageEvents(StageEvents.EnemyDied);
                transform.parent.gameObject.SetActive(false);
            }

        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Operator"))
            {
                Operator op = other.gameObject.GetComponentInChildren<Operator>();
                if (!this.Targets.Contains(op))
                {
                    this.Targets.Add(op);
                }
            }
        }


        public void Patrol()
        {
            var paths = pathFinder.GetPath();
            StartCoroutine(FollowPath(paths));
        }

        private IEnumerator FollowPath(List<Waypoint> paths)
        {
            foreach (Waypoint path in paths)
            {
                yield return StartCoroutine(MoveToWaypoint(path));
            }
            yield return null;
        }

        public IEnumerator MoveToWaypoint(Waypoint path)
        {
            Vector3 gridPos = new Vector3(path.GetGridPos().x, transform.position.y, path.GetGridPos().y);

            while (Vector3.Distance(transform.position, gridPos) > 0.001f && !_isDead)
            {
                Vector3 direction = gridPos - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f * Time.deltaTime);

                float step = speed * Time.deltaTime;
                transform.parent.position = Vector3.MoveTowards(transform.position, gridPos, step);

                yield return new WaitUntil(() => !IsBlocked || !IsAttacking);
            }

            yield return null;
        }


        public void SwitchState(IEnemyState state)
        {
            _currentState.EnemyExitState(this);

            if (state == null)
            {
                return;
            }

            _currentState = state;
            _currentState.EnemyEnterState(this);
        }

        public override void OnDied()
        {
            if (!_isDead)
            {
                _isDead = true;
                StartCoroutine(OnDiedAnimation());
                NotifyStageEvents(StageEvents.EnemyDied);
            }
        }

        public IEnumerator OnDiedAnimation()
        {
            _anim.SetTrigger("OnDied");
            yield return new WaitForSeconds(2.0f);
            Destroy(this.transform.parent.gameObject);
        }
    }
}

