using UnityEngine;
using UnityEngine.Events;

public class InteractionReceiver : MonoBehaviour
{
    [SerializeField]
    private string interactionInfo;
    [SerializeField]
    private UnityEvent onInteract;
    [SerializeField]
    private UnityEvent onStartedToBeTheCurrentInteraction;
    [SerializeField]
    private UnityEvent onStoppedToBeTheCurrentInteraction;

    public UnityEvent OnStartedToBeTheCurrentInteraction => onStartedToBeTheCurrentInteraction;
    public UnityEvent OnStoppedToBeTheCurrentInteraction => onStoppedToBeTheCurrentInteraction;

    public string InteractionInfo { get => interactionInfo; set => interactionInfo = value; }

    public void Interact()
    {
        onInteract.Invoke();
    }
}
