using GitHub_User_Activity;
using Octokit;
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
            var activity = await gitApi(username);

            if (activity == null)
            {
                return;
            }

            DisplayData(activity);

        });

        rootCommand.Parse(args).Invoke();
    }

    private static async Task<string?> gitApi(string username)
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
                return null;
            }
                //Console.WriteLine("Ответ от API:");
                //Console.WriteLine(response);

            var content = await response.Content.ReadAsStringAsync();
            return content;

        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
            return null;
        }
    }

    private static void DisplayData(string data)
    {
        try
        {
            using var doc = JsonDocument.Parse(data);
            var root = doc.RootElement;


        } 
        catch (HttpRequestException e) { Console.WriteLine("Display_error"); }
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