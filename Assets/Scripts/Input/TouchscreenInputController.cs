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
        private Button _buildCancelledButton;

        internal override void Awake()
        {
            base.Awake();
            Input.backButtonLeavesApp = true;
        }

        internal void Start()
        {            
            var canvas = FindObjectOfType<Canvas>();
            _joystick = canvas.GetComponentInChildren<UiJoystick>(includeInactive: true);

            var buttons = canvas.GetComponentInChildren<UiButtons>(includeInactive: true);
            _buildButton = buttons.BuildButton;
            _buildCancelledButton = buttons.BuildCancelledButton;

            _joystick.gameObject.SetActive(true);
            _buildButton.gameObject.SetActive(true);
            _buildButton.onClick.AddListener(PressBuild);
            _buildCancelledButton.onClick.AddListener(() =>
            {
                CancelBuilding();
                _buildCancelledButton.gameObject.SetActive(false);
            });
        }

        internal override void Update()
        {
            base.Update();
            _buttonWasPressed = false;
        }

        protected override void OnItemSelected()
        {
            _buildCancelledButton.gameObject.SetActive(true);
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

        protected override bool IsPressed()
        {
            if (_buttonWasPressed)
            {
                _buttonWasPressed = false;
                return true;
            }
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
            _buttonWasPressed = true;
            _buildCancelledButton.gameObject.SetActive(false);
        }

        private bool IsPointOverUi(Vector2 position)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = position;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count != 0;
        }
    }
}
