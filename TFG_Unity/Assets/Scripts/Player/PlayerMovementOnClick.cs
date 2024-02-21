using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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
        #region Properties

        [SerializeField] private Camera _camera;
        [SerializeField]private NavMeshAgent _navMeshAgent;
        private RaycastHit _hit;

        #endregion

        #region Methods

        private void OnDisable()
        {
            UserInput.OnWalking -= ClearNavMeshAgentPath;
        }
        
        /// <summary>
        /// Mueve el personaje a través de su navMeshAgent a la posicion indicada cuando se hace click con el ratón.
        /// Comprueba el objeto con el que interactuar y llama al evento correspondiente si hace falta.
        /// </summary>
        /// <param name="context"></param>
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
                        break;
                    case Iteractables.Element:
                        _navMeshAgent.SetDestination(_hit.point);
                        break;
                }
            }
        }

        /// <summary>
        /// Detiene el movimiento del navMeshAgent. Se detiene cuando llamamos el evento OnWalking.
        /// </summary>
        public void ClearNavMeshAgentPath()
        {
            UserInput.OnWalking -= ClearNavMeshAgentPath;
            if (_navMeshAgent.hasPath)
            {
                _navMeshAgent.isStopped = true;
            }
            _navMeshAgent.ResetPath();
            _navMeshAgent.path.ClearCorners();

        }
        #endregion
    }
}