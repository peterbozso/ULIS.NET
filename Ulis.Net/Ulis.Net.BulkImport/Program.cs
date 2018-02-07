﻿using ConsoleProgressBar;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
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

                var translatorClient = bool.Parse(config["UseMicrosoftTranslator"])
                    ? new MicrosoftTranslatorClient(config["MicrosoftSubscriptionKey"]) as ITranslatorClient
                    : new GoogleTranslatorClient(config["GoogleSubscriptionKey"]);

                var ulisClient = new UlisClient(translatorClient, config["LuisAppId"], config["LuisAppKey"], config["LuisRegion"]);
                
                using (var outputCsv = new CsvWriter(new StreamWriter(File.Create(config["OutputCsv"]), Encoding.UTF8)))
                {
                    outputCsv.WriteField(Column0Header);
                    outputCsv.WriteField(Column1Header);
                    outputCsv.NextRecord();

                    var inputLines = File.ReadAllLines(config["InputText"]);

                    var processedCount = 0;
                    foreach (var inputLine in inputLines)
                    {
                        var result = await ulisClient.QueryAsync(inputLine);

                        outputCsv.WriteField(inputLine);
                        outputCsv.WriteField(result.Query);
                        outputCsv.NextRecord();

                        processedCount++;
                        progressBar.Progress.Report((double)processedCount / inputLines.Length);
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