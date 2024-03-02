using MyGame.Core.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyGame.Core.Inputs
{
    public class UnitSelect : MonoBehaviour
    {
        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private List<Unit> curUnits = new(); //current selected multiple unit
        public List<Unit> CurUnits { get { return curUnits; } }

        [SerializeField]
        private Building curBuilding;
        public Building CurBuilding { get { return curBuilding; } }

        [SerializeField]
        private ResourceSource curResource;
        public ResourceSource CurResource { get { return curResource; } }

        private List<Selectable> select = new();

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
                    case "Resource":
                        ResourceSelect(hit);
                        break;
                }
            }
        }

        private void SelectUnit(RaycastHit hit)
        {
            var unit = hit.collider.GetComponent<Unit>();

            if (GameManager.instance.MyFaction.IsMyUnit(unit))
            {
                curUnits.Add(unit);
                select.Add(unit.SelectionVisual.ToggleSelectionVisual(true));
                ShowUnit(unit);
            }
        }

        private void SelectBuilding(RaycastHit hit)
        {
            curBuilding = hit.collider.GetComponent<Building>();

            select.Add(curBuilding.SelectionVisual.ToggleSelectionVisual(true));

            if (GameManager.instance.MyFaction.IsMyBuilding(curBuilding))
            {
                ShowBuilding(curBuilding);//Show building info
            }
        }

        private void ResourceSelect(RaycastHit hit)
        {
            curResource = hit.collider.GetComponent<ResourceSource>();
            if (curResource == null)
                return;

            select.Add(curResource.SelectionVisual.ToggleSelectionVisual(true));

            ShowResource();//Show resource info
        }

        private void ClearAllSelectionVisual()
        {
            ActionManager.instance.ClearAllInfo();
            if (select.Count == 0) return;
            select.Select(x => x.ToggleSelectionVisual(false));
            select.Clear();
        }

        private void ClearEverything()
        {
            ClearAllSelectionVisual();
            curUnits.Clear();
            curBuilding = null;

            InfoManager.instance.ClearAllInfo();
        }

        private void ShowUnit(Unit u)
        {
            InfoManager.instance.ShowAllInfo(u);

            if (u.IsBuilder)
            {
                ActionManager.instance.ShowBuilderMode(u);
            }
        }

        private void ShowBuilding(Building b)
        {
            InfoManager.instance.ShowAllInfo(b);
            ActionManager.instance.ShowCreateUnitMode(b);
        }

        private void ShowResource()
        {
            InfoManager.instance.ShowAllInfo(curResource);//Show resource info in Info Panel
        }
    }
}