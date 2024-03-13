using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Notebook_Controller : MonoBehaviour
{
    public float max_y = -1;

    public float min_y = -7.5f;

    public List<Compromise> compromises;

    public List<Note> notes;

    public GameObject pictureCorrection;

    public TextMeshProUGUI compromiseHeading;
    public TextMeshProUGUI[] compromiseLines;

    public TextMeshProUGUI notesHeading;
    public TextMeshProUGUI[] notesLines;

    public TextMeshProUGUI notebookHint;

    public PenController pen;

    public void AddNote(NoteID id)
    {
        if (notes.Exists(a => a.id == id)) return;

        notebookHint.text = "[Tab] New note added";
        notebookHint.color = new Color(1, 1, 1, 0.3f);

        if (notes.Count == 0) AddCompromiseHeading();

        switch (id)
        {
            case NoteID.Joshua_ID:
                notes.Add(new Note(id));
                notesLines[notes.Count - 1].text = "- Joshua Braun (24), German";
                break;
        }
    }

    public void AddCompromiseHeading()
    {
        compromiseHeading.text = "What if...";
    }

    public void AddNotesHeading()
    {
        notesHeading.text = "Notes";
    }

    public void AddWhatIf(CompromiseID id)
    {
        if (compromises.Exists(a => a.id == id)) return;

        if (compromises.Count == 0) AddCompromiseHeading();

        notebookHint.text = "[Tab] New Compromise Added";
        notebookHint.color = new Color(1, 1, 1, 0.3f);

        switch (id)
        {
            case CompromiseID.HangingPicturesIsHard:
                compromises.Add(new Compromise(CompromiseID.HangingPicturesIsHard, 
                    () => { pictureCorrection.SetActive(true); return true; },
                    () => { pictureCorrection.SetActive(false); return true; },
                    compromiseLines[compromises.Count]));
                compromiseLines[compromises.Count - 1].text = "ö Hanging pictures was hard?";
                break;
        }
    }

    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        compromises = new List<Compromise>();
        notes = new List<Note>();
    }

    // Update is called once per frame
    void Update()
    {
        float lerp_value = Input.GetAxis("Notebook");
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(min_y, max_y, lerp_value), transform.localPosition.z);
        image.color = new Color(1, 1, 1, Mathf.Lerp(0.5f, 1, lerp_value));
        if (lerp_value >= 0.2f)
        {
            notebookHint.text = "[Tab] for Notebook";
            notebookHint.color = new Color(1, 1, 1, 0.1f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Mathf.Abs(pen.transform.localPosition.x - 5) > 5)
            {
                Debug.Log(pen.transform.localPosition.x + " zu weit daneben");
                return;
            }

            int requested_line = Mathf.RoundToInt(Mathf.Floor(Mathf.Abs(pen.transform.localPosition.y - 155) / 10));
            
            if (requested_line >= compromises.Count)
            {
                Debug.Log("Requested line: " + requested_line + ", not that many compromises exist.");
                return;
            }

            compromises[requested_line].SetActive(!compromises[requested_line].IsActive());
        }
    }
}

public class Compromise
{
    public Compromise(CompromiseID _id, Func<bool> whena, Func<bool> wheni, TextMeshProUGUI text)
    {
        id = _id;
        whenActive = whena;
        whenInactive = wheni;
        active = false;
        textField = text;
    }

    public void SetActive(bool newActive)
    {
        active = newActive;
        if (newActive)
        {
            whenActive.Invoke();
            textField.text = textField.text.Replace('ö', 'ü');
        }
        else
        {
            whenInactive.Invoke();
            textField.text = textField.text.Replace('ü', 'ö');
        }
    }

    public bool IsActive()
    {
        return active;
    }

    public CompromiseID id;
    private bool active;
    private TextMeshProUGUI textField;
    public Func<bool> whenActive;
    public Func<bool> whenInactive;
}

public class Note
{
    public Note(NoteID _id)
    {
        id = _id;
        active = true;
    }
    public NoteID id;
    public bool active;
}

public enum NoteID
{
    Joshua_ID = 0, Clock_Lazy = 1, ProbablyDied2315 = 2, Died2215 = 3
}

public enum CompromiseID
{
    NONE = 0,

    HangingPicturesIsHard = 1
}