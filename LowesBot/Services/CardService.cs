using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using AdaptiveCards;
using LowesBot.Dialogs;
using LowesBot.Services;
using Microsoft.Bot.Builder.Dialogs;

namespace LowesBot
{
    public static class CardService
    {
        public static AdaptiveCard GetGreetingCard(State state)
        {
            var path = HostingEnvironment.MapPath($"/cards/greeting.json");
            var json = File.ReadAllText(path);
            switch (state)
            {
                case State.Welcome_Closed:
                    json = json
                        .Replace("%option1%", ResourceHelper.GetString("B1")) // Find a store.
                        .Replace("%option2%", ResourceHelper.GetString("B2")) // Check order status
                        .Replace("%option3%", ResourceHelper.GetString("B3")) // Returns & Exchanges
                        .Replace("%greetingtext%", ResourceHelper.GetString("G1"));
                    break;
                case State.Welcome_AlmostClosed:
                    json = json
                        .Replace("%option1%", ResourceHelper.GetString("B1")) // Find a store.
                        .Replace("%option2%", ResourceHelper.GetString("B2")) // Check order status
                        .Replace("%option3%", ResourceHelper.GetString("B3")) // Returns & Exchanges
                        .Replace("%greetingtext%", ResourceHelper.GetString("G2"));
                    break;
                case State.Welcome_Open:
                    json = json
                        .Replace("%option1%", ResourceHelper.GetString("B1")) // Find a store.
                        .Replace("%option2%", ResourceHelper.GetString("B2")) // Check order status
                        .Replace("%option3%", ResourceHelper.GetString("B3")) // Returns & Exchanges
                        .Replace("%greetingtext%", ResourceHelper.GetString("G1"));
                    break;
                case State.AnythingElse:
                    json = json
                        .Replace("%option1%", ResourceHelper.GetString("No")) 
                        .Replace("%option2%", ResourceHelper.GetString("Yes")) 
                        .Replace("%messagetext%", ResourceHelper.GetString("A1")); // "Is there anything else I can help you with?"
                    break;
            }
            return AdaptiveCard.FromJson(json).Card;
        }

        public static AdaptiveCard GetOrderNumberCard()
        {
            var path = HostingEnvironment.MapPath($"/cards/ordernumber.json");
            var json = File.ReadAllText(path)
                .Replace("%prompt1%", "B1_1") // Provide your order number (textblock)
                .Replace("%prompt2%", "B1_2") // Order Number (placeholdertext)
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
    }
}