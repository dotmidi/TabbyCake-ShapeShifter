using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using JetBrains.Annotations;

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
    public Image squareSelector;
    public Button square0;
    public Button square1;
    public Button square2;
    public Button square3;
    public Button square4;
    public Image triangleSelector;
    public Button triangle0;
    public Button triangle1;
    public Button triangle2;
    public Button triangle3;
    public Button triangle4;
    public Image circleSelector;
    public Button circle0;
    public Button circle1;
    public Button circle2;
    public Button circle3;
    public Button circle4;
    public Image diamondSelector;
    public Button diamond0;
    public Button diamond1;
    public Button diamond2;
    public Button diamond3;
    public Button diamond4;

    [Header("Soundtracks")]
    public AudioSource mainMenuMusic;
    public AudioSource gameMusic;

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
        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            CosmeticsInit();
        }

        // PlayerPrefs.DeleteAll();

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
            // Debug.Log(PlayerPrefs.GetInt("Sound"));
            if (PlayerPrefs.GetInt("Sound") == 1)
            {
                gameMusic.Play();
            }
            else if (PlayerPrefs.GetInt("Sound") == 0)
            {
                gameMusic.Stop();
            }
            if (PlayerPrefs.GetInt("Tutorial") == 0)
            {
                PlayerPrefs.SetInt("Tutorial", 1);
                PlayerPrefs.Save();
                ShowTutorial();
            }
            if (PlayerPrefs.GetInt("Cheats") == 1)
            {
                PlayerScript.health = 1000;
                PlayerPrefs.DeleteKey("Cheats");
            }
        }
        else if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {
                mainMenuMusic.Play();
            }
            else if (PlayerPrefs.GetInt("Sound") == 0)
            {
                mainMenuMusic.Stop();
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
            // TutorialPopup.SetActive(false);
            StarPopup.SetActive(true);
        }
        else if (popupName == "star")
        {
            // StarPopup.SetActive(false);
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
                mainMenuMusic.Play();
            }
            else
            {
                PlayerPrefs.SetInt("Sound", 0);
                SoundToggle.SetActive(false);
                mainMenuMusic.Stop();
            }
        }

        PlayerPrefs.Save();
        // Debug.Log(
        //     "Current control scheme (0 for swipe, 1 for tap): " + PlayerPrefs.GetInt("Controls")
        // );
        // Debug.Log("Sound setting (0 for off, 1 for on): " + PlayerPrefs.GetInt("Sound"));
    }

    public void StopMusicAferDeath()
    {
        gameMusic.Stop();
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
            PlayerPrefs.SetInt("Sound", 1);
            SoundToggle.SetActive(true);
            mainMenuMusic.Play();
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 0);
            SoundToggle.SetActive(false);
            mainMenuMusic.Stop();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameMusic.volume = 0.2f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameMusic.volume = 1f;
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
        //initialise save data object
        cosmeticSaveData = new CosmeticSaveDataModel();
        //move selectors to correct position
        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            SetSelectorPositions();
        }
        //assign square buttons
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
        //assign triangle buttons
        triangle0.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.TRIANGLE, 0);
            }
        );
        triangle1.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.TRIANGLE, 1);
            }
        );
        triangle2.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.TRIANGLE, 2);
            }
        );
        triangle3.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.TRIANGLE, 3);
            }
        );
        triangle4.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.TRIANGLE, 4);
            }
        );
        //assign circle buttons
        circle0.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.CIRCLE, 0);
            }
        );
        circle1.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.CIRCLE, 1);
            }
        );
        circle2.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.CIRCLE, 2);
            }
        );
        circle3.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.CIRCLE, 3);
            }
        );
        circle4.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.CIRCLE, 4);
            }
        );
        //assign diamond buttons
        diamond0.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.DIAMOND, 0);
            }
        );
        diamond1.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.DIAMOND, 1);
            }
        );
        diamond2.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.DIAMOND, 2);
            }
        );
        diamond3.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.DIAMOND, 3);
            }
        );
        diamond4.onClick.AddListener(
            delegate
            {
                ModifyCosmeticData(Shape.DIAMOND, 4);
            }
        );
    }

    public void ModifyCosmeticData(Shape shape, int SpriteID)
    {
        // Debug.Log("TEST");
        if (shape == Shape.SQUARE)
        {
            cosmeticSaveData.squareSprite = SpriteID;
            if (SpriteID == 0)
            {
                squareSelector.rectTransform.position = square0.transform.position;
            }
            else if (SpriteID == 1)
            {
                squareSelector.rectTransform.position = square1.transform.position;
            }
            else if (SpriteID == 2)
            {
                squareSelector.rectTransform.position = square2.transform.position;
            }
            else if (SpriteID == 3)
            {
                squareSelector.rectTransform.position = square3.transform.position;
            }
            else if (SpriteID == 4)
            {
                squareSelector.rectTransform.position = square4.transform.position;
            }
            //cosmeticSaveData.squareSelectPos = squareSelector.rectTransform.position;
        }
        if (shape == Shape.TRIANGLE)
        {
            cosmeticSaveData.triangleSprite = SpriteID;
            if (SpriteID == 0)
            {
                triangleSelector.rectTransform.position = triangle0.transform.position;
            }
            else if (SpriteID == 1)
            {
                triangleSelector.rectTransform.position = triangle1.transform.position;
            }
            else if (SpriteID == 2)
            {
                triangleSelector.rectTransform.position = triangle2.transform.position;
            }
            else if (SpriteID == 3)
            {
                triangleSelector.rectTransform.position = triangle3.transform.position;
            }
            else if (SpriteID == 4)
            {
                triangleSelector.rectTransform.position = triangle4.transform.position;
            }
            //cosmeticSaveData.triangleSelectPos = triangleSelector.rectTransform.position;
        }
        if (shape == Shape.CIRCLE)
        {
            cosmeticSaveData.circleSprite = SpriteID;
            if (SpriteID == 0)
            {
                circleSelector.rectTransform.position = circle0.transform.position;
            }
            else if (SpriteID == 1)
            {
                circleSelector.rectTransform.position = circle1.transform.position;
            }
            else if (SpriteID == 2)
            {
                circleSelector.rectTransform.position = circle2.transform.position;
            }
            else if (SpriteID == 3)
            {
                circleSelector.rectTransform.position = circle3.transform.position;
            }
            else if (SpriteID == 4)
            {
                circleSelector.rectTransform.position = circle4.transform.position;
            }
            //cosmeticSaveData.circleSelectPos = circleSelector.rectTransform.position;
        }
        if (shape == Shape.DIAMOND)
        {
            cosmeticSaveData.diamondSprite = SpriteID;
            if (SpriteID == 0)
            {
                diamondSelector.rectTransform.position = diamond0.transform.position;
            }
            else if (SpriteID == 1)
            {
                diamondSelector.rectTransform.position = diamond1.transform.position;
            }
            else if (SpriteID == 2)
            {
                diamondSelector.rectTransform.position = diamond2.transform.position;
            }
            else if (SpriteID == 3)
            {
                diamondSelector.rectTransform.position = diamond3.transform.position;
            }
            else if (SpriteID == 4)
            {
                diamondSelector.rectTransform.position = diamond4.transform.position;
            }
            //cosmeticSaveData.diamondSelectPos = diamondSelector.rectTransform.position;
        }
        SaveCosmeticData();
    }

    [ContextMenu("Save")]
    public void SaveCosmeticData()
    {
        string json = JsonUtility.ToJson(cosmeticSaveData);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        // Debug.Log("Writing file to:" + Application.persistentDataPath);
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
            // Debug.Log("Square sprite ID:" + cosmeticSaveData.squareSprite);
            // Debug.Log("Triangle sprite ID:" + cosmeticSaveData.triangleSprite);
            // Debug.Log("Circle sprite ID:" + cosmeticSaveData.circleSprite);
            // Debug.Log("Diamond sprite ID:" + cosmeticSaveData.diamondSprite);
            //Debug.Log("Square select Position:" + cosmeticSaveData.squareSelectPos);
            //Debug.Log("Triangle select Position:" + cosmeticSaveData.triangleSelectPos);
            //Debug.Log("Circle select Position:" + cosmeticSaveData.circleSelectPos);
            //Debug.Log("Diamond select Position:" + cosmeticSaveData.diamondSelectPos);
        }
        else
        {
            // Debug.LogWarning("Save file not found, creating a new one.");
            // Create a new default save data and save it
            cosmeticSaveData = new CosmeticSaveDataModel();
            SaveCosmeticData();
        }
    }

    public void SetSelectorPositions()
    {
        LoadCosmeticData();
        //square selector
        if (cosmeticSaveData.squareSprite == 0)
        {
            squareSelector.rectTransform.position = square0.transform.position;
        }
        else if (cosmeticSaveData.squareSprite == 1)
        {
            squareSelector.rectTransform.position = square1.transform.position;
        }
        else if (cosmeticSaveData.squareSprite == 2)
        {
            squareSelector.rectTransform.position = square2.transform.position;
        }
        else if (cosmeticSaveData.squareSprite == 3)
        {
            squareSelector.rectTransform.position = square3.transform.position;
        }
        else if (cosmeticSaveData.squareSprite == 4)
        {
            squareSelector.rectTransform.position = square4.transform.position;
        }
        //triangle selector
        if (cosmeticSaveData.triangleSprite == 0)
        {
            triangleSelector.rectTransform.position = triangle0.transform.position;
        }
        else if (cosmeticSaveData.triangleSprite == 1)
        {
            triangleSelector.rectTransform.position = triangle1.transform.position;
        }
        else if (cosmeticSaveData.triangleSprite == 2)
        {
            triangleSelector.rectTransform.position = triangle2.transform.position;
        }
        else if (cosmeticSaveData.triangleSprite == 3)
        {
            triangleSelector.rectTransform.position = triangle3.transform.position;
        }
        else if (cosmeticSaveData.triangleSprite == 4)
        {
            triangleSelector.rectTransform.position = triangle4.transform.position;
        }
        //circle selector
        if (cosmeticSaveData.circleSprite == 0)
        {
            circleSelector.rectTransform.position = circle0.transform.position;
        }
        else if (cosmeticSaveData.circleSprite == 1)
        {
            circleSelector.rectTransform.position = circle1.transform.position;
        }
        else if (cosmeticSaveData.circleSprite == 2)
        {
            circleSelector.rectTransform.position = circle2.transform.position;
        }
        else if (cosmeticSaveData.circleSprite == 3)
        {
            circleSelector.rectTransform.position = circle3.transform.position;
        }
        else if (cosmeticSaveData.circleSprite == 4)
        {
            circleSelector.rectTransform.position = circle4.transform.position;
        }
        //diamond selector
        if (cosmeticSaveData.diamondSprite == 0)
        {
            diamondSelector.rectTransform.position = diamond0.transform.position;
        }
        else if (cosmeticSaveData.diamondSprite == 1)
        {
            diamondSelector.rectTransform.position = diamond1.transform.position;
        }
        else if (cosmeticSaveData.diamondSprite == 2)
        {
            diamondSelector.rectTransform.position = diamond2.transform.position;
        }
        else if (cosmeticSaveData.diamondSprite == 3)
        {
            diamondSelector.rectTransform.position = diamond3.transform.position;
        }
        else if (cosmeticSaveData.diamondSprite == 4)
        {
            diamondSelector.rectTransform.position = diamond4.transform.position;
        }

        //squareSelector.rectTransform.position = cosmeticSaveData.squareSelectPos;
        //triangleSelector.rectTransform.position = cosmeticSaveData.triangleSelectPos;
        //circleSelector.rectTransform.position = cosmeticSaveData.circleSelectPos;
        //diamondSelector.rectTransform.position = cosmeticSaveData.diamondSelectPos;
    }

    /*
    void ProcessSaveData(CosmeticSaveDataModel cosmeticSaveData)
    {
        int square = cosmeticSaveData.squareSprite;
        int triangle = cosmeticSaveData.triangleSprite;
        int circle = cosmeticSaveData.circleSprite;
        int diamond = cosmeticSaveData.diamondSprite;
    }
    */
}

[Serializable]
public class CosmeticSaveDataModel
{
    public int squareSprite;
    public int triangleSprite;
    public int circleSprite;
    public int diamondSprite;
    //public Vector3 squareSelectPos;
    //public Vector3 triangleSelectPos;
    //public Vector3 circleSelectPos;
    //public Vector3 diamondSelectPos;
}
