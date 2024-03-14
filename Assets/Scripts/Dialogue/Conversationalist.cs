using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Conversationalist : MonoBehaviour
{
    public Color textColor = Color.white;

    public TextMeshProUGUI textField;

    public Camera cam;

    private Animator anim;

    protected bool done_talking = true;

    private float initial_alpha;

    public bool i_talk_with_phone_in_hand;

    public void Think(string thought, float time)
    {
        is_thought = true;
        done_talking = false;
        textField.text = thought;
        Invoke("DoneTalking", time);
        if (!i_talk_with_phone_in_hand)
            textField.color = new Color(textColor.r, textColor.g, textColor.b, initial_alpha * 0.65f);
    }

    public bool is_thought = false;

    public void Say(string sentence, float time)
    {
        is_thought = false;
        done_talking = false;
        textField.text = sentence;
        textField.color = new Color(textColor.r, textColor.g, textColor.b, initial_alpha);
        
        Invoke("DoneTalking", time*1.5f);
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
        initial_alpha = textColor.a;
        anim = GetComponent<Animator>();
        textField.color = textColor;
        //textField.text = gameObject.name;
        textField.text = "";
    }

    void Update()
    {
        anim.SetBool("Talking", !done_talking && !is_thought);
        Vector3 relative_screen_position = ((transform.position - cam.transform.position) + new Vector3(1,1,0)) / 2;
        //textField.rectTransform.pivot = new Vector2(Mathf.Clamp01(relative_screen_position.x), 0);
        textField.transform.position = new Vector3(Mathf.Clamp(relative_screen_position.x * Camera.main.pixelWidth, 350, 1570), textField.transform.position.y, 0);
    }
}