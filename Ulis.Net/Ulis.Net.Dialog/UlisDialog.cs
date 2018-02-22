using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace Ulis.Net.Dialog
{
    [Serializable]
    public class UlisDialog<TResult> : LuisDialog<TResult>
    {
        public UlisDialog(params ILuisService[] services) : base(services)
        {
        }

        protected override async Task<string> GetLuisQueryTextAsync(IDialogContext context, IMessageActivity message)
        {
            return await Task.FromResult(message.Text);
        }
    }
}