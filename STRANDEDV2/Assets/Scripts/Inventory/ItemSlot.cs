using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    public ItemSlot() { }

    public ItemSlot(EquipmentSlotType equipmentSlotType)
    {
        EquipmentSlotType = equipmentSlotType;
    }

    public event Action Changed;
    public Item Item;
    SlotData _slotData;

    public bool IsEmpty => Item == null;

    public bool HasStackSpaceAvailable => _slotData.StackCount < Item.MaxStackSize;

    public int StackCount => _slotData.StackCount;

    public int AvailableStackSpace => Item != null ? Item.MaxStackSize * _slotData.StackCount : 0;
    
    public readonly EquipmentSlotType EquipmentSlotType;

    public void SetItem(Item item, int stackCount = 1)
    {
        Item = item;
        _slotData.ItemName = item?.name ?? string.Empty;
        _slotData.StackCount = stackCount;
        Changed?.Invoke();
    }

    public void Bind(SlotData slotData)
    {
        _slotData = slotData;
        var item = Resources.Load<Item>("Items/" + _slotData.ItemName);
        Item = item;
        Changed?.Invoke();
    }

    public void Swap(ItemSlot slotToSwapWith)
    {
        var itemInOtherSlot = slotToSwapWith.Item;
        int stackCountInOtherSlot = slotToSwapWith.StackCount;
        slotToSwapWith.SetItem(Item, StackCount);
        SetItem(itemInOtherSlot, stackCountInOtherSlot);
    }

    public void ModifyStack(int amount)
    {
        _slotData.StackCount += amount;
        if (_slotData.StackCount <= 0)
            SetItem(null);
        else
            Changed?.Invoke();
    }

    public void RemoveItem()
    {
        SetItem(null);
    }

    public bool CanHold(Item item)
    {
        if (item == null)
            return true;

        if (EquipmentSlotType != null && item.EquipmentSlotType != EquipmentSlotType)
            return false;

        return true;
    }
}

[Serializable]

public class SlotData
{
    public string SlotName;
    public string ItemName;
    public int StackCount;
}
