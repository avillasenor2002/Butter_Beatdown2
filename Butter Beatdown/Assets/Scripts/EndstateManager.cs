using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndstateManager : MonoBehaviour
{
    public TimerScript timer;
    public ButterChurn Butt;
    public GameObject EndUI;
    public TMP_Text scoreText;

    [Header("High Score System")]
    public ScoreManager scoreManager; // Assign in inspector

    [Header("Next Scene")]
    public string nextSceneName; // Scene to load after hold

    // Called when butter churn wins
    public void ButterWin()
    {
        EndUI.SetActive(true);

        if (Butt.score <= 300)
            scoreText.text = "D";
        else if (Butt.score > 300 && Butt.score <= 600)
            scoreText.text = "C";
        else if (Butt.score > 600 && Butt.score <= 900)
            scoreText.text = "B";
        else if (Butt.score > 900 && Butt.score <= 1000)
            scoreText.text = "A";
        else if (Butt.score > 1000 && Butt.score <= 1150)
            scoreText.text = "S";
        else if (Butt.score > 1150)
            scoreText.text = "S+";

        // Record high score
        if (scoreManager != null)
            scoreManager.RecordScore(Butt.score);

        // ===== FIX: Assign winning churn =====
        if (Butt != null)
        {
            Butt.winningChurn = Butt;       // mark this churn as the winner
            Butt.canInteract = false;       // disable normal churning
            Butt.holdTimer = 0f;            // reset hold timer
            if (Butt.endHoldFillUI != null)
                Butt.endHoldFillUI.fillAmount = 0f;

            // Assign the scene to load
            Butt.nextSceneName = nextSceneName;
        }
    }

    public void CalculateWinner()
    {
        Debug.Log("Calculating Winner");
        bool istie = false;
    }
}
