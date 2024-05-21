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
        private bool _isShowing = false;

        /// <summary>
        /// Muestra una ventana de aviso o consejo con un texto a mostrar al usuario
        /// </summary>
        /// <param name="textToDisplay"></param>
        public void ShowPanel(string textToDisplay)
        {
            if (_isShowing) return;
            
            _isShowing = true;
            
            canvasPanel.SetActive(true);
            panelText.text = textToDisplay;
            canvasPanel.transform.DOMoveX(650, 2f).SetEase(Ease.InBounce);
            
            Invoke("HidePanel", 8f);
        }

        /// <summary>
        /// Esconde el panel de aviso/Consejo con una animacion
        /// </summary>
        private void HidePanel()
        {
            canvasPanel.transform.DOMoveX(1000, 2f).SetEase(Ease.InOutBack)
                .OnComplete(() =>
                {
                    canvasPanel.SetActive(false);
                    _isShowing = false;
                });
            
        }
    }
}
