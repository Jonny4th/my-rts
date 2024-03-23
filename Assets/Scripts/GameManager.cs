using MyGame.Core.Inputs;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Faction myFaction;
    public Faction MyFaction { get { return myFaction; } }

    [SerializeField] private Faction enemyFaction;
    public Faction EnemyFaction { get { return enemyFaction; } }

    //All factions in this game (2 factions for now)
    [SerializeField] private Faction[] factions;

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;

        SetupPlayers(Settings.mySide, Settings.EnemySide);
    }

    void Start()
    {
        MainUI.instance.UpdateAllResource(myFaction);
        CameraController.instance.FocusOnPostion(myFaction.StartPosition.position);
    }

    public void SetupPlayers(Nation myNation, Nation enemyNation)
    {
        foreach(var f in factions)
        {
            if(f.Nation == myNation)
            {
                myFaction = f;

                f.gameObject.AddComponent<UnitSelect>();
                f.gameObject.AddComponent<UnitCommand>();
            }
            else if (f.Nation == enemyNation)
            {
                enemyFaction = f;

                f.gameObject.AddComponent<FactionAI>();
                f.gameObject.AddComponent<AIController>();
                f.gameObject.AddComponent<AISupport>();
                f.gameObject.AddComponent<AIDoNothing>();
                f.gameObject.AddComponent<AIStrike>();
                f.gameObject.AddComponent<AICreateHQ>();
                f.gameObject.AddComponent<AICreateHouse>();
                f.gameObject.AddComponent<AICreateBarrack>();
            }
        }
    }
}
