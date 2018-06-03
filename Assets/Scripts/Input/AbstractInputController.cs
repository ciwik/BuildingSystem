using System;
using UnityEngine;

namespace InputSystem
{
    public abstract class AbstractInputController : MonoBehaviour, IInputController
    {
        private Action<Vector3, Quaternion> _moveAction;
        private Func<RaycastHit, bool> _raycastAction;
        private Action _buildAction;
        private Action _buildingCancelAction;
        private Action<BlockItem> _blockItemSelectAction;
        private Action _blockItemResetAction;

        private UiBuildingsInventory _buildingsInventory;

        private Vector2 _sensitivity = new Vector2(1f, 1f);
        private float _maxBuildingDistance = 30f;
        private bool _isBuilding;

        internal virtual void Awake()
        {
            _buildingsInventory = FindObjectOfType<UiBuildingsInventory>();
        }

        public IInputController WithMoveListener(Action<Vector3, Quaternion> moveAction)
        {
            _moveAction = moveAction;
            return this;
        }

        public IInputController WithBuildListeners(Action buildAction, Action buildingCancelAction)
        {
            _buildAction = buildAction + _buildingsInventory.Reset + StopBuilding;
            _buildingCancelAction = buildingCancelAction + _buildingsInventory.Reset + StopBuilding;
            return this;
        }

        public IInputController WithRaycastListener(Func<RaycastHit, bool> raycastAction)
        {
            _raycastAction = raycastAction;
            return this;
        }

        public IInputController WithItemSelectListeners(Action<BlockItem> blockItemSelectAction,
            Action blockItemResetAction)
        {
            _blockItemSelectAction = b =>
            {
                StartBuilding();
                blockItemSelectAction(b);
                OnItemSelected();
            };
            _blockItemResetAction = blockItemResetAction + StopBuilding;
            _buildingsInventory.WithInventoryItemListeners(_blockItemSelectAction, _blockItemResetAction);
            return this;
        }

        internal virtual void Update()
        {
            if (_isBuilding)
            {
                UpdateBuilding();
            }
            else
            {
                UpdateMove();
            }
        }

        protected abstract bool GetDirection(out Vector3 direction);
        protected abstract bool GetEulerAnglesRotation(out Vector2 eulerAnglesRotation);
        protected abstract Vector2 GetPositionOnScreen();
        protected abstract bool IsPressed();
        protected virtual void OnItemSelected() { }

        protected void CancelBuilding()
        {
            _buildingCancelAction();
        }

        protected void SelectItem(BlockItem item)
        {
            _blockItemSelectAction(item);
        }

        protected void SelectItem(int itemIndex)
        {
            SelectItem(_buildingsInventory.Select(itemIndex));
        }

        private void UpdateMove()
        {
            Vector3 direction;
            Vector2 eulerAnglesRotation;
            if (GetDirection(out direction) | GetEulerAnglesRotation(out eulerAnglesRotation))
            {
                Quaternion rotation = Quaternion.Euler(-eulerAnglesRotation.y * _sensitivity.y,
                    eulerAnglesRotation.x * _sensitivity.x,
                    0f);
                _moveAction(direction, rotation);
            }
        }

        private void UpdateBuilding()
        {
            RaycastHit raycastHit;
            var position = GetPositionOnScreen();
            var ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (Vector3.Distance(Camera.main.transform.position, raycastHit.point) < _maxBuildingDistance)
                {
                    if (_raycastAction(raycastHit) && IsPressed() && _isBuilding)
                    {
                        _buildAction();
                    }
                }
            }
        }

        private void StartBuilding()
        {
            _isBuilding = true;
        }

        private void StopBuilding()
        {
            _isBuilding = false;
        }
    }
}