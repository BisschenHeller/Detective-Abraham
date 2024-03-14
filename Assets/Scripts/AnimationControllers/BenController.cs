using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenController : MonoBehaviour
{
    public bool phone_out = false;

    private Animator anim;

    [Range(-1.2f, 2.14f)]
    public float target_x;

    public float speed = 1;

    public float arrival_threshold = 0.1f;

    public void SetNervosity(float nervous)
    {
        anim.SetFloat("Nervous", nervous);
    }

    //public GameObject TestCube;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        SetNervosity(1);
    }
    
    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Phone", phone_out);

        if (Mathf.Abs(target_x - transform.localPosition.x) > arrival_threshold)
        {
            float walk = Mathf.Sign(target_x - transform.localPosition.x) * speed * Time.deltaTime;
            transform.localPosition += new Vector3(walk, 0, 0);
            anim.SetFloat("Speed", Mathf.Clamp(walk, -1, 1));
        } else {
            anim.SetFloat("Speed", 0);
        }
        //transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x * 100.0f) / 100.0f, transform.localPosition.y, transform.localPosition.z);
        //TestCube.transform.localPosition = new Vector3(target_x, 0, 0);
        //TestCube.transform.localScale = new Vector3(arrival_threshold, 0.3f, 0.1f);
    }

    public bool has_gun_out = false;

    public void TakeGunOut()
    {
        has_gun_out = true;
        anim.SetBool("GunOut", true);
        anim.SetBool("Phone", false);
    }

    public void PutGunAway()
    {
        has_gun_out = false;
        anim.SetBool("GunOut", false);
    }

    public bool IsDoneWalking()
    {
        return Mathf.Abs(target_x - transform.localPosition.x) <= arrival_threshold;
    }
}
