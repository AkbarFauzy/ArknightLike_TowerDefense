using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using TowerDefence.Observer;
using UnityEngine;

namespace TowerDefence.Module.Gameplay
{
    public class PlaceObjectOnGrid : StageSubject
    {
        private Plane _plane;
        private Vector3 _mousePosition;
        public Vector3 SmoothMousePosition;

        private CubeEditor[] _placeable_cubes;
        private bool _isRotating;
        public GameObject CurrentOperator { get; private set; }

        private void Start()
        {
            _plane = new Plane(Vector3.up, transform.position);
            _placeable_cubes = GetComponentsInChildren<CubeEditor>();
            _isRotating = false;
        }

        private void Update()
        {
            GetMousePositionOnGrid();
        }
                               
        public void SetCurrentOperator(GameObject op)
        {
            CurrentOperator = op;
            if(CurrentOperator != null)
            {
                CurrentOperator.SetActive(true);
            }
        }

        public void DestroyCurrentOpPlacement()
        {
            Destroy(CurrentOperator);
            CurrentOperator = null;
            _isRotating = false;
        }

        void GetMousePositionOnGrid()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (CurrentOperator == null) return;

            if (_plane.Raycast(ray, out var enter))
            {
                _mousePosition = ray.GetPoint(enter);
                SmoothMousePosition = _mousePosition;
                _mousePosition.y = 0;
                _mousePosition = Vector3Int.RoundToInt(_mousePosition);
                foreach (var cube in _placeable_cubes)
                {
                    if (_mousePosition != cube.transform.position || !cube.isPlaceable) continue;

                    CurrentOperator.GetComponent<ObjFollowMouse>().isOnGrid = true;

                    if (Input.GetMouseButton(0) && !_isRotating && CurrentOperator != null)
                    {
                        CurrentOperator.transform.position = cube.transform.position + new Vector3(0f, 0.7f, 0f);
                    }
                    else if (Input.GetMouseButtonUp(0) && !_isRotating && CurrentOperator != null)
                    {
                        cube.isPlaceable = false;
                        _isRotating = true;
                        CurrentOperator.GetComponent<ObjFollowMouse>().enabled = false;
                        CurrentOperator.GetComponentInChildren<Operator>().SetGrid(cube);
                    }
                }
                if (_isRotating)
                {
                    StartRotating();
                }
            }
        }

        private void StartRotating()
        {
            if (CurrentOperator == null) return;

            var operatorComp = CurrentOperator.GetComponentInChildren<Operator>();

            Vector3 direction = _mousePosition - CurrentOperator.transform.position;

            if (Input.GetMouseButton(0))
            {
                // Rotate based on the direction (facing)
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    // Left or Right
                    if (direction.x > 0)
                    {
                        operatorComp.transform.rotation = Quaternion.Euler(0f, 90f, 0f);  // Facing right
                    }
                    else
                    {
                        operatorComp.transform.rotation = Quaternion.Euler(0f, -90f, 0f);  // Facing left
                    }
                }
                else
                {
                    // Forward or Backward
                    if (direction.z > 0)
                    {
                        operatorComp.transform.rotation = Quaternion.Euler(0f, 0f, 0f);  // Facing forward
                    }
                    else
                    {
                        operatorComp.transform.rotation = Quaternion.Euler(0f, 180f, 0f);  // Facing backward
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0) && (Mathf.Abs(direction.x) > 0.5f || Mathf.Abs(direction.z) > 0.5f))
            {
                _isRotating = false;
                operatorComp.SwitchState(operatorComp.DeployedState);
                NotifyOperatorEvents(StageCharacterEvents.CharacterDeployed, operatorComp);
                CurrentOperator = null;
            }
        }
    }
}
