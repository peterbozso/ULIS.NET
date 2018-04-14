using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web.Configuration;
using Ulis.Net.Dialog;
using Ulis.Net.Dialog.Translators;

namespace Ulis.Net.TrainBot.Dialogs
{
    [Serializable]
    public class RootDialog : UlisDialog<object>
    {
        private static NameValueCollection Config => WebConfigurationManager.AppSettings;

        private static LuisService LuisService =>
            new LuisService(
                new LuisModelAttribute(Config["LuisModelId"], Config["LuisSubscriptionKey"], domain: Config["LuisDomain"]));

        private static TranslatorSerializationWrapperBase Translator
        {
            get
            {
                var subscriptionKey = Config["TranslatorSubscriptionKey"];

                return Config["TranslatorProvider"] == "Microsoft"
                    ? new MicrosoftTranslatorWrapper(subscriptionKey) as TranslatorSerializationWrapperBase
                    : new GoogleTranslatorWrapper(subscriptionKey);
            }
        }

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