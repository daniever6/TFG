using _Scripts.Dialogues;
using _Scripts.Managers;
using Cinemachine;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_02
{
    public class StartDialogueLevel02 : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera npcCamera;
        [SerializeField] private DialogueTrigger startDialogue;

        private void Start()
        {
            DialogueManager.OnDialogueFinish += FinishStartDialogue;
            startDialogue.TriggerEvent();
        }

        private void OnDestroy()
        {
            DialogueManager.OnDialogueFinish -= FinishStartDialogue;
        }

        /// <summary>
        /// Desactiva la camera del NPC y destruye el script
        /// </summary>
        private void FinishStartDialogue()
        {
            npcCamera.enabled = false;
            Destroy(this);
        }
    }
}
