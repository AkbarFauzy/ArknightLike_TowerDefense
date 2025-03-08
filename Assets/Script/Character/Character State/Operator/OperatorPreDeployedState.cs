using TowerDefence.Module.Characters;

namespace TowerDefence.Module.State {

    public class OperatorPreDeployedState : IOperatorState
    {
        public void OperatorEnterState(Operator op)
        {
            op.OnPreDeployed();
        }

        public void OperatorUpdateState(Operator op)
        {
            return;
        }

        public void OperatorExitState(Operator op)
        {

        }


    }

}