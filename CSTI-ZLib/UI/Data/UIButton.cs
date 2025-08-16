using CSTI_ZLib.LuaLIbs.Utils;
using TMPro;
using UnityEngine;

namespace CSTI_ZLib.UI.Data;

public class UIButton : UIImage
{
    protected RectTransform? SubNode;
    protected TextMeshProUGUI? SubTextCom;

    public string Label = "";
    public float FontSize = 16;
    public Vector2 SubPosition;
    public Vector2 SubSize;
    public TextAlignmentOptions TextAlignment = TextAlignmentOptions.Center;

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
        if (!string.IsNullOrEmpty(Label))
        {
            SubNode = Self.GetChildOrCreate(nameof(SubTextCom));
            SubNode.anchoredPosition = SubPosition;
            SubNode.sizeDelta = new Vector2(Mathf.Max(0, SubSize.x), Mathf.Max(0, SubSize.y));

            SubTextCom = SubNode.GetOrAdd<TextMeshProUGUI>();
            SubTextCom.text = new LocalizedString { DefaultText = Label, LocalizationKey = Label }.ToString();
            SubTextCom.fontSize = FontSize;
            SubTextCom.alignment = TextAlignment;
        }
        else if (SubTextCom != null)
        {
            SubTextCom.enabled = false;
        }
    }
}