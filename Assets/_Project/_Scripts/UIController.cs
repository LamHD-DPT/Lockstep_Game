using Racer.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

internal class UIController : SingletonPattern.Singleton<UIController>
{
    private bool _isGameover;
    private GameUITween _gameUITween;

    [field: SerializeField]
    public FillBar FillBar { get; private set; }

    [SerializeField, TextArea(), Space(5)]
    private string[] guideValues;

    [Space(5), Header("TEXTS")]
    [SerializeField] private TextMeshProUGUI[] combinationTexts;

    [Space(5), Header("IMAGES")]
    [SerializeField] private Image pauseI;
    [SerializeField] private Image menuI;

    [Space(5), Header("SPRITES")]
    [SerializeField] private Sprite[] pauseSpr;
    [SerializeField] private Sprite[] menuSpr;

    [SerializeField] private TextMeshProUGUI degreeT;
    [SerializeField] private TextMeshProUGUI guideT;

    [Space(5)]
    [SerializeField] private Slider slider;


    // TODO: Populate Info text values
    // TODO: Populate guide text values, esp the last value on gameover

    private void Start()
    {
        _gameUITween = GetComponent<GameUITween>();

        GameManager.Instance.OnGameState += Instance_OnGameState;
        GameManager.Instance.OnCurrentStep += Instance_OnCurrentStep;
    }

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
        if (!_isGameover && DialController.Instance.Degree == Metrics.MinDegree)
            SetSliderValue(0);
    }

    public void SwitchRight()
    {
        if (!_isGameover && DialController.Instance.Degree == Metrics.MinDegree)
            SetSliderValue(360);
    }

    public void PauseGame(bool isPause)
    {
        if (_isGameover)
            return;

        SwapPauseSprite(isPause);

        slider.interactable = !isPause;
    }

    public void LoadScene()
    {
        Debug.Log(_isGameover ? "Next Level!" : "Main Scene");
    }

    private void SwapPauseSprite(bool isPause)
    {
        pauseI.sprite = isPause ? pauseSpr[1] : pauseSpr[0];
    }

    private void SwapMenuSprite(bool isWin)
    {
        menuI.sprite = isWin ? menuSpr[1] : menuSpr[0];

    }

    private void Instance_OnCurrentStep(Step step)
    {
        switch (step)
        {
            case Step.One:
            case Step.Two:
            case Step.Three:
                FillBar.IncreaseFill((int)step - 1);
                break;

            case Step.Zero:
                FillBar.DecreaseFill(2);
                FillBar.DecreaseFill(1);
                FillBar.DecreaseFill(0);
                break;
        }
    }

    private void Instance_OnGameState(State state)
    {
        switch (state)
        {
            case State.Playing: // Weird
                _gameUITween.DisplayInfoUI(true, 1f);
                _gameUITween.DisplayActionBtnUI(true, 1f);
                _gameUITween.DisplayCombinationUI(true);
                _gameUITween.DisplayFillUI(true, .1f);
                _gameUITween.DisplayFrameUI(true, .1f);
                _gameUITween.DisplaySliderUI(true, .2f);
                _gameUITween.DisplayCwUI(true, .3f);
                _gameUITween.DisplayAcwUI(true, .3f);
                _gameUITween.DisplayGuideUI(true, .4f);
                _gameUITween.DisplayDegreeUI(true, .4f);
                break;
            case State.Exit:
                _isGameover = true;
                _gameUITween.DisplayInfoUI(!true);
                _gameUITween.DisplayCombinationUI(!true);
                _gameUITween.DisplayFillUI(!true);
                _gameUITween.DisplayFrameUI(!true);
                _gameUITween.DisplaySliderUI(!true);
                _gameUITween.DisplayCwUI(!true);
                _gameUITween.DisplayAcwUI(!true);
                _gameUITween.DisplayGuideUI(!true);
                _gameUITween.DisplayDegreeUI(!true);
                _gameUITween.DisplayActionBtnUI(!true);
                break;
            case State.Gameover:
                _isGameover = true;
                SwapMenuSprite(_isGameover);
                _gameUITween.DisplayInfoUI(!true);
                _gameUITween.DisplayCombinationUI(!true);
                _gameUITween.DisplayFillUI(!true);
                _gameUITween.DisplayFrameUI(!true);
                _gameUITween.DisplaySliderUI(!true);
                _gameUITween.DisplayCwUI(!true);
                _gameUITween.DisplayAcwUI(!true);
                _gameUITween.DisplayGuideUI(!true, 1f);
                _gameUITween.DisplayDegreeUI(!true);
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameState -= Instance_OnGameState;
        GameManager.Instance.OnCurrentStep -= Instance_OnCurrentStep;
    }
}
