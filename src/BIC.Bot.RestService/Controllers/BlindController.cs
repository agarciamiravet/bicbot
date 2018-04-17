namespace BIC.Bot.RestService.Controllers
{
    using System.Threading.Tasks;
    using BIC.Bot.RestService.ServiceBus;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/Blind")]
    public class BlindController : Controller
    {
        [HttpPost]
        [Route("BlindUpKitchen")]
        public async Task BlindUpKitchen()
        {
            var actionToPerform = (int)Enums.HomeActions.BlindUpKitchen;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageBlindToHome(actionToPerform);
        }


        [HttpPost]
        [Route("BlindDownKitchen")]
        public async Task BlindOffKitchen()
        {
            var actionToPerform = (int)Enums.HomeActions.BlindDownKitchen;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageBlindToHome(actionToPerform);
        }

        [HttpPost]
        [Route("BlindUpBedroom")]
        public async Task BlindUpBedroom()
        {
            var actionToPerform = (int)Enums.HomeActions.BlindUpBedroom;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageBlindToHome(actionToPerform);
        }


        [HttpPost]
        [Route("BlindDownBedroom")]
        public async Task BlindDownBedroom()
        {
            var actionToPerform = (int)Enums.HomeActions.BlindDownBedroom;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageBlindToHome(actionToPerform);
        }


  
        [HttpPost]
        [Route("BlindUpAll")]
        public async Task BlindUpAll()
        {
            var actionToPerform = (int)Enums.HomeActions.BlindUpAll;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageBlindToHome(actionToPerform);
        }


        [HttpPost]
        [Route("BlindDownAll")]
        public async Task BlindOffAll()
        {
            var actionToPerform = (int)Enums.HomeActions.BlindDownAll;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageBlindToHome(actionToPerform);
        }


        [HttpPost]
        [Route("BlindUpMailbox")]
        public async Task blindUpMailbox()
        {
            var actionToPerform = (int)Enums.HomeActions.BlindUpMailBox;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageBlindToHome(actionToPerform);
        }


        [HttpPost]
        [Route("BlindDownMailbox")]
        public async Task blindUpBedroom()
        {
            var actionToPerform = (int)Enums.HomeActions.BlindDownMailbox;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageBlindToHome(actionToPerform);
        }

        [HttpPost]
        [Route("MailUser")]
        public async Task MailUser()
        {
            var actionToPerform = (int)Enums.HomeActions.MailUser;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageBlindToHome(actionToPerform);
        }
    }
}

