using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject LeaderboardCanvas;
    public GameObject CosmeticsCanvas;
    public GameObject SettingsCanvas;
    public GameObject CreditsCanvas;
    public GameObject PauseCanvas;
    public GameObject PauseButton;

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    public void NextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PopUp(string menuName)
    {
        GameObject canvas = GetCanvas(menuName);

        if (menuName == "pause")
        {
            PauseGame();
            PauseButton.SetActive(false);
        }

        if (menuName == "resume")
        {
            ResumeGame();
            PauseButton.SetActive(true);
        }

        if (canvas != null)
        {
            canvas.SetActive(!canvas.activeSelf);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private GameObject GetCanvas(string menuName)
    {
        switch (menuName)
        {
            case "pause":
                return PauseCanvas;
            case "resume":
                return PauseCanvas;
            case "leaderboard":
                return LeaderboardCanvas;
            case "cosmetics":
                return CosmeticsCanvas;
            case "settings":
                return SettingsCanvas;
            case "credits":
                return CreditsCanvas;
            default:
                return null;
        }
    }
}
