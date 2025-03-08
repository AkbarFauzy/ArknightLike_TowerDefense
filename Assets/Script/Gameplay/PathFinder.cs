using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Waypoint startWayPoint, endWaypoint;

    Queue<Waypoint> queue = new Queue<Waypoint>();
    List<Waypoint> path = new List<Waypoint>();

    bool isRunning = true;
    Waypoint searchCenter;

    Vector2Int[] directions = {
         Vector2Int.up,
         Vector2Int.down,
         Vector2Int.right,
         Vector2Int.left,

    };

    public List<Waypoint> GetPath() {
        if (path.Count == 0)
        {
            CalculatePath();
        }
        return path;
    }

    public void CalculatePath()
    {
        PathFind();
        CreatePath();
    }


    private void PathFind() {
        queue.Enqueue(startWayPoint);
        while (queue.Count > 0 && isRunning) {
            searchCenter = queue.Dequeue();
            HaltIfEndFound();
            ExploreNeighbours();
            searchCenter.isExplored = true;
        }
    }

    private void CreatePath() {
        path.Add(endWaypoint);
        Waypoint previous = endWaypoint.exploreFrom;
        while (previous != startWayPoint) {
            path.Add(previous);
            previous = previous.exploreFrom;
        }
        path.Add(startWayPoint);
        path.Reverse();
    }

    private void ExploreNeighbours() {
        if (!isRunning) { return; }

        foreach (Vector2Int direction in directions) {
            Vector2Int explorationCoordinates = searchCenter.GetGridPos() + direction;
            if (PathManager.Instance.GetGrid().ContainsKey(explorationCoordinates)) {
                QueueNewNeighbour(explorationCoordinates);
            }
        }
    }

    private void QueueNewNeighbour(Vector2Int neighbourCoordinates) {
        Waypoint neighbour = PathManager.Instance.GetGrid()[neighbourCoordinates];
        if (!neighbour.isExplored || !queue.Contains(neighbour)) {
            queue.Enqueue(neighbour);
            neighbour.exploreFrom = searchCenter;
        }
    }

    private void HaltIfEndFound() { 
        if (searchCenter == endWaypoint) { 
            isRunning = false; 
        } 
    }
}
