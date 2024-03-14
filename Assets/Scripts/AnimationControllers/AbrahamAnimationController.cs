using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AbrahamAnimationController : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private float walking_speed;

    [SerializeField]
    private GameObject imperfect_Abraham;

    private Animator imp_anim;

    public GameObject backdrop_slices;

    public bool locked;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        imp_anim = imperfect_Abraham.GetComponent<Animator>();

        anim.SetFloat("Speed", 0);
        imp_anim.SetFloat("Speed", 0);
    }

    private float lastMovement_sign = 1;

    int GetUpperClamp(float pos_x)
    {
        // -3 Stopp.
        if (pos_x < -2.68f) return 6; // -2.68 Fahrstuhl		0 - 6
        if (pos_x < -2.56f) return 5; // -2.56 Fahrstuhltür	2 - 5
        if (pos_x < -2.44f) return 8; // -2.44 Vor Schuhregal	0 - 8
        if (pos_x < -2.19f) return 7; // -2.19 Schuhregal	0 - 7
        if (pos_x < -0.67f) return 8; // -0.67 Flur Offen	0 - 8
        if (pos_x < -0.56f) return 3; // -0.56 Türe		2 - 3
        if (pos_x < -0.29f) return 6; // -0.29 Vor Schrank	1 - 6
        if (pos_x < -0.02f) return 7; // -0.02 Küche vorne	0 - 7
        if (pos_x <  0.10f) return 7;  // 0.10 Hinter Tisch	5 - 7
        if (pos_x < 0.22f) return 7;  // 0.22 Hinter stuhl 	6 - 7
        if (pos_x < 0.43f) return 7; // 0.43 Hinter Joshua	5 - 7
        else return 7;               // bis 0.6		0 - 7
    }

    int GetLowerClamp(float pos_x)
    {
        // -3 Stopp.
        if (pos_x < -2.68f) return 0; // -2.68 Fahrstuhl		0 - 6
        if (pos_x < -2.56f) return 2; // -2.56 Fahrstuhltür	2 - 5
        if (pos_x < -2.44f) return 0; // -2.44 Vor Schuhregal	0 - 8
        if (pos_x < -2.19f) return 0; // -2.19 Schuhregal	0 - 7
        if (pos_x < -0.67f) return 0; // -0.67 Flur Offen	0 - 8
        if (pos_x < -0.56f) return 2; // -0.56 Türe		2 - 3
        if (pos_x < -0.29f) return 1; // -0.29 Vor Schrank	1 - 6
        if (pos_x < -0.02f) return 0; // -0.02 Küche vorne	0 - 7
        if (pos_x < 0.10f) return 5;  // 0.10 Hinter Tisch	5 - 7
        if (pos_x < 0.22f) return 6;  // 0.22 Hinter stuhl 	6 - 7
        if (pos_x < 0.43f) return 5; // 0.43 Hinter Joshua	5 - 7
        else return 0;               // bis 0.6		0 - 7
    }

    public bool force_right = false;

    // Update is called once per frame
    void Update()
    {
        if (locked)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.005f);
            anim.SetFloat("Speed", 0);
            imp_anim.SetFloat("Speed", 0);
            return;
        }

        float horizmovement = Input.GetAxis("Horizontal") * (Input.GetKey(KeyCode.LeftShift)? 1.5f : 1);
        float vertmovement = Input.GetAxis("Vertical");
        if (force_right) horizmovement = Mathf.Abs(horizmovement);

        anim.SetFloat("Speed", Mathf.Max(Mathf.Abs(horizmovement), Mathf.Abs(vertmovement)));
        imp_anim.SetFloat("Speed", Mathf.Max(Mathf.Abs(horizmovement), Mathf.Abs(vertmovement)));

        if (horizmovement != 0)
        {
            transform.rotation = Quaternion.identity;
            transform.Rotate(0, horizmovement > 0 ? 180 : 0, 0);
            lastMovement_sign = Mathf.Sign(horizmovement);
        }

        float moving_y = vertmovement * walking_speed * Time.deltaTime;

        transform.Translate((horizmovement > 0 ? -1 : 1) * horizmovement * walking_speed * Time.deltaTime, // Horizontal Movement
            0, 0 );


        transform.localPosition = new Vector3(  // Vertical Movement
            transform.localPosition.x,
            transform.localPosition.y + moving_y,
            0);// Changing Layers, multiplying last movement dir because sprite might be flipped
        if (transform.localPosition.y + 0.18f > 0.01)
        {
            sliceNo++;
            //Debug.Log("Slice nach hinten (Zu Layer " + sliceNo + ") gehen weil transform.localposition.y = " + transform.localPosition.y);
        }
        else if (transform.localPosition.y + 0.18f < -0.001)
        {
            sliceNo--;
            //Debug.Log("Slice vorne (Zu Layer " + sliceNo + ") gehen weil transform.localposition.y = " + transform.localPosition.y);
        }

        int upperclamp = GetUpperClamp(transform.position.x);
        int lowerclamp = GetLowerClamp(transform.position.x);

        sliceNo = Mathf.Clamp(sliceNo, lowerclamp, upperclamp);
        if (sliceNo == lowerclamp)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Max(-0.18f, transform.localPosition.y), -0.001f);
        }
        else if (sliceNo == upperclamp)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Min(-0.17f, transform.localPosition.y), -0.001f);
        }
        
        
        transform.parent = backdrop_slices.transform.GetChild(sliceNo);
    }

    int sliceNo = 0;
}
