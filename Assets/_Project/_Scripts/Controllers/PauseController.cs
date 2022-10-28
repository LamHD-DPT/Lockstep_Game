using UnityEngine;

internal class PauseController : MonoBehaviour
{
    private bool _hasPaused;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;

        _gameManager.OnGameState += GameManager_OnGameState;
    }

    private void GameManager_OnGameState(State state)
    {
        if (state.Equals(State.Exit))
            Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _gameManager.CurrentState == State.Playing)
            SetPause();
    }

    public void SetPause()
    {
        switch (_hasPaused)
        {
            case false:
                Pause();
                break;
            case true:
                Resume();
                break;
        }
    }

    private void Pause()
    {
        _hasPaused = true;

        UIControllerGame.Instance.PauseGame(_hasPaused);

        Time.timeScale = 0;
    }

    private void Resume()
    {
        _hasPaused = false;

        UIControllerGame.Instance.PauseGame(_hasPaused);

        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        _gameManager.OnGameState -= GameManager_OnGameState;
    }
}
