using UnityEngine;

namespace MyGame.Core.Inputs
{
    public class UnistCommand : MonoBehaviour
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

        private void UnitsMoveToPosition(Vector3 dest, Unit unit)
        {
            if (unit != null)
                unit.MoveToPosition(dest);
        }

        private void CommandToGround(RaycastHit hit, Unit unit)
        {
            UnitsMoveToPosition(hit.point, unit);
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
                        CommandToGround(hit, unitSelect.CurUnit);
                        break;
                }
            }
        }

        private void CreateVFXMarker(Vector3 pos, GameObject vfxPrefab)
        {
            if (vfxPrefab == null) return;

            Instantiate(vfxPrefab, new Vector3(pos.x, 0.1f, pos.z), Quaternion.identity);
        }
    }
}