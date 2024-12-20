using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public partial class instance
    {
        public int id { get; set; }
        public string serial_no { get; set; }
        public Nullable<int> sequence_no { get; set; }
        public string dp_type { get; set; }
        public string application_names { get; set; }
        public string mode { get; set; }
        public Nullable<int> appl_count { get; set; }     
        public string blob { get; set; }
        public byte[] byte_blob { get; set; }
        public string blob_backup { get; set; }
        public Nullable<System.DateTime> assign_date { get; set; }
        public string device_id { get; set; }
        public int device_type { get; set; }
        public string platform_details { get; set; }
        public string activation_medium { get; set; }

        public Nullable<int> status { get; set; }

        public string created_by { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
    }
}
