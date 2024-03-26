/// <summary>
/// Interfaz de Comandos
/// </summary>
public interface ICommand
{
    public void Execute();
    public bool Execute(object data);
}
