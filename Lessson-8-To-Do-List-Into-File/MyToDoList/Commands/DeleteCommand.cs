using MyToDoList.Data;
using System.Threading.Tasks;

namespace MyToDoList.Commands;
//Домашнее задание на четверг
//1. Добавить команды: "Удалить задачу" 
internal class DeleteCommand : ICommand
{
    private readonly IToDoList _toDoList;

    public string Description => "Удаление задачи из списка";

    public DeleteCommand(IToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public void Execute()
    {
        do
        {
             Console.WriteLine("Введи идентификатор задачи для удаления");
            if (int.TryParse(Console.ReadLine(), out int id) && id >= 0 && id <= _toDoList.ToDoItems().Count )
            {
            _toDoList.Delete(id);
            break;
            }
            else
            {
                Console.WriteLine("Неверный идентификатор задачи");
            }
        } while (true);
    }
}