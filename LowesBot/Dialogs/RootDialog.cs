using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task StartAsync(IDialogContext context)
        {
            await SendCardAsync(context);
        }

        public async Task SendCardAsync(IDialogContext context)
        {
            var card = (++_prompts == 1)
                ? CardFactory.GetGreetingCard(context)
                : CardFactory.GetAnythingElseCard();
            await context.PostAsync(card);
            context.Wait(ResumeAfterCardAsync);
        }

        public async Task ResumeAfterCardAsync(IDialogContext context, IAwaitable<object> result)
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

        public Task HandleButtonInput(IDialogContext context, string json, ButtonData button)
        {
            if (button.Id == 1)
            {
                context.Call(new FindStoreDialog(), ResumeAfterChildDialog);
            }
            else if (button.Id == 2)
            {
                context.Call(new OrderNumberDialog(), ResumeAfterChildDialog);
            }
            else if (button.Id == 3)
            {
                context.Call(new ReturnsAndExchangesDialog(), ResumeAfterChildDialog);
            }
            return Task.CompletedTask;
        }

        public async Task HandleFreeformInput(IDialogContext context, string text)
        {
            if (await DialogHelper.TryRecognizedFormat(context, text, ResumeAfterChildDialog))
            {
                await SendCardAsync(context);
            }
            else if (DialogHelper.TryUsingText(context, text))
            {
                // nothing
            }
            else if (await DialogHelper.TryUsingLuis(context, text))
            {
                await SendCardAsync(context);
            }
            else
            {
                await context.PostAsync("Sorry, I don't understand.");
                await SendCardAsync(context);
            }
        }

        public async Task ResumeAfterChildDialog(IDialogContext context, IAwaitable<object> result)
        {
            await SendCardAsync(context);
        }

        public void ExitDialog(IDialogContext context, object value = null)
        {
            context.EndConversation(value?.ToString());
        }
    }
}