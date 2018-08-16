using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace LowesBot.Dialogs
{
    [Serializable]
    internal class ReturnsAndExchangesDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            throw new System.NotImplementedException();
        }

        public async Task MessageReceivedAsync(IDialogContext context)
        {
            await context.PostAsync("What is your order number?");
        }
    }
}