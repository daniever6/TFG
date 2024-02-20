using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Player
{
    public enum Iteractables
    {
        None,
        Ground,
        Npc,
        Element
    }
    
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
            UserInput.OnWalking -= ClearNavMeshAgentPath;
            _walkToClick.action.Disable();
        }
        
        public void WalkToPoint(InputAction.CallbackContext context)
        {
            _navMeshAgent.isStopped = false;

            UserInput.OnWalking += ClearNavMeshAgentPath;
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out _hit, Mathf.Infinity))
            {
                string colliderTag = _hit.collider.tag;
                Iteractables parsedEnum = (Iteractables)Enum.Parse(typeof(Iteractables), colliderTag);
                switch(parsedEnum)
                {
                    case Iteractables.None:
                        break;
                    case Iteractables.Ground:
                        _navMeshAgent.SetDestination(_hit.point);
                        break;
                    case Iteractables.Npc:
                        _navMeshAgent.SetDestination(_hit.point);
                        Debug.Log("NPC clicked");
                        break;
                    case Iteractables.Element:
                        _navMeshAgent.SetDestination(_hit.point);
                        break;
                }
            }
        }

        public void ClearNavMeshAgentPath()
        {
            UserInput.OnWalking -= ClearNavMeshAgentPath;
            if(_navMeshAgent.hasPath) _navMeshAgent.isStopped = true;
        }
        #endregion
    }
}