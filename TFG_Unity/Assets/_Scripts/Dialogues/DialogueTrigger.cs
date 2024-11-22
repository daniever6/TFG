using _Scripts.Managers;
using _Scripts.NPCs;
using _Scripts.Utilities;
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.Dialogues
{
    public class DialogueTrigger : Trigger
    {
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private Dialogue dialogue;
        private NpcRotator npcRotator;

        private void Start()
        {
            if(TryGetComponent<NpcRotator>(out var rotator))
            {
                npcRotator = rotator;
            }

            DialogueManager.OnDialogueFinish += EndDialogue;
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
            if(npcRotator != null)
            {
                npcRotator.RotateToPlayer();
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
        }
    }
}