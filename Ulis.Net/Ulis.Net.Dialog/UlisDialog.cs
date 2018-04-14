using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ulis.Net.Dialog.Attributes;
using Ulis.Net.Dialog.Translators;
using Ulis.Net.Library;

namespace Ulis.Net.Dialog
{
    [Serializable]
    public class UlisDialog<TResult> : LuisDialog<TResult>
    {
        private readonly ITranslatorClient _translator;

        public UlisDialog(TranslatorWrapperBase translator = null, params ILuisService[] services) : base(services)
        {
            if (translator == null)
            {
                var translatorAttribute = GetType().GetCustomAttributes<TranslatorAttributeBase>(inherit: true).FirstOrDefault();
                translator = translatorAttribute.TranslatorWrapper;
            }

            SetField.NotNull(out _translator, nameof(translator), translator);
        }

        protected override async Task<string> GetLuisQueryTextAsync(IDialogContext context, IMessageActivity message)
        {
            return await _translator.TranslateAsync(message.Text);
        }
    }
}