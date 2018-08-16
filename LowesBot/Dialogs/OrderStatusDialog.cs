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
            await SendNumberCardAsync(context);
        }

        private async Task SendNumberCardAsync(IDialogContext context)
        {
            var card = CardService.GetOrderNumberCard();
            await context.PostAsync(card);
            context.Wait(ReceiveNumberCardAsync);
        }

        private async Task ReceiveNumberCardAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var json = activity.Value.ToString();

            var button = ButtonData.Parse(json);
            if (button.Id == 1)
            {
                var order = OrderData.Parse(json);
                if (LowesHelper.IsValidOrderId(order.Number))
                {
                    await SendStatusCardAsync(context, order);
                }
                else
                {
                    // tell them it is invalid
                }
            }
            else if (button.Id == 2)
            {
                // they clicked cancel
                // TODO: return home
                context.Done("Heading home.");
            }
        }

        private async Task SendStatusCardAsync(IDialogContext context, OrderData order)
        {
            var card = CardService.GetOrderStatusCard(order);
            await context.PostAsync(card);
            // TODO: return home
            context.Done("Heading home.");
        }
    }
}