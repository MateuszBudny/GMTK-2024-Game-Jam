using AetherEvents;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Lowkey Framework/Dialogue/SequenceSO", fileName = "_DialogueSequenceSO")]
public class DialogueSequenceSO : ScriptableObject
{
    [SerializeField]
    private List<DialogueRecord> dialogues;

    private Queue<DialogueRecord> dialoguesQueue;

    public void StartDialogue()
    {
        DialogueFinished.AddListener(TryToPlayNextDialogue);
        dialoguesQueue = new Queue<DialogueRecord>(dialogues);

        TryToPlayNextDialogue(null);
    }

    private void TryToPlayNextDialogue(DialogueFinished context)
    {
        if(dialoguesQueue.Count == 0)
        {
            DialogueFinished.RemoveListener(TryToPlayNextDialogue);
            return;
        }

        new DialogueRequested(dialoguesQueue.Dequeue()).Invoke();
    }
}

[Serializable]
public class DialogueRecord
{
    public DialogueActorSO actor;
    public string dialogue;
}