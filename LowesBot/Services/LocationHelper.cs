using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Location;
using Microsoft.Bot.Connector;
namespace LowesBot.Services
{
    public static class LocationHelper
    {
        static string _key;
        static LocationHelper()
        {
            _key = ConfigHelper.MapKey;
        }

        public static void Ask(IDialogContext context, ResumeAfter<Place> callback, string prompt = null)
        {
            if (string.IsNullOrEmpty(prompt))
            {
                prompt = "What is your location?";
            }
            var dialog = new LocationDialog(_key, context.Activity.ChannelId, prompt, LocationOptions.SkipFinalConfirmation | LocationOptions.SkipFavorites, LocationRequiredFields.None);
            context.Call(dialog, callback);
        }
    }
}