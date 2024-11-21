using _Scripts.Dialogues;
using _Scripts.Managers;
using _Scripts.Utilities;
using Cinemachine;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_02
{
    public class StartDialogueLevel02 : GameplayMonoBehaviour<StartDialogueLevel02>
    {
        [SerializeField] private CinemachineVirtualCamera npcCamera;
        [SerializeField] private DialogueTrigger startDialogue;
        [SerializeField] private GameObject entregarButton;
        [SerializeField] private Animator npcAnimator;

        private void Start()
        {
            npcAnimator.Play("Talk");
            npcAnimator.speed = 1f;

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
            npcAnimator.Play("Idle");
            DialogueManager.OnDialogueFinish -= FinishStartDialogue;
            npcCamera.enabled = false;
            entregarButton.SetActive(true);
            enabled = false;
        }

        protected override void OnPostPaused()
        {
            base.OnPostPaused();
        }

        protected override void OnPostResumed()
        {
            base.OnPostResumed();
        }
    }
}
