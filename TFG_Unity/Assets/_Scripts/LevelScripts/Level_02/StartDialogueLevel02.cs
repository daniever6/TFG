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
        [SerializeField] private GameObject entregarButton;

        private void Start()
        {
            enabled = true;
            entregarButton.SetActive(false);
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
            DialogueManager.OnDialogueFinish -= FinishStartDialogue;
            npcCamera.enabled = false;
            entregarButton.SetActive(true);
            enabled = false;
        }
    }
}
