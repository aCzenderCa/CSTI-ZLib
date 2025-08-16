using CSTI_ZLib.LuaLIbs.Utils;
using CSTI_ZLib.UI.Com;
using UnityEngine;
using UnityEngine.UI;

namespace CSTI_ZLib.UI.Data
{
    public class UIScrollPanel : UIPanel
    {
        protected EventOnScroll? EventOnScroll;
        public bool HorizontalScrollEnable;
        public bool VerticalScrollEnable;
        public Rect ChildrenBounds;
        public float ScrollSpeed = 1;
        public Vector2 TotalScrollOffset;

        protected override void ValidInit()
        {
            base.ValidInit();
            if (Self == null) return;
            EventOnScroll = Self.GetOrAdd<EventOnScroll>();
            EventOnScroll.Scroll -= EventScrollScroll;
            EventOnScroll.Scroll += EventScrollScroll;

            foreach (var uiBase in Children)
            {
                if (ChildrenBounds.xMin > uiBase.LocalPosition.x) ChildrenBounds.xMin = uiBase.LocalPosition.x;
                if (ChildrenBounds.yMin > uiBase.LocalPosition.y) ChildrenBounds.yMin = uiBase.LocalPosition.y;
                if (ChildrenBounds.xMax < uiBase.LocalPosition.x + uiBase.Size.x)
                    ChildrenBounds.xMax = uiBase.LocalPosition.x + uiBase.Size.x;
                if (ChildrenBounds.yMax < uiBase.LocalPosition.y + uiBase.Size.y)
                    ChildrenBounds.yMax = uiBase.LocalPosition.y + uiBase.Size.y;
            }
        }

        private void EventScrollScroll(Vector2 scrollDelta, RectTransform self)
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
}