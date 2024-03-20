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
        public MoveCommand(IPlayerController playerController) : base(playerController){}
        public override void Execute()
        {
            if(_playerReciver.Equals(null)) return;
            _playerReciver.Move(Vector2.zero);
        }

        public override void Execute(object data)
        {
            if(_playerReciver.Equals(null)) return;
            _playerReciver.Move((Vector2)data);
        }
    }

    /// <summary>
    /// Comando encargado de llamar al metodo WalkToPoint de la clase PlayerMovementOnClick
    /// Comprueba si es nulo o esta activo antes de lanzar el comando
    /// </summary>
    public class WalkOnClickCommand : ACommand
    {
        public WalkOnClickCommand(IPlayerController playerController) : base(playerController){}

        public override void Execute()
        {
            if(_playerReciver.Equals(null)) return;
            _playerReciver.WalkToPoint(new InputAction.CallbackContext());
        }

        public override void Execute(object data)
        {
            if(_playerReciver.Equals(null)) return;
            _playerReciver.WalkToPoint(new InputAction.CallbackContext());
        }
    }
    
    /// <summary>
    /// Comando encargado de la interaccion de objetos en primera persona
    /// </summary>
    public class UseCommand : ACommand
    {
        public UseCommand(IPlayerController playerController) : base(playerController){}

        public override void Execute()
        {
            var a = 0;
        }

        public override void Execute(object data)
        {
            var a = 0;
        }
    }

    /// <summary>
    /// Comando encargado de pausar el juego a traves del ChangeState del GameManager
    /// </summary>
    public class PauseCommand : ACommand
    {
        public PauseCommand(IPlayerController playerController) : base(playerController)
        {
        }

        public override void Execute()
        {
            GameState gameState = GameManager.Instance._gameState != GameState.Pause
                ? GameState.Pause
                : GameManager.Instance._previousGameState;
            GameManager.Instance.ChangeState(gameState);
        }

        public override void Execute(object data)
        {
            GameState gameState = GameManager.Instance._gameState != GameState.Pause
                ? GameState.Pause
                : GameManager.Instance._previousGameState;
            GameManager.Instance.ChangeState(gameState);
        }
    }

    
}