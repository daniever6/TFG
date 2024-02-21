using System;
using Command;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// Clase encargada del input del personaje
    /// </summary>
    public class UserInput : Utilities.Singleton<MonoBehaviour>
    {
        #region Properties
        
        [SerializeField] [CanBeNull] private PlayerMovement _playerMovement;
        [SerializeField] [CanBeNull] private PlayerMovementOnClick _playerMovementOnClick;
        
        private ICommand _moveCommand;
        private ICommand _walkOnClickCommand;
        public static event Action OnWalking;   
        private Vector2 _moveDirection;
        
        [Header("Input Actions")] 
        [SerializeField] private InputActionReference _move;
        [SerializeField] private InputActionReference _walkOnClick;
        
        #endregion

        #region Lifecycle Methods
        private void Start()
        {
            _moveCommand = _playerMovement != null ? new MoveCommand(_playerMovement) : null;
            _walkOnClickCommand = _playerMovementOnClick != null ? new WalkOnClickCommand(_playerMovementOnClick) : null;
        }

        private void FixedUpdate()
        {
            _moveDirection = _move.action.ReadValue<Vector2>();
            _moveCommand?.Execute(_moveDirection);  
        }

        private void OnEnable()
        {
            _move.action.performed += ctx => {  _moveCommand?.Execute(_moveDirection);  OnWalking?.Invoke();}; 
            _walkOnClick.action.performed += context =>  _walkOnClickCommand?.Execute(context);
        }

        private void OnDisable()
        {
            _move.action.performed -= ctx => { _moveCommand?.Execute(_moveDirection); }; 
            _walkOnClick.action.performed -= context =>  _walkOnClickCommand?.Execute(context);
            _walkOnClick.action.Disable();
        }
        
        #endregion
    }
}
