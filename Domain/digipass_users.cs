using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class digipass_users
    {

        public int id { get; set; }
        public string user_id { get; set; }

        public string token_type { get; set; }

        
        public string serial_no { get; set; }   
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

        public int status { get; set; }

        public string  is_main { get; set; }

        

        public Nullable<System.DateTime> created_time { get; set; } = DateTime.Now;
    }
}
