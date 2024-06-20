using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { 
    True_Damage, 
    Physical_Damage, 
    Magic_Damage
};

public class Character : MonoBehaviour
{
    protected List<IStageObserver> _observers = new List<IStageObserver>();

    public Animator anim;
    public CharacterStats characterStats;

    [SerializeField] private ProgressBar Healthbar;

    private AttackType atkType;
    public float CurrentHP;
    public int CurrentATK;
    public int CurrentDEF;
    public int CurrentRES;
    public float CurrentASPD;
    public float current_sp;
    private float sp_regen = 1.0f;
    private int _blockCount;
    private bool _isAttacking;
    private bool _isSkill;
    protected bool _isDead;

    #region
    public string CharacterName { get => characterStats.characterName;}
    public float BaseHP { get => characterStats.baseHP;}
    public int BaseATK { get => characterStats.baseATK;}
    public int BaseDEF { get => characterStats.baseDEF;}
    public int BaseRES { get => characterStats.baseRES;}
    public float ASPD { get => characterStats.attackSpeed;}
    public bool IsAttacking { get => _isAttacking;}
    public bool IsSkill { get => _isSkill; }
    public int BlockCount { get => _blockCount; set => _blockCount = value; }

    #endregion

    public List<Character> targets;

    [HideInInspector] public AbilityHolder abilityHolder;
    [SerializeField] private GameObject attack_range;
    private GameObject skill_range;

    private int Flat_Def_Down;
    private float Scaling_Def_Down; 
    private float Phys_Dmg_Taken_Up;

    private int Flat_Res_Down;
    private float Scaling_Res_Down;
    private float Magic_Dmg_Taken_Up;

    public float NormalizedASPD() => 0f;
    public void SetStats(CharacterStats stats) {
        characterStats = stats;
    }

    protected void Start()
    {
        Flat_Def_Down = 0;
        Scaling_Def_Down = 0.0f;
        Phys_Dmg_Taken_Up = 1.0f;

        Flat_Res_Down = 0;
        Scaling_Res_Down = 0.0f;
        Magic_Dmg_Taken_Up = 1.0f;

        atkType = characterStats.atkType;
        CurrentHP = characterStats.baseHP;
        CurrentATK = characterStats.baseATK;
        CurrentDEF = characterStats.baseDEF;
        CurrentRES = characterStats.baseRES;
        CurrentASPD = characterStats.attackSpeed;
        BlockCount = characterStats.numberOfBlock;
        Healthbar.SetProgressValues(CurrentHP/characterStats.baseHP);

        if (targets.Count == 0)
        {
            targets = new List<Character>();
        }

        targets = new List<Character>();
    }                                          

    public void ToggleAttackingAnimation(bool val) {
        _isAttacking = val;
        anim.SetBool("isAttacking", val);
    }

    public void ToggleSkillAnimation(bool val) {
        _isSkill = val;
        anim.SetBool("isSkill", val);
    }

    public void SetSkillRange(GameObject range) {
        if (range == null) {
            skill_range = attack_range;
            return;
        }

        skill_range = Instantiate(range);
        skill_range.transform.SetParent(this.transform);
        skill_range.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    public void ToggleSkillRange(bool val) {
        attack_range.SetActive(!val);
        skill_range.SetActive(val);
    }

    public IEnumerator Attack()
    {
        if (targets.Count == 0)
        {
            yield break;
        }

        switch (atkType) {
            case AttackType.True_Damage:
                break;
            case AttackType.Physical_Damage:
                targets[0].TakePhysicalDamage(CurrentATK);
                break;
            case AttackType.Magic_Damage:
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(60/50f);   //change to aspd
        ToggleAttackingAnimation(false);
        anim.SetBool("isAttacking", _isAttacking);
    }


    public void TakeArtDamage(float other_character_final_attack) {
        float Art_Damage = other_character_final_attack * (1.0f - ((CurrentRES + Flat_Res_Down) * (1.0f - Scaling_Res_Down) / 100f)) * Magic_Dmg_Taken_Up;
        CurrentHP -= Mathf.Floor(Art_Damage);
        Healthbar.SetProgressValues(CurrentHP/BaseHP);
    }

    public void TakePhysicalDamage(float other_character_final_attack) {
        float Phys_Damage = (other_character_final_attack - (CurrentDEF + Flat_Def_Down) * (1.0f - Scaling_Def_Down)) * Phys_Dmg_Taken_Up;

        if (Phys_Damage < 0 )
        {
            Phys_Damage = 0;
        }

        CurrentHP -= Mathf.Floor(Phys_Damage);
        Debug.Log("Damage is :"+ Phys_Damage);
        Debug.Log(CurrentHP);
        Debug.Log("Now: " + CurrentHP / BaseHP);
        Healthbar.SetProgressValues(CurrentHP/BaseHP);
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
