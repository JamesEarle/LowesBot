using System;
using System.Threading.Tasks;
using LowesBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace LowesBot.Dialogs
{
    internal interface ICardDialog : IDialog<object>
    {
        Task SendCardAsync(IDialogContext context);
        Task AfterSendCardAsync(IDialogContext context, IAwaitable<object> result);
        Task HandleButtonInput(IDialogContext context, string value, ButtonData button);
        Task HandleFreeformInput(IDialogContext context, string text);
        Task ResumeAfterChildDialog(IDialogContext context, IAwaitable<object> result);
        void ExitDialog(IDialogContext context, object value = null);
    }
}