using GitHub_User_Activity;
using Octokit;
using Spectre.Console;
using Spectre.Console.Cli;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Net.Http.Headers;

internal class Program
{
    private static readonly HttpClient client = new HttpClient();

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
            await gitApi(username);

        });

        rootCommand.Parse(args).Invoke();
    }

    private static async Task gitApi(string username)
    {
        var url = $"https://api.github.com/users/{Uri.EscapeDataString(username)}/events";

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        client.DefaultRequestHeaders.Add("User-Agent", "GitHub_User_Activity");

        try
        {
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"GitHub API returned {(int)response.StatusCode} {response.ReasonPhrase}.");
                return;
            }
                Console.WriteLine("Ответ от API:");
                Console.WriteLine(response);
            
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
        }
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