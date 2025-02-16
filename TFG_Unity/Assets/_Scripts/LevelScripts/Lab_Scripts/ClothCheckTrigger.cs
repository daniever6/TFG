using System.Collections;
using _Scripts.Dialogues;
using _Scripts.LevelScripts.Level_00;
using _Scripts.Managers;
using _Scripts.Player;
using _Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

namespace _Scripts.LevelScripts.Lab_Scripts
{
    public class ClothCheckTrigger : GameplayMonoBehaviour<ClothCheckTrigger>
    {
        [SerializeField] private CheckRopaCorrecta checker;
        [SerializeField] private ParticleSystem poisonParticleSystem;
        [SerializeField] private GameObject player;
        private PlayerInteractor _playerInteractor;

        [SerializeField] private GameObject alumno;
        [SerializeField] private GameObject bandeja;
        [SerializeField] private GameObject bandejaPosition;
        [SerializeField] private GameObject propParent;
        [SerializeField] private DialogueTrigger dropAcidDialogue;

        private NpcState alumnoState;

        [SerializeField] private NavMeshAgent _npcNavMeshAgent;
        private bool _isGamePaused = false;

        private void Start()
        {
            alumno.TryGetComponent<NpcState>(out alumnoState);

            player.TryGetComponent(out _playerInteractor);
            alumno.TryGetComponent<NavMeshAgent>(out _npcNavMeshAgent);
        }
        
        protected override void OnPostPaused()
        {
            base.OnPostPaused();
            _isGamePaused = true;
            if (!_npcNavMeshAgent.IsUnityNull() && _npcNavMeshAgent.isActiveAndEnabled)
            {
                _npcNavMeshAgent.isStopped = true;
            }
        }

        protected override void OnPostResumed()
        {
            base.OnPostResumed();
            _isGamePaused = false;
            if (!_npcNavMeshAgent.IsUnityNull() && _npcNavMeshAgent.isActiveAndEnabled)
            {
                _npcNavMeshAgent.isStopped = false;
            }
        }

        /// <summary>
        /// Llama a comprobar si el jugador esta bien vestido. Si esta mal vestido, un npc colisionará con el
        /// y le tirará acido encima matandolo
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player")) return;

            Random random = new Random();
            int timeBeforeChase = random.Next(5, 10);
            
            if (checker.CheckClothes())
            {
                CancelChase();
            }
            else
            {
                SetAlumnoChaser();

                _playerInteractor.enabled = false;
                this.enabled = false;
                alumno.layer = LayerMask.NameToLayer("Default");
                
                StartCoroutine(WaitBeforeChase(timeBeforeChase));
            }
        }

        /// <summary>
        /// Prepara al NPC previamente a tirarle el acido al jugador
        /// </summary>
        public void SetAlumnoChaser()
        {
            // Establece la posicion de la bandeja
            bandeja.transform.parent = bandejaPosition.transform;
            bandeja.transform.localPosition = Vector3.zero;

            // Añade el NavMeshAgent al NPC
            if(!alumno.TryGetComponent<NavMeshAgent>(out _npcNavMeshAgent))
            {
                _npcNavMeshAgent = alumno.AddComponent<NavMeshAgent>();
            }

        }

        /// <summary>
        /// Desactiva el script si el NPC esta bien vestido
        /// </summary>
        public void CancelChase()
        {
            bandejaPosition.transform.parent = propParent.transform;
            Destroy(this);
        }
        
        /// <summary>
        /// Corrutina que espera un tiempo antes de perseguir al jugador
        /// </summary>
        /// <param name="delayBeforeChase">Tiempo de espera</param>
        /// <param name="navMeshAgent">NavMeshAgent del npc</param>
        /// <returns></returns>
        private IEnumerator WaitBeforeChase(int delayBeforeChase)
        {
            if (_isGamePaused)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(delayBeforeChase);

            alumnoState?.ChangeState(NpcStates.Walking);
            
            StartCoroutine(ChasePlayer());
        }

        /// <summary>
        /// Corrutina que persigue al jugador hasta colisionar con el y tirarle el acido encima
        /// </summary>
        /// <param name="npcNavMeshAgent">NavMeshAgent del npc</param>
        /// <returns></returns>
        private IEnumerator ChasePlayer()
        {
            alumno.TryGetComponent<NavMeshObstacle>(out var obstacle);

            if(obstacle != null)
            {
                obstacle.enabled = false;
            }

            _npcNavMeshAgent.enabled = true;

            while (true)
            {
                if (_isGamePaused)
                {
                    yield return null;
                }
                
                float distanceToPlayer = Vector3.Distance(player.transform.position, alumno.transform.position);

                _npcNavMeshAgent?.SetDestination(player.transform.position);

                if (distanceToPlayer < 2)
                {
                    _npcNavMeshAgent.isStopped = true;
                    break;
                }

                yield return null;
            }

            var effect = Instantiate(poisonParticleSystem, player.transform.position, Quaternion.LookRotation(Vector3.up));
            effect.transform.parent = player.transform;
            dropAcidDialogue?.TriggerEvent();
            
            alumnoState.ChangeState(NpcStates.Idle);

            StartCoroutine(PlayerDeath());
        }

        /// <summary>
        /// Abre la escena de muerte despues de unos segundos
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayerDeath()
        {
            if (_isGamePaused)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(5f);
            
            DeathInvoker.Instance.KillAnimation(GameLevels.LevelRecibidor, "Te ha caido ácido encima", 5);
            
            Destroy(this); 
        }

    }
}
