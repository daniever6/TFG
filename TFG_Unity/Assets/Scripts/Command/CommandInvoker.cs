namespace Command
{
    public class CommandInvoker : ICommand
    {
        public ICommand _command;

        public CommandInvoker(ICommand command)
        {
            _command = command;
        }
        
        public void Execute()
        {
            _command.Execute();
        }

        public void Execute(object data)
        {
            _command.Execute(data);
        }
    }
}