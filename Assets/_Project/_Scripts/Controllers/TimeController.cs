using System;
using Racer.SaveManager;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private TimeSpan _timeSpan;

    private bool _isDemo;
    private bool _isGameover;

    private float _currentTime;
    private float _bestTime;

    [SerializeField] private TextMeshPro timeT3d;
    [SerializeField] private TextMeshProUGUI countdownT;


    private void Start()
    {
        _bestTime = SaveManager.GetFloat("BestTime");

        _isDemo = GameManager.Instance.IsOnDemo;

        GameManager.Instance.OnGameState += Instance_OnGameState;
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

        UpdateGamePlayTime();
    }

    /// <summary>
    /// Time player spends on a session.
    /// </summary>
    private void UpdateGamePlayTime()
    {
        _timeSpan = TimeSpan.FromSeconds(_currentTime);

        if (_timeSpan.Hours < 60)
            countdownT.text = $"{_timeSpan.Minutes}m:{_timeSpan.Seconds}s";

        if (_timeSpan.Hours >= 60)
            countdownT.text = $"{_timeSpan.Hours}h:{_timeSpan.Minutes}m:{_timeSpan.Seconds}s";
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
            UIController.Instance.BestTimeTween();
    }
}
