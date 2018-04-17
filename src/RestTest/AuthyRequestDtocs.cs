using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    public class AuthyRequestDto
    {
        public string UserRequest { get; set; }

        public string ActionToTake { get; set; }

        public string EMail { get; set; }
    }
}
