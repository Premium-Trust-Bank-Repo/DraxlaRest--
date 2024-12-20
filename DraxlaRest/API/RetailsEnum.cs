using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DraxlaRest.API
{
    public class RetailsEnum
    {
        public string fullName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public string registeredPhoneNumber { get; set; }
        public string registeredEmailAddress { get; set; }
    }

    public class RetailsUser
    {
        public RetailsEnum result { get; set; }
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
    }
}