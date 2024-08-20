using Aether;

namespace AetherEvents
{
    public class DialogueRequested : Event<DialogueRequested>
    {
        public readonly DialogueSequenceSO fromSequence;
        public readonly DialogueRecord dialogueRecord;

        public DialogueRequested(DialogueSequenceSO fromSequence, DialogueRecord dialogueRecord)
        {
            this.fromSequence = fromSequence;
            this.dialogueRecord = dialogueRecord;
        }
    }
}