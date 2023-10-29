namespace MyToDoList.Data;

    public class Task
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public DateTime AddedAt { get; set; }
    public DateTime CompletedAt { get; set; }
}