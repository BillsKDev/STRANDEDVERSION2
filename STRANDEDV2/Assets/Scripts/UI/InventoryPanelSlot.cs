using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryPanelSlot : MonoBehaviour, 
    IPointerEnterHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IEndDragHandler,
    IDragHandler,
    IPointerClickHandler
{
    static InventoryPanelSlot Focused;

    ItemSlot _itemSlot;
    [SerializeField] Image _draggedItemIcon;
    [SerializeField] Image _itemIcon;
    [SerializeField] Outline _outline;
    [SerializeField] Color _draggingColor = Color.grey;
    [SerializeField] TMP_Text _stackCountText;
    [SerializeField] EquipmentSlotType _equipmentSlotType;

    public EquipmentSlotType EquipmentSlotType => _equipmentSlotType;

    public void Bind(ItemSlot itemSlot)
    {
        _itemSlot = itemSlot;
        _itemSlot.Changed += UpdateIconAndStackSize;

        UpdateIconAndStackSize();
    }

    void UpdateIconAndStackSize()
    {
        if (_itemSlot.Item != null)
        {
            _itemIcon.sprite = _itemSlot.Item.Icon;
            _itemIcon.enabled = true;
            _stackCountText.SetText(_itemSlot.StackCount.ToString());
            _stackCountText.enabled = _itemSlot.Item.MaxStackSize > 1;
        }
        else
        {
            _itemIcon.sprite = null;
            _itemIcon.enabled = false;
            _stackCountText.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Focused = this;
        _outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Focused == this)
            Focused = null;

        _outline.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_itemSlot.IsEmpty)
            return;

        _itemIcon.color = _draggingColor;
        _draggedItemIcon.sprite = _itemIcon.sprite;
        _draggedItemIcon.enabled = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Focused == null && Input.GetKey(KeyCode.LeftShift))
            Inventory.Instance.RemoveItemFromSlot(_itemSlot);

        if (_itemSlot.IsEmpty == false && Focused != null)
            Inventory.Instance.Swap(_itemSlot, Focused._itemSlot);

        _itemIcon.color = Color.white;
        _draggedItemIcon.sprite = null;
        _draggedItemIcon.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _draggedItemIcon.transform.position = eventData.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemTooltipPanel.Instance.ShowItem(_itemSlot); 
    }
}
