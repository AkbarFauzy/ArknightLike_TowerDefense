using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : IEnemyState
{
    public void EnemyEnterState(Enemy enemy)
    {
        enemy.patrol();
    }

    public void EnemyExitState(Enemy enemy)
    {
        enemy.OnDied();
    }

    public void EnemyUpdateState(Enemy enemy)
    {
        if (enemy.CurrentHP <= 0)
        {
            EnemyExitState(enemy);
            return;
        }

        if (!enemy.IsAttacking && enemy.IsBlocked && enemy.targets.Count != 0)
        {
            if (enemy.targets[0] == null)
            {
                enemy.targets.RemoveAt(0);
                enemy.IsBlocked = false;
            }

            if (!enemy.IsSkill)
            {
                enemy.ToggleAttackingAnimation(true);
            }
        }
    }
}
