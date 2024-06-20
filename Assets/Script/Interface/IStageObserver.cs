using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStageObserver
{
    public void OnNotify(StageEvents stageEvent);
    public void OnDPUsed(int value);
    public void OnDPGenerated(int value);
    public void OnNotifyCharacterSpawn(Character character);
    public void OnUIEvent(StageUIEvents stageUIEvent, Operator character);
}
