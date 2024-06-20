using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operator : Character
{
    protected IOperatorState _currentState;
    public IOperatorState CurrentOperatorState { get => _currentState; }

    public OperatorPreDeployedState PreDeployedState = new OperatorPreDeployedState();
    public OperatorDeployedState DeployedState = new OperatorDeployedState();

    [Header("Operator Stats")]
    public int cost;
    [SerializeField] [Range(1,6)] private int rarity;
    [SerializeField] private int lvl;
    [SerializeField] private int blockedEnemy;
    [SerializeField] private float _redeployTime;

    [Header("Operator UI")]
    [HideInInspector] public StageCard stageCard;
    public UIOperatorActions UIOperatorAction;

    private CubeEditor _deployed_grid;

    public bool MaxedOutBlock() => BlockCount <= blockedEnemy;

    private void Awake()
    {
        _currentState = PreDeployedState;
        _currentState.OperatorEnterState(this);
    }

    private new void Start()
    {
        base.Start();
        cost = characterStats.cost;

        if (TryGetComponent<AbilityHolder>(out AbilityHolder holder))
        {
            abilityHolder = holder;
        }
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.OperatorUpdateState(this);
        }
    }

    public void SwitchState(IOperatorState state)
    {
        _currentState.OperatorExitState(this);
        
        if (state == null) {
            return;
        }

        _currentState = state;
        _currentState.OperatorEnterState(this);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") && !MaxedOutBlock())
        {
            blockedEnemy += other.gameObject.GetComponent<Enemy>().BlockCount;
            other.gameObject.GetComponent<Enemy>().IsBlocked = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Character enemy = other.gameObject.GetComponentInChildren<Enemy>();
            if (!this.targets.Contains(enemy))
            {
                this.targets.Add(enemy);
            }
         
            if (enemy.CurrentHP <= 0)
            {
                blockedEnemy -= enemy.BlockCount;
                other.enabled = false;
            }
        }

        if (CurrentHP <= 0) {
            other.GetComponentInChildren<Enemy>().IsBlocked = false;
        }

    }

    public void SetGrid(CubeEditor grid) {
        _deployed_grid = grid;
    }

    private void OnTriggerExit(Collider other)
    {
        Character enemy = other.gameObject.GetComponentInChildren<Enemy>();
        this.targets.Remove(enemy);
    }

    public void OnDied()
    {
        _deployed_grid.isPlaceable = true;
        cost += Mathf.RoundToInt((float) cost * 1.5f);
        var colliders = gameObject.GetComponentsInChildren<BoxCollider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        StartCoroutine(OnDiedAnimation());
    }

    private IEnumerator OnDiedAnimation()
    {
        anim.SetTrigger("OnDied");
        yield return new WaitForSeconds(2.0f);
        Destroy(this.transform.parent.gameObject);
    }

    public void OnMouseDown()
    {
        NotifyUIObserver(StageUIEvents.CharacterDetails, this);
        NotifyUIObserver(StageUIEvents.CharacterAction, this);
    }

    public void InvokeCancel() {
        Destroy(this.gameObject.transform.parent);
    }
}
