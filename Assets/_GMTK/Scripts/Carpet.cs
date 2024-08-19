using System.Collections.Generic;
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

    [SerializeField] public Pattern startingPattern;
    [SerializeField] public Pattern wantedPattern;
    [SerializeField] public List<Pattern> usablePatterns;

    public CarpetRepairStation CarpetRepairStationItIsIn { get; set; }
    public int SellPrice { get; set; } = 5;

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

    public void Sell()
    {
        PlayerWallet.Instance.Money += SellPrice;
        Destroy(gameObject);
    }
}
