using BIC.Bot.Constants;
using BIC.Bot.Services.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace BIC.Bot.Services
{
    public class OTPSmsService
    {
        private string checkAuthUserEndpoint = "api/OneTimePassword/CheckOneTimePassword";


        public async Task<bool> CheckOneTimePasswordCode(string userName, string code, string channelId)
        {
            var urlService = WebConfigurationManager.AppSettings["authRestServiceUrl"];

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(urlService)
            };

            var otpCodeDto = new OtpCodeDto();
            otpCodeDto.UserName = userName;
            otpCodeDto.OneTimePasswordCode = code;
            otpCodeDto.ChannelId = channelId;

            StringContent content = new StringContent(JsonConvert.SerializeObject(otpCodeDto), Encoding.UTF8, ContentType.Json);

            var checkUserAccessTokenRequest = await client.PostAsync(checkAuthUserEndpoint, content);

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