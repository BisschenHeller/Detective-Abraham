using UnityEngine;

[RequireComponent (typeof(MeshRenderer))]
public class BlinkingController : MonoBehaviour
{
    private MeshRenderer m_MeshRenderer = null;

    private void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        m_MeshRenderer.material.SetFloat("blinking", Input.GetAxis("Blink"));
        m_MeshRenderer.material.SetFloat("eyes_closed", Input.GetAxis("Eyes_Closed"));
        
        m_MeshRenderer.material.SetFloat("timer", Time.fixedTime);
    }
}