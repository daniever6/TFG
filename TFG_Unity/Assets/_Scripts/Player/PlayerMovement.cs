using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("References")] 
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Camera _camera;
    private RaycastHit _hit;
    
    [Header("Properties")] 
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _rotationSpeed = 200f;
    private Vector2 _moveDirection;

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
        _rigidbody.velocity = new Vector3(direction.x * _moveSpeed, 0, direction.y * _moveSpeed);
        if (direction != Vector2.zero)
        {
            Rotate(direction);
        }
    }

    /// <summary>
    /// Gira al jugador hacia la direccion de su movimiento
    /// </summary>
    /// <param name="direction">Direccion a la que se dirige el jugador</param>
    public void Rotate(Vector2 direction)
    {
        Vector3 forwardPlayerDirection = new Vector3(direction.x, 0, direction.y);
        Quaternion toRotation = Quaternion.LookRotation(forwardPlayerDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation,
            _rotationSpeed * Time.deltaTime);
    }

    #endregion

}
