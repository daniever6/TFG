using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("References")] 
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Camera _camera;
    private RaycastHit _hit;
    
    [Header("Properties")] 
    [SerializeField] private float _moveVelocity = 7f;
    private Vector2 _moveDirection;

    [Header("Input Actions")] 
    [SerializeField] private InputActionReference _move;

    public static event Action OnWalking;

    #endregion

    #region Lifecycle Methods

    private void OnEnable()
    {
        _move.action.performed += ctx => { OnWalking?.Invoke(); }; 
    }

    private void OnDisable()
    {
        _move.action.performed -= ctx => { OnWalking?.Invoke(); }; 
    }

    private void Update()
    {
        _moveDirection = _move.action.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_moveDirection.x * _moveVelocity, 0, _moveDirection.y * _moveVelocity);
    }

    private void LateUpdate()
    {
        _camera.transform.position = this.transform.position + new Vector3(0, 7.5f, -5);
    }

    #endregion
    
}
