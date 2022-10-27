using System.Collections;
using Racer.SoundManager;
using Racer.Utilities;
using UnityEngine;

internal class CameraController : MonoBehaviour
{
    private bool _isShifted;

    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Vector3 initialPos;

    [SerializeField, Space(5)] private float smoothTime = .25f;

    [Header("EFFECTS"), Space(5)]
    [SerializeField] private ParticleSystem winFx;
    [SerializeField] private AudioClip winSfx;

    private void Awake()
    {
        GetComponentInChildren<Camera>().backgroundColor = RandomColor();
    }

    // TODO: Move to Awake()
    private void Start()
    {
        GameManager.Instance.OnGameState += Instance_OnGameState;
    }

    private void Instance_OnGameState(State state)
    {
        if (state == State.Gameover)
            StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        yield return Utility.GetWaitForSeconds(1.5f);

        float elapsed = 0;

        while (elapsed < smoothTime)
        {
            elapsed += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, targetPos, elapsed / smoothTime);

            yield return 0;
        }

        transform.position = targetPos;

        SoundManager.Instance.PlaySfx(winSfx);

        winFx.Play();
    }

    #region Cheat
    [ContextMenu(nameof(ShiftCam))]
    private void ShiftCam()
    {
        transform.position = !_isShifted ? initialPos : targetPos;

        _isShifted = !_isShifted;
    }

    private static Color RandomColor(float s = .15f, float v = 1f)
    {
        // Todo
        var hue = (Random.Range(0f, 10f) / 10.0f) + .01f;

        return Color.HSVToRGB(hue, s, v);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameState -= Instance_OnGameState;
    }
    #endregion
}
