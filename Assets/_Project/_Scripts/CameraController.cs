using System.Collections;
using Racer.Utilities;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool _isShifted;

    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Vector3 initialPos;

    [SerializeField, Space(5)] private float smoothTime = .25f;

    [SerializeField, Space(5)] private ParticleSystem winFx;

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

        winFx.Play();
    }

    [ContextMenu(nameof(ShiftCam))]
    private void ShiftCam()
    {
        transform.position = _isShifted ? targetPos : initialPos;

        _isShifted = !_isShifted;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameState -= Instance_OnGameState;
    }
}
