using InputSystem;
using UnityEngine;

public class MouseKeyboardInputController : AbstractInputController
{
    private class AxisNames
    {
        public const string Horizontal = "Horizontal";
        public const string Vertical = "Vertical";
        public const string MouseX = "Mouse X";
        public const string MouseY = "Mouse Y";
    }

    private KeyCode _forwardKey = KeyCode.W,
        _backwardKey = KeyCode.S,
        _leftKey = KeyCode.A,
        _rightKey = KeyCode.D;    

    public void Awake()
    {
        Cursor.visible = false;
    }

    protected override Vector3 GetDirection()
    {        
        return new Vector3(Input.GetAxis(AxisNames.Horizontal),
            0,
            Input.GetAxis(AxisNames.Vertical));
    }

    protected override Vector2 GetEulerAnglesRotation()
    {
        return new Vector2(Input.GetAxis(AxisNames.MouseX),
            Input.GetAxis(AxisNames.MouseY));
    }
}
