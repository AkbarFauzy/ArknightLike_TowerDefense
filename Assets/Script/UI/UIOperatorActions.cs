using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using UnityEngine.UI;
using UnityEngine;

namespace TowerDefence.Module.StageUI {
    public class UIOperatorActions : MonoBehaviour
    {
        [SerializeField] private Button _skillButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Operator _operator;

        public void ToogleOperatorActions(bool showSkillButton = true)
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }

            if (!showSkillButton)
            {
                _skillButton.gameObject.SetActive(false);
            }
        }

        public void ClickOutsideUI()
        {
            /*op.OperatorDetailPanel.ToogleOperatorDetails();
            // change this to use event
            // Stages._stageObserver.cameraManager.SwitchState();
            ToogleOperatorActions();*/
        }

        public void ToogleSkillButtonOn()
        {
            _skillButton.interactable = true;
        }

        public void ToogleSkillButtonOff()
        {
            _skillButton.interactable = false;
        }

        public void SetSkillButtonImage(Sprite img)
        {
            _skillButton.GetComponent<Image>().sprite = img;
        }
    }
}

