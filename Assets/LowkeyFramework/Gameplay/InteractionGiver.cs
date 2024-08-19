using System.Collections.Generic;
using System.Linq;
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
        allCurrentInteractions.RemoveAll(interaction => interaction == null);

        if(CurrentClosestInteraction)
        {
            CurrentClosestInteraction.OnStoppedToBeTheCurrentInteraction.Invoke();
        }
        if(allCurrentInteractions.Count > 0)
        {
            int highestPrio = allCurrentInteractions.Max(interaction => interaction.Priority);
            CurrentClosestInteraction = allCurrentInteractions.Where(interaction => interaction.Priority == highestPrio).MinBy(interaction => Vector3.SqrMagnitude(interaction.transform.position - transform.position));
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
