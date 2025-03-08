using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using TowerDefence.Module.State;
using UnityEngine;


namespace TowerDefence.Module.Ability {
    public enum AbilityState
    {
        cooldown,
        ready,
        active,
    }
    public class AbilityHolder : MonoBehaviour
    {
        public float current_sp;
        public int selected_ability_index;
        private float sp_regen = 1.0f;
        [SerializeField] private UIProgressBar SPBar;
        [SerializeField] private GameObject _skillReadyIcon;
        [SerializeField] private Ability[] ability = new Ability[3];

        private Operator _op;

        public Ability SelectedAbility { get => ability[selected_ability_index]; }
        public Sprite GetSelectedAbilityIcon { get => ability[selected_ability_index].Icon; }
        public AbilityType GetSelectedAbilityType { get => ability[selected_ability_index].AbilityType; }
        public AbilityState GetAbilityState { get => _currentAbilityState; }

        private AbilityState _currentAbilityState = AbilityState.cooldown;
        private float _activeTimer = 0f;
        private bool _isEnabled = false;

        private void Awake()
        {
            current_sp = ability[selected_ability_index].InitialSP;
            this.enabled = false;
        }

        private void Start()
        {
            _op = this.GetComponent<Operator>();
            _op.SetSkillRange(SelectedAbility.Range);
        }

        private void Update()
        {
            if (_op.CurrentOperatorState is OperatorDeployedState)
            {
                this.enabled = true;
            }

            if (!_isEnabled)
                return;

            switch (_currentAbilityState)
            {
                case AbilityState.cooldown:
                    if (current_sp < SelectedAbility.SPCost)
                    {
                        current_sp += Time.deltaTime * sp_regen;
                        SPBar.SetProgressValues(current_sp / ability[selected_ability_index].SPCost);
                        _skillReadyIcon.SetActive(false);
                    }
                    else
                    {
                        _currentAbilityState = AbilityState.ready;
                        _skillReadyIcon.SetActive(true);
                    }
                    break;

                case AbilityState.active:
                    _activeTimer -= Time.deltaTime;
                    SPBar.SetProgressValues(_activeTimer / SelectedAbility.Duration);
                    if (_activeTimer <= 0f) {
                        _currentAbilityState = AbilityState.cooldown;
                        DeactivateSkill();
                    }
                    break;

                case AbilityState.ready:
                    _skillReadyIcon.SetActive(true);
                    break;

                default:
                    break;
            }
        }

        private void OnEnable()
        {
            _isEnabled = true;
        }

        public void InvokeSkill()
        {
            Debug.Log("Skill Invoke");
            _currentAbilityState = AbilityState.active;
            _op.ToggleSkill(true);
            _activeTimer = SelectedAbility.Duration;
            current_sp = 0;
            SelectedAbility.Activate(_op);
        }

        public void DeactivateSkill()
        {
            _op.ToggleSkill(false);
            SelectedAbility.Deactivate(_op);
        }

    }

}

