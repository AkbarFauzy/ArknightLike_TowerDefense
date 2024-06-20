using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIOperatorActions : MonoBehaviour
{
    [SerializeField] private Button skillButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Operator op;

    public void ToogleOperatorActions(bool showSkillButton=true)
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }

        if (!showSkillButton) {
            skillButton.gameObject.SetActive(false);
        }
    }

    public void ClickOutsideUI() {
        /*op.OperatorDetailPanel.ToogleOperatorDetails();
        // change this to use event
        // Stages._stageObserver.cameraManager.SwitchState();
        ToogleOperatorActions();*/
    }

    public void ToogleSkillButtonOn() {
        skillButton.interactable = true;
    }

    public void ToogleSkillButtonOff() {
        skillButton.interactable = false;
    }

    public void SetSkillButtonImage(Sprite img) {
        skillButton.GetComponent<Image>().sprite = img;
    }
}
