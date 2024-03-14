using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCenter : MonoBehaviour
{
    [SerializeField]
    private Conversationalist officerBen;
    [SerializeField]
    private Conversationalist joshuaBraun;
    [SerializeField]
    private Conversationalist abraham;
    [SerializeField]
    private Conversationalist businessSven;
    [SerializeField]
    private Conversationalist bomschSven;

    public Notebook_Controller notebook;

    [SerializeField]
    private MyCursor cursorLock;

    private List<Phrase> currentDialogue = null;
    private int currentDialogueStep = 0;

    public bool talkedaboutmurder = false;

    public bool showdown = false;

    private void Showdown()
    {
        FindObjectOfType<Storyteller>().Showdown();
    }

    public void StartDialogue(DialogueID id)
    {
        currentDialogueStep = 0;
        abraham.GetComponent<AbrahamAnimationController>().locked = true;
        switch (id)
        {
            case DialogueID.Monologue_CrookedPainting:
                currentDialogue = new List<Phrase>() {
                    new Phrase(abraham, "Is it that hard to hang a painting straight?", 2),
                    new Phrase(officerBen, "Yea I heard you the first time.", 2)};
                break;
            case DialogueID.Monologue_AdmirePainting:
                currentDialogue = new List<Phrase>() {
                    new Phrase(abraham, "Beautiful.", 1),
                    new Phrase(abraham, "Pristine.", 1),
                    new Phrase(abraham, "I'd recognize his work anywhere.", 2),
                };
                break;
            case DialogueID.Monologue_ReadDoorbell01:
                currentDialogue = new List<Phrase>() { new Phrase(abraham, "\"P. Endyair\".", 1.5f, true) };
                break;
            case DialogueID.Monologue_ReadDoorbell02:
                currentDialogue = new List<Phrase>() { new Phrase(abraham, "\"D. Snudds\".", 1.5f, true) };
                break;
            case DialogueID.Monologue_ShoesReal:
                currentDialogue = new List<Phrase>() { new Phrase(abraham, "Ugh, some people...", 1, true) };
                break;
            case DialogueID.Monologue_ShoesPerf:
                currentDialogue = new List<Phrase>() { new Phrase(abraham, "A neatly organized shoe rack.", 2, true) };
                break;
            case DialogueID.Monologue_WallCrack:
                currentDialogue = new List<Phrase>() {
                    new Phrase(abraham, "This building is in dire need of a rennovation.", 2, false),
                    new Phrase(officerBen, "mhm", 1, true)
                };
                break;
            case DialogueID.Monologue_OfficerBen:
                if (!talkedaboutmurder)
                {
                    currentDialogue = new List<Phrase>() {
                        new Phrase(abraham, "Ben's a good guy.", 1, true),
                        new Phrase(abraham, "I've worked with him for years.", 2, true),
                        new Phrase(abraham, "", 0.5f, true),
                        new Phrase(officerBen, "Hey Abe", 1, false),
                        new Phrase(officerBen, "did you see that guy who turns into a pickle?", 3, false),
                        new Phrase(officerBen, "Funniest shit I've ever seen.", 2, true)
                    };
                }
                else
                {
                    currentDialogue = new List<Phrase>() {
                        new Phrase(abraham, "I've never seen him this tense.", 1.5f, true),
                        new Phrase(abraham, "Suicide cases are one thing", 1.5f, true),
                        new Phrase(abraham, "but it's not every day you get a shooter.", 2.0f, true)
                    };
                }
                break;
            case DialogueID.Dialogue_Ben:
                if (talkedaboutmurder && notebook.ContainsNote(NoteID.MurdererIsStillHere))
                {
                    currentDialogue = new List<Phrase>
                    {
                        new Phrase(abraham, "Ben!", 1.0f),
                        new Phrase(officerBen, "What is it!", 1.0f),
                        new Phrase(abraham, "The only guy who left the building tonight", 2),
                        new Phrase(abraham, "left before joshua was shot.", 2),
                        new Phrase(abraham, "So the murderer is still in the building.", 2),
                        new Phrase(officerBen, "...", 2.0f, true),
                        new Phrase(officerBen, "!!!", 1.0f, false, () => { officerBen.GetComponent<BenController>().SetNervosity(1.5f); return false; }),
                        new Phrase(officerBen, "Abe, I forgot to tell you something", 2.0f, true),
                        new Phrase(abraham, "what?", 1.0f, true),
                        new Phrase(officerBen, "I picked the lock when I came here.", 2.0f, true),
                        new Phrase(abraham, "...", 1.0f, true),
                        new Phrase(officerBen, "It was locked from the inside!", 1.0f, true),
                        new Phrase(abraham, "Are you telling me..", 2, true),
                        new Phrase(abraham, "the guy is still in there!?", 1.0f, true),
                        new Phrase(abraham, "In the apartment?", 1.5f, true, () => { Invoke("Showdown", 1); return false; })
                    };
                }
                else if (!talkedaboutmurder && notebook.ContainsNote(NoteID.IfSuicideGun))
                {
                    talkedaboutmurder = true;
                    currentDialogue = new List<Phrase>{
                        new Phrase(abraham, "Ben!", 1.0f, false, () => { officerBen.GetComponent<BenController>().phone_out = false; return false; }),
                        new Phrase(abraham, "Forensics weren't here yet, right?", 2.0f),
                        new Phrase(officerBen, "No, I was the first to arrive here.", 2.0f),
                        new Phrase(officerBen, "Like, an hour ago?", 2.0f),
                        new Phrase(abraham, "Did you take something from the apartment?", 2.0f),
                        new Phrase(officerBen, "Do I look like a fool to you?", 2.0f),
                        new Phrase(officerBen, "Of course I didn't.", 2.0f),
                        new Phrase(officerBen, "What's going on?", 2.0f),
                        new Phrase(abraham, "That guy did not commit suicide.", 2.0f),
                        new Phrase(abraham, "This is a murder scene.", 2.0f),
                        new Phrase(officerBen, "Ah fuck. You serious?", 2.0f),
                        new Phrase(officerBen, "There was a homeless guy out front.", 2.0f),
                        new Phrase(officerBen, "Maybe go ask if he saw someone come or go?", 2.0f),
                        new Phrase(officerBen, "I'll stand guard here until reinforcements arrive.", 3.5f, false,
                            () => { officerBen.GetComponent<BenController>().TakeGunOut();
                                    officerBen.GetComponent<BenController>().target_x = 0.86f; return false;
                            })
                    };
                }
                else if (talkedaboutmurder)
                {
                    currentDialogue = new List<Phrase>()
                    {
                        new Phrase(abraham, "Ben.", 1),
                        new Phrase(abraham, "In through your nose, out through your mouth.", 3),
                        new Phrase(officerBen, "Shut up man.", 2)
                    };
                }
                else
                {
                    currentDialogue = new List<Phrase>()
                    {
                        new Phrase(abraham, "It's hard to look at, isn't it?", 2, false),
                        new Phrase(officerBen, "Sure is.", 1, true),
                        new Phrase(abraham, "One of these days I'm going to quit.", 2, false),
                        new Phrase(officerBen, "mhm", 1.5f, true),
                        new Phrase(abraham, "It's tilted by like 10 degrees!", 2, false),
                        new Phrase(abraham, "", 1, true),
                        new Phrase(officerBen, "You serious?", 1.5f, false)
                    };
                }

                break;
            case DialogueID.Monologue_Kitchen_ClearSky:
                currentDialogue = new List<Phrase>() {
                    new Phrase(abraham, "Nothing more soothing than a clear night sky.", 2, true)
                };
                break;
            case DialogueID.Monologue_Kitchen_NeatDumpster:
                currentDialogue = new List<Phrase>() {
                    new Phrase(abraham, "Lid on top,", 1, false),
                    new Phrase(abraham, "trash actually inside.", 1.5f, false),
                    new Phrase(abraham, "just like god intended.", 1.5f, false)
                };
                break;
            case DialogueID.Monologue_Kitchen_Dumpster:
                currentDialogue = new List<Phrase> {
                    new Phrase(abraham, "Sheesh.", 1.5f, true),
                    new Phrase(abraham, "This guy was m e s s y.", 1.5f, true)
                };
                break;
            case DialogueID.Monologue_Kitchen_Wardrobe:
                currentDialogue = new List<Phrase> {
                    new Phrase(abraham, "A remarkable piece of furniture.", 1.5f, true),
                    new Phrase(abraham, "I wonder what's inside.", 1.5f, true),
                    new Phrase(abraham, "If this game was developed with more attention to detail", 2.5f, true),
                    new Phrase(abraham, "I might just have a peek.", 1.5f, true)
                };
                break;
            case DialogueID.Monologue_Kitchen_Joshua:
                currentDialogue = new List<Phrase> { new Phrase(abraham, "Peacefully sipping his scotch.", 1.5f, true) };
                break;
            case DialogueID.Monologue_Taking_Elevator_Not:
                currentDialogue = new List<Phrase> {
                    new Phrase(abraham, "Hmmm...", 1.5f, true),
                    new Phrase(abraham, "No.", 1, true),
                };
                break;
            case DialogueID.Dialogue_Joshua:
                if (!notebook.ContainsNote(NoteID.IfSuicideGun))
                {
                    currentDialogue = new List<Phrase> {
                    new Phrase(abraham, "How are you doing my good man?", 2.0f),
                    new Phrase(joshuaBraun, "I'm doing splendid, frriend.", 2.0f),
                    new Phrase(abraham, "I'm glad to hear it.", 2.0f),
                    new Phrase(abraham, "Mind telling me how you died?", 2.0f),
                    new Phrase(joshuaBraun, "I slipped and fell.", 1.5f),
                    new Phrase(joshuaBraun, "Wat do you sink?", 1.5f),
                    new Phrase(abraham, "Yea yea but what exactly happened?", 2.5f),
                    new Phrase(joshuaBraun, "Abraham.", 1.5f),
                    new Phrase(joshuaBraun, "I need you to be a bit more self-aware.", 3.0f),
                    new Phrase(joshuaBraun, "Sis conversation is entirely wissin your head.", 3.0f),
                    new Phrase(joshuaBraun, "I only have sis accent because you saw my german Personalausweis.", 3.0f),
                    new Phrase(abraham, "", 1.5f, true),
                    new Phrase(abraham, "Welp, worth a shot.", 2.5f, true)
                    };
                }
                else
                {
                    currentDialogue = new List<Phrase> {
                    new Phrase(abraham, "So you didn't do it yourself, huh?", 2.0f),
                    new Phrase(joshuaBraun, "Nein mann.", 1.0f),
                    new Phrase(joshuaBraun, "Someone else did", 1.5f),
                    new Phrase(joshuaBraun, "Can't tell you anything more", 1.5f),
                    new Phrase(joshuaBraun, "I'm just a projection of what you know.", 2.5f),
                    new Phrase(abraham, "Please try to stay more in character.", 2.0f),
                    };
                }
                break;
            case DialogueID.Dialogue_BusinessSven:
                if (notebook.ContainsCompromise(CompromiseID.SvenDidntChooseThisLife))
                {
                    currentDialogue = new List<Phrase> {
                        new Phrase(abraham, "Hello again.", 1.5f),
                        new Phrase(businessSven, "Abraham you know I'm only in your head.", 2.5f),
                        new Phrase(businessSven, "You've exhausted our creativity enough.", 2.5f),
                        new Phrase(businessSven, "Go bother some other part of your subconscience.", 2.5f),
                    };
                }
                else
                {
                    currentDialogue = new List<Phrase> {
                    new Phrase(abraham, "Good evening.", 2.0f),
                    new Phrase(businessSven, "Ah hello, can I help you?", 2.0f),
                    new Phrase(abraham, "I just wanted to chat while we wait.", 2.0f),
                    new Phrase(businessSven, "I'm really not one for smalltalk", 2.0f),
                    new Phrase(businessSven, "Sorry.", 1.0f),
                    new Phrase(abraham, "That's okay I'll do the heavy lifting.", 2.0f),
                    new Phrase(businessSven, "Sheesh, take a hint.", 2.0f, true),
                    new Phrase(businessSven, "", 1.0f, true),
                    new Phrase(abraham, "How come you're up so late?", 2.0f),
                    new Phrase(businessSven, "Work stuff.", 1.0f),
                    new Phrase(abraham, "It's 1:20 a.m.", 1.5f),
                    new Phrase(businessSven, "It's a 720h shift.", 1.5f),
                    new Phrase(businessSven, "I'm on a coffee break.", 1.5f),
                    new Phrase(abraham, "Must be tough.", 1.5f),
                    new Phrase(businessSven, "It's my dream job", 1.5f),
                    new Phrase(businessSven, "but it can be taxing.", 1.5f),
                    new Phrase(businessSven, "", 0.5f, true),
                    new Phrase(businessSven, "Honestly,", 1.5f),
                    new Phrase(businessSven, "I couldn't do it without my wife and daughter.", 2.5f),
                    new Phrase(businessSven, "Without them, I think I'd just give it all up.", 2.5f),
                    new Phrase(businessSven, "In this world,", 1.5f),
                    new Phrase(businessSven, "they never died in a car crash.", 2.5f),
                    new Phrase(abraham, "", 1.5f, true),
                    new Phrase(abraham, "oof.", 1.5f, true, () => { notebook.AddWhatIf(CompromiseID.SvenDidntChooseThisLife); return true; }),
                    new Phrase(abraham, "Can't really blame him for going off the rails.", 2.5f, true)
                };
                    talkedtosven = true;
                }
                break;
            case DialogueID.Monologue_Outside_BusinessSven:
                currentDialogue = new List<Phrase> {
                    new Phrase(abraham, "Italian suit.", 2.0f, true),
                    new Phrase(abraham, "pinstripes so fine I can't even see them at this resolution.", 2.5f, true),
                    new Phrase(abraham, "Custom tailored by the looks of it.", 2.0f, true),
                    new Phrase(businessSven, "...", 0.5f),
                    new Phrase(businessSven, "Is that guy staring at me?", 1.5f, true),
                };
                break;
            case DialogueID.Monologue_Outside_CleanHaltestelle:
                currentDialogue = new List<Phrase> {
                    new Phrase(abraham, "It's a sign for a bus stop.", 1.5f),
                    new Phrase(abraham, "I don't think there's any busses a this hour though", 2.5f),
                    new Phrase(businessSven, "Your negativity is upsetting.", 2.0f),
                };
                break;
            case DialogueID.Monologue_Outside_CrookedHaltestelle:
                currentDialogue = new List<Phrase> { new Phrase(abraham, "It's a sign for a bus stop.", 1.5f),
                                                     new Phrase(abraham, "It's seen better days.", 1.5f) };
                break;
            case DialogueID.Monologue_Outside_BomschSven:
                if (talkedtosven)
                {
                    currentDialogue = new List<Phrase> { new Phrase(abraham, "Poor guy.", 1.5f, true) };
                } else
                {
                    currentDialogue = new List<Phrase> { new Phrase(abraham, "Yuck.", 1.5f, true) };
                }
                break;
            case DialogueID.Dialogue_BomschSven:
                if (notebook.can_talk_to_sven)
                {
                    if (notebook.ContainsNote(NoteID.JoshuaCameWithCompany))
                    {
                        currentDialogue = new List<Phrase> {
                            new Phrase(abraham, "Hello again.", 1.5f),
                            new Phrase(bomschSven, "I told you everything I know.", 2.5f),
                            new Phrase(bomschSven, "You even scribbled everything down.", 2.5f),
                            new Phrase(bomschSven, "Go away, let me sleep.", 2.5f)
                        };
                    }
                    else
                    {
                        currentDialogue = new List<Phrase> { new Phrase(abraham, "Good Evening.", 1.5f),
                                                     new Phrase(bomschSven, "...", 1.5f),
                                                     new Phrase(bomschSven, "you talking to me?", 1.5f),
                                                     new Phrase(abraham, "I have some questions if you don't mind.", 1.5f),
                                                     new Phrase(bomschSven, "spare some change in return?", 1.5f),
                                                     new Phrase(abraham, "", 0.4f),
                                                     new Phrase(bomschSven, "Come on, you can do better than that.", 1.5f),
                                                     new Phrase(abraham, "", 0.4f),
                                                     new Phrase(bomschSven, "he he he", 1.5f),
                                                     new Phrase(bomschSven, "what do you want?", 1.5f),
                                                     new Phrase(abraham, "Detective Abraham, police.", 1.5f),
                                                     new Phrase(abraham, "I'm investigating a murder.", 1.5f),
                                                     new Phrase(abraham, "Have you noticed anyone leaving the building at night?", 2.5f),
                                                     new Phrase(bomschSven, "lemme think...", 2.5f),
                                                     new Phrase(bomschSven, "at 8 or something two people entered the building", 2.5f),
                                                     new Phrase(bomschSven, "one of them I know, he lives here.", 1.5f),
                                                     new Phrase(bomschSven, "Joshua.", 1.5f),
                                                     new Phrase(bomschSven, "didn't know the other guy.", 1.5f),
                                                     new Phrase(bomschSven, "and I know everyone who lives here.", 1.5f),
                                                     new Phrase(bomschSven, "ermm...", 1.5f),
                                                     new Phrase(bomschSven, "ah and someone left the building at 11.", 1.5f, false, () => { notebook.AddNote(NoteID.SomeoneLeftAt23); return true; }),
                                                     new Phrase(abraham, "Was it the guy who arrived earlier? With Joshua?", 3.0f),
                                                     new Phrase(bomschSven, "sorry, can't remember.", 1.5f),
                                                     new Phrase(bomschSven, "was half asleep..", 1.5f),
                                                     //new Phrase(bomschSven, "neither of them came out again.", 1.5f),
                                                     new Phrase(abraham, "But you remember it being 11 p.m?", 2.0f),
                                                     new Phrase(bomschSven, "listen boy", 1.0f),
                                                     new Phrase(bomschSven, "my head might be spinning with vodka,", 2.0f),
                                                     new Phrase(bomschSven, "but", 1.0f),
                                                     new Phrase(bomschSven, "", 2, true),
                                                     new Phrase(bomschSven, "...", 1, true),
                                                     new Phrase(bomschSven, "#", 3.0f, true),
                                                     new Phrase(abraham, "Seems like a trustworthy witness to me.", 2.5f, true,
                                                        () => {notebook.AddNote(NoteID.JoshuaCameWithCompany); return false; }) };
                    }
                }
                else
                {
                    currentDialogue = new List<Phrase> {new Phrase(abraham, "I can't.", 1.0f,true),
                                                        new Phrase(abraham, "He's filthy and smelly.", 1.5f, true),
                                                        new Phrase(abraham, "It's just too much.", 1.5f, true) };
                }
                break;
            case DialogueID.Monologue_Kitchen_OutWindow:
                currentDialogue = new List<Phrase> { new Phrase(abraham, "We're pretty high up.", 1.5f, true),
                                                     new Phrase(abraham, "Can barely tell through this dirty window though.", 2.5f, true)};
                break;
            case DialogueID.Monologue_Kitchen_CleanClock:
                currentDialogue = new List<Phrase> {    new Phrase(abraham, "It shows the correct time.", 1.5f, false),
                                                        new Phrase(abraham, "It's 1:20 a.m.", 1.5f, true,
                                                        () => { FindObjectOfType<ClockGame>().StartMingame(); return false; }) };
                break;
            case DialogueID.Monologue_Kitchen_DirtyClock:
                currentDialogue = new List<Phrase> { new Phrase(abraham, "This is interesting.", 1.5f, false,
                                                     () => { FindObjectOfType<ClockGame>().StartMingame(); return false; }) };
                break;
            case DialogueID.Monologue_Kitchen_Handgun:
                currentDialogue = new List<Phrase> { new Phrase(abraham, "It's a gun.", 1.0f),
                                                     new Phrase(abraham, "The barrel smells like whiskey.", 1.5f, true)};
                break;
            case DialogueID.Monologue_Kitchen_HandgunMissing:
                currentDialogue = new List<Phrase> { new Phrase(abraham, "There's no weapon!", 1.0f),
                                                     new Phrase(abraham, "If Joshua did this himself", 1.0f),
                                                     new Phrase(abraham, "It'd still be here.", 1.0f, false,
                                                        () => { FindObjectOfType<Notebook_Controller>().AddNote(NoteID.IfSuicideGun); return false; }),
                                                     new Phrase(abraham, "I need to tell Ben about this!", 1.5f, true)};
                break;
            case DialogueID.Monologue_Kitchen_WrongClock:
                currentDialogue = new List<Phrase> { new Phrase(abraham, "Joshua's clock is not only covered in brain,", 2.0f),
                                                     new Phrase(abraham, "It's also 1 hour too early.", 2.0f, false,
                                                        () => { notebook.AddNote(NoteID.Clock_Lazy);
                                                                notebook.AddWhatIf(CompromiseID.JohuasClockWasAccurate);
                                                                return false; }),
                                                     new Phrase(abraham, "Must have forgotten to change from winter time.", 2.0f)
                                                    };
                break;
            case DialogueID.Monologue_Kitchen_Corpse:
                currentDialogue = new List<Phrase> {new Phrase(abraham, "Shot in the face.", 1.5f),
                                                        new Phrase(abraham, "Point blank.", 1.5f),
                                                        new Phrase(abraham, "Either by himself or..", 1.5f) };
                break;
            default:
                //Debug.Log(id.DisplayName() + ": Dialogue Not yet implemented.");
                DialogueOver();
                return;
        }
        currentDialogue[0].Execute();
    }

    private bool talkedtosven = false;

    private void DialogueOver()
    {
        currentDialogue = null;
        cursorLock.locked = false;
        abraham.GetComponent<AbrahamAnimationController>().locked = false;
    }

    private void Update()
    {
        if (currentDialogue == null) return;
        if (currentDialogue[currentDialogueStep].guy.IsDoneTalking())
        {
            currentDialogue[currentDialogueStep].doneAction.Invoke();
            currentDialogueStep++;
            if (currentDialogueStep >= currentDialogue.Count)
            {
                DialogueOver();
            }
            else
            {
                currentDialogue[currentDialogueStep].Execute();
            }
        }
    }
}

public class Phrase
{
    public Func<bool> doneAction;

    public Phrase(Conversationalist conv, string phrase, float duration, bool thinking, Func<bool> actionWhenDone) { guy = conv; sentence = phrase; time = duration; thought = thinking; doneAction = actionWhenDone; }

    public Phrase(Conversationalist conv, string phrase, float duration, bool thinking) : this(conv, phrase, duration, thinking, () => {return false;}) { }

    public Phrase(Conversationalist conv, string phrase, float duration) : this(conv, phrase, duration, false) {}

    public void Execute()
    {
        if (thought)
        {
            guy.Think(sentence, time);
        } else
        {
            guy.Say(sentence, time);
        }
        
    }

    public Conversationalist guy;
    public string sentence;
    public float time;
    public bool thought;
}

public enum DialogueID
{
    Monologue_CrookedPainting = 0, 
    Monologue_AdmirePainting = 1,
    Monologue_ReadDoorbell01 = 2,
    Monologue_ReadDoorbell02 = 3,
    Monologue_ShoesReal = 4,
    Monologue_ShoesPerf = 5,
    Monologue_WallCrack = 6,
    Monologue_OfficerBen = 7,
    Monologue_Kitchen_ClearSky = 20,
    Monologue_Kitchen_NeatDumpster = 21,
    Monologue_Kitchen_Dumpster = 22,
    Monologue_Kitchen_Wardrobe = 23,

    Monologue_Kitchen_Joshua = 24,
    Monologue_Taking_Elevator_Not = 25,
    Monologue_Kitchen_OutWindow = 26,
    Monologue_Kitchen_CleanClock = 27,
    Monologue_Kitchen_DirtyClock = 28,
    Monologue_Kitchen_Handgun = 29,
    Monologue_Kitchen_HandgunMissing = 30,
    Monologue_Kitchen_WrongClock = 31,
    Monologue_Kitchen_Corpse = 32,


    Monologue_Outside_BusinessSven = 40,
    Monologue_Outside_BomschSven = 41,
    Monologue_Outside_CrookedHaltestelle = 42,
    Monologue_Outside_CleanHaltestelle = 43,

    Dialogue_Ben = 100,
    Dialogue_Joshua = 101,
    Dialogue_BusinessSven = 102,
    Dialogue_BomschSven = 103
} 