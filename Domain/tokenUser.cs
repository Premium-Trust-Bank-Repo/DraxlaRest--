using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class tokenUser
    {
        public int tbid { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string staticPwd { get; set; }
        public string linkUserId { get; set; }
        public Nullable<int> lockCount { get; set; }
        public Nullable<int> locked { get; set; }
        public Nullable<int> disabled { get; set; }
        public Nullable<int> roleId { get; set; }
        public Nullable<System.DateTime> lastPwdSetTime { get; set; }
        public System.DateTime createTime { get; set; }
        public System.DateTime modifyTime { get; set; }
        public Nullable<System.DateTime> lastAuthTime { get; set; }
        public Nullable<System.DateTime> expirationTime { get; set; }
    }
}
