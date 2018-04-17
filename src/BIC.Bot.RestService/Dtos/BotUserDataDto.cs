using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BIC.Bot.RestService.Dtos
{
    public class BotUserDataDto
    {
        public string UserConversationId { get; set; }

        public string UserName { get; set; }

        public string BotConversationId { get; set; }

        public string BotName { get; set; }

        public string ChannelId { get; set; }

    }
}
