using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    const int GeneralSize = 9;
    const int CraftingSize = 9;

    public ItemSlot[] GeneralSlot = new ItemSlot[GeneralSize];
    public ItemSlot[] CraftingSlot = new ItemSlot[CraftingSize];
    public List<ItemSlot> EquipmentSlots = new List<ItemSlot>();

    List<SlotData> _slotDatas;
    [SerializeField] EquipmentSlotType[] _allEquipmentSlotTypes;
    
    [SerializeField] Item _debugItem;

    public static Inventory Instance { get; private set; }

#if UNITY_EDITOR
    void OnValidate() => _allEquipmentSlotTypes = Extensions.GetAllInstances<EquipmentSlotType>();
#endif

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < GeneralSize; i++)
            GeneralSlot[i] = new ItemSlot();
        for (int i = 0; i < CraftingSize; i++)
            CraftingSlot[i] = new ItemSlot();

        foreach (var slotType in _allEquipmentSlotTypes)
        {
            var slot = new ItemSlot(slotType);
            EquipmentSlots.Add(slot);
        }
    }

    bool AddItemToSlots(Item item, IEnumerable<ItemSlot> slots)
    {
        var stackableSlot = slots.FirstOrDefault(t => t.Item == item && t.HasStackSpaceAvailable);
        if (stackableSlot != null)
        {
            stackableSlot.ModifyStack(1);
            return true;
        }

        var slot = slots.FirstOrDefault(t => t.IsEmpty);
        if (slot != null)
        {
            slot.SetItem(item);
            return true;
        }
        return false;
    }

    public void AddItemFromEvent(Item item) => AddItem(item);

    public void AddItem(Item item, InventoryType preferredItemType = InventoryType.General)
    {
        var preferredSlots = preferredItemType == InventoryType.General ? GeneralSlot : CraftingSlot;
        var backupSlots = preferredItemType == InventoryType.General ? CraftingSlot : GeneralSlot;

        if (AddItemToSlots(item, preferredSlots))
            return;

        if (AddItemToSlots(item, backupSlots))
            return;
    }

    public void Bind(List<SlotData> slotDatas)
    {
        _slotDatas = slotDatas;

        BindToSlots(slotDatas, GeneralSlot, "General");
        BindToSlots(slotDatas, CraftingSlot, "Crafting");
        BindToSlots(slotDatas, EquipmentSlots, "Equipment");
    }

    static void BindToSlots(List<SlotData> slotDatas, IEnumerable<ItemSlot> slots, string slotName)
    {
        for (var i = 0; i < slots.Count(); i++)
        {
            var slot = slots.ElementAt(i);

            string name = slot.EquipmentSlotType != null ? slot.EquipmentSlotType.name : slotName + i;
            var slotData = slotDatas.FirstOrDefault(t => t.SlotName == name);

            if (slotData == null)
            {
                slotData = new SlotData() { SlotName = name };
                slotDatas.Add(slotData);
            }

            slot.Bind(slotData);
        }
    }

    public void ClearCraftingSlots()
    {
        foreach (var slot in CraftingSlot)
            slot.RemoveItem();
    }

    public void RemoveItemFromSlot(ItemSlot itemSlot)
    {
        itemSlot.RemoveItem();
    }

    public void Swap(ItemSlot sourceSlot, ItemSlot targetSlot)
    {
        if (!sourceSlot.CanHold(targetSlot.Item) || !targetSlot.CanHold(sourceSlot.Item))
            return;

        if (targetSlot != null && targetSlot.IsEmpty && Input.GetKey(KeyCode.LeftShift))
        {
            targetSlot.SetItem(sourceSlot.Item) ;
            sourceSlot.ModifyStack(-1);
        }
        else if (targetSlot != null && targetSlot.Item == sourceSlot.Item && targetSlot.HasStackSpaceAvailable)
        {
            int numberToMove = Mathf.Min(targetSlot.AvailableStackSpace, sourceSlot.StackCount);
            if (Input.GetKey(KeyCode.LeftShift) && numberToMove > 1)
                numberToMove = 1;
            targetSlot.ModifyStack(numberToMove);
            sourceSlot.ModifyStack(-numberToMove);
        }
        else
        sourceSlot.Swap(targetSlot);
    }
    
    public ItemSlot GetEquipmentSlot(EquipmentSlotType equipmentSlotType)
    {
        return EquipmentSlots.FirstOrDefault(t => t.EquipmentSlotType == equipmentSlotType);
    }
}

public enum InventoryType
{
    General,
    Crafting
}