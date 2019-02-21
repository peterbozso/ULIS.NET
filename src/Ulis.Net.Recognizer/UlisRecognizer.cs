using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Schema;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ulis.Net.Library;

namespace Ulis.Net.Recognizer
{
    public class UlisRecognizer : LuisRecognizer, IRecognizer
    {
        private readonly ITranslatorClient _translatorClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UlisRecognizer"/> class.
        /// </summary>
        /// <param name="application">The LUIS application to use to recognize text.</param>
        /// <param name="translatorClient">Implementation of the <see cref="ITranslatorClient"/> interface that will do the translation before sending the text to LUIS.</param>
        /// <param name="predictionOptions">(Optional) The LUIS prediction options to use. Spell checking options will be ignored, because doing spell checking after translation and before LUIS could skew the results. Spell checking should be done before using this recognizer.</param>
        /// <param name="includeApiResults">(Optional) TRUE to include raw LUIS API response.</param>
        /// <param name="clientHandler">(Optional) Custom handler for LUIS API calls to allow mocking.</param>
        public UlisRecognizer(LuisApplication application, ITranslatorClient translatorClient,
            LuisPredictionOptions predictionOptions = null, bool includeApiResults = false, HttpClientHandler clientHandler = null)
            : base(application, predictionOptions, includeApiResults, clientHandler)
        {
            _translatorClient = translatorClient;

            if (predictionOptions != null)
            {
                predictionOptions.BingSpellCheckSubscriptionKey = null;
                predictionOptions.SpellCheck = false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UlisRecognizer"/> class.
        /// </summary>
        /// <param name="service">The LUIS service from configuration.</param>
        /// <param name="translatorClient">Implementation of the <see cref="ITranslatorClient"/> interface that will do the translation before sending the text to LUIS.</param>
        /// <param name="predictionOptions">(Optional) The LUIS prediction options to use. Spell checking options will be ignored, because doing spell checking after translation and before LUIS could skew the results. Spell checking should be done before using this recognizer.</param>
        /// <param name="includeApiResults">(Optional) TRUE to include raw LUIS API response.</param>
        /// <param name="clientHandler">(Optional) Custom handler for LUIS API calls to allow mocking.</param>
        public UlisRecognizer(LuisService service, ITranslatorClient translatorClient,
            LuisPredictionOptions predictionOptions = null, bool includeApiResults = false, HttpClientHandler clientHandler = null)
            : base(service, predictionOptions, includeApiResults, clientHandler)
        {
            _translatorClient = translatorClient;

            if (predictionOptions != null)
            {
                predictionOptions.BingSpellCheckSubscriptionKey = null;
                predictionOptions.SpellCheck = false;
            }
        }

        public new async Task<RecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var utterance = GetUtterance(turnContext);
            if (utterance == null)
            {
                return null;
            }
            
            var translatedUtterance = await _translatorClient.TranslateAsync(utterance).ConfigureAwait(false);
            turnContext.Activity.Text = translatedUtterance;

            var result = await base.RecognizeAsync(turnContext, cancellationToken).ConfigureAwait(false);
            result.Text = utterance;
            result.AlteredText = translatedUtterance;

            turnContext.Activity.Text = utterance;

            return result;
        }

        public new async Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken)
            where T : IRecognizerConvert, new()
        {
            var result = new T();
            result.Convert(await RecognizeAsync(turnContext, cancellationToken).ConfigureAwait(false));
            return result;
        }

        private static string GetUtterance(ITurnContext turnContext)
        {
            if (turnContext.Activity.Type != ActivityTypes.Message)
            {
                return null;
            }

            var utterance = turnContext.Activity?.AsMessageActivity()?.Text;

            if (string.IsNullOrWhiteSpace(utterance))
            {
                return null;
            }

            return utterance;
        }
    }
}
