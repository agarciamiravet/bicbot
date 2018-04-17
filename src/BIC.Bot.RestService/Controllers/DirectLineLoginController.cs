namespace BIC.Bot.RestService.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BIC.Bot.RestService.Data;
    using BIC.Bot.RestService.Dtos;
    using Microsoft.AspNetCore.Mvc;


    [Produces("application/json")]
    [Route("api/DirectLineLogin")]
    public class DirectLineLoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DirectLineLoginController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("UserLogged")]
        public async Task<bool> CheckUserIsLogged([FromBody] DirectLineUserDto directLineUserDto)
        {
            var directLineLogin = _context.DirectLineLogins.Where(directLineLoginUser => directLineLoginUser.UserName == directLineUserDto.UserName && directLineLoginUser.ExpirationTime > DateTime.UtcNow).FirstOrDefault();

            if(directLineLogin == null)
            {
                return false;
            }
            return true;
        }
    }
}