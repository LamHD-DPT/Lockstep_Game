using Racer.SaveManager;
using Racer.Utilities;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
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

        countdownT.text =  Utility.TimeFormat(_currentTime);
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
