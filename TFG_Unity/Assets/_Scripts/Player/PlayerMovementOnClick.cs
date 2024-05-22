using System;
using System.Collections;
using _Scripts.Dialogues;
using _Scripts.Interactables;
using _Scripts.Player;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Player
{
    public enum Iteractables
    {
        None,
        Ground,
        Alfombrilla,
        Npc,
        Interactable,
    }
    
    /// <summary>
    /// Clase que controla el movimiento del jugador a traves del raton mediante OnClick
    /// </summary>
    public class PlayerMovementOnClick:GameplayMonoBehaviour
    {
        #region Properties

        [SerializeField] private Camera _camera;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        private RaycastHit _hit;

        #endregion

        #region Inheritance Methods

        protected override void OnPostPaused()
        {
            UserInput.OnWalking -= ClearNavMeshAgentPath;
            _navMeshAgent.isStopped = true;
        }

        protected override void OnPostResumed()
        {
            UserInput.OnWalking += ClearNavMeshAgentPath;
            _navMeshAgent.isStopped = false;
            if(_navMeshAgent.hasPath) _navMeshAgent.SetDestination(_navMeshAgent.pathEndPosition);
        }

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
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            _navMeshAgent.isStopped = false;

            UserInput.OnWalking += ClearNavMeshAgentPath;
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out _hit, Mathf.Infinity))
            {
                Iteractables parsedEnum;
                Enum.TryParse(_hit.collider.tag, out parsedEnum);
                switch(parsedEnum)
                {
                    case Iteractables.None:
                        break;
                    case Iteractables.Ground:
                        _navMeshAgent.SetDestination(_hit.point);
                        break;
                    case Iteractables.Npc:
                        _navMeshAgent.SetDestination(_hit.point);
                        StartCoroutine(WaitForDestination(_hit.collider.GetComponent<DialogueTrigger>()));
                        break;
                    case Iteractables.Interactable:
                        _navMeshAgent.SetDestination(_hit.point);
                        StartCoroutine(WaitForDestination(_hit.collider.GetComponent<Trigger>()));
                        break;
                    default:
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
        
        /// <summary>
        /// Espera a que el jugador llegue al destino para realizar la accion
        /// </summary>
        /// <param name="trigger">Referencia a la clase que ejecutara el metodo TriggerEvent</param>
        /// <typeparam name="T">Generico que hereda de la clase abstracta Trigger</typeparam>
        /// <returns>Acaba la corrutina si la distancia restante <= 0.1f</returns>
        IEnumerator WaitForDestination<T> (T trigger) where T : Trigger
        {
            while (_navMeshAgent.remainingDistance > 0f)
            {
                if (_navMeshAgent.remainingDistance <= 1f)
                {
                    trigger?.TriggerEvent();
                    ClearNavMeshAgentPath();
                    yield break;
                }
                yield return null;
            }
        }
        #endregion
    }
}