using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonStartHoverEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonStopHoverEvent?.Invoke();
    }

    public UnityEvent ButtonStartHoverEvent, ButtonStopHoverEvent;
}
