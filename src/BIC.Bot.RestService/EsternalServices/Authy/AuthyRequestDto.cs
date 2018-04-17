using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BIC.Bot.RestService.EsternalServices.Authy
{
    public class AuthyRequestDto
    {
        public string UserRequest { get; set; }

        public string ActionToTake { get; set; }

        public string EMail { get; set; }
    }
}
