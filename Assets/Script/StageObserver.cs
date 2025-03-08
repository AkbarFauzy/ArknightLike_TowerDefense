using System.Collections;
using System.Collections.Generic;
using TowerDefence.Observer;
using TowerDefence.Module.Characters;
using UnityEngine;

public class StageObserver : StageSubject, IStageObserver
{
    protected UIStageDP _stageDP;

    protected Dictionary<StageEvents, System.Action> _stageEventHandlers;

    protected Dictionary<StageCharacterEvents, System.Action<Operator>> _stageOperatorEventHandlers;
    protected Dictionary<StageCharacterEvents, System.Action<Enemy>> _stageEnemyEventHandlers;

    protected void Awake()
    {
        _stageEventHandlers = new Dictionary<StageEvents, System.Action>();
        _stageOperatorEventHandlers = new Dictionary<StageCharacterEvents, System.Action<Operator>>();
        _stageEnemyEventHandlers = new Dictionary<StageCharacterEvents, System.Action<Enemy>>();
    }

    protected void Start()
    {
        _stageDP = GetComponent<UIStageDP>();
    }

    public void OnNotify(StageEvents stageEvent)
    {
        if (_stageEventHandlers.ContainsKey(stageEvent))
        {
            _stageEventHandlers[stageEvent]();
        }
    }

    public void OnOperatorEvents(StageCharacterEvents stageOperatorEvent, Operator op)
    {
        if (_stageOperatorEventHandlers.ContainsKey(stageOperatorEvent))
        {
            _stageOperatorEventHandlers[stageOperatorEvent](op);
        }
    }

    public void OnEnemyEvents(StageCharacterEvents stageEnemyEvent, Enemy enemy)
    {
        if (_stageEnemyEventHandlers.ContainsKey(stageEnemyEvent))
        {
            _stageEnemyEventHandlers[stageEnemyEvent](enemy);
        }
    }

    public void OnDPUsed(int value)
    {
        if (_stageDP != null) {
            _stageDP.SubstractCurrentDP(value);
        }
    }

    public void OnDPGenerated(int value)
    {
        if (_stageDP != null)
        {
            _stageDP.AddCurrentDP(value);
        }
    }
}
