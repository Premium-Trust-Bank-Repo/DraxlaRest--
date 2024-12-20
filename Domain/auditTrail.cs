using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class auditTrail
    {
        public int tbid { get; set; }
        public Nullable<int> userId { get; set; }
        public string tableName { get; set; }
        public string columnName { get; set; }
        public string actionPerformed { get; set; }
        public string oldValue { get; set; }
        public string newValue { get; set; }
        public Nullable<System.DateTime> date { get; set; }
    }
}
