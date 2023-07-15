using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DevMenu;

public class ButtonRightClickHandler : MonoBehaviour, IPointerClickHandler
{
    public delegate void RightClickDelegate();
    public event RightClickDelegate OnRightClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick?.Invoke();
        }
    }
}
