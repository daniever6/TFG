using UnityEngine;

namespace _Scripts.Utilities.Command
{
    /// <summary>
    /// Clase abstracta de comandos
    /// </summary>
    public abstract class ACommand : ICommand
    {
        protected MonoBehaviour Reciver;

        public ACommand(MonoBehaviour reciver)
        {
            Reciver = reciver;
        }

        public abstract void Execute();
        
        public abstract bool Execute(object data);
    }
}