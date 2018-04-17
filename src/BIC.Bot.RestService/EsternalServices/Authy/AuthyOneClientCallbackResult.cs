using BIC.Bot.RestService.EsternalServices.Authy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BIC.Bot.RestService.EsternalServices.Authy
{
   
    public class Details
    {
        public string __invalid_name__Email_Address { get; set; }
    }

public class DeviceDetails
{
}

public class HiddenDetails
{
    public string ip { get; set; }
}

public class Transaction
{
    public Details details { get; set; }
    public DeviceDetails device_details { get; set; }
    public string device_geolocation { get; set; }
    public string device_signing_time { get; set; }
    public bool encrypted { get; set; }
    public bool flagged { get; set; }
    public HiddenDetails hidden_details { get; set; }
    public string message { get; set; }
    public string reason { get; set; }
    public string requester_details { get; set; }
    public string status { get; set; }
    public string uuid { get; set; }
    public string created_at_time { get; set; }
    public string customer_uuid { get; set; }
}

public class ApprovalRequest
{
    public Transaction transaction { get; set; }
    public string logos { get; set; }
    public string expiration_timestamp { get; set; }
}

public class AuthyOneClientCallbackResult
{
    public string device_uuid { get; set; }
    public string callback_action { get; set; }
    public string uuid { get; set; }
    public string status { get; set; }
    public ApprovalRequest approval_request { get; set; }
    public string signature { get; set; }
    public int authy_id { get; set; }
}
}
