using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemTooltipPanel : MonoBehaviour, IPointerClickHandler
{
    public static ItemTooltipPanel Instance { get; private set; }

    [SerializeField] TMP_Text _name;
    [SerializeField] TMP_Text _description;
    [SerializeField] Image _icon;
    [SerializeField] Button _placeButton;

    CanvasGroup _canvasGroup;
    ItemSlot _itemSlot;

    void Awake()
    {
        Instance = this;
        _canvasGroup = GetComponent<CanvasGroup>();
        Toggle(false);
        _placeButton.onClick.AddListener(TryPlace);
    }

    void TryPlace()
    {
        PlacementManager.Instance.BeginPlacement(_itemSlot);
        Toggle(false);
    }

    public void ShowItem(ItemSlot itemSlot)
    {
        _itemSlot = itemSlot;
        Item item = itemSlot.Item;
        if (item == null)
        {
            Toggle(false);
        }
        else
        {
            Toggle(true);
            _name.SetText(item.name);
            _description.SetText(item.Description);
            _icon.sprite = item.Icon;
            _placeButton.gameObject.SetActive(item.PlaceablePrefab != null);
        }
    }

    void Toggle(bool visible)
    {
        _canvasGroup.alpha = visible ? 1f : 0;
        _canvasGroup.interactable = visible;
        _canvasGroup.blocksRaycasts = visible;
    }

    public void OnPointerClick(PointerEventData eventData) => Toggle(false);
}
