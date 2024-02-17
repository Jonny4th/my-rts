using MyGame.Core;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private bool toBuild = false; //this builder has duty to build things
    [SerializeField] private bool showGhost = false; //ghost building is showing

    [SerializeField] private GameObject[] buildingList; // Buildings that this unit can build
    public GameObject[] BuildingList { get { return buildingList; } }
    [SerializeField] private GameObject[] ghostBuildingList; // Transparent buildings according to building list

    [SerializeField] private GameObject newBuilding; // Current building to build
    public GameObject NewBuilding { get { return newBuilding; } set { newBuilding = value; } }

    [SerializeField] private GameObject ghostBuilding; // Tranparent building to check site to build
    public GameObject GhostBuilding { get { return ghostBuilding; } set { ghostBuilding = value; } }

    [SerializeField] private GameObject inProgressBuilding; // The building a unit is currently building
    public GameObject InProgressBuilding { get { return inProgressBuilding; } set { inProgressBuilding = value; } }

    private Unit unit;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    public void ToCreateNewBuilding(int i) //Start call from ActionManager UI Btns
    {
        if (buildingList[i] == null)
            return;

        Building b = buildingList[i].GetComponent<Building>();

        if (!unit.Faction.CheckBuildingCost(b)) return; //don't have enough resource to build
        else
        {
            //Create ghost building at the mouse position
            ghostBuilding = Instantiate(ghostBuildingList[i],
                                        Input.mousePosition,
                                        Quaternion.identity, unit.Faction.GhostBuildingParent);

            toBuild = true;
            newBuilding = buildingList[i]; //Set prefab into new building
            showGhost = true;
        }
    }

}
