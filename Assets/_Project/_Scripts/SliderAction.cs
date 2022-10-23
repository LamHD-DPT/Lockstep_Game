using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Slider _slider;

    private int _clockwiseCount;
    private int _progress;

    private int _prevValue;
    private int _curValue;


    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log($"Value before dragging: {_slider.value}");

        _prevValue = (int)_slider.value;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //  Debug.Log($"Value after dragging: {_slider.value}");

        _curValue = (int)_slider.value;

        StepOne();

        StepTwo();
    }

    private void StepOne()
    {
        if (_progress == 0 && (_curValue == 0 || _curValue == Metrics.MaxRotation))
        {
            _progress++;

            UIController.Instance.SetGuideText(_progress);
        }
    }

    private void StepTwo()
    {
        if (_progress == 1)
        {
            if (_prevValue == Metrics.MaxRotation && _curValue == 0)
            {

                _clockwiseCount++;

                Debug.Log($"Clockwise Count: {_clockwiseCount}");
            }
            else
            {
                _progress--;

                _clockwiseCount = 0;

                UIController.Instance.SetGuideText(_progress);
            }

            if (_clockwiseCount == 3)
            {
                _progress++;

                UIController.Instance.SetGuideText(_progress);
            }
        }
    }
}
