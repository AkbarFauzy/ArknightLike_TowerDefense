using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using TowerDefence.Observer;
using UnityEngine;

namespace TowerDefence.Module.Gameplay
{
    public class PlaceObjectOnGrid : MonoBehaviour
    {
        private List<IStageObserver> _observers = new List<IStageObserver>();

        private Plane plane;
        private Vector3 mousePosition;
        public Vector3 smoothMousePosition;

        [HideInInspector] public GameObject latestOP;
        private CubeEditor[] placeable_cubes;
        private bool isRotating;

        private void Start()
        {
            plane = new Plane(Vector3.up, transform.position);
            placeable_cubes = GetComponentsInChildren<CubeEditor>();
            isRotating = false;
        }

        private void Update()
        {
            GetMousePositionOnGrid();
        }

        public void DestroyCurrentOpPlacement()
        {
            Destroy(latestOP);
            latestOP = null;
            isRotating = false;
        }

        void GetMousePositionOnGrid()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (latestOP == null) return;

            if (plane.Raycast(ray, out var enter))
            {
                mousePosition = ray.GetPoint(enter);
                smoothMousePosition = mousePosition;
                mousePosition.y = 0;
                mousePosition = Vector3Int.RoundToInt(mousePosition);
                foreach (var cube in placeable_cubes)
                {
                    if (mousePosition != cube.transform.position || !cube.isPlaceable) continue;

                    latestOP.GetComponent<ObjFollowMouse>().isOnGrid = true;

                    if (Input.GetMouseButton(0) && !isRotating && latestOP != null)
                    {
                        latestOP.transform.position = cube.transform.position + new Vector3(0f, 0.7f, 0f);
                    }
                    else if (Input.GetMouseButtonUp(0) && !isRotating && latestOP != null)
                    {
                        cube.isPlaceable = false;
                        isRotating = true;
                        latestOP.GetComponent<ObjFollowMouse>().enabled = false;
                        latestOP.GetComponentInChildren<Operator>().SetGrid(cube);
                    }
                }
                if (isRotating)
                {
                    StartRotating();
                }
            }
        }

        private void StartRotating()
        {
            Vector3 rotation = mousePosition - latestOP.transform.position;
            if (Input.GetMouseButton(0) && latestOP != null)
            {
                if (rotation.x > 0.5f)
                {
                    latestOP.GetComponentInChildren<Operator>().gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                else if (rotation.z > 0.5f)
                {
                    latestOP.GetComponentInChildren<Operator>().gameObject.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                }
                else if (rotation.x < -0.5f)
                {
                    latestOP.GetComponentInChildren<Operator>().gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                else if (rotation.z < -0.5f)
                {
                    latestOP.GetComponentInChildren<Operator>().gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }
            }
            else if (Input.GetMouseButtonUp(0) && (rotation.x > 0.5f || rotation.x < -0.5f || rotation.z > 0.5f || rotation.z < -0.5f))
            {
                isRotating = false;
                latestOP.GetComponentInChildren<Operator>().SwitchState(latestOP.GetComponentInChildren<Operator>().DeployedState);
                NotifyObserverCharacterSpawn(latestOP.GetComponentInChildren<Operator>());
                latestOP = null;
            }
        }
        public void AddObserver(IStageObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IStageObserver observer)
        {
            _observers.Remove(observer);
        }

        private void NotifyObserverCharacterSpawn(Operator op)
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnNotifyCharacterSpawn(op);
            });
        }

    }
}
