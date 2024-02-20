public interface ICommand
{
    public void Execute();
    public void Execute(object data);
}
