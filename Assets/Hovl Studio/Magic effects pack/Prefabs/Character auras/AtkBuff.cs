using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu]
public class AtkBuff : Ability
{
    public override void Activate(Character character)
    {
        Debug.Log("AtkBuff Activate");

        character.CurrentATK += (int)(character.BaseATK * multiplier);
        GameObject Parent = character.gameObject;

        instantiated_vfx = Instantiate(vfx_asset);
        instantiated_vfx.transform.SetParent(Parent.transform);
        instantiated_vfx.transform.localPosition = new Vector3(0f, 0f, 0f);
        instantiated_vfx.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public override void Deactivate(Character character)
    {
        character.CurrentATK -= (int)(character.BaseATK * multiplier);
        Destroy(instantiated_vfx);
    }

}
