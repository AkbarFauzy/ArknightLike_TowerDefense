using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOperatorState
{
    public void OperatorEnterState(Operator op);
    public void OperatorUpdateState(Operator op);
    public void OperatorExitState(Operator op);
}
