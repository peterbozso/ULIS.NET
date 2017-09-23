using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ulis.Net.Library;

namespace Ulis.Net.Tools
{
    class Program
    {
        private static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var translatorClient = Boolean.Parse(config["UseMicrosoftTranslator"])
                ? new MicrosoftTranslatorClient(config["MicrosoftSubscriptionKey"]) as ITranslatorClient
                : new GoogleTranslatorClient(config["GoogleSubscriptionKey"]);

            var ulisClient = new UlisClient(translatorClient, config["LuisAppId"], config["LuisAppKey"]);

            var result = await ulisClient.QueryAsync("Szia!");
            Console.WriteLine(
                $"Translated text: {result.OriginalQuery} | Top scoring intent: {result.TopScoringIntent.Name}");
        }
    }
}