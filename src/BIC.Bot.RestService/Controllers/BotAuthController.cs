using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BIC.Bot.RestService.Data;
using BIC.Bot.RestService.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BIC.Bot.RestService.Controllers
{
    [Produces("application/json")]
    [Route("api/botauth")]
    public class BotAuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BotAuthController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("checkauthuser")]
        public bool CheckAuthuser([FromBody] UserBotAuthDto userBothAuthDto)
        {
            var result = false;

            var accessCodeIsValidOrExists = _context.BotLogin.Where(botLogin => botLogin.SkypeUserName == userBothAuthDto.UserName && botLogin.ExpirationTime > DateTime.UtcNow).FirstOrDefault();
           
            if(accessCodeIsValidOrExists == null)
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
        }

    }
}