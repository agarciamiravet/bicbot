namespace BIC.Bot.RestService.Controllers
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIC.Bot.RestService.Data;
    using BIC.Bot.RestService.Data.Entities;
    using BIC.Bot.RestService.EsternalServices.Authy;
    using BIC.Bot.RestService.ServiceBus;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [Route("api/authy")]
    public class AuthyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthyController(ApplicationDbContext context)
        {
            this._context = context;
        }

           
        [HttpPost]
        public bool CheckAuthyIdentifier(string authyID)
        {
            var isValidAuthyIdentifier = _context.AuthyAuthorization
                 .Where(authy => authy.AuthyIdentifier == authyID && 
                        authy.ExpirationValidityDate < DateTime.UtcNow)
                .FirstOrDefault();

            return (isValidAuthyIdentifier != null) ? true : false;
        }

        [Route("AuthyRequest")]
        [HttpPost]
        public void MakeAuthyRequest([FromBody]AuthyRequestDto authyRequestDto)
        {
            var authyRequest = new OneTouchClient("[KEY]", "[KEY]");

            var formatmessage = authyRequestDto.UserRequest + " ha solicitado permiso para: " + authyRequestDto.ActionToTake;

           var approvalRequest =  authyRequest.SendApprovalRequest(formatmessage, authyRequestDto.EMail);

            var authyAuthorization = new AuthyAuthorization();

            authyAuthorization.AuthyIdentifier = approvalRequest.ApprovalRequest["uuid"];
            authyAuthorization.AcionToExecute = authyRequestDto.ActionToTake;
            authyAuthorization.ExpirationValidityDate = DateTime.Now.AddMinutes(30);
            authyAuthorization.UserName = authyRequestDto.UserRequest;

            this._context.AuthyAuthorization.Add(authyAuthorization);
            this._context.SaveChanges();


        }

        [HttpPost]
        public string PerformActionWithAuthyId(string authyID)
        {
            var isValidAuthyIdentifier = _context.AuthyAuthorization
                 .Where(authy => authy.AuthyIdentifier == authyID &&
                        authy.ExpirationValidityDate < DateTime.UtcNow)
                .FirstOrDefault();

            if (isValidAuthyIdentifier != null)
            {
                return "El authyID es incorrecto";
            }
            else
            {
                return "Operación realizada con exito";
            }
        }

        [Route("AuthyResult")]
        [HttpPost]
        public async Task AuthyOneActionResult([FromBody] AuthyOneClientCallbackResult req)
        {
 
            var mensajeBot = string.Empty;

            if (req.status == "approved")
            {
                mensajeBot = "La solicitud ha sido aprobada";

               var authyRegister =  _context.AuthyAuthorization.Where(authy => authy.AuthyIdentifier == req.uuid).FirstOrDefault();

                if(authyRegister != null)
                {
                    int actionToPerform = 0;
                    var bicServiceBus = new BicServiceBus();

                    // Encender luz
                    if (authyRegister.AcionToExecute == "encenderluzbaño")
                    {
                        actionToPerform = (int)Enums.HomeActions.LightOnBathRoom;
                        await this.sendBotMessage("Encendida luz del baño ok ");
                    }  
                    if (authyRegister.AcionToExecute == "encenderluzcocina")
                    {
                        actionToPerform = (int)Enums.HomeActions.LightOnKitchen;
                        await this.sendBotMessage("Encendida luz de la cocina");
                    }

                    if (authyRegister.AcionToExecute == "encenderluzrecibidor")
                    {
                        actionToPerform = (int)Enums.HomeActions.LightOnEntranceHall;
                        await this.sendBotMessage("Encendida luz del recibidor");
                    }

                    if (authyRegister.AcionToExecute == "encenderluzhabitacion")
                    {
                        actionToPerform = (int)Enums.HomeActions.LightOnBedroom;
                        await this.sendBotMessage("Encendida luz de la habitación");
                    }

                    //apagar luz
                    if (authyRegister.AcionToExecute == "apagarluzbaño")
                    {
                        actionToPerform = (int)Enums.HomeActions.LightOffBathRoom;
                        await this.sendBotMessage("Apagada luz del baño");
                    }

                    if (authyRegister.AcionToExecute == "apagarluzcocina")
                    {
                        actionToPerform = (int)Enums.HomeActions.LightOffKitchen;
                        await this.sendBotMessage("Apagada luz de la cocina");
                    }

                    if (authyRegister.AcionToExecute == "apagarluzrecibidor")
                    {
                        actionToPerform = (int)Enums.HomeActions.LightOffEntranceHall;
                        await this.sendBotMessage("Apagada luz del recibidor");
                    }

                    if (authyRegister.AcionToExecute == "apagarluzhabitacion")
                    {
                        actionToPerform = (int)Enums.HomeActions.LightOffBedroom;
                        await this.sendBotMessage("Apagada luz de la habitación");
                    }

                    if(authyRegister.AcionToExecute == "entregarpaquetecartero")
                    {
                        actionToPerform = (int)Enums.HomeActions.MailUser;
                        await this.sendBotMessage("Buzón abierto para depositar el paquete");
                    }

                    if(authyRegister.AcionToExecute == "entregarpaquetecartero")
                    {
                        await bicServiceBus.SendMessageBlindToHome(actionToPerform);
                    }
                    else
                    {
                        await bicServiceBus.SendMessageToHome(actionToPerform);

                    }
                }

            }
            else
            {
                mensajeBot = "La solicitud ha sido rechazada";
            }
        }

        private async Task sendBotMessage(string messageText)
        {
            MicrosoftAppCredentials.TrustServiceUrl("https://smba.trafficmanager.net/apis");

            var conversationId = "[KEY]";
            var userAccount = new Microsoft.Bot.Connector.ChannelAccount("[KEY]", "[KEY]");
            var botAccount = new Microsoft.Bot.Connector.ChannelAccount("[KEY]", "[KEY]");
            var connector = new ConnectorClient(new Uri("https://smba.trafficmanager.net/apis"), "[KEY]", "[KEY]");

            // Create a new message.
            Microsoft.Bot.Connector.IMessageActivity message = Microsoft.Bot.Connector.Activity.CreateMessageActivity();
            if (!string.IsNullOrEmpty(conversationId) && !string.IsNullOrEmpty("skype"))
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
            message.Conversation = new Microsoft.Bot.Connector.ConversationAccount(id: conversationId);
            message.Text = messageText;
            message.Locale = "es-us";
            await connector.Conversations.SendToConversationAsync((Activity)message);
        }

    }
}