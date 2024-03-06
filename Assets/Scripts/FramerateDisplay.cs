using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FramerateDisplay : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        m_TextMeshProUGUI.text = Mathf.Round(1.0f / Time.deltaTime) + "FPS";
    }
}
