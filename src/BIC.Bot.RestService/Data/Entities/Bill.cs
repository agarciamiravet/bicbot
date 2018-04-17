namespace BIC.Bot.RestService.Data.Entities
{
   using System;

  
    public class Bill
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime BillStartDate { get; set; }

        public DateTime BillEndDate { get; set; }
    }
}
