using System;
using Building;
using UnityEngine;

namespace InputSystem
{
    public interface IInputController
    {
        IInputController WithMoveListener(Action<Vector3, Quaternion> moveAction);        
        IInputController WithRaycastListener(Func<RaycastHit, bool> raycastAction);
        IInputController WithBuildListeners(Action buildAction, Action buildingCancelAction);
        IInputController WithItemSelectListeners(Action<BlockItem> blockItemSelectAction, Action blockItemResetAction);
    }
}