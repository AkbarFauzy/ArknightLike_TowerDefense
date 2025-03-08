using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using UnityEngine;

namespace TowerDefence.Observer {
    public abstract class StageSubject : MonoBehaviour
    {
        protected List<IStageObserver> _observers = new List<IStageObserver>();

        public void AddObserver(IStageObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IStageObserver observer)
        {
            _observers.Remove(observer);
        }

        protected void NotifyStageEvents(StageEvents stageEvent)
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnNotify(stageEvent);
            });
        }

        protected void NotifyGeneratingDP(int value)
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnDPGenerated(value);
            });
        }

        protected void NotifyUseDP(int value)
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnDPUsed(value);
            });
        }

        protected void NotifyOperatorEvents(StageCharacterEvents stageCharacterEvent, Operator op)
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnOperatorEvents(stageCharacterEvent, op);
            });
        }

    }
}

