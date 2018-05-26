using Extensions;
using Ui;
using UnityEngine;

namespace InputSystem
{
    public class TouchscreenInputController : AbstractInputController
    {
        private UiJoystick _joystick;

        public void Awake()
        {
            var canvas = FindObjectOfType<Canvas>();
            _joystick = canvas.GetComponentInChildren<UiJoystick>(includeInactive: true);
            _joystick.gameObject.SetActive(true);

            Input.backButtonLeavesApp = true;
        }

        protected override bool GetDirection(out Vector3 direction)
        {
            direction = _joystick.Direction;
            return !direction.IsZero();
        }

        protected override bool GetEulerAnglesRotation(out Vector2 eulerAnglesRotation)
        {
            //Do nothing if UI is focused
            if (_joystick.IsFocused && Input.touchCount == 1)
            {
                eulerAnglesRotation = Vector2.zero;
                return false;
            }

            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 currentDelta = touch.deltaPosition;
                    eulerAnglesRotation = new Vector2(
                        currentDelta.x * 180f / Screen.width,
                        currentDelta.y * 90f / Screen.height
                    );
                    return true;
                }
            }

            eulerAnglesRotation = Vector2.zero;
            return false;
        }
    }
}
