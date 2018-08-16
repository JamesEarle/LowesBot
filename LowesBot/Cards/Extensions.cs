using System.Threading.Tasks;
using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LowesBot
{
    public static partial class Extensions
    {
        /// <summary>
        /// This special extension method does all the conversion for you!
        /// </summary>
        /// <param name="context"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        public static Task PostAsync(this IDialogContext context, AdaptiveCard card)
        {
            var message = context.MakeMessage();
            var attachment = new Attachment()
            {
                Content = card,
                ContentType = AdaptiveCard.ContentType
            };
            message.Attachments.Add(attachment);
            return context.PostAsync(message);
        }

        public static Attachment ToAttachment(this AdaptiveCard adaptiveCard)
        {
            return new Attachment()
            {
                Content = adaptiveCard,
                ContentType = AdaptiveCard.ContentType
            };
        }
    }
}