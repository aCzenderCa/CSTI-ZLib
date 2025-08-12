using System;
using CSTI_LuaActionSupport.AllPatcher;
using CSTI_LuaActionSupport.LuaCodeHelper;

namespace CSTI_ZLib.LuaLIbs.Utils
{
    public class CardBridgeDataToken : IDisposable
    {
        public CardActionPatcher.DataNodeTableAccessBridge Data => _card.Data!;
        private readonly CardAccessBridge _card;

        public CardBridgeDataToken(CardAccessBridge card)
        {
            card.InitData();
            _card = card;
        }

        public void Dispose()
        {
            _card.SaveData();
        }
    }

    public static class CardBridgeUtils
    {
        public static CardBridgeDataToken UseData(this CardAccessBridge card)
        {
            return new CardBridgeDataToken(card);
        }
    }
}