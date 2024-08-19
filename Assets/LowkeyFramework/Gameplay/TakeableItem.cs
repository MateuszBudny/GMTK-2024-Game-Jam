using UnityEngine;

public class TakeableItem : MonoBehaviour
{
    [SerializeField]
    private TakeableItemType type;

    public TakeableItemType Type => type;
    public PhysicalInventory InventoryItIsIn { get; set; }
    public bool IsInAnyInventory => InventoryItIsIn;
}
