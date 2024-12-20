using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DraxlaRest.Models
{
    public class UserEnumeration
    {
        public string userID { get; set; }
        public string sno { get; set; }
        public string username { get; set; }
        public string customerFirstName { get; set; }
        public string customerFullname { get; set; }
        public string customerLastName { get; set; }
        public string customerMiddleName { get; set; }
        public string customerEmail { get; set; }
        public string customerPhoneNo { get; set; }
        public string responseCode { get; set; }
        public string customerMsg { get; set; }

        
    }
}