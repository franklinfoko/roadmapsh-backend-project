using System.CommandLine;

namespace task-cli;

class Program
{
    static int Main(string[] args)
    {
        Argument<string> taskDesciption = new("taskDesciption")
        {
            DescriptionAttribute = "The description of the task."
        };
        Option<string> addDescription = new("add")
        {
            Description = "Option used to add task description"
        };

        RootCommand rootCommand = new();
        rootCommand.Arguments.Add(taskDesciption);
        rootCommand.Options.Add(addDescription);
    }
}