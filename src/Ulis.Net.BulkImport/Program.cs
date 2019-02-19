using ConsoleProgressBar;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ulis.Net.Library;

namespace Ulis.Net.BulkImport
{
    internal class Program
    {
        private const string SettingsFileName = "appsettings.json";
        private const string Column0Header = "Original text";
        private const string Column1Header = "Translated text";

        private static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            using (var progressBar = new ProgressBar())
            {
                progressBar.Progress.Report(0);

                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(SettingsFileName).Build();

                var translatorSubscriptionKey = config["TranslatorSubscriptionKey"];
                var translatorClient = config["TranslatorProvider"] == "Microsoft"
                    ? new MicrosoftTranslatorClient(translatorSubscriptionKey)
                    : new GoogleTranslatorClient(translatorSubscriptionKey)
                    as ITranslatorClient;

                var ulisClient = new UlisClient(new HttpClient(), translatorClient,
                    config["LuisModelId"], config["LuisSubscriptionKey"], config["LuisDomain"]);
                
                using (var outputCsv = new CsvWriter(new StreamWriter(File.Create(config["OutputCsv"]), Encoding.UTF8)))
                {
                    outputCsv.WriteField(Column0Header);
                    outputCsv.WriteField(Column1Header);
                    outputCsv.NextRecord();

                    var inputLines = File.ReadAllLines(config["InputText"], Encoding.UTF8);

                    var processedCount = 0;
                    foreach (var inputLine in inputLines)
                    {
                        var result = await ulisClient.QueryAsync(inputLine);

                        outputCsv.WriteField(result.OriginalQuery);
                        outputCsv.WriteField(result.LuisResult.Query);
                        outputCsv.NextRecord();

                        processedCount++;
                        progressBar.Progress.Report((double)processedCount / inputLines.Length);
                    }
                }
            }
        }
    }
}