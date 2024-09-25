using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Dialogues;
using _Scripts.UI.Scripts;
using _Scripts.Utilities;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    
    public class DialogueManager : Utilities.Singleton<DialogueManager>
    {
        #region Class implementation

        public static event Action OnDialogueFinish; 

        [SerializeField] private GameObject dialogueCanvas;
        
        [SerializeField] private Image unitImage;
        [SerializeField] private TextMeshProUGUI unitNameText;
        [SerializeField] private TextMeshProUGUI dialogText;
        [SerializeField] private float lettersWaitVelocity = 0f;
        
        private static Queue<string> _sentences = new();
        private static Queue<Dialogue> _dialogues = new();
        
        #endregion

        #region Dialogue Logic Methods
        
        /// <summary>
        /// Almacena todos los dialogos dentro de una cola de dialogos para mostrarlos
        /// </summary>
        /// <param name="dialogues">Dialogos a almacenar</param>
        public void GetDialogues(Dialogue[] dialogues)
        {
            if (UnityObjectUtility.IsUnityNull(dialogues))
            {
                return;
            }
            
            GameManager.Instance.ChangeState(GameState.Dialogue);
            
            dialogueCanvas.SetActive(true);
            _dialogues.Clear();
            
            foreach (Dialogue dialogue in dialogues)
            {
                _dialogues.Enqueue(dialogue);
            }
            
            DisplayNextDialogue();
        }
        
        /// <summary>
        /// Muestra la siguiente frase del dialogo, al acabar llama a EndDialog
        /// </summary>
        public void DisplayNextDialogue()
        {
            if (_dialogues.Count == 0)
            {
                EndDialogue();
                return;
            }

            Dialogue dialogue = _dialogues.Dequeue();
            StartDialogue(dialogue);
        }
        
        /// <summary>
        /// Lleva la lógica del comienzo de los dialogos con los personajes
        /// </summary>
        /// <param name="dialogue"></param>
        private void StartDialogue(Dialogue dialogue)
        {
            //_unitImage.sprite = dialogue.UnitImage;
            unitNameText.text = dialogue.UnitName;
            _sentences.Clear();
            
            foreach (string sentence in dialogue.Sentences)
            {
                _sentences.Enqueue(sentence);
            }

            DisplayDialogueSentence();
        }

        public void DisplayDialogueSentence()
        {
            if (_sentences.Count == 0)
            {
                DisplayNextDialogue();

                return;
            }
            
            string sentence = _sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
            
        }

        /// <summary>
        /// Cierra la pestaña de los dialogos
        /// </summary>
        private void EndDialogue()
        {
            dialogueCanvas.SetActive(false);
            OnDialogueFinish?.Invoke();
            GameManager.Instance.ChangeState(GameState.Resume);
        }

        /// <summary>
        /// Método para que las letras de los dialogos salgan una a una mediante una corrutina
        /// </summary>
        /// <param name="sentence">Frase a mostrar</param>
        /// <returns></returns>
        private IEnumerator TypeSentence(string sentence)
        {
            dialogText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogText.text += letter;
                yield return new WaitForSeconds(lettersWaitVelocity);
            }
        }
        
        #endregion
    }
}