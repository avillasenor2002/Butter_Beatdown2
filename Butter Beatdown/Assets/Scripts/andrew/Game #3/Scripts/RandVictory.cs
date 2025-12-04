using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandVictory : MonoBehaviour
{
    public GameObject David;
    public GameObject Flower;
    public GameObject Toast;
    public GameObject Meerkat;
    public GameObject ToiletJr;
    public float objspwn;

    // Start is called before the first frame update
    void Awake()
    {
        objspwn = UnityEngine.Random.Range(1, 10);
        if (objspwn == 1)
        {
            Flower.SetActive(true);
        }

        if (objspwn == 2)
        {
            Flower.SetActive(true);
        }

        if (objspwn == 3)
        {
            Toast.SetActive(true);
        }

        if (objspwn == 4)
        {
            Toast.SetActive(true);
        }

        if (objspwn == 5)
        {
            Meerkat.SetActive(true);
        }

        if (objspwn == 6)
        {
            Meerkat.SetActive(true);
        }

        if (objspwn == 7)
        {
            Flower.SetActive(true);
        }

        if (objspwn == 8)
        {
            David.SetActive(true);
        }

        if (objspwn == 9)
        {
            ToiletJr.SetActive(true);
        }

        if (objspwn == 10)
        {
            ToiletJr.SetActive(true);
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
