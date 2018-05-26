using System;
using UnityEngine;

namespace InputSystem
{
    public abstract class AbstractInputController : MonoBehaviour, IInputController
    {
        private Action<Vector3, Quaternion> _moveAction;

        private Vector2 _sensitivity = new Vector2(2f, 1f);

        public void Update ()
        {
            Vector3 direction = GetDirection();
            Vector2 eulerAnglesRotation = GetEulerAnglesRotation();
            Quaternion rotation = Quaternion.Euler(-eulerAnglesRotation.y * _sensitivity.y,
                eulerAnglesRotation.x * _sensitivity.x,
                0f);
            _moveAction.Invoke(direction, rotation);
        }

        public void AddListener(Action<Vector3, Quaternion> moveAction)
        {
            _moveAction = moveAction;
        }

        protected abstract Vector3 GetDirection();
        protected abstract Vector2 GetEulerAnglesRotation();
    }
}
