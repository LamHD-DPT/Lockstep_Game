using Racer.SaveManager;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private TextMeshPro levelText;

    public int CurrentLevel { get; private set; }

    private void Start()
    {
        CurrentLevel = SaveManager.GetInt("Level");

        GameManager.Instance.OnGameState += Instance_OnGameState;
    }

    public void NextLevel()
    {
        CurrentLevel++;
        levelText.text = $"{CurrentLevel}";

        SaveManager.SaveInt("Level", CurrentLevel);
    }

    private void Instance_OnGameState(State state)
    {
        if (state == State.Gameover)
            NextLevel();
    }
}
