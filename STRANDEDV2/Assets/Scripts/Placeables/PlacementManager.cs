using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] float _rotateRate = 1000f;
    List<PlaceableData> _placeableDatas;
    [SerializeField] List<Placeable> _allPlaceables;

    ItemSlot _itemSlot;
    Placeable _placeable;
    public static PlacementManager Instance { get; private set; }

    private void Awake() => Instance = this;

    public void BeginPlacement(ItemSlot itemSlot)
    {
        _itemSlot = itemSlot;

        _placeable = Instantiate(_itemSlot.Item.PlaceablePrefab);
        _placeable.transform.SetParent(transform);
    }

    void Update()
    {
        if (_placeable == null) return;

        var rotation = -Input.mouseScrollDelta.y * Time.deltaTime * _rotateRate;
        _placeable.transform.Rotate(0, rotation, 0);

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, _layerMask, QueryTriggerInteraction.Ignore))
        {
            _placeable.transform.position = hitInfo.point;
            if (Input.GetMouseButton(0))
                FinishPlacement();
        }
    }

    void FinishPlacement()
    {
        _placeableDatas.Add(new PlaceableData()
        {
            PlaceablePrefab = _itemSlot.Item.PlaceablePrefab.name,
            Position = _placeable.transform.position,
            Rotation = _placeable.transform.rotation
        });

        _placeable.Place();
        _placeable = null;
        _itemSlot.RemoveItem();
        _itemSlot = null;
    }

    public void Bind(List<PlaceableData> placeableDatas)
    {
        _placeableDatas = placeableDatas;

        foreach (var placeableData in _placeableDatas)
        {
            var prefab = _allPlaceables.FirstOrDefault(t => t.name == placeableData.PlaceablePrefab);
            
            if (prefab != null)
            {
                var placeable = Instantiate(prefab, placeableData.Position, placeableData.Rotation);
                if (placeable != null)
                    placeable.Place();
            }
        }
    }
}
