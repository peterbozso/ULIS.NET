using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Ulis.Net.Recognizer;

namespace Ulis.Net.TrainingBot
{
    public class TrainingBot : IBot
    {
        private readonly UlisRecognizer _ulisRecognizer;

        public TrainingBot(UlisRecognizer ulisRecognizer)
        {
            _ulisRecognizer = ulisRecognizer;
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var result = await _ulisRecognizer.RecognizeAsync(turnContext, cancellationToken);
                var topScoringIntent = result.GetTopScoringIntent();

                var message =
                    $"ULIS translated that as: \"{result.AlteredText}\"\n\n" +
                    $"Top scoring intent: {topScoringIntent.intent} | Score: {topScoringIntent.score}\n\n" +
                    $"Entities:\n\n{result.Entities}";

                await turnContext.SendActivityAsync(message, cancellationToken: cancellationToken);
            }
        }
    }
}