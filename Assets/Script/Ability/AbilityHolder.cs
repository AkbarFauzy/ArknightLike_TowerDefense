using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using TowerDefence.Module.State;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

namespace TowerDefence.Module.Ability {
    public class AbilityHolder : MonoBehaviour
    {
        public float current_sp;
        public int selected_ability_index;
        private float sp_regen = 1.0f;
        [SerializeField] private ProgressBar SPBar;
        [SerializeField] private Ability[] ability = new Ability[3];
        [SerializeField] private Button activate_button;

        private bool isActivate = false;
        private Operator op;

        public Ability SelectedAbility { get => ability[selected_ability_index]; }
        public Sprite GetSelectedAbilityIcon { get => ability[selected_ability_index].Icon; }
        public AbilityType GetSelectedAbilityType { get => ability[selected_ability_index].AbilityType; }

        enum AbilityState
        {
            cooldown,
            ready,
            active,
        }

        private void Awake()
        {
            current_sp = ability[selected_ability_index].InitialSP;
            this.enabled = false;
        }

        private void Start()
        {
            SetSkillIcon();
            op = this.GetComponent<Operator>();
            op.SetSkillRange(SelectedAbility.Range);
        }

        private void Update()
        {
            if (op.CurrentOperatorState is OperatorDeployedState)
            {
                this.enabled = true;
            }
        }


        private void OnEnable()
        {
            StartCoroutine(StartSPGeneration());
        }

        private IEnumerator StartSPGeneration()
        {
            while (true)
            {
                if (!isActivate)
                {
                    if (current_sp >= SelectedAbility.SPCost)
                    {
                        activate_button.interactable = true;
                    }
                    else
                    {
                        activate_button.interactable = false;
                        current_sp += sp_regen;
                        SPBar.SetProgressValues(current_sp / ability[selected_ability_index].SPCost);
                    }

                }
                yield return new WaitForSeconds(1.0f);
            }
        }

        public void InvokeSkill()
        {
            op.ToggleSkill(true);
            StartCoroutine(ActivateSkill());
        }

        public void DeactivateSkill()
        {
            op.ToggleSkill(false);
            SelectedAbility.Deactivate();
        }

        public IEnumerator ActivateSkill()
        {
            if (current_sp < SelectedAbility.SPCost && !isActivate)
            {
                yield return null;
            }
            current_sp = 0;
            StartCoroutine(StartTimer());
            SelectedAbility.Activate();
            yield return new WaitUntil(() => !isActivate);
            DeactivateSkill();
        }

        public void PlayVFX()
        {
            SelectedAbility.SpawnVFX();
        }

        public void SetSkillIcon()
        {
            activate_button.GetComponent<Image>().sprite = SelectedAbility.Icon;
        }

        public IEnumerator StartTimer()
        {
            isActivate = true;
            yield return new WaitForSeconds(SelectedAbility.Duration);
            isActivate = false;
        }


    }

}

