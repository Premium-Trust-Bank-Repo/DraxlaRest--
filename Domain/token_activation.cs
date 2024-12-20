using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class token_activation
    {
        public int id { get; set; }
        public string user_id { get; set; }     
        public string token_serial_number { get; set; }
        public string otp { get; set; }
        public string validation_response { get; set; }
        public string operation { get; set; }      
        public Nullable<System.DateTime> operation_date { get; set; }
        public Nullable<int> status { get; set; }
    }
}
