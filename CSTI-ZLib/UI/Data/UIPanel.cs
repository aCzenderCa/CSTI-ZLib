using System.Collections.Generic;
using System.Linq;
using CSTI_ZLib.LuaLIbs.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CSTI_ZLib.UI.Data;

public class UIPanel : UIImage
{
    public List<CommonUIBase> Children = new();
    public bool GridLayoutEnable; // 当存在无限放置的轴时，优先在另一个轴上排列，否则优先水平排列，不存在无限放置的轴时，子节点数量存在上限
    public int GridSizeHorizontal = 1; // 每行可以放置多少个UI节点，小于等于0表示无限
    public int GridSizeVertical = -1; // 每列可以放置多少个UI节点，小于等于0表示无限

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

    public UICard AddCard(string name, string image, string subImage, float x = 0, float y = 0, float width = 100, float height = 100)
    {
        var uiCard = new UICard
        {
            Name = name,
            Sprite = image,
            LocalPosition = new Vector2(x, y),
            Size = new Vector2(width, height),
            Rotation = 0,
            SubSize = new Vector2(width, height),
            SubSpriteName = subImage,
        };
        Children.Add(uiCard);
        return uiCard;
    }

    public UIText AddText(string name, string label, float fontSize, float x = 0, float y = 0, float width = 100,
        float height = 100)
    {
        var uiText = new UIText
        {
            Name = name,
            Text = label,
            FontSize = fontSize,
            LocalPosition = new Vector2(x, y),
            Size = new Vector2(width, height),
            Rotation = 0,
        };
        Children.Add(uiText);
        return uiText;
    }

    public UIButton AddButton(string name, string image, string label, float fontSize, float x = 0, float y = 0, float width = 100,
        float height = 100)
    {
        var uiButton = new UIButton
        {
            Name = name,
            Sprite = image,
            Label = label,
            FontSize = fontSize,
            LocalPosition = new Vector2(x, y),
            Size = new Vector2(width, height),
            Rotation = 0,
            SubSize = new Vector2(width, height),
        };
        Children.Add(uiButton);
        return uiButton;
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

        BuildOrInitChildren(Self);
    }

    protected virtual void BuildOrInitChildren(RectTransform parent)
    {
        if (GridLayoutEnable)
        {
            var zMax = 0f;
            var zCount = 0;
            var wCount = 0;
            var childPoint = default(Vector2);
            if (GridSizeVertical < 1)
            {
                foreach (var child in Children)
                {
                    child.LocalPosition = childPoint;
                    childPoint.x += child.Size.x;
                    zCount++;
                    zMax = Mathf.Max(zMax, child.LocalPosition.y);
                    if (zCount >= GridSizeHorizontal)
                    {
                        childPoint.x = 0;
                        childPoint.y += zMax;
                        zMax = 0;
                        zCount = 0;
                    }
                }
            }
            else if (GridSizeHorizontal < 1)
            {
                foreach (var child in Children)
                {
                    child.LocalPosition = childPoint;
                    childPoint.y += child.Size.y;
                    zCount++;
                    zMax = Mathf.Max(zMax, child.LocalPosition.x);
                    if (zCount >= GridSizeVertical)
                    {
                        childPoint.y = 0;
                        childPoint.x += zMax;
                        zMax = 0;
                        zCount = 0;
                    }
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    child.LocalPosition = childPoint;
                    childPoint.x += child.Size.x;
                    zCount++;
                    zMax = Mathf.Max(zMax, child.LocalPosition.y);
                    if (zCount >= GridSizeHorizontal)
                    {
                        childPoint.x = 0;
                        childPoint.y += zMax;
                        zCount = 0;
                        zMax = 0;
                        wCount++;
                        if (wCount >= GridSizeVertical)
                        {
                            break;
                        }
                    }
                }
            }
        }

        foreach (var uiBase in Children)
        {
            if (uiBase.Self == null)
            {
                uiBase.Build(parent);
            }
            else
            {
                uiBase.FullInit();
            }
        }
    }
}