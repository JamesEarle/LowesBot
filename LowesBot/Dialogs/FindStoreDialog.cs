using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using LowesBot.Models;
using LowesBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Location;
using Microsoft.Bot.Connector;

namespace LowesBot.Models
{
}

namespace LowesBot.Dialogs
{
    [Serializable]
    public class FindStoreDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            if (UserData.TryGetLocation(context, out var place))
            {
                await VerifySavedLocationAsync(context, place);
            }
            else
            {
                await RequestLocationAsync(context);
            }
        }

        #region VerifySavedLocation

        private Task VerifySavedLocationAsync(IDialogContext context, Place place)
        {
            PromptDialog.Confirm(context, AfterSavedVerifyLocationAsync, $"Is {place.ToString()} still your location?");
            return Task.CompletedTask;
        }

        private async Task AfterSavedVerifyLocationAsync(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await SearchForStoresAsync(context);
            }
            else
            {
                await RequestLocationAsync(context);
            }
        }

        #endregion

        #region RequestLocation

        private Task RequestLocationAsync(IDialogContext context)
        {
            LocationHelper.Ask(context, AfterRequestLocationAsync);
            return Task.CompletedTask;
        }

        private async Task AfterRequestLocationAsync(IDialogContext context, IAwaitable<Place> result)
        {
            try
            {
                var place = await result;
                if (place != null)
                {
                    var address = place.GetPostalAddress();
                    UserData.SetLocation(context, place);
                }
                else
                {
                    await context.PostAsync("OK, cancelled");
                }

            }
            catch (Exception ex)
            {
                await context.PostAsync(ex.Message);
                throw;
            }
        }

        #endregion

        private Task SearchForStoresAsync(IDialogContext context)
        {
            throw new NotImplementedException();
        }

    }
}
