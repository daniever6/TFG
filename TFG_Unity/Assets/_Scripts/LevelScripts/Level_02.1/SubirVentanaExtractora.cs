using System;
using System.Collections.Generic;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_02._1
{
    public class SubirVentanaExtractora : MonoBehaviour
    {
        [SerializeField] private PlayerHand hand;
        [SerializeField] private GameObject ventanaParent;
        [SerializeField] private GameObject ventana;
        [SerializeField] private GameObject ventanaSuperior;
        [SerializeField] private Collider ventanaCollider;
        [SerializeField] private Material materialVentanaOriginal;
        [SerializeField] private Material materialVentanaTransparente;

        private Renderer ventanaRenderer;
        private Vector3 _mousePosition;
        private Camera _camera;

        public static bool IsTooHigh = false;
        
        private Vector3 initialPos; //Posicion inicial
        private float upperLimitY; //Limite de altura
        private float safeMinUpperPosY; //Altura minima segura
        private float safeMaxUpperPosY; //Altura maxima segura
        
        

        private void Start()
        {
            ventanaRenderer = ventana.GetComponent<MeshRenderer>();

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

                ventanaRenderer.material = materialVentanaTransparente;
                ventanaSuperior.GetComponent<MeshRenderer>().material = materialVentanaTransparente;
            }
            else
            {
                ventanaCollider.enabled = true;
                alphaValue = 1;
                ventanaRenderer.material = materialVentanaOriginal;
                ventanaSuperior.GetComponent<MeshRenderer>().material = materialVentanaOriginal;
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

            ventanaParent.transform.position = new Vector3(initialPos.x, clampedY, initialPos.z);
            
            hand.transform.position = transform.position;

            IsTooHigh = !IsAlturaSegura();
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
