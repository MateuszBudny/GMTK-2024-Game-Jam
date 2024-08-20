using AetherEvents;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Lowkey Framework/Dialogue/SequenceSO", fileName = "_DialogueSequenceSO")]
public class DialogueSequenceSO : ScriptableObjectWithID
{
    [SerializeField]
    private List<DialogueRecord> dialogues;

    private Queue<DialogueRecord> dialoguesQueue;

    public void StartDialogue()
    {
        DialogueFinished.AddListener(TryToPlayNextDialogue);
        DialogueRequested.AddListener(CheckIfOtherDialogueIsPlaying);
        dialoguesQueue = new Queue<DialogueRecord>(dialogues);

        TryToPlayNextDialogue(null);
    }

    private void TryToPlayNextDialogue(DialogueFinished context)
    {
        if(dialoguesQueue.Count == 0)
        {
            StopDialogue();
            return;
        }

        new DialogueRequested(this, dialoguesQueue.Dequeue()).Invoke();
    }

    private void CheckIfOtherDialogueIsPlaying(DialogueRequested context)
    {
        if(context.fromSequence.ID == ID)
            return;

        StopDialogue();
    }

    private void StopDialogue()
    {
        DialogueFinished.RemoveListener(TryToPlayNextDialogue);
        DialogueRequested.RemoveListener(CheckIfOtherDialogueIsPlaying);
    }
}

[Serializable]
public class DialogueRecord
{
    public DialogueActorSO actor;
    public string dialogue;
}