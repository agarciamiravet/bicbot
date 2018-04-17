using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIC.Bot.Services.Dtos
{
    public class OtpCodeDto
    {
        public string UserName { get; set; }

        public string OneTimePasswordCode { get; set; }

        public string ChannelId { get; set; }
    }
}