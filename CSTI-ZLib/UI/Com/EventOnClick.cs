using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CSTI_ZLib.UI.Com
{
    public class EventOnClick : MonoBehaviour, IPointerClickHandler
    {
        public event Action? OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }
    }
}