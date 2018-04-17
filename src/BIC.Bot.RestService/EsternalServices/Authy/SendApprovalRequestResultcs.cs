namespace BIC.Bot.RestService.EsternalServices.Authy
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class SendApprovalRequestResult : AuthyResult
    {
        [JsonProperty("approval_request")]
        public IDictionary<string, string> ApprovalRequest { get; set; }
    }
}
