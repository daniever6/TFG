namespace Command
{
    public abstract class ACommand : ICommand
    {
        protected PlayerMovement _playerMovement;

        public ACommand(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;
        }
        public abstract void Execute();
        
        public abstract void Execute(object data);
    }
}