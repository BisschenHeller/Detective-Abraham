using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject[] slices = null;

    private AbrahamAnimationController abraham;

    // Start is called before the first frame update
    void Start()
    {
        if (slices == null) Debug.LogError("No Scene Slices!");
    }

    // Update is called once per frame
    private float lastSpeed = 0;

    void FixedUpdate()
    {
        float currentSpeed = Mathf.Clamp(abraham.GetCurrentSpeed(), -1, 1);
        if (lastSpeed != currentSpeed)
        {
            for (int i = 0; i < slices.Length; i++)
            {
                float displacement = i * 0.01f;

            }
        }
        lastSpeed = currentSpeed;
    }
}
