using UnityEngine;

namespace CSTI_ZLib.UI.Utils
{
    public static class AreaUtils
    {
        public static void UpdateChildrenBounds(ref this Rect area, Rect child)
        {
            if (area.xMin > child.x) area.xMin = child.x;
            if (area.yMin > child.y) area.yMin = child.y;

            if (area.xMax < child.xMax) area.xMax = child.xMax;
            if (area.yMax < child.yMax) area.yMax = child.yMax;
        }
    }
}