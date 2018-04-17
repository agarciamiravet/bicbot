using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BIC.Bot.RestService.Dtos
{
    public class BotUserRegisterDto
    {
        public int Id { get; set; }

        public string Activity { get; set; }

        public string UserName { get; set; }

        public string FromID { get; set; }

        public string FromName { get; set; }

        public string ServiceUrl { get; set; }

        public string ChannelId { get; set; }

        public string Conversation { get; set; }

        public DateTime RegistryDate { get; set; }
    }
}
