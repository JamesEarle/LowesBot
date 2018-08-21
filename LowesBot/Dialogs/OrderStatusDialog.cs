using System;
using System.Threading.Tasks;
using LowesBot.Models;
using LowesBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LowesBot.Dialogs
{
    [Serializable]
    internal class OrderStatusDialog : ICardDialog
    {
        private OrderData _order;

        public Task StartAsync(IDialogContext context)
        {
            AskForOrderNumber(context);
            return Task.CompletedTask;
        }

        #region AskForOrderNumber

        private void AskForOrderNumber(IDialogContext context)
        {
            context.Call(new OrderNumberDialog(), AfterAskForOrderNumberAsync);
        }

        public async Task AfterAskForOrderNumberAsync(IDialogContext context, IAwaitable<object> result)
        {
            if ((await result) is OrderData order)
            {
                _order = order;
                await SendCardAsync(context);
            }
            else
            {
                ExitDialog(context);
            }
        }

        #endregion

        public async Task SendCardAsync(IDialogContext context)
        {
            await CardService.ShowOrderStatusCardAsync(context, _order, null);
            PromptDialog.Confirm(context, ResumeAfterConfirm, "Look up another order?");
        }

        public Task AfterSendCardAsync(IDialogContext context, IAwaitable<object> result)
        {
            throw new NotImplementedException();
        }

        private async Task ResumeAfterConfirm(IDialogContext context, IAwaitable<bool> answer)
        {
            if (await answer)
            {
                await StartAsync(context);
            }
            else
            {
                ExitDialog(context);
            }
        }

        public Task HandleButtonInput(IDialogContext context, string value, ButtonData button) => throw new NotImplementedException();
        public Task HandleFreeformInput(IDialogContext context, string text) => throw new NotImplementedException();
        public Task ResumeAfterChildDialog(IDialogContext context, IAwaitable<object> result) => throw new NotImplementedException();

        public void ExitDialog(IDialogContext context, object value = null)
        {
            context.Done(value);
        }
    }
}