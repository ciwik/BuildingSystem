using System;
using UnityEngine;

namespace InputSystem
{
    public interface IInputController
    {
        void WithMoveListener(Action<Vector3, Quaternion> moveAction);
        void WithBuildListener(Action buildAction);
        void WithRaycastingListener(Func<RaycastHit, bool> raycastAction);
        void StartBuilding();
        void StopBuilding();
    }
}