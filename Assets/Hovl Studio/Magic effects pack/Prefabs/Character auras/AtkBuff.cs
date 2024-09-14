using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TowerDefence.Module.Characters;

namespace TowerDefence.Module.Ability {
    [CreateAssetMenu]
    public class AtkBuff : Ability
    {
        public override void Activate()
        {
            Debug.Log("AtkBuff Activate");
/*
            character.CurrentATK += (int)(character.BaseATK * Multiplier);
            GameObject Parent = character.gameObject;*/

            instantiated_vfx = Instantiate(vfx_asset);
/*            instantiated_vfx.transform.SetParent(Parent.transform);*/
            instantiated_vfx.transform.localPosition = new Vector3(0f, 0f, 0f);
            instantiated_vfx.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        public override void Deactivate()
        {
            /*character.CurrentATK -= (int)(character.BaseATK * Multiplier);*/
            Destroy(instantiated_vfx);
        }

    }
}

