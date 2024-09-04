using System;
using System.Collections;
using _Scripts.Dialogues;
using _Scripts.LevelScripts.Level_00;
using _Scripts.Managers;
using _Scripts.Player;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace _Scripts.LevelScripts.Lab_Scripts
{
    public class ClothCheckTrigger : GameplayMonoBehaviour<ClothCheckTrigger>
    {
        [SerializeField] private CheckRopaCorrecta checker;
        [SerializeField] private GameObject npc;
        [SerializeField] private ParticleSystem poisonParticleSystem;
        [SerializeField] private GameObject player;
        private PlayerInteractor _playerInteractor;
        
        private NavMeshAgent _npcNavMeshAgent;
        private bool _isGamePaused = false;

        private void Start()
        {
            player.TryGetComponent(out _playerInteractor);
            _npcNavMeshAgent = npc.GetComponent<NavMeshAgent>();
        }
        
        protected override void OnPostPaused()
        {
            base.OnPostPaused();
            _isGamePaused = true;
            _npcNavMeshAgent.isStopped = true;
        }

        protected override void OnPostResumed()
        {
            base.OnPostResumed();
            _isGamePaused = false;
            _npcNavMeshAgent.isStopped = false;
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
                Destroy(this);
            }
            else
            {
                _playerInteractor.enabled = false;
                this.enabled = false;
                npc.layer = LayerMask.NameToLayer("Default");
                
                StartCoroutine(WaitBeforeChase(timeBeforeChase));
            }
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
            
            StartCoroutine(ChasePlayer());
        }

        /// <summary>
        /// Corrutina que persigue al jugador hasta colisionar con el y tirarle el acido encima
        /// </summary>
        /// <param name="npcNavMeshAgent">NavMeshAgent del npc</param>
        /// <returns></returns>
        private IEnumerator ChasePlayer()
        {
            while (true)
            {
                if (_isGamePaused)
                {
                    yield return null;
                }
                
                float distanceToPlayer = Vector3.Distance(player.transform.position, npc.transform.position);

                _npcNavMeshAgent.SetDestination(player.transform.position);

                if (distanceToPlayer < 2)
                {
                    _npcNavMeshAgent.isStopped = true;
                    break;
                }

                yield return null;
            }

            var effect = Instantiate(poisonParticleSystem, player.transform.position, Quaternion.LookRotation(Vector3.up));
            effect.transform.parent = player.transform;
            npc.GetComponent<DialogueTrigger>().TriggerEvent();

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
            
            DeathInvoker.Instance.KillAnimation(GameLevels.Level0, "Te ha caido ácido encima", 5);
            
            Destroy(this); 
        }

    }
}
