using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreDisplay : MonoBehaviour
{
    [Header("High Score Display")]
    public TMP_Text[] highScoreTexts; // Assign 5 TMP Texts in inspector
    public Image fillUI;               // UI fill for countdown
    public float countdownDuration = 10f;
    public string nextSceneName;       // Scene to load after countdown

    private ScoreManager scoreStorage;

    private void Start()
    {
        // Find the persistent HighScoreStorage in the scene
        scoreStorage = FindObjectOfType<ScoreManager>();
        if (scoreStorage != null)
            DisplayHighScores();

        StartCoroutine(CountdownToNextScene());
    }

    private void DisplayHighScores()
    {
        if (scoreStorage == null || highScoreTexts == null) return;

        List<int> topScores = scoreStorage.GetTopScores();

        for (int i = 0; i < highScoreTexts.Length && i < topScores.Count; i++)
        {
            if (highScoreTexts[i] != null)
            {
                highScoreTexts[i].text = topScores[i].ToString(); // Only score, no position
            }
        }
    }

    private IEnumerator CountdownToNextScene()
    {
        float timer = countdownDuration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (fillUI != null)
                fillUI.fillAmount = timer / countdownDuration;

            yield return null;
        }

        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}
