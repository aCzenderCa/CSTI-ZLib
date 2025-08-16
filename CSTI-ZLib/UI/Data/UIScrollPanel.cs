using CSTI_ZLib.LuaLIbs.Utils;
using CSTI_ZLib.UI.Com;
using CSTI_ZLib.UI.Utils;
using UnityEngine;

namespace CSTI_ZLib.UI.Data;

public class UIScrollPanel : UIPanel
{
    protected EventOnScroll? EventOnScroll;
    protected Vector2 TotalScrollOffset;
    protected Rect ChildrenBounds;

    public bool HorizontalScrollEnable;
    public bool VerticalScrollEnable;
    public float ScrollSpeed = 1;

    protected void UpdateChildrenBounds()
    {
        foreach (var uiBase in Children)
        {
            ChildrenBounds.UpdateChildrenBounds(uiBase.Rect);
        }
    }

    protected override void ValidInit()
    {
        base.ValidInit();
        if (Self == null) return;

        EventOnScroll = Self.GetOrAdd<EventOnScroll>();
        EventOnScroll.Scroll -= EventScrollScroll;
        EventOnScroll.Scroll += EventScrollScroll;
        Self.sizeDelta = Size;
        UpdateChildrenBounds();
    }

    public override void Reset()
    {
        base.Reset();
        TotalScrollOffset = Vector2.zero;
    }

    protected virtual void EventScrollScroll(Vector2 scrollDelta, RectTransform self)
    {
        if (HorizontalScrollEnable)
        {
            var d = scrollDelta.x * ScrollSpeed;
            TotalScrollOffset.x += d;
            var xMin = LocalPosition.x + ChildrenBounds.xMax - Size.x;
            var xMax = LocalPosition.x + ChildrenBounds.xMin;
            xMax = -xMax;
            xMin = Mathf.Min(-xMin, xMax);
            TotalScrollOffset.x = Mathf.Clamp(TotalScrollOffset.x, xMin, xMax);
        }

        if (VerticalScrollEnable)
        {
            var d = scrollDelta.y * ScrollSpeed;
            TotalScrollOffset.y += d;
            var yMin = LocalPosition.y + ChildrenBounds.yMax - Size.y;
            var yMax = LocalPosition.y + ChildrenBounds.yMin;
            yMax = -yMax;
            yMin = Mathf.Min(-yMin, yMax);
            TotalScrollOffset.y = Mathf.Clamp(TotalScrollOffset.y, yMin, yMax);
        }

        self.anchoredPosition = LocalPosition + TotalScrollOffset;
    }
}