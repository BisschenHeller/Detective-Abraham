using UnityEngine;

public class TalkingLeech : MonoBehaviour
{
    private Animator anim;

    public Conversationalist leechingFrom;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("Talking", !leechingFrom.IsDoneTalking() && !leechingFrom.is_thought);
    }
}