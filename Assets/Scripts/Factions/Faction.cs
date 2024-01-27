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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
