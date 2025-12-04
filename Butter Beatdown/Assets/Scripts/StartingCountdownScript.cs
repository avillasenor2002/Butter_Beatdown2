using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingCountdownScript : MonoBehaviour
{
    public TMP_Text countdownText;
    public GameObject startingUI;
    public ButterChurn Butt;
    //public MoveControler PlayerBravoMovement;
    public TimerScript timer;

    public void begin()
    {
        StartCoroutine(StartCountdown());
    }

    public IEnumerator StartCountdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f);

        startingUI.SetActive(false);
        Butt.isStarted = true;
        //PlayerBravoMovement.isemptying = false;
        timer.StartFunction();
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
