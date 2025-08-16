using BepInEx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CSTI_ZLib.UI.Com;

public class EventOnScroll : MonoBehaviour, IScrollHandler
{
    public delegate void OnScrollDelegate(Vector2 scrollDelta, RectTransform self);

    public event OnScrollDelegate? Scroll;

    public void OnScroll(PointerEventData eventData)
    {
        var scrollDelta = eventData.scrollDelta;
        if (UnityInput.Current.GetKey(KeyCode.LeftControl))
        {
            Scroll?.Invoke(new Vector2(scrollDelta.y, scrollDelta.x), (RectTransform)transform);
        }
        else
        {
            Scroll?.Invoke(scrollDelta, (RectTransform)transform);
        }
    }
}