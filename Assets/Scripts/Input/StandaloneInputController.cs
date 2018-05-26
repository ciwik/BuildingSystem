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

        public void Awake()
        {
            Cursor.visible = false;
        }

        protected override bool GetDirection(out Vector3 direction)
        {
            direction = new Vector3(Input.GetAxis(AxisNames.Horizontal),
                0,
                Input.GetAxis(AxisNames.Vertical));
            return direction.sqrMagnitude > float.Epsilon;            
        }

        protected override bool GetEulerAnglesRotation(out Vector2 eulerAnglesRotation)
        {
            eulerAnglesRotation = new Vector2(Input.GetAxis(AxisNames.MouseX),
                Input.GetAxis(AxisNames.MouseY));
            return eulerAnglesRotation.sqrMagnitude > float.Epsilon;
        }
    }
}