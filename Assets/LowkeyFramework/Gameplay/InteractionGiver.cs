using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionGiver : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro currentInteractionInfo;

    private List<InteractionReceiver> allCurrentInteractions = new List<InteractionReceiver>();
    public InteractionReceiver CurrentClosestInteraction { get; private set; }

    private void Update()
    {
        if(CurrentClosestInteraction)
        {
            CurrentClosestInteraction.OnStoppedToBeTheCurrentInteraction.Invoke();
        }
        if(allCurrentInteractions.Count > 0)
        {
            CurrentClosestInteraction = allCurrentInteractions.MinBy(interaction => Vector3.SqrMagnitude(interaction.transform.position - transform.position));
            CurrentClosestInteraction.OnStartedToBeTheCurrentInteraction.Invoke();
        }
        else
        {
            CurrentClosestInteraction = null;
        }

        if(currentInteractionInfo)
        {
            currentInteractionInfo.text = CurrentClosestInteraction == null ? "" : CurrentClosestInteraction.InteractionInfo;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractionReceiver receiver = other.GetComponentInParent<InteractionReceiver>();
        if(receiver)
        {
            if(!allCurrentInteractions.Contains(receiver))
            {
                allCurrentInteractions.Add(receiver);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractionReceiver receiver = other.GetComponentInParent<InteractionReceiver>();
        if(receiver)
        {
            allCurrentInteractions.Remove(receiver);
        }
    }

    public void TryToInteract(InputActionReference actionRef)
    {
        if(!CurrentClosestInteraction)
            return;

        CurrentClosestInteraction.TryToInteract(actionRef);
    }
}
