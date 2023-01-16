using UnityEngine;

public class EquipmentPanel : ToggleablePanel
{
    void Start() => Bind(Inventory.Instance);

    public void Bind(Inventory inventory)
    {
        var panelSlots = GetComponentsInChildren<InventoryPanelSlot>();
        foreach (var panelSlot in panelSlots)
        {
            panelSlot.Bind(inventory.GetEquipmentSlot(panelSlot.EquipmentSlotType));
        }
    }
}