using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DraxlaRest.API
{
    public class SendSMSRequest
    {
        public string phoneNumber { get; set; }
        public string messageBody { get; set; }
    }

    public class SendSMSResponse
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }

    }
}