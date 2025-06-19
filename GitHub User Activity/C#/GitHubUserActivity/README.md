# GitHub Activity CLI

**GitHub Activity CLI** is a lightweight command-line tool written in **C# (.NET 9.0)** that fetches and displays the most recent public GitHub activity for any user. It supports optional filtering by event type and customizable output formats (`table` or `json`).

---

## Features

- Fetch public GitHub activity for any user
- Filter results by event type (e.g., `PushEvent`, `WatchEvent`)
- Choose output format: readable table or structured JSON
- Installable as a global `.NET tool` using `dotnet tool install`
- Built using `System.CommandLine` and `HttpClient`

---

## Prerequisites

Make sure you have:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download) installed
- Internet access (the tool uses the GitHub public API)

---

## Project Setup & Build

### 1. Clone the repository

```bash
git clone https://github.com/franklinfoko/roadmapsh-project.git
cd roadmapsh-project/'GitHub User Activity'/C#/GitHubUserActivity
```

### 2. Build the project

```bash
dotnet build
```

### 3. Generate the .nupkg package

```bash
dotnet pack --configuration Release
```

This will create the file:
```bash
./bin/Release/github-activity.1.0.0.nupkg
```

### 4. Install the CLI globally

```bash
dotnet tool install --global --add-source ./bin/Release github-activity
```

### 5. Usage

Once installed, you can run the CLI from anywhere:
```bash
github-activity <username> [--filter <event-type>] [--format <table|json>]
```

Examples:
```bash
# Show recent events
github-activity franklinfoko

# Filter only PushEvents
github-activity franklinfoko --filter PushEvent

# Output JSON instead of table
github-activity franklinfoko --format json
```  

Sample Output:

- Table Output
```bash
Recent GitHub Activity for franklinfoko:
- [PushEvent] on repo: franklinfoko/.NET-Microservice-Project at 06/18/2025 22:19:11
- [PushEvent] on repo: franklinfoko/.NET-Microservice-Project at 06/17/2025 16:08:54
- [CreateEvent] on repo: franklinfoko/.NET-Microservice-Project at 06/17/2025 16:05:58
- [CreateEvent] on repo: franklinfoko/.NET-Microservice-Project at 06/17/2025 16:04:11
- [PushEvent] on repo: franklinfoko/Project-Building-a-Simple-API-with-Copilot at 06/16/2025 02:54:20
```

- JSON Output
```bash
[
  {
    "type": "PushEvent",
    "repo": {
      "name": "franklinfoko/.NET-Microservice-Project"
    },
    "created_at": "2025-06-18T22:19:11Z"
  },
  {
    "type": "PushEvent",
    "repo": {
      "name": "franklinfoko/.NET-Microservice-Project"
    },
    "created_at": "2025-06-17T16:08:54Z"
  },
  {
    "type": "PushEvent",
    "repo": {
      "name": "franklinfoko/Project-Building-a-Simple-API-with-Copilot"
    },
    "created_at": "2025-06-16T02:54:20Z"
  },
  {
    "type": "PushEvent",
    "repo": {
      "name": "franklinfoko/my-leetcode-problem-solved"
    },
    "created_at": "2025-06-13T15:47:42Z"
  },
  {
    "type": "PushEvent",
    "repo": {
      "name": "franklinfoko/my-leetcode-problem-solved"
    },
    "created_at": "2025-06-13T15:08:45Z"
  },
  {
    "type": "PushEvent",
    "repo": {
      "name": "franklinfoko/my-leetcode-problem-solved"
    },
    "created_at": "2025-06-13T14:31:22Z"
  },
  {
    "type": "PushEvent",
    "repo": {
      "name": "franklinfoko/my-leetcode-problem-solved"
    },
    "created_at": "2025-06-08T05:49:14Z"
  },
  {
    "type": "PushEvent",
    "repo": {
      "name": "franklinfoko/devops-path"
    },
    "created_at": "2025-06-07T00:25:37Z"
  },
  {
    "type": "PushEvent",
    "repo": {
      "name": "News-Technologies-Academy/devops-path"
    },
    "created_at": "2025-06-07T00:24:10Z"
  },
  {
    "type": "PushEvent",
    "repo": {
      "name": "franklinfoko/devops-course-repo"
    },
    "created_at": "2025-06-06T23:48:24Z"
  },
  {
    "type": "PushEvent",
    "repo": {
      "name": "franklinfoko/roadmapsh-project"
    },
    "created_at": "2025-05-28T01:26:47Z"
  },
  {
    "type": "PushEvent",
    "repo": {
      "name": "franklinfoko/roadmapsh-project"
    },
    "created_at": "2025-05-28T00:11:14Z"
  }
]
```

### 5. Cleanup

```bash
dotnet tool uninstall --global github-activity
dotnet tool list --global
```