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
        #region Class implementation
        
        [Header("Component's References")]
        [SerializeField] [CanBeNull] private PlayerMovement _playerMovement;
        [SerializeField] [CanBeNull] private PlayerMovementOnClick _playerMovementOnClick;
        
        private ICommand _moveCommand;
        private ICommand _walkOnClickCommand;
        private ICommand _pauseCommand;
        
        public static event Action OnWalking;   
        private Vector2 _moveDirection;
        
        [Header("Input Actions")] 
        [SerializeField] private InputActionReference _move;
        [SerializeField] private InputActionReference _walkOnClick;
        [SerializeField] private InputActionReference _pause;
        
        #endregion

        #region Input handler methods
        private void Start()
        {
            _moveCommand = _playerMovement != null ? new MoveCommand(_playerMovement) : null;
            _walkOnClickCommand = _playerMovementOnClick != null ? new WalkOnClickCommand(_playerMovementOnClick) : null;
            _pauseCommand = new PauseCommand(null);
        }

        private void FixedUpdate()
        {
            _moveDirection = _move.action.ReadValue<Vector2>();
            _moveCommand?.Execute(_moveDirection);  
        }

        private void OnEnable()
        {
            _move.action.performed += ctx => {  _moveCommand?.Execute(_moveDirection);  OnWalking?.Invoke();}; 
            _walkOnClick.action.performed += ctx =>  _walkOnClickCommand?.Execute(ctx);
            _pause.action.performed += ctx => _pauseCommand?.Execute();
        }

        private void OnDisable()
        {   
            _move.action.Disable();
            _walkOnClick.action.Disable();
            _pause.action.Disable();

        }
        
        #endregion
    }
}
