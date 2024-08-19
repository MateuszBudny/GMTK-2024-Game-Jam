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

    public CarpetRepairStation CarpetRepairStationItIsIn { get; set; }

    public void TryToTakeItem()
    {
        if(!PhysicalInventory.Instance.HasFreeSpace)
            return;

        PhysicalInventory.Instance.AddItem(takeableHandler);
        interactionReceiver.InteractionInfo = isInInventoryInteractionInfo;

        if(CarpetRepairStationItIsIn)
        {
            CarpetRepairStationItIsIn.CarpetRemoved();
            CarpetRepairStationItIsIn = null;
        }
    }

    public void TryToDropItem()
    {
        if(!takeableHandler.IsInAnyInventory)
            return;

        PhysicalInventory.Instance.RemoveItem(takeableHandler);
        interactionReceiver.InteractionInfo = isNotInInventoryInteractionInfo;
    }

    public void CarpetPutIntoRepairStation(CarpetRepairStation repairStation)
    {
        interactionReceiver.InteractionInfo = isNotInInventoryInteractionInfo;
        CarpetRepairStationItIsIn = repairStation;
    }
}
