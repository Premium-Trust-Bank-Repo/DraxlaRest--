using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DraxlaRest.Models
{
    public class StaffDetails
    {
        public string role { get; set; }
        public string branch { get; set; }
        public string staff_name { get; set; }
        public string fullname { get; set; }
        public int usertype { get; set; }
        public int response_code { get; set; }
        public string response_description { get; set; }

       
    }
}