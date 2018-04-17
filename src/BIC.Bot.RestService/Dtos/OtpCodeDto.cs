using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BIC.Bot.RestService.Dtos
{
    public class OtpCodeDto
    {
        public string UserName { get; set; }

        public string OneTimePasswordCode { get; set; }

        public string ChannelId { get; set; }

    }
}
