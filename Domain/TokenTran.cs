using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class TokenTran
    {
        public long id { get; set; }
        public string trans_name { get; set; }
        public string userid { get; set; }
        public string serial_no { get; set; }
        public string from_acct { get; set; }
        public string to_acct { get; set; }
        public string trans_amt { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> trans_date { get; set; }
        public string confirm_code { get; set; }
        public Nullable<System.DateTime> confirm_date { get; set; }
    }
}
