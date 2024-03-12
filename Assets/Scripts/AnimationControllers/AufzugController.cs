using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AufzugController : MonoBehaviour
{
    public Animator flow;
    
    public Animator knopfe;

    public bool arrived = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Arrived()
    {
        arrived = true;
        flow.SetBool("Arrived", true);
        knopfe.SetBool("Arrived", true);
    }
}
