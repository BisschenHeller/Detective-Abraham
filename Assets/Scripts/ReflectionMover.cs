using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionMover : MonoBehaviour
{
    [SerializeField]
    private AbrahamAnimationController abe = null;

    [SerializeField]
    private float offset = 0;

    void Start()
    {
        if (abe == null) Debug.LogError("Abraham Not Assigned!");
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, offset - 2*abe.transform.localPosition.y, transform.localPosition.z);
    }
}
