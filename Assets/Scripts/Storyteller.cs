using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Storyteller : MonoBehaviour
{
    public Conversationalist conv_OfficerBen;
    public BenController contr_OfficerBen;

    private StateMachine stateMachine;

    private void Start()
    {
        

        GameState endState = new GameState(null, () => { Debug.Log("End of the game.");  return 1; }, () => { return false; });
        
        GameState benWalksToTheKitchen = new GameState(endState, () =>
        {
            contr_OfficerBen.target_x = -0.2f;
            return 0;
        }, () => { return contr_OfficerBen.IsDoneWalking(); });
        GameState benSaysRightThisWay = new GameState(benWalksToTheKitchen, () =>
            {
                contr_OfficerBen.target_x = -2.35f;
                conv_OfficerBen.Say("Right this way.", 2);
                return 0;
            }, () => { return conv_OfficerBen.IsDoneTalking(); });
        GameState benSaysHello = new GameState(benSaysRightThisWay, () =>
        {
            contr_OfficerBen.target_x = -2.35f;
            conv_OfficerBen.Say("Ah there you are, Abe.", 3);
            return 0;
        }, () => { return conv_OfficerBen.IsDoneTalking(); });



        stateMachine = new StateMachine(benSaysHello);
        stateMachine.Start();
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