using System;
using _Scripts.Player;
using Command;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public enum PlayerState
    {
        FirstPerson = 0,
        ThirdPerson = 1
    }
    
    /// <summary>
    /// Clase encargada del input del personaje
    /// </summary>
    public class UserInput : Utilities.Singleton<UserInput>
    {
        #region Class implementation
        public static event Action<PlayerState> OnPlayerStateChanged;
        public static event Action OnWalking;

        [SerializeField] private PlayerState _playerState;
        private Vector2 _moveDirection;
        
        [Header("Component's References")]
        [SerializeField] private PlayerController _playerController;
        //[SerializeField] [CanBeNull] private PlayerMovement _playerMovement;
        //[SerializeField] [CanBeNull] private PlayerMovementOnClick _playerMovementOnClick;
        
        private ICommand _moveCommand;
        private ICommand _walkOnClickCommand;
        private ICommand _useCommand;
        private ICommand _pauseCommand;
        
        [Header("Input Actions")] 
        [SerializeField][CanBeNull] private InputActionReference _move;
        [SerializeField][CanBeNull] private InputActionReference _walkOnClick;
        [SerializeField] private InputActionReference _use;
        [SerializeField] private InputActionReference _pause;

        #endregion

        #region Config Methods
        
        private void Start()
        {
            _moveCommand = _playerController.PlayerMovement != null? new MoveCommand(_playerController) : null;
            _walkOnClickCommand = _playerController.PlayerMovementOnClick != null? new WalkOnClickCommand(_playerController) : null;
            _useCommand = new UseCommand(_playerController);
            _pauseCommand = new PauseCommand(_playerController);
            
            ChangePlayerState(_playerState);
        }
        
        /// <summary>
        /// Metodo que controla el input del jugador
        /// </summary>
        /// <param name="newState">Estado nuevo del jugador</param>
        public void ChangePlayerState(PlayerState newState)
        {
            OnPlayerStateChanged?.Invoke(newState);
            
            _playerState = newState;

            switch (newState)
            {
                case PlayerState.FirstPerson:
                    EnableFirstPersonInput();
                    break;
                
                case PlayerState.ThirdPerson:
                    EnableThirdPersonInput();
                    break;
            }
        }

        private void FixedUpdate()
        {
            _moveDirection = _move.action.ReadValue<Vector2>();
            _moveCommand?.Execute(_moveDirection);
        }

        private void OnEnable()
        {
            _pause.action.performed += ctx => _pauseCommand?.Execute();
        }

        private void OnDisable()
        {   
            DisableThirdPersonInput();
            DisableFirstPersonInput();
            _pause.action.Disable();

        }
        
        #endregion

        #region FIRST PERSON METHODS

        private void EnableFirstPersonInput()
        {
            DisableThirdPersonInput();

            if(_use != null) _use.action.performed += ctx => _useCommand?.Execute(ctx);
        }

        private void DisableFirstPersonInput()
        {
            _use.action.Disable();
        }

        #endregion
        
        
        #region THIRD PERSON METHODS

        /// <summary>
        /// Suscribe los metodos de tercera persona
        /// </summary>
        private void EnableThirdPersonInput()
        {
            DisableFirstPersonInput();
            
            if(_move != null)_move.action.performed += ctx =>
            {
                _moveCommand?.Execute();
                OnWalking?.Invoke();
            };
            if(_walkOnClick != null)_walkOnClick.action.performed += ctx => _walkOnClickCommand?.Execute(ctx);
        }

        /// <summary>
        /// Desusbribe los metodos del movimiento de tercera persona
        /// </summary>
        private void DisableThirdPersonInput()
        {
            if(_move != null)_move.action.Disable();
            if(_walkOnClick != null)_walkOnClick.action.Disable();
        }

        #endregion
    }
}
