using System.Runtime.CompilerServices;
using Managers;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Utilities;

namespace Dialogues
{
    public class DialogueTrigger : Trigger
    {
        [SerializeField] private DialogueManager _dialogueManager;
        [SerializeField] private Dialogue _dialogue;

        public override void TriggerEvent()
        {
            _dialogueManager.StartDialog(_dialogue);
        }
    }
}