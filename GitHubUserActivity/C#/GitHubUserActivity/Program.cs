using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.CommandLine;
using System.CommandLine.Invocation;


var rootCommand = new RootCommand("GithHub Activity CLI");

var username = new Argument<string>("username", "GitHub username");
var filterOption = new Option<string>("--filter", "Filter by event typee");
var formatOption = new Option<string>("--format", () => "table", "Output format: 'table' or 'json'");

rootCommand.AddArgument(username);
rootCommand.AddOption(filterOption);
rootCommand.AddOption(formatOption);

rootCommand.SetHandler(async (username, filter, format) =>
{
    string apiUrl = $"https://api.github.com/users/{username}/events";

    using HttpClient client = new();
    client.DefaultRequestHeaders.UserAgent.ParseAdd("CSharpApp");

    try
    {
        HttpResponseMessage response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Failed to fetch data: {response.StatusCode}");
            return;
        }

        string json = await response.Content.ReadAsStringAsync();

        var events = JsonSerializer.Deserialize<GitHubEvent[]>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (events == null || events.Length == 0)
        {
            Console.WriteLine("No recent avtivity found");
            return;
        }

        var filtered = string.IsNullOrEmpty(filter)
            ? events
            : events.Where(e => e.Type != null && e.Type.Equals(filter, StringComparison.OrdinalIgnoreCase)).ToArray();

        if (format == "json")
        {
            string outputJson = JsonSerializer.Serialize(filtered, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(outputJson);
        }
        else
        {
            Console.WriteLine($"\nRecent GitHub Activity for {username}:");
            foreach (var e in filtered.Take(5))
            {
                Console.WriteLine($"- [{e.Type}] on repo: {e.Repo?.Name ?? "unknown"} at {e.CreatedAt}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
},
username, filterOption, formatOption);

await rootCommand.InvokeAsync(args);

public class GitHubEvent
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("repo")]
    public Repo? Repo { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class Repo
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}