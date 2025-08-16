using System;
using CSTI_ZLib.LuaLIbs.Utils;

namespace CSTI_ZLib.LuaLIbs.Data
{
    public class CardDataBase
    {
        public string Id;
        public string Name;
        public string Description;
        public float Weight;
        public string CardType = nameof(CardTypes.Item);

        public CardDataBase(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        protected virtual void OnBuild(CardData cardData)
        {
        }

        public CardData Build()
        {
            var cardData = ObjectInit<CardData>.Empty;
            cardData.UniqueID = Id;
            cardData.name = $"{Id}_{Name}";
            cardData.CardName = new LocalizedString { DefaultText = Name, LocalizationKey = Name };
            cardData.CardDescription = new LocalizedString { DefaultText = Description, LocalizationKey = Description };
            cardData.ObjectWeight = Weight;
            Enum.TryParse(CardType, out cardData.CardType);

            var cardImageId = $"CardImage_{Id}";
            cardData.LoadImage(cardImageId, (data, sprite) => data.CardImage = sprite);

            OnBuild(cardData);

            GameLoad.Instance.DataBase.AllData.Add(cardData);
            cardData.Init();

            return cardData;
        }
    }
}