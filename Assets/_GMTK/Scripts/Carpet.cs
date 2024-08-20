using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private Material repairedMaterial;

    [SerializeField] public Pattern startingPattern;
    [SerializeField] public Pattern wantedPattern;
    [SerializeField] public List<Pattern> usablePatterns;

    public TakeableItem TakeableHandler => takeableHandler;

    public CarpetRepairStation CarpetRepairStationItIsIn { get; set; }
    public int SellPrice { get; set; } = 5;
    public bool IsFinished
    {
        get => isFinished;
        set
        {
            isFinished = value;
            List<Material> materialsToSet = meshRenderer.materials.ToList();
            materialsToSet[0] = repairedMaterial;
            meshRenderer.SetMaterials(materialsToSet);
        }
    }

    private bool isFinished;

    public void Interact()
    {
        if(PhysicalInventory.Instance.HasItem(takeableHandler))
        {
            TryToDropItem();
        }
        else
        {
            TryToTakeItem();
        }
    }

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

    private void TryToDropItem()
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
