using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Observer;
using TowerDefence.Module.Ability;

public enum AttackType { 
    True_Damage, 
    Physical_Damage, 
    Magic_Damage
};

namespace TowerDefence.Module.Characters {
    public abstract class Character : MonoBehaviour, IDamageable, IHaveAbility
    {
        protected List<IStageObserver> _observers = new List<IStageObserver>();
        protected Animator _anim;

        public CharacterStats CharacterStats;

        [SerializeField] private ProgressBar Healthbar;

        private AttackType atkType;
        public float CurrentHP;
        public int CurrentATK;
        public int CurrentDEF;
        public int CurrentRES;
        public float CurrentASPD;
        public float CurrnetSP;

        private float _sp_regen = 1.0f;
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
        public float ASPD { get => CharacterStats.attackSpeed; }
        public bool IsAttacking { get => _isAttacking; }
        public bool IsSkill { get => _isSkill; }
        public int BlockCount { get => _blockCount; set => _blockCount = value; }

        #endregion

        public List<Character> Targets;

        [HideInInspector] public AbilityHolder abilityHolder;
        [SerializeField] private GameObject _attack_range;
        private GameObject skill_range;

        private int _flat_def_down;
        private float _scaling_def_down;
        private float _phys_dmg_taken_up;

        private int _flat_res_down;
        private float _scaling_res_down;
        private float _magic_dmg_taken_up;

        public float NormalizedASPD() => 0f;
        public abstract void OnDied();

        protected void Start()
        {
            _flat_def_down = 0;
            _scaling_def_down = 0.0f;
            _phys_dmg_taken_up = 1.0f;

            _flat_res_down = 0;
            _scaling_res_down = 0.0f;
            _magic_dmg_taken_up = 1.0f;

            atkType = CharacterStats.atkType;
            CurrentHP = CharacterStats.baseHP;
            CurrentATK = CharacterStats.baseATK;
            CurrentDEF = CharacterStats.baseDEF;
            CurrentRES = CharacterStats.baseRES;
            CurrentASPD = CharacterStats.attackSpeed;
            BlockCount = CharacterStats.numberOfBlock;
            Healthbar.SetProgressValues(CurrentHP / CharacterStats.baseHP);

            if (Targets.Count == 0)
            {
                Targets = new List<Character>();
            }

            Targets = new List<Character>();
            _anim = GetComponentInChildren<Animator>();
        }

        public void ToggleAttackingAnimation(bool val)
        {
            _isAttacking = val;
            _anim.SetBool("isAttacking", val);
        }

        public void ToggleSkillAnimation(bool val)
        {
            _isSkill = val;
            _anim.SetBool("isSkill", val);
        }
        public void SetStats(CharacterStats stats)
        {
            CharacterStats = stats;
        }

        public void SetSkillRange(GameObject range)
        {
            if (range == null)
            {
                skill_range = _attack_range;
                return;
            }

            skill_range = Instantiate(range);
            skill_range.transform.SetParent(this.transform);
            skill_range.transform.localPosition = new Vector3(0f, 0f, 0f);
        }

        public void ToggleSkill(bool val)
        {
            _anim.SetBool("isSkill", val);
            _attack_range.SetActive(!val);
            skill_range.SetActive(val);
        }

        public IEnumerator Attack()
        {
            if (Targets.Count == 0)
            {
                yield break;
            }

            switch (atkType)
            {
                case AttackType.True_Damage:
                    break;
                case AttackType.Physical_Damage:
                    Targets[0].TakePhysicalDamage(CurrentATK);
                    break;
                case AttackType.Magic_Damage:
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(60 / 50f);   //change to aspd
            ToggleAttackingAnimation(false);
            _anim.SetBool("isAttacking", _isAttacking);
        }


        public void TakeArtDamage(float other_character_final_attack)
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
            Debug.Log("Damage is :" + Phys_Damage);
            Debug.Log(CurrentHP);
            Debug.Log("Now: " + CurrentHP / BaseHP);
            Healthbar.SetProgressValues(CurrentHP / BaseHP);
        }

        public void AddObserver(IStageObserver observer)
        {
            Debug.Log(this + " added observer " + observer);
            _observers.Add(observer);
        }

        public void RemoveObserver(IStageObserver observer)
        {
            _observers.Remove(observer);
        }

        protected void NotifyObserver(StageEvents stageEvent)
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnNotify(stageEvent);
            });
        }

        protected void NotifyUIObserver(StageUIEvents stageUIEvent, Operator op)
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnUIEvent(stageUIEvent, op);
            });
        }

        public void NotifyGeneratingDP(int value)
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnDPGenerated(value);
            });
        }

  
    }

}

