using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro; // Add this namespace for TextMeshPro
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private string filePath;
    private HighScoreList highScoreList;
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro text component

    // Start is called before the first frame update
    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "highscores.json");
        LoadHighScores();
        // DisplayTopScores(); // Display scores on start
    }

    public void AddScore(int score)
    {
        highScoreList.highScores.Add(new HighScore(score));
        SortHighScores(); // Sort the high scores after adding a new one
        SaveHighScores();
        // DisplayTopScores(); // Update the display after adding a new score
    }

    private void SortHighScores()
    {
        highScoreList.highScores.Sort((x, y) => y.score.CompareTo(x.score)); // Sort in descending order
    }

    private void SaveHighScores()
    {
        string json = JsonUtility.ToJson(highScoreList, true);
        File.WriteAllText(filePath, json);
        Debug.Log("High scores saved to " + filePath);
    }

    private void LoadHighScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            highScoreList = JsonUtility.FromJson<HighScoreList>(json);
            Debug.Log("High scores loaded from " + filePath);
        }
        else
        {
            highScoreList = new HighScoreList();
            Debug.Log("No high scores found. Starting a new list.");
        }
    }

    public void DisplayTopScores()
    {
        // Create a string to display the top 5 scores
        string displayText = "Top Scores\n";
        int count = Mathf.Min(highScoreList.highScores.Count, 5); // Get the minimum between the number of scores and 5

        for (int i = 0; i < count; i++)
        {
            displayText += highScoreList.highScores[i].score + "\n"; // Format the score
        }

        // Update the TextMeshPro text component
        scoreText.text = displayText;
    }

    // Nested class for a single high score
    [System.Serializable]
    public class HighScore
    {
        public int score;

        public HighScore(int score)
        {
            this.score = score;
        }
    }

    // Nested class for a list of high scores
    [System.Serializable]
    public class HighScoreList
    {
        public List<HighScore> highScores = new List<HighScore>();
    }
}
