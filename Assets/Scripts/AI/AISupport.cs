using MyGame.Core;
using System.Collections.Generic;
using UnityEngine;

public class AISupport : MonoBehaviour
{
    [SerializeField] private List<GameObject> fighters = new(); //fighter
    public List<GameObject> Fighters { get { return fighters; } }

    [SerializeField] private List<GameObject> builders = new(); //builder
    public List<GameObject> Builders { get { return builders; } }

    [SerializeField] private List<GameObject> workers = new(); //worker
    public List<GameObject> Workers { get { return workers; } }

    [SerializeField] private List<GameObject> hq = new();
    public List<GameObject> Hq { get {  return hq; } }

    [SerializeField] private List<GameObject> houses = new();
    public List<GameObject> Houses { get {  return houses; } }

    [SerializeField] private List<GameObject> barracks = new();
    public List<GameObject> Barracks { get {  return barracks; } }

    [SerializeField] private Faction faction;
    public Faction Faction { get { return faction; } }

    // Start is called before the first frame update
    void Start()
    {
        faction = GetComponent<Faction>();
    }

    public void Refresh()
    {
        fighters.Clear();
        workers.Clear();
        builders.Clear();

        foreach (Unit u in faction.AliveUnits)
        {
            if(u.gameObject == null) continue;

            if (u.IsBuilder) //if it is a builder
                builders.Add(u.gameObject);

            if (u.IsWorker) //if it is a worker
                workers.Add(u.gameObject);

            if (!u.IsBuilder && !u.IsWorker) //if it is a fighter
                fighters.Add(u.gameObject);
        }

        hq.Clear();
        houses.Clear();
        barracks.Clear();

        foreach(var b in faction.AliveBuildings)
        {
            if(b == null) continue;

            if(b.IsHQ) hq.Add(b.gameObject);
            if(b.IsHousing) houses.Add(b.gameObject);
            if(b.IsBarrack) barracks.Add(b.gameObject);
        }
    }
}
