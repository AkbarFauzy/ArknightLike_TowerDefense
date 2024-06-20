using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected IEnemyState _currentState;
    public IEnemyState CurrentCharacterState { get => _currentState; }

    public EnemyStandbyState StandbyState = new EnemyStandbyState();
    public EnemyPatrolState PatrolState = new EnemyPatrolState();

    private bool _isBlocked;
    [SerializeField] private float speed;
    private PathFinder pathFinder;

    public bool IsBlocked { get => _isBlocked; set => _isBlocked = value; }

    private void Awake()
    {
        _currentState = StandbyState;
        _currentState.EnemyEnterState(this);
    }

    private new void Start()
    {
        base.Start();
        _isBlocked = false;
        if (pathFinder == null) {
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

    public void patrol() {
        var paths = pathFinder.GetPath();
        StartCoroutine(FollowPath(paths));
    }

    private IEnumerator FollowPath(List<Waypoint> paths)
    {
        foreach (Waypoint path in paths) {
            yield return StartCoroutine(MoveToWaypoint(path));
        }
        yield return null;
    }


    public IEnumerator MoveToWaypoint(Waypoint path)
    {
        Vector3 gridPos = new Vector3(path.GetGridPos().x, transform.position.y, path.GetGridPos().y);

        while (Vector3.Distance(transform.position, gridPos) > 0.001f)
        {
            Vector3 direction = gridPos - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f * Time.deltaTime);

            float step = speed * Time.deltaTime;
            transform.parent.position = Vector3.MoveTowards(transform.position, gridPos, step);

            yield return new WaitUntil(() => !_isBlocked);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Operator"))
        {
            Operator op = collision.gameObject.GetComponentInChildren<Operator>();
            this.targets.Remove(op);

            _isBlocked = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Operator")) {
            if (!collision.gameObject.GetComponentInChildren<Operator>().MaxedOutBlock())
            {
                _isBlocked = true;
            }
            Debug.Log("ENEMY HIT AN OPERATOR");
        }

        if (collision.gameObject.CompareTag("PlayerBase"))
        {
            Debug.Log("Hit Base");
            NotifyObserver(StageEvents.TakeBaseDamage);
            NotifyObserver(StageEvents.EnemyDied);
            StartCoroutine(OnDiedAnimation());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Operator"))
        {
            Operator op = other.gameObject.GetComponentInChildren<Operator>();
            if (!this.targets.Contains(op))
            {
                this.targets.Add(op);
            }
        }
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


    public void OnDied()
    {
        if (!_isDead) {
            _isDead = true;
            StartCoroutine(OnDiedAnimation());
            NotifyObserver(StageEvents.EnemyDied);
        }
    }

    public IEnumerator OnDiedAnimation() {
        anim.SetTrigger("OnDied");
        yield return new WaitForSeconds(2.0f);
        Destroy(this.transform.parent.gameObject);
    }
}
