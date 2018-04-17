using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIC.Bot.RestService.Data.Entities
{
    [Table("DirectLineUsers")]
    public class DirectLineUser
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public DateTime RegisteredDate { get; set; }

        public string EMail { get; set; }

        public string UserId { get; set; }

        public string SecretKey { get; set; }
    }
}
