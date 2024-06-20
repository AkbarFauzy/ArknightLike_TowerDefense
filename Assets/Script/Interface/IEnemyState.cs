using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    public void EnemyEnterState(Enemy enemy);
    public void EnemyUpdateState(Enemy enemy);
    public void EnemyExitState(Enemy enemy);
}
