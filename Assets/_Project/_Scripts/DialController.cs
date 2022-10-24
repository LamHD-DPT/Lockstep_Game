using Racer.Utilities;
using UnityEngine;

internal class DialController : SingletonPattern.SingletonPersistent<DialController>
{
    private UIController _uiController;

    private Vector3 _angle;

    private float _degree;

    public int Degree
    {
        get => Mathf.RoundToInt(_degree);
        private set => _degree = value;
    }

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

        if (Degree == Metrics.MaxDegree)
            Degree = Metrics.MinDegree;

        // Debug.Log($"Degree and value while syncing: {_degree}, {value}");

        _uiController.SetDegreeText(Degree);

        _angle.z = -value;
    }

    private void Update()
    {
        // Smoothly move to target rotation.
        icon.localRotation = Quaternion.Slerp(icon.localRotation, Quaternion.Euler(_angle), Time.deltaTime * rotateSpeed);

        if (Input.GetKeyUp(KeyCode.L) && Degree == Metrics.MinDegree)
            _uiController.SetSliderValue(0);

        else if (Input.GetKeyUp(KeyCode.K) && Degree == Metrics.MinDegree)
            _uiController.SetSliderValue(360);
    }
}
