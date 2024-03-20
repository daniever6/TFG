﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public interface IPlayerController
    {
        public void Move(Vector2 MoveDirection);
        public void WalkToPoint(InputAction.CallbackContext context);
        public void Use();
    }
}