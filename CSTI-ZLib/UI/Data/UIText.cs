using CSTI_ZLib.LuaLIbs.Utils;
using TMPro;

namespace CSTI_ZLib.UI.Data;

public class UIText : CommonUIBase
{
    protected TextMeshProUGUI? TextCom;
    public float FontSize = 16;
    public string Text = "";
    public TextAlignmentOptions TextAlignment = TextAlignmentOptions.Center;

    protected override void Init()
    {
        base.Init();

        if (Self == null) return;
        if (!string.IsNullOrEmpty(Text))
        {
            TextCom = Self.GetOrAdd<TextMeshProUGUI>();
            TextCom.text = new LocalizedString { DefaultText = Text, LocalizationKey = Text }.ToString();
            TextCom.fontSize = FontSize;
            TextCom.alignment = TextAlignment;
        }
        else if (TextCom != null)
        {
            TextCom.enabled = false;
        }
    }
}