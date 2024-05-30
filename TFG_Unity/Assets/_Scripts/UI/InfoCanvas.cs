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
        private RectTransform panelRectTransform;
        private bool _isShowing = false;
        private string _messageText = String.Empty;
        private bool _messageShown = false;


        private void Start()
        {
            GameManager.OnBeforeGameStateChanged += DisplayCanvas;
            repeatMessageButton.SetActive(false);
            panelRectTransform = canvasPanel.GetComponent<RectTransform>();
        }

        public void RepeatAnimation()
        {
            ShowPanelAnim();
        }

        /// <summary>
        /// Ejecuta el texto indicado en una pantalla de informacion
        /// </summary>
        /// <param name="newText">Texto a mostrar</param>
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

        /// <summary>
        /// Ejecuta la animacion de la pantalla de informacion
        /// </summary>
        /// <param name="gameState">Comprueba si gameState esta en Resume para ejecutarlo</param>
        public void DisplayCanvas(GameState gameState)
        {
            if (!_messageShown && gameState == GameState.Resume && _messageText != String.Empty)
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
            panelRectTransform.DOAnchorPosX(-10, 2f).SetEase(Ease.InOutBack);
            
            Invoke("HidePanelAnim", 8f);
        }

        /// <summary>
        /// Esconde el panel de aviso/Consejo con una animacion
        /// </summary>
        private void HidePanelAnim()
        {
            panelRectTransform.DOAnchorPosX(400, 2f).SetEase(Ease.InOutBack)
                .OnComplete(() =>
                {
                    canvasPanel.SetActive(false);
                    repeatMessageButton.SetActive(true);
                    _isShowing = false;
                });
            
        }
    }
}
