using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractionReceiver : MonoBehaviour
{

    [SerializeField]
    private string interactionInfo;
    [SerializeField]
    private List<ActionInteractionRecord> interactions = new List<ActionInteractionRecord>();
    [SerializeField]
    private UnityEvent onStartedToBeTheCurrentInteraction;
    [SerializeField]
    private UnityEvent onStoppedToBeTheCurrentInteraction;

    public UnityEvent OnStartedToBeTheCurrentInteraction => onStartedToBeTheCurrentInteraction;
    public UnityEvent OnStoppedToBeTheCurrentInteraction => onStoppedToBeTheCurrentInteraction;

    public string InteractionInfo { get => interactionInfo; set => interactionInfo = value; }

    public void TryToInteract(InputActionReference actionRef)
    {
        ActionInteractionRecord record = interactions.FirstOrDefault(record => record.interactionInputAction == actionRef);
        if(record == null)
            return;

        record.onInteract.Invoke();
    }

    [Serializable]
    public class ActionInteractionRecord
    {
        public InputActionReference interactionInputAction;
        public UnityEvent onInteract;
    }
}