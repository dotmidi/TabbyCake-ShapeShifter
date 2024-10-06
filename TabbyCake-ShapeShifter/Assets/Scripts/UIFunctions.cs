using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFunctions : MonoBehaviour
{
    [Header("Main UI Canvases")]
    public GameObject LeaderboardCanvas;
    public GameObject CosmeticsCanvas;
    public GameObject SettingsCanvas;
    public GameObject CreditsCanvas;
    public GameObject PauseCanvas;

    [Header("Pause and Tutorial UI")]
    public GameObject PauseButton;
    public GameObject TutorialCanvas;
    public GameObject TutorialPopup;
    public GameObject StarPopup;
    public GameObject GlitchPopup;

    [Header("Control Toggles")]
    public GameObject SwipeToggle;
    public GameObject TapToggle;
    public GameObject SoundToggle;

    [Header("Game Logic Scripts")]
    public MonoBehaviour LevelScript1;
    public MonoBehaviour LevelScript2;
    public PlayerScript PlayerScript;

    [Header("Miscellaneous UI")]
    public GameObject HighScoreText;
    public GameObject Tutorial;

    [Header("Cosmetics")]
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

        if (SceneManager.GetActiveScene().name == "MainGame")
        {
            if (PlayerPrefs.GetInt("Tutorial") == 0)
            {
                PlayerPrefs.SetInt("Tutorial", 1);
                PlayerPrefs.Save();
                ShowTutorial();
            }
        }
    }

    public void ShowTutorial()
    {
        HighScoreText.SetActive(false);
        LevelScript1.enabled = false;
        LevelScript2.enabled = false;
        TutorialCanvas.SetActive(true);
        TutorialPopup.SetActive(true);
    }

    public void NextInTutorial(string popupName)
    {
        if (popupName == "tutorial")
        {
            TutorialPopup.SetActive(false);
            StarPopup.SetActive(true);
        }
        else if (popupName == "star")
        {
            StarPopup.SetActive(false);
            GlitchPopup.SetActive(true);
        }
        else if (popupName == "glitch")
        {
            GlitchPopup.SetActive(false);
            CloseTutorial();
        }
    }

    public void CloseTutorial()
    {
        TutorialCanvas.SetActive(false);
        LevelScript1.enabled = true;
        LevelScript2.enabled = true;
        PlayerScript.HighScore = 0;
        HighScoreText.SetActive(true);
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
        // Debug.Log(
        //     "Current control scheme (0 for swipe, 1 for tap): " + PlayerPrefs.GetInt("Controls")
        // );
        // Debug.Log("Sound setting (0 for off, 1 for on): " + PlayerPrefs.GetInt("Sound"));
    }

    public void NextScene(string sceneName)
    {
        PlayerPrefs.Save();
        // Debug.Log(PlayerPrefs.GetInt("Controls"));
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

        if (menuName == "settings")
        {
            UpdateSettingsUI();
            PlayerPrefs.Save();
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
