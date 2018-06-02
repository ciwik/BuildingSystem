using System;
using UnityEngine;

public class UiBuildingsInventory : MonoBehaviour
{
    private UiBuildingItem _currentUiItem;
    private Action<BuildingItem> _inventoryItemSelectedAction;
    private Action _inventoryItemUnselectedAction;

    [SerializeField]
    private UiBuildingItem _uiItemPrefab;

    public void Awake()
    {
        foreach (var item in FindObjectOfType<Buildings>().Items)
        {
            var uiItem = Instantiate(_uiItemPrefab, transform);
            uiItem.Init(item.Title, () => OnClick(item, uiItem));
        }
    }

    public void WithInventoryItemListeners(Action<BuildingItem> inventoryItemSelectedAction, Action inventoryItemUnselectedAction)
    {
        _inventoryItemSelectedAction = inventoryItemSelectedAction;
        _inventoryItemUnselectedAction = inventoryItemUnselectedAction;
    }

    public void Reset()
    {
        _currentUiItem?.Unselect();
        _currentUiItem = null;
    }

    private void OnClick(BuildingItem item, UiBuildingItem uiItem)
    {
        _currentUiItem?.Unselect();
        _inventoryItemUnselectedAction();

        _currentUiItem = uiItem;            
        _currentUiItem?.Select();
        _inventoryItemSelectedAction(item);        
    }
}
