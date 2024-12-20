using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class digipass_activities
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public string serial_no { get; set; }
        public string response { get; set; }
        public string operation { get; set; }
        
        public Nullable<System.DateTime> operation_date { get; set; }
    }
}
