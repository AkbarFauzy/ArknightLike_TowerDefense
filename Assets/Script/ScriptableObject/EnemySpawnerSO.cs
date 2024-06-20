using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawner Data", menuName = "Spawner Data")]

public class EnemySpawnerSO : ScriptableObject
{
    [System.Serializable]
    public struct Spawn {
        public CharacterStats data;
        public int numberOfSpawn; 
    }

    public GameObject Pref;
    public float spawnInterval;
    public List<Spawn> Instance;
}
