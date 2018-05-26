using System;
using UnityEngine;

namespace InputSystem
{
    public interface IInputController
    {
        void AddListener(Action<Vector3, Quaternion> moveAction);
    }
}