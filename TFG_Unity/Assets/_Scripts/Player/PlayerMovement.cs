using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.Player
{
    /// <summary>
    /// Clase que controla el movimiento del jugador a través de teclado
    /// </summary>
    public class PlayerMovement : GameplayMonoBehaviour<PlayerMovement>
    {
        #region Variables

        [Header("References")] 
        [SerializeField] private Rigidbody rbRigidbody;
        [SerializeField] private Camera mainCamera;
        private RaycastHit _hit;
    
        [Header("Properties")] 
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 200f;

        #endregion

        #region Lifecycle Methods

    
        /// <summary>
        /// Llamamos a actualizar la posicion de la camara despues del movimiento del personaje
        /// </summary>
        private void LateUpdate()
        {
            mainCamera.transform.position = this.transform.position + new Vector3(0, 9, -6);
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
        public void StopMovement()
        {
            rbRigidbody.velocity = Vector3.zero;
        }

        /// <summary>
        /// Realiza las siguientes acciones cuando se pausa el juego
        /// </summary>
        protected override void OnPostPaused()
        {
            StopMovement();
        }

        #endregion

    }
}
