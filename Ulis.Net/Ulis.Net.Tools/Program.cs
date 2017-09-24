using System.IO;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Ulis.Net.Library;

namespace Ulis.Net.Tools
{
    class Program
    {
        private const string Column0Header = "Original text";
        private const string Column1Header = "Translated text";

        private static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var translatorClient = bool.Parse(config["UseMicrosoftTranslator"])
                ? new MicrosoftTranslatorClient(config["MicrosoftSubscriptionKey"]) as ITranslatorClient
                : new GoogleTranslatorClient(config["GoogleSubscriptionKey"]);

            var ulisClient = new UlisClient(translatorClient, config["LuisAppId"], config["LuisAppKey"]);

            using (var inputCsv = new CsvReader(File.OpenText(config["InputCsv"]),
                new CsvConfiguration {HasHeaderRecord = false}))
            using (var outputCsv = new CsvWriter(new StreamWriter(File.Create(config["OutputCsv"]), Encoding.UTF8)))
            {
                outputCsv.WriteField(Column0Header);
                outputCsv.WriteField(Column1Header);
                outputCsv.NextRecord();

                while (inputCsv.Read())
                {
                    var inputLine = inputCsv.GetField<string>(0);
                    var result = await ulisClient.QueryAsync(inputLine);

                    outputCsv.WriteField(inputLine);
                    outputCsv.WriteField(result.OriginalQuery);
                    outputCsv.NextRecord();
                }
            }
        }
    }
}