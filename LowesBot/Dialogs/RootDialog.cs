using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using LowesBot.Models;
using LowesBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace LowesBot.Dialogs
{
    [Serializable]
    public class RootDialog : ICardDialog
    {
        int _prompts = 0;
        private readonly Country _country;

        public RootDialog(HttpRequest request)
        {
            var language = request.UserLanguages[0];
            _country = BusinessRulesService.DetermineCountry(language);
        }

        public async Task StartAsync(IDialogContext context)
        {
            await SendCardAsync(context);
        }

        public async Task SendCardAsync(IDialogContext context)
        {
            var card = (++_prompts == 1)
                ? CardFactory.GetGreetingCard(context, _country)
                : CardFactory.GetAnythingElseCard();
            await context.PostAsync(card);
            context.Wait(AfterSendCardAsync);
        }

        public async Task AfterSendCardAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (!string.IsNullOrEmpty(activity.Text))
            {
                await HandleFreeformInput(context, activity.Text);
            }
            else if (!string.IsNullOrEmpty(activity.Value?.ToString()))
            {
                var value = activity.Value.ToString();
                if (ButtonData.TryParse(value, out var button))
                {
                    await HandleButtonInput(context, value, button);
                }
                else
                {
                    // this is an error
                }
            }
            else if (_prompts > 1)
            {
                await context.PostAsync("Say wa?!");
            }
        }

        public async Task HandleButtonInput(IDialogContext context, string json, ButtonData button)
        {
            if (button.Id == 1)
            {
                context.Call(new FindStoreDialog(), ResumeAfterChildDialog);
            }
            else if (button.Id == 2)
            {
                context.Call(new OrderStatusDialog(), ResumeAfterChildDialog);
            }
            else if (button.Id == 3)
            {
                await CardService.ShowStoreContactCardAsync(context, new StoreData(), ResumeAfterChildDialog);
            }
        }

        public async Task HandleFreeformInput(IDialogContext context, string text)
        {
            if (await DialogHelper.TryRecognizedFormat(context, text, ResumeAfterChildDialog))
            {
                await StartAsync(context);
            }
            else if (DialogHelper.TryParseExit(text))
            {
                ExitDialog(context);
            }
            else if (await DialogHelper.TryUsingLuis(context, text))
            {
                await StartAsync(context);
            }
            else
            {
                await context.PostAsync("Sorry, I don't understand.");
                await StartAsync(context);
            }
        }

        public async Task ResumeAfterChildDialog(IDialogContext context, IAwaitable<object> result)
        {
            await StartAsync(context);
        }

        public void ExitDialog(IDialogContext context, object value = null)
        {
            context.EndConversation(value?.ToString());
        }
    }
}