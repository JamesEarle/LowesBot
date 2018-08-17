using LowesBot.Dialogs;
using LowesBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LowesBot.Services
{
    public static class DialogHelper
    {
        public static async Task<bool> TryUsingLuis(IDialogContext context, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            else if (TryUsingText(context, text))
            {
                return true;
            }
            var result = await LuisService.RecognizeAsync(text);
            if (result.Intents.Any()
                && result.TopScoringIntent.Score >= ConfigHelper.LuisScore)
            {
                throw new NotImplementedException();
            }
            else
            {
                return false;
            }
            return true;
        }

        public static bool TryUsingText(IDialogContext context, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            else if (new[] { "quit", "exit", "home", "restart", "reset", "go home", "home" }.Contains(text.ToLower()))
            {
                context.EndConversation(string.Empty);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> TryRecognizedFormat(IDialogContext context, string value, ResumeAfter<IMessageActivity> callback)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            else if (LowesHelper.IsValidInvoiceNumber(value))
            {
                await CardService.ShowInvoiceStatusAsync(context, new InvoiceData { Number = value }, callback);
            }
            else if (LowesHelper.IsValidOrderNumber(value))
            {
                await CardService.ShowOrderStatusAsync(context, new OrderData { Number = value }, callback);
            }
            else if (LowesHelper.IsValidPurchaseOrder(value))
            {
                await CardService.ShowPurchaseOrderStatusAsync(context, new PurchaseOrderData { Number = value }, callback);
            }
            else
            {
                return false;
            }
            return true;
        }

    }
}