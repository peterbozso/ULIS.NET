using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace Ulis.Net.TrainBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var ulisResult = await WebApiApplication.UlisClient.QueryAsync(activity.Text);

            await context.PostAsync($"Translated as: {ulisResult.LuisResult.Query} \n\nTop scoring intent:" +
                $"{ulisResult.LuisResult.TopScoringIntent.Intent} ({ulisResult.LuisResult.TopScoringIntent.Score})");

            context.Wait(MessageReceivedAsync);
        }
    }
}