using System.Collections.Generic;
using TowerDefence.Module.Characters;
using UnityEngine;

namespace TowerDefence.Observer {
    public interface IStageObserver
    {
        public void OnNotify(StageEvents stageEvent);
        public void OnDPUsed(int value);
        public void OnDPGenerated(int value);
        public void OnOperatorEvents(StageCharacterEvents stageOperatorEvent, Operator character);
        public void OnEnemyEvents(StageCharacterEvents stageEnemyEvents, Enemy enemy);
    }

}
