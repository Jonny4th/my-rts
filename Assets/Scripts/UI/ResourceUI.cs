using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resourceText;
    [SerializeField] private ResourceSource resource;

    public void OnQuantityChange()
    {
        resourceText.text = resource.Quantity.ToString();
    }

}
