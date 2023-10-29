using MyToDoList.Data;

namespace MyToDoList.Commands;

internal class AddCommand : ICommand
{
    private readonly IToDoList _toDoList;

    public string Description => "Добавить задачу";

    public AddCommand(IToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public void Execute()
    {
        do
        {
            Console.WriteLine("Введи описание задачи");
            var task = Console.ReadLine();

            if (task != null)
            {
                _toDoList.Add(task);
                break;
            }
        } while (true);
    }
}
