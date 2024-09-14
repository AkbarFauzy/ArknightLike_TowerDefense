using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Ground, Air};

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> Squad;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else {
            Destroy(this);
        }           
    }

}
