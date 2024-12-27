using _Scripts.Dialogues;
using _Scripts.LevelScripts.Level_01;
using _Scripts.Managers;
using _Scripts.UI;
using _Scripts.Utilities;
using Cinemachine;
using JetBrains.Annotations;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Scripts.NPCs
{
    public class BurnNpc : GameplayMonoBehaviour<BurnNpc>
    {
        [SerializeField] private NpcState state;                            // Estado del NPC
        [SerializeField] private ParticleEffectManager particlesManager;    // Instanciador de particulas
        [SerializeField] private CinemachineVirtualCamera npcCamera;        // Camara que apunta al NPC

        [SerializeField][CanBeNull] private DialogueTrigger helpDialogue;    // Dialogo del npc 
        [SerializeField][CanBeNull] private DialogueTrigger thanksDialogue;    // Dialogo del npc 

        private bool isTalking = false;
        private static bool isActivate = false;
        private GameObject activeVFX;

        private void Start()
        {
            npcCamera.enabled = false;
        }

        private void OnEnable()
        {
            DialogueManager.OnDialogueFinish += PointAtPlayer;
        }

        private void OnDisable()
        {
            DialogueManager.OnDialogueFinish -= PointAtPlayer;
        }

        /// <summary>
        /// Activa el evento de NPC ardiendo
        /// </summary>
        public void StartBurning()
        {
            if (isActivate)
            {
                return;
            }

            isActivate = true;

            PointAtNPC();

            state.ChangeState(NpcStates.Burning);

            //Instancia particulas
            activeVFX = particlesManager.InstantiateParticleInPos("Fuego", transform);

            activeVFX.SetActive(true);
        }

        /// <summary>
        /// Metodo que activa la camara para enfocar al npc
        /// </summary>
        private async void PointAtNPC()
        {
            npcCamera.enabled = true;

            InfoCanvas.Instance.ShowMessage("Ayuda al compañero que se está quemando");

            if (!helpDialogue.IsUnityNull())
            {
                helpDialogue.TriggerEvent();
            }
            else
            {
                await Task.Delay(4000);
                PointAtPlayer();
            }
            
        }

        /// <summary>
        /// Desactiva la camara del NPC para volver a apuntar al jugador
        /// </summary>
        private void PointAtPlayer()
        {
            npcCamera.enabled = false;

            isTalking = false;
        }

        /// <summary>
        /// Detiene el evento de fuego en NPC
        /// </summary>
        public void StopBurning()
        {
            if (!isActivate)
            {
                return;
            }

            isActivate = false;

            state.ChangeState(NpcStates.Idle);

            // Detiene el sistema de particulas
            if (!activeVFX.IsUnityNull())
            {
                activeVFX.TryGetComponent<ParticleSystem>(out ParticleSystem currentParticle);

                currentParticle?.Stop();

                DestroyImmediate(currentParticle, true);
                DestroyImmediate(activeVFX);

                activeVFX = null;
            }
            

            if (!thanksDialogue.IsUnityNull())
            {
                isTalking = true;
                
                thanksDialogue.TriggerEvent();
            }

            this.enabled = false;

            
        }
    }
}
