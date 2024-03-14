using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Storyteller : MonoBehaviour
{
    public Conversationalist conv_OfficerBen;
    public BenController contr_OfficerBen;

    public Conversationalist perf_abraham;
    public AbrahamAnimationController movement_abe;
    public Conversationalist joshua;
    public TutorialManager subtitle;

    public BlinkingController blinkingController;

    public AufzugController aufzug;

    public GameObject flur_parent;

    public Notebook_Controller notebook;

    public MyCursor cursorLock;

    private void MoveAbraham(float x)
    {
        perf_abraham.transform.localPosition = new Vector3(x, perf_abraham.transform.localPosition.y, perf_abraham.transform.localPosition.z);
    }

    private void MoveBen(float x)
    {
        conv_OfficerBen.transform.localPosition = new Vector3(x, conv_OfficerBen.transform.localPosition.y, conv_OfficerBen.transform.localPosition.z);
    }

    private StateMachine stateMachine;

    private void Start()
    {
        GameState endState = new GameState(null, () => { Debug.Log("End of the game."); return 1; }, () => { return false; });

        __PART01__(endState);
    }

    private GameState freeRoamBetween1and2;

    /* 
     * starts with abraham taking an elevator up Josuas Hosue. Ends with him in the kitchen, free to find out why joshua couldn't have killen himself.
     */
    private void __PART01__(GameState nextState)
    {
        freeRoamBetween1and2 = new GameState(nextState,
            () => { contr_OfficerBen.target_x = 0.58f; movement_abe.locked = false; return 0; },
            () => { return false; });

        GameState benBriefing07 = new GameState(freeRoamBetween1and2,
            () => { conv_OfficerBen.Say("That smell makes my head hurt", 2.5f); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState benBriefing06 = new GameState(benBriefing07,
            () => {
                conv_OfficerBen.Say("I'll be outside if you need me.", 2.5f);
                notebook.AddWhatIf(CompromiseID.JohuaCommittedSuicide);
                return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState benBriefing05 = new GameState(benBriefing06,
            () => { conv_OfficerBen.Say("Just do your job, look around or whatever", 3.0f); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState benBriefing04 = new GameState(benBriefing05,
            () => { conv_OfficerBen.Say("Probably offed himself by the looks of it.", 2.5f); notebook.AddWhatIf(CompromiseID.JohuaCommittedSuicide); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState benBriefing02 = new GameState(benBriefing04,
            () => { conv_OfficerBen.Say("Here, this is his ID.", 2.5f); notebook.AddNote(NoteID.Joshua_ID); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState benBriefing01 = new GameState(benBriefing02,
            () => { conv_OfficerBen.Say("Yup, nasty business, that.", 2.5f); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState monologue05 = new GameState(benBriefing01,
            () => { blinkingController.StopForce(); MakeAbrahamThink("And Joshua Braun was found dead with a hole in his forehead.", 4); return 0; },
            () => { return !abe_thinking; });

        GameState monologue04 = new GameState(monologue05,
            () => { MakeAbrahamThink("But alas, the night was cloudy.", 2); return 0; },
            () => { return !abe_thinking; });

        GameState monologue03 = new GameState(monologue04,
            () => { MakeAbrahamThink("and Joshua Braun was enjoying a glass of scotch by himself.", 4); return 0; },
            () => { return !abe_thinking; });

        GameState monologue02 = new GameState(monologue03,
            () => { MakeAbrahamThink("Today was a clear night", 1); return 0; },
            () => { return !abe_thinking; });

        GameState monologue01 = new GameState(monologue02,
            () => { movement_abe.force_right = false; movement_abe.locked = true; MakeAbrahamThink("In my perfect world", 1); return 0; },
            () => { return !abe_thinking; });

        GameState forceBlink = new GameState(monologue01,
            () => { subtitle.ClearTutorialText(); blinkingController.ForceBlink(); movement_abe.force_right = true; subtitle.ClearTutorialText(); return 0; },
            () => { return movement_abe.transform.localPosition.x >= -0.22f; });

        GameState followToKitchen = new GameState(forceBlink,
            () => { subtitle.SetTutorialText("Follow Ben to the kitchen."); movement_abe.locked = false; return 0; },
            () => { return movement_abe.transform.localPosition.x > -0.7f; });

        GameState transition3 = new GameState(followToKitchen,
            () => { subtitle.ClearTutorialText(); MakeAbrahamThink("", 1); return 0; },
            () => { return !abe_thinking; });

        GameState tutorial06 = new GameState(transition3,
            () => { MakeAbrahamThink("", 3); subtitle.SetTutorialText("If you press [Space] now, you'll see Abraham's perfect world was altered by your compromise."); return 0; },
            () => { return !abe_thinking && Input.GetAxis("Eyes_Closed") > 0.5f; });

        GameState transition2 = new GameState(tutorial06,
            () => { subtitle.ClearTutorialText(); MakeAbrahamThink("", 1); return 0; },
            () => { return !abe_thinking; });

        GameState tutorial05 = new GameState(transition2,
            () => { subtitle.SetTutorialText("Now tick the box in your notepad to alter the world in Abraham's mind."); return 0; },
            () => { return notebook.tutorial_image_comp; });

        GameState transition = new GameState(tutorial05,
            () => { subtitle.ClearTutorialText(); notebook.notebookHint.color = new Color(1, 1, 1, 0.2f); MakeAbrahamThink("", 1); return 0; },
            () => { return !abe_thinking; });

        GameState tutorial04 = new GameState(transition,
            () => { subtitle.SetTutorialText("Try to catch the aberration caused by the crooked image with your mouse.([Space] and release)"); return 0; },
            () => { return notebook.ContainsCompromise(CompromiseID.HangingPicturesIsHard); });

        GameState tutorial03 = new GameState(tutorial04,
            () => { cursorLock.locked = false; MakeAbrahamThink("", 4); subtitle.SetTutorialText("Use your mouse to interact with the world around you."); return 0; },
            () => { return !abe_thinking; });

        GameState tutorial02 = new GameState(tutorial03,
            () => { subtitle.ClearTutorialText(); MakeAbrahamThink("Ugh, gives me a headache every time.", 4); return 0; },
            () => { return !abe_thinking; });

        GameState tutorial01 = new GameState(tutorial02,
            () => { subtitle.SetTutorialText("Release [Space] to open your eyes again."); return 0; },
            () => { return Input.GetAxis("Aberrations_Linger") <= 0.5f; });

        GameState commentOnCrookedPicture = new GameState(tutorial01,
            () => { perf_abraham.Say("Is it so hard to hang a picture straight?", 3); subtitle.ClearTutorialText(); return 0; },
            () => { return perf_abraham.IsDoneTalking(); });

        GameState teachAboutSwitchingViews = new GameState(commentOnCrookedPicture,
            () => { movement_abe.locked = true; subtitle.SetTutorialText("Hold [Space] to imagine how perfect the world could be."); return 0; },
            () => { return blinkingController.GetEyesClosed() == 1; });

        GameState abeStartsTalkingToHimself = new GameState(teachAboutSwitchingViews,
            () => { MakeAbrahamThink("Ugh...", 1.5f); return 0; },
            () => { return movement_abe.transform.position.x >= -1.9; });

        GameState benWalksToTheKitchen = new GameState(abeStartsTalkingToHimself,
            () => { contr_OfficerBen.target_x = 1.25f; movement_abe.locked = false; return 0; },
            () => { return movement_abe.transform.position.x >= -2.2; });

        GameState benSaysRightThisWay = new GameState(benWalksToTheKitchen,
            () => { conv_OfficerBen.Say("Right this way.", 1.5f); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState benSaysHello = new GameState(benSaysRightThisWay,
            () => { subtitle.ClearTutorialText(); conv_OfficerBen.Say("Ah there you are, Abe.", 2); movement_abe.locked = true; return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState abeLeavesElevator = new GameState(benSaysHello,
            () => { movement_abe.locked = false; subtitle.SetTutorialText("Use [WASD] to walk"); return 0; },
            () => { return perf_abraham.transform.localPosition.x >= -2.45f; });

        GameState elevatorRide04 = new GameState(abeLeavesElevator,
            () => { flur_parent.transform.localPosition = new Vector3(-1.62f, Mathf.Lerp(-0.04f, 0.86f, 1 - Mathf.Sin(Mathf.PI * (1 - aufzug.speed) / 2)), 0); return 0; },
            () => { MakeAbrahamThink("Oh how I wish to live in that world...", 3); return 0; },
            () => { return aufzug.arrived; });

        GameState elevatorRide03 = new GameState(elevatorRide04,
            () => { MakeAbrahamThink("Nobody ever rides a dirty elevator.", 3); return 0; },
            () => { return !abe_thinking; });

        GameState elevatorRide02 = new GameState(elevatorRide03,
            () => { MakeAbrahamThink("In my perfect world", 2); return 0; },
            () => { return !abe_thinking; });

        GameState elevatorRide01 = new GameState(elevatorRide02,
            () => { MakeAbrahamThink("", 3); cursorLock.locked = true; MoveAbraham(-2.85f); MoveBen(-0.14f); contr_OfficerBen.target_x = -0.14f; contr_OfficerBen.phone_out = true; movement_abe.locked = true; return 0; },
            () => { return !abe_thinking; });

        stateMachine = new StateMachine(elevatorRide01);
        stateMachine.Start();
    }

    public List<GameObject> obenWardrobe_Window;
    public List<GameObject> closedWardrobe_Window;

    public void Showdown()
    {
        GameState endState = new GameState(null, () => { SceneManager.LoadScene("EndScene"); return 1; }, () => { return false; });

        GameState finalWords8 = new GameState(endState,
            () => { blinkingController.StopForce(); perf_abraham.Say("What would I see if I looked down that window?", 2); return 0; },
            () => { return !abe_thinking; });

        GameState finalWords7 = new GameState(finalWords8,
            () => { perf_abraham.Say("In my perfect world, where everything is just", 2); return 0; },
            () => { return !abe_thinking; });

        GameState finalWords6 = new GameState(finalWords7,
            () => { perf_abraham.Say("Fate took the killer's life as well.", 1); return 0; },
            () => { return !abe_thinking; });

        GameState finalWords5 = new GameState(finalWords6,
            () => { perf_abraham.Say("And in a rush of irony", 1); return 0; },
            () => { return !abe_thinking; });

        GameState finalWords4 = new GameState(finalWords5,
            () => { perf_abraham.Say("but fate plays her pranks sometimes.", 2); return 0; },
            () => { return !abe_thinking; });

        GameState finalWords3 = new GameState(finalWords4,
            () => { perf_abraham.Say("this could have been a nice saturday for joshua.", 2); return 0; },
            () => { return !abe_thinking; });

        GameState finalWords2 = new GameState(finalWords3,
            () => { perf_abraham.Say("In my perfect world", 1); return 0; },
            () => { return !abe_thinking; });

        GameState finalWords = new GameState(finalWords2, 
            () => { blinkingController.ForceBlink(); notebook.DeactivateCompromises(); MakeAbrahamThink("", 1); return 0; }, 
            () => { return !abe_thinking; });

        GameState care = new GameState(finalWords,
            () => { conv_OfficerBen.Say("So that problem took care of itself.", 2); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState outwindow = new GameState(care,
            () => { conv_OfficerBen.Say("He went out of the window.", 2); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState holyshit = new GameState(outwindow,
            () => { conv_OfficerBen.Say("Holy shit.", 2); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState freeze = new GameState(holyshit,
            () => {
                contr_OfficerBen.GetComponent<BenController>().target_x = 1.36f;
                
                conv_OfficerBen.Say("HEY! FREEZE!", 1.5f);
                joshua.Say("CRASH!",1);
                movement_abe.force_right = true;
                obenWardrobe_Window.ForEach(a => a.GetComponent<SpriteRenderer>().enabled = true);
                closedWardrobe_Window.ForEach(a => a.GetComponent<SpriteRenderer>().enabled = false);
                
                return 0; },
            () => { return movement_abe.transform.localPosition.x >= -0.56f; }
            );

        stateMachine = new StateMachine(freeze);
        stateMachine.Start();
    }

    private bool abe_thinking = false;

    private void MakeAbrahamThink(string thought, float seconds)
    {
        abe_thinking = true;
        perf_abraham.textField.text = thought;
        Invoke("StopAbrahamsThought", seconds);
    }

    private void StopAbrahamsThought()
    {
        Invoke("EndThinkingBreak", 1);
        perf_abraham.textField.text = "";
    }

    private void EndThinkingBreak()
    {
        abe_thinking = false;
    }

    private void Update()
    {
        stateMachine.Update();
    }
}

public class StateMachine
{
    public StateMachine(GameState initialState)
    {
        currentState = initialState;
    }

    private GameState currentState;

    public void Start()
    {
        currentState.on_entry();
    }

    public void Update()
    {
        currentState.Update();
        if (currentState.exit_condition())
        {
            currentState = currentState.next;
            currentState.on_entry();
        }
    }
}

public class GameState
{
    public GameState(GameState nextState, Func<int> entry, Func<bool> exit)
    {
        next = nextState;
        on_entry = entry;
        exit_condition = exit;
        update = () => { return 0; };
    }

    public GameState(GameState nextState, Func<int> every_frame, Func<int> entry, Func<bool> exit)
    {
        next = nextState;
        on_entry = entry;
        exit_condition = exit;
        update = every_frame;
    }

    public GameState next;

    public Func<bool> exit_condition;

    public Func<int> on_entry;
    
    public Func<int> update;

    public void Start()
    {
        on_entry();
    }

    public void Update()
    {
        update();
    }
}