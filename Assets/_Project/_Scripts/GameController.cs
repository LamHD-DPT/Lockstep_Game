using System;
using Racer.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

internal class GameController : SingletonPattern.Singleton<GameController>
{
    public int[] CombinationValues { get; private set; }

    private float _randomValue;

    public event Action<Step> OnCurrentStep;

    [SerializeField] private Step currentStep;


    protected override void Awake()
    {
        base.Awake();

        InitValues();
    }

    private void Start()
    {
        UIController.Instance.SetCombinationTexts(CombinationValues[0], CombinationValues[1], CombinationValues[2]);

        UIController.Instance.SetSliderValue(_randomValue);
    }

    private void InitValues()
    {
        CombinationValues = new[]
        {
            Random.Range(1,
                Metrics.MaxDegree),
            Random.Range(1,
                Metrics.MaxDegree),
            Random.Range(1,
                Metrics.MaxDegree)
        };

        _randomValue = Random.Range(10, 360);
    }


    public void SetStep(Step newStep)
    {
        OnCurrentStep?.Invoke(newStep);

        currentStep = newStep;
    }
}