using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace Ulis.Net.Dialog
{
    [Serializable]
    public class UlisDialog<TResult> : LuisDialog<TResult>
    {
        private readonly TranslatorClientSerializationWrapper _translator;

        public UlisDialog(TranslatorClientSerializationWrapper translator, params ILuisService[] services) : base(services)
        {
            SetField.NotNull(out _translator, nameof(translator), translator);
        }

        protected override async Task<string> GetLuisQueryTextAsync(IDialogContext context, IMessageActivity message)
        {
            return await _translator.TranslateAsync(message.Text);
        }
    }
}