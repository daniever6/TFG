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
        [SerializeField] private Image _unitImage;
        [SerializeField] private TextMeshProUGUI _unitNameText;
        [SerializeField] private TextMeshProUGUI _dialogText;
        private Queue<string> _sentences;

        private void Start()
        {
            _sentences = new Queue<string>();
        }

        public void StartDialog(Dialogue dialogue)
        {
            _unitImage = dialogue.UnitImage;
            _unitNameText.text = dialogue.UnitName;
            _sentences.Clear();
            
            foreach (string sentence in dialogue.Sentences)
            {
                _sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

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

        IEnumerator TypeSentence(string sentence)
        {
            _dialogText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                _dialogText.text += letter;
                yield return null;
            }
        }
    }
}