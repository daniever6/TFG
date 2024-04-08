using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Player
{
    /// <summary>
    /// Clase que controla el movimiento del jugador a través de teclado
    /// </summary>
    public class PlayerMovement : GameplayMonoBehaviour
    {
        #region Variables

        [Header("References")] 
        [SerializeField] private Rigidbody rbRigidbody;
        [SerializeField] private Camera mainCamera;
        private RaycastHit _hit;
    
        [Header("Properties")] 
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 200f;
        private Vector2 _moveDirection;

        #endregion

        #region Lifecycle Methods

    
        /// <summary>
        /// Llamamos a actualizar la posicion de la camara despues del movimiento del personaje
        /// </summary>
        private void LateUpdate()
        {
            mainCamera.transform.position = this.transform.position + new Vector3(0, 7.5f, -5);
        }

        /// <summary>
        /// Mueve el personaje en la direccion indicada
        /// </summary>
        /// <param name="direction">Direccion recibida mediante inputActionReference Move(WASD)</param>
        public void Move(Vector2 direction)
        {
            rbRigidbody.velocity = new Vector3(direction.x * moveSpeed, 0, direction.y * moveSpeed);
            if (direction != Vector2.zero)
            {
                Rotate(direction);
            }
        }

        /// <summary>
        /// Gira al jugador hacia la direccion de su movimiento
        /// </summary>
        /// <param name="direction">Direccion a la que se dirige el jugador</param>
        private void Rotate(Vector2 direction)
        {
            Vector3 forwardPlayerDirection = new Vector3(direction.x, 0, direction.y);
            Quaternion toRotation = Quaternion.LookRotation(forwardPlayerDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation,
                rotationSpeed * Time.deltaTime);
        }
        
        /// <summary>
        /// Detiene el movimiento del jugador
        /// </summary>
        private void StopMovement()
        {
            rbRigidbody.velocity = Vector3.zero;
            _moveDirection = Vector2.zero;
        }

        /// <summary>
        /// Metodo que se encarga de interactuar con el objeto interactable mas cercano, y que responde al
        /// inputActionReference interact del UserInput
        /// </summary>
        public void Interact()
        {
            Collider[] collidersBuffer = new Collider[10];
            Queue<Collider> interactables = new Queue<Collider>();
            
            int numCollisions = Physics.OverlapSphereNonAlloc(transform.position, 3f, collidersBuffer);

            for (int i = 0; i < numCollisions; i++)
            {
                if (collidersBuffer[i].gameObject.layer.Equals(LayerMask.NameToLayer("Interactable")))
                {
                    interactables.Enqueue(collidersBuffer[i]);
                }
            }

            float closestDistance = Mathf.Infinity;
            GameObject closestObject = null;

            while (interactables.Count>0)
            {
                Collider colliderObject = interactables.Dequeue();
                float distance = Vector3.Distance(transform.position, colliderObject.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = colliderObject.gameObject;
                }
            }

            if (!closestObject.IsUnityNull())
            {
                StopMovement();
                closestObject.GetComponent<Trigger>().TriggerEvent();
            }
        }

        #endregion

    }
}
