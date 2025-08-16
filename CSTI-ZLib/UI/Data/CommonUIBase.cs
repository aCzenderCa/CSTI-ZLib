using System;
using CSTI_ZLib.LuaLIbs.Utils;
using CSTI_ZLib.UI.Com;
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

    private EventOnClick? _eventOnClick;
    private event Action? IntelOnClick;

    protected event Action? OnClick
    {
        add
        {
            IntelOnClick += value;
            UpdateOnClick();
        }
        remove
        {
            IntelOnClick -= value;
            UpdateOnClick();
        }
    }

    private void UpdateOnClick()
    {
        if (Self == null) return;
        _eventOnClick = Self.GetOrAdd<EventOnClick>();
        _eventOnClick.OnClick -= EventOnClickOnClick;
        _eventOnClick.OnClick += EventOnClickOnClick;
    }

    protected virtual void EventOnClickOnClick()
    {
        IntelOnClick?.Invoke();
    }

    public void AddOnClick(Action action)
    {
        OnClick += action;
    }

    public void RemoveOnClick(Action action)
    {
        OnClick -= action;
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

    /// <summary>
    /// 事件执行顺序 Base.Init -> Super.Init
    /// </summary>
    protected virtual void Init()
    {
        if (Self == null) return;
        Self.sizeDelta = Size;
        Self.anchoredPosition = LocalPosition;
        Self.localRotation = Quaternion.Euler(0, 0, Rotation);
        Self.localScale = LocalScale;

        if (IntelOnClick != null)
        {
            UpdateOnClick();
        }
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