using InputSystem;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private Builder _builder;    

    private CharacterController _characterController;
    private IInputController _inputController;
    private Camera _camera;    

    internal void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        _builder = FindObjectOfType<Builder>();

#if UNITY_STANDALONE
        _inputController = gameObject.AddComponent<StandaloneInputController>();
#endif
#if UNITY_ANDROID || UNITY_IOS
        _inputController = gameObject.AddComponent<TouchscreenInputController>();
#endif
    }

    internal void Start()
    {
        InitListeners();
    }

    internal void FixedUpdate()
    {
        _characterController.Move(Physics.gravity * Time.fixedDeltaTime);
    }

    private void InitListeners()
    {
        _inputController.WithMoveListener(Move)
            .WithRaycastingListener(_builder.PlaceBlock)
            .WithBuildListeners(buildAction:_builder.Build, 
                buildingCancelAction:_builder.ResetBuilding)
            .WithItemSelectListeners(blockItemSelectAction:_builder.StartBuilding, 
                blockItemResetAction:_builder.ResetBuilding);
    }

    private void Move(Vector3 direction, Quaternion rotation)
    {
        Quaternion fullRotation = _camera.transform.rotation * rotation;
        _camera.transform.eulerAngles = new Vector3(fullRotation.eulerAngles.x, fullRotation.eulerAngles.y);  
        _characterController.Move(_camera.transform.rotation * direction * _speed * Time.deltaTime);        
    }
}
