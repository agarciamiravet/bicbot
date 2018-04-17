using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BIC.Bot.RestService.Data.Entities
{
    public class DirectLineLogins
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public DateTime ExpirationTime { get; set; }

        public string Jwt { get; set; }
    }
}
