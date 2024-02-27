using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Command
{
    /// <summary>
    /// Comando encargado de llamar al metodo Move del PlayerMovement
    /// Comprueba si es nulo o esta activo antes de lanzar el metodo
    /// </summary>
    public class MoveCommand : ACommand
    {
        public MoveCommand(PlayerMovement playerMovement) : base(playerMovement){}
        public override void Execute()
        {
            if(_playerReciver.Equals(null) || !_playerReciver.isActiveAndEnabled) return;
            ((PlayerMovement)_playerReciver).Move(Vector2.zero);
        }

        public override void Execute(object data)
        {
            if(_playerReciver.Equals(null) || !_playerReciver.isActiveAndEnabled) return;
            ((PlayerMovement)_playerReciver).Move((Vector2)data);
        }
    }

    /// <summary>
    /// Comando encargado de llamar al metodo WalkToPoint de la clase PlayerMovementOnClick
    /// Comprueba si es nulo o esta activo antes de lanzar el comando
    /// </summary>
    public class WalkOnClickCommand : ACommand
    {
        public WalkOnClickCommand(PlayerMovementOnClick playerMovementOnClick) : base(playerMovementOnClick){}

        public override void Execute()
        {
            if(_playerReciver.Equals(null) || !_playerReciver.isActiveAndEnabled) return;
            ((PlayerMovementOnClick)_playerReciver).WalkToPoint(new InputAction.CallbackContext());
        }

        public override void Execute(object data)
        {
            if(_playerReciver.Equals(null) || !_playerReciver.isActiveAndEnabled) return;
            ((PlayerMovementOnClick)_playerReciver).WalkToPoint(new InputAction.CallbackContext());
        }
    }

    /// <summary>
    /// Comando encargado de pausar el juego a traves del ChangeState del GameManager
    /// </summary>
    public class PauseCommand : ACommand
    {
        public PauseCommand(MonoBehaviour playerReciver) : base(playerReciver)
        {
        }

        public override void Execute()
        {
            GameState gameState = GameManager.Instance._gameState != GameState.Pause
                ? GameState.Pause
                : GameState.Resume;
            GameManager.Instance.ChangeState(gameState);
        }

        public override void Execute(object data)
        {
            GameState gameState = GameManager.Instance._gameState != GameState.Pause
                ? GameState.Pause
                : GameState.Resume;
            GameManager.Instance.ChangeState(gameState);
        }
    }
}