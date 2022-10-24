using Racer.Utilities;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

internal class UIController : SingletonPattern.Singleton<UIController>
{
    [SerializeField, TextArea()] private string[] guideValues;

    [Space(5), Header("TEXTS")]
    [SerializeField] private TextMeshProUGUI[] combinationTexts;

    [SerializeField] private TextMeshProUGUI degreeT;
    [SerializeField] private TextMeshProUGUI guideT;


    [Space(5)]
    [SerializeField] private Slider slider;


    public void SetDegreeText(float value)
    {
        degreeT.text = $"{value}";
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }

    public void SetCombinationTexts(int v1, int v2, int v3)
    {
        combinationTexts[0].text = $"{v1}";
        combinationTexts[1].text = $"{v2}";
        combinationTexts[2].text = $"{v3}";
    }

    public void SetGuideText(int index)
    {
        guideT.text = guideValues[index];
    }

    public void SwitchLeft()
    {
        if (DialController.Instance.Degree == Metrics.MinDegree)
            SetSliderValue(0);
    }

    public void SwitchRight()
    {
        if (DialController.Instance.Degree == Metrics.MinDegree)
            SetSliderValue(360);
    }
}
