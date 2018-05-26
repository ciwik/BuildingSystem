using Ui;
using UnityEngine;

namespace InputSystem
{
    public class TouchInputController : AbstractInputController
    {
        private UiJoystick _joystick;

        public void Awake()
        {
            _joystick = FindObjectOfType<UiJoystick>();
            _joystick.GetComponentInParent<Canvas>().enabled = true;

            Input.backButtonLeavesApp = true;
        }

        protected override Vector3 GetDirection()
        {
            return _joystick.Direction;
        }

        protected override Vector2 GetEulerAnglesRotation()
        {
            //Do nothing if UI is focused
            if (_joystick.IsFocused && Input.touchCount == 1)
            {
                return Vector2.zero;
            }

            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 currentDelta = touch.deltaPosition;
                    return new Vector2(
                        currentDelta.x * 180f / Screen.width,
                        currentDelta.y * 90f / Screen.height
                    );
                }
            }
            return Vector2.zero;
        }
    }
}
