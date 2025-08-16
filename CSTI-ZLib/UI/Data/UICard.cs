using CSTI_ZLib.LuaLIbs.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CSTI_ZLib.UI.Data;

public class UICard : UIImage
{
    protected RectTransform? SubNode;
    protected Image? SubImageCom;

    public Vector2 SubPosition;
    public Vector2 SubSize;
    public string SubSpriteName = "";

    public void SetSubPosition(float x, float y)
    {
        SubPosition = new Vector2(x, y);
    }

    public void SetSubSize(float width, float height)
    {
        SubSize = new Vector2(width, height);
    }

    protected override void Init()
    {
        base.Init();
        if (Self == null) return;
        if (!string.IsNullOrEmpty(SubSpriteName))
        {
            SubNode = Self.GetChildOrCreate(nameof(SubImageCom));
            SubNode.anchoredPosition = SubPosition;
            SubNode.sizeDelta = new Vector2(Mathf.Max(0, SubSize.x), Mathf.Max(0, SubSize.y));

            SubImageCom = SubNode.GetOrAdd<Image>();
            InitImage(SubNode, SubImageCom, SubSpriteName, Color, SubSize);
        }
        else if (SubImageCom != null)
        {
            SubImageCom.enabled = false;
        }
    }
}