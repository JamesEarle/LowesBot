using System;
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
            if (UserData.Location.TryRead(context, out var place))
            {
                await VerifySavedLocationAsync(context, place);
            }
            else
            {
                await RequestUserLocationAsync(context);
            }
        }

        #region VerifyUserSavedLocation

        private Task VerifySavedLocationAsync(IDialogContext context, Place place)
        {
            PromptDialog.Confirm(context, AfterSavedVerifyLocationAsync, $"Is {place.Name} still your location?");
            return Task.CompletedTask;
        }

        private async Task AfterSavedVerifyLocationAsync(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await ShowStoresAsync(context);
            }
            else
            {
                UserData.Location.Clear(context);
                await RequestUserLocationAsync(context);
            }
        }

        #endregion

        #region RequestUserLocation

        private Task RequestUserLocationAsync(IDialogContext context)
        {
            LocationHelper.Ask(context, AfterRequestUserLocationAsync);
            return Task.CompletedTask;
        }

        private async Task AfterRequestUserLocationAsync(IDialogContext context, IAwaitable<Place> result)
        {
            try
            {
                var place = await result;
                if (place != null)
                {
                    if (!UserData.Location.HasValue(context))
                    {
                        UserData.Location.Write(context, place);
                    }
                    await ShowStoresAsync(context);
                }
                else
                {
                    context.Done(string.Empty);
                }

            }
            catch (Exception ex)
            {
                await context.PostAsync(ex.Message);
                context.Done(string.Empty);
            }
        }

        #endregion

        private async Task ShowStoresAsync(IDialogContext context)
        {
            if (UserData.Location.TryRead(context, out var place))
            {
                var stores = StoreService.FindStores(place);
                if (stores.Any())
                {
                    await CardService.ShowStoreContactCardsAsync(context, stores, CardService.Arrangement.Carousel, null);
                    PromptDialog.Confirm(context, AfterSearchAgainPromptAsync, "Would you like to search again?");
                }
                else
                {
                    await context.PostAsync("No stores found.");
                    context.Done(string.Empty);
                }
            }
            else
            {
                await context.PostAsync("Place not found.");
                context.Done(string.Empty);
            }
        }

        private async Task AfterSearchAgainPromptAsync(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await RequestUserLocationAsync(context);
            }
            else
            {
                context.Done(string.Empty);
            }
        }
    }
}

