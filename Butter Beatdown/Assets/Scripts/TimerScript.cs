using System.Collections;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float totalTime = 60f;
    public TMP_Text timerText;
    public bool gameEnd = false;

    // Support for two churn controllers
    public ButterChurn churn1;
    public ButterChurn churn2;

    public EndstateManager endstateManager;

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

                // When timer hits 10 seconds, both churns enter sicko mode
                if (timeLeft == 10)
                {
                    if (churn1 != null) churn1.sickomode = true;
                    if (churn2 != null) churn2.sickomode = true;
                }
            }
        }

        Debug.Log("Countdown finished!");
        endstateManager.ButterWin();
    }

    private void UpdateTimerText(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:0}:{1:0}", minutes, seconds);
    }
}
