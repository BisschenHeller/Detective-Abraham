using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AbrahamAnimationController : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private float walking_speed;

    public float GetCurrentSpeed()
    {
        return Input.GetAxis("Horizontal") * walking_speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", 0);
    }

    // Update is called once per frame
    void Update()
    {
        float horizmovement = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(horizmovement));


        if (horizmovement != 0)
        {
            transform.rotation = Quaternion.identity;
            transform.Rotate(0, horizmovement > 0 ? 180 : 0, 0);
        }
        
        transform.Translate((horizmovement > 0 ? -1 : 1) * horizmovement * walking_speed, 0, 0);
    }
}
