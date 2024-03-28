using _Scripts.LevelScripts;
using _Scripts.Managers;
using Command;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Utilities.Command
{
    /// <summary>
    /// Comando encargado de llamar al metodo Move del PlayerMovement
    /// Comprueba si es nulo o esta activo antes de lanzar el metodo
    /// </summary>
    public class MoveCommand : ACommand
    {
        public MoveCommand(IPlayerController playerController) : base((MonoBehaviour)playerController){}
        public override void Execute()
        {
            if(Reciver.Equals(null)) return;
            ((IPlayerController)Reciver).Move(Vector2.zero);
        }

        public override bool Execute(object data)
        {
            if(Reciver.Equals(null)) return false;
            ((IPlayerController)Reciver).Move((Vector2)data);
            return true;
        }
    }

    /// <summary>
    /// Comando encargado de llamar al metodo WalkToPoint de la clase PlayerMovementOnClick
    /// Comprueba si es nulo o esta activo antes de lanzar el comando
    /// </summary>
    public class WalkOnClickCommand : ACommand
    {
        public WalkOnClickCommand(IPlayerController playerController) : base((MonoBehaviour)playerController){}

        public override void Execute()
        {
            if(Reciver.Equals(null)) return;
            ((IPlayerController)Reciver).WalkToPoint(new InputAction.CallbackContext());
        }

        public override bool Execute(object data)
        {
            if(Reciver.Equals(null)) return false;
            ((IPlayerController)Reciver).WalkToPoint(new InputAction.CallbackContext());
            return true;
        }
    }
    
    /// <summary>
    /// Comando encargado de la interaccion de objetos en primera persona
    /// </summary>
    public class UseCommand : ACommand
    {
        public UseCommand(IPlayerController playerController) : base((MonoBehaviour)playerController){}

        public override void Execute()
        {
            if(Reciver.Equals(null)) return;
            ((IPlayerController)Reciver).Use(new InputAction.CallbackContext());
        }

        public override bool Execute(object data)
        {
            if(Reciver.Equals(null)) return false;
            ((IPlayerController)Reciver).Use(new InputAction.CallbackContext());
            return true;
        }
    }

    /// <summary>
    /// Comando encargado de pausar el juego a traves del ChangeState del GameManager
    /// </summary>
    public class PauseCommand : ACommand
    {
        public PauseCommand(IPlayerController playerController) : base((MonoBehaviour)playerController)
        {
        }

        public override void Execute()
        {
            GameState gameState = GameManager.Instance.GameState != GameState.Pause
                ? GameState.Pause
                : GameManager.Instance.PreviousGameState;
            GameManager.Instance.ChangeState(gameState);
        }

        public override bool Execute(object data)
        {
            GameState gameState = GameManager.Instance.GameState != GameState.Pause
                ? GameState.Pause
                : GameManager.Instance.PreviousGameState;
            GameManager.Instance.ChangeState(gameState);
            return true;
        }
    }

    public class CombinationCommand : ACommand
    {
        public CombinationCommand(ILevel level) : base((MonoBehaviour)level){}

        public override void Execute()
        {
        }

        public override bool Execute(object data)
        {
            if (Reciver.Equals(null)) return false;
            return ((ILevel)Reciver).PerformCombination((string) data);
        }
    }
}