using TowerDefence.Module.Characters;

namespace TowerDefence.Module.State {
    public interface IOperatorState
    {
        public void OperatorEnterState(Operator op);
        public void OperatorUpdateState(Operator op);
        public void OperatorExitState(Operator op);
    }
}

