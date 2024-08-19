using UnityEngine;

public class Carpet : MonoBehaviour
{
    [SerializeField]
    private TakeableItem takeableHandler;
    [SerializeField]
    private InteractionReceiver interactionReceiver;
    [TextArea]
    [SerializeField]
    private string isInInventoryInteractionInfo;
    [TextArea]
    [SerializeField]
    private string isNotInInventoryInteractionInfo;

    public void TryToTakeItem()
    {
        if(!PhysicalInventory.Instance.HasFreeSpace)
            return;

        PhysicalInventory.Instance.AddItem(takeableHandler);
        interactionReceiver.InteractionInfo = isInInventoryInteractionInfo;
    }

    public void TryToDropItem()
    {
        if(!takeableHandler.IsInAnyInventory)
            return;

        PhysicalInventory.Instance.RemoveItem(takeableHandler);
        interactionReceiver.InteractionInfo = isNotInInventoryInteractionInfo;
    }

    public void CarpetPutIntoRepairStation()
    {
        interactionReceiver.InteractionInfo = "";
    }
}
