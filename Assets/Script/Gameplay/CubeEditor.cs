using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(Waypoint))]
public class CubeEditor : MonoBehaviour
{
    public bool isPlaceable;
    Waypoint waypoint;
    
    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }

    void Update()
    {
        int gridSize = waypoint.GetGridSize();

        transform.position = new Vector3(waypoint.GetGridPos().x, 0f, waypoint.GetGridPos().y);

        string labelText = (waypoint.GetGridPos().x / gridSize) + "," + (waypoint.GetGridPos().y / gridSize);
        gameObject.name = labelText;
    }
}
