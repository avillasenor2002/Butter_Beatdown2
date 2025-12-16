using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore_"; // Keys: HighScore_1 to HighScore_5

    [Header("Default Top 5 Scores")]
    public int[] defaultTopScores = new int[5] { 1000, 800, 600, 400, 200 };

    private void Awake()
    {
        // Ensure only one instance exists and persist between scenes
        if (FindObjectsOfType<ScoreManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Initialize default scores if none exist
        InitializeDefaultScores();
    }

    private void InitializeDefaultScores()
    {
        bool anyScoreExists = false;

        for (int i = 1; i <= 5; i++)
        {
            if (PlayerPrefs.HasKey(HighScoreKey + i))
            {
                anyScoreExists = true;
                break;
            }
        }

        if (!anyScoreExists)
        {
            for (int i = 0; i < 5; i++)
            {
                PlayerPrefs.SetInt(HighScoreKey + (i + 1), defaultTopScores[i]);
            }
            PlayerPrefs.Save();
        }
    }

    // Record a new score at the end of the game
    public void RecordScore(int score)
    {
        List<int> scores = new List<int>();

        // Load existing scores
        for (int i = 1; i <= 5; i++)
        {
            scores.Add(PlayerPrefs.GetInt(HighScoreKey + i, 0));
        }

        // Add the new score
        scores.Add(score);

        // Sort descending
        scores.Sort((a, b) => b.CompareTo(a));

        // Keep top 5
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(HighScoreKey + (i + 1), scores[i]);
        }

        PlayerPrefs.Save();
    }

    // Retrieve top 5 scores in descending order
    public List<int> GetTopScores()
    {
        List<int> topScores = new List<int>();
        for (int i = 1; i <= 5; i++)
        {
            topScores.Add(PlayerPrefs.GetInt(HighScoreKey + i, 0));
        }
        return topScores;
    }

    // Optional: Clear all high scores
    public void ClearHighScores()
    {
        for (int i = 1; i <= 5; i++)
        {
            PlayerPrefs.DeleteKey(HighScoreKey + i);
        }
        PlayerPrefs.Save();
    }
}
