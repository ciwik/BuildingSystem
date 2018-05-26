using InputSystem;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.25f;

    private CharacterController _characterController;
    private IInputController _inputController;

    public void Awake()
    {
        _characterController = GetComponent<CharacterController>();

#if UNITY_STANDALONE
        _inputController = gameObject.AddComponent<MouseKeyboardInputController>();
#else
        _inputController = gameObject.AddComponent<TouchInputController>();
#endif
        _inputController.AddListener(Move);
    }

    private void Move(Vector3 direction, Quaternion rotation)
    {
        transform.rotation *= rotation;
        _characterController.Move(transform.rotation * direction * _speed);        
    }
}
