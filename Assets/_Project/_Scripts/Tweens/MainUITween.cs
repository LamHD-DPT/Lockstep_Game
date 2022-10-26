using DG.Tweening;
using UnityEngine;


internal class MainUITween : MonoBehaviour
{
    [SerializeField] private UIElement infoUI;
    [SerializeField] private UIElement infoIco;


    public void DisplayInfoUI(bool value)
    {
        if (value)
            infoUI.rectTransform.DOAnchorPos(infoUI.EndValue, infoUI.Duration).SetEase(infoUI.EaseType);
        else
            infoUI.rectTransform.DOAnchorPos(infoUI.StartValue, infoUI.Duration).SetEase(infoUI.EaseType);
    }

  
    private void RotateInfoIcon(bool value)
    {
        // Using the y-vector in place of z for rotation.
        if (value)
            infoIco.rectTransform.DOLocalRotate(new Vector3(infoIco.EndValue.x,
                        0,
                        infoIco.EndValue.y),
                    infoIco.Duration)
                .SetEase(infoIco.EaseType);
        else
            infoIco.rectTransform.DOLocalRotate(new Vector3(infoIco.StartValue.x,
                        0,
                        infoIco.StartValue.y),
                    infoIco.Duration)
                .SetEase(infoIco.EaseType);
    }
}
