using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BIC.Bot.RestService.Data;
using BIC.Bot.RestService.Dtos;
using BIC.Bot.RestService.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OATH.Net;

namespace BIC.Bot.RestService.Controllers
{
    [Produces("application/json")]
    [Route("api/OneTimePassword")]
    public class OneTimePasswordController : Controller
    {

        private readonly ApplicationDbContext _context;

        public OneTimePasswordController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("CheckOneTimePassword")]
        public async Task<bool> CheckOneTimePassword([FromBody] OtpCodeDto otpCodeDto)
        {
            var secretkey = string.Empty;
            var email = string.Empty;
            var userId = string.Empty;
            

            if (otpCodeDto.ChannelId == "sms")
            {
                var smsUser = this._context.SmsUser.Where(smsuser => smsuser.UserName == otpCodeDto.UserName).FirstOrDefault();

                if (smsUser == null)
                {
                    return false;
                }

                secretkey = smsUser.SecretKey;
                email = smsUser.EMail;
                userId = smsUser.UserId;
            }

            if (otpCodeDto.ChannelId == "directline")
            {
                var directLineUser = this._context.DirectLineUser.Where(smsuser => smsuser.UserName == otpCodeDto.UserName).FirstOrDefault();

                if (directLineUser == null)
                {
                    return false;
                }

                secretkey = directLineUser.SecretKey;
                email = directLineUser.EMail;
                userId = directLineUser.UserId;
            }

            int otpDigits = 6;

            var secretKey = secretkey;

            Key key = new Key(secretKey); 
            var secret = key.Base32; 

            TimeBasedOtpGenerator otp = new TimeBasedOtpGenerator(key, otpDigits);
            var time = GetNistTime();
            var tst = otp.GenerateOtp(time);
            Key keySecret = new Key(secretKey);

            time = GetNistTime();

            TimeBasedOtpGenerator otp3 = new TimeBasedOtpGenerator(keySecret, otpDigits);

            var valid = otp.ValidateOtp(otpCodeDto.OneTimePasswordCode, time);

            if(valid)
            {
                var jwtoken = new JwtManager();
                var expirationTime = DateTime.UtcNow.AddMinutes(59);
                var jwt =  jwtoken.GenerateJwtToken(email, userId, 60);

                if(otpCodeDto.ChannelId == "sms")
                {
                    _context.SmsLogin.Add(new Data.Entities.SmsLogin { UserName = otpCodeDto.UserName, ExpirationTime = expirationTime, Jwt = jwt });
                    _context.SaveChanges();
                }

                if (otpCodeDto.ChannelId == "directline")
                {
                    _context.DirectLineLogins.Add(new Data.Entities.DirectLineLogins { UserName = otpCodeDto.UserName, ExpirationTime = expirationTime, Jwt = jwt.ToString() });
                    _context.SaveChanges();
                }


            }

            return valid;
        }



        public static DateTime GetNistTime()
        {
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
            var response = myHttpWebRequest.GetResponse();
            string todaysDates = response.Headers["date"];
            return DateTime.ParseExact(todaysDates,
                                       "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                       CultureInfo.InvariantCulture.DateTimeFormat,
                                       DateTimeStyles.AssumeUniversal);
        }
    }
}