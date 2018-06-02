using Extensions;
using UnityEngine;

namespace InputSystem
{
    public class StandaloneInputController : AbstractInputController
    {
        private class AxisNames
        {
            public const string Horizontal = "Horizontal";
            public const string Vertical = "Vertical";
            public const string MouseX = "Mouse X";
            public const string MouseY = "Mouse Y";
        }

        internal void Start()
        {
            Cursor.visible = false;
        }

        internal override void Update()
        {
            base.Update();
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CancelBuilding();
                    return;
                }
                for (var key = KeyCode.Alpha1; key < KeyCode.Alpha9; key++)
                {
                    if (Input.GetKeyDown(key))
                    {                        
                        SelectItem(key - KeyCode.Alpha0 - 1);
                        return;
                    }
                }
            }
        }

        protected override bool GetDirection(out Vector3 direction)
        {
            direction = new Vector3(Input.GetAxis(AxisNames.Horizontal),
                0,
                Input.GetAxis(AxisNames.Vertical));
            return !direction.IsZero();            
        }

        protected override bool GetEulerAnglesRotation(out Vector2 eulerAnglesRotation)
        {
            eulerAnglesRotation = new Vector2(Input.GetAxis(AxisNames.MouseX),
                Input.GetAxis(AxisNames.MouseY));
            return !eulerAnglesRotation.IsZero();
        }

        protected override Vector2 GetPositionOnScreen()
        {
            return Input.mousePosition;
        }

        protected override bool IsPressed()
        {
            return Input.GetMouseButtonDown(0);
        }
    }
}