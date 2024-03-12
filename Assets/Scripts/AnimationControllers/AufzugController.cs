using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AufzugController : MonoBehaviour
{
    public bool arrived = false;

    public float speed = 1;

    void Start()
    {
        
    }

    public bool slowing_down;

    // Update is called once per frame
    void Update()
    {
        if (slowing_down)
        {
            speed -= Time.deltaTime;
            speed = Mathf.Clamp01(speed);
        }
        if (speed == 0) arrived = true;
    }

    public void Arrived()
    {
        slowing_down = true;
    }
}
