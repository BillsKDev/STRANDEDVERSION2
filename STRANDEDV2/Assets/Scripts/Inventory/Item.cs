using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]

public class Item : ScriptableObject
{
    public EquipmentSlotType EquipmentSlotType;
    public Sprite Icon;
    public string ModelName;
    public string Description;
    public int MaxStackSize;
    public Placeable PlaceablePrefab;

    [ContextMenu("Add 1")]
    public void Add1() => Inventory.Instance.AddItem(this);
}
