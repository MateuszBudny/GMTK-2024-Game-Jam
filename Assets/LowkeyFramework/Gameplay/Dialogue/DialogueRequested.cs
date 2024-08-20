using Aether;

namespace AetherEvents
{
    public class DialogueRequested : Event<DialogueRequested>
    {
        public readonly DialogueRecord dialogueRecord;

        public DialogueRequested(DialogueRecord dialogueRecord)
        {
            this.dialogueRecord = dialogueRecord;
        }
    }
}