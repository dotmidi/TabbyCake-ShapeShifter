using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to manage scene loading

public class SceneLoader : MonoBehaviour
{
    // Function to load a new scene
    public void NextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
