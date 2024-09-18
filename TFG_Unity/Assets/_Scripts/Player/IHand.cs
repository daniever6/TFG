using System;
using _Scripts.Interactables;
using UnityEngine;

namespace _Scripts.Player
{
    public interface IHand
    {
        public void GrabObject(LevelInteractable objectToGrab, GameObject newParent);

        public void DropObject(GameObject newParent, Vector3 position = default);

        public Vector3 HandInitialPosition { get; }
    }
}