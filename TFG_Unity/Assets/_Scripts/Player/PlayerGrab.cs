using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Interactables;
using Dialogues;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerGrab : GameplayMonoBehaviour
{
    private Camera _camera;
    private RaycastHit _hit;

    [SerializeField] private GameObject _rightHand;
    [SerializeField] private GameObject _leftHand;

    private void Start()
    {
        _camera = Camera.main;
    }

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
                    _hit.transform.position = _rightHand.transform.position;
                    break;
            }
        }
    }
}
