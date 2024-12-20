using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraxlaRest.Models
{
    public class Authentication
    {
        public string userid { get; set; }
        public string accesscode { get; set; }
        public string sno { get; set; }

        //public string requestReference { get; set; }
        //public string accountNumber { get; set; }
    }
}
