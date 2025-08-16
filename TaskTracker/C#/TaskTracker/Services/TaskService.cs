using TaskTracker.Models;
using TaskTracker.Persistence;

namespace TaskTracker.Services;

public class TaskService
{
    private readonly JsonTaskStore _store;

    public TaskService(string filePath) => _store = new JsonTaskStore(filePath);

    public void Add(string description, string status = "todo")
    {
        var tasks = _store.Load();
        string id = (tasks.Count + 1).ToString();

        tasks[id] = new Task
        {
            Id = int.Parse(id),
            Description = description,
            Status = status,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _store.Save(tasks);
        Console.WriteLine($"Task added successfully (ID: {id}).");
    }

    public void Update(string id, string? description = null, string? status = null)
    {
        var tasks = _store.Load();
        if (!tasks.TryGetValue(id, out var task))
        {
            Console.WriteLine($"Task with ID {id} not found.");
            return;
        }

        if (description is not null) task.Description = description;
        if (status is not null) task.Status = status;
        task.UpdatedAt = DateTime.Now;

        _store.Save(tasks);
        Console.WriteLine($"Task {id} updated.");
    }

    public void Delete(string id)
    {
        var tasks = _store.Load();
        if (tasks.Remove(id))
        {
            _store.Save(tasks);
            Console.WriteLine($"Task {id} deleted.");
        }
        else Console.WriteLine($"Task with ID {id} not found.");
    }

    public IEnumerable<Task> List(string? statusFilter = null)
    {
        var tasks = _store.Load().Values;
        return statusFilter is null
            ? tasks
            : tasks.Where(t => t.Status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase));
    }
}
