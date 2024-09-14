using UnityEngine;

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

namespace TowerDefence.Module.Ability {
    public class Ability : ScriptableObject
    {
        public string SkillName;
        public string Description;
        public float InitialSP;
        public float SPCost;
        public float Duration;
        public float Multiplier;
        public SPRegenType SPType;
        public AbilityType AbilityType;
        public Sprite Icon;
        public GameObject Range;
        public GameObject vfx_asset;
        protected GameObject instantiated_vfx;

        public virtual void Activate() { }

        public virtual void Deactivate() { }

        public virtual void SpawnVFX() { }
    }
}

