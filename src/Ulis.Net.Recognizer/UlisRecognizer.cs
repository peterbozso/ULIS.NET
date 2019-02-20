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
    internal class UlisRecognizer : LuisRecognizer, IRecognizer
    {
        private readonly ITranslatorClient _translator;

        public UlisRecognizer(LuisApplication application, ITranslatorClient translator,
            LuisPredictionOptions predictionOptions = null, bool includeApiResults = false, HttpClientHandler clientHandler = null)
            : base(application, predictionOptions, includeApiResults, clientHandler)
        {
            _translator = translator;
        }

        public UlisRecognizer(LuisService service, ITranslatorClient translator,
            LuisPredictionOptions predictionOptions = null, bool includeApiResults = false, HttpClientHandler clientHandler = null)
            : base(service, predictionOptions, includeApiResults, clientHandler)
        {
            _translator = translator;
        }

        public new async Task<RecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var utterance = GetUtterance(turnContext);
            if (utterance == null)
            {
                return null;
            }
            
            var translatedUtterance = await _translator.TranslateAsync(utterance).ConfigureAwait(false);
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
