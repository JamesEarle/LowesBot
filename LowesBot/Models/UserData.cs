using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace LowesBot.Models
{
    public static class UserData
    {
        public static bool TryGetLocation(IDialogContext context, out Place place)
        {
            if (context.UserData.TryGetValue<string>("Location", out var json))
            {
                place = JsonConvert.DeserializeObject<Place>(json);
                return true;
            }
            else
            {
                place = default(Place);
                return false;
            }
        }
        public static void SetLocation(IDialogContext context, Place place)
        {
            var json = JsonConvert.SerializeObject(place);
            context.UserData.SetValue("Location", json);
        }
    }
}
