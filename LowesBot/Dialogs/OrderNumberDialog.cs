using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LowesBot.Models;
using LowesBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace LowesBot.Dialogs
{

    [Serializable]
    internal class OrderNumberDialog : ICardDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await SendCardAsync(context);
        }

        public async Task SendCardAsync(IDialogContext context)
        {
            var card = CardFactory.GetOrderNumberCard();
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
                    // this is not possible
                }
            }
            else
            {
                PleaseReEnterNumber(context);
            }
        }

        public async Task HandleButtonInput(IDialogContext context, string value, ButtonData button)
        {
            if (button.Id == 1 && OrderNumberFormData.TryParse(value, out var form))
            {
                await HandleFreeformInput(context, form.OrderNumber);
            }
            else
            {
                ExitDialog(context);
            }
        }

        public async Task HandleFreeformInput(IDialogContext context, string text)
        {
            if (DialogHelper.TryUsingText(context, text))
            {
                // nothing
            }
            else if (LowesHelper.IsValidOrderNumber(text))
            {
                await CardService.ShowOrderStatusAsync(context, new OrderData { Number = text }, ResumeAfterChildDialog);
                PromptDialog.Confirm(context, ResumeAfterConfirm, "Look up another order?");
            }
            else
            {
                PleaseReEnterNumber(context);
            }
        }

        private void PleaseReEnterNumber(IDialogContext context)
        {
            PromptDialog.Number(context, ResumeAfterPleaseReEnterNumber, "That doesn't look correct. Please reenter your order number.");
        }

        private async Task ResumeAfterPleaseReEnterNumber(IDialogContext context, IAwaitable<long> result)
        {
            try
            {
                var value = await result;
                await HandleFreeformInput(context, value.ToString());
            }
            catch (TooManyAttemptsException ex)
            {
                ExitDialog(context, ex);
            }
            catch (Exception ex)
            {
                ExitDialog(context, ex);
            }
        }

        public Task ResumeAfterChildDialog(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Confirm(context, ResumeAfterConfirm, "Look up another order?");
            return Task.CompletedTask;
        }

        private async Task ResumeAfterConfirm(IDialogContext context, IAwaitable<bool> answer)
        {
            if (await answer)
            {
                await SendCardAsync(context);
            }
            else
            {
                ExitDialog(context);
            }
        }

        public void ExitDialog(IDialogContext context, object value = null)
        {
            context.Done(value);
        }
    }
}