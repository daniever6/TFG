using System;
using _Scripts.Utilities;
using _Scripts.Utilities.Command;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    public enum PlayerState
    {
        FirstPerson = 0,
        ThirdPerson = 1
    }
    
    /// <summary>
    /// Clase encargada del input del personaje
    /// </summary>
    public class UserInput : Singleton<UserInput>
    {
        #region Class implementation
        public static event Action<PlayerState> OnPlayerStateChanged;
        public static event Action OnWalking;

        [SerializeField] private PlayerState playerState;
        private Vector2 _moveDirection;
        
        [Header("Component's References")]
        [SerializeField] private PlayerController playerController;
        
        private ICommand _moveCommand;
        private ICommand _walkOnClickCommand;
        private ICommand _useCommand;
        private ICommand _pauseCommand;
        private ICommand _interactCommand;
        
        [Header("Input Actions")] 
        [SerializeField][CanBeNull] private InputActionReference move;
        [SerializeField][CanBeNull] private InputActionReference walkOnClick;
        [SerializeField] private InputActionReference use;
        [SerializeField] private InputActionReference pause;
        [SerializeField] private InputActionReference interact;

        #endregion

        #region Config Methods
        
        private void Start()
        {
            _moveCommand = playerController.PlayerMovement != null? new MoveCommand(playerController) : null;
            _walkOnClickCommand = playerController.PlayerMovementOnClick != null? new WalkOnClickCommand(playerController) : null;
            _useCommand = new UseCommand(playerController);
            _pauseCommand = new PauseCommand(playerController);
            _interactCommand = new InteractCommand(playerController);
            
            ChangePlayerState(playerState);
        }
        
        /// <summary>
        /// Metodo que controla el input del jugador
        /// </summary>
        /// <param name="newState">Estado nuevo del jugador</param>
        public void ChangePlayerState(PlayerState newState)
        {
            OnPlayerStateChanged?.Invoke(newState);
            
            playerState = newState;

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
            _moveDirection = move.action.ReadValue<Vector2>();
            _moveCommand?.Execute(_moveDirection);
        }

        /// <summary>
        /// Activa las acciones principales del input
        /// </summary>
        private void OnEnable()
        {
            pause.action.performed += PauseActionMethod;
        }

        /// <summary>
        /// Desabilita todas las acciones del input
        /// </summary>
        private void OnDisable()
        {   
            DisableThirdPersonInput();
            DisableFirstPersonInput();
            pause.action.performed -= PauseActionMethod;
            pause.action.Disable();
        }

        #endregion

        #region FIRST PERSON METHODS

        /// <summary>
        /// Suscribe las acciones de primera persona
        /// </summary>
        private void EnableFirstPersonInput()
        {
            DisableThirdPersonInput();

            if(use != null) use.action.performed += ctx => _useCommand?.Execute(ctx);
        }

        /// <summary>
        /// Desuscribe las acciones de primera persona
        /// </summary>
        private void DisableFirstPersonInput()
        {
            use.action.Disable();
        }

        #endregion
        
        
        #region THIRD PERSON METHODS

        /// <summary>
        /// Suscribe las acciones de tercera persona
        /// </summary>
        private void EnableThirdPersonInput()
        {
            DisableFirstPersonInput();
            
            if(move != null)move.action.performed += ctx =>
            {
                _moveCommand?.Execute();
                OnWalking?.Invoke();
            };
            if (walkOnClick != null) walkOnClick.action.performed += ctx => _walkOnClickCommand?.Execute(ctx);
            if (interact != null) interact.action.performed += ctx => _interactCommand?.Execute();
        }

        /// <summary>
        /// Desusbribe las acciones del movimiento de tercera persona
        /// </summary>
        private void DisableThirdPersonInput()
        {
            if(move != null) move.action.Disable();
            if(walkOnClick != null) walkOnClick.action.Disable();
            if(interact != null) interact.action.Disable();
        }
        
        /// <summary>
        /// Metodo que ejecuta el comando de pausa
        /// </summary>
        /// <param name="ctx"></param>
        public void PauseActionMethod(InputAction.CallbackContext ctx)
        {
            _pauseCommand?.Execute();
        }

        #endregion
    }
}
