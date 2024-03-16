using MyGame.Core;
using System.Collections.Generic;
using UnityEngine;

public enum Nation
{
    Neutral = 0,
    Britain,
    Pirates,
    France,
    Spain,
    Portugal,
    Netherland
}

public class Faction : MonoBehaviour
{
    [SerializeField] private Nation nation;
    public Nation Nation => nation;

    [Header("Resource")]
    [SerializeField] private int food;
    public int Food { get => food; set => food = value; }

    [SerializeField] private int wood;
    public int Wood { get => wood; set => wood = value; }

    [SerializeField] private int gold;
    public int Gold { get => gold; set => gold = value; }

    [SerializeField] private int stone;
    public int Stone { get => stone; set => stone = value; }

    [SerializeField]
    private int newResourceRange = 50;

    [Header("Own Units")]
    [SerializeField] private List<Unit> aliveUnits = new List<Unit>();
    public List<Unit> AliveUnits { get => aliveUnits; }

    [SerializeField] private List<Building> aliveBuildings = new();
    public List<Building> AliveBuildings { get => aliveBuildings; }

    [SerializeField] private Transform unitsParent;
    public Transform UnitsParent => unitsParent;

    [SerializeField] private Transform buildingsParent;
    public Transform BuildingsParent => buildingsParent;

    [SerializeField] private Transform ghostBuildingParent;
    public Transform GhostBuildingParent => ghostBuildingParent;

    [SerializeField]
    private Transform startPosition; //start position for Faction
    public Transform StartPosition { get { return startPosition; } }

    [SerializeField] private GameObject[] buildingPrefabs;
    public GameObject[] BuildingPrefabs { get { return buildingPrefabs; } }

    [SerializeField] private GameObject[] unitPrefabs;
    public GameObject[] UnitPrefabs { get { return unitPrefabs; } }

    private int unitLimit = 6; //Initial unit limit
    public int UnitLimit { get { return unitLimit; } }
    private int housingUnitNum = 5; //number of units per each housing
    public int HousingUnitNum { get { return housingUnitNum; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetHQSpawnPos()
    {
        var hq = aliveBuildings.Find(b => b != null && b.IsHQ);
        if(hq != null) return hq.SpawnPoint.position;
        return startPosition.position;
    }

    #region Unit
    public bool IsMyUnit(Unit u)
    {
        return aliveUnits.Contains(u);
    }

    public bool CheckUnitCost(Unit unit)
    {
        if(food < unit.UnitCost.food)
            return false;

        if(wood < unit.UnitCost.wood)
            return false;

        if(gold < unit.UnitCost.gold)
            return false;

        if(stone < unit.UnitCost.stone)
            return false;

        return true;
    }

    public bool CheckUnitCost(int id)
    {
        if(id >= UnitPrefabs.Length) return false;

        var unit = UnitPrefabs[id].GetComponent<Unit>();

        return CheckUnitCost(unit);
    }

    public void DeductUnitCost(Unit unit)
    {
        food -= unit.UnitCost.food;
        wood -= unit.UnitCost.wood;
        gold -= unit.UnitCost.gold;
        stone -= unit.UnitCost.stone;
    }
    #endregion

    #region Building
    public bool IsMyBuilding(Building b)
    {
        return aliveBuildings.Contains(b);
    }

    public bool CheckBuildingCost(Building building)
    {
        if(food < building.StructureCost.food)
            return false;

        if(wood < building.StructureCost.wood)
            return false;

        if(gold < building.StructureCost.gold)
            return false;

        if(stone < building.StructureCost.stone)
            return false;

        return true;
    }

    public void DeductBuildingCost(Building building)
    {
        food -= building.StructureCost.food;
        wood -= building.StructureCost.wood;
        gold -= building.StructureCost.gold;
        stone -= building.StructureCost.stone;
    }
    #endregion

    public void GainResource(ResourceType resourceType, int amount)
    {
        switch(resourceType)
        {
            case ResourceType.Food:
                food += amount;
                break;
            case ResourceType.Wood:
                wood += amount;
                break;
            case ResourceType.Gold:
                gold += amount;
                break;
            case ResourceType.Stone:
                stone += amount;
                break;
        }

        if(this == GameManager.instance.MyFaction)
            MainUI.instance.UpdateAllResource(this);
    }

    // gets the closest resource to the position (random between nearest 3 for some variance)
    public ResourceSource GetClosestResource(Vector3 pos, ResourceType rType)
    {
        ResourceSource[] closest = new ResourceSource[2];
        float[] closestDist = new float[2];

        foreach(ResourceSource resource in ResourceManager.instance.Resources)
        {
            if(resource == null) continue;
            if(resource.RsrcType != rType) continue;

            float dist = Vector3.Distance(pos, resource.transform.position);
            if(dist > newResourceRange) continue;

            for(int x = 0; x < closest.Length; x++)
            {
                if(closest[x] == null)
                {
                    closest[x] = resource;
                    closestDist[x] = dist;
                    break;
                }
                else if(dist < closestDist[x])
                {
                    closest[x] = resource;
                    closestDist[x] = dist;
                    break;
                }
            }
        }

        return closest[UnityEngine.Random.Range(0, closest.Length)];
    }

    public void UpdateHousingLimit()
    {
        unitLimit = 6; //starting unit Limit

        foreach (Building b in aliveBuildings)
        {
            if (b.IsHousing && b.IsFunctional)
            {
                unitLimit += housingUnitNum;
            }
        }

        if (unitLimit >= 100)
            unitLimit = 100;
        else if (unitLimit < 0)
            unitLimit = 0;

        MainUI.instance.UpdateAllResource(this);
    }
}
