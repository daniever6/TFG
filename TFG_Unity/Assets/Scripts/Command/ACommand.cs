using UnityEngine;

namespace Command
{
    /// <summary>
    /// Clase abstracta de comandos
    /// </summary>
    public abstract class ACommand : ICommand
    {
        protected MonoBehaviour _playerReciver;

        public ACommand(MonoBehaviour playerReciver)
        {
            _playerReciver = playerReciver;
        }

        public abstract void Execute();
        
        public abstract void Execute(object data);
    }
}