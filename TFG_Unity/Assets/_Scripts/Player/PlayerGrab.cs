using System;
using System.Collections;
using _Scripts.Interactables;
using DG.Tweening;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utilities;

public class PlayerGrab : GameplayMonoBehaviour
{
    private Camera _camera;
    private RaycastHit _hit;

    [SerializeField] private GameObject _interactablesParent;

    [SerializeField] private PlayerHand _playerHand;

    private void Start()
    {
        _camera = Camera.main;
    }

    /// <summary>
    /// Lleva la logica de la interaccion de los objetos en los niveles del juego
    /// </summary>
    /// <param name="context"></param>
    public void Grab(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
            
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _hit, Mathf.Infinity))
        {
            Iteractables parsedEnum;
            Enum.TryParse(_hit.collider.tag, out parsedEnum);
            switch(parsedEnum)
            {
                case Iteractables.Interactable:
                    GrabObject(_hit.collider.gameObject, _playerHand);
                    break;
                case Iteractables.Ground:
                    DropObject(_playerHand);
                    break;
            }
        }
    }

    /// <summary>
    /// Mueve la mano hacia el objeto y coloca el objeto como hijo de la mano
    /// </summary>
    /// <param name="objectToGrab">Objeto a coger</param>
    /// <param name="hand">Mano con el que realiza la accion</param>
    public void GrabObject(GameObject objectToGrab, PlayerHand hand)
    {
        hand.transform.DOMove(objectToGrab.transform.position, 1)
            .OnComplete(() =>
            {
                if (hand.ObjectSelected != null)
                {
                    DropObject(hand);
                }
                objectToGrab.transform.SetParent(hand.transform);
                hand.ObjectSelected = objectToGrab;
                
                objectToGrab.GetComponent<Collider>().enabled = false;
                
                GoToInitialPosition(hand);
            });
    }

    /// <summary>
    /// Devuelve la mano hacia hacia su posicion inicial
    /// </summary>
    /// <param name="hand">Mano a realizar la accion</param>
    public void GoToInitialPosition(PlayerHand hand)
    {
        hand.transform.DOMove(hand.HandInitialPosition, 1);
    }

    public void DropObject(PlayerHand hand)
    {
        try
        {
            if (hand.ObjectSelected.Equals(null)) return;

            var droppedObject = hand.ObjectSelected;

            droppedObject.transform.SetParent(_interactablesParent.transform);
            droppedObject.transform.position = hand.ObjectInialPosition;
            droppedObject.GetComponent<Collider>().enabled = true;

            hand.ObjectSelected = null;
        }
        catch (Exception e)
        {
        }
        
    }
}
