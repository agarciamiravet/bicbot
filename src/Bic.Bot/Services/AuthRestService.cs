namespace BIC.Bot.Services
{
    using BIC.Bot.Constants;
    using BIC.Bot.Services.Dtos;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Configuration;

    public class AuthRestService
    {
        private  string checkAuthUserEndpoint = "api/botauth/checkauthuser";

        private string checkSmsUserEndpoint = "api/SmsLogin/userlogged";

        private string checkDirectLineUserEndpoint = "api/DirectLineLogin/userlogged";

        public async Task<bool> CheckUserIsAuthenticated(string userName)
        {
            var urlService = WebConfigurationManager.AppSettings["authRestServiceUrl"];

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(urlService)
            };

            var botAuth = new UserBotAuthDto();
            botAuth.UserName = userName;

            StringContent content = new StringContent(JsonConvert.SerializeObject(botAuth), Encoding.UTF8, ContentType.Json);

            var checkUserAccessTokenRequest = await client.PostAsync(checkAuthUserEndpoint, content);

            var checkUserAccessTokenResponse = await checkUserAccessTokenRequest.Content.ReadAsStringAsync();

            if(checkUserAccessTokenResponse == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckSmsUserIsAuthenticated(string userName)
        {
            var urlService = WebConfigurationManager.AppSettings["authRestServiceUrl"];

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(urlService)
            };

            var smsUser = new SmsUserDto();
            smsUser.UserName = userName;

            StringContent content = new StringContent(JsonConvert.SerializeObject(smsUser), Encoding.UTF8, ContentType.Json);

            var checkUserAccessTokenRequest = await client.PostAsync(checkSmsUserEndpoint, content);

            var checkUserAccessTokenResponse = await checkUserAccessTokenRequest.Content.ReadAsStringAsync();

            if (checkUserAccessTokenResponse == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckDirectLineUserIsAuthenticated(string userName)
        {
            var urlService = WebConfigurationManager.AppSettings["authRestServiceUrl"];

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(urlService)
            };

            var directLineUserDto = new DirectLineUserDto();
            directLineUserDto.UserName = userName;

            StringContent content = new StringContent(JsonConvert.SerializeObject(directLineUserDto), Encoding.UTF8, ContentType.Json);

            var checkUserAccessTokenRequest = await client.PostAsync(checkDirectLineUserEndpoint, content);

            var checkUserAccessTokenResponse = await checkUserAccessTokenRequest.Content.ReadAsStringAsync();

            if (checkUserAccessTokenResponse == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}