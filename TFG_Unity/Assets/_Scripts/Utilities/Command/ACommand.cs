using Player;
using UnityEngine;

namespace Command
{
    /// <summary>
    /// Clase abstracta de comandos
    /// </summary>
    public abstract class ACommand : ICommand
    {
        protected IPlayerController _playerReciver;

        public ACommand(IPlayerController playerReciver)
        {
            _playerReciver = playerReciver;
        }

        public abstract void Execute();
        
        public abstract void Execute(object data);
    }
}