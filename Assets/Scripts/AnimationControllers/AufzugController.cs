using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AufzugController : MonoBehaviour
{
    public bool arrived = false;

    public float speed = 1;

    public float direction = 1;

    public bool slowing_down;

    public bool speeding_up;

    public GameObject flurParent;

    // Update is called once per frame
    void Update()
    {
        if (slowing_down)
        {
            speed -= Time.deltaTime;
        } else if (speeding_up) {
            speed += Time.deltaTime;
        }
        speed = Mathf.Clamp01(speed);
        if (speed == 0) arrived = true;
        flurParent.transform.localPosition = new Vector3(-1.62f, Mathf.Lerp(-0.04f, 0.86f, 1 - Mathf.Sin(Mathf.PI * (1 - speed) / 2)), 0);
    }

    public void GoDown()
    {
        speed = 0; arrived = false; slowing_down = false; speeding_up = true; direction = -1;
    }

    public void GoUp()
    {
        speed = 1; arrived = false; slowing_down = false; speeding_up = false; direction = 1; Invoke("Arrived", 5);
    }

    

    public void Arrived()
    {
        slowing_down = true;
    }
}
