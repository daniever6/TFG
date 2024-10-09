using System;
using System.Collections.Generic;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_02._1
{
    public class SubirVentanaExtractora : MonoBehaviour
    {
        [SerializeField] private PlayerHand hand;
        [SerializeField] private GameObject ventana;
        [SerializeField] private Collider ventanaCollider;
        [SerializeField] private List<Material> materialesVentana;
        private Vector3 _mousePosition;
        private Camera _camera;
        
        private Vector3 initialPos; //Posicion inicial
        private float upperLimitY; //Limite de altura
        private float safeMinUpperPosY; //Altura minima segura
        private float safeMaxUpperPosY; //Altura maxima segura
        
        

        private void Start()
        {
            foreach (var material in materialesVentana)
            {
                Color currentColor = material.color;
                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1);

                material.color = newColor;
            }
            
            _camera = Camera.main;
            initialPos = ventana.transform.position;
            
            upperLimitY = initialPos.y + 0.5f;
            safeMinUpperPosY = initialPos.y + 0.15f;
            safeMaxUpperPosY = initialPos.y + 0.25f;
        }
        
        /// <summary>
        /// Devuelve la posicion del raton en el mundo
        /// </summary>
        /// <returns>Vector 3 de la posicion virtual del raton</returns>
        private Vector3 GetMousePosition()
        {
            return _camera.WorldToScreenPoint(transform.position);
        }

        
        /// <summary>
        /// Calcula la posicion del raton y agarra el agarre de la ventana
        /// </summary>
        private void OnMouseDown()
        {
            _mousePosition = Input.mousePosition - GetMousePosition();
            hand.transform.position = transform.position;
        }

        /// <summary>
        /// Devuelve la mano a su posicion inicial cuando se levanta el raton y calcula la opacidad de los materiales
        /// </summary>
        private void OnMouseUp()
        {
            hand.GoToInitialPosition();

            ///Calcula el nuevo alpha del color
            float alphaValue = 1f;
            if (ventana.transform.position.y >= safeMinUpperPosY)
            {
                ventanaCollider.enabled = false;
                alphaValue = 0f;
            }
            else
            {
                ventanaCollider.enabled = true;
                alphaValue = 1;
            }
            
            // Aplica el nuevo color
            foreach (var material in materialesVentana)
            {
                Color currentColor = material.color;
                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, alphaValue);

                material.color = newColor;
            }
        }

        /// <summary>
        /// Calcula la posicion de la altura de la ventana mientras se hace el drag del raton
        /// </summary>
        private void OnMouseDrag()
        {
            if (PlayerGrab.IsTweening) return;
            
            Vector3 newPosition = _camera.ScreenToWorldPoint(Input.mousePosition - _mousePosition);

            //newPosition.y += 0.25f;
            
            float clampedY = Mathf.Clamp(newPosition.y, initialPos.y, upperLimitY);

            ventana.transform.position = new Vector3(initialPos.x, clampedY, initialPos.z);
            
            hand.transform.position = transform.position;
        }

        /// <summary>
        /// Devuelve si la posicion actual de la ventana es segura o no
        /// </summary>
        /// <returns>True si es segura y false en caso contrario</returns>
        private bool IsAlturaSegura()
        {
            var position = ventana.transform.position;
            return position.y >= safeMinUpperPosY && position.y <= safeMaxUpperPosY;
        }
    }
}
