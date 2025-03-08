using System.Collections.Generic;
using TowerDefence.Module.Characters;
using TowerDefence.Module.Ability;
using UnityEngine.UI;
using UnityEngine;

namespace TowerDefence.Module.StageUI {
    public class UIOperatorActions : StageObserver
    {
        private AbilityHolder _currentAbilityHolder;
        [SerializeField] private Button _skillButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private GameObject _panel;


        protected new void Awake()
        {
            _stageOperatorEventHandlers = new Dictionary<StageCharacterEvents, System.Action<Operator>>() 
            {
                { StageCharacterEvents.CharacterPreDeployed, (op) => { ShowOperatorActionsPanel(op); } },
                { StageCharacterEvents.CharacterDeployed, (op) => { HideOperatorActionsPanel(); } },
                { StageCharacterEvents.CharacterAction, (op) => { OnShowOperatorActionPanel(op); } },
            };
        }

        private void Update()
        {
            if (_currentAbilityHolder == null)
                return;


            if (_currentAbilityHolder.GetAbilityState == AbilityState.ready)
            {
                _skillButton.interactable = true;
            }
            else
            {
                _skillButton.interactable = false;
            }
        }

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

        private void OnShowOperatorActionPanel(Operator op) {
            if (transform.parent == op.transform.parent)
            {
                HideOperatorActionsPanel();
            }
            else
            {
                ShowOperatorActionsPanel(op);
            }
        }

        public void ShowOperatorActionsPanel(Operator op)
        {
            Debug.Log("Operator Action Panel Active");
            SetAbilityHolder(op.GetComponent<AbilityHolder>());
            gameObject.transform.SetParent(op.transform.parent);
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
            _skillButton.onClick.AddListener(() => _currentAbilityHolder.InvokeSkill());
            _panel.SetActive(true);
        }

        public void HideOperatorActionsPanel()
        {
            Debug.Log("Operator Action Panel Disabled");
            gameObject.transform.SetParent(null);
            gameObject.transform.position = new Vector3(0, 0, 0);
            _skillButton.onClick.RemoveAllListeners();
            _panel.SetActive(false);
        }

        public void SetAbilityHolder(AbilityHolder abilityHolder) {
            _currentAbilityHolder = abilityHolder;
            SetSkillIcon();
        }

        public void SetSkillIcon()
        {
            _skillButton.GetComponent<Image>().sprite = _currentAbilityHolder.SelectedAbility.Icon;
        }

        public void SetSkillButtonImage(Sprite img)
        {
            _skillButton.GetComponent<Image>().sprite = img;
        }
    }
}

