using System;
using UnityEngine;

namespace _Scripts.Interactables
{
    /// <summary>
    /// Clase que maneja define las manos del personaje en los niveles del juego
    /// </summary>
    public class PlayerHand:MonoBehaviour
    {
        [SerializeField] private GameObject _hand;
        private GameObject _objectSelected { get; set; }
        private Vector3 _handHandInitialPosition;
        private Vector3 _objectInitialPosition;
        public Vector3 HandInitialPosition => _handHandInitialPosition;
        public Vector3 ObjectInialPosition => _objectInitialPosition;
        public GameObject ObjectSelected
        {
            get { return _objectSelected; }
            set
            {
                if(!value.Equals(null)) _objectInitialPosition = value.transform.position;
                _objectSelected = value;
            }
        }

        private void Start()
        {
            _handHandInitialPosition = _hand.transform.position;
        }
    }
}