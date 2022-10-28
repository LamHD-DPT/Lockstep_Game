using Racer.LoadManager;
using Racer.SaveManager;
using Racer.Utilities;
using TMPro;
using UnityEngine;

public class UIControllerMain : MonoBehaviour
{
    private float _bestTime;
    private int _level;

    [SerializeField] private TextMeshProUGUI levelT;
    [SerializeField] private TextMeshProUGUI bestTimeT;

    private void Awake()
    {
        _bestTime = SaveManager.GetFloat("BestTime");
        _level = SaveManager.GetInt("Level");
    }

    private void Start()
    {
        levelT.text = $"{_level}";
        bestTimeT.text = Utility.TimeFormat(_bestTime);
    }


    public void LoadScene(bool isDemo)
    {
        SaveManager.SaveBool("Demo", isDemo);

        LoadManager.Instance.LoadSceneAsync(1);
    }

    public void ClearSave()
    {
        SaveManager.ClearAllPrefs();

        LoadManager.Instance.LoadSceneAsync(0);
    }

    public void ExitGame()
    {
#if !UNITY_EDITOR
        Application.Quit();
#else
        Debug.Log("Exited!");
#endif
    }
}
