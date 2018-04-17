using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIC.Bot.RestService.Data.Entities
{
    [Table("BotLogins") ]
    public class BotLogin
    {
        [Key]
        public int Id { get; set; }
        
        public string Jwt { get; set; }

        public DateTime ExpirationTime { get; set; }

        public string UserName { get; set; }

        public string SkypeUserName { get; set; }
    }
}
