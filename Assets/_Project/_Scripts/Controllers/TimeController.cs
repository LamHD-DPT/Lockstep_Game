using Racer.SaveManager;
using Racer.Utilities;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

internal class TimeController : MonoBehaviour
{
    private TextPopulator _textPopulator;
    private GameManager _gameManager;

    private bool _isDemo;
    private bool _isGameover;

    private float _notifyTime;
    private float _currentTime;
    private float _bestTime;

    [SerializeField] private TextMeshPro timeT3d;
    [SerializeField] private TextMeshProUGUI countdownT;

    private void Awake()
    {
        _textPopulator = TextPopulator.Instance;
        _gameManager = GameManager.Instance;

        _isDemo = _gameManager.IsOnDemo;
        _gameManager.OnGameState += Instance_OnGameState;

        _bestTime = SaveManager.GetFloat("BestTime");

        _notifyTime = Metrics.AlertTime;
    }

    private void Instance_OnGameState(State state)
    {
        if (state != State.Gameover) return;

        _isGameover = true;

        CompareTime();
    }

    private void Update()
    {
        if (_isDemo || _isGameover)
            return;

        _currentTime = Time.timeSinceLevelLoad;

        if (_currentTime >= _notifyTime)
        {
            UIControllerGame.Instance.ShowInfo(
                _textPopulator.TipTexts[Random.Range(_textPopulator.TipTexts.Count - 2,
                    _textPopulator.TipTexts.Count)]);

            _notifyTime += Metrics.AlertTime;
        }

        countdownT.text = Utility.TimeFormat(_currentTime);
    }

    /// <summary>
    /// Sets a new highscore if there exists none.
    /// Overwrites already existing highscore if gotten a value less than it.
    /// </summary>
    private void CompareTime()
    {
        timeT3d.text = $"Time: {countdownT.text}";

        SaveManager.SaveFloat("BestTime", _currentTime);

        if (_bestTime == 0 || _currentTime < _bestTime)
            UIControllerGame.Instance.BestTimeTween();
    }

    private void OnDestroy()
    {
        _gameManager.OnGameState -= Instance_OnGameState;
    }
}
