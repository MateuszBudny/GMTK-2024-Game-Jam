using UnityEngine;

public class CarpetSellPoint : MonoBehaviour
{
    [SerializeField]
    private InteractionReceiver interactionReceiver;
    [TextArea]
    [SerializeField]
    private string canSellInteractionInfo = "[E] Sell Carpet";
    [SerializeField]
    private TakeableItemType carpetItemType;

    private string lastSellInteractionInfo = "";

    public void SetProperInteractionInfo()
    {
        if(PhysicalInventory.Instance.HasItemOfType(carpetItemType))
        {
            interactionReceiver.InteractionInfo = $"{canSellInteractionInfo} (${PhysicalInventory.Instance.PeekItemOfType(carpetItemType).GetComponent<Carpet>().SellPrice})";
        }
        else
        {
            interactionReceiver.InteractionInfo = lastSellInteractionInfo;
        }
    }

    public void TryToSellCarpet()
    {
        if(!PhysicalInventory.Instance.HasItemOfType(carpetItemType))
            return;

        Carpet carpet = PhysicalInventory.Instance.TryToTakeItemOfType(carpetItemType).GetComponent<Carpet>();
        lastSellInteractionInfo = $"+${carpet.SellPrice}";
        interactionReceiver.InteractionInfo = lastSellInteractionInfo;
        carpet.Sell();
    }
}
