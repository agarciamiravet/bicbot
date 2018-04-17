namespace BIC.Bot.RestService.Controllers
{
    using System;
    using System.Threading.Tasks;
    using BIC.Bot.RestService.Data;
    using BIC.Bot.RestService.Data.Entities;
    using BIC.Bot.RestService.Dtos;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/registrationbotuser")]
    public class RegistrationBotUserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrationBotUserController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("registerbotuser")]
        public async Task CreateRegistrationBotUser([FromBody] BotUserRegisterDto botUserRegisterDto)
        {

            var newBotUserRegister = new BotProactiveMessages();

            newBotUserRegister.Activity = botUserRegisterDto.Activity;
            newBotUserRegister.ChannelId = botUserRegisterDto.ChannelId;
            newBotUserRegister.Conversation = botUserRegisterDto.Conversation;
            newBotUserRegister.FromID = botUserRegisterDto.FromID;
            newBotUserRegister.FromName = botUserRegisterDto.FromName;
            newBotUserRegister.RegistryDate = DateTime.Now;
            newBotUserRegister.ServiceUrl = botUserRegisterDto.ServiceUrl;
            newBotUserRegister.UserName = botUserRegisterDto.UserName;

           await  _context.BotProactiveMessage.AddAsync(newBotUserRegister);
           await  _context.SaveChangesAsync();

        }
    }
}