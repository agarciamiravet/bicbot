namespace BIC.Bot
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Builder.Dialogs;
    using System.Web.Http.Description;
    using System.Net.Http;
    using System;
    using BIC.Bot.Dialogs;
    using System.Net;

    using BIC.Bot.Services.Dtos;
    using BIC.Bot.Services;
    using BIC.Bot.Constants;
    using System.Web.Configuration;
    using BIC.Bot.Resources;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Autofac;
    using System.Threading;
    using BIC.Bot.BotStore;

    [BotAuthentication]
    public class MessagesController : ApiController
    {

        internal static IDialog<object> MakeRoot()
        {
            return Chain.From(() => new BotHomeDialog());
        }

        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            // check if activity is of type message
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                var message = activity as IMessageActivity;
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
                {
                    var botDataStore = scope.Resolve<IBotDataStore<BotData>>();

                    var key = new AddressKey()
                    {
                        BotId = message.Recipient.Id,
                        ChannelId = message.ChannelId,
                        UserId = message.From.Id,
                        ConversationId = message.Conversation.Id,
                        ServiceUrl = message.ServiceUrl
                    };
                    var userData = await botDataStore.LoadAsync(key, BotStoreType.BotUserData, CancellationToken.None);

                    userData.SetProperty("key 1", "value1");
                    userData.SetProperty("key 2", "value2");

                    await botDataStore.SaveAsync(key, BotStoreType.BotUserData, userData, CancellationToken.None);
                    await botDataStore.FlushAsync(key, CancellationToken.None);
                }

                if (activity.Text.ToLower()== BotMessageType.Notification)
                {
                    await HandleNotificationMessages(activity, connector);
                }
                else
                {
                    var authRestService = new AuthRestService();

                    if (activity.ChannelId == Channel.Sms)
                    {
                        int accessCode;
                        bool isNumeric = int.TryParse(activity.Text, out accessCode);

                        if (isNumeric)
                        {
                            var otpSmsService = new OTPSmsService();
                            var isAccessCodeValid = await otpSmsService.CheckOneTimePasswordCode(activity.From.Id, accessCode.ToString(),activity.ChannelId);

                            if (!isAccessCodeValid)
                            {
                                Activity reply = activity.CreateReply("el codigo es invalido ");
                                await connector.Conversations.ReplyToActivityAsync(reply);
                            }
                            else
                            {
                                Activity reply = activity.CreateReply("Autenticación realizada con éxito");
                                await connector.Conversations.ReplyToActivityAsync(reply);
                            }
                        }

                        else
                        {

                            var smsUserAuthenticated = await authRestService.CheckSmsUserIsAuthenticated(activity.From.Id);

                            if (!smsUserAuthenticated)
                            {
                                Activity reply = activity.CreateReply(MessagesDialog.SmsUserNotLogged);
                                await connector.Conversations.ReplyToActivityAsync(reply);
                            }
                            else
                            {
                                await HandleSmsMessages(activity, connector);
                            }
                        }
                    }
                    else
                    {
                        var userIsAuthenticated = false;

                        if(activity.ChannelId == "directline")
                        {
                            userIsAuthenticated = await authRestService.CheckDirectLineUserIsAuthenticated(activity.From.Id);
                        }
                        else
                        {
                            userIsAuthenticated = await authRestService.CheckUserIsAuthenticated(activity.From.Name);
                        }

                        if (activity.Text == BotMessageType.Notification)
                        {
                            await HandleNotificationMessages(activity, connector);
                        }

                        if(activity.Text == BotMessageType.Userlogged)
                        {
                            Activity reply = activity.CreateReply(Resources.MessagesDialog.AuthLoginSucceded);
                            await connector.Conversations.ReplyToActivityAsync(reply);
                        }

  
                        if (userIsAuthenticated)
                        {
                            await Conversation.SendAsync(activity, () => new BotHomeDialog());
                        }
                        else
                        {
                            if(activity.ChannelId == Channel.DirectLine)
                            {
                                //check number
                                int accessCode;
                                bool isNumeric = int.TryParse(activity.Text, out accessCode);

                                if (isNumeric)
                                {
                                    var otpSmsService = new OTPSmsService();
                                    var isAccessCodeValid = await otpSmsService.CheckOneTimePasswordCode(activity.From.Id, accessCode.ToString(), activity.ChannelId);

                                    if (!isAccessCodeValid)
                                    {
                                        Activity reply = activity.CreateReply("El código es inválido: " + activity.From.Id);
                                        await connector.Conversations.ReplyToActivityAsync(reply);
                                    }
                                    else
                                    {
                                        Activity reply = activity.CreateReply("Autenticación realizada con éxito");
                                        await connector.Conversations.ReplyToActivityAsync(reply);
                                    }
                                }
                                else
                                {
                                    Activity reply = activity.CreateReply(MessagesDialog.DirectLineMessageLogin);

                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                }
                            }
                            else
                            {
                                var urlLogin = WebConfigurationManager.AppSettings["authLoginUrl"];
                                var codeAccessUrl = "?codeaccess=" + DateTime.Now.Ticks.ToString();
                                var channelId = "&channelId=" + activity.ChannelId;
                                var definitiveUrlLogin = urlLogin + codeAccessUrl + channelId;
                                Activity reply = activity.CreateReply(MessagesDialog.AuthLoginRequired + activity.From.Name + System.Environment.NewLine + definitiveUrlLogin);

                                await connector.Conversations.ReplyToActivityAsync(reply);
                            }
                        }
                    }
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        private async Task HandleSmsMessages(Activity activity, ConnectorClient connector)
        {
            var message = string.Empty;

            //message = "el sms funciona";
            message = await this.executeSmsHomeAction(activity.Text);

            Activity reply = activity.CreateReply(message);
            await connector.Conversations.ReplyToActivityAsync(reply);
        }

        private async Task<string> executeSmsHomeAction(string actionToExecute)
        {
            HomeRestService homeRestService = new HomeRestService();

            var defaultResult = MessagesDialog.SmsActionNotUnderstanding;

            var result = await this.executeSmsHomeLightOnAction(actionToExecute, homeRestService);

            if(string.IsNullOrEmpty(result))
            {
                result = await this.executeSmsHomeLightOffAction(actionToExecute, homeRestService);
            }

            if(string.IsNullOrEmpty(result))
            {
                result = await this.executeSmsHomeBlindUpAction(actionToExecute, homeRestService);
            }

            if(string.IsNullOrEmpty(result))
            {
                result = await this.executeSmsHomeBlindDownAction(actionToExecute, homeRestService);
            }

            if (string.IsNullOrEmpty(result))
            {
                result =  defaultResult;
            }

            return result;
        }

        private async Task<string> executeSmsHomeLightOnAction(string actionToExecute, HomeRestService homeRestService)
        {
            string message = string.Empty;

            if (actionToExecute.ToLower() == SmsMessage.LightOnHome)
            {
                await homeRestService.ExecuteAction(HomeRestService.lightOnAllAction);
                message = MessagesDialog.LightOnHomeMessage;
            }

            if (actionToExecute.ToLower() == SmsMessage.LightOnBathRoom)
            {
                await homeRestService.ExecuteAction(HomeRestService.lightOnBathRoomAction);
                message = MessagesDialog.LightOnBathroomMessage;
            }

            if (actionToExecute.ToLower() == SmsMessage.LightOnEntranceHall)
            {
                await homeRestService.ExecuteAction(HomeRestService.lightOnEntranceHallAction);
                message = MessagesDialog.LightOnEntranceHallMessage;
            }

            if (actionToExecute.ToLower() == SmsMessage.LightOnbedroom ||
                actionToExecute.ToLower() == SmsMessage.LightOnbedroomAlternative ||
                actionToExecute.ToLower() == SmsMessage.LightOnbedroomOtherAlternative)
            {
                await homeRestService.ExecuteAction(HomeRestService.lightOnBedroomAction);
                message = MessagesDialog.LigthOnBedroomMessage;
            }

            if (actionToExecute.ToLower() == SmsMessage.LightOnKitchen)
            {
                await homeRestService.ExecuteAction(HomeRestService.lightOnKitchenAction);
                message = MessagesDialog.LightOnKitchenMessage;
            }

            return message;
           
        }

        private async Task<string> executeSmsHomeLightOffAction(string actionToExecute, HomeRestService homeRestService)
        {
            string message = string.Empty;

            if (actionToExecute.ToLower() == SmsMessage.LightOffHome)
            {
                await homeRestService.ExecuteAction(HomeRestService.lightOffAllAction);
                message = MessagesDialog.LightOffHomeMessage;
            }

            if (actionToExecute.ToLower() == SmsMessage.LightOffBathRoom)
            {
                await homeRestService.ExecuteAction(HomeRestService.lightOffBathRoomAction);
                message = MessagesDialog.LightOffBathroomMessage;

            }

            if (actionToExecute.ToLower() == SmsMessage.LightOffEntranceHall)
            {
                await homeRestService.ExecuteAction(HomeRestService.lightOffEntranceHallAction);
                message = MessagesDialog.LightOffEntranceHallMessage;
            }

            if (actionToExecute.ToLower() == SmsMessage.LightOffbedroom ||
                actionToExecute.ToLower() == SmsMessage.LightOffbedroomAlternative ||
                actionToExecute.ToLower() == SmsMessage.LightOffbedroomOtherAlternative)
            {
                await homeRestService.ExecuteAction(HomeRestService.lightOffBedroomAction);
                message = MessagesDialog.LightOnBathroomMessage;
            }

            if (actionToExecute.ToLower() == SmsMessage.LightOffKitchen)
            {
                await homeRestService.ExecuteAction(HomeRestService.lightOffKitchenAction);
                message = MessagesDialog.LightOffKitchenMessage;
            }

            return message;
        }

        private async Task<string> executeSmsHomeBlindUpAction(string actionToExecute, HomeRestService homeRestService)
        {
            string message = string.Empty;

            if (actionToExecute.ToLower() == SmsMessage.BlindUpHomme)
            {
                await homeRestService.ExecuteAction(HomeRestService.blindUpAllAction);
                message = MessagesDialog.BlindUpHomeAll;
            }

            if (actionToExecute.ToLower() == SmsMessage.BlindUpKitchen)
            {
                await homeRestService.ExecuteAction(HomeRestService.blindUpKitchenAction);
                message = MessagesDialog.BlindUpKitchen;
            }

            if (actionToExecute.ToLower() == SmsMessage.BlindUpMailBox)
            {
                await homeRestService.ExecuteAction(HomeRestService.blindUpMailboxAction);
                message = MessagesDialog.BlindUpMailBox;
            }

            if (actionToExecute.ToLower() == SmsMessage.BlindUpBedRoom || 
                actionToExecute.ToLower() == SmsMessage.BlindUpBedRoomAlternative)
            {
                await homeRestService.ExecuteAction(HomeRestService.blindUpBedroomAction);
                message = MessagesDialog.BlindUpBedroom;
            }

            return message;
        }

        private async Task<string> executeSmsHomeBlindDownAction(string actionToExecute, HomeRestService homeRestService)
        {
            string message = string.Empty;

            if (actionToExecute.ToLower() == SmsMessage.BlindDownHomme)
            {
                await homeRestService.ExecuteAction(HomeRestService.blindDownAllAction);
                message = MessagesDialog.BlindDownHomeAll;
            }

            if (actionToExecute.ToLower() == SmsMessage.BlindDownKitchen)
            {
                await homeRestService.ExecuteAction(HomeRestService.blindDownKitchenAction);
                message = MessagesDialog.BlindDownKitchen;
            }

            if (actionToExecute.ToLower() == SmsMessage.BlindDownMailBox)
            {
                await homeRestService.ExecuteAction(HomeRestService.blindDownMailboxAction);
                message = MessagesDialog.BlindDownMailbox;
            }

            if (actionToExecute.ToLower() == SmsMessage.BlindDownBedRoom ||
                actionToExecute.ToLower() == SmsMessage.BlindDownBedRoomAlternative)
            {
                await homeRestService.ExecuteAction(HomeRestService.blindDownBedroomAction);
                message = MessagesDialog.BlindDownBedroom;
            }

            return message;
        }


        private async Task HandleNotificationMessages(Activity activity, ConnectorClient connector)

        {
            var toId = activity.From.Id;
            var toName = activity.From.Name;
            var fromId = activity.Recipient.Id;
            var fromName = activity.Recipient.Name;
            var serviceUrl = activity.ServiceUrl;
            var channelId = activity.ChannelId;
            var conversationId = activity.Conversation.Id;

            var messageConversation = string.Concat("Activity: ", toId, " toName: " + toName + " fromID: " + fromId + " fromName:", fromName + " serviceurl: " + serviceUrl + " channelid:" + channelId + " conversation: " + conversationId);
            Activity reply = activity.CreateReply(messageConversation);


            var botUserRegisterDto = new BoutUserRegisterDto
            {
                Activity = toId,
                ChannelId = channelId,
                Conversation = conversationId,
                FromID = fromId,
                FromName = fromName,
                UserName = toName,
                RegistryDate = DateTime.Now,
                ServiceUrl = serviceUrl
            };

            var homeRestService = new HomeRestService();

            await homeRestService.RegisterBotUser(botUserRegisterDto);

            await connector.Conversations.ReplyToActivityAsync(reply);
        }
    }
}