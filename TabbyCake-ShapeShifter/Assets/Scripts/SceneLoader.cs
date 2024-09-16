using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to manage scene loading

public class SceneLoader : MonoBehaviour
{
    public GameObject LeaderboardCanvas;
    public GameObject CosmeticsCanvas;
    public GameObject SettingsCanvas;
    public GameObject CreditsCanvas;
    void Start()
    {
        Application.targetFrameRate = 60;  // Set the target frame rate to 60

        QualitySettings.vSyncCount = 0;  // Disable VSync
    }
    // Function to load a new scene
    public void NextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PopUp(string menuName)
    {
        // make a switch case with options leaderboard, cosmetics, settings credits. These will uncover the canvas with the respective menu
        switch (menuName)
        {
            case "leaderboard":
                LeaderboardCanvas.SetActive(!LeaderboardCanvas.activeSelf);
                break;
            case "cosmetics":
                CosmeticsCanvas.SetActive(!CosmeticsCanvas.activeSelf);
                break;
            case "settings":
                SettingsCanvas.SetActive(!SettingsCanvas.activeSelf);
                break;
            case "credits":
                CreditsCanvas.SetActive(!CreditsCanvas.activeSelf);
                break;
        }
    }
}
