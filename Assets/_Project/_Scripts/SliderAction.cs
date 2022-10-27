using Racer.SoundManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderAction : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler
{
    private RaycastResult _raycastResult;

    private Slider _slider;
    private GameManager _gameManager;

    private bool _isDrag;

    private bool _isOnStep;
    private bool _wasOnStep;

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
                UIController.Instance.SetGuideText(_progress);
        }
    }

    // TODO: Vibration

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;

        _prevValue = (int)_slider.value;
        _curValue = _prevValue;
    }

    private void ResetStep()
    {
        _isOnStep = false;
        _wasOnStep = false;

        _clockwiseCount = 0;
        _antiClockwiseCount = 0;
    }

    private void ModifyStep()
    {
        switch (_wasOnStep)
        {
            case false:
                _wasOnStep = _isOnStep;
                break;
            case true:
                _isOnStep = false;
                _wasOnStep = !_isOnStep;
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _raycastResult = eventData.pointerPressRaycast;

        // Knob depth: 6
        if (!_isDrag && _raycastResult.depth != 6)
        {
            Progress = 0;
            _gameManager.SetStep(Step.Zero);
            ResetStep();
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        _isDrag = true;

        if (Progress == 2)
        {
            if (_curValue < _prevValue &&
                DialController.Instance.Degree == _gameManager.CombinationValues[0])
            {
                _isOnStep = true;

                ModifyStep();
            }
            else if (_antiClockwiseCount > 1)
            {
                _isOnStep = false;
            }
        }

        if (Progress == 3)
        {
            if (_antiClockwiseCount == 1 &&
                DialController.Instance.Degree == _gameManager.CombinationValues[0])
            {
                _isOnStep = true;

                ModifyStep();
            }
        }

        if (Progress == 4)
        {
            if (_antiClockwiseCount == 1 &&
                 DialController.Instance.Degree == _gameManager.CombinationValues[1])
            {
                _isOnStep = true;

                ModifyStep();
            }
        }

        if (Progress == 5)
        {
            if (_clockwiseCount == 1 &&
                DialController.Instance.Degree == _gameManager.CombinationValues[2])
            {
                _isOnStep = true;

                ModifyStep();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log($"Value before dragging: {_slider.value}");

        _isDrag = true;

        _prevValue = (int)_slider.value;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log($"Value after dragging: {_slider.value}");

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
        if (_prevValue == Metrics.MaxRotation && _curValue == Metrics.MinDegree)
        {
            _clockwiseCount++;

            PlaySfx();

          //  Debug.Log($"Clockwise Count: {_clockwiseCount}");
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

        if (_isOnStep && _wasOnStep &&
            DialController.Instance.Degree == _gameManager.CombinationValues[0])
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
        if (_isOnStep && _wasOnStep &&
            DialController.Instance.Degree == _gameManager.CombinationValues[0])
        {
            Progress++;

            PlaySfx();

            _antiClockwiseCount = 0;

            ResetStep();
        }
        else if (_curValue != Metrics.MaxRotation || _antiClockwiseCount > 1)
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
        if (_isOnStep && _wasOnStep &&
            DialController.Instance.Degree == _gameManager.CombinationValues[1])
        {
            Progress++;

            _gameManager.SetStep(Step.Two);

            _antiClockwiseCount = 0;

            ResetStep();
        }
        else if (_curValue != Metrics.MaxRotation || _antiClockwiseCount > 1)
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
        if (_isOnStep && _wasOnStep &&
            DialController.Instance.Degree == _gameManager.CombinationValues[2])
        {
            _gameManager.SetStep(Step.Three);

            Progress++;

            _clockwiseCount = 0;

            ResetStep();
        }
        else if (_curValue != Metrics.MinDegree || _clockwiseCount > 1)
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
