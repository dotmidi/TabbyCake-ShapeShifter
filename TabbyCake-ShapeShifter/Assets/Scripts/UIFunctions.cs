using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    //cosmetics
    public CosmeticSaveDataModel cosmeticSaveData;
    public Button square0;
    public Button square1;
    public Button square2;
    public Button square3;
    public Button square4;

    public enum Shape
    {
        SQUARE,
        TRIANGLE,
        CIRCLE,
        DIAMOND
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //cosmetics stuff


        PlayerPrefs.DeleteAll();

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

    void CosmeticsInit()
    {
        cosmeticSaveData = new CosmeticSaveDataModel();
        square0.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.SQUARE, 0);
            }
        );
        square1.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.SQUARE, 1);
            }
        );
        square2.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.SQUARE, 2);
            }
        );
        square3.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.SQUARE, 3);
            }
        );
        square4.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.SQUARE, 4);
            }
        );
    }

    public void ModifyCosmeticData(Shape shape, int SpriteID)
    {
        Debug.Log("TEST");
        if (shape == Shape.SQUARE)
        {
            cosmeticSaveData.squareSprite = SpriteID;
        }
        if (shape == Shape.TRIANGLE)
        {
            cosmeticSaveData.triangleSprite = SpriteID;
        }
        if (shape == Shape.CIRCLE)
        {
            cosmeticSaveData.circleSprite = SpriteID;
        }
        if (shape == Shape.DIAMOND)
        {
            cosmeticSaveData.diamondSprite = SpriteID;
        }
        SaveCosmeticData();
    }

    [ContextMenu("Save")]
    public void SaveCosmeticData()
    {
        string json = JsonUtility.ToJson(cosmeticSaveData);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        Debug.Log("Writing file to:" + Application.persistentDataPath);
    }

    [ContextMenu("Load")]
    public void LoadCosmeticData()
    {
        string filePath = Application.persistentDataPath + "/save.json";

        if (File.Exists(filePath))
        {
            cosmeticSaveData = JsonUtility.FromJson<CosmeticSaveDataModel>(
                File.ReadAllText(filePath)
            );
            Debug.Log("Square sprite ID:" + cosmeticSaveData.squareSprite);
            Debug.Log("Triangle sprite ID:" + cosmeticSaveData.triangleSprite);
            Debug.Log("Circle sprite ID:" + cosmeticSaveData.circleSprite);
            Debug.Log("Diamond sprite ID:" + cosmeticSaveData.diamondSprite);
        }
        else
        {
            Debug.LogWarning("Save file not found, creating a new one.");
            // Create a new default save data and save it
            cosmeticSaveData = new CosmeticSaveDataModel();
            SaveCosmeticData();
        }
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
