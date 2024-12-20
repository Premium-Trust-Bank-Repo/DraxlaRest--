using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class menuControl
    {
        public int menuId { get; set; }
        public string menuName { get; set; }
        public string url { get; set; }
        public Nullable<int> parent { get; set; }
        public Nullable<int> Priority { get; set; }
        public string description { get; set; }
        public string tablename { get; set; }
        public string imageUrl { get; set; }
    }
}
