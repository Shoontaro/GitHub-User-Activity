using System.ComponentModel;
using Spectre.Console.Cli;
using Spectre.Console;
using System.CommandLine;
using System.Net.Http.Headers;
using System.CommandLine.Parsing;

internal class Program
{
    private static void Main(string[] args)
    {
        var rootCommand = new RootCommand("");

        Argument<string> name = new("name")
        {
            DefaultValueFactory = (_) => "Shoontaro",
        };

        rootCommand.Add(name);

        rootCommand.SetAction(parseResult =>
        {
            Console.WriteLine($"Name: {parseResult.GetValue(name)}");
        });

        rootCommand.Parse(args).Invoke();
    }

    public void Commands()
    {
        while (true) {
            string command = Console.ReadLine()?? "";

            Argument<string> name = new("name")
            {
                DefaultValueFactory = (_) => "Shoontaro", //объектный инициализатор
            };
        }
    }
}

//public class GreetSettings : CommandSettings
//{
//    [CommandArgument(0, "<name>")]
//    [Description("The name to greet")]
//    public required string Name { get; init; }
//}

//public class GreetCommand : Command<GreetSettings>
//{
//    protected override int Execute(CommandContext context, GreetSettings settings, CancellationToken cancellation)
//    {
//            AnsiConsole.MarkupLine($"Hello, [green]{settings.Name}[/]!");
//        return 0;
//    }
//}

//public class 