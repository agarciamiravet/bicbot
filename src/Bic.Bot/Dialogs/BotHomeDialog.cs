namespace BIC.Bot.Dialogs
{
    using BIC.Bot.Services;
    using BIC.Bot.Services.Dtos;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Microsoft.Bot.Connector;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;


    [LuisModel("[KEY]", "[KEY]", domain: "westeurope.api.cognitive.microsoft.com", SpellCheck = true , BingSpellCheckSubscriptionKey = "[KEY]")]
    [Serializable]
    public class BotHomeDialog : LuisDialog<object>, IDialog<object>
    {
        private string messageRequestAuthoration = "Hemos enviado una petición a Padre Bic para que autorice esta acción";
        private string[] houseSpaces = { "Cocina", "Baño", "Recibidor", "Habitacion", "Todas" };


        #region Ayuda

        [LuisIntent("Ayuda")]
        public async Task Ayuda(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score < 0.70)
            {
                await context.PostAsync("No te entendí un carajo");
                context.Wait(MessageReceived);

            }
            else
            {
                var activity = context.MakeMessage();
                activity.Text = Resources.MessagesDialog.HelpMessage;

                await context.PostAsync(activity);
                context.Wait(MessageReceived);
            }
        }


        #endregion

        #region Temperatura


        [LuisIntent("Temperatura")]
        public async Task Temperatura(IDialogContext context, LuisResult result)
        {
            HomeRestService homeRestService = new HomeRestService();
            await homeRestService.ExecuteAction(HomeRestService.temperatureRequestAction);

            var typingMessage = context.MakeMessage();
            typingMessage.Type = ActivityTypes.Typing;
            await context.PostAsync(typingMessage);
        }

        #endregion

        #region Luces

        [LuisIntent("ApagarElementoCasa")]
        public async Task ApagarElementoCasa(IDialogContext context, LuisResult result)
        {
            context.PrivateConversationData.SetValue("laststep", "apagarelementocasa");

            var entities = result.Entities.ToList();
            string message = string.Empty;

            if (entities.Where(m => m.Type == "espacio").FirstOrDefault() != null)
            {
                HomeRestService homeRestService = new HomeRestService();

                var userCanPerformAction = homeRestService.CanUserPerformAction(context.Activity.From.Name);

                var entityEspacio = entities.Where(m => m.Type == "espacio").Select(n => n.Entity).FirstOrDefault();

                if (entityEspacio == "cocina")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.lightOffKitchenAction);
                        message = "Luz de la cocina apagada";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "apagarluzcocina",
                            EMail = "rebeca@bicbot.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                if (entityEspacio == "baño")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.lightOffBathRoomAction);
                        message = "Luz del baño apagagada";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "apagarluzbaño",
                            EMail = "rebeca@bicbot.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                if (entityEspacio == "recibidor")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.lightOffEntranceHallAction);
                        message = "Luz del recibidor apagada";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "apagarluzrecibidor",
                            EMail = "rebeca@bicbot.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                if (entityEspacio == "habitacion" || entityEspacio == "habitación")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.lightOffBedroomAction);
                        message = "Luz de la habitación apagada";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "apagarluzhabitacion",
                            EMail = "rebeca@bicbot.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                var activityMessage = context.MakeMessage();
                activityMessage.Type = ActivityTypes.Message;
                await context.PostAsync(message);
            }

            if (string.IsNullOrEmpty(message))
            {
                var dialog = new PromptDialog.PromptChoice<string>(houseSpaces, "Que parte de la casa quieres apagar", "Esta no es una opción válida", 2);
                context.Call(dialog, AfterUserHasChoiceLedLowOption);
            }
        }

        [LuisIntent("EncenderElementoCasa")]
        public async Task EncenderElementoCasa(IDialogContext context, LuisResult result)
        {
            context.PrivateConversationData.SetValue("laststep", "encenderelementocasa");

            var entities = result.Entities.ToList();
            string message = string.Empty;

            if (entities.Where(m => m.Type == "espacio").FirstOrDefault() != null)
            {
                HomeRestService homeRestService = new HomeRestService();

                var userCanPerformAction = homeRestService.CanUserPerformAction(context.Activity.From.Name);

                var entityEspacio = entities.Where(m => m.Type == "espacio").Select(n => n.Entity).FirstOrDefault();

                if (entityEspacio == "cocina")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.lightOnKitchenAction);
                        message = "Luz de la cocina encendida";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "encenderluzcocina",
                            EMail = "rebeca@bicbo.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                if (entityEspacio == "baño")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.lightOnBathRoomAction);
                        message = "Luz del baño encendida";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "encenderluzbaño",
                            EMail = "rebeca@bicbot.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                if (entityEspacio == "recibidor")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.lightOnEntranceHallAction);
                        message = "Luz del recibidor encendida";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "encenderluzrecibidor",
                            EMail = "rebeca@bicbot.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                if (entityEspacio == "habitacion" || entityEspacio == "habitación")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.lightOnBedroomAction);
                        message = "Luz de la habitación encendida";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "encenderluzhabitacion",
                            EMail = "rebeca@bicbot.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                var activityMessage = context.MakeMessage();
                activityMessage.Type = ActivityTypes.Message;
                await context.PostAsync(message);
            }


            if (string.IsNullOrEmpty(message))
            {
                var dialog = new PromptDialog.PromptChoice<string>(houseSpaces, "¿De que parte de la casa quieres encender?", "Esta no es una opción válida", 2);
                context.Call(dialog, AfterUserHasChoiceLedHighOption);
            }
        }

        private async Task AfterUserHasChoiceLedHighOption(IDialogContext context, IAwaitable<string> result)
        {
            var message = string.Empty;
            string userChoice = await result;
            string messageBody = string.Empty;

            HomeRestService homeRestService = new HomeRestService();

            if (homeRestService.CanUserPerformAction(context.Activity.From.Name))
            {
                if (userChoice.ToLower() == "cocina")
                {
                    await homeRestService.ExecuteAction(HomeRestService.lightOnKitchenAction);
                    message = "Luz de la cocina encendida";
                }

                if (userChoice.ToLower() == "baño")
                {
                    await homeRestService.ExecuteAction(HomeRestService.lightOnBathRoomAction);
                    message = "Luz del baño encendida";
                }

                if (userChoice.ToLower() == "recibidor")
                {
                    await homeRestService.ExecuteAction(HomeRestService.lightOnEntranceHallAction);
                    message = "Luz del recibidor encendida";
                }

                if (userChoice.ToLower() == "habitacion")
                {
                    await homeRestService.ExecuteAction(HomeRestService.lightOnBedroomAction);
                    message = "Luz de la habitación encendida";
                }

                if (userChoice.ToLower() == "todas")
                {
                    await homeRestService.ExecuteAction(HomeRestService.lightOnAllAction);
                    message = "Todas las luces de la casa encendida";
                }
            }
            else
            {
                var authorizeAction = string.Empty;

                if (userChoice.ToLower() == "cocina")
                {
                    authorizeAction = "encenderluzcocina";
                }

                if (userChoice.ToLower() == "baño")
                {
                    authorizeAction = "encenderluzbaño";

                }

                if (userChoice.ToLower() == "recibidor")
                {
                    authorizeAction = "encenderluzrecibidor";

                }

                if (userChoice.ToLower() == "habitacion")
                {
                    authorizeAction = "encenderluzrecibidor";
                }

                await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                {
                    UserRequest = context.Activity.From.Name,
                    ActionToTake = authorizeAction,
                    EMail = "rebeca@bicbot.com"
                });
                message = "Hemos enviado una petición a Padre Bic para que autorice esta acción";
                var typingMessage = context.MakeMessage();
                typingMessage.Type = ActivityTypes.Typing;
                await context.PostAsync(typingMessage);

            }


            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        private async Task AfterUserHasChoiceLedLowOption(IDialogContext context, IAwaitable<string> result)
        {
            var message = string.Empty;
            string userChoice = await result;
            string messageBody = string.Empty;

            HomeRestService homeRestService = new HomeRestService();

            if (homeRestService.CanUserPerformAction(context.Activity.From.Name))
            {

                if (userChoice.ToLower() == "cocina")
                {
                    await homeRestService.ExecuteAction(HomeRestService.lightOffKitchenAction);
                    message = "Luz de la cocina apagada";
                }

                if (userChoice.ToLower() == "baño")
                {
                    await homeRestService.ExecuteAction(HomeRestService.lightOffBathRoomAction);
                    message = "Luz del baño apagada";
                }

                if (userChoice.ToLower() == "recibidor")
                {
                    await homeRestService.ExecuteAction(HomeRestService.lightOffEntranceHallAction);
                    message = "Luz del recibidor apagada";
                }

                if (userChoice.ToLower() == "habitacion")
                {
                    await homeRestService.ExecuteAction(HomeRestService.lightOffBedroomAction);
                    message = "Luz de la habitación apagada";
                }

                if (userChoice.ToLower() == "todas")
                {
                    await homeRestService.ExecuteAction(HomeRestService.lightOffAllAction);
                    message = "Todas las luces de la casa apagadas.";
                }
            }
            else
            {
                var authorizeAction = string.Empty;

                if (userChoice.ToLower() == "cocina")
                {
                    authorizeAction = "apagarluzcocina";
                }

                if (userChoice.ToLower() == "baño")
                {
                    authorizeAction = "apagarluzbaño";

                }

                if (userChoice.ToLower() == "recibidor")
                {
                    authorizeAction = "apagarluzrecibidor";

                }

                if (userChoice.ToLower() == "habitacion")
                {
                    authorizeAction = "apagarluzrecibidor";
                }

                await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                {
                    UserRequest = context.Activity.From.Name,
                    ActionToTake = authorizeAction,
                    EMail = "rebeca@bicbot.com"
                });

                message = "Hemos enviado una petición a Padre Bic para que autorice esta acción";
                var typingMessage = context.MakeMessage();
                typingMessage.Type = ActivityTypes.Typing;
                await context.PostAsync(typingMessage);
            }

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        #endregion

        #region Persianas

        [LuisIntent("BajarPersiana")]
        public async Task BajarPersiana(IDialogContext context, LuisResult result)
        {
            context.PrivateConversationData.SetValue("laststep", "bajarventana");


            var entities = result.Entities.ToList();
            string message = string.Empty;

            if (entities.Where(m => m.Type == "espacio").FirstOrDefault() != null)
            {
                HomeRestService homeRestService = new HomeRestService();

                var userCanPerformAction = homeRestService.CanUserPerformAction(context.Activity.From.Name);

                var entityEspacio = entities.Where(m => m.Type == "espacio").Select(n => n.Entity).FirstOrDefault();

                if (entityEspacio == "cocina")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.blindDownKitchenAction);
                        message = "Persiana de la cocina bajada";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "bajarpersianacocina",
                            EMail = "rebeca@bicbo.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                if (entityEspacio == "habitacion" || entityEspacio == "habitación")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.blindDownBedroomAction);
                        message = "Persiana de la habitación bajada";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "bajarpersianahabitacion",
                            EMail = "rebeca@bicbo.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }


                if (entityEspacio == "buzon" || entityEspacio == "buzón")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.blindDownMailboxAction);
                        message = "Persiana del buzón bajada";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "bajarpersianabuzon",
                            EMail = "rebeca@bicbo.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                var activityMessage = context.MakeMessage();
                activityMessage.Type = ActivityTypes.Message;
                await context.PostAsync(message);
            }
            else
            {
                var activity = context.MakeMessage();
                activity.Text = "¿De que parte de la casa quieres bajar la persiana?";

                var dialog = new PromptDialog.PromptChoice<string>(new string[] { "Cocina", "Habitacion", "Buzón", "Todas" }, "¿De que parte de la casa quieres bajar la persiana?", "Esta no es una opción válida", 2);
                context.Call(dialog, AfterUserHasChosenBajarPersianaOptionAsync);
            }
        }

        private async Task AfterUserHasChosenBajarPersianaOptionAsync(IDialogContext context, IAwaitable<string> result)
        {
            string message = string.Empty;

            HomeRestService homeRestService = new HomeRestService();

            string userChoice = await result;

            if (userChoice.ToLower() == "cocina")
            {
                await homeRestService.ExecuteAction(HomeRestService.blindDownKitchenAction);
                message = "Persiana de la cocina bajada";
            }

            if (userChoice.ToLower() == "habitacion")
            {
                await homeRestService.ExecuteAction(HomeRestService.blindDownBedroomAction);
                message = "Persiana de la habitacion bajada";
            }


            if (userChoice.ToLower() == "buzón")
            {
                await homeRestService.ExecuteAction(HomeRestService.blindDownMailboxAction);
                message = "Persiana del buzón bajada";
            }


            if (userChoice.ToLower() == "todas")
            {
                await homeRestService.ExecuteAction(HomeRestService.blindDownAllAction);
                message = "Todas las persianas bajadas";
            }

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }


        [LuisIntent("SubirPersiana")]
        public async Task SubirPersiana(IDialogContext context, LuisResult result)
        {
            context.PrivateConversationData.SetValue("laststep", "subirventana");

            string message = string.Empty;
            var entities = result.Entities.ToList();

            if (entities.Where(m => m.Type == "espacio").FirstOrDefault() != null)
            {
                HomeRestService homeRestService = new HomeRestService();

                var userCanPerformAction = homeRestService.CanUserPerformAction(context.Activity.From.Name);

                var entityEspacio = entities.Where(m => m.Type == "espacio").Select(n => n.Entity).FirstOrDefault();

                if (entityEspacio == "cocina")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.blindUpKitchenAction);
                        message = "Persiana de la cocina subida";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "subirpersianacocina",
                            EMail = "rebeca@bicbo.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                if (entityEspacio == "habitacion" || entityEspacio == "habitación")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.blindUpBedroomAction);
                        message = "Persiana de la habitación subida";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "subirpersianahabitacion",
                            EMail = "rebeca@bicbo.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }


                if (entityEspacio == "buzon" || entityEspacio == "buzón")
                {
                    if (userCanPerformAction)
                    {
                        await homeRestService.ExecuteAction(HomeRestService.blindUpMailboxAction);
                        message = "Persiana del buzón subida";
                    }
                    else
                    {
                        await homeRestService.MakeAuthyRequest(new AuthyRequestDto
                        {
                            UserRequest = context.Activity.From.Name,
                            ActionToTake = "subirpersianabuzon",
                            EMail = "rebeca@bicbo.com"
                        });
                        message = messageRequestAuthoration;
                    }
                }

                var activityMessage = context.MakeMessage();
                activityMessage.Type = ActivityTypes.Message;
                await context.PostAsync(message);
            }
            else
            {

                var activity = context.MakeMessage();
                activity.Text = "¿De que parte de la casa quieres subir la persiana?";

                var dialog = new PromptDialog.PromptChoice<string>(new string[] { "Cocina", "Habitacion", "Buzón", "Todas" }, "¿De que parte de la casa quieres subir la persiana?", "Esta no es una opción válida", 2);
                context.Call(dialog, AfterUserHasChosenSubirPersianaOptionAsync);
            }
        }

        private async Task AfterUserHasChosenSubirPersianaOptionAsync(IDialogContext context, IAwaitable<string> result)
        {
            string message = string.Empty;
            var messageBody = string.Empty;

            string userChoice = await result;

            HomeRestService homeRestService = new HomeRestService();

            if (userChoice.ToLower() == "cocina")
            {
                await homeRestService.ExecuteAction(HomeRestService.blindUpKitchenAction);
                message = "Persiana de la cocina subida";
            }

            if (userChoice.ToLower() == "habitacion")
            {
                await homeRestService.ExecuteAction(HomeRestService.blindUpBedroomAction);
                message = "Persiana de la habitacion subida";
            }


            if (userChoice.ToLower() == "buzón")
            {
                await homeRestService.ExecuteAction(HomeRestService.blindUpMailboxAction);
                message = "Persiana del buzón subida";
            }


            if (userChoice.ToLower() == "todas")
            {
                await homeRestService.ExecuteAction(HomeRestService.blindUpAllAction);
                message = "Todas las ventanas subidas";
            }

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }


        #endregion

        #region Saludos


        
       [LuisIntent("SaludoBicBot")]
        public async Task SaludoBicBot(IDialogContext context, LuisResult result)
        {
                var activity = context.MakeMessage();
                activity.Text = "Muchas gracias por asistir a la charla";
                activity.Attachments.Add(new Attachment(contentType: "video/mp4", name: "Video", thumbnailUrl: "https://i.ytimg.com/vi/zu9W8AtZy7U/hqdefault.jpg", contentUrl: "https://www.youtube.com/watch?v=zu9W8AtZy7U"));

                await context.PostAsync(activity);
        }



        [LuisIntent("Greetings")]
        public async Task Greetings(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score < 0.70)
            {
                await context.PostAsync(Resources.MessagesDialog.UserMessageNotUnderstanding);
                context.Wait(MessageReceived);

            }
            else
            {
                var greetingMessage = string.Empty;

                if (DateTime.Now.Hour > 7)
                {
                    greetingMessage = Resources.MessagesDialog.GreetingsMorning;
                }

                if (DateTime.Now.Hour > 12)
                {
                    greetingMessage = Resources.MessagesDialog.GreetingsLater;
                }

                if (DateTime.Now.Hour > 20 && DateTime.Now.Hour < 7)
                {
                    greetingMessage = Resources.MessagesDialog.GreetingNight;
                }


                var activity = context.MakeMessage();
                activity.Text = greetingMessage + " " +  activity.Recipient.Name;

                await context.PostAsync(activity);
                context.Wait(MessageReceived);
            }
        }


        #endregion

        #region NingunaIntencion
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = Resources.MessagesDialog.UserMessageNotUnderstanding;

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
        #endregion

        #region IBI
        [LuisIntent("Pagar")]
        public async Task Pay(IDialogContext context, LuisResult result)
        {
            var o = result.Entities;
            var activity = context.MakeMessage();

            if (result.Entities.Count > 0)
            {
                var entityIbi = result.Entities.Where(entity => entity.Entity == "ibi").FirstOrDefault();

                if (entityIbi != null)
                {
                    HttpClient client = new HttpClient();

                    client.BaseAddress = new Uri("https://bicbotlogin.azurewebsites.net/");

                    HttpResponseMessage response = await client.GetAsync("api/Bill/ibi");

                    if (response.IsSuccessStatusCode)
                    {
                        activity.Text = await response.Content.ReadAsStringAsync();
                    }
                }
            }

            await context.PostAsync(activity);
            context.Wait(MessageReceived);
        }
        #endregion

        #region Cartero


        [LuisIntent("CarteroPaquete")]
        public async Task CarteroBuzon(IDialogContext context, LuisResult result)
        {
            var authorizeAction = "entregarpaquetecartero";

            HomeRestService homeRestService = new HomeRestService();
            await homeRestService.MakeAuthyRequest(new AuthyRequestDto
            {
                UserRequest = context.Activity.From.Name,
                ActionToTake = authorizeAction,
                EMail = "deliverycourier@bicbot.com"
            });

            await context.PostAsync(messageRequestAuthoration);
            context.Wait(MessageReceived);
        }
        #endregion

        #region DestruirBicBot

        [LuisIntent("DestruirBicBot")]
        public async Task DestruirBicBot(IDialogContext context, LuisResult result)
        {
            var message = Resources.MessagesDialog.MessageHal9000;
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
        #endregion

        #region ApagarDicho

        [LuisIntent("ApagarDicho")]
        public async Task ApagarDicho(IDialogContext context, LuisResult result)
        {
            HomeRestService homeRestService = new HomeRestService();
            await homeRestService.ExecuteAction(HomeRestService.lightOnAllAction);

            var message = "Todas las luces de la casa encendida";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
        #endregion
    }
}