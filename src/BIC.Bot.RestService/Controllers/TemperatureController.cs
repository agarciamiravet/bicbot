namespace BIC.Bot.RestService.Controllers
{
    using System.Threading.Tasks;
    using BIC.Bot.RestService.ServiceBus;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/Temperature")]
    public class TemperatureController : Controller
    {
        [Route("Get")]
        public async Task GetTemperature()
        {
            var actionToPerform = (int)Enums.HomeActions.GetTemperature;

            var bicServiceBus = new BicServiceBus();

            await bicServiceBus.SendMessageToHome(actionToPerform);
        }
    }
}