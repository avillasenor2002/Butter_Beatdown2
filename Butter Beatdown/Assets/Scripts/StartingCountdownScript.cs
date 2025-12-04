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
    public ButterChurn churnA;   // First churn
    public ButterChurn churnB;   // Second churn

    [Header("Other")]
    public TimerScript timer;

    [Header("Audio")]
    public AudioSource countdownEndAudio; // Audio to play when countdown ends

    [Header("Options")]
    public bool autoStart = false; // OPTIONAL: auto-trigger countdown at Start()

    private void Start()
    {
        if (autoStart)
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

        // Start both churns
        if (churnA != null) churnA.isStarted = true;
        if (churnB != null) churnB.isStarted = true;

        // Start the main timer
        if (timer != null) timer.StartFunction();

        // Play audio after countdown ends
        if (countdownEndAudio != null)
        {
            countdownEndAudio.Play();
        }
    }

    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
