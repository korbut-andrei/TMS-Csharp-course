
//и "Перевести задачу в выполенные (достижения)".
//2. Добавить время добавления задачи (в список задач) и время выполнения в список достижений.
//3. Добавить сохрание состояния в файл, что бы между перезапусками программы сохранялись добавленные ранее задачи.
using MyToDoList.Data;

namespace MyToDoList.Commands;

internal class CompleteCommand : ICommand
{
    private readonly IToDoList _toDoList;

    public string Description => "Выполнение задачи";

    public CompleteCommand(IToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public void Execute()
    {
        do
        {
            Console.WriteLine("Введи идентификатор выполненной задачи");
            if (int.TryParse(Console.ReadLine(), out int id) && id >= 0 && id <= _toDoList.ToDoItems().Count)
            {
                _toDoList.MarkAsCompleted(id);
                break;
            }
            else
            {
                Console.WriteLine("Неверный идентификатор задачи");
            }
        } while (true);
    }
}
