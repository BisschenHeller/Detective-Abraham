using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
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

    public ClockGame clockGame;

    public List<GameObject> joshua_dead_blood_gun;
    public List<GameObject> joshua_alive_clock;

    public List<GameObject> bomsch_and_cup;
    public GameObject businessman;

    private int wrongNoteIndex = -1;

    private int suicideIndex = -1;
    private int guyLeavingIndex = -1;

    public bool can_talk_to_sven = false;

    public bool ContainsNote(NoteID note)
    {
        return notes.Exists(a => a.id == note);
    }

    public bool ContainsCompromise(CompromiseID id)
    {
        return compromises.Exists(a => a.id == id);
    }

    public void AddNote(NoteID id)
    {
        if (notes.Exists(a => a.id == id)) return;

        notebookHint.text = "[Tab] New note added";
        notebookHint.color = new Color(1, 1, 1, 0.3f);

        if (notes.Count == 0) AddNotesHeading();

        switch (id)
        {
            case NoteID.Joshua_ID:
                notes.Add(new Note(id));
                notesLines[notes.Count - 1].text = "- Joshua Braun (24), German";
                break;
            case NoteID.Clock_Lazy:
                notes.Add(new Note(id));
                notesLines[notes.Count - 1].text = "- Joshua's clock is 1h too early";
                if (notes.Exists(a => a.id == NoteID.ProbablyDied2215))
                {
                    notesLines[wrongNoteIndex].text = "<s>" + notesLines[wrongNoteIndex].text;
                }
                break;
            case NoteID.ProbablyDied2215:
                notes.Add(new Note(id));
                notesLines[notes.Count - 1].text = "- was shot at 22:15";
                wrongNoteIndex = notes.Count - 1;
                break;
            case NoteID.Died2315:
                notes.Add(new Note(id));
                if (notes.Exists(a => a.id == NoteID.ProbablyDied2215))
                {
                    notesLines[notes.Count - 1].text = "- was actually shot at 23:15";
                }
                else
                {
                    notesLines[notes.Count - 1].text = "- was shot at 23:15";
                }
                if (notes.Exists(a => a.id == NoteID.SomeoneLeftAt23))
                {
                    AddNote(NoteID.Alibi);
                }

                break;
            case NoteID.IfSuicideGun:
                notes.Add(new Note(id));
                notesLines[notes.Count - 1].text = "- No gun nearby ~ No suicide!";
                compromiseLines[suicideIndex].text = "<s>" + compromiseLines[suicideIndex].text;
                break;
            case NoteID.JoshuaCameWithCompany:
                notes.Add(new Note(id));
                notesLines[notes.Count - 1].text = "- came home at 8 with company";
                break;
            case NoteID.SomeoneLeftAt23:
                notes.Add(new Note(id));
                notesLines[notes.Count - 1].text = "- someone left at 23:00 !!";
                guyLeavingIndex = notes.Count - 1;
                if (notes.Exists(a => a.id == NoteID.Died2315))
                {
                    AddNote(NoteID.Alibi);
                }
                break;
            case NoteID.Alibi:
                notes.Add(new Note(NoteID.Alibi));
                notesLines[notes.Count - 1].text = "~Innocent. Joshua died later.";
                notesLines[guyLeavingIndex].text = "<s>" + notesLines[guyLeavingIndex].text;

                break;
            default:
                Debug.LogError("Note not Implemented: " + id.DisplayName());
                break;
        }
        
        if (notes.Exists(a => a.id == NoteID.IfSuicideGun) &&
            notes.Exists(a => a.id == NoteID.JoshuaCameWithCompany) &&
            notes.Exists(a => a.id == NoteID.Died2315) &&
            notes.Exists(a => a.id == NoteID.SomeoneLeftAt23))
        {
            notes.Add(new Note(NoteID.MurdererIsStillHere));
            notesLines[notes.Count - 1].text = "  <u>The killer is in the building!!";
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
                compromises.Add(new Compromise(id, 
                    () => { pictureCorrection.SetActive(true); return true; },
                    () => { pictureCorrection.SetActive(false); return true; },
                    compromiseLines[compromises.Count]));
                compromiseLines[compromises.Count - 1].text = "ö Hanging pictures was hard";
                break;
            case CompromiseID.JohuasClockWasAccurate:
                compromises.Add(new Compromise(id,
                    () => { clockGame.SwitchZiffernblattCorrect(true); return true; },
                    () => { clockGame.SwitchZiffernblattCorrect(false); return true; },
                    compromiseLines[compromises.Count]));
                compromiseLines[compromises.Count - 1].text = "ö Joshuas clock was accurate";
                break;
            case CompromiseID.JohuaCommittedSuicide:
                compromises.Add(new Compromise(id,
                    () => { joshua_dead_blood_gun.ForEach(a => a.SetActive(true));
                            joshua_alive_clock.ForEach(a => a.SetActive(false));
                            clockGame.SwitchZiffernblattCleanliness(false);
                            return true; },
                    () => { joshua_dead_blood_gun.ForEach(a => a.SetActive(false));
                            joshua_alive_clock.ForEach(a => a.SetActive(true));
                            clockGame.SwitchZiffernblattCleanliness(true);
                            return true; },
                    compromiseLines[compromises.Count]));
                compromiseLines[compromises.Count - 1].text = "ö Joshua comitted suicide";
                suicideIndex = compromises.Count - 1;
                break;
            case CompromiseID.SvenDidntChooseThisLife:
                compromises.Add(new Compromise(id,
                    () => {
                        can_talk_to_sven = true;
                        bomsch_and_cup.ForEach(a => a.SetActive(true));
                        businessman.SetActive(false);
                        return true;
                    },
                    () => {
                        can_talk_to_sven = false;
                        bomsch_and_cup.ForEach(a => a.SetActive(false));
                        businessman.SetActive(true);
                        return true;
                    },
                    compromiseLines[compromises.Count]));
                compromiseLines[compromises.Count - 1].text = "ö Sven didn't choose this life";
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

        joshua_dead_blood_gun.ForEach(a => a.SetActive(false));
        joshua_alive_clock.ForEach(a => a.SetActive(true));
        bomsch_and_cup.ForEach(a => a.SetActive(false));
        businessman.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            AddWhatIf(CompromiseID.JohuaCommittedSuicide);
            Debug.Log("Added Compromise " + CompromiseID.JohuaCommittedSuicide.DisplayName() + " Artificially");
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            AddWhatIf(CompromiseID.JohuasClockWasAccurate);
            Debug.Log("Added Compromise " + CompromiseID.JohuasClockWasAccurate.DisplayName() + " Artificially");
        }

        float lerp_value = Input.GetAxis("Notebook");
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(min_y, max_y, lerp_value), transform.localPosition.z);
        image.color = new Color(1, 1, 1, Mathf.Lerp(0.5f, 1, lerp_value));
        if (lerp_value >= 0.2f)
        {
            notebookHint.text = "[Tab] for Notebook";
            notebookHint.color = new Color(1, 1, 1, 0.1f);
        }

        if (lerp_value == 1 && Input.GetMouseButtonDown(0))
        {
            if (Mathf.Abs(pen.transform.localPosition.x - 5) > 5)
            {
                //Debug.Log(pen.transform.localPosition.x + " zu weit daneben");
                return;
            }

            int requested_line = Mathf.RoundToInt(Mathf.Floor(Mathf.Abs(pen.transform.localPosition.y - 161) / 10.0f));
            
            if (requested_line >= compromises.Count)
            {
                //Debug.Log("Requested line: " + requested_line + ", not that many compromises exist.");
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
    Joshua_ID = 0,   
    Clock_Lazy = 1, 
    Died2315 = 2, 
    ProbablyDied2215 = 3,
    IfSuicideGun = 4,
    JoshuaCameWithCompany = 5,
    MurdererIsStillHere = 6,
    SomeoneLeftAt23 = 7,
    Alibi = 8
}

public enum CompromiseID
{
    NONE = 0,

    HangingPicturesIsHard = 1,
    JohuasClockWasAccurate = 2,
    JohuaCommittedSuicide = 3,
    SvenDidntChooseThisLife = 4,
    Feign_IfSuicideWhereGun = 20,
    Feign_WrongClock = 21
}