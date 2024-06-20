using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

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
    public Sprite GetSelectedAbilityIcon { get => ability[selected_ability_index].icon;}
    public AbilityType GetSelectedAbilityType { get => ability[selected_ability_index].abilityType; }

    enum AbilityState {
        cooldown,
        ready,
        active,
    }

    private void Awake()
    {
        current_sp = ability[selected_ability_index].initial_sp;
        this.enabled = false;
    }

    private void Start()
    {
        SetSkillIcon();
        op = this.GetComponent<Operator>();
        op.SetSkillRange(SelectedAbility.range);
    }

    private void Update()
    {
        if (op.CurrentOperatorState is OperatorDeployedState )
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
                if (current_sp >= SelectedAbility.sp_cost)
                {
                    activate_button.interactable = true;
                }
                else
                {
                    activate_button.interactable = false;
                    current_sp += sp_regen;
                    SPBar.SetProgressValues(current_sp / ability[selected_ability_index].sp_cost);
                }

            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void InvokeSkill()
    {
        op.anim.SetBool("isSkill", true);
        op.ToggleSkillRange(true);
        StartCoroutine(ActivateSkill());
    }

    public void DeactivateSkill()
    {
        op.ToggleSkillRange(false);
        op.anim.SetBool("isSkill", false);
        SelectedAbility.Deactivate(op);
    }

    public IEnumerator ActivateSkill() {
        if (current_sp < SelectedAbility.sp_cost && !isActivate) {
            yield return null;
        }
        current_sp = 0;
        StartCoroutine(StartTimer());
        SelectedAbility.Activate(this.op);
        yield return new WaitUntil(() => !isActivate);
        DeactivateSkill();
    }

    public void PlayVFX() {
        SelectedAbility.SpawnVFX(this.op);
    }

    public void SetSkillIcon() {
        activate_button.GetComponent<Image>().sprite = SelectedAbility.icon;
    }

    public IEnumerator StartTimer()
    {
        isActivate = true;
        yield return new WaitForSeconds(SelectedAbility.duration);
        isActivate = false;
    }


}
