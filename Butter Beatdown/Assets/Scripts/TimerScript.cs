using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float totalTime = 60f;
    public TMP_Text timerText;
    //public TMP_Text timerTextBG;
    public bool gameEnd = false;
    public ButterChurn Butt;
    public EndstateManager endstateManager;


    public void StartFunction()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        if (gameEnd != true)
        {
            float timeLeft = totalTime;
            while (timeLeft > 0)
            {
                yield return new WaitForSeconds(1f);
                timeLeft -= 1f;
                UpdateTimerText(timeLeft);
                if (timeLeft == 10)
                {
                    Butt.sickomode = true;
                }
            }
        }

        // Countdown finished, do something here
        Debug.Log("Countdown finished!");
        endstateManager.ButterWin();
    }

    private void UpdateTimerText(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:0}:{1:0}", minutes, seconds);
        //timerTextBG.text = string.Format("{0:0}:{1:0}", minutes, seconds);
    }
}
