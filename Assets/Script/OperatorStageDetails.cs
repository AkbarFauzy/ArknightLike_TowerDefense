using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using UnityEngine;
using TMPro;

namespace TowerDefence.Module.StageUI {
    public class OperatorStageDetails : MonoBehaviour
    {
        [SerializeField] private GameObject UIOperatorDetailPanel;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailName;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailATK;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailDEF;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailRES;
        [SerializeField] private TextMeshProUGUI UIOperatorDetailBLOCK;

        /*    [SerializeField] private ProgressBar UIOperatorDetailHealthBar;
            [SerializeField] private ProgressBar UIOperatorDetailHealthValue;*/

        public bool IsOperatorDetailOpen { get => UIOperatorDetailPanel.activeSelf; }

        public void ToogleOperatorDetails(Operator op = null)
        {
            if (op == null || UIOperatorDetailPanel.activeSelf)
            {
                UIOperatorDetailPanel.SetActive(false);
            }
            else
            {
                UIOperatorDetailPanel.SetActive(true);
                UIOperatorDetailName.text = op.CharacterName;
                UIOperatorDetailATK.text = op.CurrentATK.ToString();
                UIOperatorDetailDEF.text = op.CurrentDEF.ToString();
                UIOperatorDetailRES.text = op.CurrentRES.ToString();
                UIOperatorDetailBLOCK.text = op.BlockCount.ToString();
            }
        }

    }
}


