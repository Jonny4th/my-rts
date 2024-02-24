using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField] private GameObject selectionVisual;
    public GameObject SelectionVisual { get { return selectionVisual; } }

    public Selectable ToggleSelectionVisual(bool flag)
    {
        if (SelectionVisual != null)
        {
            SelectionVisual.SetActive(flag);
        }

        return this;
    }
}
