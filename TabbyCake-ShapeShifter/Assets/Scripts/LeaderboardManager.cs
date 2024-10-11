using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private string filePath;
    private HighScoreList highScoreList = new HighScoreList();
    public TextMeshProUGUI scoreText;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "highscores.json");
        LoadHighScores();
    }

    public void AddScore(int score)
    {
        highScoreList.highScores.Add(new HighScore(score));
        SortHighScores();
        SaveHighScores();
    }

    private void SortHighScores()
    {
        highScoreList.highScores.Sort((x, y) => y.score.CompareTo(x.score));
    }

    private void SaveHighScores()
    {
        string json = JsonUtility.ToJson(highScoreList, true);
        File.WriteAllText(filePath, json);
        // Debug.Log($"High scores saved to {filePath}");
    }

    private void LoadHighScores()
    {
        if (!File.Exists(filePath))
            return; // Early exit if no file exists

        string json = File.ReadAllText(filePath);
        highScoreList = JsonUtility.FromJson<HighScoreList>(json);
        // Debug.Log($"High scores loaded from {filePath}");
    }

    public void DisplayTopScores()
    {
        StringBuilder displayText = new StringBuilder("Top Scores\n");
        int count = Mathf.Min(highScoreList.highScores.Count, 5);

        for (int i = 0; i < count; i++)
        {
            displayText.Append(highScoreList.highScores[i].score).Append("\n");
        }

        scoreText.text = displayText.ToString();
    }

    [System.Serializable]
    public class HighScore
    {
        public int score;

        public HighScore(int score)
        {
            this.score = score;
        }
    }

    [System.Serializable]
    public class HighScoreList
    {
        public List<HighScore> highScores = new List<HighScore>();
    }
}
