using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AbrahamAnimationController : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private float walking_speed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", 0);
    }

    private float lastMovement_sign = 1;
    float GetUpperClamp(float pos_x)
    {
        
        if (pos_x < -0.55) return 0.4f;
        if (pos_x < -0.25) return 0.6f;
        if (pos_x < -0.15) return 0.8f;
        else return 0.6f;
    }

    float GetLowerClamp(float pos_x)
    {
        if (pos_x < -0.55) return 0.2f;
        if (pos_x < -0.09) return 0;
        if (pos_x < 0.05) return 0.3f;
        if (pos_x < 0.43) return 0.5f;
        else return 0.0f;
    }

   

    private float depth_value = 0;

    // Update is called once per frame
    void Update()
    {
        float horizmovement = Input.GetAxis("Horizontal") * (Input.GetKey(KeyCode.LeftShift)? 1.5f : 1);
        float vertmovement = Input.GetAxis("Vertical");

        anim.SetFloat("Speed", Mathf.Max(Mathf.Abs(horizmovement), Mathf.Abs(vertmovement)));

        if (horizmovement != 0)
        {
            transform.rotation = Quaternion.identity;
            transform.Rotate(0, horizmovement > 0 ? 180 : 0, 0);
            lastMovement_sign = Mathf.Sign(horizmovement);
        }

        float moving_y = vertmovement * walking_speed * Time.deltaTime;

        transform.Translate((horizmovement > 0 ? -1 : 1) * horizmovement * walking_speed * Time.deltaTime, // Horizontal Movement
            0, 0 );

        {
            float desired = depth_value + moving_y*10;
            float possible = Mathf.Clamp(desired, GetLowerClamp(transform.localPosition.x), GetUpperClamp(transform.localPosition.x));
            depth_value = possible;
        }

        transform.localPosition = new Vector3(  // Vertical Movement
            transform.localPosition.x,
            depth_value*0.1f - 0.17f,
            1 + depth_value*0.1f);// Changing Layers, multiplying last movement dir because sprite might be flipped
    }
}
