using UnityEngine;

public class SpeedLeech : MonoBehaviour
{
    private Animator anim;

    public AufzugController controller;

    private void Start()
    {
        anim = GetComponent<Animator>();   
    }

    private void Update()
    {
        anim.SetFloat("Speed", controller.speed * controller.direction);
    }
}