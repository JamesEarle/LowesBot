using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace LowesBot.Services
{
    public class ActivityHelper
    {
        private Activity _activity;

        public ActivityHelper(Activity activity)
        {
            _activity = activity;
        }

        public async Task ProcessAsync()
        {
            switch (_activity.Type)
            {
                case ActivityTypes.Message: await OnMessage?.Invoke(_activity); break;
                case ActivityTypes.ContactRelationUpdate: OnContactRelationUpdate?.Invoke(_activity); break;
                case ActivityTypes.ConversationUpdate: OnConversationUpdate?.Invoke(_activity); break;
                case ActivityTypes.Typing: OnTyping?.Invoke(_activity); break;
                case ActivityTypes.Ping: OnPing?.Invoke(_activity); break;
                case ActivityTypes.EndOfConversation: OnEndOfConversation?.Invoke(_activity); break;
                case ActivityTypes.Event: OnEvent?.Invoke(_activity); break;
                case ActivityTypes.Invoke: OnInvoke?.Invoke(_activity); break;
                case ActivityTypes.DeleteUserData: OnDeleteUserData?.Invoke(_activity); break;
                case ActivityTypes.MessageUpdate: OnMessageUpdate?.Invoke(_activity); break;
                case ActivityTypes.MessageDelete: OnMessageDelete?.Invoke(_activity); break;
                case ActivityTypes.InstallationUpdate: OnInstallationUpdate?.Invoke(_activity); break;
                case ActivityTypes.MessageReaction: OnMessageReaction?.Invoke(_activity); break;
                case ActivityTypes.Suggestion: OnSuggestion?.Invoke(_activity); break;
                case ActivityTypes.Trace: OnTrace?.Invoke(_activity); break;
                default: throw new NotSupportedException(_activity.Type);
            }
        }

        /// <summary>
        /// Represents a communication between bot and user.
        /// </summary>
        public Func<IMessageActivity, Task> OnMessage { get; set; } = null;

        /// <summary>
        /// Contact relation update activities signal a change in the relationship between the recipient and a user within the channel. Contact relation update activities generally do not contain user-generated content. The relationship update described by a contact relation update activity exists between the user in the from field (often, but not always, the user initiating the update) and the user or bot in the recipient field.
        /// Contact relation update activities are identified by a type value of contactRelationUpdate.
        /// Activity.From + Activity.Action represent what happened.
        /// </summary>
        public Func<IContactRelationUpdateActivity, Task> OnContactRelationUpdate { get; set; } = null;

        /// <summary>
        /// Conversation update activities describe a change in a conversation's members, description, existence, or otherwise. Conversation update activities generally do not contain user-generated content. The conversation being updated is described in the conversation field.
        /// Conversation update activities are identified by a type value of conversationUpdate.
        /// Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info.
        /// </summary>
        /// <remarks>
        /// Not available in all channels
        /// </remarks>
        public Func<IConversationUpdateActivity, Task> OnConversationUpdate { get; set; } = null;

        /// <summary>
        /// Indicates that the user or bot on the other end of the conversation is compiling a response.
        /// </summary>
        public Func<ITypingActivity, Task> OnTyping { get; set; } = null;

        /// <summary>
        /// Represents an attempt to determine whether a bot's endpoint is accessible.
        /// </summary>
        public Func<Activity, Task> OnPing { get; set; } = null;

        /// <summary>
        /// End of conversation activities signal the end of a conversation from the recipient's perspective. This may be because the conversation has been completely ended, or because the recipient has been removed from the conversation in a way that is indistinguishable from it ending. The conversation being ended is described in the conversation field.
        /// End of conversation activities are identified by a type value of endOfConversation.
        /// </summary>
        public Func<IEndOfConversationActivity, Task> OnEndOfConversation { get; set; } = null;

        /// <summary>
        /// Event activities communicate programmatic information from a client or channel to a bot. The meaning of an event activity is defined by the name field, which is meaningful within the scope of a channel. Event activities are designed to carry both interactive information (such as button clicks) and non-interactive information (such as a notification of a client automatically updating an embedded speech model).
        /// Represents a communication sent to a bot that is not visible to the user.
        /// </summary>
        public Func<IEventActivity, Task> OnEvent { get; set; } = null;

        /// <summary>
        /// Invoke activities communicate programmatic information from a client or channel to a bot, and have a corresponding return payload for use within the channel. The meaning of an invoke activity is defined by the name field, which is meaningful within the scope of a channel.
        /// Represents a communication sent to a bot to request that it perform a specific operation. This activity type is reserved for internal use by the Microsoft Bot Framework.
        /// </summary>
        public Func<IInvokeActivity, Task> OnInvoke { get; set; } = null;

        /// <summary>
        /// Indicates to a bot that a user has requested that the bot delete any user data it may have stored.
        /// </summary>
        public Func<Activity, Task> OnDeleteUserData { get; set; } = null;

        /// <summary>
        /// Message reaction activities represent a social interaction on an existing message activity within a conversation. The original activity is referred to by the id and conversation fields within the activity. The from field represents the source of the reaction (i.e., the user that reacted to the message).
        /// Message reaction activities are identified by a type value of messageReaction.
        /// </summary>
        public Func<IMessageReactionActivity, Task> OnMessageReaction { get; set; } = null;

        /// <summary>
        /// Message update activities represent an update of an existing message activity within a conversation. The updated activity is referred to by the id and conversation fields within the activity, and the message update activity contains all fields in the revised message activity.
        /// Message update activities are identified by a type value of messageUpdate.
        /// </summary>
        public Func<Activity, Task> OnMessageUpdate { get; set; } = null;

        /// <summary>
        /// Message delete activities represent a deletion of an existing message activity within a conversation. The deleted activity is referred to by the id and conversation fields within the activity.
        /// Message delete activities are identified by a type value of messageDelete.
        /// </summary>
        public Func<Activity, Task> OnMessageDelete { get; set; } = null;

        /// <summary>
        /// Installation update activities represent an installation or uninstallation of a bot within an organizational unit (such as a customer tenant or "team") of a channel. Installation update activities generally do not represent adding or removing a channel.
        /// Installation update activities are identified by a type value of installationUpdate.
        /// </summary>
        public Func<Activity, Task> OnInstallationUpdate { get; set; } = null;
        public Func<Activity, Task> OnSuggestion { get; set; } = null;
        public Func<Activity, Task> OnTrace { get; set; } = null;

    }
}