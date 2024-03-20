
using _Scripts.Player;
using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] [CanBeNull] private PlayerMovement _playerMovement;
    [SerializeField] [CanBeNull] private PlayerMovementOnClick _playerMovementOnClick;

    public PlayerMovement PlayerMovement => _playerMovement;
    public PlayerMovementOnClick PlayerMovementOnClick => _playerMovementOnClick;
        
    public void Move(Vector2 direction)
    {
        _playerMovement.Move(direction);
    }

    public void WalkToPoint(InputAction.CallbackContext context)
    {
        _playerMovementOnClick.WalkToPoint(context);
    }

    public void Use()
    {
        throw new System.NotImplementedException();
    }
}
