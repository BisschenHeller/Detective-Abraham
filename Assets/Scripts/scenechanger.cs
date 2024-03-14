using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scenechanger : MonoBehaviour
{
    private float opacity = 1;

    private float direction = 1;

    public Image abdeckung;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Change", 8);
    }

    private void Change()
    {
        direction = -1;
    }

    // Update is called once per frame
    void Update()
    {
        opacity -= Time.deltaTime * direction;
        
        opacity = Mathf.Clamp01(opacity);
        if (opacity == 1) SceneManager.LoadScene("BackupScene");
        abdeckung.color = new Color(0, 0, 0, opacity);
    }
}
