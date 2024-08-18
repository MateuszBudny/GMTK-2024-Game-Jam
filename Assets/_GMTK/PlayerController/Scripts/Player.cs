using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private InteractionGiver interactionGiver;

    public void OnInteraction(InputValue value)
    {
        if(value.isPressed)
        {
            interactionGiver.Interact();
        }
    }
}
