using Racer.SoundManager;
using Racer.Utilities;
using UnityEngine;

[DefaultExecutionOrder(-8)]
internal class DialController : SingletonPattern.Singleton<DialController>
{
    private Animator _animator;
    private UIControllerGame _uiControllerGame;

    private Vector3 _angle;

    private bool _isSet;
    private bool _isGameover;
    private float _degree;

    public int Degree
    {
        get => Mathf.RoundToInt(_degree);
        private set => _degree = value;
    }

    [SerializeField] private float rotateSpeed = 5;
    [SerializeField] private Transform icon;
    [SerializeField] private Transform pivot;

    [Header("SFXs"), Space(5)]
    [SerializeField] private AudioClip lockSfx;
    [SerializeField] private AudioClip unlockSfx;

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.OnGameState += Instance_OnGameState;

        _animator = GetComponent<Animator>();

        _uiControllerGame = UIControllerGame.Instance;
    }

    public void Rotate(float value)
    {
        if (_isGameover)
            return;

        // Not really precise, but fair enough.
        _degree = Metrics.MaxDegree - (value / Metrics.MaxRotation * Metrics.MaxDegree);

        if (Degree == Metrics.MaxDegree)
            Degree = Metrics.MinDegree;

        _uiControllerGame.SetDegreeText(Degree);

        _angle.z = value;
    }

    private void Update()
    {
        if (_isGameover)
            return;

        // Smoothly move to target rotation.
        icon.localRotation = Quaternion.Slerp(icon.localRotation,
            Quaternion.Euler(_angle),
            Time.deltaTime * rotateSpeed);
    }

    private void Instance_OnGameState(State state)
    {
        switch (state)
        {
            case State.Exit:
                _isGameover = true;
                break;

            case State.GameoverDemo:
            case State.Gameover:
                _isGameover = true;
                _animator.SetTrigger(Metrics.Unlock);
                break;
        }
    }

    private void PlayLockSfx() => SoundManager.Instance.PlaySfx(lockSfx);

    public void PlayUnlockSfx() => SoundManager.Instance.PlaySfx(unlockSfx);

    private void OnDestroy()
    {
        GameManager.Instance.OnGameState -= Instance_OnGameState;
    }

    #region Cheat
    [ContextMenu(nameof(SetPosAndRot))]
    private void SetPosAndRot()
    {
        if (_isSet)
        {
            pivot.localPosition = new Vector3(-0.343f, 0.176f, -0.082f);
            pivot.localEulerAngles = Vector3.zero;
        }
        else
        {
            pivot.localPosition = new Vector3(-0.343f, 0.341f, -0.082f);
            pivot.localEulerAngles = new Vector3(0, 130, 0);
        }

        _isSet = !_isSet;
    }
    #endregion
}
