using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class userSessionLog
    {
        public int tbid { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<System.DateTime> loginTime { get; set; }
        public Nullable<System.DateTime> logoutTime { get; set; }
        public Nullable<System.DateTime> lastActionDate { get; set; }
        public string isActive { get; set; }
        public string clientName { get; set; }
        public string ipAddress { get; set; }
        public string mac { get; set; }
        public virtual admin admin { get; set; }
    }
}
