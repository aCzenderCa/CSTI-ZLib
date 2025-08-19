using System;
using System.Collections.Generic;
using System.Linq;
using ChatTreeLoader.Util;
using CSTI_LuaActionSupport.Helper;
using CSTI_LuaActionSupport.LuaCodeHelper;
using CSTI_ZLib.LuaLIbs.Utils;
using CSTI_ZLib.UI.Com;
using CSTI_ZLib.UI.Utils;
using CSTI_ZLib.Utils;
using NLua;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CSTI_ZLib.UI.Data;

public class CommonUIBase : IDisposable
{
    public RectTransform? Self;
    public string Name = "";
    public Vector2 Size = new(100, 100);
    public Vector2 LocalPosition;
    public float Rotation;
    public Vector2 LocalScale = Vector2.one;

    #region Event

    private EventOnClick? _eventOnClickCom;
    private EventOnDropOn? _eventOnDropOnCom;
    private event Action? IntelOnClick;
    private List<LuaFunction>? IntelLuaOnClicks;
    private event EventOnDropOn.OnCardDropOnDelegate? IntelOnCardDropOn;
    private List<LuaFunction>? IntelLuaOnCardDropOn;

    internal event Action? OnClick
    {
        add
        {
            IntelOnClick += value;
            UpdateEvents();
        }
        remove
        {
            IntelOnClick -= value;
            UpdateEvents();
        }
    }

    internal event EventOnDropOn.OnCardDropOnDelegate? OnCardDropOn
    {
        add
        {
            IntelOnCardDropOn += value;
            UpdateEvents();
        }
        remove
        {
            IntelOnCardDropOn -= value;
            UpdateEvents();
        }
    }

    private void UpdateEvents()
    {
        if (Self == null) return;
        if (IntelOnClick != null || IntelLuaOnClicks != null)
        {
            _eventOnClickCom = Self.GetOrAdd<EventOnClick>();
            _eventOnClickCom.ClickEvent -= EventClickEventComClick;
            _eventOnClickCom.ClickEvent += EventClickEventComClick;
        }

        if (IntelOnCardDropOn != null || IntelLuaOnCardDropOn != null)
        {
            _eventOnDropOnCom = Self.GetOrAdd<EventOnDropOn>();
            _eventOnDropOnCom.CardDropOnEvent -= EventOnClickComOnCardDropOn;
            _eventOnDropOnCom.CardDropOnEvent += EventOnClickComOnCardDropOn;
        }
    }

    private void EventClickEventComClick()
    {
        IntelOnClick?.Invoke();
        if (IntelLuaOnClicks != null)
        {
            foreach (var intelLuaOnClick in IntelLuaOnClicks)
            {
                intelLuaOnClick.Call();
            }

            GameManager.Instance.ProcessCache().ProcessAll();
        }
    }

    private void EventOnClickComOnCardDropOn(IReadOnlyList<InGameCardBase> draggedCards)
    {
        IntelOnCardDropOn?.Invoke(draggedCards);
        var cardAccessBridges = draggedCards.Select(card => new CardAccessBridge(card)).ToLuaList();
        if (IntelLuaOnCardDropOn != null)
        {
            foreach (var intelLuaOnCardDropOn in IntelLuaOnCardDropOn)
            {
                intelLuaOnCardDropOn.Call(cardAccessBridges);
            }

            GameManager.Instance.ProcessCache().ProcessAll();
        }
    }

    public void AddOnClick(LuaFunction action)
    {
        IntelLuaOnClicks ??= [];
        IntelLuaOnClicks.Add(action);
        UpdateEvents();
    }

    public void RemoveOnClick(LuaFunction action)
    {
        IntelLuaOnClicks ??= [];
        IntelLuaOnClicks.Remove(action);
        UpdateEvents();
    }

    public void AddOnCardDropOn(LuaFunction action)
    {
        IntelLuaOnCardDropOn ??= [];
        IntelLuaOnCardDropOn.Add(action);
        UpdateEvents();
    }

    public void RemoveOnCardDropOn(LuaFunction action)
    {
        IntelLuaOnCardDropOn ??= [];
        IntelLuaOnCardDropOn.Remove(action);
        UpdateEvents();
    }

    #endregion

    public Rect Rect => new(LocalPosition, Size);

    public void Build(Transform parent)
    {
        Self = parent.GetChildOrCreate(Name);
        FullInit();
    }

    public void FullInit()
    {
        Init();
        ValidInit();
    }

    public void SetPosition(float x, float y)
    {
        LocalPosition = new Vector2(x, y);
    }

    public void SetSize(float width, float height)
    {
        Size = new Vector2(width, height);
    }

    /// <summary>
    /// 事件执行顺序 Base.Init -> Super.Init
    /// </summary>
    protected virtual void Init()
    {
        if (Self == null) return;
        Self.sizeDelta = new Vector2(Mathf.Max(0, Size.x), Mathf.Max(0, Size.y));
        Self.anchoredPosition = LocalPosition;
        Self.localRotation = Quaternion.Euler(0, 0, Rotation);
        Self.localScale = LocalScale;

        UpdateEvents();
    }

    /// <summary>
    /// 事件执行顺序 Base.ValidInit -> Super.ValidInit
    /// </summary>
    protected virtual void ValidInit()
    {
    }

    public virtual void Reset()
    {
    }

    public void Destroy()
    {
        if (Self != null)
        {
            Object.Destroy(Self.gameObject);
        }
    }

    public virtual void Dispose()
    {
        if (Self == null) return;
        Self.DestroyCom<EventOnClick>();
    }

    ~CommonUIBase()
    {
        Dispose();
    }
}