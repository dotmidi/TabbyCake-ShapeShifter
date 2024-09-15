using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to manage scene loading

public class SceneLoader : MonoBehaviour
{
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
}
