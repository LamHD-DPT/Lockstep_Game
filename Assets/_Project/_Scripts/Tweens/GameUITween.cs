using DG.Tweening;
using Racer.SoundManager;
using Racer.Utilities;
using UnityEngine;

internal class GameUITween : MonoBehaviour
{
    private Tween _tween;

    private bool _isResetPos;
    private bool _isResetScale;

    [SerializeField] private UIElement infoUI;
    [SerializeField] private UIElement combinationUI;
    [SerializeField] private UIElement fillUI;
    [SerializeField] private UIElement frameUI;
    [SerializeField] private UIElement slideUI;
    [SerializeField] private UIElement clockwiseUI;
    [SerializeField] private UIElement antiClockwiseUI;
    [SerializeField] private UIElement guideUI;
    [SerializeField] private UIElement degreeUI;
    [SerializeField] private UIElement actionBtnUI;
    [SerializeField] private UIElement newUI;


    public void DisplayInfoUI(bool value, float delay = 0, AudioClip clip = null)
    {
        if (value)
            infoUI.rectTransform.DOAnchorPos(infoUI.EndValue,
                    infoUI.Duration)
                .SetDelay(delay)
                .SetUpdate(true)
                .SetEase(infoUI.EaseType)
                .OnComplete(() =>
                {
                    if (clip != null)
                        SoundManager.Instance.PlaySfx(clip, .5f);
                });
        else
            HideUI(infoUI, delay);
    }

    public void HideInfoUI()
    {
        DisplayInfoUI(false);
    }

    public void DisplayCombinationUI(bool value, float delay = 0)
    {
        if (value)
            MoveUI(combinationUI, delay);
        else
            HideUI(combinationUI, delay);
    }

    public void DisplayFillUI(bool value, float delay = 0)
    {
        if (value)
            MoveUI(fillUI, delay);
        else
            HideUI(fillUI, delay);
    }

    public void DisplayFrameUI(bool value, float delay = 0)
    {
        if (value)
            MoveUI(frameUI, delay);
        else
            HideUI(frameUI, delay);
    }

    public void DisplaySliderUI(bool value, float delay = 0)
    {
        if (value)
            MoveUI(slideUI, delay);
        else
            HideUI(slideUI, delay);
    }

    public void DisplayCwUI(bool value, float delay = 0)
    {
        if (value)
            MoveUI(clockwiseUI, delay);
        else
            HideUI(clockwiseUI, delay);
    }

    public void DisplayAcwUI(bool value, float delay = 0)
    {
        if (value)
            MoveUI(antiClockwiseUI, delay);
        else
            HideUI(antiClockwiseUI, delay);
    }

    public void DisplayActionBtnUI(bool value, float delay = 0)
    {
        if (value)
            MoveUI(actionBtnUI, delay);
        else
            HideUI(actionBtnUI, delay);
    }

    public void PlayNewUI()
    {
        newUI.rectTransform.gameObject.ToggleActive(true);

        _tween = newUI.rectTransform.DOScale(newUI.EndValue,
                newUI.Duration)
            .SetEase(newUI.EaseType)
            .SetLoops(-1,
                LoopType.Yoyo);
    }

    public void DisplayGuideUI(bool value, float delay = 0)
    {
        if (value)
            ScaleUI(guideUI, delay);
        else
            ShrinkUI(guideUI, delay);
    }

    public void DisplayDegreeUI(bool value, float delay = 0)
    {
        if (value)
            ScaleUI(degreeUI, delay);
        else
            ShrinkUI(degreeUI, delay);
    }


    private static void MoveUI(UIElement uiElement, float delay = 0f)
    {
        uiElement.rectTransform.DOAnchorPos(uiElement.EndValue,
                uiElement.Duration)
            .SetDelay(delay)
            .SetUpdate(true)
            .SetEase(uiElement.EaseType);
    }


    private static void HideUI(UIElement uiElement, float delay = 0f)
    {
        uiElement.rectTransform.DOAnchorPos(uiElement.StartValue, uiElement.Duration)
            .SetDelay(delay);
    }

    private static void ScaleUI(UIElement uiElement, float delay = 0f)
    {
        uiElement.rectTransform.DOScale(uiElement.EndValue,
                uiElement.Duration)
            .SetDelay(delay)
            .SetEase(uiElement.EaseType)
            .SetUpdate(true);
    }

    private static void ShrinkUI(UIElement uiElement, float delay = 0f)
    {
        uiElement.rectTransform.DOScale(uiElement.StartValue, uiElement.Duration)
            .SetDelay(delay);
    }

    private void ResetPos(UIElement uiElement)
    {
        uiElement.rectTransform.anchoredPosition = _isResetPos ? uiElement.StartValue : uiElement.EndValue;
    }

    private void ResetScale(UIElement uiElement)
    {
        uiElement.rectTransform.localScale = _isResetScale ? uiElement.EndValue : uiElement.StartValue;
    }

    #region Cheat
    [ContextMenu(nameof(ResetAllPos))]
    private void ResetAllPos()
    {
        ResetPos(infoUI);
        ResetPos(combinationUI);
        ResetPos(fillUI);
        ResetPos(frameUI);
        ResetPos(slideUI);
        ResetPos(clockwiseUI);
        ResetPos(antiClockwiseUI);
        ResetPos(actionBtnUI);

        _isResetPos = !_isResetPos;
    }

    [ContextMenu(nameof(ResetAllScale))]
    private void ResetAllScale()
    {
        ResetScale(guideUI);
        ResetScale(degreeUI);

        _isResetScale = !_isResetScale;
    }

    private void OnDisable()
    {
        if (DOTween.instance)
            _tween?.Kill();
    }
    #endregion
}
