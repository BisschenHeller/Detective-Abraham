using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Storyteller : MonoBehaviour
{
    public Conversationalist conv_OfficerBen;
    public BenController contr_OfficerBen;

    public Conversationalist perf_abraham;
    public AbrahamAnimationController movement_abe;

    public TutorialManager subtitle;

    public BlinkingController blinkingController;

    public AufzugController aufzug;

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
        GameState endState = new GameState(null, () => { Debug.Log("End of the game.");  return 1; }, () => { return false; });

        GameState freeRoam = new GameState(endState,
            () => { contr_OfficerBen.target_x = -1.0f; movement_abe.locked = false; return 0; },
            () => { return false; });

        GameState benBriefing06 = new GameState(freeRoam,
            () => { conv_OfficerBen.Say("I'll be outside if you need me.", 2.5f); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState benBriefing05 = new GameState(benBriefing06,
            () => { conv_OfficerBen.Say("but I'll let you figure out the hole in that theory yourself.", 3.5f); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState benBriefing04 = new GameState(benBriefing05,
            () => { conv_OfficerBen.Say("See most of us think he did it himself", 2.5f); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        /*GameState benBriefing03 = new GameState(benBriefing04,
            () => { conv_OfficerBen.Say("Joshua Braun, 24 Years old.", 2.5f); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });*/

        GameState benBriefing02 = new GameState(benBriefing04,
            () => { conv_OfficerBen.Say("Here, this is his ID.", 2.5f); return 0; },
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
            () => { blinkingController.ForceBlink(); movement_abe.force_right = true; subtitle.ClearTutorialText(); return 0; },
            () => { return movement_abe.transform.localPosition.x >= -0.2f; });

        GameState takeControlBack = new GameState(forceBlink,
            () => { movement_abe.locked = false; subtitle.SetTutorialText("Use your mouse to interact with the world"); return 0; },
            () => { return movement_abe.transform.position.x >= -0.8; });

        GameState commentOnCrookedPicture = new GameState(takeControlBack,
            () => { perf_abraham.Say("Is it so hard to hang a picture straight?", 3); subtitle.ClearTutorialText(); return 0; },
            () => { return perf_abraham.IsDoneTalking(); });

        GameState teachAboutSwitchingViews = new GameState(commentOnCrookedPicture,
            () => { movement_abe.locked = true; subtitle.SetTutorialText("Hold [Space] to imagine how perfect the world could be."); return 0; },
            () => { return blinkingController.GetEyesClosed() == 1; });

        GameState abeStartsTalkingToHimself = new GameState(teachAboutSwitchingViews,
            () => { MakeAbrahamThink("Ugh...", 1.5f); return 0; },
            () => { return movement_abe.transform.position.x >= -1.9; });

        GameState benWalksToTheKitchen = new GameState(abeStartsTalkingToHimself,
            () => { contr_OfficerBen.target_x = -0.2f; movement_abe.locked = false; return 0; },
            () => { return movement_abe.transform.position.x >= -2.2; });

        GameState benSaysRightThisWay = new GameState(benWalksToTheKitchen,
            () => { contr_OfficerBen.target_x = -1.85f; conv_OfficerBen.Say("Right this way.", 2); return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState benSaysHello = new GameState(benSaysRightThisWay,
            () => { contr_OfficerBen.target_x = -1.85f; subtitle.ClearTutorialText(); conv_OfficerBen.Say("Ah there you are, Abe.", 3); movement_abe.locked = true;  return 0; },
            () => { return conv_OfficerBen.IsDoneTalking(); });

        GameState abeLeavesElevator = new GameState(benSaysHello,
            () => { movement_abe.locked = false; contr_OfficerBen.phone_out = true; MoveBen(-1.85f);  subtitle.SetTutorialText("Use [WASD] to walk"); return 0; },
            () => { return perf_abraham.transform.localPosition.x >= -2.45f; });

        GameState elevatorRide04 = new GameState(abeLeavesElevator,
            () => { MakeAbrahamThink("It's a beautiful place...", 2); return 0; },
            () => { return aufzug.arrived; });

        GameState elevatorRide03 = new GameState(elevatorRide04,
            () => { MakeAbrahamThink("Nobody ever rides a dirty elevator.", 3); return 0; },
            () => { return !abe_thinking; });

        GameState elevatorRide02 = new GameState(elevatorRide03,
            () => { MakeAbrahamThink("In my perfect world", 2); return 0; },
            () => { return !abe_thinking; });

        GameState elevatorRide01 = new GameState(elevatorRide02,
            () => { MakeAbrahamThink("", 2); MoveAbraham(-2.9f); movement_abe.locked = true; return 0; },
            () => { return !abe_thinking; });

        stateMachine = new StateMachine(elevatorRide01);
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
    }

    public GameState next;

    public Func<bool> exit_condition;

    public Func<int> on_entry;

    public void Start()
    {
        on_entry();
    }

    public void Update()
    {

    }
}