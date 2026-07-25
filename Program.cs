using Octokit;
using Spectre.Console;
using Spectre.Console.Cli;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Net.Http.Headers;

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

            string username = parseResult.GetValue(name);

            Console.WriteLine($"Name: {parseResult.GetValue(name)}");
            try
            {
                Console.WriteLine($"Запрос активности для пользователя: {username}...");
                GetUserActivityAsync(username);
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Ошибка API GitHub: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        });

        rootCommand.Parse(args).Invoke();
    }

    private static readonly GitHubClient client = new GitHubClient( new Octokit.ProductHeaderValue("GithubUserActivity"));
    

    static async Task GetUserActivityAsync(string username)
    {
        // client.Credentials = new Credentials("ваш_personal_access_token");
        // Получаем список публичных событий пользователя (ограничено последними 300 событиями)
        var events = await client.Activity.Events.GetAllUserPerformedPublic(username);

        if (events.Count == 0)
        {
            Console.WriteLine("Публичная активность за последние 90 дней не найдена.");
            return;
        }

        Console.WriteLine($"\nНайдено записей об активности: {events.Count}");

        // Извлекаем самое последнее событие
        var lastEvent = events[0];
        Console.WriteLine($"\nПоследнее действие:");
        Console.WriteLine($"- Тип события: {lastEvent.Type}");
        Console.WriteLine($"- Репозиторий: {lastEvent.Repo.Name}");
        Console.WriteLine($"- Дата и время: {lastEvent.CreatedAt.ToLocalTime()}");
    }

    //private void gitApi()
    //{
    //    client.DefaultRequestHeaders.Accept.Clear();
    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
    //    client.DefaultRequestHeaders.Add("User-Agent", "CSharp-Console-GitHub-App");
    //}

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