using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleProgressBar;
using CsvHelper;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Ulis.Net.Recognizer;
using Ulis.Net.Recognizer.Translation;

namespace Ulis.Net.BulkImport
{
    internal class Program
    {
        private const string Column0Header = "Original text";
        private const string Column1Header = "Translated text";
        private const string SettingsFileName = "appsettings.json";

        private static TurnContext GetContext(string utterance)
        {
            var adapter = new TestAdapter();
            var activity = new Activity
            {
                Type = ActivityTypes.Message,
                Text = utterance,
                Conversation = new ConversationAccount(),
                Recipient = new ChannelAccount(),
                From = new ChannelAccount()
            };
            return new TurnContext(adapter, activity);
        }

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
                var sourceLanguage = config["SourceLanguage"];
                var translatorClient = config["TranslatorProvider"] == "Microsoft"
                    ? new MicrosoftTranslatorClient(new HttpClient(), translatorSubscriptionKey, sourceLanguage)
                    : new GoogleTranslatorClient(translatorSubscriptionKey, sourceLanguage)
                    as ITranslatorClient;

                var luisApplication = new LuisApplication(config["LuisAppId"], config["LuisEndpointKey"], config["LuisEndpoint"]);

                var ulisRecognizer = new UlisRecognizer(luisApplication, translatorClient);

                using (var outputCsv = new CsvWriter(new StreamWriter(File.Create(config["OutputCsv"]), Encoding.UTF8)))
                {
                    outputCsv.WriteField(Column0Header);
                    outputCsv.WriteField(Column1Header);
                    outputCsv.NextRecord();

                    var inputLines = File.ReadAllLines(config["InputText"], Encoding.UTF8);

                    var processedCount = 0;
                    foreach (var inputLine in inputLines)
                    {
                        var result = await ulisRecognizer.RecognizeAsync(GetContext(inputLine), CancellationToken.None);

                        outputCsv.WriteField(result.Text);
                        outputCsv.WriteField(result.AlteredText);
                        outputCsv.NextRecord();

                        processedCount++;
                        progressBar.Progress.Report((double)processedCount / inputLines.Length);
                    }
                }
            }
        }
    }
}