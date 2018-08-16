using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Location;
using Microsoft.Bot.Connector;

namespace LowesBot.Dialogs
{
    [Serializable]
    public class FindStoreDialog :  IDialog<object>
    {
        private const string AzureMapsKey = "9tTbSzD-t9Oc4DTDFie47oPbjDW6NhI2wmVUtn3EL9Q";

        public async Task StartAsync(IDialogContext context)
        {
            await Respond(context);

            context.Wait(MessageReceivedAsync);
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

            if (String.IsNullOrEmpty(userLocation))
            {
                await context.PostAsync("Where are you located?");
                context.UserData.SetValue<bool>("GetLocation", true);
            }
            else
            {
                // Store in App Secrets later
                var apiKey = AzureMapsKey;
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