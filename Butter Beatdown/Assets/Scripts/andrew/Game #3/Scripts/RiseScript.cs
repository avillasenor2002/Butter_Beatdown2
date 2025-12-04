using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseScript : MonoBehaviour
{
    public float risey;
    public float currenty;
    public float maxy;
    public float x;
    public GameObject A;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currenty < maxy)
        {
            currenty = currenty + risey;
            this.transform.position = new Vector3(x, currenty, 0);
            A.SetActive(true);
        }
    }
}
