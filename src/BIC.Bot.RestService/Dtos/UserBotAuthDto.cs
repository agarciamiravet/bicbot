using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BIC.Bot.RestService.Dtos
{
    public class UserBotAuthDto
    {
        public string UserName { get;set;}

        public string ChannelId { get; set; }
    }
}
