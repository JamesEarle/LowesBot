using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LowesBot.Dialogs
{
    public enum State { Welcome_Closed, Welcome_AlmostClosed, Welcome_Open, AnythingElse }

    [Serializable]
    public class RootDialog : IDialog<object>
    {

        public async Task StartAsync(IDialogContext context)
        {
            await SendGreetingCardAsync(context, IsOpen());
        }

        State IsOpen() { /* todo */ return State.Welcome_Open; }

        private async Task SendGreetingCardAsync(IDialogContext context, State state)
        {
            // handle state here!
            var card = CardService.GetGreetingCard(state);
            await context.PostAsync(card);
            context.Wait(ReceiveGreetingCardAsync);
        }

        private async Task ReceiveGreetingCardAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var json = activity.Value.ToString();
            var button = ButtonData.Parse(json); // See what button they clicked in response to previous card
            await OnOptionSelected(context, button);
        }

        private async Task OnOptionSelected(IDialogContext context, ButtonData button)
        {
            try
            {
                switch (button.Id)
                {
                    case 1:
                        context.Call(new FindStoreDialog(), ResumeAfterOptionDialog);
                        break;
                    case 2:
                        context.Call(new OrderStatusDialog(), ResumeAfterOptionDialog);
                        break;
                    case 3:
                        context.Call(new ReturnsAndExchangesDialog(), ResumeAfterOptionDialog);
                        break;
                    case 6:
                        await context.PostAsync("Thanks for using Lowe's Bot!");
                        context.Done("Process Terminated");
                        break;
                    default:
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                Console.WriteLine(ex.Message);
                await context.PostAsync("Oops! Too many attempts. You can try starting again.");
                context.Wait(ReceiveGreetingCardAsync);
            }
        }

        private Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            return SendGreetingCardAsync(context, State.AnythingElse);
        }
    }
}