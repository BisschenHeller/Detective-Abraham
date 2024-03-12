using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Conversationalist : MonoBehaviour
{
    public Color textColor = Color.white;

    public TextMeshProUGUI textField;

    public Camera cam;

    private Animator anim;

    protected bool done_talking = true;

    public void Say(string sentence, float time)
    {
        done_talking = false;
        textField.text = sentence;
        Invoke("DoneTalking", time);
    }

    private void DoneTalking()
    {
        Invoke("EndOfPause", 0.25f);
        textField.text = "";
    }

    private void EndOfPause()
    {
        done_talking = true;
    }

    public bool IsDoneTalking()
    {
        return done_talking;
    }

    public void Start()
    {
        anim = GetComponent<Animator>();
        textField.color = textColor;
        //textField.text = gameObject.name;
        textField.text = "";
    }

    void Update()
    {
        anim.SetBool("Talking", !done_talking);
        Vector3 relative_screen_position = ((transform.position - cam.transform.position) + new Vector3(1,1,0)) / 2;
        //Debug.Log("Relative Screen Pos " + gameObject.name + ": " + relative_screen_position + "  results in x = " + (relative_screen_position.x * Camera.main.pixelWidth));
        textField.transform.position = new Vector3(relative_screen_position.x * Camera.main.pixelWidth, textField.transform.position.y, 0);
    }
}