using System;
using _Scripts.Interactables;
using _Scripts.Utilities;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    /// <summary>
    /// Esta clase controla el movimiento y accion conjunta de las dos manos del jugador
    /// </summary>
    public class PlayerGrab : GameplayMonoBehaviour<PlayerGrab>
    {
        private Camera _camera;
        private RaycastHit _hit;
        public static bool IsTweening = false;

        [SerializeField] private GameObject interactablesParent;

        [SerializeField] private GameObject manoDerecha;
        [SerializeField] private GameObject manoIzquierda;

        private IHand rightHand;
        private IHand leftHand;

        private void Start()
        {
            _camera = Camera.main;

            manoDerecha.TryGetComponent(out rightHand);
            manoIzquierda.TryGetComponent(out leftHand);
        }

        /// <summary>
        /// Lleva la logica de la interaccion de los objetos en los niveles del juego
        /// </summary>
        /// <param name="context"></param>
        public void Grab(InputAction.CallbackContext context)
        {
            if (EventSystem.current.IsPointerOverGameObject() || IsTweening) return;
            
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out _hit, Mathf.Infinity))
            {
                Iteractables parsedEnum;
                IHand hand = ChoosePlayerHand(_hit.point);
                Enum.TryParse(_hit.collider.tag, out parsedEnum);
                switch(parsedEnum)
                {
                    case Iteractables.Interactable:
                        var selectedObject = _hit.collider.gameObject.GetComponent<LevelInteractable>();
                        hand.GrabObject(selectedObject, interactablesParent);
                        break;
                    case Iteractables.Ground:
                        hand.DropObject(interactablesParent);
                        break;
                    case Iteractables.Alfombrilla:
                        var alfombrillaObject = _hit.collider.gameObject.GetComponent<LevelInteractable>();
                        hand.GrabObject(alfombrillaObject, interactablesParent);
                        break;
                }
            }
        }

        /// <summary>
        /// Metodo que calcula la distancia entre el objeto y las manos, para determinar que mano
        /// va a llevar a cabo la accion
        /// </summary>
        /// <param name="clickedPoint">Punto a calcular la distancia de manos</param>
        /// <returns></returns>
        private IHand ChoosePlayerHand(Vector3 clickedPoint)
        {
            Vector3 rightHandPosition = rightHand.HandInitialPosition;
            Vector3 leftHandPosition = leftHand.HandInitialPosition;

            float rightDistance = Vector3.Distance(rightHandPosition, clickedPoint);
            float leftDistance = Vector3.Distance(leftHandPosition, clickedPoint);

            if (rightDistance > leftDistance) return leftHand;

            return rightHand;
        }
    }
}
