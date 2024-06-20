using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    public bool isExplored = false;
    public Waypoint exploreFrom;

    const int GRID_SIZE = 1;

    public int GetGridSize()
    {
        return GRID_SIZE;
    }

    public Vector2Int GetGridPos()
    {
        return new Vector2Int(
                Mathf.RoundToInt(transform.position.x / GRID_SIZE),
                Mathf.RoundToInt(transform.position.z / GRID_SIZE)
            );
    }

}
