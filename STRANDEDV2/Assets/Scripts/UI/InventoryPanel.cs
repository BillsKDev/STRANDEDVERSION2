using System.Linq;
using UnityEngine;

public class InventoryPanel : ToggleablePanel
{
    void Start() => Bind(Inventory.Instance);

    public void Bind(Inventory inventory)
    {
        var panelSlots = GetComponentsInChildren<InventoryPanelSlot>();

        for (int i = 0; i < panelSlots.Length; i++)
        {
            panelSlots[i].Bind(inventory.GeneralSlot[i]);
        }
    }
}
