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

        private static event Action InputHandler;
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
                    HandleFirstPerson();
                    break;
                
                case PlayerState.ThirdPerson:
                    HandleThirdPerson();
                    break;
            }
        }

        private void FixedUpdate()
        {
            InputHandler?.Invoke();
        }

        private void OnEnable()
        {
            if(_move != null) _move.action.performed += ctx => {  _moveCommand?.Execute(_moveDirection);  OnWalking?.Invoke();}; 
            if(_walkOnClick != null) _walkOnClick.action.performed += ctx =>  _walkOnClickCommand?.Execute(ctx);
            _use.action.performed += ctx => _useCommand?.Execute(ctx);
            _pause.action.performed += ctx => _pauseCommand?.Execute();
        }

        private void OnDisable()
        {   
            if(_move != null)_move.action.Disable();
            if(_walkOnClick != null)_walkOnClick.action.Disable();
            _use.action.Disable();
            _pause.action.Disable();

        }
        
        #endregion

        #region FIRST PERSON METHODS

        private void HandleFirstPerson()
        {
            DisableThirdPersonInput();

            if (_use != null)
            {
                InputHandler += Interact;
            }
        }

        private void DisableFirstPersonInput()
        {
            InputHandler -= Interact;
        }

        private void Interact()
        {
        }

        #endregion
        
        
        #region THIRD PERSON METHODS

        /// <summary>
        /// Suscribe los metodos de tercera persona
        /// </summary>
        private void HandleThirdPerson()
        {
            DisableFirstPersonInput();
            
            if (_move != null)
            {
                InputHandler += KeyboardMovement;
            }

            if (_walkOnClick != null)
            {
                InputHandler += ClickMovement;
            }
        }

        /// <summary>
        /// Desusbribe los metodos del movimiento de tercera persona
        /// </summary>
        private void DisableThirdPersonInput()
        {
            InputHandler -= KeyboardMovement;
            InputHandler -= ClickMovement;
        }
        
        /// <summary>
        /// Controla el movimiento del jugador mediante teclado
        /// </summary>
        private void KeyboardMovement()
        {
            _moveDirection = _move.action.ReadValue<Vector2>();
            _moveCommand?.Execute(_moveDirection);
        }

        /// <summary>
        /// Controla el movimiento e interacciones del jugador cuando se hace click
        /// </summary>
        private void ClickMovement()
        {
            _moveDirection = _move.action.ReadValue<Vector2>();
            _moveCommand?.Execute(_moveDirection);
        }

        #endregion
    }
}
