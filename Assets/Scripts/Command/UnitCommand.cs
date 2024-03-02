using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Core.Inputs
{
    public class UnitCommand : MonoBehaviour
    {
        public LayerMask layerMask;
        private UnitSelect unitSelect;

        private Camera cam;

        void Awake()
        {
            unitSelect = GetComponent<UnitSelect>();
        }

        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main;

            layerMask = LayerMask.GetMask("Unit", "Building", "Resource", "Ground");
        }

        // Update is called once per frame
        void Update()
        {
            // mouse up
            if (Input.GetMouseButtonUp(1))
            {
                TryCommand(Input.mousePosition);
            }
        }

        private void UnitsMoveToPosition(Vector3 dest, List<Unit> units)
        {
            foreach(var unit in units)
            {
                if (unit != null)
                    unit.MoveToPosition(dest);
            }
        }

        private void CommandToGround(RaycastHit hit, List<Unit> units)
        {
            UnitsMoveToPosition(hit.point, units);
            CreateVFXMarker(hit.point, MainUI.instance.SelectionMarker);
        }

        private void TryCommand(Vector2 screenPos)
        {
            Ray ray = cam.ScreenPointToRay(screenPos);

            //if we left-click something
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, layerMask))
            {
                switch (hit.collider.tag)
                {
                    case "Ground":
                        CommandToGround(hit, unitSelect.CurUnits);
                        break;
                    case "Resource":
                        ResourceCommand(hit, unitSelect.CurUnits);
                        break;
                }
            }
        }

        private void CreateVFXMarker(Vector3 pos, GameObject vfxPrefab)
        {
            if (vfxPrefab == null) return;

            Instantiate(vfxPrefab, new Vector3(pos.x, 0.1f, pos.z), Quaternion.identity);
        }

        // called when we command units to gather a resource
        private void UnitsToGatherResource(ResourceSource resource, List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                if(unit.IsWorker)
                    unit.Worker.ToGatherResource(resource, resource.transform.position);
                else
                    unit.MoveToPosition(resource.transform.position);
            }
        }

        private void ResourceCommand(RaycastHit hit, List<Unit> units)
        {
            UnitsToGatherResource(hit.collider.GetComponent<ResourceSource>(), units);
            CreateVFXMarker(hit.transform.position, MainUI.instance.SelectionMarker);
        }
    }
}