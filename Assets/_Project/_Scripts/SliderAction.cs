using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderAction : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Slider _slider;

    private bool _isOnStep;
    private bool _wasOnStep;

    private int _antiClockwiseCount;
    private int _clockwiseCount;
    private int _progress;

    private int _prevValue;
    private int _curValue;

    public int Progress
    {
        get => _progress;
        set
        {
            _progress = value;
            UIController.Instance.SetGuideText(_progress);
        }
    }

    private void Awake()
    {
        _slider = GetComponent<Slider>();
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

    public void OnDrag(PointerEventData eventData)
    {
        if (Progress == 2)
        {
            if (_curValue < _prevValue &&
                DialController.Instance.Degree == GameController.Instance.CombinationValues[0])
            {
                _isOnStep = true;

                ModifyStep();
            }
        }

        if (Progress == 3)
        {
            if (_antiClockwiseCount == 1 &&
                DialController.Instance.Degree == GameController.Instance.CombinationValues[0])
            {
                _isOnStep = true;

                ModifyStep();
            }
        }

        if (Progress == 4)
        {
            if (_antiClockwiseCount == 1 &&
                GameController.Instance.CombinationValues[1] == DialController.Instance.Degree)
            {
                _isOnStep = true;

                ModifyStep();
            }
        }

        if (Progress == 5)
        {
            if (_clockwiseCount == 1 &&
                GameController.Instance.CombinationValues[2] == DialController.Instance.Degree)
            {
                _isOnStep = true;

                ModifyStep();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log($"Value before dragging: {_slider.value}");

        _prevValue = (int)_slider.value;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log($"Value after dragging: {_slider.value}");

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
                StepFour();
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
        }
    }

    private void StepTwo()
    {
        if (_prevValue == Metrics.MaxRotation && _curValue == Metrics.MinDegree)
        {
            _clockwiseCount++;

            Debug.Log($"Clockwise Count: {_clockwiseCount}");
        }
        else
        {
            Progress = 0;
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
            Progress++;

            ResetStep();
        }
        else
        {
            Progress -= 2;
        }
    }

    private void StepFour()
    {
        if (_isOnStep && _wasOnStep)
        {
            Progress++;

            _antiClockwiseCount = 0;

            ResetStep();
        }
        else if (_curValue != Metrics.MaxRotation)
        {
            Progress = 0;
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
            Progress++;

            _clockwiseCount = 0;

            ResetStep();
        }
        else if (_curValue != Metrics.MinDegree)
        {
            Progress = 0;
        }
        else
        {
            _clockwiseCount++;
        }
    }
}
