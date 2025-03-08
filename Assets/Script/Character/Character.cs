using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Observer;
using TowerDefence.Module.Ability;

public enum AttackType
{
    Ranged,
    Ranged_AOE,
    Melee,
    Melee_AOE,
    Multi,
};

public enum AttackDamageType { 
    True_Damage, 
    Physical_Damage, 
    Magic_Damage
};

namespace TowerDefence.Module.Characters {
    public abstract class Character : StageSubject, IDamageable, IHaveAbility
    {
        protected Animator _anim;

        [Space]
        [Header("Character Stats")]
        public CharacterStats CharacterStats;
        
        [Space]
        [Header("Character Canvas")]
        [SerializeField] private UIProgressBar Healthbar;
        
        private AttackDamageType _atkDamageType;

        public float CurrentHP { get; set; }
        public int CurrentATK { get; set; }
        public int CurrentDEF { get; set; }
        public int CurrentRES { get; set; }
        public float CurrentASPD { get; set; }
        public float CurrentSP { get; set; }
        public int CurrentCost { get; set; }

        public int NumberOfAttackedTarget { get; set; }

/*        private float _sp_regen = 1.0f;*/
        private int _blockCount;
        private bool _isAttacking;
        private bool _isSkill;
        protected bool _isDead;

        #region
        public string CharacterName { get => CharacterStats.characterName; }
        public float BaseHP { get => CharacterStats.baseHP; }
        public int BaseATK { get => CharacterStats.baseATK; }
        public int BaseDEF { get => CharacterStats.baseDEF; }
        public int BaseRES { get => CharacterStats.baseRES; }
        public int BaseCost { get => CharacterStats.cost; }
        public float ASPD { get => CharacterStats.attackSpeed; }
        public int BaseNumberOfAttackedTarget { get => CharacterStats.numberOfTarget; }
        public bool IsAttacking { get => _isAttacking; }
        public bool IsSkill { get => _isSkill; }
        public bool IsFullHealth { get => CurrentHP >= BaseHP; }
        public int BlockCount { get => _blockCount; set => _blockCount = value; }


        #endregion

        public List<Character> Targets { get; private set; }

        [Space]
        [Header("Ability")]
        public AbilityHolder AbilityHolder;
        [SerializeField] protected CharacterRange _attack_range;
        private GameObject _skill_range;

        private int _flat_def_down;
        private float _scaling_def_down;
        private float _phys_dmg_taken_up;

        private int _flat_res_down;
        private float _scaling_res_down;
        private float _magic_dmg_taken_up;

        private float _atk_interval = 1f;

        public abstract void OnDied();

        protected virtual void Start()
        {
            _flat_def_down = 0;
            _scaling_def_down = 0.0f;
            _phys_dmg_taken_up = 1.0f;

            _flat_res_down = 0;
            _scaling_res_down = 0.0f;
            _magic_dmg_taken_up = 1.0f;

            _atkDamageType = CharacterStats.atkType;
            CurrentHP = CharacterStats.baseHP;
            CurrentATK = CharacterStats.baseATK;
            CurrentDEF = CharacterStats.baseDEF;
            CurrentRES = CharacterStats.baseRES;
            CurrentASPD = CharacterStats.attackSpeed;
            BlockCount = CharacterStats.numberOfBlock;
            NumberOfAttackedTarget = CharacterStats.numberOfTarget;
            Healthbar.SetProgressValues(CurrentHP / CharacterStats.baseHP);

            Targets = new List<Character>();
            _anim = GetComponentInChildren<Animator>();
            
            CurrentCost = CharacterStats.cost;

            if (TryGetComponent(out AbilityHolder holder))
            {
                AbilityHolder = holder;
            }
        }

        public void ToggleAttackingAnimation(bool val)
        {
            StartCoroutine(OnAttackingAnimation(val));   
        }

        private IEnumerator OnAttackingAnimation(bool val)
        {
            _isAttacking = val;
            _anim.SetBool("isAttacking", val);
            yield return new WaitForSeconds(_atk_interval);
        }

        public void SetStats(CharacterStats stats)
        {
            CharacterStats = stats;
        }

        public void SetSkillRange(GameObject range)
        {
            if (range == null)
            {
                _skill_range = _attack_range.gameObject;
                return;
            }

            _skill_range = Instantiate(range);
            _skill_range.transform.SetParent(this.transform);
            _skill_range.transform.localPosition = new Vector3(0f, 0f, 0f);
            _skill_range.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            var colliders = _skill_range.GetComponentsInChildren<BoxCollider>();
            foreach (var collider in colliders)
            {
                collider.enabled = true;
            }
            _skill_range.SetActive(false);
        }

        public void ToggleSkill(bool val)
        {
            _anim.SetBool("isSkill", val);
            _attack_range.gameObject.SetActive(!val);
            _skill_range.SetActive(val);
        }

        public void Attack()
        {
            if (Targets.Count == 0)
            {
                return;
            }

            int targetLimit = Mathf.Min(NumberOfAttackedTarget, Targets.Count);

            for (int i= 0; i < targetLimit; i++)
            {
                switch (_atkDamageType)
                {
                    case AttackDamageType.True_Damage:
                        break;
                    case AttackDamageType.Physical_Damage:
                        Targets[i].TakePhysicalDamage(CurrentATK);
                        break;
                    case AttackDamageType.Magic_Damage:
                        Targets[i].TakeMagicDamage(CurrentATK);
                        break;
                    default:
                        break;
                }
            }
        }

        public void TakeMagicDamage(float other_character_final_attack)
        {
            float Art_Damage = other_character_final_attack * (1.0f - ((CurrentRES + _flat_res_down) * (1.0f - _scaling_res_down) / 100f)) * _magic_dmg_taken_up;
            CurrentHP -= Mathf.Floor(Art_Damage);
            Healthbar.SetProgressValues(CurrentHP / BaseHP);
        }

        public void TakePhysicalDamage(float other_character_final_attack)
        {
            float Phys_Damage = (other_character_final_attack - (CurrentDEF + _flat_def_down) * (1.0f - _scaling_def_down)) * _phys_dmg_taken_up;

            if (Phys_Damage < 0)
            {
                Phys_Damage = 0;
            }

            CurrentHP -= Mathf.Floor(Phys_Damage);
            Healthbar.SetProgressValues(CurrentHP / BaseHP);
        }

        public void TakeHeal(float other_character_final_heal)
        {
            CurrentHP += Mathf.Floor(other_character_final_heal);
            Healthbar.SetProgressValues(CurrentHP / BaseHP);
        }

        public void ToogleAttackRangeVisual(bool val)
        {
            if (val)
            {
                _attack_range.ShowRangeVisual();
            }
            else {
                _attack_range.HideRangeVisual();
            }
        }

    }

}

