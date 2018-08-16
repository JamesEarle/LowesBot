using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LowesBot.Dialogs
{
    [Serializable]
    internal class OrderStatusDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var card = CardService.GetOrderStatusCard();
            await context.PostAsync(card);
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            var orderNumber = message.Text;

            await context.PostAsync($"Checking status of order #{orderNumber}...");
            var status = CheckStatus(orderNumber);
            await context.PostAsync(status);

            context.Done(message);
        }

        private string CheckStatus(string orderNumber)
        {
            // Do DB Call given order number
            return $"Order #{orderNumber} - Out for delivery on {DateTime.Today.ToString("M/d")}";
        }
    }
}