namespace BIC.Bot.RestService.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BIC.Bot.RestService.Data;
    using BIC.Bot.RestService.Dtos;
    using Microsoft.AspNetCore.Mvc;


    [Produces("application/json")]
    [Route("api/SmsLogin")]
    public class SmsLoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SmsLoginController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("UserLogged")]
        public async Task<bool> CheckUserIsLogged([FromBody] SmsUserDto smsUserDto)
        {
            var smsLogin = _context.SmsLogin.Where(smslogin => smslogin.UserName == smsUserDto.UserName && smslogin.ExpirationTime > DateTime.UtcNow).FirstOrDefault();

            if(smsLogin == null)
            {
                return false;
            }
            return true;
        }
    }
}