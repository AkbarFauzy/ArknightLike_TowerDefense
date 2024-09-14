using TowerDefence.Module.Characters;

namespace TowerDefence.Module.State {

    public class OperatorPreDeployedState : IOperatorState
    {
        public void OperatorEnterState(Operator op)
        {
            op.abilityHolder.SetSkillIcon();


            /*      if (op.UIOperatorAction.gameObject.activeSelf)
                    {
                        return;
                    }
                    op.UIOperatorAction.ToogleOperatorActions(false);*/
        }

        public void OperatorUpdateState(Operator op)
        {
            return;
        }

        public void OperatorExitState(Operator op)
        {
            /* Debug.Log(op);
             if (op.UIOperatorAction.gameObject.activeSelf)
             {
                 return;
             }
             op.UIOperatorAction.ToogleOperatorActions(false);*/
        }


    }

}