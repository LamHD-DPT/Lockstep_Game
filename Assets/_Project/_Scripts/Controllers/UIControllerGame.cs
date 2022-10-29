using Racer.LoadManager;
using Racer.SoundManager;
using Racer.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

[DefaultExecutionOrder(-9)]
internal class UIControllerGame : SingletonPattern.Singleton<UIControllerGame>
{
    private GameUITween _gameUITween;
    private SoundManager _soundManager;
    private TextPopulator _textPopulator;
    private GameManager _gameManager;

    private bool _isGameoverDemo;
    private bool _isGameover;

    [field: SerializeField]
    public FillBar FillBar { get; private set; }

    [Space(5), Header("TEXTS")]
    [SerializeField] private TextMeshProUGUI[] combinationTexts;

    [SerializeField] private TextMeshProUGUI degreeT;
    [SerializeField] private TextMeshProUGUI guideT;
    [SerializeField] private TextMeshProUGUI infoT;

    [Space(5), Header("IMAGES")]
    [SerializeField] private Image pauseI;
    [SerializeField] private Image menuI;

    [Space(5), Header("SPRITES")]
    [SerializeField] private Sprite[] pauseSpr;
    [SerializeField] private Sprite[] menuSpr;

    [Space(5)]
    [SerializeField] private Slider slider;

    [Header("SFXs"), Space(5)]
    [SerializeField] private AudioClip errorSfx;
    [SerializeField] private AudioClip correctSfx;
    [SerializeField] private AudioClip winSfx;
    [SerializeField] private AudioClip hintSfx;


    protected override void Awake()
    {
        base.Awake();

        _gameUITween = GetComponent<GameUITween>();
        _soundManager = SoundManager.Instance;
        _textPopulator = TextPopulator.Instance;
        _gameManager = GameManager.Instance;

    }

    private void Start()
    {
        infoT.text = InitText;

        _gameManager.OnGameState += Instance_OnGameState;
        _gameManager.OnCurrentStep += Instance_OnCurrentStep;
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
        guideT.text = index <= _textPopulator.GuideTexts.Count - 1
            ? _textPopulator.GuideTexts[index]
            : _textPopulator.CheerUpTexts[Random.Range(0,
                _textPopulator.CheerUpTexts.Count)];
    }

    public void SwitchLeft()
    {
        if ((!_isGameover || !_isGameoverDemo) &&
            DialController.Instance.Degree == Metrics.MinDegree)
            SetSliderValue(0);
    }

    public void SwitchRight()
    {
        if ((!_isGameover || !_isGameoverDemo) &&
            DialController.Instance.Degree == Metrics.MinDegree)
            SetSliderValue(360);
    }

    public void PauseGame(bool isPause)
    {
        if (_isGameover || _isGameoverDemo)
            return;

        SwapPauseSprite(isPause);

        if (isPause)
            _soundManager.GetSnapShot(1).TransitionTo(.1f);
        else
            _soundManager.GetSnapShot(0).TransitionTo(.1f);

        slider.interactable = !isPause;
    }

    public void LoadScene()
    {
        _gameManager.SetGameState(State.Exit);
        LoadManager.Instance.LoadSceneAsync(menuI.sprite == menuSpr[0] ? 0 : 1);
    }

    public void BestTimeTween()
    {
        _gameUITween.PlayNewUI();
    }

    public void ShowInfo(string text)
    {
        infoT.text = text;
        _gameUITween.DisplayInfoUI(true, 0, hintSfx);
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

                _soundManager.PlaySfx(correctSfx);
                break;

            case Step.Zero:
                FillBar.DecreaseFill(2);
                FillBar.DecreaseFill(1);
                FillBar.DecreaseFill(0);

                _soundManager.PlaySfx(errorSfx, .5f);

                Haptics.Vibrate(100);

                ShowInfo(_textPopulator.TipTexts[Random.Range(2, 8)]);

                CancelInvoke(nameof(RefreshText));
                Invoke(nameof(RefreshText), 2.5f);
                break;
        }
    }

    private void RefreshText() => ShowInfo(InitText);

    private string InitText => _gameManager.IsOnDemo ?
        _textPopulator.TipTexts[0] :
        _textPopulator.TipTexts[1];

    // Weird
    private void Instance_OnGameState(State state)
    {
        switch (state)
        {
            case State.Playing:
                _gameUITween.DisplayCombinationUI(true);
                _gameUITween.DisplayInfoUI(true, 1.5f, hintSfx);
                _gameUITween.DisplayActionBtnUI(true, 1f);
                _gameUITween.DisplayFillUI(true, .1f);
                _gameUITween.DisplayFrameUI(true, .1f);
                _gameUITween.DisplaySliderUI(true, .2f);
                _gameUITween.DisplayCwUI(true, .3f);
                _gameUITween.DisplayAcwUI(true, .3f);
                _gameUITween.DisplayGuideUI(true, .4f);
                _gameUITween.DisplayDegreeUI(true, .4f);
                break;

            case State.Gameover:
                _isGameover = true;

                _gameUITween.DisplayGuideUI(false, 1f);

                ShowInfo(_textPopulator.CheerUpTexts[Random.Range
                    (0, _textPopulator.CheerUpTexts.Count)]);

                _gameUITween.DisplayInfoUI(false, 1.5f);

                SwapMenuSprite(_isGameover);

                OnExitUI(false);
                break;

            case State.GameoverDemo:
                _isGameoverDemo = true;
                slider.interactable = false;
                _gameUITween.DisplayInfoUI(false);

                _soundManager.PlaySfx(winSfx, .8f);

                OnExitUI(false);
                break;

            case State.Exit:
                _gameUITween.DisplayActionBtnUI(false);
                _gameUITween.DisplayGuideUI(false);
                _gameUITween.DisplayInfoUI(false);

                OnExitUI(false);
                break;
        }
    }

    private void OnExitUI(bool v)
    {
        _gameUITween.DisplayCombinationUI(v);
        _gameUITween.DisplayFillUI(v);
        _gameUITween.DisplayFrameUI(v);
        _gameUITween.DisplaySliderUI(v);
        _gameUITween.DisplayCwUI(v);
        _gameUITween.DisplayAcwUI(v);
        _gameUITween.DisplayDegreeUI(v);
    }

    private void OnDestroy()
    {
        _gameManager.OnGameState -= Instance_OnGameState;
        _gameManager.OnCurrentStep -= Instance_OnCurrentStep;
    }
}
