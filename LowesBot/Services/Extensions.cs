using System.Collections.Generic;
using System.Linq;
using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LowesBot
{
    public static partial class Extensions
    {
        public static IMessageActivity ToCarousel(this IEnumerable<AdaptiveCard> cards, IDialogContext context)
        {
            var attachments = cards.Select(x => new Attachment()
            {
                Content = x,
                ContentType = AdaptiveCard.ContentType
            });
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = attachments.ToList();
            return reply;
        }

    }
}
