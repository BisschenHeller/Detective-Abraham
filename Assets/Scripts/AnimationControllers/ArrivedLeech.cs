using UnityEngine;

public class ArrivedLeech : MonoBehaviour
{
    private Animator anim;

    public AufzugController controller;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("Arrived", controller.arrived);
    }

    public void Arrived()
    {
        
    }
}