using UnityEngine;
using UnityEngine.EventSystems;

public class DragEvent : MonoBehaviour
{
    public Vector2 Start;
    public Vector2 Current;
    public Vector2 Result;

    public void HandleDragStart(BaseEventData eventData)
    {
        Start = Input.mousePosition;
    }

    public void HandleDragging(BaseEventData eventData)
    {
        Current = Input.mousePosition;
        Result = Current - Start;
    }
}
