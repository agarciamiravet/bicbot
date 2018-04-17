namespace BIC.Bot.Services
{
    using BIC.Bot.Services.Dtos;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Configuration;

    public class HomeRestService
    {
        public const string lightOffBathRoomAction = "api/Light/LightOffBathRoom";
        
        public const string lightOffKitchenAction = "api/Light/LightOffKitchen";

        public const string lightOffEntranceHallAction = "api/Light/lightOffEntranceHall";

        public const string lightOffBedroomAction = "api/Light/LightOffBedroom";

        public const string lightOffAllAction = "api/Light/LightOffAll";

        public const string lightOnAllAction = "api/Light/LightOnAll";

        public const string lightOnKitchenAction = "api/Light/LightOnKitchen";

        public const string lightOnBathRoomAction = "api/Light/LightOnBathRoom";

        public const string lightOnEntranceHallAction = "api/Light/LightOnEntranceHall";

        public const string lightOnBedroomAction = "api/Light/LightOnBedroom";

        public const string blindUpKitchenAction = "api/Blind/BlindUpKitchen";

        public const string blindDownKitchenAction = "api/Blind/BlindDownKitchen";

        public const string blindUpBedroomAction = "api/Blind/BlindUpBedroom";

        public const string blindDownBedroomAction = "api/Blind/BlindDownBedroom";

        public const string blindUpAllAction = "api/Blind/BlindUpAll";

        public const string blindDownAllAction = "api/Blind/BlindDownAll";

        public const string blindUpMailboxAction = "api/Blind/BlindUpMailbox";

        public const string blindDownMailboxAction = "api/Blind/BlindDownMailbox";

        public const string makeAuthyRequestAction = "api/authy/AuthyRequest";

        public const string temperatureRequestAction = "api/Temperature/Get";

        public const string mailUserRequestAction = "api/Blind/MailUser";

        public async Task ExecuteAction(string action)
        {
            using (var httpClient = new HttpClient())
            {
                var urlService = WebConfigurationManager.AppSettings["homeRestServiceUrl"];

                httpClient.BaseAddress = new Uri(urlService);

                await httpClient.PostAsync(action, null);
            }     
        }

        public async Task RegisterBotUser(BoutUserRegisterDto boutUserRegisterDto)
        {
           
            using (var httpClient = new HttpClient())
            {
                var user = new BoutUserRegisterDto();
                var urlService = WebConfigurationManager.AppSettings["homeRestServiceUrl"];
                httpClient.BaseAddress = new Uri(urlService);
                StringContent content = new StringContent(JsonConvert.SerializeObject(boutUserRegisterDto), Encoding.UTF8, "application/json");

                await httpClient.PostAsync("api/registrationbotuser/registerbotuser", content);
            }
        }

        public async Task MakeAuthyRequest(AuthyRequestDto authyRequest)
        {
            using (var httpClient = new HttpClient())
            {
                var user = new BoutUserRegisterDto();
                var urlService = WebConfigurationManager.AppSettings["homeRestServiceUrl"];
                httpClient.BaseAddress = new Uri(urlService);
                StringContent content = new StringContent(JsonConvert.SerializeObject(authyRequest), Encoding.UTF8, "application/json");
                await httpClient.PostAsync(makeAuthyRequestAction, content);
            }
        }

        public bool CanUserPerformAction(string userName)
        {
            if(userName == "Rebeca Bic" || userName == "rebeca")
            {
                return false;
            }

            return true;
        }
    }
}