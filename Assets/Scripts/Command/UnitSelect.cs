using MyGame.Core.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyGame.Core.Inputs
{
    public class UnitSelect : MonoBehaviour
    {
        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private Unit curUnit; //current selected single unit
        public Unit CurUnit { get { return curUnit; } }

        [SerializeField]
        private Building curBuilding;
        public Building CurBuilding { get { return curBuilding; } }

        private Selectable select;

        private Camera cam;
        private Faction faction;

        public static UnitSelect instance;

        #region Mono
        void Awake()
        {
            faction = GetComponent<Faction>();
        }

        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main;
            layerMask = LayerMask.GetMask("Unit", "Building", "Resource", "Ground");

            if (instance != null) Destroy(this);
            instance = this;
        }

        void Update()
        {
            //mouse down
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                ClearEverything();
            }

            // mouse up
            if (Input.GetMouseButtonUp(0))
            {
                TrySelect(Input.mousePosition);
            }
        }
        #endregion

        private void SelectUnit(RaycastHit hit)
        {
            curUnit = hit.collider.GetComponent<Unit>();

            select = hit.collider.GetComponent<Selectable>();
            select.ToggleSelectionVisual(true);

            if (GameManager.instance.MyFaction.IsMyUnit(curUnit))
            {
                ShowUnit(curUnit);
            }
        }

        private void SelectBuilding(RaycastHit hit)
        {
            curBuilding = hit.collider.GetComponent<Building>();

            select = curBuilding.SelectionVisual;
            select.ToggleSelectionVisual(true);

            if (GameManager.instance.MyFaction.IsMyBuilding(curBuilding))
            {
                ShowBuilding(curBuilding);//Show building info
            }
        }

        private void TrySelect(Vector2 screenPos)
        {
            Ray ray = cam.ScreenPointToRay(screenPos);

            //if we left-click something
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, layerMask))
            {
                switch (hit.collider.tag)
                {
                    case "Unit":
                        SelectUnit(hit);
                        break;
                    case "Building":
                        SelectBuilding(hit);
                        break;
                }
            }
        }

        private void ClearAllSelectionVisual()
        {
            if (select != null)
                select.ToggleSelectionVisual(false);

            ActionManager.instance.ClearAllInfo();
        }

        private void ClearEverything()
        {
            ClearAllSelectionVisual();
            curUnit = null;
            curBuilding = null;
            select = null;

            InfoManager.instance.ClearAllInfo();
        }

        private void ShowUnit(Unit u)
        {
            InfoManager.instance.ShowAllInfo(u);

            if(u.IsBuilder)
            {
                ActionManager.instance.ShowBuilderMode(u);
            }
        }

        private void ShowBuilding(Building b)
        {
            InfoManager.instance.ShowAllInfo(b);
            ActionManager.instance.ShowCreateUnitMode(b);
        }
    }
}