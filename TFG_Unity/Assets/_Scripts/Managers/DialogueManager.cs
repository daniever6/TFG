using System.Collections;
using System.Collections.Generic;
using Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        #region Class implementation
        
        [SerializeField] private Image _unitImage;
        [SerializeField] private TextMeshProUGUI _unitNameText;
        [SerializeField] private TextMeshProUGUI _dialogText;
        [SerializeField] private float _lettersWaitVelocity = 0f;
        private Queue<string> _sentences;

        private void Start()
        {
            _sentences = new Queue<string>();
        }
        
        #endregion

        #region Dialogue Logic Methods

        /// <summary>
        /// Lleva la lógica del comienzo de los dialogos con los personajes
        /// </summary>
        /// <param name="dialogue"></param>
        public void StartDialog(Dialogue dialogue)
        {
            _unitImage.sprite = dialogue.UnitImage;
            _unitNameText.text = dialogue.UnitName;
            _sentences.Clear();
            
            foreach (string sentence in dialogue.Sentences)
            {
                _sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        /// <summary>
        /// Muestra la siguiente frase del dialogo, al acabar llama a EndDialog
        /// </summary>
        public void DisplayNextSentence()
        {
            if (_sentences.Count == 0)
            {
                EndDialog();
                return;
            }

            string sentence = _sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        public void EndDialog()
        {
            
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