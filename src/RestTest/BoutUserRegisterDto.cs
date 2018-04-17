using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    public class BoutUserRegisterDto
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
