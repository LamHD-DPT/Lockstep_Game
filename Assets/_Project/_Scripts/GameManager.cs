using System;
using System.Collections;
using Racer.SaveManager;
using Racer.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

internal class GameManager : SingletonPattern.Singleton<GameManager>
{
    private int _randomNumber;

    public int[] CombinationValues { get; private set; }

    private float _randomValue;

    public event Action<Step> OnCurrentStep;
    public event Action<State> OnGameState;

    // For visualization in the inspector
    [SerializeField] private Step currentStep;
    [SerializeField] private State gameState;

    [Space(5), SerializeField] private float initTime = 30;

    [SerializeField] private bool isDemo;

    public State CurrentState => gameState;

    public bool IsOnDemo { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        InitValues();

        IsOnDemo = SaveManager.GetBool("Demo") || isDemo;
    }

    private IEnumerator Start()
    {
        yield return Utility.GetWaitForSeconds(initTime);

        SetGameState(State.Playing);

        UIController.Instance.SetCombinationTexts(CombinationValues[0], CombinationValues[1], CombinationValues[2]);
        UIController.Instance.SetSliderValue(_randomValue);
    }

    private void InitValues()
    {
        CombinationValues = new int[3];

        _randomNumber = Random.Range(1, Metrics.MaxDegree);

        CombinationValues[0] = _randomNumber;
        CombinationValues[1] = Random.Range(0, _randomNumber);
        CombinationValues[2] = Random.Range(_randomNumber, Metrics.MaxDegree);

        _randomValue = Random.Range(5, 355);
    }

    public void SetStep(Step newStep)
    {
        currentStep = newStep;

        OnCurrentStep?.Invoke(currentStep);

        if (newStep == Step.Three)
        {
            SetGameState(IsOnDemo ? State.GameoverDemo : State.Gameover);
            Haptics.Vibrate(500);
        }
    }

    public void SetGameState(State newState)
    {
        gameState = newState;

        OnGameState?.Invoke(gameState);
    }

    #region Cheat
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
            SetGameState(State.Gameover);

        else if (Input.GetKeyDown(KeyCode.L))
            SetGameState(State.Exit);

        else if (Input.GetKeyDown(KeyCode.J))
            SetGameState(State.GameoverDemo);
#endif
    }
    #endregion
}