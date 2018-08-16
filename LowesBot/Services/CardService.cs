using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;

namespace LowesBot
{
    public static class CardService
    {
        public static AdaptiveCard GetGreetingCard()
        {
            var path = HostingEnvironment.MapPath($"/cards/greeting.json");
            var json = File.ReadAllText(path); // .Replace("%initialvalue%", initial_text);
            return AdaptiveCard.FromJson(json).Card;
        }

        public static AdaptiveCard GetOrderNumberCard()
        {
            var path = HostingEnvironment.MapPath($"/cards/ordernumber.json");
            var json = File.ReadAllText(path); // .Replace("%initialvalue%", initial_text);
            return AdaptiveCard.FromJson(json).Card;
        }

        public static AdaptiveCard GetOrderStatusCard()
        {
            var path = HostingEnvironment.MapPath($"/cards/orderstatus.json");
            var json = File.ReadAllText(path); // .Replace("%initialvalue%", initial_text);
            return AdaptiveCard.FromJson(json).Card;
        }

        //public static async Task SendTranslateCard(IDialogContext context, string initial_text)
        //{
        //    var card = GetTranslateCard(initial_text);
        //    await context.PostAsync(card);
        //}
    }
}