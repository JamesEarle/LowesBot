using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using AdaptiveCards;
using LowesBot.Dialogs;
using LowesBot.Models;
using LowesBot.Services;
using Microsoft.Bot.Builder.Dialogs;

namespace LowesBot.Services
{
    public static class CardFactory
    {
        public static AdaptiveCard GetAnythingElseCard()
        {
            var path = HostingEnvironment.MapPath($"/cards/greeting.json");
            var json = File.ReadAllText(path);
            json = json
                .Replace("%option1%", ResourceHelper.GetString("No"))
                .Replace("%option2%", ResourceHelper.GetString("Yes"))
                .Replace("%messagetext%", ResourceHelper.GetString("A1")); // "Is there anything else I can help you with?"
            return AdaptiveCard.FromJson(json).Card;
        }

        public static AdaptiveCard GetGreetingCard(IDialogContext context)
        {
            var date = DateTime.Now;
            var zone = TimeZoneHelper.DetermineZone(date);
            var state = LowesHelper.DetermineBusinessHourState(zone, date);
            var path = HostingEnvironment.MapPath($"/cards/greeting.json");
            var json = File.ReadAllText(path);
            switch (state)
            {
                case BusinessHourState.Unknown:
                case BusinessHourState.Closed:
                    json = json
                        .Replace("%option1%", ResourceHelper.GetString("B1")) // Find a store.
                        .Replace("%option2%", ResourceHelper.GetString("B2")) // Check order status
                        .Replace("%option3%", ResourceHelper.GetString("B3")) // Returns & Exchanges
                        .Replace("%greetingtext%", ResourceHelper.GetString("G1"));
                    break;
                case BusinessHourState.Closing:
                    json = json
                        .Replace("%option1%", ResourceHelper.GetString("B1")) // Find a store.
                        .Replace("%option2%", ResourceHelper.GetString("B2")) // Check order status
                        .Replace("%option3%", ResourceHelper.GetString("B3")) // Returns & Exchanges
                        .Replace("%greetingtext%", ResourceHelper.GetString("G2"));
                    break;
                case BusinessHourState.Open:
                    json = json
                        .Replace("%option1%", ResourceHelper.GetString("B1")) // Find a store.
                        .Replace("%option2%", ResourceHelper.GetString("B2")) // Check order status
                        .Replace("%option3%", ResourceHelper.GetString("B3")) // Returns & Exchanges
                        .Replace("%greetingtext%", ResourceHelper.GetString("G1"));
                    break;
            }
            return AdaptiveCard.FromJson(json).Card;
        }

        public static AdaptiveCard GetOrderNumberCard()
        {
            var path = HostingEnvironment.MapPath($"/cards/ordernumber.json");
            var json = File.ReadAllText(path)
                .Replace("%prompt1%", ResourceHelper.GetString("B1_1")) // Provide your order number (textblock)
                .Replace("%prompt2%", ResourceHelper.GetString("B1_2")) // Order Number (placeholdertext)
                .Replace("%option1%", "Submit") // Submit Order Number (button)
                .Replace("%option2%", "Cancel"); // Cancel (button)
            return AdaptiveCard.FromJson(json).Card;
        }

        public static AdaptiveCard GetOrderStatusCard(OrderData data)
        {
            var path = HostingEnvironment.MapPath($"/cards/orderstatus.json");
            var json = File.ReadAllText(path)
                .Replace("%orderNumber%", data.Number) // #
                .Replace("%prompt2%", "Order Number"); // Order Number (textblock)
            return AdaptiveCard.FromJson(json).Card;
        }

        public static AdaptiveCard GetInvoiceStatusCard(InvoiceData data)
        {
            throw new NotImplementedException();
        }

        public static AdaptiveCard GetPurchaseOrderStatusCard(PurchaseOrderData data)
        {
            throw new NotImplementedException();
        }
    }
}