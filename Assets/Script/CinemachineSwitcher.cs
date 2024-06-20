using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private CinemachineVirtualCamera stageCamera;
    [SerializeField] private CinemachineVirtualCamera operatorCamera;

    public void SwitchState(GameObject op = null)
    {
        if (op == null)
        {
            stageCamera.Priority = 1;
            operatorCamera.Priority = 0;
        }
        else {
            stageCamera.Priority = 0;
            operatorCamera.Priority = 1;
            operatorCamera.LookAt = op.transform;
            operatorCamera.Follow = op.transform;
        }
    }


}
