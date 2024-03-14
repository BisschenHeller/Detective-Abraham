using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SchlussworteScript : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update

    bool OnOff = true;

    void Start()
    {
        Invoke("One", 2);
        Invoke("One", 5);
        Invoke("Two", 7);
        Invoke("Two", 9);
    }

    private void One()
    {
        textMeshProUGUI.text = "I'll leave that question to you.";
        textMeshProUGUI.color = OnOff ? new Color(0.9056604f, 0.3033108f, 0.8284168f, 1) : Color.black;
        OnOff = !OnOff;
    }

    private void Two()
    {
        transform.position -= new Vector3(0, 400, 0);
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        textMeshProUGUI.text = "Time to call for cleanup.";
        textMeshProUGUI.color = OnOff ? new Color(0.9056604f, 0.3033108f, 0.8284168f, 1) : Color.black;
        OnOff = !OnOff;
    }
}
