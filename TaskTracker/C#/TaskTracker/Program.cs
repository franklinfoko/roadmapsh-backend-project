using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

class Task
{
    public required int Id { get; set; }
    public required string Description { get; set; }
    public required string Status { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}

class TaskManager
{
    public static void SaveTasks(string filePath, Dictionary<string, Task> tasks)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(tasks, options);
        File.WriteAllText(filePath, json);
    }

    public static Dictionary<string, Task> LoadTasks(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return [];
        }

        try
        {
            string json = File.ReadAllText(filePath);
            Dictionary<string, Task> tasks = JsonSerializer.Deserialize<Dictionary<string, Task>>(json) ?? [];
            return tasks;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error loading tasks: {ex.Message}");
            File.WriteAllText(filePath, "{}"); // Reset file if there's an error
            return [];
        }
    }

    public static void AddTask(string description, string filePath, string status = "todo")
    {
        var tasks = LoadTasks(filePath); // Load existing tasks
        string id = (tasks.Count + 1).ToString();
        var task = new Task
        {
            Id = int.Parse(id),
            Description = description,
            Status = status,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        tasks[id] = task;

        SaveTasks(filePath, tasks);
        Console.WriteLine($"Task added successfully (ID: {id}).");
    }

    public static void UpdateTask(string id, string description, string filePath)
    {
        var tasks = LoadTasks(filePath);
        if (tasks.ContainsKey(id))
        {
            var task = tasks[id];
            task.Description = description;
            task.UpdatedAt = DateTime.Now;

            SaveTasks(filePath, tasks);
            Console.WriteLine($"Task {id} updated successfully.");
        }
        else
        {
            Console.WriteLine($"Task with ID {id} not found.");
        }
    }

    public static void UpdateTaskStatus(string id, string status, string filePath)
    {
        var tasks = LoadTasks(filePath);
        if (tasks.ContainsKey(id))
        {
            var task = tasks[id];
            task.Status = status;
            task.UpdatedAt = DateTime.Now;

            SaveTasks(filePath, tasks);
            Console.WriteLine($"Task {id} status updated to '{status}'.");
        }
        else
        {
            Console.WriteLine($"Task with ID {id} not found.");
        }
    }

    public static void DeleteTask(string id, string filePath)
    {
        var tasks = LoadTasks(filePath);
        if (tasks.Remove(id))
        {
            SaveTasks(filePath, tasks);
            Console.WriteLine($"Task {id} deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Task with ID {id} not found.");
        }
    }

    public static void ListTasks(string filePath)
    {
        var tasks = LoadTasks(filePath);
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found.");
            return;
        }

        foreach (var task in tasks.Values)
        {
            Console.WriteLine($"ID: {task.Id}, Description: {task.Description}, Status: {task.Status}, Created At: {task.CreatedAt}, Updated At: {task.UpdatedAt}");
        }
    }

    public static void ListTasksByStatus(string status, string filePath)
    {
        var tasks = LoadTasks(filePath);
        var filteredTasks = new List<Task>();

        foreach (var task in tasks.Values)
        {
            if (task.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
            {
                filteredTasks.Add(task);
            }
        }

        if (filteredTasks.Count == 0)
        {
            Console.WriteLine($"No tasks found with status '{status}'.");
            return;
        }

        foreach (var task in filteredTasks)
        {
            Console.WriteLine($"ID: {task.Id}, Description: {task.Description}, Status: {task.Status}, Created At: {task.CreatedAt}, Updated At: {task.UpdatedAt}");
        }
    }

}

class Program
{
    static void Main(string[] args)
    {
        TaskManager taskManager = new();

        Console.WriteLine("Welcome to the Task Tracker!");

        string filePath = "tasks.json";

        if (args.Length == 0)
        {
            Console.WriteLine("No command provided. Available commands: add, update, delete, list, mark-done, mark-in-progress");
            Console.WriteLine("Usage: <command> [arguments]");
            return;
        }

        string command = args[0].ToLower();
        switch (command)
        {
            case "add":
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: add <description> [status]");
                    return;
                }
                string description = args[1];
                string status = args.Length > 2 ? args[2] : "todo";
                TaskManager.AddTask(description, filePath, status);
                break;
            case "update":
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: update <id> <description> <status>");
                    return;
                }
                description = args[2];
                TaskManager.UpdateTask(args[1], description, filePath);
                break;
            case "delete":
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: delete <id>");
                    return;
                }
                TaskManager.DeleteTask(args[1], filePath);
                break;
            case "list":
                if (args.Length > 1)
                {
                    if (args[1].ToLower() == "done")
                    {
                        TaskManager.ListTasksByStatus("done", filePath);
                    }
                    else if (args[1].ToLower() == "todo")
                    {
                        TaskManager.ListTasksByStatus("todo", filePath);
                    }
                    else if (args[1].ToLower() == "in-progress")
                    {
                        TaskManager.ListTasksByStatus("in-progress", filePath);
                    }
                }
                else
                {
                    TaskManager.ListTasks(filePath);
                }
                break;
            case "mark-done":
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: mark-done <id>");
                    return;
                }
                TaskManager.UpdateTaskStatus(args[1], "done", filePath);
                break;
            case "mark-in-progress":
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: mark-in-progress <id>");
                    return;
                }
                TaskManager.UpdateTaskStatus(args[1], "in-progress", filePath);
                break;
            default:
                Console.WriteLine($"Unknown command: {command}");
                break;
        }
    }
}