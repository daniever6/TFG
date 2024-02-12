using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Player
{
    public class PlayerMovementOnClick:MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField]private NavMeshAgent _navMeshAgent;
        private RaycastHit _hit;
        private string _groundTag = "Ground";
        
        [Header("Input Actions")] 
        [SerializeField] private InputActionReference _walkToClick;

        #region Methods

        private void OnEnable()
        {
            _walkToClick.action.Enable();
            _walkToClick.action.performed += WalkToPoint;
        }

        private void OnDisable()
        {
            _walkToClick.action.performed -= WalkToPoint;
            _walkToClick.action.Disable();
        }
        
        public void WalkToPoint(InputAction.CallbackContext context)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out _hit, Mathf.Infinity))
            {
                if (_hit.collider.tag.Equals(_groundTag))
                {
                    _navMeshAgent.SetDestination(_hit.point);
                }
            }
        }

        #endregion
    }
}