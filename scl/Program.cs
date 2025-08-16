using System.CommandLine;
using System.CommandLine.Invocation;

class Program
{
    static int Main(string[] args)
    {
        var rootCommand = new RootCommand
        {
            new Option<int>("add", "Add task description"),
            new Option<bool>("update", "the id of task to update"),
            new Argument<string>("input", "A required input argument")
        };

        rootCommand.Handler = CommandHandler.Create<int, bool, string>((number, flag, input) =>
        {
            // Your application logic goes here
            Console.WriteLine($"Number: {number}");
            Console.WriteLine($"Flag: {flag}");
            Console.WriteLine($"Input: {input}");
        });

        return rootCommand.Invoke(args);
    }
}