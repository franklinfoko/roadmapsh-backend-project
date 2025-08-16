"""
FUNCTIONS DEFINITIONS
"""

from datetime import datetime
import json
import os

JSON_FILE_PATH = "task.json"

def load_tasks():
    """Load tasks from a JSON file"""
    if os.path.exists(JSON_FILE_PATH):
        with open(JSON_FILE_PATH, "r", encoding="utf-8") as file:
            tasks = json.load(file)
    else:
        tasks = {}
    return tasks

dict_tasks = load_tasks()

def save_tasks(tasks):
    """Save tasks to a JSON file."""
    with open(JSON_FILE_PATH, "w", encoding="utf-8") as file:
        json.dump(tasks, file)

def add_task(description):
    """Add a new task with the given description."""
    task_id = len(dict_tasks) + 1
    task_description = description
    status = "todo"
    created_at = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
    updated_at = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
    dict_tasks[task_id] = {
        "description": task_description,
        "status": status,
        "created_at": created_at,
        "updated_at": updated_at
     }
    # Save the updated tasks to the JSON file
    save_tasks(dict_tasks)

    return dict_tasks

def list_tasks(status=None):
    """List all tasks or filter by status."""
    tasks = load_tasks()
    if not tasks:
        return "No tasks found."
    if status:
        filtered_tasks = {
            task_id: task for task_id, task in tasks.items() if task['status'] == status}
        return filtered_tasks if filtered_tasks else "No tasks found with the specified status."
    return tasks

def update_task(task_id, description):
    """Update the description of a task by its ID."""
    tasks = load_tasks()
    if str(task_id) in tasks:
        tasks[str(task_id)]['description'] = description
        tasks[str(task_id)]['updated_at'] = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        save_tasks(tasks)
        return f"Task {task_id} updated."

    return f"Task {task_id} not found."

def modify_status(status, task_id):
    """Modify the status of a task by its ID."""
    tasks = load_tasks()
    if str(task_id) in tasks:
        tasks[str(task_id)]['status'] = status
        tasks[str(task_id)]['updated_at'] = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        save_tasks(tasks)
        return f"Task {task_id} status updated to {status}."

    return f"Task {task_id} not found."

def delete_task(task_id):
    """Delete a task by its ID."""
    tasks = load_tasks()
    if str(task_id) in tasks:
        del tasks[str(task_id)]
        save_tasks(tasks)
        return f"Task {task_id} deleted."

    return f"Task {task_id} not found."
