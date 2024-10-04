using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour
{
    public GameObject LeaderboardCanvas;
    public GameObject CosmeticsCanvas;
    public GameObject SettingsCanvas;
    public GameObject CreditsCanvas;
    public GameObject PauseCanvas;
    public GameObject PauseButton;
    public GameObject Tutorial;
    public GameObject SwipeToggle;
    public GameObject TapToggle;
    public GameObject SoundToggle;

    CosmeticSaveDataModel cosmeticSaveData;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        cosmeticSaveData = new CosmeticSaveDataModel();

        PlayerPrefs.DeleteAll(); // Uncomment if you need to reset PlayerPrefs

        if (!PlayerPrefs.HasKey("Controls"))
        {
            PlayerPrefs.SetInt("Controls", 0);
        }

        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
    }

    public void SettingsToggles(string toggleName)
    {
        if (toggleName == "swipe")
        {
            PlayerPrefs.SetInt("Controls", 0);
            SwipeToggle.SetActive(true);
            TapToggle.SetActive(false);
        }
        else if (toggleName == "tap")
        {
            PlayerPrefs.SetInt("Controls", 1);
            SwipeToggle.SetActive(false);
            TapToggle.SetActive(true);
        }
        else if (toggleName == "sound")
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                PlayerPrefs.SetInt("Sound", 1);
                SoundToggle.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt("Sound", 0);
                SoundToggle.SetActive(false);
            }
        }

        PlayerPrefs.Save();
        Debug.Log(
            "Current control scheme (0 for swipe, 1 for tap): " + PlayerPrefs.GetInt("Controls")
        );
        Debug.Log("Sound setting (0 for off, 1 for on): " + PlayerPrefs.GetInt("Sound"));
    }

    public void NextScene(string sceneName)
    {
        if (!PlayerPrefs.HasKey("FirstTime"))
        {
            PopUp("tutorial");
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.Save();
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }

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

        if (menuName == "settings")
        {
            UpdateSettingsUI();
        }

        if (canvas != null)
        {
            canvas.SetActive(!canvas.activeSelf);
        }
    }

    private void UpdateSettingsUI()
    {
        if (PlayerPrefs.GetInt("Controls") == 0)
        {
            SwipeToggle.SetActive(true);
            TapToggle.SetActive(false);
        }
        else
        {
            SwipeToggle.SetActive(false);
            TapToggle.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundToggle.SetActive(true);
        }
        else
        {
            SoundToggle.SetActive(false);
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

    public void ToggleControls()
    {
        if (PlayerPrefs.GetInt("Controls") == 0)
        {
            PlayerPrefs.SetInt("Controls", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Controls", 0);
        }
    }

    private GameObject GetCanvas(string menuName)
    {
        switch (menuName)
        {
            case "tutorial":
                return Tutorial;
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

    [ContextMenu("Save")]
    void SaveCosmeticData()
    {
        string json = JsonUtility.ToJson(cosmeticSaveData);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        Debug.Log("Writing file to:" + Application.persistentDataPath);
    }

    [ContextMenu("Load")]
    void LoadCosmeticData()
    {
        CosmeticSaveDataModel cosmeticSaveData = JsonUtility.FromJson<CosmeticSaveDataModel>(
            File.ReadAllText(Application.persistentDataPath + "/save.json")
        );
        Debug.Log(cosmeticSaveData.squareSprite);
        Debug.Log(cosmeticSaveData.triangleSprite);
        Debug.Log(cosmeticSaveData.circleSprite);
        Debug.Log(cosmeticSaveData.diamondSprite);
    }

    void ProcessSaveData(CosmeticSaveDataModel cosmeticSaveData)
    {
        int square = cosmeticSaveData.squareSprite;
        int triangle = cosmeticSaveData.triangleSprite;
        int circle = cosmeticSaveData.circleSprite;
        int diamond = cosmeticSaveData.diamondSprite;


    }
}

[Serializable]
public class CosmeticSaveDataModel
{
    public int squareSprite;
    public int triangleSprite;
    public int circleSprite;
    public int diamondSprite;
}
