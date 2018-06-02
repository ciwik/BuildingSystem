using System;
using UnityEngine;

namespace InputSystem
{
    public interface IInputController
    {
        IInputController WithMoveListener(Action<Vector3, Quaternion> moveAction);        
        IInputController WithRaycastingListener(Func<RaycastHit, bool> raycastAction);
        IInputController WithBuildListeners(Action buildAction, Action buildingCancelAction);
        IInputController WithItemSelectListeners(Action<BlockItem> blockItemSelectAction, Action blockItemResetAction);
    }
}