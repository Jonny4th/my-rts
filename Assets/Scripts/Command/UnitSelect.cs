using MyGame.Unit;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Unit curUnit; //current selected single unit
    public Unit CurUnit { get { return curUnit; } }

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

        Debug.Log("Selected Unit");
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
            }
        }
    }

    private void ClearAllSelectionVisual()
    {
        if (select != null)
            select.ToggleSelectionVisual(false);
    }

    private void ClearEverything()
    {
        ClearAllSelectionVisual();
        curUnit = null;
        select = null;
    }
}
