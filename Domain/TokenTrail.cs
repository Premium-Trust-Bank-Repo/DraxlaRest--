using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class TokenTrail
    {
        public int tbid { get; set; }
        public string userId { get; set; }
        public string tokenSerialNumber { get; set; }
        public string operation { get; set; }
        public string validationResponse { get; set; }
        public Nullable<System.DateTime> operationDate { get; set; }
    }
}
