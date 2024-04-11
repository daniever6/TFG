using _Scripts.Utilities;
using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    public class PlayerController : GameplayMonoBehaviour, IPlayerController
    {
        [SerializeField] [CanBeNull] private PlayerMovement playerMovement;
        [SerializeField] [CanBeNull] private PlayerMovementOnClick playerMovementOnClick;
        [SerializeField] [CanBeNull] private PlayerInteractor playerInteractor;
        [SerializeField] [CanBeNull] private PlayerGrab playerGrab;

        public PlayerMovement PlayerMovement => playerMovement;
        public PlayerMovementOnClick PlayerMovementOnClick => playerMovementOnClick;
        
        public void Move(Vector2 direction)
        {
            if (!this.enabled) return;
            playerMovement!.Move(direction);
        }

        public void WalkToPoint(InputAction.CallbackContext context)
        {
            if (!this.enabled) return;
            playerMovementOnClick!.WalkToPoint(context);
        }

        public void Use(InputAction.CallbackContext context)
        {
            if (!this.enabled) return;
            playerGrab!.Grab(context);
        }

        public void Interact()
        {
            if (!this.enabled) return;
            playerMovement!.StopMovement();
            playerInteractor!.Interact();
        }
    }
}
