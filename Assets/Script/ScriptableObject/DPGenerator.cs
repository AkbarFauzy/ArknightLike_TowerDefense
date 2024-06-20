using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class DPGenerator: Ability
{   
    public override void Activate(Character character)                  
    {
        Debug.Log("DP Generator Activate");

        if (duration > 0) {
            
        }
        else {
            character.NotifyGeneratingDP((int)multiplier);
        }
        
       
        
    }

    public override void Deactivate(Character character)
    {
    }
}
