using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Threading.Tasks;
using System.Web.Configuration;
using Ulis.Net.Dialog;

namespace Ulis.Net.TrainBot.Dialogs
{
    [Serializable]
    public class RootDialog : UlisDialog<object>
    {
        private static LuisService LuisService =>
            new LuisService(
                new LuisModelAttribute(WebConfigurationManager.AppSettings["LuisModelId"],
                    WebConfigurationManager.AppSettings["LuisSubscriptionKey"],
                    domain: WebConfigurationManager.AppSettings["LuisDomain"]));

        public RootDialog() : base(LuisService)
        {
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }
    }
}