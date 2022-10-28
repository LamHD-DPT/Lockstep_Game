using Racer.SoundManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class SliderAction : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler
{
    private RaycastResult _raycastResult;

    private Slider _slider;
    private GameManager _gameManager;

    private bool _isDrag;

    private bool _isOnStep;
    private bool _wasOnStep = true;

    private int _antiClockwiseCount;
    private int _clockwiseCount;
    private int _progress;

    private int _prevValue;
    private int _curValue;

    [Header("SFXs")]
    [SerializeField] private AudioClip progressSfx;

    public int Progress
    {
        get => _progress;
        set
        {
            _progress = value;

            if (_gameManager.IsOnDemo)
                UIControllerGame.Instance.SetGuideText(_progress);
        }
    }

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        _prevValue = (int)_slider.value;
        _curValue = _prevValue;
    }

    private void ResetStep()
    {
        _isOnStep = false;
        _wasOnStep = !_isOnStep;

        _clockwiseCount = 0;
        _antiClockwiseCount = 0;
    }

    private void ModifyStep(int index)
    {
        if (_isOnStep && DialController.Instance.Degree != _gameManager.CombinationValues[index])
        {
            _wasOnStep = false;
        }
    }

    private void SetIsOnStep()
    {
        _isOnStep = true;

        _wasOnStep = _isOnStep;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _raycastResult = eventData.pointerPressRaycast;

        if (_isDrag || _raycastResult.depth == 6) return;

        Progress = 0;

        _gameManager.SetStep(Step.Zero);

        ResetStep();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _isDrag = true;

        if (Progress == 2 && _wasOnStep)
        {
            if (_curValue > _prevValue &&
                 DialController.Instance.Degree == _gameManager.CombinationValues[0])
            {
                SetIsOnStep();
            }

            ModifyStep(0);
        }

        if (Progress == 3 && _wasOnStep)
        {
            if (_antiClockwiseCount == 1 &&
                DialController.Instance.Degree == _gameManager.CombinationValues[0])
            {
                SetIsOnStep();
            }

            ModifyStep(0);
        }

        if (Progress == 4 && _wasOnStep)
        {
            if (_antiClockwiseCount == 1 &&
                 DialController.Instance.Degree == _gameManager.CombinationValues[1])
            {
                SetIsOnStep();
            }

            ModifyStep(1);
        }

        if (Progress == 5 && _wasOnStep)
        {
            if (_clockwiseCount == 1 &&
                DialController.Instance.Degree == _gameManager.CombinationValues[2])
            {
                SetIsOnStep();
            }

            ModifyStep(2);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDrag = true;

        _prevValue = (int)_slider.value;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDrag = false;

        _curValue = (int)_slider.value;

        if (_prevValue == (int)_slider.value)
            return;

        switch (Progress)
        {
            case 0:
                StepOne();
                break;
            case 1:
                StepTwo();
                break;
            case 2:
                StepThree();
                break;
            case 3:
                StepFour();
                break;
            case 4:
                StepFive();
                break;
            case 5:
                StepSix();
                break;
        }
    }

    private void StepOne()
    {
        ResetStep();

        if (_curValue == Metrics.MinDegree || _curValue == Metrics.MaxRotation)
        {
            Progress++;

            PlaySfx();
        }
    }

    private void StepTwo()
    {
        if (_prevValue == Metrics.MinDegree && _curValue == Metrics.MaxRotation)
        {
            _clockwiseCount++;

            PlaySfx();

            // Debug.Log($"Clockwise Count: {_clockwiseCount}");
        }
        else
        {
            Progress = 0;
            _gameManager.SetStep(Step.Zero);
        }
        if (_clockwiseCount == 3)
        {
            _clockwiseCount = 0;

            Progress++;
        }
    }

    private void StepThree()
    {
        // Whenever while dragging, value exceeds, report.

        if (_isOnStep && _wasOnStep)
        {
            _gameManager.SetStep(Step.One);

            Progress++;

            ResetStep();
        }
        else
        {
            Progress = 0;
            _gameManager.SetStep(Step.Zero);
        }
    }

    private void StepFour()
    {
        if (_isOnStep && _wasOnStep)
        {
            Progress++;

            PlaySfx();

            _antiClockwiseCount = 0;

            ResetStep();
        }
        else if (_curValue != Metrics.MinRotation || _antiClockwiseCount > 1)
        {
            Progress = 0;
            _gameManager.SetStep(Step.Zero);
        }
        else
        {
            _antiClockwiseCount++;
        }
    }


    private void StepFive()
    {
        if (_isOnStep && _wasOnStep)
        {
            Progress++;

            _gameManager.SetStep(Step.Two);

            _antiClockwiseCount = 0;

            ResetStep();
        }
        else if (_curValue != Metrics.MinRotation || _antiClockwiseCount > 1)
        {
            Progress = 0;
            _gameManager.SetStep(Step.Zero);
        }
        else
        {
            _antiClockwiseCount++;
        }
    }

    private void StepSix()
    {
        if (_isOnStep && _wasOnStep)
        {
            _gameManager.SetStep(Step.Three);

            Progress++;

            _clockwiseCount = 0;

            ResetStep();
        }
        else if (_curValue != Metrics.MaxRotation || _clockwiseCount > 1)
        {
            Progress = 0;
            _gameManager.SetStep(Step.Zero);
        }
        else
        {
            _clockwiseCount++;
        }
    }

    private void PlaySfx()
    {
        SoundManager.Instance.PlaySfx(progressSfx, .25f);
    }
}
