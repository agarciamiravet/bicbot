using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BIC.Bot.RestService.Controllers
{
    [Produces("application/json")]
    [Route("api/HomeNotification")]
    public class HomeNotificationController : Controller
    {
        // GET: api/HomeNotification
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/HomeNotification/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/HomeNotification
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/HomeNotification/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
