using System;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Interactables
{
    /// <summary>
    /// Clase que maneja define las manos del personaje en los niveles del juego
    /// </summary>
    public class PlayerHand:MonoBehaviour
    {
        [SerializeField] private GameObject _hand;
        private GameObject _objectSelected;
        private Vector3 _handInitialPosition;
        private Vector3? _objectInitialPosition;

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
            _handInitialPosition = _hand.transform.position;
        }

        private void OnMouseUp()
        {
            _hand.transform.DOMove(_handInitialPosition, 1);
        }
    }
}