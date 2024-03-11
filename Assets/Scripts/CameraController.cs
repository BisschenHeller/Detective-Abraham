using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject perfectAbraham;

    [Range(0, 0.99f)]
    public float stickiness = 0.99f;

    // Update is called once per frame
    void Update()
    {
        float posX = perfectAbraham.transform.localPosition.x;
        float camera_targetPos = transform.localPosition.x;

        if (posX > -0.6)
        {
            // Inside Joshua's kitchen
            camera_targetPos = 0;

        } else
        {
            // Im Flur
            camera_targetPos = perfectAbraham.transform.localPosition.x;
        }

        transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, camera_targetPos, Time.deltaTime * 5), transform.localPosition.y, transform.localPosition.z);
    }
}
