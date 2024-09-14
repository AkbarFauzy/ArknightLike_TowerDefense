using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using UnityEngine;

namespace TowerDefence.Module.Ability
{
    [CreateAssetMenu]
    public class DPGenerator : Ability
    {
        public override void Activate()
        {
            Debug.Log("DP Generator Activate");

            if (Duration > 0)
            {

            }
            else
            {
               /* character.NotifyGeneratingDP((int)Multiplier);*/
            }
        }

        public override void Deactivate()
        {
        }
    }
}
