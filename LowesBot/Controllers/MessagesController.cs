using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LowesBot.Dialogs;
using LowesBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LowesBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            switch (activity.Type)
            {
                case ActivityTypes.Message:
                    await SimulateTypingAsync(activity, 1000);
                    await Conversation.SendAsync(activity, () => new RootDialog());
                    break;
                case ActivityTypes.ConversationUpdate when (FirstTime(activity)):
                    await Conversation.SendAsync(activity, () => new RootDialog());
                    break;
                case ActivityTypes.ConversationUpdate:
                case ActivityTypes.ContactRelationUpdate:
                case ActivityTypes.Typing:
                case ActivityTypes.Ping:
                case ActivityTypes.EndOfConversation:
                case ActivityTypes.Event:
                case ActivityTypes.Invoke:
                case ActivityTypes.DeleteUserData:
                case ActivityTypes.MessageUpdate:
                case ActivityTypes.MessageDelete:
                case ActivityTypes.InstallationUpdate:
                case ActivityTypes.MessageReaction:
                case ActivityTypes.Suggestion:
                case ActivityTypes.Trace:
                default:
                    // do nothing
                    break;
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private async Task SimulateTypingAsync(Activity activity, int delay)
        {
            var reply = activity.CreateReply();
            reply.Type = ActivityTypes.Typing;
            reply.Text = null;
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            await connector.Conversations.ReplyToActivityAsync(reply);
            await Task.Delay(delay);

        }

        private bool FirstTime(Activity activity)
        {
            if (activity.MembersAdded == null || !activity.MembersAdded.Any())
            {
                return false;
            }
            else
            {
                var first = activity.MembersAdded.First();
                return Equals(activity.Recipient.Id, first.Id);
            }
        }
    }
}