using System;
using System.Threading.Tasks;
using LowesBot.Models;
using LowesBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LowesBot.Dialogs
{
    [Serializable]
    internal class OrderNumberDialog : IDialog<OrderData>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await AskNumber(context, "What is your order number?");
        }

        private async Task AskNumber(IDialogContext context, string prompt)
        {
            await context.PostAsync(prompt);
            context.Wait(AfterAskNumber);
        }

        private async Task AfterAskNumber(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                var answer = activity.Text;
                if (BusinessRulesService.TryParseOrderNumber(answer, out var number))
                {
                    ExitDialog(context, new OrderData { Number = number });
                }
                else if (DialogHelper.TryParseExit(answer))
                {
                    ExitDialog(context, answer);
                }
                else
                {
                    await AskNumber(context, "The number you entered is not valid.");
                }
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

        public void ExitDialog(IDialogContext context, object value = null)
        {
            context.Done(value);
        }
    }
}