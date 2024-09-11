using _Scripts.Managers;
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

        public override void TriggerEvent()
        {
            dialogueManager.GetDialogues(new []{dialogue});
        }
    }
}