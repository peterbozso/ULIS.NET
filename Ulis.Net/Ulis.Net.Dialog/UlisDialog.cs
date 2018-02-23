using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ulis.Net.Dialog
{
    [Serializable]
    public class UlisDialog<TResult> : LuisDialog<TResult>
    {
        private readonly TranslatorClientSerializationWrapper _translator;

        public UlisDialog(TranslatorClientSerializationWrapper translator = null,
            params ILuisService[] services) : base(services)
        {
            if (translator == null)
            {
                var translatorAttribute =
                    GetType().GetCustomAttributes<UlisTranslatorAttribute>(inherit: true).FirstOrDefault();
                translator = new TranslatorClientSerializationWrapper(
                    translatorAttribute.TranslationProvider, translatorAttribute.SubscriptionKey);
            }

            SetField.NotNull(out _translator, nameof(translator), translator);
        }

        protected override async Task<string> GetLuisQueryTextAsync(IDialogContext context, IMessageActivity message)
        {
            return await _translator.TranslateAsync(message.Text);
        }
    }
}