namespace BIC.Bot.RestService.Controllers
{
    using System.Threading.Tasks;
    using BIC.Bot.RestService.ServiceBus;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/Light")]
    public class LightController : Controller
    {
        [Route("LightOnKitchen")]
        public async Task LightOnKitchen()
        {
            var actionToPerform = (int)Enums.HomeActions.LightOnKitchen;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageToHome(actionToPerform);       
        }

        [Route("LightOffKitchen")]
        public async Task LightOffKitchen()
        {
            var actionToPerform = (int)Enums.HomeActions.LightOffKitchen;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageToHome(actionToPerform);
        }

        [Route("LightOnBedroom")]
        public async Task LightOnBedroom()
        {
            var actionToPerform = (int)Enums.HomeActions.LightOnBedroom;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageToHome(actionToPerform);
        }

        [Route("LightOffBedroom")]
        public async Task LightOffBedroom()
        {
            var actionToPerform = (int)Enums.HomeActions.LightOffBedroom;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageToHome(actionToPerform);
        }

        [HttpPost]
        [Route("LightOnBathRoom")]
        public async Task LightOnBathRoom()
        {
            var actionToPerform = (int)Enums.HomeActions.LightOnBathRoom;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageToHome(actionToPerform);
        }

        [HttpPost]
        [Route("LightOffBathRoom")]
        public async Task LightOffBathRoom()
        {
            var actionToPerform = (int)Enums.HomeActions.LightOffBathRoom;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageToHome(actionToPerform);
        }

        [Route("LightOnEntranceHall")]
        public async Task LightOnEntranceHall()
        {
            var actionToPerform = (int)Enums.HomeActions.LightOnEntranceHall;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageToHome(actionToPerform);
        }

        [Route("LightOffEntranceHall")]
        public async Task LightOffEntranceHall()
        {
            var actionToPerform = (int)Enums.HomeActions.LightOffEntranceHall;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageToHome(actionToPerform);
        }

        [Route("LightOnAll")]
        public async Task LightOnAll()
        {
            var actionToPerform = (int)Enums.HomeActions.LightOnAll;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageToHome(actionToPerform);
        }

        [Route("LightOffAll")]
        public async Task LightOffAll()
        {
            var actionToPerform = (int)Enums.HomeActions.LightOffAll;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageToHome(actionToPerform);
        }
    }
}
