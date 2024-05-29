using System;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts.Interactables
{
    /// <summary>
    /// Clase que permite arrastrar objetos de la escena
    /// </summary>
    public class MouseDrag : MonoBehaviour
    {
        private Vector3 _mousePosition;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        /// <summary>
        /// Devuelve la posición del objeto actual respecto a la posicion de la camara
        /// </summary>
        /// <returns></returns>
        private Vector3 GetMousePosition()
        {
            return _camera.WorldToScreenPoint(transform.position);
        }

        /// <summary>
        /// Traslada la el objeto a la posicion del raton on click
        /// </summary>
        private void OnMouseDown()
        {
            _mousePosition = Input.mousePosition - GetMousePosition();
        }

        /// <summary>
        /// Traslada el objeto a la posicion del objeto mientras se mantiene presionado
        /// </summary>
        private void OnMouseDrag()
        {
            if (PlayerGrab.IsTweening) return;
            transform.position = _camera.ScreenToWorldPoint(Input.mousePosition - _mousePosition);
        }
    
    }
}
