using UnityEngine;

[RequireComponent (typeof(MeshRenderer))]
public class BlinkingController : MonoBehaviour
{
    private MeshRenderer m_MeshRenderer = null;

    public float GetBlinking()
    {
        return Input.GetAxis("Blink");
    }

    public float GetEyesClosed()
    {
        return Input.GetAxis("Eyes_Closed");
    }

    private void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    private float artificial_blink = 0;
    private float artificial_ec = 0;
    private float artificial_al = 0;

    private bool force = false;

    public bool forceAwake = false;

    public void ForceBlink()
    {
        force = true;
    }

    public void StopForce()
    {
        force = false;
    }

    public void ForceAwake()
    {
        forceAwake = true;
    }

    public void StopForceAwake()
    {
        forceAwake = false;
    }

    public static bool way_down = false;

    public float lastValue = 0;

    private void Update()
    {
        if (forceAwake) { return; }
        if (force)
        {
            artificial_blink = Mathf.Clamp01(artificial_blink + Time.deltaTime * 3);
            artificial_ec = Mathf.Clamp01(artificial_blink + Time.deltaTime * 0.5f);
            artificial_al = Mathf.Clamp01(artificial_al + Time.deltaTime * 0.5f);
        } else
        {
            artificial_blink = Mathf.Clamp01(artificial_blink - Time.deltaTime * 3);
            artificial_ec = Mathf.Clamp01(artificial_blink - Time.deltaTime * 5);
            artificial_al = Mathf.Clamp01(artificial_al - Time.deltaTime * 0.4f);
        }

        way_down = lastValue > Mathf.Max(artificial_blink, Input.GetAxis("Blink"));
        lastValue = Mathf.Max(artificial_blink, Input.GetAxis("Blink"));

        m_MeshRenderer.material.SetFloat("blinking", Mathf.Max(artificial_blink, Input.GetAxis("Blink")));
        m_MeshRenderer.material.SetFloat("eyes_closed", Mathf.Max(artificial_ec, Input.GetAxis("Eyes_Closed")));
        m_MeshRenderer.material.SetFloat("aberrations_linger", Mathf.Max(artificial_al, Input.GetAxis("Aberrations_Linger")));
        m_MeshRenderer.material.SetFloat("timer", Time.fixedTime);
        m_MeshRenderer.material.SetFloat("notebook", Input.GetAxis("Notebook"));
    }
}