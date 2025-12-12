using System.Collections;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [Header("Timer")]
    public float totalTime = 60f;
    public TMP_Text timerText;
    public bool gameEnd = false;

    [Header("Churns (Players)")]
    public ButterChurn churn1;   // Player 1
    public ButterChurn churn2;   // Player 2

    [Header("End Screen")]
    public EndstateManager endstateManager;

    // Updated: Winner UI (text only)
    public TMP_Text winnerScoreText;     // Shows the winning score (recolored)
    public TMP_Text winnerPlayerText;    // Shows "Player X Wins!"

    public void StartFunction()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        if (!gameEnd)
        {
            float timeLeft = totalTime;

            while (timeLeft > 0)
            {
                yield return new WaitForSeconds(1f);

                timeLeft -= 1f;
                UpdateTimerText(timeLeft);

                // Activate sicko mode at 10 seconds
                if (timeLeft == 10)
                {
                    if (churn1 != null) churn1.sickomode = true;
                    if (churn2 != null) churn2.sickomode = true;
                }
            }
        }

        Debug.Log("Countdown finished!");

        // Determine winner
        ButterChurn winner = DetermineWinner();

        // Update endscreen UI
        UpdateEndscreenUI(winner);

        // Trigger the end sequence
        endstateManager.ButterWin();
    }

    private void UpdateTimerText(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timerText.text = string.Format("{0:0}:{1:0}", minutes, seconds);
    }

    private ButterChurn DetermineWinner()
    {
        if (churn1 == null || churn2 == null)
        {
            Debug.LogError("One or both churn references missing.");
            return churn1 != null ? churn1 : churn2;
        }

        if (churn1.score > churn2.score)
            return churn1;

        if (churn2.score > churn1.score)
            return churn2;

        // Tie → default Player 1
        return churn1;
    }

    private void UpdateEndscreenUI(ButterChurn winner)
    {
        if (winner == null)
        {
            Debug.LogError("Winner is null; cannot update endscreen.");
            return;
        }

        // Winner score text
        if (winnerScoreText != null)
        {
            winnerScoreText.text = winner.score.ToString();

            // RECOLOR winner score text using the winner’s score text color
            if (winner.scoreText != null)
                winnerScoreText.color = winner.scoreText.color;
        }

        // Winner label text
        if (winnerPlayerText != null)
        {
            if (winner.playerID == ButterChurn.PlayerID.Player1)
                winnerPlayerText.text = "1";
            else
                winnerPlayerText.text = "2";
        }
    }
}
