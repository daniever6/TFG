using System;
using System.Collections;
using System.Collections.Generic;
using Command;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utilities;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("References")] 
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Camera _camera;
    private RaycastHit _hit;
    
    [Header("Properties")] 
    [SerializeField] private float _moveVelocity = 7f;
    private Vector2 _moveDirection;

    [Header("Input Actions")] 
    [SerializeField] private InputActionReference _move;


    #endregion

    #region Lifecycle Methods

    
    /// <summary>
    /// Llamamos a actualizar la posicion de la camara despues del movimiento del personaje
    /// </summary>
    private void LateUpdate()
    {
        _camera.transform.position = this.transform.position + new Vector3(0, 7.5f, -5);
    }

    /// <summary>
    /// Mueve el personaje en la direccion indicada
    /// </summary>
    /// <param name="direction">Direccion recibida mediante inputActionReference Move(WASD)</param>
    public void Move(Vector2 direction)
    {
        _rigidbody.velocity = new Vector3(direction.x * _moveVelocity, 0, direction.y * _moveVelocity);
    }

    #endregion

}
