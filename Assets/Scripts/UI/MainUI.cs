using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    private GameObject selectionMarker;
    public GameObject SelectionMarker { get => selectionMarker; }

    [SerializeField]
    private RectTransform selectionBox;
    public RectTransform SelectionBox { get => selectionBox; }

    [SerializeField]
    private TextMeshProUGUI unitCountText;

    [SerializeField]
    private TextMeshProUGUI foodText;

    [SerializeField]
    private TextMeshProUGUI woodText;

    [SerializeField]
    private TextMeshProUGUI goldText;

    [SerializeField]
    private TextMeshProUGUI stoneText;

    public static MainUI instance;

    private Canvas canvas;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        canvas = GetComponent<Canvas>();
    }

    public void UpdateAllResource(Faction faction)
    {
        unitCountText.text = $"{faction.AliveUnits.Count}/{faction.UnitLimit}";
        foodText.text = faction.Food.ToString();
        woodText.text = faction.Wood.ToString();
        goldText.text = faction.Gold.ToString();
        stoneText.text = faction.Stone.ToString();
    }

    public Vector3 ScalePosition(Vector3 pos)
    {
        Vector3 newPos;

        newPos = new Vector3(pos.x * canvas.transform.localScale.x,
                             pos.y * canvas.transform.localScale.y,
                             pos.z * canvas.transform.localScale.z);

        return newPos;
    }
}
