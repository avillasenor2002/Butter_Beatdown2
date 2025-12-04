using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandSelectStart: MonoBehaviour
{
    public GameObject Toilet;
    public GameObject FlowerPot;
    public GameObject Toaster;
    public float objspwn;

    public GameObject PoolBG;
    public GameObject SkyBG;
    public GameObject HillsideBG;
    public float BGspwn;
    // Start is called before the first frame update
    void Start()
    {
            objspwn = UnityEngine.Random.Range(1, 5);
            if (objspwn == 1)
            {
                Toilet.SetActive(true);
            }

            if (objspwn == 2)
            {
                Toilet.SetActive(true);
            }

            if (objspwn == 3)
            {
                FlowerPot.SetActive(true);
            }

            if (objspwn == 4)
            {
                Toaster.SetActive(true);
            }

            if (objspwn == 5)
            {
                Toaster.SetActive(true);
            }

        BGspwn = UnityEngine.Random.Range(1, 6);
        if (BGspwn == 1)
        {
            PoolBG.SetActive(true);
        }

        if (BGspwn == 2)
        {
            PoolBG.SetActive(true);
        }

        if (BGspwn == 3)
        {
            SkyBG.SetActive(true);
        }

        if (BGspwn == 4)
        {
            SkyBG.SetActive(true);
        }

        if (BGspwn == 5)
        {
            HillsideBG.SetActive(true);
        }

        if (BGspwn == 6)
        {
            HillsideBG.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
