using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using UnityEngine;
using TMPro;

public class UIStageDP : StageObserver
{
    public delegate void DPChangeEventHandler(float newDPValue);
    public event DPChangeEventHandler OnDpChanged;
    private float current_progress_value;

    [HideInInspector] private int current_stage_dp;

    [SerializeField] private int initial_stage_dp = 10;
    [SerializeField] private int max_dp = 99;
    [SerializeField] private float dp_regen_speed = 2f;

    [SerializeField] private UIProgressBar dp_bar;
    [SerializeField] private TextMeshProUGUI dp_text;


    protected new void Start()
    {
        base.Start();
        current_progress_value = 0;
        current_stage_dp = initial_stage_dp + 5;
        dp_text.text = current_stage_dp.ToString();
    }

    private void Update()
    {
        if (current_stage_dp == max_dp) return;

        current_progress_value += (dp_regen_speed * Time.deltaTime);
        if (current_progress_value >= 1)  {
            current_stage_dp += 1;
            dp_text.text = current_stage_dp.ToString();
            current_progress_value -= 1;
        }
        OnDpChanged?.Invoke(current_stage_dp);
        dp_bar.SetProgressValues(current_progress_value);
    }

    public int GetCurrentDP() {
        return current_stage_dp;
    }

    public void AddCurrentDP(int value) {
        current_stage_dp += value;
        if (current_stage_dp > max_dp) {
            current_stage_dp = max_dp;
        }
        dp_text.text = current_stage_dp.ToString();
        OnDpChanged?.Invoke(current_stage_dp);
    }

    public void SubstractCurrentDP(int value)
    {
        current_stage_dp -= value;
        dp_text.text = current_stage_dp.ToString();
        OnDpChanged?.Invoke(current_stage_dp);
    }

}
