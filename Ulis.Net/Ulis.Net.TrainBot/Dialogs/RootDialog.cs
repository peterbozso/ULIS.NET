using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
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

        private static TranslatorClientSerializationWrapper Translator =>
            new TranslatorClientSerializationWrapper(
                (TranslationProvider)Enum.Parse(typeof(TranslationProvider), WebConfigurationManager.AppSettings["TranslatorProvider"]),
                WebConfigurationManager.AppSettings["TranslatorSubscriptionKey"]);

        public RootDialog() : base(Translator, LuisService)
        {
        }

        protected override async Task DispatchToIntentHandler(IDialogContext context, IAwaitable<IMessageActivity> item,
           IntentRecommendation bestIntent, LuisResult result)
        {
            string message =
                $"ULIS translated that as: {result.Query}\n\n" +
                $"Intent: {result.TopScoringIntent.Intent} | Score: {result.TopScoringIntent.Score}\n\n" +
                $"Entities: {JsonConvert.SerializeObject(result.Entities)}";
            await context.PostAsync(message);
            context.Done(0);
        }
    }
}