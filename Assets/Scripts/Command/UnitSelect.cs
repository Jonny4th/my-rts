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

        [SerializeField]
        private RectTransform selectionBox;
        private Vector2 oldAnchoredPos;
        private Vector2 startPos;

        [SerializeField]
        private Unit curEnemy;

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

            selectionBox = MainUI.instance.SelectionBox;

            if (instance != null) Destroy(this);
            instance = this;
        }

        void Update()
        {
            //mouse down
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;

                if (EventSystem.current.IsPointerOverGameObject()) return;

                ClearEverything();
            }

            if(Input.GetMouseButton(0))
            {
                UpdateSelectionBox(Input.mousePosition);
            }

            // mouse up
            if (Input.GetMouseButtonUp(0))
            {
                ReleaseSelectionBox(Input.mousePosition);
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
            else
            {
                curEnemy = unit;
                select.Add(curEnemy.SelectionVisual.ToggleSelectionVisual(true));
                ShowEnemyUnit(unit);
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

            select.ForEach(x => x.ToggleSelectionVisual(false));
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

        private void ShowEnemyUnit(Unit u)
        {
            InfoManager.instance.ShowEnemyAllInfo(u);
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

        private void UpdateSelectionBox(Vector3 mousePos)
        {
            //Debug.Log("Mouse Pos - " + curMousePos);
            if (!selectionBox.gameObject.activeInHierarchy && curBuilding == null)
                selectionBox.gameObject.SetActive(true);

            float width = mousePos.x - startPos.x;
            float height = mousePos.y - startPos.y;

            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);

            //store old position for real unit selection
            oldAnchoredPos = selectionBox.anchoredPosition;
        }

        private void ReleaseSelectionBox(Vector2 mousePos)
        {
            //Debug.Log("Step 2 - " + _doubleClickMode);
            Vector2 min; //down-left corner
            Vector2 max; //top-right corner

            selectionBox.gameObject.SetActive(false);

            min = oldAnchoredPos - (selectionBox.sizeDelta / 2);
            max = oldAnchoredPos + (selectionBox.sizeDelta / 2);

            //Debug.Log("min = " + min);
            //Debug.Log("max = " + max);

            foreach (Unit unit in GameManager.instance.MyFaction.AliveUnits)
            {
                Vector2 unitPos = cam.WorldToScreenPoint(unit.transform.position);

                if (unitPos.x <= min.x || unitPos.x >= max.x || unitPos.y <= min.y || unitPos.y >= max.y) continue;

                curUnits.Add(unit);
                select.Add(unit.SelectionVisual.ToggleSelectionVisual(true));
            }
            selectionBox.sizeDelta = new Vector2(0, 0); //clear Selection Box's size;
        }

    }
}