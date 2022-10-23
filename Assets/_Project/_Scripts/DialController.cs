using System;
using Racer.Utilities;
using UnityEngine;

internal class DialController : SingletonPattern.SingletonPersistent<DialController>
{
    private UIController _uiController;

    private float _degree;

    private Vector3 _angle;

    [SerializeField] private float rotateSpeed = 5;

    [SerializeField] private Transform icon;


    private void Start()
    {
        _uiController = UIController.Instance;
    }

    public void Rotate(float value)
    {
        // Not really precise, but fair enough.
        _degree = (value / Metrics.MaxRotation * Metrics.MaxDegree);

        _degree = Mathf.RoundToInt(_degree);

        if (value.Equals(Metrics.MaxRotation))
            _degree = 0;

        // Debug.Log($"Degree and value while syncing: {_degree}, {value}");

        _uiController.SetDegreeText(_degree);

        _angle.z = -value;
    }

    private void Update()
    {
        // Smoothly move to target rotation.
        icon.localRotation = Quaternion.Slerp(icon.localRotation, Quaternion.Euler(_angle), Time.deltaTime * rotateSpeed);

        if (Input.GetKeyUp(KeyCode.L))
            _uiController.SetSliderValue(0);

        else if (Input.GetKeyUp(KeyCode.K))
            _uiController.SetSliderValue(360);
    }
}
