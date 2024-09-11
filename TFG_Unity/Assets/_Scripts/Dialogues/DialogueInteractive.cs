using _Scripts.Managers;
using _Scripts.NPCs;
using _Scripts.Player;
using _Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Dialogues
{
    public class DialogueInteractive : Trigger
    {
        [SerializeField] private Canvas npcDialogueCanvas;
        [SerializeField] private NpcRotator npcRotator;

        private void Start()
        {
            HideNpcCanvas();
            
            NpcRotator.OnRotateToStartPos += HideNpcCanvas;
        }

        /// <summary>
        /// Gira el npc hacia el jugador y activa el Canvas del NPC
        /// </summary>
        public override void TriggerEvent()
        {
            PlayerController.Instance.enabled = false;
            
            npcRotator.RotateToPlayer();
            npcDialogueCanvas.gameObject.SetActive(true);
        }

        /// <summary>
        /// Oculta el panel de conversacion del NPC
        /// </summary>
        private void HideNpcCanvas()
        {
            if (npcDialogueCanvas.IsUnityNull())
            {
                return;
            }   
            
            npcDialogueCanvas.gameObject.SetActive(false);
            
            PlayerController.Instance.enabled = true;
        }
    }
}
