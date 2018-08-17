using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using LowesBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Location;
using Microsoft.Bot.Connector;

namespace LowesBot.Dialogs
{
    [Serializable]
    public class FindStoreDialog :  IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await RequestLocationAsync(context);
            // context.Wait(MessageReceivedAsync);
        }

        private Task RequestLocationAsync(IDialogContext context)
        {
            var apiKey = ConfigHelper.MapKey;
            var prompt = "Where should I ship your order? Type or say an address.";
            var locationDialog = new LocationDialog(apiKey, context.Activity.ChannelId, prompt);
            context.Call(locationDialog, AfterLocationDialog);
            return Task.CompletedTask;
        }

        private async Task AfterLocationDialog(IDialogContext context, IAwaitable<Place> result)
        {
            try
            {
                var place = await result;
                if (place != null)
                {
                    var address = place.GetPostalAddress();
                    var name = address != null ?
                        $"{address.StreetAddress}, {address.Locality}, {address.Region}, {address.Country} ({address.PostalCode})" :
                        "the pinned location";
                    await context.PostAsync($"OK, I will ship it to {name}");
                }
                else
                {
                    await context.PostAsync("OK, cancelled");
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            var userLocation = String.Empty;
            var getLocation = false;

            context.UserData.TryGetValue<string>("Location", out userLocation);
            context.UserData.TryGetValue<bool>("GetLocation", out getLocation);

            if (getLocation)
            {
                userLocation = message.Text;
                context.UserData.SetValue<string>("Location", userLocation);
                context.UserData.SetValue<bool>("GetLocation", false);
            }

            await Respond(context, message);

            // call next dialog to show locations near them
            context.Done(message);
        }

        private static async Task Respond(IDialogContext context, IMessageActivity message = null)
        {
            var userLocation = String.Empty;

            context.UserData.TryGetValue<string>("Location", out userLocation);
            // context.UserData.SetValue<bool>("GetLocation", true);

            if (String.IsNullOrEmpty(userLocation))
            {
                await context.PostAsync("Where are you located?");
            }
            else
            {
                // Store in App Secrets later
                var apiKey = ConfigHelper.MapKey;
                var prompt = $"We found this location for {userLocation}, is this right?.";
                var locationDialog = new LocationDialog(apiKey, message.ChannelId, prompt);

                context.Call(locationDialog, (dialogContext, result) =>
                {
                    return null;
                });
                // Find Location and verify using Locatioon, prompt for "is this correct?"
                //await context.PostAsync($"We found this location for {userLocation}, is this right?.");
                // show embedded map with Location results.
            }
        }
    }
}