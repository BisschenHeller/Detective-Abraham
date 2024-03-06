using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackdropController : MonoBehaviour
{
    public GameObject[] slices = null;

    private float[] start_position;

    [SerializeField]
    [Tooltip("Determines how fast the backdrop slices move")]
    private float slide_speed = 0.1f;

    [SerializeField]
    private AbrahamAnimationController abraham = null;

    // Start is called before the first frame update
    void Start()
    {
        if (slices == null) Debug.LogError("No Scene Slices!");
        if (abraham == null) Debug.LogError("Abraham Not Assigned!");

        start_position = new float[slices.Length];
        for (int i = 0; i < slices.Length; i++) 
        {
            start_position[i] = slices[i].transform.localPosition.x;
        }
    }

    // Update is called once per frame
    private float lastSpeed = 0;

    void Update()
    {
        float currentSpeed = Input.GetAxis("Horizontal");
        if (lastSpeed != currentSpeed && Mathf.Abs(lastSpeed) < Mathf.Abs(currentSpeed))
        {
            for (int i = 1; i < slices.Length; i++)
            {
                float max_displacement = i * 0.01f;

                slices[i].transform.localPosition += new Vector3(-i * currentSpeed * slide_speed * Time.deltaTime, 0, 0);

                slices[i].transform.localPosition = new Vector3(
                    Mathf.Clamp(slices[i].transform.localPosition.x, start_position[i] - max_displacement, start_position[i] + max_displacement),
                    slices[i].transform.localPosition.y,
                    slices[i].transform.localPosition.z);
            }
        }
        lastSpeed = currentSpeed;
    }
}
