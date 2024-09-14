using TowerDefence.Module.Characters;


namespace TowerDefence.Module.State {
    public interface IEnemyState
    {
        public void EnemyEnterState(Enemy enemy);
        public void EnemyUpdateState(Enemy enemy);
        public void EnemyExitState(Enemy enemy);
    }
}

