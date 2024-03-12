using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockGame : MonoBehaviour
{
    [Range(-24,24)]
    public float einstellung = 0;

    public float actualTime = 1.2f;

    public Texture richtig2215;
    public Texture falsch2315;

    public GameObject perf_minutenzeiger;
    public GameObject perf_stundenzeiger;

    public GameObject real_minutenzeiger;
    public GameObject real_stundenzeiger;

    public GameObject clockParent;

    public bool inProgress = false;

    void Start()
    {
        // When Abraham Enters the building, it's 1:20 a.m.
        actualTime = 4.0f / 3.0f;
        clockParent.transform.position = clockParent.transform.position - new Vector3(0, 100, 0);
    }

    public bool movingClockBack = false;

    public bool done = false;

    public bool correct_time = false;

    public bool solved = false;

    public bool solved_wrong = false;

    public void SwitchZiffernblatt(bool _richtig2215)
    {
        Debug.Log("Switching to " + _richtig2215);
        GetComponent<RawImage>().texture = _richtig2215 ? richtig2215 : falsch2315;
        correct_time = _richtig2215;
    }

    // Update is called once per frame
    void Update()
    {
        // Ziel:  -3.08333 (Falsche Uhr)
        // Sonst: -4.08333 (Richtige Uhr)
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (inProgress) EndMinigame();
            else StartMingame();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchZiffernblatt(!correct_time);
        }

        if (solved) return;
        if (!inProgress && !movingClockBack) return;

        if (movingClockBack)
        {
            einstellung /= 1.1f;
            if (einstellung <= 0.01f)
            {
                Invoke("Done", 0.5f);
                movingClockBack = false;
            }
        }

        if (inProgress) {
            einstellung += Input.mouseScrollDelta.y * 0.08333333333f;
        }
        
        perf_minutenzeiger.transform.rotation = Quaternion.identity;
        perf_stundenzeiger.transform.rotation = Quaternion.identity;
        real_minutenzeiger.transform.rotation = Quaternion.identity;
        real_stundenzeiger.transform.rotation = Quaternion.identity;

        perf_minutenzeiger.transform.Rotate(new Vector3(0,0,-360 * (actualTime % 1.0f)));
        perf_stundenzeiger.transform.Rotate(new Vector3(0,0,-30 * actualTime));

        real_minutenzeiger.transform.Rotate(new Vector3(0, 0, -360 * ((actualTime + einstellung + 1) % 1.0f)));
        real_stundenzeiger.transform.Rotate(new Vector3(0, 0, -30 * (actualTime + einstellung + 1)));


        if ((correct_time && (Mathf.Abs(einstellung + 4.08333f) < 0.005)))
        {
            solved = true;
            FindObjectOfType<Notebook_Controller>().AddNote(NoteID.Died2215);
            enabled = false;
        }
        else if (!solved_wrong && !correct_time && (Mathf.Abs(einstellung + 3.08333f) < 0.005))
        {
            solved_wrong = !solved;
            if (solved_wrong)
            {
                FindObjectOfType<Notebook_Controller>().AddNote(NoteID.ProbablyDied2315);
            }
        }
    }

    public void StartMingame()
    {
        actualTime = 1.3333333333f;
        einstellung = 0;
        inProgress = true;
        clockParent.transform.localPosition += new Vector3(0, 100, 0);
    }

    public void EndMinigame()
    {
        inProgress = false;
        movingClockBack = true;
    }

    public void Done()
    {
        Debug.Log("Done");
        clockParent.transform.localPosition -= new Vector3(0, 100, 0);
    }
}
