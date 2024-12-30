using _Scripts.Managers;
using _Scripts.NPCs;
using _Scripts.Utilities;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.Dialogues
{
    public class DialogueTrigger : Trigger
    {
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private Dialogue dialogue;

        private NpcStates npcPreviousState = NpcStates.None;

        private NpcState npcState;
        private NpcRotator npcRotator;

        private void Start()
        {
            TryGetComponent<NpcRotator>(out npcRotator);
            TryGetComponent<NpcState>(out npcState);

            DialogueManager.OnDialogueFinish += EndDialogue;

            if (dialogueManager.IsUnityNull())
            {
                dialogueManager = DialogueManager.Instance;
            }
        }

        private void OnDisable()
        {
            DialogueManager.OnDialogueFinish -= EndDialogue;
        }

        /// <summary>
        /// Llama a mostrar los dialogos
        /// </summary>
        public override void TriggerEvent()
        {
            if(dialogueManager.IsUnityNull() || (npcState != null && npcState.IsDying()))
            {
                return;
            }

            // Accion de hablar con el jugador
            if(npcRotator != null)
            {
                npcRotator.RotateToPlayer();
            }

            if (!npcState.IsUnityNull()) 
            {
                npcPreviousState = npcState.State;

                npcState.ChangeState(NpcStates.Talking);
            }

            dialogueManager.GetDialogues(new []{dialogue});
        }

        /// <summary>
        /// Vuelve el NPC a su posicion inicial
        /// </summary>
        private void EndDialogue()
        {
            if(npcRotator != null)
            {
                npcRotator.RotateToInitial();
            }

            npcState?.ChangeState(npcPreviousState);
        }
    }
}