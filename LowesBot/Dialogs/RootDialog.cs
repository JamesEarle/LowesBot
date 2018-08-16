using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace LowesBot.Dialogs
{
    public partial class ButtonData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        public static ButtonData Parse(string json)
        {
            return JsonConvert.DeserializeObject<ButtonData>(json);
        }
    }

    public partial class FormData
    {
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }

        public static FormData Parse(string json)
        {
            return JsonConvert.DeserializeObject<FormData>(json);
        }
    }

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        // Dialog Options
        private const string FindAStoreOption = "Find a Store";
        private const string OrderStatusOption = "Check Order Status";
        private const string ReturnsAndExchangesOption = "Returns & Exchanges";
        private const string DoneOption = "I'm done";

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var card = CardService.GetGreetingCard();
            await context.PostAsync(card);
            context.Wait(MessageReceivedAsync1);
            //context.Wait(MessageReceivedAsync1
            //PromptDialog.Choice(
            //    context,
            //    this.OnOptionSelected,
            //    new List<string>() {
            //        FindAStoreOption,
            //        OrderStatusOption,
            //        ReturnsAndExchangesOption
            //    },
            //    String.Format("Hi! I'm the Lowe's Bot. How can I help you today?"));
        }

        private async Task MessageReceivedAsync1(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var json = activity.Value.ToString();
            var data = ButtonData.Parse(json);
            await OnOptionSelected(context, data.Id);
        }

        private async Task OnOptionSelected(IDialogContext context, long id)
        {
            try
            {
                
                switch (id)
                {
                    case 1:
                        context.Call(new OrderStatusDialog(), this.ResumeAfterOptionDialog);
                        break;
                    case 2:
                        context.Call(new FindStoreDialog(), this.ResumeAfterOptionDialog);
                        break;
                    case 3:
                        context.Call(new ReturnsAndExchangesDialog(), this.ResumeAfterOptionDialog);
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
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message {ex.Message}");
            }
            finally
            {
                //PromptDialog.Choice(
                //context,
                //this.OnOptionSelected,
                //new List<string>() {
                //    FindAStoreOption,
                //    OrderStatusOption,
                //    ReturnsAndExchangesOption,
                //    DoneOption
                //},
                //String.Format("How else can I help you today?"));
            }
        }
    }
}