using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class rolesAssignment
    {
        public int roleAssgId { get; set; }
        public int roleId { get; set; }
        public int menuId { get; set; }
        public Nullable<int> userId { get; set; }
        public System.DateTime dateCreated { get; set; }
        public string status { get; set; }
        public Nullable<int> canInsert { get; set; }
        public Nullable<int> canDelete { get; set; }
        public Nullable<int> canUpdate { get; set; }
        public Nullable<int> canauth { get; set; }
        public Nullable<int> authId { get; set; }
        public virtual admin admin { get; set; }
    }
}
