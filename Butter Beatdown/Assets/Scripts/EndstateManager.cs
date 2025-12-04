using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndstateManager : MonoBehaviour
{
    //public CatBalanceScript PlayerAlphaHP;
    //public MoveControler PlayerAlphaMovement;

    //public CatBalanceScript PlayerBravoHP;
    //public MoveControler PlayerBravoMovement;

    public TimerScript timer;
    public ButterChurn Butt;
    public GameObject EndUI;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    public void ButterWin()
    {
       //Debug.Log("Player Alpha Wins");
        /*PlayerAlphaMovement.isemptying = true;
        if (PlayerBravoHP != null )
        {
            PlayerBravoMovement.isemptying = true;
        }*/
        EndUI.SetActive(true);

        if (Butt.score <= 300)
        {
            scoreText.text = "D";
        }

        if (Butt.score > 300 && Butt.score <= 600)
        {
            scoreText.text = "C";
        }

        if (Butt.score > 600 && Butt.score <= 900)
        {
            scoreText.text = "B";
        }

        if (Butt.score > 900 && Butt.score <= 1000)
        {
            scoreText.text = "A";
        }

        if(Butt.score > 1000 && Butt.score <= 1150)
        {
            scoreText.text = "S";
        }

        if (Butt.score > 1150)
        {
            scoreText.text = "S+";
        }
    }

    public void CalculateWinner()
    {
        Debug.Log("Calculating Winner");
        bool istie = false;
        
    }
}
