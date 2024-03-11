using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Notebook_Controller : MonoBehaviour
{
    public float max_y = -1;

    public float min_y = -7.5f;

    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float lerp_value = Input.GetAxis("Notebook");
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(min_y, max_y, lerp_value), transform.localPosition.z);
        image.color = new Color(1, 1, 1, Mathf.Lerp(0.5f, 1, lerp_value));
    }
}