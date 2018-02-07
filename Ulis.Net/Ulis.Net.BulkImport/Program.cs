using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleProgressBar;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Ulis.Net.Library;

namespace Ulis.Net.BulkImport
{
    class Program
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

                var translatorClient = bool.Parse(config["UseMicrosoftTranslator"])
                    ? new MicrosoftTranslatorClient(config["MicrosoftSubscriptionKey"]) as ITranslatorClient
                    : new GoogleTranslatorClient(config["GoogleSubscriptionKey"]);

                var ulisClient = new UlisClient(translatorClient, config["LuisAppId"], config["LuisAppKey"]);

                using (var inputCsv = new CsvReader(File.OpenText(config["InputCsv"]),
                    new Configuration { HasHeaderRecord = false}))
                using (var outputCsv = new CsvWriter(new StreamWriter(File.Create(config["OutputCsv"]), Encoding.UTF8)))
                {
                    outputCsv.WriteField(Column0Header);
                    outputCsv.WriteField(Column1Header);
                    outputCsv.NextRecord();

                    var inputLines = inputCsv.GetRecords<InputLine>().ToList();

                    var processedCount = 0;
                    foreach (var inputLine in inputLines)
                    {
                        var result = await ulisClient.QueryAsync(inputLine.OriginalText);

                        outputCsv.WriteField(inputLine.OriginalText);
                        outputCsv.WriteField(result.OriginalQuery);
                        outputCsv.NextRecord();

                        processedCount++;
                        progressBar.Progress.Report((double) processedCount / inputLines.Count);
                    }
                }
            }
        }

        public class InputLine
        {
            public string OriginalText { get; set; }
        }
    }
}