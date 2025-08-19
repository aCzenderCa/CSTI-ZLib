using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CSTI_ZLib.UI.Com;

public class EventOnClick : MonoBehaviour, IPointerClickHandler
{
    public event Action? ClickEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickEvent?.Invoke();
    }
}