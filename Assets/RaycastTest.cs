using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Physics.Raycast(new Vector3(0,0,0), new Vector3(0,0,1)))
        {
            Debug.Log("Hit!");
        } else
        {
            Debug.Log("No Hit");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
