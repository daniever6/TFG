using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    
    public class DialogueManager : Singleton<DialogueManager>
    {
        #region Class implementation

        [SerializeField] private GameObject _dialogueCanvas;
        
        [SerializeField] private Image _unitImage;
        [SerializeField] private TextMeshProUGUI _unitNameText;
        [SerializeField] private TextMeshProUGUI _dialogText;
        [SerializeField] private float _lettersWaitVelocity = 0f;
        
        private Queue<string> _sentences;
        private Queue<Dialogue> _dialogues;

        private void Start()
        {
            _sentences = new Queue<string>();
            _dialogues = new Queue<Dialogue>();
        }
        
        #endregion

        #region Dialogue Logic Methods
        
        /// <summary>
        /// Almacena todos los dialogos dentro de una cola de dialogos para mostrarlos
        /// </summary>
        /// <param name="dialogues">Dialogos a almacenar</param>
        public void GetDialogues(Dialogue[] dialogues)
        {
            GameManager.Instance.ChangeState(GameState.Dialogue);
            
            _dialogueCanvas.SetActive(true);
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
        public void StartDialogue(Dialogue dialogue)
        {
            //_unitImage.sprite = dialogue.UnitImage;
            _unitNameText.text = dialogue.UnitName;
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
        public void EndDialogue()
        {
            _dialogueCanvas.SetActive(false);
            GameManager.Instance.ChangeState(GameState.Resume);
        }

        /// <summary>
        /// Método para que las letras de los dialogos salgan una a una mediante una corrutina
        /// </summary>
        /// <param name="sentence">Frase a mostrar</param>
        /// <returns></returns>
        IEnumerator TypeSentence(string sentence)
        {
            _dialogText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                _dialogText.text += letter;
                yield return new WaitForSeconds(_lettersWaitVelocity);
            }
        }
        
        #endregion
    }
}