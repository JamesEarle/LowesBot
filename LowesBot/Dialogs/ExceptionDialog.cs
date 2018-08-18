using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LowesBot.Dialogs
{
    public class ExceptionDialog : IDialog<object>
    {
        private readonly ExceptionVerbocity _verbocity;
        public enum ExceptionVerbocity { Full, Minimal, None }
        public ExceptionDialog(ExceptionVerbocity verbocity) => _verbocity = verbocity;

        public async Task StartAsync(IDialogContext context)
        {
            //try { context.Call(new RootDialog(null), ResumeAsync); }
            //catch (Exception ex) { await ShowExceptionAsync(context, ex); }
        }

        private async Task ResumeAsync(IDialogContext context, IAwaitable<object> result)
        {
            //try { context.Done(await result); }
            //catch (Exception ex) { await ShowExceptionAsync(context, ex); }
        }

        private async Task ShowExceptionAsync(IDialogContext context, Exception ex)
        {
            switch (_verbocity)
            {
                case ExceptionVerbocity.Full:
                    var newline = "  \n";
                    var stack = System.Text.RegularExpressions.Regex.Split(ex.StackTrace, Environment.NewLine);
                    var stack_disp = string.Join(newline, stack.Take(50));
                    var frame = context.Frames.Select(x => x.Target?.ToString());
                    var frame_disp = string.Join(newline, frame.Take(50));
                    await context.PostAsync($"EXCEPTION{newline}{ex.Message}{newline}FRAMES{newline}{frame_disp}{newline}STACKTRACE{newline}{stack_disp}");
                    break;
                case ExceptionVerbocity.Minimal:
                    await context.PostAsync(ex.Message);
                    break;
                case ExceptionVerbocity.None:
                    return;
            }
        }
    }
}