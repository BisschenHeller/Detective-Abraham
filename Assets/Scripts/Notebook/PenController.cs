using UnityEngine;
using UnityEngine.UI;


[RequireComponent (typeof(Image))]
public class PenController : MonoBehaviour
{
    private Image pen_image;

    public GameObject schatten;

    public float schatten_abstand = 5;

    public Vector2 minmax_x;
    public Vector2 minmax_y;

    private void Start()
    {
        pen_image = GetComponent<Image>();
    }

    private void Update()
    {
        pen_image.color = new Color(1, 1, 1, Input.GetAxis("Notebook"));
    }

    private void LateUpdate()
    {
        transform.localPosition = new Vector3(
             Mathf.Clamp(transform.localPosition.x, minmax_x.x, minmax_x.y),
             Mathf.Clamp(transform.localPosition.y, minmax_y.x, minmax_y.y),
             transform.localPosition.z
            );
        if (Input.GetMouseButton(0))
        {
            schatten.transform.localPosition = transform.localPosition;

        }
        else
        {
            transform.localPosition += new Vector3(0, schatten_abstand, 0);
            schatten.transform.localPosition = transform.localPosition - new Vector3(0, schatten_abstand, 0);
        }
    }
}