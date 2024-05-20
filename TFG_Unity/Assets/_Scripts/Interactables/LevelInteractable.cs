using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Interactables
{
    public class LevelInteractable : MonoBehaviour
    {
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Vector3 _grabPosition;
        private Quaternion _grabRotation;
        [SerializeField][CanBeNull] private Transform grabTransform;

        public Vector3 InitialPosition => _initialPosition;
        public Quaternion GrabRotation => _grabRotation;
        public Vector3 GrabLocalPos => _grabPosition;
        public Quaternion InitialRotation => _initialRotation;

        private void Start()
        {
            if (grabTransform != null)
            {
                _grabRotation = grabTransform.rotation;
                _grabPosition = grabTransform.localPosition;
            }
            _initialRotation = transform.rotation;
            _initialPosition = transform.position;
        }
    }
}