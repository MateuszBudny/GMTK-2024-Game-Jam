using AetherEvents;
using TMPro;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField]
    private DialogueActorSO actor;
    [SerializeField]
    private TextMeshPro dialogueTMP;

    private void Awake()
    {
        dialogueTMP.text = "";
        DialogueRequested.AddListener(TryToPlayDialogue);
        DialogueFinished.AddListener(HideDialogue);
    }

    public void ShowDialogue(string dialogue)
    {
        dialogueTMP.text = dialogue + "\n[CTRL]";
        dialogueTMP.gameObject.SetActive(true);
    }

    public void HideDialogue(DialogueFinished context)
    {
        dialogueTMP.gameObject.SetActive(false);
    }

    private void TryToPlayDialogue(DialogueRequested context)
    {
        HideDialogue(null);

        if(context.dialogueRecord.actor.ID != actor.ID)
            return;

        ShowDialogue(context.dialogueRecord.dialogue);
    }
}
