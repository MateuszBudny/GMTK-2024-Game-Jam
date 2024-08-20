using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicalInventory : SingleBehaviour<PhysicalInventory>
{
    [SerializeField]
    private List<PhysicalItemSlot> slots;

    public List<TakeableItem> Items => slots.Select(slot => slot.item).Where(item => item).ToList();
    public bool HasFreeSpace => slots.Any(slot => slot.item == null);

    public bool HasItem(TakeableItem item) => slots.Any(slot => slot.item == item);
    public bool HasItemOfType(TakeableItemType type) => Items.Any(item => item.Type == type);
    public TakeableItem PeekItemOfType(TakeableItemType type) => Items.FirstOrDefault(item => item.Type == type);

    public void AddItem(TakeableItem item)
    {
        PhysicalItemSlot firstEmptySlot = slots.FirstOrDefault(slot => slot.item == null);
        if(firstEmptySlot == null)
        {
            throw new Exception("Inventory is full and you are trying to add another item!");
        }

        firstEmptySlot.item = item;
        item.transform.parent = firstEmptySlot.slotTransform;
        item.transform.position = firstEmptySlot.slotTransform.position;
        item.transform.SetPositionAndRotation(firstEmptySlot.slotTransform.position, firstEmptySlot.slotTransform.rotation);
        item.InventoryItIsIn = this;
        if(item.TryGetComponent(out Rigidbody rigid))
        {
            rigid.isKinematic = true;
        }

        SoundManager.Instance.Play(Audio.PickUp);
    }

    public TakeableItem TryToTakeItemOfType(TakeableItemType type)
    {
        if(!HasItemOfType(type))
            return null;

        TakeableItem item = Items.First(item => item.Type == type);
        RemoveItem(item);

        return item;
    }

    public void RemoveItem(TakeableItem item)
    {
        item.transform.parent = null;
        item.InventoryItIsIn = null;
        if(item.TryGetComponent(out Rigidbody rigid))
        {
            rigid.isKinematic = false;
        }

        PhysicalItemSlot slot = slots.First(slot => slot.item == item);
        slot.item = null;

        SoundManager.Instance.Play(Audio.Drop);
    }

    [Serializable]
    public class PhysicalItemSlot
    {
        public Transform slotTransform;
        public TakeableItem item;
    }
}
