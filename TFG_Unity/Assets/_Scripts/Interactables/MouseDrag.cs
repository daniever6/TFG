using System;
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

        private Vector3 GetMousePosition()
        {
            return _camera.WorldToScreenPoint(transform.position);
        }

        private void OnMouseDown()
        {
            _mousePosition = Input.mousePosition - GetMousePosition();
        }

        private void OnMouseDrag()
        {
            transform.position = _camera.ScreenToWorldPoint(Input.mousePosition - _mousePosition);
        }
    
    }
}
