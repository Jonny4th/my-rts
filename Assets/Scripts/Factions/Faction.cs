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

    [SerializeField] private List<Unit> aliveUnits = new List<Unit>();
    public List<Unit> AliveUnits { get => aliveUnits; }

    [SerializeField] private List<Building> aliveBuildings = new();
    public List<Building> AliveBuildings { get => aliveBuildings; }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckUnitCost(Unit unit)
    {
        if (food < unit.UnitCost.food)
            return false;

        if (wood < unit.UnitCost.wood)
            return false;

        if (gold < unit.UnitCost.gold)
            return false;

        if (stone < unit.UnitCost.stone)
            return false;

        return true;
    }

    public void DeductUnitCost(Unit unit)
    {
        food -= unit.UnitCost.food;
        wood -= unit.UnitCost.wood;
        gold -= unit.UnitCost.gold;
        stone -= unit.UnitCost.stone;
    }

    public bool IsMyUnit(Unit u)
    {
        return aliveUnits.Contains(u);
    }

    public bool IsMyBuilding(Building b)
    {
        return aliveBuildings.Contains(b);
    }

}
