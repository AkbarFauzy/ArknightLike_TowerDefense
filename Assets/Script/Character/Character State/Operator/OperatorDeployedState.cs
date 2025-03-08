using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using UnityEngine;

namespace TowerDefence.Module.State {
    public class OperatorDeployedState : IOperatorState
    {
        public void OperatorEnterState(Operator op)
        {
            op.OnDeployed();
        }
        public void OperatorUpdateState(Operator op)
        {
            if (op.CurrentHP <= 0)
            {
                OperatorExitState(op);
            }

            if (op.Targets.Count == 0)
            {
                op.ToggleAttackingAnimation(false);
                return;
            }

            if (op.Targets[0] == null || op.Targets[0].CurrentHP <= 0)
            {
                op.Targets.RemoveAt(0);
                op.ToggleAttackingAnimation(false);
                return;
            }

            if (op.IsSkill)
            {

            }

            if (!op.IsAttacking)
            {
                op.ToggleAttackingAnimation(true);
            }
        }

        public void OperatorExitState(Operator op)
        {
            op.OnDied();
        }
    }
}

