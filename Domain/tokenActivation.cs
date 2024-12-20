using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class tokenActivation
    {
        public int tbid { get; set; }
        public string user_id { get; set; }     
        public string tokenSerialNumber { get; set; }
        public string Otp { get; set; }
        public string validationResponse { get; set; }
        public string operation { get; set; }      
        public Nullable<System.DateTime> operationDate { get; set; }
        public Nullable<int> status { get; set; }
    }
}
