using System;
using System.Threading.Tasks;

namespace MyToDoList.Data;

public class ToDoList : IToDoList
{
    public List<Task> _toDoTasks = new List<Task>();
    public List<Task> _doneTasks = new List<Task>();

    public void Add(string task)
    {

        var newTask = new Task
        {
            Id = Guid.NewGuid(),
            Description = task,
            AddedAt = DateTime.Now
        };
        _toDoTasks.Add(newTask);
        AddTasksToFile(newTask, "C:\\testLesson8\\tasksToDo.txt");
    }

    public void Delete(int id)
    {
        _toDoTasks.RemoveAt(id);
        DeleteRecordFromFileByIndex("C:\\testLesson8\\tasksToDo.txt", id);
    }

    public void MarkAsCompleted(int id)
    {
        var task = _toDoTasks[id];
        _toDoTasks[id].AddedAt = DateTime.Now;
        _toDoTasks.RemoveAt(id);
        DeleteRecordFromFileByIndex("C:\\testLesson8\\tasksToDo.txt", id);
        _doneTasks.Add(task);
        AddTasksToFile(task, "C:\\testLesson8\\tasksCompleted.txt");
    }

    public List<string> ToDoItems()
    {
        var items = new List<string>();

        for (int i = 0; i < _toDoTasks.Count; i++)
        {
            var task = _toDoTasks[i];
            var description = task.Description;
            var addedDateTime = task.AddedAt;
            var index = i;

            items.Add($"{index}. Task: {description}; Started at: {addedDateTime}");

        }
        return items;
    }

    public List<string> DoneItems()
    {
        var items = new List<string>();

        for (int i = 0; i < _doneTasks.Count; i++)
        {
            var task = _doneTasks[i];
            var description = task.Description;
            var addedDateTime = task.AddedAt;
            var completedDateTime = task.CompletedAt;
            TimeSpan dateDifference = completedDateTime - addedDateTime;
            var index = i;

            items.Add($"{index}. Task: {description}; Started at: {addedDateTime}; Completed at: {dateDifference.Days}; Time spent: {dateDifference.Days}");
        }

        return items;
    }

    private void AddTasksToFile(Task task, string filePath)
    {
        using (var writer = new StreamWriter(filePath, append: true))
        {
            string taskData = $"{task.Id},{task.Description},{task.AddedAt},{task.CompletedAt}";
            writer.WriteLine(taskData);
        }
    }

    private static void DeleteRecordFromFileByIndex(string filePath, int index)
    {
        // Load existing records from the file
        List<Task> listOfTasks = ParseFile(filePath);

        if (index >= 0 && index < listOfTasks.Count)
        {
            // Remove the record at the specified index
            listOfTasks.RemoveAt(index);

            // Save the updated records back to the file
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var task in listOfTasks)
                {
                    string taskData = $"{task.Id},{task.Description},{task.AddedAt},{task.CompletedAt}";
                    writer.WriteLine(taskData);
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid index. Record not deleted.");
        }
    }

    public static List<Task> ParseFile(string filePath)
    {
        List<Task> taskItems = new List<Task>();

        // Check if the file exists, and if not, create it
        if (!File.Exists(filePath))
        {
            // Create an empty file
            File.Create(filePath).Close();
        }
        // Read lines from the file
        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines)
        {
            // Split the line using a comma as a delimiter
            string[] parts = line.Split(',');

            if (parts.Length == 4)
            {
                if (Guid.TryParse(parts[0], out Guid id) && DateTime.TryParse(parts[2], out DateTime AddedAt) && DateTime.TryParse(parts[3], out DateTime completedAt))
                {
                    Task taskItem = new Task
                    {
                        Id = id,
                        Description = parts[1],
                        AddedAt = AddedAt,
                        CompletedAt = completedAt
                    };
                    taskItems.Add(taskItem);
                }
            }
        }
        return taskItems;
    }

}