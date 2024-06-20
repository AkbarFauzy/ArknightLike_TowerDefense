using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorDeployedState : IOperatorState
{
    public void OperatorEnterState(Operator op)
    {
        var colliders = op.gameObject.GetComponentsInChildren<BoxCollider>();
        op.UIOperatorAction.ToogleOperatorActions();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
        op.abilityHolder.enabled = true;
/*        op.ToggleSkillRange(false);*/
        op.stageCard.OnDeployed();
    }
    public void OperatorUpdateState(Operator op)
    {
        if (op.CurrentHP <= 0)
        {
            OperatorExitState(op);
        }

        if (op.targets.Count == 0) {
            op.ToggleAttackingAnimation(false);
            return;
        }

        if (op.targets[0] == null || op.targets[0].CurrentHP <= 0) {
            op.targets.RemoveAt(0);
            op.ToggleAttackingAnimation(false);
            return;
        }

        if (op.IsSkill) { 
        
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
