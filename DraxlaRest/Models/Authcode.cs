using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DraxlaRest.Models
{
    public class Authcode
    {
        public string userId { get; set; }
       // public string requestId { get; set; }
        public string authorizationCode { get; set; }
       // public int channel { get; set; }
        public string accessCode { get; set; }
        public string email_address { get; set; }
        public string telephone { get; set; }

        public string message_body { get; set; }
        public string message_subject { get; set; }

        //public string accountType { get; set; }
    }
}