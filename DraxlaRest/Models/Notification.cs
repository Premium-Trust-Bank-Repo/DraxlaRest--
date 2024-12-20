using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DraxlaRest.Models
{
    public class Notification
    {
        public string channel { get; set; }
        public string requestId { get; set; }
        public string sender { get; set; }
        public string senderName { get; set; }
        public string recipient { get; set; }
        public string subject { get; set; }
        public string emailBody { get; set; }
        public string accountNumber { get; set; }
        public string copy { get; set; }
        public string bCopy { get; set; }
        public string contentStream { get; set; }
        public string contentType { get; set; }
        public string fileName { get; set; }
        public string isBodyHtml { get; set; }
        public string linkedResources { get; set; }
        public string msgType { get; set; }


    }
}