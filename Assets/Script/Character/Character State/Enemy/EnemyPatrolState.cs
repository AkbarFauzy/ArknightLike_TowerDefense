using TowerDefence.Module.Characters;

namespace TowerDefence.Module.State{
    public class EnemyPatrolState : IEnemyState
    {
        public void EnemyEnterState(Enemy enemy)
        {
            enemy.Patrol();
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

            if (enemy.Targets.Count == 0)
            {
                enemy.ToggleAttackingAnimation(false);
                return;
            }

            if (enemy.Targets.Count != 0)
            {
                if (enemy.Targets[0] == null)
                {
                    enemy.Targets.RemoveAt(0);
                    enemy.IsBlocked = false;
                    return;
                }

                if (!enemy.IsAttacking)
                {
                    enemy.ToggleAttackingAnimation(true);
                }
            }
        }
    }

}
