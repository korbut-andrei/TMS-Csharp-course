using System.Collections.Generic;

namespace MyToDoList.Data;

public interface IToDoList
{
    void Add(string task);

    void Delete(int id);

    void MarkAsCompleted(int id);

    List<string> ToDoItems();

    List<string> DoneItems();
}
