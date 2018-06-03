using System;
using System.Collections.Generic;
using Building;
using UnityEngine;

namespace Ui
{
    public class UiBuildingsInventory : MonoBehaviour
    {
        private UiBuildingItem _currentUiItem;
        private Action<BlockItem> _inventoryItemSelectedAction;
        private Action _inventoryItemUnselectedAction;
        private List<UiBuildingItem> _uiItems;
        private BlockList _blockList;

        [SerializeField]
        private UiBuildingItem _uiItemPrefab;

        internal void Awake()
        {
            _uiItems = new List<UiBuildingItem>();
            _blockList = FindObjectOfType<BlockList>();

            foreach (var item in _blockList.Items)
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

        public bool TrySelect(int index, out BlockItem item)
        {
            if (index >= _blockList.Items.Length || index < 0)
            {
                item = null;
                return false;
            }
            OnClick(_blockList.Items[index], _uiItems[index]);
            item = _blockList.Items[index];
            return true;
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
}
