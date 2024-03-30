using MyGame.Core;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapBlip : MonoBehaviour
{
    private GameObject blip;
    public GameObject Blip => blip;

    Unit unit;
    Building building;

    Faction faction;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        building = GetComponent<Building>();
    }

    private void Start()
    {
        blip = Instantiate(MiniMap.instance.blipPrefab);
        blip.transform.SetParent(MiniMap.instance.blipParent.transform);
        SetColor();
    }

    private void Update()
    {
        blip.transform.position = MiniMap.instance.worldPosToMinimapPos(transform.position);
        blip.transform.position = MainUI.instance.ScalePosition(blip.transform.position);
    }

    private void SetColor()
    {
        if (unit != null)
            faction = unit.Faction;

        if (building != null)
        {
            faction = building.Faction;
            blip.GetComponent<RectTransform>().sizeDelta = new Vector2(6f, 6f);
        }

        if (faction != null)
            blip.GetComponent<Image>().color = faction.GetNationColor();
        else
            blip.GetComponent<Image>().color = Color.white;
    }

    void OnDestroy()
    {
        Destroy(blip);
    }
}
