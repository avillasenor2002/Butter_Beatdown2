using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingCountdownScript : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text countdownText;
    public GameObject startingUI;

    [Header("Churns")]
    public ButterChurn churnA;
    public ButterChurn churnB;

    [Header("Other")]
    public TimerScript timer;

    [Header("Audio")]
    public AudioSource countdownEndAudio;

    private void Start()
    {
        // Always begin countdown on scene start
        begin();
    }

    public void begin()
    {
        StartCoroutine(StartCountdown());
    }

    public IEnumerator StartCountdown()
    {
        startingUI.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "2";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "1";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "Go!";
        yield return new WaitForSecondsRealtime(1f);

        startingUI.SetActive(false);

        // Start churns
        if (churnA != null) churnA.isStarted = true;
        if (churnB != null) churnB.isStarted = true;

        // Start main timer
        if (timer != null) timer.StartFunction();

        // Play audio when countdown finishes
        if (countdownEndAudio != null)
            countdownEndAudio.Play();
    }

    public void ReloadScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
