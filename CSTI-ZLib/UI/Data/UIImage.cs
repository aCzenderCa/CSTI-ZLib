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
        if (string.IsNullOrEmpty(Sprite)) return;

        if (ImageCom == null) ImageCom = Self.GetOrAdd<Image>();
        ImageCom.color = Color;
        if (ModLoader.ModLoader.SpriteDict.TryGetValue(Sprite, out var sprite))
        {
            ImageCom.sprite = sprite;
        }

        ImageCom.enabled = ImageCom.sprite;
    }
}