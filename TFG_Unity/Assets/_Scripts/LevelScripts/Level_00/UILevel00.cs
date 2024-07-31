using System.Collections.Generic;
using _Scripts.Utilities;
using Cinemachine;
using TMPro;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_00
{
    public class UILevel00 : GameplayMonoBehaviour<UILevel00>
    {
        [SerializeField] private Canvas clothingCanvas;
        [SerializeField] private CinemachineVirtualCamera generalCamera;
        [SerializeField] private List<GameObject> bodyCameras;
        [SerializeField] private List<GameObject> bodyUIPanels;
        [SerializeField] private TextMeshProUGUI headerTitle;
        private string[] _bodyParts = { "Peinado", "Torso", "Manos", "Pantalones", "Calzado" };
        private int _currentBodyPart = 0;
        private bool _isCanvasOpen = true;

        /// <summary>
        /// Cambia la parte del cuerpo en la UI del selector de ropa
        /// </summary>
        /// <param name="direction">(+1) para avanzar. (-1) para retroceder</param>
        public void ChangeBodyPart(int direction)
        {
            bodyUIPanels[_currentBodyPart].SetActive(false);
            bodyCameras[_currentBodyPart].SetActive(false);

            _currentBodyPart = (_currentBodyPart + direction) % (_bodyParts.Length);
            if (_currentBodyPart < 0)
            {
                _currentBodyPart = _bodyParts.Length - 1;
            }
            
            bodyUIPanels[_currentBodyPart].SetActive(true);
            headerTitle.text = _bodyParts[_currentBodyPart];
            bodyCameras[_currentBodyPart].SetActive(true);
        }

        /// <summary>
        /// Activa el canvas de selector de ropa
        /// </summary>
        public void OpenClothSelector()
        {
            _isCanvasOpen = true;
            
            clothingCanvas.enabled = true;
            _currentBodyPart = 0;
            
            bodyUIPanels[_currentBodyPart].SetActive(true);
            headerTitle.text = _bodyParts[_currentBodyPart];
            bodyCameras[_currentBodyPart].SetActive(true);
        }

        /// <summary>
        /// Desactiva el canvas de selector de ropa y aplica los cambios
        /// </summary>
        public void AcceptChanges()
        {
            _isCanvasOpen = false;
            
            clothingCanvas.enabled = false;

            bodyUIPanels[_currentBodyPart].SetActive(false);
            bodyCameras[_currentBodyPart].SetActive(false);
            
            generalCamera.enabled = true;
        }

        protected override void OnPostPaused()
        {
            base.OnPostPaused();
            clothingCanvas.enabled = false;
        }

        protected override void OnPostResumed()
        {
            base.OnPostResumed();
            if (_isCanvasOpen)
            {
                clothingCanvas.enabled = true;
            }
        }
    }
}
