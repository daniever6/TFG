using System;
using UnityEngine;

namespace _Scripts.Interactables
{
    public class ObjectInteractable : MonoBehaviour
    {
        private Vector3 _initialPosition;
        public Vector3 InitialPosition => _initialPosition;

        private void Start()
        {
            _initialPosition = transform.position;
        }
    }
}