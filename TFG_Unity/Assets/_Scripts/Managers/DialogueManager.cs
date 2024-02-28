using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public enum DialogueType
    {
        OneUnitDialogue,
        ManyUnitsDialogue
    }
    
    public class DialogueManager : Singleton<DialogueManager>
    {
        #region Class implementation

        [SerializeField] private GameObject _pauseCanvas;
        
        [SerializeField] private Image _unitImage;
        [SerializeField] private TextMeshProUGUI _unitNameText;
        [SerializeField] private TextMeshProUGUI _dialogText;
        [SerializeField] private float _lettersWaitVelocity = 0f;

        private DialogueType _dialogueType;
        private Queue<string> _sentences;
        private Queue<DialogueSerializable> _dialogues;

        private void Start()
        {
            _sentences = new Queue<string>();
            _dialogues = new Queue<DialogueSerializable>();
        }
        
        #endregion

        #region Dialogue Logic Methods

        /// <summary>
        /// Lleva la lógica del comienzo de los dialogos con los personajes
        /// </summary>
        /// <param name="dialogue"></param>
        public void StartDialogue(Dialogue dialogue)
        {
            _pauseCanvas.SetActive(true);
            _dialogueType = DialogueType.OneUnitDialogue;
            
            //_unitImage.sprite = dialogue.UnitImage;
            _unitNameText.text = dialogue.UnitName;
            _sentences.Clear();
            
            foreach (string sentence in dialogue.Sentences)
            {
                _sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        public void CinematicDialogue(Queue<DialogueSerializable> dialogues)
        {
            _pauseCanvas.SetActive(true);
            _dialogueType = DialogueType.ManyUnitsDialogue;

            _dialogues.Clear();
            foreach (var dialogue in dialogues)
            {
                _dialogues.Enqueue(dialogue);
            }
            
            DisplayNextSentence();
        }

        /// <summary>
        /// Muestra la siguiente frase del dialogo, al acabar llama a EndDialog
        /// </summary>
        public void DisplayNextSentence()
        {
            switch (_dialogueType)
            {
                case DialogueType.OneUnitDialogue:
                    DisplayOneUnitDialogue();
                    break;
                
                case DialogueType.ManyUnitsDialogue:
                    DisplayManyUnitDialogues();
                    break;
            }
        }

        private void DisplayOneUnitDialogue()
        {
            if (_sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            
            string sentence = _sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        
        private void DisplayManyUnitDialogues()
        {
            if (_dialogues.Count == 0)
            {
                EndDialogue();
                return;
            }
            DialogueSerializable currentDialogue = _dialogues.Dequeue();
            //_unitImage.sprite = currentDialogue.UnitImage;
            _unitNameText.text = currentDialogue.UnitName;
            
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentDialogue.Sentences));
        }

        /// <summary>
        /// Cierra la pestaña de los dialogos
        /// </summary>
        public void EndDialogue()
        {
            _pauseCanvas.SetActive(false);
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