﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class UiBuildingsInventory : MonoBehaviour
{
    private UiBuildingItem _currentUiItem;
    private Action<BlockItem> _inventoryItemSelectedAction;
    private Action _inventoryItemUnselectedAction;
    private List<UiBuildingItem> _uiItems;
    private Blocks _blocks;

    [SerializeField]
    private UiBuildingItem _uiItemPrefab;

    internal void Awake()
    {
        _uiItems = new List<UiBuildingItem>();
        _blocks = FindObjectOfType<Blocks>();

        foreach (var item in _blocks.Items)
        {
            var uiItem = Instantiate(_uiItemPrefab, transform);
            uiItem.Init(item.Title, () => OnClick(item, uiItem));
            _uiItems.Add(uiItem);
        }
    }

    public void WithInventoryItemListeners(Action<BlockItem> inventoryItemSelectedAction, Action inventoryItemUnselectedAction)
    {
        _inventoryItemSelectedAction = inventoryItemSelectedAction;
        _inventoryItemUnselectedAction = inventoryItemUnselectedAction;
    }

    public void Reset()
    {
        _currentUiItem?.Unselect();
        _currentUiItem = null;
    }

    public BlockItem Select(int index)
    {
        if (index >= _uiItems.Count)
        {
            index = index % _uiItems.Count;
        }

        OnClick(_blocks.Items[index], _uiItems[index]);
        return _blocks.Items[index];
    }

    private void OnClick(BlockItem item, UiBuildingItem uiItem)
    {
        _currentUiItem?.Unselect();
        _inventoryItemUnselectedAction();

        _currentUiItem = uiItem;            
        _currentUiItem?.Select();
        _inventoryItemSelectedAction(item);        
    }
}
