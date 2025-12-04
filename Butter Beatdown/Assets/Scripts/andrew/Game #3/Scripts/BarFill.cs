using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarFill : MonoBehaviour
{
    public Image FillUI;
    public Image CircleUI;
    public Image IndicatorUI;
    public GameObject Plunger;
    public GameObject winstate;
    public GameObject winstateBG;
    public GameObject A;
    public float currentwater;
    public float maxwater;
    public float waterloss;
    public float wateradd;

    public float currentfill;
    public float maxfill;
    public float filladd;

    public float randfillHigh;
    public float randfillLow;

    public float plungerx;
    public float plungerDown;
    public float plungerUp;

    public bool win;
    public float randomspwn;

    // Start is called before the first frame update
    void Start()
    {
            randfillHigh = 65;
            randfillLow = 35;
    }

    // Update is called once per frame
    void Update()
    {
        //Script used to update Water Tank UI to match the "curentwater" variable.
        FillUI.fillAmount = (currentwater / maxwater);
        CircleUI.fillAmount = (currentfill / maxfill);

        //When the space bar is pressed the current water decreases.
        if (Input.GetKeyDown(KeyCode.Space) && win == false)
        {
            currentwater = currentwater + wateradd;
            Plunger.transform.position = new Vector3(plungerx, plungerDown, 0);
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Plunger.transform.position = new Vector3(plungerx, plungerUp, 0);
        }

        if (currentwater < 0)
        {
            currentwater = 0;
        }

        if (currentwater > 100)
        {
            currentwater = 100;
        }

        if ((currentwater > randfillLow) && (currentwater < randfillHigh))
        {
            currentfill = currentfill + filladd;
            Debug.Log("Hello World!");
        }

        if (currentfill > 99.9)
        {
            win = true;
        }

        if (win == false)
        {
            currentwater = currentwater - waterloss;
        }


        if (win == true)
        {
            winstate.SetActive(true);
            winstateBG.SetActive(true);
            Plunger.transform.position = new Vector3(plungerx, plungerUp, 0);
            plungerUp = plungerUp + waterloss;
        }
    }
}
