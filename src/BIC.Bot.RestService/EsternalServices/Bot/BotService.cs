using BIC.Bot.RestService.Dtos;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace BIC.Bot.RestService.EsternalServices.Bot
{
    public class BotService
    {
        public async Task SendBotMessage(string messageText, BotUserDataDto botUserDataDto)
        {

            MicrosoftAppCredentials.TrustServiceUrl("https://smba.trafficmanager.net/apis");

            var conversationId = botUserDataDto.UserConversationId;
            var userAccount = new Microsoft.Bot.Connector.ChannelAccount(botUserDataDto.UserConversationId, botUserDataDto.UserName);
            var botAccount = new Microsoft.Bot.Connector.ChannelAccount(botUserDataDto.BotConversationId, botUserDataDto.BotName);
            var connector = new ConnectorClient(new Uri("https://smba.trafficmanager.net/apis"), "[KEY]", "[KEY]");

            // Create a new message.
            Microsoft.Bot.Connector.IMessageActivity message = Activity.CreateMessageActivity();
            if (!string.IsNullOrEmpty(conversationId) && !string.IsNullOrEmpty(botUserDataDto.ChannelId))
            {
                // If conversation ID and channel ID was stored previously, use it.
                message.ChannelId = "skype";
            }
            else
            {
                // Conversation ID was not stored previously, so create a conversation. 
                // Note: If the user has an existing conversation in a channel, this will likely create a new conversation window.
                conversationId = (await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount)).Id;
            }

            // Set the address-related properties in the message and send the message.
            message.From = botAccount;
            message.Recipient = userAccount;
            message.Conversation = new ConversationAccount(id: conversationId);
            message.Text = messageText;
            message.Locale = "es-us";
            await connector.Conversations.SendToConversationAsync((Activity)message);
        }
    }
}
