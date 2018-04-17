using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    class Program
    {

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri ("http://localhost:44321/");

            // HttpResponseMessage response = await client.PostAsync("api/Light/LightOnAll", null);

            //if (response.IsSuccessStatusCode)
            //{
            //    var product = await response.Content.ReadAsStringAsync();
            //}
            //await client.PostAsync("api/Light/LightOffBedroom", null);

            //await client.PostAsync("api/Light/LightOnBathRoom", null);

            //await client.PostAsync("api/Light/LightOffEntranceHall", null);

        //public int Id { get; set; }

        //public string Activity { get; set; }

        //public string UserName { get; set; }

        //public string FromID { get; set; }

        //public string FromName { get; set; }

        //public string ServiceUrl { get; set; }

        //public string ChannelId { get; set; }

        //public string Conversation { get; set; }

        //public DateTime RegistryDate { get; set; }

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("userRequest", "Rebeca"));
            nvc.Add(new KeyValuePair<string, string>("actionToTake", "Encencer luz del pasillo"));
            nvc.Add(new KeyValuePair<string, string>("email", "rebeca@bicbot.com"));


            var user = new BoutUserRegisterDto();

            user.Activity = "20000";

            var authyRequest = new AuthyRequestDto { ActionToTake = "Encencer luz del pasillo", EMail = "rebeca@bicbot.com", UserRequest = "Rebeca" };



            var otpCodeDto = new OtpCodeDto();

            otpCodeDto.OneTimePasswordCode = "157639";
            otpCodeDto.UserName = "+34635912877";

            StringContent content = new StringContent(JsonConvert.SerializeObject(otpCodeDto), Encoding.UTF8, "application/json");


            //StringContent content = new StringContent(JsonConvert.SerializeObject(authyRequest), Encoding.UTF8, "application/json");

            // StringContent content = new StringContent(JsonConvert.SerializeObject(botAuth), Encoding.UTF8, "application/json");

            // client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJmYXRoZXJAYmljYm90LmNvbSIsImp0aSI6ImY1NDM3OWJiLWViMjMtNGIwNS05YjU5LTIwOGEzMGVmYjkxOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiY2QxZTkyMTgtZmUyNS00MjRiLThlYWYtZDc4YWM4ODY0NDQ3IiwiZXhwIjoxNTE5MTcyNjUyLCJpc3MiOiJodHRwOi8veW91cmRvbWFpbi5jb20iLCJhdWQiOiJodHRwOi8veW91cmRvbWFpbi5jb20ifQ.9ACzuimdqbKlEbpux7E9rUDRI5DyLH-JM-UKtO2EDY0");

            //var h = await client.PostAsync("api/OneTimePassword/CheckOneTimePassword", content);

            //var j = await  h.Content.ReadAsStringAsync();



            var smslogin = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44321/");
            var smsLogin = new SmsUserDto();
            smsLogin.UserName = "+34635912877";
            StringContent contentlogin = new StringContent(JsonConvert.SerializeObject(otpCodeDto), Encoding.UTF8, "application/json");
            var postsmslogin = await client.PostAsync("api/SmsLogin/userlogged", contentlogin);


            var jt = "lll";
        }
    }
}
