using GitHub_User_Activity;
using Spectre.Console;
using Spectre.Console.Cli;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var rootCommand = new RootCommand("");

        Argument<string> name = new("name")
        {
            DefaultValueFactory = (_) => "Shoontaro"
        };
        
        rootCommand.Add(name);

        rootCommand.SetAction(async parseResult =>
        {
            Validator.NameValidator(parseResult.GetValue(name));

            string username = parseResult.GetValue(name)??"Shoontaro";

            Console.WriteLine($"Name: {parseResult.GetValue(name)}");
            var activity = await API.gitApi(username);

            if (activity == null)
            {
                return;
            }

            API.DisplayData(activity);

        });

        rootCommand.Parse(args).Invoke();
    }

    

    //public void Commands()
    //{
    //    while (true) {
    //        string command = Console.ReadLine()?? "";

    //        Argument<string> name = new("name")
    //        {
    //            DefaultValueFactory = (_) => "Shoontaro", //объектный инициализатор
    //        };
    //    }
    //}
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