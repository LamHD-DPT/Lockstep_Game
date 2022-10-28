using System.Collections;
using Racer.SoundManager;
using Racer.Utilities;
using UnityEngine;

internal class CameraController : MonoBehaviour
{
    private bool _isShifted;
    private Color _color;

    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Vector3 initialPos;

    [SerializeField, Space(5)]
    private float smoothTime = .25f;

    [Header("EFFECTS"), Space(5)]
    [SerializeField] private ParticleSystem winFx;
    [SerializeField] private AudioClip winSfx;

    [Header("OTHERS"), Space(5)]
    [SerializeField] private Material floorMaterial;

    private void Awake()
    {
        _color = RandomColor();

        GameManager.Instance.OnGameState += Instance_OnGameState;

        GetComponentInChildren<Camera>().backgroundColor = floorMaterial.color = _color;
    }


    private void Instance_OnGameState(State state)
    {
        if (state == State.Gameover)
            StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        yield return Utility.GetWaitForSeconds(2f);

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
    #endregion

    private static Color RandomColor(float s = .3f, float v = 1f)
    {
        var hue = (Random.Range(0f, 10f) / 10.0f);

        return Color.HSVToRGB(hue, s, v);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameState -= Instance_OnGameState;
    }
}
