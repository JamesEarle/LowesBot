using System.Threading.Tasks;
using LowesBot.Models;
using LowesBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LowesBot.Services
{
    public static class CardService
    {
        public static async Task ShowOrderStatusAsync(IDialogContext context, OrderData data, ResumeAfter<IMessageActivity> callback)
        {
            var card = CardFactory.GetOrderStatusCard(data);
            await context.PostAsync(card);
            context.Wait(callback);
        }

        public static async Task ShowPurchaseOrderStatusAsync(IDialogContext context, PurchaseOrderData data, ResumeAfter<IMessageActivity> callback)
        {
            var card = CardFactory.GetPurchaseOrderStatusCard(data);
            await context.PostAsync(card);
            context.Wait(callback);
        }

        public static async Task ShowInvoiceStatusAsync(IDialogContext context, InvoiceData data, ResumeAfter<IMessageActivity> callback)
        {
            var card = CardFactory.GetInvoiceStatusCard(data);
            await context.PostAsync(card);
            context.Wait(callback);
        }
    }
}