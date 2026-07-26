using GitHub_User_Activity;
using Spectre.Console;
using Spectre.Console.Cli;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GitHub_User_Activity
{
    public class API
    {
        //private static readonly HttpClient client = new HttpClient();
        //client.DefaultRequestHeaders.Add("User-Agent", "GitHub_User_Activity");
        private static readonly HttpClient client = new HttpClient();
        public static async Task<string?> gitApi(string username)
        {
            var url = $"https://api.github.com/users/{Uri.EscapeDataString(username)}/events";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "GitHub_User_Activity");

            try
            {
                var events = await client.GetFromJsonAsync<GitHubEvent[]>(url);
                Console.WriteLine($"\nНайдено событий: {events.Length}\n");

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"GitHub API вернул {(int)response.StatusCode} {response.ReasonPhrase}.");
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

        public static void DisplayData(string data)
        {
            try
            {
                Console.WriteLine(data);

                using var doc = JsonDocument.Parse(data);
                var root = doc.RootElement;


            }
            catch (HttpRequestException e) { Console.WriteLine("Display_error @e", e); }
        }
    }
    public class GitHubEvent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("repo")]
        public RepoInfo Repo { get; set; }

        [JsonPropertyName("payload")]
        public PayloadInfo Payload { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }
    public class RepoInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class PayloadInfo
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("commits")]
        public CommitInfo[] Commits { get; set; }

        [JsonPropertyName("issue")]
        public IssueInfo Issue { get; set; }
    }

    public class CommitInfo
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class IssueInfo
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
