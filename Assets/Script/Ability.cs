using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum AbilityType
{
    buffer,
    debuffer,
}

public enum SPRegenType
{
    normal,
    defence,
    attack,
}

public class Ability : ScriptableObject
{
    public string skill_name;
    public string description;
    public float initial_sp;
    public float sp_cost;
    public float duration;
    public float multiplier;
    public SPRegenType sp_type;
    public AbilityType abilityType;
    public Sprite icon;
    public GameObject range;
    public GameObject vfx_asset;
    protected GameObject instantiated_vfx;

    public virtual void Activate(Character character) { }

    public virtual void Deactivate(Character character) { }

    public virtual void SpawnVFX(Character character) { }
}
