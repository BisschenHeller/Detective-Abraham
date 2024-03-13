using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private InteractionType interactionType;
    [SerializeField]
    private string subtitle_prompt = "";
    
    [SerializeField]
    private bool aberrated = false;
    [SerializeField]
    private CompromiseID compromise;

    [SerializeField]
    private DialogueID dialogueID;


    public DialogueID GetDialogueID()
    {
        return dialogueID;
    }

    private bool acknowledged_aberration = false;

    public string GetSubtitlePrompt()
    {
        return subtitle_prompt;
    }

    public InteractionType GetInteractionType()
    {
        if (aberrated && !acknowledged_aberration)
        {
            if (Input.GetAxis("Aberrations_Linger") > 0 && BlinkingController.way_down)
            {
                return InteractionType.Aberration;
            }
        }
        return interactionType;
    }

    public CompromiseID GetCompromise()
    {
        if (!aberrated) Debug.LogError("Compromise Requested from non-aberrated object " + gameObject.name);
        return compromise;
    }
}

public enum InteractionInfo
{
    NONE = 0,
    
    start_dialogue_with_Joshua = 100,

    comment_on_clouds = 200,
    comment_on_corpse = 201,
    comment_on_dirty_kitchen = 202,
    comment_on_clear_sky = 203,
    comment_on_crooked_painting = 204,
    comment_on_painting = 205
}


