using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;

namespace DraxlaRest.Models
{
    public class returnDigipassJson
    {
        public string status { get; set; }
        public string message { get; set; }
        public string static_vector { get; set; }
        public string message_vector { get; set; }
        public string transport_key { get; set; }
        public string token_type { get; set; }    
        public List<digipass> data { get; set; }
    }
}