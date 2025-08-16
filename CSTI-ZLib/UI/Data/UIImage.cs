using CSTI_ZLib.LuaLIbs.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CSTI_ZLib.UI.Data;

public class UIImage : CommonUIBase
{
    private static Sprite? _defaultSprite;
    protected Image? ImageCom;

    protected static Sprite DefaultSprite
    {
        get
        {
            if (_defaultSprite == null)
            {
                _defaultSprite = UnityEngine.Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
            }

            return _defaultSprite;
        }
    }

    public Color Color = Color.white;
    public string Sprite = "";

    public void SetColor(float r, float g, float b)
    {
        Color = new Color(r, g, b, Color.a);
    }

    public void SetAlpha(float a)
    {
        Color = new Color(Color.r, Color.g, Color.b, a);
    }

    protected override void Init()
    {
        base.Init();

        if (Self == null) return;
        if (!string.IsNullOrEmpty(Sprite))
        {
            if (ImageCom == null) ImageCom = Self.GetOrAdd<Image>();
            InitImage(Self, ImageCom, Sprite, Color, Size);
        }
        else if (ImageCom != null)
        {
            ImageCom.enabled = false;
        }
    }

    protected static void InitImage(RectTransform self, Image image, string spriteName, Color color, Vector2 size)
    {
        image.color = color;
        if (ModLoader.ModLoader.SpriteDict.TryGetValue(spriteName, out var sprite))
        {
            image.sprite = sprite;
            var ratio = sprite.rect.width / sprite.rect.height;
            if (size is { x: < 0, y: < 0 })
            {
                self.sizeDelta = sprite.rect.size;
            }
            else if (size is { x: < 0, y: > 0 })
            {
                self.sizeDelta = new Vector2(ratio * size.y, size.y);
            }
            else if (size is { x: > 0, y: < 0 })
            {
                self.sizeDelta = new Vector2(size.x, size.x / ratio);
            }
        }

        image.enabled = image.sprite;
    }
}