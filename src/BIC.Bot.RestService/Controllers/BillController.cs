namespace BIC.Bot.RestService.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using BIC.Bot.RestService.Data;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/Bill")]
    public class BillController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [Route("ibi")]
        public async Task<string> GetIbiInfo()
        {
            var ibiFromDatabase = _context.Bill.Where(bill => bill.Type == "IBI").FirstOrDefault();

            if(ibiFromDatabase != null)
            {
                return "El IBI se paga del " + ibiFromDatabase.BillStartDate.ToShortDateString() + " al " + ibiFromDatabase.BillEndDate.ToShortDateString();
            }
            else
            {
                return "Esa información no esta disponible";
            }
        }
    }
}