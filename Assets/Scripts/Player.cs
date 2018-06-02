﻿using InputSystem;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private Builder _builder;
    [SerializeField]
    private UiBuildingsInventory _buildingsInventory;

    private CharacterController _characterController;
    private IInputController _inputController;
    private Camera _camera;    

    public void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        _builder = FindObjectOfType<Builder>();

        InitListeners();
    }

    public void FixedUpdate()
    {
        _characterController.Move(Physics.gravity * Time.fixedDeltaTime);
    }

    private void InitListeners()
    {
#if UNITY_STANDALONE
        _inputController = gameObject.AddComponent<StandaloneInputController>();
#endif
#if UNITY_ANDROID || UNITY_IOS
        _inputController = gameObject.AddComponent<TouchscreenInputController>();
#endif
        _inputController.WithMoveListener(Move);
        _inputController.WithRaycastingListener(_builder.PlaceBlock);
        _inputController.WithBuildListener(() =>
        {
            _builder.Build();
            _buildingsInventory.Reset();
        });

        _buildingsInventory.WithInventoryItemListeners(b =>
        {
            _builder.StartBuilding(b);
            _inputController.StartBuilding();
        }, () =>
        {
            _builder.ResetBuilding();
            _inputController.StopBuilding();
        });
    }

    private void Move(Vector3 direction, Quaternion rotation)
    {
        Quaternion fullRotation = _camera.transform.rotation * rotation;
        _camera.transform.eulerAngles = new Vector3(fullRotation.eulerAngles.x, fullRotation.eulerAngles.y);  
        _characterController.Move(_camera.transform.rotation * direction * _speed * Time.deltaTime);        
    }
}
