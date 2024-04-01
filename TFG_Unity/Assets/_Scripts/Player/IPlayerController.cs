using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public interface IPlayerController
    {
        public void Move(Vector2 moveDirection);
        public void WalkToPoint(InputAction.CallbackContext context);
        public void Use(InputAction.CallbackContext context);
        public void Interact();
    }
}