using UnityEngine;

public class CarpetRepairStation : MonoBehaviour
{
    [SerializeField]
    private TakeableItemType carpetTakeableItemType;
    [SerializeField]
    private InteractionReceiver interactionReceiver;
    [TextArea]
    [SerializeField]
    private string isPossibleToPutCarpetInteractionInfoText;
    [TextArea]
    [SerializeField]
    private string isNotPossibleToPutCarpetInteractionInfoText;
    [SerializeField]
    private Transform carpetSlot;

    [SerializeField]
    private Carpet carpetInside;

    public bool CanCarpetBePutInside => PhysicalInventory.Instance.HasItemOfType(carpetTakeableItemType) && !carpetInside;

    private void Update()
    {
        if(CanCarpetBePutInside)
        {
            interactionReceiver.InteractionInfo = isPossibleToPutCarpetInteractionInfoText;
        }
        else
        {
            interactionReceiver.InteractionInfo = isNotPossibleToPutCarpetInteractionInfoText;
        }
    }

    public void Enter()
    {
        Debug.Log("enter");
    }

    public void TryToPutCarpet()
    {
        if(!CanCarpetBePutInside)
            return;

        carpetInside = PhysicalInventory.Instance.TryToTakeItemOfType(carpetTakeableItemType).GetComponent<Carpet>();
        carpetInside.transform.parent = carpetSlot;
        carpetInside.transform.SetPositionAndRotation(carpetSlot.position, carpetSlot.rotation);
        carpetInside.GetComponent<Rigidbody>().isKinematic = true;
        carpetInside.CarpetPutIntoRepairStation();
    }
}
