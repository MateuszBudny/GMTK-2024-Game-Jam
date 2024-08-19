using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractionReceiver : MonoBehaviour
{
    [SerializeField]
    private int priority = 0;
    [SerializeField]
    private string interactionInfo;
    [SerializeField]
    private InteractionReceiverInfoText infoTextHandler;
    [SerializeField]
    private List<ActionInteractionRecord> interactions = new List<ActionInteractionRecord>();
    [SerializeField]
    private UnityEvent onStartedToBeTheCurrentInteraction;
    [SerializeField]
    private UnityEvent onStoppedToBeTheCurrentInteraction;

    public UnityEvent OnStartedToBeTheCurrentInteraction => onStartedToBeTheCurrentInteraction;
    public UnityEvent OnStoppedToBeTheCurrentInteraction => onStoppedToBeTheCurrentInteraction;

    public int Priority => priority;
    public string InteractionInfo { get => interactionInfo; set => interactionInfo = value; }

    private void Awake()
    {
        if(infoTextHandler)
        {
            onStartedToBeTheCurrentInteraction.AddListener(infoTextHandler.OnStartedBeingTheCurrentInteraction);
            onStoppedToBeTheCurrentInteraction.AddListener(infoTextHandler.OnStoppedBeingTheCurrentInteraction);
        }
    }

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