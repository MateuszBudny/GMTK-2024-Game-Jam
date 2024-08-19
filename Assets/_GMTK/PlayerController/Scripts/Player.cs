using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private InteractionGiver interactionGiver;
    [SerializeField]
    private InputActionReference primaryActionRef;
    [SerializeField]
    private InputActionReference secondaryActionRef;
    [SerializeField]
    private InputActionReference additionalActionRef;

    public void OnPrimaryInteraction(InputValue value)
    {
        if(value.isPressed)
        {
            interactionGiver.TryToInteract(primaryActionRef);
        }
    }

    public void OnSecondaryInteraction(InputValue value)
    {
        if(value.isPressed)
        {
            interactionGiver.TryToInteract(secondaryActionRef);
        }
    }

    public void OnAdditionalInteraction(InputValue value)
    {
        if(value.isPressed)
        {
            interactionGiver.TryToInteract(additionalActionRef);
        }
    }
}
