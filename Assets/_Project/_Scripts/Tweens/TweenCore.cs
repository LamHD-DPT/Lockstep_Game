using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

internal class TweenCore
{
    public float Duration = .2f;
    public Ease EaseType = Ease.Linear;
}

internal class TweenProperties : TweenCore
{
    public Vector2 EndValue;
    public Vector2 StartValue;
}

internal class TweenProperties2 : TweenCore
{
    public float EndValue;
    public float StartValue;
}

[Serializable]
internal class UIElement : TweenProperties
{
    public RectTransform rectTransform;
}

[Serializable]
internal class UICanvasGroup : TweenProperties2
{
    public CanvasGroup canvasGroup;
}

[Serializable]
internal class UIImage : TweenProperties2
{
    public Image image;
}