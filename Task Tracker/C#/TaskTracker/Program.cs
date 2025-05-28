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
            //Console.WriteLine($"Error loading tasks: {ex.Message}");
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

    public static void UpdateTask(string id, string description, string status, string filePath)
    {
        var tasks = LoadTasks(filePath);
        if (tasks.ContainsKey(id))
        {
            var task = tasks[id];
            task.Description = description;
            task.Status = status;
            task.UpdatedAt = DateTime.Now;

            SaveTasks(filePath, tasks);
            Console.WriteLine($"Task {id} updated successfully.");
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

    public static void AddCommandLine(string command)
    {
        // This method can be used to add command line functionality in the future
        Console.WriteLine($"Command received: {command}");
        var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            Console.WriteLine("No command provided.");
            return;
        }
        string action = parts[0].ToLower();
        switch (action)
        {
            case "add":
                if (parts.Length < 2)
                {
                    Console.WriteLine("Usage: add <description> [status]");
                    return;
                }
                string description = parts[1];
                string status = parts.Length > 2 ? parts[2] : "todo";
                AddTask(description, "tasks.json", status);
                break;
            case "update":
                if (parts.Length < 4)
                {
                    Console.WriteLine("Usage: update <id> <description> <status>");
                    return;
                }
                UpdateTask(parts[1], parts[2], parts[3], "tasks.json");
                break;
            case "delete":
                if (parts.Length < 2)
                {
                    Console.WriteLine("Usage: delete <id>");
                    return;
                }
                DeleteTask(parts[1], "tasks.json");
                break;
            case "list":
                ListTasks("tasks.json");
                break;
            default:
                Console.WriteLine($"Unknown command: {action}");
                break;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        TaskManager taskManager = new();

        string filePath = "tasks.json";
        while (true)
        {
            Console.WriteLine("task-cli");

            if (File.Exists(filePath))
            {
                //.AddTask("Sample task description", filePath);
                //TaskManager.UpdateTask("3", "Updated task description", "done", filePath);
                //TaskManager.DeleteTask("1", filePath);
                //TaskManager.ListTasks(filePath);
                //TaskManager.ListTasksByStatus("in-progress", filePath);
            }

            // Add a break or user input to exit the loop if needed
            break;
        }
        
    }
}