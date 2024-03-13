using UnityEngine;

public class TakingTheElevator : MonoBehaviour
{
    public AufzugController animController;

    public AbrahamAnimationController abraham;

    public MeshRenderer rendererForFade;

    public CameraController cam;

    public float fade = 0.0f;

    public bool fading = false;

    public float fadingDir;

    public bool nachdrausssen;

    private void Done()
    {
        fading = false;
    }

    private void DoneFading()
    {
        if (fadingDir == -1)
        {
            Done();
            return;
        }
        if (nachdrausssen)
        {
            cam.transform.localPosition = new Vector3(-10,-2,-1);
            abraham.transform.localPosition = new Vector3(-9.94f, 0.17f, -0.005f);
            Debug.Log("Displaced nach drauﬂen.");
        } else
        {
            cam.transform.localPosition = new Vector3(-2.7f, -2, -1);
            abraham.transform.localPosition = new Vector3(-2.81f, 0.17f, -0.005f);
            Debug.Log("Displaced nach drinnen.");
        }
        fadingDir = -1 * fadingDir;
    }

    public void TakeElevatorUp()
    {
        fading = true;
        fadingDir = 1;
        nachdrausssen = false;
    }

    public void TakeElevatorDown()
    {
        fading = true;
        fadingDir = 1;
        nachdrausssen = true;
    }

    private void Update()
    {
        if (fading)
        {
            fade += Time.deltaTime * fadingDir;
            fade = Mathf.Clamp01(fade);
            rendererForFade.material.SetFloat("fade", fade);
            if (fade == 0 || fade == 1) DoneFading();
        }
    }
}