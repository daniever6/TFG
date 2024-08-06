using System;
using System.Collections.Generic;
using _Scripts.Utilities;
using Cinemachine;
using TMPro;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_00
{
    public class UISelectorRopa : GameplayMonoBehaviour<UISelectorRopa>
    {
        [SerializeField] private Canvas clothingCanvas;
        [SerializeField] private Canvas endEditCanvas;
        private Canvas currentActiveCanvas;
        
        [SerializeField] private CinemachineVirtualCamera generalCamera;
        [SerializeField] private List<GameObject> bodyCameras;
        [SerializeField] private List<GameObject> bodyUIPanels;
        [SerializeField] private TextMeshProUGUI headerTitle;
        
        private string[] _bodyParts = { "Peinado", "Torso", "Manos", "Pantalones", "Calzado" };
        private int _currentBodyPart = 0;
        private bool _isCanvasOpen = true;

        private void Start()
        {
            _isCanvasOpen = true;
            currentActiveCanvas = clothingCanvas;
            currentActiveCanvas.gameObject.SetActive(true);
            endEditCanvas.gameObject.SetActive(false);
        }

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
            ChangeCurrentActiveCanvas(clothingCanvas);
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
            ChangeCurrentActiveCanvas(endEditCanvas);

            bodyUIPanels[_currentBodyPart].SetActive(false);
            bodyCameras[_currentBodyPart].SetActive(false);
            
            generalCamera.enabled = true;
        }

        /// <summary>
        /// Activa el nuevo canvas del nivel y desactiva el canvas que estaba activo
        /// </summary>
        /// <param name="canvas">Nuevo canvas a activar</param>
        private void ChangeCurrentActiveCanvas(Canvas canvas)
        {
            currentActiveCanvas.gameObject.SetActive(false);

            currentActiveCanvas = canvas;

            currentActiveCanvas.gameObject.SetActive(true);
        }

        protected override void OnPostPaused()
        {
            base.OnPostPaused();

            currentActiveCanvas.enabled = false;
        }

        protected override void OnPostResumed()
        {
            base.OnPostResumed();
            if (_isCanvasOpen)
            {
                currentActiveCanvas.enabled = true;
            }
        }
    }
}
