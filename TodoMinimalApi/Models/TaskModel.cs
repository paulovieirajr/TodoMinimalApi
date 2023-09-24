namespace TodoMinimalApi.Models;

public class TaskModel
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }

    public TaskModel(string? title, bool isCompleted)
    {
        Title = title;
        IsCompleted = isCompleted;
    }
}