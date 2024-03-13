using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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



    [SerializeField]
    private MyCursor cursorLock;

    private List<Phrase> currentDialogue = null;
    private int currentDialogueStep = 0;

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
                currentDialogue = new List<Phrase>() {
                    new Phrase(abraham, "Ben's a good guy.", 1, true),
                    new Phrase(abraham, "I've worked with him for years.", 2, true),
                    new Phrase(abraham, "", 0.5f, true),
                    new Phrase(officerBen, "Hey Abe", 1, false),
                    new Phrase(officerBen, "did you see that guy who turns into a pickle?", 3, false),
                    new Phrase(officerBen, "Funniest shit I've ever seen.", 2, true)
                };
                break;
            case DialogueID.Dialogue_Ben:
                currentDialogue = new List<Phrase>()
                {
                    new Phrase(abraham, "It's hard to look at, isn't it?", 2, false),
                    new Phrase(officerBen, "Sure is.", 1, true),
                    new Phrase(abraham, "One of these days I'm going to quit.", 2, false),
                    new Phrase(officerBen, "mhm", 1.5f, true),
                    new Phrase(abraham, "It's tilted by like 10 degrees!", 2, false),
                    new Phrase(abraham, "", 1, true),
                    new Phrase(officerBen, "You serious?", 1.5f, false),
                    new Phrase(abraham, "", 2, true),
                    new Phrase(abraham, "Ah yes, that's also a terrible tragedy.", 2, true)
                };
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
                currentDialogue = new List<Phrase> {
                    new Phrase(abraham, "How are you doing my good man?", 2.0f),
                    new Phrase(joshuaBraun, "I'm doing splendid, friend.", 2.0f),
                    new Phrase(abraham, "I'm glad to hear it.", 2.0f),
                    new Phrase(abraham, "Mind telling me how you died?", 2.0f),
                    new Phrase(joshuaBraun, "I slipped and fell.", 1.5f),
                    new Phrase(joshuaBraun, "What do you think?", 1.5f),
                    new Phrase(abraham, "Yea yea but what exactly happened?", 2.5f),
                    new Phrase(joshuaBraun, "Abraham.", 1.5f),
                    new Phrase(joshuaBraun, "I can't tell you anything you don't already know.", 3.0f),
                    new Phrase(joshuaBraun, "This conversation is entirely within your head.", 3.0f),
                    new Phrase(joshuaBraun, "I only have this accent because you saw my german Personalausweis.", 3.0f),
                    new Phrase(abraham, "", 1.5f, true),
                    new Phrase(abraham, "Welp, worth a shot.", 2.5f, true)
                };
                break;
            case DialogueID.Dialogue_BusinessSven:
                currentDialogue = new List<Phrase> {
                    new Phrase(abraham, "Good evening.", 2.0f),
                    new Phrase(businessSven, "Ah hello, can I help you?", 2.0f),
                    new Phrase(abraham, "I just wanted to chat while we wait.", 2.0f),
                    new Phrase(businessSven, "I'm not really one for smalltalk", 2.0f),
                    new Phrase(businessSven, "Sorry.", 1.0f),
                    new Phrase(abraham, "That's okay I'll do the hard part.", 2.0f),
                    new Phrase(businessSven, "Sheesh, take a hint.", 1.0f, true),
                    new Phrase(abraham, "How come you're up so late?", 2.0f),
                    new Phrase(businessSven, "Work stuff.", 1.0f),
                    new Phrase(abraham, "It's 1:30 a.m.", 1.5f),
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
                    new Phrase(abraham, "", 1.5f)
                };
                break;
            case DialogueID.Monologue_Outside_BusinessSven:
                currentDialogue = new List<Phrase> {
                    new Phrase(abraham, "Italian suit.", 2.0f, true),
                    new Phrase(abraham, "pinstripes so fine I can't even see them at this resolution.", 2.0f, true),
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
                currentDialogue = new List<Phrase> { new Phrase(abraham, "I really don't want to talk to that guy.", 1.5f, true)
                };
                break;
            case DialogueID.Dialogue_BomschSven:
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
                                                     new Phrase(bomschSven, "at 8 or something three people entered the building", 2.5f),
                                                     new Phrase(bomschSven, "two of them I know, they live here.", 1.5f),
                                                     new Phrase(bomschSven, "Joshua and David.", 1.5f),
                                                     new Phrase(bomschSven, "didn't know the third guy.", 1.5f),
                                                     new Phrase(bomschSven, "none of them left the building since.", 1.5f),
                                                     new Phrase(abraham, "Are you sure Joshua was with them?", 2.5f),
                                                     new Phrase(bomschSven, "listen boy, my head might be spinning with vodka,", 2.0f),
                                                     new Phrase(bomschSven, "", 2.5f, true),
                                                     new Phrase(bomschSven, "#", 1.5f, true),
                                                     new Phrase(abraham, "Seems like a trustworthy witness to me.", 2.5f, true)
                };
                break;
            case DialogueID.Monologue_Kitchen_OutWindow:
                currentDialogue = new List<Phrase> { new Phrase(abraham, "We're pretty high up.", 1.5f, true),
                                                     new Phrase(abraham, "Can barely tell through this dirty window though.", 2.5f, true)};
                break;
            default:
                Debug.Log(id.DisplayName() + ": Dialogue Not yet implemented.");
                DialogueOver();
                return;
        }
        currentDialogue[0].Execute();
    }

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

    public Phrase(Conversationalist conv, string phrase, float duration, bool thinking, Func<bool> actionWhenDone)
    {
        guy = conv; sentence = phrase; time = duration; thought = thinking; doneAction = actionWhenDone;
    }

    public Phrase(Conversationalist conv, string phrase, float duration, bool thinking) : this(conv, phrase, duration, thinking, () => {return false;})
    {
        
    }

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
    
    Monologue_Outside_BusinessSven = 40,
    Monologue_Outside_BomschSven = 41,
    Monologue_Outside_CrookedHaltestelle = 42,
    Monologue_Outside_CleanHaltestelle = 43,

    Dialogue_Ben = 100,
    Dialogue_Joshua = 101,
    Dialogue_BusinessSven = 102,
    Dialogue_BomschSven = 103
} 