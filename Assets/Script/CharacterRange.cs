using System.Collections.Generic;
using TowerDefence.Observer;
using UnityEngine;

namespace TowerDefence.Module.Characters {
    public class CharacterRange : StageObserver
    {
        private new void Awake()
        {
            _stageOperatorEventHandlers = new Dictionary<StageCharacterEvents, System.Action<Operator>>()
            {
                { StageCharacterEvents.CharacterAction, (op) => { OnShowOperatorActionPanel(op); } },
            };
        }

        private void OnShowOperatorActionPanel(Operator op) {
            ShowRangeVisual();
        }

        public void HideRangeVisual()
        {
            for (int i=0; i<transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public void ShowRangeVisual()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}

