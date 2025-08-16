using System.Collections.Generic;
using System.Linq;
using CSTI_ZLib.LuaLIbs.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CSTI_ZLib.UI.Data;

public class UIPanel : UIImage
{
    public List<CommonUIBase> Children = new();

    #region Add

    public UIPanel AddPanel(string name, string image, float x = 0, float y = 0, float width = 100, float height = 100)
    {
        var uiPanel = new UIPanel
        {
            Name = name,
            Sprite = image,
            LocalPosition = new Vector2(x, y),
            Size = new Vector2(width, height),
            Rotation = 0,
        };
        Children.Add(uiPanel);
        return uiPanel;
    }

    public UIImage AddImage(string name, string image, float x = 0, float y = 0, float width = 100, float height = 100)
    {
        var uiImage = new UIImage
        {
            Name = name,
            Sprite = image,
            LocalPosition = new Vector2(x, y),
            Size = new Vector2(width, height),
            Rotation = 0,
        };
        Children.Add(uiImage);
        return uiImage;
    }

    public UIText AddText(string name, string text, float fontSize, float x = 0, float y = 0, float width = 100,
        float height = 100)
    {
        var uiText = new UIText
        {
            Name = name,
            Text = text,
            FontSize = fontSize,
            LocalPosition = new Vector2(x, y),
            Size = new Vector2(width, height),
            Rotation = 0,
        };
        Children.Add(uiText);
        return uiText;
    }

    public UIScrollPanel AddScrollPanel(string name, bool horizontalScroll, float x = 0, float y = 0,
        float width = 100, float height = 100, float scrollSpeed = 1)
    {
        var uiScrollPanel = new UIScrollPanel
        {
            Name = name,
            LocalPosition = new Vector2(x, y),
            Size = new Vector2(width, height),
            Rotation = 0,
            HorizontalScrollEnable = horizontalScroll,
            VerticalScrollEnable = !horizontalScroll,
            ScrollSpeed = scrollSpeed,
        };
        Children.Add(uiScrollPanel);
        return uiScrollPanel;
    }

    public UIScrollRect AddScrollRect(string name, bool horizontalScroll, float x = 0, float y = 0,
        float width = 100, float height = 100, float scrollSpeed = 1, bool mask = false)
    {
        var uiScrollRect = new UIScrollRect
        {
            Name = name,
            LocalPosition = new Vector2(x, y),
            Size = new Vector2(width, height),
            Rotation = 0,
            HorizontalScrollEnable = horizontalScroll,
            VerticalScrollEnable = !horizontalScroll,
            ScrollSpeed = scrollSpeed,
            Mask = mask,
        };
        Children.Add(uiScrollRect);
        return uiScrollRect;
    }

    #endregion

    /// <summary>
    /// 事件执行顺序 Base.Init -> ChildrenDep0.Init -> ... -> ChildrenDepN.Init -> ChildrenDepN.ValidInit ->
    /// ... -> ChildrenDep0.ValidInit -> Base.ValidInit
    /// </summary>
    protected override void Init()
    {
        base.Init();

        if (Self == null) return;
        if (!string.IsNullOrEmpty(Sprite))
        {
            var image = Self.GetOrAdd<Image>();
            image.type = Image.Type.Sliced;
            if (!image.sprite)
            {
                image.sprite = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(sprite => sprite.name == Sprite);
            }
        }

        BuildOrInitChildren();
    }

    protected virtual void BuildOrInitChildren()
    {
        if (Self == null) return;
        foreach (var uiBase in Children)
        {
            if (uiBase.Self == null)
            {
                uiBase.Build(Self);
            }
            else
            {
                uiBase.FullInit();
            }
        }
    }

    public override void Dispose()
    {
        base.Dispose();

        foreach (var uiBase in Children)
        {
            uiBase.Dispose();
        }

        if (Self != null) Object.Destroy(Self.transform);
    }
}