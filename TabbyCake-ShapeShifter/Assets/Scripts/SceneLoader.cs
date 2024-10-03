using System;
using System.IO;
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
    public GameObject Tutorial;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        PlayerPrefs.DeleteAll();

        Debug.Log(PlayerPrefs.GetInt("FirstTime"));
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
        CosmeticSaveDataModel cosmeticSaveData = new CosmeticSaveDataModel();
        cosmeticSaveData.squareSprite = 1;
        cosmeticSaveData.triangleSprite = 2;
        cosmeticSaveData.circleSprite = 3;
        cosmeticSaveData.diamondSprite = 4;

        string json = JsonUtility.ToJson(cosmeticSaveData);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        Debug.Log("Writing file to:" + Application.persistentDataPath);
    }

    [ContextMenu("Load")]
    void LoadCosmeticData()
    {
        CosmeticSaveDataModel cosmeticSaveData = JsonUtility.FromJson<CosmeticSaveDataModel>(File.ReadAllText(Application.persistentDataPath + "/save.json"));
        Debug.Log(cosmeticSaveData.squareSprite);
        Debug.Log(cosmeticSaveData.triangleSprite);
        Debug.Log(cosmeticSaveData.circleSprite);
        Debug.Log(cosmeticSaveData.diamondSprite);
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
