using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BIC.Bot.RestService.Data.Entities
{
    [Table("SmsLogins")]
    public class SmsLogin
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public DateTime ExpirationTime { get; set; }

        public string Jwt { get; set; }

    }
}
