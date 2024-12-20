using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class admin
    {
        public admin()
        {
            this.rolesAssignments = new List<rolesAssignment>();
            this.userSessionLogs = new List<userSessionLog>();
        }

        public int userID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public Nullable<int> roleID { get; set; }
        public string fullName { get; set; }
        public string emailAddress { get; set; }
        public string mobileNo { get; set; }
        public Nullable<System.DateTime> lastLogoutDate { get; set; }
        public Nullable<System.DateTime> lastLoginDate { get; set; }
        public Nullable<System.DateTime> currentLoginDate { get; set; }
        public string multipleLogin { get; set; }
        public Nullable<int> accessDays { get; set; }
        public Nullable<System.DateTime> passwordExpiry { get; set; }
        public Nullable<byte> status { get; set; }
        public string isFirstLogin { get; set; }
        public string enforcePswdChange { get; set; }
        public Nullable<byte> loginStatus { get; set; }
        public Nullable<byte> loginCount { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public Nullable<int> authorisedBy { get; set; }
        public Nullable<System.DateTime> dateAuthorised { get; set; }
        public Nullable<int> modifiedBy { get; set; }
        public Nullable<System.DateTime> dateModifiedBy { get; set; }
        public Nullable<int> source { get; set; }
        public Nullable<int> accesstype { get; set; }
        public virtual ICollection<rolesAssignment> rolesAssignments { get; set; }
        public virtual ICollection<userSessionLog> userSessionLogs { get; set; }
    }
}
