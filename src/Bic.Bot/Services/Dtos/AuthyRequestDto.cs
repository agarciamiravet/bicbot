using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIC.Bot.Services.Dtos
{
   
        public class AuthyRequestDto
        {
            public string UserRequest { get; set; }

            public string ActionToTake { get; set; }

            public string EMail { get; set; }
        }
}