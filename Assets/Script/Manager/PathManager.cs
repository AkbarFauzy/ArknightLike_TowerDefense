using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [HideInInspector] public static PathManager Instance;
    [SerializeField] private List<PathFinder> _pathFinders;
    
    private Dictionary<Vector2Int, Waypoint> _grid = new Dictionary<Vector2Int, Waypoint>();

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            foreach (PathFinder pathFinder in _pathFinders)
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
        _grid.Clear();
        foreach (Waypoint waypoint in waypoints)
        {
            waypoint.isExplored = false;
            waypoint.exploreFrom = null;
            _grid.Add(waypoint.GetGridPos(), waypoint);
        }
    }


    public Dictionary<Vector2Int, Waypoint> GetGrid() {
        return _grid;
    }

}
