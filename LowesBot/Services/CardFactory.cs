using System;
using System.IO;
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
                .Replace("%option_store%", ResourceHelper.GetString("B1"))
                .Replace("%option_order%", ResourceHelper.GetString("B2"))
                .Replace("%option_return%", ResourceHelper.GetString("B3"))
                .Replace("%prompt_title%", "Anything else?"); // "Is there anything else I can help you with?"
            return AdaptiveCard.FromJson(json).Card;
        }

        public static AdaptiveCard GetGreetingCard(IDialogContext context)
        {
            var date = DateTime.Now;
            var zone = TimeZoneHelper.DetermineZone(date);
            var state = LowesHelper.DetermineBusinessHourState(zone, date);
            var path = HostingEnvironment.MapPath($"/cards/greeting.json");
            var json = File.ReadAllText(path)
                .Replace("%option_store%", ResourceHelper.GetString("B1"))
                .Replace("%option_order%", ResourceHelper.GetString("B2"))
                .Replace("%option_return%", ResourceHelper.GetString("B3"));
            switch (state)
            {
                case BusinessHourState.Unknown:
                case BusinessHourState.Closed:
                    json = json
                        .Replace("%prompt_title%", ResourceHelper.GetString("G1"));
                    break;
                case BusinessHourState.Closing:
                    json = json
                        .Replace("%prompt_title%", ResourceHelper.GetString("G2"));
                    break;
                case BusinessHourState.Open:
                    json = json
                        .Replace("%prompt_title%", ResourceHelper.GetString("G1"));
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
                .Replace("%prompt_title%", "Order details")

                .Replace("%prompt_number%", "Number")
                .Replace("%value_number%", data.Number)

                .Replace("%prompt_date%", "Date")
                .Replace("%value_date%", data.Date)

                .Replace("%prompt_status%", "Status")
                .Replace("%value_status%", data.Status)

                .Replace("%prompt_amount%", "Amount")
                .Replace("%value_amount%", data.Amount)

                .Replace("%value_details%", data.Description)
                .Replace("%value_image%", data.Image);
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