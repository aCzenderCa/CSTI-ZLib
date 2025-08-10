using CSTI_LuaActionSupport.LuaCodeHelper;

namespace CSTI_ZLib.LuaLIbs.Utils
{
    public static class CardUtils
    {
        public static bool TryParseCard(object card, out CardAccessBridge cardBridge)
        {
            cardBridge = ParseCard(card);
            return cardBridge != null;
        }

        public static CardAccessBridge? ParseCard(object card)
        {
            if (card is CardAccessBridge cardAccessBridge)
            {
                return cardAccessBridge;
            }

            if (card is InGameCardBase inGameCardBase)
            {
                return new CardAccessBridge(inGameCardBase);
            }

            return null;
        }
    }
}