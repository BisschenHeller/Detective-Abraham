using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private TextMeshProUGUI textField;

    [SerializeField]
    private float yOffset = 5;

    private float initialy;

    [SerializeField]
    private float speed = 1;

    private void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        initialy = transform.position.y;
    }

    float progress = 0;
    float direction = 1;
    public void SetTutorialText(string text)
    {
        progress = 0;
        direction = 1;
        textField.text = text;
        transform.localPosition = new Vector3(0, yOffset, 0);
        textField.color = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        progress += Time.deltaTime * speed * direction;
        Mathf.Clamp(progress, 0, 1);
        
        textField.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, progress));
        transform.position = new Vector3(0, initialy + Mathf.Lerp(yOffset, 0, progress), 0);
    }

    public void ClearTutorialText()
    {
        progress = 1;
        direction = -1;
    }
}