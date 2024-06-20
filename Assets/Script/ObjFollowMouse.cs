using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFollowMouse : MonoBehaviour
{
    private PlaceObjectOnGrid placeObjectonGrid;
    public bool isOnGrid;

    private void Start()
    {
        placeObjectonGrid = FindObjectOfType<PlaceObjectOnGrid>();
        isOnGrid = false;
    }

    private void Update()
    {
        if (!isOnGrid) {
            transform.position = placeObjectonGrid.smoothMousePosition + new Vector3(0, 0.7f, 0);
        }
    }
}
