using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
            // https://docs.microsoft.com/en-us/azure/bot-service/bot-service-activities-entities?view=azure-bot-service-3.0&tabs=cs
            switch (activity.Type)
            {
                /// <summary>
                /// Your bot will send message activities to communicate information to and receive message activities from users. Some messages may simply consist of plain text, while others may contain richer content such as text to be spoken, suggested actions, media attachments, rich cards, and channel-specific data.
                /// </summary>
                case ActivityTypes.Message:
                    await SimulateTypingAsync(activity, 1000);
                    await Conversation.SendAsync(activity, () => new RootDialog(HttpContext.Current.Request));
                    break;
                /// <summary>
                /// If your bot receives a conversation update activity indicating that a user has joined the conversation, you may choose to have it respond by sending a welcome message to that user.
                /// </summary>
                /// <remarks>
                /// Not available in all channels
                /// </remarks>
                case ActivityTypes.ConversationUpdate when (SendWelcome(activity)):
                    await Conversation.SendAsync(activity, () => new RootDialog(HttpContext.Current.Request));
                    break;
                /// <summary>
                /// A bot receives a conversation update activity whenever it has been added to a conversation, other members have been added to or removed from a conversation, or conversation metadata has changed.
                /// </summary>
                /// <remarks>
                /// Not available in all channels
                /// </remarks>
                case ActivityTypes.ConversationUpdate:
                /// <summary>
                /// Contact relation update activities signal a change in the relationship between the recipient and a user within the channel. Contact relation update activities generally do not contain user-generated content. The relationship update described by a contact relation update activity exists between the user in the from field (often, but not always, the user initiating the update) and the user or bot in the recipient field.
                /// Contact relation update activities are identified by a type value of contactRelationUpdate.
                /// Activity.From + Activity.Action represent what happened.
                /// </summary>
                case ActivityTypes.ContactRelationUpdate:
                /// <summary>
                /// A bot receives a contact relation update activity whenever it is added to or removed from a user's contact list. The value of the activity's action property (add | remove) indicates whether the bot has been added or removed from the user's contact list.
                /// </summary>
                case ActivityTypes.Typing:
                /// <summary>
                /// Represents an attempt to determine whether a bot's endpoint is accessible.
                /// </summary>
                case ActivityTypes.Ping:
                /// <summary>
                /// End of conversation activities signal the end of a conversation from the recipient's perspective. This may be because the conversation has been completely ended, or because the recipient has been removed from the conversation in a way that is indistinguishable from it ending. The conversation being ended is described in the conversation field.
                /// End of conversation activities are identified by a type value of endOfConversation.
                /// </summary>
                case ActivityTypes.EndOfConversation:
                /// <summary>
                /// A bot receives an end of conversation activity to indicate that the user has ended the conversation. A bot may send an end of Conversation activity to indicate to the user that the conversation is ending.
                /// Event activities communicate programmatic information from a client or channel to a bot. The meaning of an event activity is defined by the name field, which is meaningful within the scope of a channel. Event activities are designed to carry both interactive information (such as button clicks) and non-interactive information (such as a notification of a client automatically updating an embedded speech model).
                /// Represents a communication sent to a bot that is not visible to the user.
                /// </summary>
                case ActivityTypes.Event:
                /// <summary>
                /// Your bot may receive an event activity from an external process or service that wants to communicate information to your bot without that information being visible to users. The sender of an event activity typically does not expect the bot to acknowledge receipt in any way.
                /// Invoke activities communicate programmatic information from a client or channel to a bot, and have a corresponding return payload for use within the channel. The meaning of an invoke activity is defined by the name field, which is meaningful within the scope of a channel.
                /// Represents a communication sent to a bot to request that it perform a specific operation. This activity type is reserved for internal use by the Microsoft Bot Framework.
                /// </summary>
                case ActivityTypes.Invoke:
                /// <summary>
                /// A bot receives a delete user data activity when a user requests deletion of any data that the bot has previously persisted for him or her. If your bot receives this type of activity, it should delete any personally identifiable information (PII) that it has previously stored for the user that made the request.
                /// </summary>
                case ActivityTypes.DeleteUserData:
                /// <summary>
                /// Message reaction activities represent a social interaction on an existing message activity within a conversation. The original activity is referred to by the id and conversation fields within the activity. The from field represents the source of the reaction (i.e., the user that reacted to the message).
                /// Message reaction activities are identified by a type value of messageReaction.
                /// </summary>
                case ActivityTypes.MessageUpdate:
                /// <summary>
                /// Message update activities represent an update of an existing message activity within a conversation. The updated activity is referred to by the id and conversation fields within the activity, and the message update activity contains all fields in the revised message activity.
                /// Message update activities are identified by a type value of messageUpdate.
                /// </summary>
                case ActivityTypes.MessageDelete:
                /// <summary>
                /// Installation update activities represent an installation or uninstallation of a bot within an organizational unit (such as a customer tenant or "team") of a channel. Installation update activities generally do not represent adding or removing a channel.
                /// Channels may send installation activities when a bot is added to or removed from a tenant, team, or other organization unit within the channel. Channels should not send installation activities when the bot is installed into or removed from a channel.
                /// Installation update activities are identified by a type value of installationUpdate.
                /// </summary>
                case ActivityTypes.InstallationUpdate:
                /// <summary>
                /// Some channels will send message reaction activities to your bot when a user reacted to an existing activity. For example, a user clicks the "Like" button on a message. The reply toId property will indicate which activity the user reacted to.
                /// The message reaction activity may correspond to any number of message reaction types that the channel defined.For example, "Like" or "PlusOne" as reaction types that a channel may send.
                /// </summary>
                case ActivityTypes.MessageReaction:
                /// <summary>
                /// Indicates that a user has reacted to an existing activity. For example, a user clicks the "Like" button on a message.
                /// </summary>
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

        private bool SendWelcome(Activity activity)
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