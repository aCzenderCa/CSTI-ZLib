using CSTI_ZLib.LuaLIbs.Utils;
using TMPro;

namespace CSTI_ZLib.UI.Data
{
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
            TextCom = Self.GetOrAdd<TextMeshProUGUI>();
            TextCom.text = Text;
            TextCom.fontSize = FontSize;
            TextCom.verticalAlignment = VerticalAlignmentOptions.Middle;
            TextCom.horizontalAlignment = HorizontalAlignmentOptions.Center;
            TextCom.alignment = TextAlignment;
        }
    }
}