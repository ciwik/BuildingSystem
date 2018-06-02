using System.Collections.Generic;
using Extensions;
using Ui;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InputSystem
{
    public class TouchscreenInputController : AbstractInputController
    {
        private UiJoystick _joystick;
        private Button _buildButton;

        public void Start()
        {
            var canvas = FindObjectOfType<Canvas>();
            _joystick = canvas.GetComponentInChildren<UiJoystick>(includeInactive: true);
            _buildButton = canvas.GetComponentInChildren<Button>(includeInactive: true);
            _joystick.gameObject.SetActive(true);
            _buildButton.gameObject.SetActive(true);
            _buildButton.onClick.AddListener(PressBuild);

            Input.backButtonLeavesApp = true;
        }

        public override void Update()
        {
            base.Update();
            _buttonWasPressed = false;
        }

        protected override bool GetDirection(out Vector3 direction)
        {
            direction = _joystick.Direction;
            return !direction.IsZero();
        }

        protected override bool GetEulerAnglesRotation(out Vector2 eulerAnglesRotation)
        {
            //Do nothing if UI is focused
            if (Input.touchCount == 1 && IsPointOverUi(Input.GetTouch(0).position))
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

        private Vector2 _prevPosition;
        protected override Vector2 GetPositionOnScreen()
        {
            if (Input.touchCount == 1)
            {
                var touch = Input.GetTouch(0);
                if (IsPointOverUi(touch.position))
                {
                    return _prevPosition;
                }

                _prevPosition = touch.position;
                return touch.position;
            }

            return _prevPosition;
        }

        private bool _buttonWasPressed = false;
        private void PressBuild()
        {
            _buttonWasPressed = isBuilding;
        }

        private bool IsPointOverUi(Vector2 position)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = position;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count != 0;
        }

        protected override bool IsPressed()
        {
            if (_buttonWasPressed)
            {
                _buttonWasPressed = false;
                return true;
            }
            return false;
        }
    }
}
