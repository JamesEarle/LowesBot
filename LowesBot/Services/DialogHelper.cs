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
        }

        public static bool TryParseExit(object value)
        {
            if (value == null
                || string.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }
            else
            {
                return new[] { "quit", "exit", "home", "restart", "reset", "go home", "home", "stop" }
                    .Contains(value.ToString().ToLower());
            }
        }

        public static async Task<bool> TryRecognizedFormat(IDialogContext context, string value, ResumeAfter<IMessageActivity> callback)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            else if (BusinessRulesService.ValidateInvoiceNumber(value))
            {
                await CardService.ShowInvoiceStatusAsync(context, new InvoiceData { Number = value }, callback);
            }
            else if (BusinessRulesService.TryParseOrderNumber(value, out var orderNumber))
            {
                await CardService.ShowOrderStatusCardAsync(context, new OrderData { Number = orderNumber }, callback);
            }
            else if (BusinessRulesService.ValidatePurchaseOrderNumber(value))
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