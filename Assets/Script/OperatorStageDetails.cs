using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace TowerDefence.Module.StageUI {
    public class OperatorStageDetails : StageObserver
    {
        private Operator _operatorReference;

        [SerializeField] private GameObject UIOperatorDetailPanel;
        [SerializeField] private Image UIOperatorDetailClassIcon;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailName;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailATK;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailDEF;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailRES;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailBLOCK;

        [SerializeField] private UIProgressBar UIOperatorDetailHealthBar;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailHealthValueText;

        [SerializeField] private Image UIOperatorDetailSkillIcon;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailSkillNameText;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailSkillDescriptionText;


        public bool IsOperatorDetailOpen { get => UIOperatorDetailPanel.activeSelf && _operatorReference != null; }

        protected new void Awake()
        {
            base.Awake();
            _stageOperatorEventHandlers = new Dictionary<StageCharacterEvents, System.Action<Operator>>()
            {
                { StageCharacterEvents.CharacterDetails, (op) => {ToogleOperatorDetails(op); } },
            };
        }

        private new void Start()
        {
            base.Start();
            UIOperatorDetailPanel.SetActive(false);
        }


        private void LateUpdate()
        {
            if (_operatorReference != null) {
                UIOperatorDetailName.text = _operatorReference.CharacterName;
                UIOperatorDetailATK.text = _operatorReference.CurrentATK.ToString();
                UIOperatorDetailDEF.text = _operatorReference.CurrentDEF.ToString();
                UIOperatorDetailRES.text = _operatorReference.CurrentRES.ToString();
                UIOperatorDetailBLOCK.text = _operatorReference.BlockCount.ToString();

                UIOperatorDetailHealthBar.SetProgressValues(_operatorReference.CurrentHP/_operatorReference.BaseHP);
                UIOperatorDetailHealthValueText.text = string.Format(_operatorReference.CurrentHP.ToString()+ " / " + _operatorReference.BaseHP.ToString());

                UIOperatorDetailSkillIcon.sprite = _operatorReference.AbilityHolder.GetSelectedAbilityIcon;
                UIOperatorDetailSkillNameText.text = _operatorReference.AbilityHolder.SelectedAbility.SkillName.ToString();
                UIOperatorDetailSkillDescriptionText.text = _operatorReference.AbilityHolder.SelectedAbility.Description.ToString();
            }
        }

        public void SetOperator(Operator op)
        {
            _operatorReference = op;
        }

        private void ToogleOperatorDetails(Operator op)
        {
            if (_operatorReference == null || _operatorReference != op)
            {
                _operatorReference = op;
                UIOperatorDetailPanel.SetActive(true);
            }
            else {
                _operatorReference = null;
                UIOperatorDetailPanel.SetActive(false);
            }
        }
    }
}


