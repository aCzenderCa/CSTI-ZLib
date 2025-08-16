using System.Linq;
using CSTI_LuaActionSupport.AllPatcher;
using CSTI_LuaActionSupport.LuaCodeHelper;
using CSTI_ZLib.LuaLIbs.Data;
using CSTI_ZLib.LuaLIbs.Utils;
using CSTI_ZLib.Patcher;
using CSTI_ZLib.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace CSTI_ZLib.LuaLIbs;

[LuaLib]
public static class CardVisualize
{
    #region Init

    internal static void LuaLibInit()
    {
        CardPatcher.OnCardInit += OnCardInit;
        CardPatcher.OnCardRecycle += OnCardRecycle;
    }

    #endregion

    #region Event

    private static void OnCardRecycle(InGameCardBase inGameCardBase)
    {
        DestroyCImage(inGameCardBase.transform);
    }

    private static void OnCardInit(InGameCardBase inGameCardBase)
    {
        var cardAccessBridge = new CardAccessBridge(inGameCardBase);
        var data = cardAccessBridge.Data!;
        if (data[CVRecordKey] is { } s)
        {
            var cardVisualizeRecord = JsonUtility.FromJson<CardVisualizeRecord>(s.ToString());
            foreach (var cardVisualizeRecordItem in cardVisualizeRecord.records)
            {
                var offset = cardVisualizeRecordItem.offset;
                var size = cardVisualizeRecordItem.size;
                switch (cardVisualizeRecordItem.dataType)
                {
                    case CardVisualizeRecordItem.DataType.IconId:
                        SetIcon(cardAccessBridge, cardVisualizeRecordItem.data, offset.x, offset.y, size.x, size.y,
                            cardVisualizeRecordItem.name);
                        break;
                    case CardVisualizeRecordItem.DataType.CardIdForIcon:
                        var cardData = CardActionPatcher.AccessTool[cardVisualizeRecordItem.data]?.UniqueIDScriptable as CardData;
                        if (cardData)
                        {
                            SetIconInternal(cardAccessBridge, cardData.CardImage, cardVisualizeRecordItem.name,
                                offset.x, offset.y, size.x, size.y);
                        }

                        break;
                    case CardVisualizeRecordItem.DataType.LocalKey:
                        SetText(cardAccessBridge, cardVisualizeRecordItem.data, offset.x, offset.y, size.x, size.y,
                            cardVisualizeRecordItem.name);
                        break;
                }
            }
        }
    }

    #endregion

    public const string CVRecordKey = "CardVisualize.Record";

    #region API

    public static void SetText(object self, string localKey, float x, float y, float width, float height,
        string? overwriteKey = null)
    {
        if (!CardUtils.TryParseCard(self, out var cardBridge)) return;

        var cardBase = cardBridge.CardBase!;
        var text = GetOrCreateText(cardBase.transform, overwriteKey ?? localKey);
        text.text = new LocalizedString { DefaultText = localKey, LocalizationKey = localKey };
        text.enableAutoSizing = true;
        var rt = text.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height);
        rt.localPosition = new Vector3(x, y, 1);
        var cardVisualizeRecordItem =
            CardVisualizeRecordItem.FromLocalized(localKey, new Vector2(x, y), new Vector2(width, height));
        if (overwriteKey != null) cardVisualizeRecordItem.name = overwriteKey;

        using var dataToken = cardBridge.UseData();
        var record = dataToken.GetOrInitRecord();
        record.Record(cardVisualizeRecordItem);
        dataToken.Data[CVRecordKey] = JsonUtility.ToJson(record);
    }

    public static void SetIcon(object self, string icon, float x, float y, float width, float height, string? overwriteKey = null)
    {
        if (!CardUtils.TryParseCard(self, out var cardBridge)) return;

        var cardBase = cardBridge.CardBase!;
        if (ModLoader.ModLoader.SpriteDict.TryGetValue(icon, out var sprite))
        {
            var image = GetOrCreateImage(cardBase.transform, overwriteKey ?? icon);
            image.sprite = sprite;
            var rt = image.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(width, height);
            rt.localPosition = new Vector3(x, y, 1);

            var cardVisualizeRecordItem = CardVisualizeRecordItem.FromIcon(icon, new Vector2(x, y), new Vector2(width, height));
            if (overwriteKey != null) cardVisualizeRecordItem.name = overwriteKey;

            using var dataToken = cardBridge.UseData();
            var record = dataToken.GetOrInitRecord();
            record.Record(cardVisualizeRecordItem);
            dataToken.Data[CVRecordKey] = JsonUtility.ToJson(record);
        }
    }

    public static void SetIconByCard(object self, object iconCard, float x, float y, float width, float height,
        string? overwriteKey = null)
    {
        if (!CardUtils.TryParseCard(self, out var cardBridge)) return;
        if (!CardUtils.TryParseCard(iconCard, out var iconCardBridge)) return;

        var sprite = iconCardBridge.CardBase!.CardModel.CardImage;
        var image = GetOrCreateImage(cardBridge.Transform!, overwriteKey ?? iconCardBridge.CardBase.name);
        image.sprite = sprite;
        var rt = image.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height);
        rt.localPosition = new Vector3(x, y, 1);

        var cardVisualizeRecordItem =
            CardVisualizeRecordItem.FromCardIcon(iconCardBridge, new Vector2(x, y), new Vector2(width, height));
        if (overwriteKey != null) cardVisualizeRecordItem.name = overwriteKey;

        using var dataToken = cardBridge.UseData();
        var record = dataToken.GetOrInitRecord();
        record.Record(cardVisualizeRecordItem);
        dataToken.Data[CVRecordKey] = JsonUtility.ToJson(record);
    }

    public static void ClearIcon(object self)
    {
        if (!CardUtils.TryParseCard(self, out var cardBridge)) return;
        DestroyCImage(cardBridge.Transform!);

        using var dataToken = cardBridge.UseData();
        dataToken.Data![CVRecordKey] = null;
    }

    #endregion

    #region Utils

    private static CardVisualizeRecord GetOrInitRecord(this CardBridgeDataToken dataToken)
    {
        CardVisualizeRecord record;
        var s = dataToken.Data[CVRecordKey]?.ToString();
        if (string.IsNullOrEmpty(s))
        {
            record = new CardVisualizeRecord();
        }
        else
        {
            record = JsonUtility.FromJson<CardVisualizeRecord>(s);
        }

        return record;
    }

    private static TextMeshProUGUI GetOrCreateText(Transform baseTr, string name = "TXT")
    {
        var cimg = baseTr.GetChildOrCreate("CIMG");
        var o = cimg.GetChildOrCreate(name);
        var txt = o.GetOrAdd<TextMeshProUGUI>();
        txt.text = "";
        var fontSetting = FontsManager.Instance.FontSets[FontsManager.Instance.CurrentFontSet].Settings.First();
        txt.font = fontSetting.FontObject;
        txt.fontMaterial = fontSetting.FontMat;
        txt.raycastTarget = false;
        return txt;
    }

    private static Image GetOrCreateImage(Transform baseTr, string name = "IMG")
    {
        var cimg = baseTr.GetChildOrCreate("CIMG");
        var o = cimg.GetChildOrCreate(name);
        var image = o.GetOrAdd<Image>();
        image.raycastTarget = false;
        return image;
    }

    private static void DestroyCImage(Transform baseTr)
    {
        if (!baseTr.TryGetChild("CIMG", out var cimg)) return;
        Object.Destroy(cimg.gameObject);
    }

    private static void SetIconInternal(CardAccessBridge cardBridge, Sprite sprite, string name, float x, float y, float width,
        float height)
    {
        var image = GetOrCreateImage(cardBridge.Transform!, name);
        image.sprite = sprite;
        var rt = image.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height);
        rt.localPosition = new Vector3(x, y, 1);
    }

    #endregion
}