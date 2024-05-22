using System;
using _Scripts.Managers;
using _Scripts.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class InfoCanvas : Singleton<InfoCanvas>
    {
        [SerializeField] private GameObject canvasPanel;
        [SerializeField] private TextMeshProUGUI panelText;
        [SerializeField] private GameObject repeatMessageButton;
        private bool _isShowing = false;
        private string _messageText = String.Empty;
        private bool _messageShown = false;


        private void Start()
        {
            GameManager.OnBeforeGameStateChanged += DisplayCanvas;
            repeatMessageButton.SetActive(false);
            
        }

        public void RepeatAnimation()
        {
            ShowPanelAnim();
        }

        public void ShowMessage(string newText)
        {
            if (!_messageText.Equals(newText))
            {
                _messageShown = false;
            }

            _messageText = newText;
            
            if (_messageText == String.Empty)
            {
                repeatMessageButton.SetActive(false);
                return;
            }
            
            DisplayCanvas(GameManager.GameState);
        }

        public void DisplayCanvas(GameState gameState)
        {
            if (!_messageShown && gameState == GameState.Resume)
            {
                ShowPanelAnim();
            }
        }

        /// <summary>
        /// Muestra una ventana de aviso o consejo con un texto a mostrar al usuario
        /// </summary>
        /// <param name="textToDisplay"></param>
        private void ShowPanelAnim()
        {
            if (_isShowing) return;
            
            _messageShown = true;
            _isShowing = true;
            
            repeatMessageButton.SetActive(false);
            
            canvasPanel.SetActive(true);
            panelText.text = _messageText;
            canvasPanel.transform.DOMoveX(650, 2f).SetEase(Ease.InOutBack);
            
            Invoke("HidePanelAnim", 8f);
        }

        /// <summary>
        /// Esconde el panel de aviso/Consejo con una animacion
        /// </summary>
        private void HidePanelAnim()
        {
            canvasPanel.transform.DOMoveX(1000, 2f).SetEase(Ease.InOutBack)
                .OnComplete(() =>
                {
                    canvasPanel.SetActive(false);
                    repeatMessageButton.SetActive(true);
                    _isShowing = false;
                });
            
        }
    }
}
