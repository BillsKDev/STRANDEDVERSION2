using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentVisuals : MonoBehaviour
{
    public List<EquipmentVisual> Visuals;

    void Start()
    {
        foreach (var slot in Inventory.Instance.EquipmentSlots)
        {
            slot.Changed += () => UpdateEquipmentVisuals(slot);
            UpdateEquipmentVisuals(slot);
        }
    }

    void UpdateEquipmentVisuals(ItemSlot slot)
    {
        foreach (var visual in Visuals.Where(t => t.EquipmentSlotType == slot.EquipmentSlotType))
        {
            if (visual.VisualModelRoot != null)
            {
                for (int i = 0; i < visual.VisualModelRoot.childCount; i++)
                {
                    var model = visual.VisualModelRoot.GetChild(i);
                    model.gameObject.SetActive(model.name == slot.Item?.ModelName);
                }
            }
        }
    }
}

[Serializable]
public class EquipmentVisual
{
    public EquipmentSlotType EquipmentSlotType;
    public Transform VisualModelRoot;
}