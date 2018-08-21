using System;
using System.Threading.Tasks;
using LowesBot.Models;
using Microsoft.Bot.Builder.Dialogs;

namespace LowesBot.Dialogs
{
    [Serializable]
    internal class ReturnsAndExchangesDialog : ICardDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await SendCardAsync(context);
        }

        public Task SendCardAsync(IDialogContext context)
        {
            context.PostAsync("Ready to return.");
            ExitDialog(context);
            return Task.CompletedTask;
        }

        public Task AfterSendCardAsync(IDialogContext context, IAwaitable<object> result)
        {
            ExitDialog(context);
            return Task.CompletedTask;
        }

        public Task HandleButtonInput(IDialogContext context, string value, ButtonData button)
        {
            throw new NotImplementedException();
        }

        public Task HandleFreeformInput(IDialogContext context, string text)
        {
            throw new NotImplementedException();
        }

        public Task ResumeAfterChildDialog(IDialogContext context, IAwaitable<object> result)
        {
            throw new NotImplementedException();
        }

        public void ExitDialog(IDialogContext context, object value = null)
        {
            throw new NotImplementedException();
        }
    }
}