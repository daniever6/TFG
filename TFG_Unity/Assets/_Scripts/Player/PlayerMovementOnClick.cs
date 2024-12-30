using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Dialogues;
using _Scripts.Utilities;
using Assets._Scripts.NPCs;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    /// <summary>
    /// Clase que controla el movimiento del jugador a traves del raton mediante OnClick
    /// </summary>
    public class PlayerMovementOnClick:GameplayMonoBehaviour<PlayerMovementOnClick>
    {
        #region Properties

        [SerializeField] private Camera _camera;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField][CanBeNull] private Animator playerAnimator;
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

        private void Update()
        {
            playerAnimator?.SetBool("ClickWalking", _navMeshAgent.hasPath);
        }

        /// <summary>
        /// Mueve el personaje a través de su navMeshAgent a la posicion indicada cuando se hace click con el ratón.
        /// Comprueba el objeto con el que interactuar y llama al evento correspondiente si hace falta.
        /// </summary>
        /// <param name="context"></param>
        public IEnumerator WalkToPoint(InputAction.CallbackContext context)
        {
            if (EventSystem.current.IsPointerOverGameObject()) yield break;
            
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

                        GameObject npc = _hit.collider.gameObject;

                        //Espera a llegar a destino
                        yield return StartCoroutine(WaitForDestination(_hit.collider.GetComponent<DialogueTrigger>()));

                        if(isAwaiting == false)
                        {
                            npc.TryGetComponent<NpcState>(out NpcState npcState);

                            if (!npcState.IsUnityNull())
                            {
                                // Si le ha caido ácido encima coge al npc
                                if (npcState.State == NpcStates.Acid)
                                {
                                    CarryNPC.Instance.Carry(npc);
                                }
                                else if(npcState.State == NpcStates.Burning && MantaIgnifugaManager.IsCarried)
                                {
                                    MantaIgnifugaManager.Instance.DropMantaOnNpc();
                                }
                                
                            }
                            
                        }

                        break;

                    case Iteractables.Interactable:
                        _navMeshAgent.SetDestination(_hit.point);
                        yield return StartCoroutine(WaitForDestination(_hit.collider.GetComponents<Trigger>()
                            .Where(t => t.enabled)?.FirstOrDefault()));

                        //Suelta al NPC en al ducha
                        if(isAwaiting == false)
                        {
                            try
                            {
                                if (CarryNPC.Instance.IsCarrying && _hit.collider.gameObject.name == "Ducha")
                                {
                                    CarryNPC.Instance.DropOnDucha();
                                }
                            }catch(Exception e)
                            {

                            }
                            
                        }
                        
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Establece el IsWalking del animator a True y el destino del NavMeshAgent
        /// </summary>
        /// <param name="point">Punto de destino</param>
        private void SetPlayerDestination(Vector3 point)
        {
            _navMeshAgent.SetDestination(point);
        }

        /// <summary>
        /// Detiene el movimiento del navMeshAgent. Se detiene cuando llamamos el evento OnWalking.
        /// </summary>
        public void ClearNavMeshAgentPath()
        {
            playerAnimator?.SetBool("ClickWalking", false);

            UserInput.OnWalking -= ClearNavMeshAgentPath;
            if (_navMeshAgent.hasPath)
            {
                _navMeshAgent.isStopped = true;
            }
            _navMeshAgent.ResetPath();
            _navMeshAgent.path.ClearCorners();

        }

        private bool isAwaiting = false;
        
        /// <summary>
        /// Espera a que el jugador llegue al destino para realizar la accion
        /// </summary>
        /// <param name="trigger">Referencia a la clase que ejecutara el metodo TriggerEvent</param>
        /// <typeparam name="T">Generico que hereda de la clase abstracta Trigger</typeparam>
        /// <returns>Acaba la corrutina si la distancia restante <= 0.1f</returns>
        IEnumerator WaitForDestination<T> (T trigger) where T : Trigger
        {
            isAwaiting = true;
            while (_navMeshAgent.remainingDistance > 0f)
            {
                if (_navMeshAgent.remainingDistance <= 1f)
                {
                    trigger?.TriggerEvent();
                    ClearNavMeshAgentPath();
                    isAwaiting = false;
                    yield break;
                }
                yield return null;
            }
        }
        #endregion
    }
}