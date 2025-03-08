using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Stats", menuName = "Character Stats")]
public class CharacterStats : ScriptableObject
{
    public AttackDamageType atkType;
    public string characterName;
    [Range(1, 6)] public int rarity;
    public int baseHP;
    public int baseATK;
    public int baseDEF;
    public int baseRES;
    public float attackSpeed;
    public int numberOfBlock;
    public int numberOfTarget;
    public int cost;
    public float redeploymentTime;

    public Sprite icon;
}

