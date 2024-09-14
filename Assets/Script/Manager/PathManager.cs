using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private List<PathFinder> pathFinders;
    [HideInInspector] public static PathManager PathManagerInstance;
    [HideInInspector] private Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();

    private void Awake()
    {
        if (PathManagerInstance == null) {
            PathManagerInstance = this;
            foreach (PathFinder pathFinder in pathFinders)
            {
                LoadBlocks();
                pathFinder.CalculatePath();
            }
        }
        else {
            Destroy(this);
        }
    }

    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();
        grid.Clear();
        foreach (Waypoint waypoint in waypoints)
        {
            waypoint.isExplored = false;
            waypoint.exploreFrom = null;
            grid.Add(waypoint.GetGridPos(), waypoint);
        }
    }


    public Dictionary<Vector2Int, Waypoint> GetGrid() {
        return grid;
    }

}
