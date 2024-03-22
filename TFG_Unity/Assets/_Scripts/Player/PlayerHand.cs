using System;
using DG.Tweening;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Player
{
    /// <summary>
    /// Clase que maneja define las manos del personaje en los niveles del juego
    /// </summary>
    public class PlayerHand:MonoBehaviour
    {
        #region Definicion de la clase

        [SerializeField] private GameObject hand;
        private Camera _camera;
        
        private GameObject _objectSelected;
        private Vector3 _handInitialPosition;
        private Vector3? _objectInitialPosition;
        private Vector3? _objectInitialLocalPosition;

        public Vector3 HandInitialPosition => _handInitialPosition;
        public Vector3? ObjectInitialPosition => _objectInitialPosition;

        public GameObject ObjectSelected
        {
            get => _objectSelected;
            set
            {
                _objectSelected = value;
                _objectInitialPosition = value?.transform.position;
            }
        }
        
        private void Start()
        {
            _handInitialPosition = hand.transform.position;
            _camera = Camera.main;
        }

        #endregion
        
        #region Acciones de la mano

        /// <summary>
        /// Controla la accion de la mano una vez soltado sobre un objeto
        /// </summary>
        public void OnMouseUp()
        {
            RaycastHit hit;

            Vector3 rayOrigin = hand.transform.position;
            Vector3 cameraPos = _camera.transform.position;
            Vector3 rayDirection = (rayOrigin-cameraPos).normalized;
            
            if(Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity))
            {
                Iteractables parsedEnum;
                Enum.TryParse(hit.collider.tag, out parsedEnum);
                switch(parsedEnum)
                {
                    case Iteractables.Interactable:
                        if (ObjectSelected.IsUnityNull()) Grab(hit.collider.gameObject);
                        break;
                }
            }
            
            GoToInitialPosition();
        }

        /// <summary>
        /// Mueve la mano hacia el objeto y coloca el objeto como hijo de la mano
        /// </summary>
        /// <param name="objectToGrab">Objeto a coger</param>
        /// <param name="newParent">Gameobject padre en el que soltar el objeto</param>
        public void GrabObject(GameObject objectToGrab, GameObject newParent)
        {
            transform.DOMove(objectToGrab.transform.position, 1)
                .OnComplete(() =>
                {
                    if (!ObjectSelected.IsUnityNull())
                    {
                        DropObject(newParent);
                    }
                    Grab(objectToGrab);
                
                    GoToInitialPosition();
                });
        }

        /// <summary>
        /// Metodo para agarrar un objeto con la mano
        /// </summary>
        /// <param name="objectToGrab">Objeto a agarrar</param>
        public void Grab(GameObject objectToGrab)
        {
            ObjectSelected = objectToGrab;

            objectToGrab.transform.SetParent(transform);
            objectToGrab.transform.localPosition = Vector3.zero;
                
            objectToGrab.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        
        /// <summary>
        /// Devuelve la mano hacia hacia su posicion inicial
        /// </summary>
        public void GoToInitialPosition()
        {
            transform.DOMove(HandInitialPosition, 1);
        }

        /// <summary>
        /// Lleva la logica del soltado de objetos. Deja el objetos de la mano en su posicion original
        /// y establece sus valores por defecto.
        /// </summary>
        /// <param name="newParent">Gameobject padre al que se le suelta el objeto</param>
        public void DropObject(GameObject newParent)
        {
            try
            {
                if (ObjectSelected.IsUnityNull()) return;

                var droppedObject = ObjectSelected;

                droppedObject.transform.position = ObjectInitialPosition.GetValueOrDefault();
                droppedObject.transform.SetParent(newParent.transform);
                droppedObject.layer = LayerMask.NameToLayer("Default");
            
                ObjectSelected = null;
            }
            catch (Exception e)
            {
            }
        
        }
        
        #endregion
    }
}