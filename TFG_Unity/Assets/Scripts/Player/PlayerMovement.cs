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
    [SerializeField] private NavMeshAgent _navMeshAgent;
    private RaycastHit _hit;
    
    [Header("Properties")] 
    [SerializeField] private float _moveVelocity = 7f;
    private Vector2 _moveDirection;

    [Header("Input Actions")] 
    [SerializeField] private InputActionReference _move;

    #endregion

    #region Lifecycle Methods

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _moveDirection = _move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_moveDirection.x * _moveVelocity, 0, _moveDirection.y * _moveVelocity);
    }

    #endregion
    
}
