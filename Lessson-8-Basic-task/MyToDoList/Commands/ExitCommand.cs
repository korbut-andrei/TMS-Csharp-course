namespace MyToDoList.Commands;

internal class ExitCommand : ICommand
{
    public string Description => "Выход";

    public void Execute()
    {
        Environment.Exit(0);
    }
}
