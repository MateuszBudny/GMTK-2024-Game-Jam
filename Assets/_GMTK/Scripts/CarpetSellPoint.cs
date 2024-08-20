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

    private bool CanSell => PhysicalInventory.Instance.HasItemOfType(carpetItemType) && PhysicalInventory.Instance.PeekItemOfType(carpetItemType).GetComponent<Carpet>().IsFinished;

    private string lastSellInteractionInfo = "";

    public void SetProperInteractionInfo()
    {
        if(CanSell)
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
        if(!CanSell)
            return;

        Carpet carpet = PhysicalInventory.Instance.TryToTakeItemOfType(carpetItemType).GetComponent<Carpet>();
        lastSellInteractionInfo = $"Last sell:\n+${carpet.SellPrice}";
        interactionReceiver.InteractionInfo = lastSellInteractionInfo;
        carpet.Sell();
    }
}
