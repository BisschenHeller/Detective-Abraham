using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockGame : MonoBehaviour
{
    [Range(-24,24)]
    public float einstellung = 0;

    private float initial_y = 0;

    public float actualTime = 1.2f;

    public Texture falsch2215;
    public Texture richtig2315;
    public Texture cleanTex;

    public GameObject perf_minutenzeiger;
    public GameObject perf_stundenzeiger;

    public GameObject real_minutenzeiger;
    public GameObject real_stundenzeiger;

    public GameObject realParent;
    public GameObject perfParent;

    public List<GameObject> dirtyZeiger;

    public bool inProgress = false;

    void Start()
    {
        // When Abraham Enters the building, it's 1:20 a.m.
        actualTime = 4.0f / 3.0f;
        initial_y = realParent.transform.position.y;
        realParent.transform.position = new Vector3(realParent.transform.position.x, initial_y-100, realParent.transform.position.z);
        perfParent.transform.position = new Vector3(perfParent.transform.position.x, initial_y-100, perfParent.transform.position.z);
        
    }

    public bool movingClockBack = false;

    public bool done = false;

    public bool _correct_time = false;
    public bool _clean = true;

    public bool solved = false;

    public bool solved_wrong = false;

    public void SwitchZiffernblattCorrect(bool correct_time)
    {
        _correct_time = correct_time;
        if (_clean) return;
        if (_correct_time)
        {
            GetComponent<RawImage>().texture = richtig2315;
        } else
        {
            GetComponent<RawImage>().texture = falsch2215;
        }
    }

    public void SwitchZiffernblattCleanliness(bool clean)
    {
        _clean = clean;
        if (_clean)
        {
            GetComponent<RawImage>().texture = cleanTex;
            dirtyZeiger.ForEach(a => a.SetActive(false));
        }
        else
        {
            dirtyZeiger.ForEach(a => a.SetActive(true));
            if (_correct_time)
            {
                GetComponent<RawImage>().texture = richtig2315;
            }
            else
            {
                GetComponent<RawImage>().texture = falsch2215;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inProgress && (Mathf.Abs(Input.GetAxis("Horizontal")) != 0 || Mathf.Abs(Input.GetAxis("Vertical")) != 0))
        {
            EndMinigame();
        }
        if (movingClockBack)
        {
            einstellung -= 2 * Time.deltaTime * Mathf.Sign(einstellung);
            if (Mathf.Abs(einstellung) <= 0.05f)
            {
                Invoke("Done", 0.5f);
                movingClockBack = false;
            }
        }
        
        if (!inProgress && !movingClockBack) return;

        if (inProgress) {
            einstellung += Input.mouseScrollDelta.y * 0.08333333333f;
        }
        
        perf_minutenzeiger.transform.rotation = Quaternion.identity;
        perf_stundenzeiger.transform.rotation = Quaternion.identity;
        real_minutenzeiger.transform.rotation = Quaternion.identity;
        real_stundenzeiger.transform.rotation = Quaternion.identity;

        perf_minutenzeiger.transform.Rotate(new Vector3(0, 0, -360 * ((actualTime + einstellung) % 1.0f)));
        perf_stundenzeiger.transform.Rotate(new Vector3(0, 0, -30 * (actualTime + einstellung - (_correct_time ? -1 : 0))));

        real_minutenzeiger.transform.Rotate(new Vector3(0, 0, -360 * ((actualTime + einstellung) % 1.0f)));
        real_stundenzeiger.transform.Rotate(new Vector3(0, 0, -30 * (actualTime + einstellung + 1)));

        if (solved) return;

        //  Blut    && Imaginäre Welt                   && kompromiss für richtige Zeit && Richtige Einstellung
        if (!_clean && Input.GetAxis("Blink") > 0.8f && _correct_time && (Mathf.Abs(einstellung + 3.08333f) < 0.005))
        {
            solved = true;
            FindObjectOfType<Notebook_Controller>().AddNote(NoteID.Died2315);
        }
        //       Blut    && !solved && !correctTime   &&
        else if (!solved_wrong && !solved && !_correct_time && (Mathf.Abs(einstellung + 4.08333f) < 0.005))
        {
            float blink = Input.GetAxis("Blink");
            if ((blink > 0.5f && !_clean) || (blink < 0.5f))
            {
                FindObjectOfType<Notebook_Controller>().AddNote(NoteID.ProbablyDied2215);
                solved_wrong = true;
            }
        }
    }

    public void StartMingame()
    {
        TutorialManager manager = FindObjectOfType<TutorialManager>();
        manager.SetTutorialText("Use your mousewheel to change the clock.");
        manager.Invoke("ClearTutorialText", 4);

        actualTime = 1.3333333333f;
        einstellung = 0;
        inProgress = true;
        realParent.transform.position = new Vector3(realParent.transform.position.x, initial_y, realParent.transform.position.z);
        perfParent.transform.position = new Vector3(perfParent.transform.position.x, initial_y, perfParent.transform.position.z);
    }

    public void EndMinigame()
    {
        inProgress = false;
        movingClockBack = true;
    }

    public void Done()
    {
        Debug.Log("Done");
        realParent.transform.position = new Vector3(realParent.transform.position.x, initial_y - 100, realParent.transform.position.z);
        perfParent.transform.position = new Vector3(perfParent.transform.position.x, initial_y - 100, perfParent.transform.position.z);
    }
}

public enum Ziffernblatt
{
    DirtyCorrect2215, DirtyWrong2315, Clean
}
