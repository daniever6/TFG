using System;
using System.Collections;
using System.Collections.Generic;
using Command;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : Utilities.Singleton<MonoBehaviour>
{
    [SerializeField]private PlayerMovement _playerMovement;
    private CommandInvoker _commandInvoker;
    private Vector2 _moveDirection;
    public static event Action OnWalking;   
    
    [Header("Input Actions")] 
    [SerializeField] private InputActionReference _move;

    private void Start()
    {
        ICommand moveCommand = new MoveCommand(_playerMovement);
        _commandInvoker = new CommandInvoker(moveCommand);
    }

    private void FixedUpdate()
    {
        _moveDirection = _move.action.ReadValue<Vector2>();
        _commandInvoker.Execute(_moveDirection);  
    }

    private void OnEnable()
    {
        _move.action.performed += ctx => {  _commandInvoker.Execute(_moveDirection);  OnWalking?.Invoke();}; 
    }

    private void OnDisable()
    {
        _move.action.performed -= ctx => { _commandInvoker.Execute(_moveDirection); }; 
    }
}
