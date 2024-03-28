using System;
using _Scripts.Utilities;
using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace _Scripts.Player
{
    public class PlayerController : GameplayMonoBehaviour, IPlayerController
    {
        [SerializeField] [CanBeNull] private PlayerMovement _playerMovement;
        [SerializeField] [CanBeNull] private PlayerMovementOnClick _playerMovementOnClick;
        [SerializeField] [CanBeNull] private PlayerGrab _playerGrab;

        public PlayerMovement PlayerMovement => _playerMovement;
        public PlayerMovementOnClick PlayerMovementOnClick => _playerMovementOnClick;
        
        public void Move(Vector2 direction)
        {
            if (!this.enabled) return;
            _playerMovement.Move(direction);
        }

        public void WalkToPoint(InputAction.CallbackContext context)
        {
            if (!this.enabled) return;
            _playerMovementOnClick.WalkToPoint(context);
        }

        public void Use(InputAction.CallbackContext context)
        {
            if (!this.enabled) return;
            _playerGrab?.Grab(context);
        }
    }
}
