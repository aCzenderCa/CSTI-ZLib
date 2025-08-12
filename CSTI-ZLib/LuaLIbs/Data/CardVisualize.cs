using System;
using System.Collections.Generic;
using CSTI_LuaActionSupport.LuaCodeHelper;
using UnityEngine;

namespace CSTI_ZLib.LuaLIbs.Data
{
    [Serializable]
    public class CardVisualizeRecordItem
    {
        public enum DataType
        {
            IconId,
            CardIdForIcon,
            LocalKey,
        }

        public DataType dataType;
        public string data;
        public string name;
        public Vector2 offset;
        public Vector2 size;

        public CardVisualizeRecordItem(string data, string name, Vector2 offset, Vector2 size, DataType dataType)
        {
            this.data = data;
            this.offset = offset;
            this.size = size;
            this.dataType = dataType;
            this.name = name;
        }

        public static CardVisualizeRecordItem FromIcon(string icon, Vector2 offset, Vector2 size)
        {
            return new CardVisualizeRecordItem(icon, icon, offset, size, DataType.IconId);
        }

        public static CardVisualizeRecordItem FromCardIcon(CardAccessBridge card, Vector2 offset, Vector2 size)
        {
            return new CardVisualizeRecordItem(card.CardBase!.CardModel.UniqueID, card.CardBase.name, offset, size, DataType.CardIdForIcon);
        }

        public static CardVisualizeRecordItem FromLocalized(string localKey, Vector2 offset, Vector2 size)
        {
            return new CardVisualizeRecordItem(localKey, localKey, offset, size, DataType.LocalKey);
        }
    }

    [Serializable]
    public class CardVisualizeRecord
    {
        public List<CardVisualizeRecordItem> records = new();

        public void Record(CardVisualizeRecordItem item)
        {
            if (records.FindIndex(recordItem =>
                    recordItem.data == item.data && recordItem.dataType == item.dataType) is var i and >= 0)
            {
                records[i] = item;
            }
            else
            {
                records.Add(item);
            }
        }
    }
}