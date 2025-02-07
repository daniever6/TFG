using _Scripts.Utilities;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

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
        private RaycastHit _hit;

        [Header("Properties")]
        private float MaxWalkVelocity = 10f;
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float rotationSpeed = 400f;

        [SerializeField][CanBeNull] private Animator playerAnimator;

        public float acceleration = 10f; // Aceleración
        private Vector2 direction;
        private float currentSpeed = 0f; // Velocidad actual del jugador

        //Sonidos
        private float nextStepTime = 0f;
        private float stepCooldown = 0.65f;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Mueve el personaje en la direccion indicada
        /// </summary>
        /// <param name="direction">Direccion recibida mediante inputActionReference Move(WASD)</param>
        public void Move(Vector2 direction)
        {
            playerAnimator?.SetBool("ManualWalking", direction != Vector2.zero);

            Vector3 moveDirection = new Vector3(direction.x, 0, direction.y).normalized;

            // Acelera suavemente hasta la velocidad máxima
            if (direction != Vector2.zero)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, acceleration * Time.fixedDeltaTime);

                //Sonido de caminar
                if (Time.time >= nextStepTime)
                {
                    SoundManager.Instance.Play("Caminar");
                    nextStepTime = Time.time + stepCooldown;  // Establece el próximo tiempo permitido
                }
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, acceleration * Time.fixedDeltaTime);
            }

            // Aplica la velocidad calculada
            rbRigidbody.velocity = moveDirection * currentSpeed;

            //rbRigidbody.velocity = new Vector3(direction.x * moveSpeed, 0, direction.y * moveSpeed);
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
