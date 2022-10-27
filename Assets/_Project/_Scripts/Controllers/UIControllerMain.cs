using Racer.LoadManager;
using Racer.SaveManager;
using TMPro;
using UnityEngine;

public class UIControllerMain : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelT;


    private void Start()
    {
        levelT.text = $"{SaveManager.GetInt("Level")}";
    }


    public void LoadScene(bool isDemo)
    {
        SaveManager.SaveBool("Demo", isDemo);

        LoadManager.Instance.LoadSceneAsync(1);
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
