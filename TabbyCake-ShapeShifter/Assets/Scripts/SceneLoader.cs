using UnityEngine;
using UnityEngine.SceneManagement; // Needed to manage scene loading

public class SceneLoader : MonoBehaviour
{
    public GameObject LeaderboardCanvas;
    public GameObject CosmeticsCanvas;
    public GameObject SettingsCanvas;
    public GameObject CreditsCanvas;

    private void Start()
    {
        Application.targetFrameRate = 60; // Set the target frame rate to 60
        QualitySettings.vSyncCount = 0; // Disable VSync
    }

    // Function to load a new scene
    public void NextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Function to toggle the visibility of the specified menu canvas
    public void PopUp(string menuName)
    {
        GameObject canvas = GetCanvas(menuName);

        if (canvas != null)
        {
            canvas.SetActive(!canvas.activeSelf); // Toggle the active state
        }
    }

    // Helper method to map menuName to the respective canvas
    private GameObject GetCanvas(string menuName)
    {
        switch (menuName)
        {
            case "leaderboard":
                return LeaderboardCanvas;
            case "cosmetics":
                return CosmeticsCanvas;
            case "settings":
                return SettingsCanvas;
            case "credits":
                return CreditsCanvas;
            default:
                return null; // Return null if no matching canvas found
        }
    }
}
