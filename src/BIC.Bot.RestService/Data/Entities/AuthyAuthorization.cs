namespace BIC.Bot.RestService.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AuthyAuthorizations")]
    public class AuthyAuthorization
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string AuthyIdentifier { get; set; }

        public string ActionToPerform { get; set; }

        public DateTime ExpirationValidityDate  { get; set; }

        public bool UsedAuthorization { get; set; }

        public string TypeAction { get; set; }

        public string AcionToExecute { get; set; }

    }
}
