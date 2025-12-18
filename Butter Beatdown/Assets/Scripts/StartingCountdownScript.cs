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
    public AudioSource countdownAudioSource;   // plays countdown SFX
    public AudioClip countdownTickClip;        // plays on 3, 2, 1
    public AudioClip countdownGoClip;          // plays on Go
    public AudioSource mainMusicAudioSource;   // main music source

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
        if (countdownAudioSource != null && countdownTickClip != null)
            countdownAudioSource.PlayOneShot(countdownTickClip);
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "2";
        if (countdownAudioSource != null && countdownTickClip != null)
            countdownAudioSource.PlayOneShot(countdownTickClip);
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "1";
        if (countdownAudioSource != null && countdownTickClip != null)
            countdownAudioSource.PlayOneShot(countdownTickClip);
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "Go!";
        if (countdownAudioSource != null && countdownGoClip != null)
            countdownAudioSource.PlayOneShot(countdownGoClip);
        yield return new WaitForSecondsRealtime(1f);

        startingUI.SetActive(false);

        // Start churns
        if (churnA != null) churnA.isStarted = true;
        if (churnB != null) churnB.isStarted = true;

        // Start main timer
        if (timer != null) timer.StartFunction();

        // Start main music
        if (mainMusicAudioSource != null && !mainMusicAudioSource.isPlaying)
            mainMusicAudioSource.Play();
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
