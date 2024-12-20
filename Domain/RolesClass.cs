using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class RolesClass
    {
        public int roleId { get; set; }
        public string roleName { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public Nullable<int> userId { get; set; }
        public string status { get; set; }
    }
}
