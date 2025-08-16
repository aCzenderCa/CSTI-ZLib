using CSTI_ZLib.LuaLIbs.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CSTI_ZLib.UI.Data;

public class UIScrollRect : UIScrollPanel
{
    protected RectMask2D? MaskCom;
    protected RectTransform? ContentRoot;
    public bool Mask;

    protected override void Init()
    {
        base.Init();
        if (Self == null) return;

        if (Mask)
        {
            MaskCom = Self.GetOrAdd<RectMask2D>();
            MaskCom.enabled = true;
        }
        else if (MaskCom != null)
        {
            MaskCom.enabled = false;
        }
    }

    protected override void BuildOrInitChildren()
    {
        if (Self == null) return;
        ContentRoot = Self.GetChildOrCreate("ContentRoot");

        foreach (var uiBase in Children)
        {
            if (uiBase.Self == null)
            {
                uiBase.Build(ContentRoot);
            }
            else
            {
                uiBase.FullInit();
            }
        }
    }

    protected override void EventScrollScroll(Vector2 scrollDelta, RectTransform self)
    {
        base.EventScrollScroll(scrollDelta, self);

        if (ContentRoot == null) return;
        self.anchoredPosition = LocalPosition;
        ContentRoot.anchoredPosition = TotalScrollOffset;
    }
}