using UnityEngine;
using TowerDefence.Module.Characters;

namespace TowerDefence.Module.Ability {
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
        public string SkillName;
        public string Description;
        public float InitialSP;
        public float SPCost;
        public float Duration;
        public float Multiplier;
        public int NumberOfTarget;
        public SPRegenType SPType;
        public AbilityType AbilityType;
        public Sprite Icon;
        public GameObject Range;
        public GameObject vfx_asset;
        protected GameObject instantiated_vfx;

        public virtual void Activate(Character character) { }

        public virtual void Deactivate(Character character) { }

        public virtual void SpawnVFX() { }
    }
}

